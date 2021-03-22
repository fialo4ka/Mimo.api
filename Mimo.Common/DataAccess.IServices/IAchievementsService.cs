using Mimo.Common.DBEntities.Achievements;
using System.Linq;

namespace Mimo.Common.DataAccess.IServices
{
    public interface IAchievementsService
    {
        IQueryable<Achievement> GetAllAchievements();
    }
}
