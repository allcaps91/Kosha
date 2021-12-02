namespace ComLibB
{
    partial class frmOcsCpPopUpMsg
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
            this.lblCP = new System.Windows.Forms.Label();
            this.panCP = new System.Windows.Forms.Panel();
            this.panInfo1 = new System.Windows.Forms.Panel();
            this.lblDrugName1 = new System.Windows.Forms.Label();
            this.lblAge1 = new System.Windows.Forms.Label();
            this.lblSex1 = new System.Windows.Forms.Label();
            this.lblPtname1 = new System.Windows.Forms.Label();
            this.lblPtno1 = new System.Windows.Forms.Label();
            this.lblCpName1 = new System.Windows.Forms.Label();
            this.panInfo0 = new System.Windows.Forms.Panel();
            this.lblDrugName0 = new System.Windows.Forms.Label();
            this.lblAge0 = new System.Windows.Forms.Label();
            this.lblSex0 = new System.Windows.Forms.Label();
            this.lblPtname0 = new System.Windows.Forms.Label();
            this.lblPtno0 = new System.Windows.Forms.Label();
            this.lblCpName0 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panCP.SuspendLayout();
            this.panInfo1.SuspendLayout();
            this.panInfo0.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCP
            // 
            this.lblCP.AutoSize = true;
            this.lblCP.Font = new System.Drawing.Font("맑은 고딕", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCP.ForeColor = System.Drawing.Color.Red;
            this.lblCP.Location = new System.Drawing.Point(12, 89);
            this.lblCP.Name = "lblCP";
            this.lblCP.Size = new System.Drawing.Size(1166, 128);
            this.lblCP.TabIndex = 0;
            this.lblCP.Text = "CP 환자가 발생 했습니다.";
            this.lblCP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panCP
            // 
            this.panCP.Controls.Add(this.panInfo1);
            this.panCP.Controls.Add(this.panInfo0);
            this.panCP.Controls.Add(this.btnSave);
            this.panCP.Controls.Add(this.lblCP);
            this.panCP.Location = new System.Drawing.Point(44, 12);
            this.panCP.Name = "panCP";
            this.panCP.Size = new System.Drawing.Size(1191, 627);
            this.panCP.TabIndex = 1;
            // 
            // panInfo1
            // 
            this.panInfo1.Controls.Add(this.lblDrugName1);
            this.panInfo1.Controls.Add(this.lblAge1);
            this.panInfo1.Controls.Add(this.lblSex1);
            this.panInfo1.Controls.Add(this.lblPtname1);
            this.panInfo1.Controls.Add(this.lblPtno1);
            this.panInfo1.Controls.Add(this.lblCpName1);
            this.panInfo1.Location = new System.Drawing.Point(12, 334);
            this.panInfo1.Name = "panInfo1";
            this.panInfo1.Size = new System.Drawing.Size(1166, 57);
            this.panInfo1.TabIndex = 4;
            // 
            // lblDrugName1
            // 
            this.lblDrugName1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDrugName1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDrugName1.ForeColor = System.Drawing.Color.Blue;
            this.lblDrugName1.Location = new System.Drawing.Point(724, 8);
            this.lblDrugName1.Name = "lblDrugName1";
            this.lblDrugName1.Size = new System.Drawing.Size(347, 40);
            this.lblDrugName1.TabIndex = 7;
            this.lblDrugName1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAge1
            // 
            this.lblAge1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAge1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAge1.ForeColor = System.Drawing.Color.Black;
            this.lblAge1.Location = new System.Drawing.Point(659, 8);
            this.lblAge1.Name = "lblAge1";
            this.lblAge1.Size = new System.Drawing.Size(65, 40);
            this.lblAge1.TabIndex = 6;
            this.lblAge1.Text = "999";
            this.lblAge1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSex1
            // 
            this.lblSex1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSex1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSex1.ForeColor = System.Drawing.Color.Black;
            this.lblSex1.Location = new System.Drawing.Point(594, 8);
            this.lblSex1.Name = "lblSex1";
            this.lblSex1.Size = new System.Drawing.Size(65, 40);
            this.lblSex1.TabIndex = 5;
            this.lblSex1.Text = "남";
            this.lblSex1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPtname1
            // 
            this.lblPtname1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPtname1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPtname1.ForeColor = System.Drawing.Color.Blue;
            this.lblPtname1.Location = new System.Drawing.Point(460, 8);
            this.lblPtname1.Name = "lblPtname1";
            this.lblPtname1.Size = new System.Drawing.Size(134, 40);
            this.lblPtname1.TabIndex = 4;
            this.lblPtname1.Text = "홍길동동";
            this.lblPtname1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPtno1
            // 
            this.lblPtno1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPtno1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPtno1.ForeColor = System.Drawing.Color.Black;
            this.lblPtno1.Location = new System.Drawing.Point(289, 8);
            this.lblPtno1.Name = "lblPtno1";
            this.lblPtno1.Size = new System.Drawing.Size(171, 40);
            this.lblPtno1.TabIndex = 3;
            this.lblPtno1.Text = "99999999";
            this.lblPtno1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCpName1
            // 
            this.lblCpName1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCpName1.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCpName1.ForeColor = System.Drawing.Color.Blue;
            this.lblCpName1.Location = new System.Drawing.Point(79, 8);
            this.lblCpName1.Name = "lblCpName1";
            this.lblCpName1.Size = new System.Drawing.Size(210, 40);
            this.lblCpName1.TabIndex = 2;
            this.lblCpName1.Text = "소아장중츱증";
            this.lblCpName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panInfo0
            // 
            this.panInfo0.Controls.Add(this.lblDrugName0);
            this.panInfo0.Controls.Add(this.lblAge0);
            this.panInfo0.Controls.Add(this.lblSex0);
            this.panInfo0.Controls.Add(this.lblPtname0);
            this.panInfo0.Controls.Add(this.lblPtno0);
            this.panInfo0.Controls.Add(this.lblCpName0);
            this.panInfo0.Location = new System.Drawing.Point(12, 272);
            this.panInfo0.Name = "panInfo0";
            this.panInfo0.Size = new System.Drawing.Size(1166, 57);
            this.panInfo0.TabIndex = 3;
            // 
            // lblDrugName0
            // 
            this.lblDrugName0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDrugName0.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDrugName0.ForeColor = System.Drawing.Color.Blue;
            this.lblDrugName0.Location = new System.Drawing.Point(724, 8);
            this.lblDrugName0.Name = "lblDrugName0";
            this.lblDrugName0.Size = new System.Drawing.Size(347, 40);
            this.lblDrugName0.TabIndex = 7;
            this.lblDrugName0.Text = "약제";
            this.lblDrugName0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAge0
            // 
            this.lblAge0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAge0.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAge0.ForeColor = System.Drawing.Color.Black;
            this.lblAge0.Location = new System.Drawing.Point(659, 8);
            this.lblAge0.Name = "lblAge0";
            this.lblAge0.Size = new System.Drawing.Size(65, 40);
            this.lblAge0.TabIndex = 6;
            this.lblAge0.Text = "999";
            this.lblAge0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSex0
            // 
            this.lblSex0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSex0.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSex0.ForeColor = System.Drawing.Color.Black;
            this.lblSex0.Location = new System.Drawing.Point(594, 8);
            this.lblSex0.Name = "lblSex0";
            this.lblSex0.Size = new System.Drawing.Size(65, 40);
            this.lblSex0.TabIndex = 5;
            this.lblSex0.Text = "남";
            this.lblSex0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPtname0
            // 
            this.lblPtname0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPtname0.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPtname0.ForeColor = System.Drawing.Color.Blue;
            this.lblPtname0.Location = new System.Drawing.Point(460, 8);
            this.lblPtname0.Name = "lblPtname0";
            this.lblPtname0.Size = new System.Drawing.Size(134, 40);
            this.lblPtname0.TabIndex = 4;
            this.lblPtname0.Text = "홍길동동";
            this.lblPtname0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPtno0
            // 
            this.lblPtno0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPtno0.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPtno0.ForeColor = System.Drawing.Color.Black;
            this.lblPtno0.Location = new System.Drawing.Point(289, 8);
            this.lblPtno0.Name = "lblPtno0";
            this.lblPtno0.Size = new System.Drawing.Size(171, 40);
            this.lblPtno0.TabIndex = 3;
            this.lblPtno0.Text = "99999999";
            this.lblPtno0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCpName0
            // 
            this.lblCpName0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCpName0.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCpName0.ForeColor = System.Drawing.Color.Blue;
            this.lblCpName0.Location = new System.Drawing.Point(79, 8);
            this.lblCpName0.Name = "lblCpName0";
            this.lblCpName0.Size = new System.Drawing.Size(210, 40);
            this.lblCpName0.TabIndex = 2;
            this.lblCpName0.Text = "Stroke";
            this.lblCpName0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(58, 532);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(1079, 75);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "확               인";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmOcsCpPopUpMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 651);
            this.ControlBox = false;
            this.Controls.Add(this.panCP);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmOcsCpPopUpMsg";
            this.Text = "frmOcsCpPopUpMsg";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmOcsCpPopUpMsg_Load);
            this.Resize += new System.EventHandler(this.frmOcsCpPopUpMsg_Resize);
            this.panCP.ResumeLayout(false);
            this.panCP.PerformLayout();
            this.panInfo1.ResumeLayout(false);
            this.panInfo0.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCP;
        private System.Windows.Forms.Panel panCP;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panInfo1;
        private System.Windows.Forms.Label lblDrugName1;
        private System.Windows.Forms.Label lblAge1;
        private System.Windows.Forms.Label lblSex1;
        private System.Windows.Forms.Label lblPtname1;
        private System.Windows.Forms.Label lblPtno1;
        private System.Windows.Forms.Label lblCpName1;
        private System.Windows.Forms.Panel panInfo0;
        private System.Windows.Forms.Label lblDrugName0;
        private System.Windows.Forms.Label lblAge0;
        private System.Windows.Forms.Label lblSex0;
        private System.Windows.Forms.Label lblPtname0;
        private System.Windows.Forms.Label lblPtno0;
        private System.Windows.Forms.Label lblCpName0;
    }
}