using LabDbContext;
using System.Collections.Generic;

public class SamplesWrapper
{
    #region Fields

    private static readonly Dictionary<string, string> _actionDictionary = new Dictionary<string, string>()
    {
        {"A", "Arrivato in laboratorio"},
        {"B", "Buttato"},
        {"F", "Finito"},
        {"S", "Spedito"},
        {"M", "Masterizzato"}
    };

    private Sample _instance;

    #endregion Fields

    #region Constructors

    public SamplesWrapper(Sample instance)
    {
        _instance = instance;
    }

    #endregion Constructors

    #region Properties

    public string Action => _actionDictionary[_instance.Code];

    public string Date => _instance.Date.ToShortDateString();

    #endregion Properties
}