using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIS526_FinalAssignment.Models;

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