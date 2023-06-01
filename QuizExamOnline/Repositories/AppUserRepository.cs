using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizExamOnline.Common;
using QuizExamOnline.Entities.AppUsers;
using QuizExamOnline.Models;
using System.Security.Cryptography;

namespace QuizExamOnline.Repositories
{
    public interface IAppUserRepository
    {
        Task<AppUserDto> Create(CreateAppUserDto appUserDto);
        Task<AppUserDto> Get();
        Task UpdateToken(long id, string refreshToken);
        Task<AppUserDto> GetUserByRefreshToken(string refreshToken);
        Task<AppUserDto> Login(UserLoginDto login);
        Task<bool> CheckEmail(string email); 
        Task<AppUserDto> GetById(long id);
    }

    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ICurrentContext _currentContext;
        public AppUserRepository(DataContext dataContext, IMapper mapper, ICurrentContext currentContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _currentContext = currentContext;
        }
        public async Task<AppUserDto> Create(CreateAppUserDto appUserDto)
        {
            var appUser = _mapper.Map< CreateAppUserDto, AppUser >(appUserDto);
            appUser.CreatedAt = DateTime.Now;
            appUser.UpdatedAt = DateTime.Now;
            _dataContext.AppUsers.Add(appUser);
            await _dataContext.SaveChangesAsync();
            var addedAppUser = await _dataContext.AppUsers.FindAsync(appUser.Id);
            AppUserRole role = new AppUserRole
            {
                RoleId = 2,
                AppUserId = addedAppUser.Id
            };
            _dataContext.AppUserRoles.Add(role);
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<AppUser, AppUserDto>(addedAppUser);
        }

        public async Task UpdateToken(long id, string refreshToken)
        {
            var appuser = await _dataContext.AppUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            appuser.RefreshToken = refreshToken;
            await _dataContext.SaveChangesAsync();  
        }

        public async Task<bool> CheckEmail(string email)
        {
            var appuser = await _dataContext.AppUsers
                                    .Where(x => x.Email == email)
                                    .FirstOrDefaultAsync();
            if (appuser == null)
            {
                return false;
            }
            return true;
        }

        public async Task<AppUserDto> Get()
        {
            var appuser = await _dataContext.AppUsers.Where(x => x.Id == _currentContext.UserId).FirstOrDefaultAsync();
            if (appuser == null) return null;
            return _mapper.Map<AppUser, AppUserDto>(appuser);
        }

        public async Task<AppUserDto> GetById(long id)
        {
            var appuser = await _dataContext.AppUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (appuser == null) return null;
            return _mapper.Map<AppUser, AppUserDto>(appuser);
        }

        public async Task<AppUserDto> GetUserByRefreshToken(string refreshToken)
        {
            var appuser = await _dataContext.AppUsers
                                    .Where(x => x.RefreshToken == refreshToken)
                                    .FirstOrDefaultAsync();
            if (appuser == null) return null;
            return _mapper.Map<AppUser, AppUserDto>(appuser);
        }

        public async Task<AppUserDto> Login(UserLoginDto login)
        {

            var appuser = await _dataContext.AppUsers
                            .Where(x => x.Email == login.Email)
                            .FirstOrDefaultAsync();
            if (appuser != null)
            {
                if (VerifyPassword(appuser.Password, login.Password))
                {
                    return _mapper.Map<AppUser, AppUserDto>(appuser);
                }
            }
            return null;
        }

        private bool VerifyPassword(string oldPassword, string newPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(oldPassword);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(newPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
