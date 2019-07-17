using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Task
{
  public abstract class TaskBase
  {
    private string _key = string.Empty;
    private TaskTrigger _trigger = null;
    private DateTime _created;
    private TaskObject _taskObject = null;
    private Guid _identifier = Guid.Empty;
    private bool _deleteMe = false;

    public string Key { get { return this._key; } }
    protected TaskObject TaskObject { get { return this._taskObject; } }
    protected TaskTrigger Trigger { get { return this._trigger; } set { this._trigger = value; } }
    protected int ExecutedTimes { get { return this._trigger.ExecutedTimes; } }
    public Guid Identifier { get { return this._identifier; } }
    public bool DeleteMe { get { return this._deleteMe; } }

    public TaskBase(string key, TaskObject taskObject, TaskTrigger trigger)
    {
      this._key = key;
      this._taskObject = taskObject;
      this._trigger = trigger;
      this._identifier = Guid.NewGuid();
      this._created = DateTime.Now;
    }

    protected void SetInitialCalledTime(DateTime time)
    {
      this._trigger.SetLastExecutionTime(time);
    }

    protected void SetTrigger(TaskTrigger trigger)
    {
      this._trigger = trigger;
    }

    public void Call(DateTime time)
    {
      if (!this._trigger.ShouldExecute(time))
      {
        this._deleteMe = true;
        return;
      }

      this._trigger.NewExecution();
      this.Execute();
    }

    protected abstract void Execute();

  }

}
