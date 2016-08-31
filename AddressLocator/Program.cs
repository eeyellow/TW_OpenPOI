using System.Collections.Generic;
using System.Text;
using System.Net;
using eeyellowUtility;
using System.IO;

namespace AddressLocator
{
    class Program
    {
        /// <summary>
        /// 從資料庫中取出未定位的資料
        /// 先用地址進行Google Map Geocoding API地址定位        
        /// 如果定位失敗，再改用Google Map Place API，對名稱進行TextSearch
        /// 定位成功後把LatLng寫回資料庫
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Database db = new Database(Config.DBConnStr(Config.DBServer.localhost, Config.DBType.Postgres, Config.DBName.TWPOI_Admin));
            db.setCommand("SELECT * FROM medical WHERE lat is null");
            var dt = db.ExecuteDataTable();

            HttpWebRequest webRequest = null;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                HttpWebResponse response = null;
                try
                {                    
                    webRequest = WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?address=" + dr["address"]) as HttpWebRequest;
                    webRequest.Method = WebRequestMethods.Http.Get;
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    response = webRequest.GetResponse() as HttpWebResponse;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true);
                        string jsonstr = sr.ReadToEnd();
                        var jsonAddr = JSONParser.GoogleAddressLocator(jsonstr);
                        if (jsonAddr.status == "OK")
                        {
                            SortedList<string, object> param = new SortedList<string, object>();
                            param.Add("@lat", jsonAddr.results[0].geometry.location.lat);
                            param.Add("@lng", jsonAddr.results[0].geometry.location.lng);
                            param.Add("@id", (int)dr["id"]);
                            db.setCommand("UPDATE medical SET lat=@lat, lng=@lng WHERE id=@id", param);
                            db.ExecuteNonQuery();
                        }
                        else
                        {
                            webRequest.Abort();
                            if (response != null) response.Dispose();

                            webRequest = WebRequest.Create("https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + dr["name"] + "&key=" + Config.GoogleMapAPIKey) as HttpWebRequest;
                            response = webRequest.GetResponse() as HttpWebResponse;
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8, true);
                                jsonstr = sr.ReadToEnd();
                                var jsonText = JSONParser.GoogleQueryText(jsonstr);
                                if(jsonText.status == "OK")
                                {
                                    SortedList<string, object> param = new SortedList<string, object>();
                                    param.Add("@lat", jsonText.results[0].geometry.location.lat);
                                    param.Add("@lng", jsonText.results[0].geometry.location.lng);
                                    param.Add("@id", (int)dr["id"]);
                                    db.setCommand("UPDATE medical SET lat=@lat, lng=@lng WHERE id=@id", param);
                                    db.ExecuteNonQuery();
                                }                                
                            }
                        }
                        sr.Close();
                        System.Threading.Thread.Sleep(2000);
                    }
                    
                }
                catch (WebException ex) { }
                finally
                {
                    webRequest.Abort();
                    if (response != null) response.Dispose();
                }
            }
        }
    }
}
