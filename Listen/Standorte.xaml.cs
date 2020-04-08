using BookCMS_WPF.DataHandling;
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

namespace BookCMS_WPF.Listen
{
    /// <summary>
    /// Interaktionslogik für Standorte.xaml
    /// </summary>
    public partial class Standorte : Window
    {
        public Standorte()
        {
            InitializeComponent();
            //this.DataContext = Admin.conn.Standort;
            LoadDG();
        }
        private void LoadDG()
        {
            dgStandort.ItemsSource = null;
            var sto = (from s in Admin.conn.Standort orderby s.SortBy select s).ToList();
            dgStandort.ItemsSource = sto;
        }
        private void BtnEditClick(object sender, RoutedEventArgs e)
        {
            Standort sel = dgStandort.SelectedItem as Standort;
            if (sel == null)
            {
                MessageBox.Show("Bitte zunächst einen Standort auswählen!");
                return;
            } 
               Listen. EditStandort ed = new EditStandort(sel);
            ed.ShowDialog();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddStandort ad = new  AddStandort();
            ad.ShowDialog();
            LoadDG();

        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Standort sel = dgStandort.SelectedItem as Standort;
            if (MessageBoxResult.Yes == MessageBox.Show("Are you shure", "Standort '" + sel.Standort1 + "' entfernen", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                var sto = from s in Admin. conn.Standort where s.ID == sel.ID select s;
                Admin.conn.Standort.DeleteAllOnSubmit(sto);
                Admin.conn.SubmitChanges();
                LoadDG();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDG();
        }
    }
}
