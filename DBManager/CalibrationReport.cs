//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class CalibrationReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CalibrationReport()
        {
            this.CalibrationFiles = new HashSet<CalibrationFiles>();
            this.ReferenceInstruments = new HashSet<Instrument>();
            this.InstrumentMeasurablePropertyMappings = new HashSet<CalibrationReportInstrumentPropertyMapping>();
        }
    
        public int ID { get; set; }
        public int Year { get; set; }
        public int Number { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> OperatorID { get; set; }
        public int laboratoryID { get; set; }
        public int instrumentID { get; set; }
        public string Notes { get; set; }
        public string Result { get; set; }
    
        public virtual Instrument Instrument { get; set; }
        public virtual Organization Laboratory { get; set; }
        public virtual Person Tech { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationFiles> CalibrationFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Instrument> ReferenceInstruments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReportInstrumentPropertyMapping> InstrumentMeasurablePropertyMappings { get; set; }
    }
}
