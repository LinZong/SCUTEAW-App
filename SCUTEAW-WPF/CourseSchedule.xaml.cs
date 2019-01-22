using AutoMapper;
using Newtonsoft.Json.Linq;
using SCUTEAW_App.Model;
using SCUTEAW_Lib.Component.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SCUTEAW_App
{
    /// <summary>
    /// CourseSchedule.xaml 的交互逻辑
    /// </summary>
    public partial class CourseSchedule : UserControl
    {
        private App app;
        public List<Grid> CoursePartGridCollection;
        public CourseSchedule()
        {
            app = (Application.Current as App);
            InitializeComponent();
            LoadCourseScheduleFramework();
            LoadQueryComboBoxContent();
        }
        public void LoadQueryComboBoxContent()
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
                        var str = app.EduAdmInstance.GetCourseScheduleJson(info.SelectedYear, info.SelectedTerm);
                        LoadCourseScheduleTable(str);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("尝试获取课程表失败"+e.Message);
                    }
                }
            }), null);
        }
        public void LoadCourseScheduleTable(string str)
        {
            var template = Resources["CourseItemTemplate"] as DataTemplate;

            if (CoursePartGridCollection != null)
            {
                for (int i = 0; i < CoursePartGridCollection.Count; i++)
                {
                    CoursePartGridCollection[i]?.Children.Clear();
                }
            }
            var root = JObject.Parse(str);
            var LabCourse = (root.GetValue("sjkList") as JArray).Cast<JObject>();
            var NormalCourseWeekGroup = (root.GetValue("kbList") as JArray).Cast<JObject>().GroupBy(x => x.GetValue("xqj").ToString());
            foreach (var day in NormalCourseWeekGroup)
            {
                int week = int.Parse(day.Key) - 1;
                var OneDayCourses = day.GroupBy(x => x.GetValue("jcs").ToString());


                foreach (var item in OneDayCourses)
                {
                    List<CourseItemModel> courseItemColl = new List<CourseItemModel>();

                    var HaveCourseTimeSplit = item.Key.Split('-');
                    int CourseLastTimePeriod = HaveCourseTimeSplit.Select(x => int.Parse(x)).Aggregate((a, b) => b - a);
                    int RowBegin = int.Parse(HaveCourseTimeSplit[0]) / 2;

                    foreach (var one in item)
                    {
                        var model = Mapper.Map<CourseItemModel>(one);
                        model.ItemColor = new SolidColorBrush(Colors.Black);
                        courseItemColl.Add(model);
                    }
                    UserControl CourseShowItem = new UserControl
                    {
                        Content = courseItemColl,
                        ContentTemplate = template
                    };
                    CoursePartGridCollection[week].Children.Add(CourseShowItem);
                    Grid.SetRow(CourseShowItem, RowBegin);
                    Grid.SetRowSpan(CourseShowItem, CourseLastTimePeriod);
                }
            }
        }
        
        public void LoadCourseScheduleFramework()
        {
            /*
             * 创建7个Grid，每个Grid有12个Row，平均分配
             */
            GridOperationExtension.InsertFrameForGrid(CourseScheduleFrameworkGrid);
            GridOperationExtension.InsertFrameForGrid(ClassSectionGrid, true, false);

            if (CoursePartGridCollection == null)
            {
                CoursePartGridCollection = new List<Grid>();
            }
            else CoursePartGridCollection.Clear();
            for (int i = 0; i < 7; i++)
            {
                Grid grid = new Grid
                {
                    Name = $"Week{i + 1}CourseList"
                };
                var RowList = Enumerable.Range(0, 6).Select(_ => new RowDefinition()).ToList();
                grid.RowDefinitions.AddRows(RowList);
                CoursePartGridCollection.Add(grid);
                CourseScheduleFrameworkGrid.Children.Add(grid);
                Grid.SetRow(grid, 1);
                Grid.SetColumn(grid, 2 + i);
                Grid.SetRowSpan(grid, 3);
                GridOperationExtension.InsertFrameForGrid(grid, true, false);
            }
        }

        private void QuerySchedule(object sender, RoutedEventArgs e)
        {
            var year = QueryTermYear.Text.Split('-')[0];
            var term = QueryTermSeason.Text;
            if (year != null && term != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var str = app.EduAdmInstance.GetCourseScheduleJson(year, term);
                        LoadCourseScheduleTable(str);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("尝试获取课程表失败");
                    }
                }), null);
            }
        }
    }



    public static class GridOperationExtension
    {
        public static void AddRows(this RowDefinitionCollection coll, List<RowDefinition> rows)
        {
            foreach (var i in rows)
            {
                coll.Add(i);
            }
        }
        public static void InsertFrameForGrid(Grid grid, bool Addrow = true, bool AddCol = true)
        {
            var rowcon = grid.RowDefinitions.Count;
            var clcon = grid.ColumnDefinitions.Count;
            if (Addrow)
            {
                for (var i = 0; i < rowcon + 1; i++)//行循环添加border
                {
                    var border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Colors.SlateGray),
                        BorderThickness = i == rowcon ? new Thickness(0, 0, 0, 1) : new Thickness(0, 1, 0, 0)
                    };

                    Grid.SetRow(border, i);
                    if (clcon != 0) Grid.SetColumnSpan(border, clcon);
                    grid.Children.Add(border);
                }
            }

            if (AddCol)
            {
                for (var j = 0; j < clcon + 1; j++)//列循环添加border
                {
                    var border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Colors.SlateGray),
                        BorderThickness = j == clcon ? new Thickness(0, 0, 1, 0) : new Thickness(1, 0, 0, 0)
                    };
                    Grid.SetColumn(border, j);
                    if (rowcon != 0) Grid.SetRowSpan(border, rowcon);
                    grid.Children.Add(border);
                }
            }
        }
    }
}
