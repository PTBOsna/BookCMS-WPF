﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookCMS_WPF.DataHandling;
using System.Xml;
using System.Data;
using System.Windows.Controls;

namespace BookCMS_WPF
{

    /// <summary>
    /// Interaktionslogik für XTest.xaml
    /// </summary>
    public partial class XTest : Window
    {

        public string sampleXml = @"H:\VisualStudio-Projekte\ReadXML\ReadXML\";
        public List<NameRolle> nr_list;
        public XTest()
        {
            InitializeComponent();
            txtInput.Focus();
            var ddc = from dd in Admin.conn.DDC_Haupt select new { Name = dd.DDC_Name, DDC = dd.DDC };

            lbDDC.ItemsSource = ddc;



        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            NameRolle nr = new NameRolle();
            nr_list = new List<NameRolle>();
            string input = txtInput.Text;
            if (string.IsNullOrEmpty(input) == true)
            {
                MessageBox.Show("Zuerst Dateiname (ohne Endung) eingeben!");
                return;
            }
            string s = s = File.ReadAllText(sampleXml + txtInput.Text.Trim() + ".mrcx"); ;
            DataHandling.DNBBookData dnbdata = new DNBBookData(s);
            //MessageBox.Show(dnbdata.dnb_nr + "/" + dnbdata.dnb_isbn_13 + "/" + dnbdata.dnb_isbn + "/" + dnbdata.dnb_titel + "/" + dnbdata.dnb_stichwort + "/" + dnbdata.dnb_index);

            //cbTitel.Items.Add(dnbdata.dnb_Autor_sort + ", " + dnbdata.dnb_Rolle);
            string[] name = dnbdata.dnb_Autor_sort.Split(',');
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
                    nr_list.Add(new NameRolle() { name = mitAutor[i], rolle = rolleMitAutor[i], nameInDB = retA[1], currID = Int32.Parse(retA[0]) });
                }

            }

            //  parts.Add(new Part() {PartName="crank arm", PartId=1234});
            txtInput.Focus();
            //lbTitel.Items.Clear();
            foreach (var item in nr_list)
            {
                var rolleID = (from ri in Admin.conn.AutorRolle where ri.AutorKurz == item.rolle select ri).FirstOrDefault();
                if (rolleID != null)
                {
                    item.currRolleID = rolleID.ID;
                }
            }

            //Datagrid vorbereiten

            var autorrolle = (from ar in Admin.conn.AutorRolle select ar.AutorRolle1).ToList();
            ComboBoxColumn.ItemsSource = autorrolle;
            DGNamen2.ItemsSource = nr_list;
            DGNamen.ItemsSource = nr_list;
            //cbTitel.ItemsSource = nr_list;
            var lang = from lg in Admin.conn.Language select lg;

            cbTest.ItemsSource = lang.ToList();

            if (dnbdata.dnb_dcc1 != null)
            {

                FindDDC(dnbdata.dnb_dcc1);
            }
            //lbTitel.Items.Add(dnbdata.dnb_Autor_sort);
            //lbTitel.Items.Add(dnbdata.dnb_Rolle);
            //lbTitel.Items.Add(dnbdata.dnb_mitautor);
            //lbTitel.Items.Add(dnbdata.dnb_mitautor_rolle);

        }

        private void FindDDC(string ddc)
        {

            string vDDC = null;
            ddc = ddc.Substring(0, ddc.Length - 1);

            List<string> dd = ddc.Split(' ').ToList();
            foreach (var item in dd)
            {
                if (item.Length > 1)
                {
                    string selDDC = item.Substring(0, 2);
                    //auf Dopplung prüfen
                    if (selDDC != vDDC)
                    {
                        var _ddc = (from d in Admin.conn.DDC_1000 where d.DDC.StartsWith(selDDC) select d).FirstOrDefault();
                        MessageBox.Show(_ddc.DDC_Name + "\r\n");
                        vDDC = selDDC;
                    }

                }
            }
        }

        private string FindAutor(string cName)
        {
            var per = (from p in Admin.conn.Person where p.Name.Contains(cName) select p).FirstOrDefault(); //new { id = p.PersonID, Name = p.SortBy };

            string erg;
            if (per != null)
            {
                erg = per.PersonID.ToString() + "#" + per.SortBy;

                return erg;
            }
            else
            {
                erg = "-1#" + "Name nicht vorhanden";
                return erg;
            }

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
            DGNameInDB.ItemsSource = searchName.ToList();
        }

        private void DGNameInDB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Person sel = DGNameInDB.SelectedItem as Person;
            MessageBox.Show(sel.PersonID.ToString());
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NameRolle nr = DGNamen.SelectedItem as NameRolle;
                if (nr.currID == -1)
                {
                    MessageBox.Show("Form AddName aufrufen");
                }
                else
                {
                    EditNameRolle enr = new EditNameRolle(nr);
                    enr.ShowDialog();
                    DGNamen.ItemsSource = nr_list;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        string _prevText = string.Empty;
        private void cbTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (var item in cbTest.Items)
            {
                if (item.ToString().StartsWith(cbTest.Text))
                {
                    _prevText = cbTest.Text;
                    return;
                }
            }
            cbTest.Text = _prevText;
        }

        private void cbTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTest.SelectedValue != null)
            {

                MessageBox.Show(cbTest.SelectedValue.ToString());
            }
        }

        private void lbDDC_changed(object sender, SelectionChangedEventArgs e)
        {
            lbDDC100.Items.Clear();
            string selDDC = lbDDC.SelectedValue.ToString();
            selDDC = selDDC.Substring(2, 1);
            var ddc = from dd in Admin.conn.DDC_1000 where dd.DDC.StartsWith(selDDC) select dd;

            foreach (var item in ddc)
            {
                lbDDC100.Items.Add(item.DDC_Name);

            }

        }

        private void loadButton()
        {
            var gen = from g in Admin.conn.Sachgruppe where g.Marked ==true orderby g.SortBy select g;
            foreach (var g in gen)
            {
                System.Windows.Controls.CheckBox newBtn = new CheckBox();
                newBtn.Content = g.Sachgruppe1;
                stkpnl.Children.Add(newBtn);

                newBtn.Tag = g.GenreID;
            } 
         
           

        }
        private void auslesen()
        {
            //List<CheckBox> loc = new List<CheckBox>();
            foreach (CheckBox item in stkpnl.Children)
            {
                if (item.IsChecked==true)
                {
                    MessageBox.Show(item.Tag.ToString() + " / " + item.Content.ToString());
                }       
            }

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadButton();
        }

        private void BtnReadGenre_clidk(object sender, RoutedEventArgs e)
        {
            auslesen();
        }

        private void BtnSaveGenre_clidk(object sender, RoutedEventArgs e)
        {
            int bookID = 0;
            GenreLink gnl = new GenreLink();
            foreach (CheckBox item in stkpnl.Children)
            {
                if (item.IsChecked == true)
                {
                    gnl.BuchID = bookID;
                    gnl.SachgruppeID = (Int32) item.Tag;
                    MessageBox.Show(item.Tag.ToString() + " / " + item.Content.ToString());
                }
            }
        }
    }
}

