using Microsoft.EntityFrameworkCore;
using Mimo.Common.DataAccess.IServices;
using Mimo.Common.DBEntities.Courses;
using System.Linq;

namespace Mimo.DataAccess
{
    public class CoursesService : ICoursesService
    {
        public IQueryable<Chapter> GetChapters()
        {
            var ctx = new MimoDbContext();
            using var transaction = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            return ctx.Chapters.Include(x => x.Lessons);
        }

        public Course GetCorseById(int? id)
        {
            var ctx = new MimoDbContext();
            using var transaction = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            return ctx.Courses.Include(x => x.Chapters).FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Course> GetCorses()
        {
            var ctx = new MimoDbContext();
            using var transaction = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            return ctx.Courses.Include(x => x.Chapters).ThenInclude(x => x.Lessons);
        }
    }
}
