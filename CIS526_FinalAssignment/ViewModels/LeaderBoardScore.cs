using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS526_FinalAssignment.ViewModels
{
    public class LeaderBoardScore
    {
        public int scoreID { get; set; }
        public int playerID { get; set; }
        public int leaderboardID { get; set; }
        public int score { get; set; }
        public int rank { get; set; }
        public string userName { get; set; }
        public string leaderboard { get; set; }
    }
}