using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class TransactionEntry
  {
    public enum Columns
    {
      Service,
      Count
    }


    private int _count = -1;
    private int _serviceID = -1;
    private DateTime _created;

    public int Count { get { return this._count; } set { this._count = value; } }
    public int ServiceID { get { return this._serviceID; } set { this._serviceID = value; } }
    public DateTime Created { get { return this._created; } set { this._created = value; } }

    public static List<TransactionEntry> Load(MobilePaywallDatabase database, string query)
    {
      List<TransactionEntry> result = new List<TransactionEntry>();

      DataTable table = database.Load(query);
      if (table == null)
        return result;

      foreach(DataRow row in table.Rows)
      {
        TransactionEntry entry = new TransactionEntry();
        entry.Count = Int32.Parse(row[(int)TransactionEntry.Columns.Count].ToString());
        entry.ServiceID = Int32.Parse(row[(int)TransactionEntry.Columns.Service].ToString());

        entry.Created = DateTime.Now;
        result.Add(entry);
      }

      return result;
    }

  }
}
