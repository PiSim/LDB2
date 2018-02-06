using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    public class StandardMainViewModel : BindableBase
    {
        public StandardMainViewModel()
        {

        }

        public string MethodMainRegionName => RegionNames.MethodMainRegion;
        public string SpecificationMainRegionName => RegionNames.SpecificationMainRegion;
        public string StandardMainRegionName => RegionNames.StandardMainRegion;

    }
}
