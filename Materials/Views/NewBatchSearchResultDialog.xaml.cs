using LabDbContext;
using System.Collections.Generic;
using System.Windows;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per NewBatchSearchResultDialog.xaml
    /// </summary>
    public partial class NewBatchSearchResultDialog : Window
    {
        #region Constructors

        public NewBatchSearchResultDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public ICollection<Batch> ParsedBatches
        {
            set => (DataContext as ViewModels.NewBatchSearchResultDialogViewModel).SetParsedBatches(value);
        }

        #endregion Properties
    }
}