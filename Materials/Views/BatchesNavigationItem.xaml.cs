using Infrastructure;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for BatchesNavigationItem.xaml
    /// </summary>
    public partial class BatchesNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public BatchesNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => MaterialViewNames.BatchesView;

        #endregion Properties
    }
}