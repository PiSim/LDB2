namespace LabDbContext
{
    public partial class SubRequirement
    {
        #region Properties

        public bool IsUpdated { get; set; } = false;

        public string Name => SubMethod?.Name;

        public string UM => SubMethod?.UM;

        #endregion Properties
    }
}