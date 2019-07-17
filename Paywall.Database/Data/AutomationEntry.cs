using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class AutomationEntry
  {

    #region # sql #

    public static string SqlIndexWildcard = "[IDS]";
    public static string SqlCommand = " SELECT a.AutomationID, a.Name, a.AutomationGroupID, a.Limit, a.ExternalOfferName, a.IsActive, a.[Index], a.Comment, a.Created, s.ServiceID, s.Name, s.Description, mo.MobileOperatorID, mo.Name "
                                    + " FROM MobilePaywall.core.Automation AS a "
                                    + " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON a.ServiceID=s.ServiceID "
                                    + " LEFT OUTER JOIN MobilePaywall.core.MobileOperator AS mo ON a.MobileOperatorID=mo.MobileOperatorID "
                                    + " WHERE a.AutomationGroupID IN ("+ AutomationEntry.SqlIndexWildcard +") "
                                    + " ORDER BY a.AutomationGroupID, a.[Index] ";

    public enum Columns
    {
      ID,
      Name,
      AutomationGroupID,
      Limit,
      ExternalOfferName,
      IsActive,
      Index,
      Comment,
      Created,
      ServiceID,
      ServiceName,
      ServiceDescription,
      MobileOperatorID,
      MobileOperatorName
    }

    #endregion

    private int _id = -1;
    private string _name = string.Empty;
    private ServiceEntry _service = null;
    private int _automationGroupID = -1;
    private MobileOperatorEntry _mobileOperator = null;
    private int _transactionLimit = -1;
    private string _externalOfferName = string.Empty;
    private bool _isActive = false;
    private int _index = -1;
    private string _comment = string.Empty;
    private string _created = string.Empty;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string Name { get { return this._name; } set { this._name = value; } }
    public ServiceEntry Service { get { return this._service; } set { this._service = value; } }
    public int AutomationGroupID { get { return this._automationGroupID; } set { this._automationGroupID = value; } }
    public MobileOperatorEntry MobileOperator { get { return this._mobileOperator; } set { this._mobileOperator = value; } }
    public int TransactionLimit { get { return this._transactionLimit; } set { this._transactionLimit = value; } }
    public string ExternalOfferName { get { return this._externalOfferName; } set { this._externalOfferName = value; } }
    public bool IsActive { get { return this._isActive; } set { this._isActive = value; } }
    public int Index { get { return this._index; } set { this._index = value; } }
    public string Comment { get { return this._comment; } set { this._comment = value; } }
    public string Created { get { return this._created; } set { this._created = value; } }


    public static List<AutomationEntry> Load(MobilePaywallDatabase database, List<AutomationGroupEntry> groups)
    {
      List<AutomationEntry> result = new List<AutomationEntry>();
      if (groups == null || groups.Count == 0)
        return result;

      string ids = "";
      foreach(AutomationGroupEntry ge in groups)
      {
        if (!ids.Equals("")) ids += ",";
        ids += ge.ID;
      }

      DataTable table = database.Load(AutomationEntry.SqlCommand.Replace(AutomationEntry.SqlIndexWildcard, ids));
      if (table == null)
        return result;

      foreach(DataRow row in table.Rows)
      {
        AutomationEntry entry = new AutomationEntry();

        entry.ID = Int32.Parse(row[(int)AutomationEntry.Columns.ID].ToString());
        entry.Name = row[(int)AutomationEntry.Columns.Name].ToString();

        ServiceEntry service = new ServiceEntry();
        service.ID = Int32.Parse(row[(int)AutomationEntry.Columns.ServiceID].ToString());
        service.Name = row[(int)AutomationEntry.Columns.ServiceName].ToString();
        service.Description = row[(int)AutomationEntry.Columns.ServiceDescription].ToString();
        entry.Service = service;

        MobileOperatorEntry mobileOperator = null;
        if(row[(int)AutomationEntry.Columns.MobileOperatorID] != DBNull.Value)
        {
          mobileOperator = new MobileOperatorEntry();
          mobileOperator.ID = Int32.Parse(row[(int)AutomationEntry.Columns.MobileOperatorID].ToString());
          mobileOperator.Name = row[(int)AutomationEntry.Columns.MobileOperatorID].ToString();
        }
        entry.MobileOperator = mobileOperator;


        entry.AutomationGroupID = Int32.Parse(row[(int)AutomationEntry.Columns.AutomationGroupID].ToString());
        entry.TransactionLimit = Int32.Parse(row[(int)AutomationEntry.Columns.Limit].ToString());
        entry.ExternalOfferName = row[(int)AutomationEntry.Columns.ExternalOfferName].ToString();
        entry.IsActive = (bool)row[(int)AutomationEntry.Columns.IsActive];
        entry.Index = Int32.Parse(row[(int)AutomationEntry.Columns.Index].ToString());
        entry.Comment = row[(int)AutomationEntry.Columns.Comment].ToString();
        entry.Created = row[(int)AutomationEntry.Columns.Created].ToString();

        result.Add(entry);
      }

      return result;
    }

  }
}
