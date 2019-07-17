using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Managers.MobilePaywall
{
  public class LoadAllMobilePaywallManager : MobilePaywallDatabase
  {

    public LoadMobilePaywallResult Load( string command, string group, string parameter)
    {
      string clickSql = " SELECT -1 ";
      string identificationSql = " SELECT -1 ";
      string transactionSql = " SELECT -1 ";
      string subsequentSql = " SELECT -1 ";

      if (command.Equals("all"))
      {
        clickSql = this.ComposeClickSql(group, parameter);
        identificationSql = this.ComposeIdentificationSql(group, parameter);
        transactionSql = this.ComposeTransactionSql(group, parameter);
        subsequentSql = this.ComposeSubsequentSql(group, parameter);
      }
      else if (command.Equals("transaction"))
        transactionSql = this.ComposeTransactionSql(group, parameter);
      else if (command.Equals("click"))
        clickSql = this.ComposeClickSql(group, parameter);

      string sqlCommand = string.Format("({0}) UNION ALL ({1}) UNION ALL ({2}) UNION ALL ({3});",
        clickSql,
        identificationSql,
        transactionSql,
        subsequentSql);      

      LoadMobilePaywallResult result = new LoadMobilePaywallResult(group, parameter);
      DataTable table = this.Load(sqlCommand);
      if (table == null)
        return result;

      result.Loaded = DateTime.Now;
      result.Clicks = Int32.Parse(table.Rows[0][0].ToString());
      result.Identifications = Int32.Parse(table.Rows[1][0].ToString());
      result.Transactions = Int32.Parse(table.Rows[2][0].ToString());
      result.Subsequents = Int32.Parse(table.Rows[3][0].ToString());

      return result;
    }


    public string ComposeClickSql(string group, string parameter)
    {
      string command = string.Empty;
      #region # sql command #

      command = " SELECT COUNT(*) FROM MobilePaywall.core.UserSession AS us ";

      if (!string.IsNullOrEmpty(group))
        command += " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID ";

      command += " WHERE  " +
                  " us.Created >= CONVERT(DATE ,GETDATE ())";

      if (group.Equals("country"))
        command += " AND s.Description LIKE '" + parameter + "%' ";
      else if (group.Equals("service"))
        command += " AND s.Name LIKE '%" + parameter + "%' ";

      #endregion

      return command;
    }

    public string ComposeIdentificationSql(string group, string parameter)
    {
      string command = string.Empty;
      #region # sql command #

      command = " SELECT COUNT(*) FROM MobilePaywall.core.UserSession AS us ";

      if (!string.IsNullOrEmpty(group))
        command += " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID ";

      command += " WHERE  " +
                  " us.Created >= CONVERT(DATE ,GETDATE ())" +
                  " AND us.CustomerID IS NOT NULL ";

      if (group.Equals("country"))
        command += " AND s.Description LIKE '" + parameter + "%' ";
      else if (group.Equals("service"))
        command += " AND s.Name LIKE '%" + parameter + "%' ";

      #endregion

      return command;
    }

    public string ComposeTransactionSql(string group, string parameter)
    {
      string command = string.Empty;
      #region # sql command #

      command = " SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] as t " +
                " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID ";

      if (!string.IsNullOrEmpty(group))
      {

        command += " LEFT OUTER JOIN MobilePaywall.core.ServiceOffer AS so ON p.ServiceOfferID=so.ServiceOfferID " +
                  " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON so.ServiceID=s.ServiceID ";
      }

      command += " WHERE  " +
                  " t.TransactionStatusID=5 AND  " +
                  " p.Created >= CONVERT(DATE ,GETDATE ()) AND " +
                  " t.Created >= CONVERT(DATE ,GETDATE ())";

      if (group.Equals("country"))
        command += " AND s.Description LIKE '" + parameter + "%' ";
      else if (group.Equals("service"))
        command += " AND s.Name LIKE '%" + parameter + "%' ";

      #endregion

      return command;
    }

    public string ComposeSubsequentSql(string group, string parameter)
    {
      string command = string.Empty;
      #region # sql command #

      command = " SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] as t " +
                " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID ";

      if (!string.IsNullOrEmpty(group))
      {

        command += " LEFT OUTER JOIN MobilePaywall.core.ServiceOffer AS so ON p.ServiceOfferID=so.ServiceOfferID " +
                  " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON so.ServiceID=s.ServiceID ";
      }

      command += " WHERE  " +
                  " t.TransactionStatusID=5 AND  " +
                  " p.Created <= CONVERT(DATE ,GETDATE ()) AND " +
                  " t.Created >= CONVERT(DATE ,GETDATE ())";

      if (group.Equals("country"))
        command += " AND s.Description LIKE '" + parameter + "%' ";
      else if (group.Equals("service"))
        command += " AND s.Name LIKE '%" + parameter + "%' ";

      #endregion

      return command;
    }

  }

  public class LoadMobilePaywallResult
  {
    private int _clicks = -1;
    private int _identifications = -1;
    private int _transactions = -1;
    private int _subsequents = -1;
    private string _group = string.Empty;
    private string _attribute = string.Empty;
    private DateTime _loaded;

    public int Clicks { get { return this._clicks; } set { this._clicks = value; } }
    public int Identifications { get { return this._identifications; } set { this._identifications = value; } }
    public int Transactions { get { return this._transactions; } set { this._transactions = value; } }
    public int Subsequents { get { return this._subsequents; } set { this._subsequents = value; } }
    public string Group { get { return this._group; } }
    public string Attribute { get { return this._attribute; } }
    public DateTime Loaded { get { return this._loaded; } set { this._loaded = value; } }
    
    public LoadMobilePaywallResult(string group, string attribute)
    {
      this._group = group;
      this._attribute = attribute;
    }
    
  }

}
