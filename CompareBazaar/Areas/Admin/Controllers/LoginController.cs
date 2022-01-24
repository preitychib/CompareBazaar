using Microsoft.AspNetCore.Mvc;

namespace CompareBaazar.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }


    }
}