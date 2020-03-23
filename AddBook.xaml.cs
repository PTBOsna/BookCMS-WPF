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
//using System.Windows.Forms;
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
        public string sampleXml = @"H:\VisualStudio-Projekte\BookCMS-WPF\test5.mrcx"; //für Testlauf
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
            var language = from lg in Admin.conn.Language orderby lg.Language1 select new { lng = lg.Language1, iso = lg.iso, id = lg.LanguageID };
            cbLang.ItemsSource = language.ToList();
            var bind = from b in Admin.conn.Bindung orderby b.SortBy select new { bnd = b.SortBy, id = b.BindingID };
            cbBindung.ItemsSource = bind.ToList();
            var typ = from t in Admin.conn.BuchTyp orderby t.SortBy select new { typ = t.SortBy, id = t.ID };
            cbBuchTyp.ItemsSource = typ.ToList();
            var sach = from s in Admin.conn.Sachgruppe orderby s.SortBy select new { sg = s.SortBy, id = s.GenreID };
            cbSachgruppe.ItemsSource = sach.ToList();
            var sto = from st in Admin.conn.Standort orderby st.SortBy select new { so = st.SortBy, id = st.ID };
            cbStandort.ItemsSource = sto.ToList();
            var kat = from k in Admin.conn.Kategorie orderby k.SortBy select new { kat = k.SortBy, id = k.CategoryID };
            cbKategorie.ItemsSource = kat.ToList();
            var ukat = from uk in Admin.conn.Unterkategorie orderby uk.SortBy select new { ukat = uk.SortBy, id = uk.SubCategoryID };
            cbUKategorie.ItemsSource = ukat.ToList();
            GetDataDNB();

            //Personen finden und Anzeign
            string[] name = db.dnb_Autor_sort.Split(',');
            string[] ret = FindAutor(name[0]).Split('#');
            nr_list.Add(new NameRolle() { name = db.dnb_Autor_sort, rolle = db.dnb_Rolle, nameInDB = ret[1], currID = Int32.Parse(ret[0]), currRolleID = FindRolleID(db.dnb_Rolle) });
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
                    nr_list.Add(new NameRolle() { name = mitAutor[i], rolle = rolleMitAutor[i], nameInDB = retA[1], currID = Int32.Parse(retA[0]), currRolleID = FindRolleID(db.dnb_Rolle) });
                }

            }
            DGPersonen.ItemsSource = nr_list;
            FindPublisher(db.dnb_verlagsname);
            FindLanguage(db.dnb_sprache);
            FindOrigLanguage(db.dnb_sprache_org);


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
            txtVerlagsort.Text = db.dnb_verlagsort;
            string tmp = null;
            ; if (string.IsNullOrEmpty(db.dnb_begleit) == false)
            {
                tmp = "Begleitmaterial:" + "\r\n" + db.dnb_begleit;
            }

            if (string.IsNullOrEmpty(db.dnb_thema) == false)
            {
                tmp += "\r\n" + "Thema:" + "\r\n" + db.dnb_thema;
            }
            txtAddInfo.Text = tmp;
            txtInhalt.Text = Admin.GetSynopsis(db.dnb_inhalt);
            //newBook.LCCN = 
            newBook.OriginalTitel = db.dnb_titel_org;
            newBook.OriginalUntertitel = db.dnb_titel_org; //prüfen!
            txtPreis.Text = db.dnb_preis;
            txtSeiten.Text = db.dnb_seiten;
            txtStichworte.Text = db.dnb_stichwort;
            txtTitel.Text = db.dnb_titel;
            txtSubTitel.Text = db.dnb_untertitel;
            txtIndex.Text = db.dnb_index;
            //Berechnete Felder
            txtTitelIndex.Text = Admin. GetTitleIndex(db.dnb_titel);
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

          
            //Prüfen ob Cover vorhanden (Abfrage mit allen drei ISBN-Varianten)
            string imgUrl = @"https://portal.dnb.de/opac/mvb/cover.htm?isbn=" ;

            if (Admin.IsPageValid(imgUrl + db.dnb_isbn_) == true)
            {
                imgUrl += db.dnb_isbn_;
            }
            else if (Admin.IsPageValid(imgUrl + db.dnb_isbn) == true)
            {
                imgUrl += db.dnb_isbn;
            }
            else if (Admin.IsPageValid(imgUrl + db.dnb_isbn_13) == true)
            {
                imgUrl += db.dnb_isbn_13;
            }
            Uri myUri = new Uri(imgUrl);
            HandleImage(ImgBox, myUri);
            //MessageBox.Show(ImgBox.Width.ToString());
        }

       

      

        private void HandleImage(Image image, Uri webUri)
        {
            BitmapDecoder bDecoder = BitmapDecoder.Create(
              webUri,
              BitmapCreateOptions.PreservePixelFormat,
              BitmapCacheOption.None);

            if (bDecoder != null && bDecoder.Frames.Count > 0)
                image.Source = bDecoder.Frames[0];
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



        private void FindLanguage(string cLang)
        {
            var lang = (from lg in Admin.conn.Language where lg.iso == cLang select lg).FirstOrDefault();
            if (lang != null)
            {
                //MessageBox.Show(lang.iso);
                txtLanguage.Text = lang.Language1;
                newBook.SprachenID = lang.LanguageID;
            }
            else { txtLanguage.Text = "Keine Angeben"; }
        }
        private void FindOrigLanguage(string cLang)
        {
            var lang = (from lg in Admin.conn.Language where lg.iso == cLang select lg).FirstOrDefault();
            if (lang != null)
            {
                //MessageBox.Show(lang.iso);
                txtOrigLang.Text = lang.Language1;
                newBook.OriginalSpracheID = lang.LanguageID;
            }
            else { txtOrigLang.Text = "Keine Angeben"; }
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

        private Int32 FindRolleID(string _rolle)
        {
            var rolleID = (from ri in Admin.conn.AutorRolle where ri.AutorKurz == _rolle select ri).FirstOrDefault();
            if (rolleID != null)
            {
                return rolleID.ID;
            }
            return -1;
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

        //Sprache finden

        private void cbLang_DropDownClosed(object sender, EventArgs e)
        {
            if (cbLang.SelectedIndex != -1)
            {
                var selLng = (from sl in Admin.conn.Language where sl.LanguageID == (Int32)cbLang.SelectedValue select sl).FirstOrDefault();
                txtLanguage.Text = selLng.Language1;
                newBook.SprachenID = selLng.LanguageID;
            }
        }

        private void cbBindung_DropDownClosed(object sender, EventArgs e)
        {
            if (cbBindung.SelectedIndex != -1)
            {
                var bind = (from b in Admin.conn.Bindung where b.BindingID == (Int32)cbBindung.SelectedValue select b).FirstOrDefault();
                txtBindung.Text = bind.Bindung1;
                newBook.BindungID = bind.BindingID;
            }
        }
        private void cbBuchTyp_DropDownClosed(object sender, EventArgs e)
        {
            if (cbBuchTyp.SelectedIndex != -1)
            {
                var typ = (from t in Admin.conn.BuchTyp where t.ID == (Int32)cbBuchTyp.SelectedValue select t).FirstOrDefault();
                txtTyp.Text = typ.SortBy;
                newBook.TypID = typ.ID;
            }

        }
        private void cbSachgruppe_DropDownClosed(object sender, EventArgs e)
        {
            if (cbSachgruppe.SelectedIndex != -1)
            {
                var sachg = (from s in Admin.conn.Sachgruppe where s.GenreID == (Int32)cbSachgruppe.SelectedValue select s).FirstOrDefault();
                txtSachgruppe.Text = sachg.SortBy;
                newBook.SachgruppeID = sachg.GenreID;
            }

        }
        private void cbKategorie_DropDownClosed(object sender, EventArgs e)
        {
            if (cbKategorie.SelectedIndex != -1)
            {
                var kate = (from k in Admin.conn.Kategorie where k.CategoryID == (Int32)cbKategorie.SelectedValue select k).FirstOrDefault();
                txtKategorie.Text = kate.SortBy;
                newBook.KategorieID = kate.CategoryID;
            }
        }
        private void cbUKategorie_DropDownClosed(object sender, EventArgs e)
        {
            if (cbUKategorie.SelectedIndex != -1)
            {
                var ukate = (from k in Admin.conn.Unterkategorie where k.SubCategoryID == (Int32)cbUKategorie.SelectedValue select k).FirstOrDefault();
                txtUKategorie.Text = ukate.SortBy;
                newBook.UnterkategorieID = ukate.SubCategoryID;
            }
        }
        private void cbStandort_DropDownClosed(object sender, EventArgs e)
        {
            if (cbStandort.SelectedIndex != -1)
            {
                var standort = (from s in Admin.conn.Standort where s.ID == (Int32)cbStandort.SelectedValue select s).FirstOrDefault();
                txtStandort.Text = standort.SortBy;
                newBook.StandortID = standort.ID;
            }

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
            newBook.VerlagsID = dnbVerlag.verlID;
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
            newBook.DruckereiID = dnbPrinter.drlID;
        }



        public void SaveNewBook()
        {
            
            if (string.IsNullOrEmpty(txtTitel.Text) == true)
            {
                MessageBox.Show("Bitte einen Titel eingeben!");
                return;
            }
            else if (string.IsNullOrEmpty(txtSignatur.Text) == true)
            {
                MessageBox.Show("Signatur fehlt!");
                return;
            }
            else if (DGVerlag.Items.Count > 0 && newBook.VerlagsID == null)
            {
                MessageBox.Show("Bitte einen Verlag auswählen!");
                return;
            }
            else if (DGDruckerei.Items.Count > 0 && newBook.DruckereiID == null)
            {
                MessageBox.Show("Bitte eine Druckerei auswählen!");
                return;
            }
            else if (string.IsNullOrEmpty(txtBindung.Text) == true)
            {
                MessageBox.Show("Bitte Bindung auswählen!");
                return;
            }
            else if (string.IsNullOrEmpty(txtTyp.Text) == true)
            {
                MessageBox.Show("Bitte Buchtyp auswählen!");
                return;
            }
            if (string.IsNullOrEmpty(txtStandort.Text) == true)
            {
                MessageBox.Show("Bitte Standort auswählen!");
                return;
            }

    newBook.Titel = txtTitel.Text;
            newBook.TitelIndex = txtTitelIndex.Text;

            newBook.AutorSort = txtAutorSort.Text;
            newBook.Signatur = txtSignatur.Text;
            newBook.Veroeffentlicht = txtJahr.Text;
            newBook.CopyrightDatum = txtJahr.Text;

            newBook.Untertitel = txtSubTitel.Text;
            newBook.TitleSort = txtTitelSort.Text;

            newBook.ISBN = txtISBN.Text;
            newBook.DNB = txtDNB.Text;
            newBook.DDC = txtDCC.Text;
            //newBook.LCCN =  
            //newBook.LCCallNum =  [nvarchar = (50) NULL,
            //newBook.LandD = txtLanguage.Text;
            //newBook.SprachenID =  [int = NULL,
            //newBook.DruckereiID =  [int = NULL,
            //newBook.BindungID =  [int = NULL,
            //newBook.AuflageID =  [int = NULL,
            newBook.Auiflage = txtAuflage.Text;
            //newBook.DruckID =  [int = NULL,
            //newBook.SerienID =  [int = NULL,
            newBook.Seiten = txtSeiten.Text;
            //newBook.Abschnitte =  [smallint = NULL,
            newBook.OriginalTitel = txtOrigTitel.Text;
            //newBook.OriginalUntertitel = txt
            //newBook.OriginaVerlagID =  [int = NULL,
            //newBook.OriginalLandID =  [int = NULL,
            //newBook.OriginalSpracheID =  [int = NULL,
            //newBook.OriginalCopyright =  [nvarchar = (8) NULL,
            //newBook.Preisangabe =  [nvarchar = (255) NULL,
            //newBook.Value =  [nvarchar = (255) NULL,
            newBook.Preis = txtPreis.Text;
            //newBook.ZustandID =  [int = NULL,
            //newBook.GutachterID =  [int = NULL,
            //newBook.Versicherung =  [int = NULL,
            //newBook.Registeriert =  [nvarchar = (8) NULL,
            //newBook.StatusID =  [int = NULL,
            //newBook.Erworben =  [nvarchar = (8) NULL,
            //newBook.ErworbenVonID =  [int = NULL,
            //newBook.PersonalRatingID =  [int = NULL,
            //newBook.BesitzerID =  [int = NULL,
            //newBook.StandortID =  [int = NULL,
            //newBook.EntleiherID =  [int = NULL,
            //newBook.DatumAusleihe =  [nvarchar = (8) NULL,
            //newBook.RueckgabeDatum =  [nvarchar = (8) NULL,
            //newBook
            //        newBook.Anmerkungen_PlainText = txtAddInfo.Text;
            //        newBook.Synopsis_PlainText = txtIndex.Text;
            //newBook.Reviews_PlainText =  [nvarchar = (max)NULL,
            //newBook.BarCode =  [nvarchar = (50) NULL,
            //newBook.OriginalSerieID =  [int = NULL,
            //newBook.zuletztGelesen =  [nvarchar = (8) NULL,
            //newBook.AnzahlGelesen =  [smallint = NULL,
            //newBook.ZustandSchutzumschlagID =  [int = NULL,
            newBook.Dim_Width = txtDim.Text;
            //newBook.Dim_Height =  [nvarchar = (255) NULL,
            //newBook.Dim_Depth =  [nvarchar = (255) NULL,
            //newBook.Verkaufspreis =  [nvarchar = (255) NULL,
            //newBook.WaehrungID =  [int = NULL,
            //newBook.VerlagsOrtID =  [int = NULL,
            //newBook.ASIN =  [nvarchar = (20) NULL,
            //newBook.LetzteAenderung =  [smalldatetime = NULL,
            //newBook.FreigabeNr =  [nvarchar = (255) NULL,
            //newBook.OriginalFreigabeNr =  [nvarchar = (255) NULL,
            //newBook.KategorieID =  [int = NULL,
            //newBook.UnterkategorieID =  [int = NULL,
            //newBook.SachgruppeID =  [int = NULL,
            newBook.Stichworte = txtStichworte.Text;

            //speichern!
            Admin.conn.Buch.InsertOnSubmit(newBook);
            Admin.conn.SubmitChanges();
            Int32 id = newBook.ID;
            // AutorBuchLink ergänzen
            AutorBuchLink abl = new AutorBuchLink();
            // gegeben ist die liste nr_list
            foreach (var autor in nr_list)
            {
                abl.BuchID = id;
                abl.PersonID = autor.currID;
                abl.RolleID = autor.currRolleID;
            }

        }

        private void BtnSave_click(object sender, RoutedEventArgs e)
        {
            SaveNewBook();
        }

        private void MeClose()
        {
           DialogResult = true;
        }

    }
}

