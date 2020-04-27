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
                var buch = (from b in Admin.conn.Buch where b.ID == bookID select b).FirstOrDefault();
                Admin.conn.Buch.DeleteOnSubmit(buch);

                //AutorBuch löschen
                //testen, ob Autor mehrfach vorhanden
              
                //zunächst PersonID's finden
                var buchPersonen = from bp in Admin.conn.AutorBuchLink where bp.BuchID == bookID select bp;

                foreach (var personen in buchPersonen)
                {
                    var person = from p in Admin.conn.AutorBuchLink where p.PersonID == personen.PersonID select p;
                    //System.Windows.Forms.MessageBox.Show(person.Count().ToString());
                    if (person.Count()==1)
                    {
                        //DelPerson(personen.PersonID);
                        var delPers = from dp in Admin.conn.Person where dp.PersonID == personen.PersonID select dp;
                        if (delPers.Count() == 1)
                        {
                            Admin.conn.Person.DeleteAllOnSubmit(delPers);
                            //DelAutorBuchLink(personen.id);  
                            //Admin.conn.AutorBuchLink.DeleteAllOnSubmit(buchPersonen);
                        }
                        else //delAutorBuchLink(personen.id);
                            Admin.conn.AutorBuchLink.DeleteAllOnSubmit(buchPersonen);
                     
                    }
                    Admin.conn.AutorBuchLink.DeleteAllOnSubmit(buchPersonen);
                }

                //GenreLink löschen
                var genLink = from gl in Admin.conn.GenreLink where gl.BuchID == bookID select gl;
                if (genLink.Count() > 0)
                {
                    Admin.conn.GenreLink.DeleteAllOnSubmit(genLink);
                }


                Admin.conn.SubmitChanges();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
