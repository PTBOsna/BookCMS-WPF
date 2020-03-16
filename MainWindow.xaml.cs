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
        Int32 cBookID;
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
            //foreach (var rolle in AutorRolle)
            //{
            //    ComboBoxItem item = new ComboBoxItem();
            //    item.Content = rolle.AutorRolle1;
            //    item.Tag = rolle.ID;   //key-value
            //    ComboBoxPersonen.Items.Add(item);
            //}
            ComboBoxPersonen.ItemsSource = AutorRolle;
            ComboBoxPersonen.SelectedValue = 7; //wird später über Settings voreingestellt
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

            //MessageBox.Show(personen.Count().ToString());

            foreach (var person in personen)
            {

                var subItem = new TreeViewItem();
                subItem.Header = person.SortBy;
                subItem.Tag = person.PersonID.ToString();
                item.Items.Add(subItem);

            }

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
                                      where ab.PersonID == Int32.Parse(id) && ab.BuchID == b.ID
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
            //MessageBox.Show(ComboBoxPersonen.SelectedValue.ToString());
            //ComboBoxItem item = ComboBoxPersonen.SelectedItem as ComboBoxItem;

            //////MessageBox.Show(item.Content.ToString());
            rolle = (int)ComboBoxPersonen.SelectedValue;
            PersonTrv.Items.Clear();
            LoadPersonTV();
        }

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            txtSuche.Text = null;
        }

        private void txtSuche_TextChanged(object sender, TextChangedEventArgs e)
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

        private void DGBuch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Buch sel = BuchGrid.SelectedItem as Buch;
            if (sel==null)
            {
                return;
            }
            var selBuch = (from b in Admin.conn.Buch
                          from s in Admin.conn.BuchTyp
                          where b.ID == sel.ID && s.ID == b.TypID
                          select new { b, s }).FirstOrDefault();
            cBookID = selBuch.b.ID;
            DetailPanel.DataContext = selBuch;
          
          
        }

        private void Click_ExitMnu(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Ablage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_ImgToGD_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Click_SettingMnu(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            AddEditBook ae = new AddEditBook(cBookID);
            ae.ShowDialog();
        }

        private void MenuItemTest_Click(object sender, RoutedEventArgs e)
        {
            AddBook ts = new AddBook("#");
            ts.ShowDialog();
        }

        private void AddDNB(object sender, RoutedEventArgs e)
        {
            SearchDNB ab = new SearchDNB();
            ab.ShowDialog();
        }

        private void MenuItemTestForm_Click(object sender, RoutedEventArgs e)
        {
            XTest xt = new XTest();
            xt.ShowDialog();
        }

        private void MenuItemTestDG_Click(object sender, RoutedEventArgs e)
        {
            XDGTest xdg = new XDGTest();
            xdg.ShowDialog();
        }
    }
}
