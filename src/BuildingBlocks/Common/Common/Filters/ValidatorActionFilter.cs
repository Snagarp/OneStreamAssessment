using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Filters
{
    /// <summary>
    /// An action filter that validates the model state before the action method executes.
    /// If the model state is invalid, it returns a 400 Bad Request response with the model state errors.
    /// </summary>
    public class ValidatorActionFilter : IActionFilter
    {
        /// <summary>
        /// This method is called before the action method is executed.
        /// It checks if the model state is valid, and if not, returns a 400 Bad Request result.
        /// </summary>
        /// <param name="context">The context for the action being executed, which contains the model state.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="context"/> is null.</exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "context cannot be null.");
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        /// <summary>
        /// This method is called after the action method executes. In this implementation, it does nothing.
        /// </summary>
        /// <param name="context">The context for the action being executed.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation required here
        }
    }
}
