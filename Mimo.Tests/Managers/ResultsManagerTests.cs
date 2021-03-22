using Mimo.Common.DataAccess.IServices;
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


        public static readonly object[][] GetUserMenuData =
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
        [Theory, MemberData(nameof(GetUserMenuData))]

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

        [Fact()]
        public void GetFinishedChaptersTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetFinishedCoursesTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}