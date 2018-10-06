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
    /// Command objects that iterates through an IEnumerable of MaterialLine entries,
    /// checking if an entry with the same code exists in the database and inserting it if it doesn't
    /// </summary>
    public class CreateMaterialLinesIfMissingCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<MaterialLine> _lineList;

        public CreateMaterialLinesIfMissingCommand(IEnumerable<MaterialLine> lineList)
        {
            _lineList = lineList;
        }

        public void Execute(LabDbEntities context)
        {
            foreach (MaterialLine newLine in _lineList)
            {
                if (!context.MaterialLines.Any(lin => lin.Code == newLine.Code))
                    context.MaterialLines.Add(newLine);
            }

            context.SaveChanges();
        }
    }
}
