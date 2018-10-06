using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Sample
    {
        public int ID { get; set; }
        public int BatchID { get; set; }
        public string Code { get; set; }
        public System.DateTime Date { get; set; }
        public int personID { get; set; }

        public virtual Batch Batch { get; set; }
        public virtual Person LogAuthor { get; set; }
    }
}