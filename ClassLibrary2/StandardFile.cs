using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class StandardFile
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int StandardID { get; set; }

        public virtual Std Standard { get; set; }
    }
}