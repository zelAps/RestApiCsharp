namespace BookApi.Models
{
    public class Book
    {
        public int Id { get; set; } // unique identifier
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
