using System.Collections.Generic;
using Npgsql;

namespace eeyellowUtility
{
    public class Database
    {
        private NpgsqlConnection conn = new NpgsqlConnection();
        private NpgsqlCommand comm = new NpgsqlCommand();

        public Database(string connStr)
        {
            conn.ConnectionString = connStr;
            comm.Connection = conn;
            conn.Open();
        }

        public void setCommand(string commText)
        {
            comm.CommandText = commText;
        }
        public void setCommand(string commText, KeyValuePair<string, object> paramPair)
        {
            comm.CommandText = commText;
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue(paramPair.Key, paramPair.Value);
        }
        public void setCommand(string commText, SortedList<string, object> paramList)
        {
            comm.CommandText = commText;
            comm.Parameters.Clear();
            foreach (KeyValuePair<string, object> param in paramList)
            {
                comm.Parameters.AddWithValue(param.Key, param.Value);
            }
        }
        public int ExecuteNonQuery()
        {
            return comm.ExecuteNonQuery();
        }
        public NpgsqlDataReader ExecuteReader()
        {
            return comm.ExecuteReader();
        }
        public object ExecuteScalar()
        {
            return comm.ExecuteScalar();
        }
    }
}
