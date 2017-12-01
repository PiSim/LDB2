using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportCreationDialog.xaml
    /// </summary>
    public partial class ReportCreationDialog : Window, IView
    {
        public ReportCreationDialog()
        {
            InitializeComponent();
        }

        public Batch Batch
        {
            get => ViewModel.SelectedBatch;
            set { ViewModel.BatchNumber = value.Number; }
        }

        public Task TaskInstance
        {
            get => ViewModel.TaskInstance;
            set => ViewModel.TaskInstance = value;
        }

        public ViewModels.ReportCreationDialogViewModel ViewModel => DataContext as ViewModels.ReportCreationDialogViewModel;

        public ViewModels.ReportCreationDialogViewModel.CreationModes CreationMode
        {
            get => ViewModel.CreationMode;
            set => ViewModel.CreationMode = value;
        }
    }
}
