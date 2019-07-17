using Paywall.SkypeAutomation.Core.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Task
{
  public class AutomatizationTaskManager : TaskManager
  {
    public static string Key = "automizationTaskManager";

    public AutomatizationTaskManager()
      : base(AutomatizationTaskManager.Key)
    { 
    
    }

    public AutomatizationTaskManager Current
    {
      get
      {
        return Program.Managers[AutomatizationTaskManager.Key] as AutomatizationTaskManager;
      }
    }

  }
}
