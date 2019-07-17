using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database
{
  public class KiwiclicksDatabase : MysqlDatabaseBase
  {
    public KiwiclicksDatabase()
      : base("mtkiwiclick")
    {
      this.SetConnectionString("Server=5.199.175.193; database=mtkiwiclick; UID=olkiwiclick; password=bdejr247");
    }

  }
}
