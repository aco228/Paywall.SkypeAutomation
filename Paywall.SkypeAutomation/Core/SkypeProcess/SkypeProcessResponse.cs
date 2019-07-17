using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeProcess
{
  public class SkypeProcessResponse
  {
    private bool _success = false;
    private string _message = string.Empty;
    private string _exception = string.Empty;

    public bool Success { get { return this._success; } }
    public string Message { get { return this._message; } }
    public string Exception { get { return this._exception; } }

    public SkypeProcessResponse(bool success, string message, string exception = "")
    {
      this._success = success;
      this._message = message;
      this._exception = exception;
    }
  }
}
