using LInst;
using Prism.Regions;
using System.Windows.Controls;

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentEdit.xaml
    /// </summary>
    public partial class InstrumentEdit : UserControl, INavigationAware
    {
        #region Constructors

        public InstrumentEdit(IRegionManager regionManager)
        {
            InitializeComponent();
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