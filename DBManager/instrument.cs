//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Instrument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instrument()
        {
            this.CalibrationReports = new HashSet<CalibrationReport>();
            this.CalibrationReportsAsReference = new HashSet<CalibrationReport>();
            this.PendingCalibrations = new HashSet<PendingCalibration>();
            this.Tests = new HashSet<Test>();
            this.MaintenanceEvent = new HashSet<InstrumentMaintenanceEvent>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int InstrumentTypeID { get; set; }
        public sbyte ControlPeriod { get; set; }
        public bool IsUnderControl { get; set; }
        public Nullable<int> supplierID { get; set; }
        public Nullable<int> manufacturerID { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReport> CalibrationReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReport> CalibrationReportsAsReference { get; set; }
        public virtual InstrumentType InstrumentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PendingCalibration> PendingCalibrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public virtual Organization Manufacturer { get; set; }
        public virtual Organization Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentMaintenanceEvent> MaintenanceEvent { get; set; }
    }
}
