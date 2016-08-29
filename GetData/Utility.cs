using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace GetData
{
    static class Utility
    {
        /// <summary>
        /// Get CSV from target URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<string[]> GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream(),Encoding.GetEncoding(950), true);
            string line;
            List<string[]> listoflines = new List<string[]>();
            while((line = sr.ReadLine()) != null)
            {
                string[] lines = line.Split(',');
                listoflines.Add(lines);
            }
            sr.Close();
            return listoflines;
        }
        
    }

}
