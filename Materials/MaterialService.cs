using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Materials.Queries;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;

namespace Materials
{
    public class MaterialService
    {
        // Service class for use internal to the Materials Module

        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public MaterialService(IDataService<LabDbEntities> labDbData,
                                IDbContextFactory<LabDbEntities> dbContextFactory,
                            IEventAggregator eventAggregator)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<BatchVisualizationRequested>()
                            .Subscribe(batchNumber =>
                            {
                                TryQuickBatchVisualize(batchNumber);
                            });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    Batch targetBatch = _labDbData.RunQuery(new BatchQuery() { ID = report.BatchID });

                    if (targetBatch.BasicReportID == null)
                    {
                        targetBatch.BasicReportID = report.ID;
                        _labDbData.Execute(new UpdateEntityCommand(targetBatch));
                    }
                });
        }

        #endregion Constructors

        #region Methods

        public Aspect CreateAspect()
        {
            Views.AspectCreationDialog aspectCreationDialog = new Views.AspectCreationDialog();

            if (aspectCreationDialog.ShowDialog() == true)
            {
                _labDbData.Execute(new InsertEntityCommand(aspectCreationDialog.AspectInstance));
                return aspectCreationDialog.AspectInstance;
            }
            else
                return null;
        }

        public Batch CreateBatch()
        {
            Views.BatchCreationDialog batchCreator = new Views.BatchCreationDialog();

            if (batchCreator.ShowDialog() == true)
            {
                EntityChangedToken token = new EntityChangedToken(batchCreator.BatchInstance,
                                                                    EntityChangedToken.EntityChangedAction.Created);
                _eventAggregator.GetEvent<BatchChanged>()
                                .Publish(token);
                return batchCreator.BatchInstance;
            }
            else
                return null;
        }

        public ExternalConstruction CreateNewExternalConstruction()
        {
            ExternalConstruction newEntry = new ExternalConstruction();
            IEnumerable<ExternalConstruction> tempList = _labDbData.RunQuery(new ExternalConstructionsQuery()).ToList();

            int nameCounter = 1;
            string curName = "Nuova Construction";
            while (true)
            {
                if (!tempList.Any(exc => exc.Name == curName))
                    break;
                else
                    curName = "Nuova Construction " + nameCounter++;
            }
            newEntry.Name = curName;
            newEntry.OemID = 1;

            newEntry.Create();

            return newEntry;
        }

        public void DeleteSample(Sample smp)
        {
            _labDbData.Execute(new DeleteEntityCommand(smp));
            SampleLogChoiceWrapper tempChoice = SampleLogActions.ActionList.First(scc => scc.Code == smp.Code);
            Batch tempBatch = _labDbData.RunQuery(new BatchQuery() { ID = smp.BatchID });

            tempBatch.ArchiveStock -= tempChoice.ArchiveModifier;
            tempBatch.LongTermStock -= tempChoice.LongTermModifier;
            _labDbData.Execute(new UpdateEntityCommand(tempBatch));
        }

        public Dictionary<string, Batch> GetBatchIndex()
        {
            Dictionary<string, Batch> outDict = _labDbData.RunQuery(new BatchesQuery())
                                                            .ToDictionary(batch => batch.Number, batch => batch);

            return outDict;
        }

        public ICollection<Tuple<string, string, string>> GetNewBatchesFromFile()
        {
            LinkedList<Tuple<string, string, string>> output = new LinkedList<Tuple<string, string, string>>();
            Dictionary<string, Batch> batchDict = GetBatchIndex();
            foreach (Tuple<string, string, string> parsedBatch in GetOrderFiles())
                if (!batchDict.ContainsKey(parsedBatch.Item1))
                    output.AddLast(parsedBatch);

            return output;
        }

        public void ShowSampleLogDialog()
        {
            Views.SampleLogDialog logger = new Views.SampleLogDialog();

            logger.Show();
        }

        public void TryQuickBatchVisualize(string batchNumber)
        {
            Batch temp = _labDbData.RunQuery(new BatchQuery() { Number = batchNumber });

            if (temp != null)
            {
                NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView, temp);
                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
            else
                _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("Batch non trovato");
        }

        private IEnumerable<Tuple<string, string, string>> GetOrderFiles()
        {
            IEnumerable<string> names = Directory.EnumerateFiles(Properties.Settings.Default.NewOrdersFolderPath);

            Tuple<string, string, string> currentParsed;
            LinkedList<Tuple<string, string, string>> output = new LinkedList<Tuple<string, string, string>>();

            foreach (string name in names)
            {
                currentParsed = TryParseFileName(name);
                if (currentParsed != null)
                    output.AddLast(currentParsed);
            }

            return output;
        }

        private Tuple<string, string, string> TryParseFileName(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            name = name.Replace(" ", string.Empty);
            string[] parsed = name.Split('-');
            if (parsed.Count() >= 2 && parsed[0].Length <= 10)
                return new Tuple<string, string, string>(parsed[0], parsed[1], path);
            else
                return null;
        }

        #endregion Methods
    }
}