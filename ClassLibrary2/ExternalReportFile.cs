using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class ExternalReportFile
    {
        public int ID { get; private set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int ExternalReportID { get; set; }

        public virtual ExternalReport ExternalReport { get; set; }
    }
}