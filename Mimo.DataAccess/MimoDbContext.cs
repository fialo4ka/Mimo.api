using Microsoft.EntityFrameworkCore;
using Mimo.Common.DBEntities.Achievements;
using Mimo.Common.DBEntities.Courses;
using Mimo.Common.DBEntities.Results;

namespace Mimo.DataAccess
{
    public class MimoDbContext : DbContext
    {


        //Achievements
        public virtual DbSet<Achievement> Achievements { get; set; }


        //Courses
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }


        //Results
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserLesson> UserLessons { get; set; }
        public virtual DbSet<UserAchievement> UserAchievements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=mimo.db;").EnableSensitiveDataLogging(); ;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Achievements
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired();

            });


            //Courses
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired();

            });
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Chapter__Course");
            });
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lesson__Chapter");
            });

            //Results
            modelBuilder.Entity<User>(entity =>
            {

            });
            modelBuilder.Entity<UserLesson>(entity =>
            {
                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.UserLesson)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserLesson__Lesson"); 
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLesson)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserLesson__User");
            });
            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasOne(d => d.Achievement)
                    .WithMany(p => p.UserAchievements)
                    .HasForeignKey(d => d.AchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserAchievement__Lesson"); 
                
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAchievements)
                    .HasForeignKey(d => d.AchievementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserAchievement__User");
            });
        }
    }
}