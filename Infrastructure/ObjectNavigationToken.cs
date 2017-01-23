using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ObjectNavigationToken
    {
        private object _instance;
        private string _viewName;

        public ObjectNavigationToken(object instance, string viewName)
        {
            _instance = instance;
            _viewName = viewName;
        }

        public object ObjectInstance
        {
            get { return _instance; }
        }

        public string ViewName
        {
            get { return _viewName; }
        }
    }
}
