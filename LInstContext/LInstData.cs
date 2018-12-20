using DataAccessCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LInst
{
    public class LInstData : DataServiceBase<LInstContext>
    {
        #region Constructors

        public LInstData(IDesignTimeDbContextFactory<LInstContext> dBContextFactory) : base(dBContextFactory)
        {
        }

        #endregion Constructors
    }
}