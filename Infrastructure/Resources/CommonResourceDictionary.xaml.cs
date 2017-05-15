using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Resources
{
    public partial class CommonResourceDictionary
    {
        private static readonly CommonResourceDictionary _instance = new CommonResourceDictionary();

        public CommonResourceDictionary()
        {
            InitializeComponent();
        }

        public static CommonResourceDictionary Instance
        {
            get { return _instance; }
        }

    }
}
