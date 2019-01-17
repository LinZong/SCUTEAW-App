using SCUTEAW_App.DataValidator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void PasswordModeLogin(object sender,RoutedEventArgs e)
        {
            var Errors = ValidatorHelper.GetValidationErrors(Login_Passwd_StudentId);
            if (Errors.Count > 0)
            {
                var ErrorMsgs = Errors.Aggregate((a, b) => a + "\n" + b);
                MessageBox.Show(ErrorMsgs, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var StuId = (FindName("Login_Passwd_StudentId") as TextBox).Text;
            var StuPasswd = (FindName("Login_Passwd_Password") as PasswordBox).Password;
        }
        private void CookieModeLogin(object sender,RoutedEventArgs e)
        {
            var Errors = ValidatorHelper.GetValidationErrors(Login_Token_StudentId,Login_Token_JSESSION,Login_Token_JwxtToken);
            if(Errors.Count>0)
            {
                var ErrorMsgs = Errors.Aggregate((a, b) => a + "\n" + b);
                MessageBox.Show(ErrorMsgs, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var StuId = (FindName("Login_Token_StudentId") as TextBox).Text;
            var Jsession = (FindName("Login_Token_JSESSION") as TextBox).Text;
            var JwxtToken = (FindName("Login_Token_JwxtToken") as TextBox).Text;
        }
        private void ShowHowToUseCookie(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }
    }


}
