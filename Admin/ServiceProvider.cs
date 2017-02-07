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
                int parsedID;
                
                if (!int.TryParse(id, parsedID))
                    continue;
                    
                File tempFile = _entities.Files1.FirstOrDefault(file => file.ID == parsedID);
                
                if (tempFile == null)
                    continue;
                
                ExternalReportFile tempExt = new ExternalReportFile();
                tempExt.ExternalReport = ext;
                tempExt.Path = tempFile.path;
                tempExt.Description = tempFile.description;
                if (tempExt.Description == null)
                    tempExt.Description = "";                
            }
        }
        
        _entities.SaveChanges();
    }
}