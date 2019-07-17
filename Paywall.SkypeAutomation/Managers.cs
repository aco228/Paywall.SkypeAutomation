using Paywall.SkypeAutomation.Base;
using Paywall.SkypeAutomation.Core.Events;
using Paywall.SkypeAutomation.Core.SkypeApi;
using Paywall.SkypeAutomation.Core.Task;
using Paywall.SkypeAutomation.Implementations.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation
{
  public class Managers
  {
    private DateTime _lastCalled;
    private List<ManagerBase> _managers = null;
    private int _threadReady = 0;

    public List<ManagerBase> List { get { return this._managers; } }
    public ManagerBase this[string key]
    {
      get
      {
        return this._managers == null ? null : (from m in this._managers where m.Key.Equals(key) select m).FirstOrDefault();
      }
    }
    public ManagerBase this[ManagersType type]
    {
      get
      {
        return this._managers == null ? null : (from m in this._managers where m.Key.Equals(type.ToString()) select m).FirstOrDefault();
      }
    }

    public DateTime Time { get { return this._lastCalled; } }

    public Managers()
    {
      this._managers = new List<ManagerBase>();
    }

    public void Add(ManagerBase manager, ManagerThreadType type)
    {
      manager.SetParent(this);
      manager.SetThreadType(type);
      this._managers.Add(manager);
    }

    public void Call(DateTime time, ManagerThreadType threadType)
    {
      if (this._threadReady != Program.NumberOfThread)
        return;

      this._lastCalled = time;
      foreach (ManagerBase manager in this._managers)
        if(manager.ThreadType == ManagerThreadType.All || manager.ThreadType == threadType)
          manager.Call();
    }

    public void ThreadReady()
    {
      this._threadReady++;
    }

  }

  public enum ManagersType
  {
    EventManager,
    SkypeConversaionManager,
    TaskManager
  }

  public enum ManagerThreadType
  {
    All,
    CommunicationThread,
    DatabaseTask,
    AutomatizationThread
  }

}
