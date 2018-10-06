using Infrastructure;
using System.Windows.Controls;

namespace Tasks.Views
{
    /// <summary>
    /// Interaction logic for TaskNavigationItem.xaml
    /// </summary>
    public partial class TaskNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public TaskNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => TaskViewNames.TaskMainView;

        #endregion Properties
    }
}