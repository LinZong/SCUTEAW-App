using SCUTEAW_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCUTEAW_App
{
    /// <summary>
    /// PersonalInfo.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class PersonalInfo : UserControl
    {
        private readonly App app;
        public List<CourseScore> ScoreList { get; set; }
        public PersonalInfoModel personalInfo { get; set; }
        public List<string> RecentCourse { get; set; }
        public PersonalInfo()
        {
            InitializeComponent();
            app = (Application.Current as App);
            LoadPersonalInfoPage();
        }
        public void LoadPersonalInfoPage()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                (string major, string name) = app.EduAdmInstance.ShowPersonalInfo();
                personalInfo = new PersonalInfoModel();
                personalInfo.Major = major;
                personalInfo.Name = name;
                personalInfo.StudentId = app.EduAdmInstance.account.UserAccount.StudentId;
                personalInfo.LoginMode = "登录模式 : " + (app.EduAdmInstance.account.CookieMode ? "Cookie" : "Password");

                PersonalInfoControl.Content = personalInfo;
            }),null);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var Scores = app.EduAdmInstance.ShowRecentScore();
                if (Scores.Count > 0)
                    ScoreList = Scores.Select(x => new CourseScore() { CourseName = x.Key, Score = int.Parse(x.Value) }).ToList();
                else ScoreList = new List<CourseScore>();

                RecentScoreListBox.ItemsSource = ScoreList;
            }),null);
        }
    }
}
