using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace WhatchaWatchin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
            List<Movie> comedies = new List<Movie>();
            comedies.Add(CreateMovieByTitle("father figures"));
            comedies.Add(CreateMovieByTitle("downsizing"));
            comedies.Add(CreateMovieByTitle("big_sick"));
            comedies.Add(CreateMovieByTitle("Lego_movie"));
            comedies.Add(CreateMovieByTitle("the house"));

            return comedies;
        }
        public static List<Movie> DramaMovieList()
        {
            List<Movie> drama = new List<Movie>();
            drama.Add(CreateMovieByTitle("dunkirk"));
            drama.Add(CreateMovieByTitle("the_founder"));
            drama.Add(CreateMovieByTitle("flatliners"));
            drama.Add(CreateMovieByTitle("hidden_figures"));
            drama.Add(CreateMovieByTitle("phantom_thread"));
            drama.Add(CreateMovieByTitle("wonder"));


            return drama;
        }
    }
}