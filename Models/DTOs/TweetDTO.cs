namespace TwitterClone_API.Models.DTOs
{
    public class TweetDTO
    {
        public string UserName { get; set; }
        public string TweetText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
