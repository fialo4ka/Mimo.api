using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimo.Common.DBEntities.Courses
{
    [Table("Chapter")]
    public class Chapter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Order { get; set; }


        public virtual Course Course { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
