namespace ImpHoleCalculation
{
    partial class FormImpulse
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
            this.ImpulsesDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chooseColumnButton = new System.Windows.Forms.Button();
            this.numAAZ = new System.Windows.Forms.Label();
            this.typeAAZ = new System.Windows.Forms.Label();
            this.excelButton = new System.Windows.Forms.Button();
            this.returnButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulsesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ImpulsesDataGridView
            // 
            this.ImpulsesDataGridView.AllowUserToOrderColumns = true;
            this.ImpulsesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImpulsesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImpulsesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.ImpulsesDataGridView.Location = new System.Drawing.Point(123, 12);
            this.ImpulsesDataGridView.Name = "ImpulsesDataGridView";
            this.ImpulsesDataGridView.Size = new System.Drawing.Size(1025, 635);
            this.ImpulsesDataGridView.TabIndex = 0;
            this.ImpulsesDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ImpulsesDataGridView_CellContentClick);
            this.ImpulsesDataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.ImpulsesDataGridView_SortCompare_1);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "HWID";
            this.Column1.Name = "Column1";
            // 
            // chooseColumnButton
            // 
            this.chooseColumnButton.Location = new System.Drawing.Point(12, 78);
            this.chooseColumnButton.Name = "chooseColumnButton";
            this.chooseColumnButton.Size = new System.Drawing.Size(75, 37);
            this.chooseColumnButton.TabIndex = 1;
            this.chooseColumnButton.Text = "Выбрать\r\nпараметры";
            this.chooseColumnButton.UseVisualStyleBackColor = true;
            this.chooseColumnButton.Click += new System.EventHandler(this.ChooseColumnButton_Click);
            // 
            // numAAZ
            // 
            this.numAAZ.AutoSize = true;
            this.numAAZ.Location = new System.Drawing.Point(9, 127);
            this.numAAZ.Name = "numAAZ";
            this.numAAZ.Size = new System.Drawing.Size(71, 13);
            this.numAAZ.TabIndex = 11;
            this.numAAZ.Text = "Номер ААЗ: ";
            // 
            // typeAAZ
            // 
            this.typeAAZ.AutoSize = true;
            this.typeAAZ.Location = new System.Drawing.Point(9, 154);
            this.typeAAZ.Name = "typeAAZ";
            this.typeAAZ.Size = new System.Drawing.Size(56, 13);
            this.typeAAZ.TabIndex = 12;
            this.typeAAZ.Text = "Тип ААЗ: ";
            // 
            // excelButton
            // 
            this.excelButton.Location = new System.Drawing.Point(5, 380);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(75, 38);
            this.excelButton.TabIndex = 13;
            this.excelButton.Text = "Cохранить\r\n в Excel";
            this.excelButton.UseVisualStyleBackColor = true;
            this.excelButton.Click += new System.EventHandler(this.ExcelButton_Click);
            // 
            // returnButton
            // 
            this.returnButton.Location = new System.Drawing.Point(5, 351);
            this.returnButton.Name = "returnButton";
            this.returnButton.Size = new System.Drawing.Size(75, 23);
            this.returnButton.TabIndex = 14;
            this.returnButton.Text = "Вернуться";
            this.returnButton.UseVisualStyleBackColor = true;
            this.returnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // FormImpulse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1160, 659);
            this.Controls.Add(this.returnButton);
            this.Controls.Add(this.excelButton);
            this.Controls.Add(this.typeAAZ);
            this.Controls.Add(this.numAAZ);
            this.Controls.Add(this.chooseColumnButton);
            this.Controls.Add(this.ImpulsesDataGridView);
            this.Name = "FormImpulse";
            this.Text = "Параметры импульсов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.closed);
            this.Load += new System.EventHandler(this.FormImpulse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImpulsesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ImpulsesDataGridView;
        private System.Windows.Forms.Button chooseColumnButton;
        private System.Windows.Forms.Label numAAZ;
        private System.Windows.Forms.Label typeAAZ;
        private System.Windows.Forms.Button excelButton;
        private System.Windows.Forms.Button returnButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}