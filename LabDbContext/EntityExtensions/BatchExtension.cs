using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{

    public partial class Batch
    {
        #region Properties

        public string AspectCode => Material?.Aspect?.Code;

        public bool HasTests
        {
            get
            {
                using (LabDbEntities entities = new LabDbEntities())
                {
                    return entities.TestRecords.Any(tstr => tstr.BatchID == ID);
                }
            }
        }

        public string MaterialFullCode => Material?.MaterialType?.Code
                                        + Material?.MaterialLine?.Code
                                        + Material?.Aspect?.Code
                                        + Material?.Recipe?.Code;

        public string MaterialLineCode => Material?.MaterialLine?.Code;
        public string MaterialTypeCode => Material?.MaterialType?.Code;
        public string RecipeCode => Material?.Recipe?.Code;

        #endregion Properties

    }
}