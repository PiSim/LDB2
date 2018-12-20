using Controls.Views;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for AdminMainView.xaml
    /// </summary>
    public partial class AdminMain : UserControl
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
            regionManager.RegisterViewWithRegion(RegionNames.OrganizationRoleManagementRegion,
                                                    typeof(Views.OrganizationsMain));
            regionManager.RegisterViewWithRegion(RegionNames.PeopleManagementRegion,
                                                    typeof(Views.PeopleMain));
        }

        #endregion Constructors
    }
}