using Infrastructure;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationNavigationItem.xaml
    /// </summary>
    public partial class SpecificationNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public SpecificationNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => SpecificationViewNames.StandardMain;

        #endregion Properties
    }
}