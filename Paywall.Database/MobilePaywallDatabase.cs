using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.Database
{
  public class MobilePaywallDatabase : DatabaseBase
  {

    public MobilePaywallDatabase()
      : base("MobilePaywall", "core")
    {
      this.SetConnectionString("Data Source=192.168.11.104;Initial Catalog=MobilePaywall;uid=saMobilePaywall;pwd=6qMzCA?2jys4;");
    }


  }
}
