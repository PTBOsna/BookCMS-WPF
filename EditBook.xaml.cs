using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für AddEditBook.xaml
    /// </summary>
    public partial class AddEditBook : Window
    {
        public Buch cBook;
        Int32 bookID;
        List<string> personen = new List<string>();
        List<NameRolle> cNameRolle = new List<NameRolle>();
        mySettings ms = new mySettings();
        public AddEditBook(Int32 _bookID)
        {
            InitializeComponent();
            this.bookID = _bookID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            // Buch laden
            if (bookID<0)
            {
                MessageBox.Show("Kein Buch ausgewählt!");
                return;
            }
            cBook = (from b in Admin.conn.Buch
                     where b.ID == bookID
                     select b).FirstOrDefault();
            //ggf. Image laden
            string cPath = System.IO.Path.Combine(mySettings.CoverPath, bookID.ToString() + ".jpg");
            Admin.ShowImage(ImgBox, cPath);
            //Personen laden
            var erg = (from abl in Admin.conn.AutorBuchLink
                       from pers in Admin.conn.Person
                       from ro in Admin.conn.AutorRolle
                       where abl.BuchID == cBook.ID && pers.PersonID == abl.PersonID && ro.ID == abl.RolleID orderby pers.SortBy
                       select new { Person = pers.SortBy, Rolle = ro.AutorRolle1, PerID = pers.PersonID, RolleID = ro.ID, AutorRolleID = abl.id }).ToList();
            foreach (var el in erg)
            {
                //personen.Add(el.Person + " (" + el.Rolle + ")");
                cNameRolle.Add(new NameRolle() { name = el.Person + " (" + el.Rolle + ")", currID=el.PerID, rolle = el.Rolle, currRolleID = el.RolleID, currNameRolleID = el.AutorRolleID  });
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
                    if (book.SachgruppeID == (int) item.Tag)
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
            var _kat = (from k in Admin.conn.Kategorie where k.CategoryID == cBook.KategorieID select k).FirstOrDefault();
            if (_kat != null)
            {
                txtKategorie.Text = _kat.Kategorie1;

            }
            var _uKat = (from u in Admin.conn.Unterkategorie where u.SubCategoryID == cBook.UnterkategorieID select u).FirstOrDefault();
            if (_uKat != null)
            {

                txtUKategorie.Text = _uKat.Unterkategorie1;
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
            var kat = from k in Admin.conn.Kategorie orderby k.SortBy select new { kat = k.SortBy, id = k.CategoryID };
            cbKategorie.ItemsSource = kat.ToList();
            var ukat = from uk in Admin.conn.Unterkategorie orderby uk.SortBy select new { ukat = uk.SortBy, id = uk.SubCategoryID };
            cbUKategorie.ItemsSource = ukat.ToList();
            var verlag = from v in Admin.conn.Verlag orderby v.SortBy select new { verl = v.SortBy, id = v.PublisherID };
            cbVerlag.ItemsSource = verlag.ToList();
            var verlagOrt = from vo in Admin.conn.VerlagsOrt orderby vo.SortBy select new { verlort = vo.SortBy, id = vo.ID };
            cbVerlagsOrt.ItemsSource = verlagOrt.ToList();
            var druck = from d in Admin.conn.Druckerei orderby d.SortBy select new { druck = d.SortBy, id = d.PrintedByID };
            cbDruckerei.ItemsSource = druck.ToList();
        }

        private void BtnSave_click(object sender, RoutedEventArgs e)
        {
            //Änderung bei Personen speichern
            string cAutorSort = null;
            foreach (var item in cNameRolle)
            {
            var editAR = (from ar in Admin.conn.AutorBuchLink where ar.id == item.currNameRolleID select ar).FirstOrDefault();
            editAR.BuchID = bookID;
                editAR.PersonID = item.currID;
                editAR.RolleID = item.currRolleID;
                cAutorSort += item.name + "; ";
            }
            //Änderung in GenereLink
            foreach (CheckBox item in ugridGenre.Children)
            {
                GenreLink gnl = new GenreLink();
                try
                {
                    if (item.IsChecked == true)
                    {
                        gnl.BuchID = bookID;
                        gnl.SachgruppeID = (Int32)item.Tag;
                        Admin.conn.GenreLink.InsertOnSubmit(gnl);
                        Admin.conn.SubmitChanges();
                        //MessageBox.Show(item.Tag.ToString() + " / " + item.Content.ToString());
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message + "\r\n" + "Genre/Sachgruppe evt. nicht übernommen!", "Fehler beim Speichern!");
                }
            }
            //AutorSort eintragen
            cBook.AutorSort = cAutorSort.Substring(0, cAutorSort.Length - 2);
            //Und alle Änderungen speichern
            Admin.conn.SubmitChanges();
                DialogResult = true;
        }

       

        private void BtnExit_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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
                var kate = (from k in Admin.conn.Kategorie where k.CategoryID == (Int32)cbKategorie.SelectedValue select k).FirstOrDefault();
                txtKategorie.Text = kate.SortBy;
                cBook.KategorieID = kate.CategoryID;
            }
        }
        private void cbUKategorie_DropDownClosed(object sender, EventArgs e)
        {
            if (cbUKategorie.SelectedIndex != -1)
            {
                var ukate = (from k in Admin.conn.Unterkategorie where k.SubCategoryID == (Int32)cbUKategorie.SelectedValue select k).FirstOrDefault();
                txtUKategorie.Text = ukate.SortBy;
                cBook.UnterkategorieID = ukate.SubCategoryID;
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
                txtStandort.Text = vlgOrt.SortBy;
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
            if (lbPersonen.SelectedValue==null)
            {
                MessageBox.Show("Bitte zuerst eine Person auswählen!");
                return;
            }
            int index = 0;
            {
                NameRolle nr = new NameRolle();
                foreach (var item in cNameRolle)
                {
                    if (item.currID ==(Int32) lbPersonen.SelectedValue)
                    {
                        nr.currID = item.currID;
                        nr.currRolleID = item.currRolleID;
                        nr.name = item.name;
                        nr.rolle = item.rolle;
                        nr.currNameRolleID = item.currNameRolleID;
                      
                EditNameRolle enr = new EditNameRolle(nr);
                enr.ShowDialog();
                        index = cNameRolle.IndexOf(item);
                    
                    }
                }
                        cNameRolle[index] = nr;
                lbPersonen.ItemsSource = null;
                lbPersonen.ItemsSource = cNameRolle;
            }
        }
    }
}
