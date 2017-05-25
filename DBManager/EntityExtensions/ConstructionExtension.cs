using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ConstructionExtension
    {
        public static void SetAspect(this Construction entry,
                                    Aspect aspectEntity)
        {
            // Sets a construction Aspect properties

            entry.Aspect = aspectEntity;
            entry.AspectID = (aspectEntity == null) ? 0 : aspectEntity.ID;
        }
    }
}
