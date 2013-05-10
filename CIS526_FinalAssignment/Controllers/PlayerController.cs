using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIS526_FinalAssignment.Models;
using CIS526_FinalAssignment.ViewModels;
using DotNetCasClient;
using System.Web.Security;
using WebMatrix.WebData;

namespace CIS526_FinalAssignment.Controllers
{
    public class PlayerController : Controller
    {
        private PlayerDBContext db = new PlayerDBContext();


        [Authorize]
        public ActionResult LogOn()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    Player p = db.Players.First(pl => pl.username == User.Identity.Name);
                    if (!WebSecurity.UserExists(p.username)) WebSecurity.CreateUserAndAccount(p.username, p.password);
                    if (p.username == "admin") assignAdmin(p);
                    return RedirectToAction("Index", "Leaderboard");
                }

                catch (InvalidOperationException e)
                {
                    Player p = new Player();
                    p.username = User.Identity.Name;
                    p.password = "testpassword";
                    p.isFrozen = false;
                    p.hasChanged = false;
                    db.Players.Add(p);
                    db.SaveChanges();
                    if(!WebSecurity.UserExists(p.username)) WebSecurity.CreateUserAndAccount(p.username, p.password);
                    if(p.username == "admin") assignAdmin(p);
                }
            }
            return RedirectToAction("Index", "Leaderboard");
        }

        public void assignAdmin(Player p)
        {
            string role = "admin";

           // WebSecurity.CreateUserAndAccount(p.username, p.password);
            if (!Roles.RoleExists(role))
            {
                // If not, create one.
                Roles.CreateRole(role);
            }

            // Add the current user to this role
            if(!Roles.IsUserInRole(p.username)) Roles.AddUserToRole(p.username, role);
        }

        public ActionResult LogOff()
        {
            CasAuthentication.SingleSignOut();
            return RedirectToAction("Index", "Leaderboard");
        }

        //
        // GET: /Player/
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.Players.ToList());
        }

        //
        // GET: /Player/Details/5

        public ActionResult Manage()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Player> players = db.Players.ToList();
                foreach (Player player in players)
                {
                    if (User.Identity.Name.Equals(player.username))
                    {
                        return View(player);
                    }
                }
            }

            return Index();
        }

        public ActionResult Details(int id = 0)
        {
            Player player = new Player();
            if (id == -1 || id==0)
            {
                try { player = db.Players.First(pl => pl.username == User.Identity.Name); }
                catch(Exception e){ return HttpNotFound();}
            }

            else
            {
                player = db.Players.Find(id);
                if (player == null)
                {
                    return HttpNotFound();
                }
            }
            return View(player);
        }

        //
        // GET: /Player/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Player/Create

        [HttpPost]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(player);
        }

        //
        // GET: /Player/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        //
        // POST: /Player/Edit/5

        [HttpPost]
        public ActionResult Edit(Player player)
        {

            player.hasChanged = true;
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        //
        // GET: /Player/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        //
        // POST: /Player/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            var pathScores = player.gamesScores.Where(gs => gs.playerID == id);
            var playerTasks = player.tasksCompleted.Where(tc => tc.playerID == id);
            foreach (PlayerTask pt in playerTasks) db.PlayerTasks.Remove(pt);
            foreach (PathScore ps in pathScores) db.PathScores.Remove(ps);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("GetTasks")]
        public JsonResult GetTasks(int id)
        {
            Player player = db.Players.Find(id);
            List<PlayerTask> tasks = player.tasksCompleted.OrderBy(t => t.completionTime).ToList();
            List<PlayerTaskVM> results = new List<PlayerTaskVM>();

            PlayerTaskVM playerVM = new PlayerTaskVM();
            foreach(PlayerTask pt in tasks)
            {
                if (!pt.task.isMilestone)
                {
                    playerVM.taskID = (int)pt.taskID;
                    playerVM.taskName = pt.task.taskName;
                    playerVM.pointsEarned = pt.pointsEarned;
                    playerVM.completionTime = pt.completionTime.Month + "/" + pt.completionTime.Day + "/" + pt.completionTime.Year;
                    results.Add(playerVM);
                }
            }
            results = results.OrderBy(s => s.completionTime).ToList();

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult approve(int id)
        {
            Player p = db.Players.Find(id);
            p.totalScore += p.frozenPoints;
            p.frozenPoints = 0;
            p.isFrozen = false;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }


        [HttpGet, ActionName("GetMilestones")]
        public JsonResult GetMilestones(int id)
        {
            Player player = db.Players.Find(id);
            List<PlayerTask> tasks = player.tasksCompleted.OrderBy(t => t.completionTime).ToList();
            List<PlayerTaskVM> results = new List<PlayerTaskVM>();

            PlayerTaskVM playerVM = new PlayerTaskVM();
            foreach (PlayerTask pt in tasks)
            {
                if (pt.task.isMilestone)
                {
                    playerVM.taskID = (int)pt.taskID;
                    playerVM.taskName = pt.task.taskName;
                    playerVM.pointsEarned = pt.pointsEarned;
                    playerVM.completionTime = pt.completionTime.Month + "/" + pt.completionTime.Day + "/" + pt.completionTime.Year;
                    results.Add(playerVM);
                }
            }
            results = results.OrderBy(s => s.completionTime).ToList();

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("GetChangedPlayers")]
        public JsonResult GetChangedPlayers()
        {
            var players = db.Players.Where(pl => pl.hasChanged == true); ;
            List<PlayerView> results = new List<PlayerView>();

            PlayerView playerView = new PlayerView();
            foreach (Player p in players)
            {
                playerView.ID = p.ID;
                playerView.isFrozen = p.isFrozen;
                playerView.totalScore = p.totalScore;
                playerView.username = p.username;
                results.Add(playerView);
            }

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}