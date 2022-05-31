using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.ViewModels
{
    public class AnswerViewModel
    {
        public Riddle Riddle { get; set; }
        public User User { get; set; }
    }
}