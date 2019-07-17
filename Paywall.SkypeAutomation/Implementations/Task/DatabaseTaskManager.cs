using Paywall.SkypeAutomation.Core.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Task
{
  public class DatabaseTaskManager : TaskManager
  {
    public static string Key = "databaseTaskManager";

    public DatabaseTaskManager()
      : base(DatabaseTaskManager.Key)
    { }

    public static DatabaseTaskManager Current
    {
      get
      {
        return Program.Managers[DatabaseTaskManager.Key] as DatabaseTaskManager;
      }
    }

  }
}
