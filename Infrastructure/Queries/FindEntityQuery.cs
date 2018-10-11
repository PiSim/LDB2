using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class FindEntityQuery<T> : IScalar<T, LabDbEntities>
    {
        private object _primaryKey;

        public FindEntityQuery(object primaryKey)
        {
            _primaryKey = primaryKey;
        }

        public T Execute(LabDbEntities context)
        {
            context.Set<T>(); .Find()
        }
    }
}
