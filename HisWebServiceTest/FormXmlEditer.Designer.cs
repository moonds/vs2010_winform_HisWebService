namespace HisWebServiceTest
{
    partial class FormXmlEditer
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectOther = new System.Windows.Forms.Button();
            this.btnDelSelect = new System.Windows.Forms.Button();
            this.lblMethodName = new System.Windows.Forms.Label();
            this.btnSaveInArgs = new System.Windows.Forms.Button();
            this.btnSaveOutArgs = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsrslblNotice = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存xmldoc对象ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加新参数方法ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.to1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.from1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defaultvalue1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mode1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txth2 = new System.Windows.Forms.TextBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtdescription = new System.Windows.Forms.TextBox();
            this.txtname = new System.Windows.Forms.TextBox();
            this.txtcall = new System.Windows.Forms.TextBox();
            this.txtlog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveNewFunc = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblNotice = new System.Windows.Forms.Label();
            this.txtClearOutput = new System.Windows.Forms.Button();
            this.txtClearInput = new System.Windows.Forms.Button();
            this.txtWSAddress = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOutPut = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.lstHistoryList = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenDialog = new System.Windows.Forms.ToolStripMenuItem();
            this.DelFile = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(148, 6);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(265, 26);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Adapter.His.xml文件拖拽至此";
            this.textBox1.Visible = false;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.AllowDrop = true;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(10, 383);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(263, 821);
            this.checkedListBox1.TabIndex = 6;
            this.checkedListBox1.Click += new System.EventHandler(this.checkedListBox1_Click);
            this.checkedListBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.checkedListBox1_DragDrop);
            this.checkedListBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.checkedListBox1_DragEnter);
            this.checkedListBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkedListBox1_KeyDown);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(10, 341);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(77, 38);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectOther
            // 
            this.btnSelectOther.Location = new System.Drawing.Point(90, 341);
            this.btnSelectOther.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectOther.Name = "btnSelectOther";
            this.btnSelectOther.Size = new System.Drawing.Size(74, 38);
            this.btnSelectOther.TabIndex = 3;
            this.btnSelectOther.Text = "反选";
            this.btnSelectOther.UseVisualStyleBackColor = true;
            this.btnSelectOther.Click += new System.EventHandler(this.btnSelectOther_Click);
            // 
            // btnDelSelect
            // 
            this.btnDelSelect.Location = new System.Drawing.Point(170, 341);
            this.btnDelSelect.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelSelect.Name = "btnDelSelect";
            this.btnDelSelect.Size = new System.Drawing.Size(104, 38);
            this.btnDelSelect.TabIndex = 14;
            this.btnDelSelect.Text = "删除选中";
            this.btnDelSelect.UseVisualStyleBackColor = true;
            this.btnDelSelect.Click += new System.EventHandler(this.btnDelSelect_Click);
            // 
            // lblMethodName
            // 
            this.lblMethodName.AutoSize = true;
            this.lblMethodName.Font = new System.Drawing.Font("Arial Narrow", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethodName.Location = new System.Drawing.Point(11, 42);
            this.lblMethodName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMethodName.Name = "lblMethodName";
            this.lblMethodName.Size = new System.Drawing.Size(0, 35);
            this.lblMethodName.TabIndex = 1;
            this.lblMethodName.Visible = false;
            // 
            // btnSaveInArgs
            // 
            this.btnSaveInArgs.Location = new System.Drawing.Point(8, 150);
            this.btnSaveInArgs.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveInArgs.Name = "btnSaveInArgs";
            this.btnSaveInArgs.Size = new System.Drawing.Size(36, 115);
            this.btnSaveInArgs.TabIndex = 26;
            this.btnSaveInArgs.Text = "&I保存入参";
            this.btnSaveInArgs.UseVisualStyleBackColor = true;
            this.btnSaveInArgs.Click += new System.EventHandler(this.btnSaveInArgs_Click);
            // 
            // btnSaveOutArgs
            // 
            this.btnSaveOutArgs.Location = new System.Drawing.Point(8, 292);
            this.btnSaveOutArgs.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveOutArgs.Name = "btnSaveOutArgs";
            this.btnSaveOutArgs.Size = new System.Drawing.Size(36, 115);
            this.btnSaveOutArgs.TabIndex = 27;
            this.btnSaveOutArgs.Text = "&O保存出参";
            this.btnSaveOutArgs.UseVisualStyleBackColor = true;
            this.btnSaveOutArgs.Click += new System.EventHandler(this.btnSaveOutArgs_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsrslblNotice});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1232);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1456, 22);
            this.statusStrip1.TabIndex = 30;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsrslblNotice
            // 
            this.tsrslblNotice.Name = "tsrslblNotice";
            this.tsrslblNotice.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.参数ToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(1456, 32);
            this.menuStrip2.TabIndex = 0;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重新加载ToolStripMenuItem,
            this.导出ToolStripMenuItem,
            this.保存xmldoc对象ToolStripMenuItem,
            this.查找ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 重新加载ToolStripMenuItem
            // 
            this.重新加载ToolStripMenuItem.Name = "重新加载ToolStripMenuItem";
            this.重新加载ToolStripMenuItem.Size = new System.Drawing.Size(215, 28);
            this.重新加载ToolStripMenuItem.Text = "重新加载";
            this.重新加载ToolStripMenuItem.Click += new System.EventHandler(this.重新加载ToolStripMenuItem_Click);
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(215, 28);
            this.导出ToolStripMenuItem.Text = "导出";
            this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
            // 
            // 保存xmldoc对象ToolStripMenuItem
            // 
            this.保存xmldoc对象ToolStripMenuItem.Name = "保存xmldoc对象ToolStripMenuItem";
            this.保存xmldoc对象ToolStripMenuItem.Size = new System.Drawing.Size(215, 28);
            this.保存xmldoc对象ToolStripMenuItem.Text = "保存xmldoc对象";
            this.保存xmldoc对象ToolStripMenuItem.Click += new System.EventHandler(this.保存xmldoc对象ToolStripMenuItem_Click);
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(215, 28);
            this.查找ToolStripMenuItem.Text = "查找";
            // 
            // 参数ToolStripMenuItem
            // 
            this.参数ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加新参数方法ToolStripMenuItem});
            this.参数ToolStripMenuItem.Name = "参数ToolStripMenuItem";
            this.参数ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
            this.参数ToolStripMenuItem.Text = "参数";
            // 
            // 添加新参数方法ToolStripMenuItem
            // 
            this.添加新参数方法ToolStripMenuItem.Name = "添加新参数方法ToolStripMenuItem";
            this.添加新参数方法ToolStripMenuItem.Size = new System.Drawing.Size(206, 28);
            this.添加新参数方法ToolStripMenuItem.Text = "添加新参数方法";
            this.添加新参数方法ToolStripMenuItem.Click += new System.EventHandler(this.添加新参数方法ToolStripMenuItem_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowDrop = true;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.to1,
            this.from1,
            this.description1,
            this.defaultvalue1,
            this.type1,
            this.mode1});
            this.dataGridView2.Location = new System.Drawing.Point(49, 616);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 33;
            this.dataGridView2.Size = new System.Drawing.Size(1078, 515);
            this.dataGridView2.TabIndex = 33;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            this.dataGridView2.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView2_DragDrop);
            this.dataGridView2.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView2_DragEnter);
            this.dataGridView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseDown);
            // 
            // to1
            // 
            this.to1.DataPropertyName = "to";
            this.to1.HeaderText = "转换后名称";
            this.to1.MinimumWidth = 10;
            this.to1.Name = "to1";
            this.to1.ToolTipText = "转换后参数名称（Hss使用参数名）";
            // 
            // from1
            // 
            this.from1.DataPropertyName = "from";
            this.from1.HeaderText = "转换前名称";
            this.from1.Name = "from1";
            this.from1.ToolTipText = "转换前参数名称（His返回名称）";
            // 
            // description1
            // 
            this.description1.DataPropertyName = "description";
            this.description1.HeaderText = "描述";
            this.description1.Name = "description1";
            this.description1.ToolTipText = "参数描述";
            // 
            // defaultvalue1
            // 
            this.defaultvalue1.DataPropertyName = "defaultvalue";
            this.defaultvalue1.HeaderText = "默认值";
            this.defaultvalue1.Name = "defaultvalue1";
            // 
            // type1
            // 
            this.type1.DataPropertyName = "type";
            this.type1.HeaderText = "变量类型";
            this.type1.Name = "type1";
            this.type1.ToolTipText = "参数变量类型";
            // 
            // mode1
            // 
            this.mode1.DataPropertyName = "mode";
            this.mode1.HeaderText = "方式";
            this.mode1.Name = "mode1";
            this.mode1.ToolTipText = "默认System";
            // 
            // txth2
            // 
            this.txth2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txth2.Location = new System.Drawing.Point(6, 503);
            this.txth2.Name = "txth2";
            this.txth2.Size = new System.Drawing.Size(41, 30);
            this.txth2.TabIndex = 39;
            this.txth2.TabStop = false;
            this.txth2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txth2_KeyPress);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(6, 462);
            this.btnDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(39, 36);
            this.btnDown.TabIndex = 38;
            this.btnDown.Text = "ˇ";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(6, 421);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(39, 37);
            this.btnUp.TabIndex = 37;
            this.btnUp.Text = "ˆ";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 38;
            this.label4.Text = "方法名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(534, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.TabIndex = 39;
            this.label5.Text = "调用名";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(891, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 20);
            this.label6.TabIndex = 40;
            this.label6.Text = "记录日志";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 69);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 41;
            this.label7.Text = "描述";
            // 
            // txtdescription
            // 
            this.txtdescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtdescription.Location = new System.Drawing.Point(67, 68);
            this.txtdescription.Name = "txtdescription";
            this.txtdescription.Size = new System.Drawing.Size(812, 30);
            this.txtdescription.TabIndex = 42;
            // 
            // txtname
            // 
            this.txtname.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtname.Location = new System.Drawing.Point(71, 5);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(452, 53);
            this.txtname.TabIndex = 43;
            // 
            // txtcall
            // 
            this.txtcall.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtcall.Location = new System.Drawing.Point(603, 3);
            this.txtcall.Name = "txtcall";
            this.txtcall.Size = new System.Drawing.Size(452, 53);
            this.txtcall.TabIndex = 44;
            // 
            // txtlog
            // 
            this.txtlog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtlog.Location = new System.Drawing.Point(970, 67);
            this.txtlog.Name = "txtlog";
            this.txtlog.Size = new System.Drawing.Size(85, 30);
            this.txtlog.TabIndex = 45;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtname);
            this.panel1.Controls.Add(this.txtlog);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtdescription);
            this.panel1.Controls.Add(this.txtcall);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(46, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1069, 112);
            this.panel1.TabIndex = 47;
            // 
            // btnSaveNewFunc
            // 
            this.btnSaveNewFunc.Location = new System.Drawing.Point(6, 15);
            this.btnSaveNewFunc.Name = "btnSaveNewFunc";
            this.btnSaveNewFunc.Size = new System.Drawing.Size(36, 115);
            this.btnSaveNewFunc.TabIndex = 48;
            this.btnSaveNewFunc.Text = "&S保存所有";
            this.btnSaveNewFunc.UseVisualStyleBackColor = true;
            this.btnSaveNewFunc.Click += new System.EventHandler(this.btnSaveNewFunc_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 25);
            this.tabControl1.Location = new System.Drawing.Point(282, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1154, 1204);
            this.tabControl1.TabIndex = 49;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView1);
            this.tabPage3.Controls.Add(this.btnDown);
            this.tabPage3.Controls.Add(this.txth2);
            this.tabPage3.Controls.Add(this.btnUp);
            this.tabPage3.Controls.Add(this.btnSaveNewFunc);
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.btnSaveOutArgs);
            this.tabPage3.Controls.Add(this.dataGridView2);
            this.tabPage3.Controls.Add(this.btnSaveInArgs);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1146, 1171);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Xml编辑";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.to,
            this.from,
            this.description,
            this.type,
            this.mode});
            this.dataGridView1.Location = new System.Drawing.Point(49, 132);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(1078, 480);
            this.dataGridView1.TabIndex = 49;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // to
            // 
            this.to.DataPropertyName = "to";
            this.to.HeaderText = "转换后名称";
            this.to.MinimumWidth = 10;
            this.to.Name = "to";
            this.to.ToolTipText = "转换后参数名称（Hss使用参数名）";
            // 
            // from
            // 
            this.from.DataPropertyName = "from";
            this.from.HeaderText = "转换前名称";
            this.from.Name = "from";
            this.from.ToolTipText = "转换前参数名称（His返回名称）";
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "描述";
            this.description.Name = "description";
            this.description.ToolTipText = "参数描述";
            // 
            // type
            // 
            this.type.DataPropertyName = "type";
            this.type.HeaderText = "变量类型";
            this.type.Name = "type";
            this.type.ToolTipText = "参数变量类型";
            // 
            // mode
            // 
            this.mode.DataPropertyName = "mode";
            this.mode.HeaderText = "方式";
            this.mode.Name = "mode";
            this.mode.ToolTipText = "默认System";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblNotice);
            this.tabPage1.Controls.Add(this.txtClearOutput);
            this.tabPage1.Controls.Add(this.txtClearInput);
            this.tabPage1.Controls.Add(this.txtWSAddress);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(1146, 1171);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "测试";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblNotice
            // 
            this.lblNotice.AutoSize = true;
            this.lblNotice.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNotice.Location = new System.Drawing.Point(334, 125);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(0, 33);
            this.lblNotice.TabIndex = 10;
            // 
            // txtClearOutput
            // 
            this.txtClearOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.txtClearOutput.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClearOutput.Location = new System.Drawing.Point(561, 83);
            this.txtClearOutput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtClearOutput.Name = "txtClearOutput";
            this.txtClearOutput.Size = new System.Drawing.Size(150, 48);
            this.txtClearOutput.TabIndex = 4;
            this.txtClearOutput.Text = "清空输出";
            this.txtClearOutput.UseVisualStyleBackColor = true;
            this.txtClearOutput.Click += new System.EventHandler(this.txtClearOutput_Click);
            // 
            // txtClearInput
            // 
            this.txtClearInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.txtClearInput.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClearInput.Location = new System.Drawing.Point(364, 82);
            this.txtClearInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtClearInput.Name = "txtClearInput";
            this.txtClearInput.Size = new System.Drawing.Size(150, 48);
            this.txtClearInput.TabIndex = 3;
            this.txtClearInput.Text = "清空输入";
            this.txtClearInput.UseVisualStyleBackColor = true;
            this.txtClearInput.Click += new System.EventHandler(this.txtClearInput_Click);
            // 
            // txtWSAddress
            // 
            this.txtWSAddress.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWSAddress.Location = new System.Drawing.Point(180, 24);
            this.txtWSAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWSAddress.Name = "txtWSAddress";
            this.txtWSAddress.Size = new System.Drawing.Size(826, 42);
            this.txtWSAddress.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(18, 88);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(150, 38);
            this.comboBox1.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(13, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(148, 30);
            this.label8.TabIndex = 8;
            this.label8.Text = "接口地址:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtOutPut);
            this.groupBox2.Location = new System.Drawing.Point(555, 138);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(574, 1014);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // txtOutPut
            // 
            this.txtOutPut.Location = new System.Drawing.Point(6, 27);
            this.txtOutPut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOutPut.Multiline = true;
            this.txtOutPut.Name = "txtOutPut";
            this.txtOutPut.Size = new System.Drawing.Size(562, 979);
            this.txtOutPut.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtInput);
            this.groupBox1.Location = new System.Drawing.Point(12, 138);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(522, 1014);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(6, 27);
            this.txtInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(508, 979);
            this.txtInput.TabIndex = 0;
            this.txtInput.MouseLeave += new System.EventHandler(this.txtInput_MouseLeave);
            // 
            // btnTest
            // 
            this.btnTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTest.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTest.Location = new System.Drawing.Point(174, 82);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(150, 48);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtFind);
            this.tabPage2.Controls.Add(this.txtHistory);
            this.tabPage2.Controls.Add(this.lstHistoryList);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(1146, 1171);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测试记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtFind
            // 
            this.txtFind.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFind.Location = new System.Drawing.Point(6, 24);
            this.txtFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(229, 39);
            this.txtFind.TabIndex = 5;
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFind_KeyDown);
            // 
            // txtHistory
            // 
            this.txtHistory.Font = new System.Drawing.Font("宋体", 12F);
            this.txtHistory.Location = new System.Drawing.Point(253, 19);
            this.txtHistory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHistory.Size = new System.Drawing.Size(887, 1125);
            this.txtHistory.TabIndex = 0;
            this.txtHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHistory_KeyDown);
            // 
            // lstHistoryList
            // 
            this.lstHistoryList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstHistoryList.FormattingEnabled = true;
            this.lstHistoryList.ItemHeight = 24;
            this.lstHistoryList.Location = new System.Drawing.Point(6, 84);
            this.lstHistoryList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstHistoryList.Name = "lstHistoryList";
            this.lstHistoryList.Size = new System.Drawing.Size(229, 1060);
            this.lstHistoryList.TabIndex = 4;
            this.lstHistoryList.SelectedIndexChanged += new System.EventHandler(this.lstHistoryList_SelectedIndexChanged);
            this.lstHistoryList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstHistoryList_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDialog,
            this.DelFile});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 60);
            // 
            // OpenDialog
            // 
            this.OpenDialog.Name = "OpenDialog";
            this.OpenDialog.Size = new System.Drawing.Size(224, 28);
            this.OpenDialog.Text = "打开文件所在位置";
            this.OpenDialog.Click += new System.EventHandler(this.OpenDialog_Click);
            // 
            // DelFile
            // 
            this.DelFile.Name = "DelFile";
            this.DelFile.Size = new System.Drawing.Size(224, 28);
            this.DelFile.Text = "删除当前文件";
            this.DelFile.Click += new System.EventHandler(this.DelFile_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 32);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(263, 304);
            this.treeView1.TabIndex = 50;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1146, 1171);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "数据库测试日志";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(189, 32);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(188, 28);
            this.toolStripMenuItem1.Text = "新增参数方法";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // FormXmlEditer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1456, 1254);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.lblMethodName);
            this.Controls.Add(this.btnDelSelect);
            this.Controls.Add(this.btnSelectOther);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormXmlEditer";
            this.Text = "WebService对接小工具";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectOther;
        private System.Windows.Forms.Button btnDelSelect;
        private System.Windows.Forms.Label lblMethodName;
        private System.Windows.Forms.Button btnSaveInArgs;
        private System.Windows.Forms.Button btnSaveOutArgs;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsrslblNotice;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重新加载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.ToolStripMenuItem 参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加新参数方法ToolStripMenuItem;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.TextBox txth2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtdescription;
        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.TextBox txtcall;
        private System.Windows.Forms.TextBox txtlog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveNewFunc;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ToolStripMenuItem 保存xmldoc对象ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查找ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.Button txtClearOutput;
        private System.Windows.Forms.Button txtClearInput;
        private System.Windows.Forms.TextBox txtWSAddress;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtOutPut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.ListBox lstHistoryList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OpenDialog;
        private System.Windows.Forms.ToolStripMenuItem DelFile;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn to;
        private System.Windows.Forms.DataGridViewTextBoxColumn from;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn to1;
        private System.Windows.Forms.DataGridViewTextBoxColumn from1;
        private System.Windows.Forms.DataGridViewTextBoxColumn description1;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultvalue1;
        private System.Windows.Forms.DataGridViewTextBoxColumn type1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mode1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}