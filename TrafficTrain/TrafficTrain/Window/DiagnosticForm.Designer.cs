namespace TrafficTrain
{
    partial class DiagnosticForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.panel_impuls = new System.Windows.Forms.Panel();
            this.label_imp = new System.Windows.Forms.Label();
            this.label_train = new System.Windows.Forms.Label();
            this.panel_train = new System.Windows.Forms.Panel();
            this.panel_impuls.SuspendLayout();
            this.panel_train.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_cancel.Location = new System.Drawing.Point(189, 35);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // panel_impuls
            // 
            this.panel_impuls.Controls.Add(this.label_imp);
            this.panel_impuls.Location = new System.Drawing.Point(0, 0);
            this.panel_impuls.Name = "panel_impuls";
            this.panel_impuls.Size = new System.Drawing.Size(225, 30);
            this.panel_impuls.TabIndex = 7;
            // 
            // label_imp
            // 
            this.label_imp.AutoSize = true;
            this.label_imp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_imp.Location = new System.Drawing.Point(21, 10);
            this.label_imp.Name = "label_imp";
            this.label_imp.Size = new System.Drawing.Size(41, 13);
            this.label_imp.TabIndex = 0;
            this.label_imp.Text = "label1";
            // 
            // label_train
            // 
            this.label_train.AutoSize = true;
            this.label_train.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_train.Location = new System.Drawing.Point(16, 11);
            this.label_train.Name = "label_train";
            this.label_train.Size = new System.Drawing.Size(41, 13);
            this.label_train.TabIndex = 0;
            this.label_train.Text = "label1";
            // 
            // panel_train
            // 
            this.panel_train.Controls.Add(this.label_train);
            this.panel_train.Location = new System.Drawing.Point(225, 0);
            this.panel_train.Name = "panel_train";
            this.panel_train.Size = new System.Drawing.Size(225, 30);
            this.panel_train.TabIndex = 9;
            // 
            // DiagnosticForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 64);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.panel_train);
            this.Controls.Add(this.panel_impuls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DiagnosticForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DiagnosticForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DiagnosticForm_Paint);
            this.panel_impuls.ResumeLayout(false);
            this.panel_impuls.PerformLayout();
            this.panel_train.ResumeLayout(false);
            this.panel_train.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Panel panel_impuls;
        private System.Windows.Forms.Label label_imp;
        private System.Windows.Forms.Label label_train;
        private System.Windows.Forms.Panel panel_train;
    }
}