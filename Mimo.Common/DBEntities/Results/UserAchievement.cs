using Mimo.Common.DBEntities.Achievements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimo.Common.DBEntities.Results
{
    public class UserAchievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AchievementId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime GetingDate { get; set; }

        public virtual Achievement Achievement { get; set; }
        public virtual User User { get; set; }

    }
}
