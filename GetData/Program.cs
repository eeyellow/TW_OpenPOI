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
            
            Database db = new Database(ConfigurationManager.ConnectionStrings["DB_TWPOI"].ToString());
            db.setCommand("SELECT id FROM medical_type WHERE name='醫院'");
            int type = (int)db.ExecuteScalar();
            for(int i = 1; i < listOfHospital.Count; i++)
            {
                SortedList<string, object> param = new SortedList<string, object>();
                param.Add("@name", listOfHospital[i][0]);
                param.Add("@phone", listOfHospital[i][1]);
                param.Add("@address", listOfHospital[i][2]);
                param.Add("@type", type);
                db.setCommand("INSERT INTO medical(name,phone,address,type) VALUES(@name,@phone,@address,@type)", param);
                db.ExecuteNonQuery();
            }
            
        }
    }
}
