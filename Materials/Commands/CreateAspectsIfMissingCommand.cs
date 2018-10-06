using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.Commands
{
    /// <summary>
    /// Command objects that iterates through an IEnumerable of Aspect entries,
    /// checking if an entry with the same code exists in the database and inserting it if it doesn't
    /// </summary>
    public class CreateAspectsIfMissingCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<Aspect> _aspectList;

        /// <summary>
        /// Base Constructor For CreateAspectsIfMissingCommand
        /// </summary>
        /// <param name="aspectList">The List of Aspect entities to check</param>
        public CreateAspectsIfMissingCommand(IEnumerable<Aspect> aspectList)
        {
            _aspectList = aspectList;
        }

        public void Execute(LabDbEntities context)
        {
            foreach (Aspect newAsp in _aspectList)
                if (!context.Aspects.Any(asp => asp.Code == newAsp.Code))
                    context.Aspects.Add(newAsp);

            context.SaveChanges();
        }
    }
}
