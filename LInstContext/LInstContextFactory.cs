using Microsoft.EntityFrameworkCore.Design;

namespace LInst
{
    public class LInstContextFactory : IDesignTimeDbContextFactory<LInstContext>
    {
        #region Methods

        public LInstContext CreateDbContext(string[] args)
        {
            return new LInstContext();
        }

        #endregion Methods
    }
}