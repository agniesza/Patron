using Patron.DAL;
using Patron.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Patron.Controllers
{
    public class AuthorThresholdsController : Controller
    {
        private PatronContext db = new PatronContext();

        // GET: AuthorThresholds
        public ActionResult Index()
        {
            var authorThresholds = db.AuthorThresholds.Include(a => a.Author);
            return View(authorThresholds.ToList());
        }

        // GET: AuthorThresholds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            if (authorThreshold == null)
            {
                return HttpNotFound();
            }
            return View(authorThreshold);
        }

        // GET: AuthorThresholds/Create
        public ActionResult Create()
        {
            
            Author author = db.Authors.Single(a => a.UserName == User.Identity.Name);
            ViewBag.authorID = author.ID;
            return View();
        }

        // POST: AuthorThresholds/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AuthorThreshold authorThreshold)
        {
            if (ModelState.IsValid)
            {
                Author author = db.Authors.Single(a => a.UserName == User.Identity.Name);
                authorThreshold.Author = author;
                db.AuthorThresholds.Add(authorThreshold);
                db.SaveChanges();
                return RedirectToAction("AuthorPage", "Authors", new { id = author.ID });
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", authorThreshold.AuthorID);
            return View(authorThreshold);
        }

        // GET: AuthorThresholds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            if (authorThreshold == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", authorThreshold.AuthorID);
            return View(authorThreshold);
        }

        // POST: AuthorThresholds/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AuthorID,Name,Description,Value,MaxNumberOfPatrons")] AuthorThreshold authorThreshold)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authorThreshold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AuthorPage", "Authors", new { id = authorThreshold.AuthorID });

            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "UserName", authorThreshold.AuthorID);
            return View(authorThreshold);
        }

        // GET: AuthorThresholds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            if (authorThreshold == null)
            {
                return HttpNotFound();
            }
            return View(authorThreshold);
        }

        // POST: AuthorThresholds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            Author author = authorThreshold.Author;
            db.AuthorThresholds.Remove(authorThreshold);
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
        //GET
        public ActionResult Support(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            if (authorThreshold == null)
            {
                return HttpNotFound();
            }
            ViewBag.NumbersOfPatrons = authorThreshold.Patrons.Count();
            ViewBag.CurrentPatron = db.Patrons.Where(p => p.UserName.Equals(User.Identity.Name)).Take(1);
            return View(authorThreshold);
        }
        public ActionResult ConfirmSupport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            if (authorThreshold == null)
            {
                return HttpNotFound();
            }
            Models.Patron patron = db.Patrons.Single(p => p.UserName == User.Identity.Name);
            authorThreshold.Patrons.Add(patron);
            db.SaveChanges();
            return View(authorThreshold);
        }
        public ActionResult CancelSupport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthorThreshold authorThreshold = db.AuthorThresholds.Find(id);
            if (authorThreshold == null)
            {
                return HttpNotFound();
            }
            Models.Patron patron = db.Patrons.Single(p => p.UserName == User.Identity.Name);
            authorThreshold.Patrons.Remove(patron);
            db.SaveChanges();
            return RedirectToAction("AuthorPage", "Authors", new { id = id });
        }
    }
}
