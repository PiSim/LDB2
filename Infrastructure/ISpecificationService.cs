
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
        void ConsolidateStandard(IEnumerable<Std> standardList,
                                Std mainEntry);
        void CreateMethod();
        void CreateSpecification();
        void StartMethodUpdate(Method toBeUpdated);
        void UpdateRequirements(IEnumerable<Requirement> enumerable);
        void UpdateSubMethods(IEnumerable<SubMethod> subMethodList);
        void DeleteStandard(Std standardInstance);
    }
}
