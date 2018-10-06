using Infrastructure;
using System.Windows.Controls;

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentsNavigationItem.xaml
    /// </summary>
    public partial class InstrumentsNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public InstrumentsNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => InstrumentViewNames.InstrumentsMainView;

        #endregion Properties
    }
}