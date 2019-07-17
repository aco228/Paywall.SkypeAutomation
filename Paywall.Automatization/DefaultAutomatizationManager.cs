using Paywall.Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Automatization
{
  public class DefaultAutomatizationManager : AutomatizationManagerBase
  {

    public DefaultAutomatizationManager()
      :base()
    {
    }
    
    public override void Restart()
    {
      this.Groups = new List<AutomationGroup>();
      this.TransactionManager.Reset();
      List<AutomationGroupEntry> automationGroupList = AutomationGroupEntry.LoadMany(this.MobilePaywallDatabase);
      List<AutomationEntry> automationList = AutomationEntry.Load(this.MobilePaywallDatabase, (from agl in automationGroupList where agl.IsActive select agl).ToList());
      List<KiwiclicksOfferEntry> offers = KiwiclicksOfferEntry.LoadMany(this.KiwiclickDataabse, automationList);

      this.Messages.Add(new AutomatizationToSkypeMessage("*Automation Groups* Loaded"));
      foreach (AutomationGroupEntry entry in automationGroupList)
      {
        if (!entry.IsActive)
          continue;

        List<AutomationEntry> groupEntryAutomations = (from ae in automationList where ae.AutomationGroupID == entry.ID select ae).ToList();
        List<KiwiclicksOfferEntry> groupOffers = (from gea in groupEntryAutomations from o in offers where gea.ExternalOfferName.Equals(o.Name) select o).ToList();

        if (groupEntryAutomations == null || groupEntryAutomations.Count == 0)
          continue;

        this.Groups.Add(new AutomationGroup(this, entry, groupEntryAutomations, groupOffers));
      }

      this.TransactionManager.Call();
      
    }

    public override void Print(int groupID = -1)
    {
      if (!this.Running)
      {
        this.Messages.Add(new AutomatizationToSkypeMessage("Automation process is stoped!"));
        return;
      }

      string output = ""; ;
      foreach (AutomationGroup ag in this.Groups)
        if(groupID == -1 || groupID == ag.ID)
          output += ag.Status;

      this.Messages.Add(new AutomatizationToSkypeMessage(output));
    }

    protected override void Execute()
    {
      if (!this.Running)
        return;

      this.TransactionManager.Call();

      foreach (AutomationGroup ag in this.Groups)
        if (ag.EntryIsActive)
          ag.Execute();
    }

  }
}
