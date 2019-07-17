using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class MobileOperatorEntry
  {
    private int _id = -1;
    private string _name = string.Empty;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string Name { get { return this._name; } set { this._name = value; } }
  }
}
