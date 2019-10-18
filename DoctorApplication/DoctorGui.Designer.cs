namespace DoctorApplication
{
    partial class DoctorGui
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
            this.loglbl = new System.Windows.Forms.Label();
            this.lbllogname = new System.Windows.Forms.Label();
            this.getlogsbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loglbl
            // 
            this.loglbl.AutoSize = true;
            this.loglbl.Location = new System.Drawing.Point(574, 13);
            this.loglbl.Name = "loglbl";
            this.loglbl.Size = new System.Drawing.Size(0, 13);
            this.loglbl.TabIndex = 0;
            // 
            // lbllogname
            // 
            this.lbllogname.AutoSize = true;
            this.lbllogname.Location = new System.Drawing.Point(461, 9);
            this.lbllogname.Name = "lbllogname";
            this.lbllogname.Size = new System.Drawing.Size(30, 13);
            this.lbllogname.TabIndex = 1;
            this.lbllogname.Text = "Logs";
            // 
            // getlogsbtn
            // 
            this.getlogsbtn.Location = new System.Drawing.Point(464, 28);
            this.getlogsbtn.Name = "getlogsbtn";
            this.getlogsbtn.Size = new System.Drawing.Size(75, 23);
            this.getlogsbtn.TabIndex = 2;
            this.getlogsbtn.Text = "get logs";
            this.getlogsbtn.UseVisualStyleBackColor = true;
            this.getlogsbtn.Click += new System.EventHandler(this.Getlogsbtn_Click);
            // 
            // DoctorGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.getlogsbtn);
            this.Controls.Add(this.lbllogname);
            this.Controls.Add(this.loglbl);
            this.Name = "DoctorGui";
            this.Text = "DoctorGui";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loglbl;
        private System.Windows.Forms.Label lbllogname;
        private System.Windows.Forms.Button getlogsbtn;
    }
}