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
        private List<OrderItem> orderItems = new List<OrderItem>();
        public ProductPage(Client user)
        {
            InitializeComponent();

            currentUser = user;

            if (currentUser == null)
            {
                UserNameTextBlock.Text = "Гость";
                StackPanelFiltrAndSearch.Visibility = Visibility.Hidden;
                OrderPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserNameTextBlock.Text =
                    currentUser.LastName + " " + currentUser.FirstName;

                RoleTextBlock.Text = currentUser.UserRole.RoleName;
            }
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.MainFrame.CanGoBack)
                Manager.MainFrame.GoBack();
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                var product = ProductListView.SelectedItem as Product;

                if (product == null)
                {
                    SizeComboBox.ItemsSource = null;
                    return;
                }

                SizeComboBox.ItemsSource = product.CommodityPosition
                    .Where(p => p.QuantityInStock > 0)
                    .OrderBy(p => p.Size)
                    .ToList();
            }
        }

        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null)
                return;

            var position = SizeComboBox.SelectedItem as CommodityPosition;

            if (position == null)
            {
                MessageBox.Show("Выберите товар и размер");
                return;
            }

            var item = orderItems.FirstOrDefault(p =>
                p.CommodityPositionID == position.CommodityPositionID);

            if (item == null)
            {
                orderItems.Add(new OrderItem
                {
                    CommodityPositionID = position.CommodityPositionID,
                    CommodityPosition = position,
                    Quantity = 1,
                    PriceAtOrderTime = position.Product.Price
                });
            }
            else
            {
                if (item.Quantity >= position.QuantityInStock)
                {
                    MessageBox.Show("Больше пар этого размера нет в наличии");
                    return;
                }

                item.Quantity++;
            }

            OrderCountTextBlock.Text = $"В заказе: {orderItems.Sum(p => p.Quantity)}";
        }
    }
}
