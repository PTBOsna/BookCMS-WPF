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

		private System.Nullable<int> _Seiten{ get; set; }

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
			var selBuecher = from b in Admin.conn.Buch where b.ID == buchID select b;
			
		}
	}
}
