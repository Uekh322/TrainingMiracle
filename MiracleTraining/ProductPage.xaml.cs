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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private Client currentUser;
        public ProductPage(Client user)
        {
            InitializeComponent();

            currentUser = user;
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentServices = YusupovMiracleShowEntities.GetContext().Product.ToList();
            int countall = currentServices.Count();
            currentServices = currentServices.Where(p => p.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();
            if (SortComboBox.SelectedIndex == 1)
                currentServices = currentServices.OrderBy(p => p.Price).ToList();
            if (SortComboBox.SelectedIndex == 2)
                currentServices = currentServices.OrderByDescending(p => p.Price).ToList();
            if (FiltrComboBox.SelectedIndex == 1)
                currentServices = currentServices.Where(p => p.Category.CategoryName == "Обувь").ToList();
            if (FiltrComboBox.SelectedIndex == 2)
                currentServices = currentServices.Where(p => p.Category.CategoryName == "Одежда").ToList();
            if (FiltrComboBox.SelectedIndex == 3)
                currentServices = currentServices.Where(p => p.Category.CategoryName == "Аксессуары").ToList();
            int countnow = currentServices.Count();
            CountTextBlock.Text = $"Найдено {countnow} из {countall}";
            ProductListView.ItemsSource = currentServices;
        }

        //ProductListView.ItemsSource = YusupovMiracleShowEntities.GetContext().Product.ToList();


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void FiltrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }
    }
}
