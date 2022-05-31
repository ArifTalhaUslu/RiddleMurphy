using PagedList;
using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.ViewModels
{
    public class MainViewModel
    {
        public List<User> Users { get; set; }
        public IEnumerable<Riddle> Riddles { get; set; }
        public Riddle TodaysRiddle { get; set; }
        public User ActiveUser { get; set; }

    }
}