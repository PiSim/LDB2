using Controls.Views;
using DBManager;
using DBManager.Services;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class CommonProcedures
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
            target.Load();

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

        public static IEnumerable<Test> GenerateTestList(IEnumerable<ISelectableRequirement> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (ISelectableRequirement req in reqList.Where(isr => isr.IsSelected))
            {
                req.RequirementInstance.Load();

                Test tempTest = new Test();
                tempTest.IsComplete = false;
                tempTest.methodID = req.RequirementInstance.Method.ID;
                tempTest.MethodIssueID = req.RequirementInstance.Method.Standard.CurrentIssue.ID;
                tempTest.Notes = req.RequirementInstance.Description;

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
                temp = new Batch()
                {
                    Number = batchNumber
                };

                temp.Create();
            }

            if (temp.Material == null)
            {
                temp.SetMaterial(StartMaterialSelection());
                temp.Update();
            }

            if (temp.Material != null)
                CheckMaterialData(temp.Material);

            return temp;
        }

        public static Material GetMaterial(string typeCode,
                                            string line,
                                            string aspectCode,
                                            string recipeCode)
        {
            Material output = null;

            Construction tempConstruction = MaterialService.GetConstruction(typeCode,
                                                                            line,
                                                                            aspectCode);

            Recipe tempRecipe = MaterialService.GetRecipe(recipeCode);

            if (tempConstruction != null && tempRecipe != null)
                output = MaterialService.GetMaterial(tempConstruction, tempRecipe);

            else
            {
                if (tempConstruction == null)
                {
                    tempConstruction = new Construction();
                    tempConstruction.SetType(MaterialService.GetMaterialType(typeCode));
                    tempConstruction.Line = line;
                    tempConstruction.SetAspect(MaterialService.GetAspect(aspectCode));

                    if (tempConstruction.Aspect == null)
                    {
                        Aspect tempAspect = new Aspect();
                        tempAspect.Code = aspectCode;
                        tempAspect.Name = "";

                        tempAspect.Create();

                        tempConstruction.SetAspect(tempAspect);
                    }

                    tempConstruction.Create();
                }

                if (tempRecipe == null)
                {
                    tempRecipe = new Recipe();
                    tempRecipe.Code = recipeCode;

                    tempRecipe.Create();
                }
            }

            if (output == null)
            {
                output = new Material();

                output.SetConstruction(tempConstruction);
                output.SetRecipe(tempRecipe);

                output.Create();
            }

            return output;
        }

        public static Material StartMaterialSelection()
        {
            MaterialCreationDialog materialPicker = new MaterialCreationDialog();
            if (materialPicker.ShowDialog() == true)
            {
                Material output = GetMaterial(materialPicker.MaterialType,
                                            materialPicker.MaterialLine,
                                            materialPicker.MaterialAspect,
                                            materialPicker.MaterialRecipe);
                return output;
            }

            else
                return null;
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
