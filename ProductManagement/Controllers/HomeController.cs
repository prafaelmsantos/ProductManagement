using Microsoft.AspNetCore.Mvc;

namespace ProductManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
