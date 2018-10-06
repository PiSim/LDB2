namespace LabDbContext.EntityExtensions
{
    public static class OrganizationRoleExtension
    {
        #region Methods

        public static void Create(this OrganizationRole entry)
        {
            // Inserts a new OrganizationRole entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.OrganizationRoles.Add(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}