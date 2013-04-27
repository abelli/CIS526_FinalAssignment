using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS526_FinalAssignment.Models
{
    public class Leaderboard
    {
        [Key]
        public int ID { get; set; }
        public string pathName { get; set; }

        public virtual ICollection<PathScore> scores { get; set; }
    }
}