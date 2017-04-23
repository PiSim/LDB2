using Infrastructure;
using System.Windows.Controls;

namespace People.Views
{
    /// <summary>
    /// Interaction logic for AdminNavigationItem.xaml
    /// </summary>
    public partial class PeopleNavigationItem : UserControl, IModuleNavigationTag
    {
        public PeopleNavigationItem()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get { return PeopleViewNames.PeopleMainView; }
        }
    }
}
