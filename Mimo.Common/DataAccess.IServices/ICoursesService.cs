using Mimo.Common.DBEntities.Courses;
using System.Linq;

namespace Mimo.Common.DataAccess.IServices
{
    public interface ICoursesService
    {
        Course GetCorseById(int? id);
        IQueryable<Chapter> GetChapters();
        IQueryable<Course> GetCorses();
    }
}
