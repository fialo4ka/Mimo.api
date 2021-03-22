using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimo.Common.DBEntities.Courses
{
    [Table("Course")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        //Description, creation dateTime, level ...
        //author, feedbacks - in separate tables and more...

        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}
