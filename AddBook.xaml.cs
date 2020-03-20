using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string cSignatur;
        //Variablen für DNB-Connect
        public string sampleXml = @"H:\VisualStudio-Projekte\ReadXML\ReadXML\test3.mrcx"; //für Testlauf
        public string dnbID;
        public DNBBookData db;
        //public List<NameRolle> nr_list;
        public List<NameRolle> nr_list;
        public List<DnbVerlag> verl_list;
        public List<DnbPrinter> print_list;
        public Buch newBook = new Buch();


        public AddBook(string _dnbID)
        {
            InitializeComponent();
            this.dnbID = _dnbID;
            //this.DataContext = newBook;
            //Hilfstabellen laden/aktivieren
            LoadAuxTab();
            btnPublNew.Visibility = Visibility.Hidden;
            NameRolle nr = new NameRolle();
            nr_list = new List<NameRolle>();
            verl_list = new List<DnbVerlag>();
            print_list = new List<DnbPrinter>();


            GetDataDNB();
            //Personen finden und Anzeign
            string[] name = db.dnb_Autor_sort.Split(',');
            string[] ret = FindAutor(name[0]).Split('#');
            nr_list.Add(new NameRolle() { name = db.dnb_Autor_sort, rolle = db.dnb_Rolle, nameInDB = ret[1], currID = Int32.Parse(ret[0]) });
            if (db.dnb_mitautor != null)
            {
                db.dnb_mitautor = db.dnb_mitautor.Substring(0, db.dnb_mitautor.Length - 1);
                db.dnb_mitautor_rolle = db.dnb_mitautor_rolle.Substring(0, db.dnb_mitautor_rolle.Length - 1);
                List<string> mitAutor = db.dnb_mitautor.Split(';').ToList();
                List<string> rolleMitAutor = db.dnb_mitautor_rolle.Split(';').ToList();
                for (int i = 0; i < mitAutor.Count; i++)
                {
                    name = mitAutor[i].Split(',');
                    string[] retA = FindAutor(name[0]).Split('#');
                    nr_list.Add(new NameRolle() { name = mitAutor[i], rolle = rolleMitAutor[i], nameInDB = retA[1], currID = Int32.Parse(retA[0]) });
                }

            }
            DGPersonen.ItemsSource = nr_list;
            FindPublisher(db.dnb_verlagsname);
            FindLanguage(db.dnb_sprache);
            FindLanguage(db.dnb_sprache_org);

          
            LoadNewData();

           
        }

        private void LoadNewData()
        {
            //Einfache Felder/ TextFelder von newBook mit Daten aus db belegel
           txtAuflage.Text = db.dnb_auflage;
            txtJahr.Text = db.dnb_jahr;
            txtDCC.Text = db.dnb_dcc1 + "; " + db.dnb_dcc2;
            txtDim.Text = db.dnb_dim;
            txtDNB.Text = db.dnb_nr;
            txtISBN.Text = db.dnb_isbn;
            //newBook.LCCN = 
            newBook.OriginalTitel = db.dnb_titel_org;
            newBook.OriginalUntertitel = db.dnb_titel_org; //prüfen!
            txtPreis.Text = db.dnb_preis;
            txtSeiten.Text = db.dnb_seiten;
            txtStichworte.Text = db.dnb_stichwort;
           txtTitel.Text = db.dnb_titel;
            txtSubTitel.Text = db.dnb_untertitel;
            //Berechnete Felder
            txtTitelIndex.Text = GetTitleIndex(db.dnb_titel);
            txtAutorSort.Text = GetAutorSort();
            txtTitelSort.Text = db.dnb_titel;
            txtSignatur.Text = GetSignatur(); 
            //und DataContext füllen
            //this.DataContext = newBook;
        }

        public void GetDataDNB()
        {
            string s;

            string key = "1159cfc6b965e8a03abc3bd8227afa"; //TODO: wird später aus den Settings entnommen
            WebClient w = new WebClient();
            w.Encoding = Encoding.UTF8;

            //MessageBox.Show(cSignatur);
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
        }

        private string GetTitleIndex(string ti)
        {
            int iLen = 20;
            string titelIndex = ti.Trim().ToUpper();
            titelIndex = Admin.CleanString(titelIndex);
            if (titelIndex.Length < 20)
            {
                iLen = titelIndex.Length;
            }
            titelIndex = titelIndex.Substring(0, iLen);
            return titelIndex;
        }
        private string GetSignatur()
        {
            // Signatur
            string lastSign = (from x in Admin.conn.Buch select x).Max(c => c.Signatur);
            int ls = Int32.Parse(lastSign) + 1;
            cSignatur = ls.ToString("00000");
            return cSignatur;
        }
        //Titelindex
        private string GetAutorSort()
        {
            string autorsort = null;
            foreach (var person in nr_list)
            {
                autorsort += person.name + "; ";
            }
           
            return autorsort.Substring(0, autorsort.Length - 2);
        }


        private void FillFields()
        {

        }
        private void FindLanguage(string cLang)
        {
            var lang = (from lg in Admin.conn.Language where lg.iso == cLang select lg).FirstOrDefault();
            if (lang != null)
            {
                //MessageBox.Show(lang.iso);
                txtLanguage.Text = lang.Language1;
            }
            else {txtLanguage.Text ="Keine Angeben"; }
        }
        private void LoadAuxTab()
        {
            var Verlag = from v in Admin.conn.Verlag select v;
        }

        private string FindAutor(string cName)
        {
            var per = (from p in Admin.conn.Person where p.Name.Contains(cName) select p).FirstOrDefault(); //new { id = p.PersonID, Name = p.SortBy };
            string erg;
            if (per != null)
            {
                erg = per.PersonID.ToString() + "#" + per.SortBy;
                return erg;
            }
            else
            {
                erg = "-1#" + "Name nicht vorhanden";
                return erg;
            }
        }
        //Verlasg finden
        private void FindPublisher(string cVerlag)
        {
            var ver = (from v in Admin.conn.Verlag where v.Verlag1.Contains(cVerlag) select v).ToList(); //new { id = p.PersonID, Name = p.SortBy };
            string erg;
            if (ver.Count > 0)
            {
                foreach (var pub in ver)
                {
                    verl_list.Add(new DnbVerlag() { verlag = cVerlag, verlagInDB = pub.SortBy, verlID = pub.PublisherID });
                }
            }
            else
            {
                verl_list.Add(new DnbVerlag() { verlag = cVerlag, verlagInDB = "nicht in DB", verlID = -1 });
                btnPublNew.Visibility = Visibility.Visible;
            }
            DGVerlag.ItemsSource = verl_list;
        }

        //Druckerei finden
        private void FindPrinter(string cPrinter)
        {
            var pr = (from d in Admin.conn.Druckerei where d.SortBy.Contains(cPrinter) select d).ToList(); //new { id = p.PersonID, Name = p.SortBy };
            string erg;
            if (pr.Count > 0)
            {
                foreach (var pub in pr)
                {
                    print_list.Add(new DnbPrinter() { druckerei = cPrinter, druckereiInDB = pub.Druckerei1, drlID = pub.PrintedByID });
                }
            }
            else
            {
                print_list.Add(new DnbPrinter() { druckerei = cPrinter, druckereiInDB = "nicht in DB", drlID = -1 });
                btnPublNew.Visibility = Visibility.Visible;
            }
            DGDruckerei.ItemsSource = verl_list;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NameRolle nr = DGPersonen.SelectedItem as NameRolle;
                if (nr.currID == -1)
                {
                    MessageBox.Show("Form AddName aufrufen");
                }
                else
                {
                    EditNameRolle enr = new EditNameRolle(nr);
                    enr.ShowDialog();
                    DGPersonen.ItemsSource = nr_list;

                }
                txtAutorSort.Text = GetAutorSort();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DG_Publischer_changed(object sender, SelectedCellsChangedEventArgs e)
        {
            DnbVerlag dnbVerlag = DGVerlag.SelectedItem as DnbVerlag;
            if (dnbVerlag.verlID == -1)
            {
                btnPublNew.Visibility = Visibility.Visible;
            }

            lblVerlag.Content = "Verlag : " + dnbVerlag.verlagInDB;

        }



        private void BtnPublischerNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Form AddVerlag aufrufen");
        }

        private void BtnPrinterNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Form AddPrinter aufrufen");
        }

        private void DG_Printer_changed(object sender, SelectedCellsChangedEventArgs e)
        {
            DnbPrinter dnbPrinter = DGDruckerei.SelectedItem as DnbPrinter;
            if (dnbPrinter.drlID == -1)
            {
                btnPublNew.Visibility = Visibility.Visible;
            }

            lblPrinter.Content = "Drukerei : " + dnbPrinter.druckereiInDB;
        }

        public void SaveNewBook()
        {
           
        }
    }
}

