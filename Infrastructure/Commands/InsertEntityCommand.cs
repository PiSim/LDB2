using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command object that inserts a given entity in the database
    /// </summary>
    public class InsertEntityCommand : ICommand<LabDbEntities>
    {
        private object _entity;

        public InsertEntityCommand(object entity)
        {
            _entity = entity;
        }


        public void Execute(LabDbEntities context)
        {
            context.Entry(_entity).State = System.Data.Entity.EntityState.Added;
            context.SaveChanges();
        }
    }
}
