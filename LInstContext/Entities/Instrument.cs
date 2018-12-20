using System;
using System.Collections.Generic;

namespace LInst
{
    public class Instrument
    {
        #region Constructors

        public Instrument()
        {
            Code = "";
            Description = "";
            IsInService = true;
            IsUnderControl = false;
            MaintenanceEvents = new HashSet<InstrumentMaintenanceEvent>();
            InstrumentFiles = new HashSet<InstrumentFile>();
            CalibrationReports = new HashSet<CalibrationReport>();
            CalibrationsAsReference = new HashSet<CalibrationReportReference>();
            InstrumentProperties = new HashSet<InstrumentProperty>();
        }

        #endregion Constructors

        #region Properties

        public DateTime? CalibrationDueDate { get; set; }
        public int? CalibrationInterval { get; set; }
        public ICollection<CalibrationReport> CalibrationReports { get; set; }
        public Organization CalibrationResponsible { get; set; }
        public int? CalibrationResponsibleID { get; set; }
        public ICollection<CalibrationReportReference> CalibrationsAsReference { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        public ICollection<InstrumentFile> InstrumentFiles { get; set; }
        public ICollection<InstrumentProperty> InstrumentProperties { get; set; }
        public InstrumentType InstrumentType { get; set; }
        public int InstrumentTypeID { get; set; }
        public bool IsInService { get; set; }
        public bool IsUnderControl { get; set; }
        public ICollection<InstrumentMaintenanceEvent> MaintenanceEvents { get; set; }
        public Organization Manufacturer { get; set; }
        public int? ManufacturerID { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public Organization Supplier { get; set; }
        public int? SupplierID { get; set; }
        public InstrumentUtilizationArea UtilizationArea { get; set; }
        public int UtilizationAreaID { get; set; }

        #endregion Properties
    }
}