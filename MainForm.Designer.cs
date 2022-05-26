namespace ImpHoleCalculation
{
    partial class MainForm
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
            this.dateBefore = new System.Windows.Forms.DateTimePicker();
            this.dateAfter = new System.Windows.Forms.DateTimePicker();
            this.AAZParametrs = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.excelButton = new System.Windows.Forms.Button();
            this.returnButton = new System.Windows.Forms.Button();
            this.startButtonTest = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.HoleListGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TempHoleGridView = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImpulsesGridView = new System.Windows.Forms.DataGridView();
            this.Column18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImpulseHoleGridView = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelNumbImpAll = new System.Windows.Forms.Label();
            this.freqAfter = new System.Windows.Forms.TextBox();
            this.freqBefore = new System.Windows.Forms.TextBox();
            this.freqStep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.holeComboBox = new System.Windows.Forms.ComboBox();
            this.OneHolecheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.autosaveCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.daysRadioButton2 = new System.Windows.Forms.RadioButton();
            this.hoursRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HoleListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempHoleGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulsesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateBefore
            // 
            this.dateBefore.CustomFormat = "yyyy-MM-dd HH:mm:ss ";
            this.dateBefore.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateBefore.Location = new System.Drawing.Point(45, 38);
            this.dateBefore.Name = "dateBefore";
            this.dateBefore.ShowUpDown = true;
            this.dateBefore.Size = new System.Drawing.Size(132, 20);
            this.dateBefore.TabIndex = 5;
            this.dateBefore.Value = new System.DateTime(2011, 11, 21, 0, 0, 0, 0);
            // 
            // dateAfter
            // 
            this.dateAfter.CustomFormat = "yyyy-MM-dd HH:mm:ss ";
            this.dateAfter.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAfter.Location = new System.Drawing.Point(45, 64);
            this.dateAfter.Name = "dateAfter";
            this.dateAfter.ShowUpDown = true;
            this.dateAfter.Size = new System.Drawing.Size(132, 20);
            this.dateAfter.TabIndex = 6;
            this.dateAfter.Value = new System.DateTime(2011, 11, 22, 0, 0, 0, 0);
            // 
            // AAZParametrs
            // 
            this.AAZParametrs.Location = new System.Drawing.Point(9, 54);
            this.AAZParametrs.Name = "AAZParametrs";
            this.AAZParametrs.Size = new System.Drawing.Size(75, 40);
            this.AAZParametrs.TabIndex = 7;
            this.AAZParametrs.Text = "Выбрать параметры";
            this.AAZParametrs.UseVisualStyleBackColor = true;
            this.AAZParametrs.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateCheckBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dateBefore);
            this.groupBox1.Controls.Add(this.dateAfter);
            this.groupBox1.Location = new System.Drawing.Point(9, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 100);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Время";
            // 
            // dateCheckBox
            // 
            this.dateCheckBox.AutoSize = true;
            this.dateCheckBox.Location = new System.Drawing.Point(17, 18);
            this.dateCheckBox.Name = "dateCheckBox";
            this.dateCheckBox.Size = new System.Drawing.Size(131, 17);
            this.dateCheckBox.TabIndex = 9;
            this.dateCheckBox.Text = "Вывести по всей БД";
            this.dateCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "До";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "От";
            // 
            // excelButton
            // 
            this.excelButton.Location = new System.Drawing.Point(109, 471);
            this.excelButton.Name = "excelButton";
            this.excelButton.Size = new System.Drawing.Size(75, 38);
            this.excelButton.TabIndex = 13;
            this.excelButton.Text = "Сохранить в Excel";
            this.excelButton.UseVisualStyleBackColor = true;
            this.excelButton.Visible = false;
            this.excelButton.Click += new System.EventHandler(this.ExcelButton_Click);
            // 
            // returnButton
            // 
            this.returnButton.Location = new System.Drawing.Point(26, 12);
            this.returnButton.Name = "returnButton";
            this.returnButton.Size = new System.Drawing.Size(139, 36);
            this.returnButton.TabIndex = 14;
            this.returnButton.Text = "Изменение параметров\r\nподключения";
            this.returnButton.UseVisualStyleBackColor = true;
            this.returnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // startButtonTest
            // 
            this.startButtonTest.Location = new System.Drawing.Point(12, 479);
            this.startButtonTest.Name = "startButtonTest";
            this.startButtonTest.Size = new System.Drawing.Size(75, 23);
            this.startButtonTest.TabIndex = 24;
            this.startButtonTest.Text = "Ок";
            this.startButtonTest.UseVisualStyleBackColor = true;
            this.startButtonTest.Click += new System.EventHandler(this.Test_Button_Click_1);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.HoleListGridView);
            this.tabPage1.Controls.Add(this.TempHoleGridView);
            this.tabPage1.Controls.Add(this.ImpulsesGridView);
            this.tabPage1.Controls.Add(this.ImpulseHoleGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1001, 599);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Скважины";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // HoleListGridView
            // 
            this.HoleListGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HoleListGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HoleListGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column12,
            this.Column14,
            this.Column15,
            this.Column16,
            this.Column17,
            this.Column19,
            this.Column20});
            this.HoleListGridView.Location = new System.Drawing.Point(6, 6);
            this.HoleListGridView.Name = "HoleListGridView";
            this.HoleListGridView.Size = new System.Drawing.Size(620, 365);
            this.HoleListGridView.TabIndex = 44;
            this.HoleListGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HoleListGridView_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "№";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Имя скважины";
            this.Column2.Name = "Column2";
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Количество импульсов";
            this.Column12.Name = "Column12";
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Время \"начало\"";
            this.Column14.Name = "Column14";
            // 
            // Column15
            // 
            this.Column15.HeaderText = "Время \"окончание\"";
            this.Column15.Name = "Column15";
            // 
            // Column16
            // 
            this.Column16.HeaderText = "X (м)";
            this.Column16.Name = "Column16";
            // 
            // Column17
            // 
            this.Column17.HeaderText = "Y (м)";
            this.Column17.Name = "Column17";
            // 
            // Column19
            // 
            this.Column19.HeaderText = "Z (м)";
            this.Column19.Name = "Column19";
            // 
            // Column20
            // 
            this.Column20.HeaderText = "Описание";
            this.Column20.Name = "Column20";
            // 
            // TempHoleGridView
            // 
            this.TempHoleGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TempHoleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TempHoleGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column8,
            this.Column4,
            this.Column9,
            this.Column10,
            this.Column11});
            this.TempHoleGridView.Location = new System.Drawing.Point(924, 581);
            this.TempHoleGridView.Name = "TempHoleGridView";
            this.TempHoleGridView.Size = new System.Drawing.Size(74, 18);
            this.TempHoleGridView.TabIndex = 4;
            this.TempHoleGridView.Visible = false;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "HoleID";
            this.Column5.Name = "Column5";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "HoleName";
            this.Column8.Name = "Column8";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Sensor ID";
            this.Column4.Name = "Column4";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "HWID";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "DateBefore";
            this.Column10.Name = "Column10";
            // 
            // Column11
            // 
            this.Column11.HeaderText = "DateAfter";
            this.Column11.Name = "Column11";
            // 
            // ImpulsesGridView
            // 
            this.ImpulsesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImpulsesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImpulsesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column18,
            this.Column24,
            this.Column13,
            this.Column34,
            this.Column3});
            this.ImpulsesGridView.Location = new System.Drawing.Point(6, 377);
            this.ImpulsesGridView.Name = "ImpulsesGridView";
            this.ImpulsesGridView.Size = new System.Drawing.Size(608, 216);
            this.ImpulsesGridView.TabIndex = 3;
            // 
            // Column18
            // 
            this.Column18.HeaderText = "№";
            this.Column18.Name = "Column18";
            // 
            // Column24
            // 
            this.Column24.HeaderText = "ID";
            this.Column24.Name = "Column24";
            // 
            // Column13
            // 
            this.Column13.HeaderText = "HWID";
            this.Column13.Name = "Column13";
            // 
            // Column34
            // 
            this.Column34.HeaderText = "Дата (импульс)";
            this.Column34.Name = "Column34";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Имя скважины";
            this.Column3.Name = "Column3";
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
            this.ImpulseHoleGridView.Location = new System.Drawing.Point(634, 306);
            this.ImpulseHoleGridView.Name = "ImpulseHoleGridView";
            this.ImpulseHoleGridView.Size = new System.Drawing.Size(361, 183);
            this.ImpulseHoleGridView.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(200, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1009, 625);
            this.tabControl1.TabIndex = 15;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(5, 538);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(120, 24);
            this.progressBar.TabIndex = 37;
            this.progressBar.Visible = false;
            // 
            // labelNumbImpAll
            // 
            this.labelNumbImpAll.AutoSize = true;
            this.labelNumbImpAll.Location = new System.Drawing.Point(2, 565);
            this.labelNumbImpAll.Name = "labelNumbImpAll";
            this.labelNumbImpAll.Size = new System.Drawing.Size(115, 13);
            this.labelNumbImpAll.TabIndex = 39;
            this.labelNumbImpAll.Text = "                                    ";
            // 
            // freqAfter
            // 
            this.freqAfter.Location = new System.Drawing.Point(93, 16);
            this.freqAfter.Name = "freqAfter";
            this.freqAfter.Size = new System.Drawing.Size(60, 20);
            this.freqAfter.TabIndex = 26;
            this.freqAfter.Text = "2";
            // 
            // freqBefore
            // 
            this.freqBefore.Location = new System.Drawing.Point(17, 16);
            this.freqBefore.Name = "freqBefore";
            this.freqBefore.Size = new System.Drawing.Size(60, 20);
            this.freqBefore.TabIndex = 25;
            this.freqBefore.Text = "1";
            // 
            // freqStep
            // 
            this.freqStep.Location = new System.Drawing.Point(57, 42);
            this.freqStep.Name = "freqStep";
            this.freqStep.Size = new System.Drawing.Size(30, 20);
            this.freqStep.TabIndex = 27;
            this.freqStep.Text = "0.1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(77, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Шаг:  ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.freqStep);
            this.groupBox4.Controls.Add(this.freqBefore);
            this.groupBox4.Controls.Add(this.freqAfter);
            this.groupBox4.Location = new System.Drawing.Point(5, 565);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(191, 72);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Частота";
            this.groupBox4.Visible = false;
            // 
            // holeComboBox
            // 
            this.holeComboBox.FormattingEnabled = true;
            this.holeComboBox.Location = new System.Drawing.Point(77, 60);
            this.holeComboBox.Name = "holeComboBox";
            this.holeComboBox.Size = new System.Drawing.Size(97, 21);
            this.holeComboBox.TabIndex = 40;
            // 
            // OneHolecheckBox
            // 
            this.OneHolecheckBox.AutoSize = true;
            this.OneHolecheckBox.Checked = true;
            this.OneHolecheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OneHolecheckBox.Location = new System.Drawing.Point(13, 19);
            this.OneHolecheckBox.Name = "OneHolecheckBox";
            this.OneHolecheckBox.Size = new System.Drawing.Size(139, 30);
            this.OneHolecheckBox.TabIndex = 41;
            this.OneHolecheckBox.Text = "Подсчитать импульсы\r\n из одной скважины";
            this.OneHolecheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Скважина:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.autosaveCheckBox);
            this.groupBox2.Controls.Add(this.OneHolecheckBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.holeComboBox);
            this.groupBox2.Location = new System.Drawing.Point(10, 215);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 226);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры";
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
            this.Column23.HeaderText = "Количество импульсов";
            this.Column23.Name = "Column23";
            // 
            // autosaveCheckBox
            // 
            this.autosaveCheckBox.AutoSize = true;
            this.autosaveCheckBox.Location = new System.Drawing.Point(16, 102);
            this.autosaveCheckBox.Name = "autosaveCheckBox";
            this.autosaveCheckBox.Size = new System.Drawing.Size(119, 30);
            this.autosaveCheckBox.TabIndex = 45;
            this.autosaveCheckBox.Text = "сохранить данные\r\n автомотически";
            this.autosaveCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.daysRadioButton2);
            this.groupBox3.Controls.Add(this.hoursRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 77);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Группировка данных";
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1221, 638);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelNumbImpAll);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.excelButton);
            this.Controls.Add(this.startButtonTest);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.returnButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AAZParametrs);
            this.Name = "MainForm";
            this.Text = "Выборка скважин";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.AllClustersForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HoleListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempHoleGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulsesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DateTimePicker dateBefore;
        public System.Windows.Forms.DateTimePicker dateAfter;
        private System.Windows.Forms.Button AAZParametrs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button excelButton;
        private System.Windows.Forms.Button returnButton;
        public System.Windows.Forms.Button startButtonTest;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView ImpulsesGridView;
        private System.Windows.Forms.DataGridView ImpulseHoleGridView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox dateCheckBox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelNumbImpAll;
        private System.Windows.Forms.TextBox freqAfter;
        private System.Windows.Forms.TextBox freqBefore;
        private System.Windows.Forms.TextBox freqStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView TempHoleGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column18;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column24;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column34;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridView HoleListGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column17;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column19;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column20;
        private System.Windows.Forms.ComboBox holeComboBox;
        private System.Windows.Forms.CheckBox OneHolecheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column23;
        private System.Windows.Forms.CheckBox autosaveCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton daysRadioButton2;
        private System.Windows.Forms.RadioButton hoursRadioButton;
    }
}