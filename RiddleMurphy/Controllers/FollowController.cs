using RiddleMurphy.Models.Context;
using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RiddleMurphy.Controllers
{
    [Authorize(Roles = "U")]
    public class FollowController : Controller
    {

        RiddleContext db = new RiddleContext();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public void Follow(int id)
        {

            var follower = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var followen = db.Users.Find(id);

            var follow = new Follow()
            {
                Followen = followen,
                Follower = follower,
            };

            db.Follows.Add(follow);
            db.SaveChanges();

        }

        [HttpPost]
        public void UnFollow(int id)
        {

            var follower = db.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var followen = db.Users.Find(id);

            var willdelete = db.Follows.Where(x => x.Followen.UserId == followen.UserId && x.Follower.UserId == follower.UserId).ToList();

            foreach (var item in willdelete)
            {
                if (item != null)
                {
                    db.Follows.Remove(item);
                    db.SaveChanges();
                }
            }

        }
    }
}