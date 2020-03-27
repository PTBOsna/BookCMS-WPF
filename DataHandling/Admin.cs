using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace BookCMS_WPF.DataHandling
{
    class Admin
    {
        public static BuchDataClassesDataContext conn = new BuchDataClassesDataContext();
        public static List<string> myAlpha = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static string currDNB_ID;

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
            url[0]= url[0].Replace(';', '&');
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
