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
        public static List<string> myAlpha = new List<string> { "A", "B", "C", "D", "E", "F", "G", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    }
}
