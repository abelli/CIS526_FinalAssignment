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

        public ActionResult Create()
        {
            ViewBag.pathID = new SelectList(db.Leaderboards, "ID", "pathName");
            return View();
        }

        //
        // POST: /Task2/Create

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
        public ActionResult SolveTask(string solution, int ID)
        {
            //check solution
            Task FinishedTask = db.Tasks.Find(ID);
            if (FinishedTask.solution == solution)
            {

                return View("CorrectAnswer", FinishedTask);
            }
            else
                return View("WrongAnswer", FinishedTask);
        }
        //
        // GET: /Task2/Delete/5
        
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