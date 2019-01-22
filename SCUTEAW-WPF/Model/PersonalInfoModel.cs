using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUTEAW_App.Model
{
    public class PersonalInfoModel
    {
        public string Name { get; set; }
        public string Major { get; set; }
        public string StudentId { get; set; }
        public string LoginMode { get; set; }
    }

    public class CourseScore
    {
        public string CourseName { get; set; }
        public string Score { get; set; }
    }
}
