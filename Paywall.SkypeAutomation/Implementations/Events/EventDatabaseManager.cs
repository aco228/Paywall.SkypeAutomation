using Paywall.SkypeAutomation.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Events
{
  public class EventDatabaseManager : EventManager
  {
    public static string Key = "eventDatabaseManager";

    public EventDatabaseManager() : 
      base("eventDatabaseManager") 
    { }

    public override void Init()
    {

    }

    public static EventDatabaseManager Current
    {
      get
      {
        return Program.Managers[EventDatabaseManager.Key] as EventDatabaseManager;  
      }
    }
  }
}
