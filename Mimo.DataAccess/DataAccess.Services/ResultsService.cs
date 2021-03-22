using Microsoft.EntityFrameworkCore;
using Mimo.Common.DataAccess.IServices;
using Mimo.Common.DBEntities.Results;
using System.Linq;

namespace Mimo.DataAccess
{
    public class ResultsService : IResultsService
    {
        public IQueryable<UserLesson> GetUserLessonsByUserGuid(string userGuid)
        {
            var ctx = new MimoDbContext();
            using var transaction = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            return ctx.UserLessons.Include(x => x.Lesson).ThenInclude(x => x.Chapter).ThenInclude(x => x.Course).Include(x => x.User).Where(x => x.User.UserGuid == userGuid);
        }

        public User GetUserByGuid(string userGuid)
        {
            var ctx = new MimoDbContext();
            using var transaction = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            return ctx.Users.FirstOrDefault(x => x.UserGuid == userGuid);
        }
    }
}
