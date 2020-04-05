using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace BookCMS_WPF.DataHandling
{
     public class DeleteBook
    {
        public static bool delBook(int bookID)
        {
            try
            {
            //Buch löschen
            var buch = from b in Admin.conn.Buch where b.ID == bookID select b;
            Admin.conn.Buch.DeleteAllOnSubmit(buch);
            //AutorBuch löschen
            //testen, ob Autor mehrfach vorhanden
            Int32 pID = 0;
            var personenBuch = from p in Admin.conn.AutorBuchLink where p.BuchID == bookID select p;
            if (personenBuch.Count()==1)
            {
                foreach (var eintrag in personenBuch)
                {
                    pID = (Int32) eintrag.PersonID;
                Admin.conn.AutorBuchLink.DeleteAllOnSubmit(personenBuch);
                }

                var c_person = from ps in Admin.conn.Person where ps.PersonID == pID select ps;
                Admin.conn.Person.DeleteAllOnSubmit(c_person);
            }

            //GenreLink löschen
            var genLink = from gl in Admin.conn.GenreLink where gl.BuchID == bookID select gl;
            if (genLink.Count()>0)
            {
                Admin.conn.GenreLink.DeleteAllOnSubmit(genLink);
            }
          

            Admin. conn.SubmitChanges();

            }
            catch (Exception)
            {

                return false;
            }
            //jetzt noch Cover löschen
            //Cover löschen

            string cPath = System.IO.Path.Combine(mySettings.CoverPath, bookID.ToString() + ".jpg");
            try
            {
                File.Delete(cPath);

            }
            catch (Exception ex)
            {
                MessageBox.Show(" - Kein Conver vorhanden");
            }
            return true;
        }
    }
}
