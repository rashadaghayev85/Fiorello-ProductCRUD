using Microsoft.AspNetCore.Mvc;

namespace FiorelloMVC.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
