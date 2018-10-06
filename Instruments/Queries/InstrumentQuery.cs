using DataAccess;
using LabDbContext;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query Object that returns an Instrument
    /// </summary>
    public class InstrumentQuery : IScalar<Instrument, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a Code that will be searched For
        /// If an ID is also provided it will have precedence over the Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets an ID to search for
        /// </summary>
        public int? ID { get; set; }

        #endregion Properties

        #region Methods

        public Instrument Execute(LabDbEntities context)
        {
            if (ID != null)
                return context.Instruments.FirstOrDefault(inst => inst.ID == ID);

            return context.Instruments.FirstOrDefault(inst => inst.Code == Code);
        }

        #endregion Methods
    }
}