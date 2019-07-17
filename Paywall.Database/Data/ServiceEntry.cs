using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class ServiceEntry
  {
    private int _id = -1;
    private string _name = string.Empty;
    private string _description = string.Empty;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string Name { get { return this._name; } set { this._name = value; } }
    public string Description { get { return this._description; } set { this._description = value; } }
  }
}
