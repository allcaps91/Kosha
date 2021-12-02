namespace ComEmrBase
{
    partial class frmEmrChartPopup
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
            this.pnlTool = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.rdoPink = new System.Windows.Forms.RadioButton();
            this.rdoViolet = new System.Windows.Forms.RadioButton();
            this.rdoBlue = new System.Windows.Forms.RadioButton();
            this.rdoGreen = new System.Windows.Forms.RadioButton();
            this.rdoLime = new System.Windows.Forms.RadioButton();
            this.rdoGold = new System.Windows.Forms.RadioButton();
            this.rdoRed = new System.Windows.Forms.RadioButton();
            this.rdoBlack = new System.Windows.Forms.RadioButton();
            this.pnlTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTool
            // 
            this.pnlTool.Controls.Add(this.btnExit);
            this.pnlTool.Controls.Add(this.btnDelete);
            this.pnlTool.Controls.Add(this.btnSave);
            this.pnlTool.Controls.Add(this.btnRedo);
            this.pnlTool.Controls.Add(this.btnUndo);
            this.pnlTool.Controls.Add(this.rdoPink);
            this.pnlTool.Controls.Add(this.rdoViolet);
            this.pnlTool.Controls.Add(this.rdoBlue);
            this.pnlTool.Controls.Add(this.rdoGreen);
            this.pnlTool.Controls.Add(this.rdoLime);
            this.pnlTool.Controls.Add(this.rdoGold);
            this.pnlTool.Controls.Add(this.rdoRed);
            this.pnlTool.Controls.Add(this.rdoBlack);
            this.pnlTool.Location = new System.Drawing.Point(0, 0);
            this.pnlTool.Name = "pnlTool";
            this.pnlTool.Size = new System.Drawing.Size(736, 40);
            this.pnlTool.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(660, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(61, 31);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(496, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 31);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(578, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(61, 31);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "저 장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Location = new System.Drawing.Point(337, 4);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(56, 31);
            this.btnRedo.TabIndex = 9;
            this.btnRedo.Text = "Redo";
            this.btnRedo.UseVisualStyleBackColor = true;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(276, 4);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(56, 31);
            this.btnUndo.TabIndex = 8;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // rdoPink
            // 
            this.rdoPink.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoPink.AutoSize = true;
            this.rdoPink.BackColor = System.Drawing.Color.Pink;
            this.rdoPink.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoPink.FlatAppearance.BorderColor = System.Drawing.Color.Pink;
            this.rdoPink.FlatAppearance.BorderSize = 5;
            this.rdoPink.FlatAppearance.CheckedBackColor = System.Drawing.Color.Pink;
            this.rdoPink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoPink.Location = new System.Drawing.Point(196, 3);
            this.rdoPink.Name = "rdoPink";
            this.rdoPink.Size = new System.Drawing.Size(29, 32);
            this.rdoPink.TabIndex = 7;
            this.rdoPink.Text = " ";
            this.rdoPink.UseVisualStyleBackColor = false;
            this.rdoPink.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoViolet
            // 
            this.rdoViolet.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoViolet.AutoSize = true;
            this.rdoViolet.BackColor = System.Drawing.Color.BlueViolet;
            this.rdoViolet.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoViolet.FlatAppearance.BorderColor = System.Drawing.Color.BlueViolet;
            this.rdoViolet.FlatAppearance.BorderSize = 5;
            this.rdoViolet.FlatAppearance.CheckedBackColor = System.Drawing.Color.BlueViolet;
            this.rdoViolet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoViolet.Location = new System.Drawing.Point(165, 3);
            this.rdoViolet.Name = "rdoViolet";
            this.rdoViolet.Size = new System.Drawing.Size(29, 32);
            this.rdoViolet.TabIndex = 6;
            this.rdoViolet.Text = " ";
            this.rdoViolet.UseVisualStyleBackColor = false;
            this.rdoViolet.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoBlue
            // 
            this.rdoBlue.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoBlue.AutoSize = true;
            this.rdoBlue.BackColor = System.Drawing.Color.SteelBlue;
            this.rdoBlue.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBlue.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.rdoBlue.FlatAppearance.BorderSize = 5;
            this.rdoBlue.FlatAppearance.CheckedBackColor = System.Drawing.Color.SteelBlue;
            this.rdoBlue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoBlue.Location = new System.Drawing.Point(134, 3);
            this.rdoBlue.Name = "rdoBlue";
            this.rdoBlue.Size = new System.Drawing.Size(29, 32);
            this.rdoBlue.TabIndex = 5;
            this.rdoBlue.Text = " ";
            this.rdoBlue.UseVisualStyleBackColor = false;
            this.rdoBlue.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoGreen
            // 
            this.rdoGreen.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoGreen.AutoSize = true;
            this.rdoGreen.BackColor = System.Drawing.Color.SeaGreen;
            this.rdoGreen.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoGreen.FlatAppearance.BorderColor = System.Drawing.Color.SeaGreen;
            this.rdoGreen.FlatAppearance.BorderSize = 5;
            this.rdoGreen.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.rdoGreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoGreen.Location = new System.Drawing.Point(103, 3);
            this.rdoGreen.Name = "rdoGreen";
            this.rdoGreen.Size = new System.Drawing.Size(29, 32);
            this.rdoGreen.TabIndex = 4;
            this.rdoGreen.Text = " ";
            this.rdoGreen.UseVisualStyleBackColor = false;
            this.rdoGreen.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoLime
            // 
            this.rdoLime.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoLime.AutoSize = true;
            this.rdoLime.BackColor = System.Drawing.Color.Lime;
            this.rdoLime.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoLime.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.rdoLime.FlatAppearance.BorderSize = 5;
            this.rdoLime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.rdoLime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoLime.Location = new System.Drawing.Point(72, 3);
            this.rdoLime.Name = "rdoLime";
            this.rdoLime.Size = new System.Drawing.Size(29, 32);
            this.rdoLime.TabIndex = 3;
            this.rdoLime.Text = " ";
            this.rdoLime.UseVisualStyleBackColor = false;
            this.rdoLime.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoGold
            // 
            this.rdoGold.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoGold.AutoSize = true;
            this.rdoGold.BackColor = System.Drawing.Color.Gold;
            this.rdoGold.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoGold.FlatAppearance.BorderColor = System.Drawing.Color.Gold;
            this.rdoGold.FlatAppearance.BorderSize = 5;
            this.rdoGold.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold;
            this.rdoGold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoGold.Location = new System.Drawing.Point(41, 3);
            this.rdoGold.Name = "rdoGold";
            this.rdoGold.Size = new System.Drawing.Size(29, 32);
            this.rdoGold.TabIndex = 2;
            this.rdoGold.Text = " ";
            this.rdoGold.UseVisualStyleBackColor = false;
            this.rdoGold.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoRed
            // 
            this.rdoRed.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoRed.AutoSize = true;
            this.rdoRed.BackColor = System.Drawing.Color.Red;
            this.rdoRed.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoRed.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.rdoRed.FlatAppearance.BorderSize = 5;
            this.rdoRed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Red;
            this.rdoRed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoRed.Location = new System.Drawing.Point(10, 3);
            this.rdoRed.Name = "rdoRed";
            this.rdoRed.Size = new System.Drawing.Size(29, 32);
            this.rdoRed.TabIndex = 1;
            this.rdoRed.Text = " ";
            this.rdoRed.UseVisualStyleBackColor = false;
            this.rdoRed.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // rdoBlack
            // 
            this.rdoBlack.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoBlack.AutoSize = true;
            this.rdoBlack.BackColor = System.Drawing.Color.Black;
            this.rdoBlack.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdoBlack.FlatAppearance.BorderSize = 5;
            this.rdoBlack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.rdoBlack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoBlack.Location = new System.Drawing.Point(227, 3);
            this.rdoBlack.Name = "rdoBlack";
            this.rdoBlack.Size = new System.Drawing.Size(29, 32);
            this.rdoBlack.TabIndex = 0;
            this.rdoBlack.Text = " ";
            this.rdoBlack.UseVisualStyleBackColor = false;
            this.rdoBlack.Click += new System.EventHandler(this.RdoColor_Click);
            // 
            // frmEmrChartPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(736, 879);
            this.Controls.Add(this.pnlTool);
            this.Name = "frmEmrChartPopup";
            this.Text = "frmEmrChartPopup";
            this.Load += new System.EventHandler(this.frmEmrChartPopup_Load);
            this.Shown += new System.EventHandler(this.frmEmrChartPopup_Shown);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.frmEmrChartPopup_Scroll);
            this.pnlTool.ResumeLayout(false);
            this.pnlTool.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTool;
        private System.Windows.Forms.RadioButton rdoPink;
        private System.Windows.Forms.RadioButton rdoViolet;
        private System.Windows.Forms.RadioButton rdoBlue;
        private System.Windows.Forms.RadioButton rdoGreen;
        private System.Windows.Forms.RadioButton rdoLime;
        private System.Windows.Forms.RadioButton rdoGold;
        private System.Windows.Forms.RadioButton rdoRed;
        private System.Windows.Forms.RadioButton rdoBlack;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExit;
    }
}