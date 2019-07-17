using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class KiwiclicksOfferEntry
  {

    #region # sql #

    public static string SqlNameWildcard = "[NAMES]";
    public static string SqlCommandMany = "SELECT offer_id, offer_name, offer_url, offer_inactive FROM mtkiwiclick.mt_offers WHERE offer_name IN ("+ KiwiclicksOfferEntry.SqlNameWildcard +");";

    public static string SqlIDWildcard = "[ID]";
    public static string SqlInactiveWildcard = "[OFFER_INACTIVE]";
    public static string SqlUrlWildcard = "[OFFER_URL]";
    public static string SqlCommandUpdate = "UPDATE mtkiwiclick.mt_offers set offer_inactive=" + KiwiclicksOfferEntry.SqlInactiveWildcard + ", offer_url='" + KiwiclicksOfferEntry.SqlUrlWildcard + "' where offer_id=" + KiwiclicksOfferEntry.SqlIDWildcard + ";";


    public enum Columns
    {
      ID,
      Name,
      OfferUrl,
      OfferInactive
    }

    #endregion

    private int _id = -1;
    private string _name = string.Empty;
    private string _url = string.Empty;
    private bool _isActive = false;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string Name { get { return this._name; } set { this._name = value;} }
    public string Url { get { return this._url; } set { this._url = value; } }
    public bool IsActive { get { return this._isActive; } set { this._isActive = value; } }
    

    // SUMMARY: Load all
    public static List<KiwiclicksOfferEntry> LoadMany(KiwiclicksDatabase database, List<AutomationEntry> automations)
    {
      List<KiwiclicksOfferEntry> result = new List<KiwiclicksOfferEntry>();
      if (automations == null || automations.Count == 0)
        return result;

      string names = "";
      foreach(AutomationEntry ae in automations)
        if(ae.IsActive && !string.IsNullOrEmpty(ae.ExternalOfferName))
        {
          if(names != "") names += ",";
          names += string.Format("'{0}'", ae.ExternalOfferName);
        }

      string command = KiwiclicksOfferEntry.SqlCommandMany.Replace(KiwiclicksOfferEntry.SqlNameWildcard, names);
      DataTable table = database.Load(command);
      if (table == null)
        return result;

      foreach(DataRow row in table.Rows)
      {
        KiwiclicksOfferEntry entry = new KiwiclicksOfferEntry();
        entry.ID = Int32.Parse(row[(int)KiwiclicksOfferEntry.Columns.ID].ToString());
        entry.Name = row[(int)KiwiclicksOfferEntry.Columns.Name].ToString();
        entry.Url = row[(int)KiwiclicksOfferEntry.Columns.OfferUrl].ToString();
        entry.IsActive = row[(int)KiwiclicksOfferEntry.Columns.OfferInactive].ToString().Equals("0");
        result.Add(entry);
      }

      return result;
    }

    public void Update(KiwiclicksDatabase database, bool isActive)
    {
      this._isActive = isActive;
      
      string inactive = !this._isActive ? "0" : "1";
      string command = KiwiclicksOfferEntry.SqlCommandUpdate.
        Replace(KiwiclicksOfferEntry.SqlIDWildcard, this._id.ToString()).
        Replace(KiwiclicksOfferEntry.SqlInactiveWildcard, inactive).
        Replace(KiwiclicksOfferEntry.SqlUrlWildcard, this._url);

      database.Execute(command);
    }

    public void ConstructUrl(AutomationEntry automation)
    {
      if (string.IsNullOrEmpty(this._url))
        return;

      // CHECK MobileOperator
      string mobileOperator = "";
      if (automation.MobileOperator != null)
      {
        if (this._url.Contains("mno"))
          this._url = this._url.Replace(string.Format("mno={0}", this.GetValue("mno")), string.Format("mno={0}", automation.MobileOperator.ID));
        else
          mobileOperator = string.Format("&mno={0}", automation.MobileOperator.ID);
      }
      else if (this._url.Contains("mno"))
        this._url = this._url.Replace(string.Format("mno={0}", this.GetValue("mno")), string.Empty);


      // Add lock
      if (this._url.Contains("cpkl="))
        this._url = this._url.Replace(string.Format("cpkl={0}", this.GetValue("cpkl")), string.Format("cpkl={0}", automation.TransactionLimit));
      else
      {
        char starting = this._url.Contains('?') ? '&' : '?';
        this._url += starting + "cpkl=" + automation.TransactionLimit + mobileOperator;
      }
    }

    public string GetValue(string parameterName)
    {
      Match match = new Regex(string.Format(@"(\?{0}=([A-Za-z0-9]+))|(\&{0}=([A-Za-z0-9]+))", parameterName)).Match(this._url);
      string result = string.Empty;
      if (!string.IsNullOrEmpty(match.Groups[2].Value.ToString()))
        result = match.Groups[2].Value.ToString();
      else if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(match.Groups[4].Value.ToString()))
        result = match.Groups[4].Value.ToString();
      return result;
    }


  }
}
