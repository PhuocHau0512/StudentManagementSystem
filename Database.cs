using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem
{
    public class Database
    {
        private static OracleConnection _conn;
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// Initialize database connection string (recommended to call at program start)
        /// </summary>
        public static void Init(string host, string port, string sid, string username, string password)
        {
            ConnectionString = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port})))(CONNECT_DATA=(SID={sid})));User Id={username};Password={password};";
            _conn = new OracleConnection(ConnectionString);
        }

        /// <summary>
        /// Get a live OracleConnection object (auto-reconnect if closed)
        /// </summary>
        public static OracleConnection GetConnection()
        {
            if (_conn == null)
                throw new Exception("Database connection is not initialized. Call Database.Init(...) first.");
            if (_conn.State != ConnectionState.Open)
                _conn.Open();
            return _conn;
        }

        /// <summary>
        /// Close the connection if open
        /// </summary>
        public static void Close()
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
                _conn.Close();
        }

        /// <summary>
        /// Run a non-query SQL command (INSERT, UPDATE, DELETE)
        /// </summary>
        public static int ExecuteNonQuery(string sql, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(sql, GetConnection()))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Run a scalar SQL command (returns first column of first row)
        /// </summary>
        public static object ExecuteScalar(string sql, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(sql, GetConnection()))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Run a query and return a DataTable (SELECT)
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(sql, GetConnection()))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Run a query and return an OracleDataReader (caller must Dispose)
        /// </summary>
        public static OracleDataReader ExecuteReader(string sql, params OracleParameter[] parameters)
        {
            var cmd = new OracleCommand(sql, GetConnection());
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
} 

        

