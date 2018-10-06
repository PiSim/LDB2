using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per SpecificationVersionEdit.xaml
    /// </summary>
    public partial class SpecificationVersionEdit : UserControl, IView, INavigationAware
    {
        #region Constructors

        public SpecificationVersionEdit()
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
            (DataContext as ViewModels.SpecificationVersionEditViewModel).SpecificationVersionInstance
                = ncontext.Parameters["ObjectInstance"] as SpecificationVersion;
        }

        #endregion Methods
    }
}