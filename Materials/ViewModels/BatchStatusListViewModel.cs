using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Materials.ViewModels
{
    public class BatchStatusListViewModel : BindableBase
    {
        private DelegateCommand _openBatch;
        private DelegateCommand<Window> _cancel, _confirm;
        private EventAggregator _eventAggregator;

        public BatchStatusListViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

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

            _eventAggregator.GetEvent<BatchCreated>().Subscribe(
                batch =>
                {
                    RaisePropertyChanged("BatchList");
                });

            _eventAggregator.GetEvent<SampleLogCreated>()
                .Subscribe(
                sample =>
                {
                    RaisePropertyChanged("BatchList");
                });

            _eventAggregator.GetEvent<BatchStatusListRefreshRequested>()
                            .Subscribe(
                            () =>
                            {
                                RaisePropertyChanged("BatchList");
                            });
        }

        public IEnumerable<Batch> BatchList
        {
            get { return MaterialService.GetBatches(); }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }



    }
}
