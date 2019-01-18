using SCUTEAW_App.DataValidator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Activation;
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
using SCUTEAW_Lib.Component.Login;
using SCUTEAW_Lib.Component.Network;
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
            InitializeComponent();
            app = (Application.Current as App);
        }
        private void LoginExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TriggleLoginButton()
        {
            Passwd_Login_Button.IsEnabled = !Passwd_Login_Button.IsEnabled;
            Cookie_Login_Button.IsEnabled = !Cookie_Login_Button.IsEnabled;
        }
        private void PasswordModeLogin(object sender, RoutedEventArgs e)
        {

            if (!ValidateLoginInfo(Login_Passwd_StudentId)) return;
            //collect Login info.
            var StuId = Login_Passwd_StudentId.Text;
            var StuPasswd = Login_Passwd_Password.Password;

            // Try to login.
            TriggleLoginButton();
            app.EduAdmInstance = string.IsNullOrEmpty(Properties.Settings.Default.ProxyString) ? 
                                    new ScutEduAdm() : 
                                    new ScutEduAdm(new EawRequest(new System.Net.WebProxy(Properties.Settings.Default.ProxyString)));

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
            app.EduAdmInstance = string.IsNullOrEmpty(Properties.Settings.Default.ProxyString) ?
                                    new ScutEduAdm() :
                                    new ScutEduAdm(new EawRequest(new System.Net.WebProxy(Properties.Settings.Default.ProxyString)));
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
    }




}
