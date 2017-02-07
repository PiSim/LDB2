using DBManager;
using System;
using System.Collections.Generic;


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
                File tempFile = _entities.Files[int.Parse(id)];
                
                ExternalReportFile tempExt = new ExternalReportFile();
                tempExt.ExternalReport = ext;
                tempExt.Path = tempFile.Path;
                tempExt.Description = tempFile.Description;                
            }
        }
        
        _entities.SaveChanges();
    }
}