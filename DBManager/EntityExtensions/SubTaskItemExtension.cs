using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class SubTaskItem
    {
        /// <summary>
        /// Creates a SubTest entry with the values currently loaded in the SubTaskItem instance
        /// </summary>
        /// <returns>The new SubTest entity</returns>
        public SubTest GetSubTest()
        {
            SubTest tempSubTest = new SubTest()
            {
                Name = Name,
                RequiredValue = RequiredValue,
                SubRequiremntID = SubRequirementID,
                SubMethodID = SubMethodID,
                UM = UM
            };

            return tempSubTest;
        }

    }
}
