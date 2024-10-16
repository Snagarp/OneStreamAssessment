namespace VendorConfiguration.Api.Controllers
{
    [Route("/api/v1/")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract class BaseApiController : ControllerBase
    {
        // Constructor for dependency injection of the mediator.
        protected BaseApiController(IMediator mediator)
        {
            Mediator = ArgumentGuard.NotNull(mediator);
        }

        // Mediator instance available to derived controllers for handling commands and queries.
        protected IMediator Mediator { get; init; }
    }
}