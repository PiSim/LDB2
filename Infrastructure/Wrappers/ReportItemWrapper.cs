using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class ReportItemWrapper : BindableBase, ISelectableRequirement
    {
        private bool _isSelected;
        private Requirement _instance;

        public ReportItemWrapper(Requirement instance)
        {
            _instance = instance;
            _isSelected = false;
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
            get { return _instance.Method.Standard.Name; }
        }


        public string Notes
        {
            get { return _instance.Description; }
            set
            {
                _instance.Description = value;
            }
        }

        public string Property
        {
            get { return _instance.Method.Property.Name; }
        }


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
    }
}
