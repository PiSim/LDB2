//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            this.SubTests = new HashSet<SubTest>();
        }
    
        public int ID { get; set; }
        public int MethodID { get; set; }
        public int reportID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> operatorID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<int> instrumentID { get; set; }
        public Nullable<int> MethodIssueID { get; set; }
        public Nullable<int> ParentTaskItemID { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual Report Report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
        public virtual Method Method { get; set; }
        public virtual Instrument instrument { get; set; }
        public virtual StandardIssue MethodIssue { get; set; }
        public virtual TaskItem ParentTaskItem { get; set; }
    }
}
