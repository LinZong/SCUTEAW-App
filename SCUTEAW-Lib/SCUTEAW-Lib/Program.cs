//using System;
//using System.IO;
//using System.Net;
//using System.Text;
//using SCUTEAW_Lib.Component.Extractor;
//using SCUTEAW_Lib.Component.Login;
//using SCUTEAW_Lib.Component.Network;

//namespace SCUTEAW_Lib
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            System.Console.WriteLine(" -- Welcome to SCUT Educational Administration Website AutoMata v0.0.1 -- ");
//            var loginer = new ScutEduAdm();
//            System.Console.WriteLine(" -- In order to login to the EAW, plz tell me some key info -- ");
//            System.Console.Write(" -- Your Student Id : ");
//            var id = System.Console.ReadLine();
//            System.Console.Write(" -- Your Password (懒得给Cookie登陆写入口了,先这样吧) : ");
//            StringBuilder pw = new StringBuilder();
//            while (true)
//            {
//                ConsoleKeyInfo ck = Console.ReadKey(true);
//                if (ck.Key != ConsoleKey.Enter)
//                {
//                    if (ck.Key != ConsoleKey.Backspace)
//                    {
//                        pw.Append(ck.KeyChar.ToString());
//                        Console.Write("*");
//                    }
//                    else
//                    {
//                        if (pw.Length > 0)
//                        {
//                            pw.Remove(pw.Length - 1, 1); Console.Write("\b \b");
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine();
//                    break;
//                }
//            }
//            var stat = loginer.LoginScutEduAdm(LoginType.UseStudentIdAndPassword, id, pw.ToString());
//            if (stat)
//            {
//                loginer.ShowPersonalInfo();

//                Console.WriteLine();
//                loginer.ShowRecentScore();
//                loginer.LogoutScutEduAdm();
//            }
//            Console.WriteLine("More function and GUI version is COMING SOON! Thank you");
//            Console.ReadLine();
//        }
//        static void TestFunc()
//        {
//            using (var rd = new StreamReader("test.txt", Encoding.UTF8))
//            {
//                var str = rd.ReadToEnd();
//                var score = ContentExtractor.ExtractScore(str);
//                foreach (var i in score)
//                {
//                    Console.WriteLine(i.Key + "  " + i.Value);
//                }
//            }
//        }
//    }
//}
