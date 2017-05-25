using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class TestExtension
    {
        public static void SetMethod(this Test entry,
                                    Method methodEntity)
        {
            entry.Method = methodEntity;
            entry.methodID = (methodEntity == null) ? 0 : methodEntity.ID;
        }
    }
}
