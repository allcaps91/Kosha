using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanSpcDiagnosisResultReport.cs
/// Description     : 특수건강진단결과표(생애포함)
/// Author          : 이상훈
/// Create Date     : 2020-03-06
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm특수건강진단표_2013.frm(Frm특수건강진단표_2013)" />
namespace ComHpcLibB
{
    public partial class frmHcPanSpcDiagnosisResultReport : Form
    {
        HicJepsuResSpecialLtdService hicJepsuResSpecialLtdService = null;
        HicJepsuService hicJepsuService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicLtdService hicLtdService = null;
        HicMcodeService hicMcodeService = null;
        HicSpcPanjengMcodeService hicSpcPanjengMcodeService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicSpcPanjengScodeService hicSpcPanjengScodeService = null;
        HicJepsuGundateService hicJepsuGundateService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill hcb = new clsHcBill();

        clsHcType cHT = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;
        
        frmHcMCodeExam FrmHcMCodeExam = null;
        frmHcSpcSCode FrmHcSpcSCode = null;
        frmHcPanSpcResultReportSetup FrmHcPanSpcResultReportSetup = null;

        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;

        long FnWRTNO;       //1차 접수번호
        long FnWrtno2;      //2차 접수번호

        long FnPano;
        string FstrSex;
        string FstrUCodes;
        string FstrJepDate;
        string FstrGjYear;
        string FstrGjBangi;
        string FstrGjJong;
        int FnUCodeBun;

        long FnRow;
        int FnDoctCnt;
        long[] FnPanDrNo = new long[10];
        string FstrFDate1;
        string FstrTDate1;
        string FstrFDate2;
        string FstrTDate2;

        int FnNotSecond;                        //2차미실시
        int FnMiPanjeng;                        //특검미판정
        int[,] FnJinCnt = new int[18, 18];      //건강진단현황
        int[,] FnYuCNT = new int[8, 3];         // 질병유소견자현황
        string[] FstrYuCode = new string[8];    //질병유질환코드
        int[,] FnJoCNT = new int[18, 8];        //조치현황

        public frmHcPanSpcDiagnosisResultReport()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuResSpecialLtdService = new HicJepsuResSpecialLtdService();
            hicJepsuService = new HicJepsuService();
            comHpcLibBService = new ComHpcLibBService();
            hicLtdService = new HicLtdService();
            hicMcodeService = new HicMcodeService();
            hicSpcPanjengMcodeService = new HicSpcPanjengMcodeService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicSpcPanjengScodeService = new HicSpcPanjengScodeService();
            hicJepsuGundateService = new HicJepsuGundateService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMenuInwon.Click += new EventHandler(eBtnClick);
            this.btnMenuBun.Click += new EventHandler(eBtnClick);
            this.btnMenuCode.Click += new EventHandler(eBtnClick);
            this.btnMenuCode2.Click += new EventHandler(eBtnClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            //this.txtLtdCode.Click += new EventHandler(eTxtClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtPrtDate.DoubleClick += new EventHandler(eTxtDblClick);
            //this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.cboBangi.KeyPress += new KeyPressEventHandler(eCboKeyPress);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboClick);
            this.rdoJob0.Click += new EventHandler(eRdoClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.rdoJob3.Click += new EventHandler(eRdoClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYEAR = 0;

            this.Location = new Point(10, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYEAR = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));
            //건진년도 Combo SET
            cboYear.Items.Clear();
            for (int i = 1; i <= 3; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYEAR));
                nYEAR -= 1;
            }
            cboYear.SelectedIndex = 0;

            //인쇄매수
            cboPrtCnt.Items.Clear();
            cboPrtCnt.Items.Add("1");
            cboPrtCnt.Items.Add("2");
            cboPrtCnt.Items.Add("3");
            cboPrtCnt.SelectedIndex = 0;

            //반기
            cboBangi.Items.Clear();
            cboBangi.Items.Add("전체");
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");
            cboBangi.SelectedIndex = 0;

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            lblLtdName2.Text = "";
            lblGigan.Text = "";
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                string strJepDate2 = "";
                string strFrDate = "";
                string strToDate = "";
                string strJob = "";
                string strGjYear = "";

                ssList.Enabled = false;

                Cursor.Current = Cursors.WaitCursor;

                fn_Screen_Clear();

                switch (cboBangi.Text)
                {
                    case "상반기":
                        FstrGjBangi = "1";
                        break;
                    case "하반기":
                        FstrGjBangi = "2";
                        break;
                    default:
                        FstrGjBangi = "전체";
                        break;
                }

                if (rdoJob0.Checked)
                {
                    strJob = "0";
                }
                else if (rdoJob1.Checked)
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked)
                {
                    strJob = "2";
                }
                else if (rdoJob3.Checked)
                {
                    strJob = "3";
                }

                strGjYear = cboYear.Text.Trim();

                lblLtdName2.Text = ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                FstrLtdCode = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                FstrFDate = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                FstrTDate = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                txtLtdCode.Text = FstrLtdCode + "." + hb.READ_Ltd_Name(FstrLtdCode);

                List<HIC_JEPSU_GUNDATE> list = hicJepsuGundateService.GetItembyInLineWrtNo(FstrFDate, FstrTDate, strJob, FstrGjBangi, strGjYear, FstrLtdCode);

                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[i].GUNDATE.IsNullOrEmpty())
                    {
                        strJepDate2 = list[i].GUNDATE;
                        if (strFrDate.IsNullOrEmpty()) strFrDate = strJepDate2;
                        if (string.Compare(strFrDate, strJepDate2) >  0) strFrDate = strJepDate2;
                        if (strFrDate.IsNullOrEmpty()) strToDate = strJepDate2;
                        if (string.Compare(strToDate, strJepDate2) < 0) strToDate = strJepDate2;
                    }
                    else
                    {
                        strJepDate2 = list[i].JEPDATE;
                        if (strFrDate.IsNullOrEmpty()) strFrDate = strJepDate2;
                        if (string.Compare(strFrDate, strJepDate2) > 0) strFrDate = strJepDate2;
                        if (strToDate.IsNullOrEmpty()) strToDate = strJepDate2;
                        if (string.Compare(strToDate, strJepDate2) < 0) strToDate = strJepDate2;
                    }
                }
                lblGigan.Text = strFrDate + " ~ " + strToDate;
                fn_Screen_Display();

                Cursor.Current = Cursors.Default;
                ssList.Enabled = true;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                int nRow = 0;
                long nTOT;
                string strFrDate = "";
                string strToDate = "";
                string strLtdCode = "";
                string strJob = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1);
                if (rdoJob0.Checked)
                {
                    strJob = "1";
                }
                else if (rdoJob1.Checked)
                {
                    strJob = "2";
                }
                else if (rdoJob2.Checked)
                {
                    strJob = "3";
                }
                else if (rdoJob3.Checked)
                {
                    strJob = "4";
                }

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 30;
                ssList.Enabled = false;

                FstrGjYear = cboYear.Text.Trim();
                switch (cboBangi.Text.Trim())
                {
                    case "상반기":
                        FstrGjBangi = "1";
                        break;
                    case "하반기":
                        FstrGjBangi = "2";
                        break;
                    default:
                        FstrGjBangi = "전체";
                        break;
                }

                //자료를 SELECT
                List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItemCountbyJepDateGjYearGjBangi(strFrDate, strToDate, FstrGjYear, FstrGjBangi, strJob, strLtdCode);

                nRead = list.Count;
                ssList.ActiveSheet.RowCount = nRead;
                nRow = 0;
                nTOT = 0;
                progressBar1.Maximum = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    nTOT += list[i].CNT;
                    nRow += 1;
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].CNT.To<string>();
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].LTDCODE.To<string>();
                    ssList.ActiveSheet.Cells[i, 3].Text = list[i].MINDATE;
                    ssList.ActiveSheet.Cells[i, 4].Text = list[i].MAXDATE;
                    progressBar1.Value = i + 1;
                }
                ssList.Enabled = true;
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "[별지 제 85 호 서식]";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

                for (int i = 0; i < int.Parse(cboPrtCnt.Text); i++) //인쇄매수
                {
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }
            }
            else if (sender == btnMenuBun)
            {
                FrmHcMCodeExam = new frmHcMCodeExam();
                FrmHcMCodeExam.ShowDialog(this);
            }
            else if (sender == btnMenuCode)
            {
                FrmHcSpcSCode = new frmHcSpcSCode();
                FrmHcSpcSCode.ShowDialog(this);
            }
            else if (sender == btnMenuCode2)
            {
                FrmHcPanSpcResultReportSetup = new frmHcPanSpcResultReportSetup();
                FrmHcPanSpcResultReportSetup.ShowDialog(this);
            }
            else if (sender == btnMenuInwon)
            {
                //HEMS 전송시 수동으로 입력후 사용하던 폼(개발 제외(자동집계)))
                //FrmLtdInwon2.Show 1;
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtLtdCode)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                        //SendKeys.Send("{TAB}");
                    }
                }
            }
        }

        void eCboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == cboBangi)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == cboYear)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
            }
        }

        void eRdoClick(object sender, EventArgs e)
        { 
            if (sender == rdoJob0)
            {
                SS1.ActiveSheet.Cells[1, 7].ForeColor = Color.FromArgb(128, 0, 128);
                SS1.ActiveSheet.Cells[1, 7].BackColor = Color.FromArgb(0, 0, 0);
                SS1.ActiveSheet.Cells[3, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[3, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[5, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[5, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[7, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[7, 7].BackColor = Color.FromArgb(255, 255, 255);
            }
            else if (sender == rdoJob1)
            {
                SS1.ActiveSheet.Cells[1, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[1, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[3, 7].ForeColor = Color.FromArgb(128, 0, 128);
                SS1.ActiveSheet.Cells[3, 7].BackColor = Color.FromArgb(0, 0, 0);
                SS1.ActiveSheet.Cells[5, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[5, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[7, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[7, 7].BackColor = Color.FromArgb(255, 255, 255);
            }
            else if (sender == rdoJob2)
            {
                SS1.ActiveSheet.Cells[1, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[1, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[3, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[3, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[5, 7].ForeColor = Color.FromArgb(128, 0, 128);
                SS1.ActiveSheet.Cells[5, 7].BackColor = Color.FromArgb(0, 0, 0);
                SS1.ActiveSheet.Cells[7, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[7, 7].BackColor = Color.FromArgb(255, 255, 255);
            }
            else if (sender == rdoJob3)
            {
                SS1.ActiveSheet.Cells[1, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[1, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[3, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[3, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[5, 7].ForeColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[5, 7].BackColor = Color.FromArgb(255, 255, 255);
                SS1.ActiveSheet.Cells[7, 7].ForeColor = Color.FromArgb(128, 0, 128);
                SS1.ActiveSheet.Cells[7, 7].BackColor = Color.FromArgb(0, 0, 0);
            }
        }

        void eTxtDblClick(object sender, EventArgs e)
        {
            frmCalendar2 frmCalendar2X = new frmCalendar2();
            clsPublic.GstrCalDate = "";
            frmCalendar2X.ShowDialog();
            frmCalendar2X.Dispose();
            frmCalendar2X = null;

            txtPrtDate.Text = clsPublic.GstrCalDate;
        }

        public DESEASE_COUNT_MODEL Search(string FstrFDate, string FstrTDate, string year, long FstrLtdCode)
        {
            this.FstrFDate = FstrFDate;
            this.FstrTDate = FstrTDate;
            this.FstrLtdCode = FstrLtdCode.ToString();

            dtpFrDate.Value = DateUtil.stringToDateTime(FstrFDate, DateTimeType.YYYY_MM_DD);
            dtpToDate.Value = DateUtil.stringToDateTime(FstrTDate, DateTimeType.YYYY_MM_DD); ;

            txtLtdCode.Text = FstrLtdCode.ToString();

            this.FstrGjBangi = "";
            this.FstrGjYear = year;
            fn_Screen_Clear();
            fn_Screen_Display();

            DESEASE_COUNT_MODEL model = new DESEASE_COUNT_MODEL();
            model.C1 = SS1.ActiveSheet.Cells[54, 5].Text.To<int>(0) + SS1.ActiveSheet.Cells[55, 5].Text.To<int>(0); //요관찰자 직업병
            model.CN = SS1.ActiveSheet.Cells[56, 5].Text.To<int>(0) + SS1.ActiveSheet.Cells[57, 5].Text.To<int>(0); //요관찰자 야간
            model.C2 = SS1.ActiveSheet.Cells[58, 5].Text.To<int>(0) + SS1.ActiveSheet.Cells[59, 5].Text.To<int>(0);//요관찰자 일반질병

            model.D1 = SS1.ActiveSheet.Cells[45, 5].Text.To<int>(0) + SS1.ActiveSheet.Cells[46, 5].Text.To<int>(0);
            model.DN = SS1.ActiveSheet.Cells[47, 5].Text.To<int>(0) + SS1.ActiveSheet.Cells[48, 5].Text.To<int>(0);
            model.D2 = SS1.ActiveSheet.Cells[49, 5].Text.To<int>(0) + SS1.ActiveSheet.Cells[50, 5].Text.To<int>(0);

            return model;
        }


        void fn_Screen_Display()
        {
            int nRead = 0;
            int nRow = 0;
            int nCol = 0;
            string strUCodes = "";
            string strJepDate2 = "";
            string strWRTNO = "";
            string strFrDate = "";
            string strToDate = "";
            long nLtdCode = 0;
            string strJob = "";

            strWRTNO = "";
            FnDoctCnt = 0;
            for (int i = 0; i < 10; i++)
            {
                FnPanDrNo[i] = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                FstrYuCode[i] = "";
                FnYuCNT[i, 0] = 0;
                FnYuCNT[i, 1] = 0;
                FnYuCNT[i, 2] = 0;
            }

            FstrFDate1 = "";    FstrTDate1 = "";  //1차 검진기간
            FstrFDate2 = "";    FstrTDate2 = "";  //2차 검진기간
            FnNotSecond = 0;    //2차미실시
            FnMiPanjeng = 0;    //특검미판정
   
            //총근로자수,대상자수,건강진단대상근로자수
            fn_Ltd_Inwon_Read();

            strFrDate = dtpFrDate.Value.ToString("yyyy-MM-dd");
            strToDate = dtpToDate.Value.ToString("yyyy-MM-dd");
            nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
            if (rdoJob0.Checked == true)
            {
                strJob = "0";
            }
            else if (rdoJob1.Checked == true)
            {
                strJob = "1";
            }
            else if (rdoJob2.Checked == true)
            {
                strJob = "2";
            }
            else if (rdoJob3.Checked == true)
            {
                strJob = "3";
            }

            Cursor.Current = Cursors.WaitCursor;

            //strFrDate = "2019-04-11";
            //strToDate = "2020-03-13";
            //nLtdCode = 483;
            //strJob = "1";
            //FstrGjBangi = "";
            //FstrGjYear = "2020";
            List<HIC_JEPSU_RES_SPECIAL_LTD> list = hicJepsuResSpecialLtdService.GetItembyJepDateGjBangiLtdCode(strFrDate, strToDate, nLtdCode, strJob, FstrGjBangi, FstrGjYear);

            nRead = list.Count;
            if (nRead != 0)
            {
                progressBar1.Maximum = nRead;
            }

            for (int i = 0; i < nRead; i++)
            {
                FnWRTNO = list[i].WRTNO;
                strWRTNO += FnWRTNO + ",";
                FnPano = list[i].PANO;
                FstrSex = list[i].SEX;
                FstrUCodes = list[i].UCODES;
                FstrGjJong = list[i].GJJONG;
                FstrGjBangi = list[i].GJBANGI;

                //1차 검진기간
                FstrJepDate = list[i].JEPDATE;
                if (FstrFDate1.IsNullOrEmpty()) FstrFDate1 = FstrJepDate;
                if (string.Compare(FstrFDate1, FstrJepDate) > 0) FstrFDate1 = FstrJepDate;
                if (FstrTDate1.IsNullOrEmpty()) FstrTDate1 = FstrJepDate;
                if (string.Compare(FstrTDate1, FstrJepDate) < 0) FstrTDate1 = FstrJepDate;

                //2차의 접수번호를 찾음
                HIC_JEPSU list2 = hicJepsuService.GetWrtNoJepDatebyPanoJepDate(FnPano, FstrJepDate, FstrGjYear, strJob);

                FnWrtno2 = 0;
                if (!list2.IsNullOrEmpty())
                {
                    FnWrtno2 = list2.WRTNO;
                    strJepDate2 = list2.JEPDATE;
                }

                //2차 검진기간
                if (FnWrtno2 > 0)
                {
                    if (FstrFDate2.IsNullOrEmpty()) FstrFDate2 = strJepDate2;
                    if (string.Compare(FstrFDate2, strJepDate2) > 0) FstrFDate2 = strJepDate2;
                    if (FstrTDate2.IsNullOrEmpty()) FstrTDate2 = strJepDate2;
                    if (string.Compare(FstrTDate2, strJepDate2) < 0) FstrTDate2 = strJepDate2;
                }

                //2차 미실시 인원
                if (list[i].SECOND_FLAG == "Y" && FnWrtno2 == 0)
                {
                    FnNotSecond += 1;
                }

                hcb.READ_HIC_RES_BOHUM1(FnWRTNO);
                //일반2차 검진의 판정결과를 읽음
                //cHT.B2.ROWID = "";
                if (FnWrtno2 > 0)
                {
                    hcb.READ_HIC_RES_BOHUM2(FnWrtno2);
                }

                //유해인자별 수진근로자수 누적
                fn_ADD_Sujin_Inwon();
                //질병유소견자 누적
                fn_ADD_JilYusogen_Inwon();
                //질병코드별 누적
                fn_ADD_YuCode_Inwon();
                //조치현황
                fn_ADD_Jochi_Inwon();

                progressBar1.Value = i + 1;
            }

            //----------( 검진기간을 표시함 )-----------------
            SS1.ActiveSheet.Cells[11, 11].Text = " " + FstrFDate1 + "~" + VB.Right(FstrTDate1, 5);
            if (!FstrFDate2.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[12, 11].Text = " " + FstrFDate2 + "~" + VB.Right(FstrTDate2, 5);
            }
            else
            {
                SS1.ActiveSheet.Cells[12, 11].Text = "";
            }

            //--------------( 대상근로자수는 수진근로자수로 대치함 )--------
            for (int i = 0; i <= 17; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    FnJinCnt[i, j] = FnJinCnt[i, j + 3];
                }
            }

            //대상근로자수
            SS1.ActiveSheet.Cells[10, 2].Text = string.Format("{0:######}", FnJinCnt[1, 0]);
            SS1.ActiveSheet.Cells[11, 2].Text = string.Format("{0:######}", FnJinCnt[1, 1]);
            SS1.ActiveSheet.Cells[12, 2].Text = string.Format("{0:######}", FnJinCnt[1, 2]);

            //--------------( 대상근로자수를 Sheet에 Display )-------------
            for (int i = 0; i <= 17; i++)
            {
                nRow = i + 19;
                for (int j = 0; j <= 17; j++)
                {
                    nCol = j + 6;
                    if(FnJinCnt[i, j] == 1)
                    {
                        string ss = "소음";
                        if(nRow == 26 && nCol==19)
                        {
                            string sxs = "소음";
                            SS1.ActiveSheet.Cells[nRow, nCol].Text = string.Format("{0:######}", FnJinCnt[i, j]) ;
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow, nCol].Text = string.Format("{0:######}", FnJinCnt[i, j]);
                        }
                       
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow, nCol].Text = string.Format("{0:######}", FnJinCnt[i, j]) ;
                    }
                }
            }

            //-------------( 질병 유소견자 현황 )-------------------
            for (int i = 0; i <= 7; i++)
            {
                switch (i)
                {
                    case 0:
                        nRow = 38;  nCol = 2;
                        break;
                    case 1:
                        nRow = 39; nCol = 2;
                        break;
                    case 2:
                        nRow = 38; nCol = 7;
                        break;
                    case 3:
                        nRow = 39; nCol = 7;
                        break;
                    case 4:
                        nRow = 38; nCol = 12;
                        break;
                    case 5:
                        nRow = 39; nCol = 12;
                        break;
                    case 6:
                        nRow = 38; nCol = 17;
                        break;
                    case 7:
                        nRow = 39; nCol = 17;
                        break;
                    default:
                        break;
                }

                if (!FstrYuCode[i].IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[nRow, nCol].Text = "  " + FstrYuCode[i].Trim();
                    SS1.ActiveSheet.Cells[nRow, nCol + 2].Text = string.Format("{0:#####}", FnYuCNT[i, 0]);
                    SS1.ActiveSheet.Cells[nRow, nCol + 3].Text = string.Format("{0:#####}", FnYuCNT[i, 1]);
                    SS1.ActiveSheet.Cells[nRow, nCol + 4].Text = string.Format("{0:#####}", FnYuCNT[i, 2]);
                }
            }

            //-------------( 조치현황 Display )---------------------\
            for (int i = 0; i <= 17; i++)
            {
                nRow = i + 42;
                for (int j = 0; j <= 7; j++)
                {
                    switch (j)
                    {
                        case 0:
                            SS1.ActiveSheet.Cells[nRow, 5].Text = string.Format("{0:#####}", FnJoCNT[i, j]);
                            break;
                        case 1:
                            SS1.ActiveSheet.Cells[nRow, 8].Text = string.Format("{0:#####}", FnJoCNT[i, j]);
                            break;
                        default:
                            SS1.ActiveSheet.Cells[nRow, j + 7].Text = string.Format("{0:#####}", FnJoCNT[i, j]);
                            break;
                    }
                }
                Console.WriteLine("nRow == " + nRow);
            }

            btnPrint.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        void fn_ADD_Sujin_Inwon()
        {
            string ArgCode = "";
            List<string> ArgSQL = new List<string>();
            int No = 0;

            ArgCode = FstrUCodes.Trim();
            if (ArgCode.IsNullOrEmpty()) return;

            ArgSQL.Clear();
            for (int i = 0; i < VB.L(ArgCode, ","); i++)
            {
                if (!VB.Pstr(ArgCode, ",", i).Trim().IsNullOrEmpty())
                {
                    ArgSQL.Add(VB.Pstr(ArgCode, ",", i));
                }
            }

            //물질 통계분류별 건수를 읽음
            if (ArgSQL != null)
            {
                List<HIC_MCODE> list = hicMcodeService.GetTongBunCountbyCode(ArgSQL);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].TONGBUN == 0)
                    {
                        MessageBox.Show(ArgSQL + " 물질에 구분이 설정안된것이 있습니다..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (list[i].TONGBUN == 16)  //야간작업
                    {
                        No = 2;
                    }
                    else
                    {
                        No = (int)list[i].TONGBUN + 2;
                    }
                    if(No == 3)
                    {
                        int row = 3;
                    }
                    if (FstrSex == "M")
                    {
                        FnJinCnt[0, 3] += 1; 
                        FnJinCnt[0, 4] += 1;
                        if (No > 1)
                        {
                            FnJinCnt[No, 3] += 1; //소음
                            FnJinCnt[No, 4] += 1;
                        }
                    }
                    else
                    {
                        FnJinCnt[0, 3] += 1;
                        FnJinCnt[0, 5] += 1;
                        if (No > 1)
                        {
                            FnJinCnt[No, 3] += 1;
                            FnJinCnt[No, 5] += 1;
                        }
                    }
                }
            }
            
            //수진근로자 실인원
            if (FstrSex == "M")
            {
                FnJinCnt[1, 3] += 1;
                FnJinCnt[1, 4] += 1;
                Application.DoEvents();
            }
            else
            {
                FnJinCnt[1, 3] += 1;
                FnJinCnt[1, 5] += 1;
            }
        }

        void fn_ADD_JilYusogen_Inwon()
        {
            int j = 0;
            int nRead = 0;
            int[,] nCnt = new int[18, 4];
            //int nRCNT = 0;
            string strPanjeng = "";
            long nPanjengDrno = 0;
            string strOk = "";
            string strFrDate = "";
            string strToDate = "";

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;

            //배열을 Clear
            for (int i = 0; i <= 17; i++)
            {
                for (int k = 0; k <= 3; k++)
                {
                    nCnt[i, k] = 0;
                }
            }
            //nRCNT = 0;       //미판정건수
            FnUCodeBun = 0; //취급물질 1번의 분류코드

            //물질분류가 없는것은 제일 처음의 물질로 함
            FnUCodeBun = hicMcodeService.GetTongBunbyCode(VB.Pstr(FstrUCodes, ",", 1));

            //특수1차 판정을 ADD
            List<HIC_SPC_PANJENG_MCODE> list = hicSpcPanjengMcodeService.GetItembyWrtNoJepDate(FnWRTNO, strFrDate, strToDate);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strPanjeng = list[i].PANJENG;   
                if (list[i].TONGBUN == 16)    //야간작업
                {
                    j = 1;
                }
                else
                {
                    j = (int)list[i].TONGBUN + 1;
                }

                if (j == 0) j = FnUCodeBun;

                if (j > 0)
                {
                    if (j == 1) //야간작업
                    {
                        switch (strPanjeng)
                        {
                            case "A":
                                nCnt[j - 1 + 2, 1] = 1; //야간작업(DN)
                                break;
                            case "9":
                                nCnt[j - 1 + 2, 3] = 1; //야간작업(CN)
                                break;
                            case "5":
                            case "6":
                            case "7":
                                break;
                            case "":
                                FnMiPanjeng += 1; //미판정
                                break;
                            default:
                                break;
                        }
                    }
                    else  //특수
                    {
                        switch (strPanjeng)
                        {
                            case "5":
                                nCnt[j - 1 + 2, 0] = 1;   //직업병(유질환자) d1
                                break;
                            case "6":
                                nCnt[j - 1 + 2, 2] = 1;   //일반질병(유질환자) d2   j=6 그밖의 분진
                                break;
                            case "3":
                                nCnt[j - 1 + 2, 3] = 1;   //직업성요관찰자 c
                                break;
                            case "7":
                            case "9":
                            case "A":
                                break;
                            case "":
                                FnMiPanjeng += 1; //미판정
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //특수2차 판정을 ADD
            if (FnWrtno2 > 0)
            {
                List<HIC_SPC_PANJENG_MCODE> list2 = hicSpcPanjengMcodeService.GetItembyWrtNoJepDate(FnWrtno2, strFrDate, strToDate);

                nRead = list2.Count;
                for (int i = 0; i < nRead; i++)
                {
                    strPanjeng = list2[i].PANJENG;
                    if (list[i].TONGBUN == 16)    //야간작업
                    {
                        j = 1;
                    }
                    else
                    {
                        j = (int)list[i].TONGBUN + 1;
                    }

                    if (j == 0) j = FnUCodeBun;
                    if (j > 0)
                    {
                        if (j == 1) //야간작업
                        {
                            switch (strPanjeng)
                            {
                                case "A":
                                    nCnt[j - 1 + 2, 1] = 1; //야간작업(DN)
                                    break;
                                case "9":
                                    nCnt[j - 1 + 2, 3] = 1; //야간작업(CN)
                                    break;
                                case "5":
                                case "6":
                                case "7":
                                    break;
                                case "":
                                    FnMiPanjeng += 1; //미판정
                                    break;
                                default:
                                    break;
                            }
                        }
                        else  //특수
                        {
                            switch (strPanjeng)
                            {
                                case "5":
                                    nCnt[j - 1 + 2, 0] = 1;   //직업병(유질환자) d1
                                    break;
                                case "6":
                                    nCnt[j - 1 + 2, 2] = 1;   //일반질병(유질환자) d2
                                    break;
                                case "3":
                                    nCnt[j - 1 + 2, 3] = 1;   //직업성요관찰자 c
                                    break;
                                case "7":
                                case "9":
                                case "A":
                                    break;
                                case "":
                                    FnMiPanjeng += 1; //미판정
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            //건수를 계산
            for (int i = 2; i <= 17; i++)
            {
                nCnt[0, 0] += nCnt[i, 0];
                nCnt[0, 1] += nCnt[i, 1];
                nCnt[0, 2] += nCnt[i, 2];
                nCnt[0, 3] += nCnt[i, 3];
            }

            if (nCnt[0, 0] > 0) nCnt[1, 0] = 1;
            if (nCnt[0, 1] > 0) nCnt[1, 1] = 1;
            if (nCnt[0, 2] > 0) nCnt[1, 2] = 1;
            if (nCnt[0, 3] > 0) nCnt[1, 3] = 1;
            //실인원 계산용 변수 Clear
            if (nCnt[1, 0] == 1) nCnt[1, 1] = 0;

            //건수를 회사합계에 누적함
            for (int i = 0; i <= 17; i++)
            {
                if (FstrSex == "M")
                {
                    FnJinCnt[i, 7] += nCnt[i, 0] + nCnt[i, 1] + nCnt[i, 2];
                    FnJinCnt[i, 9] += nCnt[i, 0];
                    FnJinCnt[i, 11] += nCnt[i, 1];
                    FnJinCnt[i, 13] += nCnt[i, 2];
                    FnJinCnt[i, 15] += nCnt[i, 3];
                    FnJinCnt[i, 16] += nCnt[i, 3];
                }
                else
                {
                    FnJinCnt[i, 8] += nCnt[i, 0] + nCnt[i, 1] + nCnt[i, 2];
                    FnJinCnt[i, 10] += nCnt[i, 0];
                    FnJinCnt[i, 12] += nCnt[i, 1];
                    FnJinCnt[i, 14] += nCnt[i, 2];
                    FnJinCnt[i, 15] += nCnt[i, 3];
                    FnJinCnt[i, 17] += nCnt[i, 3];
                }
                FnJinCnt[i, 6] = FnJinCnt[i, 7] + FnJinCnt[i, 8];   //계
            }

            //-----( 특수판정 판정의사를 읽음(순서: 2차,1차) ------------
            nPanjengDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo2(FnWrtno2);

            if (nPanjengDrno == 0)
            {
                nPanjengDrno = hicSpcPanjengService.GetPanjengDrNobyWrtNo2(FnWRTNO);
            }

            //판정의사 명단 등록
            if (nPanjengDrno > 0)
            {
                if (FnDoctCnt == 0)
                {
                    FnDoctCnt = 1;
                    FnPanDrNo[FnDoctCnt] = nPanjengDrno;
                }
                else
                {
                    strOk = "";
                    for (int i = 0; i < FnDoctCnt; i++)
                    {
                        if (FnPanDrNo[i] == nPanjengDrno)
                        {
                            strOk = "OK";
                            break;
                        }
                    }
                    if (strOk != "OK")
                    {
                        FnDoctCnt += 1;
                        FnPanDrNo[FnDoctCnt] = nPanjengDrno;
                    }
                }
            }
        }

        void fn_ADD_YuCode_Inwon()
        {
            int nRead = 0;
            int nNo = 0;
            string strDBun = "";

            //특수1차 판정을 ADD
            List<HIC_SPC_PANJENG_SCODE> list = hicSpcPanjengScodeService.GetItembyWrtNo(FnWRTNO);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strDBun = list[i].DBUN;
                if (!strDBun.IsNullOrEmpty())
                {
                    nNo = 7;
                    //해당 질병코드의 위치를 찾음
                    for (int j = 0; j <= 7; j++)
                    {
                        if (FstrYuCode[j].IsNullOrEmpty())
                        {
                            nNo = j;
                            break;
                        }
                        if (strDBun == FstrYuCode[j])
                        {
                            nNo = j;
                            break;
                        }
                    }
                    FstrYuCode[nNo] = strDBun;
                    if (FstrSex == "M")
                    {
                        FnYuCNT[nNo, 0] += 1;
                        FnYuCNT[nNo, 1] += 1;
                    }
                    else
                    {
                        FnYuCNT[nNo, 0] += 1;
                        FnYuCNT[nNo, 2] += 1;
                    }
                }
            }

            //특수2차 판정을 ADD
            if (FnWrtno2 > 0)
            {
                List<HIC_SPC_PANJENG_SCODE> list2 = hicSpcPanjengScodeService.GetItembyWrtNo(FnWrtno2);

                nRead = list2.Count;
                for (int i = 0; i < nRead; i++)
                {
                    strDBun = list2[i].DBUN;
                    if (!strDBun.IsNullOrEmpty())
                    {
                        nNo = 7;
                        //해당 질병코드의 위치를 찾음
                        for (int j = 0; j <= 7; j++)
                        {
                            if (FstrYuCode[j].Trim().IsNullOrEmpty())
                            {
                                nNo = j;
                                break;
                            }
                            if (strDBun == FstrYuCode[j])
                            {
                                nNo = j;
                                break;
                            }
                        }
                        FstrYuCode[nNo] = strDBun;
                        if (FstrSex == "M")
                        {
                            FnYuCNT[nNo, 0] += 1;
                            FnYuCNT[nNo, 1] += 1;
                        }
                        else
                        {
                            FnYuCNT[nNo, 0] += 1;
                            FnYuCNT[nNo, 2] += 1;
                        }
                    }
                }
            }
        }

        void fn_ADD_Jochi_Inwon()
        {
            int jj = 0;
            int k = 0;
            int nRead = 0;
            int nCount = 0;
            int[,] nCnt = new int[6, 8];
            string strPanjeng = "";
            string strFlagC = "";

            //배열을 Clear
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    nCnt[i, j] = 0;
                }
            }

            //특수1차 판정을 ADD
            List<HIC_SPC_PANJENG> list = hicSpcPanjengService.GetPanjengSahuCodebyWrtNo(FnWRTNO);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strPanjeng = list[i].PANJENG;
                switch (strPanjeng)
                {
                    case "5":
                        nCnt[0, 0] = 1;
                        jj = 1;  //직업병(유질환자)
                        break;
                    case "A":
                        nCnt[1, 0] = 1;
                        jj = 2;  //야간작업(유질환자)
                        break;
                    case "6":
                        nCnt[2, 0] = 1;
                        jj = 3;  //일반질병(유질환자)
                        break;

                    case "3":
                        nCnt[3, 0] = 1;
                        jj = 4;  //직업성 요관찰자
                        break;
                    case "9":
                        nCnt[4, 0] = 1;
                        jj = 5;  //야간작업 요관찰자
                        break;
                    case "4":
                        nCnt[5, 0] = 1;
                        jj = 6;  //일반 요관찰자
                        break;
                    default:
                        jj = 0;
                        break;
                }
                if (jj > 0)
                {
                    nCount = 9;
                    for (int y = 0; y <= 8; y++)
                    {
                        if (VB.L(list[i].SAHUCODE, nCount.ToString()) > 1)
                        {
                            switch (nCount)
                            {
                                case 7:
                                    nCnt[jj - 1, 1] = 1; //근로금지및제한
                                    break;
                                case 6:
                                    nCnt[jj - 1, 2] = 1; //작업전환
                                    break;
                                case 5:
                                    nCnt[jj - 1, 3] = 1; //근로시간단축
                                    break;
                                case 4:
                                    nCnt[jj - 1, 4] = 1; //근무중치료
                                    break;
                                case 3:
                                    nCnt[jj - 1, 5] = 1; //추적검사
                                    break;
                                case 2:
                                    nCnt[jj - 1, 6] = 1; //보호구착용
                                    break;
                                default:
                                    nCnt[jj - 1, 7] = 1; //기타
                                    break;
                            }
                            break;
                        }
                        nCount -= 1;
                    }
                }
            }
           

            //특수2차 판정을 ADD
            if (FnWrtno2 > 0)
            {
                List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GetPanjengSahuCodebyWrtNo(FnWrtno2);

                nRead = list2.Count;
                for (int i = 0; i < nRead; i++)
                {
                    strPanjeng = list2[i].PANJENG;
                    switch (strPanjeng)
                    {
                        case "5":
                            nCnt[0, 0] = 1;
                            jj = 1;  //직업병(유질환자)
                            break;
                        case "A":
                            nCnt[1, 0] = 1;
                            jj = 2;  //야간작업(유질환자)
                            break;
                        case "6":
                            nCnt[2, 0] = 1;
                            jj = 3;  //일반질병(유질환자)
                            break;

                        case "3":
                            nCnt[3, 0] = 1;
                            jj = 4;  //직업성 요관찰자
                            break;
                        case "9":
                            nCnt[4, 0] = 1;
                            jj = 5;  //야간작업 요관찰자
                            break;
                        case "4":
                            nCnt[5, 0] = 1;
                            jj = 6;  //일반 요관찰자
                            break;
                        default:
                            jj = 0;
                            break;
                    }
                    if (jj > 0)
                    {
                        nCount = 9;
                        for (int y = 0; y <= 8; y++)
                        {
                            if (VB.L(list[i].SAHUCODE, nCount.ToString()) > 1)
                            {
                                switch (nCount)
                                {
                                    case 7:
                                        nCnt[jj - 1, 1] = 1; //근로금지및제한
                                        break;
                                    case 6:
                                        nCnt[jj - 1, 2] = 1; //작업전환
                                        break;
                                    case 5:
                                        nCnt[jj - 1, 3] = 1; //근로시간단축
                                        break;
                                    case 4:
                                        nCnt[jj - 1, 4] = 1; //근무중치료
                                        break;
                                    case 3:
                                        nCnt[jj - 1, 5] = 1; //추적검사
                                        break;
                                    case 2:
                                        nCnt[jj - 1, 6] = 1; //보호구착용
                                        break;
                                    default:
                                        nCnt[jj - 1, 7] = 1; //기타
                                        break;
                                }
                                break;
                            }
                            nCount -= 1;
                        }
                    }
                }
            }

            //조치별 1개만 남기고 Clear(우선순위: 근로금지및제환 -> 기타)
            for (int i = 0; i <= 5; i++)
            {
                k = 0;
                for (int j = 1; j <= 7; j++)
                {
                    if (nCnt[i, j] > 0)
                    {
                        if (k == 0)
                        {
                            k = 1;
                        }
                        else
                        {
                            nCnt[i, j] = 0;
                        }
                    }
                }
                //조치사항이 없으면 기타로 처리
                if (k == 0 && nCnt[i, 0] > 0)
                {
                    nCnt[i, 7] = 1;
                }
            }

            //회사별 누계에 ADD
            for (int i = 0; i <= 7; i++)
            {
                if (FstrSex == "M")
                {
                    //직업병
                    FnJoCNT[0, i] += nCnt[0, i];
                    FnJoCNT[1, i] += nCnt[0, i];
                    FnJoCNT[3, i] += nCnt[0, i];
                    //야간작업               
                    FnJoCNT[0, i] += nCnt[1, i];
                    FnJoCNT[1, i] += nCnt[1, i];
                    FnJoCNT[5, i] += nCnt[1, i];
                    //일반질병           
                    FnJoCNT[0, i] += nCnt[2, i];
                    FnJoCNT[1, i] += nCnt[2, i];
                    FnJoCNT[7, i] += nCnt[2, i];

                    //직업병(요관찰)
                    FnJoCNT[9, i] += nCnt[3, i];
                    FnJoCNT[10, i] += nCnt[3, i];
                    FnJoCNT[12, i] += nCnt[3, i];
                    //야간작업(요관찰)
                    FnJoCNT[9, i] += nCnt[4, i];
                    FnJoCNT[10, i] += nCnt[4, i];
                    FnJoCNT[14, i] += nCnt[4, i];
                    //야간작업(일반질병)
                    FnJoCNT[9, i] += nCnt[5, i];
                    FnJoCNT[10, i] += nCnt[5, i];
                    FnJoCNT[16, i] += nCnt[5, i];
                }
                else
                {
                    //직업병
                    FnJoCNT[0, i] += nCnt[0, i];
                    FnJoCNT[2, i] += nCnt[0, i];
                    FnJoCNT[4, i] += nCnt[0, i];
                    //야간작업
                    FnJoCNT[0, i] += nCnt[1, i];
                    FnJoCNT[2, i] += nCnt[1, i];
                    FnJoCNT[6, i] += nCnt[1, i];
                    //일반질병
                    FnJoCNT[0, i] += nCnt[2, i];
                    FnJoCNT[2, i] += nCnt[2, i];
                    FnJoCNT[8, i] += nCnt[2, i];

                    //직업병(요관찰)
                    FnJoCNT[9, i] += nCnt[3, i];
                    FnJoCNT[11, i] += nCnt[3, i];
                    FnJoCNT[13, i] += nCnt[3, i];
                    //야간작업(요관찰)
                    FnJoCNT[9, i] += nCnt[4, i];
                    FnJoCNT[11, i] += nCnt[4, i];
                    FnJoCNT[15, i] += nCnt[4, i];
                    //야간작업(일반질병)
                    FnJoCNT[9, i] += nCnt[5, i];
                    FnJoCNT[11, i] += nCnt[5, i];
                    FnJoCNT[17, i] += nCnt[5, i];
                }
            }
        }

        void fn_Screen_Clear()
        {
            lblLtdName2.Text = "";
            lblGigan.Text = "";
            btnPrint.Enabled = false;

            //건강진단현황 Clear
            for (int i = 0; i <= 17; i++)
            {
                for (int j = 0; j <= 17 ; j++)
                {
                    FnJinCnt[i, j] = 0;
                }
            }

            //질병유소견자 현황 Clear
            for (int i = 0; i <= 7; i++)
            {
                FstrYuCode[i] = "";
                for (int j = 0; j <= 2; j++)
                {
                    FnYuCNT[i, j] = 0;
                }
            }
            //조치현황 Clear
            for (int i = 0; i <= 17; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    FnJoCNT[i, j] = 0;
                }
            }

            SS1.ActiveSheet.Cells[19, 6, 36, 23].Text = "";
            SS1.ActiveSheet.Cells[38, 1, 39, 23].Text = "";
            SS1.ActiveSheet.Cells[42, 5, 59, 14].Text = "";
            SS1.ActiveSheet.Cells[40, 15, 59, 23].Text = "";

            for (int i = 41; i <= 59; i++)
            {
                for (int j = 15; j <= 21; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }
        }

        /// <summary>
        /// 총근로자수,대상자수,건강진단대상근로자수
        /// </summary>
        void fn_Ltd_Inwon_Read()
        {
            int nInwon1 = 0;
            int nInwon2 = 0;
            int nInwon3 = 0;
            long nPanjengDrno;
            string strOk = "";
            string strGwanse = "";

            //회사별 연도/반기별 검진대상인원을 읽음
            COMHPC list = comHpcLibBService.GetItembyYearLtdCodeBangi(FstrGjYear, FstrLtdCode, FstrGjBangi);

            if (!list.IsNullOrEmpty())
            {
                //총근로자수
                nInwon1 = list.INWON011; //총근로자(남)
                nInwon2 = list.INWON012; //총근로자(여)
                nInwon3 = nInwon1 + nInwon2;
            }

            //총근로자수
            if (nInwon3 == 0)
            {
                SS1.ActiveSheet.Cells[10, 2].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[10, 2].Text = nInwon3.ToString();
            }
            if (nInwon1 == 0)
            {
                SS1.ActiveSheet.Cells[10, 2].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[11, 2].Text = nInwon1.ToString();
            }
            if (nInwon2 == 0)
            {
                SS1.ActiveSheet.Cells[12, 2].Text = "";
            }
            else
            {
                SS1.ActiveSheet.Cells[12, 2].Text = nInwon2.ToString();
            }

            //-----------( 회사 주소 및 업종등 )------------------

            //회사 정보를 읽음
            HIC_LTD list2 = hicLtdService.GetIetmbyCode(FstrLtdCode.To<long>());

            if (!list2.IsNullOrEmpty())
            {
                //사업장등록번호
                SS1.ActiveSheet.Cells[11, 19].Text = list2.SAUPNO;
                SS1.ActiveSheet.Cells[12, 19].Text = list2.UPJONG;
                SS1.ActiveSheet.Cells[14, 0].Text = VB.Space(1) + "사업장명 : " + list2.SANGHO;
                SS1.ActiveSheet.Cells[10, 19].Text = list2.HEMSNO;  //사업장관리번호
                SS1.ActiveSheet.Cells[15, 0].Text = VB.Space(1) + "소 재 지 : " + list2.JUSO + " " + list2.JUSODETAIL;
                SS1.ActiveSheet.Cells[15, 10].Text = "(전화번호 : " + list2.TEL + ")";
                SS1.ActiveSheet.Cells[14, 16].Text = "주요생산품 : " + list2.JEPUMLIST;
                if (txtPrtDate.Text.Trim().IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[42, 15].Text = " 작성일자 : " + VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일";
                }
                else
                {
                    SS1.ActiveSheet.Cells[42, 15].Text = " 작성일자 : " + VB.Left(txtPrtDate.Text, 4) + "년 " + VB.Mid(txtPrtDate.Text, 6, 2) + "월 " + VB.Right(txtPrtDate.Text, 2) + "일";
                }
                SS1.ActiveSheet.Cells[44, 15].Text = VB.Space(1) + "건진기관명 : 대한보건환경연구소";
                SS1.ActiveSheet.Cells[49, 15].Text = VB.Space(1) + "사 업 주 : " + VB.Left(list2.DAEPYO + VB.Space(10), 15).Trim() + "   (서명 또는 인)";
                SS1.ActiveSheet.Cells[51, 15].Text = VB.Space(1) + "고용노동부 : " + hb.READ_HIC_CODE("02", list2.GWANSE);
                SS1.ActiveSheet.Cells[53, 15].Text = VB.Space(1) + "지방고용노동청(지청)장   귀하";
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
