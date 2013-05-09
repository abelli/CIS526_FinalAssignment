using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS526_FinalAssignment.Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public byte[] Image { get; set; }
        public string taskName { get; set; }
        public string description { get; set; }
        public int pointTotal { get; set; }
        public bool isMilestone { get; set; }
        public int milestoneBonus { get; set; }
        public string solution { get; set; }

        public int? pathID { get; set; }

        public virtual Leaderboard path { get; set; }
        public virtual ICollection<PlayerTask> playersCompleted { get; set; }
    }
}