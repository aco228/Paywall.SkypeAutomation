using Paywall.SkypeAutomation.Core.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeCommand
{
  public abstract class SkypeCommandSetBase
  {
    private string _key = string.Empty;
    protected List<SkypeCommandBase> _commands = null;

    public SkypeCommandSetBase(string key)
    {
      this._key = key;
      this._commands = new List<SkypeCommandBase>();
    }

    public void AddCommand(SkypeCommandBase command)
    {
      this._commands.Add(command);
    }

    // SUMMARY: Return command from set if exists
    public SkypeCommandBase GetCommand(string key)
    {
      return (from c in this._commands where c.Key.Equals(key) select c).FirstOrDefault();
    }

    // SUMMARY: Execute commands from messages
    public virtual void ExecuteCommands(List<SkypeMessage> messages)
    {
      foreach (SkypeMessage message in messages)
        foreach (SkypeCommandBase command in this._commands)
          command.Call(message);
    }

  }
}
