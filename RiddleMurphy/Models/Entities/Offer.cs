using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class Offer
    {
        public int OfferId { get; set; }
        public int OfferAmount { get; set; }
        public Riddle OfferRiddle { get; set; }
    }
}