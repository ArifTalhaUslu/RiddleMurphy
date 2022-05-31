using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public ICollection<Riddle> Riddles { get; set; }
        public int Followers { get; set; }
        public int Follows { get; set; }
    }
}