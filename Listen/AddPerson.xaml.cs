using BookCMS_WPF.DataHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookCMS_WPF.Listen
{
    /// <summary>
    /// Interaktionslogik für AddPerson.xaml
    /// </summary>
    public partial class AddPerson : Window
    {
        public string name;
        public Person pers;
        bool IsNew;
        Int32 PerID;
        public AddPerson(string _name, bool _isNew)
        {
            InitializeComponent();
            this.name = _name;
            this.IsNew = _isNew;
            if (IsNew == true)
            {
                pers = new Person();
                this.DataContext = pers;
            }
            else
            {
                PerID = Int32.Parse(name);
                var pers = (from p in Admin.conn.Person where p.PersonID == PerID select p).FirstOrDefault();
            this.DataContext = pers;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsNew==false)
            {
                return;
            }

            if (name.Contains(','))
            {
                txtSortBy.Text = name;
                string[] namePart = name.Split(',');

                txtName.Text = namePart[1].Trim() + " " + namePart[0];
            }
            else
                MessageBox.Show(name + "\r\n" + "Bitte Namen mauell eingeben!");
        }

        private void BtnSave_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNew == true)
                {
                    Admin.conn.Person.InsertOnSubmit(pers);
                }
                Admin.conn.SubmitChanges();
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Fehler beim Speichern!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

                DialogResult = true;
        }

        private void BtnCancel_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
