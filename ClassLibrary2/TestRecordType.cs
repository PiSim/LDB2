using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class TestRecordType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestRecordType()
        {
            this.TestRecords = new HashSet<TestRecord>();
        }

        public int ID { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestRecord> TestRecords { get; set; }
    }
}