using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Task
{
  public class TaskTrigger : TriggerBase
  {
    public TaskTrigger(long time, TriggerTimeType type, int executeLimit = -1, bool executeOnce = false)
      :base(time, type, executeLimit, executeOnce)
    { }

  }
}
