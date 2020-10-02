using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Patron.DAL;
using Patron.Models;

namespace Patron.Controllers
{
    public class CommentsController : Controller
    {
        private PatronContext db = new PatronContext();

        public ActionResult AuthorComments(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comments = db.Comments.OrderByDescending(c => c.Date).Include(c => c.Author).Include(c => c.Patron).Where(c => c.AuthorID==id);
            Comment cc = comments.First();
            ViewBag.AuthorId = cc.AuthorID;
            ViewBag.isThisAuthor = false;
            if (cc.Author.UserName.Equals(User.Identity.Name))
            {
                ViewBag.isThisAuthor = true;
            }
            ViewBag.isLoggedInAsPatron = false;
            
                foreach (var item in cc.Author.Payments)
                {
                    if (item.Patron.UserName.Equals(User.Identity.Name))
                    {
                    ViewBag.isLoggedInAsPatron = true;
                }

                }
                
            

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(comments.ToPagedList(pageNumber, pageSize));
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        // GET: Comments/Create
        public ActionResult CreateComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron p = db.Patrons.Single(pp => pp.UserName == User.Identity.Name);

            ViewBag.Author = db.Authors.Find(id);
            Comment comment = new Comment
            {
                Patron = p,
                Author = db.Authors.Find(id),
                Date = DateTime.Now,
                Rate = 5,
                Content = " "
            };

            db.Comments.Add(comment);
            db.SaveChanges();
            int idik = comment.ID;
            Comment com = db.Comments.Find(idik);
            return RedirectToAction("Create", "Comments", new { id = com.ID });

        }
        // GET: Comments/Create
        public ActionResult Create( int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Author = comment.Author;
            ViewBag.Patron = comment.Patron;

            return View(comment);
        }

        // POST: Comments/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
             //comment.Date = DateTime.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AuthorComments", "Comments", new { id = comment.AuthorID });

          
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
           return View(comment);
        }

        // POST: Comments/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AuthorComments", "Comments", new { id = comment.AuthorID });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Comment comment = db.Comments.Find(id);
            Author a = comment.Author;
            if (comment == null)
            {
                return HttpNotFound();
            }
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("AuthorComments", "Comments", new { id = a.ID });

        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
