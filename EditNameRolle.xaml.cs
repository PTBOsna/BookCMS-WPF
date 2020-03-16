﻿using System;
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
    /// Interaktionslogik für EditNameRolle.xaml
    /// </summary>
    public partial class EditNameRolle : Window
    {
        NameRolle cNR;
        public EditNameRolle(NameRolle _cNR)
        {
            InitializeComponent();
            this.cNR = _cNR;
            this.DataContext = cNR;
           
            var ar = from a in Admin.conn.AutorRolle select new { id = a.ID, rolle = a.AutorKurz + " (" + a.AutorRolle1 + ")" };
            cbRolle.ItemsSource = ar;
        }

        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var searchName = from sn in Admin.conn.Person where sn.SortBy.StartsWith(txtSuche.Text.Trim()) select sn;
            Int32 count = searchName.Count();
            DGNameInDB.ItemsSource = searchName.ToList();
        }

        private void DGNameInDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person sel = DGNameInDB.SelectedItem as Person;
            txtName.Text = sel.SortBy;
            txtNameInDB.Text = sel.PersonID.ToString();
        }

        private void cbRolle_DropDownClosed(object sender, EventArgs e)
        {
  MessageBox.Show(cbRolle.Text);
            //Int32 selID = Int32.Parse( cbRolle.Text);
            //var rol = from r in Admin.conn.AutorRolle where r.ID == (Int32) cbRolle.SelectedItem select r;
            txtRolle.Text = cbRolle.Text;
            txtRolleID.Text = cbRolle.SelectedValue.ToString();
        }
        

      
    }

}
