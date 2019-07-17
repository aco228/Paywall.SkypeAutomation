using Paywall.SkypeAutomation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Task
{
  public partial class TaskManager : ManagerBase
  {
    private List<TaskBase> _tasks = null;

    public TaskManager(string key)
      : base(key)
    {
      this._tasks = new List<TaskBase>();
    }

    public TaskBase this[string key]
    {
      get
      {
        return (from t in this._tasks where t.Key.Equals(key) select t).FirstOrDefault();
      }
    }

    public int Count
    {
      get
      {
        return this._tasks.Count;
      }
    }

    public static TaskManager Current
    {
      get
      {
        return Program.Managers[ManagersType.TaskManager] as TaskManager;
      }
    }

    public void AddTask(TaskBase task)
    {
      this._tasks.Add(task);
    }

    public override void Execute()
    {
      // first delete unecesseary tasks
      for (int i = 0; i < this._tasks.Count; i++ )
        if(this._tasks.ElementAt(i).DeleteMe)
        {
          this._tasks.RemoveAt(i);
          i--;
        }

      foreach (TaskBase task in this._tasks)
        task.Call(this.Time);
    }
  }
}
