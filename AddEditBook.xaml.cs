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
        public AddEditBook(Int32 _bookID)
        {
            InitializeComponent();
            this.bookID = _bookID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Buch cBook = (from b in Admin.conn.Buch
                          where b.ID == bookID
                          select b).FirstOrDefault();
            this.DataContext = cBook;
        }
    }
}
