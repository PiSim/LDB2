using Controls.Views;
using LInst;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentEdit.xaml
    /// </summary>
    public partial class InstrumentEdit : UserControl, IView, INavigationAware
    {
        #region Constructors

        public InstrumentEdit(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.InstrumentEditMetrologyRegion,
                                                    typeof(Views.Metrology));
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
            (DataContext as ViewModels.InstrumentEditViewModel).InstrumentInstance =
               ncontext.Parameters["ObjectInstance"] as Instrument;
        }

        #endregion Methods
    }
}