using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TenPinBowling.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ten Pin Bowling Technical Test";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Created by Steven Sagar";

            return View();
        }
    }
}