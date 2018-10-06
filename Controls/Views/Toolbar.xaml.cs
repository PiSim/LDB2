using Infrastructure;
using Infrastructure.Events;
using Prism.Events;
using System.Windows.Controls;
using System.Windows.Input;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for ToolbarView.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        #region Fields

        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public Toolbar(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationToken token = new NavigationToken((MainNavigationComboBox.SelectedItem as IModuleNavigationTag).ViewName);
            _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (DataContext as ViewModels.ToolbarViewModel).RunSearchCommand.Execute(SearchBox.Text);
        }

        #endregion Methods
    }
}