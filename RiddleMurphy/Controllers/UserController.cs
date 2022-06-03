using RiddleMurphy.Models.Context;
using RiddleMurphy.Models.Entities;
using RiddleMurphy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiddleMurphy.Controllers
{
    [Authorize(Roles = "U")]
    public class UserController : Controller
    {
        RiddleContext db = new RiddleContext();

        public ActionResult LikedRiddles()
        {
            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);

            //order by desc because topside holds last liked riddle
            var likes = db.Likes.Include("LikedRiddle").OrderByDescending(x => x.LikesId).Where(x => x.Liker.UserId == ActiveUser.UserId).ToList();
            
            List<Riddle> riddles = new List<Riddle>();
            foreach (var item in likes)
            {
                riddles.Add(db.Riddles.Include("Owner").FirstOrDefault(x=>x.RiddleId == item.LikedRiddle.RiddleId));
            }
            riddles.Capacity = riddles.Count;
            
            return View(riddles);
        }

        public ActionResult Search()
        {
            List<User> model = new List<User>();//this is basic protection for nullreferenceexception
            model.Capacity = model.Count;//and this is classic RAM protection trick
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(string username)
        {
            List<User> model = new List<User>();
            #region This is dangerous for SQL Injection PROBLEM
            //var MatchesIdList = db.Database.SqlQuery<int>($"select UserId from dbo.Users where UserName LIKE '%{username}%'").ToList();
            /*
            foreach (var MatchesId in MatchesIdList)
            {
                model.Add(db.Users.Find(MatchesId));
            }
             model.Capacity = model.Count;
            */
            #endregion

            var users = db.Users.Where(x=>x.UserRole != "A").ToList();
            foreach (var item in users)
            {
                if (item.UserName.ToLower().IndexOf(username.ToLower(),0,item.UserName.Length)>-1)
                {
                    model.Add(item);
                }
            }
            model.Capacity = model.Count;

            return View(model);
        }

        public ActionResult Index()
        {
            double totalEarned = 0;
            double totalSpent = 0;

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            UserProfileViewModel model = new UserProfileViewModel();

            var UsersRiddles = db.Riddles.Where(r => r.Owner.UserId == ActiveUser.UserId).ToList();

            model.User = ActiveUser;
            model.Riddles = UsersRiddles;
            model.Followers = db.Follows.Where(x => x.Followen.UserId == ActiveUser.UserId).Count();
            model.Follows = db.Follows.Where(x => x.Follower.UserId == ActiveUser.UserId).Count();
            
            int activeRiddles = 0;

            foreach (var item in model.Riddles)
            {
                if (item.RiddleState)
                {
                    activeRiddles++;
                }
            }

            ViewBag.ActiveRiddlesCount = activeRiddles;

            #region Calculating
            var processes = db.AccountProcesses.Where(x => x.CoinAccount.UserId == ActiveUser.UserId).ToList();

            foreach (var process in processes)
            {
                if (process.IsProcesPositive && !process.ProcessType.Equals("Buying Coins") && !process.ProcessType.Equals("Welcome Bonus"))
                {
                    totalEarned += process.ProcessAmount;
                }
                else if (!process.IsProcesPositive && !process.ProcessType.Equals("Buying Coins") && !process.ProcessType.Equals("Welcome Bonus"))
                {
                    totalSpent += process.ProcessAmount;
                }
            }

            try
            {
                double sum = totalEarned + ((totalSpent) * (-1));

                double earnedPercent = ((totalEarned / sum) * 100);
                double spentPercent = ((((totalSpent) * (-1)) / sum) * 100);

                ViewData["totalEarnedPercent"] = Convert.ToInt32(earnedPercent) + "%";
                ViewData["totalSpentPercent"] = Convert.ToInt32(spentPercent) + "%";
            }
            catch (Exception)
            {
                ViewData["totalEarnedPercent"] = "0%";
                ViewData["totalSpentPercent"] = "0%";
            }

            #endregion

            return View(model);
        }

        public ActionResult Wallet()
        {
            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            WalletViewModel model = new WalletViewModel();

            model.Account = db.CoinAccounts.Find(ActiveUser.UserId);
            model.Owner = db.Users.Find(ActiveUser.UserId);
            model.Processes = db.AccountProcesses.Include("Riddle").Where(x => x.CoinAccount.UserId == ActiveUser.UserId).OrderByDescending(x => x.ProcessTime).ToList();

            #region Calculating
            long totalEarned = 0;
            long totalSpent = 0;

            foreach (var process in model.Processes)
            {
                if (process.IsProcesPositive && !process.ProcessType.Equals("Buying Coins") && !process.ProcessType.Equals("Welcome Bonus"))
                {
                    totalEarned += process.ProcessAmount;
                }
                else if (!process.IsProcesPositive && !process.ProcessType.Equals("Buying Coins") && !process.ProcessType.Equals("Welcome Bonus"))
                {
                    totalSpent += process.ProcessAmount;
                }
            }

            model.Earned = totalEarned;
            model.Spend = totalSpent; 
            #endregion

            return View(model);
        }

        public ActionResult Analytics()
        {
            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var model = db.Users.Include("Account").Include("UserRiddles").FirstOrDefault(x=>x.UserId == ActiveUser.UserId);

            #region Total Counting
            long Tlike = 0;
            long Texpense = 0;
            long Tincome = 0;

            foreach (var item in model.UserRiddles)
            {
                if(!item.isRiddleRejected){
                    Tlike += item.RiddleLike;
                    Texpense += item.RiddlePrize;
                    Tincome += (item.AttemptCounter * item.RiddleCost);
                }
            }

            ViewData["Tlike"] = Tlike;
            ViewData["Texpense"] = Texpense;
            ViewData["Tincome"] = Tincome;
            ViewData["Ttotal"] = Tincome - Texpense; 
            #endregion
            
            return View(model);
        }

        public ActionResult Followers(int id)
        {
            var model = new FollowListViewModel();
            model.OrderFollowers = true;
            model.User = db.Users.Find(id);
            model.Followers = new List<User>();

            var FollowerIdList = db.Database.SqlQuery<int>($"select Follower_UserId from dbo.Follows where Followen_UserId = {model.User.UserId}").ToList();

            foreach (var item in FollowerIdList)
            {
                model.Followers.Add(db.Users.Find(item));
            }

            model.Followers.Capacity = model.Followers.Count;
            return View("Follow", model);
        }
        public ActionResult Follows(int id)
        {
            var model = new FollowListViewModel();
            model.OrderFollowers = false;
            model.User = db.Users.Find(id);
            model.Follows = new List<User>();

            var FollowsIdList = db.Database.SqlQuery<int>($"select Followen_UserId from dbo.Follows where Follower_UserId = {model.User.UserId}").ToList();

            foreach (var item in FollowsIdList)
            {
                model.Follows.Add(db.Users.Find(item));
            }
            model.Follows.Capacity = model.Follows.Count;
            return View("Follow", model);
        }

        [HttpPost]
        public ActionResult BuyCoins(int coins, string password)
        {
            User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
            if (coins>0 && password.Equals(ActiveUser.UserPassword))
            {
                if (coins < 1000)
                {
                    ActiveUser.Account.Balance += coins;
                    var process = new AccountProcess()
                    {
                        CoinAccount = ActiveUser.Account,
                        IsProcesPositive = true,
                        ProcessAmount = coins,
                        ProcessTime = DateTime.Now,
                        ProcessType = "Buying Coins"
                    };
                    db.AccountProcesses.Add(process);
                    db.SaveChanges();
                    return RedirectToAction("Wallet");
                }
                else
                {
                    ViewBag.msg = "This number is too high";
                }
            }
            else
            {
                ViewBag.msg = "Process Denied";
            }
            
            return View(ActiveUser);
        }

        public ActionResult BuyCoins()
        {
            ViewBag.msg = "";
            User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
            return View(ActiveUser);
        }

        public ActionResult Auction()
        {
            User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
            ViewBag.User = ActiveUser;//----------------------------------------------------------------------------------------------------------------kontrolü ön tarafta da yapmak için paran yetmio krşm kontrolü
            return View(db.Riddles.Where(x => x.Owner.UserId == ActiveUser.UserId && x.RiddleState && !x.isRiddleRejected).ToList());
        }

        public void GiveOffer(int id, int amount)
        {
            User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
            var riddle = db.Riddles.Include("Owner").FirstOrDefault(x=>x.RiddleId == id);

            if (amount<100000000)
            {
                if (riddle.Owner.UserId == ActiveUser.UserId)//for extra security
                {
                    if (ActiveUser.Account.Balance > amount)
                    {
                        ActiveUser.Account.Balance -= amount;
                        AccountProcess process = new AccountProcess()
                        {
                            CoinAccount = ActiveUser.Account,
                            IsProcesPositive = false,
                            ProcessAmount = (-1) * amount,
                            ProcessTime = DateTime.Now,
                            ProcessType = "Giving Offer",
                            Riddle = riddle
                        };
                        db.AccountProcesses.Add(process);

                        var offer = new Offer()
                        {
                            OfferAmount = amount,
                            OfferRiddle = riddle
                        };
                        db.Offers.Add(offer);
                        db.SaveChanges();
                    }
                }
            }
        }
        public ActionResult EditProfile()
        {
            User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
            return View(ActiveUser);
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
            if (user.UserId == ActiveUser.UserId)
            {
                ActiveUser.UserBio = user.UserBio;
                ActiveUser.UserPassword = user.UserPassword;
                ActiveUser.UserProfileImgPath = user.UserProfileImgPath;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}