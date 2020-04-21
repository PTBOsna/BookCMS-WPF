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
using BookCMS_WPF;
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF.Listen
{
    /// <summary>
    /// Interaktionslogik für Personen.xaml
    /// </summary>
    public partial class Personen : Window
    {
        public Personen()
        {
            InitializeComponent();
            this.DataContext = Admin.conn.Person;
        }

        private void BtnEditClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPerson ad = new AddPerson("Name");
            ad.ShowDialog();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
