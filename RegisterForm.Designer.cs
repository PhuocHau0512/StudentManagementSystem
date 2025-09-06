namespace StudentManagementSystem
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblStudentId, lblFullName, lblDOB, lblGender, lblEmail, lblPhone, lblUsername, lblPassword;
        private System.Windows.Forms.TextBox txtStudentId, txtFullName, txtEmail, txtPhone, txtUsername, txtPassword;
        private System.Windows.Forms.DateTimePicker dtpDOB;
        private System.Windows.Forms.ComboBox cbGender;
        private System.Windows.Forms.Button btnRegister, btnRegisterFace;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblStudentId = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblDOB = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtStudentId = new System.Windows.Forms.TextBox();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.dtpDOB = new System.Windows.Forms.DateTimePicker();
            this.cbGender = new System.Windows.Forms.ComboBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnRegisterFace = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Labels
            this.lblStudentId.Location = new System.Drawing.Point(40, 25);
            this.lblStudentId.Text = "Student ID:";
            this.lblFullName.Location = new System.Drawing.Point(40, 65);
            this.lblFullName.Text = "Full Name:";
            this.lblDOB.Location = new System.Drawing.Point(40, 105);
            this.lblDOB.Text = "Date of Birth:";
            this.lblGender.Location = new System.Drawing.Point(40, 145);
            this.lblGender.Text = "Gender:";
            this.lblEmail.Location = new System.Drawing.Point(40, 185);
            this.lblEmail.Text = "Email:";
            this.lblPhone.Location = new System.Drawing.Point(40, 225);
            this.lblPhone.Text = "Phone:";
            this.lblUsername.Location = new System.Drawing.Point(40, 265);
            this.lblUsername.Text = "Username:";
            this.lblPassword.Location = new System.Drawing.Point(40, 305);
            this.lblPassword.Text = "Password:";

            // TextBoxes/Inputs
            this.txtStudentId.Location = new System.Drawing.Point(150, 22);
            this.txtFullName.Location = new System.Drawing.Point(150, 62);
            this.dtpDOB.Location = new System.Drawing.Point(150, 102);
            this.dtpDOB.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.cbGender.Location = new System.Drawing.Point(150, 142);
            this.cbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });
            this.txtEmail.Location = new System.Drawing.Point(150, 182);
            this.txtPhone.Location = new System.Drawing.Point(150, 222);
            this.txtUsername.Location = new System.Drawing.Point(150, 262);
            this.txtPassword.Location = new System.Drawing.Point(150, 302);
            this.txtPassword.UseSystemPasswordChar = true;

            // Buttons
            this.btnRegister.Location = new System.Drawing.Point(60, 350);
            this.btnRegister.Size = new System.Drawing.Size(110, 32);
            this.btnRegister.Text = "Register";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            this.btnRegisterFace.Location = new System.Drawing.Point(200, 350);
            this.btnRegisterFace.Size = new System.Drawing.Size(140, 32);
            this.btnRegisterFace.Text = "Register Face";
            this.btnRegisterFace.Click += new System.EventHandler(this.btnRegisterFace_Click);

            // RegisterForm
            this.ClientSize = new System.Drawing.Size(400, 410);
            this.Controls.Add(this.lblStudentId);
            this.Controls.Add(this.txtStudentId);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblDOB);
            this.Controls.Add(this.dtpDOB);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.cbGender);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnRegisterFace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register Student";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
