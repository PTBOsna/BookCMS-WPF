using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookCMS_WPF.DataHandling;
using System.Xml;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für XTest.xaml
    /// </summary>
    public partial class XTest : Window
    {
        public XTest()
        {
            InitializeComponent();
            txtInput.Focus();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<string[]> erg = null;
            string key = "1159cfc6b965e8a03abc3bd8227afa"; //TODO: wird später aus den Settings entnommen
            WebClient w = new WebClient();
            w.Encoding = Encoding.UTF8;
            string urlEnc = WebUtility.UrlEncode(txtInput.Text);

            string s = w.DownloadString("https://services.dnb.de/sru/dnb?version=1.1&operation=searchRetrieve&query=" + urlEnc + "&recordSchema=MARC21-xml&accessToken=" + key);
            DataHandling.DNBBookData dnbdata = new DNBBookData(s);
            MessageBox.Show(dnbdata.dnb_nr + "/" + dnbdata.dnb_isbn_13 + "/" + dnbdata.dnb_isbn + "/" + dnbdata.dnb_titel + "/" + dnbdata.dnb_stichwort + "/" + dnbdata.dnb_index);
            //txtVorschau.Text = s;
            txtInput.Focus();
            // auch als XML-Doc laden
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            var nodeList = doc.GetElementsByTagName("numberOfRecords");
            Int32 count = 0;
            
            foreach (XmlNode item in nodeList)
            {
                MessageBox.Show(item.InnerText);
                count = Int32.Parse(item.InnerText);
                if(count>1)
                {
                    if (count>10)
                    {
                        MessageBox.Show("Die Suche brachte mehr als 10 Treffer!" + "\r\n" + "Bitte Suchfrage ändern!");
                        return;
                    }
                   
                    
                    lbTitel.ItemsSource=  InfoMulti._InfoMulti(s).Split('~').ToList() ;
                    return;
                }
            }
        }

        //private List<string> ReadText(string sampleXml, string code)
        //{
        //    List<string> list = new List<string>();
            
        //    using (StringReader reader = new StringReader(sampleXml))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            if (line.Contains("tag=\"" + code + "\""))
        //            {
        //                line = reader.ReadLine();
        //                while (line.Contains("</datafield>") == false)
        //                {
        //                    if (line.Contains("code=\"c\""))
        //                    {
        //                        //LBShow.Items.Add(SelectString(line)); // Write to console.
        //                        list.Add(SelectString(line)); // Add to list.
        //                        //MessageBox.Show(SelectString(line));
        //                    }
        //                    line = reader.ReadLine();
        //                    //return list;
        //                }

        //            }

        //        }
        //            return list;

        //    }
        //}
        //public string SelectString(string myString)
        //{
        //    string[] stringPart = myString.Split('>');
        //    string[] ret = stringPart[1].Split('<');

        //    return ret[0];

        //}
    }
}
