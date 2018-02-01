using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Microsoft.Practices.Unity;
using Prism.Events;
using Reports.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

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


            _eventAggregator.GetEvent<ReportStatusCheckRequested>()
                            .Subscribe(
                report =>
                {
                    // areTestsComplete is true if all tests are marked as complete

                    bool areTestsComplete = report.AreTestsComplete();

                    if ((report.IsComplete && !areTestsComplete)
                        || (!report.IsComplete && areTestsComplete))
                    {
                        report.IsComplete = !report.IsComplete;
                        report.Update();

                        _eventAggregator.GetEvent<ReportCompleted>()
                                        .Publish(report);
                    }

                });
        }

        /// <summary>
        /// Shows PO Creation Dialog and adds a PO entry using the entered info
        /// </summary>
        /// <param name="target">The Report to which the PO will be added</param>
        /// <returns>The new PO</returns>
        public PurchaseOrder AddPOToExternalReport(ExternalReport target)
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

                    AddToProjectExternalCost(targetReport.ProjectID,
                                            output.Total);

                    return output;
                }

                else
                    return null;
            }
        }

        /// <summary>
        /// Adds a sum to a project's ExternalCost field
        /// The addition is optimistic and assumes noone is altering the project entry
        /// </summary>
        /// <param name="projectID">The ID of the project to Update</param>
        /// <param name="sumToAdd">The amount that will be added.</param>
        public void AddToProjectExternalCost(int? projectID,
                                            double sumToAdd)
        {
            using (DBEntities entities = new DBEntities())
            {
                Project connectedEntry = entities.Projects
                                                 .First(prj => prj.ID == projectID);

                connectedEntry.TotalExternalCost += sumToAdd;

                entities.SaveChanges();
            }
        }


        public void ApplyControlPlan(IEnumerable<ISelectableRequirement> reqList, ControlPlan conPlan)
        {
            IEnumerable<ControlPlanItem> itemList = conPlan.GetControlPlanItems();

            foreach (ISelectableRequirement isr in reqList)
            {
                isr.IsSelected = itemList.First(cpi => cpi.RequirementID == isr.RequirementInstance.ID
                                                        || cpi.RequirementID == isr.RequirementInstance.OverriddenID)
                                        .IsSelected;
            }
        }



        public Requirement GenerateRequirement(Method method)
        {
            method.LoadSubMethods();

            Requirement tempReq = new Requirement();
            tempReq.MethodID = method.ID;
            tempReq.IsOverride = false;
            tempReq.Name = "";
            tempReq.Description = "";
            tempReq.Position = 0;

            foreach (SubMethod measure in method.SubMethods)
            {
                SubRequirement tempSub = new SubRequirement();
                tempSub.SubMethodID = measure.ID;
                tempReq.SubRequirements.Add(tempSub);
            }

            return tempReq;
        }



        public IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> reqList)
        {
            List<TaskItem> output = new List<TaskItem>();

            foreach (Requirement req in reqList)
            {
                TaskItem tempItem = new TaskItem();

                tempItem.Description = req.Description;
                tempItem.MethodID = req.MethodID;
                tempItem.Name = req.Name;
                tempItem.Position = 0;
                tempItem.RequirementID = req.ID;
                tempItem.SpecificationVersionID = req.SpecificationVersionID;

                foreach (SubRequirement sreq in req.SubRequirements)
                {
                    SubTaskItem tempSubItem = new SubTaskItem();

                    tempSubItem.Name = sreq.SubMethod.Name;
                    tempSubItem.RequiredValue = sreq.RequiredValue;
                    tempSubItem.SubMethodID = sreq.SubMethodID;
                    tempSubItem.SubRequirementID = sreq.ID;
                    tempSubItem.UM = sreq.SubMethod.UM;

                    tempItem.SubTaskItems.Add(tempSubItem);
                }

                output.Add(tempItem);
            }

            return output;
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

                IEnumerable<Test> testList = GenerateTestList(testDialog.SelectedRequirements);

                foreach (Test tst in testList)
                    tst.ReportID = entry.ID;
                testList.CreateTests();

                return true;
            }

            else
                return false;
        }

        public Report CreateReport() => CreateReport(null);

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

            // Opens creation dialog and checks for success
            if (creationDialog.ShowDialog() == true)
            {
                // Reference to newly created instance
                Report output = CreateReportFromCreationDialog(creationDialog.ViewModel);

                // Sets the report as basic if none exists
                output.SetAsBasicIfNoReport();

                // Raise creation event
                _eventAggregator.GetEvent<ReportCreated>()
                                .Publish(output);

                // Return new instance
                return output;
            }

            return null;
        }



        public int GetNextReportNumber()
        {
            // Returns the next available ReportNumber

            using (DBEntities entities = new DBEntities())
            {
                return entities.Reports
                                .Max(rep => rep.Number) + 1;
            }
        }

        public int GetNextExternalReportNumber(int year)
        {
            // Returns the next available ExternalReport number for a given year

            using (DBEntities entities = new DBEntities())
            {
                try
                {
                    return entities.ExternalReports
                                    .Where(erep => erep.Year == year)
                                    .Max(erep => erep.Number) + 1;
                }
                catch
                {
                    return 1;
                }
            }
        }

        public ExternalReport CreateExternalReport()
        {
            Views.ExternalReportCreationDialog creationDialog = new Views.ExternalReportCreationDialog();

            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<ExternalReportCreated>()
                                .Publish(creationDialog.ExternalReportInstance);
                return creationDialog.ExternalReportInstance;
            }
            else
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

            // If the tested Batch does not have a "basic" report, add reference to current instance

            if (dialogViewModelInstance.SelectedBatch.BasicReportID == null)
            {
                using (DBEntities entities = new DBEntities())
                {
                    entities.Batches
                            .First(btc => btc.ID == dialogViewModelInstance.SelectedBatch.ID)
                            .BasicReportID = output.ID;

                    entities.SaveChanges();
                }
            }

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

        /// <summary>
        /// Recalculates the duration of every report, then updates the DB with the new values
        /// </summary>
        public void UpdateAllWorkHours()
        {
            using (DBEntities entities = new DBEntities())
            {
                foreach (Report rpt in entities.Reports)
                    rpt.GetDuration();

                entities.SaveChanges();
            }
        }
    }
}
