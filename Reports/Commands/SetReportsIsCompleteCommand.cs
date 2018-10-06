using DataAccess;
using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Reports.Commands
{
    /// <summary>
    /// Command object that sets the
    /// </summary>
    public class SetReportsIsCompleteCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private IEnumerable<Report> _reportList;
        private bool _status;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="reportList">An IEnumerable containing the instances to update</param>
        /// <param name="statusToSet">A bool representing the completion status that will be set in the given Entities</param>
        public SetReportsIsCompleteCommand(IEnumerable<Report> reportList,
                                            bool statusToSet)
        {
            _reportList = reportList;
            _status = statusToSet;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            foreach (Report rep in _reportList)
            {
                Report connectedEntity = context.Reports.FirstOrDefault(connrep => connrep.ID == rep.ID);
                if (connectedEntity != null)
                    connectedEntity.IsComplete = _status;
            }

            context.SaveChanges();
        }

        #endregion Methods
    }
}