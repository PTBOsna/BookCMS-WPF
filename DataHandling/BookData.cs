using BookCMS_WPF.DataHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookCMS_WPF.DataHandling
{

    public class DNBBookData
    {
        public string dnb_nr { get; set; }                  //016 a
        public string dnb_isbn { get; set; }                //020 a
        public string dnb_preis { get; set; }               //020 c
        public string dnb_isbn_ { get; set; }               //020 9
        public string dnb_isbn_13 { get; set; }             //024 a
        public string dnb_sprache { get; set; }             //041 a
        public string dnb_sprache_org { get; set; }         //041 h
        public string dnb_dcc1 { get; set; }                //082 a
        public string dnb_dcc2 { get; set; }                //084 a
        public string dnb_Autor_sort { get; set; }          //100 a
        public string dnb_Rolle { get; set; }               //100 4
        public string dnb_rolle_lang { get; set; }          //100 e
        public string dnb_titel_org { get; set; }           //240 a
        public string dnb_titel { get; set; }               //245 a
        public string dnb_untertitel { get; set; }          //245 b
        public string dnb_autor { get; set; }               //245 c
        public string dnb_auflage { get; set; }             //250 a
        public string dnb_verlagsort { get; set; }          //264 a
        public string dnb_verlagsname { get; set; }         //264 b
        public string dnb_jahr { get; set; }                //264 c
        public string dnb_seiten { get; set; }              //300 a
        public string dnb_dim { get; set; }                 //300 c
        public string dnb_begleit { get; set; }             //300 e
        public string dnb_sammlung { get; set; }            //490 a
        public string dnb_stichwort { get; set; }           //650 a
        public string dnb_index { get; set; }               //653 a
        public string dnb_mitautor { get; set; }            //700 a
        public string dnb_mitautor_rolle { get; set; }      //700 4
        public string dnb_mitautor_rolle_lang { get; set; } //700 e
        public string dnb_thema { get; set; }               //926 x



        public DNBBookData(string inputStr)
        {
            using (StringReader reader = new StringReader(inputStr))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("tag=\"016\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_nr = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"020\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_isbn = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"c\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_preis = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"9\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_isbn_ = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"024\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_isbn_13 = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }

                    if (line.Contains("tag=\"041\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_sprache = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"h\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_sprache_org = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"082\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_dcc1 = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"084\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_dcc2 = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            line = reader.ReadLine();
                            //return list;
                        }

                    }

                    if (line.Contains("tag=\"100\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_Autor_sort = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"e\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_rolle_lang = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"4\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_Rolle = SelectString(line); // Add to list.
                                //MessageBox.Show(SelectString(line));
                            }
                            line = reader.ReadLine();
                            //return list;
                        }
                    }
                    if (line.Contains("tag=\"240\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_titel_org = SelectString(line); // Add to list.
                                                                    //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"245\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_titel = SelectString(line);

                                //List<string> infolist = new List<string>();
                                if (dnb_titel.StartsWith("&#"))
                                {
                                    string[] erg = dnb_titel.Split(';');

                                    dnb_titel = erg[1].Substring(0, 3) + erg[2];
                                }// Add to list.
                                 //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"b\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_untertitel = SelectString(line); // Add to list.
                                                                     //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"c\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_autor = SelectString(line); // Add to list.
                                                                //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }
                    }
                    if (line.Contains("tag=\"250\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_auflage = SelectString(line); // Add to list.
                                                                  //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"264\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_verlagsort = SelectString(line); // Add to list.
                                                                     //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"b\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_verlagsname = SelectString(line); // Add to list.
                                                                      //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"c\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_jahr = SelectString(line); // Add to list.
                                                               //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;

                        }
                    }
                    if (line.Contains("tag=\"300\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_seiten = SelectString(line); // Add to list.
                                                                 //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"c\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_dim = SelectString(line); // Add to list.
                                                              //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_begleit = SelectString(line); // Add to list.
                                                                  //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;


                        }
                    }
                    if (line.Contains("tag=\"490\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_sammlung = SelectString(line); // Add to list.
                                                                   //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }

                    if (line.Contains("tag=\"650\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //tag 650 ist rekursiev!
                                dnb_stichwort += SelectString(line) + ";"; // Add to list.
                                                                           //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }

                    if (line.Contains("tag=\"653\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //653 ist rekursiv
                                dnb_index += SelectString(line) + ";"; // Add to list.
                                                                       //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                    if (line.Contains("tag=\"700\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"a\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_mitautor += SelectString(line) + ";"; // Add to list.
                                                                   //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"e\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_mitautor_rolle_lang = SelectString(line); // Add to list.
                                                                              //MessageBox.Show(SelectString(line));
                            }
                            if (line.Contains("code=\"4\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_mitautor_rolle += SelectString(line) + ";"; // Add to list.
                                                                         //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                        }   //return list;
                    }
                    if (line.Contains("tag=\"926\""))
                    {
                        line = reader.ReadLine();
                        while (line.Contains("</datafield>") == false)
                        {
                            if (line.Contains("code=\"x\""))
                            {
                                //LBShow.Items.Add(SelectString(line)); // Write to console.
                                dnb_thema = SelectString(line); // Add to list.
                                                                //MessageBox.Show(SelectString(line));
                            }

                            line = reader.ReadLine();
                            //return list;
                        }

                    }
                }
            }


        }
        public static string SelectString(string myString)
        {
            string[] stringPart = myString.Split('>');
            string[] ret = stringPart[1].Split('<');

            return ret[0];

        }
    }
}
public class InfoMulti
{

    public static string _InfoMulti(string myString)
    {
        //InfoMulti erg = new InfoMulti();
        string input = null;
        //List<string> erg = new List<string>();
        using (StringReader reader = new StringReader(myString))
        {

            var titelMulti = new List<InfoMulti[]>();
            string line;
            //int count = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("tag=\"016\""))
                {
                    line = reader.ReadLine();
                    while (line.Contains("</datafield>") == false)
                    {
                        if (line.Contains("code=\"a\""))
                        {
                            //LBShow.Items.Add(SelectString(line)); // Write to console.
                            ////MessageBox.Show(SelectString(line).ToString());
                            //dummy += SelectString(line) + "#";// Add to list.

                            input += DNBBookData.SelectString(line) + "; ";
                        }
                        //erg.Add(input);
                        line = reader.ReadLine();

                    }
                }

                if (line.Contains("tag=\"245\""))
                {
                    line = reader.ReadLine();
                    while (line.Contains("</datafield>") == false)
                    {
                        if (line.Contains("code=\"a\""))
                        {
                            //LBShow.Items.Add(SelectString(line)); // Write to console.
                            ////MessageBox.Show(SelectString(line).ToString());
                            //dummy += SelectString(line) + "#";// Add to list.
                            string cName = DNBBookData.SelectString(line);
                            if (cName.StartsWith("&#"))
                            {
                                string[] erg = cName.Split(';');
                                cName = erg[1].Substring(0, 3) + erg[2];
                            }
                            input += cName + "; ";
                        }

                        line = reader.ReadLine();

                    }
                }
                if (line.Contains("tag=\"250\""))
                {
                    line = reader.ReadLine();
                    while (line.Contains("</datafield>") == false)
                    {
                        if (line.Contains("code=\"a\""))
                        {
                            //LBShow.Items.Add(SelectString(line)); // Write to console.
                            ////MessageBox.Show(SelectString(line).ToString());
                            //dummy += SelectString(line) + "#";// Add to list.

                            input += DNBBookData.SelectString(line) + "; ";
                        }

                        line = reader.ReadLine();

                    }
                }
                if (line.Contains("tag=\"264\""))
                {
                    line = reader.ReadLine();
                    while (line.Contains("</datafield>") == false)
                    {
                        if (line.Contains("code=\"c\""))
                        {
                            //LBShow.Items.Add(SelectString(line)); // Write to console.
                            ////MessageBox.Show(SelectString(line).ToString());
                            //dummy += SelectString(line) + "#";// Add to list.

                            input += DNBBookData.SelectString(line) + "~";
                        }

                        line = reader.ReadLine();

                    }

                }
                //count += 1;
                //MessageBox.Show("count: " + count.ToString());
                //erg.Add(input);
            }
        }
        return input;


    }

}











