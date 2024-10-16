//2023 (c) TD Synnex - All Rights Reserved.



using Identity.Security.SecureVault;

namespace Configuration.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SecureVaultController : ControllerBase
{
    private readonly ILogger<SecureVaultController> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISecureVaultHelper _secureVaultHelper;
    public SecureVaultController(
        ILogger<SecureVaultController> logger,
        IConfiguration configuration,
        ISecureVaultHelper secureVaultHelper
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration;
        _secureVaultHelper= secureVaultHelper;

    }


    [HttpGet]
    [Route("{secretName}")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Get(string secretName)
    {
        try
        {
            return Ok(await _secureVaultHelper.GetSecretAsync(secretName));
        }catch (Exception ex)
        {
            _logger.LogInformation(
                      $"Unable to retrieve secret for {secretName} "+ex.Message);

            return Ok(string.Empty);
        }
    }
}



