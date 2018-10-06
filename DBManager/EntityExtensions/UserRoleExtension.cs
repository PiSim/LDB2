namespace LabDbContext.EntityExtensions
{
    public static class UserRoleExtension
    {
        #region Methods

        public static void Create(this UserRole entry)
        {
            // Inserts a new UserRole in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.UserRoles.Add(entry);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}