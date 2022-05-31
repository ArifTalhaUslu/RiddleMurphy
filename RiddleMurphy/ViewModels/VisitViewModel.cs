using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.ViewModels
{
    public class VisitViewModel
    {
        public User HomeOwner { get; set; }
        public User Visitor { get; set; }
        public bool IsVisitorFollowing { get; set; }
        public int HomeOwnerFollowerCount { get; set; }
        public int HomeOwnerFollowsCount { get; set; }

    }
}