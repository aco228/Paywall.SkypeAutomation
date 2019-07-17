using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database
{
  public abstract class EntryBase
  {
    protected int _id = -1;

    public int ID { get { return this._id; } }


  }
}
