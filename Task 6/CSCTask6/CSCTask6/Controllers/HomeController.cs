using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CSCTask6.Models;
using Stripe;

namespace CSCTask6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Charge()
        {
            ViewBag.Title = "Charge";

            return View();
        }

        public ActionResult Testdb()
        {
            ViewBag.Title = "Testdb";

            return View();
        }

        public ActionResult Sub()
        {
            ViewBag.Title = "Sub";

            return View();
        }

    }
}
