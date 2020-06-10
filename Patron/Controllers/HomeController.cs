using System.Web.Mvc;
using Patron.DAL;
using Patron.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Patron.Controllers
{
    
    public class HomeController : Controller
    {
        private PatronContext db = new PatronContext();

        public ActionResult Index()
        {
            var thresholds = db.AuthorThresholds;
            var authors = db.Authors.Include(a => a.Categories);
            authors = thresholds.OrderBy(at => at.Patrons.Count).Select(t => t.Author).Distinct().Take(3);
            return View(authors.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}