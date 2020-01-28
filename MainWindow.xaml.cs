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
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string selDilplay;
        Int32 rolle;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rolle = 7;
            selDilplay = "Titel";
            var buch = from b in Admin.conn.Buch select b;
            BuchGrid.ItemsSource = buch.ToList();
            //TreeView Personen laden
            LoadPersonTV();
            //Combobox Rolle laden
            var AutorRolle = from ar in Admin.conn.AutorRolle orderby ar.AutorRolle1 select ar;
            foreach (var rolle in AutorRolle)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = rolle.AutorRolle1;
                item.Tag = rolle.ID;   //key-value
                ComboBoxPersonen.Items.Add(item);
            }

        }

        private void LoadPersonTV()
        {

            foreach (var letter in Admin.myAlpha)
            {
                //create a new Item
                var item = new TreeViewItem();

                item.Header = letter;
                item.Tag = letter;
                //Add ad dummy-Item
                item.Items.Add(null);
                item.Expanded += PersonExpandet;
                PersonTrv.Items.Add(item);
            }
        }

        private void PersonExpandet(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (item.Items.Count != 1 || item.Items[0] != null)
            {
                return;
            }
            item.Items.Clear();

            var personen = from p in Admin.conn.PersonRolle where p.SortBy.StartsWith(item.Tag.ToString()) && p.RolleID == rolle orderby p.SortBy select p;


            foreach (var person in personen)
            {

                var subItem = new TreeViewItem();
                subItem.Header = person.SortBy;
                subItem.Tag = person.PersonID.ToString();
                item.Items.Add(subItem);

            }

        }

        private void DGModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (tb1.SelectedIndex == -1)
            {
                return;
            }
            string n = (tb1.SelectedItem as TabItem).Header.ToString();
            if (n == "Alles")
            {
                n = "";
            }
            if (selDilplay == "Titel")
            {
                var buch = from b in Admin.conn.Buch where b.TitelIndex.StartsWith(n) select b;
                if (buch.Count() > 0 == true)
                {
                    BuchGrid.ItemsSource = buch.ToList();
                }
                else
                {
                    if (BuchGrid == null)
                    {
                        return;
                    }
                    BuchGrid.ItemsSource = null;
                }
            }
            if (n == "0-9")
            {
                var buch = from b in Admin.conn.Buch
                           where b.TitelIndex.StartsWith("0") ||
                          b.TitelIndex.StartsWith("1") ||
                         b.TitelIndex.StartsWith("2") ||
                           b.TitelIndex.StartsWith("3") ||
                             b.TitelIndex.StartsWith("4") ||
                               b.TitelIndex.StartsWith("5") ||
                                 b.TitelIndex.StartsWith("6") ||
                                   b.TitelIndex.StartsWith("7") ||
                                     b.TitelIndex.StartsWith("8") ||
                                       b.TitelIndex.StartsWith("9")
                           select b;
                if (buch.Count() > 0 == true)
                {
                    BuchGrid.ItemsSource = buch.ToList();
                }
            }

        }

        //private void btSelAutor_Click(object sender, RoutedEventArgs e)
        //{
        //    selDilplay = "Autor";
        //}

        //private void btSelTitel_Click(object sender, RoutedEventArgs e)
        //{
        //    selDilplay = "Titel";
        //}

        private void PersonTrv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (PersonTrv.SelectedItem != null)
            {
                var item = (TreeViewItem)PersonTrv.SelectedItem;
                string id = item.Tag as string;
                // Prüfen ob Node eine Ziffer ist
                try
                {
                    Int32.Parse(id);
                    var myAutorBuch = from ab in Admin.conn.AutorBuchLink
                                      from b in Admin.conn.Buch
                                      where ab.PersonID == Int32.Parse(id) && b.ID == ab.BuchID
                                      select b;
                    if (myAutorBuch != null)
                    {
                        BuchGrid.ItemsSource = myAutorBuch;
                    }
                }
                catch (Exception)
                {
                    return;
                }

            }
        }


        private void ComboBoxPersonen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = ComboBoxPersonen.SelectedItem as ComboBoxItem;
            rolle = (int)item.Tag;
            //MessageBox.Show(rolle.ToString());
            PersonTrv.Items.Clear();
            LoadPersonTV();
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            txtSuche.Text = null;
        }

        private void txtSuche_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(txtSuche.Text);
            if (string.IsNullOrEmpty(txtSuche.Text) == false)
            {
                var buch = from b in Admin.conn.Buch where b.TitelIndex.Contains(txtSuche.Text) select b;
                BuchGrid.ItemsSource = buch.ToList();
            }
            else
            {
                var buch = from b in Admin.conn.Buch select b;
                BuchGrid.ItemsSource = buch.ToList();
            }

        }
    }
}
