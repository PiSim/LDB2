using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class RequirementOverrideToken
    {
        private object _requirement, _specificationVersion;

        public RequirementOverrideToken(object requirement, object specVersion)
        {
            _requirement = requirement;
            _specificationVersion = specVersion;
        }

        public object Requirement
        {
            get { return _requirement; }
        }

        public object SpecificationVersion
        {
            get { return _specificationVersion; }
        }
    }
}
