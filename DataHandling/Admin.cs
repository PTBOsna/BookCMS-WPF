using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
public class NameRolle
{
    public string name { get; set; }
    public string rolle { get; set; }
    public string nameInDB { get; set; }
    public Int32 currID { get; set; }
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
