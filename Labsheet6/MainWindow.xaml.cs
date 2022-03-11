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

namespace Labsheet6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities db = new Entities();
        public MainWindow()
        {
            InitializeComponent();
        }

        //Exercise 1
        private void Ex1Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Categories
                        join p in db.Products on c.CategoryName equals p.Category.CategoryName
                        orderby c.CategoryName
                        select new { Category = c.CategoryName, Product = p.ProductName };

            var results = query.ToList();

            Ex1DgDisplay.ItemsSource = results;

            Ex1TblkCount.Text = results.Count.ToString();
        }

        //Exercise 2
        private void Ex2Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        orderby p.Category.CategoryName, p.ProductName
                        select new { Category = p.Category.CategoryName, Product = p.ProductName };
            var results = query.ToList();

            Ex2DgDisplay.ItemsSource = results;
            Ex2TblkCount.Text = results.Count.ToString();
        }

        private void Ex3Button_Click(object sender, RoutedEventArgs e)
        {
            //return the total number of ords for product 7
            var query1 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail.UnitPrice * detail.Quantity
                          );

            var query2 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail.UnitPrice * detail.Quantity);

            int numberOfOrders = query1.Count();
            decimal totalValue = query2.Sum();
            decimal averageValue = query2.Average();

            Ex3TblkCount.Text = string.Format(
                "Total number of orders {0}\nValue of Orders {1:C}\nAverage Order Value{2:C}",
                numberOfOrders, totalValue, averageValue);
        }

        private void Ex4Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        where customer.Orders.Count >= 20
                        select new
                        {
                            Name = customer.CompanyName,
                            OrderCount = customer.Orders.Count
                        };
            Ex4DgDisplay.ItemsSource = query.ToList();

        }

        private void Ex5Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        where customer.Orders.Count < 3
                        select new
                        {
                            Company = customer.CompanyName,
                            City = customer.City,
                            Region = customer.Region,
                            Order = customer.Orders.Count
                        };

            Ex5DgDisplay.ItemsSource = query.ToList();

        }
        //exercise 6
        private void Ex6Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in db.Customers
                        orderby customer.CompanyName
                        select customer.CompanyName;


            Ex6LbxCustomers.ItemsSource = query.ToList();
        }
        //ex 6 part 2
        private void Ex6LbxCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string company = (string)Ex6LbxCustomers.SelectedItem;

            if (company != null)
            {
                var query = (from detail in db.Order_Details
                             where detail.Order.Customer.CompanyName == company
                             select detail.UnitPrice * detail.Quantity).Sum();

                Ex6TblkCount.Text = string.Format("Total for supplier {0}\n\n{1:c}", company, query);
            }
        }

        private void Ex7Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        group p by p.Category.CategoryName into g
                        orderby g.Count() descending
                        select new
                        {
                            Category = g.Key,
                            Count = g.Count()

                        };

            Ex7DgDisplay.ItemsSource = query.ToList();
        }
    }
}
