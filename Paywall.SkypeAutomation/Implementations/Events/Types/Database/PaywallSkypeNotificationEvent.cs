using Paywall.Database.Managers.MobilePaywall;
using Paywall.SkypeAutomation.Core.Events;
using Paywall.SkypeAutomation.Core.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Events.Types
{
  public class PaywallSkypeNotificationEvent : EventBase
  {

    public SkypeNotificationDatabase DatabaseManager = null;
    public int _lastSkypeID = -1;

    public PaywallSkypeNotificationEvent()
      : base("paywallSkypeNotificationEvent", new EventTrigger(5, Core.TriggerTimeType.Seconds, false, true))
    {
      this.DatabaseManager = new SkypeNotificationDatabase();
      this._lastSkypeID = this.DatabaseManager.GetLastID();
    }

    protected override void Execute()
    {
      List<SkypeNotificationMobilePaywall> fatals = this.DatabaseManager.Load(this._lastSkypeID);
      if (fatals.Count == 0)
        return;

      foreach (SkypeNotificationMobilePaywall entry in fatals)
      {
        SkypeConversationBase communication = (Program.Managers[ManagersType.SkypeConversaionManager] as SkypeConversionManager)[entry.Identifier];
        if (communication == null)
          continue;

        string output = string.Format("_{0}_ {1} *{2}* : {3}", entry.Created, Environment.NewLine, entry.Sender, entry.Message);
        Program.SkypeProcess.Send(communication, output);
        this._lastSkypeID = entry.ID;
      }
    }

  }
}
