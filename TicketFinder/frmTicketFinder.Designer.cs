namespace TicketFinder
{
    partial class frmTicketFinder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtTicketNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.chkStartDate = new System.Windows.Forms.CheckBox();
            this.chkEndDate = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTicketText = new System.Windows.Forms.TextBox();
            this.cmbEstablishment = new System.Windows.Forms.ComboBox();
            this.chkEstablishment = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.btnSaveColumns = new System.Windows.Forms.Button();
            this.txtSummaryData = new System.Windows.Forms.TextBox();
            this.chkTimePeriod = new System.Windows.Forms.CheckBox();
            this.cmbTimePeriod = new System.Windows.Forms.ComboBox();
            this.btnCustomReports = new System.Windows.Forms.Button();
            this.btnTextFile = new System.Windows.Forms.Button();
            this.chkSingleDay = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(32, 12);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(200, 20);
            this.dtpStartDate.TabIndex = 0;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(284, 12);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEndDate.TabIndex = 1;
            // 
            // txtTicketNumber
            // 
            this.txtTicketNumber.Location = new System.Drawing.Point(112, 63);
            this.txtTicketNumber.Name = "txtTicketNumber";
            this.txtTicketNumber.Size = new System.Drawing.Size(100, 20);
            this.txtTicketNumber.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ticket Number";
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToOrderColumns = true;
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(12, 143);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(1001, 487);
            this.dgvData.TabIndex = 4;
            this.dgvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            this.dgvData.DoubleClick += new System.EventHandler(this.dgvData_DoubleClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(409, 91);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkStartDate
            // 
            this.chkStartDate.AutoSize = true;
            this.chkStartDate.Checked = true;
            this.chkStartDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartDate.Location = new System.Drawing.Point(15, 15);
            this.chkStartDate.Name = "chkStartDate";
            this.chkStartDate.Size = new System.Drawing.Size(15, 14);
            this.chkStartDate.TabIndex = 6;
            this.chkStartDate.UseVisualStyleBackColor = true;
            this.chkStartDate.CheckedChanged += new System.EventHandler(this.chkStartDate_CheckedChanged);
            // 
            // chkEndDate
            // 
            this.chkEndDate.AutoSize = true;
            this.chkEndDate.Checked = true;
            this.chkEndDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEndDate.Location = new System.Drawing.Point(267, 15);
            this.chkEndDate.Name = "chkEndDate";
            this.chkEndDate.Size = new System.Drawing.Size(15, 14);
            this.chkEndDate.TabIndex = 7;
            this.chkEndDate.UseVisualStyleBackColor = true;
            this.chkEndDate.CheckedChanged += new System.EventHandler(this.chkEndDate_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ticket Text";
            // 
            // txtTicketText
            // 
            this.txtTicketText.Location = new System.Drawing.Point(151, 117);
            this.txtTicketText.Name = "txtTicketText";
            this.txtTicketText.Size = new System.Drawing.Size(252, 20);
            this.txtTicketText.TabIndex = 8;
            // 
            // cmbEstablishment
            // 
            this.cmbEstablishment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstablishment.Enabled = false;
            this.cmbEstablishment.FormattingEnabled = true;
            this.cmbEstablishment.Location = new System.Drawing.Point(284, 36);
            this.cmbEstablishment.Name = "cmbEstablishment";
            this.cmbEstablishment.Size = new System.Drawing.Size(200, 21);
            this.cmbEstablishment.Sorted = true;
            this.cmbEstablishment.TabIndex = 10;
            // 
            // chkEstablishment
            // 
            this.chkEstablishment.AutoSize = true;
            this.chkEstablishment.Location = new System.Drawing.Point(267, 39);
            this.chkEstablishment.Name = "chkEstablishment";
            this.chkEstablishment.Size = new System.Drawing.Size(15, 14);
            this.chkEstablishment.TabIndex = 11;
            this.chkEstablishment.UseVisualStyleBackColor = true;
            this.chkEstablishment.CheckedChanged += new System.EventHandler(this.chkEstablishment_CheckedChanged);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(12, 115);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSaveColumns
            // 
            this.btnSaveColumns.Location = new System.Drawing.Point(409, 115);
            this.btnSaveColumns.Name = "btnSaveColumns";
            this.btnSaveColumns.Size = new System.Drawing.Size(75, 23);
            this.btnSaveColumns.TabIndex = 13;
            this.btnSaveColumns.Text = "Save";
            this.btnSaveColumns.UseVisualStyleBackColor = true;
            this.btnSaveColumns.Click += new System.EventHandler(this.btnSaveColumns_Click);
            // 
            // txtSummaryData
            // 
            this.txtSummaryData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummaryData.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSummaryData.Location = new System.Drawing.Point(490, 12);
            this.txtSummaryData.Multiline = true;
            this.txtSummaryData.Name = "txtSummaryData";
            this.txtSummaryData.Size = new System.Drawing.Size(523, 125);
            this.txtSummaryData.TabIndex = 14;
            // 
            // chkTimePeriod
            // 
            this.chkTimePeriod.AutoSize = true;
            this.chkTimePeriod.Location = new System.Drawing.Point(267, 66);
            this.chkTimePeriod.Name = "chkTimePeriod";
            this.chkTimePeriod.Size = new System.Drawing.Size(15, 14);
            this.chkTimePeriod.TabIndex = 16;
            this.chkTimePeriod.UseVisualStyleBackColor = true;
            this.chkTimePeriod.Visible = false;
            this.chkTimePeriod.CheckedChanged += new System.EventHandler(this.chkTimePeriod_CheckedChanged);
            // 
            // cmbTimePeriod
            // 
            this.cmbTimePeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimePeriod.Enabled = false;
            this.cmbTimePeriod.FormattingEnabled = true;
            this.cmbTimePeriod.Location = new System.Drawing.Point(284, 63);
            this.cmbTimePeriod.Name = "cmbTimePeriod";
            this.cmbTimePeriod.Size = new System.Drawing.Size(200, 21);
            this.cmbTimePeriod.Sorted = true;
            this.cmbTimePeriod.TabIndex = 15;
            this.cmbTimePeriod.Visible = false;
            // 
            // btnCustomReports
            // 
            this.btnCustomReports.Location = new System.Drawing.Point(16, 86);
            this.btnCustomReports.Name = "btnCustomReports";
            this.btnCustomReports.Size = new System.Drawing.Size(93, 23);
            this.btnCustomReports.TabIndex = 17;
            this.btnCustomReports.Text = "Custom Reports";
            this.btnCustomReports.UseVisualStyleBackColor = true;
            this.btnCustomReports.Visible = false;
            this.btnCustomReports.Click += new System.EventHandler(this.btnCustomReports_Click);
            // 
            // btnTextFile
            // 
            this.btnTextFile.Location = new System.Drawing.Point(115, 86);
            this.btnTextFile.Name = "btnTextFile";
            this.btnTextFile.Size = new System.Drawing.Size(93, 23);
            this.btnTextFile.TabIndex = 18;
            this.btnTextFile.Text = "Text Report";
            this.btnTextFile.UseVisualStyleBackColor = true;
            this.btnTextFile.Visible = false;
            this.btnTextFile.Click += new System.EventHandler(this.btnTextFile_Click);
            // 
            // chkSingleDay
            // 
            this.chkSingleDay.AutoSize = true;
            this.chkSingleDay.Location = new System.Drawing.Point(15, 38);
            this.chkSingleDay.Name = "chkSingleDay";
            this.chkSingleDay.Size = new System.Drawing.Size(77, 17);
            this.chkSingleDay.TabIndex = 19;
            this.chkSingleDay.Text = "Single Day";
            this.chkSingleDay.UseVisualStyleBackColor = true;
            this.chkSingleDay.CheckedChanged += new System.EventHandler(this.chkSingleDay_CheckedChanged);
            // 
            // frmTicketFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 642);
            this.Controls.Add(this.chkSingleDay);
            this.Controls.Add(this.btnTextFile);
            this.Controls.Add(this.btnCustomReports);
            this.Controls.Add(this.chkTimePeriod);
            this.Controls.Add(this.cmbTimePeriod);
            this.Controls.Add(this.txtSummaryData);
            this.Controls.Add(this.btnSaveColumns);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.chkEstablishment);
            this.Controls.Add(this.cmbEstablishment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTicketText);
            this.Controls.Add(this.chkEndDate);
            this.Controls.Add(this.chkStartDate);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTicketNumber);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Name = "frmTicketFinder";
            this.Text = "Ticket Finder";
            this.Load += new System.EventHandler(this.frmTicketFinder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.TextBox txtTicketNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox chkStartDate;
        private System.Windows.Forms.CheckBox chkEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTicketText;
        private System.Windows.Forms.ComboBox cmbEstablishment;
        private System.Windows.Forms.CheckBox chkEstablishment;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private System.Windows.Forms.Button btnSaveColumns;
        private System.Windows.Forms.TextBox txtSummaryData;
        private System.Windows.Forms.CheckBox chkTimePeriod;
        private System.Windows.Forms.ComboBox cmbTimePeriod;
        private System.Windows.Forms.Button btnCustomReports;
        private System.Windows.Forms.Button btnTextFile;
        private System.Windows.Forms.CheckBox chkSingleDay;
    }
}

