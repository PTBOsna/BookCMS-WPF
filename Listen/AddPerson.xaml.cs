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
        public AddPerson(string _name)
        {
            InitializeComponent();
            this.name = _name;
            pers = new Person();
            this.DataContext = pers;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
            Admin .conn.Person.InsertOnSubmit(pers);
            Admin.conn.SubmitChanges();
            Admin.currPersonID = pers.PersonID;
            //NameRolle nr = new NameRolle();
            //nr.name = name;
            //nr.currID = pers.PersonID;
           
            DialogResult = true;
        }

        private void BtnCancel_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
 