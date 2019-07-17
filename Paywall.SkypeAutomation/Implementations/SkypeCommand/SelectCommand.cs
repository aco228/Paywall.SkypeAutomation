using Paywall.Database.Managers.MobilePaywall;
using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Core.SkypeCommand;
using Paywall.SkypeAutomation.Core.Task;
using Paywall.SkypeAutomation.Implementations.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeCommand
{

  #region # Example Commands #
  /// <summary>
  /// SELECT ALL
  /// SELECT ALL FOR COUNTRY DE
  /// SELECT ALL FOR SERVICE mobilerotic
  /// SELECT TRANSACTIONS
  /// ( TRANSACTIONS, CLICKS, HELP )
  /// </summary>
  #endregion

  public class SelectCommand : SkypeCommandBase
  {
    public SelectCommand(SkypeConversationBase conversion)
      : base("selectCommand", "select", conversion)
    {

      this.AddHelpMessageLine(" - SELECT COMMAND EXAMPLES:");
      this.AddHelpMessageLine(" --- SELECT ALL");
      this.AddHelpMessageLine("--- SELECT ALL FOR COUNTRY DE");
      this.AddHelpMessageLine("--- SELECT ALL FOR SERVICE mobilerotic");
      this.AddHelpMessageLine("--- SELECT TRANSACTIONS");
      this.AddHelpMessageLine("----- ( select could be: TRANSACTIONS, CLICKS, HELP )");

    }


    protected override SkypeCommandResultBase CheckAndPrepare(SkypeMessage message)
    {
      SkypeCommandResultBase result = new SkypeCommandResultBase(this, message);
      string command = string.Empty,
        group = string.Empty,
        attribute = string.Empty;

      if (string.IsNullOrEmpty(message.Message))
        return result;

      string[] data = message.Message.Split(' ');
      if (data.Length == 1)
        return result;

      if (!data[0].ToLower().Equals("select"))
        return result;

      #region # parse parameter #
      switch(data[1].ToLower())
      {
        case "all": command = "all"; break;
        case "transaction":
        case "transactions": command = "transaction"; break;
        case "help": command = "help"; break;
        case "click":
        case "clicks": command = "click"; break;
        default: command = "all"; break;
      }
      #endregion
      #region # parse for atributes #
      
      if (data.Length == 5 && data[2].ToLower().Equals("for"))
      {
        if (data[3].ToLower().Equals("service"))
          group = "service";
        else if (data[3].ToLower().Equals("country"))
          group = "country";

        // add search text
        if (!string.IsNullOrEmpty(group))
          attribute = data[4].ToLower();
      }

      #endregion

      result.Parameters.Add(command);
      result.Parameters.Add(group);
      result.Attributes.Add(attribute);
      result.Success = true;
      return result;
    }

    protected override void Execute(SkypeCommandResultBase result)
    {
      string command = result.Parameters.ElementAt(0); // help, all, transaction, clicks
      string group = result.Parameters.ElementAt(1);
      string attribute = result.Attributes.ElementAt(0);

      if(command.Equals("help"))
      {
        Program.SkypeProcess.Send(this.Conversion, this.HelpMessage);
        return;
      }

      SelectCommandTaskObject taskObject = new SelectCommandTaskObject();
      taskObject.Data = new List<object>();
      (taskObject.Data as List<object>).Add(result);
      (taskObject.Data as List<object>).Add(command);
      (taskObject.Data as List<object>).Add(group);
      (taskObject.Data as List<object>).Add(attribute);


      DefaultTask task = new DefaultTask(taskObject);
      DatabaseTaskManager.Current.AddTask(task);
      
      return;
    }
  }

  public class SelectCommandTaskObject : TaskObject
  {

    public override void Execute()
    {
      SkypeCommandResultBase commandResult = (this.Data as List<object>).ElementAt(0) as SkypeCommandResultBase;
      string command = (this.Data as List<object>).ElementAt(1).ToString();
      string group = (this.Data as List<object>).ElementAt(2).ToString();
      string attribute = (this.Data as List<object>).ElementAt(3).ToString();

      LoadAllMobilePaywallManager manager = new LoadAllMobilePaywallManager();
      LoadMobilePaywallResult result = manager.Load(command, group, attribute);

      string output = string.Format("Response to *{0}*", commandResult.Message.Sender) + Environment.NewLine;
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

      Program.SkypeProcess.Send(commandResult.RespondeTo, output);
    }

  }


}
