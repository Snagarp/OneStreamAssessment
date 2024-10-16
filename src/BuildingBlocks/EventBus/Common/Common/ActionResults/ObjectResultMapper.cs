//2023 (c) TD Synnex - All Rights Reserved.
using Common.Validation;

using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Text.Json.Nodes;

namespace Common.ActionResults
{
    public static class ObjectResultMapper
    {
        public static ObjectResult GetResult(dynamic result)
        {ArgumentGuard.NotNull(result, "result");

            int status = GetResponseStatus(result);
            
            if (status == 400)
                return new BadRequestObjectResult(result);
            else if (status == 500)
                return new InternalServerErrorObjectResult(result);

            return new OkObjectResult(result);
        }

        private static int GetResponseStatus(JsonNode result)
        {
            ArgumentGuard.NotNull(result, "result");

            int status = ((int)HttpStatusCode.OK);
            if (result is null) return status;
            if (result.GetType() == typeof(JsonObject))
            {
                status = result["status"]?.GetValue<Int16>() ?? status;
            }
               
            return status;
        }
    }
}
