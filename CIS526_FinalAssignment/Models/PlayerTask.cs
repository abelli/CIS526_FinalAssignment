using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS526_FinalAssignment.Models
{
    public class PlayerTask
    {
        [Key]
        public int ID { get; set; }
        public int? playerID { get; set; }
        public int? taskID { get; set; }
        public int pointsEarned { get; set; }
        public DateTime completionTime { get; set; }

        [ForeignKey("playerID")]
        public virtual Player player { get; set; }
        
        [ForeignKey("taskID")]
        public virtual Task task { get; set; }

    }
}