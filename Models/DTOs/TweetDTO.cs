namespace TwitterClone_API.Models.DTOs
{
    public class TweetDTO
    {
        public string TweetText { get; set; }
        public DateOnly CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
