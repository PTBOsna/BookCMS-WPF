using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace BookCMS_WPF.DataHandling
{
    class Admin
    {
        public static BuchDataClassesDataContext conn = new BuchDataClassesDataContext();
        public static List<string> myAlpha = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static int currPersonID;

        public static string CleanString(string cString)
        {
            //alle Sonderzeichen entfernen
            for (int i = cString.Length - 1; i >= 0; i--)
            {
                if (Char.IsLetterOrDigit(cString[i]) == false)
                {
                    cString = cString.Remove(i, 1);
                }
            }
            return cString;
        }

        public static void addAutorBuchLInk(AutorBuchLink autorBuchLink)
        {

            {
                conn.AutorBuchLink.InsertOnSubmit(autorBuchLink);
                conn.SubmitChanges();
            }
        }


        public static bool IsPageValid(string page)
        {
            WebClient w = new WebClient();
            try
            {
                w.DownloadString(page);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string GetTitleIndex(string ti)
        {
            int iLen = 15;
            string titelIndex = ti.Trim().ToUpper();
            titelIndex = Admin.CleanString(titelIndex);
            if (titelIndex.Length < 15)
            {
                iLen = titelIndex.Length;
            }
            titelIndex = titelIndex.Substring(0, iLen);
            return titelIndex;
        }
        public static string GetSynopsis(string cUrl)
        {
            string[] url = cUrl.Split('#');
            url[0] = url[0].Replace(';', '&');
            WebBrowser web = new WebBrowser();

            Stopwatch timer = Stopwatch.StartNew(); // start a timer

            try
            {

                web.ScriptErrorsSuppressed = true;
                web.Navigate(url[0]);

                // MAKE SURE ReadyState = Complete
                while (web.ReadyState.ToString() != "Complete")
                {
                    if (timer.ElapsedMilliseconds >= 1000) break;
                    Application.DoEvents();

                }
                return web.Document.Body.InnerText;

            }
            catch (Exception)
            {
                return "Keine Inhaltsangabe verfügbar";
            }
        }

        public static void ShowImage(System.Windows.Controls.Image img, string cpath)
        {
            if (File.Exists(cpath) == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(cpath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;

                img.Source = bitmap;
                bitmap.EndInit();
            }

        }

        public static string ChkDDC_Kat(string _ddc)
        {
            string ddc2 = _ddc.Substring(0, _ddc.Length - 2);
            string ergeb = null;

            if (string.IsNullOrEmpty(_ddc) == false)
            {
                ddc2 = ddc2.Replace(" ", "");
                List<string> ddc_name = ddc2.Split(';').ToList();
                string vor = ddc_name[0];
                foreach (var name in ddc_name)
                {
                    ddc2 = name.Substring(0, 1);

                    if (Char.IsNumber(ddc2, 0) == true && ddc2 != vor)
                    {
                        try
                        {
                            var ddc = (from d in Admin.conn.DDC_Haupt where d.DDC_Haupt1.Contains(ddc2) select new { code = d.DDC_Haupt1, DDCname = d.DDC_Name }).FirstOrDefault();
                            vor = ddc2;
                            ergeb += ddc.code + ", " + ddc.DDCname + "\r\n";
                        }
                        catch (Exception)
                        {

                            return "Keine Vorschläge";

                        }

                    }

                }
                return ergeb.Substring(0, ergeb.Length - 2);
            }
            return "Keine Vorschläge";
        }
        public static string ChkDDC_UKat(string _ddc)
        {
            string ddc2 = _ddc.Substring(0, _ddc.Length - 2);
            string ergeb = null;
            int i = 0; // für Prüfung ob Ziffer (mit int.TryParse...)
            int ls = 2;
            if (string.IsNullOrEmpty(_ddc) == false)
            {
                ddc2 = ddc2.Replace(" ", "");
                List<string> ddc_name = ddc2.Split(';').ToList();
                string vor = ddc_name[0];
                foreach (var name in ddc_name)
                {
                    if (name.Length > 3)
                    {
                        ls = 3;
                    }
                    ddc2 = name.Substring(0, ls);
                    //if (int.TryParse(ddc2, out i) == true)
                    //{
                    if (Char.IsNumber(ddc2, 0) == true && ddc2 != vor && int.TryParse(ddc2, out i) == true)
                    {
                        try
                        {
                            var ddc = (from d in Admin.conn.DDC_1000 where d.DDC.StartsWith(ddc2) select new { code = d.DDC, DDCname = d.DDC_Name }).FirstOrDefault();
                            vor = ddc2;
                            ergeb += ddc.code + ", " + ddc.DDCname + "\r\n";
                        }
                        catch (Exception)
                        {

                            return "Keine Vorschläge";

                        }

                    }
                    //}

                }
                return ergeb.Substring(0, ergeb.Length - 2);
            }
            return "Keine Vorschläge";
        }

        public static void LoadGenre(UniformGrid _ugrid)
        {
            var gen = from g in Admin.conn.Sachgruppe where g.Marked == true orderby g.SortBy select g;
            foreach (var g in gen)
            {
                System.Windows.Controls.CheckBox newCkBox = new System.Windows.Controls.CheckBox();
                newCkBox.Content = g.Sachgruppe1;
                _ugrid.Children.Add(newCkBox);

                newCkBox.Tag = g.GenreID;
            }
        }

      
    }




    public class NameRolle
    {
        public string name { get; set; }
        public string rolle { get; set; }
        public string nameInDB { get; set; }
        public Int32 currID { get; set; }
        public Int32 currRolleID { get; set; }
        public Int32 currNameRolleID { get; set; }

        public NameRolle()
        {

        }
    }

    public class DnbVerlag
    {
        public string verlag { get; set; }
        public string verlagInDB { get; set; }
        public Int32 verlID { get; set; }
    }

    public class DnbPrinter
    {
        public string druckerei { get; set; }
        public string druckereiInDB { get; set; }
        public Int32 drlID { get; set; }
    }

    public  class mySettings
    {
        public static string CoverPath { get; set; }
        public static Int32 StarRolle { get; set; }
        public static string DNB_API { get; set; }
        public static string Google_API { get; set; }

        public static mySettings loadSetting()
        {
            mySettings cms = new mySettings();
            var set = from ms in Admin.conn.Settings select ms;
            foreach (var item in set)
            {
                CoverPath = item.CoverPath;
                StarRolle = (Int32)item.StartRolle;
                DNB_API = item.DNB_API;
                Google_API = item.Google_API;
            }
            return cms;  
        }
    }




}


