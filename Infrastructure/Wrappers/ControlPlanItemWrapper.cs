using Infrastructure.Wrappers;
using LabDbContext;
using Prism.Mvvm;
using System.Collections;

namespace Infrastructure
{
    public class ControlPlanItemWrapper : BindableBase, ISelectableRequirement
    {
        #region Fields

        private bool _isSelected;

        #endregion Fields

        #region Constructors

        public ControlPlanItemWrapper(Requirement requirement) : base()
        {
            RequirementInstance = requirement;
        }

        #endregion Constructors

        #region Properties

        public double Duration => 0;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        bool ISelectableRequirement.IsSelected { get => false; set { } }
        public string Method => RequirementInstance.MethodVariant.Method.Standard.Name;

        public string MethodName => null;
        string ITestItem.MethodName => null;
        public string Notes => RequirementInstance.Description;
        public string Property => RequirementInstance.MethodVariant.Method.Property.Name;
        public string PropertyName => null;
        string ITestItem.PropertyName => null;
        public Requirement RequirementInstance { get; }

        Requirement ISelectableRequirement.RequirementInstance => null;
        IEnumerable ITestItem.SubItems => null;
        public IEnumerable SubTests => null;
        public double WorkHours => 0;
        double? ITestItem.WorkHours => 0;

        #endregion Properties
    }
}