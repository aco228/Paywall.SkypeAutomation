using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Implementations.SkypeCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeApi
{
  public class AleksandarKonatarConversation : SkypeConversationBase
  {
    #region # default strings #
    
    private string _helpString = "Command list : " + Environment.NewLine +
                                 " -help ( Help )" + Environment.NewLine + 
                                 " -s eroticvids.mobi ( get stats for eroticvids ) " + Environment.NewLine + 
                                 " -c de ( get stats for country de )" + Environment.NewLine + 
                                 " -test ( TESTING )";
    
    #endregion

    public static string Key = "monkeys.aleksandar.konatar";

    public AleksandarKonatarConversation()
      : base(AleksandarKonatarConversation.Key, "Aleksandar Konatar", "monkeys.aleksandar.konatar", "Test conversation")
    {
      this.CommandSet = new DefaultCommandSet();
      this.CommandSet.AddCommand(new AleksandarKonatarCommand(this));
    }

    public static AleksandarKonatarConversation Current
    {
      get
      {
        return (Program.Managers[ManagersType.SkypeConversaionManager] as SkypeConversionManager)[AleksandarKonatarConversation.Key] as AleksandarKonatarConversation;
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
