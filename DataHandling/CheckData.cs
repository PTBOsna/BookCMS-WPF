using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCMS_WPF.DataHandling
{
    class CheckData
    {
        public static string CheckISBN(string newISBN)
        {

            var buch = (from b in Admin.conn.Buch
                        where b.ISBN == newISBN
                        select new { Titel = b.Titel, Autor = b.AutorSort }).FirstOrDefault();
            if (buch != null)
            {
                return buch.Titel + " von " + buch.Autor;

            }
            else
                return null;
        }
        public static string CheckTitel(string newTitel)
        {
            string ergeb=null;
            newTitel = newTitel.ToUpper();
            newTitel = newTitel.Replace(" ", "");
            int len = newTitel.Length;
            if (len > 19)
                { len = 19; }

            newTitel = newTitel.Substring(0, len);
            var buch = from b in Admin.conn.Buch
                        where b.TitelIndex.Contains(newTitel)
                        select new { Titel = b.Titel, Autor = b.AutorSort };
            if (buch != null)
            {
                foreach (var item in buch)
                {
                    ergeb += item.Titel + " von " + item.Autor + "\r\n";

                }
                return ergeb ;

            }
            else
                return null;
        }
    }



}

