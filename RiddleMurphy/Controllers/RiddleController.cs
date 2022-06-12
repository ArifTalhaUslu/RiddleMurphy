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
    public class RiddleController : Controller
    {
        RiddleContext db = new RiddleContext();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public int GetLikes(int id)
        {
            return db.Riddles.Find(id).RiddleLike;
        }

        public bool IsUserLikesThis(int id)
        {
            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var riddle = db.Riddles.Find(id);

            var result = db.Likes.FirstOrDefault(x => x.Liker.UserId == ActiveUser.UserId && x.LikedRiddle.RiddleId == riddle.RiddleId);

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPost]
        public void Like(int id)
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            Riddle riddle = db.Riddles.Find(id);
            Likes like = null;

            like = db.Likes.FirstOrDefault(x => x.Liker.UserId == ActiveUser.UserId && x.LikedRiddle.RiddleId == riddle.RiddleId);

            if (like == null)//user don't likes this
            {
                like = new Likes();

                like.Liker = ActiveUser;
                like.LikedRiddle = riddle;
                riddle.RiddleLike++;
                db.Likes.Add(like);

                db.SaveChanges();
            }
            else
            {
                riddle.RiddleLike--;
                db.Likes.Remove(like);
                db.SaveChanges();
            }


        }

        public ActionResult Details(int id)//int id? 
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var model = db.Riddles.Find(id);

            if (model.Owner == ActiveUser)//Security
            {
                return View(model);
            }

            return HttpNotFound();

        }

        public ActionResult Share()
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            ActiveUser.Account = db.CoinAccounts.Find(ActiveUser.UserId);

            ViewData["ActiveUsersBalance"] = ActiveUser.Account.Balance;
            return View();


        }

        [HttpPost]
        public ActionResult Share(Riddle riddle)
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            ActiveUser.Account = db.CoinAccounts.Find(ActiveUser.UserId);
            riddle.Owner = ActiveUser;

            ViewData["ActiveUsersBalance"] = ActiveUser.Account.Balance;

            AccountProcess process = new AccountProcess()
            {
                CoinAccount = ActiveUser.Account,
                IsProcesPositive = false,
                ProcessAmount = (-1) * (riddle.RiddlePrize),
                ProcessTime = DateTime.Now,
                ProcessType = "Riddle Sharing",
                Riddle = riddle
            };

            if (ActiveUser.Account.Balance < riddle.RiddlePrize)
            {
                ViewData["BalanceErrorMessage"] = "You don't have enough Murphy Coins";
                return View("Share");
            }
            else
            {
                ActiveUser.Account.Balance -= riddle.RiddlePrize;
                riddle.isRiddleRejected = false;
                db.Riddles.Add(riddle);
                db.AccountProcesses.Add(process);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Main");


        }

        public ActionResult Answer(int id)
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            ActiveUser.Account = db.CoinAccounts.Find(ActiveUser.UserId);

            var model = new AnswerViewModel();
            model.Riddle = db.Riddles.Include("Owner").FirstOrDefault(x => x.RiddleId == id);
            model.User = model.Riddle.Owner;

            if (model.Riddle.IsRiddleAnswered)
            {
                return RedirectToAction("Index", "Main");
            }

            if (model.Riddle.RiddleCost > ActiveUser.Account.Balance)
            {
                ViewData["Message"] = "Not enough MC";
            }
            else
            {
                ViewData["Message"] = "";
            }

            ViewData["ActiveUsersBalance"] = ActiveUser.Account.Balance;
            return View(model);


        }

        [HttpPost]
        public ActionResult Answer(string answer, int id, int ownerid)
        {

            User ActiveUser = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var riddle = db.Riddles.Find(id);
            var rOwner = db.Users.Find(ownerid);
            ActiveUser.Account = db.CoinAccounts.Find(ActiveUser.UserId);
            rOwner.Account = db.CoinAccounts.Find(ownerid);

            if (riddle.IsRiddleAnswered)//for security
            {
                return RedirectToAction("Index", "Main");
            }
            else if (!riddle.RiddleState)//for security
            {
                return RedirectToAction("Index", "Main");
            }
            else if (riddle.isRiddleRejected)//for security
            {
                return RedirectToAction("Index", "Main");
            }
            else
            {
                if (ownerid == ActiveUser.UserId)
                {
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    if (ActiveUser.Account.Balance < riddle.RiddleCost)
                    {
                        //not enough credits - for extra security
                        return RedirectToAction("Answer", "Riddle", id);
                    }
                    else
                    {
                        riddle.AttemptCounter++;
                        //true & false under here
                        if (answer.Equals(riddle.RiddleAnswer))
                        {
                            //true answer

                            rOwner.Account.Balance += riddle.RiddleCost;
                            var process = new AccountProcess()
                            {
                                CoinAccount = rOwner.Account,
                                IsProcesPositive = true,
                                ProcessAmount = riddle.RiddleCost,
                                ProcessTime = DateTime.Now,
                                ProcessType = "Riddle Gain",
                                Riddle = riddle
                            };

                            ActiveUser.Account.Balance += riddle.RiddlePrize;
                            var process2 = new AccountProcess()
                            {
                                CoinAccount = ActiveUser.Account,
                                IsProcesPositive = true,
                                ProcessAmount = riddle.RiddlePrize,
                                ProcessTime = DateTime.Now,
                                ProcessType = "Riddle Prize",
                                Riddle = riddle
                            };

                            ActiveUser.Account.Balance -= riddle.RiddleCost;
                            var process3 = new AccountProcess()
                            {
                                CoinAccount = ActiveUser.Account,
                                IsProcesPositive = false,
                                ProcessAmount = (-1) * riddle.RiddleCost,
                                ProcessTime = DateTime.Now,
                                ProcessType = "Riddle Answering Cost",
                                Riddle = riddle
                            };

                            db.AccountProcesses.Add(process);
                            db.AccountProcesses.Add(process2);
                            db.AccountProcesses.Add(process3);

                            riddle.IsRiddleAnswered = true;
                            riddle.SolwerId = ActiveUser.UserId;
                            riddle.SolwerName = ActiveUser.UserName;
                            db.SaveChanges();
                            return RedirectToAction("Wallet", "User");
                        }
                        else
                        {
                            rOwner.Account.Balance += riddle.RiddleCost;
                            var process = new AccountProcess()
                            {
                                CoinAccount = rOwner.Account,
                                IsProcesPositive = true,
                                ProcessAmount = riddle.RiddleCost,
                                ProcessTime = DateTime.Now,
                                ProcessType = "Riddle Gain",
                                Riddle = riddle
                            };

                            ActiveUser.Account.Balance -= riddle.RiddleCost;
                            var process2 = new AccountProcess()
                            {
                                CoinAccount = ActiveUser.Account,
                                IsProcesPositive = false,
                                ProcessAmount = (-1) * riddle.RiddleCost,
                                ProcessTime = DateTime.Now,
                                ProcessType = "Riddle Answering Cost",
                                Riddle = riddle
                            };

                            db.AccountProcesses.Add(process);
                            db.AccountProcesses.Add(process2);
                            db.SaveChanges();

                            return RedirectToAction("Wallet", "User");
                        }
                    }
                }
            }
        }
    }
}
