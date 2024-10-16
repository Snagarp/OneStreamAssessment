//2023 (c) TD Synnex - All Rights Reserved.


namespace Configuration.API.Controllers;

public class HomeController : Controller
{
    // GET: /<controller>/
    public IActionResult Index()
    {
        return Ok("ok");
        //return new RedirectResult("~/swagger");
    }
}
