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
        private string _regionName;

        public object ObjectInstance
        {
            get { return _instance; }
        }

        public string RegionName
        {
            get { return _regionName; }
        }
    }
}
