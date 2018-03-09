using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WhatchaWatchin.Models;
using Models;
using System;
using System.Data;
using System.Web;
using Microsoft.AspNet.Identity;

namespace WhatchaWatchin.Controllers
{
    public class ReviewedMediasController : Controller
    {
        public wwEntities db = new wwEntities();

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

        public ActionResult SendData(string[] userRatings)
        {
            List<int> baseSurveyMovieIDs;
            List<ReviewedMedia> reviewedMediaList = new List<ReviewedMedia>();
            if (Session["genreChoice"].ToString() == "Comedy")
            {
                baseSurveyMovieIDs = new List<int>() { 1, 2, 3, 4, 5 };
            }
            else
            {
                baseSurveyMovieIDs = new List<int>() { 6, 7, 8, 9, 10 };
            }
            for (int i = 0; i < userRatings.Length; i++)
            {
                ReviewedMedia reviewedMovie = new ReviewedMedia
                {
                    MovieID = baseSurveyMovieIDs[i],
                    UserID = User.Identity.GetUserId(),
                    UserRating = int.Parse(userRatings[i])
                };
                reviewedMediaList.Add(reviewedMovie);
            }
            StoreInDatabase(reviewedMediaList);
            //return View(Url.Action("index", "Home"));
            return RedirectToAction("Index", "Home");
        }

        public void StoreInDatabase(List<ReviewedMedia> baseFive)
        {
            foreach (ReviewedMedia movie in baseFive)
            {
                if (ModelState.IsValid)
                {
                    db.ReviewedMedias.Add(movie);
                }
            }
            db.SaveChanges();
        }

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
