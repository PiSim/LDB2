using Controls.Views;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for AdminMainView.xaml
    /// </summary>
    public partial class AdminMain : UserControl, IView
    {
        #region Constructors

        public AdminMain(IRegionManager regionManager)
        {
            InitializeComponent();

            regionManager.RegisterViewWithRegion(RegionNames.AdminUserMainRegion,
                                                    typeof(Views.UserMain));
            regionManager.RegisterViewWithRegion(RegionNames.InstrumentTypeManagementRegion,
                                                    typeof(Views.InstrumentTypeMain));
            regionManager.RegisterViewWithRegion(RegionNames.InstrumentUtilizationAreasRegion,
                                                    typeof(Views.InstrumentUtilizationAreaMain));
            regionManager.RegisterViewWithRegion(RegionNames.MeasurableQuantityManagementRegion,
                                                    typeof(Views.MeasurableQuantityMain));
            regionManager.RegisterViewWithRegion(RegionNames.UnitOfMeasurementManagementRegion,
                                                    typeof(Views.MeasurementUnitMain));
            regionManager.RegisterViewWithRegion(RegionNames.OrganizationRoleManagementRegion,
                                                    typeof(Views.OrganizationsMain));
            regionManager.RegisterViewWithRegion(RegionNames.PeopleManagementRegion,
                                                    typeof(Views.PeopleMain));
            regionManager.RegisterViewWithRegion(RegionNames.PropertyManagementRegion,
                                                    typeof(Views.PropertyMain));
        }

        #endregion Constructors
    }
}