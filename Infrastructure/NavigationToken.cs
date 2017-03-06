using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class NavigationToken
    {
        private object _instance;
        private string _regionName, _viewName;

        public NavigationToken(string viewName,
                                string regionName = RegionNames.MainRegion,
                                object instance = null)
        {
            _instance = instance;
            _regionName = regionName;
            _viewName = viewName;
        }

        public object ObjectInstance
        {
            get { return _instance; }
        }

        public string RegionName
        {
            get { return _regionName; }
        }

        public string ViewName
        {
            get { return _viewName; }
        }
    }
}
