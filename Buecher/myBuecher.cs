using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookCMS_WPF.DataHandling;

namespace BookCMS_WPF.Buecher
{
    class myBuecher
    {
		private int _ID { get; set; }

		private System.Nullable<bool> _Marked { get; set; } 

		private System.Nullable<short> _MediaCount{ get; set; }

		private string _Titel{ get; set; }

		private string _TitelIndex{ get; set; }

		private string _AutorSort{ get; set; }

		private string _Signatur{ get; set; }

		private string _Veroeffentlicht{ get; set; }

		private string _CopyrightDatum{ get; set; }

		private System.Nullable<int> _TypID{ get; set; }

		private string _Untertitel{ get; set; }

		private string _TitleSort{ get; set; }

		private System.Nullable<int> _VerlagsID{ get; set; }

		private string _ISBN{ get; set; }

		private string _DNB{ get; set; }

		private string _DDC{ get; set; }

		private string _LCCN{ get; set; }

		private string _LCCallNum{ get; set; }

		private System.Nullable<int> _LandD{ get; set; }

		private System.Nullable<int> _SprachenID{ get; set; }

		private System.Nullable<int> _DruckereiID{ get; set; }

		private System.Nullable<int> _BindungID{ get; set; }

		private System.Nullable<int> _AuflageID{ get; set; }

		private string _Auiflage{ get; set; }

		private System.Nullable<int> _DruckID{ get; set; }

		private System.Nullable<int> _SerienID{ get; set; }

		private string _Seiten{ get; set; }

		private System.Nullable<short> _Abschnitte{ get; set; }

		private string _OriginalTitel{ get; set; }

		private string _OriginalUntertitel{ get; set; }

		private System.Nullable<int> _OriginaVerlagID{ get; set; }

		private System.Nullable<int> _OriginalLandID{ get; set; }

		private System.Nullable<int> _OriginalSpracheID{ get; set; }

		private string _OriginalCopyright{ get; set; }

		private string _Preisangabe{ get; set; }

		private string _Value{ get; set; }

		private string _Preis{ get; set; }

		private System.Nullable<int> _ZustandID{ get; set; }

		private System.Nullable<int> _GutachterID{ get; set; }

		private System.Nullable<int> _Versicherung{ get; set; }

		private string _Registeriert{ get; set; }

		private System.Nullable<int> _StatusID{ get; set; }

		private string _Erworben{ get; set; }

		private System.Nullable<int> _ErworbenVonID{ get; set; }

		private System.Nullable<int> _PersonalRatingID{ get; set; }

		private System.Nullable<int> _BesitzerID{ get; set; }

		private System.Nullable<int> _StandortID{ get; set; }

		private System.Nullable<int> _EntleiherID{ get; set; }

		private string _DatumAusleihe{ get; set; }

		private string _RueckgabeDatum{ get; set; }

		private bool _verliehen{ get; set; }

		private bool _zurueckerhalten{ get; set; }

		private string _zurueckDatum{ get; set; }

		private System.Nullable<int> _zurueckVonID{ get; set; }

		private string _EmailOverdueDate{ get; set; }

		private string _EmailReminderDate{ get; set; }

		private string _EmailReservedDate{ get; set; }

		private string _Anmerkungen_PlainText{ get; set; }

		private string _Synopsis_PlainText{ get; set; }

		private string _Reviews_PlainText{ get; set; }

		private string _BarCode{ get; set; }

		private System.Nullable<int> _OriginalSerieID{ get; set; }

		private string _zuletztGelesen{ get; set; }

		private System.Nullable<short> _AnzahlGelesen{ get; set; }

		private System.Nullable<int> _ZustandSchutzumschlagID{ get; set; }

		private string _Dim_Width{ get; set; }

		private string _Dim_Height{ get; set; }

		private string _Dim_Depth{ get; set; }

		private string _Verkaufspreis{ get; set; }

		private System.Nullable<int> _WaehrungID{ get; set; }

		private System.Nullable<int> _VerlagsOrtID{ get; set; }

		private string _ASIN{ get; set; }

		private System.Nullable<System.DateTime> _LetzteAenderung{ get; set; }

		private string _FreigabeNr{ get; set; }

		private string _OriginalFreigabeNr{ get; set; }

		private System.Nullable<int> _KategorieID{ get; set; }

		private System.Nullable<int> _UnterkategorieID{ get; set; }

		private System.Nullable<int> _SachgruppeID{ get; set; }

		private string _Stichworte{ get; set; }

		public  myBuecher(Int32 buchID)
		{
			var selBuch = from b in Admin.conn.Buch where b.ID == buchID select b;
			foreach (var el in selBuch)
			{
				_ID = el.ID;
				_Marked = (bool)el.Marked;
				_MediaCount = (short)el.MediaCount;
				_Titel = el.Titel;
				_TitelIndex = el.TitelIndex;
				_AutorSort = el.AutorSort;
				_Signatur = el.Signatur;
				_Veroeffentlicht = el.Veroeffentlicht;
				_CopyrightDatum = el.CopyrightDatum;
				_TypID = el.TypID;
				_Untertitel = el.Untertitel;
				_TitleSort = el.TitleSort; ;
				_VerlagsID = el.VerlagsID;
				_ISBN = el.ISBN;
				_DNB = el.DNB;
				_DDC = el.DDC;
				_LCCN = el.LCCN;
				_LCCallNum = el.LCCN;
				_LandD = el.LandD;
				_SprachenID = el.SprachenID;
				_DruckereiID = el.DruckereiID;
				_BindungID = el.BindungID;
				_AuflageID = el.AuflageID;
				_Auiflage = el.Auiflage;
				_DruckID = el.DruckID;
				_SerienID = el.SerienID;
				_Seiten = el.Seiten;
				_Abschnitte = (short)el.Abschnitte;
				_OriginalTitel = el.OriginalTitel;
				_OriginalUntertitel = el.OriginalUntertitel;
				_OriginaVerlagID = el.OriginaVerlagID;
				_OriginalLandID = el.OriginalLandID;
				_OriginalSpracheID = el.OriginalSpracheID;
				_OriginalCopyright = el.OriginalCopyright;
				_Preisangabe = el.Preisangabe;
				_Value = el.Value;
				_Preis = el.Preis;
				_ZustandID = el.ZustandID;
				_GutachterID = el.GutachterID;
				_Versicherung = el.Versicherung;
				_Registeriert = el.Registeriert;
				_StatusID = el.StatusID;
				_Erworben = el.Erworben;
				_ErworbenVonID = el.ErworbenVonID;
				_PersonalRatingID = el.PersonalRatingID;
				_BesitzerID = el.BesitzerID;
				_StandortID = el.StandortID;
				_EntleiherID = el.EntleiherID;
				_DatumAusleihe = el.DatumAusleihe;
				_RueckgabeDatum = el.RueckgabeDatum;
				_verliehen = el.verliehen;
				_zurueckerhalten = el.zurueckerhalten;					   
				_zurueckDatum = el.zurueckDatum;
				_zurueckVonID = el.zurueckVonID;
				_EmailOverdueDate = el.EmailOverdueDate;
				_EmailReminderDate = el.EmailReminderDate;
				_EmailReservedDate = el.EmailReservedDate;
				_Anmerkungen_PlainText = el.Anmerkungen_PlainText;
				_Synopsis_PlainText = el.Synopsis_PlainText;
				_Reviews_PlainText = el.Reviews_PlainText;
				_BarCode = el.BarCode;
				_OriginalSerieID = el.OriginalSerieID;
				_zuletztGelesen = el.zuletztGelesen;
				_AnzahlGelesen = el.AnzahlGelesen;
				_ZustandSchutzumschlagID = el.ZustandSchutzumschlagID;
				_Dim_Width = el.Dim_Width;
				_Dim_Height = el.Dim_Height;
				_Dim_Depth = el.Dim_Depth;
				_Verkaufspreis = el.Verkaufspreis;
				_WaehrungID = el.WaehrungID;
				_VerlagsOrtID = el.VerlagsOrtID;
				_ASIN = el.ASIN;
				_LetzteAenderung = (DateTime) el.LetzteAenderung;
				_FreigabeNr = el.FreigabeNr;
				_OriginalFreigabeNr = el.OriginalFreigabeNr;
				_KategorieID = el.KategorieID;
				_UnterkategorieID = el.UnterkategorieID;
				_SachgruppeID = el.SachgruppeID;
				_Stichworte = el.Stichworte;

			}
		}
	}
}
