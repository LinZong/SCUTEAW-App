using System.Windows;
using System.Windows.Input;

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
            if(!app.IsInOfflineMode)
            {
                Closing += ClosingMainWindow;
            }
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

        private void LogoutHandler(object sender, RoutedEventArgs e)
        {
            app.EduAdmInstance?.LogoutScutEduAdm();
            LoginWindow mwin = new LoginWindow();
            Application.Current.MainWindow = mwin;
            Close();
            mwin.Show();
        }
    }
}
