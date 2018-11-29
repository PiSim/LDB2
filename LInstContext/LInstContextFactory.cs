using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class LInstContextFactory : IDesignTimeDbContextFactory<LInstContext>
    {
        public LInstContext CreateDbContext(string[] args)
        {
            return new LInstContext();
        }
    }
}
