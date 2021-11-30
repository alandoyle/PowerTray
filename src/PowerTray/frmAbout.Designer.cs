namespace PowerTray
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.llURL = new System.Windows.Forms.LinkLabel();
            this.lblGPL = new System.Windows.Forms.Label();
            this.lblIcon = new System.Windows.Forms.Label();
            this.llIconURL = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::PowerTray.Properties.Resources.PowerTray;
            this.pbLogo.Location = new System.Drawing.Point(12, -9);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(132, 132);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(156, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(154, 37);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "<NAME>";
            this.lblName.DoubleClick += new System.EventHandler(this.lblName_DoubleClick);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(160, 50);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(92, 13);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "<DESCRIPTION>";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(339, 304);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // llURL
            // 
            this.llURL.AutoSize = true;
            this.llURL.Location = new System.Drawing.Point(160, 75);
            this.llURL.Name = "llURL";
            this.llURL.Size = new System.Drawing.Size(41, 13);
            this.llURL.TabIndex = 6;
            this.llURL.TabStop = true;
            this.llURL.Text = "<URL>";
            this.llURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llURL_LinkClicked);
            // 
            // lblGPL
            // 
            this.lblGPL.AutoSize = true;
            this.lblGPL.Location = new System.Drawing.Point(12, 126);
            this.lblGPL.Name = "lblGPL";
            this.lblGPL.Size = new System.Drawing.Size(402, 143);
            this.lblGPL.TabIndex = 7;
            this.lblGPL.Text = resources.GetString("lblGPL.Text");
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.Location = new System.Drawing.Point(12, 280);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(56, 13);
            this.lblIcon.TabIndex = 8;
            this.lblIcon.Text = "Icons from";
            // 
            // llIconURL
            // 
            this.llIconURL.AutoSize = true;
            this.llIconURL.Location = new System.Drawing.Point(66, 280);
            this.llIconURL.Name = "llIconURL";
            this.llIconURL.Size = new System.Drawing.Size(130, 13);
            this.llIconURL.TabIndex = 9;
            this.llIconURL.TabStop = true;
            this.llIconURL.Text = "https://www.iconsdb.com";
            this.llIconURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llIconURL_LinkClicked);
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 339);
            this.Controls.Add(this.llIconURL);
            this.Controls.Add(this.lblIcon);
            this.Controls.Add(this.lblGPL);
            this.Controls.Add(this.llURL);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pbLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAbout";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel llURL;
        private System.Windows.Forms.Label lblGPL;
        private System.Windows.Forms.Label lblIcon;
        private System.Windows.Forms.LinkLabel llIconURL;
    }
}