using DBManager;
using System;
using System.Windows;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for NewPODialog.xaml
    /// </summary>
    public partial class NewPODialog : Window
    {
        private Currency _currency;
        private DateTime _date;
        private Organization _supplier;
        private string _number;
        private float _total;

        public NewPODialog(DBEntities entities)
        {
            DataContext = new ViewModels.NewPODialogViewModel(entities, this);
            InitializeComponent();
        }

        public Currency Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
            }
        }

        public DateTime Date
        {
            get { return _date; }
            internal set { _date = value; }
        }

        public string Number
        {
            get { return _number; }
            internal set { _number = value; }
        }

        public Organization Supplier
        {
            get { return _supplier; }
            set { _supplier = value; }
        }

        public float Total
        {
            get { return _total; }
            internal set { _total = value; }
        }

        public void SetSupplier(Organization target)
        {
            (DataContext as ViewModels.NewPODialogViewModel).SetOrganization(target);
        }
    }
}
