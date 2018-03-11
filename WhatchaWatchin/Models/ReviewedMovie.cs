

namespace Models
{
    public class ReviewedMovie
    {
        public int MovieID { get; set; }
        public string UserID { get; set; }
        public int UserRating { get; set; }

        public ReviewedMovie()
        {

        }
        public ReviewedMovie(int movieID, string userID, int userRating)
        {
            MovieID = movieID;
            UserID = userID;
            UserRating = userRating;
        }
    }
}