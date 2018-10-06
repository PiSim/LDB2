using Infrastructure;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for MaterialsNavigationItem.xaml
    /// </summary>
    public partial class MaterialsNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public MaterialsNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => MaterialViewNames.MaterialView;

        #endregion Properties
    }
}