namespace HC_IF
{
    partial class frmHcKiosk_Verify
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
            this.panInfo = new DevComponents.DotNetBar.PanelEx();
            this.lblHphone = new DevComponents.DotNetBar.LabelX();
            this.lblJuso = new DevComponents.DotNetBar.LabelX();
            this.btnNo = new DevComponents.DotNetBar.ButtonX();
            this.btnYes = new DevComponents.DotNetBar.ButtonX();
            this.lblTel = new DevComponents.DotNetBar.LabelX();
            this.lblInfo2 = new DevComponents.DotNetBar.LabelX();
            this.lblInfo1 = new DevComponents.DotNetBar.LabelX();
            this.panWait = new System.Windows.Forms.Panel();
            this.Progress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.label1 = new System.Windows.Forms.Label();
            this.panInfo.SuspendLayout();
            this.panWait.SuspendLayout();
            this.SuspendLayout();
            // 
            // panInfo
            // 
            this.panInfo.CanvasColor = System.Drawing.SystemColors.Control;
            this.panInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panInfo.Controls.Add(this.panWait);
            this.panInfo.Controls.Add(this.lblHphone);
            this.panInfo.Controls.Add(this.lblJuso);
            this.panInfo.Controls.Add(this.btnNo);
            this.panInfo.Controls.Add(this.btnYes);
            this.panInfo.Controls.Add(this.lblTel);
            this.panInfo.Controls.Add(this.lblInfo2);
            this.panInfo.Controls.Add(this.lblInfo1);
            this.panInfo.DisabledBackColor = System.Drawing.Color.Empty;
            this.panInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panInfo.Location = new System.Drawing.Point(0, 0);
            this.panInfo.Name = "panInfo";
            this.panInfo.Size = new System.Drawing.Size(794, 489);
            this.panInfo.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panInfo.Style.BackColor1.Color = System.Drawing.Color.LightBlue;
            this.panInfo.Style.BackColor2.Color = System.Drawing.Color.SteelBlue;
            this.panInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panInfo.Style.GradientAngle = 90;
            this.panInfo.TabIndex = 0;
            // 
            // lblHphone
            // 
            // 
            // 
            // 
            this.lblHphone.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblHphone.Font = new System.Drawing.Font("굴림", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHphone.Location = new System.Drawing.Point(51, 316);
            this.lblHphone.Name = "lblHphone";
            this.lblHphone.Size = new System.Drawing.Size(698, 37);
            this.lblHphone.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.lblHphone.TabIndex = 6;
            this.lblHphone.Text = "010-1324-4567";
            this.lblHphone.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblJuso
            // 
            // 
            // 
            // 
            this.lblJuso.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblJuso.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblJuso.Location = new System.Drawing.Point(12, 262);
            this.lblJuso.Name = "lblJuso";
            this.lblJuso.Size = new System.Drawing.Size(769, 55);
            this.lblJuso.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.lblJuso.TabIndex = 5;
            this.lblJuso.Text = "경북 포항시 남구 대잠동 270-1번지 포항성모병원 3층 의료정보과";
            this.lblJuso.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // btnNo
            // 
            this.btnNo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNo.Font = new System.Drawing.Font("나눔고딕", 33.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNo.Location = new System.Drawing.Point(435, 376);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(262, 85);
            this.btnNo.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "아니오 (No)";
            // 
            // btnYes
            // 
            this.btnYes.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnYes.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnYes.Font = new System.Drawing.Font("나눔고딕", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnYes.Location = new System.Drawing.Point(105, 376);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(262, 85);
            this.btnYes.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "예 (Yes)";
            // 
            // lblTel
            // 
            // 
            // 
            // 
            this.lblTel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblTel.Font = new System.Drawing.Font("맑은 고딕", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTel.ForeColor = System.Drawing.Color.Blue;
            this.lblTel.Location = new System.Drawing.Point(51, 122);
            this.lblTel.Name = "lblTel";
            this.lblTel.Size = new System.Drawing.Size(698, 56);
            this.lblTel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.lblTel.TabIndex = 2;
            this.lblTel.Text = "010-1234-4567";
            this.lblTel.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblInfo2
            // 
            // 
            // 
            // 
            this.lblInfo2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInfo2.Font = new System.Drawing.Font("맑은 고딕", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInfo2.Location = new System.Drawing.Point(51, 172);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(698, 84);
            this.lblInfo2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.lblInfo2.TabIndex = 1;
            this.lblInfo2.Text = "123456-1******";
            this.lblInfo2.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // lblInfo1
            // 
            // 
            // 
            // 
            this.lblInfo1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInfo1.Font = new System.Drawing.Font("맑은 고딕", 44.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInfo1.Location = new System.Drawing.Point(51, 32);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(698, 84);
            this.lblInfo1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.lblInfo1.TabIndex = 0;
            this.lblInfo1.Text = "홍길동전님이 맞습니까?";
            this.lblInfo1.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // panWait
            // 
            this.panWait.Controls.Add(this.label1);
            this.panWait.Controls.Add(this.Progress);
            this.panWait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panWait.Location = new System.Drawing.Point(0, 0);
            this.panWait.Name = "panWait";
            this.panWait.Size = new System.Drawing.Size(794, 489);
            this.panWait.TabIndex = 7;
            // 
            // Progress
            // 
            // 
            // 
            // 
            this.Progress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Progress.Location = new System.Drawing.Point(213, 151);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(343, 268);
            this.Progress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.Progress.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(99, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(572, 36);
            this.label1.TabIndex = 1;
            this.label1.Text = "접수처리 중입니다. 잠시만 기다려주세요.";
            // 
            // frmHcKiosk_Verify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 489);
            this.Controls.Add(this.panInfo);
            this.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHcKiosk_Verify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "본인확인";
            this.panInfo.ResumeLayout(false);
            this.panWait.ResumeLayout(false);
            this.panWait.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panInfo;
        private DevComponents.DotNetBar.ButtonX btnYes;
        private DevComponents.DotNetBar.LabelX lblTel;
        private DevComponents.DotNetBar.LabelX lblInfo2;
        private DevComponents.DotNetBar.LabelX lblInfo1;
        private DevComponents.DotNetBar.ButtonX btnNo;
        private DevComponents.DotNetBar.LabelX lblHphone;
        private DevComponents.DotNetBar.LabelX lblJuso;
        private System.Windows.Forms.Panel panWait;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.CircularProgress Progress;
    }
}