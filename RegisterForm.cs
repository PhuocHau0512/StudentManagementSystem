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
    public partial class RegisterForm : Form
    {
        private string _userId;
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // 1. Validate input
            string studentId = txtStudentId.Text.Trim();
            string fullname = txtFullName.Text.Trim();
            DateTime dob = dtpDOB.Value.Date;
            string gender = cbGender.SelectedItem?.ToString() ?? "";
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // 2. Create salt/hash for password
            string salt = CryptoHelper.GenerateSalt();
            string passwordHash = CryptoHelper.HashPassword(password, salt);

            // 3. AES encrypt email/phone (demo: using salt as key, you can use any strong key scheme)
            string encryptedEmail = CryptoHelper.AESEncrypt(email, salt);
            string encryptedPhone = CryptoHelper.AESEncrypt(phone, salt);

            // 4. Create new user/account in Oracle
            _userId = Guid.NewGuid().ToString("N").Substring(0, 12);

            try
            {
                using (var conn = Database.GetConnection())
                {
                    // Insert into STUDENTS
                    var cmd1 = new OracleCommand(
                        @"INSERT INTO STUDENTS (STUDENT_ID, FULLNAME, DOB, GENDER, EMAIL, PHONE, ADDRESS, MAJOR, CLASS, CREATED_AT)
                        VALUES (:id, :name, :dob, :gender, UTL_RAW.CAST_TO_RAW(:email), UTL_RAW.CAST_TO_RAW(:phone), :address, :major, :class, SYSDATE)", conn);
                    cmd1.Parameters.Add(":id", studentId);
                    cmd1.Parameters.Add(":name", fullname);
                    cmd1.Parameters.Add(":dob", dob);
                    cmd1.Parameters.Add(":gender", gender);
                    cmd1.Parameters.Add(":email", encryptedEmail);
                    cmd1.Parameters.Add(":phone", encryptedPhone);
                    cmd1.Parameters.Add(":address", ""); // Optional
                    cmd1.Parameters.Add(":major", "IT"); // Or get from input
                    cmd1.Parameters.Add(":class", "IT01"); // Or get from input
                    cmd1.ExecuteNonQuery();

                    // Insert into ACCOUNTS
                    var cmd2 = new OracleCommand(
                        @"INSERT INTO ACCOUNTS
                        (USER_ID, STUDENT_ID, USERNAME, PASSWORD_HASH, SALT, ROLE_ID, STATUS, CREATED_AT)
                        VALUES (:uid, :sid, :uname, :phash, :salt, :role, 'Active', SYSDATE)", conn);
                    cmd2.Parameters.Add(":uid", _userId);
                    cmd2.Parameters.Add(":sid", studentId);
                    cmd2.Parameters.Add(":uname", username);
                    cmd2.Parameters.Add(":phash", passwordHash);
                    cmd2.Parameters.Add(":salt", salt);
                    cmd2.Parameters.Add(":role", 2); // 2 = Student role
                    cmd2.ExecuteNonQuery();
                }

                MessageBox.Show("Registration successful. Now register your face biometric.");
                LogHelper.LogAction(_userId, "REGISTER", "User registered", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed: " + ex.Message);
                LogHelper.LogAction(_userId ?? "N/A", "REGISTER", $"Registration failed: {ex.Message}", "Fail");
            }
        }

        private void btnRegisterFace_Click(object sender, EventArgs e)
        {
            // Ensure registration is done first
            if (string.IsNullOrEmpty(_userId))
            {
                MessageBox.Show("Please register account first (click Register) before face registration.");
                return;
            }

            // Register face
            string imagePath, imageHash;
            if (BiometricHelper.Face_Verification_Modal(_userId, out imagePath, out imageHash))
            {
                try
                {
                    using (var conn = Database.GetConnection())
                    {
                        var cmd = new OracleCommand(
                            @"INSERT INTO BIOMETRICS (BIOMETRIC_ID, USER_ID, IMAGE_HASH, IMAGE_PATH, IMAGE_TYPE, REGISTERED_AT, STATUS)
                              VALUES (:bid, :uid, :hash, :path, 'face', SYSDATE, 'Valid')", conn);
                        cmd.Parameters.Add(":bid", Guid.NewGuid().ToString());
                        cmd.Parameters.Add(":uid", _userId);
                        cmd.Parameters.Add(":hash", imageHash);
                        cmd.Parameters.Add(":path", imagePath);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Face registration successful!");
                    LogHelper.LogAction(_userId, "FACE_REGISTER", "Face image registered", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save biometric: " + ex.Message);
                    LogHelper.LogAction(_userId, "FACE_REGISTER", $"Biometric DB error: {ex.Message}", "Fail");
                }
            }
            else
            {
                LogHelper.LogAction(_userId, "FACE_REGISTER", "Face image capture failed", "Fail");
            }
        }

        // Xóa hàm btnRegisterStudentFace_Click bị thừa đi và sửa lại hàm này.
        //private void btnRegisterFace_Click(object sender, EventArgs e)
        //{
        //    // Đảm bảo người dùng đã nhấn nút Register để có _userId
        //    if (string.IsNullOrEmpty(_userId))
        //    {
        //        MessageBox.Show("Vui lòng đăng ký thông tin tài khoản trước khi đăng ký khuôn mặt.", "Thông báo");
        //        return;
        //    }

        //    // Sử dụng 'using' để đảm bảo Form được giải phóng tài nguyên đúng cách
        //    using (var registrationForm = new Face_Registration_Modal())
        //    {
        //        // Gán mã người dùng (USER_ID) cho Form đăng ký
        //        registrationForm.UserId = this._userId;

        //        // Hiển thị Form và chờ kết quả
        //        if (registrationForm.ShowDialog() == DialogResult.OK)
        //        {
        //            // Nếu đăng ký thành công, lấy dữ liệu trả về
        //            string imagePath = registrationForm.SavedImagePath;
        //            string imageHash = registrationForm.SavedImageHash;

        //            // ==> Tại đây, bạn sẽ viết code để lưu thông tin sinh trắc học vào database
        //            try
        //            {
        //                using (var conn = Database.GetConnection())
        //                {
        //                    var cmd = new OracleCommand(
        //                        @"INSERT INTO BIOMETRICS (BIOMETRIC_ID, USER_ID, IMAGE_HASH, IMAGE_PATH, IMAGE_TYPE, STATUS)
        //                  VALUES (:bid, :uid, :hash, :path, 'face', 'Valid')", conn);
        //                    cmd.Parameters.Add(new OracleParameter(":bid", Guid.NewGuid().ToString("N")));
        //                    cmd.Parameters.Add(new OracleParameter(":uid", _userId));
        //                    cmd.Parameters.Add(new OracleParameter(":hash", imageHash));
        //                    cmd.Parameters.Add(new OracleParameter(":path", imagePath));
        //                    cmd.ExecuteNonQuery();
        //                }
        //                MessageBox.Show("Lưu thông tin khuôn mặt vào database thành công!");
        //                LogHelper.LogAction(_userId, "FACE_REGISTER", "Đăng ký khuôn mặt thành công", "Success");
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi khi lưu thông tin khuôn mặt: " + ex.Message, "Lỗi Database");
        //                LogHelper.LogAction(_userId, "FACE_REGISTER", $"Lỗi DB: {ex.Message}", "Fail");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Quá trình đăng ký khuôn mặt đã bị hủy.");
        //            LogHelper.LogAction(_userId, "FACE_REGISTER", "Người dùng hủy đăng ký khuôn mặt", "Fail");
        //        }
        //    }
        //}
    }
}
