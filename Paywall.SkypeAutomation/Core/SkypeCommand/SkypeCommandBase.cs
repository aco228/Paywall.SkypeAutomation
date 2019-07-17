using Paywall.SkypeAutomation.Core.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeCommand
{
  public abstract class SkypeCommandBase
  {
    private string _key = string.Empty;
    private string _commandName = string.Empty;
    private List<string> _helpMessage = null;
    private SkypeConversationBase _conversion = null;

    public string Key { get { return this._key; } }
    protected string CommandName { get { return this._commandName; } }
    public SkypeConversationBase Conversion { get { return this._conversion; } }

    public SkypeCommandBase(string key, string commandName, SkypeConversationBase conversion)
    {
      this._key = key;
      this._commandName = commandName.ToLower();
      this._conversion = conversion;
      this._helpMessage = new List<string>();
    }

    protected abstract SkypeCommandResultBase CheckAndPrepare(SkypeMessage message);
    protected abstract void Execute(SkypeCommandResultBase result);

    // SUMMARY: Add line to help message for command
    protected void AddHelpMessageLine(string message)
    {
      this._helpMessage.Add(message);
    }

    // SUMMARY: Print helpmessage
    protected string HelpMessage
    {
      get
      {
        string message = string.Empty;
        foreach (string m in this._helpMessage)
          message += m + Environment.NewLine;
        return message;
      }
    }

    public virtual void Call(SkypeMessage message)
    {
      SkypeCommandResultBase result = this.CheckAndPrepare(message);
      if (result.Success)
        this.Execute(result);      
    }

  }
}
