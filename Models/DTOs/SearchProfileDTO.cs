namespace TwitterClone_API.Models.DTOs
{
    public class SearchProfileDTO
    {
        public string UserName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public List<TweetDTO> Tweets { get; set; }
    }
}
