using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per StandardEdit.xaml
    /// </summary>
    public partial class StandardEdit : UserControl, IView, INavigationAware
    {
        #region Constructors

        public StandardEdit()
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
            (DataContext as ViewModels.StandardEditViewModel).StandardInstance =
               ncontext.Parameters["ObjectInstance"] as Std;
        }

        #endregion Methods
    }
}