﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }


        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("L:\\Documenti macchine e strumenti\\_Report Tarature Laboratorio")]
        public string CalibrationReportPath
        {
            get
            {
                return ((string)(this["CalibrationReportPath"]));
            }
            set
            {
                this["CalibrationReportPath"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("L:\\RAPPORTI_DI_PROVA\\TEST ESTERNI")]
        public string ExternalReportPath
        {
            get
            {
                return ((string)(this["ExternalReportPath"]));
            }
            set
            {
                this["ExternalReportPath"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("L:\\RAPPORTI_DI_PROVA\\2017")]
        public string ReportPath
        {
            get
            {
                return ((string)(this["ReportPath"]));
            }
            set
            {
                this["ReportPath"] = value;
            }
        }
    }
}
