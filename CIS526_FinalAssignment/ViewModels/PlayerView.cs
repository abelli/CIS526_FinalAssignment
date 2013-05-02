using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS526_FinalAssignment.ViewModels
{
    public class PlayerView
    {
        public int ID { get; set; }
        public string username { get; set; }
        public int totalScore { get; set; }
        public bool isFrozen { get; set; }
    }
}