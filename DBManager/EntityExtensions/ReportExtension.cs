using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ReportExtension
    {
        public static void SetBatch(this Report entry,
                                    Batch batchEntity)
        {
            entry.Batch = batchEntity;
            entry.BatchID = (batchEntity == null) ? 0 : batchEntity.ID;
        }

        public static void SetSpecificationVersion(this Report entry,
                                                    SpecificationVersion specificationVersionEntity)
        {
            entry.SpecificationVersion = specificationVersionEntity;
            entry.SpecificationVersionID = (specificationVersionEntity == null) ? 0 : specificationVersionEntity.ID;
        }
    }
}
