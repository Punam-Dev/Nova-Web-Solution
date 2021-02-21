using NovaWebSolution.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NovaWebSolution.Controllers
{
    [Authorize]
    [Authenticate]
    public class HomeController : Controller
    {
        //private readonly IToastNotification _toastNotification;

        public HomeController()
        {
            //_logger = logger;
            //_toastNotification = toastNotification;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public ActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}