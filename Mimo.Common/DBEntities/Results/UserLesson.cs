using Mimo.Common.DBEntities.Courses;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mimo.Common.DBEntities.Results
{
    public class UserLesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public bool IsCompleted { get; set; }


        public virtual User User { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
