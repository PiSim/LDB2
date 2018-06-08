using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class TestQuery : IQuery<Test>
    {
        string _methodName;

        public TestQuery()
        {

        }

        public string MethodName
        {
            get => _methodName;
            set => _methodName = value;
        }

        public IQueryable<Test> RunQuery(DBEntities entities)
        {
            IQueryable<Test> query = entities.Tests.Include(tst => tst.TestRecord.Batch.Material.Aspect);

            if (!string.IsNullOrWhiteSpace(_methodName))
                query = query.Where(tst => tst.Method.Name.Contains(_methodName));

            return query;
        }
    }
}
