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
using System.Windows.Shapes;

namespace SCUTEAW_App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly App app;
        public MainWindow()
        {
            InitializeComponent();
            app = (Application.Current as App);
            Closing += ClosingMainWindow;
        }

        private void ClosingMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            app.EduAdmInstance?.LogoutScutEduAdm();
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            app.EduAdmInstance?.LogoutScutEduAdm();
            Application.Current.Shutdown();
        }
        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
           e.CanExecute = true;
        }
    }
}
