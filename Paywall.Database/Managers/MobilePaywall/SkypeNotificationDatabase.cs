using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Managers.MobilePaywall
{
  public  class SkypeNotificationDatabase : MobilePaywallDatabase
  {

    public int GetLastID()
    {
      string command = string.Empty;

      #region # sql command #

      command = "SELECT TOP 1 SkypeNotificationID FROM MobilePaywall.log.SkypeNotification ORDER BY SkypeNotificationID DESC;";

      #endregion

      DataTable table = this.Load(command);
      if (table == null)
        return 0;

      return Int32.Parse(table.Rows[0][0].ToString());
    }

    public List<SkypeNotificationMobilePaywall> Load(int lastID)
    {
      string command = string.Empty;

      #region # sql command #

      command = "SELECT SkypeNotificationID, Sender, Identifier, Message, Created FROM MobilePaywall.log.SkypeNotification WHERE SkypeNotificationID > " + lastID + ";";

      #endregion

      List<SkypeNotificationMobilePaywall> result = new List<SkypeNotificationMobilePaywall>();
      DataTable table = this.Load(command);
      if (table == null)
        return result;

      foreach (DataRow row in table.Rows)
      {
        SkypeNotificationMobilePaywall entry = new SkypeNotificationMobilePaywall();
        entry.ID = Int32.Parse(row[(int)SkypeNotificationMobilePaywall.Columns.ID].ToString());
        entry.Sender = row[(int)SkypeNotificationMobilePaywall.Columns.Sender].ToString();
        entry.Identifier = row[(int)SkypeNotificationMobilePaywall.Columns.Identifier].ToString();
        entry.Message = row[(int)SkypeNotificationMobilePaywall.Columns.Message].ToString();
        entry.Created = row[(int)SkypeNotificationMobilePaywall.Columns.Created].ToString();
        result.Add(entry);
      }

      return result;
    }
  }

  public class SkypeNotificationMobilePaywall
  {
    private int _id = -1;
    private string _sender = string.Empty;
    private string _identifier = string.Empty;
    private string _message = string.Empty;
    private string _created = string.Empty;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string Sender { get { return this._sender; } set { this._sender = value; } }
    public string Identifier { get { return this._identifier; } set { this._identifier = value; } }
    public string Message { get { return this._message; } set { this._message = value; } }
    public string Created { get { return this._created; } set { this._created = value; } }

    public enum Columns
    {
      ID,
      Sender,
      Identifier,
      Message,
      Created
    }
  }

}
