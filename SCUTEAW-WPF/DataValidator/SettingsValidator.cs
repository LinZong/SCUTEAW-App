using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SCUTEAW_App.DataValidator
{
    public class ValidateProxyString : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string url = value as string;
            if(string.IsNullOrEmpty(url)) return new ValidationResult(true, null);//代表清空了代理设置，此时应该通过验证.

            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
                return new ValidationResult(true, null);
            return new ValidationResult(false, Properties.Resources.ProxyMustBeUrl);
        }
    }
}
