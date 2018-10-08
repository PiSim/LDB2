namespace LabDbContext
{
    public partial class SubTaskItem
    {
        #region Methods

        /// <summary>
        /// Creates a SubTest entry with the values currently loaded in the SubTaskItem instance
        /// </summary>
        /// <returns>The new SubTest entity</returns>
        public SubTest GetSubTest()
        {
            SubTest tempSubTest = new SubTest()
            {
                Name = Name,
                RequiredValue = RequiredValue,
                SubRequiremntID = SubRequirementID,
                SubMethodID = SubMethodID,
                UM = UM
            };

            return tempSubTest;
        }

        #endregion Methods
    }
}