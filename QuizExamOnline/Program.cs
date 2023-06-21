using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizExamOnline.Common;
using QuizExamOnline.Migrations;
using QuizExamOnline.Models;
using QuizExamOnline.Repositories;
using QuizExamOnline.Services;
using QuizExamOnline.Services.AppUsers;
using QuizExamOnline.Services.ExamHistories;
using QuizExamOnline.Services.Exams;
using QuizExamOnline.Services.Questions;
using System.Drawing;
using System.Text;
using static StackExchange.Redis.Role;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://192.168.96.93:5094",
            ValidAudience = "https://192.168.96.93:5094",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4y7XS2AHicSOs2uUJCxwlHWqTJNExW3UDUjMeXi96uLEso1YV4RazqQubpFBdx0zZGtdxBelKURhh0WXxPR0mEJQHk_0U9HeYtqcMManhoP3X2Ge8jgxh6k4C_Gd4UPTc6lkx0Ca5eRE16ciFQ6wmYDnaXC8NbngGqartHccAxE"))
        };
    });
builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer("Data Source=DESKTOP-PL7Q9Q6; Initial Catalog=TestQuizExamOnline;Integrated Security=True;TrustServerCertificate=True;"));
builder.Services.AddScoped<DbSeeder>();
builder.Services.AddScoped<IGeneralRepository, GeneralRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerQuestionRepository, AnswerQuestionRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IHistoryExamRepository, HistoryExamRepository>();

builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IGeneralService, GeneralService>();
builder.Services.AddScoped<IExamHistoryService, ExamHistoryService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExamService, ExamService>();

builder.Services.AddSingleton<ICurrentContext, CurrentContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseAuthentication();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetService<DbSeeder>();
    await DbSeeder.Migrate(services);
}
app.UseAuthorization();

app.MapControllers();

app.Run();
