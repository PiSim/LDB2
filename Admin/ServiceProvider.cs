using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;


public class ServiceProvider
{
    private DBEntities _entities;
    
    public ServiceProvider(DBEntities entities)
    {
        _entities = entities;
    }
    
    public void BuildMeasurements()
    {
        foreach (Method mtd in _entities.Methods)
        {
            string[] _meaList = mtd.Measurements.Split((char)007);
            foreach (string mea in _meaList)
            {
                Measurement tempMea = new Measurement();
                tempMea.Name = mea;
                tempMea.UM = mtd.UM;
                mtd.Measurements.Add(tempMea);
            }
            
        }
        
        _entities.SaveChanges();
    }

}