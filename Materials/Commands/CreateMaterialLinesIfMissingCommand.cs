using DataAccess;
using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Materials.Commands
{
    /// <summary>
    /// Command objects that iterates through an IEnumerable of MaterialLine entries,
    /// checking if an entry with the same code exists in the database and inserting it if it doesn't
    /// </summary>
    public class CreateMaterialLinesIfMissingCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private IEnumerable<MaterialLine> _lineList;

        #endregion Fields

        #region Constructors

        public CreateMaterialLinesIfMissingCommand(IEnumerable<MaterialLine> lineList)
        {
            _lineList = lineList;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            foreach (MaterialLine newLine in _lineList)
            {
                if (!context.MaterialLines.Any(lin => lin.Code == newLine.Code))
                    context.MaterialLines.Add(newLine);
            }

            context.SaveChanges();
        }

        #endregion Methods
    }
}