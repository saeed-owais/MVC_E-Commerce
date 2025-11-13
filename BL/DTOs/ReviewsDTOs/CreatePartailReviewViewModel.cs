namespace BLL.DTOs.ReviewsDTOs
{
    public class CreatePartailReviewViewModel
    {
        public string ProductId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1–5 stars
    }
}
