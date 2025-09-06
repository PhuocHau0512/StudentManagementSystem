using Emgu.CV;
using Emgu.CV.Structure;
using StudentManagementSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    // Lớp này kế thừa từ Form và là một 'partial class', nó sẽ tự động liên kết với file Designer.cs
    public partial class Face_Registration_Modal : Form
    {
        // Các biến để xử lý webcam và ảnh
        private VideoCapture _capture;
        private Timer _timer;
        private Image<Bgr, byte> _currentFrame;

        // Các thuộc tính (properties) để trả kết quả về cho Form chính đã gọi nó
        public string SavedImagePath { get; private set; }
        public string SavedImageHash { get; private set; }
        public string UserId { get; set; } // Dùng để truyền Mã số sinh viên vào Form này

        // Hàm khởi tạo của Form
        public Face_Registration_Modal()
        {
            // Lệnh này rất quan trọng, nó khởi tạo các control đã được thiết kế (PictureBox, Button,...)
            InitializeComponent();

            // Gán các sự kiện cho các control và cho chính Form
            this.btnCapture.Click += BtnCapture_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.Load += Face_Registration_Modal_Load;
            this.FormClosing += Face_Registration_Modal_FormClosing;
        }

        // Sự kiện này chạy khi Form được tải lên lần đầu
        private void Face_Registration_Modal_Load(object sender, EventArgs e)
        {
            try
            {
                _capture = new VideoCapture(0); // Khởi tạo webcam mặc định
                if (!_capture.IsOpened)
                {
                    MessageBox.Show("Không thể truy cập webcam.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Tạo một Timer để cập nhật khung hình từ webcam liên tục
                _timer = new Timer();
                _timer.Interval = 33; // Khoảng 30 khung hình/giây
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo webcam: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // Mỗi lần Timer tick, hàm này sẽ được gọi để lấy frame mới từ webcam
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Giải phóng frame cũ trước khi lấy frame mới
                _currentFrame?.Dispose();
                _currentFrame = _capture.QueryFrame()?.ToImage<Bgr, byte>();

                if (_currentFrame != null)
                {
                    // Giải phóng ảnh cũ trong PictureBox trước khi gán ảnh mới
                    pbPreview.Image?.Dispose();
                    // Hiển thị frame lên PictureBox (pbPreview là tên bạn đặt trong Designer)
                    pbPreview.Image = _currentFrame.AsBitmap();
                }
            }
            catch (Exception) { /* Bỏ qua các lỗi có thể xảy ra khi đang bắt hình */ }
        }

        // Sự kiện khi nhấn nút "Capture"
        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (_currentFrame == null)
            {
                MessageBox.Show("Không có khung hình nào được chụp. Vui lòng thử lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(UserId))
            {
                MessageBox.Show("Mã số sinh viên (UserID) chưa được thiết lập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Gọi hàm từ lớp BiometricHelper để lưu và băm ảnh
            bool success = BiometricHelper.SaveAndHashImage(UserId, _currentFrame, out string path, out string hash);

            if (success)
            {
                // Lưu kết quả vào properties để Form chính có thể lấy
                this.SavedImagePath = path;
                this.SavedImageHash = hash;
                MessageBox.Show("Đăng ký khuôn mặt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Đặt kết quả trả về là OK
                this.Close();
            }
            else
            {
                MessageBox.Show("Lưu hoặc băm ảnh thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        // Sự kiện khi nhấn nút "Cancel"
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Sự kiện này chạy ngay trước khi Form đóng lại
        private void Face_Registration_Modal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Rất quan trọng: Phải giải phóng tài nguyên để tránh rò rỉ bộ nhớ và treo webcam
            _timer?.Stop();
            _timer?.Dispose();
            _capture?.Dispose();
            _currentFrame?.Dispose();
            pbPreview.Image?.Dispose();
        }
    }
}