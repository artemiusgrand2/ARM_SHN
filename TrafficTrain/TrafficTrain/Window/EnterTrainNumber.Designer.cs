namespace TrafficTrain
{
    partial class EnterTrainNumber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterTrainNumber));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_numbertrain = new System.Windows.Forms.TextBox();
            this.label_namestation = new System.Windows.Forms.Label();
            this.comboBox_namestation = new System.Windows.Forms.ComboBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.checkBox_DR = new System.Windows.Forms.CheckBox();
            this.checkBox_N = new System.Windows.Forms.CheckBox();
            this.textBox_N = new System.Windows.Forms.TextBox();
            this.checkBox_Y = new System.Windows.Forms.CheckBox();
            this.checkBox_E = new System.Windows.Forms.CheckBox();
            this.checkBox_BM = new System.Windows.Forms.CheckBox();
            this.checkBox_M = new System.Windows.Forms.CheckBox();
            this.checkBox_D = new System.Windows.Forms.CheckBox();
            this.checkBox_O = new System.Windows.Forms.CheckBox();
            this.checkBox_PB = new System.Windows.Forms.CheckBox();
            this.checkBox_PD = new System.Windows.Forms.CheckBox();
            this.checkBox_SP = new System.Windows.Forms.CheckBox();
            this.checkBox_T = new System.Windows.Forms.CheckBox();
            this.checkBox_NBP = new System.Windows.Forms.CheckBox();
            this.textBox_remarka = new System.Windows.Forms.TextBox();
            this.label_remarka = new System.Windows.Forms.Label();
            this.textBox_namestationend = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер поезда";
            // 
            // textBox_numbertrain
            // 
            this.textBox_numbertrain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_numbertrain.Location = new System.Drawing.Point(15, 28);
            this.textBox_numbertrain.Name = "textBox_numbertrain";
            this.textBox_numbertrain.Size = new System.Drawing.Size(121, 20);
            this.textBox_numbertrain.TabIndex = 1;
            this.textBox_numbertrain.Click += new System.EventHandler(this.textBox_numbertrain_Click);
            this.textBox_numbertrain.TextChanged += new System.EventHandler(this.textBox_numbertrain_TextChanged);
            // 
            // label_namestation
            // 
            this.label_namestation.AutoSize = true;
            this.label_namestation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_namestation.Location = new System.Drawing.Point(144, 9);
            this.label_namestation.Name = "label_namestation";
            this.label_namestation.Size = new System.Drawing.Size(149, 15);
            this.label_namestation.TabIndex = 2;
            this.label_namestation.Text = "Вводится со станции";
            // 
            // comboBox_namestation
            // 
            this.comboBox_namestation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_namestation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_namestation.Location = new System.Drawing.Point(147, 27);
            this.comboBox_namestation.Name = "comboBox_namestation";
            this.comboBox_namestation.Size = new System.Drawing.Size(146, 21);
            this.comboBox_namestation.TabIndex = 3;
            this.comboBox_namestation.SelectedIndexChanged += new System.EventHandler(this.comboBox_namestation_SelectedIndexChanged);
            // 
            // button_ok
            // 
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ok.Location = new System.Drawing.Point(226, 168);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "Принять";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cancel.Location = new System.Drawing.Point(226, 197);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // checkBox_DR
            // 
            this.checkBox_DR.AutoSize = true;
            this.checkBox_DR.Location = new System.Drawing.Point(15, 66);
            this.checkBox_DR.Name = "checkBox_DR";
            this.checkBox_DR.Size = new System.Drawing.Size(42, 17);
            this.checkBox_DR.TabIndex = 6;
            this.checkBox_DR.Text = "ДР";
            this.checkBox_DR.UseVisualStyleBackColor = true;
            this.checkBox_DR.CheckedChanged += new System.EventHandler(this.checkBox_DR_CheckedChanged);
            // 
            // checkBox_N
            // 
            this.checkBox_N.AutoSize = true;
            this.checkBox_N.Location = new System.Drawing.Point(15, 89);
            this.checkBox_N.Name = "checkBox_N";
            this.checkBox_N.Size = new System.Drawing.Size(34, 17);
            this.checkBox_N.TabIndex = 7;
            this.checkBox_N.Text = "Н";
            this.checkBox_N.UseVisualStyleBackColor = true;
            this.checkBox_N.CheckedChanged += new System.EventHandler(this.checkBox_N_CheckedChanged);
            // 
            // textBox_N
            // 
            this.textBox_N.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_N.Enabled = false;
            this.textBox_N.Location = new System.Drawing.Point(53, 88);
            this.textBox_N.Name = "textBox_N";
            this.textBox_N.Size = new System.Drawing.Size(51, 20);
            this.textBox_N.TabIndex = 8;
            this.textBox_N.TextChanged += new System.EventHandler(this.textBox_N_TextChanged);
            // 
            // checkBox_Y
            // 
            this.checkBox_Y.AutoSize = true;
            this.checkBox_Y.Location = new System.Drawing.Point(15, 112);
            this.checkBox_Y.Name = "checkBox_Y";
            this.checkBox_Y.Size = new System.Drawing.Size(34, 17);
            this.checkBox_Y.TabIndex = 9;
            this.checkBox_Y.Text = "У";
            this.checkBox_Y.UseVisualStyleBackColor = true;
            this.checkBox_Y.CheckedChanged += new System.EventHandler(this.checkBox_Y_CheckedChanged);
            // 
            // checkBox_E
            // 
            this.checkBox_E.AutoSize = true;
            this.checkBox_E.Location = new System.Drawing.Point(15, 157);
            this.checkBox_E.Name = "checkBox_E";
            this.checkBox_E.Size = new System.Drawing.Size(33, 17);
            this.checkBox_E.TabIndex = 10;
            this.checkBox_E.Text = "Э";
            this.checkBox_E.UseVisualStyleBackColor = true;
            this.checkBox_E.CheckedChanged += new System.EventHandler(this.checkBox_E_CheckedChanged);
            // 
            // checkBox_BM
            // 
            this.checkBox_BM.AutoSize = true;
            this.checkBox_BM.Location = new System.Drawing.Point(15, 180);
            this.checkBox_BM.Name = "checkBox_BM";
            this.checkBox_BM.Size = new System.Drawing.Size(42, 17);
            this.checkBox_BM.TabIndex = 11;
            this.checkBox_BM.Text = "ВМ";
            this.checkBox_BM.UseVisualStyleBackColor = true;
            this.checkBox_BM.CheckedChanged += new System.EventHandler(this.checkBox_BM_CheckedChanged);
            // 
            // checkBox_M
            // 
            this.checkBox_M.AutoSize = true;
            this.checkBox_M.Location = new System.Drawing.Point(15, 203);
            this.checkBox_M.Name = "checkBox_M";
            this.checkBox_M.Size = new System.Drawing.Size(35, 17);
            this.checkBox_M.TabIndex = 12;
            this.checkBox_M.Text = "М";
            this.checkBox_M.UseVisualStyleBackColor = true;
            this.checkBox_M.CheckedChanged += new System.EventHandler(this.checkBox_M_CheckedChanged);
            // 
            // checkBox_D
            // 
            this.checkBox_D.AutoSize = true;
            this.checkBox_D.Location = new System.Drawing.Point(63, 66);
            this.checkBox_D.Name = "checkBox_D";
            this.checkBox_D.Size = new System.Drawing.Size(35, 17);
            this.checkBox_D.TabIndex = 13;
            this.checkBox_D.Text = "Д";
            this.checkBox_D.UseVisualStyleBackColor = true;
            this.checkBox_D.CheckedChanged += new System.EventHandler(this.checkBox_D_CheckedChanged);
            // 
            // checkBox_O
            // 
            this.checkBox_O.AutoSize = true;
            this.checkBox_O.Location = new System.Drawing.Point(63, 112);
            this.checkBox_O.Name = "checkBox_O";
            this.checkBox_O.Size = new System.Drawing.Size(34, 17);
            this.checkBox_O.TabIndex = 14;
            this.checkBox_O.Text = "O";
            this.checkBox_O.UseVisualStyleBackColor = true;
            this.checkBox_O.CheckedChanged += new System.EventHandler(this.checkBox_O_CheckedChanged);
            // 
            // checkBox_PB
            // 
            this.checkBox_PB.AutoSize = true;
            this.checkBox_PB.Location = new System.Drawing.Point(63, 134);
            this.checkBox_PB.Name = "checkBox_PB";
            this.checkBox_PB.Size = new System.Drawing.Size(41, 17);
            this.checkBox_PB.TabIndex = 15;
            this.checkBox_PB.Text = "ПВ";
            this.checkBox_PB.UseVisualStyleBackColor = true;
            this.checkBox_PB.CheckedChanged += new System.EventHandler(this.checkBox_PB_CheckedChanged);
            // 
            // checkBox_PD
            // 
            this.checkBox_PD.AutoSize = true;
            this.checkBox_PD.Location = new System.Drawing.Point(63, 157);
            this.checkBox_PD.Name = "checkBox_PD";
            this.checkBox_PD.Size = new System.Drawing.Size(42, 17);
            this.checkBox_PD.TabIndex = 16;
            this.checkBox_PD.Text = "РД";
            this.checkBox_PD.UseVisualStyleBackColor = true;
            this.checkBox_PD.CheckedChanged += new System.EventHandler(this.checkBox_PD_CheckedChanged);
            // 
            // checkBox_SP
            // 
            this.checkBox_SP.AutoSize = true;
            this.checkBox_SP.Location = new System.Drawing.Point(63, 180);
            this.checkBox_SP.Name = "checkBox_SP";
            this.checkBox_SP.Size = new System.Drawing.Size(41, 17);
            this.checkBox_SP.TabIndex = 17;
            this.checkBox_SP.Text = "СП";
            this.checkBox_SP.UseVisualStyleBackColor = true;
            this.checkBox_SP.CheckedChanged += new System.EventHandler(this.checkBox_SP_CheckedChanged);
            // 
            // checkBox_T
            // 
            this.checkBox_T.AutoSize = true;
            this.checkBox_T.Location = new System.Drawing.Point(15, 134);
            this.checkBox_T.Name = "checkBox_T";
            this.checkBox_T.Size = new System.Drawing.Size(33, 17);
            this.checkBox_T.TabIndex = 18;
            this.checkBox_T.Text = "T";
            this.checkBox_T.UseVisualStyleBackColor = true;
            this.checkBox_T.CheckedChanged += new System.EventHandler(this.checkBox_T_CheckedChanged);
            // 
            // checkBox_NBP
            // 
            this.checkBox_NBP.AutoSize = true;
            this.checkBox_NBP.Location = new System.Drawing.Point(63, 203);
            this.checkBox_NBP.Name = "checkBox_NBP";
            this.checkBox_NBP.Size = new System.Drawing.Size(49, 17);
            this.checkBox_NBP.TabIndex = 19;
            this.checkBox_NBP.Text = "НВП";
            this.checkBox_NBP.UseVisualStyleBackColor = true;
            this.checkBox_NBP.CheckedChanged += new System.EventHandler(this.checkBox_NBP_CheckedChanged);
            // 
            // textBox_remarka
            // 
            this.textBox_remarka.Location = new System.Drawing.Point(147, 88);
            this.textBox_remarka.Name = "textBox_remarka";
            this.textBox_remarka.Size = new System.Drawing.Size(146, 20);
            this.textBox_remarka.TabIndex = 20;
            this.textBox_remarka.Visible = false;
            this.textBox_remarka.TextChanged += new System.EventHandler(this.textBox_remarka_TextChanged);
            // 
            // label_remarka
            // 
            this.label_remarka.AutoSize = true;
            this.label_remarka.Location = new System.Drawing.Point(144, 70);
            this.label_remarka.Name = "label_remarka";
            this.label_remarka.Size = new System.Drawing.Size(63, 13);
            this.label_remarka.TabIndex = 21;
            this.label_remarka.Text = "Пояснения";
            this.label_remarka.Visible = false;
            // 
            // textBox_namestationend
            // 
            this.textBox_namestationend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_namestationend.Location = new System.Drawing.Point(147, 28);
            this.textBox_namestationend.Name = "textBox_namestationend";
            this.textBox_namestationend.Size = new System.Drawing.Size(98, 20);
            this.textBox_namestationend.TabIndex = 22;
            this.textBox_namestationend.Visible = false;
            this.textBox_namestationend.TextChanged += new System.EventHandler(this.textBox_namestationend_TextChanged);
            // 
            // EnterTrainNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(313, 232);
            this.Controls.Add(this.textBox_namestationend);
            this.Controls.Add(this.label_remarka);
            this.Controls.Add(this.textBox_remarka);
            this.Controls.Add(this.checkBox_NBP);
            this.Controls.Add(this.checkBox_T);
            this.Controls.Add(this.checkBox_SP);
            this.Controls.Add(this.checkBox_PD);
            this.Controls.Add(this.checkBox_PB);
            this.Controls.Add(this.checkBox_O);
            this.Controls.Add(this.checkBox_D);
            this.Controls.Add(this.checkBox_M);
            this.Controls.Add(this.checkBox_BM);
            this.Controls.Add(this.checkBox_E);
            this.Controls.Add(this.checkBox_Y);
            this.Controls.Add(this.textBox_N);
            this.Controls.Add(this.checkBox_N);
            this.Controls.Add(this.checkBox_DR);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.comboBox_namestation);
            this.Controls.Add(this.label_namestation);
            this.Controls.Add(this.textBox_numbertrain);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnterTrainNumber";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EnterTrainNumber";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.EnterTrainNumber_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EnterTrainNumber_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EnterTrainNumber_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_numbertrain;
        private System.Windows.Forms.Label label_namestation;
        private System.Windows.Forms.ComboBox comboBox_namestation;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.CheckBox checkBox_DR;
        private System.Windows.Forms.CheckBox checkBox_N;
        private System.Windows.Forms.TextBox textBox_N;
        private System.Windows.Forms.CheckBox checkBox_Y;
        private System.Windows.Forms.CheckBox checkBox_E;
        private System.Windows.Forms.CheckBox checkBox_BM;
        private System.Windows.Forms.CheckBox checkBox_M;
        private System.Windows.Forms.CheckBox checkBox_D;
        private System.Windows.Forms.CheckBox checkBox_O;
        private System.Windows.Forms.CheckBox checkBox_PB;
        private System.Windows.Forms.CheckBox checkBox_PD;
        private System.Windows.Forms.CheckBox checkBox_SP;
        private System.Windows.Forms.CheckBox checkBox_T;
        private System.Windows.Forms.CheckBox checkBox_NBP;
        private System.Windows.Forms.TextBox textBox_remarka;
        private System.Windows.Forms.Label label_remarka;
        private System.Windows.Forms.TextBox textBox_namestationend;
    }
}