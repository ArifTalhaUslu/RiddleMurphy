using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.ViewModels
{
    public class WalletViewModel
    {
        public User Owner { get; set; } 
        public CoinAccount Account { get; set; }

        public long Spend { get; set; }
        public long Earned { get; set; }
        public IEnumerable<AccountProcess> Processes { get; set; }
    }
}