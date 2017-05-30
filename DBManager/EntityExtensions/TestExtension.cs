using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class TestExtension
    {

        public static void CreateTests(this IEnumerable<Test> testList)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Tests.AddRange(testList);
                entities.SaveChanges();
            }
        }

        public static void SetMethod(this Test entry,
                                    Method methodEntity)
        {
            entry.Method = methodEntity;
            entry.MethodID = (methodEntity == null) ? 0 : methodEntity.ID;
        }
    }
}
