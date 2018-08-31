using DBManager;
using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Infrastructure.Wrappers
{
    /// <summary>
    /// Mimics a Test Entity to Ease Listing, runs eventHandler in parent interface when IsSelection is changed
    /// </summary>
    public class ReportItemWrapper : BindableBase, ISelectableRequirement
    {
        private bool _isSelected;
        private IRequirementSelector _parent;
        private Requirement _instance;

        public ReportItemWrapper(Requirement instance,
                                IRequirementSelector parent)
        {
            _instance = instance;
            _isSelected = false;
            _parent = parent;
        }

        public double Duration
        {
            get { return _instance.MethodVariant.Method.Duration; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                _parent.OnRequirementSelectionChanged();
                RaisePropertyChanged("IsSelected");
            }
        }

        public string Method => _instance.MethodVariant.StandardName;

        public string Notes
        {
            get { return _instance?.Description; }
            set
            {
                _instance.Description = value;
            }
        }

        public string Property
        {
            get { return _instance.MethodVariant.PropertyName; }
        }
        
        public Requirement Requirement => _instance;

        public Requirement RequirementInstance
        {
            get { return _instance; }
        }

        public IEnumerable<SubRequirement> SubRequirements
        {
            get
            {
                return _instance.SubRequirements.ToList();
            }
        }

        public IEnumerable<SubRequirement> SubTests => _instance.SubRequirements.ToList();
        
        public double? WorkHours => _instance?.MethodVariant?.Method?.Duration;

        public string PropertyName => _instance?.MethodVariant?.PropertyName;

        public string MethodName => _instance?.MethodVariant?.StandardName;

        public IEnumerable SubItems => _instance.SubRequirements.ToList();
    }
}
