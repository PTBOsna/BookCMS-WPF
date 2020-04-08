using BookCMS_WPF.DataHandling;
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

namespace BookCMS_WPF.Listen
{
    /// <summary>
    /// Interaktionslogik für EditStandort.xaml
    /// </summary>
    public partial class EditStandort : Window
    {
        Standort standort;
        public EditStandort(Standort _standort)
        {
            InitializeComponent();
            this.standort = _standort;
            txtStandort.Text = standort.Standort1;
            txtHinweise.Text = standort.Notiz_PlainText;
        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {
            standort.Standort1 = txtStandort.Text.Trim();
            standort.SortBy = txtStandort.Text.Trim();
            standort.Notiz_PlainText = txtHinweise.Text.Trim();
            Admin.conn.SubmitChanges();
            DialogResult = true;
        }

        private void BtnCanel_click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
