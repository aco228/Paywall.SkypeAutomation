using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Events
{
  public abstract class EventBase
  {
    private string _eventKey = string.Empty;
    private EventTrigger _trigger = null;
    private DateTime _created;

    public string EventKey { get { return this._eventKey; } }
    protected EventTrigger Trigger { get { return this._trigger; } }
    protected DateTime Time { get { return this._trigger.LastExecutedTime; } }
    protected int ExecutedTimes { get { return this._trigger.ExecutedTimes; } }

    public EventBase(string key, EventTrigger trigger)
    {
      this._eventKey = key;
      this._trigger = trigger;
    }

    // SUMMARY: Set Initial called time from children
    protected void SetInitialCalledTime(DateTime time)
    {
      this._trigger.SetLastExecutionTime(time);
    }

    // SUMMARY: Change trigger
    protected void SetTrigger(EventTrigger trigger)
    {
      this._trigger = trigger;
    }

    public string NextTrigger()
    {
      return string.Format("{0} {1}", this._trigger.Time, this._trigger.Type.ToString() );
    }

    // SUMMARY: Chech if event could be executed and execute it
    public void Call(DateTime time)
    {
      if (!this._trigger.ShouldExecute(time))
        return;

      this._trigger.NewExecution();
      this.Execute();
    }

    protected abstract void Execute();

  }

}
