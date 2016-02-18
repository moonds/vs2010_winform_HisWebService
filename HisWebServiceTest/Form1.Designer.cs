namespace HisWebServiceTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenDialog = new System.Windows.Forms.ToolStripMenuItem();
            this.DelFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.lstHistoryList = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblNotice = new System.Windows.Forms.Label();
            this.txtWSAddress = new System.Windows.Forms.TextBox();
            this.txtErrorOutput = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOutPut = new System.Windows.Forms.TextBox();
            this.txtClearOutput = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtClearInput = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDialog,
            this.DelFile});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(229, 86);
            // 
            // OpenDialog
            // 
            this.OpenDialog.Name = "OpenDialog";
            this.OpenDialog.Size = new System.Drawing.Size(228, 30);
            this.OpenDialog.Text = "打开文件所在位置";
            this.OpenDialog.Click += new System.EventHandler(this.OpenDialog_Click);
            // 
            // DelFile
            // 
            this.DelFile.Name = "DelFile";
            this.DelFile.Size = new System.Drawing.Size(228, 30);
            this.DelFile.Text = "删除当前文件";
            this.DelFile.Click += new System.EventHandler(this.DelFile_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtFind);
            this.tabPage2.Controls.Add(this.txtHistory);
            this.tabPage2.Controls.Add(this.lstHistoryList);
            this.tabPage2.Location = new System.Drawing.Point(4, 44);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(1036, 965);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测试记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtFind
            // 
            this.txtFind.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFind.Location = new System.Drawing.Point(27, 28);
            this.txtFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(208, 39);
            this.txtFind.TabIndex = 5;
            this.txtFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFind_KeyDown);
            // 
            // txtHistory
            // 
            this.txtHistory.Font = new System.Drawing.Font("宋体", 12F);
            this.txtHistory.Location = new System.Drawing.Point(258, 10);
            this.txtHistory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHistory.Size = new System.Drawing.Size(758, 784);
            this.txtHistory.TabIndex = 0;
            this.txtHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHistory_KeyDown);
            // 
            // lstHistoryList
            // 
            this.lstHistoryList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstHistoryList.FormattingEnabled = true;
            this.lstHistoryList.ItemHeight = 24;
            this.lstHistoryList.Location = new System.Drawing.Point(27, 90);
            this.lstHistoryList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstHistoryList.Name = "lstHistoryList";
            this.lstHistoryList.Size = new System.Drawing.Size(208, 676);
            this.lstHistoryList.TabIndex = 4;
            this.lstHistoryList.SelectedIndexChanged += new System.EventHandler(this.lstHistoryList_SelectedIndexChanged);
            this.lstHistoryList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstHistoryList_MouseUp);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblNotice);
            this.tabPage1.Controls.Add(this.txtWSAddress);
            this.tabPage1.Controls.Add(this.txtErrorOutput);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(1036, 965);
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
            // txtWSAddress
            // 
            this.txtWSAddress.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWSAddress.Location = new System.Drawing.Point(185, 29);
            this.txtWSAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWSAddress.Name = "txtWSAddress";
            this.txtWSAddress.Size = new System.Drawing.Size(826, 42);
            this.txtWSAddress.TabIndex = 9;
            // 
            // txtErrorOutput
            // 
            this.txtErrorOutput.Location = new System.Drawing.Point(14, 789);
            this.txtErrorOutput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtErrorOutput.Multiline = true;
            this.txtErrorOutput.Name = "txtErrorOutput";
            this.txtErrorOutput.Size = new System.Drawing.Size(898, 104);
            this.txtErrorOutput.TabIndex = 10;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(119, 115);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(208, 41);
            this.comboBox1.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 30);
            this.label1.TabIndex = 8;
            this.label1.Text = "接口地址:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtOutPut);
            this.groupBox2.Controls.Add(this.txtClearOutput);
            this.groupBox2.Location = new System.Drawing.Point(486, 167);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(516, 612);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出";
            // 
            // txtOutPut
            // 
            this.txtOutPut.Location = new System.Drawing.Point(7, 88);
            this.txtOutPut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOutPut.Multiline = true;
            this.txtOutPut.Name = "txtOutPut";
            this.txtOutPut.Size = new System.Drawing.Size(490, 518);
            this.txtOutPut.TabIndex = 1;
            // 
            // txtClearOutput
            // 
            this.txtClearOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.txtClearOutput.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClearOutput.Location = new System.Drawing.Point(21, 26);
            this.txtClearOutput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtClearOutput.Name = "txtClearOutput";
            this.txtClearOutput.Size = new System.Drawing.Size(153, 48);
            this.txtClearOutput.TabIndex = 4;
            this.txtClearOutput.Text = "清空输出";
            this.txtClearOutput.UseVisualStyleBackColor = true;
            this.txtClearOutput.Click += new System.EventHandler(this.txtClearOutput_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtInput);
            this.groupBox1.Controls.Add(this.txtClearInput);
            this.groupBox1.Location = new System.Drawing.Point(12, 167);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(446, 612);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(7, 88);
            this.txtInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(426, 502);
            this.txtInput.TabIndex = 0;
            this.txtInput.MouseLeave += new System.EventHandler(this.txtInput_MouseLeave);
            // 
            // txtClearInput
            // 
            this.txtClearInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.txtClearInput.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtClearInput.Location = new System.Drawing.Point(8, 26);
            this.txtClearInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtClearInput.Name = "txtClearInput";
            this.txtClearInput.Size = new System.Drawing.Size(152, 48);
            this.txtClearInput.TabIndex = 3;
            this.txtClearInput.Text = "清空输入";
            this.txtClearInput.UseVisualStyleBackColor = true;
            this.txtClearInput.Click += new System.EventHandler(this.txtClearInput_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTest.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTest.Location = new System.Drawing.Point(16, 115);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(92, 46);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 40);
            this.tabControl1.Location = new System.Drawing.Point(14, 70);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1044, 1013);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 1188);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "接口对接";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OpenDialog;
        private System.Windows.Forms.ToolStripMenuItem DelFile;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.ListBox lstHistoryList;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.TextBox txtWSAddress;
        private System.Windows.Forms.TextBox txtErrorOutput;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtOutPut;
        private System.Windows.Forms.Button txtClearOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button txtClearInput;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TabControl tabControl1;

    }
}

