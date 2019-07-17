using Paywall.SkypeAutomation.Core.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeApi
{
  public class BotFatalConversation : SkypeConversationBase
  {

    public static string Key = "#mbot.BotFatal";

    public BotFatalConversation()
      : base(BotFatalConversation.Key, "#mbot.BotFatal", "#monkeys.paywall.bot/$*T;756", "Fatal conversation")
    {
      this.NonExecutable = true;
    }


    public static BotFatalConversation Current
    {
      get
      {
        return (Program.Managers[ManagersType.SkypeConversaionManager] as SkypeConversionManager)[BotFatalConversation.Key] as BotFatalConversation;
      }
    }

    protected override void Execute()
    { }

  }
}
