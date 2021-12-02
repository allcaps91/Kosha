using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComSupLibB.SupLbEx;
using System.Data;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Drawing;
using FarPoint.Win.Spread;
using System.Text;
using System.ComponentModel;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaAct.cs
/// Description     : 검사결과 등록 / 변경
/// Author          : 이상훈
/// Create Date     : 2019-08-20
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHaAct01.frm(FrmHaAct), FrmHaAct03.frm, FrmHcAct01.frm, FrmHcAct03.frm " />

namespace HC_Act
{
    public partial class frmHaAct : BaseForm
    {
        HicResultService hicResultService = null;
        BasPcconfigService basPcconfigService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicExcodeService hicExcodeService = null;
        HicBcodeService hicBcodeService = null;
        clsSpread sp = null;
        HeaJepsuService heaJepsuService = null;
        HicBarcodeReqService hicBarcodeReqService = null;
        ActingCheckService actingCheckService = null;
        PatientInfoService patientInfoService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HicJepsuService hicJepsuService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResultActiveService hicResultActiveService = null;
        WaitCheckService waitCheckService = null;
        ExamDisplayService examDisplayService = null;
        HicRescodeService hicRescodeService = null;
        HeaWomenService heaWomenService = null;
        HicResultHisService hicResultHisService = null;
        SelectJeplistService selectJeplistService = null;
        HicResSpecialService hicResSpecialService = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        BasBcodeService basBcodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicPatientService hicPatientService = null;
        HicXrayResultService hicXrayResultService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicSunapService hicSunapService = null;
        HicJepsuResultService hicJepsuResultService = null;
        XrayResultnewService xrayResultnewService = null;
        HicWaitService hicWaitService = null;
        HicCodeService hicCodeService = null;
        HeaResultService heaResultService = null;

        ComFunc cF = new ComFunc();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcAct hcact = new clsHcAct();
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsHaBase ha = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        SerialPort m_sp = null;

        FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

        //InBody InterFace 용 선언 ===================================================================================================================
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern long SendMessage(long hWnd, string wMsg, long wParam, string lParam);

        [DllImport("user32.dll", EntryPoint = "FindWindowExA")]
        private static extern long FindWindowEx(long hWind1, long hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        private static extern long GetWindow(long hWnd, string wCmd);

        [DllImport("user32.dll", EntryPoint = "GetClassNameA")]
        private static extern long GetClassName(long hWnd, string lpClassName, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "FindWindowA")]
        private static extern long FindWindow(string className, string lpWindowName);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(long hWnd, string wMsg, string wParam, long lParam);
        //=============================================================================================================================================

        const uint GW_HWNDFIRST = 0;
        const uint GW_HWNDNEXT = 2;
        const uint GW_CHILD = 5;
        const string WM_GETTEXT = "&HD";
        const string WM_GETTEXTLENGTH = "&HE";
        const string WM_SETTEXT = "&HC";
        const string WM_COMMAND = "&H111";

        long FnWRTNO;
        string strJepDate;
        string FstrRoom;
        string FstrRoom2;       // 등록한 방 번호
        long FnRowNo;           // 메모리타자기 위치 저장용
        long FnClickRow;        // Help를 Click한 Row
        string FstrPartExam;    // 파트별 입력할 검사항목       

        string FstrSex;         // 성별
        long FnAge;
        string FstrGjYear;
        string FstrGjJong;
        long FnHeaWRTNO;        // 종합검진 접수번호
        long FnHcWRTNO;         // 일반건진 접수번호
        string FstrName;
        string FstrJumin;
        string FstrPtno = "";
        long FnPano;
        //string FstrPartG;
        List<string> FstrPartG = new List<string>();
        string FstrTemp;
        string FstrSDate;
        string FstrCOMMIT;
        string FstrGongDan;

        //DateTime? FstrJepDate;
        string FstrJepDate;
        string FstrGbChul;

        long FnTimer;           // 자동액팅 대기시간(초)
        bool FbExamBarCodeReq;
        string FstrBuffer;

        string InBodyVer;       //inBody 버전

        ////////////////////////////////////////////////
        /// FrmHcAct.frm 변수
        ////////////////////////////////////////////////
        long FnWrtno2;
        bool FbUAScanPc;          //소변검체 바코드 읽는 PC 여부(True/False)
        bool FbUAScanSave;        //소변검사 자동저장 여부
        string FstrWaitRoom;      
        string FstrWaitName;      
        string Fstr청력인터페이스;
        string FstrComm1;         
        long FnTimerList;         
        bool FbAutoSave;          
        long FnTimerSave;         
        bool FbWeightPc;          //체중계 인터페이스 PC 여부
        double FnOLD_Height;      //종전 키
        double FnOLD_Weight;      //종전 몸무게
        ////////////////////////////////////////////////

        ////////////////////////////////////////////////
        ///Screen_Display 변수
        ///////////////////////////////////////////////
        string strBlood = "";
        string strSex = "";
        string strPart = "";

        int nREAD = 0;
        int nRow = 0;
        string strExcode = "";
        string strHName = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strNomal = "";
        string strGbHelp = "";

        long nHeaPano = 0;
        string strXrayno = "";

        string strToDate = "";
        string strTemp = "";
        string strYYYY = "";
        string[] strSpcExam = new string[6];

        int nCNT = 0;
        string strExCode = "";
        string strJumin;
        string str낙상주의;
        string strAllWrtno = "";

        ///////////////////////////////////////////////////
        /// btnSave 사용 변수
        ///////////////////////////////////////////////////
        //string strResult;
        string strCode;
        string strROWID;
        string strROWID1;
        string strPanjeng;
        string strChange;
        //string strResCode;
        string strResType;
        int nHEIGHT;
        int nWeight;
        string strBiman;

        double nEyeL;
        double nEyeR;
        int nEarL;
        int nEarR;
        int nBloodH;
        int nBloodL;
        int nResult;
        //int nREAD;

        long nDataCNT;
        long nResultCNT;
        string strGbSTS;
        string strEndoSo;
        string strWomen;    // 여성정밀 참고치
        string strWomen1;
        string strWomen2;
        string strActMemo;
        //string strSex;     
        int nAge;
        int nWait;
        string strSName;
        string strGjJong;
        string strNoise;
        string strMsg;
        //string strYYYY;

        string strCODE = "";
        string strNewPan = "";
        string strGb2Audio = "";
        int nHeight = 0;
        double nCha = 0;

        //접수마스타의 상태 설정용 변수
        int nNullCNT = 0;
        string strExamRemark = "";
        string strDispOk = "";
        string strNOIN1 = "";
        string strNOIN2 = "";
        string strNOIN3 = "";
        ///////////////////////////////////////////////////
        long FnActWrtno;    //액팅여부를 검사할 접수번호
        /// <summary>검체기본정보</summary>
        public DataTable gDtSpecCode;

        string FstrHeaLastTime;
        int FnTimerCNT;
        string FstrBDate;
        string FstrRetValue;

        public frmHaAct()
        {
            InitializeComponent();

            SetControl();

            SetEvent();
        }

        void SetControl()
        {
            sp = new clsSpread();

            hc.Read_INI(CboPart);

            lblExTitle.Text = CboPart.Text;

            for (int i = 0; i < CboPart.Items.Count; i++)
            {
                CboPart.SelectedIndex = i;
                if (VB.Pstr(CboPart.Text, ".", 1) == "13")
                {
                    if (MessageBox.Show("혈액검사 전용PC 세팅", "PC세팅", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        lblExTitle.Text = CboPart.Text;
                        break;
                    }
                    else
                    {
                        CboPart.SelectedIndex = 0;
                        lblExTitle.Text = CboPart.Text;
                        break;
                    }
                }
            }

            for (int i = 0; i < CboPart.Items.Count; i++)
            {
                CboPart.SelectedIndex = i;
                FstrPartG.Add(VB.Pstr(CboPart.Text, ".", 1));
            }

            if (CboPart.Items.Count > 0)
            {
                CboPart.SelectedIndex = 0;
            }
        }

        void SetEvent()
        {
            hicResultService = new HicResultService();
            basPcconfigService = new BasPcconfigService();
            hicExcodeService = new HicExcodeService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicBcodeService = new HicBcodeService();
            heaJepsuService = new HeaJepsuService();
            hicBarcodeReqService = new HicBarcodeReqService();
            actingCheckService = new ActingCheckService();
            heaSunapdtlService = new HeaSunapdtlService();
            hicJepsuService = new HicJepsuService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResultActiveService = new HicResultActiveService();
            waitCheckService = new WaitCheckService();
            examDisplayService = new ExamDisplayService();
            hicRescodeService = new HicRescodeService();
            heaWomenService = new HeaWomenService();
            hicResultHisService = new HicResultHisService();
            selectJeplistService = new SelectJeplistService();
            hicResSpecialService = new HicResSpecialService();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            basBcodeService = new BasBcodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicPatientService = new HicPatientService();
            hicXrayResultService = new HicXrayResultService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicJepsuResultService = new HicJepsuResultService();
            xrayResultnewService = new XrayResultnewService();
            hicWaitService = new HicWaitService();
            hicCodeService = new HicCodeService();
            heaResultService = new HeaResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSang.Click += new EventHandler(eBtnClick);

            this.timer.Tick += new EventHandler(eTimerTick);
            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timer2.Tick += new EventHandler(eTimerTick);
            this.timerAutoActing.Tick += new EventHandler(eTimerTick);
            this.timerHeaRsltAutoSend.Tick += new EventHandler(eTimerTick);
            this.timerSecondBarCodePrt.Tick += new EventHandler(eTimerTick);

            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTextBoxKeyPress);

            this.tabControl1.Click += new EventHandler(eTabClock);

            this.ssChk.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssChk.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.EditModeOn += new EventHandler(eSpreadEditModeOn);
            this.SS2.EditModeOff += new EventHandler(eSpreadEditModeOff);
            this.SS2.Change += new ChangeEventHandler(eSpreadChange);

            this.SS2.KeyDown += new KeyEventHandler(eSpreadKeyDown);
            this.SS2.KeyPress += new KeyPressEventHandler(eSpreadKeyPress);
            this.SS2.KeyUp += new KeyEventHandler(eSpreadKeyUp);
            this.SS2.LeaveCell += new LeaveCellEventHandler(eSpreadLeaveCell);

            //this.ssChk.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssChk.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);

            this.ssJepList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.chkCorrectedVision.Click += new EventHandler(eCheckBoxClick);
            this.chkCorrectedHearing.Click += new EventHandler(eCheckBoxClick);
        }

        private void Spread_Height_Set(FpSpread spdNm, int nHt)
        {
            spdNm.ActiveSheet.Rows[-1].Height = nHt;
        }

        void fn_Screen_Clear()
        {
            if (clsHcVariable.GstrHicPart == "1")   //종검
            {
                //KOSMOS_PMPA.PC_HEA_WAIT_PATIENT_CLEAR(WRTNO, ROOMCODE);
                Dictionary<string, object> dic = heaSangdamWaitService.Sangdam_Wait_Update(long.Parse(txtWrtNo.Text), FstrRoom);

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in dic)
                {
                    stringBuilder.AppendLine(item.Key + " : " + item.Value);
                }
            }

            #region 일검
            txtWrtNo.Text = "";
            FstrPtno = "";
            FnClickRow = 0;
            FnWrtno2 = 0;
            FnHeaWRTNO = 0;
            lblJepsu.Text = "";
            lblJepsu.ForeColor = Color.Green;
            lblJepsu.BackColor = Color.FromArgb(255, 255, 192);
            lblXray.Text = "";
            FbAutoSave = false;
            Fstr청력인터페이스 = "";
            timer1.Enabled = false;
            FnOLD_Height = 0;
            FnOLD_Weight = 0;
            #endregion

            //인적사항
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 3].Text = "";

            sp.Spread_All_Clear(SS2);
            Spread_Height_Set(SS2, 35);
            sp.Spread_All_Clear(ssChk);

            btnSave.Enabled = true;
            SS2.ActiveSheet.RowCount = 40;
            ssChk.ActiveSheet.RowCount = 20;
            timer1.Enabled = false;

            if (clsHcVariable.GstrHicPart == "1")   //종검
            {
                lblSDate.Text = "";
                lblExam.Text = "";
                //menuPFT.Enabled = false;
            }
            else if (clsHcVariable.GstrHicPart == "2")   //일검
            {
                //Xrayno_Set.Enabled = false;
                txtXrayNo.Text = "";
                btnSave.Enabled = true;
                //menu보류대장.Enabled = false;
                //menuPFT.Enabled = false;
                
                lblBpH.Text = "0";
                lblBpL.Text = "0";
            }
        }

        void eTabClock(object sender, EventArgs e)
        {
            if (sender == tabControl1)
            {
                if (tabControl1.SelectedTab == tabJepsu)
                {
                    fn_menuList_Click();
                    txtWrtNo.Focus();
                }
                else if (tabControl1.SelectedTab == tabChk)
                {
                    if (!txtWrtNo.Text.IsNullOrEmpty())
                    {
                        FnWRTNO = long.Parse(txtWrtNo.Text);
                        ACTING_CHECK(FnWRTNO, dtpFDate.Text);
                    }
                    txtWrtNo.Focus();
                }

                txtWrtNo.Focus();
            }
        }

        void fn_menuList_Click()
        {
            int nRead = 0;
            int nRead2 = 0;
            long nWrtNo = 0;
            string strTemp = "";
            string strSDate = "";
            //string strExcode = "";
            string strGbChul = "";            

            int nMiAct = 0;
            string strOK = "";

            strSDate = dtpFDate.Text;
            tabControl1.TabIndex = 0;
            tabControl1.SelectedTab = tabJepsu;

            if (rdoJepsuGubun2.Checked == true)
            {
                strGbChul = "1";
            }
            else if (rdoJepsuGubun3.Checked == true)
            {
                strGbChul = "2";
            }
            else
            {
                strGbChul = "";
            }

            ssJepList.DataSource = hicJepsuHeaExjongService.GetGbStsbyWrtNo(strSDate);
            if (ssJepList.ActiveSheet.RowCount == 0) ssJepList.ActiveSheet.RowCount = 30;

            if (clsHcVariable.GstrHicPart == "1")   //종검
            {
                if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
                {

                    List<HIC_JEPSU_HEA_EXJONG> list = hicJepsuHeaExjongService.GetGbStsbyWrtNo(strSDate);

                    nRead = list.Count;

                    for (int i = 0; i < nRead; i++)
                    {
                        ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();

                        if (!list[i].ACTMEMO.IsNullOrEmpty())
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].BackColor = Color.NavajoWhite;
                        }
                        else
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].BackColor = Color.White;
                        }

                        nWrtNo = list[i].WRTNO;

                        if (nWrtNo > 0)
                        {
                            ssJepList.ActiveSheet.Cells[i, 2].Text = list[i].AMPM2 == "2" ? "오후" : "";

                            //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                            List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo);
                            nRead2 = listActive.Count;

                            if (nRead2 > 0)
                            {
                                for (int j = 0; j < nRead2; j++)
                                {
                                    if (listActive[j].ACTIVE == "N" || listActive[j].ACTIVE.IsNullOrEmpty())
                                    {
                                        strTemp = "OK";
                                    }
                                }

                                if (strTemp == "OK")
                                {
                                    ssJepList.ActiveSheet.Cells[i, 3].Text = "X";
                                }
                                else
                                {
                                    ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                                }
                                strTemp = "";
                            }
                            string[] strExcode = new string[] { "TX20", "TX64", "TX41" };
                            //수면내시경 노란색
                            HIC_RESULT listRslt = hicResultService.Read_ExCode2(nWrtNo, strExcode);

                            if (listRslt != null)
                            {
                                ssJepList.ActiveSheet.Cells[i, 3].BackColor = Color.PaleGoldenrod;
                            }

                            //인바디 전송여부
                            if (list[i].INBODY == "Y")
                            {
                                ssJepList.ActiveSheet.Cells[i, 5].Text = "○";
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[i, 5].Text = "X";
                            }

                            //공단검진 여부
                            if (list[i].GONGDAN == "Y")
                            {
                                ssJepList.ActiveSheet.Cells[i, 6].Text = "○";
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[i, 6].Text = "X";
                            }

                            //바코드 미발행이면 붉은색으로 표시
                            if (list[i].GBEXAM != "Y")
                            {
                                ssJepList.ActiveSheet.Cells[i, 6].BackColor = Color.Tomato;
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[i, 6].BackColor = Color.White;
                            }
                        }

                        ssJepList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;

                        if (list[i].LTDCODE != 0)
                        {
                            ssJepList.ActiveSheet.Cells[i, 4].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        }
                        else
                        {
                            ssJepList.ActiveSheet.Cells[i, 4].Text = "개인종검";
                        }
                    }
                }
                else  //종검스테이션 계측현황 조회
                {
                    List<HIC_JEPSU_HEA_EXJONG> list = hicJepsuHeaExjongService.GetWrtnobyGubun_All_HEA(strSDate);

                    nRead = list.Count;
                    nRow = 0;

                    for (int i = 0; i < nRead; i++)
                    {
                        ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();

                        nWrtNo = list[i].WRTNO;

                        //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                        //nMiAct = hicResultService.GetResultCount(nWrtNo);
                        List<HEA_RESULT> list2 = heaResultService.GetActiveExCodebyWrtNo(nWrtNo);

                        nRead2 = list2.Count;

                        if (nRead2 > 0)
                        {

                        }

                        //nMiAct = 

                        strOK = "OK";
                        if (chkMiAct.Checked == true && nMiAct <= 0)
                        {
                            strOK = "";
                        }

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            if (ssJepList.ActiveSheet.RowCount < nRow)
                            {
                                ssJepList.ActiveSheet.RowCount = nRow;
                            }

                            ssJepList.ActiveSheet.Cells[nRow, 0].Text = nWrtNo.ToString();

                            if (!list[i].ACTMEMO.Trim().IsNullOrEmpty())
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 0].BackColor = Color.White;
                            }

                            ssJepList.ActiveSheet.Cells[nRow, 1].Text = list[i].SNAME.Trim();
                            if (nMiAct == 0)
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = "Ⅹ";
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = "○";
                            }

                            if (list[i].LTDCODE != 0)
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = "개인종검";
                            }
                        }
                    }

                    ssJepList.ActiveSheet.RowCount = nRow;
                    lblWrtNo.Text = "";
                }
            }
            else if (clsHcVariable.GstrHicPart == "2")  //일검
            {
                if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
                {

                    if (!FstrWaitRoom.IsNullOrEmpty())
                    {
                        List<HIC_JEPSU> list = hicJepsuService.GetWrtnobyGubun(FstrWaitRoom);

                        nRead = list.Count;

                        for (int i = 0; i < nRead; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();

                            nWrtNo = list[i].WRTNO;

                            if (nWrtNo > 0)
                            {
                                //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                                List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo);

                                nRead2 = listActive.Count;

                                if (nRead2 > 0)
                                {
                                    for (int j = 0; j < nRead2; j++)
                                    {
                                        if (listActive[j].ACTIVE == "N" || listActive[j].ACTIVE.IsNullOrEmpty())
                                        {
                                            strTemp = "OK";
                                        }
                                    }

                                    if (strTemp == "OK")
                                    {
                                        ssJepList.ActiveSheet.Cells[i, 3].Text = "X";
                                    }
                                    else
                                    {
                                        ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                                    }
                                    strTemp = "";
                                }
                                //종검 마스타에서 종검번호를 찾음
                                nHeaPano = hicPatientService.GetPanobyPtno(list[i].PTNO);

                                if (nHeaPano > 0)
                                {
                                    FnHeaWRTNO = heaJepsuService.GetWrtNobyHeaPaNo(nHeaPano, dtpFDate.Text);

                                    if (FnHeaWRTNO > 0)
                                    {
                                        ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                                    }
                                }
                                ssJepList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                                ssJepList.ActiveSheet.Cells[i, 4].Text = list[i].SEX;
                                ssJepList.ActiveSheet.Cells[i, 5].Text = list[i].NAME;
                            }
                        }
                    }
                    else
                    {
                        List<HIC_JEPSU> list = hicJepsuService.GetWrtnobyJepDate(strJepDate, strGbChul);

                        nRead = list.Count;

                        for (int i = 0; i < nRead; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();

                            nWrtNo = list[i].WRTNO;

                            if (nWrtNo > 0)
                            {
                                //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                                List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo);

                                nRead2 = listActive.Count;

                                if (nRead2 > 0)
                                {
                                    for (int j = 0; j < nRead2; j++)
                                    {
                                        if (listActive[j].ACTIVE == "N" || listActive[j].ACTIVE.IsNullOrEmpty())
                                        {
                                            strTemp = "OK";
                                        }
                                    }

                                    if (strTemp == "OK")
                                    {
                                        ssJepList.ActiveSheet.Cells[i, 3].Text = "X";
                                    }
                                    else
                                    {
                                        ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                                    }
                                    strTemp = "";
                                }
                                //종검 마스타에서 종검번호를 찾음
                                nHeaPano = hicPatientService.GetPanobyPtno(list[i].PTNO);

                                if (nHeaPano > 0)
                                {
                                    FnHeaWRTNO = heaJepsuService.GetWrtNobyHeaPaNo(nHeaPano, dtpFDate.Text);

                                    if (FnHeaWRTNO > 0)
                                    {
                                        ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                                    }
                                }
                                ssJepList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                                ssJepList.ActiveSheet.Cells[i, 4].Text = list[i].SEX;
                                ssJepList.ActiveSheet.Cells[i, 5].Text = list[i].NAME;
                            }
                        }
                    }
                }
                else  //일검 접수 계측현황 조회
                {
                    List<HIC_JEPSU_HEA_EXJONG> list = hicJepsuHeaExjongService.GetWrtnobyGubun_All_HIC(strSDate);

                    nRead = list.Count;
                    nRow = 0;

                    for (int i = 0; i < nRead; i++)
                    {
                        ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.ToString();

                        nWrtNo = list[i].WRTNO;

                        //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                        nMiAct = hicResultService.GetResultCount(nWrtNo);

                        strOK = "OK";
                        if (chkMiAct.Checked == true && nMiAct <= 0)
                        {
                            strOK = "";
                        }

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            if (ssJepList.ActiveSheet.RowCount < nRow)
                            {
                                ssJepList.ActiveSheet.RowCount = nRow;
                            }

                            ssJepList.ActiveSheet.Cells[nRow, 0].Text = nWrtNo.ToString();

                            if (!list[i].ACTMEMO.Trim().IsNullOrEmpty())
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 0].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 0].BackColor = Color.White;
                            }

                            ssJepList.ActiveSheet.Cells[nRow, 1].Text = list[i].SNAME.Trim();
                            if (nMiAct == 0)
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = "Ⅹ";
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = "○";
                            }

                            if (list[i].LTDCODE != 0)
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[nRow, 2].Text = "개인종검";
                            }
                        }
                    }

                    ssJepList.ActiveSheet.RowCount = nRow;
                    lblWrtNo.Text = "";
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                tabControl1.TabIndex = 1;

                if (clsHcVariable.GstrHicPart == "1")   //종합검진
                {
                    fn_Screen_Clear();
                    txtWrtNo.Focus();
                }
                else if (clsHcVariable.GstrHicPart == "2")   //일반검진
                {

                }
            }
            else if (sender == btnSave)
            {
                string strRoomCd = "";

                if (clsHcVariable.GstrHicPart == "1")   //종합검진
                {
                    if (chkInBodySend.Checked == true)
                    {
                        if (InBodyVer == "신버전1")
                        {
                            fn_Work_Inbody_Data_New1();
                        }
                        else if (InBodyVer == "신버전")
                        {
                            fn_Work_Inbody_Data_New();
                        }
                    }

                    //인적사항을 READ
                    HEA_JEPSU HicJepsulist = heaJepsuService.Read_Jepsu2(long.Parse(txtWrtNo.Text));

                    if (!HicJepsulist.RID.IsNullOrEmpty())
                    {
                        strYYYY = HicJepsulist.SDATE.ToString().Substring(0, 4);
                        strSex = HicJepsulist.SEX;
                        strSName = HicJepsulist.SNAME;
                        strGjJong = HicJepsulist.GJJONG;
                        nAge = Convert.ToInt32(HicJepsulist.AGE);
                    }

                    //특수소음 누락 방지
                    strNoise = "";
                    //strActMemo = ssPatInfo.ActiveSheet.Cells[5, 1].Text.Trim();

                    if (VB.InStr(strActMemo, "특수소음") > 0) strNoise = "Y";

                    if (strNoise == "Y")
                    {
                        for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                        {
                            strCode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CODE].Text.Trim();
                            strChange = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text.Trim();
                            if (strChange == "Y")
                            {
                                if (strCode == "TH12" || strCode == "TH15" || strCode == "TH22" || strCode == "TH25")
                                {
                                    if (MessageBox.Show("일반건진 특수소음 검사 대상입니다." + "\r\n" + "특수소음 검사를 하셨습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        break;
                                    }
                                    MessageBox.Show("특수소음을 검사 후 결과를 저장하세요!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }
                    }

                    //자료에 오류가 있는지 점검함
                    for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                    {
                        strCode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CODE].Text.Trim();
                        strResult = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text.Trim();

                        if (!strResult.IsNullOrEmpty())
                        {
                            //혈압은 숫자만 가능함
                            if (strCode == "A108" || strCode == "A109")
                            {
                                if (VB.IsNumeric(strResult) == false)
                                {
                                    MessageBox.Show(i + "번줄 혈압은 숫자만 입력 가능합니다", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }

                    nDataCNT = 0;
                    nResultCNT = 0;

                    for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                    {
                        strCode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CODE].Text.Trim();
                        strResult = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text.Trim();
                        strPanjeng = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.PANJENG].Text.Trim();
                        strResCode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESCODE].Text.Trim();
                        strChange = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text.Trim();
                        strROWID = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.ROWID].Text.Trim();
                        strResType = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESTYPE].Text.Trim();

                        if (strCode == "A103")
                        {
                            strResult = hm.Biman_Gesan(FnWRTNO);
                            strResult = VB.Pstr(strResult, ".", 1);
                        }

                        if (strResult == "본인 제외")
                        {
                            strResult = "본인제외";
                        }

                        //TX90:동맥경화협착검사
                        if (strCode == "TX90" && strChange == "Y")
                        {
                            if (strResult.Trim() == "동맥 경화검사: , 동맥 협착검사:")
                            {
                                strChange = "";
                            }
                        }

                        if (strChange == "Y" || !strResult.IsNullOrEmpty())
                        {
                            //History에 INSERT
                            int result = hicResultHisService.Result_History_Insert(clsPublic.GstrJobSabun, strResult.Replace("'", "`"), strROWID);

                            if (result < 0)
                            {
                                MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            int result1 = hicResultHisService.Result_Update(strResult.Replace("'", "`"), strPanjeng, strResCode, clsPublic.GstrJobSabun);

                            if (result1 < 0)
                            {
                                MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        if (strCode == "E908" || strCode == "E909")
                        {
                            strWomen = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.REFERENCE].Text.Trim();
                            strWomen1 = VB.Pstr(strWomen, "~", 1);
                            strWomen2 = VB.Pstr(strWomen, "~", 2);

                            int result = heaWomenService.Merge_Women_Reference(strWomen1, strWomen2, strWomen1, strWomen2, FnWRTNO, strCode);

                            if (result < 0)
                            {
                                MessageBox.Show("종검 여성정밀 참고치 갱신중 오류가 발생함!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    //청력검사 결과 자동입력
                    hb.Update_Audio_Result(long.Parse(txtWrtNo.Text), strSex);

                    //폐활량검사 결과 자동입력
                    hb.Update_Lung_Capacity(long.Parse(txtWrtNo.Text), strSex);

                    //MDRD-GFR 자동계산 2012년부터
                    hm.MDRD_GFR_Gesan(FnWRTNO, strSex, nAge);

                    //해당접수번호의 결과입력대상건수, 결과입력건수를 READ
                    List<HIC_RESULT> RsltList = hicResultService.Read_Result2(FnWRTNO);

                    nDataCNT = 0;
                    nResultCNT = 0;

                    for (int i = 0; i < RsltList.Count; i++)
                    {
                        nDataCNT += 1;
                        if (!RsltList[i].RESULT.IsNullOrEmpty())
                        {
                            nResultCNT += 1;
                            if (RsltList[i].EXCODE == "TX20" && !RsltList[i].RESULT.IsNullOrEmpty()) { strEndoSo = RsltList[i].RESULT; }
                            if (RsltList[i].EXCODE == "TX22" && !RsltList[i].RESULT.IsNullOrEmpty()) { strEndoSo = RsltList[i].RESULT; }
                            if (RsltList[i].EXCODE == "TX23" && !RsltList[i].RESULT.IsNullOrEmpty()) { strEndoSo = RsltList[i].RESULT; }
                        }
                    }

                    //접수마스타에 상태를 UPDATE
                    strGbSTS = "1";  //수진자등록
                    if (nResultCNT == nDataCNT || (nResultCNT == nDataCNT - 1 && strEndoSo.IsNullOrEmpty()))
                    {
                        strGbSTS = "3"; //입력완료
                    }

                    if (nResultCNT > 0 && nResultCNT < nDataCNT - 1)
                    {
                        strGbSTS = "2"; //입력중
                    }

                    int result2 = heaJepsuService.Update_Hea_Jepsu_GbSts(strGbSTS, strActMemo, FnWRTNO);

                    if (result2 < 0)
                    {
                        MessageBox.Show("접수마스타에 입력상태 변경중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //다른검사실에 등록되어 있는지 확인
                    if (!FstrRoom2.IsNullOrEmpty())
                    {
                        strRoomCd = FstrRoom2;
                    }
                    else
                    {
                        strRoomCd = FstrRoom;
                    }

                    List<HEA_SANGDAM_WAIT> HicSDlist = heaSangdamWaitService.Etc_ExamRoom_RegConfirm(long.Parse(txtWrtNo.Text), strRoomCd);
                    if (HicSDlist.Count > 0)
                    {
                        //기존의 자료가 있으면 삭제함
                        int result = heaSangdamWaitService.Delete_Sangdam_Wait(long.Parse(txtWrtNo.Text), strRoomCd);

                        if (result < 0)
                        {
                            MessageBox.Show("타 검사실 대기순번 삭제시 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    //상담대기순번 완료
                    nREAD = 0;
                    nCNT = 0;
                    List<HIC_RESULT> ActDList = hicResultService.Read_Result_Acitve(long.Parse(txtWrtNo.Text), FstrPartG);

                    if (ActDList.Count > 0)
                    {
                        for (int i = 0; i < ActDList.Count; i++)
                        {
                            if (ActDList[i].ACTIVE == "Y")
                            {
                                nCNT += 1;
                            }
                        }
                    }

                    string[] strJongSQL;

                    strJongSQL = new string[] { FstrRoom };

                    List<HEA_SANGDAM_WAIT> GbCallList = heaSangdamWaitService.Read_Sangdam_GbCall(long.Parse(txtWrtNo.Text), FstrRoom);

                    if (GbCallList.Count > 0)
                    {
                        if (nCNT == nREAD)
                        {
                            int result1 = heaSangdamWaitService.Update_Sangdam_GbCall(FnWRTNO, strJongSQL);

                            if (result1 < 0)
                            {
                                return;
                            }
                        }
                        else
                        {
                            int result3 = heaSangdamWaitService.Update_Sangdam_CallTime(FnWRTNO, FstrRoom);

                            if (result3 < 0)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (nCNT == nREAD)
                        {
                            HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                            item.WRTNO = FnWRTNO;
                            item.SNAME = strSName;
                            item.SEX = strSex;
                            item.AGE = nAge;
                            item.GJJONG = strGjJong;
                            item.GBCALL = "Y";
                            item.GUBUN = FstrRoom;
                            item.WAITNO = 0;

                            int result3 = heaSangdamWaitService.Insert_Sangdam_Wait(item);

                            if (result3 < 0)
                            {
                                MessageBox.Show("상담대기 순번등록 중 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            if (FstrRoom2.IsNullOrEmpty())
                            {
                                HEA_SANGDAM_WAIT WaitNoList = heaSangdamWaitService.Read_Sangdam_WaitNo(FstrRoom);

                                HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                                item.WRTNO = FnWRTNO;
                                item.SNAME = strSName;
                                item.SEX = strSex;
                                item.AGE = nAge;
                                item.GJJONG = strGjJong;
                                item.GBCALL = "";
                                item.GUBUN = FstrRoom;
                                item.WAITNO = Convert.ToInt32(WaitNoList.WAITNO);

                                int result4 = heaSangdamWaitService.Insert_Sangdam_Wait(item);

                                if (result4 < 0)
                                {
                                    MessageBox.Show("상담대기 순번등록 중 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                else if (clsHcVariable.GstrHicPart == "2")   //일반검진
                {
                    if (Police_Result_Check() == false)
                    {
                        MessageBox.Show("경찰공무원 채용검진의 청력검사 결과를 수치로 입력해주세요!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //자료에 오류가 있는지 점검함
                    strDispOk = "";
                    for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                    {
                        strCode = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                        strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (!strResult.IsNullOrEmpty())    //혈압은 숫자만 가능함
                        {
                            if (strCode == "A108" || strCode == "A109")
                            {
                                if (VB.IsNumeric(strResult) == false)
                                {
                                    MessageBox.Show(i + 1 + "번줄 혈압은 숫자만 가능합니다!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                else if (strCode == "A111" || strCode == "A112")    //요단백
                                {
                                    strResCode = VB.Pstr(SS2.ActiveSheet.Cells[i, 2].Text.Trim(), ".", 1);
                                    switch (strResult)
                                    {
                                        case "01":
                                        case "02":
                                        case "03":
                                        case "04":
                                        case "05":
                                        case "06":
                                        case "99":
                                            break;
                                        default:
                                            MessageBox.Show("뇨당,뇨단백 결과는 01~06,99만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                    }
                                }
                            }

                            //키,몸무게 종전과 5이상 차이가 발생하면 메세지 표시
                            if (strCode == "A101" && FnOLD_Height != 0)
                            {
                                nCha = FnOLD_Height - double.Parse(strResult);
                                if (nCha >= 5 || nCha <= -5)
                                {
                                    strDispOk = "OK";
                                }                                
                            }
                            else if (strCode == "A102" && FnOLD_Weight != 0)
                            {
                                nCha = FnOLD_Weight - long.Parse(strResult);
                                if (nCha >= 5 || nCha <= -5)
                                {
                                    strDispOk = "OK";
                                }
                            }

                            //노인신체기능검사
                            if (strCode == "A118") strNOIN1 = strResult;
                            if (strCode == "A119") strNOIN2 = strResult;
                            if (strCode == "A120") strNOIN3 = strResult;
                        }
                    }

                    //노인신체기능검사
                    if (!strNOIN1.IsNullOrEmpty())
                    {
                        if (string.Compare(strNOIN1, "21") >= 0)
                        {
                            if (VB.Left(strNOIN2, 2) == "01")
                            {
                                MessageBox.Show("보행장애 검사결과를 확인해주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    //키,몸무게 종전과 5이상 차이가 발생하면 메세지 표시
                    if (strDispOk == "OK")
                    {
                        strMsg = "키 또는 몸무게 종전 결과값과 5이상 차이가 발생함.";
                        strMsg += "저장을 하시겠습니까?";
                        if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    //BP 전송
                    if (chkBP.Checked == true)
                    {
                        fn_Work_BP_Data();
                    }

                    //비만도 계산 및 Update
                    if (FstrGjJong == "56")
                    {
                        hm.Biman_Gesan(FnWRTNO);
                    }
                    else
                    {
                        hm.Biman_Gesan(FnWRTNO);    //체질량 자동계산 A117
                    }

                    hm.Update_Audiometry(FnWRTNO);  //기도청력 시 기본청력 정상입력

                    //hm.GFR_Gesan_Life(FnWRTNO, FstrSex, FnAge); //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                    hm.MDRD_GFR_Gesan(FnWRTNO, strSex, nAge);  //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                    hm.AIR3_AUTO(FnWRTNO);                      //AIR 3분법 자동계산
                    hm.AIR6_AUTO(FnWRTNO);                      //AIR 6분법 자동계산
                    hm.LDL_Gesan(FnWRTNO);                      //LDL콜레스테롤 계산
                    hm.TIBC_Gesan(FnWRTNO);                     //TIBC총철결합능 계산

                    //접수마스타의 상태를 변경
                    hm.Result_EntryEnd_Check(long.Parse(txtWrtNo.Text));

                    //62종 혈액종검 체혈액팅
                    hm.Update_Blood_Acting(FnPano, FnWRTNO, dtpFDate.Text);
                    //다음방 자동 지정
                    if (FstrWaitRoom == "10" || FstrWaitRoom == "14")
                    {
                        hcact.WAIT_NextRoom_SET(FnWrtno2, FnPano, FstrWaitRoom);
                    }
                    timer1.Enabled = false;
                }

                fn_Screen_Clear();
                fn_Screen_Display();

                txtWrtNo.Focus();
            }
            else if (sender == btnSang)
            {
                if (string.IsNullOrEmpty(txtWrtNo.Text.Trim()))
                {
                    MessageBox.Show("접수번호가 공란입니다!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                frmWaitSeqReg f = new frmWaitSeqReg(FnWRTNO, FnPano);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
                f.Dispose();
            }
        }

        void fn_Work_BP_Data()
        {
            string strMsg = "";
            string strSYS = "";
            string strDIA = "";
            string strMEAN = "";
            string strPUL = "";
            string strPULPRE = "";

            long lngHandle = 0;
            long chlHandle = 0;
            long chlHandle2 = 0;
            long hCmd = 0;

            string[] strExamList = new string[] { "A108", "A231" };
            string[] strExamList1 = new string[] { "A109", "A232" };

            lngHandle = FindWindow("TMain", "Member's information and Results.");

            if (lngHandle > 0)
            {
                //13번째 핸들위치 최고혈압
                chlHandle2 = FindWindowEx(lngHandle, 0, "TPanel", null);
                for (int i = 1; i < 12; i++)
                {
                    chlHandle2 = FindWindowEx(lngHandle, chlHandle2, "TPanel", null);
                }

                strSYS = GetText(chlHandle2).Trim();

                //11번째 핸들위치 최고혈압
                chlHandle2 = FindWindowEx(lngHandle, 0, "TPanel", null);
                for (int i = 1; i < 12; i++)
                {
                    chlHandle2 = FindWindowEx(lngHandle, chlHandle2, "TPanel", null);
                }
                strDIA = GetText(chlHandle2).Trim();
            }            

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "A108" || SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "A231") //혈압최고
                {
                    if (!strSYS.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = !SS2.ActiveSheet.Cells[i, 2].Text.Trim().IsNullOrEmpty() ? SS2.ActiveSheet.Cells[i, 0].Text.Trim() : strSYS;
                    }
                }
                else if (SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "A108" || SS2.ActiveSheet.Cells[i, 0].Text.Trim() == "A231") //혈압최저
                {
                    if (!strDIA.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = !SS2.ActiveSheet.Cells[i, 2].Text.Trim().IsNullOrEmpty() ? SS2.ActiveSheet.Cells[i, 0].Text.Trim() : strDIA;
                    }
                }
            }

            if (strSYS.IsNullOrEmpty() || VB.Val(strSYS) == 0)
            {
                strMsg += "최고혈압,";
            }

            if (strDIA.IsNullOrEmpty() || VB.Val(strDIA) == 0)
            {
                strMsg += "최저혈압";
            }

            if (!strMsg.IsNullOrEmpty())
            {
                MessageBox.Show(strMsg + " 누락. BP 측정이 제대로 되었는지 확인바랍니다.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //최고혈압 업데이트
            if (!strSYS.IsNullOrEmpty())
            {
                int result = hicResultService.Update_Hic_Result(strSYS, long.Parse(clsType.User.IdNumber), FnWRTNO, strExamList);

                if (result < 0)
                {
                    MessageBox.Show("최고혈압 update 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //최저혈압 업데이트
            if (!strDIA.IsNullOrEmpty())
            {
                int result1 = hicResultService.Update_Hic_Result(strDIA, long.Parse(clsType.User.IdNumber), FnWRTNO, strExamList1);

                if (result1 < 0)
                {
                    MessageBox.Show("최저혈압 update 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// 경찰공무원 채용검진 청력결과 정상 여부(true=정상, false=결과값오류)
        /// </summary>
        /// <returns></returns>
        bool Police_Result_Check()
        {
            bool rtnVal = false;
            string strCode = "";
            string strResult = "";
            long nWrtNo = 0;
            int nRowCount = 0;

            if (txtWrtNo.Text.Trim().IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 공란입니다!", "필수항목 누락", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            nWrtNo = long.Parse(txtWrtNo.Text);

            HIC_SUNAPDTL listSunapDtl = hicSunapdtlService.GetSunapDtlbyWrtNo(nWrtNo, "2116");

            if (listSunapDtl == null)
            {
                rtnVal = true;
                return rtnVal;
            }

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                if (!strResult.IsNullOrEmpty())
                {
                    switch (strCODE)
                    {
                        case "A106":
                        case "A107":
                        case "TH12":
                        case "TH22":
                            rtnVal = true;
                            break;
                        default:
                            rtnVal = false;
                            break;
                    }

                    if (rtnVal == true)
                    {
                        if (strResult == "." || strResult == "정상" || strResult == "비정상")
                        {
                            rtnVal = false;
                            return rtnVal;
                        }
                        else if (VB.IsNumeric(strResult) == false)
                        {
                            rtnVal = false;
                            return rtnVal;
                        }
                    }
                }
            }
            return rtnVal;
        }

        void fn_Work_Inbody_Data_New()
        {
            ///TODO : 이상훈 (2019.07.18) INBODY 관련 보류
            //string strHeight = "";
            //string strWeight = "";
            //string strAbdomen = "";
            //string strDateTime = "";
            //string strMsg = "";
            //string strMsg2 = "";
            //string strPATH = "";
            //string SS = "";
            //string strTemp = "";
            //string strNewDateTime = "";
            //string strLOCALID = "";

            //strPATH = @"C:\LookinBody120\Database\LookinBODY.MDB";

            ////실측정값
            //strHeight = "";
            //strWeight = "";
            //strAbdomen = "";
            //strDateTime = "";
            //strNewDateTime = "";

            ///TODO : 이상훈 (2019.07.18) 공통모듈 작업 후 재작업 필요
            //MDBadoConnect_BioSpace(strPATH)                    '본체 MDB 접속

            //if (GstrMDBAdoMsg == "NO")
            //{
            //    MessageBox.Show("InBody 자료를 불러올 수 없습니다.", "InBody 연계불가!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    return;
            //}

            //종합결과값
            //SQL = " SELECT USER_INFO1_TBL.LOCAL_ID, USER_INFO1_TBL.HEIGHT "
            //SQL = SQL & " FROM USER_INFO1_TBL"
            //SQL = SQL & " WHERE USER_ID = '" & FstrPtno & "'"
            //SQL = SQL & " ORDER BY LOCAL_ID DESC "
            //Call MDBAdoOpenSet(AdoRes, SQL, , , False)
            //If MDBRowindicator > 0 Then
            //    strLOCALID = Trim(AdoRes!LOCAL_ID & "")
            //    strHeight = Trim(AdoRes!Height & "") '키
            //End If
            //'Call MDBAdoCloseSet(AdoRes)



            //SQL = " SELECT BCA_TBL.DATETIMES, BCA_TBL.WT "
            //SQL = SQL & " FROM BCA_TBL"
            //SQL = SQL & " WHERE LOCAL_ID = " & strLOCALID & " "
            //SQL = SQL & " ORDER BY DATETIMES DESC "
            //Call MDBAdoOpenSet(AdoRes, SQL, , , False)
            //If MDBRowindicator > 0 Then
            //    strWeight = Trim(AdoRes!WT & "") '몸무게
            //    strDateTime = Trim(AdoRes!DATETIMES & "") '시간(키값)
            //End If
            //'Call MDBAdoCloseSet(AdoRes)



            //SQL = "SELECT ED_TBL.DATETIMES, ED_TBL.ABD"
            //SQL = SQL & " FROM ED_TBL"
            //SQL = SQL & " WHERE ED_TBL.DATETIMES='" & strDateTime & "' "
            //SQL = SQL & " ORDER BY DATETIMES DESC "
            //Call MDBAdoOpenSet(AdoRes, SQL, , , False)
            //If MDBRowindicator > 0 Then
            //    strAbdomen = Trim(AdoRes!ABD & "") '허리둘레
            //End If
            //'Call MDBAdoCloseSet(AdoRes)


            //For i = 1 To SS2.MaxRows
            //    SS2.Row = i
            //    SS2.Col = 1
            //    If Trim(SS2.Text) = "A101" Then         '신장
            //        If strHeight<> "" Then
            //            SS2.Col = 3: SS2.Text = IIf(Trim(SS2.Text) <> "", Trim(SS2.Text), strHeight)
            //        End If
            //    ElseIf Trim(SS2.Text) = "A102" Then     '몸무게
            //        If strWeight<> "" Then
            //            SS2.Col = 3: SS2.Text = IIf(Trim(SS2.Text) <> "", Trim(SS2.Text), strWeight)
            //        End If
            //    ElseIf Trim(SS2.Text) = "A115" Then     '허리둘레
            //        If strAbdomen<> "" Then
            //            SS2.Col = 3: SS2.Text = IIf(Trim(SS2.Text) <> "", Trim(SS2.Text), strAbdomen)
            //        End If
            //    End If
            //Next i


            //If strHeight = "" Or Val(strHeight) = 0 Then
            //    strMsg = strMsg & "신장값,"
            //End If


            //If strWeight = "" Or Val(strWeight) = 0 Then
            //    strMsg = strMsg & "체중값,"
            //End If


            //If strMsg<> "" Then
            //   MsgBox strMsg & " 누락. 인바디 측정이 제대로 되었는지 확인바랍니다.", vbCritical, "확인요망"
            //End If


            //'임시로 메세지 처리함
            //If strMsg2<> "" Then
            //   MsgBox strMsg2, vbCritical, "확인요망"
            //End If



            //adoConnect.BeginTrans


            //'신장 업데이트
            //If strHeight<> "" Then
            //   SQL = " UPDATE HEA_RESULT SET RESULT ='" & strHeight & "' "
            //    SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
            //    SQL = SQL & "   AND EXCODE ='A101' "
            //    SQL = SQL & "   AND (RESULT ='' OR RESULT IS NULL) "
            //    result = AdoExecute(SQL)
            //    If result<> 0 Then
            //       MsgBox "DB Error!!", vbInformation, "확인"
            //        adoConnect.RollbackTrans
            //        Exit Sub
            //    End If
            //End If


            //'몸무게 업데이트
            //If strWeight<> "" Then
            //   SQL = " UPDATE HEA_RESULT SET RESULT ='" & strWeight & "' "
            //    SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
            //    SQL = SQL & "   AND EXCODE ='A102' "
            //    SQL = SQL & "   AND (RESULT ='' OR RESULT IS NULL) "
            //    result = AdoExecute(SQL)
            //    If result<> 0 Then
            //       MsgBox "DB Error!!", vbInformation, "확인"
            //        adoConnect.RollbackTrans
            //        Exit Sub
            //    End If
            //End If


            //'허리둘레 업데이트
            //If strAbdomen<> "" Then
            //   SQL = " UPDATE HEA_RESULT SET RESULT ='" & strAbdomen & "' "
            //    SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
            //    SQL = SQL & "   AND EXCODE ='A115' "
            //    SQL = SQL & "   AND (RESULT ='' OR RESULT IS NULL) "
            //    result = AdoExecute(SQL)
            //    If result<> 0 Then
            //       MsgBox "DB Error!!", vbInformation, "확인"
            //        adoConnect.RollbackTrans
            //        Exit Sub
            //    End If
            //End If


            //'InBody 전송여부 업데이트
            //SQL = " UPDATE HEA_JEPSU SET INBODY ='Y' "
            //SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
            //result = AdoExecute(SQL)
            //If result<> 0 Then
            //   MsgBox "DB Error!!", vbInformation, "확인"
            //    adoConnect.RollbackTrans
            //    Exit Sub
            //End If

            //If FnHcWRTNO > 0 Then
            //    '신장 업데이트
            //    SQL = " UPDATE HIC_RESULT SET RESULT ='" & strHeight & "' "
            //    SQL = SQL & " WHERE WRTNO = " & FnHcWRTNO & " "
            //    SQL = SQL & "   AND EXCODE ='A101' "
            //    result = AdoExecute(SQL)
            //    If result<> 0 Then
            //       MsgBox "DB Error!!", vbInformation, "확인"
            //        adoConnect.RollbackTrans
            //        Exit Sub
            //    End If


            //    '몸무게 업데이트
            //    SQL = " UPDATE HIC_RESULT SET RESULT ='" & strWeight & "' "
            //    SQL = SQL & " WHERE WRTNO = " & FnHcWRTNO & " "
            //    SQL = SQL & "   AND EXCODE ='A102' "
            //    result = AdoExecute(SQL)
            //    If result<> 0 Then
            //       MsgBox "DB Error!!", vbInformation, "확인"
            //        adoConnect.RollbackTrans
            //        Exit Sub
            //    End If

            //    '허리둘레 업데이트
            //    SQL = " UPDATE HIC_RESULT SET RESULT ='" & strAbdomen & "' "
            //    SQL = SQL & " WHERE WRTNO = " & FnHcWRTNO & " "
            //    SQL = SQL & "   AND EXCODE ='A115' "
            //    result = AdoExecute(SQL)
            //    If result<> 0 Then
            //       MsgBox "DB Error!!", vbInformation, "확인"
            //        adoConnect.RollbackTrans
            //        Exit Sub
            //    End If
            //End If

            //adoConnect.CommitTrans
        }

        void fn_Work_Inbody_Data_New1()
        {
            ///TODO : 이상훈 (2019.07.18) INBODY 관련 보류
        //    Dim i%
        //    Dim strHeight         As String
        //    Dim strWeight         As String
        //    Dim strAbdomen        As String
        //    Dim strDateTime       As String
        //    Dim strMsg            As String
        //    Dim strMsg2           As String
        //    Dim strPATH$, SS$, strTemp$, strNewDateTime$
        //    Dim strLOCALID        As String


        //    strMsg = ""
        //    strMsg2 = ""
        //    'strPATH = "C:\LookinBody120\Database\LookinBody.mdb"
        //    strPATH = "Z:\Database\LookinBody.MDB"
        //    'strPATH = "C:\LookinBODY.MDB"


        //    '실측정값
        //    strHeight = ""
        //    strWeight = ""
        //    strAbdomen = ""
        //    strDateTime = ""
        //    strNewDateTime = ""


        //    Call MDBadoConnect_BioSpace(strPATH)                    '본체 MDB 접속
        //    'Call MDBadoConnect_BioSpace2("Z:\masterDB.MDB")         '상담실 MDB 접속

        //    If GstrMDBAdoMsg = "NO" Then
        //        MsgBox "InBody 자료를 불러올 수 없습니다.", vbCritical, "InBody 연계불가!!"
        //        Exit Sub
        //    End If




        //'종합결과값
        //    SQL = " SELECT USER_INFO1_TBL.LOCAL_ID, USER_INFO1_TBL.HEIGHT "
        //    SQL = SQL & " FROM USER_INFO1_TBL"
        //    SQL = SQL & " WHERE USER_ID = '" & FstrPtno & "'"
        //    SQL = SQL & " ORDER BY LOCAL_ID DESC "
        //    Call MDBAdoOpenSet(AdoRes, SQL, , , False)
        //    If MDBRowindicator > 0 Then
        //        strLOCALID = Trim(AdoRes!LOCAL_ID & "")
        //        strHeight = Trim(AdoRes!Height & "") '키
        //    End If
        //    'Call MDBAdoCloseSet(AdoRes)



        //    SQL = " SELECT BCA_TBL.DATETIMES, BCA_TBL.WT "
        //    SQL = SQL & " FROM BCA_TBL"
        //    SQL = SQL & " WHERE LOCAL_ID = " & strLOCALID & " "
        //    SQL = SQL & " ORDER BY LOCAL_ID DESC "
        //    Call MDBAdoOpenSet(AdoRes, SQL, , , False)
        //    If MDBRowindicator > 0 Then
        //        strWeight = Trim(AdoRes!WT & "") '몸무게
        //        strDateTime = Trim(AdoRes!DATETIMES & "") '시간(키값)
        //    End If
        //    'Call MDBAdoCloseSet(AdoRes)



        //    SQL = "SELECT ED_TBL.DATETIMES, ED_TBL.ABD"
        //    SQL = SQL & " FROM ED_TBL"
        //    SQL = SQL & " WHERE ED_TBL.DATETIMES='" & strDateTime & "' "
        //    SQL = SQL & " ORDER BY DATETIMES DESC "
        //    Call MDBAdoOpenSet(AdoRes, SQL, , , False)
        //    If MDBRowindicator > 0 Then
        //        strAbdomen = Trim(AdoRes!ABD & "") '허리둘레
        //    End If
        //    'Call MDBAdoCloseSet(AdoRes)


        //    For i = 1 To SS2.MaxRows
        //        SS2.Row = i
        //        SS2.Col = 1
        //        If Trim(SS2.Text) = "A101" Then         '신장
        //            If strHeight<> "" Then
        //                SS2.Col = 3: SS2.Text = IIf(Trim(SS2.Text) <> "", Trim(SS2.Text), strHeight)
        //            End If
        //        ElseIf Trim(SS2.Text) = "A102" Then     '몸무게
        //            If strWeight<> "" Then
        //                SS2.Col = 3: SS2.Text = IIf(Trim(SS2.Text) <> "", Trim(SS2.Text), strWeight)
        //            End If
        //        ElseIf Trim(SS2.Text) = "A115" Then     '허리둘레
        //            If strAbdomen<> "" Then
        //                SS2.Col = 3: SS2.Text = IIf(Trim(SS2.Text) <> "", Trim(SS2.Text), strAbdomen)
        //            End If
        //        End If
        //    Next i


        //    If strHeight = "" Or Val(strHeight) = 0 Then
        //        strMsg = strMsg & "신장값,"
        //    End If


        //    If strWeight = "" Or Val(strWeight) = 0 Then
        //        strMsg = strMsg & "체중값,"
        //    End If


        //    If strMsg<> "" Then
        //       MsgBox strMsg & " 누락. 인바디 측정이 제대로 되었는지 확인바랍니다.", vbCritical, "확인요망"
        //    End If


        //    '임시로 메세지 처리함
        //    If strMsg2<> "" Then
        //       MsgBox strMsg2, vbCritical, "확인요망"
        //    End If



        //    adoConnect.BeginTrans


        //    '신장 업데이트
        //    If strHeight<> "" Then
        //       SQL = " UPDATE HEA_RESULT SET RESULT ='" & strHeight & "' "
        //        SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
        //        SQL = SQL & "   AND EXCODE ='A101' "
        //        SQL = SQL & "   AND (RESULT ='' OR RESULT IS NULL) "
        //        result = AdoExecute(SQL)
        //        If result<> 0 Then
        //           MsgBox "DB Error!!", vbInformation, "확인"
        //            adoConnect.RollbackTrans
        //            Exit Sub
        //        End If
        //    End If


        //    '몸무게 업데이트
        //    If strWeight<> "" Then
        //       SQL = " UPDATE HEA_RESULT SET RESULT ='" & strWeight & "' "
        //        SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
        //        SQL = SQL & "   AND EXCODE ='A102' "
        //        SQL = SQL & "   AND (RESULT ='' OR RESULT IS NULL) "
        //        result = AdoExecute(SQL)
        //        If result<> 0 Then
        //           MsgBox "DB Error!!", vbInformation, "확인"
        //            adoConnect.RollbackTrans
        //            Exit Sub
        //        End If
        //    End If


        //    '허리둘레 업데이트
        //    If strAbdomen<> "" Then
        //       SQL = " UPDATE HEA_RESULT SET RESULT ='" & strAbdomen & "' "
        //        SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
        //        SQL = SQL & "   AND EXCODE ='A115' "
        //        SQL = SQL & "   AND (RESULT ='' OR RESULT IS NULL) "
        //        result = AdoExecute(SQL)
        //        If result<> 0 Then
        //           MsgBox "DB Error!!", vbInformation, "확인"
        //            adoConnect.RollbackTrans
        //            Exit Sub
        //        End If
        //    End If


        //    'InBody 전송여부 업데이트
        //    SQL = " UPDATE HEA_JEPSU SET INBODY ='Y' "
        //    SQL = SQL & " WHERE WRTNO = " & FnWRTNO & " "
        //    result = AdoExecute(SQL)
        //    If result<> 0 Then
        //       MsgBox "DB Error!!", vbInformation, "확인"
        //        adoConnect.RollbackTrans
        //        Exit Sub
        //    End If

        //    If FnHcWRTNO > 0 Then
        //        '신장 업데이트
        //        SQL = " UPDATE HIC_RESULT SET RESULT ='" & strHeight & "' "
        //        SQL = SQL & " WHERE WRTNO = " & FnHcWRTNO & " "
        //        SQL = SQL & "   AND EXCODE ='A101' "
        //        result = AdoExecute(SQL)
        //        If result<> 0 Then
        //           MsgBox "DB Error!!", vbInformation, "확인"
        //            adoConnect.RollbackTrans
        //            Exit Sub
        //        End If


        //        '몸무게 업데이트
        //        SQL = " UPDATE HIC_RESULT SET RESULT ='" & strWeight & "' "
        //        SQL = SQL & " WHERE WRTNO = " & FnHcWRTNO & " "
        //        SQL = SQL & "   AND EXCODE ='A102' "
        //        result = AdoExecute(SQL)
        //        If result<> 0 Then
        //           MsgBox "DB Error!!", vbInformation, "확인"
        //            adoConnect.RollbackTrans
        //            Exit Sub
        //        End If


        //        '허리둘레 업데이트
        //        SQL = " UPDATE HIC_RESULT SET RESULT ='" & strAbdomen & "' "
        //        SQL = SQL & " WHERE WRTNO = " & FnHcWRTNO & " "
        //        SQL = SQL & "   AND EXCODE ='A115' "
        //        result = AdoExecute(SQL)
        //        If result<> 0 Then
        //           MsgBox "DB Error!!", vbInformation, "확인"
        //            adoConnect.RollbackTrans
        //            Exit Sub
        //        End If
        //    End If


        //    adoConnect.CommitTrans
        }

        void eTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                string strPtno = "";
                string strTemp = "";
                string strSDate = "";

                if (txtWrtNo.Text.Trim().IsNullOrEmpty()) return;

                if (e.KeyChar == (char)13)
                {
                    strTemp = txtWrtNo.Text.Trim();
                    fn_Screen_Clear();
                    txtWrtNo.Text = strTemp;

                    if (txtWrtNo.Text.Length > 6)
                    {
                        //외래번호로 접수번호 찾기
                        HEA_JEPSU list = heaJepsuService.Get_WrtNo(strPtno, dtpFDate.Text);

                        if (list != null)
                        {
                            txtWrtNo.Text = list.WRTNO.ToString();
                        }
                        else
                        {
                            MessageBox.Show(txtWrtNo.Text.Trim() + " 는 접수된 번호가 아닙니다. 외래번호(OPD) 확인요망!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            eBtnClick(btnCancel, new EventArgs());
                            txtWrtNo.Text = "";
                            txtWrtNo.Focus();
                            return;
                        }
                    }

                    sp.Spread_All_Clear(ssChk);
                    ssChk.ActiveSheet.RowCount = 20;
                    fn_Screen_Display();

                    //폐활량검사 액팅방이고 액팅이 안된경우(일반검진이 있는 경우에만)
                    if (!clsHcVariable.GstrPFTSN.IsNullOrEmpty() && FnHcWRTNO > 0)
                    {
                        string ResList = hicResSpecialService.Read_Res_Special(FnHcWRTNO);

                        if (!ResList.IsNullOrEmpty())
                        {
                            HIC_RESULT RsltList = hicResultService.Read_Result3(FnHcWRTNO);

                            if (RsltList != null)
                            {
                                //menuPFT.Enabled = true;
                                if (RsltList.RESULT.Trim().IsNullOrEmpty())
                                {
                                    //menuPFT_Click();
                                }
                            }
                        }
                    }

                    if (chkInBodySend.Checked == true)
                    {
                        if (InBodyVer == "신버전1")
                        {
                            Work_InBody_Interface_NEW1();
                            Work_InBody_Control_NEW1();
                        }
                        else if (InBodyVer == "신버전")
                        {
                            Work_InBody_Interface_NEW();
                            Work_InBody_Control_NEW();
                        }
                    }

                    txtWrtNo.SelectAll();
                    SS2.ActiveSheet.SetActiveCell(0, (int)clsHcType.Instrument_Result.RESULT);
                }
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (sender == this.ssChk)
            {
                if (ssChk.ActiveSheet.NonEmptyRowCount == 0) return;

                if (e.Row < 0 || e.ColumnHeader == true) return;

                strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                {
                    ssChk.ActiveSheet.Cells[i, 5].Text = "";
                }

                //해당 검사실 대기명단
                List<HEA_SANGDAM_WAIT> Wait_List = heaSangdamWaitService.Read_Sangdam_Wait_List(strPart, "");

                for (int i = 0; i < Wait_List.Count; i++)
                {
                    ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List[i].SNAME;
                    ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List[i].WRTNO.ToString();
                }
            }
            else if (sender == this.SS2)
            {
                string strResCode = "";

                if (e.Column != 3) return;

                if (SS2.ActiveSheet.NonEmptyRowCount == 0) return;

                strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                if (strResCode.IsNullOrEmpty())
                {
                    FnClickRow = 0;
                    return;
                }
            }
        }

        void eSpreadEditModeOn(object sender, EventArgs e)
        {   
            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
            int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;
            string strResCode = "";

            if (SS2.ActiveSheet.NonEmptyRowCount == 0) return;

            if (nCol != 2) return;

            strResCode = SS2.ActiveSheet.Cells[nRow, 7].Text.Trim();
            if (strResCode.IsNullOrEmpty())
            {
                FnClickRow = -1;
                return;
            }
        }

        void eSpreadEditModeOff(object sender, EventArgs e)
        {

        }

        void eSpreadKeyDown(object sender, KeyEventArgs e)
        {
            string strCode = "";
            string strResult = "";
            string strResCode = "";
            string strResType = "";
            string strOK = "";

            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
            int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;

            if (SS2.ActiveSheet.NonEmptyRowCount == 0) return;

            if (nRow < 0 || nRow > SS2.ActiveSheet.RowCount)
            {
                return;
            }

            strOK = "";
            FnRowNo = nRow;

            switch (strCode)
            {
                case "A101":
                case "A102":
                case "A108":
                case "A109":
                case "A114":
                case "A115":
                case "ZD04":
                    break;
                default:
                    strOK = "Y";
                    break;
            }

            if (strOK == "Y")
            {
                strResult = "";
                lblGuide.Text = "F2: 측정불가 F3: 본인제외 F4: 정상 F5: 비정상 F6: 교정 F7: 색각이상 F8: 9.9 F9: 미실시";

                switch (e.KeyCode)
                {
                    case Keys.F2:
                        strResult = "측정불가";
                        break;
                    case Keys.F3:
                        strResult = "본인제외";
                        break;
                    case Keys.F4:
                        strResult = "정상";
                        break;
                    case Keys.F5:
                        strResult = "비정상";
                        break;
                    case Keys.F6:
                        strResult = "교정";
                        break;
                    case Keys.F7:
                        strResult = "색각이상";
                        break;
                    case Keys.F8:
                        strResult = "9.9";
                        break;
                    case Keys.F9:
                        strResult = "미실시";
                        break;
                    default:
                        break;
                }
            }

            strResCode = SS2.ActiveSheet.Cells[nRow, 7].Text.Trim();
            SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";
            strResType = SS2.ActiveSheet.Cells[nRow, 10].Text.Trim();

            if (!strResult.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[nRow, 2].Text = FnRowNo.ToString();
                SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";
                FnRowNo += 1;
                if (FnRowNo > SS2.ActiveSheet.RowCount)
                {
                    FnRowNo = SS2.ActiveSheet.RowCount;
                }
                SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
            }
        }

        void eSpreadKeyPress(object sender, KeyPressEventArgs e)
        {
            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
            int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;

            if (e.KeyChar != 13)
            {
                return;
            }

            if (nRow == SS2.ActiveSheet.RowCount)
            {
               if (MessageBox.Show("결과값을 저장하시겠습니까?", "확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    eBtnClick(btnSave, new EventArgs());
                }
            }
        }

        void eSpreadKeyUp(object sender, KeyEventArgs e)
        {
            string strCode = "";
            bool bKey1 = false;
            bool bKey2 = false;

            //TODO : 이상훈(2019.08.09) 
            //If KeyCode > 47 And KeyCode< 58 Then bKey1 = True
            //If KeyCode > 64 And KeyCode< 91 Then bKey2 = True
            //if (0bKey1 == false && bKey2 == false) return;

            strCode = SS2.ActiveSheet.Cells[(int)FnRowNo, 0].Text.Trim();

            //숫자값을 사용하는 코드인지 확인
            strTemp = hicExcodeService.GetResultTypebyCode(strCode);

            if (strTemp == "1")
            {
                //숫자일경우
                if (VB.IsNumeric(SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text) == false)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
                }
            }
            else if (strTemp == "2")
            {
                //문자일경우
                if (VB.IsNumeric(SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text) == true)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
                }
            }
        }

        void eSpreadLeaveCell(object sender, LeaveCellEventArgs e)
        {
            string strGuBun = "";
            string strCode = "";
            string strResCode = "";
            string strHelp = "";
            string strData = "";

            FnRowNo = e.Row;

            if (e.Column != 2) return;

            strCode = VB.Pstr(SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text.Trim(), ".", 1);
            if (strCode.IsNullOrEmpty()) return;
            if (strCode == "미실시") return;

            strGuBun = SS2.ActiveSheet.Cells[(int)FnRowNo, 7].Text.Trim();
            if (strGuBun.IsNullOrEmpty()) return;

            if (strCode.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[(int)FnRowNo, 4].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[(int)FnRowNo, 4].Text = ha.READ_ResultName(strGuBun, strCode);
                if (SS2.ActiveSheet.Cells[(int)FnRowNo, 4].Text.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text = "";
                    MessageBox.Show(strCode + " 가 결과코드값에 등록이 안됨!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            string strResult = "";
            string strOK = "";

            if (sender == chkCorrectedVision)
            {
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    switch (SS2.ActiveSheet.Cells[i, 0].Text.Trim())
                    {
                        case "A104":
                        case "A105":
                            strOK = "OK";
                            break;
                        default:
                            break;
                    }
                    if (strOK == "OK")
                    {
                        strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (chkCorrectedVision.Checked == true)
                        {
                            if (VB.Left(strResult, 1) != "(" && VB.Right(strResult, 1) != ")")
                            {
                                strResult = "(" + strResult + ")";
                                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                            }                
                        }
                        else
                        {
                            if (VB.Left(strResult, 1) == "(" && VB.Right(strResult, 1) == ")")
                            {
                                strResult = VB.STRCUT(strResult, "(", ")");
                                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                            }
                        }
                    }
                }
            }
            else if (sender == chkCorrectedHearing)
            {
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    switch (SS2.ActiveSheet.Cells[i, 0].Text.Trim())
                    {
                        case "A106":
                        case "A107":
                            strOK = "OK";
                            break;
                        default:
                            break;
                    }
                    if (strOK == "OK")
                    {
                        strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (chkCorrectedHearing.Checked == true)
                        {
                            if (VB.Left(strResult, 1) != "(" && VB.Right(strResult, 1) != ")")
                            {
                                strResult += "(교정)";
                                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                            }
                        }
                        else
                        {
                            if (VB.Right(strResult, 4) == "(교정)")
                            {
                                strResult = VB.STRCUT(strResult, "", "(");
                                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                            }
                        }
                    }
                }
            }   
        }

        void eSpreadChange(object sender, ChangeEventArgs e)
        {
            double nData = 0;
            string strData = "";
            string strCode = "";

            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
            int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;

            strCode = SS2.ActiveSheet.Cells[nRow, 0].Text.Trim();
            nData = double.Parse(SS2.ActiveSheet.Cells[nRow, 2].Text.Trim());
            strData = SS2.ActiveSheet.Cells[nRow, 2].Text.Trim();
            SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";

            //숫자값을 사용하는 코드인지 확인
            if (hicExcodeService.GetResultTypebyCode(strCODE) == "1")
            {
                if (nData > 999)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                    return;
                }

                switch (strCODE)
                {
                    case "A108":    //혈압(최고)
                        if (nData > 250 || nData < 60)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    case "A109":    //혈압(최저)
                        if (nData > 250 || nData < 40)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    case "A114":    //소변Ph
                        if (nData > 8 || nData < 6)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    case "A104":    //시력(좌)
                    case "A105":    //시력(우)
                    case "C203":    //교정시력(좌)
                    case "C204":    //교정시력(우)
                        nData = double.Parse(SS2.ActiveSheet.Cells[nRow, 2].Text.Trim().Replace("(", "").Replace(")", ""));
                        if ((nData > 2 || nData < 0.1) && strData != ".")
                        {
                            if (nData != 9.9)
                            {
                                MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                            }
                        }
                        break;
                    case "A115":    //허리둘레
                        if (nData > 150 || nData < 50)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    case "ZD04":    //흉부둘레
                        if (nData > 150 || nData < 50)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    case "A101":    //신장
                        if (nData > 200 || nData < 100)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    case "A102":    //체중
                        if (nData > 200 || nData < 10)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                        }
                        break;
                    //청력(F4 Only)
                    case "A106":
                    case "A107":
                        break;
                    default:
                        break;
                }
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            int nAge;
            string strPart;
            string strSName;
            string strSex;
            string strGjJong;
            string strRoom;

            FpSpread o = (FpSpread)sender;

            if (sender == this.ssChk)
            {
                if (ssChk.ActiveSheet.NonEmptyRowCount == 0) return;

                if (e.Column == 4)
                {
                    if (txtWrtNo.Text.Trim().IsNullOrEmpty())
                    {
                        txtWrtNo.Focus();
                        return;
                    }

                    HEA_JEPSU Wait_List = heaJepsuService.Read_Jepsu_Wait_List(long.Parse(txtWrtNo.Text), dtpFDate.Text);

                    if (Wait_List.RID.IsNullOrEmpty())
                    {
                        MessageBox.Show(txtWrtNo.Text + " 접수내역이 없습니다", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        txtWrtNo.Focus();
                        return;
                    }

                    strPart = ssChk.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                    //해당방에 등록되어 있는지 확인
                    HEA_SANGDAM_WAIT SDReg_List = heaSangdamWaitService.Read_Sangdam_Wait_RegList(long.Parse(txtWrtNo.Text), strPart);

                    if (!SDReg_List.RID.IsNullOrEmpty())
                    {
                        if (SDReg_List.GBCALL == "Y")
                        {
                            MessageBox.Show(SDReg_List.GUBUN + " 번방의 검사가 이미 완료되었습니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            MessageBox.Show(SDReg_List.GUBUN + " 번방에 이미 등록되어 있습니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        return;
                    }

                    //다른검사실에 등록되어 있는지 확인
                    HEA_SANGDAM_WAIT SDEtcReg_List = heaSangdamWaitService.Read_Sangdam_EtcRoomReg(long.Parse(txtWrtNo.Text), strPart);

                    if (!SDEtcReg_List.RID.IsNullOrEmpty())
                    {
                        if (!SDEtcReg_List.CALLTIME.IsNullOrEmpty())
                        {
                            MessageBox.Show(SDEtcReg_List.GUBUN + " 번방에서 검사중입니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                        else
                        {
                            //기존의 자료가 있으면 삭제함
                            int result = heaSangdamWaitService.Delete_Sangdam_PreData(FnWRTNO, strPart);

                            if (result < 0)
                            {
                                MessageBox.Show("타 검사실 대기순번 삭제시 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    Application.DoEvents();

                    //해당방에 등록
                    HEA_SANGDAM_WAIT View_List = heaSangdamWaitService.Read_Sangdam_View(long.Parse(txtWrtNo.Text));

                    if (!View_List.RID.IsNullOrEmpty())
                    {
                        strSName = View_List.SNAME;
                        strSex = View_List.SEX;
                        nAge = Convert.ToInt32(View_List.AGE);
                        strGjJong = View_List.GJJONG;

                        HEA_SANGDAM_WAIT View_List2 = heaSangdamWaitService.Read_Sangdam_View2(strPart);

                        HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                        item.WRTNO = FnWRTNO;
                        item.SNAME = strSName;
                        item.SEX = strSex;
                        item.AGE = nAge;
                        item.GJJONG = strGjJong;
                        item.GBCALL = "";
                        item.GUBUN = FstrRoom;
                        item.WAITNO = Convert.ToInt32(View_List2.WAITNO);

                        int result = heaSangdamWaitService.Insert_Sangdam_Wait2(item);

                        if (result < 0)
                        {
                            MessageBox.Show("상담대기 순번등록 중 오류 발생!!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        FstrRoom2 = strPart;
                    }

                    for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                    {
                        strRoom = ssChk.ActiveSheet.Cells[i, 6].Text.Trim();

                        HEA_SANGDAM_WAIT WrtNoCnt_List = heaSangdamWaitService.Read_Sangdam_WrtNoCnt(strPart);

                        if (strRoom != "99")
                        {
                            ssChk.ActiveSheet.Cells[i, 3].Text = WrtNoCnt_List.CNT;
                        }
                    }

                    //현 검사실 대기명단
                    List<HEA_SANGDAM_WAIT> Wait_List1 = heaSangdamWaitService.Read_Sangdam_Wait_List(strPart, "N");

                    for (int i = 0; i < Wait_List1.Count; i++)
                    {
                        ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List1[i].SNAME;
                        ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List1[i].WRTNO.ToString();
                    }
                }
                else if (e.Column == 5)                 
                {
                    if (VB.Val(ssChk.ActiveSheet.Cells[e.Row, 8].Text) == 0)
                    {
                        txtWrtNo.Focus();
                        return;
                    }
                    txtWrtNo.Text = ssChk.ActiveSheet.Cells[e.Row, 8].Text;
                    txtWrtNo.Focus();

                    eTextBoxKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
                    return;
                }
                else
                {
                    tabControl1.TabIndex = 0;
                    tabControl1.SelectedTab = tabJepsu;
                    sp.Spread_All_Clear(ssJepList);
                    ssJepList.ActiveSheet.RowCount = 30;

                    if (e.Column == 1)
                    {
                        strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                        List<HEA_SANGDAM_WAIT> Waitlist1 = heaSangdamWaitService.Read_Sangdam_View3(strPart);
                        
                        ssJepList.ActiveSheet.RowCount = Waitlist1.Count;

                        for (int i = 0; i < Waitlist1.Count; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = Waitlist1[i].WRTNO.ToString();
                            ssJepList.ActiveSheet.Cells[i, 1].Text = Waitlist1[i].SNAME;
                        }
                    }
                    else
                    {
                        strPart = ssChk.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                        List<Select_JepList> Waitlist2 = selectJeplistService.Read_Sangdam_View4(dtpFDate.Text, strPart);

                        for (int i = 0; i < Waitlist2.Count; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = Waitlist2[i].WRTNO.ToString();
                            ssJepList.ActiveSheet.Cells[i, 1].Text = Waitlist2[i].SNAME;
                            ssJepList.ActiveSheet.Cells[i, 2].Text = Waitlist2[i].AMPM2;
                            ssJepList.ActiveSheet.Cells[i, 4].Text = Waitlist2[i].NAME;
                        }
                    }
                }
                txtWrtNo.Focus();
            }
            if (sender == this.ssJepList)
            {
                long nWrtNo = 0;

                if (e.RowHeader == true) return;

                if (ssJepList.ActiveSheet.NonEmptyRowCount == 0) return;

                fn_Screen_Clear();

                txtWrtNo.Text = ssJepList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                nWrtNo = long.Parse(txtWrtNo.Text);

                eTextBoxKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));

                tabControl1.TabIndex = 1;
                tabControl1.SelectedTab = tabChk;

                txtWrtNo.Focus();
                txtWrtNo.SelectionStart = 0;
                txtWrtNo.SelectionLength = txtWrtNo.Text.Length;
            }
            else if (sender == this.SS2)
            {
                string strResCode = "";
                string strData = "";

                if (e.Column != 2)
                {
                    return;                    
                }

                SS2.ActiveSheet.SetActiveCell(e.Row, 2);
            }
        }

        void eTimerTick(object sender, EventArgs e)
        {
            if (sender == timer)
            {
                string strYoil = "";

                lblDateTime.Text = "";
                ComFunc.ReadSysDate(clsDB.DbCon);
                lblDateTime.Text = clsPublic.GstrSysDate + " " + clsVbfunc.GetYoIl(clsPublic.GstrSysDate) + " " + clsPublic.GstrSysTime;
            }
            else if (sender == timer1)
            {
                //long lngHandle;
                //long chlHandle;
                //long chlHandle2;
                //string strBP1;
                //string strBP2;
                //string strBP3;
                //string strExcode;

                //lngHandle = FindWindow("TMain", "Member's information and Results.")
                //LabelBP.Caption = "1:" & Time$
                //If lngHandle > 0 Then
                //    LabelBP.Caption = "2:" & Time$

                //    strBP1 = "": strBP2 = "": strBP3 = ""
                //    For i = 1 To 13
                //        chlHandle2 = FindWindowEx(lngHandle, chlHandle2, "TPanel", vbNullString)
                //        If chlHandle2 > 0 Then
                //            Select Case i
                //                Case 13: strBP1 = Trim(GetText(chlHandle2)) '최고혈압
                //                Case 11: strBP2 = Trim(GetText(chlHandle2)) '최저혈압
                //                Case 10: strBP3 = Trim(GetText(chlHandle2)) '맥박
                //            End Select
                //        End If
                //    Next i
                //    If strBP1 = "" Or strBP2 = "" Then Exit Sub


                //    LabelBP.Caption = strBP1 & "/" & strBP2 & "/" & strBP3


                //    FrameBP.Visible = True
                //    Timer1.Enabled = False


                //    '검사결과에 표시
                //    For i = 1 To SS2.MaxRows
                //        SS2.Row = i: SS2.Col = 1: strExcode = Trim(SS2.Text)
                //        If strExcode = "A108" Then
                //            SS2.Col = 3: SS2.Text = strBP1
                //            SS2.Col = 9: SS2.Text = "Y"
                //        ElseIf strExcode = "A109" Then
                //            SS2.Col = 3: SS2.Text = strBP2
                //            SS2.Col = 9: SS2.Text = "Y"
                //        ElseIf strExcode = "TA07" Then
                //            SS2.Col = 3: SS2.Text = strBP3
                //            SS2.Col = 9: SS2.Text = "Y"
                //        End If
                //    Next i
                //End If
            }
            else if (sender == timer2)
            {
                string strExcode;
                string strTE43;
                string strTE44;

                if (VB.InStr(FstrBuffer, "L-A:") > 0)
                {
                    strTE43 = VB.STRCUT(FstrBuffer, "L01: ", "\r\n");
                    strTE44 = VB.STRCUT(FstrBuffer, "R01: ", "\r\n");
                    FstrBuffer = "";

                    //검사결과에 표시
                    for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                    {
                        strExcode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CODE].Text.Trim();
                        if (strExcode == "TE43")
                        {
                            SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text = strTE43;
                            SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text = "Y";
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text = strTE44;
                            SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text = "Y";
                        }
                    }
                }
            }
            else if (sender == timerAutoActing)
            {
                FnTimer += 1;

                if (FnTimer >= 5)
                {
                    FnTimer = 0;
                    timerAutoActing.Enabled = false;
                    FnActWrtno = 0;
                    menuAutoActing_Click();
                    timerAutoActing.Enabled = true;
                    txtWrtNo.Focus();
                }
            }
            else if (sender == timerHeaRsltAutoSend)
            {
                string strYoil = "";
                bool bOK = false;
                bool bLastSend = false;

                if (clsCompuInfo.gstrCOMIP != "192.168.41.124") return;

                timerHeaRsltAutoSend.Enabled = false;

                ComFunc.ReadSysDate(clsDB.DbCon);

                strYoil = cF.READ_YOIL(clsDB.DbCon, clsPublic.GstrSysDate);

                //-------------------------------------------------------
                //  종합검진 결과 자동전송 실행
                //  작업시간(평일):   08:00, 13:00, 16:30
                //  작업시간(토요일): 08:00, 11:30
                //-------------------------------------------------------

                bOK = false;
                bLastSend = false;
                if (strYoil == "토요일")
                {
                    if (string.Compare(clsPublic.GstrSysTime, "08:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "08:30") <= 0)
                    {
                        if (VB.Pstr(FstrHeaLastTime, "{}", 2) != "08:00")
                        {
                            FstrHeaLastTime = clsPublic.GstrSysDate + "{}08:00{}";
                            bOK = true;
                        }
                    }
                    else if (string.Compare(clsPublic.GstrSysTime, "11:30") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:00") <= 0)
                    {
                        if (VB.Pstr(FstrHeaLastTime, "{}", 2) != "11:30")
                        {
                            FstrHeaLastTime = clsPublic.GstrSysDate + "{}11:30{}";
                            bOK = true;
                            bLastSend = true;
                        }
                    }
                }
                else
                {
                    if (string.Compare(clsPublic.GstrSysTime, "08:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "08:30") <= 0)
                    {
                        if (VB.Pstr(FstrHeaLastTime, "{}", 2) != "08:00")
                        {
                            FstrHeaLastTime = clsPublic.GstrSysDate + "{}08:00{}";
                            bOK = true;
                        }
                    }
                    else if (string.Compare(clsPublic.GstrSysTime, "13:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "13:30") <= 0)
                    {
                        if (VB.Pstr(FstrHeaLastTime, "{}", 2) != "13:00")
                        {
                            FstrHeaLastTime = clsPublic.GstrSysDate + "{}13:00{}";
                            bOK = true;
                        }
                    }
                    else if (string.Compare(clsPublic.GstrSysTime, "16:30") >= 0 && string.Compare(clsPublic.GstrSysTime, "17:00") <= 0)
                    {
                        if (VB.Pstr(FstrHeaLastTime, "{}", 2) != "16:30")
                        {
                            FstrHeaLastTime = clsPublic.GstrSysDate + "{}16:30{}";
                            bOK = true;
                            bLastSend = true;
                        }
                    }
                }

                if (bOK == true)
                {
                    ///TODO : 이상훈 종검결과자동전송.txt 파일 사용 확인 필요
                    //cF.Write_File(@"c:\cmc\종검결과자동전송.txt", FstrHeaLastTime);
                    //frmHeaResultAutoSend f = new frmHeaResultAutoSend();
                    //f.ShowDialog();
                }

                //평일: 16:30, 토요일: 11:30 마감 전송
                if (bLastSend == true)
                {
                    FnTimerCNT = 0;
                    fn_TodayHic_Result_Update(FstrBDate);
                }

                //5분마다 작업을 처리함
                FnTimerCNT += 1;
                if (FnTimerCNT > 5)
                {
                    FnTimerCNT = 0;
                    fn_TodayHic_AutoAction(FstrBDate);
                }

                timerHeaRsltAutoSend.Enabled = true;
            }
            else if (sender == timerSecondBarCodePrt)
            {
                //10초마다 2차검진자 접수증(바코드)인쇄
                long nREAD = 0;
                long nCNT = 0;
                long nWRTNO = 0;
                long nPano = 0;
                string strJumin = "";
                string strRowId = "";

                if (clsCompuInfo.gstrCOMIP != "192.168.41.91" && clsCompuInfo.gstrCOMIP != "192.168.41.32")
                {
                    return;
                }
                timerSecondBarCodePrt.Enabled = false;

                List<HIC_WAIT> list = hicWaitService.GetReExambyDate();

                nREAD = list.Count;

                for (int i = 0; i < nREAD; i++)
                {
                    strJumin = clsAES.DeAES(list[i].JUMIN2);

                    HIC_JEPSU listJepsu = hicJepsuService.GetAllbyJumin2(list[i].GJJONG, list[i].JUMIN2);

                    strRowId = listJepsu.RID;

                    if (listJepsu != null)
                    {
                        nWRTNO = listJepsu.WRTNO;
                        nPano = listJepsu.PANO;

                        //성명 성별 나이 종류 주민 날짜
                        clsPublic.GstrRetValue = "";
                        clsPublic.GstrRetValue = listJepsu.SNAME.Trim() + "^^";
                        clsPublic.GstrRetValue += listJepsu.SEX.Trim() + "^^";
                        clsPublic.GstrRetValue += listJepsu.AGE + "^^";
                        clsPublic.GstrRetValue += listJepsu.GJJONG.Trim() + hb.READ_GjJong_Name(listJepsu.GJJONG.Trim()) + "^^";
                        clsPublic.GstrRetValue += VB.Left(strJumin, 6) + "-";
                        clsPublic.GstrRetValue += VB.Mid(strJumin, 7, 1) + "******" + "^^";
                        clsPublic.GstrRetValue += clsPublic.GstrSysDate + "^^";
                        clsPublic.GstrRetValue += string.Format("{0:#}", listJepsu.LTDCODE) + "^^";

                        //검사항목 조회
                        List<HIC_EXCODE> listPart = hicExcodeService.GetEntPartbyWrtNo(nWRTNO);

                        string[] arrCodeName = listPart.GetStringArray("CODENAME");

                        nCNT = listPart.Count;
                        FstrRetValue = "";

                        for (int j = 0; j < nCNT; j++)
                        {
                            FstrRetValue += hicCodeService.Read_Hic_CodeName(arrCodeName[j]);
                            if (j == nCNT - 1) break;
                            FstrRetValue += "^^";
                        }

                        fn_HcJepsuReport(nWRTNO, nPano);

                        int result = hicWaitService.Update_SecondPrint(strRowId.Trim());

                        if (result < 0)
                        {
                            MessageBox.Show("검사결과 등록중 오류 발생! (골밀도검사)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                timerSecondBarCodePrt.Enabled = true;
            }
        }

        void Work_InBody_Interface_NEW()
        {
            string strPath;
            string strBirth;
            string strJumin1;
            string strJumin2;

            strPath = @"C:\LookinBody120\Database\LookinBody.mdb";

            ///TODO : 이상훈(2019-07-12) INBODY I/F 시 재작업 필요
            //MDBadoConnect_BioSpace(strPATH)   //메인PC
            //if (GstrMDBAdoMsg == "NO") return;

        //    SQL = "SELECT USER_ID FROM USER_INFO1_TBL WHERE USER_ID ='" & FstrPtno & "' "
        //    Call MDBAdoOpenSet(AdoRes, SQL)

        //    '인바디 환자정보 체크
        //    If MDBRowindicator = 0 Then


        //        adoConnect2.BeginTrans

        //        SQL = " SELECT Pano,SName,Sex,Birth,Jumin1,Jumin2,Jumin3 "
        //        SQL = SQL & " From KOSMOS_PMPA.BAS_PATIENT "
        //        SQL = SQL & " WHERE PANO = '" & FstrPtno & "' "
        //        Call AdoOpenSet(Rs, SQL)


        //        If RowIndicator > 0 Then

        //            strJumin1 = Trim(AdoGetString(Rs, "Jumin1", 0))


        //            If Trim(AdoGetString(Rs, "Jumin3", 0)) <> "" Then
        //                strJumin2 = Trim(DeAES(AdoGetString(Rs, "Jumin3", 0)))
        //            Else
        //                strJumin2 = Trim(AdoGetString(Rs, "Jumin2", 0))
        //            End If


        //            strBirth = Trim(AdoGetString(Rs, "Birth", 0))
        //            If strBirth = "" Then
        //                strBirth = Date_Format_BirthDate(strJumin1, strJumin2)
        //            End If



        //'            SQL = " INSERT INTO USER_INFO1_TBL (USER_ID,NAME,Gender, AGE,Height,BirthDay,Birth_Select,Tel_Home,Tel_HP,E_Mail,Exe_Now,User_Reg_Date ) VALUES ("
        //'            SQL = SQL & " '" & FstrPtno & "','" & Trim(AdoGetString(Rs, "SNAME", 0)) & "','" & Trim(AdoGetString(Rs, "SEX", 0)) & "', "
        //'            SQL = SQL & "  " & AGE_YEAR_GESAN(strJumin1 & strJumin2, GstrSysDate) & ",50,'" & strBirth & "','N','--','--','@',0,'" & GstrSysDate & " " & GstrSysTime & ":00' ) "
        //            SQL = " INSERT INTO USER_INFO1_TBL ( USER_ID, NAME, GENDER, Age, BirthDay, USER_REG_DATE, UPDATE_DATE) VALUES ("
        //            SQL = SQL & " '" & FstrPtno & "','" & Trim(AdoGetString(Rs, "SNAME", 0)) & "','" & Trim(AdoGetString(Rs, "SEX", 0)) & "', "
        //            SQL = SQL & "  " & AGE_YEAR_GESAN(strJumin1 & strJumin2, GstrSysDate) & ",'" & strBirth & "','" & GstrSysDate & " " & GstrSysTime & ":00' ,'" & GstrSysDate & " " & GstrSysTime & ":00' )"


        //            result = MDBAdoExecute(SQL)


        //            If result<> 0 Then
        //               MsgBox "DB Error!!", vbInformation, "확인"
        //                adoConnect2.RollbackTrans
        //                Exit Sub
        //            End If
        //        End If
        //        Call AdoCloseSet(Rs)

        //        adoConnect2.CommitTrans


        //    End If

        //    Call MDBAdoCloseSet(AdoRes)


        //    Call MyAdoDisConnect
        //    Call MyAdoDisConnect2
        }

        void Work_InBody_Interface_NEW1()
        {
            string strPath;
            string strBirth;
            string strJumin1;
            string strJumin2;

            strPath = @"Z:\Database\LookinBody.MDB";

            ///TODO : 이상훈(2019-07-12) INBODY I/F 시 재작업 필요
            //MDBadoConnect_BioSpace(strPATH)   //메인PC
            //if (GstrMDBAdoMsg == "NO") return;

        //    SQL = "SELECT USER_ID FROM USER_INFO1_TBL WHERE USER_ID ='" & FstrPtno & "' "
        //    Call MDBAdoOpenSet(AdoRes, SQL)
        //    '''MsgBox "조회했음"
        //    '인바디 환자정보 체크
        //    If MDBRowindicator = 0 Then


        //        adoConnect2.BeginTrans

        //        SQL = " SELECT Pano,SName,Sex,Birth,Jumin1,Jumin2,Jumin3 "
        //        SQL = SQL & " From KOSMOS_PMPA.BAS_PATIENT "
        //        SQL = SQL & " WHERE PANO = '" & FstrPtno & "' "
        //        Call AdoOpenSet(Rs, SQL)



        //        If RowIndicator > 0 Then

        //            strJumin1 = Trim(AdoGetString(Rs, "Jumin1", 0))


        //            If Trim(AdoGetString(Rs, "Jumin3", 0)) <> "" Then
        //                strJumin2 = Trim(DeAES(AdoGetString(Rs, "Jumin3", 0)))
        //            Else
        //                strJumin2 = Trim(AdoGetString(Rs, "Jumin2", 0))
        //            End If


        //            strBirth = Trim(AdoGetString(Rs, "Birth", 0))
        //            If strBirth = "" Then
        //                strBirth = Date_Format_BirthDate(strJumin1, strJumin2)
        //            End If



        //'            SQL = " INSERT INTO USER_INFO1_TBL (USER_ID,NAME,Gender, AGE,Height,BirthDay,Birth_Select,Tel_Home,Tel_HP,E_Mail,Exe_Now,User_Reg_Date ) VALUES ("
        //'            SQL = SQL & " '" & FstrPtno & "','" & Trim(AdoGetString(Rs, "SNAME", 0)) & "','" & Trim(AdoGetString(Rs, "SEX", 0)) & "', "
        //'            SQL = SQL & "  " & AGE_YEAR_GESAN(strJumin1 & strJumin2, GstrSysDate) & ",50,'" & strBirth & "','N','--','--','@',0,'" & GstrSysDate & " " & GstrSysTime & ":00' ) "
        //            SQL = " INSERT INTO USER_INFO1_TBL ( USER_ID, NAME, GENDER, Age, BirthDay, USER_REG_DATE, UPDATE_DATE) VALUES ("
        //            SQL = SQL & " '" & FstrPtno & "','" & Trim(AdoGetString(Rs, "SNAME", 0)) & "','" & Trim(AdoGetString(Rs, "SEX", 0)) & "', "
        //            SQL = SQL & "  " & AGE_YEAR_GESAN(strJumin1 & strJumin2, GstrSysDate) & ",'" & strBirth & "','" & GstrSysDate & " " & GstrSysTime & ":00' ,'" & GstrSysDate & " " & GstrSysTime & ":00' )"


        //            result = MDBAdoExecute(SQL)
        //            '''MsgBox "저장했음"
        //            If result<> 0 Then
        //               MsgBox "DB Error!!", vbInformation, "확인"
        //                adoConnect2.RollbackTrans
        //                Exit Sub
        //            End If
        //        End If
        //        Call AdoCloseSet(Rs)

        //        adoConnect2.CommitTrans


        //    End If

        //    Call MDBAdoCloseSet(AdoRes)


        //    Call MyAdoDisConnect
        //    Call MyAdoDisConnect2
        //    '''MsgBox "정상완료"
        }

        void Work_InBody_Control_NEW()
        {
            int nCnt = 0;
            long hCmd = 0;
            long IngHand = 0;
            long lngHandle = 0;
            long chlHandle = 0;
            long chlHandle1 = 0;
            long chlHandle2 = 0;
            long chlHandle3 = 0;
            long result = 0;

            string strOK = "";

            lngHandle = FindWindow("WindowsForms10.Window.8.app.0.141b42a_r12_ad1", "Lookin'Body");

            if (lngHandle > 0)
            {
                chlHandle = FindWindowEx(lngHandle, 0, "WindowsForms10.Window.8.app.0.141b42a_r12_ad1", null);

                for (int i = 1; i < 500; i++)
                {
                    if (strOK == "OK")
                    {
                        break;
                    }

                    if (chlHandle > 0)
                    {
                        chlHandle1 = FindWindowEx(chlHandle, 0, "WindowsForms10.Window.8.app.0.141b42a_r12_ad1", null);

                        for (int j = 1; j < 500; j++)
                        {
                            if (chlHandle1 > 0)
                            {
                                chlHandle2 = FindWindowEx(chlHandle1, 0, "WindowsForms10.EDIT.app.0.141b42a_r12_ad1", null);

                                for (int k = 1; k < 2; k++)
                                {
                                    chlHandle2 = FindWindowEx(chlHandle1, chlHandle2, "WindowsForms10.EDIT.app.0.141b42a_r12_ad1", null);
                                }

                                if (chlHandle2 > 0)
                                {
                                    strOK = "OK";
                                    break;
                                }
                            }
                            chlHandle1 = FindWindowEx(chlHandle, chlHandle1, "WindowsForms10.Window.8.app.0.141b42a_r12_ad1", null);
                        }
                    }
                    chlHandle = FindWindowEx(lngHandle, chlHandle, "WindowsForms10.Window.8.app.0.141b42a_r12_ad1", null);
                }
                hCmd = FindWindowEx(chlHandle1, 0, "WindowsForms10.BUTTON.app.0.141b42a_r12_ad1", "검색");  //## 버튼 핸들값 취득

                if (chlHandle2 > 0)
                {
                    if (WindowTextSet(chlHandle2, FstrPtno) == true)
                    {
                        ///TODO : 이상훈(2019.07.12) INBODY I/F 시 재작업 필요
                        PostMessage(hCmd, WM_COMMAND, "0&", hCmd);
                    }
                }
            }
        }

        void Work_InBody_Control_NEW1()
        {
            int nCnt = 0;
            long hCmd = 0;
            long IngHand = 0;
            long lngHandle = 0;
            long chlHandle = 0;
            long chlHandle1 = 0;
            long chlHandle2 = 0;
            long chlHandle3 = 0;
            long result = 0;

            string strOK = "";

            lngHandle = FindWindow("WindowsForms10.Window.8.app.0.2bf8098_r11_ad1", "Lookin'Body");

            if (lngHandle > 0)
            {
                chlHandle = FindWindowEx(lngHandle, 0, "WindowsForms10.Window.8.app.0.2bf8098_r11_ad1", null);

                for (int i = 0; i < 500; i++)
                {
                    if (strOK == "OK")
                    {
                        break;
                    }

                    if (chlHandle > 0)
                    {
                        chlHandle1 = FindWindowEx(chlHandle, 0, "WindowsForms10.Window.8.app.0.2bf8098_r11_ad1", null);

                        for (int j = 0; j < 500; j++)
                        {
                            if (chlHandle1 > 0)
                            {
                                chlHandle2 = FindWindowEx(chlHandle1, 0, "WindowsForms10.EDIT.app.0.2bf8098_r11_ad1", null);

                                for (int k = 1; k < 2; k++)
                                {
                                    chlHandle2 = FindWindowEx(chlHandle1, chlHandle2, "WindowsForms10.EDIT.app.0.2bf8098_r11_ad1", null);
                                }

                                if (chlHandle2 > 0)
                                {
                                    strOK = "OK";
                                    break;
                                }
                            }
                            chlHandle1 = FindWindowEx(chlHandle, chlHandle1, "WindowsForms10.Window.8.app.0.2bf8098_r11_ad1", null);
                        }
                    }
                    chlHandle = FindWindowEx(lngHandle, chlHandle, "WindowsForms10.Window.8.app.0.2bf8098_r11_ad1", null);
                }
                hCmd = FindWindowEx(chlHandle1, 0, "WindowsForms10.BUTTON.app.0.2bf8098_r11_ad1", "검색");  //## 버튼 핸들값 취득

                if (chlHandle2 > 0)
                {
                    if (WindowTextSet(chlHandle2, FstrPtno) == true)
                    {
                        ///TODO : 이상훈(2019.07.12) INBODY I/F 시 재작업 필요
                        //PostMessage(hCmd, WM_COMMAND, 0&, hCmd);
                    }
                }
            }
        }

        bool WindowTextSet(long hWnd, string strText)
        {
            bool rtnVal = false;

            rtnVal = (SendMessage(hWnd, WM_SETTEXT, strText.Length, strText) != 0);

            return rtnVal;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();
            lblDateTime.Text = "";
            lblDateTime.Text = clsPublic.GstrSysDate + " " + clsVbfunc.GetYoIl(clsPublic.GstrSysDate) + " " + clsPublic.GstrSysTime;
            conHcPatInfo1.SetDisPlay("25420", "O", "2018-12-13", "07998114", "HR", "11");
            //SetControl();
            this.Text = this.Text + " " + "☞작업자: " + clsPublic.GstrJobName;

            FstrBDate = clsPublic.GstrSysDate;

            clsHcVariable.GstrHicPart = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");

            if (clsHcVariable.GstrHicPart == "1")   //종검
            {
                pnlHc.Visible = false;
                pnlptInfo.Top = pnlptInfo.Top - 35;
                panMain.Top = panMain.Top - 35;
                panList.Top = panList.Top - 35;

                lblGuide.Top = lblGuide.Top + 35;
                SS2.Height = SS2.Height + 35;
                ssJepList.Height = ssJepList.Height + 35;
                ssChk.Height = ssChk.Height + 35;
                pnlBP.Top = pnlBP.Top + 35;

                pnlptInfo.Height = pnlptInfo.Height + 35;
                panMain.Height = panMain.Height + 35;
                tabControl1.Height = tabControl1.Height + 35;
                panList.Height = panList.Height + 35;
            }
            else if (clsHcVariable.GstrHicPart == "2")  //일검
            {
                pnlHc.Visible = true;
            }
            else
            {
                MessageBox.Show("일반검진/종합검진 Setting이 되어 있지 않습니다!!!" + "\r\n" + "부서 설정 후 프로그램이 종료됩니다!" + "\r\n" + "재시작 하십시오!", "PC Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                frmHicPcSet f = new frmHicPcSet();
                f.ShowDialog();
                Application.Exit();                
            }

            if (VB.Pstr(CboPart.Text, ".", 1) == "**") //전체
            {
                lblExTitle.Text = "**.전체";
            }

            if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
            {
                HIC_EXCODE haroomlist = hicExcodeService.Read_HaRoom(VB.Pstr(CboPart.Text, ".", 1));

                FstrRoom = haroomlist.HAROOM;
            }
            else //파트가 접수(전체)일때
            {                
                FstrRoom = "ALL";
            }

            fn_Screen_Clear();

            txtWrtNo.Focus();

            if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
            {
                timerAutoActing.Enabled = false;        //종검자동액팅 Timer
                timerHeaRsltAutoSend.Enabled = false;   //종검 결과 자동전송 Timer
                timerSecondBarCodePrt.Enabled = false;  //2차검진 접수증(바코드) 인쇄 Timer

                if (chkInBodySend.Checked == true)
                {
                    Chk_InBody();
                }

                if (FstrPartG.ToString() ==  "13")
                {
                    gDtSpecCode = lbExSQL.sel_EXAM_SPECODE_Code(clsDB.DbCon);//검체등 기본코드 메모리 Load
                                                                             //gsWard = "TO";    //PC의병동코드(건강증진센터) //사용하는곳 없음.
                }

                //검사실 바코드 자동인쇄요청 PC 여부 설정
                BarCode_AutoPrint();

                timer2.Enabled = false;
                if (lblExTitle.Text == "8.안압 검사")
                {
                    FstrBuffer = "";
                    if (null == m_sp)
                    {
                        m_sp = new SerialPort();
                        m_sp.PortName = "";                     ///TODO : 이상훈 (2019.07.12) PortName 설정 필요
                        m_sp.BaudRate = Convert.ToInt32("");    ///TODO : 이상훈 (2019.07.12) 보드레이트 설정 필요
                        m_sp.Open();
                    }
                    else
                    {
                        if (!m_sp.IsOpen)
                        {
                            m_sp.Open();
                        }
                    }
                    timer2.Enabled = true;
                }

                //체중계 자동인터페이스 연결 PC인지 점검
                List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode("BAS_체중계연결PC", clsCompuInfo.gstrCOMIP);

                if (list.Count > 0)
                {
                    FbWeightPc = true;
                }
                else
                {
                    FbWeightPc = false;
                }

                //소변컵 라벨을 읽어 소변검사 액팅하는 PC인지 점검
                List<HIC_BCODE> list2 = hicBcodeService.GetCodeNamebyBcode("BAS_소변컵바코드읽기PC", clsCompuInfo.gstrCOMIP);

                if (list2.Count > 0)
                {
                    FbUAScanPc = true;
                }
                else
                {
                    FbUAScanPc = false;
                }

                //????????????????????????????????
                //If Val(GstrHelpCode) > 0 Then
                //    TxtWRTNO = Val(GstrHelpCode)
                //    Call Screen_Display
                //    GstrHelpCode = ""
                //End If

                //검사실코드 및 명칭을 읽음
                List<HIC_BCODE> list3 = hicBcodeService.GetCodeNamebyBcode("BAS_표시장비검사실PC", clsCompuInfo.gstrCOMIP);

                FstrWaitRoom = "";
                FstrWaitName = "";

                if (list3.Count > 0)
                {
                    strData = list3[0].NAME;
                    if (VB.Pstr(strData, "{}", 1) == "일반")
                    {
                        FstrWaitRoom = string.Format("{0:00}", VB.Pstr(strData, "{}", 2));
                        FstrWaitName = VB.Pstr(strData, "{}", 3);
                        this.Text += " 【검사실:" + FstrWaitRoom + "." + FstrWaitName + "】";
                        this.tabJepsu.Text = "대기자명단";
                        //CmdSang.Caption = "대기등록";
                    }
                }

                List<BAS_BCODE> list4 = basBcodeService.GetCodeNamebyBCode("HIC_촬영기사", clsType.User.Sabun);
                cboExId.SetItems(list4, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            }
            else
            {
                timerAutoActing.Enabled = true;        //종검자동액팅 Timer
                timerHeaRsltAutoSend.Enabled = true;   //종검 결과 자동전송 Timer
                timerSecondBarCodePrt.Enabled = true;  //2차검진 접수증(바코드) 인쇄 Timer

                //timer.Enabled = true;
                timer1.Enabled = false;
                timer2.Enabled = false;
            }
        }

        void fn_Screen_Display()
        {
            string strRowId = "";

            strToDate = dtpFDate.Value.ToShortDateString();
            strYYYY = strToDate.Substring(0, 4);
            strPart = VB.Pstr(CboPart.Text, ".", 1);

            //일검
            chkCorrectedVision.Checked = false;
            chkCorrectedHearing.Checked = false;

            if (strPart == "W")
            {
                btnSave.Enabled = false;
            }

            if (!txtWrtNo.Text.Trim().IsNormalized())
            {
                FnWRTNO = long.Parse(txtWrtNo.Text);
            }

            lblXray.Text = "";
            FnOLD_Height = 0;
            FnOLD_Weight = 0;
            strAllWrtno = "";

            if (VB.Pstr(lblExTitle.Text, ".", 1) == "**")
            {
                btnSave.Enabled = false;
                ///TODO : 이상훈(2019.08.19) 접수파트 조회시 로직 추가
            }
            else
            {
                if (FstrPartG.ToString().IsNullOrEmpty())
                {
                    MessageBox.Show("검사항목이 설정되지 않았습니다." + "\r\n" + "PC환경설정에서 검사항목을 설정하여 주십시오.", "검사항목미설정", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                if (strPart == "W")
                {
                    btnSave.Enabled = false;
                }

                if (!txtWrtNo.Text.IsNullOrEmpty())
                {
                    FnWRTNO = long.Parse(txtWrtNo.Text);
                }

                //삭제된것 체크
                if (hb.READ_JepsuSTS(FnWRTNO) == "D")
                {
                    MessageBox.Show("접수번호(" + FnWRTNO + ")는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                sp.Spread_All_Clear(SS2);
                SS2.ActiveSheet.RowCount = 30;
                Spread_Height_Set(SS2, 35);

                FbUAScanSave = false;

                if (FnWRTNO == 0)
                {
                    MessageBox.Show("접수번호가 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                { 
                    ufn_Update_Patient_Call();
                    ufn_Screen_Injek_display();         //인적사항을 Display
                    ufn_Screen_Exam_Items_display();    //검사항목을 Display
                    ufn_Screen_OLD_Result_Display();    //종전 2개의 결과 보여줌

                    //일검 - 방사선 직촬번호 입력
                    strRowId = hicResultService.GetXrayNoByWrtno(FnWRTNO);

                    if (strRowId != null)
                    {
                        if (!txtXrayNo.Text.Trim().IsNullOrEmpty())
                        {
                            int result = hicResultService.Update_ResultbyRowId(txtXrayNo.Text.Trim(), long.Parse(clsType.User.IdNumber), strRowId);

                            if (result < 0)
                            {
                                MessageBox.Show("XRAY 번호 등록중 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    //흉부촬영실 다음방 표시
                    if (FstrWaitRoom == "10")
                    {
                        strTemp = hicSangdamWaitService.GetNextRoomByWrtNo(FnWRTNO);
                        lblWaitRoom.Text = "";
                        lblWaitRoom.Visible = false;
                        FbAutoSave = false;
                        //흉부촬영실만 자동액팅을 함
                        if (strTemp == "10")
                        {
                            lblWaitRoom.Text = " " + hcact.READ_NextWait_Room(FnWRTNO);
                            lblWaitRoom.Visible = true;

                            //흉부촬영실 자동 액팅
                            FbAutoSave = true;
                            FnTimerSave = 0;
                            timer2.Enabled = true;
                        }
                    }

                    //바코드 자동발행 요청
                    if (FbExamBarCodeReq == true)
                    {
                        int result = hicBarcodeReqService.HIC_BARCODE_REQ_Insert(FnWRTNO);

                        if (result < 0)
                        {
                            return;
                        }
                    }

                    //흉부촬영실은 액팅점검 안함
                    if (FstrWaitRoom != "10")
                    {
                        ACTING_CHECK(FnWRTNO, strToDate, FnPano);
                    }

                    ///TODO : 이상훈 (2019.08.12) 종검만????
                    ufn_Screen_SpcExam_Display();   //일특항목 표시
                                                    ///TODO : 이상훈 (2019.08.12) 종검만????
                    Work_BP_Control();

                    //menu보류대장.Enabled = true;

                    //키,몸무게 인터페이스
                    if (FbWeightPc == true)
                    {
                        m_sp = new SerialPort();
                        m_sp.Close();
                        m_sp.Open();
                        FstrComm1 = "";
                    }
                }
            }
        }

        void ufn_Update_Patient_Call()
        {   
            int result = heaSangdamWaitService.Update_Patient_Call(FnWRTNO, FstrRoom);

            if (result < 0)
            {
                return;
            }
        }

        void ufn_Screen_Injek_display()
        {
            string strGbNaksang = "";
            string strGjJong = "";
            string strJepDate = "";
            string strJumin = "";
            string strSex = "";

            string strGbExam = "";
            string strExams = "";

            string strTemp = "";

            //인적사항을 READ
            PATIENT_INFO PtInfolist = patientInfoService.GetItembyWrtNoPart(FnWRTNO, clsHcVariable.GstrHicPart);

            if (PtInfolist.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 않됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWrtNo.Focus();
                return;
            }
            else
            {
                FstrPtno = PtInfolist.PTNO;
                FstrName = PtInfolist.SNAME;
                FstrSex = PtInfolist.SEX;
                FnAge = PtInfolist.AGE;
                FnPano = long.Parse(PtInfolist.PANO);
                FstrJumin = PtInfolist.JUMIN;
                FstrSDate = PtInfolist.JEPDATE;
                FstrGongDan = PtInfolist.GONGDAN;
                strGbNaksang = PtInfolist.GBNAKSANG;

                strSex = PtInfolist.SEX;
                strJumin = PtInfolist.JUMIN;
                strJepDate = PtInfolist.JEPDATE;
                strGjJong = PtInfolist.GJJONG;
                strGbExam = PtInfolist.GBEXAM;

                if (strPart == "13")
                {
                    lblExam.Visible = true;
                    if (strGbExam == "Y")
                    {
                        lblExam.Text = "검체발행";
                        lblExam.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblExam.Text = "검체미발행";
                        lblExam.ForeColor = Color.Red;
                    }
                }

                ssPatInfo.ActiveSheet.Cells[0, 1].Text = PtInfolist.SNAME;
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = PtInfolist.JUMIN.Substring(0, 6) + "-" + PtInfolist.JUMIN.Substring(6, 1) + "******";
                ssPatInfo.ActiveSheet.Cells[1, 1].Text = PtInfolist.SEX + "/" + PtInfolist.AGE;
                ssPatInfo.ActiveSheet.Cells[1, 3].Text = PtInfolist.ACTMEMO;

                HEA_JEPSU Jepsulist = heaJepsuService.Read_Jepsu(FnPano, FstrSDate);

                if (Jepsulist.RID != null)
                {
                    lblSDate.Text = "최종수검일" + "\r\n" + Jepsulist.SDATE1.ToString();
                }
                else
                {
                    lblSDate.Text = "검진첫회";
                }

                strExams = "";
                //추가검사 조회
                List<HEA_SUNAPDTL> YNamelist = heaSunapdtlService.Read_YName(FnWRTNO);

                if (YNamelist.Count > 0)
                {
                    for (int i = 0; i < YNamelist.Count; i++)
                    {
                        strExams = strExams + YNamelist[i].YNAME + ","; 
                    }
                }

                if (!strExams.IsNullOrEmpty())
                {
                    strExams = strExams.Substring(0, strExams.Length - 1);
                }

                //ssPatInfo.ActiveSheet.Cells[3, 1].Text = strExams;

                if (clsHcVariable.GstrHicPart == "1")
                {
                    //일반검진 내역 조회
                    HIC_JEPSU HicJepsulist = hicJepsuService.Read_Jepsu(FnPano, FstrSDate);

                    if (HicJepsulist.RID != null)
                    {
                        FnHcWRTNO = HicJepsulist.WRTNO;
                    }
                }
                else
                {
                    if (clsHcVariable.GstrHicPart == "2")   //일반검진
                    {
                        //방사선 번호 찾기
                        strXrayno = "";
                        txtXrayNo.Text = "";
                        strXrayno = PtInfolist.XRAYNO;
                        if (!strXrayno.IsNullOrEmpty())
                        {
                            strTemp = hicXrayResultService.GetXrayNobyXrayNo(strXrayno);
                            if (strTemp != null)
                            {
                                ///TODO : 이상훈(2019.08.12) 메뉴구성 확정 후 재 확인 필요
                                //Xrayno_Set.Enabled = true;
                            }
                            else
                            {
                                ///TODO : 이상훈(2019.08.12) 메뉴구성 확정 후 재 확인 필요
                                //Xrayno_Set.Enabled = false;
                            }
                            txtXrayNo.Text = strXrayno;
                        }

                        //종합검진 여부 읽음
                        nHeaPano = 0;
                        FnHeaWRTNO = 0;

                        //종검 마스타에서 종검번호를 찾음
                        nHeaPano = hicPatientService.GetPanobyJumin(clsAES.AES(strJumin));

                        if (nHeaPano > 0)
                        {
                            FnHeaWRTNO = heaJepsuService.GetWrtNobyPano(nHeaPano, FstrGjYear + "-01-01", strJepDate);
                        }

                        //오늘 모든 접수번호 찾기
                        List<HIC_JEPSU> list = hicJepsuService.GetWrtnoByPano_All(FnPano, strJepDate);

                        for (int i = 0; i < list.Count; i++)
                        {
                            strAllWrtno += list[i].WRTNO.ToString() + ",";
                        }

                        if (VB.Right(strAllWrtno, 1) == ",")
                        {
                            strAllWrtno = VB.Left(strAllWrtno, strAllWrtno.Length - 1);
                        }
                    }
                }

                //접수에서 로드시 미수납 여부 표시
                if (VB.Pstr(lblExTitle.Text, ".", 1) == "**")
                {
                    lblFormTitle.Text = "결과입력 Data 점검";
                    btnSave.Enabled = false;
                    SS2.ActiveSheet.Cells[0, 0, SS2.ActiveSheet.RowCount - 1, SS2.ActiveSheet.ColumnCount - 1].Locked = true;

                    if (PtInfolist.SUNAP != "Y")
                    {
                        lblSunap.Visible = true;
                    }
                    else
                    {
                        lblSunap.Visible = false;
                    }
                }
                else
                {
                    btnSave.Enabled = true;
                    lblSunap.Visible = false;
                }
            }
        }

        /// <summary>
        /// 검사항목을 Display
        /// </summary>
        void ufn_Screen_Exam_Items_display()
        {
            string strFlag = "";
            string strTemp2 = "";
            string strCode = "";
            string strName = "";

            List<EXAM_DISPLAY> ExamDspList = examDisplayService.Read_ExamList(FnWRTNO, FstrPartG);

            nREAD = ExamDspList.Count;
            nRow = 0;
            strFlag = "N";

            for (int i = 0; i < nREAD; i++)
            {
                strResult = ExamDspList[i].RESULT;
                strResCode = ExamDspList[i].RESCODE;
                strResultType = ExamDspList[i].RESULTTYPE;
                strGbCodeUse = ExamDspList[i].GBCODEUSE;
                strExcode = ExamDspList[i].EXCODE;
                strHName = ExamDspList[i].HNAME;
                nRow += 1;

                if (nRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = nRow;
                }

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].Text = strExcode;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.EXAMNAME].Text = strHName;

                if (strExCode == "ZD04")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].BackColor = Color.FromArgb(128, 255, 255);
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].BackColor = Color.FromArgb(255, 255, 255);
                }

                ufn_Combo_Set(strResCode);

                if (!strResCode.IsNullOrEmpty() && !strResult.IsNullOrEmpty())
                {
                    HIC_RESCODE ResCodeList = hicRescodeService.Read_ResCode_Single(strResCode, strResult);

                    strCode = ResCodeList.CODE;
                    strName = ResCodeList.NAME;

                    clsSpread.gSdCboItemFindLeft(SS2, nRow - 1, (int)clsHcType.Instrument_Result.RESULT, 2, strCode); //Spread Combo Item 찾아서 매칭
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = strResult;
                }

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].ForeColor = Color.FromArgb(0, 0, 0);
                //A103(비만도)는 자동계산(입력금지)
                if (strGbCodeUse == "N" || strExcode == "A103")
                {
                    if (strExcode != "A151" && strExcode != "TH01" && strExcode != "TH02")
                    {
                        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].CellType = txt;

                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = "";
                    }
                }

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.EXAMNAME].Text = ExamDspList[i].HNAME;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = strResult;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].ForeColor = Color.FromArgb(0, 0, 0);

                #region 일검 2017-02-06 소변컵 스캔 PC에서 결과가 공란은 자동으로 정상으로 표시
                if (FbUAScanPc == true && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].ForeColor = Color.FromArgb(0, 0, 255);
                    switch (strExcode)
                    {
                        case "A111":
                        case "A112":
                        case "A113":
                            SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = "01.음성";
                            FbUAScanSave = true;
                            break;
                        case "A114":
                            SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = "6.0";
                            FbUAScanSave = true;
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region 일검(비만도 자동계산함)
                if (strExCode == "A103")
                {
                    if (string.Compare(FstrGjYear, "2005") >= 0 && string.Compare(FstrGjYear, "2007") <= 0)
                    {
                        strResCode = "061";
                    }
                    else
                    {
                        strResCode = "065";
                    }
                }
                #endregion

                #region TX90: 동맥경화협착검사 결과가 공란이면 기본문구 표시.
                if (strExcode == "TX90" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = "동맥 경화검사: , 동맥 협착검사: ";
                }
                #endregion

                #region 자동계산은 선택못함
                switch (strExcode)
                {
                    case "A103":
                    case "TH91":
                    case "TH90":
                    case "A117":
                    case "A116":
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Locked = true;
                        break;
                    default:
                        break;
                }
                #endregion

                #region 일검
                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text = ha.READ_ResultName(strResCode, strResult);
                    }
                }
                #endregion

                if (!strResCode.IsNullOrEmpty())
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = strName;  //READ_HeaResultName(strResCode, strResult) 
                    }
                }

                if (ExamDspList[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text = "*";
                }

                if (ExamDspList[i].EXCODE == "A103")
                {   
                    strTemp = hm.Biman_Gesan(FnWRTNO);

                    if (VB.Val(strResult) <= 89)
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 222);
                    }
                    else if (VB.Val(strResult) >= 90 && VB.Val(strResult) <= 110)
                    {
                    }
                    else if (VB.Val(strResult) > 110)
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 222);
                    }
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = VB.Pstr(strTemp, ".", 1);
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = strTemp;
                }

                //혈액인자(RH -)
                if (ExamDspList[i].EXCODE == "H841")
                {
                    if (strResult == "-")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 222);
                    }
                }

                //참고치를 Dispaly
                if (strExcode != "E908" && strExcode != "E909")
                {
                    if (FstrSex == "M")
                    {
                        strNomal = ExamDspList[i].MIN_M + "-" + ExamDspList[i].MAX_M;
                    }
                    else
                    {
                        strNomal = ExamDspList[i].MIN_F + "-" + ExamDspList[i].MAX_F;
                    }
                }
                else
                {
                    HEA_WOMEN WRefList = heaWomenService.Read_Women_Reference(FnWRTNO);

                    if (strExcode == "E908")
                    {
                        strNomal = WRefList.MIN_ONE + "-" + WRefList.MAX_ONE;
                    }
                    else if (strExcode == "E909")
                    {
                        strNomal = WRefList.MIN_TWO + "-" + WRefList.MAX_TWO;
                    }
                }
                if (strNomal == "~")
                {
                    strNomal = "";
                }

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.REFERENCE].Text = strNomal;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESCODE].Text = strResCode;

                if (ExamDspList[i].EXCODE == "A151")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESCODE].Text = "007";
                }

                if (ExamDspList[i].EXCODE == "TH01" || ExamDspList[i].EXCODE == "TH02")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESCODE].Text = "022";
                }

                if (strFlag == "N")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CHANGE].Text = "";
                }

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.ROWID].Text = ExamDspList[i].RID;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESTYPE].Text = strResultType;

                //판정결과를 다시 Check함(L=Low,H=High)                
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text = hb.Result_Panjeng(ExamDspList[i].EXCODE, strResult, strNomal);

                if (SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.REFERENCE].Text == "L")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 220);
                }
                else if (SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.REFERENCE].Text == "H")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 220);
                }
                
                if (strExcode == "A271" || strExcode == "A272")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text = "";
                }

                if (SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].BackColor != Color.FromArgb(250, 210, 222))
                {
                    if (!ExamDspList[i].PANJENG.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 222);
                    }
                }
            }

            SS2.ActiveSheet.RowCount = nRow;
        }

        void ufn_Combo_Set(string strResCode)
        {
            string strList = "";

            //자료를 READ
            List<HIC_RESCODE> ResCodeList = hicRescodeService.Read_ResCode(strResCode);

            string[] arrCodeName = ResCodeList.GetStringArray("CODENAME");

            combo.Items = arrCodeName;
            combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
            combo.MaxDrop = 6;
            combo.MaxLength = 150;
            combo.ListWidth = 150;
            combo.Editable = false;
            SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.RESULT].CellType = combo;

            SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.HELP].Text = "Y";
        }

        /// <summary>
        /// 종전 2개의 결과 보여줌
        /// </summary>
        void ufn_Screen_OLD_Result_Display()
        {   
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1).Value = "결과 1";
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT2).Value = "결과 2";

            List<HEA_JEPSU> JepsuList = heaJepsuService.Read_Wrtno_SDate(FnPano, strJepDate);

            nREAD = JepsuList.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    if (i < 2)  break;

                    SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1 + i).Value = JepsuList[i].SDATE;

                    List<HIC_RESULT> RsltList = hicResultService.Get_Results(JepsuList[i].WRTNO);

                    for (int j = 0; j < RsltList.Count; j++)
                    {
                        strExcode = RsltList[j].EXCODE;
                        for (int k = 0; k < SS2.ActiveSheet.RowCount; k++)
                        {
                            if (SS2.ActiveSheet.Cells[k, (int)clsHcType.Instrument_Result.CODE].Text.Trim() == strExcode)
                            {
                                SS2.ActiveSheet.Cells[k, (int)clsHcType.Instrument_Result.PRERESULT1 + i].Text = RsltList[j].RESULT;
                            }
                        }
                        //키,몸무게 종전값 보관
                        if (FnOLD_Height == 0 && strExCode == "A101")
                        {
                            FnOLD_Height = double.Parse(RsltList[j].RESULT);
                        }
                        if (FnOLD_Weight == 0 && strExCode == "A102")
                        {
                            FnOLD_Weight = double.Parse(RsltList[j].RESULT);
                        }
                    }
                }
            }
            else
            {
                SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1).Value = "결과 1";
                SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT2).Value = "결과 2";
            }
        }

        void ACTING_CHECK(long ArgWRTNO, string ArgDate, long ArgPano = 0)
        {
            int nRead = 0;
            int nRead2 = 0;
            int nRow = 0;
            int nRow1 = 0;
            int nRow2 = 0;

            string strPart = "";
            string strJong = "";
            string strOldHaRoom = "";
            string strChk1 = "";    //일검

            bool bColor = false;

            string strExams = "";
            string strTemp = "";

            string strGbWait = "";

            string strOK = "";
            string strExName = "";

            bool boolSort = false;

            sp.Spread_All_Clear(ssChk);
            ssChk.ActiveSheet.RowCount = 20;

            if (clsHcVariable.GstrHicPart == "2")   //일검
            {
                if (actingCheckService.GetCodebyWrtno(ArgWRTNO) == "3116")
                {
                    strChk1 = "OK";
                }
            }

            strPart = dtpFDate.Text.Left(1);

            if (clsHcVariable.GstrHicPart == "1")   //종검
            {
                if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
                {
                    List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK(ArgWRTNO, ArgDate);

                    nREAD = list.Count;
                    ssChk.ActiveSheet.RowCount = nREAD;
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            nRow1 += 1;
                            nRow += 1;

                            if (i == 0)
                            {
                                ssChk.ActiveSheet.Cells[i, 6].Text = list[i].HAROOM;
                                strOldHaRoom = list[i].HAROOM;
                                nRow2 = nRow;
                            }

                            ssChk.ActiveSheet.Cells[nRow, 6].Text = list[i].HAROOM;
                            if (strOldHaRoom != list[i].HAROOM)
                            {
                                strOldHaRoom = list[i].HAROOM;
                                nRow2 = nRow;
                                if (bColor == true)
                                {
                                    bColor = false;
                                }
                                else
                                {
                                    bColor = true;
                                }
                            }

                            ssChk.ActiveSheet.Cells[nRow, 0].Text = list[i].NAME;
                            if (bColor == true)
                            {
                                ufn_Line_Color();
                            }
                            //상태점검
                            List<HIC_RESULT_ACTIVE> Rsltlist = hicResultActiveService.Read_Result(FnWRTNO, FstrRoom);

                            if (Rsltlist.Count == 0)
                            {
                                List<HIC_RESULT_ACTIVE> Actlist2 = hicResultActiveService.Read_Active(FnWRTNO, FstrRoom);
                                if (Actlist2.Count > 0)
                                {
                                    ssChk.ActiveSheet.Cells[nRow, 1].Text = "미검";
                                    ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 0, 0);
                                }
                                else
                                {
                                    ssChk.ActiveSheet.Cells[nRow, 1].Text = "완료";
                                    ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(0, 0, 0);
                                }
                            }
                            else
                            {
                                List<HIC_RESULT_ACTIVE> Rsltlist2 = hicResultActiveService.Read_Result2(FnWRTNO, FstrRoom);

                                if (Rsltlist2.Count > 0)
                                {
                                    ssChk.ActiveSheet.Cells[nRow, 1].Text = "미검";
                                    ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 0, 0);
                                }
                                else
                                {
                                    ssChk.ActiveSheet.Cells[nRow, 1].Text = "완료";
                                    ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(0, 0, 0);
                                }
                            }

                            if (ArgWRTNO == 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 1].Text = "";
                            }

                            //대기점검
                            List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait(ArgDate, FstrRoom);

                            nRead2 = Waitlist.Count;

                            if (nRead2 > 0)
                            {
                                //nRead2 = nRead2;
                            }

                            if (nRead2 == 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 2].Text = "0";
                            }
                            else
                            {
                                ssChk.ActiveSheet.Cells[nRow, 2].Text = nRead2.ToString();
                            }

                            //검사실 대기인원
                            HEA_SANGDAM_WAIT ExamWaitlist = waitCheckService.Read_Exam_Wait(ArgDate, FstrRoom);

                            if (ExamWaitlist.RID != null)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 3].Text = ExamWaitlist.CNT;

                                if (list[i].HAROOM == "99")
                                {
                                    ssChk.ActiveSheet.Cells[nRow, 3].Text = "";
                                }
                            }
                            ssChk.ActiveSheet.Cells[nRow, 7].Text = list[i].HAROOM;

                            if (strOldHaRoom != list[i].HAROOM.Trim())
                            {
                                if (nRow1 > 0)
                                {
                                    ssChk.ActiveSheet.AddSpanCell(nRow2, 3, nRow1, 1);
                                    ssChk.ActiveSheet.AddSpanCell(nRow2, 4, nRow1, 1);
                                    ssChk.ActiveSheet.AddSpanCell(nRow2, 5, nRow1, 1);
                                }
                                nRow1 = 0;
                            }
                        }

                        //현 검사실 대기명단
                        List<HEA_SANGDAM_WAIT> NowExamWaitlist = heaSangdamWaitService.Read_Now_Wait(ArgDate, FstrRoom);

                        if (NowExamWaitlist.Count > 0)
                        {
                            for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                            {
                                ssChk.ActiveSheet.Cells[i, 5].Text = NowExamWaitlist[i].SNAME;
                                ssChk.ActiveSheet.Cells[i, 8].Text = NowExamWaitlist[i].WRTNO.ToString();
                            }
                        }
                    }
                }
                else  // 종검 접수 계측현황 조회
                {
                    tabControl1.TabIndex = 1;
                    tabControl1.SelectedTab = tabChk;

                    List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_ALL(ArgWRTNO);

                    nRead = list.Count;
                    ssChk.ActiveSheet.RowCount = list.Count;
                    nRow = 0;
                    //미검 항목을 Display
                    for (int i = 0; i < nRead; i++)
                    {
                        strOK = "";
                        if (list[i].RESULT.Trim().IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }
                        if (strOK == "OK")
                        {
                            strExName = list[i].HNAME.Trim();
                            strExName = strExName.Replace("(ACT)", "");
                            nRow += 1;
                            ssChk.ActiveSheet.Cells[nRow, 0].Text = strExName;
                            ssChk.ActiveSheet.Cells[nRow, 1].Text = "미검";
                            ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }

                    //검사완료
                    for (int i = 0; i < nRead - 1; i++)
                    {
                        strOK = "";
                        if (!list[i].HNAME.Trim().IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }

                        if (strOK == "OK")
                        {
                            strExName = list[i].HNAME.Trim();
                            strExName = strExName.Replace("(ACT)", "");

                            nRow += 1;
                            ssChk.ActiveSheet.Cells[nRow, 0].Text = strExName;

                            ssChk.ActiveSheet.Cells[nRow, 1].Text = "완료";
                            ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    lblWrtNo.Text = "접수번호: " + string.Format("{0:###0}", FnWRTNO);

                    //대기점검
                    List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait_All(ArgDate);

                    nRead2 = Waitlist.Count;

                    if (nRead2 == 0)
                    {
                        ssChk.ActiveSheet.Cells[nRow, 2].Text = "0";
                    }
                    else
                    {
                        ssChk.ActiveSheet.Cells[nRow, 2].Text = nRead2.ToString();
                    }

                    //2차정밀청력 대상자 표시
                    if (hicSunapService.GetSunapAmtbyWrtNo(ArgWRTNO) > 0)
                    {
                        if (hicResultService.GetExCodebyWrtNo(ArgWRTNO) > 0)
                        {
                            if (hicResultService.GetExCodebyWrtNo_Second(ArgWRTNO) > 0)
                            {
                                ssChk.ActiveSheet.RowCount += 1;
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "2차 정밀청력 수납대상";
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "미검";
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].ForeColor = Color.FromArgb(255, 0, 0);
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].BackColor = Color.FromArgb(255, 255, 255);
                            }
                        }
                    }

                    //미검을 상단에 표시
                    clsSpread.gSpdSortRow(ssChk, 1, ref boolSort, true);
                }
            }
            else if (clsHcVariable.GstrHicPart == "2")   //일검
            {
                string strGB = "";

                if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
                {
                    if (rdoJepsuGubun2.Checked == true)
                    {
                        strGB = "1";
                    }
                    else if (rdoJepsuGubun3.Checked == true)
                    {
                        strGB = "2";
                    }
                    else
                    {
                        strGB = "";
                    }

                    List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_HIC(ArgWRTNO, ArgDate, ArgPano, strChk1);

                    nREAD = list.Count;
                    ssChk.ActiveSheet.RowCount = nREAD;
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            nRow1 += 1;
                            nRow += 1;

                            ssChk.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                            ssChk.ActiveSheet.Cells[i, 3].Text = list[i].ENTPART;

                            //청력(특수) 및 폐기능검사 대기인원수 표시
                            strGbWait = "";
                            if (list[i].NAME == "청력(특수)") strGbWait = "'12', '13'";
                            if (list[i].NAME == "폐활량") strGbWait = "'06', '07'";
                            if (list[i].NAME == "자궁암검사") strGbWait = "'124'";

                            //상태점검

                            ssChk.ActiveSheet.Cells[nRow, 0].Text = list[i].NAME;
                            if (bColor == true)
                            {
                                ufn_Line_Color();
                            }
                            //상태점검
                            List<HIC_RESULT_ACTIVE> Actlist = hicResultActiveService.Read_Active_Hic(FnWRTNO, ArgPano, FstrRoom);
                            if (Actlist.Count > 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 1].Text = "미검";
                                ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(255, 0, 0);
                                ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(255, 255, 255);
                            }
                            else
                            {
                                ssChk.ActiveSheet.Cells[nRow, 1].Text = "완료";
                                ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(0, 0, 0);
                                ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 255, 255);
                            }

                            if (ArgWRTNO == 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 1].Text = "";
                            }

                            //대기점검
                            List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait_Hic(ArgDate, FstrRoom, strGB);

                            nRead2 = Waitlist.Count;

                            if (nRead2 == 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 2].Text = "0";
                            }

                            if (nRead2 == 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow, 2].Text = nRead2.ToString();
                            }

                            //청력(특수) 및 폐기능검사 대기인원수를 표시함
                            ssChk.ActiveSheet.Cells[i, 2].Text = waitCheckService.Read_Exam_Wait_Hic(FstrRoom).ToString();
                        }
                    }
                }
                else  // 일검 접수 계측현황 조회
                {
                    tabControl1.TabIndex = 1;
                    tabControl1.SelectedTab = tabChk;

                    List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_ALL(ArgWRTNO);

                    nRead = list.Count;
                    ssChk.ActiveSheet.RowCount = list.Count;
                    nRow = 0;
                    //미검 항목을 Display
                    for (int i = 0; i < nRead; i++)
                    {
                        strOK = "";
                        if (list[i].RESULT.Trim().IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }
                        if (strOK == "OK")
                        {
                            strExName = list[i].HNAME.Trim();
                            strExName = strExName.Replace("(ACT)", "");
                            nRow += 1;
                            ssChk.ActiveSheet.Cells[nRow, 0].Text = strExName;
                            ssChk.ActiveSheet.Cells[nRow, 1].Text = "미검";
                            ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }

                    //검사완료
                    for (int i = 0; i < nRead - 1; i++)
                    {
                        strOK = "";
                        if (!list[i].HNAME.Trim().IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }

                        if (strOK == "OK")
                        {
                            strExName = list[i].HNAME.Trim();
                            strExName = strExName.Replace("(ACT)", "");

                            nRow += 1;
                            ssChk.ActiveSheet.Cells[nRow, 0].Text = strExName;

                            ssChk.ActiveSheet.Cells[nRow, 1].Text = "완료";
                            ssChk.ActiveSheet.Cells[nRow, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                    lblWrtNo.Text = "접수번호: " + string.Format("{0:###0}", FnWRTNO);

                    //대기점검
                    List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait_All(ArgDate);

                    nRead2 = Waitlist.Count;

                    if (nRead2 == 0)
                    {
                        ssChk.ActiveSheet.Cells[nRow, 2].Text = "0";
                    }
                    else
                    {
                        ssChk.ActiveSheet.Cells[nRow, 2].Text = nRead2.ToString();
                    }

                    //2차정밀청력 대상자 표시
                    if (hicSunapService.GetSunapAmtbyWrtNo(ArgWRTNO) > 0)
                    {
                        if (hicResultService.GetExCodebyWrtNo(ArgWRTNO) > 0)
                        {
                            if (hicResultService.GetExCodebyWrtNo_Second(ArgWRTNO) > 0)
                            {
                                ssChk.ActiveSheet.RowCount += 1;
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "2차 정밀청력 수납대상";
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "미검";
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].ForeColor = Color.FromArgb(255, 0, 0);
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].BackColor = Color.FromArgb(255, 255, 255);
                            }
                        }
                    }

                    //미검을 상단에 표시
                    clsSpread.gSpdSortRow(ssChk, 1, ref boolSort, true);
                }
            }
        }

        void ufn_Line_Color()
        {
            ssChk.ActiveSheet.Cells[nRow, 0].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[nRow, 1].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[nRow, 2].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[nRow, 3].BackColor = Color.FromArgb(255, 255, 128);
        }

        void Work_BP_Control()
        {
            long lngHandle = 0;
            long chlHandle = 0;
            long chlHandle2 = 0;
            long hCmd;
            string strWRTNO= "";

            lngHandle = FindWindow("TMain", "Member's information and Results.");
            if (lngHandle == 0)
            {
                return;
            }

            strWRTNO = string.Format("0:00000000", txtWrtNo.Text);

            if (lngHandle > 0)
            {
                chlHandle = FindWindowEx(lngHandle, 0, "TEdit", null);
                chlHandle2 = FindWindowEx(lngHandle, chlHandle, "TEdit", null);

                if (chlHandle2 > 0)
                {
                    WindowTextSet(chlHandle2, strWRTNO);
                }
            }

            //핸들의 값이 없다면 신규
            if (GetText(chlHandle).IsNullOrEmpty())
            {
                //'ID-NO
                if (chlHandle > 0)
                {
                    WindowTextSet(chlHandle, strWRTNO);
                }

                //7번째 핸들위치
                chlHandle2 = FindWindowEx(lngHandle, chlHandle, "TEdit", null);
                for (int i = 1; i < 6; i++)
                {
                    chlHandle2 = FindWindowEx(lngHandle, chlHandle2, "TEdit", null);
                }

                if (chlHandle2 > 0)
                {
                    WindowTextSet(chlHandle2, VB.Right(strWRTNO, 5));
                }


                //'16번째 핸들위치
                chlHandle2 = FindWindowEx(lngHandle, 0, "TPanel", null);
                for (int i = 1; i < 15; i++)
                {
                    chlHandle2 = FindWindowEx(lngHandle, chlHandle2, "TPanel", null);
                }

                hCmd = FindWindowEx(chlHandle2, 0, "TBitBtn", "Insert");  //## 버튼 핸들값 취득

                if (hCmd > 0)
                {
                    PostMessage(hCmd, WM_COMMAND, "0&", hCmd);
                }

                timer1.Enabled = true;
            }
        }

        /// <summary>
        /// 핸들의 캡션을 읽어오는 함수
        /// </summary>
        /// <param name="lngHwnd"></param>
        /// <returns></returns>
        string GetText(long lngHwnd)
        {
            string rtnVal = "";
            long nlngLen = 0;
            string strBuf = "";

            if (lngHwnd > 0)
            {
                nlngLen = SendMessage(lngHwnd, WM_GETTEXTLENGTH, 0, "0");
                if (nlngLen != 0)
                {
                    nlngLen += 1;
                    strBuf = VB.Space(Convert.ToInt32(nlngLen));
                    nlngLen = SendMessage(lngHwnd, WM_GETTEXT, nlngLen, strBuf);
                    rtnVal = VB.Left(strBuf, Convert.ToInt32(nlngLen));
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 종합검진+일특 누락방지
        /// </summary>
        void ufn_Screen_SpcExam_Display()
        {
            //배치전 소음
            List<HIC_SUNAPDTL> UCodelist = hicSunapdtlService.Read_UCode(FstrPtno, strJepDate);

            for (int i = 0; i < 6; i++)
            {
                strSpcExam[i] = "";
            }

            for (int i = 0; i < UCodelist.Count; i++)
            {
                switch (UCodelist[i].UCODE)
                {
                    case "L06":
                    case "L07":
                        strSpcExam[5] = "N";    //채용소음
                        break;
                    default:
                        break;
                }
            }

            //일반건진의 내용 점검
            List<HIC_RESULT> RsltList = hicResultService.Read_Result(FstrPtno, strJepDate);

            if (RsltList.Count > 0)
            {
                for (int i = 0; i < RsltList.Count; i++)
                {
                    switch (RsltList[i].EXCODE)
                    {
                        case "MU11":
                        case "MU15":
                        case "MU12":
                        case "MU74":
                            strSpcExam[0] = "N"; //소변(특수)
                            break;
                        case "LM10":
                            strSpcExam[1] = "N"; //객담
                            break;
                        case "TR11":
                            strSpcExam[2] = "N"; //폐활량3회
                            break;
                        case "TH12":
                        case "TH22":
                            strSpcExam[3] = "N"; //기도청력
                            break;
                        case "TZ08":
                            strSpcExam[4] = "N"; //진동
                            break;
                        default:
                            break;
                    }
                }

                //액팅여부
                for (int i = 0; i < RsltList.Count; i++)
                {
                    if (!RsltList[i].RESULT.IsNullOrEmpty())
                    {
                        switch (RsltList[i].EXCODE)
                        {
                            case "A903":    //특수소변
                                if (strSpcExam[0] == "N")
                                {
                                    strSpcExam[0] = "Y";
                                }
                                break;
                            case "A902":    //객담
                                if (strSpcExam[1] == "N")
                                {
                                    strSpcExam[1] = "Y";
                                }
                                break;
                            case "A920":    //폐활량3회
                                if (strSpcExam[2] == "N")
                                {
                                    strSpcExam[2] = "Y";
                                }
                                break;
                            case "TH12":    //특수소음
                            case "TH22":    //특수소음
                                strSpcExam[3] = "Y";
                                break;
                            case "TZ08":    //진동
                                strSpcExam[4] = "Y";
                                break;
                            case "TH16":    //채용소음
                            case "TH26":    //채용소음
                                strSpcExam[5] = "Y";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //종검에서 검사여부
            List<HIC_RESULT> RsltList2 = hicResultService.Read_Result2(FnWRTNO);

            //액팅여부
            for (int i = 0; i < RsltList2.Count; i++)
            {
                switch (RsltList2[i].EXCODE)
                {
                    case "A903":    //특수소변
                        if (strSpcExam[0] == "N")
                        {
                            strSpcExam[0] = "Y";
                        }
                        break;
                    case "A902":    // 객담
                        if (strSpcExam[1] == "N")
                        {
                            strSpcExam[1] = "Y";
                        }
                        break;
                    case "A920":    // 폐활량3회
                        if (strSpcExam[2] == "N")
                        {
                            strSpcExam[2] = "Y";
                        }
                        break;
                    case "TZ08":    //진동
                        if (strSpcExam[4] == "N")
                        {
                            strSpcExam[4] = "Y";
                        }
                        break;
                    default:
                        break;
                }
            }

            if (!strSpcExam[5].IsNullOrEmpty())
            {
                strSpcExam[3] = "";
            }

            if (!strSpcExam[0].IsNullOrEmpty() || !strSpcExam[1].IsNullOrEmpty() || !strSpcExam[2].IsNullOrEmpty() || !strSpcExam[3].IsNullOrEmpty() || !strSpcExam[4].IsNullOrEmpty() || !strSpcExam[5].IsNullOrEmpty())
            {
                strTemp = "■일특:";
                for (int i = 0; i < 6; i++)
                {
                    if (!strSpcExam[i].IsNullOrEmpty())
                    {
                        ssChk.ActiveSheet.RowCount += 1;
                        switch (i)
                        {
                            case 0:
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 0].Text = "특수 소변";
                                strTemp += "특수소변,";
                                break;
                            case 1:
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 0].Text = "객담";
                                strTemp += "객담,";
                                break;
                            case 2:
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 0].Text = "폐활량 3회";
                                strTemp += "폐활량 3회,";
                                break;
                            case 3:
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 0].Text = "특수 소음";
                                strTemp += "특수 소음,";
                                break;
                            case 4:
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 0].Text = "진동";
                                strTemp += "진동,";
                                break;
                            case 5:
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 0].Text = "채배 소음";
                                strTemp += "채배 소음,";
                                break;
                            default:
                                break;
                        }
                        ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 1].Text = "";
                        if (strSpcExam[i] == "Y")
                        {
                            ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount, 1].Text = "완료";
                        }
                    }
                }
            }
        }

        void Chk_InBody()
        {
            long lngHandle;

            lngHandle = FindWindow("WindowsForms10.Window.8.app.0.2bf8098_r11_ad1", "Lookin'Body");

            if (lngHandle > 0)
            {
                InBodyVer = "신버전1";
            }
            else
            {
                lngHandle = FindWindow("WindowsForms10.Window.8.app.0.141b42a_r12_ad1", "Lookin'Body");

                if (lngHandle > 0)
                {
                    InBodyVer = "신버전";
                }
            }

            if (InBodyVer.IsNullOrEmpty())
            {
                MessageBox.Show("인바디 프로그램 실행 안됨!! 확인요망합니다!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void BarCode_AutoPrint()
        {
            List<HIC_BCODE> list = hicBcodeService.BarCodeAutoPrintSet(clsCompuInfo.gstrCOMIP);
            if (list.Count > 0)
            {
                FbExamBarCodeReq = true;
            }
            else
            {
                FbExamBarCodeReq = false;
            }
        }

        void menuAutoActing_Click()
        {
            long nREAD = 0;
            long nCNT = 0;
            long nWRTNO = 0;
            string strPtno = "";
            string strExCode = "";
            string strData = "";
            string strTemp = "";
            string strOK = "";
            int nACT_SET_CNT = 0;
            string[] strACT_SET_Code = new string[100];
            string[] strACT_SET_DB = new string[100];
            string[] strACT_SET_Data = new string[100];

            timerAutoActing.Enabled = false;

            for (int i = 0; i < 100; i++)
            {
                strACT_SET_Code[i] = "";
                strACT_SET_DB[i] = "";
                strACT_SET_Data[i] = "";
            }

            //----------------------------------------------
            //  자동액팅 설정값을 읽어 변수에 저장함
            //----------------------------------------------
            List<HIC_BCODE> list = hicBcodeService.Read_Code_All("HEA_종검자동액팅", "");

            nREAD = list.Count;
            nACT_SET_CNT = 0;
            for (int i = 0; i < nREAD; i++)
            {
                strTemp = list[i].NAME.Trim();
                if (!VB.Pstr(strTemp, "{}", 1).Trim().IsNullOrEmpty() && !VB.Pstr(strTemp, "{}", 1).IsNullOrEmpty())
                {
                    nACT_SET_CNT += 1;
                    strACT_SET_Code[nACT_SET_CNT] = VB.Pstr(strTemp, "{}", 1);
                    strACT_SET_DB[nACT_SET_CNT] = VB.Pstr(strTemp, "{}", 2);
                    strData = VB.Pstr(strTemp, "{}", 3);
                    nCNT = VB.L(strData, ";");
                    strTemp = "";
                    for (int j = 0; j < nCNT; j++)
                    {
                        if (!VB.Pstr(strData, ";", j).IsNullOrEmpty())
                        {
                            strTemp += "'" + VB.Pstr(strData, ";", j) + "',";
                        }
                    }

                    if (!strTemp.IsNullOrEmpty())
                    {
                        strTemp = VB.Left(strTemp, strTemp.Length - 1);
                        strACT_SET_Data[nACT_SET_CNT] = strTemp;
                    }
                }
            }

            //----------------------------------------------
            //  액팅 항목중 미액팅만 검색함
            //----------------------------------------------
            List<HIC_JEPSU_RESULT> listAct = hicJepsuResultService.GetNoActingbyWrtNo(FnWRTNO);
            if (listAct != null && listAct.Count > 0)
            {
                nREAD = listAct.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = listAct[i].WRTNO;
                    strPtno = listAct[i].PTNO;
                    strExCode = listAct[i].EXCODE;

                    //신장+체중(ACT) 또는 체성분(ACT)
                    if (strExCode == "A918" || strExCode == "A919")
                    {
                        if (hicResultService.GetResultCount_ACT(nWRTNO) > 0)
                        {
                            fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                        }
                    }

                    strOK = "";
                    for (int j = 1; j <= nACT_SET_CNT; j++)
                    {
                        if (strACT_SET_Code[j] == strExCode)
                        {
                            strOK = "OK";
                            if (strACT_SET_DB[j] == "1")
                            {
                                if (comHpcLibBService.GetSpecNobyPtNo(strPtno, strACT_SET_Data[j]) > 0)
                                {
                                    fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                                }
                            }
                        }
                        else if (strACT_SET_DB[j] == "2")
                        {
                            if (comHpcLibBService.GetXRaybyPaNo(strPtno, strACT_SET_Data[j]) > 0)
                            {
                                fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j] == "3")
                        {
                            if (comHpcLibBService.GetEndobyPaNo(strPtno, strACT_SET_Data[j]) > 0)
                            {
                                fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j] == "4")
                        {
                            if (comHpcLibBService.GetEtcJupMstbyPaNo(strPtno, strACT_SET_Data[j]) > 0)
                            {
                                fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j] == "5")
                        {
                            if (comHpcLibBService.GetHicResultbyWrtNo(nWRTNO, strACT_SET_Data[j]) > 0)
                            {
                                fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j] == "6")
                        {
                            if (comHpcLibBService.GetHeaSangdamWaitbyWrtNo(nWRTNO, strACT_SET_Data[j]) > 0)
                            {
                                fn_HIC_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                    }
                }
            }
            FnActWrtno = 0;
            FnTimer = 0;
            timerAutoActing.Enabled = true;
            txtWrtNo.Focus();
        }

        /// <summary>
        /// HIC_Result에 자동엑팅 업데이트
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argExCode"></param>
        /// <param name="argResult"></param>
        void fn_HIC_Auto_Acting(long argWrtNo, string argExCode, string argResult)
        {
            int result = hicResultHisService.Result_History_Insert2(clsPublic.GstrJobSabun, strResult.Replace("'", "`"), argWrtNo, argExCode);

            if (result < 0)
            {
                MessageBox.Show("자동 액팅중(History) 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int result1 = hicResultHisService.Result_Update(strResult.Replace("'", "`"), strPanjeng, "X05", clsPublic.GstrJobSabun, argExCode);

            if (result1 < 0)
            {
                MessageBox.Show("자동 액팅중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 당일검진_자동액팅()
        /// </summary>
        /// <param name="argBDate"></param>
        void fn_TodayHic_AutoAction(string argBDate)
        {
            int nREAD;
            int nREAD2;
            string strROWID;
            string strPtNo;
            long nPano;
            string strJepDate;
            string strActExCode;
            string strExCode;
            string strResCode;
            string strActGbn;
            string strActOK;
            string strActValue;
            long nWRTNO;

            List<HIC_JEPSU_RESULT> list = hicJepsuResultService.GetActingInfobyBDate(argBDate);

            nREAD = list.Count;

            for (int i = 0; i < nREAD; i++)
            {
                strROWID = list[i].RID;
                strPtNo = list[i].PTNO;
                strJepDate = list[i].JEPDATE;
                nWRTNO = list[i].WRTNO;
                nPano = list[i].PANO;

                strExCode = hcact.READ_ExamCode2XrayCode(list[i].CODE.Trim());       //검사코드
                strActExCode = list[i].EXCODE.Trim();   //액팅코드
                strResCode = list[i].RESCODE.Trim();
                strActGbn = hcact.READ_ActingCodeGubun(list[i].CODE.Trim());

                strActOK = "";
                switch (strActGbn)
                {
                    case "방사선":
                        if (hicXrayResultService.GetXrayCountByPtnoJepDate(strPtNo, strJepDate, DateTime.Parse(strJepDate).AddDays(1).ToShortDateString(), strExCode) > 0)
                        {
                            strActOK = "OK";
                        }
                        break;
                    case "위내시경":
                        if (hicXrayResultService.GetEndoCountByPtnoJepDate(strPtNo, strJepDate, DateTime.Parse(strJepDate).AddDays(1).ToShortDateString(), "2") > 0)
                        {
                            strActOK = "OK";
                        }
                        break;
                    case "대장내시경":
                        if (hicXrayResultService.GetEndoCountByPtnoJepDate(strPtNo, strJepDate, DateTime.Parse(strJepDate).AddDays(1).ToShortDateString(), "3") > 0)
                        {
                            strActOK = "OK";
                        }
                        break;
                    default:
                        break;
                }

                strActValue = "01";

                if (strActOK == "OK" && !strActValue.IsNullOrEmpty() && !strROWID.IsNullOrEmpty())
                {
                    int result = hicResultService.Update_Auto_Result(strActValue, long.Parse(clsType.User.IdNumber), nWRTNO, strROWID);

                    if (result < 0)
                    {
                        MessageBox.Show("당일 검사결과 전송시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //접수마스타의 상태를 변경
                hm.Result_EntryEnd_Check(nWRTNO);
            }
            //----------------------------------
            //  출장검진 채혈 자동 액팅
            //----------------------------------
            List<HIC_JEPSU_RESULT> listAuto = hicJepsuResultService.GetActingInfobyBDate_Chul(argBDate);

            nREAD = listAuto.Count;

            for (int i = 0; i < nREAD; i++)
            {
                strROWID = listAuto[i].RID;
                strPtNo = listAuto[i].PTNO;
                strJepDate = listAuto[i].JEPDATE;
                nWRTNO = listAuto[i].WRTNO;
                nPano = listAuto[i].PANO;

                //혈액검사 결과 여부
                HIC_RESULT lst = hicResultService.GetResultCount_Blood(nWRTNO);
                
                if (lst.CNT > 0)
                {
                    int result = hicResultService.Update_Auto_Result("01", long.Parse(clsPublic.GstrJobSabun), nWRTNO, strROWID);

                    if (result < 0)
                    {
                        MessageBox.Show("검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //접수마스타의 상태를 변경
                    hm.Result_EntryEnd_Check(nWRTNO);
                }
            }
        }

        /// <summary>
        /// 당일검진결과_UPDATE()
        /// </summary>
        /// <param name="argDate"></param>
        void fn_TodayHic_Result_Update(string argDate)
        {
            int nREAD = 0;
            long nWRTNO = 0;
            string strPtNo = "";
            string strResult = "";
            List<string> strExcode = new List<string>();

            List<HIC_JEPSU> lst = hicJepsuService.GetJepsuInfobyJepDate(argDate);

            nREAD = lst.Count;

            for (int i = 0; i < nREAD; i++)
            {
                FstrJepDate = lst[i].JEPDATE;
                FstrGjYear = lst[i].GJYEAR;
                FstrSex = lst[i].SEX;
                FnAge = lst[i].AGE;
                FstrGbChul = lst[i].GBCHUL;
                nWRTNO = lst[i].WRTNO;
                strPtNo = string.Format("{0:00000000}", lst[i].PTNO);

                //검사결과 완료 Flag 자동등록
                //구강검사(ZD00),(ZD01) 누락자는 "." 찍기
                if (hicResultService.GetResultCount_Flag(nWRTNO) > 0)
                {
                    int result = hicResultService.Update_Result_Flag(long.Parse(clsType.User.IdNumber), nWRTNO);

                    if (result < 0)
                    {
                        MessageBox.Show("검사결과 일괄등록중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //출장검진은 체혈,X-Ray,폐활량 액팅 자동으로 달기
                if (FstrGbChul == "Y")
                {
                    if (hicResultService.GetResultCount_ChulAutFlag(nWRTNO) > 0)
                    {
                        string[] strCodes = new string[] { "A135", "A136", "A899", "A902" };

                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = "01";
                        item.ACTIVE = "Y";
                        item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                        item.WRTNO = nWRTNO;

                        int result = hicResultService.Update_Result_ChulAutoFlag(item, strCodes);

                        if (result < 0)
                        {
                            MessageBox.Show("검사결과 일괄등록중 오류 발생! (출장체혈)", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                List<XRAY_RESULTNEW> listRslt = xrayResultnewService.GetResultNewbyPano(strPtNo, argDate);

                if (listRslt.Count == 1)
                {
                    if (string.Compare(listRslt[0].RESULT, "-1") >= 0)
                    {
                        strResult = "01";
                    }
                    else if (string.Compare(listRslt[0].RESULT, "-1.1") >= 0 && string.Compare(listRslt[0].RESULT, "-2.4") >= 0)
                    {
                        strResult = "02";
                    }
                    else if (string.Compare(listRslt[0].RESULT, "-2.5") <= 0)
                    {
                        strResult = "03";
                    }
                    //골밀도검사 업데이트
                    if (hicResultService.GetResultCount_BMD(nWRTNO) > 0)
                    {
                        //결과를 저장
                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = strResult;
                        item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                        item.WRTNO = nWRTNO;
                        strExcode.Clear();
                        strExcode.Add("TX07");

                        int result = hicResultService.Update_ResultbyWrtNo(item, strExcode, "");

                        if (result < 0)
                        {
                            MessageBox.Show("검사결과 등록중 오류 발생! (골밀도검사)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                //비만도 계산 및 Update                   
                hm.Biman_Gesan(FnWRTNO);    //체질량 자동계산 A117
                hm.Update_Audiometry(FnWRTNO);  //기도청력 시 기본청력 정상입력
                //hm.GFR_Gesan_Life(FnWRTNO, FstrSex, FnAge); //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                hm.MDRD_GFR_Gesan(FnWRTNO, FstrSex, FnAge);  //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                hm.AIR3_AUTO(FnWRTNO);                      //AIR 3분법 자동계산
                hm.LDL_Gesan(FnWRTNO);                      //LDL콜레스테롤 계산
                hm.TIBC_Gesan(FnWRTNO);                     //TIBC총철결합능 계산

                //접수마스타의 상태를 변경
                hm.Result_EntryEnd_Check(FnWRTNO);
            }
        }

        /// <summary>
        /// 건진접수증()
        /// </summary>
        /// <param name="nWrtNo"></param>
        /// <param name="nPano"></param>
        void fn_HcJepsuReport(long nWrtNo, long nPano)
        {
            int nCNT = 0;
            int nUA검사 = 0;
            long nWRTNO1 = 0;
            string strGjyear = "";
            string strJepDate = "";
            string strGjJong = "";
            string strOK = "";
            string strSecondSayu = "";
            string strTemp = "";
            string[] strUA검사구분 = new string[5];

            long x = 0;
            long y = 0;
            string[] strGel = new string[25];

            for (int i = 0; i <= 5; i++)
            {
                strUA검사구분[i] = "";
            }

            //바코드 접수증 출력
            //Set Printer = Printers(clsHcVariable.GnPrtNo2) '신용카드

            //Printer.FontName = "3 of 9 Barcode"   '바코드 폰트 (window 폰트디릭토리에 3 of 9 barcode 화일이 설치되어야함).
            //x = 200: y = 1100 '기본
            //Call Text_Print(Printer.FontName, 20, "", 0, 0, "*" & Trim(ArgWRTNO) & "*")   '바코드 라인 인쇄-1
            //Call Text_Print(Printer.FontName, 20, "", 280, 0, "*" & Trim(ArgWRTNO) & "*") '바코드 라인 인쇄-2
            //' 이름(성별/나이)
            //Call Text_Print("맑은 고딕 Bold", 15, "", 0, 2000, Trim(P(GstrRetValue, "^^", 1)) & "(" & Trim(P(GstrRetValue, "^^", 2)) & "/" & Trim(P(GstrRetValue, "^^", 3)) & ")")
            //Call Text_Print("굴림체", 10, "", 400, 2300, Trim(P(GstrRetValue, "^^", 5))) ' 주민번호


            //'첫번째 검사 항목은 강제 출력 세팅
            //Call Text_Print("굴림체", 10, "", 750, 2050, Trim(P(FstrRetValue, "^^", 1)))  ' 검사항목1

            //nCNT = L(FstrRetValue, "^^")
            //For i = 2 To nCNT
            //    Call Text_Print("굴림체", 10, "", y, x, Trim(P(FstrRetValue, "^^", i)))  ' 검사항목2
            //    x = x + (((LenH(Trim(P(FstrRetValue, "^^", i))) / 2) * 200) + 100)
            //    If x > 3600 Then
            //        y = y + 350
            //        x = 200
            //    End If
            //Next i


            //nCNT = 0: nUA검사 = 0


            //'UA4종 검사여부 체크
            //SQL = " SELECT EXCODE FROM HIC_RESULT WHERE WRTNO = " & ArgWRTNO & " "
            //Call AdoOpenSet(Rs, SQL)
            //For i = 0 To RowIndicator -1
            //    Select Case Trim(AdoGetString(Rs, "EXCODE", i))
            //        Case "A111": nUA검사 = nUA검사 + 1
            //        Case "A112": nUA검사 = nUA검사 + 1
            //        Case "A113": nUA검사 = nUA검사 + 1
            //        Case "A114": nUA검사 = nUA검사 + 1
            //        Case "LU38": nUA검사 = nUA검사 + 1
            //        Case "LU54": strUA검사구분(1) = "UA10종"    'LU54 한개만 있으면 UA10종
            //        Case "A236": strUA검사구분(2) = "코티닌"    '코티닌 검사
            //        Case "TZ46": strUA검사구분(2) = "코티닌"    '코티닌 검사
            //        Case "E923": strUA검사구분(3) = "TBPE"      'TBPE 검사
            //    End Select
            //Next i
            //Call AdoCloseSet(Rs)


            //If nUA검사 = 4 Then strUA검사구분(0) = "UA4종"

            //'---------------------------------
            //'    특수2차 여부
            //'---------------------------------
            //SQL = "SELECT GjYear,GjJong,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate FROM HIC_JEPSU "
            //SQL = SQL & "WHERE WRTNO=" & ArgWRTNO & " "
            //Call AdoOpenSet(rs1, SQL)
            //strGjyear = "": strJepDate = "": strGjJong = "": strOK = ""
            //If RowIndicator > 0 Then
            //    strGjyear = Trim(AdoGetString(rs1, "GjYear", 0))
            //    strGjJong = Trim(AdoGetString(rs1, "GjJong", 0))
            //    strJepDate = Trim(AdoGetString(rs1, "JepDate", 0))
            //End If
            //Call AdoCloseSet(rs1)
            //strSecondSayu = ""
            //If strGjyear<> "" Then
            //   SQL = "SELECT WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate FROM HIC_JEPSU "
            //    SQL = SQL & "WHERE Pano=" & ArgPano & " "
            //    SQL = SQL & "  AND GjYear='" & strGjyear & "' "
            //    If strGjJong >= "16" And strGjJong <= "19" Then
            //        SQL = SQL & "  AND GjJong IN ('11','12','13','14') " '1차검진
            //    Else
            //        SQL = SQL & "  AND GjJong IN ('21','22','23','24','25','26') " '1차검진
            //    End If
            //    SQL = SQL & "  AND JepDate<TO_DATE('" & strJepDate & "','YYYY-MM-DD') "
            //    SQL = SQL & "  AND DelDate IS NULL "
            //    SQL = SQL & "ORDER BY JepDate DESC "
            //    Call AdoOpenSet(rs1, SQL)
            //    nWRTNO1 = 0
            //    If RowIndicator > 0 Then nWRTNO1 = AdoGetNumber(rs1, "WRTNO", 0)
            //    Call AdoCloseSet(rs1)


            //    If nWRTNO1 > 0 Then
            //        SQL = "SELECT COUNT(*) CNT FROM HIC_SPC_PANJENG "
            //        SQL = SQL & "WHERE WRTNO=" & nWRTNO1 & " "
            //        SQL = SQL & "  AND Panjeng='7' " 'R판정
            //        Call AdoOpenSet(rs1, SQL)
            //        If AdoGetNumber(rs1, "CNT", 0) > 0 Then strOK = "OK"
            //        Call AdoCloseSet(rs1)
            //    End If


            //    '2차재검 사유를 읽음
            //    SQL = "SELECT WRTNO,TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,Second_Sayu FROM HIC_JEPSU "
            //    SQL = SQL & "WHERE Pano=" & ArgPano & " "
            //    SQL = SQL & "  AND GjYear='" & strGjyear & "' "
            //    SQL = SQL & "  AND JepDate<TO_DATE('" & strJepDate & "','YYYY-MM-DD') "
            //    SQL = SQL & "  AND DelDate IS NULL "
            //    SQL = SQL & "  AND Second_Sayu IS NOT NULL "
            //    SQL = SQL & "ORDER BY JepDate DESC "
            //    Call AdoOpenSet(rs1, SQL)
            //    strSecondSayu = Trim(AdoGetString(rs1, "Second_Sayu", 0))
            //    Call AdoCloseSet(rs1)


            //End If


            //y = 2800
            //If strOK = "OK" Then
            //    Call Text_Print("맑은 고딕", 11, "", y, 300, "★(특수2차)")
            //ElseIf strGjJong = "44" Or strGjJong = "45" Or strGjJong = "46" Then
            //    Call Text_Print("맑은 고딕", 11, "", y, 300, "★(생애2차)")
            //Else
            //    Call Text_Print("맑은 고딕", 11, "", y, 300, "★★★(2차)")
            //End If


            //For i = 0 To 5
            //    If strUA검사구분(i) <> "" Then
            //        nCNT = nCNT + 1
            //        y = y + 300
            //        Call Text_Print("맑은 고딕", 11, "", y, 300, "●(" & strUA검사구분(i) & ")")
            //    End If
            //Next i


            //If strSecondSayu<> "" Then
            //   strSecondSayu = "●2차재검사유:" & strSecondSayu
            //    strTemp = TextBox_2_Multiline(strSecondSayu, 38)
            //    For i = 1 To 4
            //        If Trim(P(strTemp, "{{@}}", i)) <> "" Then
            //            nCNT = nCNT + 1
            //            y = y + 300
            //            Call Text_Print("맑은 고딕", 10, "", y, 300, Trim(P(strTemp, "{{@}}", i)))
            //        End If
            //    Next i
            //End If


            //Printer.FontBold = False
            //Printer.CurrentY = 500
            //Printer.FontName = "굴림체"
            //Printer.FontSize = 11


            //strGel(1) = "┌────────────────┐"
            //strGel(2) = "│가셔야할곳☞                    │"
            //strGel(3) = "│                                │"
            //strGel(4) = "│                                │"
            //strGel(5) = "│                                │"
            //strGel(6) = "│                                │"
            //strGel(7) = "├────────────────┤"
            //strGel(8) = "│ 검사 완료후 접수창구에 본 증을 │"
            //strGel(9) = "│ 제출하셔야 처리가 완료됩니다.  │"
            //strGel(10) = "│                  ☏054-289-4270│"
            //strGel(11) = "├────────────────┤"
            //strGel(12) = "│                                │"
            //If nCNT > 0 Then
            //    For i = 13 To(12 + nCNT)
            //        strGel(i) = "│                                │"
            //    Next i
            //End If
            //strGel(13 + nCNT) = "└────────────────┘"
            //strGel(14 + nCNT) = " 접수번호: " & ArgWRTNO
            //strGel(15 + nCNT) = " 검진번호: " & ArgPano


            //For j = 1 To(15 + nCNT)
            //    If strGel(j) <> "" Then
            //        Printer.CurrentX = 0
            //        Printer.Print strGel(j)
            //    End If
            //Next j


            //If y <= 3300 Then y = 3300


            //Call Text_Print("맑은 고딕", 10, "", 2800, 2000, READ_Ltd_Name(Trim(P(GstrRetValue, "^^", 7))))         '회사명
            //Call Text_Print("맑은 고딕", 12, "", y + 1000, 300, Trim(P(GstrRetValue, "^^", 4)))                      '검진종류
            //Call Text_Print("맑은 고딕", 12, "", y + 1000, 2400, Trim(P(GstrRetValue, "^^", 6)))                     '검진일자


            //Printer.FontBold = False
            //Printer.EndDoc
        }
    }
}
