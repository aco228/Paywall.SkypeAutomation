using Paywall.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Automatization
{
  public class Automation
  {
    private AutomationGroup _group = null;
    private AutomationEntry _entry = null;
    private TransactionEntry _transaction = null;
    private KiwiclicksOfferEntry _offer = null;
    private string _message = string.Empty;
    private DateTime _lastTransaction;
    private bool _isActive = false;

    public int ID { get { return this._entry.ID; } }
    public AutomationEntry Data { get { return this._entry; } }
    public int MobileOperatorID { get { return this._entry.MobileOperator != null ? this._entry.MobileOperator.ID : -1; } }
    public bool EntryIsActive { get { return this._entry.IsActive && string.IsNullOrEmpty(this._message); } }
    public bool OfferExsist { get { return this._offer != null; } }
    public bool IsActive { get { return this._isActive; } }
    public bool Finished { get { return this._transaction.Count >= this._entry.TransactionLimit; } }

    public string Status
    {
      get
      {
        string automationName = string.Format("{0} - {1}", this._entry.Service.Name, this._entry.Name);
        string transactions = string.Format(" *({0}/{1})* ({2}min)", this._transaction.Count, this._entry.TransactionLimit, (DateTime.Now - this._lastTransaction).Minutes.ToString());

        if (!string.IsNullOrEmpty(this._message))
          return string.Format("-             X  ~{0}~ {1}", automationName, this._message);
        if (this.Finished)
          return string.Format("-             X  ~{0}~ {1}", automationName, transactions);
        else if (!this._isActive)
          return string.Format("-             X  _{0}_ {1}", automationName, transactions);
        else
          return string.Format("-     *===>*  *{0}* {1}", automationName, transactions);
      }
    }

    public string Header
    {
      get
      {
        return string.Format("*{0}* .{1}. *{2}* .{3}: ({4}/{5}) :",
           this._group.Data.Country.TwoLetterIsoCode,
           this._group.Data.Name,
           this.Data.Service.Name,
           this.Data.Name,
           this._transaction.Count,
           this._entry.TransactionLimit);
      }
    }

    public Automation(AutomationGroup parent, AutomationEntry entry)
    {
      this._group = parent;

      this._transaction = new TransactionEntry();
      this._transaction.Count = 0;

      this._transaction.Created = DateTime.Now;
      this._entry = entry;
    }

    public void SetOffer(List<KiwiclicksOfferEntry> offers)
    {
      KiwiclicksOfferEntry offer = (from o in offers where o.Name.Equals(this._entry.ExternalOfferName) select o).FirstOrDefault();
      if (offer == null)
      {
        this._message = "No offer";
        return;
      }

      this._offer = offer;
      this._isActive = false;
      this._offer.ConstructUrl(this.Data);
      this._offer.Update(this._group.Manager.KiwiclickDataabse, false);
      this._lastTransaction = DateTime.Now;

      TransactionEntry intialTransaction = this._group.Manager.TransactionManager.Get(this);
      if (intialTransaction != null)
        this._transaction = intialTransaction;

    }

    // SUMMARY: Stop automation
    public void Stop()
    {
      if (!this._isActive && this._offer != null)
        return;

      this._isActive = false;
      this._offer.Update(this._group.Manager.KiwiclickDataabse, false);
    }

    public void Start()
    {
      if (this._isActive && this._offer != null || this.Finished)
        return;

      this._isActive = true;
      this._offer.Update(this._group.Manager.KiwiclickDataabse, false);
      this._lastTransaction = DateTime.Now;
    }

    private void UpdateTransactions()
    {
      TransactionEntry transaction = (from t in this._group.Manager.TransactionManager.Transaction where t.ServiceID == this.ID select t).FirstOrDefault();

      if (transaction == null) 
        return;

      if(transaction.Count > this._transaction.Count)
      {
        this._transaction = transaction;
        this._lastTransaction = DateTime.Now;
      }
    }

    private void Check()
    {
      string message = "";
      if (this._transaction.Count >= this._entry.TransactionLimit)
        message = " <<<      " + this.Header + string.Format(" *FINISHED* ");
      else if (DateTime.Now > this._lastTransaction.AddMinutes(this._group.RotationTime))
        message = " <<<      " + this.Header + string.Format(" *no transaction in {0}min* ", this._group.RotationTime);

      if(!string.IsNullOrEmpty(message))
      {
        this._isActive = false;
        this._offer.Update(this._group.Manager.KiwiclickDataabse, false);
        this._group.Rotation(this, message);
      }

    }

    public void Execute()
    {
      if (!this._isActive)
        return;

      this.UpdateTransactions();
      this.Check();
    }
  }
}
