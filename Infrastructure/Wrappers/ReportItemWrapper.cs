using LabDbContext;
using Prism.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Wrappers
{
    /// <summary>
    /// Mimics a Test Entity to Ease Listing, runs eventHandler in parent interface when IsSelection is changed
    /// </summary>
    public class ReportItemWrapper : BindableBase, ISelectableRequirement
    {
        #region Fields

        private bool _isSelected;
        private IRequirementSelector _parent;

        #endregion Fields

        #region Constructors

        public ReportItemWrapper(Requirement instance,
                                IRequirementSelector parent)
        {
            RequirementInstance = instance;
            _isSelected = false;
            _parent = parent;
        }

        #endregion Constructors

        #region Properties

        public double Duration => RequirementInstance.MethodVariant.Method.Duration;

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

        public string Method => RequirementInstance.MethodVariant.StandardName;

        public string MethodName => RequirementInstance?.MethodVariant?.StandardName;

        public string Notes
        {
            get { return RequirementInstance?.Description; }
            set
            {
                RequirementInstance.Description = value;
            }
        }

        public string Property => RequirementInstance.MethodVariant.PropertyName;

        public string PropertyName => RequirementInstance?.MethodVariant?.PropertyName;
        public Requirement Requirement => RequirementInstance;

        public Requirement RequirementInstance { get; }

        public IEnumerable SubItems => RequirementInstance.SubRequirements.ToList();
        public IEnumerable<SubRequirement> SubRequirements => RequirementInstance.SubRequirements.ToList();

        public IEnumerable<SubRequirement> SubTests => RequirementInstance.SubRequirements.ToList();

        public double? WorkHours => RequirementInstance?.MethodVariant?.Method?.Duration;

        #endregion Properties
    }
}