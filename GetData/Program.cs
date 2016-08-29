using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace GetData
{
    class Program
    {
        static void Main(string[] args)
        {
            var listOfHospital = Utility.GetCSV(ConfigurationManager.AppSettings["URL_Hospital"]);

        }
    }
}
