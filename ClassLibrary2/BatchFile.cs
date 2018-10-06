using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class BatchFile
    {
        public int ID { get; private set; }
        public int BatchID { get; private set; }
        public string Path { get; set; }
        public string Description { get; set; }

        public virtual Batch Batch { get; set; }
    }
}