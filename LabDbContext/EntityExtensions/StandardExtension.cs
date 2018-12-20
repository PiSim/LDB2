using System;
using System.Collections.Generic;
using System.Linq;

namespace LabDbContext
{
    public partial class Std
    {
        #region Methods

        /// <summary>
        /// Returns all the StandardFiles related to this standard
        /// </summary>
        /// <returns>An IEnumerable of StandardFiles entities</returns>
        [Obsolete]
        public IEnumerable<StandardFile> GetFiles()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardFiles
                                .Where(stf => stf.StandardID == ID)
                                .ToList();
            }
        }

        #endregion Methods
    }
}