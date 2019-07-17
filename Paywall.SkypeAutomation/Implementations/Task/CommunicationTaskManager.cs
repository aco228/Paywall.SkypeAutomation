using Paywall.SkypeAutomation.Core.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Task
{
  public class CommunicationTaskManager : TaskManager
  {
    public static string Key = "communicationTaskManager";

    public CommunicationTaskManager()
      : base(CommunicationTaskManager.Key)
    {}

    public CommunicationTaskManager Current
    {
      get
      {
        return Program.Managers[CommunicationTaskManager.Key] as CommunicationTaskManager;
      }
    }

  }
}
