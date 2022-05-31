using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RiddleMurphy.Models.Entities
{
    public class AccountProcess
    {
        [Key]
        public int ProcessId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ProcessType { get; set; }
        public bool IsProcesPositive { get; set; }
        public long ProcessAmount { get; set; }
        public Riddle Riddle { get; set; }//it can be null - and holds process's related riddle

        [Range(typeof(DateTime), "1/1/2020", "1/1/2100")]
        public DateTime ProcessTime { get; set; }
        public CoinAccount CoinAccount { get; set; }
    }
}