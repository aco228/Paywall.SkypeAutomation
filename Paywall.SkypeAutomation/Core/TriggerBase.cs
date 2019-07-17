using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core
{
  public abstract class TriggerBase
  {
    protected bool _executeOnce = false;
    protected int _executeLimit = -1;
    protected int _executedTimes = 0;
    protected long _time = 0;
    protected TriggerTimeType _type = TriggerTimeType.Miliseconds;
    protected DateTime _lastExecutedTime;

    public long Time { get { return this._time; } }
    public TriggerTimeType Type { get { return this._type; } }
    public bool ExecuteOnce { get { return this._executeOnce; } }
    public int ExecuteLimit { get { return this._executeLimit; } }
    public int ExecutedTimes { get { return this._executedTimes; } }
    public DateTime LastExecutedTime { get { return this._lastExecutedTime; } }

    public TriggerBase(long time, TriggerTimeType type, int executeLimit, bool executeOnce = false, bool executeImmediately = true)
    {
      this._time = time;
      this._type = type;
      this._executeLimit = executeLimit;
      this._executeOnce = executeOnce;

      if(executeImmediately)
        this._lastExecutedTime = DateTime.Now.AddMilliseconds(-10 - this.Miliseconds);
      else
        this._lastExecutedTime = DateTime.Now;
    }

    public long Miliseconds
    {
      get
      {
        if (this._type == TriggerTimeType.Miliseconds)
          return this._time;
        else if (this._type == TriggerTimeType.Seconds)
          return this._time * 1000;
        else if (this._type == TriggerTimeType.Minutes)
          return this._time * 1000 * 60;
        else if (this._type == TriggerTimeType.Hours)
          return this._time * 1000 * 60 * 60;
        else if (this._type == TriggerTimeType.Days)
          return this._time * 1000 * 60 * 60 * 24;
        else
          return this._time;
      }
    }

    public void SetLastExecutionTime(DateTime time)
    {
      this._lastExecutedTime = time;
    }

    public void NewExecution()
    {
      this._executedTimes += 1;
      this._lastExecutedTime = DateTime.Now;
      this.OnExecution();
    }

    public bool ShouldExecute(DateTime time)
    {

      if (this._executedTimes > 0 && this._executeOnce)
        return false;
      if (this._executeLimit > 0 && this._executedTimes >= this._executeLimit)
        return false;
      if (this._lastExecutedTime.AddMilliseconds(this.Miliseconds) > time)
        return false;

      return true;
    }

    protected virtual void OnExecution()
    { }



  }

  public enum TriggerTimeType
  {
    Miliseconds,
    Seconds,
    Minutes,
    Hours,
    Days
  }
}
