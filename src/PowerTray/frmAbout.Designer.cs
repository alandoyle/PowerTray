namespace PowerTray
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.PictureBox_Logo = new System.Windows.Forms.PictureBox();
            this.Label_Name = new System.Windows.Forms.Label();
            this.Label_Description = new System.Windows.Forms.Label();
            this.Button_OK = new System.Windows.Forms.Button();
            this.LinkLabel_URL = new System.Windows.Forms.LinkLabel();
            this.Label_GPL = new System.Windows.Forms.Label();
            this.Label_Icon = new System.Windows.Forms.Label();
            this.LinkLabel_IconURL = new System.Windows.Forms.LinkLabel();
            this.Button_Exit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox_Logo
            // 
            this.PictureBox_Logo.Image = global::PowerTray.Properties.Resources.PowerTray;
            this.PictureBox_Logo.Location = new System.Drawing.Point(12, -9);
            this.PictureBox_Logo.Name = "Logo";
            this.PictureBox_Logo.Size = new System.Drawing.Size(132, 132);
            this.PictureBox_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox_Logo.TabIndex = 0;
            this.PictureBox_Logo.TabStop = false;
            // 
            // Label_Name
            // 
            this.Label_Name.AutoSize = true;
            this.Label_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_Name.Location = new System.Drawing.Point(156, 13);
            this.Label_Name.Name = "Name";
            this.Label_Name.Size = new System.Drawing.Size(188, 46);
            this.Label_Name.TabIndex = 1;
            this.Label_Name.Text = "<NAME>";
            // 
            // Label_Description
            // 
            this.Label_Description.AutoSize = true;
            this.Label_Description.Location = new System.Drawing.Point(160, 50);
            this.Label_Description.Name = "Description";
            this.Label_Description.Size = new System.Drawing.Size(102, 15);
            this.Label_Description.TabIndex = 2;
            this.Label_Description.Text = "<DESCRIPTION>";
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(339, 304);
            this.Button_OK.Name = "OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 23);
            this.Button_OK.TabIndex = 5;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // LinkLabel_URL
            // 
            this.LinkLabel_URL.AutoSize = true;
            this.LinkLabel_URL.Location = new System.Drawing.Point(160, 75);
            this.LinkLabel_URL.Name = "URL";
            this.LinkLabel_URL.Size = new System.Drawing.Size(46, 15);
            this.LinkLabel_URL.TabIndex = 6;
            this.LinkLabel_URL.TabStop = true;
            this.LinkLabel_URL.Text = "<URL>";
            this.LinkLabel_URL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_URL_LinkClicked);
            // 
            // Label_GPL
            // 
            this.Label_GPL.AutoSize = true;
            this.Label_GPL.Location = new System.Drawing.Point(12, 126);
            this.Label_GPL.Name = "GPL";
            this.Label_GPL.Size = new System.Drawing.Size(442, 165);
            this.Label_GPL.TabIndex = 7;
            this.Label_GPL.Text = resources.GetString("GPL.Text");
            // 
            // Label_Icon
            // 
            this.Label_Icon.AutoSize = true;
            this.Label_Icon.Location = new System.Drawing.Point(12, 280);
            this.Label_Icon.Name = "Icon";
            this.Label_Icon.Size = new System.Drawing.Size(64, 15);
            this.Label_Icon.TabIndex = 8;
            this.Label_Icon.Text = "Icons from";
            // 
            // LinkLabel_IconURL
            // 
            this.LinkLabel_IconURL.AutoSize = true;
            this.LinkLabel_IconURL.Location = new System.Drawing.Point(66, 280);
            this.LinkLabel_IconURL.Name = "IconURL";
            this.LinkLabel_IconURL.Size = new System.Drawing.Size(142, 15);
            this.LinkLabel_IconURL.TabIndex = 9;
            this.LinkLabel_IconURL.TabStop = true;
            this.LinkLabel_IconURL.Text = "https://www.iconsdb.com";
            this.LinkLabel_IconURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_IconURL_LinkClicked);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Location = new System.Drawing.Point(15, 304);
            this.Button_Exit.Name = "btnExit";
            this.Button_Exit.Size = new System.Drawing.Size(75, 23);
            this.Button_Exit.TabIndex = 10;
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.UseVisualStyleBackColor = true;
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 339);
            this.Controls.Add(this.Button_Exit);
            this.Controls.Add(this.LinkLabel_IconURL);
            this.Controls.Add(this.Label_Icon);
            this.Controls.Add(this.Label_GPL);
            this.Controls.Add(this.LinkLabel_URL);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Label_Description);
            this.Controls.Add(this.Label_Name);
            this.Controls.Add(this.PictureBox_Logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAbout_FormClosing);
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox_Logo;
        private System.Windows.Forms.Label Label_Name;
        private System.Windows.Forms.Label Label_Description;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.LinkLabel LinkLabel_URL;
        private System.Windows.Forms.Label Label_GPL;
        private System.Windows.Forms.Label Label_Icon;
        private System.Windows.Forms.LinkLabel LinkLabel_IconURL;
        private System.Windows.Forms.Button Button_Exit;
    }
}