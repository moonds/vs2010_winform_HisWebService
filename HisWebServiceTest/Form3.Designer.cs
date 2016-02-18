namespace HisWebServiceTest
{
    partial class Form3
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnReLoad = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnSaveCurrent = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectOther = new System.Windows.Forms.Button();
            this.btnDelSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMethodName = new System.Windows.Forms.Label();
            this.btnExportXml = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加新参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除当前条ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加入参方法ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddInArg = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(314, 619);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1436, 469);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "出参列表";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(322, 655);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1423, 433);
            this.panel2.TabIndex = 1;
            this.panel2.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(325, 547);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1436, 429);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "入参列表";
            this.groupBox1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(6, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1430, 392);
            this.panel1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(321, 57);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(615, 31);
            this.textBox1.TabIndex = 6;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // btnReLoad
            // 
            this.btnReLoad.Location = new System.Drawing.Point(942, 51);
            this.btnReLoad.Name = "btnReLoad";
            this.btnReLoad.Size = new System.Drawing.Size(160, 37);
            this.btnReLoad.TabIndex = 5;
            this.btnReLoad.Text = "&ReLoad";
            this.btnReLoad.UseVisualStyleBackColor = true;
            this.btnReLoad.Click += new System.EventHandler(this.btnReLoad_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(45, 123);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(262, 888);
            this.checkedListBox1.TabIndex = 6;
            this.checkedListBox1.Click += new System.EventHandler(this.checkedListBox1_Click);
            // 
            // btnSaveCurrent
            // 
            this.btnSaveCurrent.Location = new System.Drawing.Point(1117, 51);
            this.btnSaveCurrent.Name = "btnSaveCurrent";
            this.btnSaveCurrent.Size = new System.Drawing.Size(160, 37);
            this.btnSaveCurrent.TabIndex = 10;
            this.btnSaveCurrent.Text = "&SaveCurrent";
            this.btnSaveCurrent.UseVisualStyleBackColor = true;
            this.btnSaveCurrent.Click += new System.EventHandler(this.btnSaveCurrent_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(45, 57);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 49);
            this.btnSelectAll.TabIndex = 12;
            this.btnSelectAll.Text = "All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectOther
            // 
            this.btnSelectOther.Location = new System.Drawing.Point(126, 57);
            this.btnSelectOther.Name = "btnSelectOther";
            this.btnSelectOther.Size = new System.Drawing.Size(75, 49);
            this.btnSelectOther.TabIndex = 13;
            this.btnSelectOther.Text = "Other";
            this.btnSelectOther.UseVisualStyleBackColor = true;
            this.btnSelectOther.Click += new System.EventHandler(this.btnSelectOther_Click);
            // 
            // btnDelSelect
            // 
            this.btnDelSelect.Location = new System.Drawing.Point(207, 57);
            this.btnDelSelect.Name = "btnDelSelect";
            this.btnDelSelect.Size = new System.Drawing.Size(75, 49);
            this.btnDelSelect.TabIndex = 14;
            this.btnDelSelect.Text = "Del";
            this.btnDelSelect.UseVisualStyleBackColor = true;
            this.btnDelSelect.Click += new System.EventHandler(this.btnDelSelect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(44, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 46);
            this.label1.TabIndex = 15;
            this.label1.Text = "当前加载方法";
            // 
            // lblMethodName
            // 
            this.lblMethodName.AutoSize = true;
            this.lblMethodName.Font = new System.Drawing.Font("Comic Sans MS", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethodName.Location = new System.Drawing.Point(310, 13);
            this.lblMethodName.Name = "lblMethodName";
            this.lblMethodName.Size = new System.Drawing.Size(0, 56);
            this.lblMethodName.TabIndex = 16;
            // 
            // btnExportXml
            // 
            this.btnExportXml.Location = new System.Drawing.Point(1290, 53);
            this.btnExportXml.Name = "btnExportXml";
            this.btnExportXml.Size = new System.Drawing.Size(162, 38);
            this.btnExportXml.TabIndex = 17;
            this.btnExportXml.Text = "&Export";
            this.btnExportXml.UseVisualStyleBackColor = true;
            this.btnExportXml.Click += new System.EventHandler(this.btnExportXml_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加新参数ToolStripMenuItem,
            this.删除当前条ToolStripMenuItem,
            this.添加入参方法ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(281, 112);
            // 
            // 添加新参数ToolStripMenuItem
            // 
            this.添加新参数ToolStripMenuItem.Name = "添加新参数ToolStripMenuItem";
            this.添加新参数ToolStripMenuItem.Size = new System.Drawing.Size(280, 36);
            this.添加新参数ToolStripMenuItem.Text = "添加新参数";
            // 
            // 删除当前条ToolStripMenuItem
            // 
            this.删除当前条ToolStripMenuItem.Name = "删除当前条ToolStripMenuItem";
            this.删除当前条ToolStripMenuItem.Size = new System.Drawing.Size(280, 36);
            this.删除当前条ToolStripMenuItem.Text = "删除";
            // 
            // 添加入参方法ToolStripMenuItem
            // 
            this.添加入参方法ToolStripMenuItem.Name = "添加入参方法ToolStripMenuItem";
            this.添加入参方法ToolStripMenuItem.Size = new System.Drawing.Size(280, 36);
            this.添加入参方法ToolStripMenuItem.Text = "添加入参方法名称";
            this.添加入参方法ToolStripMenuItem.Click += new System.EventHandler(this.添加入参方法ToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(321, 103);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1429, 82);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "方法参数";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.to,
            this.from,
            this.type,
            this.description});
            this.dataGridView1.Location = new System.Drawing.Point(320, 230);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(890, 299);
            this.dataGridView1.TabIndex = 21;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // 序号
            // 
            this.序号.HeaderText = "id";
            this.序号.Name = "序号";
            // 
            // to
            // 
            this.to.HeaderText = "to";
            this.to.Name = "to";
            // 
            // from
            // 
            this.from.HeaderText = "from";
            this.from.Name = "from";
            // 
            // type
            // 
            this.type.HeaderText = "type";
            this.type.Name = "type";
            // 
            // description
            // 
            this.description.HeaderText = "description";
            this.description.Name = "description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(320, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 25);
            this.label2.TabIndex = 22;
            this.label2.Text = "入参列表";
            // 
            // btnAddInArg
            // 
            this.btnAddInArg.Location = new System.Drawing.Point(422, 189);
            this.btnAddInArg.Name = "btnAddInArg";
            this.btnAddInArg.Size = new System.Drawing.Size(124, 31);
            this.btnAddInArg.TabIndex = 23;
            this.btnAddInArg.Text = "新增入参";
            this.btnAddInArg.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1963, 1075);
            this.Controls.Add(this.btnAddInArg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnExportXml);
            this.Controls.Add(this.lblMethodName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDelSelect);
            this.Controls.Add(this.btnSelectOther);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnSaveCurrent);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnReLoad);
            this.Name = "Form3";
            this.Text = "Form3";
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnReLoad;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button btnSaveCurrent;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectOther;
        private System.Windows.Forms.Button btnDelSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMethodName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnExportXml;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 添加新参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除当前条ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加入参方法ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn to;
        private System.Windows.Forms.DataGridViewTextBoxColumn from;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddInArg;
    }
}