using System;

using System.Windows;
using System.Windows.Forms;
using Ookii;
using Ookii.Dialogs.Wpf;
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF
{
    /// <summary>
    /// Interaktionslogik für MySettings.xaml
    /// </summary>
    public partial class MySettings : Window
    {
        
        public MySettings()
        {
            InitializeComponent();

            this.DataContext = Admin.conn.Settings;
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        //ookii-wpf-Dialog genutzt, da Windows.Forms nicht verwendbar ist!
        {
            //VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            //string selPath = null;
            //dialog.Description = "Please select a folder.";
            //dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
            //if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            //    MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            //if ((bool)dialog.ShowDialog(this))
            //    selPath =  dialog.SelectedPath;

            //    txtImgPath.Text = selPath;
            System.Windows.Forms.FolderBrowserDialog fd = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtImgPath.Text = fd.SelectedPath.ToString();
                
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Admin.conn.SubmitChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
