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
        private ControlPlanItem _item;
        private Requirement _requirement;

        internal ControlPlanItemWrapper(ControlPlan parent,
                                        Requirement requirement) : base()
        {
            _parent = parent;
            _requirement = requirement;
            _item = _parent.ControlPlanItems.FirstOrDefault(cpi => cpi.MethodID == _requirement.MethodID);
            _isSelected = _item != null;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value)
                {
                    _item = new ControlPlanItem();
                    _item.Method = _requirement.Method;
                    _parent.ControlPlanItems.Add(_item);
                }

                else
                {
                    _parent.ControlPlanItems.Remove(_item);
                    _item = null;
                }

                _isSelected = value;
                OnPropertyChanged("IsSelected");
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
    }
}
