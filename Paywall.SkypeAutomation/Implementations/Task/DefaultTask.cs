using Paywall.SkypeAutomation.Core.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Task
{
  
  public class DefaultTask : TaskBase
  {

    public DefaultTask(TaskObject taskObject)
      :base("defaultTask", taskObject, new TaskTrigger(0, Core.TriggerTimeType.Miliseconds, 1, true))
    { }

    protected override void Execute()
    {
      if (this.TaskObject == null)
        return;

      this.TaskObject.Execute();
    }

  }
}
