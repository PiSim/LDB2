﻿using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.TaskItems = new HashSet<TaskItem>();
        }

        public int ID { get; set; }
        public int RequesterID { get; set; }
        public int SpecificationVersionID { get; set; }
        public int BatchID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Progress { get; set; }
        public Nullable<int> PriorityModifier { get; set; }
        public Nullable<int> PipelineOrder { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ReportID { get; set; }
        public double WorkHours { get; set; }
        public bool TP1_NONUSARE { get; set; }
        public bool TP2_NONUSARE { get; set; }
        public bool TP3_NONUSARE { get; set; }

        public virtual Person Requester { get; set; }
        public virtual SpecificationVersion SpecificationVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Report Report { get; set; }
    }
}