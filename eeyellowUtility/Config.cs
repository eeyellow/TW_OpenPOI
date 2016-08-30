namespace eeyellowUtility
{
    public static class Config
    {
        /// <summary>
        /// 資料庫伺服器位置
        /// </summary>
        /// <returns></returns>
        private static string Get_DBServer(DBServer s)
        {
            string result = "";
            switch ((int)s)
            {
                case 0:
                    result = "Server=localhost;";
                    break;
                case 1:
                    result = "Server=localhost;";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 資料庫類型
        /// </summary>
        /// <returns></returns>
        private static string Get_DBType(DBType t) {
            string result = "";
            switch ((int)t)
            {
                case 0:
                    result = "Port=5432;";
                    break;
                case 1:
                    result = "Port=1433;";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 資料庫名稱
        /// </summary>
        /// <returns></returns>
        private static string Get_DBName(DBName n)
        {
            string result = "";
            switch ((int)n)
            {
                case 0:
                    result = "User Id=twuser;Password=twpassword;Database=tw_poi;";
                    break;
                case 1:
                    result = "User Id=twuser;Password=twpassword;Database=tw_poi;";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 取得資料庫連線字串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string DBConnStr(DBServer s, DBType t, DBName n)
        {
            string result = "";
            result += Get_DBServer(s) + Get_DBType(t) + Get_DBName(n);
            return result;
        }
        /// <summary>
        /// 取得OpenData URL網址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DataURL(URL url)
        {
            string result = "";
            switch ((int)url)
            {
                case 0:
                    result = "http://www.mohw.gov.tw/MOHW_Upload/doc/opendata/醫院基本資料.csv";
                    break;
            }
            return result;
        }
                
        public enum DBServer
        {
            localhost = 0,
            AWS = 1
        }
        public enum DBType
        {
            Postgres = 0,
            MSSQL = 1
        }
        public enum DBName
        {
            TWPOI_Admin = 0,
            TWPOI_Guest = 1
        }
        public enum URL
        {
            Hospital = 0
        }
    }
}
