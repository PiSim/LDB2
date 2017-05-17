using DBManager;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class MaterialInfoMainViewModel
    {
        public MaterialInfoMainViewModel()
        {

        }
        
        public string MaterialInfoAspectRegionName
        {
            get { return RegionNames.MaterialInfoAspectRegion; }
        }

        public string MaterialInfoColourRegionName
        {
            get { return RegionNames.MaterialInfoColourRegion; }
        }

        public string MaterialInfoConstructionRegionName
        {
            get { return RegionNames.MaterialInfoCostructionRegion; }
        }
    }
}
