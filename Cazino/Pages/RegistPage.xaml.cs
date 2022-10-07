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
using Cazino.DB;

namespace Cazino.Pages
{
    /// <summary>
    /// Interaction logic for RegistPage.xaml
    /// </summary>
    public partial class RegistPage : Page
    {
        public RegistPage()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            var login = tbLogin.Text;
            var password = pbPassword.Password;
            var secondPassword = pbSecondPassword.Password;

            if (password == "" || login == "")
            {
                MessageBox.Show("Логин и/или пароль не могут быть пустыми", "Ошибка");
                return;
            }

            if (password != secondPassword)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка");
                return;
            }

            if (!DataAccess.CheckLogin(login))
            {
                MessageBox.Show("Данный логин уже занят", "Ошибка");
                return;
            }

            if (DataAccess.RegistartionUser(login, password))
            {
                var user = DataAccess.GetUser(login, password);
                NavigationService.Navigate(new LoginPage());
            }
        }
    }
}
