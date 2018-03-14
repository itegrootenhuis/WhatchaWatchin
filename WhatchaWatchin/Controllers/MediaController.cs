using Microsoft.AspNet.Identity;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WhatchaWatchin.Models;
using System.Configuration;

namespace WhatchaWatchin.Controllers
{
    public class MediaController : Controller
    {
        private wwEntities db = new wwEntities();

        public ActionResult AddSingleRateToDB(int userRating)
        {
            ReviewedMedia r = new ReviewedMedia()
            {
                MovieID = int.Parse(Session["returnedMovieID"].ToString()),
                UserID = User.Identity.GetUserId(),
                UserRating = userRating
            };

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WhatchaWatchinConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.sp_StoreOrUpdateReviewedMedia", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter inputMovieID = new SqlParameter("@MovieID", r.MovieID);
            SqlParameter inputUserID = new SqlParameter("@UserID", r.UserID);
            SqlParameter inputUserRating = new SqlParameter("@UserRating", r.UserRating);

            cmd.Parameters.Add(inputMovieID);
            cmd.Parameters.Add(inputUserID);
            cmd.Parameters.Add(inputUserRating);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataTable();
            con.Open();
            da.Fill(ds);
            con.Close();

            return RedirectToAction("index", "home");
        }

        public ActionResult GetMovieToRate(string movieToSearch)
        {
            string apiKey = ConfigurationManager.AppSettings["MovieAPIKey"];
            List<Medium> mediums = new List<Medium>();
            HttpWebRequest request = WebRequest.CreateHttp("http://www.omdbapi.com/?apikey=" + apiKey + "&t=" + movieToSearch);
            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            String data = rd.ReadToEnd();
            JObject o = JObject.Parse(data);

            JToken _title = o["Title"];
            JToken _plot = o["Plot"];
            JToken _poster = o["Poster"];
            JToken _genre = o["Genre"];
            JToken _type = o["Type"];
            JToken _year = o["Year"];
            JToken _mpaaRating = o["Rated"];
            JToken _runtime = o["Runtime"];
            JToken _language = o["Language"];
            JToken _imdbRating = o["imdbRating"];
            JToken _website = o["Website"];
            JToken _imdbID = o["imdbID"];

            try
            {
                Medium m = new Medium(_title.ToString(), _plot.ToString(), _poster.ToString(), _genre.ToString(), _year.ToString(), _type.ToString(), _mpaaRating.ToString(), _runtime.ToString(), _language.ToString(), decimal.Parse(_imdbRating.ToString()), _website.ToString(), _imdbID.ToString());
                mediums.Add(m);
                MethodThatAddsMovieOjbectToDatabase(m);
            }
            catch (Exception e)
            {
                ViewBag.BadMovieSearch = movieToSearch;
                return View("ErrorSingleSearch");
            }
            return View("SingleRate", mediums);
        }

        public ActionResult SingleRate()
        {
            return View();
        }

        public void MethodThatAddsMovieOjbectToDatabase(Medium m)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WhatchaWatchinConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.sp_StoreMedia", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter inputTitle = new SqlParameter("@Title", m.Title);
            SqlParameter inputPlot = new SqlParameter("@Plot", m.Plot);
            SqlParameter inputPoster = new SqlParameter("@Poster", m.Poster);
            SqlParameter inputGenre = new SqlParameter("@Genre", m.Genre);
            SqlParameter inputYear = new SqlParameter("@Year", m.Year);
            SqlParameter inputType = new SqlParameter("@Type", m.Type);
            SqlParameter inputRuntime = new SqlParameter("@Runtime", m.Runtime);
            SqlParameter inputLanguage = new SqlParameter("@Language", m.Language);
            SqlParameter inputMPAARating = new SqlParameter("@MPAARating", m.MPAARating);
            SqlParameter inputIMDBRating = new SqlParameter("@IMDBRating", m.IMDBRating);
            SqlParameter inputWebsite = new SqlParameter("@Website", m.Website);
            SqlParameter inputimdbID = new SqlParameter("@imdbID", m.imdbID);

            cmd.Parameters.Add(inputTitle);
            cmd.Parameters.Add(inputPlot);
            cmd.Parameters.Add(inputPoster);
            cmd.Parameters.Add(inputGenre);
            cmd.Parameters.Add(inputYear);
            cmd.Parameters.Add(inputType);
            cmd.Parameters.Add(inputRuntime);
            cmd.Parameters.Add(inputLanguage);
            cmd.Parameters.Add(inputMPAARating);
            cmd.Parameters.Add(inputIMDBRating);
            cmd.Parameters.Add(inputWebsite);
            cmd.Parameters.Add(inputimdbID);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataTable();
            con.Open();
            da.Fill(ds);
            con.Close();

            List<rmID> returnedMovies = new List<rmID>();

            foreach (DataRow row in ds.Rows)
            {
                rmID returnedMovie = new rmID
                {
                    MovieID = int.Parse(row.ItemArray[0].ToString().Trim())
                };
                returnedMovies.Add(returnedMovie);
            }
            Session["returnedMovieID"] = returnedMovies[0].MovieID;
        }




        // GET: Media
        public ActionResult Index()
        {
            return View(db.Media.ToList());
        }

        // GET: Media/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medium medium = db.Media.Find(id);
            if (medium == null)
            {
                return HttpNotFound();
            }
            return View(medium);
        }

        // GET: Media/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Media/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Plot,Genre,Year,Type,Runtime,Language,MPAARating,IMDBRating,Website,imdbID,Poster")] Medium medium)
        {
            if (ModelState.IsValid)
            {
                db.Media.Add(medium);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medium);
        }

        // GET: Media/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medium medium = db.Media.Find(id);
            if (medium == null)
            {
                return HttpNotFound();
            }
            return View(medium);
        }

        // POST: Media/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Plot,Genre,Year,Type,Runtime,Language,MPAARating,IMDBRating,Website,imdbID,Poster")] Medium medium)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medium).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medium);
        }

        // GET: Media/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medium medium = db.Media.Find(id);
            if (medium == null)
            {
                return HttpNotFound();
            }
            return View(medium);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medium medium = db.Media.Find(id);
            db.Media.Remove(medium);
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
