using Paywall.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Automatization
{
  public class AutomationGroup
  {
    private AutomatizationManagerBase _manager = null;
    private AutomationGroupEntry _entry = null;
    private List<Automation> _automations = null;
    private bool _isActive = false;
    private int _maxID = -1;

    public int ID { get { return this._entry.ID; } }
    public AutomationGroupEntry Data { get { return this._entry; } }
    public CoalisionType TimeCoalision { get { return this._entry.StartDateTimeCoalision; } }
    public AutomatizationManagerBase Manager { get { return this._manager; } }
    public List<Automation> ActiveAutomations { get { return (from a in this._automations where a.EntryIsActive && a.OfferExsist select a).ToList(); } }
    public List<Automation> RunningAutomations { get { return (from a in this._automations where a.IsActive select a).ToList(); } }
    public int RotationTime { get { return this._entry.RotationTime; } }
    public bool Finished { get { return (from a in this._automations where a.Finished || !a.EntryIsActive || !a.OfferExsist select a).Count() == this._automations.Count; } }
    public bool EntryIsActive { get { return this._entry.IsActive; } }
    public bool IsActive { get { return this._isActive; } set { this._isActive = value; } }

    public string Status
    {
      get
      {
        string automationName = string.Format("{0}. (*{1}*) {2}", this._entry.ID, this._entry.Country.TwoLetterIsoCode, this._entry.Name);

        if (!this._isActive)
          return string.Format("-   ~{0}~ ( Not active ) ", automationName) + Environment.NewLine;
        else if (this.TimeCoalision == CoalisionType.Before)
          return string.Format("-   ~{0}~ ( Should start {0} ) ", automationName, this._entry.StartTime) + Environment.NewLine;
        else if (this.TimeCoalision == CoalisionType.After)
          return string.Format("-   ~{0}~ ( Ended in {0} ) ", automationName, this._entry.EndTime) + Environment.NewLine;
        else if (this.Finished)
          return string.Format("-   *{0}* ( Finished! ) ", automationName) + Environment.NewLine;

        string output = string.Format("-   *{0}* ( Rotation on {1}min )  ", automationName, this._entry.RotationTime) + Environment.NewLine;
        foreach (Automation a in this._automations)
          output += a.Status + Environment.NewLine;

        return output;
      }
    }

    public AutomationGroup(AutomatizationManagerBase parent, AutomationGroupEntry entry, List<AutomationEntry> automations, List<KiwiclicksOfferEntry> offers)
    {
      this._manager = parent;
      this._entry = entry;
      this._automations = new List<Automation>();
      foreach (AutomationEntry ae in automations)
      {
        this._automations.Add(new Automation(this, ae));
        if (ae.ID > this._maxID) this._maxID = ae.ID;
      }

      this.SetOffers(offers);

    }

    public void SetOffers(List<KiwiclicksOfferEntry> offers)
    {
      foreach (Automation a in this._automations)
        a.SetOffer(offers);

      if (this.EntryIsActive && this.TimeCoalision == CoalisionType.Now)
      {
        this._isActive = true;
        if ((from a in this._automations where a.IsActive select a).Count() == 0)
          this.Rotation();
      }
    }

    public void Rotation(Automation reference = null, string priorMessage = "")
    {
      this.TimeError();
      if (!this._isActive)
        return;

      string message = string.Empty;
      this.CloseAutomations();

      if (!string.IsNullOrEmpty(priorMessage))
        message += priorMessage + Environment.NewLine;

      if (this.ActiveAutomations.Count == 1)
        return;

      Automation nextAutomation = NextAutomation(reference);
      if (nextAutomation == null)
        throw new ArgumentException("WTF?");
      nextAutomation.Start();

      message += "      >>> " + nextAutomation.Header + " *STARTED* ";
      this.Manager.Messages.Add(new AutomatizationToSkypeMessage(message));
    }

    public void CloseAutomations()
    {
      foreach (Automation auto in this.RunningAutomations)
        auto.Stop();
    }

    // SUMMARY: Find next automation
    public Automation NextAutomation(Automation reference = null)
    {
      int errorTries = this._automations.Count;
      int tried = 0;

      int index = reference == null ? 0 : reference.ID;
      index = index + 1 > this._maxID ? 0 : index + 1;

      while(true)
      {
        if(tried > errorTries)
          break;

        Automation automation = (from a in this._automations where a.ID == index && a.OfferExsist && !a.IsActive && !a.Finished && a.EntryIsActive select a).FirstOrDefault();
        if (automation != null)
          return automation;

        index++;
        tried++;
      }

      return null;
    }

    public int GetIndex(Automation reference)
    {
      if (reference == null)
        return 0;

      for (int i = 0; i < this._automations.Count - 1; i++)
        if (reference.ID == this._automations.ElementAt(i).ID)
          return i;
      return 0;
    }


    public void TimeError()
    {
      if(this.TimeCoalision != CoalisionType.Now && this._isActive)
      {
        this.CloseAutomations();
        this._isActive = false;
      }

      if(this.TimeCoalision == CoalisionType.Now && !this._isActive)
      {

      }
    }

    public void Execute()
    {
      this.TimeError();
      if (!this._isActive)
        return;

      foreach (Automation automation in this._automations)
        if (automation.EntryIsActive)
          automation.Execute();
    }

  }

  public enum RotationReason
  {
    Unknown,
    Limit,
    Time,
    NoAction
  }

}
