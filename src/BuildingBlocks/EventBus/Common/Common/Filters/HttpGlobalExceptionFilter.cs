//2023 (c) TD Synnex - All Rights Reserved.

using Common.ActionResults;
using Common.Exceptions;
using Common.Validation;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Net;

namespace Common.Filters.HttpGlobalExceptionFilter;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpGlobalExceptionFilter> logger;

    public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
    {
        this.logger = ArgumentGuard.NotNull(logger);
    }

    public void OnException(ExceptionContext context)
    {
        ArgumentGuard.NotNull(context, nameof(context));

        logger.LogError(new EventId(context.Exception.HResult),
           context.Exception,
           context.Exception.Message);


        if (context.Exception.GetType().GetInterface("IDomainException") == typeof(IDomainException))
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };
            if (context.Exception.InnerException != null && context.Exception.InnerException is ValidationException validationException)
            {
                problemDetails.Errors.Add("DomainValidations", validationException.Errors.Select(err => err.ErrorMessage).ToArray());
            }
            else
            {
                var errors = new List<string>
                    {
                        context.Exception.Message
                    };

                if (context.Exception.GetType().GetInterface("IHasAdditionalData") == typeof(IHasAdditionalData))
                {
                    var additionalDataException = context.Exception as IHasAdditionalData;

                    errors.AddRange(additionalDataException?.GetAdditionalData()!);
                }
                problemDetails.Errors.Add("DomainValidations", errors.ToArray());
            }

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else
        {
            var json = new JsonErrorResponse
            {
                Messages = new[] { "An error occur.Try it again." }
            };

            //if (env.IsDevelopment()) //Uncomment when app is stable. Detail error messages should only display in prod
            //{
            //json.DeveloperMessage = JsonConvert.SerializeObject(context.Exception);
            json.DeveloperMessage = context.Exception.Message;
            if (context.Exception.InnerException != null)
                json.DeveloperMessage += context.Exception.InnerException.Message ?? "";
            //}

            // Result asigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
            // It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        context.ExceptionHandled = true;
    }

    private class JsonErrorResponse
    {
        public string[] Messages { get; set; }

        public string DeveloperMessage { get; set; }
    }
}