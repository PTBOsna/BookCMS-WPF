using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookCMS_WPF.DataHandling
{
    public class DNBDataHandling
    {
       
        
        public static DNBBookData GetDataDNB(string dnbID)
        {

            string s = null; 
             string sampleXml = @"H:\VisualStudio-Projekte\BookCMS-WPF\test5.mrcx";
        //string key = "1159cfc6b965e8a03abc3bd8227afa"; //TODO: wird später aus den Settings entnommen
        string key = mySettings.DNB_API;
            //MessageBox.Show(sett.CoverPath);
            WebClient w = new WebClient();
            w.Encoding = Encoding.UTF8;

            //MessageBox.Show(cSignatur);
            if (dnbID == "#")
            { //für Testlauf
                s = File.ReadAllText(sampleXml);
            }
            else
            {
                string urlEnc = WebUtility.UrlEncode(dnbID);
                s = w.DownloadString("https://services.dnb.de/sru/dnb?version=1.1&operation=searchRetrieve&query=" + urlEnc + "&recordSchema=MARC21-xml&accessToken=" + key);

            }
            DNBBookData db = new DNBBookData(s);


            ////Prüfen ob Cover vorhanden (Abfrage mit allen drei ISBN-Varianten)
            //string imgUrl = @"https://portal.dnb.de/opac/mvb/cover.htm?isbn=";

            //if (Admin.IsPageValid(imgUrl + db.dnb_isbn_) == true)
            //{
            //    imgUrl += db.dnb_isbn_;
            //}
            //else if (Admin.IsPageValid(imgUrl + db.dnb_isbn) == true)
            //{
            //    imgUrl += db.dnb_isbn;
            //}
            //else if (Admin.IsPageValid(imgUrl + db.dnb_isbn_13) == true)
            //{
            //    imgUrl += db.dnb_isbn_13;
            //}
            //else
            //{
            //    cbSaveCover.IsChecked = false;
            //    lbCoverDNB.Content = "Kein Conver vorhanden!";
            //    return;
            //}
            //Uri myUri = new Uri(imgUrl);
            //HandleImage(MyImage, myUri);
            //imgLoad(imgUrl);
            ////MessageBox.Show(ImgBox.Width.ToString());
            return db;
        }

        public static string GetCover(DNBBookData db)
        {

            //Prüfen ob Cover vorhanden (Abfrage mit allen drei ISBN-Varianten)
            string imgUrl = @"https://portal.dnb.de/opac/mvb/cover.htm?isbn=";

            if (Admin.IsPageValid(imgUrl + db.dnb_isbn_) == true)
            {
                imgUrl += db.dnb_isbn_;
            }
            else if (Admin.IsPageValid(imgUrl + db.dnb_isbn) == true)
            {
                imgUrl += db.dnb_isbn;
            }
            else if (Admin.IsPageValid(imgUrl + db.dnb_isbn_13) == true)
            {
                imgUrl += db.dnb_isbn_13;
            }
            else
            { return null; }

            return imgUrl;
        }
    }
}
