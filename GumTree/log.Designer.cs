namespace GumTree
{
    partial class log
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(log));
            this.tb_license = new System.Windows.Forms.TextBox();
            this.bt_ok = new System.Windows.Forms.Button();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_license
            // 
            this.tb_license.BackColor = System.Drawing.Color.Silver;
            this.tb_license.Location = new System.Drawing.Point(159, 33);
            this.tb_license.Name = "tb_license";
            this.tb_license.Size = new System.Drawing.Size(232, 20);
            this.tb_license.TabIndex = 1;
            this.tb_license.MouseDown += new System.Windows.Forms.MouseEventHandler(this.log_MouseDown);
            this.tb_license.MouseMove += new System.Windows.Forms.MouseEventHandler(this.log_MouseMove);
            // 
            // bt_ok
            // 
            this.bt_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_ok.Location = new System.Drawing.Point(103, 73);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(85, 24);
            this.bt_ok.TabIndex = 2;
            this.bt_ok.Text = "ok";
            this.bt_ok.UseVisualStyleBackColor = true;
            this.bt_ok.Click += new System.EventHandler(this.bt_ok_Click);
            this.bt_ok.MouseDown += new System.Windows.Forms.MouseEventHandler(this.log_MouseDown);
            this.bt_ok.MouseMove += new System.Windows.Forms.MouseEventHandler(this.log_MouseMove);
            // 
            // bt_cancel
            // 
            this.bt_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.bt_cancel.Location = new System.Drawing.Point(243, 73);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(85, 24);
            this.bt_cancel.TabIndex = 2;
            this.bt_cancel.Text = "cancel";
            this.bt_cancel.UseVisualStyleBackColor = true;
            this.bt_cancel.Click += new System.EventHandler(this.bt_cancel_Click_1);
            // 
            // log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImage = global::GumTree.Properties.Resources.Untitled_7;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(413, 109);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_ok);
            this.Controls.Add(this.tb_license);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "log";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "log";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.log_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.log_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_license;
        private System.Windows.Forms.Button bt_ok;
        private System.Windows.Forms.Button bt_cancel;

    }
}