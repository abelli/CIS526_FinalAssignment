using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS526_FinalAssignment.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int totalScore { get; set; }
        public bool isFrozen { get; set; }

        public virtual ICollection<PathScore> gamesScores { get; set; }
        public virtual ICollection<PlayerTask> tasksCompleted { get; set; }

    }

    public class PlayerDBContext : DbContext
    {
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<PlayerTask> PlayerTasks { get; set; }
        public DbSet<PathScore> PathScores { get; set; }
    }
}