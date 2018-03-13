using Microsoft.AspNet.Identity;
using Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WhatchaWatchin.Models;

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

        public ActionResult SendData(string[] userRatings)
        {
            List<int> baseSurveyMovieIDs;
            List<ReviewedMedia> reviewedMediaList = new List<ReviewedMedia>();
            if (Session["genreChoice"].ToString() == "Comedy")
            {
                baseSurveyMovieIDs = new List<int>() { 1, 2, 3, 4, 5 };
            }
            else if (Session["genreChoice"].ToString() == "Action")
            {

                baseSurveyMovieIDs = new List<int>() { 38, 68, 69, 70, 12 };

            }

            else if (Session["genreChoice"].ToString() == "Thriller")
            {

                baseSurveyMovieIDs = new List<int>() { 63, 65, 64, 67, 71 };

            }
            else if (Session["genreChoice"].ToString() == "Horror")
            {

                baseSurveyMovieIDs = new List<int>() { 72, 73, 74, 75, 76 };

            }
            else if (Session["genreChoice"].ToString() == "Family")
            {

                baseSurveyMovieIDs = new List<int>() { 77, 78, 22, 79, 80 };

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

            return RedirectToAction("Index", "Home");
        }

        public void StoreInDatabase(List<ReviewedMedia> baseFive)
        {
            foreach (ReviewedMedia movie in baseFive)
            {
                if (ModelState.IsValid)
                {
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WhatchaWatchinConnection"].ConnectionString);
                    SqlCommand cmd = new SqlCommand("dbo.sp_StoreOrUpdateReviewedMedia", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter inputMovieID = new SqlParameter("@MovieID", movie.MovieID);
                    SqlParameter inputUserID = new SqlParameter("@UserID", movie.UserID);
                    SqlParameter inputUserRating = new SqlParameter("@UserRating", movie.UserRating);

                    cmd.Parameters.Add(inputMovieID);
                    cmd.Parameters.Add(inputUserID);
                    cmd.Parameters.Add(inputUserRating);

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataTable();
                    con.Open();
                    da.Fill(ds);
                    con.Close();
                }
            }
            RedirectToAction("index");
        }

        public ActionResult TheAlgorithm()
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WhatchaWatchinConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.sp_GetRecommendedMovie", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter inputParameter = new SqlParameter("@UserID", User.Identity.GetUserId());
            cmd.Parameters.Add(inputParameter);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataTable();
            con.Open();

            da.Fill(ds);

            con.Close();
            List<Movie> returnedMovies = new List<Movie>();

            foreach (DataRow row in ds.Rows)
            {
                Movie returnedMovie = new Movie
                {
                    Title = row.ItemArray[1].ToString().Trim(),
                    Plot = row.ItemArray[2].ToString(),
                    Genre = row.ItemArray[3].ToString(),
                    Year = int.Parse(row.ItemArray[4].ToString()),
                    Type = row.ItemArray[5].ToString().Trim(),
                    Runtime = row.ItemArray[6].ToString().Trim(),
                    Language = row.ItemArray[7].ToString().Trim(),
                    MpaaRating = row.ItemArray[8].ToString().Trim(),
                    ImdbRating = row.ItemArray[9].ToString(),
                    Website = row.ItemArray[10].ToString(),
                    ImdbID = row.ItemArray[11].ToString(),
                    Poster = row.ItemArray[12].ToString()
                };

                returnedMovies.Add(returnedMovie);
            }

            return View("MovieSuggestions", returnedMovies);
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
