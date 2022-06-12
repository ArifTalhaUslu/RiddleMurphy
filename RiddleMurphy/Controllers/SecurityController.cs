using RiddleMurphy.Models.Context;
using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RiddleMurphy.Controllers
{
    public class SecurityController : Controller
    {
        RiddleContext db = new RiddleContext();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(User user)
        {

            var userInDb = db.Users.FirstOrDefault(x => x.UserName == user.UserName && x.UserPassword == user.UserPassword);

            if (userInDb != null)
            {
                FormsAuthentication.SetAuthCookie(userInDb.UserName, false);
                return RedirectToAction("Index", "Main");
            }
            else
            {
                ViewData["ErrorMessage"] = "Access Denied";
                return View();
            }

        }

        [Authorize(Roles = "A,U")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}