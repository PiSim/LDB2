using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBEntities : LabDBEntities
    {
        public Sample CreateSampleForBatch(Batch batch, string actionCode)
        {
            Sample output = new Sample();
            output.batch = batch;
            output.date = DateTime.Now;
            output.code = actionCode;
            Samples.Add(output);
            return output;
        }

        public Batch GetBatchByNumber(string number, bool new_instance_if_none_found = true)
        {
            Batch output = base.Batches.FirstOrDefault(bb => bb.Number == number);

            if (output == null && new_instance_if_none_found)
            {
                output = new Batch();
                output.Number = number;
                Batches.Add(output);
            }

            return output;
        }

        public Material GetMaterial(string type,
                                    string line,
                                    string aspectCode,
                                    string recipeCode,
                                    bool new_instance_if_not_found = true)
        {
            Construction tempConstruction;
            Material output;
            Recipe tempRecipe = Recipes.FirstOrDefault(rec => rec.Code == recipeCode);
            Aspect tempAspect = Aspects.FirstOrDefault(asp => asp.Code == aspectCode);
            if (tempAspect != null)
            {
                tempConstruction = Constructions.FirstOrDefault(
                    cons => cons.Aspect == tempAspect &&
                            cons.Line == line &&
                            cons.Type == type);

                if (tempConstruction != null && tempRecipe != null)
                {
                    output = Materials.FirstOrDefault(
                        mat => mat.Construction == tempConstruction &&
                               mat.Recipe == tempRecipe);

                    if (output != null)
                        return output;

                    else if (new_instance_if_not_found == false)
                        return null;
                }

                else if (tempConstruction == null)
                {
                    tempConstruction = new Construction();
                    tempConstruction.Aspect = tempAspect;
                    tempConstruction.Line = line;
                    tempConstruction.Type = type;
                }

                else if (tempRecipe == null)
                {
                    tempRecipe = new Recipe();
                    tempRecipe.Code = recipeCode;
                }

                output = new Material();
                output.Construction = tempConstruction;
                output.Recipe = tempRecipe;
                return output;
            }

            else
            {
                tempAspect = new Aspect();
                tempAspect.Code = aspectCode;

                tempConstruction = new Construction();
                tempConstruction.Aspect = tempAspect;
                tempConstruction.Line = line;
                tempConstruction.Type = type;

                if (tempRecipe == null)
                {
                    tempRecipe = new Recipe();
                    tempRecipe.Code = recipeCode;
                }

                output = new Material();
                output.Construction = tempConstruction;
                output.Recipe = tempRecipe;

                return output;
            }
        }
    }
}
