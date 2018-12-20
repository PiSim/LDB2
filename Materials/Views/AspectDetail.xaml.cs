using LabDbContext;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per AspectDetail.xaml
    /// </summary>
    public partial class AspectDetail : UserControl, INavigationAware
    {
        #region Constructors

        public AspectDetail()
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
            (DataContext as ViewModels.AspectDetailViewModel).AspectInstance =
               ncontext.Parameters["ObjectInstance"] as Aspect;
        }

        #endregion Methods
    }
}