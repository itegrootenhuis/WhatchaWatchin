namespace Models
{
    public class ReturnedCurrentUserRating
    {
        public string Title { get; set; }
        public double Rating { get; set; }

        public ReturnedCurrentUserRating()
        {

        }
        public ReturnedCurrentUserRating(string title, double rating)
        {
            Title = title;
            Rating = rating;
        }
    }

}