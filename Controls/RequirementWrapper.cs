using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{
    internal class RequirementWrapper
    {
        private bool _isSelected;
        private Requirement _instance;

        internal RequirementWrapper(Requirement instance)
        {
            _instance = instance;
            _isSelected = true;
        }

        public string Method
        {
            get { return _instance.Method.Standard.Organization.Name + " " + _instance.Method.Standard.Name; }
        }

        public Requirement Instance
        {
            get { return _instance; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public string Property
        {
            get { return _instance.Method.Property.Name; }
        }
    }
}
