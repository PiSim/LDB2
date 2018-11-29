using DataAccessCore;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;

namespace Instruments.ViewModels
{
    public class AddPropertyDialogViewModel : BindableBase
    {
        #region Fields

        private IDataService<LInstContext> _lInstData;

        #endregion Fields

        #region Constructors

        public AddPropertyDialogViewModel(IDataService<LInstContext> lInstData)
        {
            _lInstData = lInstData;

            CancelCommand = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }
        
        #endregion Properties
    }
}