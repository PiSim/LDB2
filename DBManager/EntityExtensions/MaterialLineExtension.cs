namespace LabDbContext.EntityExtensions
{
    public static class MaterialLineExtension
    {
        #region Methods

        public static void Create(this MaterialLine entry)
        {
            // Inserts a new MaterialLine entry

            using (LabDbEntities entitites = new LabDbEntities())
            {
                entitites.MaterialLines.Add(entry);

                entitites.SaveChanges();
            }
        }

        #endregion Methods
    }
}