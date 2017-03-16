using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class RequirementWrapper : BindableBase
    {
        private DBEntities _entities;
        private Requirement _requirementInstance;
        private SpecificationVersion _versionInstance;

        internal RequirementWrapper(Requirement instance,
                                    SpecificationVersion version,
                                    DBEntities entities) : base()
        {
            _entities = entities;
            _requirementInstance = instance;
            _versionInstance = version;
        }

        public bool CanSetOverride
        {
            get { return !_versionInstance.IsMain;  }
        }
        
        public string Description
        {
            get { return _requirementInstance.Description; }
            set
            {
                _requirementInstance.Description = value;
            }
        }

        public bool IsOverride
        {
            get { return (_requirementInstance.IsOverride == 1) ? true : false; }

            set
            {
                if (value)
                    _requirementInstance = _entities.AddOverride(_versionInstance, _requirementInstance);
                else
                    _requirementInstance = _entities.RemoveOverride(_requirementInstance);

                OnPropertyChanged("SubRequirements");
            }
        }

        public List<SubRequirement> SubRequirements
        {
            get { return new List<SubRequirement>(_requirementInstance.SubRequirements); }
        }

        public string Standard
        {
            get
            {
                return _requirementInstance.Method.Standard.Name;
            }
        }

        public string Test
        {
            get { return _requirementInstance.Method.Property.Name; }
        }
    }

}
