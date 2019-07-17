using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Managers.MobilePaywall
{
  public class FatalMobilePaywallManager : MobilePaywallDatabase
  {

    public int GetLastWebLogID()
    {
      string command = string.Empty;

      #region # sql command #

      command = "SELECT TOP 1 WebLogID FROM MobilePaywall.log.WebLog WHERE Level LIKE 'FATAL' ORDER BY WebLogID DESC;";

      #endregion

      DataTable table = this.Load(command);
      if (table == null)
        return 0;

      return Int32.Parse(table.Rows[0][0].ToString());
    }


    public List<FatalMobilePaywall> Load(int lastID)
    {
      string command = string.Empty;

      #region # sql command #

      command = "SELECT WebLogID, Date, Message, Exception FROM MobilePaywall.log.WebLog WHERE Level LIKE 'FATAL' AND WebLogID > " + lastID + ";";

      #endregion

      List<FatalMobilePaywall> result = new List<FatalMobilePaywall>();
      DataTable table = this.Load(command);
      if (table == null)
        return result;

      foreach(DataRow row in table.Rows)
      {
        FatalMobilePaywall entry = new FatalMobilePaywall();
        entry.WebLogID = Int32.Parse(row[(int)FatalMobilePaywall.Columns.WebLogID].ToString());
        entry.Date = row[(int)FatalMobilePaywall.Columns.Date].ToString();
        entry.Message = row[(int)FatalMobilePaywall.Columns.Logger].ToString();
        entry.Exception = !string.IsNullOrEmpty(row[(int)FatalMobilePaywall.Columns.Exception].ToString());
        result.Add(entry);
      }

      return result;
    }

  }

  public class FatalMobilePaywall
  {
    private int _webLogID = -1;
    private string _date = string.Empty;
    private string _message = string.Empty;
    private bool _exception = false;

    public int WebLogID { get { return this._webLogID; } set { this._webLogID = value; } }
    public string Date { get { return this._date; } set { this._date = value; } }
    public string Message { get { return this._message.Substring(this._message.IndexOf(' '), 100); } set { this._message = value; } }
    public bool Exception { get { return this._exception; } set { this._exception = value; } }
    public string HasException
    {
      get
      {
        return this._exception ? "HAS EXCEPTION" : "";
      }
    }

    public enum Columns
    {
      WebLogID,
      Date,
      Logger,
      Exception
    }

  }

}
