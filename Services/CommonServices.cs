using Controls.Views;
using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class CommonServices
    {

        public static void ApplyControlPlan(IEnumerable<ISelectableRequirement> reqList, ControlPlan conPlan)
        {
            if (conPlan.IsDefault)
                foreach (ReportItemWrapper riw in reqList)
                    riw.IsSelected = true;
            else
            {
                foreach (ReportItemWrapper riw in reqList)
                    riw.IsSelected = false;

                foreach (ControlPlanItem cpi in conPlan.ControlPlanItems)
                {
                    ISelectableRequirement tempRIW = reqList.FirstOrDefault(riw => riw.RequirementInstance.ID == cpi.Requirement.ID ||
                                                    (riw.RequirementInstance.IsOverride && riw.RequirementInstance.Overridden.ID == cpi.Requirement.ID));
                    if (tempRIW != null)
                        tempRIW.IsSelected = true;

                    else
                        throw new InvalidOperationException();
                }
            }
        }

        public static void CheckMaterialData(Material target)
        {
            if (target.Construction.Project == null)
            {
                ProjectPickerDialog prjDialog = new ProjectPickerDialog();
                if (prjDialog.ShowDialog() == true)
                    target.Construction.Project = prjDialog.ProjectInstance;
            }

            if (target.Recipe.Colour == null)
            {
                ColorPickerDialog colourPicker = new ColorPickerDialog();
                if (colourPicker.ShowDialog() == true)
                    target.Recipe.Colour = colourPicker.ColourInstance;
            }
        }

        public static IEnumerable<Test> GenerateTestList(IEnumerable<TaskItemWrapper> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (TaskItemWrapper req in reqList.Where(isr => isr.IsSelected))
            {
                Test tempTest = new Test();
                tempTest.IsComplete = false;
                tempTest.Method = req.RequirementInstance.Method;
                tempTest.MethodIssue = tempTest.Method.Standard.CurrentIssue;
                tempTest.Notes = req.RequirementInstance.Description;
                tempTest.TaskItems.Add(req.TaskItemInstance);

                foreach (SubRequirement subReq in req.RequirementInstance.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subReq.SubMethod.Name;
                    tempSubTest.Requirement = subReq.RequiredValue;
                    tempSubTest.UM = subReq.SubMethod.UM;
                    tempTest.SubTests.Add(tempSubTest);
                }
                output.Add(tempTest);
            }

            return output;
        }

        public static Batch GetBatch(string batchNumber)
        {
            Batch temp = MaterialService.GetBatch(batchNumber);

            if (temp == null)
            {
                temp = new Batch();
                temp.Number = batchNumber;
                temp.Create();
            }

            if (temp.Material == null)
                temp.Material = GetMaterial();

            if (temp.Material != null)
                CheckMaterialData(temp.Material);

            return temp;
        }

        private static Material GetMaterial()
        {
            Material output = null;
            MaterialCreationDialog matDialog = new MaterialCreationDialog();

            if (matDialog.ShowDialog() == true)
            {
                Construction tempConstruction = MaterialService.GetConstruction(matDialog.MaterialType,
                                                                                matDialog.MaterialLine,
                                                                                matDialog.MaterialAspect);

                Recipe tempRecipe = MaterialService.GetRecipe(matDialog.MaterialRecipe);

                if (tempConstruction != null && tempRecipe != null)
                    output = MaterialService.GetMaterial(tempConstruction, tempRecipe);

                else
                {
                    if (tempConstruction == null)
                    {
                        tempConstruction = new Construction();
                        tempConstruction.Type = MaterialService.GetMaterialType(matDialog.MaterialType);
                        tempConstruction.Line = matDialog.MaterialLine;
                        tempConstruction.Aspect = MaterialService.GetAspect(matDialog.MaterialAspect);

                        if (tempConstruction.Aspect == null)
                        {
                            Aspect tempAspect = new Aspect();
                            tempAspect.Code = matDialog.MaterialAspect;
                            tempAspect.Name = "";

                            tempAspect.Create();

                            tempConstruction.Aspect = tempAspect;
                        }


                        tempConstruction.Update();
                    }

                    if (tempRecipe == null)
                    {
                        tempRecipe = new Recipe();
                        tempRecipe.Code = matDialog.MaterialRecipe;

                        tempRecipe.Create();
                    };

                    output.Update();
                }

                if (output == null)
                {
                    output = new Material();
                    output.Construction = tempConstruction;
                    output.Recipe = tempRecipe;

                    output.Create();
                }
            }

            return output;
        }

        public static Batch StartBatchSelection()
        {
            BatchPickerDialog batchPicker = new BatchPickerDialog();
            if (batchPicker.ShowDialog() == true)
            {
                Batch output = GetBatch(batchPicker.BatchNumber);
                return output;
            }

            else
                return null;
        }
    }
}
