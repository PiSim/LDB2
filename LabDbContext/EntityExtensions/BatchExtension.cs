namespace LabDbContext
{
    public partial class Batch
    {
        #region Properties

        public string AspectCode => Material?.Aspect?.Code;

        public bool HasTests => TestRecords.Count != 0;

        public string MaterialFullCode => Material?.MaterialType?.Code
                                        + Material?.MaterialLine?.Code
                                        + Material?.Aspect?.Code
                                        + Material?.Recipe?.Code;

        public string MaterialLineCode => Material?.MaterialLine?.Code;
        public string MaterialTypeCode => Material?.MaterialType?.Code;
        public string RecipeCode => Material?.Recipe?.Code;

        #endregion Properties
    }
}