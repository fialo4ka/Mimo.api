using Mimo.Common.DataAccess.IServices;
using Mimo.Common.DBEntities.Courses;
using Mimo.Common.DBEntities.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mimo.Managers.Managers.Tests
{
    public class ResultsManagerTests
    {
        private readonly Mock<IResultsService> _resultsService;
        private readonly Mock<IAchievementsService> _achievementsService;
        private readonly Mock<ICoursesService> _coursesService;

        private readonly ResultsManager _resultsManager;

        public ResultsManagerTests()
        {
            _resultsService = new Mock<IResultsService>();
            _achievementsService = new Mock<IAchievementsService>();
            _coursesService = new Mock<ICoursesService>();

            _resultsManager = new ResultsManager(_resultsService.Object, _achievementsService.Object, _coursesService.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("someNotValidUserGuid")]

        public void GetCompletedLessonsTest_Fails(string userGuid)
        {
            _resultsService.Setup(x => x.GetUserLessonsByUserGuid(It.IsAny<string>())).Returns((new List<UserLesson>()).AsQueryable());
            _resultsService.Setup(x => x.GetUserByGuid(It.IsAny<string>())).Returns((User)null);

            var result = _resultsManager.GetCompletedLessons(userGuid);
            Assert.True(result != null);
            Assert.False(result.Success);
        }


        public static readonly object[][] GetUserLessonData =
        {
            new object[] { "userGuid", 0, new List<UserLesson> { }},

            new object[] { "userGuid", 1, new List<UserLesson> { new UserLesson { UserId = 1, LessonId = 1, IsCompleted = true, EndDate = DateTime.Now},//count
                                                              new UserLesson { UserId = 1, LessonId = 1, IsCompleted = true, EndDate = DateTime.Now} }},//not count as not unique

            new object[] { "userGuid", 2, new List<UserLesson> { new UserLesson { UserId = 1, LessonId = 1, IsCompleted = true, EndDate = DateTime.Now}, //count
                                                              new UserLesson { UserId = 1, LessonId = 1, IsCompleted = true, EndDate = DateTime.Now}, //not count as not unique
                                                              new UserLesson { UserId = 1, LessonId = 2, IsCompleted = false, EndDate = DateTime.Now}, //not count as not passed
                                                              new UserLesson { UserId = 1, LessonId = 3, IsCompleted = true, EndDate = DateTime.Now}, //count
            }}
        };
        [Theory, MemberData(nameof(GetUserLessonData))]

        public void GetCompletedLessonsTest_Passt(string userGuid, int resultCount, List<UserLesson> userLessonsData)
        {
            _resultsService.Setup(x => x.GetUserLessonsByUserGuid(It.IsAny<string>())).Returns(userLessonsData.AsQueryable());
            _resultsService.Setup(x => x.GetUserByGuid(It.IsAny<string>())).Returns(new User { UserGuid = userGuid, Id = 1 });

            var result = _resultsManager.GetCompletedLessons(userGuid);
            Assert.True(result != null);
            Assert.True(result.Success);
            Assert.True(result.Model.Count == resultCount);

        }

        [Fact()]
        public void GetUserAchievementTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void CheckUserTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetFinishedUniqueUserLessonsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        public static readonly object[][] GetUserLessonForChapterData =
        {
            new object[] { 0, null, new List<Chapter>{}},
            new object[] { 0, new List<UserLesson> { },
                                new List<Chapter>{ new Chapter { Id = 1, Lessons = new List<Lesson>{ new Lesson { Id = 1}, new Lesson { Id = 2}} }, //need to be in result
                                                    new Chapter { Id = 2, Lessons = new List<Lesson>{ new Lesson { Id = 3}, new Lesson { Id = 4}, new Lesson { Id = 5}}}, //need to be in result
                                                    new Chapter { Id = 3, Lessons = new List<Lesson>{ new Lesson { Id = 6}, new Lesson { Id = 7}, new Lesson { Id = 8}}},
                                                 }
            },
            new object[] { 0, new List<UserLesson> { },
                                new List<Chapter>{}
            },

            new object[] {  2, new List<UserLesson> { new UserLesson { UserId = 1, LessonId = 1, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 1} },
                                                                new UserLesson { UserId = 1, LessonId = 2, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 1}},
                                                                new UserLesson { UserId = 1, LessonId = 3, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 2}},
                                                                new UserLesson { UserId = 1, LessonId = 4, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 2}},
                                                                new UserLesson { UserId = 1, LessonId = 5, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 2}},
                                                                new UserLesson { UserId = 1, LessonId = 6, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 3}}
                                                    },
                                new List<Chapter>{ new Chapter { Id = 1, Lessons = new List<Lesson>{ new Lesson { Id = 1}, new Lesson { Id = 2}} }, //need to be in result
                                                    new Chapter { Id = 2, Lessons = new List<Lesson>{ new Lesson { Id = 3}, new Lesson { Id = 4}, new Lesson { Id = 5}}}, //need to be in result
                                                    new Chapter { Id = 3, Lessons = new List<Lesson>{ new Lesson { Id = 6}, new Lesson { Id = 7}, new Lesson { Id = 8}}},
                                                    new Chapter { Id = 4, Lessons = new List<Lesson>{ new Lesson { Id = 9}, new Lesson { Id = 10 } }},
                                                 }

            },
        };

        [Theory, MemberData(nameof(GetUserLessonForChapterData))]
        public void GetFinishedChaptersTest_Passt(int resultCount, List<UserLesson> finishedLessons, List<Chapter> allChapters)
        {
            if (allChapters == null)
            {
                _coursesService.Setup(x => x.GetChapters()).Returns((IQueryable<Chapter>)null);

            }
            else
            {
                _coursesService.Setup(x => x.GetChapters()).Returns(allChapters.AsQueryable());
            }

            var result = _resultsManager.GetFinishedChapters(finishedLessons);
            Assert.True(result != null);
            Assert.True(result.Count == resultCount);
        }

        public static readonly object[][] GetUserLessonForCoursesData = {
            new object[] { 0, null, null, null},
            new object[] { 0, new List<Course> { }, null, null},
            new object[] { 0, new List<Course> { }, new List<Chapter>{ }, null},
            new object[] { 0, new List<Course> { }, new List<Chapter>{ }, new List<UserLesson> { }},

            new object[] { 1, new List<Course> { new Course { Id = 1, Chapters = new List<Chapter> { new Chapter { Id = 1}, new Chapter { Id = 2} } },//need to be in result
                                                new Course { Id = 2, Chapters = new List<Chapter> { new Chapter { Id = 3}, new Chapter { Id = 4} }}},
                            new List<Chapter> { new Chapter { Id = 1, CourseId = 1, Lessons = new List<Lesson> { new Lesson { Id = 1 }, new Lesson { Id = 2 } }} ,
                                                new Chapter { Id = 2, CourseId = 1, Lessons = new List<Lesson>{ new Lesson { Id = 3}, new Lesson { Id = 4}, new Lesson { Id = 5}}}},
                            new List<UserLesson> { new UserLesson { UserId = 1, LessonId = 1, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 1, Chapter = new Chapter{ Id = 1, CourseId = 1 } } },//need to be in result
                                                    new UserLesson { UserId = 1, LessonId = 2, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 1, Chapter = new Chapter{ Id = 1, CourseId = 1 } }},//need to be in result
                                                    new UserLesson { UserId = 1, LessonId = 3, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 2, Chapter = new Chapter{Id = 2, CourseId = 1 } }},//need to be in result
                                                    new UserLesson { UserId = 1, LessonId = 4, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 2, Chapter = new Chapter{Id = 2, CourseId = 1 } }},//need to be in result
                                                    new UserLesson { UserId = 1, LessonId = 5, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 2, Chapter = new Chapter{Id = 2, CourseId = 1 } }},//need to be in result
                                                    new UserLesson { UserId = 1, LessonId = 6, IsCompleted = true, EndDate = DateTime.Now, Lesson = new Lesson{ ChapterId = 3, Chapter = new Chapter{Id = 3, CourseId = 2,} }}
            }

            },
        };
        [Theory, MemberData(nameof(GetUserLessonForCoursesData))]

        public void GetFinishedCoursesTest_Passt(int resultCount, List<Course> allCourses, List<Chapter> finishedChapters, List<UserLesson> finishedLessons)
        {
            if (allCourses == null)
            {
                _coursesService.Setup(x => x.GetCorses()).Returns((IQueryable<Course>)null);
            }
            else
            {
                _coursesService.Setup(x => x.GetCorses()).Returns(allCourses.AsQueryable());
            }
            var result = _resultsManager.GetFinishedCourses(finishedLessons, finishedChapters);
            Assert.True(result != null);
            Assert.True(result.Count == resultCount);
        }
    }
}