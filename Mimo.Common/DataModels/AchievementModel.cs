using Mimo.Common.DBEntities.Achievements;

namespace Mimo.Common.DataModels
{
    public class AchievementModel
    {
        public string AchievementName { get; set; }
        public int AchievementId { get; set; }
        public bool IsCompleted { get; set; }
        public string Progress { get; set; }
    }
}
