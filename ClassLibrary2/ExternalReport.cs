﻿using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class ExternalReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExternalReport()
        {
            this.ExternalReportFiles = new HashSet<ExternalReportFile>();
            this.Deprecated = new HashSet<Method>();
            this.TestRecords = new HashSet<TestRecord>();
            this.MethodVariants = new HashSet<MethodVariant>();
        }

        public int ID { get; set; }
        public string Description { get; set; }
        public int ExternalLabID { get; set; }
        public bool MaterialSent { get; set; }
        public bool RequestDone { get; set; }
        public string Samples { get; set; }
        public bool ReportReceived { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public int Year { get; set; }
        public int Number { get; set; }
        public string OrderNumber { get; set; }
        public double OrderTotal { get; set; }
        public Nullable<System.DateTime> MaterialSentDate { get; set; }
        public Nullable<System.DateTime> RequestDoneDate { get; set; }
        public Nullable<System.DateTime> ReportReceivedDate { get; set; }
        public bool HasOrder { get; set; }
        public string TBDel { get; set; }
        public Nullable<int> TBDel2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalReportFile> ExternalReportFiles { get; set; }
        public virtual Organization ExternalLab { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Method> Deprecated { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestRecord> TestRecords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodVariant> MethodVariants { get; set; }
    }
}