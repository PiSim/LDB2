using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class ReportFile
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int reportID { get; set; }
        public string Path { get; set; }

        public virtual Report Report { get; set; }
    }
}