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
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für AddEditBook.xaml
    /// </summary>
    public partial class AddEditBook : Window
    {
        Int32 bookID;
        List<string> personen = new List<string>();
        public AddEditBook(Int32 _bookID)
        {
            InitializeComponent();
            this.bookID = _bookID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {// Buch laden
            Buch cBook = (from b in Admin.conn.Buch
                          where b.ID == bookID
                          select b).FirstOrDefault();

            //Personen laden
            var erg = (from abl in Admin.conn.AutorBuchLink
                       from pers in Admin.conn.Person
                       from ro in Admin.conn.AutorRolle
                       where abl.BuchID == cBook.ID && pers.PersonID == abl.PersonID && ro.ID == abl.RolleID
                       select new { Person = pers.SortBy, Rolle = ro.AutorRolle1 }).ToList();
            foreach (var el in erg)
            {
                personen.Add(el.Person + " (" + el.Rolle + ")");
            }
            lbPersonen.ItemsSource = personen;
            this.DataContext = cBook;

            //Verknüpfungen laden
            var _verlag = (from v in Admin.conn.Verlag where v.PublisherID == cBook.VerlagsID select v.SortBy).FirstOrDefault();
            txtVerlag.Text = _verlag;
            var _verlagsOrt = (from v in Admin.conn.VerlagsOrt where v.ID == cBook.VerlagsOrtID select v.SortBy).FirstOrDefault();
            txtVerlagsOrt.Text = _verlagsOrt;
            var _druckerei = (from d in Admin.conn.Druckerei where d.PrintedByID == cBook.DruckereiID select d.SortBy).FirstOrDefault();
            txtDruckerei.Text = _druckerei;

        }
    }
}
