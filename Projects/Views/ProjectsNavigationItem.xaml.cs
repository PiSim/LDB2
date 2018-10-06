using Infrastructure;
using System.Windows.Controls;

namespace Projects.Views
{
    /// <summary>
    /// Interaction logic for ProjectsNavigationItem.xaml
    /// </summary>
    public partial class ProjectsNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public ProjectsNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => ProjectsViewNames.ProjectMainView;

        #endregion Properties
    }
}