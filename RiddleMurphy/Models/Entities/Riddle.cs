using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class Riddle
    {
        public int RiddleId { get; set; }
        [Required(ErrorMessage = "This area must be filled")]
        public string RiddleHeader { get; set; }
        [Required(ErrorMessage = "This area must be filled")]
        public string RiddleText { get; set; }
        [Required(ErrorMessage = "This area must be filled")]
        public string RiddleAnswer { get; set; }
        [Required(ErrorMessage = "This area must be filled")]
        public int RiddlePrize { get; set; }
        [Required(ErrorMessage = "This area must be filled")]
        public int RiddleCost { get; set; }
        public int RiddleLike { get; set; }
        public bool RiddleState { get; set; }
        public bool isRiddleRejected { get; set; }
        public bool IsRiddleAnswered { get; set; }
        public int SolwerId { get; set; }
        public string SolwerName { get; set; }
        public long AttemptCounter { get; set; }
        public bool isTodaysRiddle { get; set; }
        public User Owner { get; set; }
    }
}
