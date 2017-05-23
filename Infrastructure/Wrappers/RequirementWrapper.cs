using DBManager;
using DBManager.Services;
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
        private Requirement _requirementInstance;
        private SpecificationVersion _versionInstance;

        public RequirementWrapper(Requirement instance,
                                    SpecificationVersion version) : base()
        {
            _requirementInstance = instance;
            _versionInstance = version;
        }

        public bool CanModify
        {
            get
            {
                if (_versionInstance.IsMain)
                    return true;

                else
                    return IsOverride;
            }
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
            get { return _requirementInstance.IsOverride; }

            set
            {
                if (value)
                    AddOverride();
                else
                    RemoveOverride();

                RaisePropertyChanged("CanModify");
                RaisePropertyChanged("SubRequirements");
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

        // Method definitions

        public void AddOverride()
        {
            Requirement newOverride = new Requirement();
            newOverride.Description = _requirementInstance.Description;
            newOverride.IsOverride = true;
            newOverride.Method = _requirementInstance.Method;
            newOverride.Overridden = _requirementInstance;
            newOverride.SpecificationVersions = _versionInstance;

            foreach (SubRequirement subReq in _requirementInstance.SubRequirements)
            {
                SubRequirement tempSub = new SubRequirement();

                tempSub.Requirement = newOverride;
                tempSub.SubMethod = subReq.SubMethod;
                tempSub.RequiredValue = subReq.RequiredValue;

                newOverride.SubRequirements.Add(tempSub);
            }

            newOverride.SpecificationVersions = _versionInstance;
            newOverride.Create();

            _requirementInstance = newOverride;
        }

        private void RemoveOverride()
        {
            Requirement tempReq = _requirementInstance.Overridden;
            _requirementInstance.Delete();
            _requirementInstance = tempReq;
        }
    }

}
