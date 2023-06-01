using TrueSight.Common;

namespace QuizExamOnline.Entities.AppUsers
{
    public class AppUserDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
