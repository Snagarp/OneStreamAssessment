//2023 (c) TD Synnex - All Rights Reserved.




using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Configuration.Api.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
