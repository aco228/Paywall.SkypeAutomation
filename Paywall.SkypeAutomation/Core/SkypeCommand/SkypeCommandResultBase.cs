using Paywall.SkypeAutomation.Core.SkypeApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeCommand
{
  public class SkypeCommandResultBase
  {

    private bool _success = false;
    private SkypeCommandBase _command = null;
    private SkypeMessage _message = null;
    private List<string> _attributes = null;
    private List<string> _parameters = null;
    private SkypeConversationBase _responseTo = null;
    private object _data = null;
    private string _comment = string.Empty;

    public bool Success { get { return this._success; } set { this._success = value; } }
    public List<string> Attributes { get { return this._attributes; } set { this._attributes = value; } }
    public List<string> Parameters { get { return this._parameters; } set { this._parameters = value; } }
    public SkypeConversationBase RespondeTo { get { return this._command.Conversion; } }
    public SkypeMessage Message { get { return this._message; } }
    public object Data { get { return this._data; } set { this._data = value; } }

    public SkypeCommandResultBase(SkypeCommandBase command, SkypeMessage message)
    {
      this._command = command;
      this._message = message;
      this._attributes = new List<string>();
      this._parameters = new List<string>();
    }

  }
}
