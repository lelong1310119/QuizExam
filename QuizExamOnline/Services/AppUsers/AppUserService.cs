using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using Nest;
using NuGet.Protocol;
using QuizExamOnline.Common;
using QuizExamOnline.Entities.AppUsers;
using QuizExamOnline.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace QuizExamOnline.Services.AppUsers
{
    public interface IAppUserService
    {
        Task<AppUserDto> CreateUser(CreateAppUserDto createAppUserDto);
        Task<AppUserDto> Get();
        Task<AppUserDto> RefreshToken(string refreshToken);
        Task<AppUserDto> Login(UserLoginDto userlogin);
        bool Validate(string refreshToken);
    }
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IConfiguration _configuration;
        private readonly ICurrentContext _currentContext;

        public AppUserService(IAppUserRepository appUserRepository, IConfiguration configuration, ICurrentContext currentContext) { 
            _appUserRepository = appUserRepository;
            _configuration = configuration;
            _currentContext = currentContext;
        }

        public async Task<AppUserDto> CreateUser(CreateAppUserDto createAppUserDto)
        {
            if (await _appUserRepository.CheckEmail(createAppUserDto.Email)) throw new AppUserExeption("Email already exists");
            if (!ValidateEmail(createAppUserDto.Email)) throw new AppUserExeption("Invalid Email");
            if (!ValidatePassword(createAppUserDto.Password)) throw new AppUserExeption("Invalid Password");
            if (!ValidateDisplayName(createAppUserDto.DisplayName)) throw new AppUserExeption("Invalid Name");

            createAppUserDto.Password = HashPassword(createAppUserDto.Password);
            var appuser = await _appUserRepository.Create(createAppUserDto);
            appuser.Token = CreateToken(appuser);
            appuser.RefreshToken = CreateRefreshToken();
            await _appUserRepository.UpdateToken(appuser.Id, appuser.RefreshToken);
            _currentContext.UserId = appuser.Id;
            return appuser;
        }

        public async Task<AppUserDto> Login(UserLoginDto userlogin)
        {
            if (!await _appUserRepository.CheckEmail(userlogin.Email)) throw new AppUserExeption("Email does noet exist");
            var appuser = await _appUserRepository.Login(userlogin);
            if (appuser == null) throw new AppUserExeption("Incorrect Password");

            appuser.Token = CreateToken(appuser);
            appuser.RefreshToken = CreateRefreshToken();
            await _appUserRepository.UpdateToken(appuser.Id, appuser.RefreshToken);
            _currentContext.UserId = appuser.Id;
            return appuser;
        }

        public async Task<AppUserDto> RefreshToken(string refreshToken)
        {
            if (!Validate(refreshToken)) throw new AppUserExeption("Invalid RefreshToken");
            var appuser = await _appUserRepository.GetUserByRefreshToken(refreshToken);
            if (appuser == null) throw new AppUserExeption("Incorrect RefreshToken");

            appuser.Token = CreateToken(appuser);
            _currentContext.UserId = appuser.Id;
            return appuser;
        }
        
        public async Task<AppUserDto> Get()
        {
            var result = await _appUserRepository.Get();
            return result;
        }

        public string CreateToken(AppUserDto user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.DisplayName)
            };
            var accessTokenSecret = _configuration["Authentication:AccessTokenSecret"];
            var issuer = _configuration["Authentication:Issuer"];
            var audience = _configuration["Authentication:Audience"];
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(double.TryParse(_configuration["Authentication:AccessTokenExpirationMinutes"], out double time) ? time : 0);
            string value = GenerateToken(
                accessTokenSecret,
                issuer,
                audience,
                expirationTime,
                claims);
            return value;
        }

        public string CreateRefreshToken()
        {
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(double.TryParse(_configuration["Authentication:RefreshTokenExpirationMinutes"], out double time) ? time : 0);
            
            var refreshTokenSecret = _configuration["Authentication:RefreshTokenSecret"];
            var issuer = _configuration["Authentication:Issuer"];
            var audience = _configuration["Authentication:Audience"];
            return GenerateToken(
                refreshTokenSecret,
                issuer,
                audience,
                expirationTime);
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        //private bool VerifyPassword(string oldPassword, string newPassword)
        //{
        //    byte[] hashBytes = Convert.FromBase64String(oldPassword);
        //    /* Get the salt */
        //    byte[] salt = new byte[16];
        //    Array.Copy(hashBytes, 0, salt, 0, 16);
        //    /* Compute the hash on the password the user entered */
        //    var pbkdf2 = new Rfc2898DeriveBytes(newPassword, salt, 10000);
        //    byte[] hash = pbkdf2.GetBytes(20);
        //    /* Compare the results */
        //    for (int i = 0; i < 20; i++)
        //        if (hashBytes[i + 16] != hash[i])
        //            return false;
        //    return true;
        //}

        public string GenerateToken(string secretKey, string issuer, string audience, DateTime utcExpirationTime,
            IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                utcExpirationTime,
                credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool Validate(string refreshToken)
        {
            var refreshTokenSecret = _configuration["Authentication:RefreshTokenSecret"];
            var issuer = _configuration["Authentication:Issuer"];
            var audience = _configuration["Authentication:Audience"];
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenSecret)),
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ValidateDisplayName(string name)
        {
            if (name == null || name.Trim() == "" || name.Length > 100) return false;
            return true;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Contains(" ") || password.Length < 8)
            {
                return false;
            }
            //var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            //return regex.IsMatch(password);
            return true;
        }

        private bool ValidateEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
