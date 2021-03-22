using Mimo.Common.DBEntities.Results;
using Mimo.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimo.Common.DBEntities.Achievements
{
    [Table("Achievement")]
    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public AchievementType AchievementType { get; set; }

        public int? AchievementObjectId { get; set; }

        public int? AchievementObjectIntData { get; set; }

        //AchievementObjectTimeStampData .. or other

        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
    }
}
