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
    public class NameRolle
    {
        public string name { get; set; }
        public string rolle { get; set; }
        public string nameInDB { get; set; }
        public Int32 currID { get; set; }
    }
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

        public string sampleXml = @"H:\VisualStudio-Projekte\ReadXML\ReadXML\";
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            NameRolle nr = new NameRolle();
            List<NameRolle> nr_list = new List<NameRolle>();
            string input = txtInput.Text;
            if (string.IsNullOrEmpty(input) == true)
            {
                MessageBox.Show("Zuerst Dateiname (ohne Endung) eingeben!");
                return;
            }
            string s = s = File.ReadAllText(sampleXml + txtInput.Text.Trim() + ".mrcx"); ;
            DataHandling.DNBBookData dnbdata = new DNBBookData(s);
            MessageBox.Show(dnbdata.dnb_nr + "/" + dnbdata.dnb_isbn_13 + "/" + dnbdata.dnb_isbn + "/" + dnbdata.dnb_titel + "/" + dnbdata.dnb_stichwort + "/" + dnbdata.dnb_index);
            //txtVorschau.Text = s;
            //Autor aus Feld 100
            //nr.name = dnbdata.dnb_Autor_sort;
            //nr.rolle = dnbdata.dnb_Rolle;
            //nr_list.Add(nr);
            //Mitautor und Rolle aus Feld 700 (ist Rekursiv) 
            //zunächst letztes Semikolon enfernen


            lbTitel.Items.Add(dnbdata.dnb_Autor_sort + ", " + dnbdata.dnb_Rolle);
            string[] name =dnbdata.dnb_Autor_sort.Split(',');
            string[] ret = FindAutor(name[0]).Split('#');
            nr_list.Add(new NameRolle() { name = dnbdata.dnb_Autor_sort, rolle = dnbdata.dnb_Rolle, nameInDB = ret[1], currID = Int32.Parse(ret[0]) });
            if (dnbdata.dnb_mitautor != null)
            {
                dnbdata.dnb_mitautor = dnbdata.dnb_mitautor.Substring(0, dnbdata.dnb_mitautor.Length - 1);
                dnbdata.dnb_mitautor_rolle = dnbdata.dnb_mitautor_rolle.Substring(0, dnbdata.dnb_mitautor_rolle.Length - 1);
                List<string> mitAutor = dnbdata.dnb_mitautor.Split(';').ToList();
                List<string> rolleMitAutor = dnbdata.dnb_mitautor_rolle.Split(';').ToList();
                for (int i = 0; i < mitAutor.Count; i++)
                {
                     name = mitAutor[i].Split(',');
                    string[] retA = FindAutor(name[0]).Split('#');
                    nr_list.Add(new NameRolle() { name = mitAutor[i], rolle = rolleMitAutor[i], nameInDB= retA[1], currID= Int32.Parse(retA[0]) });
                }

            }

            //  parts.Add(new Part() {PartName="crank arm", PartId=1234});
            txtInput.Focus();
            //lbTitel.Items.Clear();
            foreach (var item in nr_list)
            {
                name = item.name.Split(',');
                lbTitel.Items.Add(item.name + "; " + item.rolle + " - " + FindAutor(name[0]));
            }

            //Datagrid vorbereiten

        
            DGNamen.ItemsSource = nr_list;

            //lbTitel.Items.Add(dnbdata.dnb_Autor_sort);
            //lbTitel.Items.Add(dnbdata.dnb_Rolle);
            //lbTitel.Items.Add(dnbdata.dnb_mitautor);
            //lbTitel.Items.Add(dnbdata.dnb_mitautor_rolle);

        }

       

        private string FindAutor(string cName)
        {
            var per = (from p in Admin.conn.Person where p.Name.Contains(cName) select p).FirstOrDefault(); //new { id = p.PersonID, Name = p.SortBy };

            string erg;
            if (per != null)
            {
                 erg =  per.PersonID.ToString() + "#" + per.SortBy  ;

                return erg;
            }
            else {
                 erg =  "-1#" + "Name nicht vorhanden" ;  
                return erg; }

            //var perList = (from pl in Admin.conn.Person where pl.Name.Contains(cName) select pl).ToList();
            //DGName.ItemsSource = var;
        }

        private void BtnAdd_click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDel_click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var searchName = from sn in Admin.conn.Person where sn.SortBy.StartsWith(txtSearch.Text.Trim()) select sn;
            Int32 count = searchName.Count();
            DGNames.ItemsSource= searchName.ToList();
        }

        private void DGNames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Person sel = DGNames.SelectedItem as Person;
            MessageBox.Show(sel.PersonID.ToString());
        }
    }
}
