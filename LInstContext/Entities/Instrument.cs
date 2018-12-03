using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LInst
{
    public class Instrument
    {
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
    }

        public int ID { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public int InstrumentTypeID { get; set; }
        public InstrumentType InstrumentType { get; set; }

        public int? SupplierID { get; set; }
        public Organization Supplier { get; set; }

        public int? ManufacturerID { get; set; }
        public Organization Manufacturer { get; set; }

        public int? CalibrationResponsibleID { get; set; }
        public Organization CalibrationResponsible { get; set; }

        public string SerialNumber { get; set; }
        public string Model { get; set; }

        public int UtilizationAreaID { get; set; }
        public InstrumentUtilizationArea UtilizationArea { get; set; }

        public bool IsInService { get; set; }
        public bool IsUnderControl { get; set; }

        public DateTime? CalibrationDueDate { get; set; }
        public int? CalibrationInterval { get; set; }
               
        public ICollection<InstrumentMaintenanceEvent> MaintenanceEvents { get; set; }
        public ICollection<InstrumentFile> InstrumentFiles { get; set; }
        public ICollection<CalibrationReport> CalibrationReports { get; set; }
        public ICollection<CalibrationReportReference> CalibrationsAsReference { get; set; }
    }
}
