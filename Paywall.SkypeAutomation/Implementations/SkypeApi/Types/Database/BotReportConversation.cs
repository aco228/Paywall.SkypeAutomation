using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Implementations.SkypeCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeApi
{
  public class BotReportConversation : SkypeConversationBase
  {

    public static string Key = "botReportConversation";

    public BotReportConversation()
      : base(BotReportConversation.Key, "#mbot.BotReport", "#mbot.BotReport", "Conversion used to for bot to report conversions and so on")
    {
      this.CommandSet.AddCommand(new TestCommand(this));
      this.CommandSet.AddCommand(new SelectCommand(this));
    }

    public static BotReportConversation Current
    {
      get
      {
        return (Program.Managers[ManagersType.SkypeConversaionManager] as SkypeConversionManager)[BotReportConversation.Key] as BotReportConversation;
      }
    }

    protected override void Execute()
    {
      List<SkypeMessage> messages = this.GetMessages();
      if (messages.Count == 0)
        return;

      Program.Blue("Messages arrived");
      this.CommandSet.ExecuteCommands(messages);
    }

  }
}
