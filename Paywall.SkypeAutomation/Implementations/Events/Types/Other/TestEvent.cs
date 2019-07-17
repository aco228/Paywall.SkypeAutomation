using Paywall.SkypeAutomation.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Implementations.Events.Types
{
  public class TestEvent : EventBase
  {
    public TestEvent()
      :base("testEvent", new EventTrigger(20, Core.TriggerTimeType.Seconds, false, false))
    {}

    protected override void Execute()
    {
      Console.WriteLine("opa");
    }
  }
}
