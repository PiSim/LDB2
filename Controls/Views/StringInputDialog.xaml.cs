using System.Windows;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for OrganizationCreationDialog.xaml
    /// </summary>
    public partial class StringInputDialog : Window
    {
        #region Constructors

        public StringInputDialog()
        {
            DataContext = new ViewModels.StringInputDialogViewModel(this);
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string InputString => (DataContext as ViewModels.StringInputDialogViewModel).InputString;

        public string Message
        {
            get { return (DataContext as ViewModels.StringInputDialogViewModel).Message; }
            set { (DataContext as ViewModels.StringInputDialogViewModel).Message = value; }
        }

        #endregion Properties
    }
}