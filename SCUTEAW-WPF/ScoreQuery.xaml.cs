using Newtonsoft.Json.Linq;
using SCUTEAW_App.Model;
using SCUTEAW_Lib.Component.Login;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// ScoreQueryxaml.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreQuery : UserControl
    {
        private App app;
        public ScoreQuery()
        {
            app = (Application.Current as App);
            InitializeComponent();
            LoadScoreList();
        }
        public void LoadScoreList()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var info = app.EduAdmInstance.GetQueryCourseScheduleInfo();
                QueryTermYear.ItemsSource = info.SelectableYear;
                QueryTermSeason.ItemsSource = info.SelectableTerm;
                if (info.SelectedYear != null && info.SelectedTerm != null)
                {
                    for (int i = 0; i < info.SelectableYear.Count; i++)
                    {
                        if (info.SelectableYear[i].StartsWith($"{info.SelectedYear}-"))
                        {
                            QueryTermYear.SelectedIndex = i;
                            break;
                        }
                    }
                    for (int i = 0; i < info.SelectableTerm.Count; i++)
                    {
                        if (info.SelectedTerm == ScutEduAdm.TransformTermIndices(info.SelectableTerm[i]))
                        {
                            QueryTermSeason.SelectedIndex = i;
                            break;
                        }
                    }
                    try
                    {
                        var str = app.EduAdmInstance.GetScoreListJson(info.SelectedYear, info.SelectedTerm);
                        LoadScoreListGrid(str);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("$尝试获取成绩失败" + e.Message);
                    }
                }
            }), null);
        }
        public void LoadScoreListGrid(string str)
        {
            var root = JObject.Parse(str);
            var coll = (root.GetValue("items") as JArray).Cast<JObject>();
            List<ScoreItemModel> list = new List<ScoreItemModel>();
            foreach (var item in coll)
            {
                var CourseName = item.GetValue("kcmc").ToString();
                var CourseNature = item.GetValue("kcxzmc").ToString();
                var Score = item.GetValue("bfzcj").ToString();
                var CourseCredit = item.GetValue("xf").ToString();
                var CourseGPA = item.GetValue("jd").ToString();
                var ScoreType = item.GetValue("ksxz").ToString();
                var CourseCollege = item.GetValue("kkbmmc").ToString();
                var CourseType = item.GetValue("kclbmc").ToString();
                list.Add(new ScoreItemModel()
                {
                    CourseName = CourseName,
                    CourseNature = CourseNature,
                    Score = Score,
                    CourseCredit = CourseCredit,
                    CourseGPA = CourseGPA,
                    ScoreType = ScoreType,
                    CourseCollege = CourseCollege,
                    CourseType = CourseType
                });
            }
            ScoreListGrid.ItemsSource = list;
        }

        private void PerformScoreListQuery(object sender, RoutedEventArgs e)
        {
            var year = QueryTermYear.Text.Split('-')[0];
            var term = QueryTermSeason.Text;
            if (year != null && term != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var str = app.EduAdmInstance.GetScoreListJson(year, term);
                        LoadScoreListGrid(str);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"尝试获取{year} - {term}的成绩失败");
                    }
                }), null);
            }
        }
    }
}
