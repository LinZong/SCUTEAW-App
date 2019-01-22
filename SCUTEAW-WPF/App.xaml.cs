using SCUTEAW_App.MapperRules;
using SCUTEAW_Lib.Component.Login;
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
        public bool IsInOfflineMode = false;
        public App()
        {
            MapperRulesConfigurator.Configure();
        }
    }
}
