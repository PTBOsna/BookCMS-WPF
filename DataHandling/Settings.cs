using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace BookCMS_WPF.DataHandling
{
    public class Settings
    {
        public string PathCover { get; set; }
        public Int32 StartRolle { get; set; }
        public string dnb_api { get; set; }
        public string google_api { get; set; }

    }

    public class HandleXML
    {
        public static void WriteXML(string path, Settings cSetting)
        {

            XmlWriterSettings xmlSet = new XmlWriterSettings();
            xmlSet.Indent = true;
            xmlSet.IndentChars = "  "; // 2 Leerzeichen
            XmlWriter writer = XmlWriter.Create(path, xmlSet);
            writer.WriteStartDocument();
            // Start-Tag des Stammelements
            writer.WriteStartElement("Settings");
            writer.WriteComment("Die Datei wurde mit XmlWriter erzeugt");
            // Start-Tag von 'Person'
            writer.WriteStartElement("sett");
            writer.WriteElementString("PathCover", cSetting.PathCover);
            writer.WriteElementString("StartRolle", cSetting.StartRolle.ToString());
            writer.WriteElementString("dnb_api", cSetting.dnb_api);
            writer.WriteElementString("google_api", cSetting.google_api);

            writer.WriteEndElement();

            writer.WriteEndElement();

            //writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public static void ReadXML(string path)
        {
            Settings cSet = new Settings();
            XPathDocument xPathDoc = new XPathDocument(@"H:\test.xml");
            XPathNavigator navigator = xPathDoc.CreateNavigator();
            navigator.MoveToChild("Settings", "");
            navigator.MoveToChild("sett", "");
            // alle untergeordneten Elemente einer 'Person'
            XPathNodeIterator descendant =
            navigator.SelectDescendants("", "", false);

            //Console.WriteLine(trenner);
            while (descendant.MoveNext())
            {
                switch (descendant.Current.Name)
                {
                    case "PathCover":
                        cSet.PathCover = descendant.Current.Value;
                        break;
                    case "StartRolle":
                        cSet.StartRolle = Int32.Parse(descendant.Current.Value);
                        break;
                    case "dnb_api":
                        cSet.dnb_api = descendant.Current.Value;
                        break;
                    case "google_api":
                        cSet.google_api = descendant.Current.Value;
                        break;
                }


            }
        }

    }


}

