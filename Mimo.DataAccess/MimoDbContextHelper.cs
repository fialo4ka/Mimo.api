using Mimo.Common.DBEntities.Achievements;
using Mimo.Common.DBEntities.Courses;
using Mimo.Common.DBEntities.Results;
using Mimo.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.DataAccess
{
    public static class MimoDbContextHelper
    {
        public static void Seed()
        {
            var ctx = new MimoDbContext();
            if (!ctx.Database.EnsureCreated())
            {
                return;
            }
            try

            {
                //Achievements
                for (int i = 0; i < AchievementList.Count; i++)
                {
                    ctx.Add(AchievementList[i]);
                }
                var result = ctx.SaveChanges();
                if (result <= 0)
                {
                    throw new Exception();
                }

                //Courses
                for (int CourseIndex = 0; CourseIndex < CourseName.Count; CourseIndex++)
                {
                    var Course = new Course { Name = CourseName[CourseIndex] };
                    ctx.Add(Course);
                    result = ctx.SaveChanges();
                    if (result <= 0)
                    {
                        throw new Exception();
                    }
                    for (int chapterIndex = 0; chapterIndex < ChapterName.Count; chapterIndex++)
                    {
                        var chapter = new Chapter
                        {
                            CourseId = Course.Id,
                            Name = ChapterName[chapterIndex],
                            Order = chapterIndex + 1
                        };
                        ctx.Add(chapter);
                        result = ctx.SaveChanges();
                        if (result <= 0)
                        {
                            throw new Exception();
                        }
                        for (int lessonIndex = 0; lessonIndex < ChapterName.Count; lessonIndex++)
                        {
                            var lesson = new Lesson
                            {
                                ChapterId = chapter.Id,
                                Text = RandomString(),
                                Order = lessonIndex + 1,
                            };
                            ctx.Add(lesson);
                        }
                    }
                }
                result = ctx.SaveChanges();
                if (result <= 0)
                {
                    throw new Exception();
                }

                //Results
                for (int i = 0; i < UserGuid.Count; i++)
                {
                    ctx.Add(new User { UserGuid = UserGuid[i] });
                }
                result = ctx.SaveChanges();
                if (result <= 0)
                {
                    throw new Exception();
                }

                for (int userId = 0; userId < UserGuid.Count; userId++)
                {
                    //random.Next(1, 60) take random lessons quantity
                    for (int countLessons = 0; countLessons < random.Next(1, 60); countLessons++)
                    {
                        var userLesson = GetRandomUserLesson(userId);
                        ctx.Add(userLesson);

                        //take random number of one lesson starting
                        for (int countSameLessons = 0; countSameLessons < random.Next(1, 50); countSameLessons++)
                        {
                            ctx.Add(GetRandomUserLesson(userId, userLesson.LessonId));
                        }
                    }
                }
                result = ctx.SaveChanges();
                if (result <= 0)
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static UserLesson GetRandomUserLesson(int userId, int? lessonId = null)
        {
            var dayDif = random.Next(1, 50);
            return new UserLesson
            {
                UserId = userId + 1,
                //take random lesson number
                LessonId = lessonId ?? random.Next(1, 70),
                StartDate = DateTime.Now.AddDays(-dayDif),
                EndDate = DateTime.Now.AddDays(-dayDif + 3),
                //take random IsCompleted
                IsCompleted = dayDif % 2 == 0
            };
        }

        //test data for db
        //Achievements

        private static List<Achievement> AchievementList => new List<Achievement>
        {
            new Achievement{Name = "Complete 5 lessons", AchievementType = AchievementType.GotLessonsQuantity, AchievementObjectIntData = 5 },
            new Achievement{Name = "Complete 25 lessons", AchievementType = AchievementType.GotLessonsQuantity, AchievementObjectIntData = 25  },
            new Achievement{Name = "Complete 50 lessons", AchievementType = AchievementType.GotLessonsQuantity, AchievementObjectIntData = 50 },
            new Achievement{Name = "Complete 1 chapter", AchievementType = AchievementType.GotChapterQuantity, AchievementObjectIntData = 1 },
            new Achievement{Name = "Complete 5 chapters", AchievementType = AchievementType.GotChapterQuantity, AchievementObjectIntData = 5 },
            new Achievement{Name = "Complete the Swift course", AchievementType = AchievementType.GotCourse, AchievementObjectId = 1},
            new Achievement{Name = "Complete the Javascript course", AchievementType = AchievementType.GotCourse, AchievementObjectId = 2},
            new Achievement{Name = "Complete the C# course", AchievementType = AchievementType.GotCourse, AchievementObjectId = 3},
        };

        //Courses
        private static List<string> CourseName => new List<string> { "Swift", "Javascript", "C#" };
        private static List<string> ChapterName => new List<string> { "basic knowledge", "limited experience", "practical application", "applied theory", "recognized authority" };

        //Results        
        private static List<string> UserGuid => new List<string> { "user1", "user2", "user3" };

        private static Random random = new Random();

        //random string generation for Lesson Text
        public static string RandomString()
        {
            const string chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ,.";
            return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
