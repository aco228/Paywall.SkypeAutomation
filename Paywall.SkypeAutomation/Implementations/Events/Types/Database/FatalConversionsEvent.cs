using Paywall.Database.Managers.MobilePaywall;
using Paywall.SkypeAutomation.Core.Events;
using Paywall.SkypeAutomation.Implementations.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Events.Types
{
  public class FatalConversionsEvent : EventBase
  {

    private FatalMobilePaywallManager DatabaseManager = null;
    private int _lastWebLogID = -1;

    public FatalConversionsEvent()
      : base("fatalConversionEvent", new EventTrigger(10, Core.TriggerTimeType.Seconds, false))
    {
      DatabaseManager = new FatalMobilePaywallManager();
      this._lastWebLogID = this.DatabaseManager.GetLastWebLogID();
      //this._lastWebLogID = 138069693;
    }

    protected override void Execute()
    {
      List<FatalMobilePaywall> fatals = this.DatabaseManager.Load(this._lastWebLogID);
      if (fatals.Count == 0)
        return;

      string output = Environment.NewLine + "*FATALS*" + Environment.NewLine;
      foreach (FatalMobilePaywall fatal in fatals)
      {
        output += string.Format(" - *{0}* - {1} - {2} _{3}_ - {4} {5}", fatal.WebLogID, fatal.Date, Environment.NewLine, fatal.Message, fatal.HasException, Environment.NewLine) + Environment.NewLine;
        this._lastWebLogID = fatal.WebLogID;
      }
      output += Environment.NewLine;

      Program.SkypeProcess.Send(BotFatalConversation.Current, output);
    }
  }
}
