using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS526_FinalAssignment.ViewModels
{
    public class PlayerTaskVM
    {
        public int taskID { get; set; }
        public string taskName { get; set; }
        public int pointsEarned { get; set; }
        public DateTime completionTime { get; set; }

    }
}