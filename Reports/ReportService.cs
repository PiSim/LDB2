using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Events;
using Reports.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Reports
{
    public class ReportService : IReportService
    {
        #region Fields
        
        private IDbContextFactory<LabDbEntities> _dbContextFactory;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public ReportService(IDataService<LabDbEntities> labDbData,
                            IDbContextFactory<LabDbEntities> dbContextFactory,
                            IEventAggregator aggregator)
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _dbContextFactory = dbContextFactory;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Opens AddTestDialog and adds the selected Tests #
        /// to an existing report if selection is succesfull
        /// </summary>
        /// <param name="entry">The report to which the tests will be added</param>
        /// <returns>A bool indicating whether the procedure was successful</returns>
        public bool AddTestsToReport(Report entry)
        {
            AddTestDialog testDialog = new AddTestDialog();
            testDialog.ReportInstance = entry ?? throw new NullReferenceException();

            if (testDialog.ShowDialog() == true)
            {
                if (testDialog.TestList.Count() == 0)
                    return false;

                // Calls the method for generating a test list from the selected
                // Requirement instances

                IEnumerable<Test> testList = GenerateTestList(
                    testDialog.TestList.Where(riw => riw.IsSelected)
                                        .Select(riw => riw.RequirementInstance));

                foreach (Test tst in testList)
                    tst.TestRecordID = entry.TestRecordID;
                testList.CreateTests();

                return true;
            }
            else
                return false;
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
            using (LabDbEntities entities = _dbContextFactory.Create())
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

        public ExternalReport CreateExternalReport()
        {
            Views.ExternalReportCreationDialog creationDialog = new Views.ExternalReportCreationDialog();

            if (creationDialog.ShowDialog() == true)
            {
                EntityChangedToken token = new EntityChangedToken(creationDialog.ExternalReportInstance,
                                                                EntityChangedToken.EntityChangedAction.Created);
                _eventAggregator.GetEvent<ExternalReportChanged>()
                                .Publish(token);
                return creationDialog.ExternalReportInstance;
            }
            else
                return null;
        }


        /// <summary>
        /// Starts the process of creating a new Report instance via the ReportCreationDialog
        /// If creation is successful an event is raised and the new Report returned
        /// </summary>
        /// <returns>The newly created report instance</returns>
        public Report CreateReport()
        {
            Views.ReportCreationDialog creationDialog = new Views.ReportCreationDialog();

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

        public Requirement GenerateRequirement(MethodVariant methodVariant)
        {
            Method method = methodVariant.GetMethod(true);
            method.LoadSubMethods();

            Requirement tempReq = new Requirement()
            {
                MethodVariantID = methodVariant.ID,
                Name = method.Name,
                Description = methodVariant.Description
            };

            tempReq.Deprecated2 = method.ID;
            tempReq.IsOverride = false;
            tempReq.Name = "";
            tempReq.Description = "";
            tempReq.Position = 0;

            foreach (SubMethod measure in method.SubMethods)
            {
                SubRequirement tempSub = new SubRequirement()
                {
                    SubMethodID = measure.ID
                };

                tempReq.SubRequirements.Add(tempSub);
            }

            return tempReq;
        }

        [Obsolete]
        public IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> reqList)
        {
            List<TaskItem> output = new List<TaskItem>();

            foreach (Requirement req in reqList)
            {
                TaskItem tempItem = new TaskItem();

                tempItem.Description = req.Description;
                tempItem.MethodID = req.MethodVariant.Method.ID;
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

        public ICollection<Test> GenerateTestList(IEnumerable<Requirement> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (Requirement req in reqList)
            {
                req.Load();

                Test tempTest = new Test()
                {
                    Deprecated2 = req.Deprecated2,
                    MethodVariantID = req.MethodVariantID
                };

                tempTest.Duration = req.MethodVariant.Method.Duration;
                tempTest.RequirementID = req.ID;
                tempTest.Notes = req.Description;

                foreach (SubRequirement subReq in req.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest()
                    {
                        SubRequiremntID = subReq.ID,
                        Name = subReq.SubMethod.Name,
                        Position = subReq.SubMethod.Position,
                        RequiredValue = subReq.RequiredValue,
                        SubMethodID = subReq.SubMethodID,
                        UM = subReq.SubMethod.UM,
                    };
                    tempTest.SubTests.Add(tempSubTest);
                }
                output.Add(tempTest);
            }

            return output;
        }

        public int GetNextExternalReportNumber(int year)
        {
            // Returns the next available ExternalReport number for a given year

            using (LabDbEntities entities = _dbContextFactory.Create())
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

        public int GetNextReportNumber()
        {
            // Returns the next available ReportNumber

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                return entities.Reports
                                .Max(rep => rep.Number) + 1;
            }
        }

        /// <summary>
        /// Iterates through all the Report instances and recalculates all the durations,
        /// committing the result to the DB
        /// </summary>
        public void RecalculateAllWorkHours()
        {
            IEnumerable<Report> reportList = _labDbData.RunQuery(new ReportsQuery());

            foreach (Report currentReport in reportList)
            {
                currentReport.UpdateDuration();
            }
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

            // Test Record is created from parent task or from the selected requirements

            output.TestRecord = new TestRecord()
            {
                BatchID = output.BatchID,
                RecordTypeID = 1
            };

            if (dialogViewModelInstance.IsCreatingFromTask)
                foreach (Test tst in dialogViewModelInstance.TaskInstance.GenerateTests())
                    output.TestRecord.Tests.Add(tst);
            else
                foreach (Test tst in GenerateTestList(dialogViewModelInstance.SelectedRequirements))
                    output.TestRecord.Tests.Add(tst);

            // Calculates total test duration

            output.TotalDuration = output.TestRecord.Tests.Sum(tst => tst.Duration);

            //Inserts new entry in the DB

            output.Create();

            // If the tested Batch does not have a "basic" report, add reference to current instance

            if (dialogViewModelInstance.SelectedBatch.BasicReportID == null)
            {
                using (LabDbEntities entities = _dbContextFactory.Create())
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

        #endregion Methods
    }
}