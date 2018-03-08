using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WhatchaWatchin.Models;

namespace WhatchaWatchin.Controllers
{
    public class ReviewedMediasController : Controller
    {
        private WhatchaWatchinEntities1 db = new WhatchaWatchinEntities1();

        public ActionResult MediaView()
        {
            return View(db.Media.ToList());
        }
        // GET: ReviewedMedias
        public ActionResult Index()
        {
            return View(db.ReviewedMedias.ToList());
        }
        //public static List<Models.Movie> ComedyList()
        //{

        //}

        // GET: ReviewedMedias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewedMedia reviewedMedia = db.ReviewedMedias.Find(id);
            if (reviewedMedia == null)
            {
                return HttpNotFound();
            }
            return View(reviewedMedia);
        }

        // GET: ReviewedMedias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewedMedias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MovieID,UserID,UserRating")] ReviewedMedia reviewedMedia)
        {
            if (ModelState.IsValid)
            {
                db.ReviewedMedias.Add(reviewedMedia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reviewedMedia);
        }

        // GET: ReviewedMedias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewedMedia reviewedMedia = db.ReviewedMedias.Find(id);
            if (reviewedMedia == null)
            {
                return HttpNotFound();
            }
            return View(reviewedMedia);
        }

        // POST: ReviewedMedias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MovieID,UserID,UserRating")] ReviewedMedia reviewedMedia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviewedMedia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reviewedMedia);
        }

        // GET: ReviewedMedias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewedMedia reviewedMedia = db.ReviewedMedias.Find(id);
            if (reviewedMedia == null)
            {
                return HttpNotFound();
            }
            return View(reviewedMedia);
        }

        // POST: ReviewedMedias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReviewedMedia reviewedMedia = db.ReviewedMedias.Find(id);
            db.ReviewedMedias.Remove(reviewedMedia);
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
