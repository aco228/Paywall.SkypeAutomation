using Paywall.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Automatization
{
  public abstract class AutomatizationManagerBase
  {
    private MobilePaywallDatabase _mobilePaywallDatabase = null;
    private KiwiclicksDatabase _kiwiclicksDatabase = null;
    private List<AutomatizationToSkypeMessage> _message = null;
    private List<AutomationGroup> _groups;
    private TransactionManager _transactionManager = null;
    
    private DateTime _lastTimeCalled;
    private bool _running = false;

    public MobilePaywallDatabase MobilePaywallDatabase { get { return this._mobilePaywallDatabase; } }
    public KiwiclicksDatabase KiwiclickDataabse { get { return this._kiwiclicksDatabase; } }
    public List<AutomatizationToSkypeMessage> Messages { get { return this._message; } set { this._message = value; } }
    public DateTime LastTimeCalled { get { return this._lastTimeCalled; } }
    public TransactionManager TransactionManager { get { return this._transactionManager; } set { this._transactionManager = value; } }
    public bool Running { get { return this._running; } set { this._running = value; } }

    public List<AutomationGroup> Groups { get { return this._groups; } protected set { this._groups = value; } }

    public AutomationGroup this[int groupID]
    {
      get
      {
        return (from ag in this._groups where ag.ID == groupID select ag).FirstOrDefault();
      }
    }

    public AutomatizationManagerBase()
    {
      this._message = new List<AutomatizationToSkypeMessage>();
      this._mobilePaywallDatabase = new MobilePaywallDatabase();
      this._kiwiclicksDatabase = new KiwiclicksDatabase();
      this._transactionManager = new TransactionManager(this);

      this._running = true;
      this.Restart();
    }


    public void Call(DateTime time)
    {
      if (!this._running)
        return;

      this._lastTimeCalled = time;
      this.Execute();
    }

    public abstract void Restart();
    public abstract void Print(int groupID);
    protected abstract void Execute();

  }
}
