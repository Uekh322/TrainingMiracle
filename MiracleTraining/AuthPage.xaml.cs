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

namespace MiracleTraining
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();

            if (login == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }

            var user = YusupovMiracleShowEntities.GetContext()
                .Client.FirstOrDefault(p => p.Login == login);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }

            Manager.MainFrame.Navigate(new ProductPage(user));
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));
        }
    }
}
