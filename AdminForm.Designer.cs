namespace StudentManagementSystem
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView logsGridView;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnRefresh, btnExport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.logsGridView = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.logsGridView)).BeginInit();
            this.SuspendLayout();

            // lblSearch
            this.lblSearch.Location = new System.Drawing.Point(20, 18);
            this.lblSearch.Size = new System.Drawing.Size(50, 20);
            this.lblSearch.Text = "Search:";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(75, 15);
            this.txtSearch.Size = new System.Drawing.Size(180, 23);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(280, 13);
            this.btnRefresh.Size = new System.Drawing.Size(70, 26);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // btnExport
            this.btnExport.Location = new System.Drawing.Point(370, 13);
            this.btnExport.Size = new System.Drawing.Size(70, 26);
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // logsGridView
            this.logsGridView.AllowUserToAddRows = false;
            this.logsGridView.AllowUserToDeleteRows = false;
            this.logsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.logsGridView.Location = new System.Drawing.Point(20, 50);
            this.logsGridView.Name = "logsGridView";
            this.logsGridView.ReadOnly = true;
            this.logsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logsGridView.Size = new System.Drawing.Size(680, 320);

            // AdminForm
            this.ClientSize = new System.Drawing.Size(720, 400);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.logsGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Logs Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.logsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
