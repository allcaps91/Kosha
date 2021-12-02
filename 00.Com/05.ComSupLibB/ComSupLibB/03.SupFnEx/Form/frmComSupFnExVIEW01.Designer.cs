namespace ComSupLibB.SupFnEx
{
    partial class frmComSupFnExVIEW01
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
            this.panheader4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panbtn1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panInfo = new System.Windows.Forms.Panel();
            this.lblinfo = new System.Windows.Forms.Label();
            this.panPano = new System.Windows.Forms.Panel();
            this.txtJumin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSName = new System.Windows.Forms.TextBox();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssList = new FarPoint.Win.Spread.FpSpread();
            this.ssList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.line2 = new DevComponents.DotNetBar.Controls.Line();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.panheader4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panbtn1.SuspendLayout();
            this.panInfo.SuspendLayout();
            this.panPano.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panheader4
            // 
            this.panheader4.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panheader4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panheader4.Controls.Add(this.label2);
            this.panheader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panheader4.Location = new System.Drawing.Point(0, 0);
            this.panheader4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panheader4.Name = "panheader4";
            this.panheader4.Size = new System.Drawing.Size(784, 40);
            this.panheader4.TabIndex = 121;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 17);
            this.label2.TabIndex = 41;
            this.label2.Text = "컨설트 내용 보기";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panbtn1);
            this.panel1.Controls.Add(this.panInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 42);
            this.panel1.TabIndex = 122;
            // 
            // panbtn1
            // 
            this.panbtn1.Controls.Add(this.btnExit);
            this.panbtn1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panbtn1.Location = new System.Drawing.Point(704, 0);
            this.panbtn1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Name = "panbtn1";
            this.panbtn1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panbtn1.Size = new System.Drawing.Size(78, 42);
            this.panbtn1.TabIndex = 30;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(5, 4);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 34);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // panInfo
            // 
            this.panInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panInfo.Controls.Add(this.lblinfo);
            this.panInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panInfo.Location = new System.Drawing.Point(0, 0);
            this.panInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panInfo.Name = "panInfo";
            this.panInfo.Padding = new System.Windows.Forms.Padding(5, 8, 5, 7);
            this.panInfo.Size = new System.Drawing.Size(704, 42);
            this.panInfo.TabIndex = 31;
            // 
            // lblinfo
            // 
            this.lblinfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblinfo.Location = new System.Drawing.Point(3, 8);
            this.lblinfo.Name = "lblinfo";
            this.lblinfo.Size = new System.Drawing.Size(693, 24);
            this.lblinfo.TabIndex = 43;
            this.lblinfo.Text = "환자명/등록번호 : 81000004/전산실연습   내원(입원)일자: 2017-05-01";
            this.lblinfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panPano
            // 
            this.panPano.Controls.Add(this.txtJumin);
            this.panPano.Controls.Add(this.label6);
            this.panPano.Controls.Add(this.label5);
            this.panPano.Controls.Add(this.txtSName);
            this.panPano.Controls.Add(this.txtPano);
            this.panPano.Controls.Add(this.label4);
            this.panPano.Dock = System.Windows.Forms.DockStyle.Top;
            this.panPano.Location = new System.Drawing.Point(0, 82);
            this.panPano.Name = "panPano";
            this.panPano.Padding = new System.Windows.Forms.Padding(3);
            this.panPano.Size = new System.Drawing.Size(784, 41);
            this.panPano.TabIndex = 138;
            // 
            // txtJumin
            // 
            this.txtJumin.Location = new System.Drawing.Point(314, 7);
            this.txtJumin.Name = "txtJumin";
            this.txtJumin.Size = new System.Drawing.Size(95, 25);
            this.txtJumin.TabIndex = 133;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(151, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 17);
            this.label6.TabIndex = 132;
            this.label6.Text = "성명";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(252, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 132;
            this.label5.Text = "주민번호";
            // 
            // txtSName
            // 
            this.txtSName.Location = new System.Drawing.Point(185, 7);
            this.txtSName.Name = "txtSName";
            this.txtSName.Size = new System.Drawing.Size(64, 25);
            this.txtSName.TabIndex = 131;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(62, 7);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(88, 25);
            this.txtPano.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 130;
            this.label4.Text = "등록번호";
            // 
            // line1
            // 
            this.line1.Dock = System.Windows.Forms.DockStyle.Top;
            this.line1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line1.Location = new System.Drawing.Point(0, 123);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(784, 8);
            this.line1.TabIndex = 141;
            this.line1.Text = "line1";
            this.line1.Thickness = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 131);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel2.Size = new System.Drawing.Size(784, 171);
            this.panel2.TabIndex = 142;
            // 
            // ssList
            // 
            this.ssList.AccessibleDescription = "ssList, Sheet1, Row 0, Column 0, ";
            this.ssList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ssList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssList.Location = new System.Drawing.Point(0, 3);
            this.ssList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssList.Name = "ssList";
            this.ssList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList_Sheet1});
            this.ssList.Size = new System.Drawing.Size(784, 168);
            this.ssList.TabIndex = 113;
            // 
            // ssList_Sheet1
            // 
            this.ssList_Sheet1.Reset();
            this.ssList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList_Sheet1.ColumnCount = 10;
            this.ssList_Sheet1.RowCount = 5;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssList_Sheet1.Columns.Get(0).Width = 71F;
            this.ssList_Sheet1.Columns.Get(1).Width = 71F;
            this.ssList_Sheet1.Columns.Get(2).Width = 71F;
            this.ssList_Sheet1.Columns.Get(3).Width = 71F;
            this.ssList_Sheet1.Columns.Get(4).Width = 71F;
            this.ssList_Sheet1.Columns.Get(5).Width = 71F;
            this.ssList_Sheet1.Columns.Get(6).Width = 71F;
            this.ssList_Sheet1.Columns.Get(7).Width = 71F;
            this.ssList_Sheet1.Columns.Get(8).Width = 71F;
            this.ssList_Sheet1.Columns.Get(9).Width = 71F;
            this.ssList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
            this.ssList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // line2
            // 
            this.line2.Dock = System.Windows.Forms.DockStyle.Top;
            this.line2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.line2.Location = new System.Drawing.Point(0, 302);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(784, 8);
            this.line2.TabIndex = 144;
            this.line2.Text = "line2";
            this.line2.Thickness = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtResult);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 310);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(784, 551);
            this.panel3.TabIndex = 145;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtResult.MaxLength = 100000;
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(784, 551);
            this.txtResult.TabIndex = 1;
            // 
            // frmComSupFnExVIEW01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 861);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.panPano);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panheader4);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupFnExVIEW01";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmComSupFnExViewConsult";
            this.panheader4.ResumeLayout(false);
            this.panheader4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panbtn1.ResumeLayout(false);
            this.panInfo.ResumeLayout(false);
            this.panPano.ResumeLayout(false);
            this.panPano.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panheader4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panbtn1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panInfo;
        private System.Windows.Forms.Label lblinfo;
        private System.Windows.Forms.Panel panPano;
        private System.Windows.Forms.TextBox txtJumin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSName;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssList;
        private FarPoint.Win.Spread.SheetView ssList_Sheet1;
        private DevComponents.DotNetBar.Controls.Line line2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtResult;
    }
}