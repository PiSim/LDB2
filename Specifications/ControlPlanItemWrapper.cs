using DBManager;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{

    internal class ControlPlanItemWrapper : BindableBase
    {
        private bool _isSelected;
        private ControlPlan _parent;
        private Requirement _requirement;

        internal ControlPlanItemWrapper(ControlPlan parent,
                                        Requirement requirement) : base()
        {
            _parent = parent;
            _requirement = requirement;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value)
                {
                    ControlPlanItem temp = new ControlPlanItem();
                    temp.Method = _requirement.Method;
                    _parent.ControlPlanItems.Add(temp);
                }

                else
                {
                    _parent.ControlPlanItems.First(cpi => cpi.MethodID == _requirement.MethodID);
                }

                OnPropertyChanged("IsSelected");
            }
        }

        public string Method
        {
            get { return _requirement.Method.Standard.Name; }
        }

    }
}
