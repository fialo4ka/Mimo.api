using Microsoft.EntityFrameworkCore;
using Mimo.Common.DataAccess.IServices;
using Mimo.Common.DBEntities.Achievements;
using System.Linq;

namespace Mimo.DataAccess
{
    public class AchievementsService : IAchievementsService
    {
        public IQueryable<Achievement> GetAllAchievements()
        {
            var ctx = new MimoDbContext();
            using var transaction = ctx.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            return ctx.Achievements;
        }
    }
}
