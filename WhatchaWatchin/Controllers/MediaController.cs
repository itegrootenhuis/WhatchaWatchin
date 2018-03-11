﻿using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhatchaWatchin.Models;

namespace WhatchaWatchin.Controllers
{
    public class MediaController : Controller
    {
        private wwEntities db = new wwEntities();

        public ActionResult GetMovieToRate(string movieToSearch)
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://www.omdbapi.com/?apikey=d0069624&t=" + movieToSearch);
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

            // try
            //{
            Medium m = new Medium(_title.ToString(), _plot.ToString(), _poster.ToString(), _genre.ToString(), _year.ToString(), _type.ToString(), _mpaaRating.ToString(), _runtime.ToString(), _language.ToString(), decimal.Parse(_imdbRating.ToString()), _website.ToString(), _imdbID.ToString());
            //Movie m = new Movie(_title.ToString(), _plot.ToString(), _poster.ToString(), _genre.ToString(), _type.ToString(), int.Parse(_year.ToString()), _mpaaRating.ToString(), _runtime.ToString(), _language.ToString(), _imdbRating.ToString(), _website.ToString(), _imdbID.ToString());
                ViewBag.theTitle = _title;
                ViewBag.thePoster = _poster;
                ViewBag.thePlot = _plot;
                ViewBag.theGenre = _genre;
                ViewBag.theYear = _year;
                MethodThatAddsMovieOjbectToDatabase(m);
            //}
            //catch (Exception e)
            //{
            //    ViewBag.SingleRateErrorMessage = "oops! looks like that movie title doesn't exist.";
            //    //return view("customErrorPage");
            //}


            return View("SingleRate");
        }

        public ActionResult SingleRate()
        {
            return View();
        }

        public void MethodThatAddsMovieOjbectToDatabase(Medium m)
        {
            //need to add code
            if (ModelState.IsValid)
            {
                db.Media.Add(m);
                db.SaveChanges();
                RedirectToAction("index");
            }
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