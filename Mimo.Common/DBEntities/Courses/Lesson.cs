using Mimo.Common.DBEntities.Results;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimo.Common.DBEntities.Courses
{
    [Table("Lesson")]
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ChapterId { get; set; }

        public int Order { get; set; }

        [Required]
        public string Text { get; set; }


        public virtual Chapter Chapter { get; set; }
        public virtual ICollection<UserLesson> UserLesson { get; set; }

    }
}
