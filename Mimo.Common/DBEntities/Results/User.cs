using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimo.Common.DBEntities.Results
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserGuid { get; set; }

        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
        public virtual ICollection<UserLesson> UserLesson { get; set; }        
            
    }
}
