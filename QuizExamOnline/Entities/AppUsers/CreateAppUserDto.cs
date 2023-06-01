﻿namespace QuizExamOnline.Entities.AppUsers
{
    public class CreateAppUserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public byte[]? Image { get; set; }
    }
}
