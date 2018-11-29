using DataAccessCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class LInstData : DataServiceBase<LInstContext>
    {
        public LInstData(IDesignTimeDbContextFactory<LInstContext> dBContextFactory) : base(dBContextFactory)
        {
        }

    }
}
