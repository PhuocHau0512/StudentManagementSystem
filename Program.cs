using System;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // QUAN TRỌNG: Khởi tạo kết nối Database trước khi chạy ứng dụng
            // !!! THAY THẾ CÁC THÔNG TIN NÀY BẰNG THÔNG TIN DATABASE CỦA BẠN !!!
            try
            {
                string host = "localhost";
                string port = "1521";
                string sid = "orcl"; // Hoặc ORCL, tùy cấu hình của bạn
                string username = "STUDEN_APP"; // Thay bằng user Oracle của bạn
                string password = "Oracle123"; // Thay bằng password của bạn
                Database.Init(host, port, sid, username, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể khởi tạo kết nối database: " + ex.Message,
                                "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng ứng dụng nếu không kết nối được DB
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());

            // Đóng kết nối khi ứng dụng thoát
            Database.Close();
        }
    }
}