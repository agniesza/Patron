using Patron.DAL;
using Patron.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace Patron.Controllers
{
    public class PostsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Posts
        public ActionResult Index(string sortOrder, string phrase, int? page)
        {
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.NumberOfRatingsSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.AuthorSortParm = String.IsNullOrEmpty(sortOrder) ? "author_desc" : "";

            var posts = db.Posts.Include(p => p.Author);
            switch (sortOrder)
            {
                
                case "Date":
                    posts = posts.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.Date);
                    break;
               
                case "author_desc":
                    posts = posts.OrderByDescending(s => s.Author.UserName);
                    break;
                default:
                    posts = posts.OrderBy(s => s.Date);
                    break;
            }
            if (phrase != null)
            {
                page = 1;
                posts = posts.Where(a => a.Author.FirstName.Contains(phrase)
                    || a.Author.LastName.Contains(phrase)
                    || a.Author.UserName.Contains(phrase)
                    || a.Title.Contains(phrase)
                    || a.Content.Contains(phrase)
                    );
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult ShowAuthorPosts(int? id, int? idpatrona, int? page, string sortOrder)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            ViewBag.RatingSortParm = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var posts = db.Posts.Where(p => p.Author.UserName == author.UserName);

            if (idpatrona != null)
            {
                Models.Patron patron = db.Patrons.Find(idpatrona);
                posts = db.Posts.Where(p => p.Patrons.Any(patr => patr.ID == idpatrona) && 
                p.Author.UserName == author.UserName);

            }
           
            switch (sortOrder)
            {
                
                case "Date":
                    posts = posts.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.Date);
                    break;
                default:
                    posts = posts.OrderBy(s => s.Title);
                    break;
            }
            ViewBag.isAuthor = false;
            if (User.Identity.Name == author.UserName)
            {
                ViewBag.isAuthor = true;
            }
            return View(posts.ToPagedList(pageNumber, pageSize));
            
        }
        public ActionResult ShowPatronPosts(int? id, int? page, string sortOrder)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            ViewBag.patron = patron;
            ViewBag.RatingSortParm = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var posts = db.Posts.Where(p => p.Patrons.Any(patr => patr.UserName==patron.UserName));
            switch (sortOrder)
            {
               
                case "Date":
                    posts = posts.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.Date);
                    break;
                default:
                    posts = posts.OrderBy(s => s.Title);
                    break;
            }

            return View(posts.ToPagedList(pageNumber, pageSize));

        }
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            Author author = db.Authors.Single(a => a.UserName == User.Identity.Name);
            ViewBag.Author = author;
            //ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName");
            return View();
        }

        // POST: Posts/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Post post, int[] authorthresholds)
        {
            post.Patrons = new List<Models.Patron>();
            if (authorthresholds != null)
            {
                foreach (var item in authorthresholds)
                {
                    post.Patrons.Add(db.Patrons.Find(item));
                }
            }
            post.Author = db.Authors.Single(a => a.UserName == User.Identity.Name);
            post.Date = DateTime.Now;
          
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("ShowAuthorPosts", "Posts", new { id = post.Author.ID });

        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", post.AuthorID);
            return View(post);
        }

        // POST: Posts/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AuthorID,Content,Raiting,NumberOfRatings, Date, Title")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowAuthorPosts", "Posts", new { id = post.AuthorID });

            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", post.AuthorID);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
