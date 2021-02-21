using NovaWebSolution.Models;
using NovaWebSolution.Repository;
using NovaWebSolution.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NovaWebSolution.Dtos;
namespace NovaWebSolution.Controllers
{
    [Authorize]
    [Authenticate]
    public class HomeController : Controller
    {
        //private readonly IToastNotification _toastNotification;
        private readonly IDashboardRepository dashboardRepository;
        public HomeController()
        {
            this.dashboardRepository = new DashboardRepository(new AppDbContext());
        }

        public ActionResult Index()
        {
            HomeCardDto homeCardDto = new HomeCardDto();
            return View(homeCardDto);
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