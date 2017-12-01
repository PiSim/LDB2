using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Microsoft.Practices.Unity;
using Prism.Events;
using Reports.Views;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports
{
    public class ReportService : IReportService
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ReportService(EventAggregator aggregator,
                            IUnityContainer container)
        {
            _container = container;
            _eventAggregator = aggregator;
        }

        [Obsolete]
        public static PurchaseOrder AddPOToExternalReport(ExternalReport target)
        {

            using (DBEntities entities = new DBEntities())
            {
                ExternalReport targetReport = entities.ExternalReports.First(xtr => xtr.ID == target.ID);

                NewPODialog poDialog = new NewPODialog();
                poDialog.SetSupplier(targetReport.ExternalLab);

                if (poDialog.ShowDialog() == true)
                {
                    PurchaseOrder output = entities.PurchaseOrders.FirstOrDefault(po => po.Number == poDialog.Number);

                    if (output == null)
                    {
                        output = new PurchaseOrder();
                        output.Number = poDialog.Number;
                        output.Organization = entities.Organizations
                            .First(sup => sup.ID == poDialog.Supplier.ID);
                        output.Total = poDialog.Total;
                    }

                    targetReport.PO = output;
                    entities.SaveChanges();

                    return output;
                }

                else
                    return null;
            }
        }

        // Interface members

        public bool AddTestsToReport(Report entry)
        {
            AddTestDialog testDialog = new AddTestDialog();
            testDialog.ReportInstance = entry ?? throw new NullReferenceException();

            if (testDialog.ShowDialog() == true)
            {
                if (testDialog.TestList.Count() == 0)
                    return false;

                IEnumerable<Test> testList = GenerateTestList(testDialog.TestList.Select(riw => riw.RequirementInstance));

                foreach (Test tst in testList)
                    tst.ReportID = entry.ID;
                testList.CreateTests();

                return true;
            }

            else
                return false;
        }

        ExternalReport IReportService.CreateExternalReport() => CreateExternalReport();

        public Report CreateReport() => CreateReport(null);
        
        ExternalReport CreateExternalReport()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the process of creating a new Report instance via the ReportCreationDialog
        /// If a Task instance is given it will be used as template
        /// If creation is successful an event is raised and the new Report returned
        /// </summary>
        /// <param name="parentTask">A Task entry to use as template</param>
        /// <returns>The newly created report instance</returns>
        public Report CreateReport(Task parentTask)
        {
            Views.ReportCreationDialog creationDialog = new Views.ReportCreationDialog();

            // If task is given sets the instance in the ReportCreationDialog
            if (parentTask != null)
            {
                creationDialog.CreationMode = ViewModels.ReportCreationDialogViewModel.CreationModes.ReportFromTask;
                creationDialog.TaskInstance = parentTask;
            }

            // If creation is succesful raise event and return the instance, otherwise return null
            if (creationDialog.ShowDialog() == true)
            {
                Report output = CreateReportFromCreationDialog(creationDialog.ViewModel);
                _eventAggregator.GetEvent<ReportCreated>()
                                .Publish(output);
                return output;
            }

            return null;
        }


        /// <summary>
        /// Process the Data input of a Report creation dialog to create a new Report instance and proceeds to insert it in the database
        /// </summary>
        /// <param name="dialogViewModelInstance">The ViewModel of the dialog used to collect data</param>
        /// <returns>The new report instance</returns>
        private Report CreateReportFromCreationDialog(ViewModels.ReportCreationDialogViewModel dialogViewModelInstance)
        {
            Report output;

            // Creates new  report instance

            output = new Report
            {
                AuthorID = dialogViewModelInstance.Author.ID,
                BatchID = dialogViewModelInstance.SelectedBatch.ID,
                Category = "TR",
                Description = (dialogViewModelInstance.SelectedControlPlan != null) ? dialogViewModelInstance.SelectedControlPlan.Name : dialogViewModelInstance.Description,
                IsComplete = false,
                Number = dialogViewModelInstance.Number,
                SpecificationVersionID = dialogViewModelInstance.SelectedVersion.ID,
                StartDate = DateTime.Now.ToShortDateString()
            };

            // Test list is created from parent task or from the selected requirements

            if (dialogViewModelInstance.IsCreatingFromTask)
            {
                output.Tests = dialogViewModelInstance.TaskInstance.GenerateTests();
            }

            else
            {
                foreach (Test tst in GenerateTestList(dialogViewModelInstance.SelectedRequirements))
                    output.Tests.Add(tst);
            }

            // Calculates total test duration

            output.TotalDuration = output.Tests.Sum(tst => tst.Duration);

            //Inserts new entry in the DB

            output.Create();

            // If using Task as template, the parent is updated with the child Report ID

            if (dialogViewModelInstance.CreationMode == ViewModels.ReportCreationDialogViewModel.CreationModes.ReportFromTask)
            {
                dialogViewModelInstance.TaskInstance.ReportID = output.ID;
                dialogViewModelInstance.TaskInstance.Update();
            }

            return output;
        }
        

        private ICollection<Test> GenerateTestList(IEnumerable<Requirement> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (Requirement req in reqList)
            {
                req.Load();

                Test tempTest = new Test();
                tempTest.Duration = req.Method.Duration;
                tempTest.IsComplete = false;
                tempTest.MethodID = req.MethodID;
                tempTest.RequirementID = req.ID;
                tempTest.Notes = req.Description;

                foreach (SubRequirement subReq in req.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subReq.SubMethod.Name;
                    tempSubTest.RequiredValue = subReq.RequiredValue;
                    tempSubTest.SubRequiremntID = subReq.ID;
                    tempSubTest.UM = subReq.SubMethod.UM;
                    tempTest.SubTests.Add(tempSubTest);
                }
                output.Add(tempTest);
            }

            return output;
        }
    }
}
