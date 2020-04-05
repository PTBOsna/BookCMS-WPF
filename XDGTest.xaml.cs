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

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für XDGTest.xaml
    /// </summary>
    public partial class XDGTest : Window
    {
        public XDGTest()
        {
            InitializeComponent();
            var pr = from p in Admin.conn.Person select p;
            this.DataContext = pr.ToList();
            DGPeson.ItemsSource = pr;
        }
    }
}
