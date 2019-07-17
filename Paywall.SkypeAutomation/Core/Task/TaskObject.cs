using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Task
{
  public abstract class TaskObject
  {
    public object Data = null;
    public abstract void Execute();
  }
}
