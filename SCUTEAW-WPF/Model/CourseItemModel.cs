using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SCUTEAW_App.Model
{
    public class CourseItemModel
    {
        public SolidColorBrush ItemColor { get; set; }
        public string CourseName { get; set; }
        public string HaveCourseRoomAndTeacher { get; set; }
        public string HaveCourseWeek { get; set; }
        public string HaveCourseRange { get; set; }

    }
}
