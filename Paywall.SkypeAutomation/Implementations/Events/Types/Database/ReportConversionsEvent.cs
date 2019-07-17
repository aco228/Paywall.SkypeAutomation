using Paywall.Database.Managers.MobilePaywall;
using Paywall.SkypeAutomation.Core.Events;
using Paywall.SkypeAutomation.Core.Task;
using Paywall.SkypeAutomation.Implementations.SkypeApi;
using Paywall.SkypeAutomation.Implementations.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Events.Types
{
  public class ReportConversionsEvent : EventBase
  {

    public ReportConversionsEvent()
      : base("reportConversions", new EventTrigger(1, Core.TriggerTimeType.Hours, false, true))
    {
      this.SetInitialCalledTime(DateTime.Now.AddMinutes(-5));
    }

    protected override void Execute()
    {
      ReportConversionEventTaskObject taskObject = new ReportConversionEventTaskObject();
      taskObject.Data = this;
      DefaultTask task = new DefaultTask(taskObject);
      DatabaseTaskManager.Current.AddTask(task);
    }
  }

  public class ReportConversionEventTaskObject : TaskObject
  {
    public override void Execute()
    {
      EventBase reportEvent = this.Data as EventBase;
      string command = "all";
      string group = string.Empty ;
      string attribute = string.Empty;

      LoadAllMobilePaywallManager manager = new LoadAllMobilePaywallManager();
      LoadMobilePaywallResult result = manager.Load(command, group, attribute);

      string output = Environment.NewLine + string.Format("*Report: *. Next one will be in *{0}*", reportEvent.NextTrigger()) + Environment.NewLine;
      output += "Report Loaded: " + result.Loaded.ToString() + Environment.NewLine;
      if (!string.IsNullOrEmpty(result.Group) && !string.IsNullOrEmpty(result.Attribute))
        output += string.Format(" *{0}* filter, search: '*{1}*' ", result.Group.ToUpper(), result.Attribute) + Environment.NewLine;

      output += Environment.NewLine;

      if (result.Clicks > -1)
        output += string.Format(" _Clicks_ = *{0}* ", result.Clicks) + Environment.NewLine;
      if (result.Identifications > -1)
        output += string.Format(" _Identifications_ = *{0}* ", result.Identifications) + Environment.NewLine;
      if (result.Transactions > -1)
        output += string.Format(" _Transactions_ = *{0}* ", result.Transactions) + Environment.NewLine;
      if (result.Subsequents > -1)
        output += string.Format(" _Subsequents_ = *{0}* ", result.Subsequents) + Environment.NewLine;

      output += Environment.NewLine;

      Program.SkypeProcess.Send(BotReportConversation.Current, output);

    }
  }

}
