﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Patron.DAL;
using Patron.Models;

namespace Patron.Controllers
{
    public class GoalsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Goals
        public ActionResult Index()
        {
            return View(db.Goals.ToList());
        }

        // GET: Goals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // GET: Goals/Create
        public ActionResult Create()
        {
            Author author= db.Authors.Single(a => a.UserName == User.Identity.Name);

            ViewBag.authorID = author.ID;

            return View();
           }

        // POST: Goals/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goal goal)
        {
            if (ModelState.IsValid)
            {
                Author author = db.Authors.Single(a => a.UserName == User.Identity.Name);
                goal.Author = author;
                goal.isAchieved = "N";
                db.Goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("AuthorPage", "Authors", new { id = author.ID });

            }

            return View(goal);
        }

        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goals/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Goal goal, string isGoalAchieved)
        {
            if (ModelState.IsValid)
            {
                    goal.isAchieved = isGoalAchieved;
                Author author = db.Authors.Single(a => a.UserName == User.Identity.Name);
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AuthorPage", "Authors", new { id = author.ID });

            }
            return View(goal);
        }

        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goal goal = db.Goals.Find(id);
            Author author = goal.Author;
            db.Goals.Remove(goal);
            db.SaveChanges();
            return RedirectToAction("AuthorPage", "Authors", new { id = author.ID });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
