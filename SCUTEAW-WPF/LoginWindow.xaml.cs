using SCUTEAW_App.DataValidator;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SCUTEAW_Lib.Component.Login;
using SCUTEAW_Lib.Component.Network;
using System;

namespace SCUTEAW_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly App app;
        public LoginWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            app = (Application.Current as App);
            LoadRememberStudentId();

        }
        private void LoginExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TriggleLoginButton()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Passwd_Login_Button.IsEnabled = !Passwd_Login_Button.IsEnabled;
                Cookie_Login_Button.IsEnabled = !Cookie_Login_Button.IsEnabled;
            }), null);
        }
        private void PasswordModeLogin(object sender, RoutedEventArgs e)
        {

            if (!ValidateLoginInfo(Login_Passwd_StudentId)) return;
            //collect Login info.
            var StuId = Login_Passwd_StudentId.Text;
            var StuPasswd = Login_Passwd_Password.Password;

            // Try to login.
            TriggleLoginButton();
            PrepareEduAdmInstance();

            if (app.EduAdmInstance.LoginScutEduAdm(LoginType.UseStudentIdAndPassword, out string FailedResult, StuId, StuPasswd))
            {
                // Jump to main window.
                MainWindow mwin = new MainWindow();
                Application.Current.MainWindow = mwin;
                Close();
                mwin.Show();
                TriggleLoginButton();
            }
            else
            {
                TriggleLoginButton();
                MessageBox.Show(FailedResult, "登陆错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void CookieModeLogin(object sender, RoutedEventArgs e)
        {
            if (!ValidateLoginInfo(Login_Token_StudentId, Login_Token_JSESSION, Login_Token_JwxtToken)) return;
            // collect Login info.
            var StuId = Login_Token_StudentId.Text;
            var Jsession = Login_Token_JSESSION.Text;
            var JwxtToken = Login_Token_JwxtToken.Text;
            // Try to login.

            TriggleLoginButton();

            PrepareEduAdmInstance();
            if (app.EduAdmInstance.LoginScutEduAdm(LoginType.UseCookie, out string FailedResult, StuId, Jsession, JwxtToken))
            {
                MainWindow mwin = new MainWindow();
                Application.Current.MainWindow = mwin;
                Close();
                mwin.Show();
                TriggleLoginButton();
            }
            else
            {
                TriggleLoginButton();
                MessageBox.Show(FailedResult, "登陆错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void PrepareEduAdmInstance()
        {
            SaveRememberStudentId();
            if (app.EduAdmInstance == null)
            {
                app.EduAdmInstance = string.IsNullOrEmpty(Properties.Settings.Default.ProxyString) ?
                                    new ScutEduAdm() :
                                    new ScutEduAdm(new EawRequest(new System.Net.WebProxy(Properties.Settings.Default.ProxyString)));
            }
            else
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.ProxyString))
                    app.EduAdmInstance.SetProxy(new System.Net.WebProxy(Properties.Settings.Default.ProxyString));
                else app.EduAdmInstance.SetProxy(new System.Net.WebProxy());
            }
        }
        private void ShowHowToUseCookie(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }
        private bool ValidateLoginInfo(params Control[] ValidateComponents)
        {
            var Errors = ValidatorHelper.GetValidationErrors(ValidateComponents);
            if (Errors.Count > 0)
            {
                var ErrorMsgs = Errors.Aggregate((a, b) => a + "\n" + b);
                MessageBox.Show(ErrorMsgs, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void SaveProxySetting(object sender, RoutedEventArgs e)
        {
            if (!ValidateLoginInfo(Login_ProxyStringBox)) return;
            Properties.Settings.Default.Save();
            MessageBox.Show("Setting saved successfully.", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadRememberStudentId()
        {
            if (Properties.Settings.Default.RememberMe && !string.IsNullOrEmpty(Properties.Settings.Default.StudentIdRemember))
            {
                Login_Passwd_StudentId.Text = Login_Token_StudentId.Text = Properties.Settings.Default.StudentIdRemember;
            }
            else
            {
                Properties.Settings.Default.StudentIdRemember = string.Empty;
                Properties.Settings.Default.Save();
            }
        }
        private void SaveRememberStudentId()
        {
            int idx = LoginModeSwitcher.SelectedIndex;
            if (Properties.Settings.Default.RememberMe)
            {
                Properties.Settings.Default.StudentIdRemember = idx == 0 ? Login_Passwd_StudentId.Text : Login_Token_StudentId.Text;
                Properties.Settings.Default.Save();
            }
        }
    }

}
