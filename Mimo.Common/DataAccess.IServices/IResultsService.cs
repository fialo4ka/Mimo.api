using Mimo.Common.DBEntities.Results;
using System.Linq;

namespace Mimo.Common.DataAccess.IServices
{
    public interface IResultsService
    {
        IQueryable<UserLesson> GetUserLessonsByUserGuid(string userGuid);
        User GetUserByGuid(string userGuid);
    }
}
