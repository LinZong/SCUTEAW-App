using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUTEAW_Lib.Component.Helper
{
    public class RequestHelper
    {
        public static string TransformTermIndices(string original)
        {
            string ReqTerm;
            switch (original)
            {
                case "1":
                    ReqTerm = "3"; break;
                case "2":
                    ReqTerm = "12"; break;
                case "3":
                    ReqTerm = "16"; break;
                default:
                    ReqTerm = original; break;
            }
            return ReqTerm;
        }
    }
}
