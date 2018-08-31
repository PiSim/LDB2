
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
        void ModifyMethodTestList(Method toBeModified);
        /// <summary>
        /// Inserts a list of MethodVariants in the database, or updates the values if the entry already exists
        /// </summary>
        /// <param name="variantList">An IEnumerable containing the MethodVariant instances to update/insert</param>
        void UpdateMethodVariantRange(IEnumerable<MethodVariant> variantList);

        void UpdateRequirements(IEnumerable<Requirement> enumerable);
        void UpdateSubMethods(IEnumerable<SubMethod> subMethodList);
        void DeleteStandard(Std standardInstance);
    }
}
