using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SCUTEAW_App.DataValidator
{
    public class ValidateStudentId : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string StuId = value as string;
            if(string.IsNullOrEmpty(StuId))
            {
                return new ValidationResult(false, Properties.Resources.NeedStudentId);
            }
            try
            {
                Convert.ToUInt64(StuId);
            }
            catch (Exception)
            {
                return new ValidationResult(false, Properties.Resources.StudentIdNeedNumber);
            }
            return new ValidationResult(true, null);
        }
    }

    public class ValidateCookies : ValidationRule
    {
        public string EmptyErrorMessage { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if (string.IsNullOrEmpty(str)) return new ValidationResult(false, EmptyErrorMessage);
            return new ValidationResult(true, null);
        }
    }
}
