using SCUTEAW_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        public bool HideScore = false;
        public PersonalInfo()
        {
            InitializeComponent();
            app = (Application.Current as App);
            if (!app.IsInOfflineMode) LoadPersonalInfoPage();
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
            }), null);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var Scores = app.EduAdmInstance.ShowRecentScore();
                if (Scores.Count > 0)
                    ScoreList = Scores.Select(x => new CourseScore() { CourseName = x.Key, Score = x.Value }).ToList();
                else ScoreList = new List<CourseScore>();
                RecentScoreListBox.ItemsSource = ScoreList;
            }), null);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var CourseList = app.EduAdmInstance.ShowRecentCourses();
                if (CourseList.Count > 0)
                    RecentCourseListBox.ItemsSource = CourseList;
            }), null);
        }

        private void TriggleScoreHide(object sender, RoutedEventArgs e)
        {
            HideScore = !HideScore;
            if(HideScore)
            {
                RecentScoreListBox.ItemTemplate = Resources["RecentScoreItemTemplateHideScore"] as DataTemplate;
                RecentScoreListBox.UpdateDefaultStyle();
                RecentScoreListBox.UpdateLayout();
                (sender as Button).Content = "显示成绩";

            }
            else
            {
                RecentScoreListBox.ItemTemplate = Resources["RecentScoreItemTemplate"] as DataTemplate;
                RecentScoreListBox.UpdateDefaultStyle();
                RecentScoreListBox.UpdateLayout();
                (sender as Button).Content = "隐藏成绩";
            }
        }
    }
}
