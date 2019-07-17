using Paywall.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Automatization
{
  public class TransactionManager
  {

    #region # sql command #

    public static string WildcardMobileOperatorTemplate = " LEFT OUTER JOIN MobilePaywall.core.Customer AS c ON p.CustomerID=c.CustomerID AND c.MobileOperatorID=[MOBILEOPERATORID] ";

    public static string SqlCommandTemplate = " SELECT [AUTOMATION_REFERENCE], COUNT(*) "
                                       + " FROM MobilePaywall.core.[Transaction] AS t "
                                       + " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID "
                                       + " LEFT OUTER JOIN MobilePaywall.core.ServiceOffer AS so ON p.ServiceOfferID=so.ServiceOfferID "
                                       + " [MOBILEOPERATPR] "
                                       + " WHERE so.ServiceID=[SERVICEID] "
                                           + " AND t.TransactionStatusID=5 "
                                           + " AND p.Created >= '[DATETIME]' "
                                           + " AND t.Created >= '[DATETIME]' ";

    #endregion

    private AutomatizationManagerBase _parent = null;
    private List<TransactionEntry> _transactions = null;
    private DateTime _lastLoaded;

    public List<TransactionEntry> Transaction { get { return this._transactions; } }

    public TransactionManager(AutomatizationManagerBase parent)
    {
      this._parent = parent;
    }

    public void Reset()
    {
      this._lastLoaded = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    }

    private string PrepareQuery()
    {
      string query = "";
      foreach(AutomationGroup ag in this._parent.Groups)
        foreach(Automation a in ag.RunningAutomations)
        {
          if (query != "") query += " UNION ALL ";

          string mobileOperatorTemplate = "";
          if (a.MobileOperatorID != -1)
            mobileOperatorTemplate = TransactionManager.WildcardMobileOperatorTemplate.Replace("[MOBILEOPERATORID]", a.MobileOperatorID.ToString());

          query += TransactionManager.SqlCommandTemplate.
            Replace("[AUTOMATION_REFERENCE]", a.ID.ToString()).
            Replace("[SERVICEID]", a.Data.Service.ID.ToString()).
            Replace("[MOBILEOPERATPR]", mobileOperatorTemplate).
            Replace("[DATETIME]", this._parent.MobilePaywallDatabase.Date(this._lastLoaded));

        }
      return query;
    }

    public TransactionEntry Get(Automation a)
    {
      if (this._transactions == null || this._transactions.Count == 0) return null;
      return (from t in this._transactions where t.ServiceID == a.ID select t).FirstOrDefault();
    }

    public void Call()
    {
      this._transactions  = TransactionEntry.Load(this._parent.MobilePaywallDatabase, this.PrepareQuery());
      if (this._transactions == null)
        this._transactions = new List<TransactionEntry>();
    }



  }
}
