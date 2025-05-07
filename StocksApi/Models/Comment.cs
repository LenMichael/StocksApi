namespace StocksApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? StockId { get; set; }
        // Navigation Property
        public Stock? Stock { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
