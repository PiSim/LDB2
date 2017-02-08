using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.ViewModels
{
    internal class ExternalReportEditViewModel : BindableBase
    {
        private DelegateCommand _addFile, _openFile, _removeFile;
        private ExternalReport _instance;
        private ExternalReportFile _selectedFile;
        private ObservableCollection<ExternalReportFile> _reportFiles;

        internal ExternalReportEditViewModel(ExternalReport instance) : base()
        {
            _instance = instance;
            _reportFiles = new ObservableCollection<ExternalReportFile>
                (_instance.ExternalReportFiles);

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;
                    
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ExternalReportFile temp = new ExternalReportFile();
                            temp.Path = pth;
                            ReportFiles.Add(temp);   
                        }
                    }
                });

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    ReportFiles.Remove(_selectedFile);
                },
                () => _selectedFile != null);
        }

        public string Description
        {
            get { return _instance.Description; }
            set { _instance.Description = value; }
        }

        public string Currency
        {
            get { return _instance.Currency; }
            set { _instance.Currency = value; }
        }

        public ObservableCollection<ExternalReportFile> ReportFiles
        {
            get { return _reportFiles; }
        }

        public double Price
        {
            get { return _instance.Price; }
            set { _instance.Price = value; }
        }

        public string PurchaseOrder
        {
            get { return _instance.purchase_order; }
            set { _instance.purchase_order = value; }
        }

        public string Samples
        {
            get { return _instance.Samples; }
            set { _instance.Samples = value; }
        }
    }
}
