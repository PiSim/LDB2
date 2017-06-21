using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class MaterialMainViewModel : BindableBase
    {
        private Material _selectedMaterial;
        private EventAggregator _eventAggregator;

        public MaterialMainViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public string MaterialDetailRegionName
        {
            get { return RegionNames.MaterialDetailRegion; }
        }

        public IEnumerable<Material> MaterialList
        {
            get { return MaterialService.GetMaterials(); }
        }

        public Material SelectedMaterial
        {
            get { return _selectedMaterial; }
            set
            {
                _selectedMaterial = value;
                _selectedMaterial.Load();

                RaisePropertyChanged();

                NavigationToken token = new NavigationToken(MaterialViewNames.MaterialDetail,
                                                            _selectedMaterial,
                                                            RegionNames.MaterialDetailRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }

    }
}
