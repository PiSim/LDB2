using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public static class BatchExtension
    {
        
        public static void SetMaterial(this Batch entry,
                                        Material materialEntity)
        {
            // Sets material and materialID values of a given material entity

            if (entry == null)
                throw new NullReferenceException();

            entry.Material = materialEntity;
            entry.MaterialID = (materialEntity == null) ? 0 : materialEntity.ID;
        }
    }
}
