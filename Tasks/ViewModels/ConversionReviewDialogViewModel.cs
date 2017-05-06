using DBManager;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tasks.ViewModels
{
    public class ConversionReviewDialogViewModel : BindableBase
    {
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private IReportServiceProvider _reportServiceProvider;
        private Report _reportInstance;
        private DBManager.Task _taskInstance;

        public ConversionReviewDialogViewModel(DBEntities entities,
                                                DBPrincipal principal,
                                                IReportServiceProvider reportServiceProvider) : base()
        {
            _entities = entities;
            _principal = principal;
            _reportServiceProvider = reportServiceProvider;

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = true;
                });
        }


    }
}
