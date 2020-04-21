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

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für XDGTest.xaml
    /// </summary>
    public partial class XDGTest : Window
    {
        
        public XDGTest()
        {
            InitializeComponent();
            var pr = from p in Admin.conn.Person select p;
            this.DataContext = pr.ToList();
            DGPeson.ItemsSource = pr;
            var ort = from o in Admin.conn.Standort select new { Ort = o.SortBy, id = o.ID };
            //.ItemsSource = pr;
            dgtest.ItemsSource = ort;
          
        }

        private void ComboBoxNorm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DGPeson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person sel = DGPeson.SelectedItem as Person;
            var selpers = from p in Admin.conn.Person where p.SortBy.StartsWith(sel.SortBy.Substring(0, 3)) select new { DBName = p.SortBy, id = p.PersonID};
            dgtest.ItemsSource = selpers;
            dgVorschlag.ItemsSource = selpers;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void dgVorschlag_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
