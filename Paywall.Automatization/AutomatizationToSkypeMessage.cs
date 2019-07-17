using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Automatization
{
  public class AutomatizationToSkypeMessage
  {
    private string _message = string.Empty;
    private bool _read = false;
    private DateTime _created;

    public string Message
    {
      get
      {
        this._read = true;
        string hour = this._created.Hour < 10 ? "0" + this._created.Hour : this._created.Hour.ToString();
        string minute = this._created.Minute < 10 ? "0" + this._created.Minute : this._created.Minute.ToString();

        return string.Format("{0}:{1} {2}{3}", hour, minute, Environment.NewLine, this._message);
      }
    }
    public bool Read { get { return this._read; } }

    public AutomatizationToSkypeMessage(string messsage)
    {
      this._message = messsage;
      this._created = DateTime.Now;
    }
  }
}
