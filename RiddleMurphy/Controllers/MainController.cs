using RiddleMurphy.Models.Context;
using RiddleMurphy.Models.Entities;
using RiddleMurphy.ViewModels;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading;

namespace RiddleMurphy.Controllers
{
    public class MainController : Controller
    {
        int pagesize = 7;//this is the number of posts per page in the scroll pagination event

        RiddleContext db = new RiddleContext();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult Index(int? page)
        {

            var users = db.Users;
            var riddles = db.Riddles;
            Riddle todaysriddle = null;
            foreach (var item in db.Riddles.Include("Owner").Where(x => x.isTodaysRiddle))
            {
                todaysriddle = item;
            }

            var model = new MainViewModel();
            model.Users = users.ToList();
            model.TodaysRiddle = todaysriddle;
            model.ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            if (!page.HasValue)
            {
                model.Riddles = riddles.Where(x => x.RiddleState).OrderByDescending(x => x.RiddleId).Take(pagesize).ToList();
            }
            else
            {
                int pageIndex = pagesize * page.Value;
                model.Riddles = riddles.Where(x => x.RiddleState).OrderByDescending(x => x.RiddleId).Skip(pageIndex).Take(pagesize).ToList();
            }


            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index", model);
            }

            return View(model);

        }

        public ActionResult Home(int? page)
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var model = new MainViewModel();
            model.ActiveUser = ActiveUser;

            List<Riddle> riddles = new List<Riddle>();
            var FollowsIdList = db.Database.SqlQuery<int>($"select Followen_UserId from dbo.Follows where Follower_UserId = {ActiveUser.UserId}").ToList();
            foreach (var item in FollowsIdList)
            {
                foreach (var riddle in db.Users.Include("UserRiddles").FirstOrDefault(x => x.UserId == item).UserRiddles)
                {
                    riddles.Add(riddle);
                }
            }

            if (!page.HasValue)
            {
                model.Riddles = riddles.Where(x => x.RiddleState).OrderByDescending(x => x.RiddleId).Take(pagesize).ToList();
            }
            else
            {
                int pageIndex = pagesize * page.Value;
                model.Riddles = riddles.Where(x => x.RiddleState).OrderByDescending(x => x.RiddleId).Skip(pageIndex).Take(pagesize).ToList();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Home", model);
            }

            return View(model);

        }

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(User user)
        {

            var userInDb = db.Users.FirstOrDefault(x => x.UserName == user.UserName);

            if (!ModelState.IsValid)
            {
                ViewData["ModelNotValidErrorMessage"] = "INFORMATIONS IS NOT VALID";
                return View();
            }
            else if (userInDb != null)
            {
                ViewData["UniqueErrorMessage"] = "This Name Has Taken Please Choose Another One";
                return View();
            }
            else
            {
                try
                {
                    CoinAccount account = new CoinAccount();

                    account.UserId = user.UserId;
                    account.Balance = 100;
                    AccountProcess process = new AccountProcess()
                    {
                        CoinAccount = account,
                        IsProcesPositive = true,
                        ProcessAmount = 100,
                        ProcessTime = DateTime.Now,
                        ProcessType = "Welcome Bonus"
                    };
                    user.UserRole = "U";
                    user.UserJoinDate = DateTime.Now;
                    //-------------------------------
                    db.Users.Add(user);
                    db.CoinAccounts.Add(account);
                    db.SaveChanges();
                    db.AccountProcesses.Add(process);
                    db.SaveChanges();
                    //-------------------------------
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewData["DatabaseErrorMessage"] = "Database ERROR";
                    return View();
                }
            }


        }

        public ActionResult Visit(int id)
        {

            var model = new VisitViewModel();
            model.Visitor = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            if (model.Visitor.UserId == id)
            {
                return RedirectToAction("Index", "User");
            }
            model.HomeOwner = db.Users.Find(id);
            model.HomeOwner.UserRiddles = db.Riddles.Where(r => r.Owner.UserId == model.HomeOwner.UserId && r.RiddleState).ToList();

            int activeRiddles = 0;

            foreach (var item in model.HomeOwner.UserRiddles)
            {
                if (item.RiddleState)
                {
                    activeRiddles++;
                }
            }

            ViewBag.ActiveRiddlesCount = activeRiddles;

            //100  4 ün takipçilerinin id lerini getiriyor:
            //select Follower_UserId from dbo.Follows where Followen_UserId = 1004

            var FollowersIdList = db.Database.SqlQuery<int>($"select Follower_UserId from dbo.Follows where Followen_UserId = {model.HomeOwner.UserId}").ToList();
            model.IsVisitorFollowing = false;
            foreach (var item in FollowersIdList)
            {
                if (item == model.Visitor.UserId)
                {
                    model.IsVisitorFollowing = true;//setle visitor takip ediyorsa homeownerı
                }
            }

            model.HomeOwnerFollowerCount = db.Follows.Where(x => x.Followen.UserId == model.HomeOwner.UserId).Count();
            model.HomeOwnerFollowsCount = db.Follows.Where(x => x.Follower.UserId == model.HomeOwner.UserId).Count();

            return View(model);
        }
    }
}

