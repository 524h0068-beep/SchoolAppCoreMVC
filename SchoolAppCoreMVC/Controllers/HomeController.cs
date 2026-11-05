
using Microsoft.AspNetCore.Mvc;

namespace SchoolAppCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
