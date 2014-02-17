﻿using System.Web.Mvc;

namespace SnakeBattleNet.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return PartialView("_Login");
        }
        public ActionResult Register()
        {
            return PartialView("_Register");
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}
