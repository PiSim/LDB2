using LInst;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for OrganizationEdit.xaml
    /// </summary>
    public partial class OrganizationEdit : UserControl, INavigationAware
    {
        #region Constructors

        public OrganizationEdit()
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
            (DataContext as ViewModels.OrganizationEditViewModel).OrganizationInstance
                = ncontext.Parameters["ObjectInstance"] as Organization;
        }

        #endregion Methods
    }
}