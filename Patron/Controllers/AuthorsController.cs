using Patron.DAL;
using Patron.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System;

namespace Patron.Controllers
{
    public class AuthorsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Authors
        public ActionResult Index(string phrase, int? page)
        {
            
            var authors = db.Authors.Include(a => a.Categories).OrderBy(aa => aa.UserName);
            if (phrase != null)
            {
                page = 1;
                authors = authors.Where(a => a.FirstName.Contains(phrase)
                    || a.LastName.Contains(phrase)
                    || a.UserName.Contains(phrase)).OrderBy(aa => aa.Payments.Count);
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        // GET: Authors/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
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
            ViewBag.Avt = author.Avatar;
            ViewBag.Categories = author.Categories;
            return View(author);
        }

        public ActionResult AuthorPage(int? id)
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
            ViewBag.Avt = author.Avatar;
            ViewBag.AuthorThresholds = author.AuthorThresholds;
            //wspierających:
            Threshold at;
            int num = 0;
            foreach (var item in author.AuthorThresholds)
            {
                at = db.AuthorThresholds.Find(item.ID);
                num += at.Patrons.Count();
            }
            ViewBag.PatronsCount = num;
            //miesięcznie
            int mvalue = 0;
            foreach (var item in author.AuthorThresholds)
            {
             //   at = db.AuthorThresholds.Find(item.ID);
                mvalue += item.Patrons.Count()*item.Value;
            }
            ViewBag.Monthly = mvalue;
            
            ViewBag.AllPatrons = db.Patrons;

            string currentUserName = User.Identity.Name;
            
                ViewBag.isPatron = false;
            ViewBag.supportedID = null;
            foreach (var item in author.AuthorThresholds)
            {
                if (item.Patrons.Any(p => p.UserName.Equals(currentUserName)))
                {
                    ViewBag.isPatron=true;
                    ViewBag.supportedID = item.ID;
                }
            }
            ViewBag.isFollower = false;
            
                if (author.Followers.Any(p => p.UserName.Equals(currentUserName)))
                {
                    ViewBag.isFollower = true;
                }
            
            ViewBag.isLoggedInAsPatron = false;
            foreach (var item in db.Patrons)
            {
                if (item.UserName.Equals(currentUserName))
                {
                    ViewBag.isLoggedInAsPatron = true;
                }
            }
            ViewBag.TotalMonay = author.Payments.Sum(p => p.Value);

            return View(author);
        }

        // GET: Authors/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
           
            ViewBag.Categories = db.Categories;
            return View();
        }

        // POST: Authors/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create( Author author, int[] cats)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["avatarFile"];
                if (file != null && file.ContentLength > 0)
                {
                    author.Avatar = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Avatars/") + author.Avatar);
                }
                author.Categories = new List<Category>();
                if (cats != null)
                {
                    foreach (var item in cats)
                    {
                        author.Categories.Add(db.Categories.Find(item));
                    }
                }
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = db.Categories;

            return View(author);
        }

        [Authorize]
        public ActionResult AddAuthorProfile(int? id)
        {
            Models.Patron p = db.Patrons.Find(id);
            Author author = new Author { UserName = p.UserName };
            
            db.Authors.Add(author);
            db.SaveChanges();

            // return View("~/Views/Patrons/Edit.cshtml", patron);
            return RedirectToAction("Edit", "Authors", new { id = author.ID });


        }
        // GET: Authors/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (db.Patrons.Any(p => p.UserName.Equals(User.Identity.Name)))
            {
                Models.Patron patron = db.Patrons.Single(p => p.UserName.Equals(User.Identity.Name));
                author.FirstName = patron.FirstName;
                author.LastName = patron.LastName;
            }
            if (author == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Categories = db.Categories; 
            return View(author);
        }

        // POST: Authors/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Edit(Author author, int[] cats)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["avatarFile"];
                if (file != null && file.ContentLength > 0)
                {
                    author.Avatar = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Avatars/") + author.Avatar);
                }
                author.Categories = new List<Category>();
                if (cats != null)
                {
                    foreach (var item in cats)
                    {
                        author.Categories.Add(db.Categories.Find(item));
                    }
                }
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AuthorPage", new { id = author.ID });

            }


            // ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", author.CategoryID);
            ViewBag.Categories = db.Categories;
            return View(author);
        }
        [Authorize]
        public ActionResult Follow(int? id)
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
            Models.Patron patron = db.Patrons.Single(p => p.UserName == User.Identity.Name);
            author.Followers.Add(patron);
            db.SaveChanges();
            return RedirectToAction("AuthorPage", new { id = author.ID });

        }
        [Authorize]
        public ActionResult Unfollow(int? id)
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
            Models.Patron patron = db.Patrons.Single(p => p.UserName == User.Identity.Name);
            author.Followers.Remove(patron);
            db.SaveChanges();
            return RedirectToAction("AuthorPage", new { id = author.ID });

        }
        
        public ActionResult Followers(int? id)
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
            return View(author);

        }
        [Authorize]
        public ActionResult EditProfile(int? id)
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
            ViewBag.Categories = db.Categories;
            List<Category> acat = (author.Categories).GetRange(0, (author.Categories).Count);
            ViewBag.AuthorCategories = acat;
            author.Categories.Clear();
            db.SaveChanges();
            return View(author);
        }
        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult EditProfile( Author author, int[] cats )
        {
            if (ModelState.IsValid)
            {
                
                HttpPostedFileBase file = Request.Files["avatarFile2"];
                if (file != null && file.ContentLength > 0)
                {
                    author.Avatar = file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Avatars/") + author.Avatar);
                }
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                if (cats != null)
                {
                    author.Categories = new List<Category>();

                    foreach (var item in cats)
                    {
                             author.Categories.Add(db.Categories.Find(item));
                            db.SaveChanges();
                        
                    }
                }
                
                return RedirectToAction("AuthorPage", new { id = author.ID });
            }
            ViewBag.AuthorCategories = author.Categories;
            return View(author);
        }
        // GET: Authors/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
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
            ViewBag.Categories = author.Categories;
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
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
