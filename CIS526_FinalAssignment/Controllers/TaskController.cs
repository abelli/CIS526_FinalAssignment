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
    public class TaskController : Controller
    {
        private PlayerDBContext db = new PlayerDBContext();

        //
        // GET: /Task2/

        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.path);
            return View(tasks.ToList());
        }

        //
        // GET: /Task2/Details/5

        public ActionResult Details(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // GET: /Task2/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.pathID = new SelectList(db.Leaderboards, "ID", "pathName");
            return View();
        }

        //
        // POST: /Task2/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pathID = new SelectList(db.Leaderboards, "ID", "pathName", task.pathID);
            return View(task);
        }

        //
        // GET: /Task2/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.pathID = new SelectList(db.Leaderboards, "ID", "pathName", task.pathID);
            return View(task);
        }

        //
        // POST: /Task2/Edit/5

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pathID = new SelectList(db.Leaderboards, "ID", "pathName", task.pathID);
            return View(task);
        }

        //
        // GET: /Task2/
        public ActionResult Complete()
        {
            return View(db.Tasks.ToList());
        }

        public ActionResult SolveTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [HttpPost]
        //[Authorize(Roles="admin")]
        public ActionResult SolveTask(string solution, int ID)
        {
            //check solution
            Task FinishedTask = db.Tasks.Find(ID);
            if (FinishedTask.solution == solution)
            {
                try
                {
                    Player p = db.Players.First(pl => pl.username == User.Identity.Name);
                    PlayerTask pt = new PlayerTask();
                    pt.playerID = p.ID;
                    pt.taskID = ID;
                    pt.completionTime = DateTime.Now;
                    //Check if player is frozen
                    if (p.isFrozen == true)
                    {
                        p.frozenPoints += FinishedTask.pointTotal;
                        pt.pointsEarned = 0;
                    }
                    else
                    {
                        pt.pointsEarned = FinishedTask.pointTotal;
                    }

                    //Check if milestone. //milestone bonus plus the 1st 10 students points
                    if (FinishedTask.isMilestone == true && FinishedTask.playersCompleted.Count < 10)
                        pt.pointsEarned += FinishedTask.milestoneBonus;

                    try
                    {
                        p.tasksCompleted.First(t => t.taskID == ID);
                        return RedirectToAction("Index");
                    }

                    catch(Exception ex)
                    {
                        PathScore ps;
                        p.totalScore += pt.pointsEarned;
                        try
                        {
                            ps = p.gamesScores.First(pas => pas.leaderboardID == FinishedTask.pathID);
                            ps.score += pt.pointsEarned;
                        }

                        catch (InvalidOperationException e)
                        {
                            ps = new PathScore();
                            ps.leaderboardID = FinishedTask.pathID;
                            ps.playerID = p.ID;
                            ps.rank = 1; //?????
                            ps.score = pt.pointsEarned;
                            db.PathScores.Add(ps);
                            db.SaveChanges();
                        }
                    }

                    db.PlayerTasks.Add(pt);
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(FinishedTask).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    return RedirectToAction("Index");
                }
                return View("CorrectAnswer", FinishedTask);
            }
            else
                return View("WrongAnswer", FinishedTask);
        }
        //
        // GET: /Task2/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // POST: /Task2/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}