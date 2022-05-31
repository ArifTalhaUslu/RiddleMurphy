using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class Likes
    {
        //unique 2 column in 1 key and columns are = Liker & LikedRiddle
        public int LikesId { get; set; }
        public User Liker { get; set; }
        public Riddle LikedRiddle { get; set; }
    }
}