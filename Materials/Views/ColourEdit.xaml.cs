using Controls.Views;
using LabDbContext;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for ColourEdit.xaml
    /// </summary>
    public partial class ColourEdit : UserControl, INavigationAware
    {
        #region Constructors

        public ColourEdit(IRegionManager regionManager)
        {
            InitializeComponent();

            regionManager.RegisterViewWithRegion(RegionNames.ColourEditBatchListRegion,
                                                typeof(BatchListControl));
        }

        #endregion Constructors

        #region Methods

        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {
        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            (DataContext as ViewModels.ColourEditViewModel).ColourInstance =
               ncontext.Parameters["ObjectInstance"] as Colour;
        }

        #endregion Methods
    }
}