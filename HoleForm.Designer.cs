namespace ImpHoleCalculation
{
    partial class HoleForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelHole = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ImpulseHoleGridView = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.impulseChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hoursRadioButton = new System.Windows.Forms.RadioButton();
            this.daysRadioButton2 = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.impulseChart)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHole
            // 
            this.labelHole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHole.AutoSize = true;
            this.labelHole.Location = new System.Drawing.Point(24, 9);
            this.labelHole.Name = "labelHole";
            this.labelHole.Size = new System.Drawing.Size(123, 13);
            this.labelHole.TabIndex = 1;
            this.labelHole.Text = "Выбранная скважина: ";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(778, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Сохранить в Excel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ImpulseHoleGridView
            // 
            this.ImpulseHoleGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImpulseHoleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImpulseHoleGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column23});
            this.ImpulseHoleGridView.Location = new System.Drawing.Point(12, 118);
            this.ImpulseHoleGridView.Name = "ImpulseHoleGridView";
            this.ImpulseHoleGridView.Size = new System.Drawing.Size(333, 320);
            this.ImpulseHoleGridView.TabIndex = 4;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "№";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Дата";
            this.Column7.Name = "Column7";
            // 
            // Column23
            // 
            this.Column23.HeaderText = "Количество событий";
            this.Column23.Name = "Column23";
            // 
            // impulseChart
            // 
            chartArea6.Name = "ChartArea1";
            this.impulseChart.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.impulseChart.Legends.Add(legend6);
            this.impulseChart.Location = new System.Drawing.Point(351, 35);
            this.impulseChart.Name = "impulseChart";
            series6.ChartArea = "ChartArea1";
            series6.IsVisibleInLegend = false;
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.impulseChart.Series.Add(series6);
            this.impulseChart.Size = new System.Drawing.Size(562, 403);
            this.impulseChart.TabIndex = 5;
            this.impulseChart.Text = "chart1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.daysRadioButton2);
            this.groupBox1.Controls.Add(this.hoursRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(27, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 77);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Группировка";
            // 
            // hoursRadioButton
            // 
            this.hoursRadioButton.AutoSize = true;
            this.hoursRadioButton.Checked = true;
            this.hoursRadioButton.Location = new System.Drawing.Point(7, 20);
            this.hoursRadioButton.Name = "hoursRadioButton";
            this.hoursRadioButton.Size = new System.Drawing.Size(71, 17);
            this.hoursRadioButton.TabIndex = 0;
            this.hoursRadioButton.TabStop = true;
            this.hoursRadioButton.Text = "по часам";
            this.hoursRadioButton.UseVisualStyleBackColor = true;
            // 
            // daysRadioButton2
            // 
            this.daysRadioButton2.AutoSize = true;
            this.daysRadioButton2.Location = new System.Drawing.Point(7, 44);
            this.daysRadioButton2.Name = "daysRadioButton2";
            this.daysRadioButton2.Size = new System.Drawing.Size(66, 17);
            this.daysRadioButton2.TabIndex = 1;
            this.daysRadioButton2.Text = "по дням";
            this.daysRadioButton2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(219, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Вычислить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(538, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(153, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Сохранить график";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // HoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.impulseChart);
            this.Controls.Add(this.ImpulseHoleGridView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelHole);
            this.Name = "HoleForm";
            this.Text = "Выбранная скважина";
            this.Load += new System.EventHandler(this.ClusterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.impulseChart)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelHole;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView ImpulseHoleGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column23;
        private System.Windows.Forms.DataVisualization.Charting.Chart impulseChart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton daysRadioButton2;
        private System.Windows.Forms.RadioButton hoursRadioButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}