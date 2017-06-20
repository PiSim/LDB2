﻿using Controls.Views;
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
        public static bool AddTestsToReport(Report entry)
        {
            if (entry == null)
                 throw new NullReferenceException();

            Views.AddTestDialog testDialog = new Views.AddTestDialog();
            testDialog.ReportInstance = entry;

            if (testDialog.ShowDialog() == true)
            {
                if (testDialog.TestList.Count() == 0)
                    return false;

                IEnumerable<Test> testList = GenerateTestList(testDialog.TestList);

                foreach (Test tst in testList)
                    tst.ReportID = entry.ID;
                testList.CreateTests();

                return true;
            }

            else
                return false;
        }

        public static void ApplyControlPlan(IEnumerable<ISelectableRequirement> reqList, ControlPlan conPlan)
        {
            if (conPlan.IsDefault)
                foreach (ISelectableRequirement riw in reqList)
                    riw.IsSelected = true;
            else
            {
                foreach (ISelectableRequirement riw in reqList)
                    riw.IsSelected = false;

                foreach (ControlPlanItem cpi in conPlan.ControlPlanItems)
                {
                    ISelectableRequirement tempRIW = reqList.FirstOrDefault(riw => riw.RequirementInstance.ID == cpi.RequirementID ||
                                                    (riw.RequirementInstance.IsOverride && riw.RequirementInstance.OverriddenID == cpi.RequirementID));
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

        public static Requirement GenerateRequirement(Method method)
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

        public static IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> reqList)
        {
            List<TaskItem> output = new List<TaskItem>();

            foreach(Requirement req in reqList)
            {
                TaskItem tempItem = new TaskItem();

                tempItem.Description = req.Description;
                tempItem.IsAssignedToReport = false;
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

        public static IEnumerable<Test> GenerateTestList(IEnumerable<TaskItemWrapper> reqList)
        {
            List<Test> output = new List<Test>();

            foreach (TaskItemWrapper req in reqList.Where(isr => isr.IsSelected))
            {
                req.TaskItemInstance.Load();

                Test tempTest = new Test();
                tempTest.IsComplete = false;
                tempTest.MethodID = req.TaskItemInstance.MethodID;
                
                tempTest.MethodIssueID = req.TaskItemInstance.Method.Standard.CurrentIssueID;

                tempTest.Notes = req.TaskItemInstance.Description;
                tempTest.RequirementID = req.TaskItemInstance.RequirementID;

                foreach (SubTaskItem subItem in req.TaskItemInstance.SubTaskItems)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subItem.Name;
                    tempSubTest.Requirement = subItem.RequiredValue;
                    tempSubTest.SubRequiremntID = subItem.SubRequirementID;
                    tempSubTest.UM = subItem.UM;
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
                tempTest.MethodID = req.RequirementInstance.Method.ID;
                tempTest.RequirementID = req.RequirementInstance.ID;

                if (req.RequirementInstance.Method.Standard.CurrentIssue != null)
                    tempTest.MethodIssueID = req.RequirementInstance.Method.Standard.CurrentIssue.ID;

                tempTest.Notes = req.RequirementInstance.Description;

                foreach (SubRequirement subReq in req.RequirementInstance.SubRequirements)
                {
                    SubTest tempSubTest = new SubTest();
                    tempSubTest.Name = subReq.SubMethod.Name;
                    tempSubTest.Requirement = subReq.RequiredValue;
                    tempSubTest.SubRequiremntID = subReq.ID;
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
                temp.MaterialID = StartMaterialSelection().ID;
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


        public static void SetTaskItemsAssignment(IEnumerable<Test> testList, DBManager.Task taskEntry)
        {
            foreach (TaskItem tski in taskEntry.TaskItems)
            {
                Test tempTest = testList.First(tst => tst.RequirementID == tski.RequirementID
                                                && !tst.TaskItems.Any());

                tski.IsAssignedToReport = true;
                tski.TestID = tempTest.ID;

            }

            taskEntry.TaskItems.Update();
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
