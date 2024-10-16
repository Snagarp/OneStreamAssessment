//2023 (c) TD Synnex - All Rights Reserved.




using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.ActionResults;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object error)
        : base(error)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}
