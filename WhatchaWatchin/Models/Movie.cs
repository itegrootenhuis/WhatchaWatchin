namespace Models
{
    public class Movie
    {
        //private JToken _title;
        //private JToken _plot;
        //private JToken _poster;
        //private JToken _genre;
        //private JToken _type;
        //private JToken _year;
        //private JToken _mpaaRating;
        //private JToken _runtime;
        //private JToken _language;
        //private JToken _imdbRating;
        //private JToken _website;
        //private JToken _imdbID;

        public string Title { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string Genre { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string MpaaRating { get; set; }
        public string Runtime { get; set; }
        public string Language { get; set; }
        public string ImdbRating { get; set; }
        public string Website { get; set; }
        public string ImdbID { get; set; }

        public Movie()
        {

        }
        public Movie(string title, string plot, string poster, string genre, string type, int year, string mpaaRating, string runtime, string language, string imdbRating, string website, string imdbID)
        {
            Title = title;
            Plot = plot;
            Poster = poster;
            Genre = genre;
            Type = type;
            Year = year;
            MpaaRating = mpaaRating;
            Runtime = runtime;
            Language = language;
            ImdbRating = imdbRating;
            Website = website;
            ImdbID = imdbID;
        }
    }
    public class ReviewedMovie : Movie
    {
        public int UserID { get; set; }
        public int Rating { get; set; }

        public ReviewedMovie(Movie movie, int userID, int rating)
        {
            UserID = userID;
            Rating = rating;
            Title = movie.Title;
            Plot = movie.Plot;
            Poster = movie.Poster;
            Genre = movie.Genre;
            Type = movie.Type;
            Year = movie.Year;
            MpaaRating = movie.MpaaRating;
            Runtime = movie.Runtime;
            Language = movie.Language;
            ImdbRating = movie.ImdbRating;
            Website = movie.Website;
            ImdbID = movie.ImdbID;
        }
    }
}