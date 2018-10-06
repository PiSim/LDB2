using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class UpdateEntryCommand<T> : ICommand<LabDbEntities>
    {

        public UpdateEntryCommand (T entry)
        {

        }

        public void Execute(LabDbEntities context)
        {

        }
    }
}
