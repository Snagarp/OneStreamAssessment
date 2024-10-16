using VendorConfiguration.Application.Commands;
using VendorConfiguration.Application.Dto;
using VendorConfiguration.Application.Queries;

namespace VendorConfiguration.Api.Controllers
{
    /// <summary>
    /// Provides methods to manage country reference table
    /// </summary>
    [ApiController]
    public class CountryController
        : BaseApiController
    {
        public CountryController(IMediator mediator) : base(ArgumentGuard.NotNull(mediator, nameof(mediator)))
        {
        }

        /// <summary>
        /// Retrieves a country record identified by countryId.
        /// </summary>
        /// <returns></returns>
        [HttpGet("countries/{countryId?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountry(CountryQuery query)
        {
            var result = await Mediator.Send(query).ConfigureAwait(true);
            return result.Match<IActionResult>(
              country => Ok(country),
              countries => Ok(countries),
              none => NotFound(),
              fault => BadRequest(fault)
              );
        }

        /// <summary>
        /// Creates a new country record using the values in the request body.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("countries")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateCountryCommand command)
        {
            ArgumentGuard.NotNull(command, nameof(command));

            var result = await Mediator.Send(command).ConfigureAwait(true);

            return result.Match<IActionResult>(
                country => CreatedAtAction(nameof(Create), new { countryId = country.CountryId }, country),
                fault => BadRequest(fault)
           );
        }

        /// <summary>
        /// Modifies the given country record identified by countryId using the values in the request body. 
        /// Specify only the propertes you want to update.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("countries/{countryId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Modify(ModifyCountryCommand command)
        {
            ArgumentGuard.NotNull(command, nameof(command));

            var result = await Mediator.Send(command).ConfigureAwait(true);

            return result.Match<IActionResult>(
                           country => Ok(country),
                           NotFound => new NotFoundResult(),
                           fault => BadRequest(fault)
                           );
        }

        /// <summary>
        /// Removes the country record identified by countryId.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpDelete("countries/{countryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent),]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(DeleteCountryCommand command)
        {
            ArgumentGuard.NotNull(command, nameof(command));

            var result = await Mediator.Send(command).ConfigureAwait(true);
            return result.Match<IActionResult>(
                           country => Ok(),
                           NotFound => new NotFoundResult(),
                           fault => BadRequest(fault)
                           );
        }

    }
}
