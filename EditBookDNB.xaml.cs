﻿using BookCMS_WPF.DataHandling;
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

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für EditBookDNB.xaml
    /// </summary>
    public partial class EditBookDNB : Window
    {
        public Buch cBook;
        Int32 bookID;
        string cKat;
        string DNBSuchString;
        DNBBookData cDNBBook;
        List<string> personen = new List<string>();
        List<NameRolle> cNameRolle = new List<NameRolle>();
        mySettings ms = new mySettings();
        public EditBookDNB(Int32 _bookID)
        {
            InitializeComponent();
            this.bookID = _bookID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Buch laden
            if (bookID < 0)
            {
                MessageBox.Show("Kein Buch ausgewählt!");
                return;
            }
            cBook = (from b in Admin.conn.Buch
                     where b.ID == bookID
                     select b).FirstOrDefault();
            //Zunächst prüfen, ob ISBN oder DNB-Id vorhanden ist
            if (cBook.DNB != null)
            {
                DNBSuchString = cBook.DNB;
            }
            else if (cBook.ISBN != null)
            {
                DNBSuchString = cBook.ISBN;
            }
            else
            {
                MessageBox.Show("Bitte zunächst eine gültige ISBN ermitteln ");
                this.Close();
            }
            //DNB-Daten laden
            cDNBBook = DNBDataHandling.GetDataDNB(DNBSuchString);
            if (cDNBBook.dnb_nr==null)
            {
                MessageBox.Show("Kein Datensatz in der DNB vorhanden!" + "\r\n" + "Bitte gültige ISBN in der DNB suchen!");
            }
            ShowDNB_Book();
            //ggf. Image laden
            string cPath = System.IO.Path.Combine(mySettings.CoverPath, bookID.ToString() + ".jpg");
            Admin.ShowImage(ImgBox, cPath);
            //Personen laden
            var erg = (from abl in Admin.conn.AutorBuchLink
                       from pers in Admin.conn.Person
                       from ro in Admin.conn.AutorRolle
                       where abl.BuchID == cBook.ID && pers.PersonID == abl.PersonID && ro.ID == abl.RolleID
                       orderby pers.SortBy
                       select new { Person = pers.SortBy, Rolle = ro.AutorRolle1, PerID = pers.PersonID, RolleID = ro.ID, AutorRolleID = abl.id }).ToList();
            foreach (var el in erg)
            {
                //personen.Add(el.Person + " (" + el.Rolle + ")");
                cNameRolle.Add(new NameRolle() { name = el.Person + " (" + el.Rolle + ")", currID = el.PerID, rolle = el.Rolle, currRolleID = el.RolleID, currNameRolleID = el.AutorRolleID });
            }
            lbPersonen.ItemsSource = cNameRolle;
            lbPersonen.DataContext = cNameRolle;
            this.DataContext = cBook;

            //Genre Laden
            Admin.LoadGenre(ugridGenre);
            var c_genre = from g in Admin.conn.GenreLink where g.BuchID == bookID select g;
            foreach (var book in c_genre)
            {
                foreach (CheckBox item in ugridGenre.Children)
                {
                    if (book.SachgruppeID == (int)item.Tag)
                    {
                        item.IsChecked = true;
                        item.FontWeight = FontWeights.Bold;
                    }
                }
            }

            //Verknüpfungen laden
            var _verlag = (from v in Admin.conn.Verlag where v.PublisherID == cBook.VerlagsID select v.SortBy).FirstOrDefault();
            txtVerlag.Text = _verlag;
            var _verlagsOrt = (from v in Admin.conn.VerlagsOrt where v.ID == cBook.VerlagsOrtID select v.SortBy).FirstOrDefault();
            txtVerlagsOrt.Text = _verlagsOrt;
            var _druckerei = (from d in Admin.conn.Druckerei where d.PrintedByID == cBook.DruckereiID select d.SortBy).FirstOrDefault();
            txtDruckerei.Text = _druckerei;
            var _bindung = (from b in Admin.conn.Bindung where b.BindingID == cBook.BindungID select b).FirstOrDefault();
            if (_bindung != null)
            {
                txtBindung.Text = _bindung.Bindung1;
            }
            var _language = (from l in Admin.conn.Language where l.LanguageID == cBook.SprachenID select l).FirstOrDefault();
            if (_language != null)
            {

                txtLanguage.Text = _language.Language1;
            }
            var _typ = (from t in Admin.conn.BuchTyp where t.ID == cBook.TypID select t).FirstOrDefault();
            if (_typ != null)
            {

                txtTyp.Text = _typ.BuchTyp1;
            }
            var _kat = (from k in Admin.conn.DDC_Haupt where k.ID == cBook.KategorieID select k).FirstOrDefault();
            if (_kat != null)
            {
                txtKategorie.Text = _kat.DDC_Name;

            }
            var _uKat = (from u in Admin.conn.DDC_1000 where u.ID == cBook.UnterkategorieID select u).FirstOrDefault();
            if (_uKat != null)
            {

                txtUKategorie.Text = _uKat.DDC_Name;
            }
            var _sachgruppe = (from s in Admin.conn.Sachgruppe where s.GenreID == cBook.SachgruppeID select s).FirstOrDefault();
            if (_sachgruppe != null)
            {

                txtSachgruppe.Text = _sachgruppe.Sachgruppe1;
            }
            var _standort = (from st in Admin.conn.Standort where st.ID == cBook.StandortID select st).FirstOrDefault();
            if (_standort != null)
            {

                txtStandort.Text = _standort.SortBy;
            }

            //Auswahlboxen laden
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
            var kat = from k in Admin.conn.DDC_Haupt orderby k.DDC select new { kat = k.DDC_Name, id = k.ID };
            cbKategorie.ItemsSource = kat.ToList();
            var ukat = from uk in Admin.conn.DDC_1000 orderby uk.DDC_Name select new { ukat = uk.DDC_Name, id = uk.ID };
            cbUKategorie.ItemsSource = ukat.ToList();
            var verlag = from v in Admin.conn.Verlag orderby v.SortBy select new { verl = v.SortBy, id = v.PublisherID };
            cbVerlag.ItemsSource = verlag.ToList();
            var verlagOrt = from vo in Admin.conn.VerlagsOrt orderby vo.SortBy select new { verlort = vo.SortBy, id = vo.ID };
            cbVerlagsOrt.ItemsSource = verlagOrt.ToList();
            var druck = from d in Admin.conn.Druckerei orderby d.SortBy select new { druck = d.SortBy, id = d.PrintedByID };
            cbDruckerei.ItemsSource = druck.ToList();
        }

        private void ShowDNB_Book()
        {
            txtTitelDNB.Text = cDNBBook.dnb_titel;
            txtSubTitelDNB.Text = cDNBBook.dnb_untertitel;
            txtVerlagDNB.Text = cDNBBook.dnb_verlagsname;
            txtVerlagsOrtDNB.Text = cDNBBook.dnb_verlagsort;
            txtDruckereiDNB.Text = "Kein DNB-Feld";
            txtAuflageDNB.Text = cDNBBook.dnb_auflage;
            txtDimDNB.Text = cDNBBook.dnb_dim;
            txtISBNDNB.Text = cDNBBook.dnb_isbn;
            txtJahrDNB.Text = cDNBBook.dnb_jahr;
            txtLanguageDNB.Text = cDNBBook.dnb_sprache;
            txtPreisDNB.Text = cDNBBook.dnb_preis;
            txtSeitenDNB.Text = cDNBBook.dnb_seiten;
            txtVerlagDNB.Text = cDNBBook.dnb_verlagsname;
            txtVerlagsOrtDNB.Text = cDNBBook.dnb_verlagsort;
            string tmp = null;
            if (string.IsNullOrEmpty(cDNBBook.dnb_begleit) == false)
            {
                tmp = "Begleitmaterial:" + "\r\n" + cDNBBook.dnb_begleit;
            }

            if (string.IsNullOrEmpty(cDNBBook.dnb_thema) == false)
            {
                tmp += "\r\n" + "Thema:" + "\r\n" + cDNBBook.dnb_thema;
            }
            txtAddInfoDNB.Text = tmp;
            if (cDNBBook.dnb_inhalt != null)
            {
                txtInhaltDNB.Text = Admin.GetSynopsis(cDNBBook.dnb_inhalt);

            }

        }

        private void cbLang_DropDownClosed(object sender, EventArgs e)
        {
            if (cbLang.SelectedIndex != -1)
            {
                var selLng = (from sl in Admin.conn.Language where sl.LanguageID == (Int32)cbLang.SelectedValue select sl).FirstOrDefault();
                txtLanguage.Text = selLng.Language1;
                cBook.SprachenID = selLng.LanguageID;
            }
        }

        private void cbBindung_DropDownClosed(object sender, EventArgs e)
        {
            if (cbBindung.SelectedIndex != -1)
            {
                var bind = (from b in Admin.conn.Bindung where b.BindingID == (Int32)cbBindung.SelectedValue select b).FirstOrDefault();
                txtBindung.Text = bind.Bindung1;
                cBook.BindungID = bind.BindingID;
            }
        }
        private void cbBuchTyp_DropDownClosed(object sender, EventArgs e)
        {
            if (cbBuchTyp.SelectedIndex != -1)
            {
                var typ = (from t in Admin.conn.BuchTyp where t.ID == (Int32)cbBuchTyp.SelectedValue select t).FirstOrDefault();
                txtTyp.Text = typ.SortBy;
                cBook.TypID = typ.ID;
            }

        }
        private void cbSachgruppe_DropDownClosed(object sender, EventArgs e)
        {
            if (cbSachgruppe.SelectedIndex != -1)
            {
                var sachg = (from s in Admin.conn.Sachgruppe where s.GenreID == (Int32)cbSachgruppe.SelectedValue select s).FirstOrDefault();
                txtSachgruppe.Text = sachg.SortBy;
                cBook.SachgruppeID = sachg.GenreID;
            }

        }
        private void cbKategorie_DropDownClosed(object sender, EventArgs e)
        {
            if (cbKategorie.SelectedIndex != -1)
            {
                var kate = (from k in Admin.conn.DDC_Haupt where k.ID == (Int32)cbKategorie.SelectedValue select k).FirstOrDefault();
                txtKategorie.Text = kate.DDC_Name;
                cBook.KategorieID = kate.ID;
                cKat = kate.DDC;

            }
        }
        private void cbUKategorie_DropDownOpened(object sender, EventArgs e)
        {
            if (cKat==null)
            {
                var kate = (from k in Admin.conn.DDC_Haupt where k.ID == cBook.KategorieID select k).FirstOrDefault();
                cKat =kate.DDC;
            }
            var ukate = from k in Admin.conn.DDC_1000 where k.DDC.StartsWith(cKat.Substring(0, 1)) select new { ukat = k.DDC + " - " + k.DDC_Name, id = k.ID };
            cbUKategorie.ItemsSource = ukate;
        }
        private void cbUKategorie_DropDownClosed(object sender, EventArgs e)
        {
            if (cbUKategorie.SelectedIndex != -1)
            {
                var ukate = (from k in Admin.conn.DDC_1000 where k.ID == (Int32)cbUKategorie.SelectedValue select k).FirstOrDefault();
                txtUKategorie.Text = ukate.DDC_Name;
                cBook.UnterkategorieID = ukate.ID;
            }
        }
        private void cbStandort_DropDownClosed(object sender, EventArgs e)
        {
            if (cbStandort.SelectedIndex != -1)
            {
                var standort = (from s in Admin.conn.Standort where s.ID == (Int32)cbStandort.SelectedValue select s).FirstOrDefault();
                txtStandort.Text = standort.SortBy;
                cBook.StandortID = standort.ID;
            }

        }

        private void cbDruckerei_DropDownClosed(object sender, EventArgs e)
        {
            if (cbDruckerei.SelectedIndex != -1)
            {
                var druck = (from d in Admin.conn.Druckerei where d.PrintedByID == (Int32)cbDruckerei.SelectedValue select d).FirstOrDefault();
                txtDruckerei.Text = druck.SortBy;
                cBook.DruckereiID = druck.PrintedByID;
            }
        }

        private void cbVerlagsOrt_DropDownClosed(object sender, EventArgs e)
        {
            if (cbVerlagsOrt.SelectedIndex != -1)
            {
                var vlgOrt = (from v in Admin.conn.VerlagsOrt where v.ID == (Int32)cbVerlagsOrt.SelectedValue select v).FirstOrDefault();
                txtVerlagsOrt.Text = vlgOrt.SortBy;
                cBook.VerlagsOrtID = vlgOrt.ID;
            }
        }

        private void cbVerlag_DropDownClosed(object sender, EventArgs e)
        {
            if (cbVerlag.SelectedIndex != -1)
            {
                var verlag = (from v in Admin.conn.Verlag where v.PublisherID == (Int32)cbVerlag.SelectedValue select v).FirstOrDefault();
                txtStandort.Text = verlag.SortBy;
                cBook.VerlagsID = verlag.PublisherID;
            }
        }

        private void BtnChangePerson_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Noch nicht implementiert!");
        }

        private void BtnSave_click(object sender, RoutedEventArgs e)
        {
            try
            {

                Admin.conn.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DialogResult = true;
        }

        private void BtnExit_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

      
    }
}