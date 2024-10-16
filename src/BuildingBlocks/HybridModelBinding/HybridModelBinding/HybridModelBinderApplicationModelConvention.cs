//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Reflection;

namespace HybridModelBinding
{
    public class HybridModelBinderApplicationModelConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    switch (action.Parameters.Count)
                    {
                        case 1:
                        {
                            var parameterModel = action.Parameters.First();
                            var parameterType = parameterModel.ParameterInfo.ParameterType;
                            var hasBindingAttribute = parameterModel.Attributes
                                .Where(x => typeof(IBindingSourceMetadata).IsAssignableFrom(x.GetType()))
                                .Any();

                            switch (hasBindingAttribute)
                            {
                                case false when
                                    parameterType.IsClass
                                    && !parameterType.IsAbstract
                                    && parameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Length > 0
                                    && parameterType != typeof(string):
                                    parameterModel.BindingInfo = new BindingInfo()
                                    {
                                        BindingSource = new HybridBindingSource()
                                    };
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
        }
    }
}
