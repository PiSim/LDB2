using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class CalibrationFiles
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int ReportID { get; set; }

        public virtual CalibrationReport CalibrationReport { get; set; }
    }
}