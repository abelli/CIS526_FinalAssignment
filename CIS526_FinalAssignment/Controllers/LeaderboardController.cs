﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIS526_FinalAssignment.Models;
using CIS526_FinalAssignment.ViewModels;
using DotNetCasClient;
using WebMatrix.WebData;

namespace CIS526_FinalAssignment.Controllers
{
    public class LeaderboardController : Controller
    {

        [Authorize]
        public ActionResult LogOn()
        {
            return RedirectToAction("Index", "Leaderboard");
        }

        public ActionResult LogOff()
        {
            CasAuthentication.SingleSignOut();
            return RedirectToAction("Index", "Leaderboard");
        }

        private PlayerDBContext db = new PlayerDBContext();
        
        //
        // GET: /Leaderboard/

        public ActionResult Index()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
            return View(db.Leaderboards.ToList());
        }

        //
        // GET: /Leaderboard/Details/5

        public ActionResult Details(int id = 0)
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

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
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Leaderboard/Create

        [HttpPost]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            db.Leaderboards.Remove(leaderboard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("GetTopTen")]
        public JsonResult GetTopTenScores(int id)
        {
            int rank = 1;
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            Rank(leaderboard);
            List<PathScore> scores = leaderboard.scores.OrderBy(p => p.rank).ToList();
            List<LeaderBoardScore> results = new List<LeaderBoardScore>();
            int max = rank + 9;
            if (max > scores.Count) max = scores.Count;

            for (int i = rank - 1; i < max; i++)
            {
                PathScore cur = scores.ElementAt(i);
                LeaderBoardScore score = new LeaderBoardScore();
                score.scoreID = cur.ID;
                score.playerID = (int)cur.playerID;
                score.leaderboardID = (int)cur.leaderboardID;
                score.score = cur.score;
                score.rank = cur.rank;
                Player player = db.Players.Find(cur.playerID);
                List<TaskView> tasks = new List<TaskView>();
                foreach (PlayerTask pt in player.tasksCompleted)
                {
                    TaskView tv = new TaskView();
                    tv.image = pt.task.image;
                    tv.task = pt.task.taskName;
                    tv.taskId = pt.task.ID;

                    tasks.Add(tv);
                }
                score.tasks = tasks.ToArray();

                score.userName = player.username;
                score.leaderboard = leaderboard.pathName;
                results.Add(score);
            }
            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("GetScores")]
        public JsonResult GetScores(int id, int rank = 1)
        {
            Leaderboard leaderboard = db.Leaderboards.Find(id);
            Rank(leaderboard);
            List<PathScore> scores = leaderboard.scores.OrderBy(p => p.rank).ToList();
            List<LeaderBoardScore> results = new List<LeaderBoardScore>();
            int max = rank + 24;
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
                List<TaskView> tasks = new List<TaskView>();
                foreach (PlayerTask pt in player.tasksCompleted)
                {
                    TaskView tv = new TaskView();
                    tv.image = pt.task.image;
                    tv.task = pt.task.taskName;
                    tv.taskId = pt.task.ID;

                    tasks.Add(tv);
                }
                score.tasks = tasks.ToArray();
                score.userName = player.username;
                score.leaderboard = leaderboard.pathName;
                results.Add(score);
            }
            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("GetPlayers")]
        public JsonResult GetPlayers()
        {
            List<Player> players = db.Players.ToList();
            List<PlayerView> results = new List<PlayerView>();

            foreach (Player player in players)
            {
                PlayerView res = new PlayerView();
                res.ID = player.ID;
                res.username = player.username;
                res.totalScore = player.totalScore;
                res.isFrozen = player.isFrozen;
                results.Add(res);
            }
            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ActionName("GetIcons")]
        public JsonResult GetIcons(int id)
        {
            Player player = db.Players.Find(id);
            List<string> icons = new List<string>();

            foreach (PlayerTask t in player.tasksCompleted)
            {
                string s = t.task.image;
                icons.Add(s);
            }

            return Json(icons.ToArray(), JsonRequestBehavior.AllowGet);
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