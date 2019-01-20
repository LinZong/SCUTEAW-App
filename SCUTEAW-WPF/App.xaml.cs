using SCUTEAW_Lib.Component.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace SCUTEAW_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //define some global variables here.
        public ScutEduAdm EduAdmInstance { get; set; }
        public bool IsInOfflineMode = true;
    }
}
