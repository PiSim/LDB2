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
                                object instance = null,
                                string regionName = null)
        {
            if (regionName == null)
                regionName = RegionNames.MainRegion;
            else
                _regionName = regionName;
                
            _instance = instance;
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
