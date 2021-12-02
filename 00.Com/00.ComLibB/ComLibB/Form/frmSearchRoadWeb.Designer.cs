namespace ComLibB
{
    partial class frmSearchRoadWeb
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panHeader = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblCrrPage = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lblDoum3 = new System.Windows.Forms.Label();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.rdoZipName17 = new System.Windows.Forms.RadioButton();
            this.rdoZipName16 = new System.Windows.Forms.RadioButton();
            this.rdoZipName15 = new System.Windows.Forms.RadioButton();
            this.rdoZipName14 = new System.Windows.Forms.RadioButton();
            this.rdoZipName13 = new System.Windows.Forms.RadioButton();
            this.rdoZipName12 = new System.Windows.Forms.RadioButton();
            this.rdoZipName11 = new System.Windows.Forms.RadioButton();
            this.rdoZipName10 = new System.Windows.Forms.RadioButton();
            this.rdoZipName09 = new System.Windows.Forms.RadioButton();
            this.rdoZipName08 = new System.Windows.Forms.RadioButton();
            this.rdoZipName07 = new System.Windows.Forms.RadioButton();
            this.rdoZipName06 = new System.Windows.Forms.RadioButton();
            this.rdoZipName05 = new System.Windows.Forms.RadioButton();
            this.rdoZipName04 = new System.Windows.Forms.RadioButton();
            this.rdoZipName03 = new System.Windows.Forms.RadioButton();
            this.rdoZipName02 = new System.Windows.Forms.RadioButton();
            this.rdoZipName01 = new System.Windows.Forms.RadioButton();
            this.rdoZipName00 = new System.Windows.Forms.RadioButton();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub2 = new System.Windows.Forms.Panel();
            this.lblTitleSub2 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panHeader.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panTitleSub2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1264, 37);
            this.panTitle.TabIndex = 76;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(1170, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(284, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "환자 주소검색 (도로명 시스템 연동)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panHeader
            // 
            this.panHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panHeader.Controls.Add(this.label4);
            this.panHeader.Controls.Add(this.btnSearch);
            this.panHeader.Controls.Add(this.txtSearch);
            this.panHeader.Controls.Add(this.label2);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.Location = new System.Drawing.Point(0, 37);
            this.panHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(1264, 44);
            this.panHeader.TabIndex = 79;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(502, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "( 페이지 당 100 건까지 조회가능)";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(409, 6);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(76, 25);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "검색";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSearch.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtSearch.Location = new System.Drawing.Point(77, 7);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(326, 25);
            this.txtSearch.TabIndex = 13;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(11, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "도로명 : ";
            // 
            // panSub01
            // 
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.lblTotalCount);
            this.panSub01.Controls.Add(this.lblPage);
            this.panSub01.Controls.Add(this.btnPrev);
            this.panSub01.Controls.Add(this.btnNext);
            this.panSub01.Controls.Add(this.lblCrrPage);
            this.panSub01.Controls.Add(this.lblTotal);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 81);
            this.panSub01.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1264, 38);
            this.panSub01.TabIndex = 80;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTotalCount.Location = new System.Drawing.Point(74, 9);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(56, 17);
            this.lblTotalCount.TabIndex = 82;
            this.lblTotalCount.Text = "1,222건";
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPage.Location = new System.Drawing.Point(287, 9);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(51, 17);
            this.lblPage.TabIndex = 81;
            this.lblPage.Text = "5 Page";
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrev.Location = new System.Drawing.Point(1081, 0);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(90, 36);
            this.btnPrev.TabIndex = 80;
            this.btnPrev.Text = "이전";
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNext.Location = new System.Drawing.Point(1171, 0);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(91, 36);
            this.btnNext.TabIndex = 79;
            this.btnNext.Text = "다음";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // lblCrrPage
            // 
            this.lblCrrPage.AutoSize = true;
            this.lblCrrPage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCrrPage.Location = new System.Drawing.Point(190, 9);
            this.lblCrrPage.Name = "lblCrrPage";
            this.lblCrrPage.Size = new System.Drawing.Size(91, 17);
            this.lblCrrPage.TabIndex = 1;
            this.lblCrrPage.Text = "현재 페이지 : ";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTotal.Location = new System.Drawing.Point(13, 9);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(55, 17);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "총 건수:";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lblDoum3);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 830);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1264, 31);
            this.panel8.TabIndex = 83;
            // 
            // lblDoum3
            // 
            this.lblDoum3.AutoSize = true;
            this.lblDoum3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDoum3.ForeColor = System.Drawing.Color.Blue;
            this.lblDoum3.Location = new System.Drawing.Point(377, 10);
            this.lblDoum3.Name = "lblDoum3";
            this.lblDoum3.Size = new System.Drawing.Size(215, 12);
            this.lblDoum3.TabIndex = 0;
            this.lblDoum3.Text = "선택후 더블클릭하시면 선택됩니다.";
            // 
            // panSub02
            // 
            this.panSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub02.Controls.Add(this.rdoZipName17);
            this.panSub02.Controls.Add(this.rdoZipName16);
            this.panSub02.Controls.Add(this.rdoZipName15);
            this.panSub02.Controls.Add(this.rdoZipName14);
            this.panSub02.Controls.Add(this.rdoZipName13);
            this.panSub02.Controls.Add(this.rdoZipName12);
            this.panSub02.Controls.Add(this.rdoZipName11);
            this.panSub02.Controls.Add(this.rdoZipName10);
            this.panSub02.Controls.Add(this.rdoZipName09);
            this.panSub02.Controls.Add(this.rdoZipName08);
            this.panSub02.Controls.Add(this.rdoZipName07);
            this.panSub02.Controls.Add(this.rdoZipName06);
            this.panSub02.Controls.Add(this.rdoZipName05);
            this.panSub02.Controls.Add(this.rdoZipName04);
            this.panSub02.Controls.Add(this.rdoZipName03);
            this.panSub02.Controls.Add(this.rdoZipName02);
            this.panSub02.Controls.Add(this.rdoZipName01);
            this.panSub02.Controls.Add(this.rdoZipName00);
            this.panSub02.Controls.Add(this.panTitleSub1);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(0, 119);
            this.panSub02.Name = "panSub02";
            this.panSub02.Padding = new System.Windows.Forms.Padding(1);
            this.panSub02.Size = new System.Drawing.Size(132, 711);
            this.panSub02.TabIndex = 85;
            // 
            // rdoZipName17
            // 
            this.rdoZipName17.AutoSize = true;
            this.rdoZipName17.Location = new System.Drawing.Point(13, 445);
            this.rdoZipName17.Name = "rdoZipName17";
            this.rdoZipName17.Size = new System.Drawing.Size(52, 21);
            this.rdoZipName17.TabIndex = 18;
            this.rdoZipName17.Text = "전체";
            this.rdoZipName17.UseVisualStyleBackColor = true;
            // 
            // rdoZipName16
            // 
            this.rdoZipName16.AutoSize = true;
            this.rdoZipName16.Location = new System.Drawing.Point(13, 418);
            this.rdoZipName16.Name = "rdoZipName16";
            this.rdoZipName16.Size = new System.Drawing.Size(78, 21);
            this.rdoZipName16.TabIndex = 17;
            this.rdoZipName16.Text = "충청남도";
            this.rdoZipName16.UseVisualStyleBackColor = true;
            // 
            // rdoZipName15
            // 
            this.rdoZipName15.AutoSize = true;
            this.rdoZipName15.Location = new System.Drawing.Point(13, 394);
            this.rdoZipName15.Name = "rdoZipName15";
            this.rdoZipName15.Size = new System.Drawing.Size(78, 21);
            this.rdoZipName15.TabIndex = 16;
            this.rdoZipName15.Text = "충청북도";
            this.rdoZipName15.UseVisualStyleBackColor = true;
            // 
            // rdoZipName14
            // 
            this.rdoZipName14.AutoSize = true;
            this.rdoZipName14.Location = new System.Drawing.Point(13, 370);
            this.rdoZipName14.Name = "rdoZipName14";
            this.rdoZipName14.Size = new System.Drawing.Size(117, 21);
            this.rdoZipName14.TabIndex = 15;
            this.rdoZipName14.Text = "제주특별자치도";
            this.rdoZipName14.UseVisualStyleBackColor = true;
            // 
            // rdoZipName13
            // 
            this.rdoZipName13.AutoSize = true;
            this.rdoZipName13.Location = new System.Drawing.Point(13, 346);
            this.rdoZipName13.Name = "rdoZipName13";
            this.rdoZipName13.Size = new System.Drawing.Size(78, 21);
            this.rdoZipName13.TabIndex = 14;
            this.rdoZipName13.Text = "전라남도";
            this.rdoZipName13.UseVisualStyleBackColor = true;
            // 
            // rdoZipName12
            // 
            this.rdoZipName12.AutoSize = true;
            this.rdoZipName12.Location = new System.Drawing.Point(13, 322);
            this.rdoZipName12.Name = "rdoZipName12";
            this.rdoZipName12.Size = new System.Drawing.Size(78, 21);
            this.rdoZipName12.TabIndex = 13;
            this.rdoZipName12.Text = "전라북도";
            this.rdoZipName12.UseVisualStyleBackColor = true;
            // 
            // rdoZipName11
            // 
            this.rdoZipName11.AutoSize = true;
            this.rdoZipName11.Location = new System.Drawing.Point(13, 298);
            this.rdoZipName11.Name = "rdoZipName11";
            this.rdoZipName11.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName11.TabIndex = 12;
            this.rdoZipName11.Text = "인천광역시";
            this.rdoZipName11.UseVisualStyleBackColor = true;
            // 
            // rdoZipName10
            // 
            this.rdoZipName10.AutoSize = true;
            this.rdoZipName10.Location = new System.Drawing.Point(13, 274);
            this.rdoZipName10.Name = "rdoZipName10";
            this.rdoZipName10.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName10.TabIndex = 11;
            this.rdoZipName10.Text = "울산광역시";
            this.rdoZipName10.UseVisualStyleBackColor = true;
            // 
            // rdoZipName09
            // 
            this.rdoZipName09.AutoSize = true;
            this.rdoZipName09.Location = new System.Drawing.Point(13, 250);
            this.rdoZipName09.Name = "rdoZipName09";
            this.rdoZipName09.Size = new System.Drawing.Size(117, 21);
            this.rdoZipName09.TabIndex = 10;
            this.rdoZipName09.Text = "세종특별자치시";
            this.rdoZipName09.UseVisualStyleBackColor = true;
            // 
            // rdoZipName08
            // 
            this.rdoZipName08.AutoSize = true;
            this.rdoZipName08.Location = new System.Drawing.Point(13, 226);
            this.rdoZipName08.Name = "rdoZipName08";
            this.rdoZipName08.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName08.TabIndex = 9;
            this.rdoZipName08.Text = "서울특별시";
            this.rdoZipName08.UseVisualStyleBackColor = true;
            // 
            // rdoZipName07
            // 
            this.rdoZipName07.AutoSize = true;
            this.rdoZipName07.Location = new System.Drawing.Point(13, 202);
            this.rdoZipName07.Name = "rdoZipName07";
            this.rdoZipName07.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName07.TabIndex = 8;
            this.rdoZipName07.Text = "부산광역시";
            this.rdoZipName07.UseVisualStyleBackColor = true;
            // 
            // rdoZipName06
            // 
            this.rdoZipName06.AutoSize = true;
            this.rdoZipName06.Location = new System.Drawing.Point(13, 178);
            this.rdoZipName06.Name = "rdoZipName06";
            this.rdoZipName06.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName06.TabIndex = 7;
            this.rdoZipName06.Text = "대전광역시";
            this.rdoZipName06.UseVisualStyleBackColor = true;
            // 
            // rdoZipName05
            // 
            this.rdoZipName05.AutoSize = true;
            this.rdoZipName05.Location = new System.Drawing.Point(13, 154);
            this.rdoZipName05.Name = "rdoZipName05";
            this.rdoZipName05.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName05.TabIndex = 6;
            this.rdoZipName05.Text = "대구광역시";
            this.rdoZipName05.UseVisualStyleBackColor = true;
            // 
            // rdoZipName04
            // 
            this.rdoZipName04.AutoSize = true;
            this.rdoZipName04.Location = new System.Drawing.Point(13, 130);
            this.rdoZipName04.Name = "rdoZipName04";
            this.rdoZipName04.Size = new System.Drawing.Size(91, 21);
            this.rdoZipName04.TabIndex = 5;
            this.rdoZipName04.Text = "광주광역시";
            this.rdoZipName04.UseVisualStyleBackColor = true;
            // 
            // rdoZipName03
            // 
            this.rdoZipName03.AutoSize = true;
            this.rdoZipName03.Location = new System.Drawing.Point(13, 106);
            this.rdoZipName03.Name = "rdoZipName03";
            this.rdoZipName03.Size = new System.Drawing.Size(78, 21);
            this.rdoZipName03.TabIndex = 4;
            this.rdoZipName03.Text = "경상남도";
            this.rdoZipName03.UseVisualStyleBackColor = true;
            // 
            // rdoZipName02
            // 
            this.rdoZipName02.AutoSize = true;
            this.rdoZipName02.Checked = true;
            this.rdoZipName02.Location = new System.Drawing.Point(13, 82);
            this.rdoZipName02.Name = "rdoZipName02";
            this.rdoZipName02.Size = new System.Drawing.Size(78, 21);
            this.rdoZipName02.TabIndex = 3;
            this.rdoZipName02.TabStop = true;
            this.rdoZipName02.Text = "경상북도";
            this.rdoZipName02.UseVisualStyleBackColor = true;
            // 
            // rdoZipName01
            // 
            this.rdoZipName01.AutoSize = true;
            this.rdoZipName01.Location = new System.Drawing.Point(13, 58);
            this.rdoZipName01.Name = "rdoZipName01";
            this.rdoZipName01.Size = new System.Drawing.Size(65, 21);
            this.rdoZipName01.TabIndex = 2;
            this.rdoZipName01.Text = "강원도";
            this.rdoZipName01.UseVisualStyleBackColor = true;
            // 
            // rdoZipName00
            // 
            this.rdoZipName00.AutoSize = true;
            this.rdoZipName00.Location = new System.Drawing.Point(13, 34);
            this.rdoZipName00.Name = "rdoZipName00";
            this.rdoZipName00.Size = new System.Drawing.Size(65, 21);
            this.rdoZipName00.TabIndex = 1;
            this.rdoZipName00.Tag = "0";
            this.rdoZipName00.Text = "경기도";
            this.rdoZipName00.UseVisualStyleBackColor = true;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(1, 1);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(128, 28);
            this.panTitleSub1.TabIndex = 0;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(31, 12);
            this.lblTitleSub1.TabIndex = 24;
            this.lblTitleSub1.Text = "지역";
            this.lblTitleSub1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.SS1);
            this.panel1.Controls.Add(this.panTitleSub2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(132, 119);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(1132, 711);
            this.panel1.TabIndex = 86;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.Location = new System.Drawing.Point(1, 29);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1128, 679);
            this.SS1.TabIndex = 89;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panTitleSub2
            // 
            this.panTitleSub2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub2.Controls.Add(this.lblTitleSub2);
            this.panTitleSub2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub2.Location = new System.Drawing.Point(1, 1);
            this.panTitleSub2.Name = "panTitleSub2";
            this.panTitleSub2.Size = new System.Drawing.Size(1128, 28);
            this.panTitleSub2.TabIndex = 88;
            // 
            // lblTitleSub2
            // 
            this.lblTitleSub2.AutoSize = true;
            this.lblTitleSub2.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub2.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub2.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub2.Name = "lblTitleSub2";
            this.lblTitleSub2.Size = new System.Drawing.Size(77, 12);
            this.lblTitleSub2.TabIndex = 25;
            this.lblTitleSub2.Text = "   검색 결과";
            this.lblTitleSub2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSearchRoadWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 861);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panSub02);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panHeader);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSearchRoadWeb";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "주소 통합검색";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panSub02.ResumeLayout(false);
            this.panSub02.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panTitleSub2.ResumeLayout(false);
            this.panTitleSub2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panHeader;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblCrrPage;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label lblDoum3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.RadioButton rdoZipName16;
        private System.Windows.Forms.RadioButton rdoZipName15;
        private System.Windows.Forms.RadioButton rdoZipName14;
        private System.Windows.Forms.RadioButton rdoZipName13;
        private System.Windows.Forms.RadioButton rdoZipName12;
        private System.Windows.Forms.RadioButton rdoZipName11;
        private System.Windows.Forms.RadioButton rdoZipName10;
        private System.Windows.Forms.RadioButton rdoZipName09;
        private System.Windows.Forms.RadioButton rdoZipName08;
        private System.Windows.Forms.RadioButton rdoZipName07;
        private System.Windows.Forms.RadioButton rdoZipName06;
        private System.Windows.Forms.RadioButton rdoZipName05;
        private System.Windows.Forms.RadioButton rdoZipName04;
        private System.Windows.Forms.RadioButton rdoZipName03;
        private System.Windows.Forms.RadioButton rdoZipName02;
        private System.Windows.Forms.RadioButton rdoZipName01;
        private System.Windows.Forms.RadioButton rdoZipName00;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Panel panTitleSub2;
        private System.Windows.Forms.Label lblTitleSub2;
        private System.Windows.Forms.RadioButton rdoZipName17;
    }
}

