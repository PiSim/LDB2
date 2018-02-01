using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Specifications
{
    public class SpecificationService : ISpecificationService
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public SpecificationService(EventAggregator eventAggregator,
                                    IDataService dataService,
                                    IUnityContainer container)
        {
            _eventAggregator = eventAggregator;
            _container = container;
        }

        public void CreateMethod()
        {
            Views.MethodCreationDialog creationDialog =
                        _container.Resolve<Views.MethodCreationDialog>();

            if (creationDialog.ShowDialog() == true)
                _eventAggregator.GetEvent<MethodChanged>()
                                .Publish(new EntityChangedToken(creationDialog.MethodInstance,
                                                                EntityChangedToken.EntityChangedAction.Created));
        }


        public void UpdateRequirements(IEnumerable<Requirement> requirementEntries)
        {
            // Updates all the Requirement entries passed as parameter

            using (DBEntities entities = new DBEntities())
            {
                foreach (Requirement req in requirementEntries)
                {
                    entities.Requirements.AddOrUpdate(req);
                    foreach (SubRequirement sreq in req.SubRequirements)
                        entities.SubRequirements.AddOrUpdate(sreq);
                }

                entities.SaveChanges();
            }
        }

        public void UpdateSubMethods(IEnumerable<SubMethod> methodEntries)
        {
            // Updates all the SubMethod entries

            using (DBEntities entities = new DBEntities())
            {
                foreach (SubMethod smtd in methodEntries)
                    entities.SubMethods.AddOrUpdate(smtd);

                entities.SaveChanges();
            }
        }
    }
}
