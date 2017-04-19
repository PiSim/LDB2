using DBManager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    public class SpecificationServiceProvider
    {
        private DBEntities _entities;
        private UnityContainer _container;

        public SpecificationServiceProvider(DBEntities entities,
                                            UnityContainer container)
        {
            _entities = entities;
            _container = container;
        }
    }
}
