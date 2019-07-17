using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class AutomationGroupEntry
  {
    #region # sql #

    public static string SqlCommandMany = " SELECT ag.AutomationGroupID, ag.Name, ag.RotationTime, ag.StartTime, ag.EndTime, ag.AutoStart, ag.IsActive, ag.Created, c.CountryID, c.GlobalName, c.TwoLetterIsoCode FROM MobilePaywall.core.AutomationGroup AS ag"
                                        + " LEFT OUTER JOIN MobilePaywall.core.Country AS c ON ag.CountryID = c.CountryID ";

    public enum Columns
    {
      ID,
      Name, 
      RotationTime,
      StartTime,
      EndTime,
      AutoStart,
      IsActive,
      Created,
      CountryID,
      CountryGlobalName,
      CountryTwoLetterIsoCode
    }

    #endregion

    private int _id = -1;
    private CountryEntry _country = null;
    private string _name = string.Empty;
    private int _rotationTime = -1;
    private string _startTime = string.Empty;
    private string _endTime = string.Empty;
    private bool _autoStart = false;
    private bool _isActive = false;
    private string _created = string.Empty;
    private DateTime _loaded;

    public int ID { get { return this._id; } set { this._id = value; } }
    public CountryEntry Country { get { return this._country; } set { this._country = value; } }
    public string Name { get { return this._name; } set { this._name = value; } }
    public int RotationTime { get { return this._rotationTime; } set { this._rotationTime = value; } }
    public string StartTime { get { return this._startTime; } set { this._startTime = value; } }
    public string EndTime { get { return this._endTime; } set { this._endTime = value; } }
    public bool IsActive { get { return this._isActive; } set { this._isActive = value; } }
    public bool AutoStart { get { return this._autoStart; } set { this._autoStart = value; } }
    public string Created { get { return this._created; } set { this._created = value; } }
    public DateTime Loaded { get { return this._loaded; } set { this._loaded = value; } }

    public CoalisionType StartDateTimeCoalision
    {
      get
      {
        string[] startTimeParts = this._startTime.Split(':');
        string[] endTimeParts = this._endTime.Split(':');

        int startH = 0, startM = 0,
          endH = 23, endM = 59;

        Int32.TryParse(startTimeParts[0], out startH);
        Int32.TryParse(startTimeParts.Length == 2 ? startTimeParts[1] : "", out startM);

        Int32.TryParse(endTimeParts[0], out endH);
        Int32.TryParse(endTimeParts.Length == 2 ? endTimeParts[1] : "", out endM);

        if (DateTime.Now.Hour >= startH && DateTime.Now.Hour <= endH)
        {

          if (DateTime.Now.Hour == startH && DateTime.Now.Minute < startM)
            return CoalisionType.Before;

          if (DateTime.Now.Hour == endH && DateTime.Now.Minute < endM)
            return CoalisionType.After;

          return CoalisionType.Now;
        }
        else if (DateTime.Now.Hour < startH)
          return CoalisionType.Before;
        else
          return CoalisionType.After;
      }
    }

    public static List<AutomationGroupEntry> LoadMany(MobilePaywallDatabase database)
    {
      List<AutomationGroupEntry> result = new List<AutomationGroupEntry>();
      DataTable table = database.Load(AutomationGroupEntry.SqlCommandMany);
      if (table == null)
        return result;

      foreach(DataRow row in table.Rows)
      {
        AutomationGroupEntry entry = new AutomationGroupEntry();
        entry.ID = Int32.Parse(row[(int)AutomationGroupEntry.Columns.ID].ToString());

        CountryEntry country = new CountryEntry();
        country.ID = Int32.Parse(row[(int)AutomationGroupEntry.Columns.CountryID].ToString());
        country.GlobalName = row[(int)AutomationGroupEntry.Columns.CountryGlobalName].ToString();
        country.TwoLetterIsoCode = row[(int)AutomationGroupEntry.Columns.CountryTwoLetterIsoCode].ToString();
        entry.Country = country;

        entry.Name = row[(int)AutomationGroupEntry.Columns.Name].ToString();
        entry.RotationTime = Int32.Parse(row[(int)AutomationGroupEntry.Columns.RotationTime].ToString());
        entry.StartTime = row[(int)AutomationGroupEntry.Columns.StartTime].ToString();
        entry.EndTime = row[(int)AutomationGroupEntry.Columns.EndTime].ToString();
        entry.IsActive = (bool)row[(int)AutomationGroupEntry.Columns.IsActive];
        entry.AutoStart = (bool)row[(int)AutomationGroupEntry.Columns.AutoStart];
        entry.Created = row[(int)AutomationGroupEntry.Columns.Created].ToString();
        entry.Loaded = DateTime.Now;

        result.Add(entry);
      }

      return result;
    }

  }

  public enum CoalisionType
  {
    Before,
    Now,
    After
  }

}
