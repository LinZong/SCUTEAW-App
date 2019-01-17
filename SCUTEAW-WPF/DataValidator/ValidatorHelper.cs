using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SCUTEAW_App.DataValidator
{
    public static class ValidatorHelper
    {
		public static List<string> GetValidationErrors(params Control[] components)
        {
            return components.Select(x => Validation.GetErrors(x))
											.Where(x=> x.Count > 0)
											.Select(x => x.First().ErrorContent as string)
											.ToList();
        }
    }
}
