using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;

namespace SCUTEAW_Lib.Component.Helper
{
    public static class LoginHelper
    {
        public static long DateTimeNowUnix()
        {
            DateTime time = DateTime.Now;
            //double intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }
        //public static RSACryptoServiceProvider GetRSAProvider(string modulus,string exponent)
        //{
        //    var Rsa = new RSACryptoServiceProvider();
        //    XmlDocument doc = new XmlDocument();

        //    var RSAKeyValue = doc.CreateElement("RSAKeyValue");
        //    var Modulus = doc.CreateElement("Modulus");
        //    var Exponent = doc.CreateElement("Exponent");

        //    Modulus.InnerText = modulus;
        //    Exponent.InnerText = exponent;

        //    RSAKeyValue.AppendChild(Modulus);
        //    RSAKeyValue.AppendChild(Exponent);
        //    doc.AppendChild(RSAKeyValue);
        //    MemoryStream buffer = new MemoryStream();
        //    XmlTextWriter writer = new XmlTextWriter(buffer, Encoding.UTF8);
        //    writer.Formatting = Formatting.Indented;
        //    doc.Save(buffer);

        //    buffer.Position = 0;
        //    StreamReader ReadXml = new StreamReader(buffer);
        //    var RSAPublicKeyXml = ReadXml.ReadToEnd();

        //    Rsa.FromXmlString(RSAPublicKeyXml);
        //    return Rsa;
        //}
        public static string EncryptPassword(string Mod, string Exp, string Passwd, out int ExternalExitCode)
        {
            string path = ConfigurationManager.AppSettings["JsPasswdEncoderPath"];
            string encoded = exec(path, Mod + " " + Exp + " " + Passwd, out ExternalExitCode);
            return encoded;
        }
        public static string exec(string ExePath, string parameters, out int ExitCode)
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = ExePath;
            pProcess.StartInfo.Arguments = parameters; //argument
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.RedirectStandardError = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
            pProcess.Start();
            string output = pProcess.StandardOutput.ReadToEnd(); //The output result
            string errput = pProcess.StandardError.ReadToEnd();
            pProcess.WaitForExit();
            ExitCode = pProcess.ExitCode;
            return output;
        }
    }

}
