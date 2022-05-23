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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelHole = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ImpulseHoleGridView = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.impulseChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.impulseChart)).BeginInit();
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
            this.ImpulseHoleGridView.Location = new System.Drawing.Point(12, 35);
            this.ImpulseHoleGridView.Name = "ImpulseHoleGridView";
            this.ImpulseHoleGridView.Size = new System.Drawing.Size(333, 403);
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
            chartArea1.Name = "ChartArea1";
            this.impulseChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.impulseChart.Legends.Add(legend1);
            this.impulseChart.Location = new System.Drawing.Point(351, 35);
            this.impulseChart.Name = "impulseChart";
            series1.ChartArea = "ChartArea1";
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.impulseChart.Series.Add(series1);
            this.impulseChart.Size = new System.Drawing.Size(562, 403);
            this.impulseChart.TabIndex = 5;
            this.impulseChart.Text = "chart1";
            // 
            // HoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 450);
            this.Controls.Add(this.impulseChart);
            this.Controls.Add(this.ImpulseHoleGridView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelHole);
            this.Name = "HoleForm";
            this.Text = "Выбранная скважина";
            this.Load += new System.EventHandler(this.ClusterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.impulseChart)).EndInit();
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
    }
}