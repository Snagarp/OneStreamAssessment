//2023 (c) TD Synnex - All Rights Reserved.




using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Configuration.Api.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
