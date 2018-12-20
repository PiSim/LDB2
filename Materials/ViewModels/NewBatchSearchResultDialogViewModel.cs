using DataAccess;
using Infrastructure.Queries;
using LabDbContext;
using Materials.Commands;
using Materials.Queries;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Materials.ViewModels
{
    public class NewBatchSearchResultDialogViewModel : BindableBase
    {
        #region Fields

        private bool _areAllBatchesSelected = true;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public NewBatchSearchResultDialogViewModel(IDataService<LabDbEntities> labDbData)
        {
            _labDbData = labDbData;
            ParsedBatches = new List<NewBatchWrapper>();
            ColorList = _labDbData.RunQuery(new ColorsQuery()).ToList();
            ConstructionList = _labDbData.RunQuery(new ExternalConstructionsQuery()).ToList();
            ProjectList = _labDbData.RunQuery(new ProjectsQuery()).ToList();
            TrialAreaList = _labDbData.RunQuery(new TrialAreasQuery()).ToList();

            CancelCommand = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                dialog =>
                {
                    IEnumerable<Batch> _toCreate = ParsedBatches.Where(nbw => nbw.IsSelected).Select(nbw => nbw.BatchInstance);
                    if (MessageBox.Show("Verranno creati " + _toCreate.Count() + " Batch. Continuare?", "Conferma inserimento", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        ///Check if all Aspects required to create the new Batches exist in the database
                        ///If not, create a new entry and insert it
                        IEnumerable<Aspect> uniqueAspects = _toCreate.GroupBy(btc => btc.Material.Aspect.Code).Select(aspg => aspg.FirstOrDefault().Material.Aspect);
                        _labDbData.Execute(new CreateAspectsIfMissingCommand(uniqueAspects));

                        ///Check if all MaterialLine required to create the new Batches exist in the database
                        ///If not, create a new entry and insert it
                        IEnumerable<MaterialLine> uniqueLines = _toCreate.GroupBy(btc => btc.Material.MaterialLine.Code).Select(aspg => aspg.FirstOrDefault().Material.MaterialLine);
                        _labDbData.Execute(new CreateMaterialLinesIfMissingCommand(uniqueLines));

                        ///Check if all MaterialType required to create the new Batches exist in the database
                        ///If not, create a new entry and insert it
                        IEnumerable<MaterialType> uniqueTypes = _toCreate.GroupBy(btc => btc.Material.MaterialType.Code).Select(aspg => aspg.FirstOrDefault().Material.MaterialType);
                        _labDbData.Execute(new CreateMaterialTypesIfMissingCommand(uniqueTypes));

                        ///Check if all Recipes required to create the new Batches exist in the database
                        ///If not, create a new entry and insert it
                        IEnumerable<Recipe> uniqueRecipes = _toCreate.GroupBy(btc => btc.Material.Recipe.Code).Select(aspg => aspg.FirstOrDefault().Material.Recipe);
                        _labDbData.Execute(new CreateRecipesIfMissingCommand(uniqueRecipes));

                        ///Iterate through the required material and check if a corresponding entry exists
                        ///If not, create a new entry and insert it
                        IEnumerable<Material> materials = _toCreate.Select(btc => btc.Material);
                        foreach (Material mat in materials)
                            _labDbData.Execute(new CreateMaterialIfMissingCommand(mat));

                        BulkNewBatchInsertCommand insertCommand = new BulkNewBatchInsertCommand(_toCreate);
                        _labDbData.Execute(insertCommand);

                        if (insertCommand.FailedBatches.Count() != 0)
                        {
                            string errMsg = "Inserimento fallito per i seguenti Batch: ";
                            foreach (Batch btc in insertCommand.FailedBatches)
                                errMsg += btc.Number += " ";
                        }

                        dialog.DialogResult = true;
                    }
                },
                dialog => AreSelectedBatchesValid);

            OpenOrderFileCommand = new DelegateCommand<NewBatchWrapper>(
                batch =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(batch.BatchInstance.OrderFilePath);
                    }
                    catch
                    {
                    }
                });
        }

        #endregion Constructors

        #region Properties

        public bool AreAllBatchesSelected
        {
            get => _areAllBatchesSelected;
            set
            {
                foreach (NewBatchWrapper nbw in ParsedBatches)
                    nbw.SetIsSelected(value);

                CheckAllBatchesSelected();
            }
        }

        public DelegateCommand<Window> CancelCommand { get; set; }

        public IEnumerable<Colour> ColorList { get; }

        public DelegateCommand<Window> ConfirmCommand { get; set; }

        public IEnumerable<ExternalConstruction> ConstructionList { get; }

        public DelegateCommand<NewBatchWrapper> OpenOrderFileCommand { get; set; }

        public ICollection<NewBatchWrapper> ParsedBatches { get; private set; }
        public IEnumerable<Project> ProjectList { get; }

        public IEnumerable<TrialArea> TrialAreaList { get; set; }

        private bool AreSelectedBatchesValid => ParsedBatches.Where(nbw => nbw.IsSelected).All(nbw => !nbw.HasErrors);

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises CanExecuteChanged on ConfirmCommand to check wether all batches have valid data for insertion
        /// </summary>
        public void OnBatchHasErrorsChanged(object sender, EventArgs e)
        {
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Updates the value of AreAllBatchesSelected and bubbles to OnBatchHasErrorChanged
        /// </summary>
        public void OnBatchSelectionChanged(object sender, EventArgs e)
        {
            CheckAllBatchesSelected();
            OnBatchHasErrorsChanged(sender, e);
        }

        public void SetParsedBatches(ICollection<Batch> parsedBatchList)
        {
            ParsedBatches = new List<NewBatchWrapper>(parsedBatchList.Count());
            foreach (Batch btc in parsedBatchList)
            {
                NewBatchWrapper newBatch = new NewBatchWrapper(btc);
                newBatch.HasErrorsChanged += OnBatchHasErrorsChanged;
                newBatch.IsSelectedChanged += OnBatchSelectionChanged;
                newBatch.MaterialDataChanged += OnBatchMaterialDataChanged;
                newBatch.RecipeDataChanged += OnBatchRecipeDataChanged;
                CheckRecipe(newBatch);
                CheckMaterial(newBatch);
                ParsedBatches.Add(newBatch);
            }
            RaisePropertyChanged("ParsedBatches");
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        private void CheckAllBatchesSelected()
        {
            bool hasSelected = false, hasNotSelected = false;
            foreach (NewBatchWrapper nbw in ParsedBatches)
            {
                if (nbw.IsSelected)
                    hasSelected = true;
                else
                    hasNotSelected = true;

                if (hasSelected && hasNotSelected)
                    break;
            }

            if (hasNotSelected ^ hasSelected)
                _areAllBatchesSelected = (hasSelected) ? true : false;

            RaisePropertyChanged("AreAllBatchesSelected");
        }

        private void CheckMaterial(NewBatchWrapper newBatch)
        {
            Material dbMaterial = _labDbData.RunQuery(new MaterialsQuery() { OrderResults = false })
                                            .FirstOrDefault(mat => mat.Aspect.Code == newBatch.AspectCode
                                                                    && mat.MaterialLine.Code == newBatch.MaterialLineCode
                                                                    && mat.MaterialType.Code == newBatch.MaterialTypeCode
                                                                    && mat.Recipe.Code == newBatch.RecipeCode);

            if (dbMaterial != null)
            {
                if (dbMaterial.Project != null)
                    newBatch.ProjectInstance = ProjectList.FirstOrDefault(prj => prj.ID == dbMaterial.ProjectID);

                if (dbMaterial.ExternalConstruction != null)
                    newBatch.ConstructionInstance = ConstructionList.FirstOrDefault(exc => exc.ID == dbMaterial.ExternalConstructionID);
            }
        }

        private void CheckRecipe(NewBatchWrapper newBatch)
        {
            Recipe dbRecipe = _labDbData.RunQuery(new RecipeQuery() { RecipeCode = newBatch.RecipeCode });
            if (dbRecipe?.Colour != null)
                newBatch.ColorInstance = ColorList.FirstOrDefault(col => col.ID == dbRecipe.ColourID);
        }

        private void OnBatchMaterialDataChanged(object sender, EventArgs e)
        {
            CheckMaterial(sender as NewBatchWrapper);
        }

        private void OnBatchRecipeDataChanged(object sender, EventArgs e)
        {
            CheckRecipe(sender as NewBatchWrapper);
        }

        #endregion Methods
    }
}