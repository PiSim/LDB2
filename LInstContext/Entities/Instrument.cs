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
        }

        public int ID { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public int InstrumentTypeID { get; set; }
        public int? SupplierID { get; set; }
        public int? ManufacturerID { get; set; }
        public int? CalibrationResponsibleID { get; set; }

        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public int UtilizationAreaID { get; set; }

        public bool IsInService { get; set; }
        public bool IsUnderControl { get; set; }

        public DateTime? CalibrationDueDate { get; set; }
        public int? CalibrationInterval { get; set; }

        public Organization CalibrationResponsible { get; set; }
        public Organization Manufacturer { get; set; }
        public Organization Supplier { get; set; }

        public ICollection<InstrumentMaintenanceEvent> MaintenanceEvents { get; }
        public ICollection<InstrumentFile> InstrumentFiles { get; }
        public ICollection<CalibrationReport> CalibrationReports { get; }
    }
}
