using Microsoft.AspNet.Identity;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using WhatchaWatchin.Models;
using WhatchaWatchin.Controllers;

namespace WhatchaWatchin.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        //dbobject
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MovieSuggestions()
        {
            //this is code for mvp
            if (Session["genreChoice"].ToString() == "Comedy")
            {
                Movie movieDisplay = CreateMovieByTitle("Other Guys");
                ViewBag.movieDisplay = movieDisplay;
                return View();
            }
            else if (Session["genreChoice"].ToString() == "Drama")
            {
                Movie movieDisplay = CreateMovieByTitle("The Godfather");
                ViewBag.movieDisplay = movieDisplay;
                return View(); 
            }
            return View();
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

        public ActionResult PickGenre()
        {
            ViewBag.Message = "Pick a Genre";

            return View();
        }

        public ActionResult Rate(string genreChoice)
        {
            Session["genreChoice"] = genreChoice;

            if (genreChoice == "Comedy")
            {
                List<Movie> firstMovieList = ComedyMovieList();
                return View(firstMovieList);

            }
            else
            {
                List<Movie> firstMovieList = DramaMovieList();
                return View(firstMovieList);

            }
        }

        //public ActionResult SendData(string[] userRatings)
        //{
        //    string[] testArr = userRatings;
        //    List<int> baseSurveyMovieIDs;
        //    List<ReviewedMedia> reviewedMediaList = new List<ReviewedMedia>();
        //    if (Session["genreChoice"].ToString() == "Comedy")
        //    {
        //        baseSurveyMovieIDs = new List<int>() { 1, 2, 3, 4, 5 };
        //    }
        //    else
        //    {
        //        baseSurveyMovieIDs = new List<int>() { 6,7,8,9,10};
        //    }
        //    for (int i = 0; i < testArr.Length; i++)
        //    {
        //        ReviewedMedia reviewedMovie = new ReviewedMedia
        //        {
        //            MovieID = baseSurveyMovieIDs[i],
        //            UserID = User.Identity.GetUserId(),
        //            UserRating = int.Parse(userRatings[i])
        //        };
        //        reviewedMediaList.Add(reviewedMovie);
        //    }
        //    //ReviewedMediasController.StoreInDatabase(reviewedMediaList);
        //    return RedirectToAction("StoreInDatabase", "ReviewedMediasController", new {List<ReviewedMedia> = reviewedMediaList});
        //}
        public ActionResult SearchMovie(string title)
        {
            //HttpWebRequest request = WebRequest.CreateHttp("http://www.omdbapi.com/?apikey=d0069624&t=titanic");
            HttpWebRequest request = WebRequest.CreateHttp("http://www.omdbapi.com/?apikey=d0069624&t=" + title);
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

            Movie m = new Movie(_title.ToString(), _plot.ToString(), _poster.ToString(), _genre.ToString(), _type.ToString(), int.Parse(_year.ToString()), _mpaaRating.ToString(), _runtime.ToString(), _language.ToString(), _imdbRating.ToString(), _website.ToString(), _imdbID.ToString());

            return View("QuizResult");
        }
        public static Movie CreateMovieByTitle(string title)
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://www.omdbapi.com/?apikey=d0069624&t=" + title);
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

            Movie m = new Movie(_title.ToString(), _plot.ToString(), _poster.ToString(), _genre.ToString(), _type.ToString(), int.Parse(_year.ToString()), _mpaaRating.ToString(), _runtime.ToString(), _language.ToString(), _imdbRating.ToString(), _website.ToString(), _imdbID.ToString());
            return m;
        }
        public static List<Movie> ComedyMovieList()
        {
            List<Movie> comedies = new List<Movie>
            {
                CreateMovieByTitle("downsizing"),
                CreateMovieByTitle("big_sick"),
                CreateMovieByTitle("Lego_movie"),
                CreateMovieByTitle("the house"),
                CreateMovieByTitle("father figures"),
            };


            return comedies;
        }
        public static List<Movie> DramaMovieList()
        {
            List<Movie> drama = new List<Movie>
            {
                CreateMovieByTitle("dunkirk"),
                CreateMovieByTitle("the_founder"),
                CreateMovieByTitle("flatliners"),
                CreateMovieByTitle("hidden_figures"),
                CreateMovieByTitle("phantom_thread"),
            };


            return drama;
        }
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

            try
            {
                Movie m = new Movie(_title.ToString(), _plot.ToString(), _poster.ToString(), _genre.ToString(), _type.ToString(), int.Parse(_year.ToString()), _mpaaRating.ToString(), _runtime.ToString(), _language.ToString(), _imdbRating.ToString(), _website.ToString(), _imdbID.ToString());
                ViewBag.theTitle = _title;
                ViewBag.thePoster = _poster;
                ViewBag.thePlot = _plot;
                ViewBag.theGenre = _genre;
                ViewBag.theYear = _year;
                //MethodThatAddsMovieOjbectToDataBase(m);
            }
            catch(Exception e)
            {
                ViewBag.SingleRateErrorMessage = "oops! looks like that movie title doesn't exist.";
            }
            

            return View("SingleRate");
        }

        public ActionResult SingleRate()
        {
            return View();
        }
        public void MethodThatAddsMovieOjbectToDatabase(Movie m)
        {
            //need to add code
        }
    }
}