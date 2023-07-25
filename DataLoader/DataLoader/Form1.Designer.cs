namespace DataLoader
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.ofdDataFile = new System.Windows.Forms.OpenFileDialog();
            this.btnOperaForecast = new System.Windows.Forms.Button();
            this.btnOperaFourWeek = new System.Windows.Forms.Button();
            this.btnLoadMicros = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnFindFile = new System.Windows.Forms.Button();
            this.cmbTableList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDelimiter = new System.Windows.Forms.ComboBox();
            this.btnWeather = new System.Windows.Forms.Button();
            this.chkNoHeaders = new System.Windows.Forms.CheckBox();
            this.cmbCulture = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(379, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load POS Journal";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(13, 442);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(479, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(379, 95);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Load Sage";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnOperaForecast
            // 
            this.btnOperaForecast.Location = new System.Drawing.Point(243, 257);
            this.btnOperaForecast.Name = "btnOperaForecast";
            this.btnOperaForecast.Size = new System.Drawing.Size(113, 23);
            this.btnOperaForecast.TabIndex = 3;
            this.btnOperaForecast.Text = "Load Opera Forcast";
            this.btnOperaForecast.UseVisualStyleBackColor = true;
            this.btnOperaForecast.Click += new System.EventHandler(this.btnOperaForecast_Click);
            // 
            // btnOperaFourWeek
            // 
            this.btnOperaFourWeek.Location = new System.Drawing.Point(243, 228);
            this.btnOperaFourWeek.Name = "btnOperaFourWeek";
            this.btnOperaFourWeek.Size = new System.Drawing.Size(113, 23);
            this.btnOperaFourWeek.TabIndex = 4;
            this.btnOperaFourWeek.Text = "Load Opera 4 Week";
            this.btnOperaFourWeek.UseVisualStyleBackColor = true;
            this.btnOperaFourWeek.Click += new System.EventHandler(this.btnOperaFourWeek_Click);
            // 
            // btnLoadMicros
            // 
            this.btnLoadMicros.Location = new System.Drawing.Point(379, 66);
            this.btnLoadMicros.Name = "btnLoadMicros";
            this.btnLoadMicros.Size = new System.Drawing.Size(113, 23);
            this.btnLoadMicros.TabIndex = 5;
            this.btnLoadMicros.Text = "Load CSV";
            this.btnLoadMicros.UseVisualStyleBackColor = true;
            this.btnLoadMicros.Click += new System.EventHandler(this.btnLoadMicros_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(13, 13);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(448, 20);
            this.txtFile.TabIndex = 6;
            // 
            // btnFindFile
            // 
            this.btnFindFile.Location = new System.Drawing.Point(468, 11);
            this.btnFindFile.Name = "btnFindFile";
            this.btnFindFile.Size = new System.Drawing.Size(24, 23);
            this.btnFindFile.TabIndex = 7;
            this.btnFindFile.Text = "...";
            this.btnFindFile.UseVisualStyleBackColor = true;
            this.btnFindFile.Click += new System.EventHandler(this.btnFindFileClick);
            // 
            // cmbTableList
            // 
            this.cmbTableList.FormattingEnabled = true;
            this.cmbTableList.Location = new System.Drawing.Point(133, 39);
            this.cmbTableList.Name = "cmbTableList";
            this.cmbTableList.Size = new System.Drawing.Size(359, 21);
            this.cmbTableList.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Database Table Name";
            // 
            // cmbDelimiter
            // 
            this.cmbDelimiter.FormattingEnabled = true;
            this.cmbDelimiter.Location = new System.Drawing.Point(133, 67);
            this.cmbDelimiter.Name = "cmbDelimiter";
            this.cmbDelimiter.Size = new System.Drawing.Size(121, 21);
            this.cmbDelimiter.TabIndex = 10;
            // 
            // btnWeather
            // 
            this.btnWeather.Location = new System.Drawing.Point(379, 227);
            this.btnWeather.Name = "btnWeather";
            this.btnWeather.Size = new System.Drawing.Size(113, 23);
            this.btnWeather.TabIndex = 12;
            this.btnWeather.Text = "Get Weather";
            this.btnWeather.UseVisualStyleBackColor = true;
            this.btnWeather.Click += new System.EventHandler(this.btnWeather_Click);
            // 
            // chkNoHeaders
            // 
            this.chkNoHeaders.AutoSize = true;
            this.chkNoHeaders.Location = new System.Drawing.Point(290, 70);
            this.chkNoHeaders.Name = "chkNoHeaders";
            this.chkNoHeaders.Size = new System.Drawing.Size(83, 17);
            this.chkNoHeaders.TabIndex = 13;
            this.chkNoHeaders.Text = "No Headers";
            this.chkNoHeaders.UseVisualStyleBackColor = true;
            // 
            // cmbCulture
            // 
            this.cmbCulture.FormattingEnabled = true;
            this.cmbCulture.Location = new System.Drawing.Point(133, 97);
            this.cmbCulture.Name = "cmbCulture";
            this.cmbCulture.Size = new System.Drawing.Size(240, 21);
            this.cmbCulture.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 477);
            this.Controls.Add(this.cmbCulture);
            this.Controls.Add(this.chkNoHeaders);
            this.Controls.Add(this.btnWeather);
            this.Controls.Add(this.cmbDelimiter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTableList);
            this.Controls.Add(this.btnFindFile);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnLoadMicros);
            this.Controls.Add(this.btnOperaFourWeek);
            this.Controls.Add(this.btnOperaForecast);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog ofdDataFile;
        private System.Windows.Forms.Button btnOperaForecast;
        private System.Windows.Forms.Button btnOperaFourWeek;
        private System.Windows.Forms.Button btnLoadMicros;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnFindFile;
        private System.Windows.Forms.ComboBox cmbTableList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDelimiter;
        private System.Windows.Forms.Button btnWeather;
        private System.Windows.Forms.CheckBox chkNoHeaders;
        private System.Windows.Forms.ComboBox cmbCulture;
    }
}

