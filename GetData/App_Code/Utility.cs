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
                List<int> indexes = line.AllIndexesOf("\"");
                if(indexes.Count != 0)
                {                    
                    line = line.Substring(0, indexes[0]) + line.Substring(indexes[0], indexes[1] - indexes[0] + 1).Replace("\"", "").Replace(",", "，");
                }
                line = ConvertToHalfwidthNumber(line);
                string[] lines = line.Split(',');
                listoflines.Add(lines);
            }
            sr.Close();
            return listoflines;
        }

        /// <summary>
        /// Find ALL character position in string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        /// <summary>
        /// 數字轉半形
        /// </summary>
        public static string ConvertToHalfwidthNumber(string input)
        {
            string text = "";
            foreach (char element in input.ToCharArray())
            {                
                if ((int)element >= 65296 & (int)element <= 65305)
                {
                    text += ((char)((int)element - 65248)).ToString();
                }
                else
                {
                    text += element.ToString();
                }
            }
            return text;
        }
    }

}
