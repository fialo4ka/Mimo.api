using Mimo.Common.DataModels;
using System.Collections.Generic;

namespace Mimo.Common.IManagers
{
    public interface IResultsManager
    {
        DataResponceModel<List<LessonProgressModel>> GetCompletedLessons(string userGuid);
        DataResponceModel<List<AchievementModel>> GetUserAchievement(string userGuid);
    }
}
