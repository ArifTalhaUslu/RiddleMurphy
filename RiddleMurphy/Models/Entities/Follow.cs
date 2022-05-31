using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class Follow
    {
        [Key]
        public long Id { get; set; }
        public User Follower { get; set; }
        public User Followen { get; set; }
    }
}