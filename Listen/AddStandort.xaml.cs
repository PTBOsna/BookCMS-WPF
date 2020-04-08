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
using BookCMS_WPF;
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF.Listen
{
    /// <summary>
    /// Interaktionslogik für AddStandort.xaml
    /// </summary>
    public partial class AddStandort : Window
    {
        public AddStandort()
        {
            InitializeComponent();
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStandort.Text) == true)
            {
                MessageBox.Show("Standort eingeben");
            }
            else
            {
                Standort standort = new Standort();
                standort.Standort1 = txtStandort.Text.Trim();
                standort.SortBy = txtStandort.Text.Trim();
                standort.Notiz_PlainText = txtHinweise.Text.Trim();
                Admin.conn.Standort.InsertOnSubmit(standort);
                Admin.conn.SubmitChanges();
                DialogResult = true;
            }
        }

        private void BtnCanel_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
