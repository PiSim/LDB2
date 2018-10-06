using DataAccess;
using LabDbContext;
using System;
using System.Linq;

namespace Materials.Commands
{
    /// <summary>
    /// Command object that updates a Batch entity and its Material data in a single transaction.
    /// Creates new Aspect, MaterialLine, Recipe and Material instances if required.
    /// Colour, ExternalConstruction and Project instances can be passed as
    /// </summary>
    public class BatchUpdateCommand : BatchInsertUpdateBase
    {
        #region Fields

        public Colour _colorInstance;
        public ExternalConstruction _externalConstructionInstance;
        public Project _projectInstance;

        private Batch _batchInstance;
        private Material _materialTemplate;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base Constructor for BatchUpdateCommand
        /// </summary>
        /// <param name="batchInstance">The Batch entity to Update</param>
        /// <param name="materialTemplate">A disconnected material instance to use as a template for the update</param>
        public BatchUpdateCommand(Batch batchInstance,
                                Material materialTemplate)
        {
            ValidateMaterialTemplate(materialTemplate);
            _materialTemplate = materialTemplate;

            _batchInstance = batchInstance ?? throw new InvalidOperationException("A target Batch instance must be provided");
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Runs the Command Routine over a given context
        /// </summary>
        /// <param name="context">A LabDbEntities context</param>
        public override void Execute(LabDbEntities context)
        {
            _context = context;

            Batch attachedBatch = _context.Batches.Find(_batchInstance.ID);
            _context.Entry(attachedBatch).CurrentValues.SetValues(_batchInstance);
            attachedBatch.Material = GetOrCreateMaterial(_materialTemplate);
            _context.SaveChanges();
        }


    #endregion Methods
    }
}