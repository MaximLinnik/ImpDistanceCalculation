namespace ImpDistanceCalculation
{
    partial class CoordinatesForm
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.locationZ = new System.Windows.Forms.TextBox();
            this.locationY = new System.Windows.Forms.TextBox();
            this.locationX = new System.Windows.Forms.TextBox();
            this.startButtonTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(247, 52);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 17);
            this.label8.TabIndex = 68;
            this.label8.Text = "Z";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(170, 52);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 17);
            this.label7.TabIndex = 67;
            this.label7.Text = "Y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(77, 52);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 17);
            this.label6.TabIndex = 66;
            this.label6.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(93, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 17);
            this.label5.TabIndex = 65;
            this.label5.Text = "Ввеедите координату";
            // 
            // locationZ
            // 
            this.locationZ.Location = new System.Drawing.Point(221, 72);
            this.locationZ.Name = "locationZ";
            this.locationZ.Size = new System.Drawing.Size(67, 22);
            this.locationZ.TabIndex = 64;
            this.locationZ.Text = "453,299";
            // 
            // locationY
            // 
            this.locationY.Location = new System.Drawing.Point(138, 72);
            this.locationY.Name = "locationY";
            this.locationY.Size = new System.Drawing.Size(67, 22);
            this.locationY.TabIndex = 63;
            this.locationY.Text = "329,781";
            // 
            // locationX
            // 
            this.locationX.Location = new System.Drawing.Point(57, 72);
            this.locationX.Name = "locationX";
            this.locationX.Size = new System.Drawing.Size(67, 22);
            this.locationX.TabIndex = 62;
            this.locationX.Text = "1147,380";
            // 
            // startButtonTest
            // 
            this.startButtonTest.Location = new System.Drawing.Point(118, 119);
            this.startButtonTest.Margin = new System.Windows.Forms.Padding(4);
            this.startButtonTest.Name = "startButtonTest";
            this.startButtonTest.Size = new System.Drawing.Size(100, 28);
            this.startButtonTest.TabIndex = 61;
            this.startButtonTest.Text = "Ок";
            this.startButtonTest.UseVisualStyleBackColor = true;
            this.startButtonTest.Click += new System.EventHandler(this.StartButtonTest_Click);
            // 
            // CoordinatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 178);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.locationZ);
            this.Controls.Add(this.locationY);
            this.Controls.Add(this.locationX);
            this.Controls.Add(this.startButtonTest);
            this.Name = "CoordinatesForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CoordinatesForm_FormClosed);
            this.Load += new System.EventHandler(this.CoordinatesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox locationZ;
        private System.Windows.Forms.TextBox locationY;
        private System.Windows.Forms.TextBox locationX;
        public System.Windows.Forms.Button startButtonTest;
    }
}