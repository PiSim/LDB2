using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Reports.ViewModels
{
    public class ExternalReportCreationDialogViewModel : BindableBase
    {
        #region Fields

        private IDataService<LabDbEntities> _labDbData;
        private IReportService _reportService;

        #endregion Fields

        #region Constructors

        public ExternalReportCreationDialogViewModel(IDataService<LabDbEntities> labDbData,
                                                    IReportService reportService) : base()
        {
            _labDbData = labDbData;
            _reportService = reportService;
            SampleDescription = "";
            TestDescription = "";

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                 parent =>
                 {
                     int year = DateTime.Now.Year - 2000;

                     ExternalReportInstance = new ExternalReport
                     {
                         Description = TestDescription,
                         Year = year,
                         Number = _reportService.GetNextExternalReportNumber(year),
                         MaterialSent = false,
                         RequestDone = false,
                         Samples = SampleDescription,
                         ReportReceived = false,
                         ExternalLabID = SelectedLab.ID,
                         ProjectID = SelectedProject.ID
                     };


                     _labDbData.Execute(new InsertEntityCommand(ExternalReportInstance));

                     parent.DialogResult = true;
                 });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }
        public DelegateCommand<Window> ConfirmCommand { get; }
        public ExternalReport ExternalReportInstance { get; private set; }

        public IEnumerable<Organization> LaboratoriesList => _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.TestLab })
                                                                        .ToList();

        public IEnumerable<Project> ProjectList => _labDbData.RunQuery(new ProjectsQuery()).ToList();
        public string SampleDescription { get; set; }
        public Organization SelectedLab { get; set; }

        public Project SelectedProject { get; set; }

        public string TestDescription { get; set; }

        #endregion Properties
    }
}