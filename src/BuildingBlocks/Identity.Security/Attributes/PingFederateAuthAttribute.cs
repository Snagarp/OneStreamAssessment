using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Threading.Tasks;
using Identity.Security.Models;

namespace Identity.Security.Attributes
{
    /// <summary>
    /// Applies Ping Federate authentication to the action.
    /// </summary>
    /// <seealso cref="ActionFilterAttribute" />
    public class PingFederateAuthAttribute : ActionFilterAttribute
    {
        private readonly ILogger<PingFederateAuthAttribute> _logger;
       
        private readonly ISecurityTokenValidator _validator;

        /// <summary>Initializes a new instance of the <see cref="PingFederateAuthAttribute"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="validator">The validator.</param>
        public PingFederateAuthAttribute(
            ILogger<PingFederateAuthAttribute> logger,
            ISecurityTokenValidator validator
            )
        {
            _logger = logger;
            _validator = validator;
        }

        /// <summary>Called asynchronously before the action, after model binding is complete.</summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        /// <param name="next">The <see cref="ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        /// <inheritdoc />
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            switch (context)
            {
                case null:
                    throw new ArgumentNullException(nameof(context));
            }
            switch (next)
            {
                case null:
                    throw new ArgumentNullException(nameof(next));
            }

            var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = context.Controller.GetType().Name;

            _logger.LogInformation(
                $"Validating PostBack token on {controllerName}.{actionName}");

            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var values))
            {
                // not header, not authorized
                var message = $"Unauthorized attempt to access {controllerName}.{actionName}. No authorization header.";
                _logger.LogWarning(message);
                context.Result =  new OneStreamUnauthorizedResult(message);
                return;
            }

            var bearerToken = (from value in values
                    where value.StartsWith("Bearer ", StringComparison.CurrentCultureIgnoreCase)
                    select new { Token = value.Replace("Bearer ", "", StringComparison.CurrentCultureIgnoreCase) })
                .SingleOrDefault()?.Token;

            if (string.IsNullOrEmpty(bearerToken))
            {
                var message = $"Unauthorized attempt to access {controllerName}.{actionName}. Missing bearer token.";
                _logger.LogWarning(message);
                context.Result = new OneStreamUnauthorizedResult(message);
                
                return;
            }

            try
            {
                _validator.ValidateToken(bearerToken, new TokenValidationParameters(), out _);
            }
            catch (SecurityTokenException ex)
            {
                var message = $"Unauthorized attempt to access {controllerName}.{actionName}. Invalid bearer token. Error: {ex.Message}";
                _logger.LogError(message);
                context.Result = new OneStreamUnauthorizedResult(message);
                
                return;
            }

            _logger.LogInformation("Successfully validated bearer token.");

            await next()
                .ConfigureAwait(false);
        }
    }
}
