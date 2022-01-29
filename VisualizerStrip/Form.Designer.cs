using VisualizerStrip.Properties;

namespace WinformsVisualization
{
    partial class Form
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
            this.VolumeLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.WebsocketURL = new System.Windows.Forms.TextBox();
            this.UrlLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // VolumeLabel
            // 
            this.VolumeLabel.AutoSize = true;
            this.VolumeLabel.Location = new System.Drawing.Point(12, 9);
            this.VolumeLabel.Name = "VolumeLabel";
            this.VolumeLabel.Size = new System.Drawing.Size(25, 13);
            this.VolumeLabel.TabIndex = 1;
            this.VolumeLabel.Text = "===";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.WebsocketURL);
            this.groupBox1.Controls.Add(this.UrlLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(591, 71);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WebSocket Settings";
            // 
            // WebsocketURL
            // 
            this.WebsocketURL.Location = new System.Drawing.Point(7, 42);
            this.WebsocketURL.Name = "WebsocketURL";
            this.WebsocketURL.Size = new System.Drawing.Size(578, 20);
            this.WebsocketURL.TabIndex = 1;
            this.WebsocketURL.TextChanged += new System.EventHandler(this.WebsocketURL_TextChanged);
            // 
            // UrlLabel
            // 
            this.UrlLabel.AutoSize = true;
            this.UrlLabel.Location = new System.Drawing.Point(6, 25);
            this.UrlLabel.Name = "UrlLabel";
            this.UrlLabel.Size = new System.Drawing.Size(29, 13);
            this.UrlLabel.TabIndex = 0;
            this.UrlLabel.Text = "URL";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 103);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 133);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.VolumeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::VisualizerStrip.Properties.Resources.Default;
            this.Name = "Form";
            this.Text = "VisualizerStrip";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label VolumeLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox WebsocketURL;
        private System.Windows.Forms.Label UrlLabel;
        private System.Windows.Forms.Button SaveButton;
    }
}