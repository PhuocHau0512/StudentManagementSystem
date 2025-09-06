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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    var cmd = new OracleCommand(
                        "SELECT USER_ID, PASSWORD_HASH, SALT, STATUS FROM ACCOUNTS WHERE USERNAME = :uname", conn);
                    cmd.Parameters.Add(":uname", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string userId = reader.GetString(0);
                            string passwordHash = reader.GetString(1);
                            string salt = reader.GetString(2);
                            string status = reader.GetString(3);

                            if (!status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Account is not active.");
                                LogHelper.LogAction(userId, "LOGIN", "Account inactive", "Fail");
                                return;
                            }

                            if (CryptoHelper.HashPassword(password, salt) == passwordHash)
                            {
                                LogHelper.LogAction(userId, "LOGIN", "Password login success", "Success");
                                MessageBox.Show("Login successful!");
                                // Optionally store session, open next form:
                                this.Hide();
                                new StudentManagerForm(userId).Show();
                            }
                            else
                            {
                                LogHelper.LogAction(userId, "LOGIN", "Wrong password", "Fail");
                                MessageBox.Show("Login failed: Wrong password.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Login failed: User not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message);
            }
        }
        private void btnFaceLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập để sử dụng chức năng này.");
                return;
            }

            string userId = null;
            try
            {
                object result = Database.ExecuteScalar("SELECT USER_ID FROM ACCOUNTS WHERE USERNAME = :uname", new OracleParameter(":uname", username));
                if (result != null)
                    userId = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thông tin người dùng: " + ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("Không tìm thấy người dùng với tên đăng nhập này.");
                return;
            }

            // Mở form xác thực khuôn mặt mới
            using (var verificationForm = new Face_Verification_Modal(userId))
            {
                this.Hide(); // Tạm ẩn form đăng nhập
                verificationForm.ShowDialog(); // Hiển thị form xác thực và chờ nó đóng lại

                if (verificationForm.IsVerified)
                {
                    LogHelper.LogAction(userId, "BIOMETRIC_LOGIN", "Đăng nhập bằng khuôn mặt thành công", "Success");
                    MessageBox.Show("Đăng nhập bằng khuôn mặt thành công!");

                    // Mở form chính và đóng form login
                    new StudentManagerForm(userId).ShowDialog();
                    this.Close();
                }
                else
                {
                    LogHelper.LogAction(userId, "BIOMETRIC_LOGIN", "Đăng nhập bằng khuôn mặt thất bại", "Fail");
                    MessageBox.Show("Xác thực khuôn mặt thất bại.");
                    this.Show(); // Hiển thị lại form đăng nhập nếu thất bại
                }
            }
        }
    }
}
