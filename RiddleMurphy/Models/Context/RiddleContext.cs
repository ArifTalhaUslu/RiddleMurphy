using RiddleMurphy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Context
{
    public class RiddleContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Riddle> Riddles { get; set; }
        public DbSet<CoinAccount> CoinAccounts { get; set; }
        public DbSet<AccountProcess> AccountProcesses { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public RiddleContext():base("RiddleMurphyConStr")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}