using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookCMS_WPF.DataHandling;
using System.Data.SqlClient;

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
        //public    mySettings ms = new mySettings();
        public MainWindow()
        {
            InitializeComponent();
            try
            {

                //conn.Open();
                //BuchDataClassesDataContext con = new BuchDataClassesDataContext(@"Server = .\SQLEXPRESS; Database = BOOKS_FROM_ACCESS.DBF; Trusted_Connection = True;");
                Admin.CheckDatabaseExists(@"Server = .\SQLEXPRESS; Database = BooksCMS_TestDB; Trusted_Connection = True;", "BooksCMS_TestDB");
                MessageBox.Show("Verbindung mit Datenbank hergestellt");


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            mySettings.loadSetting();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            selDilplay = "Titel";
            LoadBooks();


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
            ComboBoxPersonen.SelectedValue = mySettings.StarRolle; //wird später über Settings voreingestellt
        }

        private void LoadBooks()

        {

            var buch = from b in Admin.conn.Buch select b;
            BuchGrid.ItemsSource = buch.ToList();
        }

        private void LoadPersonTV()
        {

            foreach (var letter in Admin.myAlpha)
            {
                //create a new Item
                var item = new TreeViewItem();
                item.FontWeight = FontWeights.Bold;
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
                var buch = from b in Admin.conn.Buch where b.Titel.Contains(txtSuche.Text) select b;
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
            ImgBox.Source = null;
            Buch sel = BuchGrid.SelectedItem as Buch;
            if (sel == null)
            {
                return;
            }
            var selBuch = (from b in Admin.conn.Buch
                           from s in Admin.conn.BuchTyp
                           where b.ID == sel.ID && s.ID == b.TypID
                           select new { b, s }).FirstOrDefault();
            cBookID = selBuch.b.ID;
            DetailPanel.DataContext = selBuch;
            //ggf. Image laden
            string cPath = System.IO.Path.Combine(mySettings.CoverPath, cBookID.ToString() + ".jpg");
            Admin.ShowImage(ImgBox, cPath);

        }

        private void Click_ExitMnu(object sender, RoutedEventArgs e)
        {
            //Admin.conn.SubmitChanges();
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
            if (cBookID==0)
            {
                MessageBox.Show("Bitte zunächst ein Buch auswählen!");
                return;
            }
            AddEditBook ae = new AddEditBook(cBookID);
            ae.ShowDialog();
        }

        private void MenuItemTest_Click(object sender, RoutedEventArgs e)
        {
            AddBook ts = new AddBook("#");
            ts.Show();
        }

        private void AddDNB(object sender, RoutedEventArgs e)
        {
            SearchDNB ab = new SearchDNB(true, null);
            ab.ShowDialog();
           
        }

        private void MenuItemTestForm_Click(object sender, RoutedEventArgs e)
        {
            XTest xt = new XTest();
            xt.Show();
        }

        private void MenuItemTestDG_Click(object sender, RoutedEventArgs e)
        {
            XDGTest xdg = new XDGTest();
            xdg.ShowDialog();
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            MySettings ms = new MySettings();
            ms.ShowDialog();
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in this.PersonTrv.Items)
            {
                TreeViewItem treeItem = this.PersonTrv.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem != null)
                    ExpandAll(treeItem, true);
                treeItem.IsExpanded = true;
            }
        }

        private void btnCollaps_Click(object sender, RoutedEventArgs e)
        {
            foreach (object item in this.PersonTrv.Items)
            {
                TreeViewItem treeItem = this.PersonTrv.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem != null)
                    ExpandAll(treeItem, false);
                treeItem.IsExpanded = false;
            }
        }
        private void ExpandAll(ItemsControl items, bool expand)
        {
            foreach (object obj in items.Items)
            {
                ItemsControl childControl = items.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;
                if (childControl != null)
                {
                    ExpandAll(childControl, expand);
                }
                TreeViewItem item = childControl as TreeViewItem;
                if (item != null)
                    item.IsExpanded = true;
            }
        }

        private void DelBook(object sender, RoutedEventArgs e)

        {
            Buch sel = BuchGrid.SelectedItem as Buch;
            if (sel == null)
            {
                MessageBox.Show("Bitte zunächst einen Titel auswählen!");
                return;
            }
            var erg = MessageBox.Show("Möchten Sie das Buch '" + sel.Titel + "' wirklich löschen?", "Buch löschen!", MessageBoxButton.YesNo);
            if (erg == MessageBoxResult.Yes)
            {

            }
            if (DeleteBook.delBook(cBookID))
            {
                LoadBooks();

                MessageBox.Show("Buch wurde gelöscht.");
            }

        }

        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            txtSuche.Text = null;
            LoadBooks();
        }

        private void btnEditDNBClick(object sender, RoutedEventArgs e)
        {
            if (cBookID == 0)
            {
                MessageBox.Show("Bitte zunächst ein Buch auswählen!");
                return;
            }
            EditBookDNB eb = new EditBookDNB(cBookID);
            eb.ShowDialog();
        }

        private void btnEditClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("noch nicht implementiert!");
        }

        private void MenuItemDDC1000_Click(object sender, RoutedEventArgs e)
        {
            Listen.DDC1000_Liste ddcList = new Listen.DDC1000_Liste();
            ddcList.ShowDialog();
        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {

            AddEditBook ae = new AddEditBook(cBookID);
            ae.ShowDialog();
          
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemStandorte_click(object sender, RoutedEventArgs e)
        {
            Listen.Standorte st = new Listen.Standorte();
            st.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Listen.Personen ps = new Listen.Personen();
            ps.ShowDialog();
        }
    }
}
