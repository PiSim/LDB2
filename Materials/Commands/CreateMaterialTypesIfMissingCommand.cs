using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.Commands
{
    /// <summary>
    /// Command objects that iterates through an IEnumerable of MaterialType entries,
    /// checking if an entry with the same code exists in the database and inserting it if it doesn't
    /// </summary>
    public class CreateMaterialTypesIfMissingCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<MaterialType> _typeList;

        public CreateMaterialTypesIfMissingCommand(IEnumerable<MaterialType> typeList)
        {
            _typeList = typeList;
        }

        public void Execute(LabDbEntities context)
        {
            foreach (MaterialType newType in _typeList)
            {
                if (!context.MaterialTypes.Any(typ => typ.Code == newType.Code))
                    context.MaterialTypes.Add(newType);
            }

            context.SaveChanges();
        }
    }
}
