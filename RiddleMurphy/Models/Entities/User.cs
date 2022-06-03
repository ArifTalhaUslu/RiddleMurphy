using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "This area must be filled")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Index(IsUnique =true)]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string UserBio { get; set; }

        [Required(ErrorMessage = "This area must be filled")]
        public string UserPassword { get; set; }
        [Required(ErrorMessage = "This area must be filled")]
        public string UserProfileImgPath { get; set; }
        [Required]
        public DateTime UserJoinDate { get; set; }
        public string UserRole { get; set; }
        public ICollection<Riddle> UserRiddles { get; set; }
        public CoinAccount Account { get; set; }
    }
}
 