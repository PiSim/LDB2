﻿using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Instrument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instrument()
        {
            this.CalibrationReports = new HashSet<CalibrationReport>();
            this.Tests = new HashSet<Test>();
            this.MaintenanceEvent = new HashSet<InstrumentMaintenanceEvent>();
            this.CalibrationReportAsReference = new HashSet<CalibrationReport>();
            this.AssociatedMethods = new HashSet<Method>();
            this.InstrumentMeasurableProperties = new HashSet<InstrumentMeasurableProperty>();
            this.InstrumentFiles = new HashSet<InstrumentFiles>();
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int InstrumentTypeID { get; set; }
        public Nullable<int> supplierID { get; set; }
        public Nullable<int> manufacturerID { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public Nullable<int> UtilizationAreaID { get; set; }
        public bool IsInService { get; set; }
        public bool IsUnderControl { get; set; }
        public Nullable<int> CalibrationResponsibleID { get; set; }
        public Nullable<System.DateTime> CalibrationDueDate { get; set; }
        public int CalibrationInterval { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReport> CalibrationReports { get; set; }
        public virtual InstrumentType InstrumentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public virtual Organization Manufacturer { get; set; }
        public virtual Organization Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentMaintenanceEvent> MaintenanceEvent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReport> CalibrationReportAsReference { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Method> AssociatedMethods { get; set; }
        public virtual InstrumentUtilizationArea InstrumentUtilizationArea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentMeasurableProperty> InstrumentMeasurableProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentFiles> InstrumentFiles { get; set; }
        public virtual Organization CalibrationResponsible { get; set; }
    }
}