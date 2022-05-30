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
            this.dateBeforeText = new System.Windows.Forms.DateTimePicker();
            this.dateAfterText = new System.Windows.Forms.DateTimePicker();
            this.AAZParametrs = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.excelButton = new System.Windows.Forms.Button();
            this.returnButton = new System.Windows.Forms.Button();
            this.startButtonTest = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ImpulseHoleGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.labelNumbImpAll = new System.Windows.Forms.Label();
            this.holeComboBox = new System.Windows.Forms.ComboBox();
            this.OneHolecheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.doubleExcelCheckBox = new System.Windows.Forms.CheckBox();
            this.daysRadioButton = new System.Windows.Forms.RadioButton();
            this.autosaveCheckBox = new System.Windows.Forms.CheckBox();
            this.hoursRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HoleListGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempHoleGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulsesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateBeforeText
            // 
            this.dateBeforeText.CustomFormat = "yyyy-MM-dd HH:mm:ss ";
            this.dateBeforeText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateBeforeText.Location = new System.Drawing.Point(45, 38);
            this.dateBeforeText.Name = "dateBeforeText";
            this.dateBeforeText.ShowUpDown = true;
            this.dateBeforeText.Size = new System.Drawing.Size(132, 20);
            this.dateBeforeText.TabIndex = 5;
            this.dateBeforeText.Value = new System.DateTime(2011, 11, 21, 0, 0, 0, 0);
            // 
            // dateAfterText
            // 
            this.dateAfterText.CustomFormat = "yyyy-MM-dd HH:mm:ss ";
            this.dateAfterText.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAfterText.Location = new System.Drawing.Point(45, 64);
            this.dateAfterText.Name = "dateAfterText";
            this.dateAfterText.ShowUpDown = true;
            this.dateAfterText.Size = new System.Drawing.Size(132, 20);
            this.dateAfterText.TabIndex = 6;
            this.dateAfterText.Value = new System.DateTime(2011, 11, 22, 0, 0, 0, 0);
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
            this.groupBox1.Controls.Add(this.dateBeforeText);
            this.groupBox1.Controls.Add(this.dateAfterText);
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
            this.tabPage1.Controls.Add(this.ImpulseHoleGridView2);
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
            // ImpulseHoleGridView2
            // 
            this.ImpulseHoleGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImpulseHoleGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImpulseHoleGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.ImpulseHoleGridView2.Location = new System.Drawing.Point(763, 573);
            this.ImpulseHoleGridView2.Name = "ImpulseHoleGridView2";
            this.ImpulseHoleGridView2.Size = new System.Drawing.Size(98, 23);
            this.ImpulseHoleGridView2.TabIndex = 45;
            this.ImpulseHoleGridView2.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "№";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Дата";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Количество импульсов";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
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
            this.HoleListGridView.Size = new System.Drawing.Size(989, 561);
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
            this.TempHoleGridView.Location = new System.Drawing.Point(634, 575);
            this.TempHoleGridView.Name = "TempHoleGridView";
            this.TempHoleGridView.Size = new System.Drawing.Size(48, 17);
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
            this.ImpulsesGridView.Location = new System.Drawing.Point(6, 583);
            this.ImpulsesGridView.Name = "ImpulsesGridView";
            this.ImpulsesGridView.Size = new System.Drawing.Size(608, 10);
            this.ImpulsesGridView.TabIndex = 3;
            this.ImpulsesGridView.Visible = false;
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
            this.ImpulseHoleGridView.Location = new System.Drawing.Point(688, 573);
            this.ImpulseHoleGridView.Name = "ImpulseHoleGridView";
            this.ImpulseHoleGridView.Size = new System.Drawing.Size(69, 23);
            this.ImpulseHoleGridView.TabIndex = 2;
            this.ImpulseHoleGridView.Visible = false;
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
            // labelNumbImpAll
            // 
            this.labelNumbImpAll.AutoSize = true;
            this.labelNumbImpAll.Location = new System.Drawing.Point(2, 565);
            this.labelNumbImpAll.Name = "labelNumbImpAll";
            this.labelNumbImpAll.Size = new System.Drawing.Size(115, 13);
            this.labelNumbImpAll.TabIndex = 39;
            this.labelNumbImpAll.Text = "                                    ";
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
            this.groupBox2.Controls.Add(this.OneHolecheckBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.holeComboBox);
            this.groupBox2.Location = new System.Drawing.Point(10, 215);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 250);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.doubleExcelCheckBox);
            this.groupBox3.Controls.Add(this.daysRadioButton);
            this.groupBox3.Controls.Add(this.autosaveCheckBox);
            this.groupBox3.Controls.Add(this.hoursRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 101);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 143);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Группировка данных";
            // 
            // doubleExcelCheckBox
            // 
            this.doubleExcelCheckBox.AutoSize = true;
            this.doubleExcelCheckBox.Location = new System.Drawing.Point(1, 59);
            this.doubleExcelCheckBox.Name = "doubleExcelCheckBox";
            this.doubleExcelCheckBox.Size = new System.Drawing.Size(134, 17);
            this.doubleExcelCheckBox.TabIndex = 46;
            this.doubleExcelCheckBox.Text = "сохранить оба файла";
            this.doubleExcelCheckBox.UseVisualStyleBackColor = true;
            // 
            // daysRadioButton
            // 
            this.daysRadioButton.AutoSize = true;
            this.daysRadioButton.Location = new System.Drawing.Point(4, 119);
            this.daysRadioButton.Name = "daysRadioButton";
            this.daysRadioButton.Size = new System.Drawing.Size(66, 17);
            this.daysRadioButton.TabIndex = 1;
            this.daysRadioButton.Text = "по дням";
            this.daysRadioButton.UseVisualStyleBackColor = true;
            // 
            // autosaveCheckBox
            // 
            this.autosaveCheckBox.AutoSize = true;
            this.autosaveCheckBox.Location = new System.Drawing.Point(1, 24);
            this.autosaveCheckBox.Name = "autosaveCheckBox";
            this.autosaveCheckBox.Size = new System.Drawing.Size(119, 30);
            this.autosaveCheckBox.TabIndex = 45;
            this.autosaveCheckBox.Text = "сохранить данные\r\n автоматически";
            this.autosaveCheckBox.UseVisualStyleBackColor = true;
            // 
            // hoursRadioButton
            // 
            this.hoursRadioButton.AutoSize = true;
            this.hoursRadioButton.Checked = true;
            this.hoursRadioButton.Location = new System.Drawing.Point(4, 95);
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
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HoleListGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TempHoleGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulsesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseHoleGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DateTimePicker dateBeforeText;
        public System.Windows.Forms.DateTimePicker dateAfterText;
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
        private System.Windows.Forms.Label labelNumbImpAll;
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
        private System.Windows.Forms.RadioButton daysRadioButton;
        private System.Windows.Forms.RadioButton hoursRadioButton;
        private System.Windows.Forms.DataGridView ImpulseHoleGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.CheckBox doubleExcelCheckBox;
    }
}