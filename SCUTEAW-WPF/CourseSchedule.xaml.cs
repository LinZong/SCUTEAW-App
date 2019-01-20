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
    /// CourseSchedule.xaml 的交互逻辑
    /// </summary>
    public partial class CourseSchedule : UserControl
    {
        public List<Grid> CoursePartGridCollection;
        public CourseSchedule()
        {
            InitializeComponent();
            LoadCourseScheduleFramework();
            
        }
        public void LoadCourseScheduleFramework()
        {
            /*
             * 创建7个Grid，每个Grid有12个Row，平均分配
             */
            GridOperationExtension.InsertFrameForGrid(CourseScheduleFrameworkGrid);
            GridOperationExtension.InsertFrameForGrid(ClassSectionGrid,true,false);
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
            }
        }
    }

    public static class GridOperationExtension
    {
        public static void AddRows(this RowDefinitionCollection coll,List<RowDefinition> rows)
        {
            foreach (var i in rows)
            {
                coll.Add(i);
            }
        }
        public static void InsertFrameForGrid(Grid grid,bool Addrow = true,bool AddCol = true)
        {
            var rowcon = grid.RowDefinitions.Count;
            var clcon = grid.ColumnDefinitions.Count;
            if(Addrow)
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

            if(AddCol)
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
