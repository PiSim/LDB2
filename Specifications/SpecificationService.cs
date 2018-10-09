using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using Prism.Events;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Specifications
{
    public class SpecificationService : ISpecificationService
    {
        #region Fields

        private IDbContextFactory<LabDbEntities> _dbContextFactory;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public SpecificationService(IDataService<LabDbEntities> labDbData,
                                    IDbContextFactory<LabDbEntities> dbContextFactory,
                                    IEventAggregator eventAggregator)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
            _dbContextFactory = dbContextFactory;
        }

        #endregion Constructors

        #region Methods

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

                foreach (Method mtd in currentStd.Methods)
                {
                    mtd.StandardID = mainEntry.ID;
                    mtd.Update();
                }

                // Retrieve specification list, set new reference and update

                foreach (Specification spc in currentStd.Specifications)
                {
                    spc.StandardID = mainEntry.ID;
                    spc.Update();
                }

                // Call method to delete currentStd

                DeleteStandard(currentStd);
            }
        }

        public void CreateMethod()
        {
            Views.MethodCreationDialog creationDialog = new Views.MethodCreationDialog();

            if (creationDialog.ShowDialog() == true)
            {
                Method newInstance = creationDialog.MethodInstance;

                newInstance.Create();

                _eventAggregator.GetEvent<MethodChanged>()
                                .Publish(new EntityChangedToken(creationDialog.MethodInstance,
                                                                EntityChangedToken.EntityChangedAction.Created));
            }
        }

        public void CreateSpecification()
        {
            Views.SpecificationCreationDialog creationDialog = new Views.SpecificationCreationDialog();

            if (creationDialog.ShowDialog() == true)
            {
                Specification newInstance = creationDialog.SpecificationInstance;

                newInstance.Create();

                _eventAggregator.GetEvent<SpecificationChanged>()
                                .Publish(new EntityChangedToken(newInstance,
                                                                EntityChangedToken.EntityChangedAction.Created));
            }
        }

        /// <summary>
        /// Deletes a Standard instance and raises the appropriate StandardChanged Event
        /// </summary>
        /// <param name="standardInstance">The Instance to delete</param>
        public void DeleteStandard(Std standardInstance)
        {
            _labDbData.Execute(new DeleteEntityCommand());
            _eventAggregator.GetEvent<StandardChanged>()
                            .Publish(new EntityChangedToken(standardInstance,
                                                            EntityChangedToken.EntityChangedAction.Deleted));
        }

        /// <summary>
        /// Begins the process of Modifying a Method's subMethod list
        /// </summary>
        /// <param name="toBeModified"> an instance of the method to alter</param>
        public void ModifyMethodTestList(Method toBeModified)
        {
            toBeModified.LoadSubMethods();

            Views.ModifyMethodSubMethodListDialog modificationDialog = new Views.ModifyMethodSubMethodListDialog();

            modificationDialog.OldVersion = toBeModified;

            if (modificationDialog.ShowDialog() == true)
            {
                using (LabDbEntities entities = _dbContextFactory.Create())
                {
                    /// Retrieves references to up to date entities from the Database

                    Method attachedOldMethod = entities.Methods.FirstOrDefault(mtd => mtd.ID == toBeModified.ID);

                    if (attachedOldMethod == null)
                        return;

                    /// Creates a new method Instance from the old one

                    Method newMethod = new Method()
                    {
                        Description = attachedOldMethod.Description,
                        ShortDescription = attachedOldMethod.ShortDescription,
                        OldVersionID = attachedOldMethod.ID,
                        PropertyID = attachedOldMethod.PropertyID,
                        StandardID = attachedOldMethod.StandardID
                    };

                    entities.Methods.Add(newMethod);

                    attachedOldMethod.IsOld = true;

                    /// Adds the new SubMethod list to the instance

                    int subMethodPositionCounter = 0;

                    foreach (SubMethod smtd in modificationDialog.SubMethodList)
                    {
                        smtd.Position = subMethodPositionCounter++;
                        newMethod.SubMethods.Add(smtd);
                    }

                    /// Updates all the variants

                    ICollection<MethodVariant> oldVariantList = attachedOldMethod.MethodVariants.ToList();

                    foreach (MethodVariant mtdvar in oldVariantList)
                    {
                        mtdvar.IsOld = true;

                        MethodVariant newVariant = new MethodVariant()
                        {
                            Description = mtdvar.Description,
                            Name = mtdvar.Name,
                            PreviousVersionID = mtdvar.ID
                        };

                        newMethod.MethodVariants.Add(newVariant);

                        BuildUpdatedRequirementsForMethodVariant(mtdvar,
                                                                newVariant,
                                                                newMethod,
                                                                entities);
                    }

                    entities.SaveChanges();

                    _eventAggregator.GetEvent<BatchChanged>()
                                    .Publish(new EntityChangedToken(newMethod,
                                                                    EntityChangedToken.EntityChangedAction.Created));
                };
            }
        }

        /// <summary>
        /// Inserts a list of MethodVariants in the database, or updates the values if the entry already exists
        /// </summary>
        /// <param name="variantList">An IEnumerable containing the MethodVariant instances to update/insert</param>
        public void UpdateMethodVariantRange(IEnumerable<MethodVariant> variantList)
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                foreach (MethodVariant mtdvar in variantList)
                    entities.MethodVariants.AddOrUpdate(mtdvar);

                entities.SaveChanges();
            }
        }

        public void UpdateRequirements(IEnumerable<Requirement> requirementEntries)
        {
            // Updates all the Requirement entries passed as parameter

            using (LabDbEntities entities = _dbContextFactory.Create())
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

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                foreach (SubMethod smtd in methodEntries)
                    entities.SubMethods.AddOrUpdate(smtd);

                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Updates all the Requirement entities for an Updated MethodVariant.
        /// Creates/Updates/Deletes Corresponding SubRequirements in order to Match the current SubMethod list of the Method
        /// </summary>
        /// <param name="oldVariant">The MethodVariant to be replaced</param>
        /// <param name="newVariant">The MethodVariant that will replace the old one</param>
        /// <param name="newMethod">A reference to the new Method instance</param>
        /// <param name="entities">A reference to the DB transaction instance</param>
        private void BuildUpdatedRequirementsForMethodVariant(MethodVariant oldVariant,
                                                            MethodVariant newVariant,
                                                            Method newMethod,
                                                            LabDbEntities entities)
        {
            foreach (Requirement req in oldVariant.Requirements.ToList())
            {
                req.MethodVariant = newVariant;

                foreach (SubMethod smtd in newMethod.SubMethods)
                {
                    SubRequirement updateTarget = req.SubRequirements.FirstOrDefault(sreq => sreq.SubMethodID == smtd.OldVersionID);

                    if (updateTarget != null)
                    {
                        updateTarget.SubMethod = smtd;
                        updateTarget.IsUpdated = true;
                    }
                    else
                        req.SubRequirements.Add(new SubRequirement()
                        {
                            IsUpdated = true,
                            RequiredValue = "",
                            SubMethod = smtd,
                        });
                }

                foreach (SubRequirement sreq in req.SubRequirements.Where(nn => !nn.IsUpdated)
                                                                    .ToList())
                    entities.Entry(sreq).State = EntityState.Deleted;
            }
        }

        #endregion Methods
    }
}