using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Core.SkypeCommand;
using Paywall.SkypeAutomation.Core.Task;
using Paywall.SkypeAutomation.Implementations.SkypeApi;
using Paywall.SkypeAutomation.Implementations.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.SkypeCommand
{
  public class TestCommand : SkypeCommandBase
  {
    public TestCommand(SkypeConversationBase conversion)
      : base("testCommand", "test", conversion)
    { }

    protected override SkypeCommandResultBase CheckAndPrepare(SkypeMessage message)
    {
      SkypeCommandResultBase result = new SkypeCommandResultBase(this, message);
      if (string.IsNullOrEmpty(message.Message))
        return result;

      string[] data = message.Message.Split(' ');
      if (data.Length <= 2)
        return result;

      if (!data[0].ToLower().Equals(this.CommandName))
        return result;

      result.Attributes.Add(data[1]);
      result.Parameters.Add(data[2]);
      result.Success = true;
      return result;
    }

    protected override void Execute(SkypeCommandResultBase result)
    {
      TestCommandTaskObject taskObject = new TestCommandTaskObject();
      taskObject.Data = result;
      DefaultTask task = new DefaultTask(taskObject);
      DatabaseTaskManager.Current.AddTask(task);
    }
  }
  
  public class TestCommandTaskObject : TaskObject
  {
    public override void Execute()
    {
      SkypeCommandResultBase result = this.Data as SkypeCommandResultBase;
      Program.SkypeProcess.Send(result.RespondeTo, string.Format("Test command: {0} and {1}", result.Attributes.ElementAt(0), result.Parameters.ElementAt(0)));
    }
  }
}
