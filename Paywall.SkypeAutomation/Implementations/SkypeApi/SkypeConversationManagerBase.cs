using Paywall.SkypeAutomation.Base;
using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Implementations.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paywall.SkypeAutomation.Core.SkypeApi
{
  public partial class SkypeConversionManager : ManagerBase
  {

    public override void Init()
    {
      this._conversations.Add(new AleksandarKonatarConversation());
      this._conversations.Add(new BotReportConversation());
      this._conversations.Add(new BotFatalConversation());
      this._conversations.Add(new SkypeAutomationConversation());
    }

  }
}
