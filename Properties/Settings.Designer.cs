﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookCMS_WPF.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\bin\\Daten\\BookCMS-T" +
            "est.accdb")]
        public string BookCMS_TestConnectionString {
            get {
                return ((string)(this["BookCMS_TestConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=BookCMS-localDB;Integrated Sec" +
            "urity=True")]
        public string BookCMS_localDBConnectionString {
            get {
                return ((string)(this["BookCMS_localDBConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=\"D:\\USERS\\PETER\\DOCUMENTS\\VISU" +
            "AL STUDIO 2017\\PROJEKTE\\BOOKCMS-LOCALDB\\BOOKCMS-LOCALDB\\BIN\\DEBUG\\BOOKSQL-DB.MDF" +
            "\";Integrated Security=True")]
        public string D__USERS_PETER_DOCUMENTS_VISUAL_STUDIO_2017_PROJEKTE_BOOKCMS_LOCALDB_BOOKCMS_LOCALDB_BIN_DEBUG_BOOKSQL_DB_MDFConnectionS {
            get {
                return ((string)(this["D__USERS_PETER_DOCUMENTS_VISUAL_STUDIO_2017_PROJEKTE_BOOKCMS_LOCALDB_BOOKCMS_LOCA" +
                    "LDB_BIN_DEBUG_BOOKSQL_DB_MDFConnectionS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Books_from_Access;Integrated S" +
            "ecurity=True")]
        public string Books_from_AccessConnectionString {
            get {
                return ((string)(this["Books_from_AccessConnectionString"]));
            }
        }
    }
}
