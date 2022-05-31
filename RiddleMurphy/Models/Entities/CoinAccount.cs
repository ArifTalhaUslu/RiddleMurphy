using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class CoinAccount
    {
        [ForeignKey("User")]
        [Index(IsUnique = true), Key]
        public int UserId { get; set; }
        public int Balance { get; set; }
        public User User { get; set; }
        public ICollection<AccountProcess> AccountProcesses { get; set; }

    }
}