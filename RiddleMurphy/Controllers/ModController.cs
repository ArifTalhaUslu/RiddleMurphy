using RiddleMurphy.Models.Context;
using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiddleMurphy.Controllers
{
    [Authorize(Roles = "A")]
    public class ModController : Controller 
    {
        public ActionResult Index()
        {
            using (RiddleContext db = new RiddleContext())
            {
                var model = db.Riddles.Include("Owner").Where(x => !x.RiddleState).ToList();
                return View(model);
            }
        }

        public ActionResult Approval(int id)
        {
            using (RiddleContext db = new RiddleContext())
            {
                if (db.Riddles.Find(id).RiddleState)
                {
                    return RedirectToAction("Index");
                }
                else if (db.Riddles.Find(id).isRiddleRejected)
                {
                    return RedirectToAction("Index");
                }
                var model = db.Riddles.Include("Owner").FirstOrDefault(x => x.RiddleId == id);
                return View(model);
            }
        }

        public void ApprovalThis(int id)
        {
            using (RiddleContext db = new RiddleContext())
            {
                var riddle = db.Riddles.Find(id);
                if (riddle != null)
                {
                    if (!riddle.isRiddleRejected)
                    {
                        riddle.RiddleState = true;
                        riddle.isRiddleRejected = false;
                        db.SaveChanges();
                    }
                }
            }
        }

        public void RejectThis(int id)
        {
            using (RiddleContext db = new RiddleContext())
            {
                var riddle = db.Riddles.Include("Owner").FirstOrDefault(x => x.RiddleId == id);
                var owner = db.Users.Include("Account").FirstOrDefault(x => x.UserId == riddle.Owner.UserId);
                if (riddle != null)
                {
                    owner.Account.Balance += riddle.RiddlePrize;
                    AccountProcess process = new AccountProcess()
                    {
                        CoinAccount = owner.Account,
                        IsProcesPositive = true,
                        ProcessAmount = riddle.RiddlePrize,
                        ProcessTime = DateTime.Now,
                        ProcessType = "Rejected post credit refund",
                        Riddle = riddle
                    };
                    db.AccountProcesses.Add(process);
                    riddle.isRiddleRejected = true;
                    db.SaveChanges();
                }
            }
        }

        public ActionResult TodaysRiddle()
        {
            ViewBag.message = "";
            using (RiddleContext db = new RiddleContext())
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult TodaysRiddle(string pass)
        {
            using (RiddleContext db = new RiddleContext())
            {
                User ActiveUser = db.Users.Include("Account").FirstOrDefault(m => m.UserName == User.Identity.Name);
                if (ActiveUser.UserPassword.Equals(pass))
                {
                    SetTodaysRiddle();
                    ViewBag.message = "Todays riddle setted";
                }
                else
                {
                    ViewBag.message = "Access Denied";
                }
                return View();
            }
        }

        public void SetTodaysRiddle()
        {
            using (RiddleContext db = new RiddleContext())
            {
                int CountOfOffers = db.Offers.Count();
                if (CountOfOffers > 0)
                {
                    var max = db.Offers.Max(x => x.OfferAmount);
                    var topoffer = db.Offers.Include("OfferRiddle").OrderByDescending(x => x.OfferAmount).ThenBy(x => x.OfferId).Take(1);

                    foreach (var item in topoffer)
                    {
                        using (RiddleContext db2 = new RiddleContext())
                        {
                            var commandText1 = "UPDATE dbo.Riddles SET isTodaysRiddle = 0";
                            db2.Database.ExecuteSqlCommand(commandText1);
                        }


                        item.OfferRiddle.isTodaysRiddle = true;
                        break;
                    }

                    foreach (var item in db.Offers.Include("OfferRiddle").ToList())
                    {
                        if (!item.OfferRiddle.isTodaysRiddle)
                        {
                            using (RiddleContext db3 = new RiddleContext())
                            {
                                string command = "update dbo.CoinAccounts set Balance += (select OfferAmount from dbo.Offers where OfferId = @refullid) WHERE UserId = (Select Owner_UserId from dbo.Riddles where RiddleId = (select OfferRiddle_RiddleId from dbo.Offers where OfferId = @refullid))";
                                var name = new SqlParameter("@refullid", item.OfferId);
                                db3.Database.ExecuteSqlCommand(command, name);
                            }

                            
                            var riddle = db.Riddles.Include("Owner").First(x=>x.RiddleId == item.OfferRiddle.RiddleId);
                            var account = db.CoinAccounts.Find(riddle.Owner.UserId);
                            AccountProcess process = new AccountProcess()
                            {
                                CoinAccount = account,
                                IsProcesPositive = true,
                                ProcessAmount = item.OfferAmount,
                                ProcessTime = DateTime.Now,
                                ProcessType = "Offer refull",
                                Riddle = riddle
                            };
                            db.AccountProcesses.Add(process);
                        }
                    }

                    var commandText2 = "DELETE dbo.Offers";
                    db.Database.ExecuteSqlCommand(commandText2);

                    db.SaveChanges();
                }
            }
        }
    }
}