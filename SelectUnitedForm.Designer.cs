namespace ImpHoleCalculation
{
    partial class SelectUnitedForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AVGAAZRoot = new System.Windows.Forms.CheckBox();
            this.MatDevAAZRoot = new System.Windows.Forms.CheckBox();
            this.koefVarAAZRoot = new System.Windows.Forms.CheckBox();
            this.AVGAAZ = new System.Windows.Forms.CheckBox();
            this.MatDevAAZ = new System.Windows.Forms.CheckBox();
            this.koefVarAAZ = new System.Windows.Forms.CheckBox();
            this.checkSumEnergy = new System.Windows.Forms.CheckBox();
            this.checkAazType = new System.Windows.Forms.CheckBox();
            this.checkAvgZ = new System.Windows.Forms.CheckBox();
            this.checkAvgY = new System.Windows.Forms.CheckBox();
            this.checkAvgX = new System.Windows.Forms.CheckBox();
            this.checkAAZCalcTime = new System.Windows.Forms.CheckBox();
            this.checkAAZEventLastTime = new System.Windows.Forms.CheckBox();
            this.checkAAZEventFirstTime = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LeadingEdgeTime = new System.Windows.Forms.CheckBox();
            this.MARSE_Threshold = new System.Windows.Forms.CheckBox();
            this.MARSE = new System.Windows.Forms.CheckBox();
            this.Area = new System.Windows.Forms.CheckBox();
            this.Threshold = new System.Windows.Forms.CheckBox();
            this.Duration = new System.Windows.Forms.CheckBox();
            this.Amplitude = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.koefVarImp = new System.Windows.Forms.CheckBox();
            this.AVGImpRoot = new System.Windows.Forms.CheckBox();
            this.MatDevImp = new System.Windows.Forms.CheckBox();
            this.MatDevImpRoot = new System.Windows.Forms.CheckBox();
            this.AVGImp = new System.Windows.Forms.CheckBox();
            this.koefVarImpRoot = new System.Windows.Forms.CheckBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkAazType);
            this.groupBox1.Controls.Add(this.checkAvgZ);
            this.groupBox1.Controls.Add(this.checkAvgY);
            this.groupBox1.Controls.Add(this.checkAvgX);
            this.groupBox1.Controls.Add(this.checkAAZCalcTime);
            this.groupBox1.Controls.Add(this.checkAAZEventLastTime);
            this.groupBox1.Controls.Add(this.checkAAZEventFirstTime);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 257);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры ААЗ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AVGAAZRoot);
            this.groupBox2.Controls.Add(this.MatDevAAZRoot);
            this.groupBox2.Controls.Add(this.koefVarAAZRoot);
            this.groupBox2.Controls.Add(this.AVGAAZ);
            this.groupBox2.Controls.Add(this.MatDevAAZ);
            this.groupBox2.Controls.Add(this.koefVarAAZ);
            this.groupBox2.Controls.Add(this.checkSumEnergy);
            this.groupBox2.Location = new System.Drawing.Point(188, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 217);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Энергия";
            // 
            // AVGAAZRoot
            // 
            this.AVGAAZRoot.AutoSize = true;
            this.AVGAAZRoot.Checked = true;
            this.AVGAAZRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGAAZRoot.Location = new System.Drawing.Point(6, 114);
            this.AVGAAZRoot.Name = "AVGAAZRoot";
            this.AVGAAZRoot.Size = new System.Drawing.Size(176, 17);
            this.AVGAAZRoot.TabIndex = 8;
            this.AVGAAZRoot.Text = "Среднее значение (корневое)";
            this.AVGAAZRoot.UseVisualStyleBackColor = true;
            // 
            // MatDevAAZRoot
            // 
            this.MatDevAAZRoot.AutoSize = true;
            this.MatDevAAZRoot.Checked = true;
            this.MatDevAAZRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MatDevAAZRoot.Location = new System.Drawing.Point(6, 137);
            this.MatDevAAZRoot.Name = "MatDevAAZRoot";
            this.MatDevAAZRoot.Size = new System.Drawing.Size(186, 17);
            this.MatDevAAZRoot.TabIndex = 9;
            this.MatDevAAZRoot.Text = "Ср. мат. отклонение (корневое)";
            this.MatDevAAZRoot.UseVisualStyleBackColor = true;
            // 
            // koefVarAAZRoot
            // 
            this.koefVarAAZRoot.AutoSize = true;
            this.koefVarAAZRoot.Checked = true;
            this.koefVarAAZRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.koefVarAAZRoot.Location = new System.Drawing.Point(6, 163);
            this.koefVarAAZRoot.Name = "koefVarAAZRoot";
            this.koefVarAAZRoot.Size = new System.Drawing.Size(161, 17);
            this.koefVarAAZRoot.TabIndex = 10;
            this.koefVarAAZRoot.Text = "Коэф вариации (корневой)";
            this.koefVarAAZRoot.UseVisualStyleBackColor = true;
            // 
            // AVGAAZ
            // 
            this.AVGAAZ.AutoSize = true;
            this.AVGAAZ.Location = new System.Drawing.Point(6, 42);
            this.AVGAAZ.Name = "AVGAAZ";
            this.AVGAAZ.Size = new System.Drawing.Size(119, 17);
            this.AVGAAZ.TabIndex = 0;
            this.AVGAAZ.Text = "Среднее значение";
            this.AVGAAZ.UseVisualStyleBackColor = true;
            // 
            // MatDevAAZ
            // 
            this.MatDevAAZ.AutoSize = true;
            this.MatDevAAZ.Location = new System.Drawing.Point(6, 65);
            this.MatDevAAZ.Name = "MatDevAAZ";
            this.MatDevAAZ.Size = new System.Drawing.Size(129, 17);
            this.MatDevAAZ.TabIndex = 1;
            this.MatDevAAZ.Text = "Ср. мат. отклонение";
            this.MatDevAAZ.UseVisualStyleBackColor = true;
            // 
            // koefVarAAZ
            // 
            this.koefVarAAZ.AutoSize = true;
            this.koefVarAAZ.Location = new System.Drawing.Point(6, 91);
            this.koefVarAAZ.Name = "koefVarAAZ";
            this.koefVarAAZ.Size = new System.Drawing.Size(104, 17);
            this.koefVarAAZ.TabIndex = 2;
            this.koefVarAAZ.Text = "Коэф вариации";
            this.koefVarAAZ.UseVisualStyleBackColor = true;
            // 
            // checkSumEnergy
            // 
            this.checkSumEnergy.AutoSize = true;
            this.checkSumEnergy.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkSumEnergy.Checked = true;
            this.checkSumEnergy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSumEnergy.Location = new System.Drawing.Point(6, 19);
            this.checkSumEnergy.Name = "checkSumEnergy";
            this.checkSumEnergy.Size = new System.Drawing.Size(83, 17);
            this.checkSumEnergy.TabIndex = 7;
            this.checkSumEnergy.Text = "Eсумм, Дж";
            this.checkSumEnergy.UseVisualStyleBackColor = true;
            // 
            // checkAazType
            // 
            this.checkAazType.AutoSize = true;
            this.checkAazType.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAazType.Checked = true;
            this.checkAazType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAazType.Location = new System.Drawing.Point(6, 33);
            this.checkAazType.Name = "checkAazType";
            this.checkAazType.Size = new System.Drawing.Size(84, 17);
            this.checkAazType.TabIndex = 17;
            this.checkAazType.Text = "Тип записи";
            this.checkAazType.UseVisualStyleBackColor = true;
            // 
            // checkAvgZ
            // 
            this.checkAvgZ.AutoSize = true;
            this.checkAvgZ.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAvgZ.Checked = true;
            this.checkAvgZ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAvgZ.Location = new System.Drawing.Point(6, 171);
            this.checkAvgZ.Name = "checkAvgZ";
            this.checkAvgZ.Size = new System.Drawing.Size(47, 17);
            this.checkAvgZ.TabIndex = 16;
            this.checkAvgZ.Text = "Z, м";
            this.checkAvgZ.UseVisualStyleBackColor = true;
            // 
            // checkAvgY
            // 
            this.checkAvgY.AutoSize = true;
            this.checkAvgY.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAvgY.Checked = true;
            this.checkAvgY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAvgY.Location = new System.Drawing.Point(6, 148);
            this.checkAvgY.Name = "checkAvgY";
            this.checkAvgY.Size = new System.Drawing.Size(47, 17);
            this.checkAvgY.TabIndex = 15;
            this.checkAvgY.Text = "Y, м";
            this.checkAvgY.UseVisualStyleBackColor = true;
            // 
            // checkAvgX
            // 
            this.checkAvgX.AutoSize = true;
            this.checkAvgX.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAvgX.Checked = true;
            this.checkAvgX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAvgX.Location = new System.Drawing.Point(6, 125);
            this.checkAvgX.Name = "checkAvgX";
            this.checkAvgX.Size = new System.Drawing.Size(47, 17);
            this.checkAvgX.TabIndex = 14;
            this.checkAvgX.Text = "Х, м";
            this.checkAvgX.UseVisualStyleBackColor = true;
            // 
            // checkAAZCalcTime
            // 
            this.checkAAZCalcTime.AutoSize = true;
            this.checkAAZCalcTime.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAAZCalcTime.Checked = true;
            this.checkAAZCalcTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAAZCalcTime.Location = new System.Drawing.Point(6, 102);
            this.checkAAZCalcTime.Name = "checkAAZCalcTime";
            this.checkAAZCalcTime.Size = new System.Drawing.Size(122, 17);
            this.checkAAZCalcTime.TabIndex = 13;
            this.checkAAZCalcTime.Text = "Время записи ААЗ";
            this.checkAAZCalcTime.UseVisualStyleBackColor = true;
            // 
            // checkAAZEventLastTime
            // 
            this.checkAAZEventLastTime.AutoSize = true;
            this.checkAAZEventLastTime.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAAZEventLastTime.Checked = true;
            this.checkAAZEventLastTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAAZEventLastTime.Location = new System.Drawing.Point(6, 79);
            this.checkAAZEventLastTime.Name = "checkAAZEventLastTime";
            this.checkAAZEventLastTime.Size = new System.Drawing.Size(167, 17);
            this.checkAAZEventLastTime.TabIndex = 12;
            this.checkAAZEventLastTime.Text = "Время последнего события";
            this.checkAAZEventLastTime.UseVisualStyleBackColor = true;
            // 
            // checkAAZEventFirstTime
            // 
            this.checkAAZEventFirstTime.AutoSize = true;
            this.checkAAZEventFirstTime.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkAAZEventFirstTime.Checked = true;
            this.checkAAZEventFirstTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAAZEventFirstTime.Location = new System.Drawing.Point(6, 56);
            this.checkAAZEventFirstTime.Name = "checkAAZEventFirstTime";
            this.checkAAZEventFirstTime.Size = new System.Drawing.Size(149, 17);
            this.checkAAZEventFirstTime.TabIndex = 11;
            this.checkAAZEventFirstTime.Text = "Время первого события";
            this.checkAAZEventFirstTime.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LeadingEdgeTime);
            this.groupBox3.Controls.Add(this.MARSE_Threshold);
            this.groupBox3.Controls.Add(this.MARSE);
            this.groupBox3.Controls.Add(this.Area);
            this.groupBox3.Controls.Add(this.Threshold);
            this.groupBox3.Controls.Add(this.Duration);
            this.groupBox3.Controls.Add(this.Amplitude);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(407, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(373, 257);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры импульсов";
            // 
            // LeadingEdgeTime
            // 
            this.LeadingEdgeTime.AutoSize = true;
            this.LeadingEdgeTime.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LeadingEdgeTime.Location = new System.Drawing.Point(17, 171);
            this.LeadingEdgeTime.Name = "LeadingEdgeTime";
            this.LeadingEdgeTime.Size = new System.Drawing.Size(139, 17);
            this.LeadingEdgeTime.TabIndex = 17;
            this.LeadingEdgeTime.Text = "Длительность фронта";
            this.LeadingEdgeTime.UseVisualStyleBackColor = true;
            // 
            // MARSE_Threshold
            // 
            this.MARSE_Threshold.AutoSize = true;
            this.MARSE_Threshold.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MARSE_Threshold.Location = new System.Drawing.Point(17, 148);
            this.MARSE_Threshold.Name = "MARSE_Threshold";
            this.MARSE_Threshold.Size = new System.Drawing.Size(98, 17);
            this.MARSE_Threshold.TabIndex = 16;
            this.MARSE_Threshold.Text = "MARSE/порог";
            this.MARSE_Threshold.UseVisualStyleBackColor = true;
            // 
            // MARSE
            // 
            this.MARSE.AutoSize = true;
            this.MARSE.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MARSE.Location = new System.Drawing.Point(17, 125);
            this.MARSE.Name = "MARSE";
            this.MARSE.Size = new System.Drawing.Size(64, 17);
            this.MARSE.TabIndex = 15;
            this.MARSE.Text = "MARSE";
            this.MARSE.UseVisualStyleBackColor = true;
            // 
            // Area
            // 
            this.Area.AutoSize = true;
            this.Area.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Area.Location = new System.Drawing.Point(17, 102);
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(73, 17);
            this.Area.TabIndex = 14;
            this.Area.Text = "Площадь";
            this.Area.UseVisualStyleBackColor = true;
            // 
            // Threshold
            // 
            this.Threshold.AutoSize = true;
            this.Threshold.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Threshold.Location = new System.Drawing.Point(17, 79);
            this.Threshold.Name = "Threshold";
            this.Threshold.Size = new System.Drawing.Size(57, 17);
            this.Threshold.TabIndex = 13;
            this.Threshold.Text = "Порог";
            this.Threshold.UseVisualStyleBackColor = true;
            // 
            // Duration
            // 
            this.Duration.AutoSize = true;
            this.Duration.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Duration.Location = new System.Drawing.Point(17, 56);
            this.Duration.Name = "Duration";
            this.Duration.Size = new System.Drawing.Size(99, 17);
            this.Duration.TabIndex = 12;
            this.Duration.Text = "Длительность";
            this.Duration.UseVisualStyleBackColor = true;
            // 
            // Amplitude
            // 
            this.Amplitude.AutoSize = true;
            this.Amplitude.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Amplitude.Location = new System.Drawing.Point(17, 33);
            this.Amplitude.Name = "Amplitude";
            this.Amplitude.Size = new System.Drawing.Size(81, 17);
            this.Amplitude.TabIndex = 11;
            this.Amplitude.Text = "Амплитуда";
            this.Amplitude.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.koefVarImp);
            this.groupBox4.Controls.Add(this.AVGImpRoot);
            this.groupBox4.Controls.Add(this.MatDevImp);
            this.groupBox4.Controls.Add(this.MatDevImpRoot);
            this.groupBox4.Controls.Add(this.AVGImp);
            this.groupBox4.Controls.Add(this.koefVarImpRoot);
            this.groupBox4.Location = new System.Drawing.Point(163, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 180);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Тип расчета параметров";
            // 
            // koefVarImp
            // 
            this.koefVarImp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.koefVarImp.AutoSize = true;
            this.koefVarImp.Location = new System.Drawing.Point(6, 77);
            this.koefVarImp.Name = "koefVarImp";
            this.koefVarImp.Size = new System.Drawing.Size(104, 17);
            this.koefVarImp.TabIndex = 2;
            this.koefVarImp.Text = "Коэф вариации";
            this.koefVarImp.UseVisualStyleBackColor = true;
            // 
            // AVGImpRoot
            // 
            this.AVGImpRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AVGImpRoot.AutoSize = true;
            this.AVGImpRoot.Checked = true;
            this.AVGImpRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AVGImpRoot.Location = new System.Drawing.Point(6, 100);
            this.AVGImpRoot.Name = "AVGImpRoot";
            this.AVGImpRoot.Size = new System.Drawing.Size(176, 17);
            this.AVGImpRoot.TabIndex = 3;
            this.AVGImpRoot.Text = "Среднее значение (корневое)";
            this.AVGImpRoot.UseVisualStyleBackColor = true;
            // 
            // MatDevImp
            // 
            this.MatDevImp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MatDevImp.AutoSize = true;
            this.MatDevImp.Location = new System.Drawing.Point(6, 51);
            this.MatDevImp.Name = "MatDevImp";
            this.MatDevImp.Size = new System.Drawing.Size(129, 17);
            this.MatDevImp.TabIndex = 1;
            this.MatDevImp.Text = "Ср. мат. отклонение";
            this.MatDevImp.UseVisualStyleBackColor = true;
            // 
            // MatDevImpRoot
            // 
            this.MatDevImpRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MatDevImpRoot.AutoSize = true;
            this.MatDevImpRoot.Checked = true;
            this.MatDevImpRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MatDevImpRoot.Location = new System.Drawing.Point(6, 123);
            this.MatDevImpRoot.Name = "MatDevImpRoot";
            this.MatDevImpRoot.Size = new System.Drawing.Size(186, 17);
            this.MatDevImpRoot.TabIndex = 4;
            this.MatDevImpRoot.Text = "Ср. мат. отклонение (корневое)";
            this.MatDevImpRoot.UseVisualStyleBackColor = true;
            // 
            // AVGImp
            // 
            this.AVGImp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AVGImp.AutoSize = true;
            this.AVGImp.Location = new System.Drawing.Point(6, 28);
            this.AVGImp.Name = "AVGImp";
            this.AVGImp.Size = new System.Drawing.Size(119, 17);
            this.AVGImp.TabIndex = 0;
            this.AVGImp.Text = "Среднее значение";
            this.AVGImp.UseVisualStyleBackColor = true;
            // 
            // koefVarImpRoot
            // 
            this.koefVarImpRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.koefVarImpRoot.AutoSize = true;
            this.koefVarImpRoot.Checked = true;
            this.koefVarImpRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.koefVarImpRoot.Location = new System.Drawing.Point(6, 149);
            this.koefVarImpRoot.Name = "koefVarImpRoot";
            this.koefVarImpRoot.Size = new System.Drawing.Size(161, 17);
            this.koefVarImpRoot.TabIndex = 5;
            this.koefVarImpRoot.Text = "Коэф вариации (корневой)";
            this.koefVarImpRoot.UseVisualStyleBackColor = true;
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(705, 284);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 9;
            this.selectButton.Text = "Ок";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // SelectUnitedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(791, 319);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectUnitedForm";
            this.Text = "Выбор параметров";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectUnitedForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox AVGAAZ;
        public System.Windows.Forms.CheckBox MatDevAAZ;
        public System.Windows.Forms.CheckBox koefVarAAZ;
        public System.Windows.Forms.CheckBox checkSumEnergy;
        public System.Windows.Forms.CheckBox checkAazType;
        public System.Windows.Forms.CheckBox checkAvgZ;
        public System.Windows.Forms.CheckBox checkAvgY;
        public System.Windows.Forms.CheckBox checkAvgX;
        public System.Windows.Forms.CheckBox checkAAZCalcTime;
        public System.Windows.Forms.CheckBox checkAAZEventLastTime;
        public System.Windows.Forms.CheckBox checkAAZEventFirstTime;
        public System.Windows.Forms.CheckBox AVGAAZRoot;
        public System.Windows.Forms.CheckBox MatDevAAZRoot;
        public System.Windows.Forms.CheckBox koefVarAAZRoot;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox AVGImpRoot;
        public System.Windows.Forms.CheckBox MatDevImpRoot;
        public System.Windows.Forms.CheckBox koefVarImpRoot;
        public System.Windows.Forms.CheckBox AVGImp;
        public System.Windows.Forms.CheckBox MatDevImp;
        public System.Windows.Forms.CheckBox koefVarImp;
        private System.Windows.Forms.Button selectButton;
        public System.Windows.Forms.CheckBox Amplitude;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.CheckBox Duration;
        public System.Windows.Forms.CheckBox Threshold;
        public System.Windows.Forms.CheckBox Area;
        public System.Windows.Forms.CheckBox MARSE;
        public System.Windows.Forms.CheckBox MARSE_Threshold;
        public System.Windows.Forms.CheckBox LeadingEdgeTime;
    }
}