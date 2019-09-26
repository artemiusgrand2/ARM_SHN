namespace TrafficTrain
{
    partial class UpdateRemarka
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
            this.button_ok = new System.Windows.Forms.Button();
            this.label_remarka = new System.Windows.Forms.Label();
            this.textBox_remarka = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_cancel.Location = new System.Drawing.Point(323, 39);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_ok.Location = new System.Drawing.Point(242, 39);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "Принять";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // label_remarka
            // 
            this.label_remarka.AutoSize = true;
            this.label_remarka.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_remarka.Location = new System.Drawing.Point(12, 17);
            this.label_remarka.Name = "label_remarka";
            this.label_remarka.Size = new System.Drawing.Size(92, 16);
            this.label_remarka.TabIndex = 4;
            this.label_remarka.Text = "Пояснения:";
            // 
            // textBox_remarka
            // 
            this.textBox_remarka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_remarka.Location = new System.Drawing.Point(111, 14);
            this.textBox_remarka.Name = "textBox_remarka";
            this.textBox_remarka.Size = new System.Drawing.Size(287, 20);
            this.textBox_remarka.TabIndex = 5;
            // 
            // UpdateRemarka
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(410, 74);
            this.Controls.Add(this.textBox_remarka);
            this.Controls.Add(this.label_remarka);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpdateRemarka";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateRemarka";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UpdateRemarka_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UpdateRemarka_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UpdateRemarka_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label_remarka;
        private System.Windows.Forms.TextBox textBox_remarka;
    }
}