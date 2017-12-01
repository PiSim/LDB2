using DBManager;
using Infrastructure.Wrappers;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Infrastructure
{

    public class ControlPlanItemWrapper : BindableBase, ISelectableRequirement
    {
        private bool _isSelected;
        private Requirement _requirement;

        public ControlPlanItemWrapper(Requirement requirement) : base()
        {
            _requirement = requirement;
        }



        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public string Method
        {
            get { return _requirement.Method.Standard.Name; }
        }

        public string Property
        {
            get { return _requirement.Method.Property.Name; }
        }

        public double Duration => throw new NotImplementedException();

        public Requirement RequirementInstance
        {
            get { return _requirement; }
        }

        public double WorkHours => throw new NotImplementedException();

        public string PropertyName => throw new NotImplementedException();

        public string MethodName => throw new NotImplementedException();

        public string Notes => _requirement.Description;

        public IEnumerable SubTests => throw new NotImplementedException();

        bool ISelectableRequirement.IsSelected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Requirement ISelectableRequirement.RequirementInstance => throw new NotImplementedException();

        string ITestItem.PropertyName => throw new NotImplementedException();

        string ITestItem.MethodName => throw new NotImplementedException();

        IEnumerable ITestItem.SubItems => throw new NotImplementedException();

        double? ITestItem.WorkHours => throw new NotImplementedException();
    }
}
