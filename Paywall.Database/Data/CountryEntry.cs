using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database.Data
{
  public class CountryEntry
  {
    private int _id = -1;
    private string _globalName = string.Empty;
    private string _twoLetterIsoCode = string.Empty;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string GlobalName { get { return this._globalName; } set { this._globalName = value; } }
    public string TwoLetterIsoCode { get { return this._twoLetterIsoCode; } set { this._twoLetterIsoCode = value; } }


  }
}
