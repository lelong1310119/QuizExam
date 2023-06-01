using Microsoft.EntityFrameworkCore;

namespace QuizExamOnline.Models
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AnswerQuestion> AnswerQuestions { get; set; }  
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamHistory> ExamHistories { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Level> Levels { get; set; }    
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }  
        public virtual DbSet<Role> Roles { get; set; }  
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-TC1PJ34D\\LONG;Initial Catalog=QuizExamOnline;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUser");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(x => x.Password)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(x => x.DisplayName)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(x => x.RefreshToken).HasMaxLength(4000);
                entity.Property(x => x.CreatedAt).HasColumnType("datetime");
                entity.Property(x => x.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("AppRole", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<AppUserRole>(entity =>
            {
                entity.ToTable("AppUserRole");
                entity.HasKey(x => new { x.RoleId, x.AppUserId });

                entity.HasOne(x => x.AppUser)
                    .WithMany(y => y.AppUserRoles)
                    .HasForeignKey(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserRole_AppUser");

                entity.HasOne(x => x.Role)
                    .WithMany(y => y.AppUserRoles)
                    .HasForeignKey(x => x.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserRole_Role");
            });
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("AppSubject", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("AppStatus", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.ToTable("AppQuestionType", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<QuestionGroup>(entity =>
            {
                entity.ToTable("AppQuestionGroup", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Level>(entity =>
            {
                entity.ToTable("AppLevel", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("AppGrade", "ENUM");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedNever();

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("AppQuestion");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
                entity.Property(x => x.Content).IsRequired();

                entity.Property(x => x.CreatedAt).HasColumnType("datetime");
                entity.Property(x => x.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(x => x.Subject)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Subject");

                entity.HasOne(x => x.Grade)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Grade");

                entity.HasOne(x => x.Level)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Level");

                entity.HasOne(x => x.QuestionGroup)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.QuestionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_QuestionGroup");

                entity.HasOne(x => x.Status)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_Status");

                entity.HasOne(x => x.QuestionType)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_QuestionType");

                entity.HasOne(x => x.AppUser)
                    .WithMany(y => y.Questions)
                    .HasForeignKey(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_AppUser");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("AppExam");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Code).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(400);
                entity.Property(x => x.Time).IsRequired();

                entity.Property(x => x.CreatedAt).HasColumnType("datetime");
                entity.Property(x => x.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(x => x.Subject)
                    .WithMany(y => y.Exams)
                    .HasForeignKey(x => x.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exam_Subject");

                entity.HasOne(x => x.Grade)
                    .WithMany(y => y.Exams)
                    .HasForeignKey(x => x.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exam_Grade");

                entity.HasOne(x => x.Level)
                    .WithMany(y => y.Exams)
                    .HasForeignKey(x => x.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exam_Level");


                entity.HasOne(x => x.Status)
                    .WithMany(y => y.Exams)
                    .HasForeignKey(x => x.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exam_Status");


                entity.HasOne(x => x.AppUser)
                    .WithMany(y => y.Exams)
                    .HasForeignKey(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exam_AppUser");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.ToTable("AppExamQuestion");
                entity.HasKey(x => new {x.ExamId, x.QuestionId});

                entity.Property(x => x.Mark).IsRequired();
                entity.HasOne(x => x.Exam)
                    .WithMany(y => y.ExamQuestions)
                    .HasForeignKey(x => x.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestion_Exam");

                entity.HasOne(x => x.Question)
                    .WithMany(y => y.ExamQuestions)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamQuestion_Question");
            });

            modelBuilder.Entity<ExamHistory>(entity =>
            {
                entity.ToTable("AppExamHistory");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CompleteTime).IsRequired();
                entity.Property(x => x.ToTalScore).IsRequired();
                entity.Property(x => x.CreateAt).HasColumnType("datetime");
                entity.Property(x => x.QuestionRight).IsRequired(); 

                entity.HasOne(x => x.Exam)
                    .WithMany(y => y.ExamHistories)
                    .HasForeignKey(x => x.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamHistory_Exam");

                entity.HasOne(x => x.AppUser)
                    .WithMany(y => y.ExamHistories)
                    .HasForeignKey(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamHistory_AppUser");
            });

            modelBuilder.Entity<AnswerQuestion>(entity =>
            {
                entity.ToTable("AppAnswerQuestion");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).ValueGeneratedOnAdd();

                entity.Property(x => x.Content).IsRequired();
                entity.Property(x => x.IsRight).IsRequired();

                entity.HasOne(x => x.Question)
                    .WithMany(y => y.AnswerQuestions)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnswerQuestion_Question");
            });

            OnModelCreatingPartial(modelBuilder);

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
