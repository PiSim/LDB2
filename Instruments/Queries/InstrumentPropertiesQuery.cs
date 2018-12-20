using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple InstrumentProperty entries given an InstrumentID
    /// </summary>
    public class InstrumentPropertiesQuery : QueryBase<InstrumentProperty, LInstContext>
    {
        #region Constructors

        public InstrumentPropertiesQuery(int instrumentID)
        {
            InstrumentID = instrumentID;
        }

        #endregion Constructors

        #region Properties

        public bool ExcludeNonCalibrationProperties { get; set; } = false;

        public int? InstrumentID { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<InstrumentProperty> Execute(LInstContext context)
        {
            IQueryable<InstrumentProperty> query = context.InstrumentProperties;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(iprop => iprop.Name);

            if (ExcludeNonCalibrationProperties)
                query = query.Where(iprop => iprop.IsCalibrationProperty);

            return query.Where(iprop => iprop.InstrumentID == InstrumentID);
        }

        #endregion Methods
    }
}