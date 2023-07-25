namespace UtilityFramework
{
    partial class frmUtilities
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
            this.components = new System.ComponentModel.Container();
            this.naviBar1 = new Guifreaks.NavigationBar.NaviBar(this.components);
            this.naviBand1 = new Guifreaks.NavigationBar.NaviBand(this.components);
            this.naviBand2 = new Guifreaks.NavigationBar.NaviBand(this.components);
            this.windowManagerPanel1 = new MDIWindowManager.WindowManagerPanel();
            this.naviButton1 = new Guifreaks.NavigationBar.NaviButton(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoProd = new System.Windows.Forms.RadioButton();
            this.rdoTest = new System.Windows.Forms.RadioButton();
            this.rdoDev = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.naviBar1)).BeginInit();
            this.naviBar1.SuspendLayout();
            this.naviBand1.SuspendLayout();
            this.naviBand2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // naviBar1
            // 
            this.naviBar1.ActiveBand = this.naviBand1;
            this.naviBar1.Controls.Add(this.naviBand1);
            this.naviBar1.Controls.Add(this.naviBand2);
            this.naviBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.naviBar1.Location = new System.Drawing.Point(0, 0);
            this.naviBar1.Name = "naviBar1";
            this.naviBar1.Size = new System.Drawing.Size(145, 684);
            this.naviBar1.TabIndex = 1;
            this.naviBar1.Text = "naviBar1";
            // 
            // naviBand1
            // 
            // 
            // naviBand1.ClientArea
            // 
            this.naviBand1.ClientArea.Location = new System.Drawing.Point(0, 0);
            this.naviBand1.ClientArea.Name = "ClientArea";
            this.naviBand1.ClientArea.Size = new System.Drawing.Size(143, 617);
            this.naviBand1.ClientArea.TabIndex = 0;
            this.naviBand1.LargeImage = global::UtilityFramework.Properties.Resources.klipper;
            this.naviBand1.Location = new System.Drawing.Point(1, 27);
            this.naviBand1.Name = "naviBand1";
            this.naviBand1.Size = new System.Drawing.Size(143, 617);
            this.naviBand1.SmallImage = global::UtilityFramework.Properties.Resources.klipper;
            this.naviBand1.TabIndex = 3;
            // 
            // naviBand2
            // 
            // 
            // naviBand2.ClientArea
            // 
            this.naviBand2.ClientArea.Location = new System.Drawing.Point(0, 0);
            this.naviBand2.ClientArea.Name = "ClientArea";
            this.naviBand2.ClientArea.Size = new System.Drawing.Size(143, 617);
            this.naviBand2.ClientArea.TabIndex = 0;
            this.naviBand2.LargeImage = global::UtilityFramework.Properties.Resources.history;
            this.naviBand2.Location = new System.Drawing.Point(1, 27);
            this.naviBand2.Name = "naviBand2";
            this.naviBand2.Size = new System.Drawing.Size(143, 617);
            this.naviBand2.SmallImage = global::UtilityFramework.Properties.Resources.history;
            this.naviBand2.TabIndex = 5;
            // 
            // windowManagerPanel1
            // 
            this.windowManagerPanel1.AllowUserVerticalRepositioning = false;
            this.windowManagerPanel1.AutoDetectMdiChildWindows = true;
            this.windowManagerPanel1.AutoHide = false;
            this.windowManagerPanel1.ButtonRenderMode = MDIWindowManager.ButtonRenderMode.Standard;
            this.windowManagerPanel1.DisableCloseAction = false;
            this.windowManagerPanel1.DisableHTileAction = false;
            this.windowManagerPanel1.DisablePopoutAction = false;
            this.windowManagerPanel1.DisableTileAction = false;
            this.windowManagerPanel1.EnableTabPaintEvent = false;
            this.windowManagerPanel1.Location = new System.Drawing.Point(147, 31);
            this.windowManagerPanel1.MinMode = false;
            this.windowManagerPanel1.Name = "windowManagerPanel1";
            this.windowManagerPanel1.Orientation = MDIWindowManager.WindowManagerOrientation.Top;
            this.windowManagerPanel1.ShowCloseButton = true;
            this.windowManagerPanel1.ShowIcons = true;
            this.windowManagerPanel1.ShowLayoutButtons = true;
            this.windowManagerPanel1.ShowTitle = true;
            this.windowManagerPanel1.Size = new System.Drawing.Size(667, 42);
            this.windowManagerPanel1.Style = MDIWindowManager.TabStyle.ClassicTabs;
            this.windowManagerPanel1.TabIndex = 3;
            this.windowManagerPanel1.TabRenderMode = MDIWindowManager.TabsProvider.Standard;
            this.windowManagerPanel1.Text = "Active Utilities";
            this.windowManagerPanel1.TitleBackColor = System.Drawing.SystemColors.ControlDark;
            this.windowManagerPanel1.TitleForeColor = System.Drawing.SystemColors.ControlLightLight;
            // 
            // naviButton1
            // 
            this.naviButton1.LargeImage = global::UtilityFramework.Properties.Resources.klipper_dock;
            this.naviButton1.Location = new System.Drawing.Point(2, 238);
            this.naviButton1.Name = "naviButton1";
            this.naviButton1.Size = new System.Drawing.Size(138, 47);
            this.naviButton1.SmallImage = global::UtilityFramework.Properties.Resources.klipper_dock;
            this.naviButton1.TabIndex = 3;
            this.naviButton1.Text = "Type Logs";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoDev);
            this.panel1.Controls.Add(this.rdoTest);
            this.panel1.Controls.Add(this.rdoProd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(145, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 29);
            this.panel1.TabIndex = 5;
            // 
            // rdoProd
            // 
            this.rdoProd.AutoSize = true;
            this.rdoProd.Checked = true;
            this.rdoProd.Location = new System.Drawing.Point(21, 6);
            this.rdoProd.Name = "rdoProd";
            this.rdoProd.Size = new System.Drawing.Size(76, 17);
            this.rdoProd.TabIndex = 0;
            this.rdoProd.TabStop = true;
            this.rdoProd.Text = "Production";
            this.rdoProd.UseVisualStyleBackColor = true;
            this.rdoProd.CheckedChanged += new System.EventHandler(this.rdoProd_CheckedChanged);
            // 
            // rdoTest
            // 
            this.rdoTest.AutoSize = true;
            this.rdoTest.Location = new System.Drawing.Point(112, 6);
            this.rdoTest.Name = "rdoTest";
            this.rdoTest.Size = new System.Drawing.Size(46, 17);
            this.rdoTest.TabIndex = 1;
            this.rdoTest.Text = "Test";
            this.rdoTest.UseVisualStyleBackColor = true;
            this.rdoTest.CheckedChanged += new System.EventHandler(this.rdoTest_CheckedChanged);
            // 
            // rdoDev
            // 
            this.rdoDev.AutoSize = true;
            this.rdoDev.Location = new System.Drawing.Point(185, 6);
            this.rdoDev.Name = "rdoDev";
            this.rdoDev.Size = new System.Drawing.Size(88, 17);
            this.rdoDev.TabIndex = 2;
            this.rdoDev.Text = "Development";
            this.rdoDev.UseVisualStyleBackColor = true;
            this.rdoDev.CheckedChanged += new System.EventHandler(this.rdoDev_CheckedChanged);
            // 
            // frmUtilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 684);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.windowManagerPanel1);
            this.Controls.Add(this.naviBar1);
            this.IsMdiContainer = true;
            this.Name = "frmUtilities";
            this.Text = "Utilities";
            ((System.ComponentModel.ISupportInitialize)(this.naviBar1)).EndInit();
            this.naviBar1.ResumeLayout(false);
            this.naviBand1.ResumeLayout(false);
            this.naviBand2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guifreaks.NavigationBar.NaviBar naviBar1;
        private Guifreaks.NavigationBar.NaviBand naviBand1;
        private Guifreaks.NavigationBar.NaviBand naviBand2;
        private MDIWindowManager.WindowManagerPanel windowManagerPanel1;
        private Guifreaks.NavigationBar.NaviButton naviButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoDev;
        private System.Windows.Forms.RadioButton rdoTest;
        private System.Windows.Forms.RadioButton rdoProd;
    }
}

