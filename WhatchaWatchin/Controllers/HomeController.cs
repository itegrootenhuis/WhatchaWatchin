﻿using Microsoft.AspNet.Identity;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace WhatchaWatchin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult WelcomeExplanation()
        {
            return View();
        }

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

        public ActionResult MyRatings()
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WhatchaWatchinConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.sp_GetUsersReviewedMedia", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter inputParameter = new SqlParameter("@UserID", User.Identity.GetUserId());
            cmd.Parameters.Add(inputParameter);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataTable();
            con.Open();
            da.Fill(ds);
            con.Close();

            List<ReturnedCurrentUserRating> returnedRatings = new List<ReturnedCurrentUserRating>();

            foreach (DataRow row in ds.Rows)
            {
                ReturnedCurrentUserRating returnedRating = new ReturnedCurrentUserRating
                {
                    Title = row.ItemArray[0].ToString().Trim(),
                    Rating = double.Parse(row.ItemArray[1].ToString())
                };
                returnedRatings.Add(returnedRating);
            }
            return View("MyRatings", returnedRatings);
        }

        public ActionResult Rate(string genreChoice)
        {
            Session["genreChoice"] = genreChoice;

            if (genreChoice == "Comedy")
            {
                List<Movie> firstMovieList = ComedyMovieList();
                return View(firstMovieList);
            }

            else if (genreChoice == "Action")
            {
                List<Movie> firstMovieList = ActionMovieList();
                return View(firstMovieList);
            }
            else if (genreChoice == "Thriller")
            {
                List<Movie> firstMovieList = ThrillerMovieList();
                return View(firstMovieList);
            }
            else if (genreChoice == "Family")
            {
                List<Movie> firstMovieList = FamilyMovieList();
                return View(firstMovieList);
            }

            else if (genreChoice == "Horror")
            {
                List<Movie> firstMovieList = HorrorMovieList();
                return View(firstMovieList);
            }
            else
            {
                List<Movie> firstMovieList = DramaMovieList();
                return View(firstMovieList);
            }
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
                CreateMovieByTitle("the wedding singer"),
                CreateMovieByTitle("the house"),
                CreateMovieByTitle("father figures"),
                CreateMovieByTitle("the pink panther"),
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
                CreateMovieByTitle("Creed")
            };
            return drama;
        }

        public static List<Movie> ActionMovieList()
        {
            List<Movie> action = new List<Movie>
            {
                CreateMovieByTitle("Avengers"),
                CreateMovieByTitle("The_Equalizer"),
                CreateMovieByTitle("Taken"),
                CreateMovieByTitle("Expendables_3"),
                CreateMovieByTitle("Deadpool"),
                CreateMovieByTitle("The Dark Knight")
            };
            return action;
        }

        public static List<Movie> HorrorMovieList()
        {

            List<Movie> horror = new List<Movie>
            {
                CreateMovieByTitle("krampus"),
                CreateMovieByTitle("lights out"),
                CreateMovieByTitle("it"),
                CreateMovieByTitle("jigsaw"),
                CreateMovieByTitle("leatherface"),
                CreateMovieByTitle("halloween")
            };
            return horror;
        }

        public static List<Movie> FamilyMovieList()
        {
            List<Movie> family = new List<Movie>
            {
                CreateMovieByTitle("The Boss Baby"),
                CreateMovieByTitle("Despicable Me"),
                CreateMovieByTitle("Finding Dory"),
                CreateMovieByTitle("Toy Story"),
                CreateMovieByTitle("Minions"),
                CreateMovieByTitle("Mulan")
            };
            return family;
        }

        public static List<Movie> ThrillerMovieList()
        {
            List<Movie> thriller = new List<Movie>
            {
                CreateMovieByTitle("Get Out"),
                CreateMovieByTitle("Transcendence"),
                CreateMovieByTitle("Split"),
                CreateMovieByTitle("Don't Breathe"),
                CreateMovieByTitle("Gravity"),
                CreateMovieByTitle("Disturbia")
            };
            return thriller;
        }
    }
}