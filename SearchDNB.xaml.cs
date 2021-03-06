﻿using BookCMS_WPF.DataHandling;
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
using System.Windows.Shapes;
using System.Xml;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für SearchDNB.xaml
    /// </summary>
    public partial class SearchDNB : Window
    {
        public DNBBookData dnbdata;
        public bool addOrEdit;
        public string dnbID { get; set; }
        public SearchDNB(bool _addOrEdit, string _titel) //Add = treu, Edit = false
        {
            InitializeComponent();
            this.addOrEdit = _addOrEdit;
            if (string.IsNullOrEmpty(_titel)==false)
            {
                txtInput.Text = _titel;
            }
            txtInput.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void connect(string suche)
            
        {
            //mySettings ms = new mySettings();
            //MessageBox.Show(ms.DNB_API);
            //string key = "1159cfc6b965e8a03abc3bd8227afa"; //TODO: wird später aus den Settings entnommen
            string key = mySettings.DNB_API;
            WebClient w = new WebClient();
            w.Encoding = Encoding.UTF8;
            string urlEnc = WebUtility.UrlEncode(suche);

            urlEnc = urlEnc.Replace("+","%20and%20");
            //string myURL = "https://services.dnb.de/sru/dnb?version=1.1&operation=searchRetrieve&query=" + urlEnc + "&recordSchema=MARC21-xml&accessToken=" + key;

            string s = w.DownloadString("https://services.dnb.de/sru/dnb?version=1.1&operation=searchRetrieve&query=" + urlEnc  + "&recordSchema=MARC21-xml&accessToken=" + key);
           dnbdata = new DNBBookData(s);
            //txtVorschau.Text = s;
            //txtInput.Focus();
            // auch als XML-Doc laden
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            var nodeList = doc.GetElementsByTagName("numberOfRecords");
            
            Int32 count = 0;
            List<string> infolist = new List<string>();
            foreach (XmlNode item in nodeList)
            {
                //MessageBox.Show(item.InnerText);
                count = Int32.Parse(item.InnerText);
                if (count >= 1)
                {
                    if (count > 20)
                    {
                        MessageBox.Show("Die Suche brachte mehr als 20 Treffer!" + "\r\n" + "Bitte Suchfrage ändern!");
                        return;
                    }
                    else
                    {
                        infolist = InfoMulti._InfoMulti(s).Split('~').ToList();
                    lbAuswahl.ItemsSource = infolist;
                    //return;
                    }
                   
                }
                else { MessageBox.Show("Kein Treffer, Suche verfeinern!"); }
            //lbAuswahl.Items.Add("Titel: " + dnbdata.dnb_titel + ", Autor: " + dnbdata.dnb_autor + ", Jahr: " + dnbdata.dnb_jahr + ", ISBN: " + dnbdata.dnb_isbn_13);
            }
            //lbAuswahl.ItemsSource = InfoMulti._InfoMulti(s).Split('~').ToList();

        }

       
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            lbAuswahl.Items.Clear();
            connect(txtInput.Text);
            if (dnbdata.dnb_titel != null)
            {
            CheckTitle(dnbdata. dnb_titel);

            }
        }
        private void CheckTitle(string dnb_titel)
        {
            int len = 10;
            if (dnb_titel.Length<10)
            {
                len = dnb_titel.Length;
            }
            string curTi = Admin. GetTitleIndex(dnb_titel);
            curTi = curTi.Substring(0, len);
            string displTi = null;
            var avblbTitel = (from b in Admin.conn.Buch where b.TitelIndex.Contains(curTi) select b).ToList();
            if (avblbTitel.Count()>0)
            {

                foreach (var selTitel in avblbTitel)
                {
                    displTi += selTitel.Titel + " Autor: " + selTitel.AutorSort + "\r\n";

                }
                displTi += "Weiter?";

                if (MessageBox.Show(displTi, "Folgende(r) Titel bereits vorhanden", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    return;
                }
              
                DialogResult = false;

            }
        }

        private void btnAdd_click(object sender, RoutedEventArgs e)
        {
            if (lbAuswahl.SelectedItem == null)
            {
                MessageBox.Show("Bitte zuerst einen Titel auswählen!");
                return;
            }
            string sel = lbAuswahl.SelectedItem as string;
            string[] erg = sel.Split(';');
            //MessageBox.Show(erg[0]);
            if (addOrEdit==true)
            {
                AddBook ab = new AddBook(erg[0]);
                ab.ShowDialog();
                DialogResult = true;

            }
            else
            {
                dnbID = erg[0];
                //EditBookDNB eb = new EditBookDNB(Int32.Parse( erg[0]));
                //eb.ShowDialog();
                DialogResult = true;
            }

        }

        private void btnExit_click(object sender, RoutedEventArgs e)
        {
            if (addOrEdit == false)
            {
                dnbID = "#"; //Suche abgebrochen


            }
            DialogResult = false;
           
        }

        private void btnNew_click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = null;
            lbAuswahl.ItemsSource = null;
            txtInput.Focus();
        }

       
    }
}
