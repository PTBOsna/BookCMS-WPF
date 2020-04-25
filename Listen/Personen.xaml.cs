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
            Person sel = dgPersonen.SelectedItem as Person;
            AddPerson ad = new AddPerson(sel.PersonID.ToString(), false);
            ad.ShowDialog();
            dgPersonen.ItemsSource = null;
            dgPersonen.ItemsSource = Admin.conn.Person;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPerson ad = new AddPerson("Name, Vorname",true);
            ad.ShowDialog();
            dgPersonen.ItemsSource = null;
            dgPersonen.ItemsSource = Admin.conn.Person;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Person sel = dgPersonen.SelectedItem as Person;
            if (MessageBoxResult.Yes == MessageBox.Show("Are you shure", "Person '" + sel.SortBy + "' entfernen", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                var sto = from s in Admin.conn.Person where s.PersonID == sel.PersonID select s;
                Admin.conn.Person.DeleteAllOnSubmit(sto);
                Admin.conn.SubmitChanges();
                dgPersonen.ItemsSource = null;
                dgPersonen.ItemsSource = Admin.conn.Person;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtSuchPerson_TextChanged(object sender, TextChangedEventArgs e)
        {
            var erg = from ep in Admin.conn.Person where ep.SortBy.StartsWith(txtSuchPerson.Text) select ep;
            dgPersonen.ItemsSource = erg;
            dgPersonen.DataContext = erg;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSuchPerson.Text = null;
        }
    }
}
