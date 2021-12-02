namespace ComLibB
{
    partial class FrmMedPregnantOrder
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOrder = new System.Windows.Forms.Button();
            this.txtPregnancyWeek = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOrdName = new System.Windows.Forms.Label();
            this.txtOrdCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(684, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "※ 임산부 초음파 코드 입력 시 임신주수는 필수 입력 항목입니다. 임신주수를 반드시 입력해 주시기 바랍니다.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnOrder);
            this.panel1.Controls.Add(this.txtPregnancyWeek);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblOrdName);
            this.panel1.Controls.Add(this.txtOrdCode);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(7, 35);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(962, 58);
            this.panel1.TabIndex = 1;
            // 
            // btnOrder
            // 
            this.btnOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrder.BackColor = System.Drawing.Color.White;
            this.btnOrder.ForeColor = System.Drawing.Color.Black;
            this.btnOrder.Location = new System.Drawing.Point(781, 13);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(167, 30);
            this.btnOrder.TabIndex = 27;
            this.btnOrder.Text = "입력 완료";
            this.btnOrder.UseVisualStyleBackColor = false;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // txtPregnancyWeek
            // 
            this.txtPregnancyWeek.Location = new System.Drawing.Point(710, 15);
            this.txtPregnancyWeek.MaxLength = 2;
            this.txtPregnancyWeek.Name = "txtPregnancyWeek";
            this.txtPregnancyWeek.Size = new System.Drawing.Size(60, 25);
            this.txtPregnancyWeek.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(632, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "(숫자만 입력)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(634, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "임신주수";
            // 
            // lblOrdName
            // 
            this.lblOrdName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrdName.Location = new System.Drawing.Point(208, 15);
            this.lblOrdName.Name = "lblOrdName";
            this.lblOrdName.Size = new System.Drawing.Size(420, 24);
            this.lblOrdName.TabIndex = 2;
            this.lblOrdName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOrdCode
            // 
            this.txtOrdCode.Location = new System.Drawing.Point(86, 15);
            this.txtOrdCode.Name = "txtOrdCode";
            this.txtOrdCode.Size = new System.Drawing.Size(118, 25);
            this.txtOrdCode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "처방코드";
            // 
            // FrmMedPregnantOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(976, 99);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMedPregnantOrder";
            this.Text = "임산부 초음파 코드 입력 및 참고사항";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMedPregnantOrder_FormClosing);
            this.Load += new System.EventHandler(this.FrmMedPregnantOrder_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPregnancyWeek;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOrdName;
        private System.Windows.Forms.TextBox txtOrdCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOrder;
    }
}