using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using eeyellowUtility;
using System.IO;

namespace AddressLocator
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database(Config.DBConnStr(Config.DBServer.localhost, Config.DBType.Postgres, Config.DBName.TWPOI_Admin));
            db.setCommand("SELECT * FROM medical limit 10");
            var dt = db.ExecuteDataTable();
            foreach(System.Data.DataRow dr in dt.Rows)
            {
                HttpWebResponse response = null;
                try
                {
                    HttpWebRequest webRequest = HttpWebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?address=" + dr["address"]) as HttpWebRequest;
                    webRequest.Method = WebRequestMethods.Http.Get;
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    response = webRequest.GetResponse() as HttpWebResponse;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true);
                        string jsonstr = sr.ReadToEnd();
                        var json = JSONParser.GoogleAddressLocator(jsonstr);
                        if (json.status == "OK")
                        {
                            SortedList<string, object> param = new SortedList<string, object>();
                            param.Add("@lat", json.results[0].geometry.location.lat);
                            param.Add("@lng", json.results[0].geometry.location.lng);
                            param.Add("@id", (int)dr["id"]);
                            db.setCommand("UPDATE medical SET lat=@lat, lng=@lng WHERE id=@id", param);
                            db.ExecuteNonQuery();
                        }
                        sr.Close();
                        System.Threading.Thread.Sleep(2000);
                    }
                }
                catch (WebException ex) { }
                finally
                {
                    if (response != null) response.Dispose();
                }
            }
        }
    }
}
