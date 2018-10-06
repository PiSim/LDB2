using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class CalibrationResult
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CalibrationResult()
        {
            this.CalibrationReports = new HashSet<CalibrationReport>();
        }

        public int ID { get; set; }
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReport> CalibrationReports { get; set; }
    }
}