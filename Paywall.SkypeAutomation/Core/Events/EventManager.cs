using Paywall.SkypeAutomation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Events
{
  public partial class EventManager : ManagerBase
  {
    protected List<EventBase> _events = null;

    public EventManager(string key)
      : base(key)
    {
      this._events = new List<EventBase>();
    }

    // SUMMARY: Get specific event
    public EventBase this[string key]
    {
      get
      {
        if (this._events == null) return null;
        return (from e in this._events where e.EventKey.Equals(key) select e).FirstOrDefault();
      }
    }

    // SUMMARY: Get number of events
    public int Count
    {
      get
      {
        return this._events == null ? 0 : this._events.Count;
      }
    }

    public static EventManager Current
    {
      get
      {
        return Program.Managers[ManagersType.EventManager] as EventManager; 
      }
    }

    public override void Execute()
    {
      foreach (EventBase e in this._events)
        e.Call(this.Time);
    }

  }
}
