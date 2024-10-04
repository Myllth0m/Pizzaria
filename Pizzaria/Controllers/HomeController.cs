using Microsoft.AspNetCore.Mvc;

namespace Pizzaria.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
