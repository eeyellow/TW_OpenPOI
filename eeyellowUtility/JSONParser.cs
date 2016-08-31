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
        public static CGoogleAddressLocator.RootObject GoogleAddressLocator(string jsonstr)
        {
            return JsonConvert.DeserializeObject<CGoogleAddressLocator.RootObject>(jsonstr);
        }

        public static CGoogleQueryText.RootObject GoogleQueryText(string jsonstr)
        {
            return JsonConvert.DeserializeObject<CGoogleQueryText.RootObject>(jsonstr);
        }
    }
}
