
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager;

namespace Infrastructure
{
    public interface ISpecificationService
    {
        void CreateMethod();
        void UpdateRequirements(IEnumerable<Requirement> enumerable);
        void UpdateSubMethods(IEnumerable<SubMethod> subMethodList);
    }
}
