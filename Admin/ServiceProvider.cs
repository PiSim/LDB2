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
    
    public void BuildExternalReportFiles()
    {
        foreach (ExternalReport ext in _entities.ExternalReports)
        {
            string[] fileIDs = ext.fileID.Split((char)007);
            
            foreach (string id in fileIDs)
            {
                File tempFile = _entities.Files1.FirstOrDefault(file => file.ID == int.Parse(id));
                
                ExternalReportFile tempExt = new ExternalReportFile();
                tempExt.ExternalReport = ext;
                tempExt.Path = tempFile.path;
                tempExt.Description = tempFile.description;                
            }
        }
        
        _entities.SaveChanges();
    }
}