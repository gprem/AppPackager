namespace AppPackager
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSourcePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTargetVersion = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEditAppManifest = new System.Windows.Forms.Button();
            this.btnEditDepManifest = new System.Windows.Forms.Button();
            this.tbCurrentDepolymentVersion = new System.Windows.Forms.TextBox();
            this.tbCurrentHTMLVersion = new System.Windows.Forms.TextBox();
            this.tbCurrentAppVersion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.chkRollbackOnError = new System.Windows.Forms.CheckBox();
            this.tbUpdateDllFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowserUpdateDllFolder = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCheckConfig = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.tbDeploymentFolderPath = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select folder:";
            // 
            // tbSourcePath
            // 
            this.tbSourcePath.Location = new System.Drawing.Point(107, 15);
            this.tbSourcePath.Name = "tbSourcePath";
            this.tbSourcePath.Size = new System.Drawing.Size(274, 20);
            this.tbSourcePath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = " New Version:";
            // 
            // tbTargetVersion
            // 
            this.tbTargetVersion.Location = new System.Drawing.Point(107, 67);
            this.tbTargetVersion.Name = "tbTargetVersion";
            this.tbTargetVersion.Size = new System.Drawing.Size(114, 20);
            this.tbTargetVersion.TabIndex = 4;
            this.tbTargetVersion.TextChanged += new System.EventHandler(this.tbTargetVersion_TextChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(325, 334);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(325, 69);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEditAppManifest);
            this.groupBox1.Controls.Add(this.btnEditDepManifest);
            this.groupBox1.Controls.Add(this.tbCurrentDepolymentVersion);
            this.groupBox1.Controls.Add(this.tbCurrentHTMLVersion);
            this.groupBox1.Controls.Add(this.tbCurrentAppVersion);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 108);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Application Info";
            // 
            // btnEditAppManifest
            // 
            this.btnEditAppManifest.Location = new System.Drawing.Point(313, 15);
            this.btnEditAppManifest.Name = "btnEditAppManifest";
            this.btnEditAppManifest.Size = new System.Drawing.Size(75, 23);
            this.btnEditAppManifest.TabIndex = 7;
            this.btnEditAppManifest.Text = "Edit";
            this.btnEditAppManifest.UseVisualStyleBackColor = true;
            this.btnEditAppManifest.Click += new System.EventHandler(this.btnEditAppManifest_Click);
            // 
            // btnEditDepManifest
            // 
            this.btnEditDepManifest.Location = new System.Drawing.Point(313, 45);
            this.btnEditDepManifest.Name = "btnEditDepManifest";
            this.btnEditDepManifest.Size = new System.Drawing.Size(75, 23);
            this.btnEditDepManifest.TabIndex = 6;
            this.btnEditDepManifest.Text = "Edit";
            this.btnEditDepManifest.UseVisualStyleBackColor = true;
            this.btnEditDepManifest.Click += new System.EventHandler(this.btnEditDepManifest_Click);
            // 
            // tbCurrentDepolymentVersion
            // 
            this.tbCurrentDepolymentVersion.Location = new System.Drawing.Point(149, 47);
            this.tbCurrentDepolymentVersion.Name = "tbCurrentDepolymentVersion";
            this.tbCurrentDepolymentVersion.ReadOnly = true;
            this.tbCurrentDepolymentVersion.Size = new System.Drawing.Size(158, 20);
            this.tbCurrentDepolymentVersion.TabIndex = 5;
            // 
            // tbCurrentHTMLVersion
            // 
            this.tbCurrentHTMLVersion.Location = new System.Drawing.Point(149, 77);
            this.tbCurrentHTMLVersion.Name = "tbCurrentHTMLVersion";
            this.tbCurrentHTMLVersion.ReadOnly = true;
            this.tbCurrentHTMLVersion.Size = new System.Drawing.Size(158, 20);
            this.tbCurrentHTMLVersion.TabIndex = 4;
            // 
            // tbCurrentAppVersion
            // 
            this.tbCurrentAppVersion.Location = new System.Drawing.Point(149, 17);
            this.tbCurrentAppVersion.Name = "tbCurrentAppVersion";
            this.tbCurrentAppVersion.ReadOnly = true;
            this.tbCurrentAppVersion.Size = new System.Drawing.Size(158, 20);
            this.tbCurrentAppVersion.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Current Deployment Version:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Current HTML Version:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Current Application Version:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // chkRollbackOnError
            // 
            this.chkRollbackOnError.AutoSize = true;
            this.chkRollbackOnError.Checked = true;
            this.chkRollbackOnError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRollbackOnError.Location = new System.Drawing.Point(6, 19);
            this.chkRollbackOnError.Name = "chkRollbackOnError";
            this.chkRollbackOnError.Size = new System.Drawing.Size(107, 17);
            this.chkRollbackOnError.TabIndex = 8;
            this.chkRollbackOnError.Text = "Rollback on error";
            this.chkRollbackOnError.UseVisualStyleBackColor = true;
            // 
            // tbUpdateDllFolder
            // 
            this.tbUpdateDllFolder.Location = new System.Drawing.Point(107, 41);
            this.tbUpdateDllFolder.Name = "tbUpdateDllFolder";
            this.tbUpdateDllFolder.Size = new System.Drawing.Size(274, 20);
            this.tbUpdateDllFolder.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Update Dll folder:";
            // 
            // btnBrowserUpdateDllFolder
            // 
            this.btnBrowserUpdateDllFolder.Location = new System.Drawing.Point(382, 40);
            this.btnBrowserUpdateDllFolder.Name = "btnBrowserUpdateDllFolder";
            this.btnBrowserUpdateDllFolder.Size = new System.Drawing.Size(24, 20);
            this.btnBrowserUpdateDllFolder.TabIndex = 11;
            this.btnBrowserUpdateDllFolder.Text = "...";
            this.btnBrowserUpdateDllFolder.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(382, 15);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 20);
            this.btnBrowse.TabIndex = 12;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCheckConfig);
            this.groupBox2.Controls.Add(this.chkRollbackOnError);
            this.groupBox2.Location = new System.Drawing.Point(15, 216);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 52);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // btnCheckConfig
            // 
            this.btnCheckConfig.Location = new System.Drawing.Point(302, 15);
            this.btnCheckConfig.Name = "btnCheckConfig";
            this.btnCheckConfig.Size = new System.Drawing.Size(83, 23);
            this.btnCheckConfig.TabIndex = 9;
            this.btnCheckConfig.Text = "Check Config";
            this.btnCheckConfig.UseVisualStyleBackColor = true;
            this.btnCheckConfig.Click += new System.EventHandler(this.btnCheckConfig_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbDeploymentFolderPath);
            this.groupBox3.Controls.Add(this.btnDeploy);
            this.groupBox3.Location = new System.Drawing.Point(15, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(394, 52);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Copy to folder";
            // 
            // btnDeploy
            // 
            this.btnDeploy.Location = new System.Drawing.Point(302, 15);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(83, 23);
            this.btnDeploy.TabIndex = 9;
            this.btnDeploy.Text = "Deploy";
            this.btnDeploy.UseVisualStyleBackColor = true;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // tbDeploymentFolderPath
            // 
            this.tbDeploymentFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeploymentFolderPath.Location = new System.Drawing.Point(6, 19);
            this.tbDeploymentFolderPath.Name = "tbDeploymentFolderPath";
            this.tbDeploymentFolderPath.Size = new System.Drawing.Size(290, 20);
            this.tbDeploymentFolderPath.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 369);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnBrowserUpdateDllFolder);
            this.Controls.Add(this.tbUpdateDllFolder);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tbTargetVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSourcePath);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proplanner Application Packager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSourcePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTargetVersion;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCurrentDepolymentVersion;
        private System.Windows.Forms.TextBox tbCurrentHTMLVersion;
        private System.Windows.Forms.TextBox tbCurrentAppVersion;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckBox chkRollbackOnError;
        private System.Windows.Forms.TextBox tbUpdateDllFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnBrowserUpdateDllFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCheckConfig;
        private System.Windows.Forms.Button btnEditAppManifest;
        private System.Windows.Forms.Button btnEditDepManifest;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.TextBox tbDeploymentFolderPath;
    }
}

