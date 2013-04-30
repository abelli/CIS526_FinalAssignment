using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIS526_FinalAssignment.Models;
using CIS526_FinalAssignment.ViewModels;

namespace CIS526_FinalAssignment.Controllers
{
    public class LeaderboardController : Controller
    {
        private PlayerDBContext db = new PlayerDBContext();
        
        //
        // GET: /Leaderboard/

        public ActionResult Index()
        {
            return View(db.Leaderboards.ToList());
        }

        //
        // GET: /Leaderboard/Details/5

        public ActionResult Details(int id = 0)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            Rank(leaderboard);
            if (leaderboard == null)
            {
                return HttpNotFound();
            }
            return View(leaderboard);
        }

        //
        // GET: /Leaderboard/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Leaderboard/Create

        [HttpPost]
        public ActionResult Create(Leaderboard leaderboard)
        {
            if (ModelState.IsValid)
            {
                db.Leaderboards.Add(leaderboard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leaderboard);
        }

        //
        // GET: /Leaderboard/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            if (leaderboard == null)
            {
                return HttpNotFound();
            }
            return View(leaderboard);
        }

        //
        // POST: /Leaderboard/Edit/5

        [HttpPost]
        public ActionResult Edit(Leaderboard leaderboard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaderboard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaderboard);
        }

        //
        // GET: /Leaderboard/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            if (leaderboard == null)
            {
                return HttpNotFound();
            }
            return View(leaderboard);
        }

        //
        // POST: /Leaderboard/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            db.Leaderboards.Remove(leaderboard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("GetJson")]
        public JsonResult GetScores(int id, int rank = 1)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            Rank(leaderboard);
            List<PathScore> scores = leaderboard.scores.OrderBy(p => p.rank).ToList();
            List<LeaderBoardScore> results = new List<LeaderBoardScore>();
            int max = rank + 50;
            if (max > scores.Count) max = scores.Count;

            for (int i = rank-1; i < max; i++) 
            {
                PathScore cur = scores.ElementAt(i);
                LeaderBoardScore score = new LeaderBoardScore();
                score.scoreID = cur.ID;
                score.playerID = (int)cur.playerID;
                score.leaderboardID = (int)cur.leaderboardID;
                score.score = cur.score;
                score.rank = cur.rank;
                Player player = db.Players.Find(cur.playerID);
                score.userName = player.username;
                score.leaderboard = leaderboard.pathName;
                results.Add(score);
            }
            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public void Rank(Leaderboard board)
        {
            List<PathScore> scores = board.scores.OrderByDescending(p => p.score).ToList();
            int i = 1;
            foreach (PathScore score in scores)
            {
                score.rank = i;
                i++;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}