using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    public class SpecificationServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private UnityContainer _container;

        public SpecificationServiceProvider(DBEntities entities,
                                            EventAggregator eventAggregator,
                                            UnityContainer container)
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _container = container;

            _eventAggregator.GetEvent<MethodCreationRequested>().Subscribe(
                () => CreateNewMethod());
        }

        private void CreateNewMethod()
        {
            Views.MethodCreationDialog creationDialog =
                        _container.Resolve<Views.MethodCreationDialog>();

            if (creationDialog.ShowDialog() == true)
                _eventAggregator.GetEvent<MethodListUpdateRequested>().Publish();
        }

    }
}
