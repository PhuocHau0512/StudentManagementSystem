using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace StudentManagementSystem
{
    public partial class Face_Verification_Modal : Form
    {
        private readonly string _userId;
        private VideoCapture _capture;
        private Timer _timer;

        // Thuộc tính để trả kết quả về cho LoginForm
        public bool IsVerified { get; private set; } = false;

        public Face_Verification_Modal(string userId)
        {
            _userId = userId;
            InitializeComponent(); // Khởi tạo các control từ file Designer

            this.Load += Face_Verification_Modal_Load;
            this.FormClosing += Face_Verification_Modal_FormClosing;
        }

        private void Face_Verification_Modal_Load(object sender, EventArgs e)
        {
            try
            {
                _capture = new VideoCapture(0);
                if (!_capture.IsOpened)
                {
                    MessageBox.Show("Không thể truy cập webcam.", "Lỗi");
                    this.Close();
                    return;
                }

                // Thử xác thực 1 giây một lần
                _timer = new Timer { Interval = 1000 };
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo webcam: " + ex.Message);
                this.Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Tạm dừng timer để tránh việc xác thực chồng chéo
            _timer.Stop();

            using (var frame = _capture.QueryFrame()?.ToImage<Bgr, byte>())
            {
                if (frame != null)
                {
                    pbPreview.Image?.Dispose();
                    pbPreview.Image = frame.AsBitmap();

                    // Cập nhật trạng thái
                    lblStatus.Text = "Đang phân tích...";

                    // Gọi hàm xác thực từ BiometricHelper
                    if (BiometricHelper.VerifyFace(_userId, frame, out double confidence))
                    {
                        // Nếu thành công
                        IsVerified = true;
                        MessageBox.Show($"Xác thực thành công! (Độ tin cậy: {confidence:0.0})");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return; // Thoát khỏi hàm
                    }
                    else
                    {
                        // Nếu thất bại, cập nhật trạng thái và tiếp tục
                        lblStatus.Text = "Không khớp. Vui lòng giữ yên...";
                    }
                }
            }

            // Bật lại timer để thử lại sau 1 giây
            _timer.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Face_Verification_Modal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Giải phóng tài nguyên
            _timer?.Stop();
            _timer?.Dispose();
            _capture?.Dispose();
            pbPreview.Image?.Dispose();
        }
    }
}
