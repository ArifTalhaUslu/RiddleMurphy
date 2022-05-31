using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.ViewModels
{
    public class FollowListViewModel
    {
        public User User { get; set; }
        public List<User> Follows { get; set; }
        public List<User> Followers { get; set; }
        public bool OrderFollowers { get; set; }
    }
}