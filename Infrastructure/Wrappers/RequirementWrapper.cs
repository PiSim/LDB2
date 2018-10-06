using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Wrappers
{
    public class IsOverrideChangedEventArgs : EventArgs
    {
        public Requirement RequirementInstance { get; set; }
        public bool IsOverride { get; set; }
    }
    
    public class RequirementWrapper : BindableBase
    {
        public event EventHandler<IsOverrideChangedEventArgs> IsOverrideChanged;

        #region Constructors

        public RequirementWrapper(Requirement instance) : base()
        {
            RequirementInstance = instance;
        }

        #endregion Constructors

        #region Properties
        
        public bool IsOverride
        {
            get { return RequirementInstance.IsOverride; }

            set
            {
                RaiseIsOverrideChanged();
            }
        }

        public string Method => RequirementInstance.MethodVariant.Method.Standard.Name;

        public string Notes
        {
            get { return RequirementInstance.Description; }
            set
            {
                RequirementInstance.Description = value;
            }
        }

        public string Property => RequirementInstance.MethodVariant.Method.Property.Name;


        public Requirement RequirementInstance
        {
            get; set;
        }

        public IEnumerable<SubRequirement> SubRequirements => RequirementInstance.SubRequirements
                                            .ToList();

        public string VariantName => RequirementInstance?.VariantName;

        #endregion Properties

        // Method definitions

        #region Methods


        private void RaiseIsOverrideChanged()
        {
            IsOverrideChanged?.Invoke(this, new IsOverrideChangedEventArgs() { IsOverride = IsOverride, RequirementInstance = RequirementInstance });
        }

        #endregion Methods
    }
}