namespace HazyBits.Twain.Cloud.Forms
{
    partial class RegistrationForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.registrationUrlLabel = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.registrationTokenTextBox = new System.Windows.Forms.TextBox();
            this.progressPictureBox = new System.Windows.Forms.PictureBox();
            this.statusLabel = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.progressPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please complete scanner registration using the following URL:";
            // 
            // registrationUrlLabel
            // 
            this.registrationUrlLabel.AutoSize = true;
            this.registrationUrlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.registrationUrlLabel.Location = new System.Drawing.Point(12, 46);
            this.registrationUrlLabel.Name = "registrationUrlLabel";
            this.registrationUrlLabel.Size = new System.Drawing.Size(160, 13);
            this.registrationUrlLabel.TabIndex = 3;
            this.registrationUrlLabel.TabStop = true;
            this.registrationUrlLabel.Tag = "";
            this.registrationUrlLabel.Text = "https://some.obscure.register.url";
            this.registrationUrlLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(276, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "When requested, provide the following registration token:";
            // 
            // registrationTokenTextBox
            // 
            this.registrationTokenTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.registrationTokenTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.registrationTokenTextBox.Location = new System.Drawing.Point(164, 116);
            this.registrationTokenTextBox.Name = "registrationTokenTextBox";
            this.registrationTokenTextBox.ReadOnly = true;
            this.registrationTokenTextBox.Size = new System.Drawing.Size(124, 31);
            this.registrationTokenTextBox.TabIndex = 5;
            this.registrationTokenTextBox.Text = "XXXXXX";
            this.registrationTokenTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // progressPictureBox
            // 
            this.progressPictureBox.Image = global::HazyBits.Twain.Cloud.Forms.Properties.Resources.Progress;
            this.progressPictureBox.Location = new System.Drawing.Point(194, 185);
            this.progressPictureBox.Name = "progressPictureBox";
            this.progressPictureBox.Size = new System.Drawing.Size(64, 64);
            this.progressPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.progressPictureBox.TabIndex = 4;
            this.progressPictureBox.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusLabel.Location = new System.Drawing.Point(12, 165);
            this.statusLabel.Multiline = true;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.ReadOnly = true;
            this.statusLabel.Size = new System.Drawing.Size(440, 14);
            this.statusLabel.TabIndex = 7;
            this.statusLabel.Text = "Waiting for registration completion...";
            this.statusLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 261);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.registrationTokenTextBox);
            this.Controls.Add(this.progressPictureBox);
            this.Controls.Add(this.registrationUrlLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "RegistrationForm";
            this.Text = "Registration Form";
            ((System.ComponentModel.ISupportInitialize)(this.progressPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel registrationUrlLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox progressPictureBox;
        private System.Windows.Forms.TextBox registrationTokenTextBox;
        private System.Windows.Forms.TextBox statusLabel;
    }
}