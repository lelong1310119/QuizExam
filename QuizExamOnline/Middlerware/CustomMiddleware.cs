using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace QuizExamOnline.Middlerware
{
    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public CustomMiddleWare(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            //if (token != null)
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            //    try
            //    {
            //        tokenHandler.ValidateToken(token, new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(key),
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ClockSkew = TimeSpan.Zero
            //        }, out var validatedToken);

            //        var jwtToken = (JwtSecurityToken)validatedToken;
            //        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            //        // Lưu thông tin user vào context để sử dụng trong các controller.
            //        context.Items["User"] = userId;
            //    }
            //    catch (Exception)
            //    {
            //        // Xử lý lỗi xác thực.
            //        context.Response.StatusCode = 401;
            //        await context.Response.WriteAsync("Invalid token.");
            //        return;
            //    }
            //}

            //await _next(context);
        }
    }
}
