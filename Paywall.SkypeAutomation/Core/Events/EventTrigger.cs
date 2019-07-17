using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.Events
{
  public class EventTrigger : TriggerBase
  {

    public EventTrigger(long time, TriggerTimeType type, bool executeOnce = false, bool executeImmediately = true)
      : base(time, type, -1, executeOnce, executeImmediately)
    { }

  }
}
