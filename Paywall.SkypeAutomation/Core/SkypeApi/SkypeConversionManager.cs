using Paywall.SkypeAutomation.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeApi
{
  public partial class SkypeConversionManager : ManagerBase
  {
    private SkypeAPI _skypeAPI = null;
    private List<SkypeConversationBase> _conversations = null;

    public SkypeAPI SkypeAPI { get { return this._skypeAPI; } }

    // SUMMARY: Construct class and add conversations
    public SkypeConversionManager()
      : base(ManagersType.SkypeConversaionManager.ToString())
    {
      this._skypeAPI = new SkypeAPI();
      this._conversations = new List<SkypeConversationBase>();
    }

    // SUMMARY: Return conversation based on key
    public SkypeConversationBase this[string key]
    {
      get
      {
        if (this._conversations == null) return null;
        return (from sc in this._conversations where sc.Key.Equals(key) select sc).FirstOrDefault();
      }
    }

    // SUMMARY: Return number of conversations
    public int Count
    {
      get
      {
        return this._conversations == null ? 0 : this._conversations.Count;
      }
    }

    public static SkypeConversionManager Current
    {
      get
      {
        return Program.Managers[ManagersType.SkypeConversaionManager] as SkypeConversionManager;
      }
    }

    public override void Execute()
    {
      foreach (SkypeConversationBase c in this._conversations)
        c.Call(this.Time);
    }

  }
}
