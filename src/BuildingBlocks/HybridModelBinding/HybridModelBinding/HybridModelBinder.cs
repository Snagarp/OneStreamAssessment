﻿//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using HybridModelBinding.Extensions;
using HybridModelBinding.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HybridModelBinding
{
    public abstract class HybridModelBinder : IModelBinder
    {
        public HybridModelBinder(
            BindStrategy bindStrategy,
            IEnumerable<string> fallbackBindingOrder = null)
        {
            this.bindStrategy = bindStrategy;
            this.fallbackBindingOrder = fallbackBindingOrder ?? FallbackBindingOrder;
        }

        private static readonly IEnumerable<string> FallbackBindingOrder = new[] { Source.Body, Source.Form, Source.Route, Source.QueryString, Source.Header, Source.Claim };

        private readonly BindStrategy bindStrategy;
        private readonly IEnumerable<string> fallbackBindingOrder;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>")]
        private readonly IList<KeyValuePair<string, IModelBinder>> modelBinders = new List<KeyValuePair<string, IModelBinder>>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>")]
        private readonly IList<KeyValuePair<string, IValueProviderFactory>> valueProviderFactories = new List<KeyValuePair<string, IValueProviderFactory>>();

        public delegate bool BindStrategy(
            IEnumerable<string> previouslyBoundValueProviderIds,
            IEnumerable<string> allValueProviderIds);

        public HybridModelBinder AddModelBinder(
            string id,
            IModelBinder modelBinder)
        {
            var existingKeys = modelBinders.Select(x => x.Key);

            if (existingKeys.Contains(id))
            {
                throw new ArgumentException(
                    $"Provided `id` of `{id}` already exists in collection.",
                    nameof(id));
            }
            else
            {
                modelBinders.Add(new KeyValuePair<string, IModelBinder>(id, modelBinder));

                return this;
            }
        }

        public HybridModelBinder AddValueProviderFactory(
            string id,
            IValueProviderFactory factory)
        {
            var existingKeys = valueProviderFactories.Select(x => x.Key);

            if (existingKeys.Contains(id))
            {
                throw new ArgumentException(
                    $"Provided `id` of `{id}` already exists in collection.",
                    nameof(id));
            }
            else
            {
                valueProviderFactories.Add(
                    new KeyValuePair<string, IValueProviderFactory>(id, factory));

                return this;
            }
        }

        public virtual async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            object hydratedModel = null;
            var boundProperties = new List<KeyValuePair<string, string>>();
            var valueProviders = new List<KeyValuePair<string, IValueProvider>>();

            object hydratedBodyModel = null;

            bindingContext.HttpContext.Request.EnableBuffering();

            foreach (var kvp in modelBinders)
            {
                await kvp.Value.BindModelAsync(bindingContext);

                hydratedBodyModel = bindingContext.Result.Model;

                if (hydratedBodyModel != null)
                {
                    valueProviders.Add(
                        new KeyValuePair<string, IValueProvider>(
                            kvp.Key,
                            new BodyValueProvider(hydratedBodyModel, bindingContext.HttpContext.Request)));

                    break;
                }
            }

            switch (hydratedModel)
            {
                case null:
                {
                    try
                    {
                        // Not sure why this throws an exception. For now, we can ignore as it does not seem to affect the outcome.
                        bindingContext.ModelState.Clear();
                    }
                    catch (Exception) { }

                    // First, let us try and use DI to get the model.
                    hydratedModel = bindingContext.HttpContext.RequestServices.GetService(bindingContext.ModelType);

                    switch (hydratedModel)
                    {
                        case null:
                            try
                            {
                                /**
                         * Using DI did not work, so let us get crude. This might also fail since the model
                         * may not have a parameterless constructor.
                         */
                                hydratedModel = Activator.CreateInstance(bindingContext.ModelType);
                            }
                            catch (Exception)
                            {
                                bindingContext.Result = ModelBindingResult.Failed();

                                return;
                            }

                            break;
                    }

                    bindingContext.Result = ModelBindingResult.Success(hydratedModel);
                    break;
                }
            }

            var hydratedBoundModel = hydratedModel as IHybridBoundModel;
            var modelProperties = hydratedModel.GetPropertiesNotPartOfType<IHybridBoundModel>().ToList();
            var valueProviderFactoryContext = new ValueProviderFactoryContext(bindingContext.ActionContext);

            foreach (var kvp in valueProviderFactories)
            {
                await kvp.Value.CreateValueProviderAsync(valueProviderFactoryContext);

                var valueProvider = valueProviderFactoryContext.ValueProviders.LastOrDefault();

                if (valueProvider != null)
                {
                    valueProviders.Add(new KeyValuePair<string, IValueProvider>(kvp.Key, valueProvider));
                }
            }

            /*
             * We'll first look if the action-parameter has defined default-ordering.
             * If nothing is found, we'll look at the model-class.
             * Lastly, we'll use the fallback.
             */
            var defaultBindingOrder = (bindingContext.ActionContext.ActionDescriptor.Parameters
                .Where(x => x.ParameterType == hydratedModel.GetType()).FirstOrDefault() as ControllerParameterDescriptor)
                ?.ParameterInfo.GetCustomAttribute<FromHybridAttribute>()?.DefaultBindingOrder
                ?? hydratedModel.GetType().GetCustomAttribute<HybridBindClassAttribute>()?.DefaultBindingOrder
                ?? fallbackBindingOrder;

            foreach (var property in modelProperties)
            {
                var fromAttribute = property.GetCustomAttribute<HybridBindPropertyAttribute>();
                var valueProviderIds = fromAttribute?.ValueProviders.ToList() ?? new List<string>();
                var hybridBindPropertyAttributes = property
                    .GetCustomAttributes<HybridBindPropertyAttribute>()
                    .OrderByDescending(x => x.Order.HasValue)
                    .ThenBy(x => x.Order)
                    .ToList();

                if (hybridBindPropertyAttributes.Count != 0)
                {
                    valueProviderIds.AddRange(hybridBindPropertyAttributes.SelectMany(x => x.ValueProviders));
                }

                switch (valueProviderIds.Count)
                {
                    case 0:
                        valueProviderIds.AddRange(defaultBindingOrder);
                        break;
                }

                var boundValueProviderIds = new List<string>(boundProperties.Where(x => x.Value == property.Name).Select(x => x.Key));

                foreach (var valueProviderId in valueProviderIds)
                {
                    var matchingHybridBindPropertyAttribute = hybridBindPropertyAttributes
                        .FirstOrDefault(x => x.ValueProviders.Contains(valueProviderId));

                    var activeBindStrategy = matchingHybridBindPropertyAttribute?.Strategy ?? (fromAttribute?.Strategy == null
                            ? bindStrategy
                            : fromAttribute.Strategy);

                    if (activeBindStrategy(boundValueProviderIds, valueProviderIds))
                    {
                        var valueProvider = valueProviders.FirstOrDefault(x => x.Key == valueProviderId).Value;

                        if (valueProvider != null)
                        {
                            var matchingUriParams = (valueProvider is BodyValueProvider bodyValueProvider
                                ? new[] { bodyValueProvider.GetObject(matchingHybridBindPropertyAttribute?.Name ?? property.Name) }
                                    .Where(x => x != null)
                                : valueProvider.GetValue(matchingHybridBindPropertyAttribute?.Name ?? property.Name)
                                    .Where(x => !string.IsNullOrEmpty(x))
                                    .Cast<object>())
                                .ToList();

                            switch (matchingUriParams.Count)
                            {
                                case 0 when valueProvider is FormValueProvider formValueProvider:
                                {
                                    var formCollectionValue = formValueProvider
                                        .GetType()
                                        .GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance)
                                        ?.GetValue(formValueProvider);

                                    switch (formCollectionValue)
                                    {
                                        case FormCollection formCollection:
                                        {
                                            var files = formCollection
                                                .Files
                                                .GetFiles(matchingHybridBindPropertyAttribute?.Name ?? property.Name);

                                            if (files != null)
                                            {
                                                matchingUriParams.AddRange(files);
                                            }

                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            if (matchingUriParams.Count != 0)
                            {
                                var isPropertyEnumerable =
                                    typeof(IEnumerable).IsAssignableFrom(property.PropertyType) &&
                                    !typeof(string).IsAssignableFrom(property.PropertyType);

                                switch (matchingUriParams.Count)
                                {
                                    case 1 when !isPropertyEnumerable:
                                    {
                                        var matchingUriParam = matchingUriParams.First();
                                        var descriptor = TypeDescriptor.GetConverter(property.PropertyType);

                                        switch (valueProvider)
                                        {
                                            case BodyValueProvider:
                                                property.SetValue(hydratedModel, matchingUriParam, null);

                                                boundValueProviderIds.Add(valueProviderId);
                                                boundProperties.Add(new KeyValuePair<string, string>(valueProviderId, property.Name));
                                                hydratedBoundModel?.HybridBoundProperties?.Add(property.Name, valueProviderId);
                                                break;
                                            default:
                                            {
                                                if (
                                                    typeof(IFormFile).IsAssignableFrom(matchingUriParam.GetType()) &&
                                                    typeof(IFormFile).IsAssignableFrom(property.PropertyType))
                                                {
                                                    property.SetValue(hydratedModel, matchingUriParam, null);

                                                    boundValueProviderIds.Add(valueProviderId);
                                                    boundProperties.Add(new KeyValuePair<string, string>(valueProviderId, property.Name));
                                                    hydratedBoundModel?.HybridBoundProperties?.Add(property.Name, valueProviderId);
                                                }
                                                else if (descriptor.CanConvertFrom(matchingUriParam.GetType()))
                                                {
                                                    property.SetValue(hydratedModel, descriptor.ConvertFrom(matchingUriParam), null);

                                                    boundValueProviderIds.Add(valueProviderId);
                                                    boundProperties.Add(new KeyValuePair<string, string>(valueProviderId, property.Name));
                                                    hydratedBoundModel?.HybridBoundProperties?.Add(property.Name, valueProviderId);
                                                }

                                                break;
                                            }
                                        }

                                        break;
                                    }
                                    default:
                                    {
                                        switch (isPropertyEnumerable)
                                        {
                                            case true:
                                            {
                                                var descriptor = property != null && property.PropertyType.GenericTypeArguments.Length != 0
                                                    ? TypeDescriptor.GetConverter(property.PropertyType.GenericTypeArguments.First())
                                                    : TypeDescriptor.GetConverter(property.PropertyType);

                                                var propertyListType = typeof(List<>).MakeGenericType(property.PropertyType.GenericTypeArguments.First());
                                                var propertyListInstance = (IList)Activator.CreateInstance(propertyListType);

                                                foreach (var matchingUriParam in matchingUriParams)
                                                {
                                                    switch (valueProvider)
                                                    {
                                                        case BodyValueProvider:
                                                        {
                                                            switch (matchingUriParam)
                                                            {
                                                                case IEnumerable matchingUriParamList:
                                                                {
                                                                    foreach (var param in matchingUriParamList)
                                                                    {
                                                                        propertyListInstance.Add(param);
                                                                    }

                                                                    break;
                                                                }
                                                            }

                                                            break;
                                                        }
                                                        default:
                                                        {
                                                            if (typeof(IFormFile).IsAssignableFrom(matchingUriParam.GetType()))
                                                            {
                                                                try
                                                                {
                                                                    propertyListInstance.Add(matchingUriParam);
                                                                }
                                                                catch (Exception) { }
                                                            }
                                                            else if (descriptor.CanConvertFrom(matchingUriParam.GetType()))
                                                            {
                                                                try
                                                                {
                                                                    propertyListInstance.Add(descriptor.ConvertFrom(matchingUriParam));
                                                                }
                                                                catch (Exception) { }
                                                            }

                                                            break;
                                                        }
                                                    }
                                                }

                                                switch (propertyListInstance.Count)
                                                {
                                                    case > 0:
                                                        property.SetValue(hydratedModel, propertyListInstance, null);

                                                        boundValueProviderIds.Add(valueProviderId);
                                                        boundProperties.Add(new KeyValuePair<string, string>(valueProviderId, property.Name));
                                                        hydratedBoundModel?.HybridBoundProperties?.Add(property.Name, valueProviderId);
                                                        break;
                                                }

                                                break;
                                            }
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
