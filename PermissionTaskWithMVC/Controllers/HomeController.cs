using Microsoft.AspNetCore.Mvc;
using PermissionTaskWithMVC.Models;
using System.Diagnostics;

namespace PermissionTaskWithMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static bool IsLoggedIn  = false;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Default()
        {
            return View();
        }

        [HttpGet("Login")]
        [HttpGet("Home/Login")]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}