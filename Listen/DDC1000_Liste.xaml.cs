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
    /// Interaktionslogik für DDC1000_Liste.xaml
    /// </summary>
    public partial class DDC1000_Liste : Window
    {
        //public BuchDataClassesDataContext con = new BuchDataClassesDataContext();
        public DDC1000_Liste()
        {
            InitializeComponent();
            //var ddc = from d in Admin.conn.DDC_1000 select d;
            this.DataContext = Admin.conn.DDC_1000;
            //dgDDC.ItemsSource = ddc;
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

        private void BtnUpdate_click(object sender, RoutedEventArgs e)
        {
            var ddc = from d in Admin.conn.DDC_1000 select d;
            foreach (var item in ddc)
            {
                if (string.IsNullOrEmpty(item.DDC_Name)==true)
                {
                    item.DDC_Name = item.DDC_Name_Engl;
                }
            }
            try
            {

                Admin.conn.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
