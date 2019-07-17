using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Base
{
  public abstract class ManagerBase
  {
    private Managers _parent = null;
    private ManagerThreadType _threadType = ManagerThreadType.All;
    private string _managerKey = string.Empty;
    private bool _nonExecutable = false;
    private bool _initiated = false;

    public string Key { get { return this._managerKey; } }
    public ManagerThreadType ThreadType { get { return this._threadType; } }
    protected DateTime Time { get { return this._parent.Time; } }
    protected bool NonExecutable { get { return this._nonExecutable; } set { this._nonExecutable = value; } }

    public ManagerBase(string key)
    {
      this._managerKey = key;
    }

    //public ManagerBase(Managers parent, ManagerThreadType threadType, string key)
    //{
    //  this._parent = parent;
    //  this._threadType = threadType;
    //  this._managerKey = key;
    //}

    public void SetParent(Managers parent) { this._parent = parent; }
    public void SetThreadType(ManagerThreadType type) { this._threadType = type; }
    
    // SUMMARY: Call manager 
    public void Call()
    {
      if (this._parent == null)
        throw new ArgumentException("Parent could not be null. Managers should be added throu Managers class");

      if(!this._initiated)
      {
        this.Init();
        this._initiated = true;
      }

      if (this._nonExecutable)
        return;

      this.Execute();
    }

    public abstract void Init();
    public abstract void Execute();
  }
}
