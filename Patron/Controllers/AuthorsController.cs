using Patron.DAL;
using Patron.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Patron.Controllers
{
    public class AuthorsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: Authors
        public ActionResult Index()
        {
            var authors = db.Authors.Include(a => a.Categories);
            return View(authors.ToList());
        }

        // GET: Authors/Details/5
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
            ViewBag.Categories = author.Categories;
            ViewBag.AuthorThresholds = author.AuthorThresholds;
            //wspierających:
            AuthorThreshold at;
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
                at = db.AuthorThresholds.Find(item.ID);
                mvalue += at.Patrons.Count()*at.Value;
            }
            ViewBag.Monthly = mvalue;
            
            ViewBag.AllPatrons = db.Patrons;

            string currentUserName = User.Identity.Name;
            ViewBag.isPatron = false;
            foreach (var item in author.AuthorThresholds)
            {
                if (item.Patrons.Any(p => p.UserName.Equals(currentUserName)))
                {
                    ViewBag.isPatron=true;
                }
            }


            return View(author);
        }

        // GET: Authors/Create
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
        public ActionResult Create( Author author, int[] cats)
        {
            if (ModelState.IsValid)
            {
               
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

            
            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
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
                return RedirectToAction("Details", new { id = author.ID });

            }


            // ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", author.CategoryID);
            ViewBag.Categories = db.Categories;
            return View(author);
        }

        // GET: Authors/Delete/5
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
