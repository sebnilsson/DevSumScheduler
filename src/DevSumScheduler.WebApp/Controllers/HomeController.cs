using System.Diagnostics;
using DevSumScheduler.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevSumScheduler.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}