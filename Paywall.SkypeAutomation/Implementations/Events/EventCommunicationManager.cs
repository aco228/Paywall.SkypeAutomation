using Paywall.SkypeAutomation.Core.Events;
using Paywall.SkypeAutomation.Implementations.Events.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Events
{
  public class EventCommunicationManager : EventManager
  {

    public static string Key = "eventCommunicationManager";

    public EventCommunicationManager()
      : base(EventCommunicationManager.Key)
    { }

    public override void Init()
    {
      this._events.Add(new ReportConversionsEvent());
      this._events.Add(new FatalConversionsEvent());
      this._events.Add(new PaywallSkypeNotificationEvent());
      //this._events.Add(new TestEvent());
    }

    public static EventCommunicationManager Current
    {
      get
      {
        return Program.Managers[EventCommunicationManager.Key] as EventCommunicationManager;
      }
    }
  }
}
