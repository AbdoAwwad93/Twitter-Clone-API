namespace TwitterClone_API.Models.DTOs
{
    public class CommentDTO
    {
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
