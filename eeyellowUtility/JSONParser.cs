using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eeyellowUtility
{
    public static class JSONParser
    {
        public static GAddrLoc GoogleAddressLocator(string jsonstr)
        {
            return JsonConvert.DeserializeObject<GAddrLoc>(jsonstr);
        }
    }
}
