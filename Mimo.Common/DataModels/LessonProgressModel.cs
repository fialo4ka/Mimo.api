using Mimo.Common.DBEntities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mimo.Common.DataModels
{
    public class LessonProgressModel
    {
        public virtual int LessonId { get; set; }
        public virtual string LessonText { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
