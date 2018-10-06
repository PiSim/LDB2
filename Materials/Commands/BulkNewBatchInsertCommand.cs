using LabDbContext;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Materials.Commands
{
    public class BulkNewBatchInsertCommand : BatchInsertUpdateBase
    {
        #region Fields

        private IEnumerable<Batch> _batchList;
        private LinkedList<Batch> _failedBatches;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base Constructor for BulkNewBatchInsertCommand
        /// </summary>
        /// <param name="batchList">A list of batches populated with the required subentities and fields required for insertion</param>
        public BulkNewBatchInsertCommand(IEnumerable<Batch> batchList)
        {
            _batchList = batchList;
            _failedBatches = new LinkedList<Batch>();
        }

        #endregion Constructors

        #region Methods

        public override void Execute(LabDbEntities context)
        {
            _context = context;

            foreach (Batch batchTemplate in _batchList)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    Batch newBatch = new Batch();
                    
                    context.Batches.Add(newBatch);

                    context.Entry(newBatch).CurrentValues.SetValues(batchTemplate);

                    newBatch.Material = context.Materials.First(mat => mat.Aspect.Code == batchTemplate.Material.Aspect.Code &&
                                                                        mat.MaterialLine.Code == batchTemplate.Material.MaterialLine.Code &&
                                                                        mat.Recipe.Code == batchTemplate.Material.Recipe.Code &&
                                                                        mat.MaterialType.Code == batchTemplate.Material.MaterialType.Code);


                    if (batchTemplate.Material.ExternalConstruction != null)
                        newBatch.Material.ExternalConstructionID = batchTemplate.Material.ExternalConstruction.ID;

                    if (batchTemplate.Material.Project != null)
                        newBatch.Material.ProjectID = batchTemplate.Material.Project.ID;

                    if (batchTemplate.Material.Recipe.Colour != null)
                        newBatch.Material.Recipe.ColourID = batchTemplate.Material.Recipe.Colour.ID;

                    try
                    {
                        transaction.Commit();
                    }
                    catch
                    {
                        _failedBatches.AddLast(batchTemplate);
                    }
                }
            }
            context.SaveChanges();
        }

        #endregion Methods

        #region Properties

        public IEnumerable<Batch> FailedBatches => _failedBatches;

        #endregion Properties
    }
}