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
    public partial class StudentManagerForm : Form
    {
        private readonly string _userId;
        // Optionally store user role here for RBAC checks
        public StudentManagerForm(string userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadStudents();
        }

        private void LoadStudents(string filter = "")
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql = "SELECT STUDENT_ID, FULLNAME, DOB, GENDER, EMAIL, PHONE, ADDRESS, MAJOR, CLASS, CREATED_AT FROM STUDENTS";
                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        sql += " WHERE LOWER(FULLNAME) LIKE :filter OR LOWER(MAJOR) LIKE :filter OR LOWER(CLASS) LIKE :filter";
                    }
                    sql += " ORDER BY CREATED_AT DESC";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(filter))
                            cmd.Parameters.Add(":filter", "%" + filter.ToLower() + "%");

                        using (var adapter = new OracleDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            adapter.Fill(dt);

                            // Optionally decrypt email/phone here if needed, using CryptoHelper

                            studentsGridView.DataSource = dt;
                        }
                    }
                }
                LogHelper.LogAction(_userId, "STUDENT_VIEW", "Loaded student list");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load students: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text.Trim());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text.Trim());
        }

        // === CRUD operation stubs for admin/manager roles ===

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // TODO: Implement Add Student dialog/form (admin/manager only)
            // Open a modal dialog, then on success:
            // LoadStudents();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (studentsGridView.CurrentRow == null)
                return;

            string studentId = studentsGridView.CurrentRow.Cells["STUDENT_ID"].Value.ToString();
            // TODO: Implement Edit Student dialog/form (admin/manager only)
            // On success:
            // LoadStudents();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (studentsGridView.CurrentRow == null)
                return;

            string studentId = studentsGridView.CurrentRow.Cells["STUDENT_ID"].Value.ToString();
            if (MessageBox.Show($"Delete student {studentId}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = Database.GetConnection())
                    {
                        var cmd = new OracleCommand("DELETE FROM STUDENTS WHERE STUDENT_ID = :id", conn);
                        cmd.Parameters.Add(":id", studentId);
                        cmd.ExecuteNonQuery();
                    }
                    LogHelper.LogAction(_userId, "STUDENT_DELETE", $"Deleted student {studentId}");
                    LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete student: " + ex.Message);
                }
            }
        }

        // Optionally: handle DataGridView events for UI enhancements

    }
}
