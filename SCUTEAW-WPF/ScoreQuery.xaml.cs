using AutoMapper;
using Newtonsoft.Json.Linq;
using SCUTEAW_App.Model;
using SCUTEAW_Lib.Component.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                        if (info.SelectedTerm == RequestHelper.TransformTermIndices(info.SelectableTerm[i]))
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
                list.Add(Mapper.Map<ScoreItemModel>(item));
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
