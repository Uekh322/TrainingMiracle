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

namespace MiracleTraining
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private readonly List<OrderItem> orderItems;
        public OrderWindow(List<OrderItem> orderItems)
        {

            InitializeComponent();
            this.orderItems = orderItems;
            OrderListView.ItemsSource = orderItems;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = orderItems.Sum(p => p.Quantity * p.PriceAtOrderTime);
            TotalTextBlock.Text = $"Итого: {total:N2} ₽";
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OrderListView.SelectedItem as OrderItem;

            if (item == null || item.Quantity <= 1)
                return;

            item.Quantity--;
            OrderListView.Items.Refresh();
            UpdateTotal();
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OrderListView.SelectedItem as OrderItem;

            if (item == null)
                return;

            if (item.Quantity >= item.CommodityPosition.QuantityInStock)
            {
                MessageBox.Show("Больше пар этого размера нет в наличии");
                return;
            }

            item.Quantity++;
            OrderListView.Items.Refresh();
            UpdateTotal();
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OrderListView.SelectedItem as OrderItem;

            if (item == null)
                return;

            orderItems.Remove(item);
            OrderListView.Items.Refresh();
            UpdateTotal();
        }
    }
}
