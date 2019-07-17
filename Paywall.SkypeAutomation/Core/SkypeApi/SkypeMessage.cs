using SKYPE4COMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeApi
{
  public class SkypeMessage
  {
    private ChatMessage _chatMessage = null;
    private string _message = string.Empty;
    private string _sender = string.Empty;
    private DateTime _date;

    public ChatMessage ChatMessage { get { return this._chatMessage; } set { this._chatMessage = value; } }
    public string Message { get { return this._message; } set { this._message = value; } }
    public string Sender { get { return this._sender; } set { this._sender = value; } }
    public DateTime Date { get { return this._date; } set { this._date = value; } }

  }
}
