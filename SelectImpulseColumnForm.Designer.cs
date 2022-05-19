namespace ImpHoleCalculation
{
    partial class SelectImpulseColumnForm
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
            this.selectButton = new System.Windows.Forms.Button();
            this.koefVarAmpl = new System.Windows.Forms.CheckBox();
            this.MatDevAmpl = new System.Windows.Forms.CheckBox();
            this.AVGAmpl = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AVGDuration = new System.Windows.Forms.CheckBox();
            this.MathDevDuration = new System.Windows.Forms.CheckBox();
            this.koefVarDuration = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.AVGThreshold = new System.Windows.Forms.CheckBox();
            this.MathDevThreshold = new System.Windows.Forms.CheckBox();
            this.koefVarThreshold = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.AVGSquare = new System.Windows.Forms.CheckBox();
            this.MathDevSquare = new System.Windows.Forms.CheckBox();
            this.koefVarSquare = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.AVGMARSE = new System.Windows.Forms.CheckBox();
            this.MathDevMARSE = new System.Windows.Forms.CheckBox();
            this.koefVarMARSE = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.AVGMARSE2 = new System.Windows.Forms.CheckBox();
            this.MathDevMARSE2 = new System.Windows.Forms.CheckBox();
            this.koefVarMARSE2 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(12, 71);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 3;
            this.selectButton.Text = "Ок";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // koefVarAmpl
            // 
            this.koefVarAmpl.AutoSize = true;
            this.koefVarAmpl.Checked = true;
            this.koefVarAmpl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.koefVarAmpl.Location = new System.Drawing.Point(6, 68);
            this.koefVarAmpl.Name = "koefVarAmpl";
            this.koefVarAmpl.Size = new System.Drawing.Size(104, 17);
            this.koefVarAmpl.TabIndex = 2;
            this.koefVarAmpl.Text = "Коэф вариации";
            this.koefVarAmpl.UseVisualStyleBackColor = true;
            // 
            // MatDevAmpl
            // 
            this.MatDevAmpl.AutoSize = true;
            this.MatDevAmpl.Checked = true;
            this.MatDevAmpl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MatDevAmpl.Location = new System.Drawing.Point(6, 42);
            this.MatDevAmpl.Name = "MatDevAmpl";
            this.MatDevAmpl.Size = new System.Drawing.Size(129, 17);
            this.MatDevAmpl.TabIndex = 1;
            this.MatDevAmpl.Text = "Ср. мат. отклонение";
            this.MatDevAmpl.UseVisualStyleBackColor = true;
            this.MatDevAmpl.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // AVGAmpl
            // 
            this.AVGAmpl.AutoSize = true;
            this.AVGAmpl.Checked = true;
            this.AVGAmpl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGAmpl.Location = new System.Drawing.Point(6, 19);
            this.AVGAmpl.Name = "AVGAmpl";
            this.AVGAmpl.Size = new System.Drawing.Size(119, 17);
            this.AVGAmpl.TabIndex = 0;
            this.AVGAmpl.Text = "Среднее значение";
            this.AVGAmpl.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AVGAmpl);
            this.groupBox1.Controls.Add(this.MatDevAmpl);
            this.groupBox1.Controls.Add(this.koefVarAmpl);
            this.groupBox1.Location = new System.Drawing.Point(6, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Амплитуда";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AVGDuration);
            this.groupBox2.Controls.Add(this.MathDevDuration);
            this.groupBox2.Controls.Add(this.koefVarDuration);
            this.groupBox2.Location = new System.Drawing.Point(162, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 100);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Длительность";
            // 
            // AVGDuration
            // 
            this.AVGDuration.AutoSize = true;
            this.AVGDuration.Checked = true;
            this.AVGDuration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGDuration.Location = new System.Drawing.Point(6, 19);
            this.AVGDuration.Name = "AVGDuration";
            this.AVGDuration.Size = new System.Drawing.Size(119, 17);
            this.AVGDuration.TabIndex = 0;
            this.AVGDuration.Text = "Среднее значение";
            this.AVGDuration.UseVisualStyleBackColor = true;
            // 
            // MathDevDuration
            // 
            this.MathDevDuration.AutoSize = true;
            this.MathDevDuration.Location = new System.Drawing.Point(6, 42);
            this.MathDevDuration.Name = "MathDevDuration";
            this.MathDevDuration.Size = new System.Drawing.Size(129, 17);
            this.MathDevDuration.TabIndex = 1;
            this.MathDevDuration.Text = "Ср. мат. отклонение";
            this.MathDevDuration.UseVisualStyleBackColor = true;
            // 
            // koefVarDuration
            // 
            this.koefVarDuration.AutoSize = true;
            this.koefVarDuration.Location = new System.Drawing.Point(6, 68);
            this.koefVarDuration.Name = "koefVarDuration";
            this.koefVarDuration.Size = new System.Drawing.Size(104, 17);
            this.koefVarDuration.TabIndex = 2;
            this.koefVarDuration.Text = "Коэф вариации";
            this.koefVarDuration.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.AVGThreshold);
            this.groupBox4.Controls.Add(this.MathDevThreshold);
            this.groupBox4.Controls.Add(this.koefVarThreshold);
            this.groupBox4.Location = new System.Drawing.Point(318, 100);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(150, 100);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Порог";
            // 
            // AVGThreshold
            // 
            this.AVGThreshold.AutoSize = true;
            this.AVGThreshold.Checked = true;
            this.AVGThreshold.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGThreshold.Location = new System.Drawing.Point(6, 19);
            this.AVGThreshold.Name = "AVGThreshold";
            this.AVGThreshold.Size = new System.Drawing.Size(119, 17);
            this.AVGThreshold.TabIndex = 0;
            this.AVGThreshold.Text = "Среднее значение";
            this.AVGThreshold.UseVisualStyleBackColor = true;
            // 
            // MathDevThreshold
            // 
            this.MathDevThreshold.AutoSize = true;
            this.MathDevThreshold.Location = new System.Drawing.Point(6, 42);
            this.MathDevThreshold.Name = "MathDevThreshold";
            this.MathDevThreshold.Size = new System.Drawing.Size(129, 17);
            this.MathDevThreshold.TabIndex = 1;
            this.MathDevThreshold.Text = "Ср. мат. отклонение";
            this.MathDevThreshold.UseVisualStyleBackColor = true;
            // 
            // koefVarThreshold
            // 
            this.koefVarThreshold.AutoSize = true;
            this.koefVarThreshold.Location = new System.Drawing.Point(6, 68);
            this.koefVarThreshold.Name = "koefVarThreshold";
            this.koefVarThreshold.Size = new System.Drawing.Size(104, 17);
            this.koefVarThreshold.TabIndex = 2;
            this.koefVarThreshold.Text = "Коэф вариации";
            this.koefVarThreshold.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.AVGSquare);
            this.groupBox5.Controls.Add(this.MathDevSquare);
            this.groupBox5.Controls.Add(this.koefVarSquare);
            this.groupBox5.Location = new System.Drawing.Point(474, 100);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(150, 100);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Площадь";
            // 
            // AVGSquare
            // 
            this.AVGSquare.AutoSize = true;
            this.AVGSquare.Checked = true;
            this.AVGSquare.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGSquare.Location = new System.Drawing.Point(6, 19);
            this.AVGSquare.Name = "AVGSquare";
            this.AVGSquare.Size = new System.Drawing.Size(119, 17);
            this.AVGSquare.TabIndex = 0;
            this.AVGSquare.Text = "Среднее значение";
            this.AVGSquare.UseVisualStyleBackColor = true;
            // 
            // MathDevSquare
            // 
            this.MathDevSquare.AutoSize = true;
            this.MathDevSquare.Location = new System.Drawing.Point(6, 42);
            this.MathDevSquare.Name = "MathDevSquare";
            this.MathDevSquare.Size = new System.Drawing.Size(129, 17);
            this.MathDevSquare.TabIndex = 1;
            this.MathDevSquare.Text = "Ср. мат. отклонение";
            this.MathDevSquare.UseVisualStyleBackColor = true;
            // 
            // koefVarSquare
            // 
            this.koefVarSquare.AutoSize = true;
            this.koefVarSquare.Location = new System.Drawing.Point(6, 68);
            this.koefVarSquare.Name = "koefVarSquare";
            this.koefVarSquare.Size = new System.Drawing.Size(104, 17);
            this.koefVarSquare.TabIndex = 2;
            this.koefVarSquare.Text = "Коэф вариации";
            this.koefVarSquare.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.AVGMARSE);
            this.groupBox6.Controls.Add(this.MathDevMARSE);
            this.groupBox6.Controls.Add(this.koefVarMARSE);
            this.groupBox6.Location = new System.Drawing.Point(630, 100);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(150, 100);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "MARSE";
            // 
            // AVGMARSE
            // 
            this.AVGMARSE.AutoSize = true;
            this.AVGMARSE.Checked = true;
            this.AVGMARSE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGMARSE.Location = new System.Drawing.Point(6, 19);
            this.AVGMARSE.Name = "AVGMARSE";
            this.AVGMARSE.Size = new System.Drawing.Size(119, 17);
            this.AVGMARSE.TabIndex = 0;
            this.AVGMARSE.Text = "Среднее значение";
            this.AVGMARSE.UseVisualStyleBackColor = true;
            // 
            // MathDevMARSE
            // 
            this.MathDevMARSE.AutoSize = true;
            this.MathDevMARSE.Location = new System.Drawing.Point(6, 42);
            this.MathDevMARSE.Name = "MathDevMARSE";
            this.MathDevMARSE.Size = new System.Drawing.Size(129, 17);
            this.MathDevMARSE.TabIndex = 1;
            this.MathDevMARSE.Text = "Ср. мат. отклонение";
            this.MathDevMARSE.UseVisualStyleBackColor = true;
            // 
            // koefVarMARSE
            // 
            this.koefVarMARSE.AutoSize = true;
            this.koefVarMARSE.Location = new System.Drawing.Point(6, 68);
            this.koefVarMARSE.Name = "koefVarMARSE";
            this.koefVarMARSE.Size = new System.Drawing.Size(104, 17);
            this.koefVarMARSE.TabIndex = 2;
            this.koefVarMARSE.Text = "Коэф вариации";
            this.koefVarMARSE.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.AVGMARSE2);
            this.groupBox7.Controls.Add(this.MathDevMARSE2);
            this.groupBox7.Controls.Add(this.koefVarMARSE2);
            this.groupBox7.Location = new System.Drawing.Point(6, 206);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(150, 100);
            this.groupBox7.TabIndex = 13;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "MARSE/Порог";
            // 
            // AVGMARSE2
            // 
            this.AVGMARSE2.AutoSize = true;
            this.AVGMARSE2.Checked = true;
            this.AVGMARSE2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGMARSE2.Location = new System.Drawing.Point(6, 19);
            this.AVGMARSE2.Name = "AVGMARSE2";
            this.AVGMARSE2.Size = new System.Drawing.Size(119, 17);
            this.AVGMARSE2.TabIndex = 0;
            this.AVGMARSE2.Text = "Среднее значение";
            this.AVGMARSE2.UseVisualStyleBackColor = true;
            // 
            // MathDevMARSE2
            // 
            this.MathDevMARSE2.AutoSize = true;
            this.MathDevMARSE2.Location = new System.Drawing.Point(6, 42);
            this.MathDevMARSE2.Name = "MathDevMARSE2";
            this.MathDevMARSE2.Size = new System.Drawing.Size(129, 17);
            this.MathDevMARSE2.TabIndex = 1;
            this.MathDevMARSE2.Text = "Ср. мат. отклонение";
            this.MathDevMARSE2.UseVisualStyleBackColor = true;
            // 
            // koefVarMARSE2
            // 
            this.koefVarMARSE2.AutoSize = true;
            this.koefVarMARSE2.Location = new System.Drawing.Point(6, 68);
            this.koefVarMARSE2.Name = "koefVarMARSE2";
            this.koefVarMARSE2.Size = new System.Drawing.Size(104, 17);
            this.koefVarMARSE2.TabIndex = 2;
            this.koefVarMARSE2.Text = "Коэф вариации";
            this.koefVarMARSE2.UseVisualStyleBackColor = true;
            // 
            // SelectImpulseColumnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 317);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.selectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectImpulseColumnForm";
            this.Load += new System.EventHandler(this.SelectImpulseColumnForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button selectButton;
        public System.Windows.Forms.CheckBox koefVarAmpl;
        public System.Windows.Forms.CheckBox MatDevAmpl;
        public System.Windows.Forms.CheckBox AVGAmpl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox AVGDuration;
        public System.Windows.Forms.CheckBox MathDevDuration;
        public System.Windows.Forms.CheckBox koefVarDuration;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.CheckBox AVGThreshold;
        public System.Windows.Forms.CheckBox MathDevThreshold;
        public System.Windows.Forms.CheckBox koefVarThreshold;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.CheckBox AVGSquare;
        public System.Windows.Forms.CheckBox MathDevSquare;
        public System.Windows.Forms.CheckBox koefVarSquare;
        private System.Windows.Forms.GroupBox groupBox6;
        public System.Windows.Forms.CheckBox AVGMARSE;
        public System.Windows.Forms.CheckBox MathDevMARSE;
        public System.Windows.Forms.CheckBox koefVarMARSE;
        private System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.CheckBox AVGMARSE2;
        public System.Windows.Forms.CheckBox MathDevMARSE2;
        public System.Windows.Forms.CheckBox koefVarMARSE2;
    }
}