using DBManager;
using DBManager.EntityExtensions;
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
        private IDataService _dataService;
        private Requirement _requirementInstance;
        private SpecificationVersion _versionInstance;

        public RequirementWrapper(Requirement instance,
                                    SpecificationVersion version,
                                    IDataService dataService) : base()
        {
            _dataService = dataService;
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
        
        public string Notes
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

                _requirementInstance.Load();
                RaisePropertyChanged("CanModify");
                RaisePropertyChanged("SubRequirements");
            }
        }
        
        public Requirement RequirementInstance
        {
            get { return _requirementInstance; }
        }

        public IEnumerable<SubRequirement> SubRequirements
        {
            get { return _requirementInstance.SubRequirements
                                            .ToList(); }
        }

        public string Method
        {
            get
            {
                return _requirementInstance.Method.Standard.Name;
            }
        }

        public string Property
        {
            get {

                return _requirementInstance.Method.Property.Name; }
        }

        public bool ReadOnly
        {
            get { return !CanModify; }
        }

        // Method definitions

        public void AddOverride()
        {
            Requirement newOverride = new Requirement
            {
                Description = _requirementInstance.Description,
                IsOverride = true,
                MethodID = _requirementInstance.MethodID,
                OverriddenID = _requirementInstance.ID,
                SpecificationVersionID = _versionInstance.ID
            };

            foreach (SubRequirement subReq in _requirementInstance.SubRequirements)
            {
                SubRequirement tempSub = new SubRequirement
                {
                    SubMethodID = subReq.SubMethodID,
                    RequiredValue = subReq.RequiredValue
                };

                newOverride.SubRequirements.Add(tempSub);
            }
            
            newOverride.Create();
            _requirementInstance = newOverride;
        }

        private void RemoveOverride()
        {
            int overrID = (int)_requirementInstance.OverriddenID;
            _requirementInstance.Delete();
            _requirementInstance = _dataService.GetRequirement(overrID);
        }
    }

}
