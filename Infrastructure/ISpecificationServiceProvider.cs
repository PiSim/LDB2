using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISpecificationServiceProvider
    {
        Specification GetSpecification(int ID);

        void CreateSpecification(Specification entry);
        void DeleteSpecification(Specification entry);
        void LoadSpecification(ref Specification entry);
        void UpdateSpecification(Specification entry);
    }
}
