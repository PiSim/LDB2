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

        /// <summary>
        /// Consolidates a list of Std entries into a single one, redirecting all references
        /// All the entries that are left unused are deleted
        /// </summary>
        /// <param name="standardList">The list of standards to consolidate</param>
        /// <param name="mainEntry">The entry that will incorporate all the others</param>
        public void ConsolidateStandard(IEnumerable<Std> standardList,
                                        Std mainEntry)
        {
            foreach (Std currentStd in standardList)
            {
                // if currentStd is mainentry, skip

                if (currentStd.ID == mainEntry.ID)
                    continue;

                // Retrieve method list, set new reference and update

                foreach (Method mtd in currentStd.GetMethods())
                {
                    mtd.StandardID = mainEntry.ID;
                    mtd.Update();
                }

                // Retrieve specification list, set new reference and update

                foreach (Specification spc in currentStd.GetSpecifications())
                {
                    spc.StandardID = mainEntry.ID;
                    spc.Update();
                }

                // Call method to delete currentStd

                DeleteStandard(currentStd);
            }
        }

        /// <summary>
        /// Deletes a Standard instance and raises the appropriate StandardChanged Event
        /// </summary>
        /// <param name="standardInstance">The Instance to delete</param>
        public void DeleteStandard(Std standardInstance)
        {
            standardInstance.Delete();
            _eventAggregator.GetEvent<StandardChanged>()
                            .Publish(new EntityChangedToken(standardInstance,
                                                            EntityChangedToken.EntityChangedAction.Deleted));
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
