using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS526_FinalAssignment.Models
{
    public class PathScore
    {
        [Key]
        public int ID { get; set; }
        public int? playerID { get; set; }
        public int? leaderboardID { get; set; }
        public int score { get; set; }
        public int rank { get; set; }

        [ForeignKey("playerID")]
        public Player player{ get; set; }

        [ForeignKey("leaderboardID")]
        public Leaderboard leaderboard { get; set; }
    }
}