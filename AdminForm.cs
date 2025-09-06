using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using StudentManagementSystem.Helpers;


namespace StudentManagementSystem
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            LoadLogs();
        }
        /// <summary>
        /// Loads all logs into the DataGridView, with optional search/filter.
        /// </summary>
        /// <param name="filter">Optional filter text (user id or action)</param>
        private void LoadLogs(string filter = "")
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql = "SELECT LOG_ID, USER_ID, ACTION, ACTION_DESC, ACTION_TIME, STATUS, IP_ADDRESS, DEVICE_NAME FROM LOG_HOATDONG";
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        sql += " WHERE LOWER(USER_ID) LIKE :filter OR LOWER(ACTION) LIKE :filter OR LOWER(ACTION_DESC) LIKE :filter";
                    }
                    sql += " ORDER BY ACTION_TIME DESC";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(filter))
                            cmd.Parameters.Add(":filter", "%" + filter.ToLower() + "%");

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);
                            logsGridView.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load logs: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLogs(txtSearch.Text.Trim());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadLogs(txtSearch.Text.Trim());
        }

        // Optional: Export logs to CSV
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (logsGridView.DataSource is DataTable dt)
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = $"Logs_{DateTime.Now:yyyyMMddHHmmss}.csv"
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportDataTableToCSV(dt, sfd.FileName);
                        MessageBox.Show("Logs exported!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Export failed: " + ex.Message);
                    }
                }
            }
        }

        private void ExportDataTableToCSV(DataTable dt, string path)
        {
            using (var sw = new System.IO.StreamWriter(path, false))
            {
                // Write headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < dt.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.WriteLine();

                // Write rows
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string val = row[i]?.ToString().Replace(",", " ");
                        sw.Write(val);
                        if (i < dt.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.WriteLine();
                }
            }
        }

        // Optionally: Add more tabs for viewing other audit tables, FGA logs, or user/session reports.
    }
}
