using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Implementations.SkypeCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeApi
{
  // 19:f0d9ecb931ab45d4a5ca0829d59ccd38@thread.skype
  // #monkeys.paywall.bot/$*T;905
  public class SkypeAutomationConversation : SkypeConversationBase
  {

    public static string Key = "#mbot.BotAutomation";

    public SkypeAutomationConversation()
      : base(SkypeAutomationConversation.Key, "#mbot.BotAutomation", "#monkeys.paywall.bot/$*T;905", "Automation conversation poll")
    {
      this.CommandSet = new DefaultCommandSet();
    }

    public static SkypeAutomationConversation Current
    {
      get
      {
        return (Program.Managers[ManagersType.SkypeConversaionManager] as SkypeConversionManager)[SkypeAutomationConversation.Key] as SkypeAutomationConversation;
      }
    }

    protected override void Execute()
    {
      List<SkypeMessage> messages = this.GetMessages();
      if (messages.Count == 0)
        return;

      this.CommandSet.ExecuteCommands(messages);
    }
  }
}
