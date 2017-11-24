﻿using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials
{
    public class MaterialService
    {
        // Service class for internal use of the Materials Module
        // Performs various CRUD operations on Material-related entities

        private DBEntities _entities;
        private DBPrincipal _principal;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public MaterialService(DBEntities entities,
                                DBPrincipal principal,
                                EventAggregator eventAggregator,
                                IUnityContainer container)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _principal = principal;

            _eventAggregator.GetEvent<BatchVisualizationRequested>()
                            .Subscribe(batchNumber =>
                            {
                                TryQuickBatchVisualize(batchNumber);
                            });

            _eventAggregator.GetEvent<SampleCreationRequested>()
                            .Subscribe(tuple =>
                            {
                                AddSampleLog(tuple.Item1, tuple.Item2);
                            });
        }

        public Sample AddSampleLog(string batchNumber, string actionCode)
        {
            Batch temp = CommonProcedures.GetBatch(batchNumber);

            Sample output = new Sample();

            output.BatchID = temp.ID;
            output.Date = DateTime.Now;
            output.Code = actionCode;
            output.personID = _principal.CurrentPerson.ID;

            output.Create();

            if (actionCode == "A" && !temp.FirstSampleArrived)
            {
                temp.FirstSampleArrived = true;
                temp.FirstSampleID = output.ID;
                temp.Update();
            }

            return output;
        }

        internal Aspect CreateAspect()
        {
            Views.AspectCreationDialog aspectCreationDialog = new Views.AspectCreationDialog();

            if (aspectCreationDialog.ShowDialog() == true)
            {
                aspectCreationDialog.AspectInstance.Create();
                return aspectCreationDialog.AspectInstance;
            }

            else
                return null;
        }

        public static ExternalConstruction CreateNewExternalConstruction()
        {
            ExternalConstruction newEntry = new ExternalConstruction();
            IEnumerable<ExternalConstruction> tempList = DBManager.Services.MaterialService.GetExternalConstructions();

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

        private void OnColorCreationRequested()
        {
            Views.ColorCreationDialog colorCreator =  _container.Resolve<Views.ColorCreationDialog>();
            colorCreator.ShowDialog();
        }

        public void TryQuickBatchVisualize(string batchNumber)
        {

            Batch temp = DBManager.Services.MaterialService.GetBatch(batchNumber);

            if (temp != null)
            {
                NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView, temp);
                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }

            else
                _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("Batch non trovato");
        }
    }
}