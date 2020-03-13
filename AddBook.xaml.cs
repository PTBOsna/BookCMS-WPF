using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaktionslogik für AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        //Variablen für IDs der verknüpften Felder
        public Int32 PersonID;
        public Int32 VerlagsID;
        public Int32 VerlagsOrtID;

        //Variablen für DNB-Connect
        public string sampleXml = @"H:\VisualStudio-Projekte\ReadXML\ReadXML\test3.mrcx"; //für Testlauf
        public string dnbID;
        public DNBBookData db;
        public AddBook(string _dnbID)
        {
            InitializeComponent();
            string s;
            this.dnbID = _dnbID;
            string key = "1159cfc6b965e8a03abc3bd8227afa"; //TODO: wird später aus den Settings entnommen
            WebClient w = new WebClient();
            w.Encoding = Encoding.UTF8;
            if (dnbID == "#")
            { //für Testlauf
                s = File.ReadAllText(sampleXml);
            }
            else
            {

                string urlEnc = WebUtility.UrlEncode(dnbID);
                s = w.DownloadString("https://services.dnb.de/sru/dnb?version=1.1&operation=searchRetrieve&query=" + urlEnc + "&recordSchema=MARC21-xml&accessToken=" + key);
            }
            db = new DNBBookData(s);
            this.DataContext = db;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)

        {


            if (db.dnb_Autor_sort != null)
            {
                string[] name = db.dnb_Autor_sort.Split(',');
                if (FindAutor(name[0]) != true)

                { ListAllPersons(); }
            }
            else
                ListAllPersons();

            string erg = CheckData.CheckISBN(db.dnb_isbn);

            if (erg != null)
            {
                MessageBox.Show(erg, "Titel bereits vorhanden!");
                return;
            }

            erg = CheckData.CheckTitel(db.dnb_titel);
            if (erg != null)
            {
                MessageBox.Show(erg, "Titel bereits vorhanden!");
                return;
            }

        }

        private bool FindAutor(string cName)
        {
            var per = from p in Admin.conn.Person where p.Name.Contains(cName) select p; //new { id = p.PersonID, Name = p.SortBy };


            if (per.Count() > 0)
            {
                cbName.Items.Clear();
                cbName.ItemsSource = per.ToList();
                cbName.Focus();
                cbName.SelectedIndex = 1;
                return true;
            };
            return false;
        }



        private void ListAllPersons()
        {
            var per_all = from p in Admin.conn.Person orderby p.SortBy select p; // new { id = p.PersonID, Name = p.SortBy };
            if (per_all.Count() > 0)
            {

                cbName.ItemsSource = per_all.ToList();
                lbName.ItemsSource = per_all.ToList();
                //cbName.Focus();
                //cbName.SelectedIndex = 1;


            }
        }

        private void cbName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            PersonID = (Int32)cbName.SelectedValue;

            var pers = (from p in Admin.conn.Person where p.PersonID == (Int32)cbName.SelectedValue select p.SortBy).FirstOrDefault();
            txtAutor.Text = pers;
            MessageBox.Show(cbName.SelectedValue.ToString());
        }
    }
}

