using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Core.SkypeCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeCommand
{
  public class AleksandarKonatarCommand : SkypeCommandBase
  {

    public AleksandarKonatarCommand(SkypeConversationBase conversation)
      : base("aleksandarkonatarcommand", "aco", conversation)
    { }

    protected override SkypeCommandResultBase CheckAndPrepare(SkypeMessage message)
    {
      SkypeCommandResultBase result = new SkypeCommandResultBase(this, message);
      if (string.IsNullOrEmpty(message.Message))
        return result;

      string[] data = message.Message.Split(' ');
      if (!data.ElementAt(0).ToLower().Equals(this.CommandName))
        return result;

      //result.Parameters = data[1];
      result.Success = true;
      return result;      
    }

    // LOAD LAST MESSAGES IDS
    protected override void Execute(SkypeCommandResultBase result)
    {
      List<SkypeMessage> messages = this.Conversion.Parent.SkypeAPI.GetLastMessages(5);
      string output = string.Empty;
      foreach (SkypeMessage sm in messages)
        output += string.Format("{0} - {1} - {2}", sm.ChatMessage.FromDisplayName, sm.ChatMessage.ChatName, sm.Message) + Environment.NewLine;

      Program.SkypeProcess.Send(this.Conversion, output);
    }
  }
}
