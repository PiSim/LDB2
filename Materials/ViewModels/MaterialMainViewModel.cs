using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Materials.ViewModels
{
    public class MaterialMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Material _selectedMaterial;

        #endregion Fields

        #region Constructors

        public MaterialMainViewModel(IDataService<LabDbEntities> labDbData,
                                    IEventAggregator eventAggregator)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Properties

        public string MaterialDetailRegionName => RegionNames.MaterialDetailRegion;

        public IEnumerable<Material> MaterialList => _labDbData.RunQuery(new MaterialsQuery())
                                                                .ToList();

        public Material SelectedMaterial
        {
            get { return _selectedMaterial; }
            set
            {
                _selectedMaterial = value;

                RaisePropertyChanged("SelectedMaterial");

                NavigationToken token = new NavigationToken(MaterialViewNames.MaterialDetail,
                                                            _selectedMaterial,
                                                            RegionNames.MaterialDetailRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }

        #endregion Properties
    }
}