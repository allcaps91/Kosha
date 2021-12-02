using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcAllGaJepsu.cs
/// Description     : 일괄가접수 작업화면
/// Author          : 이상훈
/// Create Date     : 2020-07-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm일괄가접수.frm(Frm일괄가접수)" />

namespace HC_Main
{
    public partial class frmHcAllGaJepsu : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicExjongService hicExjongService = null;
        HicPatientService hicPatientService = null;
        BasPatientService basPatientService = null;
        WorkNhicService workNhicService = null;
        HicCodeService hicCodeService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HicGroupcodeService hicGroupcodeService = null;
        HicGroupexamExcodeService hicGroupexamExcodeService = null;
        HicSunapdtlWorkService hicSunapdtlWorkService = null;
        HicSunapWorkService hicSunapWorkService = null;
        HicLtdService hicLtdService = null;
        BasMailnewService basMailnewService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicJepsuExjongPatientService hicJepsuExjongPatientService = null;
        HicJepsuWorkPatientHeaJepsuService hicJepsuWorkPatientHeaJepsuService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        frmHcCodeHelp FrmHcCodeHelp = null;
        HIC_CODE CodeHelpItem = null;

        frmHcHarmfulFactor FrmHcHarmfulFactor = null;

        frmHcSAmtSelect FrmHcSAmtSelect = null;

        frmHcNhicSub FrmHcNhicSub = null;

        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        clsSpread sp = new clsSpread();
        clsHcType ht = new clsHcType();

        string FstrJumin;
        string FstrBurate;
        string FstrSExams;
        string FstrUCodes;
        string FstrPANO;
        int FnRow;
        long FnTotalAmt;    //일반건진비
        long FnJohapAmt;    //조합부담
        long FnLtdAmt;      //회사부담
        long FnBoninAmt;    //본인부담
        long FnIpAmt;       //입금액
        long FnChaiAmt;     //차액
        long FnCardAmt;     //카드금액
        string FstrSelWRTNO;
        int FnCurrentRow;

        string strGkiho = "";
        string strJisa = "";
        string strFirst = "";

        string strLiver = "";
        string str생애구분 = "";
        string strNhicYN = "";

        string str수검여부 = "";

        string strNhicChk = "";

        string strJisaCode = "";

        string strUCodes = "";
        string strGjjong = "";

        string strGbSelf = "";
        string strBurate = "";
        string strCode = "";

        string FstrCode = "";
        string FstrName = "";

        List<long> Fstr2ChaWrtno = new List<long>();

        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

        public frmHcAllGaJepsu()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcAllGaJepsu(List<long> str2ChaWrtno)
        {
            InitializeComponent();
            Fstr2ChaWrtno = str2ChaWrtno;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicExjongService = new HicExjongService();
            hicPatientService = new HicPatientService();
            basPatientService = new BasPatientService();
            workNhicService = new WorkNhicService();
            hicCodeService = new HicCodeService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicGroupcodeService = new HicGroupcodeService();
            hicGroupexamExcodeService = new HicGroupexamExcodeService();
            hicSunapdtlWorkService = new HicSunapdtlWorkService();
            hicSunapWorkService = new HicSunapWorkService();
            hicLtdService = new HicLtdService();
            basMailnewService = new BasMailnewService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicJepsuExjongPatientService = new HicJepsuExjongPatientService();
            hicJepsuWorkPatientHeaJepsuService = new HicJepsuWorkPatientHeaJepsuService();


            this.Load += new EventHandler(eFormLoad);
            //혈액종양검사
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnInfo.Click += new EventHandler(eBtnClick);
            this.btnDir1.Click += new EventHandler(eBtnClick);
            this.btnImport.Click += new EventHandler(eBtnClick);
            this.btnSunapInfo.Click += new EventHandler(eBtnClick);
            this.btnAddNew.Click += new EventHandler(eBtnClick);
            this.btnNhic.Click += new EventHandler(eBtnClick);
            this.btnSet.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnRun.Click += new EventHandler(eBtnClick);
            this.btnAddHelp.Click += new EventHandler(eBtnClick);
            this.btnUCodeHelp.Click += new EventHandler(eBtnClick);
            this.btnAmt.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.chkExcel.Click += new EventHandler(eChkBoxClick);
            this.chkGaJep.Click += new EventHandler(eChkBoxClick);
            this.chkIEMunjin.Click += new EventHandler(eChkBoxClick);
            this.chkJep.Click += new EventHandler(eChkBoxClick);
            this.chkMkName.Click += new EventHandler(eChkBoxClick);
            this.chkSelect.Click += new EventHandler(eChkBoxClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.pnlSunap.Click += new EventHandler(ePnlClick);
            this.SS1.LeaveCell += new LeaveCellEventHandler(eSpdLeaveCell);
            this.SS2.LeaveCell += new LeaveCellEventHandler(eSpdLeaveCell);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);
            this.rdoCh0.Click += new EventHandler(eRdoClick);
            this.rdoCh1.Click += new EventHandler(eRdoClick);
        }

        void ePnlClick(object sender, EventArgs e)
        {
            pnlSunap.Visible = false;
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoCh0)
            {
                if (rdoCh0.Checked == true)
                {
                    for (int i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
                    {
                        SS1_Sheet1.Columns.Get(i).AllowAutoSort = false;
                    }

                    //SS1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
                }
            }
            else if (sender == rdoCh1)
            {
                if (rdoCh0.Checked == false)
                {
                    for (int i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
                    {
                        SS1_Sheet1.Columns.Get(i).AllowAutoSort = true;
                    }
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS2)
            {
                if (e.Column == 3)
                {
                    eBtnClick(btnAmt, new EventArgs()); //금액을 계산
                }
            }
        }

        void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (sender == SS1)
            {
                switch (e.NewColumn)
                {
                    case 2:
                        lblSts.Text = "검진종류는 수정 가능합니다. ( 예: 11, 12 , 13 두자리 숫자로 입력가능함 )";
                        break;
                    case 29:
                        lblSts.Text = "1.조합100%, 2.회사100%, 3.본인100%, 4.조합,본인50%, 5.조합,회사50%, 6.회사,본인50% ";
                        break;
                    case 30:
                        lblSts.Text = "1.생산직 2.특수직 3.사무직 ";
                        break;
                    default:
                        lblSts.Text = "검진년도,반기는 가접수 적용일자에 의해 자동 변경됩니다.(기존 가접수된 자료는 가접수한 자료가 불러옵니다)";
                        break;
                }
            }
            else if (sender == SS2)
            {
                switch (e.NewColumn)
                {
                    case 3:
                        lblSts.Text = "1.조합100% 2.회사100% 3.본인100% 4.조합,본인50% 5.조합,회사50% 6.회사,본인50%";
                        break;
                    default:
                        lblSts.Text = "검진년도 , 반기는 가접수 적용일자에 의해 자동 변경됩니다.";
                        break;
                }
            }
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            this.Location = new Point(10, 10);

            clsHcType.TEC.USE = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
            dtpGJepDate.Text = clsPublic.GstrSysDate;

            txtLtdName.Text = "";

            //검진종류 SET
            cboJong.Items.Clear();
            cboJong.Items.Add("");
            hb.ComboJong_AddItem(cboJong);

            SS1_Sheet1.Columns.Get(6).Visible = false;
            SS1_Sheet1.Columns.Get(16).Visible = false;

            for (int i = 23; i <= 25; i++)
            {
                SS1_Sheet1.Columns.Get(i).Visible = false;
            }
            SS1_Sheet1.Columns.Get(24).Visible = true;
            for (int i = 36; i <= 44; i++)
            {
                SS1_Sheet1.Columns.Get(i).Visible = false;
            }
            SS1_Sheet1.Columns.Get(46).Visible = false;
            SS1_Sheet1.Columns.Get(50).Visible = false;
            SS1_Sheet1.Columns.Get(52).Visible = false;

            SS2_Sheet1.Columns.Get(5).Visible = false;  //OldQty
            SS2_Sheet1.Columns.Get(6).Visible = false;  //Self
            SS2_Sheet1.Columns.Get(7).Visible = false;  //OldAmt
            SS2_Sheet1.Columns.Get(8).Visible = false;  //기본수가

            //미수계정
            cboMisu.Items.Clear();
            cboMisu.Items.Add("");
            cboMisu.Items.Add("01.개인미수");
            //할인계정
            cboHalin.Items.Clear();
            cboHalin.Items.Add("");
            cboHalin.Items.Add("01.금액할인");

            sp.Spread_All_Clear(SS1);
            fn_Screen_Clear();

            pnlSunap.Visible = false;

            //2차 가접수 접수번호 목록이 있으면
            if (!Fstr2ChaWrtno.IsNullOrEmpty() && Fstr2ChaWrtno.Count > 0)
            {
                chkJep.Checked = false;
                chkMkName.Checked = true;
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                string strLtdCode = "";

                if (txtLtdName.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdName.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdName.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdName.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdName.Text = "";
                }
            }
            else if (sender == btnInfo)
            {
                string strTemp = "";

                strTemp = "Excel 파일형식은 다음과 같습니다. (순번=열순서)" + "\r\n" + "\r\n";
                strTemp += "1.성명" + "\r\n";
                strTemp += "2.주민번호" + "\r\n";
                strTemp += "3.전화번호" + "\r\n";
                strTemp += "4.휴대폰번호" + "\r\n";
                strTemp += "5.학년" + "\r\n";
                strTemp += "6.반" + "\r\n";
                strTemp += "7.번호" + "\r\n";
                strTemp += "8.부서" + "\r\n";
                strTemp += "9.사번" + "\r\n";
                strTemp += "10.입사일자" + "\r\n";
                strTemp += "11.공정코드";

                MessageBox.Show(strTemp, "EXCEL 파일형식", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnDir1)
            {
                OpenFileDialog file = new OpenFileDialog();

                file.Title = "열기";
                file.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                file.FilterIndex = 1;
                txtFile1.Text = file.FileName;

                if (file.ShowDialog() == DialogResult.OK)
                {
                    txtFile1.Text = file.FileName;
                    if (txtFile1.Text.IsNullOrEmpty())
                    {
                        btnImport.Enabled = false;
                    }
                    else
                    {
                        btnImport.Enabled = true;
                    }
                }
            }
            else if (sender == btnImport)
            {
                bool Y = false;
                bool X = false;

                if (txtFile1.Text.IsNullOrEmpty()) return;

                SS5.Visible = true;
                X = SS5.IsExcelFile(txtFile1.Text);

                if (X == true)
                {
                    Y = SS5.OpenExcel(txtFile1.Text);
                    if (Y == true)
                    {
                        MessageBox.Show("엑셀파일 불러오기 작업이 완료되었습니다.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("엑셀파일 불러오기 작업에 오류가 있습니다.", "작업오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("엑셀파일이 아니거나 잠겨져 있습니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                fn_Impot_ExcelFile_Spread();
            }
            else if (sender == btnSunapInfo)
            {
                if (pnlSunap.Visible == true)
                {
                    pnlSunap.Visible = false;
                }
                else
                {
                    pnlSunap.Visible = true;
                }
            }
            else if (sender == btnAddNew)
            {
                int nCnt = 0;
                string strTemp = "";

                SS1.ActiveSheet.RowCount += 1;

                nCnt = SS1.ActiveSheet.RowCount;

                for (int i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
                {
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i].CellType = txt;
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount - 1, i].Locked = false;
                }

                if (SS1.ActiveSheet.RowCount > 1)
                {
                    for (int i = 1; i < SS1.ActiveSheet.ColumnCount; i++)
                    {
                        strTemp = SS1.ActiveSheet.Cells[nCnt - 2, i].Text;
                        SS1.ActiveSheet.Cells[nCnt - 1, i].Text = strTemp;
                    }
                }
            }
            else if (sender == btnNhic)
            {
                string strPtNo = "";
                string strSname = "";
                string strJumin = "";
                string strJong = "";
                string strYear = "";
                string strChasu = "";
                int nCnt = 0;
                string strEXAMA = "";
                string strEXAME = "";
                string strEXAMG = "";
                string strSExams = "";
                string strEXAMF = "";

                //2020-09-22 추가
                string strEXAMB = "";
                string strEXAMD = "";
                string strEXAMH = "";

                int result = 0;

                if (MessageBox.Show("가접수 인원이 많을수록 작업시간이 오래걸립니다. 그래도 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                //DATA_ERROR_CHECK =====================================================================================

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        if (SS1.ActiveSheet.Cells[i, 2].Text.IsNullOrEmpty())
                        {
                            MessageBox.Show(i + " 번째 줄 검진종류가 공란입니다.", "자격조회 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (SS1.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                        {
                            MessageBox.Show(i + " 번째 줄 수진자명이 공란입니다.", "자격조회 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (SS1.ActiveSheet.Cells[i, 9].Text.IsNullOrEmpty())
                        {
                            MessageBox.Show(i + " 번째 줄 주민번호가 공란입니다.", "자격조회 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (SS1.ActiveSheet.Cells[i, 15].Text.IsNullOrEmpty())
                        {
                            MessageBox.Show(i + " 번째 줄 병원번호가 공란입니다.", "자격조회 불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                //=====================================================================================

                nCnt = 0;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCnt += 1;
                    }
                }

                progressBar1.Maximum = nCnt;
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCnt += 1;

                        strYear = VB.Left(SS1.ActiveSheet.Cells[i, 1].Text, 4);
                        strJong = VB.Left(SS1.ActiveSheet.Cells[i, 2].Text, 2);
                        strSname = SS1.ActiveSheet.Cells[i, 4].Text;
                        strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                        strPtNo = SS1.ActiveSheet.Cells[i, 15].Text;


                        //루틴 클리어
                        strJisa = ""; strNhicYN = ""; strGkiho = ""; strNhicChk = "";  strLiver = ""; strJisaCode = ""; strChasu = ""; str수검여부 = ""; strFirst = "";
                        strEXAMA = ""; strEXAME = ""; strEXAMG = ""; strEXAMF = ""; strEXAMB = ""; strEXAMD = ""; strEXAMH = ""; strSExams = "";

                        strChasu = hicExjongService.GetChasubyCode(strJong);

                        str생애구분 = "N";
                        if (strJong == "35" || strJong == "41" || strJong == "42" || strJong == "43" || strJong == "44" || strJong == "45" || strJong == "46")
                        {
                            str생애구분 = "Y";
                        }

                        //항상 신규로 조회하게 함
                        if (SS1.ActiveSheet.Cells[i, 20].Text != "Y")
                        {
                            if (!strJumin.IsNullOrEmpty())
                            {
                                //clsDB.setBeginTran(clsDB.DbCon);

                                result = workNhicService.DeleteDataAllByJuminNo(clsAES.AES(strJumin));

                                if (result < 0)
                                {
                                    //clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("자격조회 History 삭제시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                //clsDB.setCommitTran(clsDB.DbCon);
                            }
                        }

                        fn_Check_Nhic_History(clsAES.AES(strJumin), strSname);

                        //자격읽기 성공한 자료가 없다면 ...(10.03.16) 자동자격조회함
                        if (strNhicChk == "N")
                        {
                            WORK_NHIC list3 = workNhicService.GetSNamebyJumin2Year(clsAES.AES(strJumin), strYear);

                            if (list3.IsNullOrEmpty())
                            {
                                //한번도 조회한적이 없고 실패가 없는 수검자

                                if (!FstrJumin.IsNullOrEmpty())
                                {
                                    WORK_NHIC list4 = workNhicService.GetSNamebyJumin2(clsAES.AES(FstrJumin), strYear);

                                    if (!list4.IsNullOrEmpty())
                                    {
                                        clsDB.setBeginTran(clsDB.DbCon);

                                        WORK_NHIC item = new WORK_NHIC();

                                        item.GUBUN = "H";
                                        item.SNAME = strSname;
                                        item.JUMIN = VB.Left(strJumin, 7) + "******";
                                        item.JUMIN2 = clsAES.AES(strJumin);
                                        item.PANO = strPtNo;
                                        item.GBSTS = "0";
                                        item.YEAR = strYear;
                                        result = workNhicService.InsertData(item);

                                        if (result < 0)
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                        }
                                        clsDB.setCommitTran(clsDB.DbCon);
                                    }
                                }
                                else
                                {
                                    clsDB.setBeginTran(clsDB.DbCon);

                                    WORK_NHIC item = new WORK_NHIC();

                                    item.GUBUN = "H";
                                    item.SNAME = strSname;
                                    item.JUMIN = VB.Left(strJumin, 7) + "******";
                                    item.JUMIN2 = clsAES.AES(strJumin);
                                    item.PANO = strPtNo;
                                    item.GBSTS = "0";
                                    item.YEAR = strYear;

                                    result = workNhicService.InsertData(item);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                    }
                                    clsDB.setCommitTran(clsDB.DbCon);
                                }
                            }
                        }

                        //자동자격조회
                        if (!strSname.IsNullOrEmpty())
                        {
                            FrmHcNhicSub = new frmHcNhicSub(strSname, strJumin, strYear, "H", strPtNo);
                            FrmHcNhicSub.ShowDialog();
                            FrmHcNhicSub = null;
                            //FrmHcNhicSub.Dispose();

                            //최근 검색한 자료가 있으면 다시 검색을 안함
                            WORK_NHIC list = workNhicService.GetItembyJumin2(clsAES.AES(strJumin));
                            
                            if (!list.IsNullOrEmpty())
                            {
                                strGkiho = list.GKIHO;   //증번호
                                strJisa = list.JISA;     //지사
                                strFirst = list.FIRST;   //1차검진
                                if (str생애구분 == "Y")
                                {
                                    strLiver = list.LIVER;
                                }
                                strEXAMA = list.EXAMA;
                                strEXAME = list.EXAME;
                                strEXAMG = list.EXAMG;
                                strEXAMF = list.EXAMF;
                                //2020-09-22
                                strEXAMB = list.LIVER;
                                strEXAMD = list.EXAMD;
                                strEXAMH = list.EXAMH;

                                strNhicYN = "Y";

                                if (!list.GBCHK01.IsNullOrEmpty())
                                {
                                    str수검여부 = "Y";
                                }
                                if (!list.GBCHK02.IsNullOrEmpty())
                                {
                                    str수검여부 = "Y";
                                }
                                //if (!list.GBCHK03.IsNullOrEmpty())
                                //{
                                //    str수검여부 = "Y";
                                //}
                                if (!list.GBCHK01_NAME.IsNullOrEmpty())
                                {
                                    str수검여부 = "Y";
                                }
                                if (!list.GBCHK02_NAME.IsNullOrEmpty())
                                {
                                    str수검여부 = "Y";
                                }
                                //if (!list.GBCHK03_NAME.IsNullOrEmpty())
                                //{
                                //    str수검여부 = "Y";
                                //}
                            }
                            else
                            {
                                strGkiho = "";
                                strNhicYN = "N";
                            }
                        }

                        //콜레스테롤 4종
                        if (strNhicYN == "Y")
                        {
                            if (strEXAMA == "대상")
                            {
                                strSExams = "1160";
                            }

                            //B형간염
                            if (strEXAMB == "대상")
                            {
                                if (strSExams.IsNullOrEmpty())
                                {
                                    strSExams = "1161";
                                }
                                else
                                {
                                    strSExams = strSExams + ",1161";
                                }
                            }

                            //고밀도
                            if (strEXAMD == "대상")
                            {
                                if (strSExams.IsNullOrEmpty())
                                {
                                    strSExams = "1162";
                                }
                                else
                                {
                                    strSExams = strSExams + ",1162";
                                }
                            }

                            //노인신체기능
                            if (strEXAMH == "대상")
                            {
                                if (strSExams.IsNullOrEmpty())
                                {
                                    strSExams = "1168";
                                }
                                else
                                {
                                    strSExams = strSExams + ",1168";
                                }
                            }

                            if (strEXAME == "대상")
                            {
                                if (strSExams.IsNullOrEmpty())
                                {
                                    strSExams = "1163";
                                }
                                else
                                {
                                    strSExams = strSExams + ",1163";
                                }
                            }

                            if (strEXAMG == "대상")
                            {
                                if (strSExams.IsNullOrEmpty())
                                {
                                    strSExams = "1164,1165,1166";
                                }
                                else
                                {
                                    strSExams = strSExams + ",1164,1165,1166";
                                }
                            }

                            if (strEXAMF == "대상")
                            {
                                if (strSExams.IsNullOrEmpty())
                                {
                                    strSExams = "1167";
                                }
                                else
                                {
                                    strSExams = strSExams + ",1167";
                                }
                            }

                            if (SS1.ActiveSheet.Cells[i, 21].Text.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[i, 21].Text = strSExams;
                            }
                            SS1.ActiveSheet.Cells[i, 55].Text = strSExams;
                        }

                        SS1.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));

                        if (str수검여부 == "Y")
                        {
                            SS1.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H8080FF"));
                        }
                        strJisaCode = fn_Read_Jisa_Code(strJisa);
                        SS1.ActiveSheet.Cells[i, 14].Text = strJisaCode;
                        SS1.ActiveSheet.Cells[i, 19].Text = strGkiho;
                        SS1.ActiveSheet.Cells[i, 20].Text = VB.L(strFirst, "비대상") > 1 ? "N" : "Y";
                        if (strFirst.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 20].Text = "N";
                        }
                        if (strLiver.IsNullOrEmpty() || strLiver == "비대상")
                        {
                            SS1.ActiveSheet.Cells[i, 35].Text = "N";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 35].Text = "Y";
                        }
                        SS1.ActiveSheet.Cells[i, 45].Text = VB.L(strFirst, "비대상") > 1 ? "N" : "Y";
                        if (SS1.ActiveSheet.Cells[i, 45].Text == "N")
                        {
                            SS1.ActiveSheet.Cells[i, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                        }
                    }
                    //progressBar1.Value = i + 1;
                }
            }
            else if (sender == btnSet)
            {

                long nPano = 0;
                string strSExam = "";
                string strUCodes = "";
                string strBurate = "";
                string strRate1 = "";
                string strRate2 = "";
                string strGbChul = "";
                string strLtdCode = "";
                string strInwon = "";
                string strSel = "";
                string strPANO = "";
                string strPtNo = "";
                string strJumin = "";
                string strName = "";
                string strMailCode = "";
                string strJuso1 = "";
                string strJuso2 = "";
                string strChasu = "";
                string strSex = "";
                string strTel = "";
                string strGong = "";
                string strSExams = "";
                string strSname = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strName = SS1.ActiveSheet.Cells[i, 4].Text;
                    strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                    if (!strJumin.IsNullOrEmpty() && strName.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + "번줄 성명이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (chkIEMunjin.Checked == true || chkExcel.Checked == true)
                {
                    if (MessageBox.Show("인터넷문진이나 엑셀파일 접수일 경우 신환번호를 자동으로 생성합니다." + "\r\n" + "그래도 계속하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strChasu = hicExjongService.GetChasubyCode(SS1.ActiveSheet.Cells[i, 2].Text);
                        SS1.ActiveSheet.Cells[i, 24].Text = strChasu;
                        strRate1 = SS1.ActiveSheet.Cells[i, 11].Text;
                        strRate2 = SS1.ActiveSheet.Cells[i, 13].Text;
                        strGbChul = SS1.ActiveSheet.Cells[i, 17].Text;
                        strInwon = SS1.ActiveSheet.Cells[i, 18].Text;
                        strLtdCode = SS1.ActiveSheet.Cells[i, 26].Text;
                        strMailCode = SS1.ActiveSheet.Cells[i, 27].Text;
                        strJuso1 = SS1.ActiveSheet.Cells[i, 28].Text;
                        strJuso2 = SS1.ActiveSheet.Cells[i, 29].Text;
                        strBurate = SS1.ActiveSheet.Cells[i, 30].Text;
                        strTel = SS1.ActiveSheet.Cells[i, 34].Text;
                        break;
                    }
                }

                long nCount = 0;
                string strTemp = "";
                string strTemp1 = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCount = nCount + 1;

                        if(!FstrSExams.IsNullOrEmpty() && nCount > 1)
                        {
                            strTemp = ""; strTemp1 = "";
                            for (int j = 0; j < VB.L(FstrSExams,","); j++)
                            {
                                strTemp = VB.Pstr(FstrSExams, ",", j+1);
                                if ( VB.Left(strTemp,2) != "11")
                                {
                                    if (strTemp1.IsNullOrEmpty())
                                    {
                                        strTemp1 = strTemp;
                                    }
                                    else
                                    {
                                        strTemp1 = strTemp1 + "," + strTemp;
                                    }
                                }
                            }
                        }

                        strSExams = SS1.ActiveSheet.Cells[i, 55].Text;
                        if (strSExams.IsNullOrEmpty())
                        {
                            if (strTemp1.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[i, 21].Text = FstrSExams;
                                SS1.ActiveSheet.Cells[i, 10].Text = hm.SExam_Names_Display(FstrSExams);
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[i, 21].Text = strTemp1;
                                SS1.ActiveSheet.Cells[i, 10].Text = hm.SExam_Names_Display(strTemp1);
                            }
                            
                        }
                        else
                        {
                            if (strTemp1.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[i, 21].Text = FstrSExams + "," + strSExams;
                                SS1.ActiveSheet.Cells[i, 10].Text = hm.SExam_Names_Display(FstrSExams);
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[i, 21].Text = strTemp1 + "," + strSExams;
                                SS1.ActiveSheet.Cells[i, 10].Text = hm.SExam_Names_Display(FstrSExams);
                            }
                        }

                        SS1.ActiveSheet.Cells[i, 22].Text = FstrUCodes;
                        SS1.ActiveSheet.Cells[i, 12].Text = hm.UCode_Names_Display(FstrUCodes);

                        if (!FstrUCodes.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 31].Text = "2";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 31].Text = "1";
                        }
                        SS1.ActiveSheet.Cells[i, 11].Text = strRate1;
                        SS1.ActiveSheet.Cells[i, 13].Text = strRate2;
                        SS1.ActiveSheet.Cells[i, 17].Text = strGbChul;
                        SS1.ActiveSheet.Cells[i, 18].Text = strInwon;
                        SS1.ActiveSheet.Cells[i, 26].Text = strLtdCode;
                        SS1.ActiveSheet.Cells[i, 27].Text = strMailCode;
                        SS1.ActiveSheet.Cells[i, 28].Text = strJuso1;
                        SS1.ActiveSheet.Cells[i, 29].Text = strJuso2;
                        SS1.ActiveSheet.Cells[i, 30].Text = strBurate;
                        SS1.ActiveSheet.Cells[i, 34].Text = strTel;
                    }
                }

                if (chkIEMunjin.Checked == true || chkExcel.Checked == true)
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        FnRow = i;
                        strSel = SS1.ActiveSheet.Cells[i, 0].Text;
                        strPANO = SS1.ActiveSheet.Cells[i, 3].Text;
                        strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                        strPtNo = SS1.ActiveSheet.Cells[i, 15].Text;
                        strLtdCode = SS1.ActiveSheet.Cells[i, 26].Text;
                        if (strSel == "True" && (strPANO == "신환" || strPtNo == "신환") && !strLtdCode.IsNullOrEmpty())
                        {
                            //외래번호
                            if (strPtNo == "신환")
                            {
                                clsDB.setBeginTran(clsDB.DbCon);
                                if (!strLtdCode.IsNullOrEmpty())
                                {
                                    HIC_LTD list = hicLtdService.GetMailCodebyCode(strLtdCode);

                                    if (!list.IsNullOrEmpty())
                                    {
                                        SS1.ActiveSheet.Cells[i, 27].Text = list.MAILCODE;
                                        SS1.ActiveSheet.Cells[i, 28].Text = list.JUSO;
                                        if (!list.MAILCODE.IsNullOrEmpty())
                                        {
                                            if (!fn_BAS_PATIENT_INSERT(i).IsNullOrEmpty())
                                            {
                                                SS1.ActiveSheet.Cells[i, 15].Text = "실패";
                                                SS1.ActiveSheet.Cells[i, 15].Font = new Font("굴림", 9, FontStyle.Bold);
                                                SS1.ActiveSheet.Cells[i, 15].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                                clsDB.setRollbackTran(clsDB.DbCon);
                                                MessageBox.Show("병원번호 생성 실패!! 전산실로 연락요망!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                return;
                                            }
                                            else
                                            {
                                                SS1.ActiveSheet.Cells[i, 15].Font = new Font("굴림", 9, FontStyle.Regular);
                                                SS1.ActiveSheet.Cells[i, 15].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("회사코드가 공란입니다. 회사코드 세팅후 다시 작업하여주십시오!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                }
                                clsDB.setCommitTran(clsDB.DbCon);
                            }
                            //검진번호
                            if (strPANO == "신환")
                            {
                                clsDB.setBeginTran(clsDB.DbCon);

                                nPano = hb.New_PatientNo_Create();
                                SS1.ActiveSheet.Cells[i, 3].Text = nPano.ToString().Trim();
                                strSname = SS1.ActiveSheet.Cells[i, 4].Text;
                                strPtNo = SS1.ActiveSheet.Cells[i, 15].Text;
                                switch (VB.Mid(strJumin, 7, 1))
                                {
                                    case "1":
                                    case "3":
                                    case "5":
                                    case "7":
                                        strSex = "M";
                                        break;
                                    case "2":
                                    case "4":
                                    case "6":
                                    case "8":
                                        strSex = "F";
                                        break;
                                    default:
                                        strSex = "";
                                        break;
                                }

                                int result = 0;
                                if(nPano>0)
                                {
                                    result = hicPatientService.InsertItem(nPano, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin), strSname, "", strPtNo, strSex, Convert.ToInt32(strLtdCode));
                                }

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("HIC_PATIENT 신규등록중 오류 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                clsDB.setCommitTran(clsDB.DbCon);



                            }
                        }
                    }
                }
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();

                cboJong.Text = "";
                txtLtdName.Text = "";
                sp.Spread_All_Clear(SS1);
                pnlSunap.Visible = false;
            }
            else if (sender == btnSearch)
            {
                int nRead1 = 0;
                int nREAD2 = 0;
                int nREAD3 = 0;
                int nREAD4 = 0;
                int nRow = 0;
                string strJepDate = "";
                string strFlag = "";
                string strOK = "";
                string strGOK = "";
                string strLtdCode = "";
                string strJong = "";
                string strJumin = "";
                string strSex = "";
                string strSecond_Exams = "";
                string strSecond_Sayu = "";

                string SQLJep = ""; //접수명단 SQL
                string SQLGaJep = ""; //가접수명단 SQL
                string SQLIEMun = ""; //인터넷명단 SQL
                string SQLName = "";  //명단작성자 SQL

                bool bGaJep = false;

                string strFrDate = "";
                string strToDate = "";

                //int nCnt = 0;
                int nCnt1 = 0;
                int nCnt2 = 0;
                int nCnt3 = 0;
                int nCnt4 = 0;

                string sQry = "";   //가접수대상 쿼리 실행 여부

                List<HIC_JEPSU_PATIENT> list1 = new List<HIC_JEPSU_PATIENT>();
                List<HIC_JEPSU_WORK> list2 = new List<HIC_JEPSU_WORK>();
                List<COMHPC> list3 = new List<COMHPC>();
                List<HIC_JEPSU_EXJONG_PATIENT> list4 = new List<HIC_JEPSU_EXJONG_PATIENT>();

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                sp.Spread_All_Clear(SS1);
                lblCnt.Text = "0 건";
                Application.DoEvents();

                pnlSunap.Visible = false;
                strLtdCode = VB.Pstr(txtLtdName.Text, ".", 1);

                if (strLtdCode.IsNullOrEmpty())
                {
                    strLtdCode = "0";
                }

                if (Fstr2ChaWrtno.IsNullOrEmpty() || Fstr2ChaWrtno.Count == 0)
                {
                    if (dtpFrDate.Value != dtpToDate.Value)
                    {
                        if (txtLtdName.Text.IsNullOrEmpty())
                        {
                            if (MessageBox.Show("회사코드없이 조회 하시겠습니까? 조회가 오래 걸릴수 있습니다.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }

                Cursor.Current = Cursors.WaitCursor;

                //1.이전 접수명단 포함
                if (chkJep.Checked == true)
                {
                    List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateLtdCode(strFrDate, strToDate, VB.Left(cboJong.Text, 2), long.Parse(strLtdCode));

                    nCnt1 = list.Count;
                    list1 = list;
                }

                //2.가접수 명단 포함
                if (chkGaJep.Checked == true)
                {
                    List<HIC_JEPSU_WORK> list = hicJepsuWorkService.GetItembyJepDateGjJongLtdCode(strFrDate, strToDate, VB.Left(cboJong.Text, 2), strLtdCode);

                    nCnt2 = list.Count;
                    list2 = list;
                }

                //3.인터넷 접수명단 포함
                if (chkIEMunjin.Checked == true)
                {
                    List<COMHPC> list = comHpcLibBService.GetHicIEMunjinItembyEntDateLtdName(strFrDate, strToDate, VB.Pstr(txtLtdName.Text, ".", 2));

                    nCnt3 = list.Count;
                    list3 = list;
                }

                //4.2차 가접수 대상
                if (chkMkName.Checked == true)
                {
                    if (strLtdCode.IsNullOrEmpty())
                    {
                        strLtdCode = "0";
                    }

                    List<HIC_JEPSU_EXJONG_PATIENT> list = hicJepsuExjongPatientService.GetItembyWrtNoJepDateGjJongLtdCode(strFrDate, strToDate, VB.Left(cboJong.Text, 2), long.Parse(strLtdCode), Fstr2ChaWrtno);

                    nCnt4 = list.Count;
                    list4 = list;
                }

                if (chkJep.Checked == true)
                {
                    if (nCnt1 > 0)
                    {
                        nRead1 = nCnt1;
                    }
                }

                if (chkGaJep.Checked == true)
                {
                    if (nCnt2 > 0)
                    {
                        nREAD2 = nCnt2;
                    }
                }

                if (chkIEMunjin.Checked == true)
                {
                    if (nCnt3 > 0)
                    {
                        nREAD3 = nCnt3;
                    }
                }

                //2차 가접수 대상
                if (chkMkName.Checked == true)
                {
                    if (nCnt4 > 0)
                    {
                        nREAD4 = nCnt4;
                    }
                }

                nRow = SS1.ActiveSheet.RowCount;

                if (nRead1 > 0)
                {
                    SS1.ActiveSheet.RowCount += nRead1;
                    progressBar1.Maximum = nRead1;
                    for (int i = 0; i < nRead1; i++)
                    {

                        strJumin = clsAES.DeAES(list1[i].JUMIN2);
                        strJepDate = list1[i].JEPDATE;

                        nRow += 1;
                        SS1.ActiveSheet.Cells[i, 1].Text = list1[i].JEPDATE;
                        SS1.ActiveSheet.Cells[i, 2].Text = list1[i].GJJONG + "." + hb.READ_GjJong_Name(list1[i].GJJONG);
                        SS1.ActiveSheet.Cells[i, 3].Text = list1[i].PANO.ToString();
                        SS1.ActiveSheet.Cells[i, 4].Text = list1[i].SNAME;
                        if (VB.Left(strJepDate, 4) != VB.Left(clsPublic.GstrSysDate, 4))
                        {
                            SS1.ActiveSheet.Cells[i, 5].Text = hb.READ_HIC_AGE_GESAN(strJumin).ToString() + "/" + list1[i].SEX;
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 5].Text = list1[i].AGE + "/" + list1[i].SEX;
                        }
                        SS1.ActiveSheet.Cells[i, 6].Text = list1[i].WRTNO.ToString();
                        SS1.ActiveSheet.Cells[i, 7].Text = hb.READ_Ltd_Name(list1[i].LTDCODE.ToString());
                        SS1.ActiveSheet.Cells[i, 8].Text = list1[i].BUSENAME;
                        SS1.ActiveSheet.Cells[i, 9].Text = strJumin;

                        SS1.ActiveSheet.Cells[i, 10].Text = hm.SExam_Names_Display(list1[i].SEXAMS);
                        SS1.ActiveSheet.Cells[i, 11].Value = 2;
                        SS1.ActiveSheet.Cells[i, 12].Text = hm.UCode_Names_Display(list1[i].UCODES);
                        SS1.ActiveSheet.Cells[i, 13].Value = 2;
                        SS1.ActiveSheet.Cells[i, 14].Text = list1[i].JISA;
                        SS1.ActiveSheet.Cells[i, 15].Text = list1[i].PTNO;
                        SS1.ActiveSheet.Cells[i, 16].Text = list1[i].KIHO;
                        SS1.ActiveSheet.Cells[i, 17].Text = list1[i].GBCHUL;
                        SS1.ActiveSheet.Cells[i, 18].Text = list1[i].GBINWON;
                        SS1.ActiveSheet.Cells[i, 19].Text = list1[i].GKIHO;
                        SS1.ActiveSheet.Cells[i, 21].Text = list1[i].SEXAMS;
                        SS1.ActiveSheet.Cells[i, 22].Text = list1[i].UCODES;
                        SS1.ActiveSheet.Cells[i, 23].Text = VB.Left(dtpGJepDate.Text, 4);
                        SS1.ActiveSheet.Cells[i, 24].Text = list1[i].GJCHASU;
                        if (string.Compare(VB.Right(dtpGJepDate.Text, 5), "07-01") < 0)
                        {
                            SS1.ActiveSheet.Cells[i, 25].Text = "1";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 25].Text = "2";
                        }
                        SS1.ActiveSheet.Cells[i, 26].Text = list1[i].LTDCODE.ToString();
                        SS1.ActiveSheet.Cells[i, 27].Text = list1[i].MAILCODE;
                        SS1.ActiveSheet.Cells[i, 28].Text = list1[i].JUSO1;
                        SS1.ActiveSheet.Cells[i, 29].Text = list1[i].JUSO2;
                        SS1.ActiveSheet.Cells[i, 30].Text = list1[i].BURATE;
                        SS1.ActiveSheet.Cells[i, 31].Text = list1[i].JIKGBN;
                        SS1.ActiveSheet.Cells[i, 32].Text = list1[i].IPSADATE;
                        SS1.ActiveSheet.Cells[i, 33].Text = list1[i].GBDENTAL;
                        SS1.ActiveSheet.Cells[i, 34].Text = list1[i].TEL;
                        SS1.ActiveSheet.Cells[i, 35].Text = "N";
                        SS1.ActiveSheet.Cells[i, 36].Text = list1[i].BOGUNSO;
                        SS1.ActiveSheet.Cells[i, 37].Text = list1[i].YOUNGUPSO;
                        SS1.ActiveSheet.Cells[i, 38].Text = list1[i].MILEAGEAM;
                        SS1.ActiveSheet.Cells[i, 39].Text = list1[i].MURYOAM;
                        SS1.ActiveSheet.Cells[i, 40].Text = list1[i].GUMDAESANG;
                        SS1.ActiveSheet.Cells[i, 41].Text = list1[i].MILEAGEAMGBN;
                        SS1.ActiveSheet.Cells[i, 42].Text = list1[i].MURYOGBN;
                        SS1.ActiveSheet.Cells[i, 43].Text = list1[i].REMARK;
                        SS1.ActiveSheet.Cells[i, 44].Text = list1[i].EMAIL;
                        SS1.ActiveSheet.Cells[i, 51].Text = list1[i].HPHONE;
                        SS1.ActiveSheet.Cells[i, 52].Text = list1[i].SEX;

                        //14종은 부담율을 S란에 표시함
                        if (list1[i].GJJONG == "14" && list1[i].BURATE != "02")
                        {
                            SS1.ActiveSheet.Cells[i, 11].Text = list1[i].BURATE;
                            SS1.ActiveSheet.Cells[i, 13].Text = list1[i].BURATE;
                        }
                        progressBar1.Value = i + 1;
                    }
                }

                nRow = SS1.ActiveSheet.RowCount;
                if (nREAD2 > 0)
                {
                    SS1.ActiveSheet.RowCount += nREAD2;
                    progressBar1.Maximum += nREAD2;
                    for (int i = 0; i < nREAD2; i++)
                    {
                        nRow += 1;
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].JEPDATE.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list2[i].GJJONG + "." + hb.READ_GjJong_Name(list2[i].GJJONG);
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list2[i].PANO.ToString();
                        if (SS1.ActiveSheet.Cells[nRow - 1, 3].Text.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "신환";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list2[i].SNAME;
                        strJepDate = list2[i].JEPDATE.ToString();
                        if (VB.Left(strJepDate, 4) != VB.Left(clsPublic.GstrSysDate, 4))
                        {
                            if (!list2[i].JUMINNO2.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(list2[i].JUMINNO2).ToString() + "/" + list2[i].SEX;
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(list2[i].JUMINNO).ToString() + "/" + list2[i].SEX;
                            }
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list2[i].AGE + "/" + list2[i].SEX;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = hb.READ_Ltd_Name(list2[i].LTDCODE.ToString());
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list2[i].BUSENAME;
                        if (!list2[i].JUMINNO2.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = clsAES.DeAES(list2[i].JUMINNO2);
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = clsAES.DeAES(list2[i].JUMINNO);
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = hm.SExam_Names_Display(list2[i].SEXAMS);
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Value = 2;
                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = hm.UCode_Names_Display(list2[i].UCODES);
                        SS1.ActiveSheet.Cells[nRow - 1, 13].Value = 2;
                        SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list2[i].JISA;
                        SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list2[i].PTNO;
                        SS1.ActiveSheet.Cells[nRow - 1, 16].Text = list2[i].KIHO;
                        SS1.ActiveSheet.Cells[nRow - 1, 17].Text = list2[i].GBCHUL;
                        SS1.ActiveSheet.Cells[nRow - 1, 18].Text = list2[i].GBINWON;
                        SS1.ActiveSheet.Cells[nRow - 1, 19].Text = list2[i].GKIHO;
                        SS1.ActiveSheet.Cells[nRow - 1, 21].Text = list2[i].SEXAMS;
                        SS1.ActiveSheet.Cells[nRow - 1, 22].Text = list2[i].UCODES;
                        SS1.ActiveSheet.Cells[nRow - 1, 23].Text = VB.Left(dtpGJepDate.Text, 4);
                        SS1.ActiveSheet.Cells[nRow - 1, 24].Text = list2[i].GJCHASU;
                        if (string.Compare(VB.Right(dtpGJepDate.Text, 5), "07-01") < 0)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 25].Text = "1";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 25].Text = "2";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 26].Text = list2[i].LTDCODE.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 27].Text = list2[i].MAILCODE;
                        SS1.ActiveSheet.Cells[nRow - 1, 28].Text = list2[i].JUSO1;
                        SS1.ActiveSheet.Cells[nRow - 1, 29].Text = list2[i].JUSO2;
                        SS1.ActiveSheet.Cells[nRow - 1, 30].Text = list2[i].BURATE;

                        SS1.ActiveSheet.Cells[nRow - 1, 31].Text = list2[i].JIKGBN;
                        SS1.ActiveSheet.Cells[nRow - 1, 32].Text = list2[i].IPSADATE.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 33].Text = list2[i].GBDENTAL;
                        SS1.ActiveSheet.Cells[nRow - 1, 34].Text = list2[i].TEL;
                        SS1.ActiveSheet.Cells[nRow - 1, 35].Text = "N";
                        SS1.ActiveSheet.Cells[nRow - 1, 36].Text = list2[i].BOGUNSO;
                        SS1.ActiveSheet.Cells[nRow - 1, 37].Text = list2[i].YOUNGUPSO;
                        SS1.ActiveSheet.Cells[nRow - 1, 38].Text = list2[i].MILEAGEAM;
                        SS1.ActiveSheet.Cells[nRow - 1, 39].Text = list2[i].MURYOAM;
                        SS1.ActiveSheet.Cells[nRow - 1, 40].Text = list2[i].GUMDAESANG;
                        SS1.ActiveSheet.Cells[nRow - 1, 41].Text = list2[i].MILEAGEAMGBN;
                        SS1.ActiveSheet.Cells[nRow - 1, 42].Text = list2[i].MURYOGBN;
                        SS1.ActiveSheet.Cells[nRow - 1, 43].Text = list2[i].REMARK;
                        SS1.ActiveSheet.Cells[nRow - 1, 44].Text = list2[i].EMAIL;
                        SS1.ActiveSheet.Cells[nRow - 1, 47].Text = list2[i].CLASS.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 48].Text = list2[i].BAN.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 49].Text = list2[i].BUN.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 51].Text = list2[i].HPHONE;
                        SS1.ActiveSheet.Cells[nRow - 1, 52].Text = list2[i].SEX;
                        //ROW 높이 설정
                        SS1_Sheet1.Rows[nRow - 1].Height = 30;

                        //14종은 부담율을 S란에 표시함
                        if (list2[i].GJJONG == "14" && list2[i].BURATE != "02")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 11].Text = list2[i].BURATE;
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = list2[i].BURATE;
                        }
                        progressBar1.Value = i + 1;
                    }
                }

                nRow = SS1.ActiveSheet.RowCount;
                if (nREAD3 > 0)
                {
                    SS1.ActiveSheet.RowCount += nREAD3;
                    progressBar1.Maximum += nREAD3;
                    for (int i = 0; i < nREAD3; i++)
                    {
                        strJumin = clsAES.DeAES(list3[i].JUMIN2);

                        //가접수된것은 표시 안함
                        if (hicJepsuWorkPatientHeaJepsuService.GetPaNoPtNobyJumin2GjYear(clsAES.AES(strJumin), VB.Left(dtpGJepDate.Text, 4)) > 0)
                        {
                            bGaJep = true;
                            SS1.ActiveSheet.RowCount -= 1;
                        }

                        if (bGaJep == false)
                        {
                            //검진종류가 같은것만 선택
                            switch (list3[i].GJJONG)
                            {
                                case "1":
                                case "2":
                                    strJong = "56";
                                    break;
                                case "3":
                                    strJong = "13";
                                    break;
                                case "4":
                                case "5":
                                    strJong = "11";
                                    break;
                                default:
                                    break;
                            }

                            if (strJong == VB.Left(cboJong.Text, 2))
                            {
                                nRow += 1;
                                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list3[i].ENTDATE.ToString();
                                switch (list3[i].GJJONG)
                                {
                                    case "1":
                                    case "2":
                                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "56.학생검진";
                                        break;
                                    case "3":
                                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "13.성인병";
                                        break;
                                    case "4":
                                    case "5":
                                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "11.사업장1차";
                                        break;
                                    default:
                                        break;
                                }

                                SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list3[i].SNAME;
                                strSex = "M";

                                switch (VB.Mid(strJumin, 7, 1))
                                {
                                    case "1":
                                    case "3":
                                    case "5":
                                    case "7":
                                        strSex = "M";
                                        break;
                                    case "2":
                                    case "4":
                                    case "6":
                                    case "8":
                                        strSex = "F";
                                        break;
                                    default:
                                        strSex = "";
                                        break;
                                }

                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list3[i].AGE + "/" + strSex;
                                SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list3[i].LTDNAME;
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strJumin;
                                SS1.ActiveSheet.Cells[nRow - 1, 27].Text = list3[i].MAILCODE;
                                SS1.ActiveSheet.Cells[nRow - 1, 28].Text = list3[i].JUSO1;
                                SS1.ActiveSheet.Cells[nRow - 1, 29].Text = list3[i].JUSO2;
                                SS1.ActiveSheet.Cells[nRow - 1, 34].Text = list3[i].TEL;
                                SS1.ActiveSheet.Cells[nRow - 1, 46].Text = "Y";
                                SS1.ActiveSheet.Cells[nRow - 1, 47].Text = list3[i].CLASS;
                                SS1.ActiveSheet.Cells[nRow - 1, 48].Text = list3[i].BAN;
                                SS1.ActiveSheet.Cells[nRow - 1, 49].Text = list3[i].BUN;
                                SS1.ActiveSheet.Cells[nRow - 1, 51].Text = list3[i].HPHONE;
                                SS1.ActiveSheet.Cells[nRow - 1, 52].Text = strSex;
                            }
                            else
                            {
                                SS1.ActiveSheet.RowCount -= 1;
                            }
                        }
                        progressBar1.Value = i + 1;
                    }
                }

                //인터넷 문진자 환자정보 Display
                if (chkIEMunjin.Checked == true)
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 45].Text == "Y")
                        {
                            HIC_PATIENT listPat = hicPatientService.GetItembyJumin2(clsAES.AES(SS1.ActiveSheet.Cells[i, 9].Text));

                            if (!listPat.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[i, 3].Text = listPat.PANO.ToString();
                                SS1.ActiveSheet.Cells[i, 3].Font = new Font("굴림", 10, FontStyle.Bold);
                                SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                                SS1.ActiveSheet.Cells[i, 14].Text = listPat.JISA;
                                SS1.ActiveSheet.Cells[i, 15].Text = listPat.PTNO;
                                SS1.ActiveSheet.Cells[i, 19].Text = listPat.GKIHO;
                                SS1.ActiveSheet.Cells[i, 26].Text = listPat.LTDCODE.ToString();
                                SS1.ActiveSheet.Cells[i, 27].Text = listPat.MAILCODE;
                                SS1.ActiveSheet.Cells[i, 28].Text = listPat.JUSO1;
                                SS1.ActiveSheet.Cells[i, 29].Text = listPat.JUSO2;
                                SS1.ActiveSheet.Cells[i, 32].Text = listPat.IPSADATE.ToString("yyyy-MM-dd");
                                SS1.ActiveSheet.Cells[i, 35].Text = listPat.LIVER2;
                                SS1.ActiveSheet.Cells[i, 36].Text = listPat.BOGUNSO;
                                SS1.ActiveSheet.Cells[i, 38].Text = listPat.YOUNGUPSO;
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[i, 3].Text = "신환";
                                SS1.ActiveSheet.Cells[i, 3].Font = new Font("굴림", 10, FontStyle.Bold);
                                SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                                //병원환자마스터에서 찾기
                                BAS_PATIENT listPat2 = basPatientService.GetCountbyJumin1Jumin3(VB.Left(strJumin, 6), clsAES.AES(VB.Right(strJumin, 7)));
                                if (!listPat2.IsNullOrEmpty())
                                {
                                    //검진만 신환
                                    SS1.ActiveSheet.Cells[i, 15].Text = listPat2.PANO;
                                }
                                else
                                {
                                    //검진/병원 둘다 신환
                                    SS1.ActiveSheet.Cells[i, 15].Text = "신환";
                                    SS1.ActiveSheet.Cells[i, 3].Font = new Font("굴림", 10, FontStyle.Bold);
                                    SS1.ActiveSheet.Cells[i, 15].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                                }
                            }
                        }
                    }
                }

                //2차명단 작성자 명단
                nRow = SS1.ActiveSheet.RowCount;
                if (nREAD4 > 0)
                {
                    SS1.ActiveSheet.RowCount += nREAD4;
                    progressBar1.Maximum += nREAD4;
                    for (int i = 0; i < nREAD4; i++)
                    {
                        bGaJep = false;
                        strJumin = clsAES.DeAES(list4[i].JUMIN2);
                        switch (VB.Left(cboJong.Text, 2))
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "41":
                            case "42":
                            case "43":
                            case "23":
                                break;
                            default:
                                bGaJep = true;
                                break;
                        }

                        //가접수된것은 표시 안함
                        if (hicJepsuWorkPatientHeaJepsuService.GetCountbyJuminGjYearGjJong(clsAES.AES(strJumin), VB.Left(dtpGJepDate.Text, 4), VB.Left(cboJong.Text, 2)) > 0)
                        {
                            bGaJep = true;
                            SS1.ActiveSheet.RowCount -= 1;
                        }

                        //가접수대상 접수여부를 다시 확인함
                        if (bGaJep == false)
                        {
                            switch (VB.Left(cboJong.Text, 2))
                            {
                                case "11":
                                case "12":
                                case "13":
                                case "14":
                                case "41":
                                case "42":
                                case "43":
                                case "23":
                                    break;
                                default:
                                    sQry = "N";
                                    break;
                            }

                            if (sQry == "")
                            {
                                if (hicJepsuPatientService.GetCountbyJuminGjYear(clsAES.AES(strJumin), VB.Left(dtpGJepDate.Text, 4), VB.Left(cboJong.Text, 2), strFrDate) > 0)
                                {
                                    bGaJep = true;
                                    SS1.ActiveSheet.RowCount -= 1;
                                }
                            }
                        }

                        if (bGaJep == false)
                        {
                            nRow += 1;
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list4[i].JEPDATE.ToString();
                            switch (list4[i].GJJONG)
                            {
                                case "11":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "16." + hb.READ_GjJong_Name("16");
                                    break;
                                case "12":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "17." + hb.READ_GjJong_Name("17");
                                    break;
                                case "13":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "18." + hb.READ_GjJong_Name("18");
                                    break;
                                case "14":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "19." + hb.READ_GjJong_Name("19");
                                    break;
                                case "41":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "44." + hb.READ_GjJong_Name("44");
                                    break;
                                case "42":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "45." + hb.READ_GjJong_Name("45");
                                    break;
                                case "43":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "46." + hb.READ_GjJong_Name("46");
                                    break;
                                case "23":
                                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = "28." + hb.READ_GjJong_Name("28");
                                    break;
                                default:
                                    break;
                            }
                            strJong = VB.Left(SS1.ActiveSheet.Cells[nRow - 1, 2].Text, 2);
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list4[i].PANO.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list4[i].SNAME;

                            strJepDate = list4[i].JEPDATE.ToString();
                            if (VB.Left(strJepDate, 4) != VB.Left(clsPublic.GstrSysDate, 4))
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list4[i].JUMIN2)) + "/" + list4[i].SEX;
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list4[i].AGE + "/" + list4[i].SEX;
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list4[i].WRTNO.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = hb.READ_Ltd_Name(list4[i].LTDCODE.ToString());
                            SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list4[i].BUSENAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strJumin;
                            strSecond_Exams = list4[i].SECOND_EXAMS;
                            strSecond_Sayu = list4[i].SECOND_SAYU;
                            if (strJong == "28")
                            {
                                if (strSecond_Exams.IsNullOrEmpty() && strSecond_Sayu == "소음성난청의심")
                                {
                                    strSecond_Exams = "N1";
                                }
                            }
                            SS1.ActiveSheet.Cells[nRow - 1, 10].Text = fn_Read_SExams_Name(fn_Read_Second_SExams(strSecond_Exams + ",", strJong, list4[i].AGE));
                            SS1.ActiveSheet.Cells[nRow - 1, 11].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 12].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list4[i].JISA;
                            SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list4[i].PTNO;
                            SS1.ActiveSheet.Cells[nRow - 1, 16].Text = list4[i].KIHO;
                            SS1.ActiveSheet.Cells[nRow - 1, 17].Text = "N"; //2차대상자는 기본은 내원검진으로 등록
                            SS1.ActiveSheet.Cells[nRow - 1, 18].Text = list4[i].GBINWON;
                            SS1.ActiveSheet.Cells[nRow - 1, 19].Text = list4[i].GKIHO;
                            SS1.ActiveSheet.Cells[nRow - 1, 20].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 21].Text = fn_Read_Second_SExams(strSecond_Exams + ",", strJong, list4[i].AGE);
                            SS1.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 23].Text = VB.Left(dtpGJepDate.Text, 4);
                            SS1.ActiveSheet.Cells[nRow - 1, 24].Text = "2";
                            SS1.ActiveSheet.Cells[nRow - 1, 25].Text = string.Compare(VB.Right(dtpGJepDate.Text, 5), "07-01") < 0 ? "1" : "2";
                            SS1.ActiveSheet.Cells[nRow - 1, 26].Text = list4[i].LTDCODE.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 27].Text = list4[i].MAILCODE;
                            SS1.ActiveSheet.Cells[nRow - 1, 28].Text = list4[i].JUSO1;
                            SS1.ActiveSheet.Cells[nRow - 1, 29].Text = list4[i].JUSO2;
                            //2차시 부담률 회사로
                            SS1.ActiveSheet.Cells[nRow - 1, 30].Text = list4[i].BURATE.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 31].Text = list4[i].JIKGBN;
                            SS1.ActiveSheet.Cells[nRow - 1, 32].Text = list4[i].IPSADATE.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 33].Text = "N";
                            SS1.ActiveSheet.Cells[nRow - 1, 34].Text = list4[i].TEL;
                            SS1.ActiveSheet.Cells[nRow - 1, 35].Text = "N";
                            SS1.ActiveSheet.Cells[nRow - 1, 36].Text = list4[i].BOGUNSO;
                            SS1.ActiveSheet.Cells[nRow - 1, 37].Text = list4[i].YOUNGUPSO;
                            SS1.ActiveSheet.Cells[nRow - 1, 38].Text = list4[i].MILEAGEAM;
                            SS1.ActiveSheet.Cells[nRow - 1, 39].Text = list4[i].MURYOAM;
                            SS1.ActiveSheet.Cells[nRow - 1, 40].Text = list4[i].GUMDAESANG;
                            SS1.ActiveSheet.Cells[nRow - 1, 41].Text = list4[i].MILEAGEAMGBN;
                            SS1.ActiveSheet.Cells[nRow - 1, 42].Text = list4[i].MURYOGBN;
                            SS1.ActiveSheet.Cells[nRow - 1, 43].Text = list4[i].REMARK;
                            SS1.ActiveSheet.Cells[nRow - 1, 44].Text = list4[i].EMAIL;
                            SS1.ActiveSheet.Cells[nRow - 1, 51].Text = list4[i].HPHONE;
                            SS1.ActiveSheet.Cells[nRow - 1, 52].Text = list4[i].SEX;
                        }
                        progressBar1.Value = i + 1;
                    }

                    switch (VB.Left(cboJong.Text, 2))
                    {
                        case "11":
                            cboJong.SelectedIndex = cboJong.FindString("16.사업장2차", -1);
                            break;
                        case "12":
                            cboJong.SelectedIndex = cboJong.FindString("17.공무원2차", -1);
                            break;
                        case "13":
                            cboJong.SelectedIndex = cboJong.FindString("18.성인병2차", -1);
                            break;
                        case "14":
                            cboJong.SelectedIndex = cboJong.FindString("19.사업장(회사부담)2차", -1);
                            break;
                        case "41":
                            cboJong.SelectedIndex = cboJong.FindString("44.생애검진(사업장)2차", -1);
                            break;
                        case "42":
                            cboJong.SelectedIndex = cboJong.FindString("45.생애검진(공무원)2차", -1);
                            break;
                        case "43":
                            cboJong.SelectedIndex = cboJong.FindString("46.생애검진(성인병)2차", -1);
                            break;
                        case "23":
                            cboJong.SelectedIndex = cboJong.FindString("28.특수검진2차", -1);
                            break;
                        default:
                            break;
                    }
                }

                //2020-01-08 줄바꿈표시 기능 제외 요청으로 막음
                //for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                //{
                //    Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 10);
                //    Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 12);

                //    if (size.Height > size1.Height)
                //    {
                //        SS1.ActiveSheet.Rows[i].Height = size.Height;
                //    }
                //    else if (size1.Height > size.Height)
                //    {
                //        SS1.ActiveSheet.Rows[i].Height = size1.Height;
                //    }
                //    else
                //    {
                //        SS1.ActiveSheet.Rows[i].Height = 20;
                //    }
                //}

                Cursor.Current = Cursors.Default;
            }
            else if (sender == btnRun)
            {
                int nREAD = 0;
                int nREAD2 = 0;
                int nCNT = 0;

                string strROWID = "";
                string strJepDate = "";

                string strPANO = "";
                string strName = "";
                string strJumin = "";
                string strSex = "";
                string strAge = "";
                string strGjChasu = "";
                string strGjBangi = "";
                string strGbChul = "";
                string strLtdCode = "";
                string strMailCode = "";
                string strJuso1 = "";
                string strJuso2 = "";
                string strPtNo = "";
                string strBurate = "";
                string strJisa = "";
                string strkiho = "";
                string strGkiho = "";
                string strJikGbn = "";

                string strSExams = "";
                string strIpsadate = "";
                string strBuseName = "";
                string strDental = "";
                string strGbInwon = "";
                string strBogen = "";
                string strTel = "";
                string strLiver2 = "";
                string strRemark = "";
                string strEmail = "";
                string strGJepDate = "";
                string strYear = "";
                string strYoungupso = "";
                string strMileageAm = "";
                string strMuryoAm = "";
                string strGumDaesang = "";
                string strMileageGbn = "";
                string strMuRyoGbn = "";
                string strClass = "";
                string strBan = "";
                string strBun = "";
                string strHPhone = "";
                string strGongjeng = "";
                string strSaBun = "";
                string str회사코드설정 = "";

                int result = 0;

                //자리 및 공란점검
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strName = SS1.ActiveSheet.Cells[i, 4].Text;

                    strBuseName = SS1.ActiveSheet.Cells[i, 8].Text;
                    if (VB.Len(strBuseName) > 25)
                    {
                        MessageBox.Show(i + 1 + "번줄 회사명 점검요망!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }

                    strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                    if (!strJumin.IsNullOrEmpty() && strName.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + 1 + "번줄 성명이 공란입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }

                //가접수 적용일자
                strGJepDate = dtpGJepDate.Text;
                nCNT = 0;

                //Inx = cboDept.FindString(strDeptCode);
                //cboDept.SelectedIndex = Inx;

                if (chkMkName.Checked == true)
                {
                    switch (VB.Left(cboJong.Text, 2))
                    {
                        case "11":
                            cboJong.SelectedIndex = cboJong.FindString("16.사업장2차", -1);
                            break;
                        case "12":
                            cboJong.SelectedIndex = cboJong.FindString("17.공무원2차", -1);
                            break;
                        case "13":
                            cboJong.SelectedIndex = cboJong.FindString("18.성인병2차", -1);
                            break;
                        case "14":
                            cboJong.SelectedIndex = cboJong.FindString("19.사업장(회사부담)2차", -1);
                            break;
                        case "41":
                            cboJong.SelectedIndex = cboJong.FindString("44.생애검진(사업장)2차", -1);
                            break;
                        case "42":
                            cboJong.SelectedIndex = cboJong.FindString("45.생애검진(공무원)2차", -1);
                            break;
                        case "43":
                            cboJong.SelectedIndex = cboJong.FindString("46.생애검진(성인병)2차", -1);
                            break;
                        case "23":
                            cboJong.SelectedIndex = cboJong.FindString("28.특수검진2차", -1);
                            break;
                        default:
                            break;
                    }
                }
                //자료건수 및 점검
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCNT += 1;
                    }
                }

                if (MessageBox.Show(nCNT + " 명의 자료를 가접수 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                fn_DATA_ERROR_CHECK();

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True") //선택한것만 가접수함.
                    {
                        fn_Screen_Clear();

                        strJepDate = ""; strGjjong = ""; strPANO = ""; FstrPANO = ""; strName = ""; strJumin = ""; FstrJumin = ""; strSex = ""; strAge = ""; strGjChasu = ""; strGjBangi = "";
                        strGbChul = ""; strLtdCode = ""; strMailCode = ""; strJuso1 = ""; strJuso2 = ""; strPtNo = ""; strBurate = ""; FstrBurate = ""; strJisa = "";
                        strkiho = ""; strGkiho = ""; strJikGbn = ""; strUCodes = ""; FstrUCodes = ""; strSExams = ""; FstrSExams = ""; strIpsadate = ""; strBuseName = ""; strDental = "";
                        strGbInwon = ""; strBogen = ""; strTel = ""; strLiver2 = ""; strRemark = ""; strEmail = ""; strYear = "";
                        strYoungupso = ""; strMileageAm = ""; strMuryoAm = ""; strGumDaesang = ""; strMileageGbn = ""; strMuRyoGbn = "";
                        strROWID = ""; strClass = ""; strBan = ""; strBun = ""; strHPhone = ""; strSaBun = "";

                        //변수저장
                        strJepDate = SS1.ActiveSheet.Cells[i, 1].Text;
                        strGjjong = VB.Left(SS1.ActiveSheet.Cells[i, 2].Text, 2);
                        strPANO = SS1.ActiveSheet.Cells[i, 3].Text;
                        strName = SS1.ActiveSheet.Cells[i, 4].Text;
                        strAge = VB.Pstr(SS1.ActiveSheet.Cells[i, 5].Text, "/", 1);
                        strSex = VB.Pstr(SS1.ActiveSheet.Cells[i, 5].Text, "/", 2);
                        strBuseName = SS1.ActiveSheet.Cells[i, 8].Text;
                        if ( VB.Len(strBuseName) > 25) { MessageBox.Show(i+1 + "번줄 회사명 점검요망!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); return;  }
                        strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                        FstrJumin = VB.Mid(strJumin, 7, 1);
                        strJisa = SS1.ActiveSheet.Cells[i, 14].Text;
                        strPtNo = SS1.ActiveSheet.Cells[i, 15].Text;
                        strkiho = SS1.ActiveSheet.Cells[i, 16].Text;
                        strGbChul = SS1.ActiveSheet.Cells[i, 17].Text;
                        strGbInwon = SS1.ActiveSheet.Cells[i, 18].Text;
                        strGkiho = SS1.ActiveSheet.Cells[i, 19].Text;

                        strSExams = SS1.ActiveSheet.Cells[i, 21].Text;
                        FstrSExams = strSExams;
                        strUCodes = SS1.ActiveSheet.Cells[i, 22].Text;
                        FstrUCodes = strUCodes;
                        //strYear = VB.Left(SS1.ActiveSheet.Cells[i, 23].Text, 4);
                        strYear = VB.Left(dtpGJepDate.Text, 4);
                        strGjChasu = SS1.ActiveSheet.Cells[i, 24].Text;
                        strGjBangi = SS1.ActiveSheet.Cells[i, 25].Text;
                        strLtdCode = SS1.ActiveSheet.Cells[i, 26].Text;
                        strMailCode = SS1.ActiveSheet.Cells[i, 27].Text;
                        strJuso1 = SS1.ActiveSheet.Cells[i, 28].Text;
                        strJuso2 = SS1.ActiveSheet.Cells[i, 29].Text;
                        strBurate = SS1.ActiveSheet.Cells[i, 30].Text;
                        FstrBurate = strBurate;
                        strJikGbn = SS1.ActiveSheet.Cells[i, 31].Text;
                        strIpsadate = SS1.ActiveSheet.Cells[i, 32].Text;
                        strDental = SS1.ActiveSheet.Cells[i, 33].Text;
                        strTel = SS1.ActiveSheet.Cells[i, 34].Text;

                        strLiver2 = SS1.ActiveSheet.Cells[i, 35].Text;
                        strBogen = SS1.ActiveSheet.Cells[i, 36].Text;
                        strYoungupso = SS1.ActiveSheet.Cells[i, 37].Text;
                        strMileageAm = SS1.ActiveSheet.Cells[i, 38].Text;
                        strMuryoAm = SS1.ActiveSheet.Cells[i, 39].Text;
                        strGumDaesang = SS1.ActiveSheet.Cells[i, 40].Text;
                        strMileageGbn = SS1.ActiveSheet.Cells[i, 41].Text;
                        strMuRyoGbn = SS1.ActiveSheet.Cells[i, 42].Text;
                        strRemark = SS1.ActiveSheet.Cells[i, 43].Text;
                        strEmail = SS1.ActiveSheet.Cells[i, 44].Text;
                        strClass = SS1.ActiveSheet.Cells[i, 47].Text;
                        strBan = SS1.ActiveSheet.Cells[i, 48].Text;
                        strBun = SS1.ActiveSheet.Cells[i, 49].Text;
                        if (!SS1.ActiveSheet.Cells[i, 51].Text.IsNullOrEmpty())
                        {
                            strHPhone = SS1.ActiveSheet.Cells[i, 51].Text.Trim();
                            strHPhone = VB.Replace(strHPhone, "'", "");
                        }
                        strGongjeng = SS1.ActiveSheet.Cells[i, 53].Text;
                        strSaBun = SS1.ActiveSheet.Cells[i, 54].Text;

                        //가접수 상태 확인
                        strROWID = comHpcLibBService.GetRowIdbyPaNoGjYearGjJong(strPANO, strYear, strGjjong);

                        //HIC_JEPSU_WORK();               //HIC_JEPSU_WORK 생성
                        //=====================================================================================
                        clsDB.setBeginTran(clsDB.DbCon);

                        str회사코드설정 = hm.Hic_LtdCode_Set(strGjjong, strLtdCode);

                        HIC_JEPSU_WORK item = new HIC_JEPSU_WORK();

                        item.GJYEAR = strYear;
                        item.JEPDATE = strGJepDate;
                        item.PANO = long.Parse(strPANO);
                        item.SNAME = strName;
                        item.SEX = strSex;
                        item.AGE = long.Parse(strAge);
                        item.GJJONG = strGjjong;
                        item.TEL = strTel;
                        item.GJCHASU = strGjChasu;
                        item.GJBANGI = strGjBangi;
                        item.LTDCODE = long.Parse(str회사코드설정);
                        item.BUSENAME = strBuseName;
                        item.MAILCODE = strMailCode;
                        item.JUSO1 = strJuso1;
                        item.JUSO2 = strJuso2;
                        item.UCODES = strUCodes;
                        item.SEXAMS = strSExams;
                        item.PTNO = strPtNo;
                        item.BURATE = strBurate;
                        item.JISA = strJisa.Trim();
                        item.KIHO = strkiho;
                        item.GKIHO = strGkiho;
                        item.JUMINNO = VB.Left(strJumin, 7) + "******";
                        item.JUMINNO2 = clsAES.AES(strJumin);
                        item.JIKGBN = strJikGbn;
                        item.GBCHUL = strGbChul;
                        item.GBDENTAL = strDental;
                        item.GBINWON = strGbInwon;
                        item.JOBSABUN = long.Parse(clsType.User.IdNumber);
                        item.BOGUNSO = strBogen;
                        item.LIVER2 = strLiver2;
                        item.YOUNGUPSO = strYoungupso;
                        if (strIpsadate.IsNullOrEmpty()) { strIpsadate = VB.Replace(strIpsadate, ".", "-");}
                        item.IPSADATE = strIpsadate; 
                        item.MILEAGEAM = strMileageAm;
                        item.MURYOAM = strMuryoAm;
                        item.MURYOGBN = strMuRyoGbn;
                        item.EMAIL = strEmail;
                        item.REMARK = strRemark;
                        if (strClass.IsNullOrEmpty())
                        {
                            item.CLASS = 0;
                        }
                        else
                        {
                            item.CLASS = long.Parse(strClass);
                        }
                        if (strBan.IsNullOrEmpty())
                        {
                            item.BAN = 0;
                        }
                        else
                        {
                            item.BAN = long.Parse(strBan);
                        }
                        if (strBun.IsNullOrEmpty())
                        {
                            item.BUN = 0;
                        }
                        else
                        {
                            item.BUN = long.Parse(strBun);
                        }
                        item.HPHONE = strHPhone;
                        item.SABUN = strSaBun;
                        if (strGjjong == "69")
                        {
                            item.GBADDPAN = "Y";
                        }
                        else
                        {
                            item.GBADDPAN = "";
                        }
                        item.RID = strROWID;

                        if (strROWID.IsNullOrEmpty())
                        {
                            //가접수마스터에 INSERT
                            result = hicJepsuWorkService.InsertAll(item);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("가접수 저장시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        //가접수 수정
                        else
                        {
                            result = hicJepsuWorkService.UpdateAllbyRowId(item);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("가접수 수정 저장시 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        //환자마스터에 증번호 UPDATE
                        result = hicPatientService.UpdatebyPtno(strGongjeng, strBuseName, strBan, strHPhone, strGkiho, strPtNo);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("증번호 UPDATE시 오류가 발생함!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        clsDB.setCommitTran(clsDB.DbCon);
                        //=====================================================================================

                        fn_Read_Sunap_Item_Groups();            //HIC_SUNAPDTL_WORK 확인
                        fn_Read_Sunap_Exam_Items();             //검사항목 세팅
                        fn_HIC_SUNAPDTL_WORK(i, strPANO);       //HIC_SUNAPDTL_WORK 생성
                        eBtnClick(btnAmt, new EventArgs());     //금액확인
                        fn_HIC_SUNAP_WORK(strPANO, strGjjong); //HIC_SUNAP_WORK 생성

                        SS1.ActiveSheet.Cells[i, 0].Text = ""; SS1.ActiveSheet.Cells[i, 0].Locked = true;
                    }
                }


                FstrSExams = "";
                FstrUCodes = "";
                MessageBox.Show(nCNT + " 건의 자료를 가접수 처리하였습니다", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == btnAddHelp)
            {
                List<string> strJong = new List<string>();

                strJong.Add(VB.Left(cboJong.Text, 2));

                FrmHcSAmtSelect = new frmHcSAmtSelect(strJong, this, "");
                FrmHcSAmtSelect.rSetGstrValue += new frmHcSAmtSelect.SetGstrValue(Exam_Value);
                FrmHcSAmtSelect.ShowDialog();
                FrmHcSAmtSelect.rSetGstrValue -= new frmHcSAmtSelect.SetGstrValue(Exam_Value);

                SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 18].Text = FstrSExams;
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 10].Text = hm.SExam_Names_Display(FstrSExams);

                txtAddExam.Text = FstrSExams + "\r\n\r\n" + hm.SExam_Names_Display(FstrSExams);

                fn_Read_Sunap_Item_Groups();
                fn_Read_Sunap_Exam_Items();
                eBtnClick(btnAmt, new EventArgs());

            }
            else if (sender == btnUCodeHelp)
            {
                FrmHcHarmfulFactor = new frmHcHarmfulFactor(strUCodes);
                FrmHcHarmfulFactor.rSetGstrValue += new frmHcHarmfulFactor.SetGstrValue(HarmfulFactor_value);
                FrmHcHarmfulFactor.ShowDialog();
                FrmHcHarmfulFactor.rSetGstrValue -= new frmHcHarmfulFactor.SetGstrValue(HarmfulFactor_value);

                SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 19].Text = FstrUCodes;
                SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 11].Text = hm.UCode_Names_Display(FstrUCodes);

                SS3.ActiveSheet.Cells[2, 1].Text = FstrUCodes;
                SS3.ActiveSheet.Cells[3, 1].Text = hm.UCode_Names_Display(FstrUCodes);

                fn_Read_Sunap_Item_Groups();
                fn_Read_Sunap_Exam_Items();
                eBtnClick(btnAmt, new EventArgs());
            }
            else if (sender == btnAmt)
            {
                double nAmt = 0;
                string strBurate = "";
                string strGjjong = "";
                string strSExam = "";
                string strUCodes = "";
                string strSRate1 = "";
                string strSRate2 = "";
                double nTotAmt = 0;
                double nJohapAmt = 0;
                double nLtdAmt = 0;
                double nBoninAmt = 0;
                double nIpGumAmt = 0;
                string strSelf = "";
                double nJoRate = 0;
                double nLtdRate = 0;
                double nBonRate = 0;
                double nAmAmt1 = 0;
                double nAmAmt2 = 0;

                strGjjong = VB.Left(cboJong.Text, 2);
                strBurate = FstrBurate;

                strSRate1 = SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 11].Text;
                strSRate2 = SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 13].Text;
                strSExam = SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 21].Text;
                if (VB.Right(strSExam, 1) == ",")
                {
                    strSExam = VB.Left(strSExam, strSExam.Length - 1);
                }
                strUCodes = SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 22].Text;

                if (!strSExam.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(strSExam, ","); i++)
                    {
                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                        {
                            if (SS2.ActiveSheet.Cells[j, 0].Text.Trim() == VB.Pstr(strSExam, ",", i + 1))
                            {
                                if (SS2.ActiveSheet.Cells[j, 3].Text.IsNullOrEmpty())
                                {
                                    SS2.ActiveSheet.Cells[j, 3].Text = strSRate1;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!strUCodes.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(strUCodes, ","); i++)
                    {
                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                        {
                            if (SS2.ActiveSheet.Cells[j, 0].Text == VB.Pstr(strUCodes, ",", i))
                            {
                                if (SS2.ActiveSheet.Cells[j, 3].Text.IsNullOrEmpty())
                                {
                                    SS2.ActiveSheet.Cells[j, 3].Text = strSRate2;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 2].Text == "")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = "0";
                    }
                    nAmt = double.Parse(SS2.ActiveSheet.Cells[i, 2].Text);
                    strSelf = SS2.ActiveSheet.Cells[i, 3].Text;

                    if (strSelf.IsNullOrEmpty())
                    {
                        strSelf = strBurate;
                    }

                    switch (long.Parse(strSelf))
                    {
                        case 1: //조합100%
                            nJoRate = 100;
                            nLtdRate = 0;
                            nBonRate = 0;
                            break;
                        case 2: //회사100%
                            nJoRate = 0;
                            nLtdRate = 100;
                            nBonRate = 0;
                            break;
                        case 3: //본인100%
                            nJoRate = 0;
                            nLtdRate = 0;
                            nBonRate = 100;
                            break;
                        case 4: //조합,본인50%
                            nJoRate = 50;
                            nLtdRate = 0;
                            nBonRate = 50;
                            break;
                        case 5: //조합,회사50%
                            nJoRate = 50;
                            nLtdRate = 50;
                            nBonRate = 0;
                            break;
                        case 6: //회사,본인50%
                            nJoRate = 0;
                            nLtdRate = 50;
                            nBonRate = 50;
                            break;
                        default:
                            nJoRate = 0;
                            nLtdRate = 0;
                            nBonRate = 0;
                            break;
                    }

                    //금액을 계산
                    if (strGjjong == "31" && nJoRate == 50)
                    {
                        nAmAmt1 = 0;
                        nAmAmt2 = 0;
                        nAmAmt1 = Math.Truncate(nAmt * nJoRate / 100) / 10;
                        nAmAmt1 *= 10;
                        nAmAmt2 = nAmt - nAmAmt1;
                        nTotAmt += nAmt;
                        if (nJoRate != 0)
                        {
                            nJohapAmt += nAmAmt1;
                        }
                        if (nLtdRate != 0)
                        {
                            nLtdAmt += nAmAmt2;
                        }
                        if (nBonRate != 0)
                        {
                            nBoninAmt += nAmAmt2;
                        }
                    }
                    else
                    {
                        nTotAmt += nAmt;
                        if (nJoRate != 0)
                        {
                            nJohapAmt += Math.Truncate(nAmt * nJoRate / 100);
                        }
                        if (nLtdRate != 0)
                        {
                            nLtdAmt += Math.Truncate(nAmt * nLtdRate / 100);
                        }
                        if (nBonRate != 0)
                        {
                            nBoninAmt += Math.Truncate(nAmt * nBonRate / 100);
                        }
                    }
                }

                lblTotAmt.Text = string.Format("{0:###,###,##0}", nTotAmt);         //총검진비
                FnTotalAmt = long.Parse(lblTotAmt.Text.Replace(",", ""));
                lblJohapAmt.Text = string.Format("{0:###,###,##0}", nJohapAmt);                //조합부담
                FnJohapAmt = long.Parse(lblJohapAmt.Text.Replace(",", ""));
                lblLtdAmt.Text = string.Format("{0:###,###,##0}", nLtdAmt);                  //회사부담
                FnLtdAmt = long.Parse(lblLtdAmt.Text.Replace(",", ""));
                lblBoninAmt.Text = string.Format("{0:###,###,##0}", nBoninAmt);              //본인부담
                FnBoninAmt = long.Parse(lblBoninAmt.Text.Replace(",", ""));

                if (!txtCardAmt.Text.IsNullOrEmpty())
                {
                    FnCardAmt = long.Parse(hb.Comma_Clear(txtCardAmt.Text));
                }

                if (nLtdAmt > 0 && !txtHalin.Text.IsNullOrEmpty() && VB.Left(cboHalin.Text, 2) == "01")
                {
                    lblLtdAmt.Text = string.Format("{0:###,###,##0}", nLtdAmt - long.Parse(hb.Comma_Clear(txtHalin.Text)));
                }

                if (nBoninAmt > 0 && !txtMisu.Text.IsNullOrEmpty() && VB.Left(cboMisu.Text, 2) == "01")
                {
                    lblBoninAmt.Text = (nBoninAmt - double.Parse(hb.Comma_Clear(txtMisu.Text))).ToString();
                }
                if (txtMisu.Text.IsNullOrEmpty())
                {
                    txtMisu.Text = "0";
                }
                nIpGumAmt = nBoninAmt - double.Parse(txtMisu.Text);
                txtIpgum.Text = string.Format("{0:###,###,##0}", nIpGumAmt);  //입금액
                FnIpAmt = long.Parse(VB.Replace(txtIpgum.Text,",",""));
            }
        }

        void Exam_Value(string argValue)
        {
            FstrSExams = argValue;
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        string fn_BAS_PATIENT_INSERT(int argRow)
        {
            string rtnVal = "";

            string strPtNo = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strSname = "";
            string strPano1 = "";
            string strJiCode = "";
            string strSex = "";
            string strBirth = "";
            string strSDate = "";
            string strEdate = "";
            string strGbBirth = "";
            string strZipCode = "";
            string strZipCode1 = "";
            string strZipCode2 = "";
            string strJuso1 = "";
            string strJuso2 = "";
            string strTel = "";
            string strHPhone = "";
            string strGbSMS = "";
            string strDeptCode = "";
            string strDRCODE = "";
            int result = 0;


            strPtNo = SS1.ActiveSheet.Cells[FnRow, 15].Text;
            strJumin1 = VB.Left(SS1.ActiveSheet.Cells[FnRow, 9].Text, 6);
            strJumin2 = VB.Right(SS1.ActiveSheet.Cells[FnRow, 9].Text, 7);
            strSname = SS1.ActiveSheet.Cells[FnRow, 4].Text;
            strZipCode = SS1.ActiveSheet.Cells[FnRow, 27].Text;
            switch (VB.Left(strJumin2, 1))
            {
                case "1":
                case "3":
                case "5":
                case "7":
                case "0":
                    strSex = "남";
                    break;
                case "2":
                case "4":
                case "6":
                case "8":
                    strSex = "여";
                    break;
                default:
                    strSex = "";
                    break;
            }

            if (strJumin1.Length != 6 || strJumin2.Length != 7)
            {
                rtnVal = "오류";
                return rtnVal;
            }

            if (strSname.IsNullOrEmpty())
            {
                rtnVal = "오류";
                return rtnVal;
            }

            if (strZipCode.IsNullOrEmpty())
            {
                rtnVal = "오류";
                return rtnVal;
            }

            if (strSex.IsNullOrEmpty())
            {
                rtnVal = "오류";
                return rtnVal;
            }

            //등록번호가 있으면 환자마스타에 있는지 점검
            if (!strPtNo.IsNullOrEmpty())
            {
                if (basPatientService.GetPaNobyPaNo(strPtNo) == strPtNo)
                {
                    rtnVal = "";
                    return rtnVal;
                }
            }

            //환자마스타가 있는지 점검함
            BAS_PATIENT list = basPatientService.GetPaNobyJumin1Jumin3(strJumin1, clsAES.AES(strJumin2));

            if (!list.IsNullOrEmpty())
            {
                strPtNo = list.PANO;
                SS1.ActiveSheet.Cells[argRow, 15].Text = strPtNo;
                rtnVal = "";
                return rtnVal;
            }

            //환자마스타에 INSERT
            if (strSex != "남")
            {
                strSex = "F";
            }
            else
            {
                strSex = "M";
            }

            strBirth = ""; strGbBirth = "";
            strSDate = clsPublic.GstrSysDate;
            strEdate = clsPublic.GstrSysDate;
            strZipCode1 = VB.Left(strZipCode, 3);
            strZipCode2 = VB.Mid(strZipCode, 4, 3);
            strJuso2 = "";
            strTel = SS1.ActiveSheet.Cells[argRow, 33].Text;
            strHPhone = "";
            strGbSMS = "";
            strDeptCode = "HR";
            strDRCODE = "7101";

            //지역코드 설정
            strJiCode = "01";
            strJiCode = basMailnewService.GetMailJiyekbyMailCode(strZipCode);

            //---------( 병원의 신환번호를 부여함 )----------
            COMHPC list2 = comHpcLibBService.GetSeq_Pano();

            if (!list2.IsNullOrEmpty())
            {
                if (list2.PANO.IsNullOrEmpty())
                {
                    rtnVal = "오류";
                    return rtnVal;

                }
            }
            strPano1 = string.Format("{0:0000000}", list2.NEXTVAL);
            strPtNo = hb.PANO_LAST_CHAR(strPano1);

            //------( 환자마스타에 INSERT )-----------
            BAS_PATIENT item = new BAS_PATIENT();

            item.PANO = strPtNo;
            item.SNAME = strSname;
            item.SEX = strSex;
            item.JUMIN1 = strJumin1;
            item.JUMIN2 = VB.Left(strJumin2, 1) + "******";
            item.STARTDATE = strSDate;
            item.LASTDATE = strEdate;
            item.ZIPCODE1 = strZipCode1;
            item.ZIPCODE2 = strZipCode2;
            item.JUSO = strJuso2;
            item.JICODE = strJiCode;
            item.TEL = strTel;
            item.HPHONE = strHPhone;
            item.EMBPRT = " ";
            item.BI = "51";
            item.PNAME = VB.Left(strSname,5);
            item.GWANGE = "1";
            item.KIHO = "";
            item.GKIHO = "";
            item.DEPTCODE = strDeptCode;
            item.DRCODE = strDRCODE;
            item.GBSPC = "0";
            item.GBGAMEK = "00";
            item.BOHUN = "";
            item.REMARK = "";
            item.SABUN = "";
            item.BUNUP = "";
            item.BIRTH = strBirth;
            item.GBBIRTH = strGbBirth;
            item.EMAIL = "";
            item.GBINFOR = "";
            item.GBJUSO = "";
            item.GBSMS = strGbSMS;
            item.HPHONE2 = "";
            item.JUMIN3 = clsAES.AES(strJumin2);

            result = basPatientService.Insert(item);

            if (result < 0)
            {
                MessageBox.Show("환자 마스터 생성 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtnVal = "오류";
                return rtnVal;
            }

            SS1.ActiveSheet.Cells[argRow, 15].Text = strPtNo;
            rtnVal = "";

            return rtnVal;
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void HarmfulFactor_value(string argString)
        {
            FstrUCodes = argString;
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == SS1)
            {
                int nCnt = 0;

                if (e.Column != 0)
                {
                    return;
                }

                if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
                {
                    SS1.ActiveSheet.Cells[e.Row, 2, e.Row, 3].BackColor = Color.FromArgb(128, 128, 255);
                    SS1.ActiveSheet.Cells[e.Row, 5, e.Row, 9].BackColor = Color.FromArgb(128, 128, 255);
                }
                else
                {
                    SS1.ActiveSheet.Cells[e.Row, 2, e.Row, 3].BackColor = Color.FromArgb(255, 255, 255);
                    SS1.ActiveSheet.Cells[e.Row, 5, e.Row, 9].BackColor = Color.FromArgb(255, 255, 255);
                }

                nCnt = 0;
                FstrSelWRTNO = "";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCnt += 1;
                        FstrSelWRTNO += SS1.ActiveSheet.Cells[i, 6].Text + ",";
                    }
                }

                if (FstrSelWRTNO.Length > 1)
                {
                    FstrSelWRTNO = VB.Mid(FstrSelWRTNO, 1, FstrSelWRTNO.Length - 1);
                }

                lblCnt.Text = nCnt + "건";
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    return;
                }

                if (e.Column == 0)
                {
                    return;
                }

                switch (e.Column)
                {
                    case 0:
                    case 1:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
                        {
                            SS1.ActiveSheet.Cells[e.Row, 0].Text = "";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                        }
                        break;
                    default:
                        break;
                }

                FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(SS1);
                eSpdBtnClick(SS1, new EditorNotifyEventArgs(view, SS1, e.Row, 0));
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strTemp = "";
                string strSExams = "";

                if (e.ColumnHeader == true)
                {
                    switch (e.Column)
                    {
                        case 17:    //출장여부
                            strTemp = SS1.ActiveSheet.Cells[0, 17].Text;
                            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                            {
                                SS1.ActiveSheet.Cells[i, 17].Text = strTemp;
                            }
                            break;
                        case 31:    //직구분
                            strTemp = SS1.ActiveSheet.Cells[0, 31].Text;
                            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                            {
                                SS1.ActiveSheet.Cells[i, 31].Text = strTemp;
                            }
                            break;
                        case 33:    //구강
                            strTemp = SS1.ActiveSheet.Cells[0, 33].Text;
                            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                            {
                                SS1.ActiveSheet.Cells[i, 33].Text = strTemp;
                            }
                            break;
                        case 35:    //2차간염
                            strTemp = SS1.ActiveSheet.Cells[0, 35].Text;
                            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                            {
                                strTemp = SS1.ActiveSheet.Cells[i, 35].Text;
                            }
                            break;
                        case 39:    //무료암
                            strTemp = SS1.ActiveSheet.Cells[0, 39].Text;
                            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                            {
                                strTemp = SS1.ActiveSheet.Cells[i, 39].Text;
                            }
                            break;
                        case 5:
                            eSpdDClick(SS1, new CellClickEventArgs(new SpreadView(), -1, 52, 0, 0, new MouseButtons(), false, false));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (e.RowHeader == true)
                    {
                        SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                        pnlSunap.Visible = true;
                        FstrSExams = SS1.ActiveSheet.Cells[e.Row, 21].Text;
                        txtAddExam.Text = FstrSExams + "\r\n\r\n" + hm.SExam_Names_Display(FstrSExams);

                        FstrUCodes = SS1.ActiveSheet.Cells[e.Row, 22].Text;
                        txtSpcExam.Text = FstrUCodes + "\r\n\r\n" + hm.UCode_Names_Display(FstrUCodes);

                        FstrPANO = SS1.ActiveSheet.Cells[e.Row, 3].Text;
                        FstrBurate = SS1.ActiveSheet.Cells[e.Row, 30].Text;

                        fn_Read_Sunap_Item_Groups();
                        fn_Read_Sunap_Exam_Items();
                        eBtnClick(btnAmt, new EventArgs());
                    }
                    else if (e.Column == 10)
                    {
                        List<string> strJong = new List<string>();
                        strJong.Clear();
                        strJong.Add(VB.Left(cboJong.Text, 2));

                        FrmHcSAmtSelect = new frmHcSAmtSelect(strJong, this, "");
                        FrmHcSAmtSelect.rSetGstrValue += new frmHcSAmtSelect.SetGstrValue(Exam_Value);
                        FrmHcSAmtSelect.ShowDialog();
                        FrmHcSAmtSelect.rSetGstrValue -= new frmHcSAmtSelect.SetGstrValue(Exam_Value);

                        if (!FstrSExams.IsNullOrEmpty())
                        {
                            //SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 18].Text = FstrSExams;
                            SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 21].Text = FstrSExams;
                            SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 10].Text = hm.SExam_Names_Display(FstrSExams);

                            txtAddExam.Text = FstrSExams + "\r\n\r\n" + hm.SExam_Names_Display(FstrSExams);
                        }
                    }
                    else if (e.Column == 12)
                    {
                        strUCodes = SS1.ActiveSheet.Cells[e.Row, 22].Text;

                        FrmHcHarmfulFactor = new frmHcHarmfulFactor(strUCodes);
                        FrmHcHarmfulFactor.rSetGstrValue += new frmHcHarmfulFactor.SetGstrValue(HarmfulFactor_value);
                        FrmHcHarmfulFactor.ShowDialog();
                        FrmHcHarmfulFactor.rSetGstrValue -= new frmHcHarmfulFactor.SetGstrValue(HarmfulFactor_value);

                        if (!FstrUCodes.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 22].Text = FstrUCodes;
                            SS1.ActiveSheet.Cells[SS1.ActiveSheet.ActiveRowIndex, 12].Text = hm.UCode_Names_Display(FstrUCodes);

                            txtAddExam.Text = FstrUCodes + "\r\n\r\n" + hm.UCode_Names_Display(FstrUCodes);
                        }
                    }
                    else if (e.Column == 17)
                    {
                        if (SS1.ActiveSheet.Cells[e.Row, 17].Text != "Y")
                        {
                            SS1.ActiveSheet.Cells[e.Row, 17].Text = "Y";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[e.Row, 17].Text = "N";
                        }
                    }
                    else if (e.Column == 33)
                    {
                        if (SS1.ActiveSheet.Cells[e.Row, 33].Text != "Y")
                        {
                            SS1.ActiveSheet.Cells[e.Row, 33].Text = "Y";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[e.Row, 33].Text = "N";
                        }
                    }
                    else if (e.Column == 35)
                    {
                        if (SS1.ActiveSheet.Cells[e.Row, 35].Text != "Y")
                        {
                            SS1.ActiveSheet.Cells[e.Row, 35].Text = "Y";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[e.Row, 35].Text = "N";
                        }
                    }
                    else if (e.Column == 39)
                    {
                        if (SS1.ActiveSheet.Cells[e.Row, 39].Text != "Y")
                        {
                            SS1.ActiveSheet.Cells[e.Row, 39].Text = "Y";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[e.Row, 39].Text = "N";
                        }
                    }
                    else if (e.Column == 54)
                    {
                        SS1.ActiveSheet.Cells[e.Row, 54].Text = "Y";


                        FrmHcCodeHelp = new frmHcCodeHelp("A2");
                        FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(Code_value);
                        FrmHcCodeHelp.ShowDialog();
                        FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(Code_value);

                        if (!FstrCode.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[e.Row, 53].Text = FstrCode.Trim();
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[e.Row, 53].Text = "";
                        }
                    }
                }
            }
        }

        private void Code_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdName.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdHelp, new EventArgs());
                    }
                }
            }
        }

        void eChkBoxClick(object sender, EventArgs e)
        {
            if (sender == chkExcel)
            {
                if (chkExcel.Enabled == true)
                {
                    grpExcel.Enabled = true;
                    chkJep.Checked = false;
                    chkMkName.Checked = false;
                    btnSet.Enabled = true;
                }
                else
                {
                    grpExcel.Enabled = false;
                }
            }
            else if (sender == chkGaJep)
            {
                if (chkGaJep.Checked == true)
                {
                    chkJep.Checked = false;
                    chkMkName.Checked = false;
                    btnSet.Enabled = true;
                }
            }
            else if (sender == chkIEMunjin)
            {
                if (chkIEMunjin.Checked == true)
                {
                    chkJep.Checked = false;
                    chkMkName.Checked = false;
                    btnSet.Enabled = true;
                }
            }
            else if (sender == chkJep)
            {
                if (chkIEMunjin.Checked == true)
                {
                    chkExcel.Checked = false;
                    chkGaJep.Checked = false;
                    chkIEMunjin.Checked = false;
                    chkMkName.Checked = false;
                    btnSet.Enabled = true;
                }
            }
            else if(sender == chkMkName)
            {
                chkJep.Checked = false;
                chkExcel.Checked = false;
                chkIEMunjin.Checked = false;
                chkGaJep.Checked = false;
            }
            else if (sender == chkSelect)
            {
                int nCnt = 0;

                FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(SS1);

                if (chkSelect.Checked == true)
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        
                        if (SS1.ActiveSheet.Cells[i, 0].CellType != txt && !SS1.ActiveSheet.Cells[i, 1].Text.IsNullOrEmpty()  && SS1.ActiveSheet.Cells[i, 0].Locked == false)
                        {
                            SS1.ActiveSheet.Cells[i, 0].Text = "True";
                            eSpdBtnClick(SS1, new EditorNotifyEventArgs(view, SS1, i, 0));
                            nCnt += 1;
                        }
                    }
                    lblCnt.Text = nCnt + "건";
                }
                else
                {
                    for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                    {
                        if (SS1.ActiveSheet.Cells[i, 0].CellType != txt && !SS1.ActiveSheet.Cells[i, 1].Text.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[i, 0].Text = "False";
                            eSpdBtnClick(SS1, new EditorNotifyEventArgs(view, SS1, i, 0));
                        }
                    }
                    lblCnt.Text = "0 건";
                }
            }
        }

        void fn_Screen_Clear()
        {
            //검진비 금액
            lblTotAmt.Text = ""; lblJohapAmt.Text = "";
            lblLtdAmt.Text = ""; lblBoninAmt.Text = "";
            lblChaAmt.Text = "";
            cboHalin.Text = ""; txtHalin.Text = "";
            cboMisu.Text = ""; txtMisu.Text = "";
            txtIpgum.Text = ""; txtCardAmt.Text = "";

            FnTotalAmt = 0; FnJohapAmt = 0;
            FnLtdAmt = 0; FnBoninAmt = 0;
            FnIpAmt = 0; FnChaiAmt = 0;
            FnCardAmt = 0; FnRow = 0;

            //선택검사
            txtAddExam.Text = "";
            txtSpcExam.Text = "";

            FstrSExams = "";
            FstrUCodes = "";

        }

        /// <summary>
        /// 엑셀파일을 가접수 스프레드에 적용
        /// </summary>
        void fn_Impot_ExcelFile_Spread()
        {
            int nSS5Cnt = 0;
            int nCnt = 0;
            int nRow = 0;
            int nCol = 0;
            string strJumin = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strSex = "";
            string strChasu = "";
            string strOK = "";
            bool bHan = false;
            bool bGaJep = false;

            sp.Spread_All_Clear(SS1);
            bHan = true;

            for (int i = 0; i < SS5.ActiveSheet.RowCount; i++)
            {
                if(!SS5.ActiveSheet.Cells[i, 1].Text.IsNullOrEmpty())
                {
                    nSS5Cnt = nSS5Cnt + 1;
                }
                else
                {
                    break;
                }
            }

            //SS1.ActiveSheet.RowCount = SS5.ActiveSheet.RowCount;
            SS1.ActiveSheet.RowCount = nSS5Cnt;

            for (int i = 0; i < SS5.ActiveSheet.RowCount; i++)
            {
                nRow += 1;
                nCnt += 1;

                strJumin = SS5.ActiveSheet.Cells[i, 1].Text.Replace(" ", "");
                strJumin = strJumin.Trim();
                strJumin1 = VB.Left(strJumin, 6);
                strJumin2 = VB.Right(strJumin, 7);
                if (!strJumin.IsNullOrEmpty())
                {
                    strJumin = strJumin.Replace("-", "");
                    string ErrCheck = ComFunc.JuminNoCheck(clsDB.DbCon, strJumin1, strJumin2);
                    if (!ErrCheck.IsNullOrEmpty())
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = "주민번호오류";
                    }

                    //가접수된것은 표시 안함
                    bGaJep = false;
                    if (comHpcLibBService.GetJepsuWrokPatientCountbyJumin2GjYearGjJong(clsAES.AES(strJumin), VB.Left(dtpGJepDate.Text, 4), VB.Left(cboJong.Text, 2)) > 0)
                    {
                        bGaJep = true;
                        SS1.ActiveSheet.Rows[nRow - 1].Remove();
                        nRow -= 1;
                    }

                    if (bGaJep == false)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            switch (j)
                            {
                                case 0:
                                    nCol = 4;   //성명 
                                    break;
                                case 1:
                                    nCol = 9;   //주민번호 
                                    break;
                                case 2:
                                    nCol = 34;   //전화번호 
                                    break;
                                case 3:
                                    nCol = 51;   //휴대폰번호 
                                    break;
                                case 4:
                                    nCol = 47;   //학년 
                                    break;
                                case 5:
                                    nCol = 48;   //반 
                                    break;
                                case 6:
                                    nCol = 49;   //번호 
                                    break;
                                case 7:
                                    nCol = 8;   //부서 
                                    break;
                                case 8:
                                    nCol = 54;   //사번 
                                    break;
                                case 9:
                                    nCol = 32;   //입사일자 
                                    break;
                                case 10:
                                    nCol = 53;   //공정코드 
                                    break;
                                default:
                                    break;
                            }

                            if (j == 0)
                            {
                                strOK = "OK";
                                //문자가 한글인지 검사함.
                                if (VB.Asc(SS5.ActiveSheet.Cells[i, j].Text) >= 48 && VB.Asc(SS5.ActiveSheet.Cells[i, j].Text) <= 57)
                                {
                                    strOK = "NO";
                                    break;
                                }
                                else if (VB.Asc(SS5.ActiveSheet.Cells[i, j].Text) >= 65 && VB.Asc(SS5.ActiveSheet.Cells[i, j].Text) <= 90)
                                {
                                    strOK = "NO";
                                    break;
                                }
                                else if (VB.Asc(SS5.ActiveSheet.Cells[i, j].Text) >= 97 && VB.Asc(SS5.ActiveSheet.Cells[i, j].Text) <= 122)
                                {
                                    strOK = "NO";
                                    break;
                                }
                                if (strOK == "OK")
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = SS5.ActiveSheet.Cells[i, j].Text.Replace(" ", "").Trim();
                                }
                            }
                            else if (j == 1)
                            {
                                if (SS1.ActiveSheet.Cells[nRow - 1, nCol].Text != "주민번호오류")
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = strJumin;
                                    switch (VB.Mid(strJumin, 7, 1))
                                    {
                                        case "1":
                                        case "3":
                                        case "5":
                                        case "7":
                                        case "0":
                                            strSex = "M";
                                            break;
                                        case "2":
                                        case "4":
                                        case "6":
                                        case "8":
                                            strSex = "F";
                                            break;
                                        default:
                                            break;
                                    }
                                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN2(strJumin) + "/" + strSex;  //나이/성별
                                    SS1.ActiveSheet.Cells[nRow - 1, 52].Text = strSex;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (j == 4 || j == 5 || j == 6)
                            {
                            }
                            else if (j == 9)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = string.Format("{0:yyyy-MM-dd}", SS5.ActiveSheet.Cells[i, j].Text);
                            }
                            else if (j == 10)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = string.Format("{0:00000}", SS5.ActiveSheet.Cells[i, j].Text);
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, nCol].Text = SS5.ActiveSheet.Cells[i, j].Text;
                            }
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = dtpGJepDate.Text;
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = cboJong.Text;
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = hb.READ_Ltd_Name(VB.Pstr(txtLtdName.Text, ".", 1));
                        SS1.ActiveSheet.Cells[nRow - 1, 26].Text = VB.Pstr(txtLtdName.Text, ".", 1);
                        SS1.ActiveSheet.Cells[nRow - 1, 46].Text = "Y";
                    }
                }
            }

            strChasu = hicExjongService.GetChasubyCode(VB.Left(cboJong.Text, 2));

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                nRow = i+1;
                if (SS1.ActiveSheet.Cells[i, 46].Text == "Y")
                {
                    strJumin = SS1.ActiveSheet.Cells[i, 9].Text;
                    //검진번호 및 병원번호 찾기
                    HIC_PATIENT list = hicPatientService.GetItembyJumin2(clsAES.AES(strJumin));

                    if (!list.IsNullOrEmpty())
                    {
                        if (list.PANO.ToString() == "0")
                        {
                            SS1.ActiveSheet.Cells[i, 3].Text = "오류";
                            SS1.ActiveSheet.Cells[i, 3].Font = new Font("굴림", 10, FontStyle.Bold);
                            SS1.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFFFFF"));
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 3].Text = list.PANO.ToString();
                        }


                        switch (VB.Mid(strJumin, 7, 1))
                        {
                            case "1":
                            case "3":
                            case "5":
                            case "7":
                            case "0":
                                strSex = "M";
                                break;
                            case "2":
                            case "4":
                            case "6":
                            case "8":
                                strSex = "F";
                                break;
                            default:
                                break;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(strJumin) + "/" + strSex;  //나이/성별
                        SS1.ActiveSheet.Cells[nRow - 1, 14].Text = list.JISA;
                        SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list.PTNO;
                        if (list.PTNO.IsNullOrEmpty())
                        {
                            fn_Bas_Ptno_Update(clsAES.AES(strJumin), VB.Left(strJumin, 6), clsAES.AES(VB.Right(strJumin, 7)), nRow - 1);
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 24].Text = strChasu;
                        if (txtLtdName.Text.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 26].Text = list.LTDCODE.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 27].Text = list.MAILCODE;
                            SS1.ActiveSheet.Cells[nRow - 1, 28].Text = list.JUSO1;
                            SS1.ActiveSheet.Cells[nRow - 1, 29].Text = list.JUSO2;
                        }
                        if (SS1.ActiveSheet.Cells[nRow - 1, 34].Text.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 34].Text = list.TEL;
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 35].Text = list.LIVER2;
                        SS1.ActiveSheet.Cells[nRow - 1, 36].Text = list.BOGUNSO;
                        SS1.ActiveSheet.Cells[nRow - 1, 37].Text = list.YOUNGUPSO;
                        if (SS1.ActiveSheet.Cells[nRow - 1, 51].Text.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 51].Text = list.HPHONE;

                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 52].Text = strSex;
                    }
                    else
                    {
                        //주민번호 체크
                        string ErrCheck = ComFunc.JuminNoCheck(clsDB.DbCon, VB.Left(SS1.ActiveSheet.Cells[nRow - 1, 9].Text, 6), VB.Right(SS1.ActiveSheet.Cells[nRow - 1, 9].Text, 7));
                        if (!ErrCheck.IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "주민번호오류";
                            MessageBox.Show(nCnt + " 째줄 주민번호가 올바르지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = "신환";
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Font = new Font("굴림", 10, FontStyle.Bold);
                            SS1.ActiveSheet.Cells[nRow - 1, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                            SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strJumin;
                            //병원환자마스터에서 찾기
                            BAS_PATIENT list2 = basPatientService.GetItembyJumin1Jumin3(VB.Left(strJumin, 6), clsAES.AES(VB.Right(strJumin, 7)));

                            if (!list2.IsNullOrEmpty())
                            {
                                //검진만 신환
                                SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list2.PANO;
                            }
                            else
                            {
                                //검진/병원 둘다 신환
                                SS1.ActiveSheet.Cells[nRow - 1, 15].Text = "신환";
                                SS1.ActiveSheet.Cells[nRow - 1, 15].Font = new Font("굴림", 10, FontStyle.Bold);
                                SS1.ActiveSheet.Cells[nRow - 1, 15].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                            }
                        }
                    }
                }
            }
        }

        void fn_Bas_Ptno_Update(string argJumin, string argJumin1, string argJumin3, int argRow)
        {
            int result = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            //병원환자마스터에서 찾기
            BAS_PATIENT list = basPatientService.GetItembyJumin1Jumin3(argJumin1, argJumin3);

            if (!list.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[argRow, 15].Text = list.PANO;
                //환자마스터에 증번호 UPDATE
                result = hicPatientService.UpdatePtNobyJumin2(list.PANO, argJumin);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("병원번호 UPDATE시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                SS1.ActiveSheet.Cells[argRow, 15].Text = "신환";
                SS1.ActiveSheet.Cells[argRow, 15].Font = new Font("굴림", 10, FontStyle.Bold);
                SS1.ActiveSheet.Cells[argRow, 15].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 최근 검색한 자료가 있으면 다시 검색을 안함
        /// </summary>
        /// <param name="argJumin"></param>
        /// <param name="argSname"></param>
        void fn_Check_Nhic_History(string argJumin, string argSname)
        {
            WORK_NHIC list = workNhicService.GetItembyJuminSName(argJumin, argSname);

            if (!list.IsNullOrEmpty())
            {
                if (!list.REL.IsNullOrEmpty() && !list.YEAR.IsNullOrEmpty())
                {
                    strGkiho = list.GKIHO;   //증번호
                    strJisa = list.JISA;     //지사
                    strFirst = list.FIRST;   //1차검진
                    if (str생애구분 == "Y")
                    {
                        strLiver = list.LIVER2;
                    }
                }

                strNhicYN = "Y";

                if (!list.GBCHK01_NAME.IsNullOrEmpty()) str수검여부 = "Y";
                if (!list.GBCHK02_NAME.IsNullOrEmpty()) str수검여부 = "Y";
                if (!list.GBCHK03_NAME.IsNullOrEmpty()) str수검여부 = "Y";
            }
            else
            {
                strNhicChk = "N";
            }
        }

        string fn_Read_Jisa_Code(string argJisa)
        {
            string rtnVal = "";

            rtnVal = hicCodeService.GetCodebyName(VB.Pstr(argJisa, "지사", 1));

            return rtnVal;

        }

        /// <summary>
        /// Data오류점검
        /// </summary>
        void fn_DATA_ERROR_CHECK()
        {
            string strGjChasu2 = "";

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    if (SS1.ActiveSheet.Cells[i, 2].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 검진종류가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 3].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 건진번호가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 성명이 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 5].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 나이가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 9].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 주민번호가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 15].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 병원번호가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 17].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 출장검진 여부가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (SS1.ActiveSheet.Cells[i, 20].Text == "N")
                    {
                        MessageBox.Show(i + " 번째 줄 비대상입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    strGjChasu2 = SS1.ActiveSheet.Cells[i, 24].Text;

                    if (strGjChasu2 == "2" && SS1.ActiveSheet.Cells[i, 21].Text.IsNullOrEmpty())
                    {
                        MessageBox.Show(i + " 번째 줄 선택검사가 공란입니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (hf.GetLength(SS1.ActiveSheet.Cells[i, 27].Text.Trim()) != 5)
                    {
                        MessageBox.Show(i + " 번째 줄 우편번호 5자리가 아닙니다.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (hf.GetLength(SS1.ActiveSheet.Cells[i, 53].Text) > 5)
                    {
                        MessageBox.Show(i + " 번째 줄 공정코드가 5자리를 초과함.", "작업불가", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        void fn_Read_Sunap_Item_Groups()
        {
            int nREAD = 0;
            string strGJepDate = "";
            string strGjjong = "";
            string strUCodes = "";

            string strUCode = "";
            string strGjJong_Gubun1 = "";
            string strGjJong_Gubun2 = "";

            List<string> strUCodeSQL = new List<string>();
            List<string> strCodeSQL = new List<string>();

            strGJepDate = dtpGJepDate.Text;
            strGjjong = VB.Left(cboJong.Text, 2);

            //취급물질별 기본검사
            strUCodes = FstrUCodes;
            if (!strUCodes.IsNullOrEmpty())
            {
                strUCodeSQL.Clear();
                for (int k = 1; k <= VB.L(strUCodes, ","); k++)
                {
                    strUCode = VB.Pstr(strUCodes, ",", k);
                    if (!strUCode.IsNullOrEmpty())
                    {
                        strUCodeSQL.Add(strUCode);
                    }
                }
            }

            //선택검사
            strUCodes = FstrSExams;
            if (!clsHcType.TEC.SELCODE.IsNullOrEmpty() && strUCodes.IsNullOrEmpty())
            {
                strUCodes = clsHcType.TEC.SELCODE;
            }

            FstrSExams = strUCodes;
            txtAddExam.Text = FstrSExams + "\r\n\r\n" + hm.SExam_Names_Display(FstrSExams);

            if (!strUCodes.IsNullOrEmpty())
            {
                strCodeSQL.Clear();
                for (int k = 1; k <= VB.L(strUCodes, ","); k++)
                {
                    strUCode = VB.Pstr(strUCodes, ",", k);
                    if (!strUCode.IsNullOrEmpty())
                    {
                        strCodeSQL.Add(strUCode);
                    }
                }
            }

            //사업장1차,사업장100% 1차이고 특수검사를 선택하면 진찰료차액을 자동 발생
            if ((strGjjong == "11" || strGjjong == "12" || strGjjong == "14" || strGjjong == "41" || strGjjong == "42") && !FstrUCodes.IsNullOrEmpty())
            {
                strGjJong_Gubun1 = "1";
            }

            //특수검진(일특,특수,배치전,채용배치전)시 신장,체중,혈압이 포함되어야 함.
            if ((strGjjong == "11" || strGjjong == "12" || strGjjong == "14" || strGjjong == "41" || strGjjong == "42" || strGjjong == "22" || strGjjong == "23" || strGjjong == "24") && !FstrUCodes.IsNullOrEmpty())
            {
                strGjJong_Gubun2 = "1";
            }

            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 50;

            if (chkMkName.Checked == true)  //2차가접수대상
            {
                fn_READ_Sunap_ITEM_Second(strGJepDate);
            }
            else
            {
                fn_READ_Sunap_ITEM_GROUP_Sub(strGJepDate, strGjjong, clsHcType.TEC.GUBUN, strUCodeSQL, strCodeSQL, strGjJong_Gubun1, strGjJong_Gubun2);
            }

            fn_READ_Sunap_ITEM_OldAmt(strGJepDate, strGjjong);

            txtAddExam.Text = FstrSExams + "\r\n\r\n" + hm.SExam_Names_Display(FstrSExams);
            txtSpcExam.Text = FstrUCodes + "\r\n\r\n" + hm.UCode_Names_Display(FstrUCodes);
        }

        void fn_Read_Sunap_Exam_Items()
        {
            int nREAD = 0;
            int nRow = 0;
            string strCode = "";
            string strJong = "";
            string strHang = "";
            string strGroupGbSuga = "";
            string strGbSuga = "";
            List<string> strExamList = new List<string>();

            long nAmtNo = 0;
            long nPrice = 0;
            long nAmt = 0;

            sp.Spread_All_Clear(SS3);
            SS3.ActiveSheet.RowCount = 30;

            strJong = VB.Left(cboJong.Text, 2);

            nRow = 0;
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCode = SS2.ActiveSheet.Cells[i, 0].Text;
                strGroupGbSuga = SS2.ActiveSheet.Cells[i, 8].Text;

                //중복검사는 제거함
                strExamList.Clear();
                for (int n = 0; n < SS3.ActiveSheet.RowCount; n++)
                {
                    if (!SS3.ActiveSheet.Cells[n, 2].Text.IsNullOrEmpty())
                    {
                        strExamList.Add(SS3.ActiveSheet.Cells[n, 2].Text);
                    }
                }

                //자료를 Read
                List<HIC_GROUPEXAM_EXCODE> list = hicGroupexamExcodeService.GetItembyGroupCode(strCode, strExamList);

                nREAD = list.Count;
                nAmt = 0;   //묶음코드별 합계금액
                //SS3.ActiveSheet.RowCount = nREAD;
                if (nREAD == 0)
                {
                    nRow += 1;
                    if (nRow > SS3.ActiveSheet.RowCount)
                    {
                        SS3.ActiveSheet.RowCount = nRow;
                    }
                    SS3.ActiveSheet.Cells[nRow - 1, 0].Text = strCode;
                    SS3.ActiveSheet.Cells[nRow - 1, 1].Text = hb.READ_Group_Name(strCode);
                    SS3.ActiveSheet.Cells[nRow - 1, 2].Text = "";
                }
                else
                {
                    for (int j = 0; j < nREAD; j++)
                    {
                        strGroupGbSuga = list[j].GBSUGA;     //그룹
                        strGbSuga = list[j].SUGAGBN;        //검사항목
                        //묶음코드에 수가적용구분이 없으면 그룹코드의 구분으로 적용함.
                        if (strGbSuga.IsNullOrEmpty())
                        {
                            strGbSuga = strGroupGbSuga;
                        }

                        nAmtNo = long.Parse(strGbSuga);

                        //접수일자를 기준으로 현재수가,Old수가를 적용함
                        DateTime date1 = new DateTime();
                        DateTime date2 = new DateTime();
                        date1 = Convert.ToDateTime(dtpGJepDate.Text);
                        date2 = Convert.ToDateTime(list[j].SUDATE);
                        if (date1 >= date2)
                        {
                            switch (nAmtNo)
                            {
                                case 1:
                                    nPrice = list[j].AMT1;
                                    break;
                                case 2:
                                    nPrice = list[j].AMT2;
                                    break;
                                case 3:
                                    nPrice = list[j].AMT3;
                                    break;
                                case 4:
                                    nPrice = list[j].AMT4;
                                    break;
                                case 5:
                                    nPrice = list[j].AMT5;
                                    break;
                                case 6:
                                    nPrice = list[j].AMT6;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (nAmtNo)
                            {
                                case 1:
                                    nPrice = list[j].OLDAMT1;
                                    break;
                                case 2:
                                    nPrice = list[j].OLDAMT2;
                                    break;
                                case 3:
                                    nPrice = list[j].OLDAMT3;
                                    break;
                                case 4:
                                    nPrice = list[j].OLDAMT4;
                                    break;
                                case 5:
                                    nPrice = list[j].OLDAMT5;
                                    break;
                                case 6:
                                    nPrice = list[j].OLDAMT6;
                                    break;
                                default:
                                    break;
                            }
                        }

                        nRow += 1;
                        if (nRow > SS3.ActiveSheet.RowCount)
                        {
                            SS3.ActiveSheet.RowCount = nRow;
                        }

                        SS3.ActiveSheet.Cells[nRow - 1, 0].Text = list[j].GROUPCODE;
                        SS3.ActiveSheet.Cells[nRow - 1, 1].Text = list[j].GROUPNAME;
                        SS3.ActiveSheet.Cells[nRow - 1, 2].Text = list[j].EXCODE;
                        if (SS3.ActiveSheet.Cells[nRow - 1, 2].Text == "E902" && FstrJumin == "2")
                        {
                            MessageBox.Show(SS3.ActiveSheet.Cells[nRow - 1, 2].Text + " 코드 성별을 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (SS3.ActiveSheet.Cells[nRow - 1, 2].Text == "E903" && FstrJumin == "1")
                        {
                            MessageBox.Show(SS3.ActiveSheet.Cells[nRow - 1, 2].Text + " 코드 성별을 확인하세요", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        SS3.ActiveSheet.Cells[nRow - 1, 3].Text = string.Format("{0:###,###,##0}", nPrice);
                        SS3.ActiveSheet.Cells[nRow - 1, 4].Text = list[j].HNAME;

                        nAmt += nPrice;
                    }
                }
                SS2.ActiveSheet.Cells[i, 2].Text = string.Format("{0:###,###,##0}", nAmt);
            }
            SS3.ActiveSheet.RowCount = nRow;
        }

        void fn_HIC_SUNAPDTL_WORK(int argRow, string argPaNo)
        {
            string strCode = "";
            long nAmt = 0;
            string strGbSelf = "";
            string strUCode = "";
            string strBurate = "";
            string strJong = "";
            string strSRate1 = "";
            string strSRate2 = "";
            string strTemp1 = "";
            string strTemp2 = "";
            int result = 0;

            strJong = VB.Left(SS1.ActiveSheet.Cells[argRow, 2].Text, 2);

            //기존의 자료가 있으면 삭제함
            clsDB.setBeginTran(clsDB.DbCon);

            if (hicSunapdtlWorkService.GetCountSunapDtlWorkbyPanoJong(long.Parse(argPaNo), strJong) > 0)
            {
                result = hicSunapdtlWorkService.DeletebyPaNoGjJong(long.Parse(argPaNo), strJong);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("가접수 수납 묶음코드내역 삭제시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            strBurate = FstrBurate;

            //자료를 신규 INSERT
            for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
            {
                strJong = VB.Left(SS1.ActiveSheet.Cells[argRow, 2].Text, 2).Trim();
                strSRate1 = SS1.ActiveSheet.Cells[argRow, 11].Text.Trim();
                strSRate2 = SS1.ActiveSheet.Cells[argRow, 13].Text.Trim();
                strTemp1 = SS1.ActiveSheet.Cells[argRow, 21].Text.Trim();
                strTemp2 = SS1.ActiveSheet.Cells[argRow, 22].Text.Trim();

                if (VB.Right(strTemp1, 1) == ",")
                {
                    strTemp1 = VB.Left(strTemp1, strTemp1.Length - 1);
                }

                if (VB.Right(strTemp2, 1) == ",")
                {
                    strTemp2 = VB.Left(strTemp2, strTemp2.Length - 1);
                }

                if (!strTemp1.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(strTemp1, ","); i++)
                    {
                        for (int k = 0; k < SS2.ActiveSheet.RowCount; k++)
                        {
                            if (SS2.ActiveSheet.Cells[k, 0].Text.Trim() == VB.Pstr(strTemp1, ",", i))
                            {
                                //2017-05-08 선택검사는 부담율을은 무조건 입력한 부담율로 저장함(김재관)
                                //2017-06-16 선택검자 무조건 부담율적용은 16종 오류로 취소함(부장님)
                                //2017-09-06 14종은 무조건 입력한 부담율로 저장함(의뢰서)
                                if (strJong == "14")
                                {
                                    SS2.ActiveSheet.Cells[k, 3].Text = strSRate1;
                                }
                                else
                                {
                                    if (SS2.ActiveSheet.Cells[k, 3].Text.IsNullOrEmpty())
                                    {
                                        SS2.ActiveSheet.Cells[k, 3].Text = strSRate1;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                if (!strTemp2.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(strTemp2, ","); i++)
                    {
                        for (int k = 0; k < SS2.ActiveSheet.RowCount; k++)
                        {
                            if (SS2.ActiveSheet.Cells[k, 4].Text == VB.Pstr(strTemp2, ",", i))
                            {
                                SS2.ActiveSheet.Cells[k, 3].Text = strSRate2;
                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    switch (SS2.ActiveSheet.Cells[i, 0].Text.Trim())
                    {
                        case "1116":
                        case "1118":
                        case "1151":
                        case "1152":
                        case "1153":
                        case "1154":
                        case "1157":
                        
                        case "1160":
                        case "1164":
                        case "1165":
                        case "1166":
                        case "1163":
                        case "1167":
                            SS2.ActiveSheet.Cells[i, 3].Text = "1";
                            break;
                        default:
                            break;
                    }
                }

                //for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                //{
                //    if (SS2.ActiveSheet.Cells[i, 0].Text == "2302")
                //    {
                //        SS2.ActiveSheet.Cells[i, 3].Text = "2";
                //        break;
                //    }
                //}

                //유해인자가 야간작업만 있고 본인 부담이면(박세진 요청)
                if (strTemp2 == "V01" && strSRate2 == "3")
                {
                    for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                    {
                        switch (SS2.ActiveSheet.Cells[i, 0].Text.Trim())
                        {
                            case "2302":
                            case "JV01":
                            case "J224":
                                SS2.ActiveSheet.Cells[i, 3].Text = strSRate2;
                                break;
                            default:
                                break;
                        }
                    }
                }

                strCode = SS2.ActiveSheet.Cells[j, 0].Text;
                nAmt = long.Parse(SS2.ActiveSheet.Cells[j, 2].Text.Replace(",", ""));
                strGbSelf = SS2.ActiveSheet.Cells[j, 3].Text;
                strUCode = SS2.ActiveSheet.Cells[j, 4].Text;
                if (strGbSelf.IsNullOrEmpty())
                {
                    strGbSelf = strBurate;
                }

                HIC_SUNAPDTL_WORK item = new HIC_SUNAPDTL_WORK();

                item.WRTNO = 0;
                item.CODE = strCode.Trim();
                item.UCODE = strUCode;
                item.AMT = nAmt;
                item.GBSELF = strGbSelf;
                item.SUDATE = dtpGJepDate.Text;
                item.PANO = long.Parse(argPaNo);
                item.GJJONG = strJong;

                result = hicSunapdtlWorkService.Insert(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("가접수 검진항목 신규 등록시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_HIC_SUNAP_WORK(string argPaNo, string argGjjong)
        {
            int result = 0;

            //HIC_SUNAP_WORK 생성

            clsDB.setBeginTran(clsDB.DbCon);

            if (hicSunapWorkService.GetCountbyPaNoGjJong(long.Parse(argPaNo), argGjjong) > 0)
            {
                result = hicSunapWorkService.DeletebyPaNoGjJong(long.Parse(argPaNo), argGjjong);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("가접수 수납내역을 삭제중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //수납 상세내역 생성

            HIC_SUNAP_WORK item = new HIC_SUNAP_WORK();

            item.WRTNO = 0;
            item.SUDATE = dtpGJepDate.Text;
            item.SEQNO = 1;
            item.PANO = long.Parse(argPaNo);
            item.TOTAMT = long.Parse(lblTotAmt.Text.Replace(",", ""));
            item.HALINGYE = VB.Left(cboHalin.Text, 2);
            item.HALINAMT = long.Parse(txtHalin.Text == "" ? "0" : txtHalin.Text.Replace(",", ""));
            item.JOHAPAMT = long.Parse(lblJohapAmt.Text.Replace(",", ""));
            item.LTDAMT = long.Parse(lblLtdAmt.Text.Replace(",", ""));
            item.BONINAMT = long.Parse(lblBoninAmt.Text.Replace(",", ""));
            item.MISUGYE = VB.Left(cboMisu.Text, 2);
            item.MISUAMT = long.Parse(txtMisu.Text.Replace(",", ""));
            item.SUNAPAMT = long.Parse(txtIpgum.Text.Replace(",", ""));
            item.JOBSABUN = long.Parse(clsType.User.IdNumber);
            item.GJJONG = argGjjong;

            result = hicSunapWorkService.Insert(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("가접수 수납내역을 삭제중 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// READ_Sunap_ITEM_2차
        /// </summary>
        void fn_READ_Sunap_ITEM_Second(string argGJepDate)
        {
            List<string> strUCodeSQL = new List<string>();
            string strUCode = "";
            int nRead = 0;

            pnlSunap.Visible = true;

            //선택검사
            strUCodes = FstrSExams.Trim();
            //28종 J301을 2801로 강제 변경
            if (strGjjong == "28")
            {
                strUCodes = strUCodes.Replace("J301", "2801");
            }

            if (!strUCodes.IsNullOrEmpty())
            {
                strUCodeSQL.Clear();
                for (int k = 1; k <= VB.L(strUCodes, ","); k++)
                {
                    strUCode = VB.Pstr(strUCodes, ",", k);
                    if (!strUCode.IsNullOrEmpty())
                    {
                        strUCodeSQL.Add(strUCode);
                    }
                }
            }

            //수납 항목을 SELECT
            List<HIC_GROUPCODE> list = hicGroupcodeService.GetItembySDate(argGJepDate, strUCodeSQL);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS2.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                SS2.ActiveSheet.Cells[i, 2].Text = "";
                SS2.ActiveSheet.Cells[i, 3].Text = list[i].GBSELF;
                SS2.ActiveSheet.Cells[i, 4].Text = list[i].UCODE;
                SS2.ActiveSheet.Cells[i, 8].Text = list[i].GBSUGA;
            }
        }

        /// <summary>
        /// 묶음코드를 READ 
        /// </summary>
        void fn_READ_Sunap_ITEM_GROUP_Sub(string argGJepDate, string argGjJong, string argGubun, List<string> argUCodes, List<string> argCodes, string strGjJong_Gubun1, string strGjJong_Gubun2)
        {
            int nRead = 0;

            //수납 항목을 SELECT
            List<HIC_GROUPCODE> listSunap = hicGroupcodeService.GetItembyJepDateGjJong(argGJepDate, argGjJong, argGubun, argUCodes, argCodes, strGjJong_Gubun1, strGjJong_Gubun2);

            nRead = listSunap.Count;
            SS2.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS2.ActiveSheet.Cells[i, 0].Text = listSunap[i].CODE;
                SS2.ActiveSheet.Cells[i, 1].Text = listSunap[i].NAME;
                SS2.ActiveSheet.Cells[i, 2].Text = "";
                //SS2.ActiveSheet.Cells[i, 3].Text = "";
                if ((argGjJong == "11" || argGjJong == "12" || argGjJong == "14" || argGjJong == "41" || argGjJong == "42") && !argUCodes.IsNullOrEmpty())
                {
                    if (!listSunap[i].UCODE.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "2";
                    }
                    else if (listSunap[i].CODE.Trim() == "2302")
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "2";
                    }
                    else if (listSunap[i].CODE.Trim() == "J224")
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = "2";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[i, 3].Text = listSunap[i].GBSELF;
                    }
                }
                //14종은 입력한 부담율로 적용(의뢰서)
                else if (argGjJong == "14")
                {
                    SS2.ActiveSheet.Cells[i, 3].Text = FstrBurate;
                }
                SS2.ActiveSheet.Cells[i, 4].Text = listSunap[i].UCODE;
                SS2.ActiveSheet.Cells[i, 8].Text = listSunap[i].GBSUGA;
            }
        }

        /// <summary>
        /// 수납한 항목을 Display
        /// </summary>
        void fn_READ_Sunap_ITEM_OldAmt(string argGJepdate, string argGjJong)
        {
            int nRead = 0;

            List<HIC_SUNAPDTL_WORK> list = hicSunapdtlWorkService.GetItembyPaNoSuDateGjJong(FstrPANO, argGJepdate, argGjJong);

            strBurate = FstrBurate;

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strJisaCode = list[i].CODE;
                strGbSelf = list[i].CODE;

                for (int q = 0; q < SS2.ActiveSheet.RowCount; q++)
                {
                    if (SS2.ActiveSheet.Cells[q, 0].Text == strCode)
                    {
                        if (argGjJong != "14")
                        {
                            if (SS2.ActiveSheet.Cells[q, 0].Text == "2302")
                            {
                                if (SS2.ActiveSheet.Cells[q, 0].Text.IsNullOrEmpty())
                                {
                                    SS2.ActiveSheet.Cells[q, 3].Text = "2";
                                }
                            }
                            else
                            {
                                if (SS2.ActiveSheet.Cells[q, 0].Text.IsNullOrEmpty())
                                {
                                    SS2.ActiveSheet.Cells[q, 3].Text = "";
                                }
                            }
                            SS2.ActiveSheet.Cells[q, 5].Text = "1";                     //Old수량을 1로 SET
                            SS2.ActiveSheet.Cells[q, 6].Text = strGbSelf;               //Old Self
                            SS2.ActiveSheet.Cells[q, 7].Text = list[i].AMT.ToString();  //Old Amt
                        }
                    }
                }
            }
        }

        string fn_Read_SExams_Name(string argSExams)
        {
            string rtnVal = "";
            string strTemp = "";

            if (argSExams == ",")
            {
                return rtnVal;
            }

            for (int i = 0; i < VB.L(argSExams, ","); i++)
            {
                strTemp = hicGroupcodeService.GetNameByCode(VB.Pstr(argSExams, ",", i + 1));
                if (!strTemp.IsNullOrEmpty())
                {
                    rtnVal += strTemp + ",";
                }
            }

            return rtnVal;
        }

        string fn_Read_Second_SExams(string argSExam, string argJong, long argAge)
        {
            string rtnVal = "";
            string strCode = "";
            string strOK = "";
            string strUCode = "";
            int nCnt1 = 0;
            int nCnt2 = 0;
            long nAge = 0;
            string strTemp = "";

            if (argSExam == ",")
            {
                return rtnVal;
            }

            strCode = argJong + "01";
            nAge = argAge;

            for (int i = 0; i < VB.L(argSExam, ","); i++)
            {
                switch (VB.Pstr(argSExam, ",", i + 1))
                {
                    case "3":
                        if (VB.Left(argJong, 1) == "4")
                        {
                            rtnVal += argJong + "09,";
                        }
                        else
                        {
                            rtnVal += argJong + "34,";
                        }
                        break;
                    case "6":
                        if (VB.Left(argJong, 1) == "4")
                        {
                            rtnVal += argJong + "08,";
                        }
                        else
                        {
                            rtnVal += argJong + "65,";
                        }
                        break;
                    case "L00":
                    case "L01":
                        rtnVal += argJong + "04,";
                        strOK = "OK";
                        break;
                    case "L02":
                        rtnVal += argJong + "03,";
                        strOK = "OK";
                        break;
                    case "L03":
                        rtnVal += argJong + "05,";
                        strOK = "OK";
                        break;
                    case "L04":
                        rtnVal += argJong + "07,";
                        strOK = "OK";
                        break;
                    case "L05":
                        rtnVal += argJong + "06,";
                        strOK = "OK";
                        break;
                    case "L06": //우울증검사
                        if (nAge == 40)
                        {
                            rtnVal += argJong + "10,";
                        }
                        else if (nAge == 66)
                        {
                            rtnVal += argJong + "11,";
                        }
                        break;
                    case "L07": //인지기능검사
                        if (nAge == 66)
                        {
                            rtnVal += argJong + "12,";
                        }
                        else if (nAge == 70 || nAge == 74)
                        {
                            rtnVal += argJong + "35,";
                        }
                        break;
                    default:
                        strTemp = hicCodeService.GetGCodebyGubunCode("53", VB.Pstr(argSExam, ",", i + 1));

                        if (!strTemp.IsNullOrEmpty())
                        {
                            rtnVal += strTemp + ",";
                        }
                        strUCode = "OK";
                        break;
                }
            }

            //특수검진 2차만 있는지 점검하여 특수검진만 2차이면 J301 코드 추가
            nCnt1 = 0;
            nCnt2 = 0;
            for (int i = 0; i < VB.L(argSExam, ","); i++)
            {
                if (VB.Left(argSExam, 1) == "4")
                {
                    nCnt1 += 1; //공단검진2차
                }
                else
                {
                    switch (VB.Pstr(argSExam, ",", i))
                    {
                        case "3":
                        case "6":
                            nCnt1 += 1; //공단검진2차
                            break;
                        case "L01":
                        case "L02":
                        case "L03":
                        case "L04":
                        case "L05":
                        case "L06":
                        case "L07":
                            nCnt1 += 1;  //공단검진2차
                            break;
                        default:
                            nCnt2 += 1; //특수검진2차
                            break;
                    }
                }
            }

            //특수만 재검이 있는 경우
            if (argJong == "18")
            {
                rtnVal = strCode + ", " + rtnVal;
            }
            else if (VB.Left(argJong, 1) == "4")
            {
                rtnVal = strCode + ", " + rtnVal;
            }
            else if (nCnt1 == 0 && nCnt2 > 0)
            {
                rtnVal = "J301" + "," + rtnVal;
            }
            //공단2차 재검이 있는 경우
            else if (nCnt1 > 0)
            {
                rtnVal = strCode + "," + rtnVal;
            }

            if (VB.Left(argJong, 1) == "4")
            {
                //생활습관검사 5개항목에만 생활습관검사 코드 추가함..
                if (strOK == "OK")
                {
                    rtnVal = argJong + "02" + rtnVal;
                }
            }

            return rtnVal;
        }
    }
}
