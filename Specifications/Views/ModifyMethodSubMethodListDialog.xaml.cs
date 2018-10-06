using LabDbContext;
using System.Collections.Generic;
using System.Windows;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per ModifyMethodSubMethodListDialog.xaml
    /// </summary>
    public partial class ModifyMethodSubMethodListDialog : Window
    {
        #region Constructors

        public ModifyMethodSubMethodListDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Method OldVersion
        {
            get => (DataContext as ViewModels.ModifyMethodSubMethodListDialogViewModel).OldVersion;
            set => (DataContext as ViewModels.ModifyMethodSubMethodListDialogViewModel).OldVersion = value;
        }

        public ICollection<SubMethod> SubMethodList => (DataContext as ViewModels.ModifyMethodSubMethodListDialogViewModel).SubMethodList;

        #endregion Properties
    }
}