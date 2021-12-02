namespace ComEmrBase
{
    partial class frmEmrFormSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmrFormSearch));
            this.panTitle = new System.Windows.Forms.Panel();
            this.mbtnSave = new System.Windows.Forms.Button();
            this.optUser = new System.Windows.Forms.RadioButton();
            this.optDept = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.mbtnCollapseAll = new System.Windows.Forms.Button();
            this.mbtnExpandAll = new System.Windows.Forms.Button();
            this.mbtnExit = new System.Windows.Forms.Button();
            this.ImageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.mbtnSearch = new System.Windows.Forms.Button();
            this.txtFormName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trvEmrForm = new System.Windows.Forms.TreeView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.Control;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.mbtnSave);
            this.panTitle.Controls.Add(this.optUser);
            this.panTitle.Controls.Add(this.optDept);
            this.panTitle.Controls.Add(this.optAll);
            this.panTitle.Controls.Add(this.cboDept);
            this.panTitle.Controls.Add(this.mbtnCollapseAll);
            this.panTitle.Controls.Add(this.mbtnExpandAll);
            this.panTitle.Controls.Add(this.mbtnExit);
            this.panTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panTitle.Size = new System.Drawing.Size(514, 37);
            this.panTitle.TabIndex = 4;
            this.panTitle.TabStop = true;
            // 
            // mbtnSave
            // 
            this.mbtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSave.Location = new System.Drawing.Point(353, 2);
            this.mbtnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnSave.Name = "mbtnSave";
            this.mbtnSave.Size = new System.Drawing.Size(56, 30);
            this.mbtnSave.TabIndex = 82;
            this.mbtnSave.Text = "등록";
            this.mbtnSave.UseVisualStyleBackColor = true;
            this.mbtnSave.Click += new System.EventHandler(this.mbtnSave_Click);
            // 
            // optUser
            // 
            this.optUser.AutoSize = true;
            this.optUser.Location = new System.Drawing.Point(100, 7);
            this.optUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optUser.Name = "optUser";
            this.optUser.Size = new System.Drawing.Size(52, 21);
            this.optUser.TabIndex = 81;
            this.optUser.TabStop = true;
            this.optUser.Text = "개인";
            this.optUser.UseVisualStyleBackColor = true;
            this.optUser.CheckedChanged += new System.EventHandler(this.optUser_CheckedChanged);
            // 
            // optDept
            // 
            this.optDept.AutoSize = true;
            this.optDept.Location = new System.Drawing.Point(53, 7);
            this.optDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optDept.Name = "optDept";
            this.optDept.Size = new System.Drawing.Size(52, 21);
            this.optDept.TabIndex = 80;
            this.optDept.TabStop = true;
            this.optDept.Text = "과별";
            this.optDept.UseVisualStyleBackColor = true;
            this.optDept.CheckedChanged += new System.EventHandler(this.optDept_CheckedChanged);
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Location = new System.Drawing.Point(6, 7);
            this.optAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(52, 21);
            this.optAll.TabIndex = 79;
            this.optAll.TabStop = true;
            this.optAll.Text = "전체";
            this.optAll.UseVisualStyleBackColor = true;
            this.optAll.CheckedChanged += new System.EventHandler(this.optAll_CheckedChanged);
            // 
            // cboDept
            // 
            this.cboDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(158, 5);
            this.cboDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(58, 25);
            this.cboDept.TabIndex = 78;
            this.cboDept.Visible = false;
            this.cboDept.SelectedIndexChanged += new System.EventHandler(this.cboDept_SelectedIndexChanged);
            // 
            // mbtnCollapseAll
            // 
            this.mbtnCollapseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnCollapseAll.Location = new System.Drawing.Point(291, 2);
            this.mbtnCollapseAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnCollapseAll.Name = "mbtnCollapseAll";
            this.mbtnCollapseAll.Size = new System.Drawing.Size(56, 30);
            this.mbtnCollapseAll.TabIndex = 70;
            this.mbtnCollapseAll.Text = "닫기";
            this.mbtnCollapseAll.UseVisualStyleBackColor = true;
            this.mbtnCollapseAll.Click += new System.EventHandler(this.mbtnCollapseAll_Click);
            // 
            // mbtnExpandAll
            // 
            this.mbtnExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExpandAll.Location = new System.Drawing.Point(235, 2);
            this.mbtnExpandAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnExpandAll.Name = "mbtnExpandAll";
            this.mbtnExpandAll.Size = new System.Drawing.Size(56, 30);
            this.mbtnExpandAll.TabIndex = 69;
            this.mbtnExpandAll.Text = "열기";
            this.mbtnExpandAll.UseVisualStyleBackColor = true;
            this.mbtnExpandAll.Click += new System.EventHandler(this.mbtnExpandAll_Click);
            // 
            // mbtnExit
            // 
            this.mbtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnExit.Location = new System.Drawing.Point(441, 2);
            this.mbtnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnExit.Name = "mbtnExit";
            this.mbtnExit.Size = new System.Drawing.Size(67, 30);
            this.mbtnExit.TabIndex = 9;
            this.mbtnExit.Text = "닫 기";
            this.mbtnExit.UseVisualStyleBackColor = true;
            this.mbtnExit.Click += new System.EventHandler(this.mbtnExit_Click);
            // 
            // ImageList2
            // 
            this.ImageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList2.ImageStream")));
            this.ImageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList2.Images.SetKeyName(0, "folder.png");
            this.ImageList2.Images.SetKeyName(1, "folder_accept.png");
            this.ImageList2.Images.SetKeyName(2, "note.png");
            this.ImageList2.Images.SetKeyName(3, "note_accept.png");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.mbtnSearch);
            this.panel1.Controls.Add(this.txtFormName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 37);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 37);
            this.panel1.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(288, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 17);
            this.label2.TabIndex = 72;
            this.label2.Text = "전체기록에서 조회 가능";
            // 
            // mbtnSearch
            // 
            this.mbtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbtnSearch.Location = new System.Drawing.Point(441, 1);
            this.mbtnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mbtnSearch.Name = "mbtnSearch";
            this.mbtnSearch.Size = new System.Drawing.Size(66, 30);
            this.mbtnSearch.TabIndex = 71;
            this.mbtnSearch.Text = "조회";
            this.mbtnSearch.UseVisualStyleBackColor = true;
            this.mbtnSearch.Click += new System.EventHandler(this.mbtnSearch_Click);
            // 
            // txtFormName
            // 
            this.txtFormName.Location = new System.Drawing.Point(69, 4);
            this.txtFormName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(210, 25);
            this.txtFormName.TabIndex = 1;
            this.txtFormName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFormName_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "서식지명:";
            // 
            // trvEmrForm
            // 
            this.trvEmrForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvEmrForm.Location = new System.Drawing.Point(0, 74);
            this.trvEmrForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trvEmrForm.Name = "trvEmrForm";
            this.trvEmrForm.Size = new System.Drawing.Size(514, 984);
            this.trvEmrForm.TabIndex = 20;
            this.trvEmrForm.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvEmrForm_NodeMouseClick);
            // 
            // frmEmrFormSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 1058);
            this.ControlBox = false;
            this.Controls.Add(this.trvEmrForm);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrFormSearch";
            this.Text = "기록지 조회";
            this.Load += new System.EventHandler(this.frmEmrFormSearch_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button mbtnExit;
        private System.Windows.Forms.Button mbtnCollapseAll;
        private System.Windows.Forms.Button mbtnExpandAll;
        private System.Windows.Forms.RadioButton optUser;
        private System.Windows.Forms.RadioButton optDept;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.ComboBox cboDept;
        internal System.Windows.Forms.ImageList ImageList2;
        private System.Windows.Forms.Button mbtnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button mbtnSearch;
        private System.Windows.Forms.TextBox txtFormName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView trvEmrForm;
        private System.Windows.Forms.Label label2;
    }
}