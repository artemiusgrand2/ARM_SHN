namespace TrafficTrain
{
    partial class Miniature
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
            this.label_name = new System.Windows.Forms.Label();
            this.label_infoload = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.ForeColor = System.Drawing.Color.Brown;
            this.label_name.Location = new System.Drawing.Point(12, 9);
            this.label_name.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(109, 24);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "АРМ ДНЦ";
            // 
            // label_infoload
            // 
            this.label_infoload.AutoSize = true;
            this.label_infoload.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_infoload.ForeColor = System.Drawing.Color.DimGray;
            this.label_infoload.Location = new System.Drawing.Point(13, 42);
            this.label_infoload.Name = "label_infoload";
            this.label_infoload.Size = new System.Drawing.Size(0, 17);
            this.label_infoload.TabIndex = 1;
            // 
            // Miniature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(325, 71);
            this.Controls.Add(this.label_infoload);
            this.Controls.Add(this.label_name);
            this.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Miniature";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Miniature";
            this.TopMost = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Miniature_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_infoload;
    }
}