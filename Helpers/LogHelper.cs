using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Net;
using Oracle.ManagedDataAccess.Client;

namespace StudentManagementSystem.Helpers
{
    public static class LogHelper
    {
        /// <summary>
        /// Log a user action to LOG_HOATDONG.
        /// </summary>
        public static void LogAction(
            string userId, string action, string actionDesc, string status = "Success")
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    var cmd = new OracleCommand(
                        @"INSERT INTO LOG_HOATDONG
                            (USER_ID, ACTION, ACTION_DESC, STATUS, IP_ADDRESS, DEVICE_NAME, ACTION_TIME)
                          VALUES
                            (:userId, :action, :actionDesc, :status, :ip, :device, SYSDATE)",
                        conn);
                    cmd.Parameters.Add(":userId", userId);
                    cmd.Parameters.Add(":action", action);
                    cmd.Parameters.Add(":actionDesc", actionDesc);
                    cmd.Parameters.Add(":status", status);
                    cmd.Parameters.Add(":ip", GetLocalIPAddress());
                    cmd.Parameters.Add(":device", Environment.MachineName);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Optionally log to a local file if DB log fails (do not throw!)
                System.IO.File.AppendAllText("log_errors.txt",
                    $"{DateTime.Now}: Failed to log to DB: {ex.Message}{Environment.NewLine}");
            }
        }

        /// <summary>
        /// Query logs by user or action for admin review.
        /// </summary>
        public static DataTable GetLogs(string filterUserId = null, string filterAction = null)
        {
            using (var conn = Database.GetConnection())
            {
                string sql = "SELECT * FROM LOG_HOATDONG WHERE 1=1";
                if (!string.IsNullOrEmpty(filterUserId))
                    sql += " AND USER_ID = :userId";
                if (!string.IsNullOrEmpty(filterAction))
                    sql += " AND ACTION = :action";
                sql += " ORDER BY ACTION_TIME DESC";

                using (var cmd = new OracleCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(filterUserId))
                        cmd.Parameters.Add(":userId", filterUserId);
                    if (!string.IsNullOrEmpty(filterAction))
                        cmd.Parameters.Add(":action", filterAction);
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Get local IPv4 address.
        /// </summary>
        private static string GetLocalIPAddress()
        {
            string ip = "127.0.0.1";
            try
            {
                foreach (var addr in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return addr.ToString();
                }
            }
            catch { }
            return ip;
        }
    }
}
