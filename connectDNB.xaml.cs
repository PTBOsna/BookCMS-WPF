using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using BookCMS_WPF;
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für connectDNB.xaml
    /// </summary>
    public partial class connectDNB : Page
    {
        string search;
        public connectDNB(string _search)
        {
            InitializeComponent();
            this.search = _search;
            txtKurzErgeb.Text = search;
            SearchDNB(search);
        }

        //private void SearchDNB(string sstring)
        //{
        //    string key = "1159cfc6b965e8a03abc3bd8227afa"; //TODO: wird später aus den Settings entnommen
        //    WebClient w = new WebClient();
        //    w.Encoding = Encoding.UTF8;
        //    string urlEnc = WebUtility.UrlEncode(sstring);

        //    string s = w.DownloadString("https://services.dnb.de/sru/dnb?version=1.1&operation=searchRetrieve&query=" + urlEnc + "&recordSchema=MARC21-xml&accessToken=" + key);
        //    DataHandling.DNBBookData dnbdata = new DataHandling.DNBBookData(s);
        //    lbAuswahl.Items.Add("Titel: "  + dnbdata.dnb_titel + ", Autor: " + dnbdata.dnb_autor + ", Jahr: " + dnbdata.dnb_jahr + ", ISBN: " + dnbdata.dnb_isbn_13);
        //    //txtVorschau.Text = s;
        //    //txtInput.Focus();
        //    // auch als XML-Doc laden
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(s);
        //    var nodeList = doc.GetElementsByTagName("numberOfRecords");
        //    Int32 count = 0;

        //    foreach (XmlNode item in nodeList)
        //    {
        //        MessageBox.Show(item.InnerText);
        //        count = Int32.Parse(item.InnerText);
        //        if (count > 1)
        //        {
        //            if (count > 10)
        //            {
        //                MessageBox.Show("Die Suche brachte mehr als 10 Treffer!" + "\r\n" + "Bitte Suchfrage ändern!");
        //                return;
        //            }


        //            lbAuswahl.ItemsSource = InfoMulti._InfoMulti(s).Split('~').ToList();
        //            return;
        //        }
        //    }
        //}

        private void btnAdd_click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
