using Microsoft.EntityFrameworkCore;
using QuizExamOnline.Enums;
using System.Security.Cryptography;
using System.Xml;

namespace QuizExamOnline.Models
{
    public class DbSeeder
    {
        public static async Task Migrate(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
            await StatusInitialize(serviceProvider);
            await LevelInitialize(serviceProvider);
            await GradeInitialize(serviceProvider);
            await SubjectInitialize(serviceProvider);
            await QuestionTypeInitialize(serviceProvider);
            await QuestionGroupInitialize(serviceProvider);
            await RoleInitialize(serviceProvider);
            await UserInitialize(serviceProvider);
        }

        public static async Task StatusInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var statusSet = context.Statuses;
                if (await statusSet.AnyAsync()) return;

                var seed = StatusEnum.StatusEnumList;
                foreach (var item in seed)
                {
                    await statusSet.AddAsync(new Status
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task LevelInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var levelSet = context.Levels;
                if (await levelSet.AnyAsync()) return;

                var seed = LevelEnum.LevelEnumList;
                foreach (var item in seed)
                {
                    await levelSet.AddAsync(new Level
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task GradeInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var gradeSet = context.Grades;
                if (await gradeSet.AnyAsync()) return;

                var seed = GradeEnum.GradeEnumList;
                foreach (var item in seed)
                {
                    await gradeSet.AddAsync(new Grade
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task SubjectInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var subjectSet = context.Subjects;
                if (await subjectSet.AnyAsync()) return;

                var seed = SubjectEnum.SubjectEnumList;
                foreach (var item in seed)
                {
                    await subjectSet.AddAsync(new Subject
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task QuestionGroupInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var questionGroupSet = context.QuestionGroups;
                if (await questionGroupSet.AnyAsync()) return;

                var seed = QuestionGroupEnum.QuestionGroupEnumList;
                foreach (var item in seed)
                {
                    await questionGroupSet.AddAsync(new QuestionGroup
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task QuestionTypeInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var questionTypeSet = context.QuestionTypes;
                if (await questionTypeSet.AnyAsync()) return;

                var seed = QuestionTypeEnum.QuestionTypeEnumList;
                foreach (var item in seed)
                {
                    await questionTypeSet.AddAsync(new QuestionType
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }
        public static async Task RoleInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var roleSet = context.Roles;
                if (await roleSet.AnyAsync()) return;

                var seed = RoleEnum.RoleEnumList;
                foreach (var item in seed)
                {
                    await roleSet.AddAsync(new Role
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,
                    });
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task UserInitialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<DataContext>();
            if (context != null)
            {
                var userSet = context.AppUsers;
                if (await userSet.AnyAsync()) return;

                var user = new AppUser
                {
                    Email = "admin@gmail.com",
                    Password = HashPassword("12345678"),
                    DisplayName = "Admin",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Image = null,
                    RefreshToken = null,
                };
                await userSet.AddAsync(user);
                await context.SaveChangesAsync();
                await context.AppUserRoles.AddAsync(new AppUserRole
                {
                    AppUserId = user.Id,
                    RoleId = 1
                });
                await context.SaveChangesAsync();
            }
        }

        private static string HashPassword(string password)
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
    }
}
