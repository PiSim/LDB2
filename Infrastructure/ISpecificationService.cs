using LabDbContext;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface ISpecificationService
    {
        #region Methods

        void ConsolidateStandard(IEnumerable<Std> standardList,
                                Std mainEntry);

        void CreateMethod();

        void CreateSpecification();

        void DeleteStandard(Std standardInstance);

        void ModifyMethodTestList(Method toBeModified);

        /// <summary>
        /// Inserts a list of MethodVariants in the database, or updates the values if the entry already exists
        /// </summary>
        /// <param name="variantList">An IEnumerable containing the MethodVariant instances to update/insert</param>
        void UpdateMethodVariantRange(IEnumerable<MethodVariant> variantList);

        void UpdateRequirements(IEnumerable<Requirement> enumerable);

        void UpdateSubMethods(IEnumerable<SubMethod> subMethodList);

        #endregion Methods
    }
}