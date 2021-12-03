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
using System.IO;
using ComLibB;
using System.Data.OleDb;
using System.Threading;
using System.Text.RegularExpressions;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHaAct1.cs
/// Description     : 검사결과 등록 / 변경(종검)
/// Author          : 이상훈
/// Create Date     : 2019-08-20
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHaAct01.frm(FrmHaAct), FrmHaAct03.frm, FrmHcAct01.frm, FrmHcAct03.frm " />

namespace HC_Act
{
    public partial class frmHaAct1 : BaseForm
    {
        HicResultService hicResultService = null;
        BasPcconfigService basPcconfigService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicExcodeService hicExcodeService = null;
        HicBcodeService hicBcodeService = null;        
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
        EtcJupmstService etcJupmstService = null;
        HeaResvExamService heaResvExamService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;

        frmHcActMain FrmHcActMain = null;
        frmHcActPFTMunjin FrmHcActPFTMunjin = null;
        frmHcPendListReg FrmHcPendListReg = null;
        frmHcActMemoSave FrmHcActMemoSave = null;
        frmHcMemo FrmHcMemo = null;
        frmViewResult FrmViewResult = null;
        frmHcAdmin_Job FrmHcAdmin_Job = null;

        ComFunc cF = new ComFunc();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcAct hcact = new clsHcAct();
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsHaBase ha = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        clsSpread sp = null;
        clsMdb Mdb = new clsMdb();


        //Serial 통신
        //SerialPort m_sp = new SerialPort();
        //delegate void SetTextCallBack(string opt);

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
        List<string> FstrRoomG = new List<string>();
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
        //////////////////////////////////////////////

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
        string[] strSpcExam = new string[7];

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
        int flag;
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
        long nAge;
        int nWait;
        string strSName;
        string strGjJong;
        string strNoise;
        string strMsg;
        //string strYYYY;

        //string strCODE = "";
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

        string FstrCboCode;
        string FstrCboName;

        string strGbNaksang;

        bool boolSort = false;
        bool blnExcelFileDownload = false;

        int iCnt;

        bool blnExitFlag = false;

        bool blnMdbSaveFlag = true;  //MDB 환자정보 저장 성공 여부

        public frmHaAct1()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHaAct1(frmHcActMain main)
        {
            InitializeComponent();
            FrmHcActMain = main;

            SetEvent();
            SetControl();
        }

        void SetControl()
        {
            sp = new clsSpread();

            hc.Read_INI(cboPart);

            lblExTitle.Text = cboPart.Text;

            for (int i = 0; i < cboPart.Items.Count; i++)
            {
                cboPart.SelectedIndex = i;
                if (VB.Pstr(cboPart.Text, ".", 1) == "13")
                {
                    if (MessageBox.Show("혈액검사 전용PC 세팅", "PC세팅", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        lblExTitle.Text = cboPart.Text;
                        cboPart.SelectedIndex = 0;
                        FstrTemp = "OK";
                        break;
                    }
                }
            }

            FstrPartG.Clear();
            FstrRoomG.Clear();

            for (int i = 0; i < cboPart.Items.Count; i++)
            {
                cboPart.SelectedIndex = i;
                FstrPartG.Add(VB.Pstr(cboPart.Text, ".", 1));
                FstrRoomG.Add(hicExcodeService.GetRoombyHeaPart(VB.Pstr(cboPart.Text, ".", 1)));
            }

            if (cboPart.Items.Count > 0)
            {
                cboPart.SelectedIndex = 0;
            }

            SheetView shv = SS2.ActiveSheet;
            InputMap im = new InputMap();
            Keystroke k = new Keystroke(Keys.Enter, Keys.None);
            im = SS2.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(k, SpreadActions.MoveToNextRow);
            im = SS2.GetInputMap(InputMapMode.WhenFocused);
            im.Put(k, SpreadActions.MoveToNextRow);
            //m_sp.DataReceived += new SerialDataReceivedEventHandler(Serial_DataReceived);
        }

        //void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    int i_recv_size = m_sp.BytesToRead;
        //    byte[] b_tmp_buf = new byte[i_recv_size];
        //    string recv_str = "";

        //    m_sp.Read(b_tmp_buf, 0, i_recv_size);
        //    recv_str = Encoding.Default.GetString(b_tmp_buf);
        //    this.BeginInvoke(new SetTextCallBack(Disp_Data), new object[] { recv_str});
        //}

        //void Disp_Data(string str)
        //{
        //    string sResult = str;
        //}

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
            patientInfoService = new PatientInfoService();
            etcJupmstService = new EtcJupmstService();
            heaResvExamService = new HeaResvExamService();
            heaJepsuPatientService = new HeaJepsuPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnPatSearch.Click += new EventHandler(eBtnClick);            

            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timer2.Tick += new EventHandler(eTimerTick);
            this.timer3.Tick += new EventHandler(eTimerTick);

            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTextBoxKeyPress);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTextBoxKeyPress); 

            this.tabControl1.Click += new EventHandler(eTabClick);

            this.ssChk.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpreadClick);

            this.ssChk.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.EditModeOn += new EventHandler(eSpreadEditModeOn);
            this.SS2.EditModeOff += new EventHandler(eSpreadEditModeOff);
            this.SS2.Change += new ChangeEventHandler(eSpreadChange);

            this.SS2.KeyDown += new KeyEventHandler(eSpreadKeyDown);
            this.SS2.KeyUp += new KeyEventHandler(eSpreadKeyUp);
            this.SS2.LeaveCell += new LeaveCellEventHandler(eSpreadLeaveCell);

            this.ssJepList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssJepList.CellClick += new CellClickEventHandler(eSpreadClick);

            this.chkCorrectedVision.Click += new EventHandler(eCheckBoxClick);
            this.chkCorrectedHearing.Click += new EventHandler(eCheckBoxClick);

            this.Menu01.Click += new EventHandler(eMenuClick);  //접수자명단조회
            this.Menu02.Click += new EventHandler(eMenuClick);  //과거결과조회
            this.Menu03.Click += new EventHandler(eMenuClick);  //내시경대기순번등록
            this.Menu04.Click += new EventHandler(eMenuClick);  //일반상담대기순번등록
            this.Menu05.Click += new EventHandler(eMenuClick);  //SONO 예약명단(당일)
            //this.Menu06.Click += new EventHandler(eMenuClick);  //InBody Data 초기화
            //this.Menu07.Click += new EventHandler(eMenuClick);  //계측입력확인
            this.Menu08.Click += new EventHandler(eMenuClick);  //자동액팅시작
            this.Menu09.Click += new EventHandler(eMenuClick);  //동의서보기
            this.Menu10.Click += new EventHandler(eMenuClick);  //스트레스검사
            this.Menu12.Click += new EventHandler(eMenuClick);  //폐활량검사표
            this.Menu13.Click += new EventHandler(eMenuClick);  //보류대장
            this.Menu14.Click += new EventHandler(eMenuClick);  //메모관리
            this.Menu15_01.Click += new EventHandler(eMenuClick);  //EMR
            this.Menu15_02.Click += new EventHandler(eMenuClick);  //OCS결과
            this.Menu16.Click += new EventHandler(eMenuClick);  //관리자작업창
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            string strResult = "";
            string strOK = "";

            if (sender == chkCorrectedVision)
            {
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strOK = "";
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
                strOK = "";

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 14].Text.Trim() == "7")   //청력파트 검사
                    {   
                        strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                        if (chkCorrectedHearing.Checked == true)
                        {
                            if (VB.Left(strResult, 1) != "(" && VB.Right(strResult, 1) != ")")
                            {
                                strResult += "(교정)";
                                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                                SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text = "Y";
                            }
                        }
                        else
                        {
                            if (VB.Right(strResult, 4) == "(교정)")
                            {
                                strResult = VB.STRCUT(strResult, "", "(");
                                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                                SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text = "Y";
                            }
                        }
                    }
                }
            }
        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == Menu01)   //접수자명단조회
            {
                fn_menuList();
            }
            else if (sender == Menu02)  //과거결과조회
            {
                frmHcPanPersonResult frm = new frmHcPanPersonResult("frmHaAct1", FstrPtno, FstrName);
                frm.ShowDialog();
            }
            else if (sender == Menu03)  //내시경대기순번등록
            {
                if (hc.OpenForm_Check("frmHaEndoWaitSeq") == true)
                {
                    return;
                }

                frmHaEndoWaitSeq frm = new frmHaEndoWaitSeq();
                frm.Show();                
            }
            else if (sender == Menu04)  //일반상담대기순번등록
            {
                if (hc.OpenForm_Check("frmHaCounselSeqReg") == true)
                {
                    return;
                }

                frmHaCounselSeqReg frm = new frmHaCounselSeqReg();
                frm.Show();
            }
            else if (sender == Menu05)  //SONO 예약명단(당일)
            {
                frmSONOReservedList frm = new frmSONOReservedList();
                frm.ShowDialog();
            }
            else if (sender == Menu08)  //자동액팅시작
            {
                if (MessageBox.Show("종검 자동액팅을 시작하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    clsHcVariable.GnAutoTimerCnt = 17;
                    FrmHcActMain.timer1.Enabled = true;
                    MessageBox.Show("종검 자동액팅을 시작함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
            }
            else if (sender == Menu09)  //동의서보기
            {
                frmHcConsentformView frm = new frmHcConsentformView();
                frm.ShowDialog();
            }
            else if (sender == Menu10)  //스트레스검사
            {
                string rtnVal = "";
                string strAudioFile = "";
                string strParam = "";

                strParam = FnWRTNO +"{}" + FstrPtno + "{}" + FstrName + "{}" + FstrSex + "{}" + FnAge + "{}" + FstrSDate;

                frmHaStressExam frm = new frmHaStressExam(strParam);
                frm.ShowDialog();

                //디렉토리 체크후 없으면 생성 있으면 폴더내 파일 삭제후 작업
                DirectoryInfo dir1 = new DirectoryInfo(@"c:\windows\system32\shimgvw.dll");
                DirectoryInfo dir2 = new DirectoryInfo(@"%ProgramFiles%\Windows Photo Gallery\PhotoViewer.dll");
                if (dir1.Exists == true)
                {
                    strAudioFile = @"rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strParam;
                }
                else if (dir2.Exists == true)
                {
                    strAudioFile = @"rundll32.exe %ProgramFiles%\Windows Photo Gallery\PhotoViewer.dll, ImageView_Fullscreen " + strParam;
                }
                else
                {
                    strAudioFile = @"rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strParam;
                }

                VB.Shell(strAudioFile, "NormalFocus");

                fn_Screen_Display();

                if (blnExitFlag == true)
                {
                    fn_Screen_Clear();
                    txtWrtNo.Text = "";
                    txtWrtNo.Focus();
                    txtWrtNo.Select();
                    return;
                }
            }
            else if (sender == Menu12)  //폐활량검사표
            {
                if (FnHcWRTNO == 0)
                {
                    return;
                }

                FrmHcActPFTMunjin = new frmHcActPFTMunjin("HAACT", "HEA", FnHcWRTNO, FstrPtno, txtWrtNo.Text.To<long>());
                FrmHcActPFTMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcActPFTMunjin.ShowDialog();                
            }
            else if (sender == Menu13)  //보류대장
            {
                FnWRTNO = txtWrtNo.Text.To<long>();

                if (FnWRTNO > 0)
                {
                    FrmHcPendListReg = new frmHcPendListReg(FnWRTNO, "2");
                    FrmHcPendListReg.ShowDialog(this);
                }
            }
            else if (sender == Menu14)  //메모관리
            {
                FnWRTNO = txtWrtNo.Text.To<long>();

                if (FnWRTNO > 0)
                {
                    FrmHcMemo = new frmHcMemo(FstrPtno);
                    FrmHcMemo.ShowDialog(this);
                }
            }
            else if (sender == Menu15_01)  //EMR
            {
                clsVbEmr.EXECUTE_NewTextEmrView(FstrPtno);
            }
            else if (sender == Menu15_02)  //OCS 결과
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == Menu16)  //관리자작업
            {
                FrmHcAdmin_Job = new frmHcAdmin_Job();
                FrmHcAdmin_Job.StartPosition = FormStartPosition.CenterParent;
                FrmHcAdmin_Job.ShowDialog(this);
            }


        }

        void fn_Screen_Clear()
        {   
            //KOSMOS_PMPA.PC_HEA_WAIT_PATIENT_CLEAR(WRTNO, ROOMCODE);
            Dictionary<string, object> dic = heaSangdamWaitService.Sangdam_Wait_Update(txtWrtNo.Text.To<long>(), FstrRoom);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in dic)
            {
                stringBuilder.AppendLine(item.Key + " : " + item.Value);
            }

            FnRowNo = 0;
            FnPano = 0;
            FnAge = 0;
            FstrSex = "";
            FstrPtno = "";
            FstrName = "";
            FstrJumin = "";
            FnWRTNO = 0;
            txtWrtNo.Text = "";
            FnClickRow = 0;
            FnHeaWRTNO = 0;
            FstrRoom2 = "";
            FstrSDate = "";
            FstrGongDan = "";

            //당일검진체크
            sp.Spread_All_Clear(ssChk);
            sp.Spread_All_Clear(SSExam1);
            sp.Spread_All_Clear(SSExam2);
            sp.Spread_All_Clear(spBarCode);

            lblResultReceivePosition.Text = "";

            lblSDate.Text = "";
            lblExam.Text = "";
            //MenuPFT.Enabled = false;
            btnSave.Enabled = true;
            //txtPano.Text = "";
            lblExam.Visible = false;

            //인적사항
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[2, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[3, 1].Text = "";

            sp.Spread_All_Clear(SS2);
            sp.SetfpsRowHeight(SS2, 35);
            sp.Spread_All_Clear(ssChk);

            btnSave.Enabled = true;
            SS2.ActiveSheet.RowCount = 40;
            ssChk.ActiveSheet.RowCount = 20;

            lblSDate.Text = "";
            lblExam.Text = "";
            timer1.Enabled = false;
            Menu12.Enabled = false;
            txtSName.Text = "";            

            conHcPatInfo1.SetDisPlay("25420", "O", clsPublic.GstrSysDate, "", "", "");
        }
        
        void eTabClick(object sender, EventArgs e)
        {
            if (sender == tabControl1)
            {
                if (tabControl1.SelectedTab == tabJepsu)
                {
                    eBtnClick(btnPatSearch, new EventArgs());
                }
                else if (tabControl1.SelectedTab == tabChk)
                {   
                    FnWRTNO = txtWrtNo.Text.To<long>();
                    ACTING_CHECK(FnWRTNO, dtpSDate.Text);
                }
                txtWrtNo.Focus();
            }
        }

        void fn_menuList()
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

            string strSName = "";

            strSDate = dtpSDate.Text;
            tabControl1.TabIndex = 0;
            tabControl1.SelectedTab = tabJepsu;

            if (ssJepList.ActiveSheet.RowCount == 0) ssJepList.ActiveSheet.RowCount = 30;

            strSName = txtSName.Text.Trim();

            List<HIC_JEPSU_HEA_EXJONG> list = hicJepsuHeaExjongService.GetGbStsbyWrtNo(strSDate, strSName);

            nRead = list.Count;
            ssJepList.ActiveSheet.RowCount = nRead;

            for (int i = 0; i < nRead; i++)
            {
                ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();

                //if (!list[i].ACTMEMO.IsNullOrEmpty())
                //{
                //    ssJepList.ActiveSheet.Cells[i, 0].BackColor = Color.NavajoWhite;
                //}
                //else
                //{
                //    ssJepList.ActiveSheet.Cells[i, 0].BackColor = Color.White;
                //}

                nWrtNo = list[i].WRTNO;

                if (nWrtNo > 0)
                {
                    ssJepList.ActiveSheet.Cells[i, 2].Text = list[i].AMPM2 == "2" ? "오후" : "";

                    //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                    //List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo, "1");
                    //nRead2 = listActive.Count;

                    //if (nRead2 > 0)
                    //{
                    //    for (int j = 0; j < nRead2; j++)
                    //    {
                    //        if (listActive[j].ACTIVE == "N" || listActive[j].ACTIVE.IsNullOrEmpty())
                    //        {
                    //            strTemp = "OK";
                    //        }
                    //    }

                    //    if (strTemp == "OK")
                    //    {
                    //        ssJepList.ActiveSheet.Cells[i, 3].Text = "X";
                    //    }
                    //    else
                    //    {
                    //        ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                    //    }
                    //    strTemp = "";
                    //}
                    string[] strExcode = new string[] { "TX20", "TX64", "TX41" };
                    //수면내시경 노란색
                    HIC_RESULT listRslt = hicResultService.Read_ExCode2(nWrtNo, strExcode);

                    if (!listRslt.IsNullOrEmpty())
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
                    ssJepList.ActiveSheet.Cells[i, 4].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                }
                else
                {
                    ssJepList.ActiveSheet.Cells[i, 4].Text = "개인종검";
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
                    fn_Screen_Clear();
                    txtWrtNo.Focus();
                }
            }
            else if (sender == btnSave)
            {
                string strRoomCd = "";
                string strYYYY = "";
                string strPtNo = "";
                string strPANO = "";
                string strSDate = "";
                string strEDATE = "";
                string StrRDate = "";

                if (txtWrtNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("접수번호가 공란입니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtWrtNo.Focus();
                    return;
                }

                btnSave.Enabled = false;

                //인적사항을 READ
                HEA_JEPSU HicJepsulist = heaJepsuService.Read_Jepsu2(txtWrtNo.Text.To<long>());

                if (!HicJepsulist.IsNullOrEmpty())
                {
                    strYYYY = HicJepsulist.SDATE1.Substring(0, 4);
                    strSex = HicJepsulist.SEX;
                    strSName = HicJepsulist.SNAME;
                    strGjJong = HicJepsulist.GJJONG;
                    nAge = HicJepsulist.AGE;

                    strGbSTS = HicJepsulist.GBSTS;
                    strPANO = HicJepsulist.PANO.To<string>();
                    strPtNo = HicJepsulist.PTNO;
                    strSDate = HicJepsulist.SDATE1;
                }

                HEA_RESV_EXAM list = heaResvExamService.GetRTimebyPaNoGbExamSDate(strPANO.To<long>(), strSDate, "07");

                if (!list.IsNullOrEmpty())
                {
                    StrRDate = VB.Left(list.RTIME.To<string>(), 10);
                    strEDATE = Convert.ToDateTime(strSDate).AddDays(1).ToShortDateString();
                }

                if (strGbSTS == "9")
                {
                    MessageBox.Show("판정완료 대상입니다. 수정 불가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = true;
                    return;
                }

                strActMemo = ssPatInfo.ActiveSheet.Cells[1, 3].Text;

                //-----------------------------------
                //특수소음 누락 방지
                //-----------------------------------
                strNoise = "";
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
                                btnSave.Enabled = true;
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
                                btnSave.Enabled = true;
                                return;
                            }
                        }
                    }
                }

                nDataCNT = 0;
                nResultCNT = 0;

                clsDB.setBeginTran(clsDB.DbCon);

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
                        strResult = hm.Biman_Gesan(FnWRTNO, "HEA");
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

                    //2020-05-23(동맥경화검사 인터페이스 관련부분)
                    if (strCode == "A894" && VB.Pstr(strResult, ".", 1) == "01" && strChange == "Y")
                    {
                        int result = etcJupmstService.UpdateGbJobbyPtNoRDate(strPtNo, StrRDate, strEDATE);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("동맥경화검사 접수UPDATE 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }
                    }

                    if (strChange == "Y")
                    {
                        //History에 INSERT
                        int result = heaResultService.Result_History_Insert(clsType.User.IdNumber, strResult.Replace("'", "`"), strROWID, "");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }

                        int result1 = heaResultService.Result_Update(strResult.Replace("'", "`"), strPanjeng, strResCode, clsType.User.IdNumber, strROWID, "");

                        if (result1 < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }
                    }

                    if (strCode == "E908" || strCode == "E909")
                    {
                        strWomen = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.REFERENCE].Text.Trim();
                        strWomen1 = VB.Pstr(strWomen, "~", 1);
                        strWomen2 = VB.Pstr(strWomen, "~", 2);

                        int result = heaWomenService.Merge_Women_Reference(strWomen1, strWomen2, strWomen1, strWomen2, FnWRTNO, strCode);

                        //if (result < 0)
                        //{
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    MessageBox.Show("종검 여성정밀 참고치 갱신중 오류가 발생함!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    btnSave.Enabled = true;
                        //    return;
                        //}
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //clsDB.setBeginTran(clsDB.DbCon);

                //청력검사 결과 자동입력
                hb.Update_Audio_Result(long.Parse(txtWrtNo.Text), strSex);

                //폐활량검사 결과 자동입력
                hb.Update_Lung_Capacity(long.Parse(txtWrtNo.Text), strSex);

                //MDRD-GFR 자동계산 2012년부터
                //hm.MDRD_GFR_Gesan(FnWRTNO, strSex, nAge, "HEA");

                //해당접수번호의 결과입력대상건수, 결과입력건수를 READ
                List<HEA_RESULT> RsltList = heaResultService.GetExCodeResultbyWrtNo(FnWRTNO);

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
                if (nResultCNT == nDataCNT || (nResultCNT == nDataCNT - 1 && !strEndoSo.IsNullOrEmpty()))
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
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("접수마스타에 입력상태 변경중 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
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

                List<HEA_SANGDAM_WAIT> HicSDlist = heaSangdamWaitService.Etc_ExamRoom_RegConfirm(txtWrtNo.Text.To<long>(), strRoomCd);
                if (HicSDlist.Count > 0)
                {
                    //기존의 자료가 있으면 삭제함
                    int result = heaSangdamWaitService.Delete_Sangdam_Wait(txtWrtNo.Text.To<long>(), strRoomCd);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("타 검사실 대기순번 삭제시 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnSave.Enabled = true;
                        return;
                    }
                }

                //상담대기순번 완료
                nREAD = 0;
                nCNT = 0;

                List<HIC_RESULT> ActDList = heaResultService.Read_Result_Acitve(txtWrtNo.Text.To<long>(), FstrPartG);

                nREAD = ActDList.Count;
                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        if (ActDList[i].ACTIVE == "Y")
                        {
                            nCNT += 1;
                        }
                    }
                }

                string[] strJongSQL;

                strJongSQL = new string[] { FstrRoom };

                List<HEA_SANGDAM_WAIT> GbCallList = heaSangdamWaitService.Read_Sangdam_GbCall(txtWrtNo.Text.To<long>(), FstrRoom);

                if (GbCallList.Count > 0)
                {
                    if (nCNT == nREAD)
                    {
                        int result1 = heaSangdamWaitService.Update_Sangdam_GbCall(FnWRTNO, strJongSQL);

                        if (result1 < 0)
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            btnSave.Enabled = true;
                            return;
                        }
                    }
                    else
                    {
                        int result3 = heaSangdamWaitService.Update_Sangdam_CallTime(FnWRTNO, FstrRoom2);

                        if (result3 < 0)
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            btnSave.Enabled = true;
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
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("상담대기 순번등록 중 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
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
                            item.WAITNO = WaitNoList.WAITNO.To<long>();

                            int result4 = heaSangdamWaitService.Insert_Sangdam_Wait(item);

                            if (result4 < 0)
                            {
                                //clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("상담대기 순번등록 중 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                btnSave.Enabled = true;
                                return;
                            }
                        }
                    }
                }
                //clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
                tabControl1.SelectedTab = tabChk;
                eTabClick(tabControl1, new EventArgs());

                txtWrtNo.Focus();
                this.ActiveControl = txtWrtNo;
                txtWrtNo.SelectAll();

                if (cboPart.FindString("3.체성분") > -1)
                {
                    if (chkInBodySend.Checked == true)
                    {
                        Dir_Check(@"C:\LookinBody120\EMR\CSV\", "*.*");
                    }
                }

                btnSave.Enabled = true;
            }
            else if (sender == btnPatSearch)
            {
                fn_menuList();
                txtWrtNo.Focus();
            }
        }

        void eTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                string strPtno = "";
                string strTemp = "";
                string strSDate = "";

                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text.Trim().IsNullOrEmpty()) return;

                    if (txtWrtNo.Text.Length > 8)
                    {
                        MessageBox.Show("8자를 초과 할 수 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fn_Screen_Clear();
                        txtWrtNo.Text = "";
                        txtWrtNo.Focus();
                        txtWrtNo.SelectAll();

                        return;
                    }

                    strTemp = txtWrtNo.Text;                    
                    fn_Screen_Clear();
                    txtWrtNo.Text = strTemp;
                    
                    if (txtWrtNo.Text.Length > 6)
                    {
                        //strPtno = VB.Pstr(txtWrtNo.Text.Trim(), " ", 1);
                        strPtno = txtWrtNo.Text.Trim();
                        txtWrtNo.Text = "";
                        //외래번호로 접수번호 찾기
                        HEA_JEPSU list = heaJepsuService.Get_WrtNo(strPtno, dtpSDate.Text);

                        if (!list.IsNullOrEmpty())
                        {
                            txtWrtNo.Text = list.WRTNO.To<string>();
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

                    if (blnExitFlag == true)
                    {
                        fn_Screen_Clear();
                        txtWrtNo.Text = "";
                        txtWrtNo.Focus();
                        txtWrtNo.Select();
                        return;
                    }

                    //폐활량검사 액팅방이고 액팅이 안된경우(일반검진이 있는 경우에만)
                    if (!clsHcVariable.GstrPFTSN.IsNullOrEmpty() && FnHcWRTNO > 0)
                    {
                        string ResList = hicResSpecialService.Read_Res_Special(FnHcWRTNO);

                        if (!ResList.IsNullOrEmpty())
                        {
                            HIC_RESULT RsltList = hicResultService.Read_Result3(FnHcWRTNO);

                            if (!RsltList.IsNullOrEmpty())
                            {
                                Menu12.Enabled = true;
                                if (RsltList.RESULT.IsNullOrEmpty())
                                {
                                    eMenuClick(Menu12, new EventArgs());
                                }
                            }
                        }
                    }

                    ////결과항목이 null 인 셀 포커스 이동
                    SS2_FOCUS();
                }
            }
            else if (sender == txtSName)
            {
                if (e.KeyChar == 13)
                {
                    eBtnClick(btnPatSearch, new EventArgs());
                }
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (sender == ssChk)
            {
                if (ssChk.ActiveSheet.NonEmptyRowCount == 0) return;

                if (e.Row < 0 || e.ColumnHeader == true) return;

                if (e.Column != 4) return;

                strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                {
                    ssChk.ActiveSheet.Cells[i, 5].Text = "";
                }

                //해당 검사실 대기명단
                List<HEA_SANGDAM_WAIT> Wait_List = heaSangdamWaitService.Read_Sangdam_Wait_List(strPart, "");

                if (!Wait_List.IsNullOrEmpty())
                {
                    for (int i = 0; i < Wait_List.Count; i++)
                    {
                        ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List[i].SNAME;
                        ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List[i].WRTNO.ToString();
                    }
                }
            }
            else if (sender == SS2)
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
            else if (sender == ssJepList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(ssJepList, e.Column, ref boolSort, true);
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

            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;

            if (SS2.InputDeviceType.ToString() == "Keyboard")
            {
                if (nRow == SS2.ActiveSheet.RowCount - 1)
                {   
                    SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";                    
                    //if (MessageBox.Show("결과값을 저장하시겠습니까?", "확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                    eBtnClick(btnSave, new EventArgs());
                    //}
                    return;
                }
            }
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

            //if (e.KeyCode == Keys.Enter)
            //{
            //    sp.setEnterKey(SS2, clsSpread.enmSpdEnterKey.Down);
            //    return;
            //}

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
                lblGuide.Text = "F1: 측정불가 F5: 본인제외 F6: 정상 F7: 색각이상 F8: 9.9 F9: 미실시";

                switch (e.KeyCode)
                {
                    case Keys.F1:
                        strResult = "측정불가";
                        break;
                    case Keys.F5:
                        strResult = "본인제외";
                        break;
                    case Keys.F6:
                        strResult = "정상";
                        break;
                    //case Keys.F6:
                    //    strResult = "비정상";
                    //    break;
                    //case Keys.F7:
                    //    strResult = "교정";
                    //    break;
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
                SS2.ActiveSheet.Cells[nRow, 2].Text = strResult;
                SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";
                FnRowNo += 1;
                if (FnRowNo > SS2.ActiveSheet.RowCount)
                {
                    FnRowNo = SS2.ActiveSheet.RowCount - 1;
                }
                SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
            }
        }

        //void eSpreadKeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 13)
        //    {
        //        int nRow = this.SS2.ActiveSheet.ActiveRow.Index;

        //        if (SS2.InputDeviceType.ToString() == "Keyboard")
        //        {
        //            if (nRow == SS2.ActiveSheet.RowCount - 1)
        //            {
        //                if (MessageBox.Show("결과값을 저장하시겠습니까?", "확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //                {
        //                    eBtnClick(btnSave, new EventArgs());
        //                }
        //                return;
        //            }
        //        }
        //    }
        //}

        void eSpreadKeyUp(object sender, KeyEventArgs e)
        {
            string strCode = "";
            bool bKey1 = false;
            bool bKey2 = false;

            int nRow = SS2.ActiveSheet.ActiveRowIndex;

            if (e.KeyCode.To<int>() > 47 && e.KeyCode.To<int>() < 58)
            {
                bKey1 = true;
            }

            if (e.KeyCode.To<int>() > 64 && e.KeyCode.To<int>() < 91)
            {
                bKey2 = true;
            }

            if (bKey1 == false && bKey2 == false)
            {
                return;
            }

            strCode = SS2.ActiveSheet.Cells[nRow, 0].Text.Trim();

            //숫자값을 사용하는 코드인지 확인
            strTemp = hicExcodeService.GetResultTypebyCode(strCode);

            if (strTemp == "1")
            {
                //숫자일경우
                if (!SS2.ActiveSheet.Cells[nRow, 2].Text.IsNullOrEmpty())
                {
                    if (VB.IsNumeric(SS2.ActiveSheet.Cells[nRow, 2].Text) == false)
                    {
                        MessageBox.Show("결과 입력이 잘못 되었습니다!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SS2.ActiveSheet.SetActiveCell(nRow, 2);
                        SS2.Focus();
                        SS2.EditMode = true;
                    }
                }
            }
            else if (strTemp == "2")
            {
                //문자일경우
                if (VB.IsNumeric(SS2.ActiveSheet.Cells[nRow, 2].Text) == true)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.SetActiveCell(nRow, 2);
                    SS2.Focus();
                    SS2.EditMode = true;
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

            if (SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text.Trim() != "")
            {
                strCode = VB.Pstr(SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text.Trim(), ".", 1);
            }
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

        void eSpreadChange(object sender, ChangeEventArgs e)
        {
            double nData = 0;
            string strData = "";
            string strCode = "";

            int nRow = e.Row;
            int nCol = e.Column;

            strCode = SS2.ActiveSheet.Cells[nRow, 0].Text.Trim();
            nData = SS2.ActiveSheet.Cells[nRow, 2].Text.Trim().To<double>();
            strData = SS2.ActiveSheet.Cells[nRow, 2].Text.Trim();
            SS2.ActiveSheet.Cells[nRow, 8].Text = "Y";

            //숫자값을 사용하는 코드인지 확인
            if (hicExcodeService.GetResultTypebyCode(strCode) == "1")
            {
                if (nData > 999)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                    return;
                }

                switch (strCode)
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
                        nData = SS2.ActiveSheet.Cells[nRow, 2].Text.Trim().Replace("(", "").Replace(")", "").To<double>();
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
            long nAge;
            string strPart;
            string strSName;
            string strSex;
            string strGjJong;
            string strRoom;

            FpSpread o = (FpSpread)sender;
            flag = 0;

            if (sender == this.ssChk)
            {
                if (ssChk.ActiveSheet.NonEmptyRowCount == 0) return;

                if (e.Column == 3)
                {
                    flag = 1;

                    if (txtWrtNo.Text.Trim().IsNullOrEmpty())
                    {
                        txtWrtNo.Focus();
                        txtWrtNo.Select();
                        return;
                    }

                    HEA_JEPSU Wait_List = heaJepsuService.Read_Jepsu_Wait_List(txtWrtNo.Text.To<long>(), dtpSDate.Text);

                    if (Wait_List.IsNullOrEmpty())
                    {
                        MessageBox.Show(txtWrtNo.Text + " 접수내역이 없습니다", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        txtWrtNo.Focus();
                        return;
                    }

                    strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                    //해당방에 등록되어 있는지 확인
                    HEA_SANGDAM_WAIT SDReg_List = heaSangdamWaitService.Read_Sangdam_Wait_RegList(txtWrtNo.Text.To<long>(), strPart);

                    if (!SDReg_List.IsNullOrEmpty())
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

                    //clsDB.setBeginTran(clsDB.DbCon);

                    //다른검사실에 등록되어 있는지 확인
                    //HEA_SANGDAM_WAIT SDEtcReg_List = heaSangdamWaitService.Read_Sangdam_EtcRoomReg(txtWrtNo.Text.To<long>(), strPart);
                    HEA_SANGDAM_WAIT SDEtcReg_List = heaSangdamWaitService.Read_Sangdam_EtcRoomReg(txtWrtNo.Text.To<long>(), FstrRoom);

                    if (!SDEtcReg_List.IsNullOrEmpty())
                    {
                        if (!SDEtcReg_List.CALLTIME.IsNullOrEmpty())
                        {
                            MessageBox.Show(SDEtcReg_List.GUBUN + " 번방에서 검사중입니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                        else
                        {
                            //기존의 자료가 있으면 삭제함
                            int result = heaSangdamWaitService.Delete_Sangdam_PreData(txtWrtNo.Text.To<long>(), FstrRoom);

                            if (result < 0)
                            {
                                //clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("타 검사실 대기순번 삭제시 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    //해당방에 등록
                    //HEA_SANGDAM_WAIT View_List = heaSangdamWaitService.Read_Sangdam_View(txtWrtNo.Text.To<long>());
                    HEA_JEPSU View_List = heaJepsuService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

                    if (!View_List.IsNullOrEmpty())
                    {
                        strSName = View_List.SNAME;
                        strSex = View_List.SEX;
                        nAge = View_List.AGE;
                        strGjJong = View_List.GJJONG;

                        HEA_SANGDAM_WAIT View_List2 = heaSangdamWaitService.Read_Sangdam_View2(strPart);

                        HEA_SANGDAM_WAIT item = new HEA_SANGDAM_WAIT();

                        item.WRTNO = FnWRTNO;
                        item.SNAME = strSName;
                        item.SEX = strSex;
                        item.AGE = nAge;
                        item.GJJONG = strGjJong;
                        item.GBCALL = "";
                        item.GUBUN = strPart;
                        item.WAITNO = View_List2.WAITNO;

                        int result = heaSangdamWaitService.Insert_Sangdam_Wait3(item);

                        if (result < 0)
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("상담대기 순번등록 중 오류 발생!!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        FstrRoom2 = strPart;
                    }

                    //clsDB.setCommitTran(clsDB.DbCon);

                    for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                    {
                        strRoom = ssChk.ActiveSheet.Cells[i, 6].Text.Trim();
                        ssChk.ActiveSheet.Cells[i, 3].Text = "";

                        HEA_SANGDAM_WAIT WrtNoCnt_List = heaSangdamWaitService.Read_Sangdam_WrtNoCnt(strRoom);

                        if (strRoom != "99")
                        {
                            ssChk.ActiveSheet.Cells[i, 3].Text = WrtNoCnt_List.CNT.To<string>();
                        }
                    }

                    //현 검사실 대기명단
                    List<HEA_SANGDAM_WAIT> Wait_List1 = heaSangdamWaitService.Read_Sangdam_Wait_List(ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim(), "N");

                    if (!Wait_List1.IsNullOrEmpty())
                    {
                        ssChk.ActiveSheet.Cells[0, 5, ssChk.ActiveSheet.RowCount - 1, 5].Text = "";
                        ssChk.ActiveSheet.Cells[0, 8, ssChk.ActiveSheet.RowCount - 1, 8].Text = "";
                        for (int i = 0; i < Wait_List1.Count; i++)
                        {
                            ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List1[i].SNAME;
                            ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List1[i].WRTNO.ToString();
                        }
                    }

                    ssChk.EditMode = false;
                    txtWrtNo.Focus();
                    txtWrtNo.SelectAll();
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
                            ssJepList.ActiveSheet.Cells[i, 0].Text = Waitlist1[i].WRTNO.To<string>();
                            ssJepList.ActiveSheet.Cells[i, 1].Text = Waitlist1[i].SNAME;
                        }
                    }
                    else
                    {
                        strPart = ssChk.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                        List<Select_JepList> Waitlist2 = selectJeplistService.Read_Sangdam_View4(dtpSDate.Text, strPart);

                        ssJepList.ActiveSheet.RowCount = Waitlist2.Count;
                        for (int i = 0; i < Waitlist2.Count; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = Waitlist2[i].WRTNO.To<string>();
                            ssJepList.ActiveSheet.Cells[i, 1].Text = Waitlist2[i].SNAME;
                            ssJepList.ActiveSheet.Cells[i, 2].Text = Waitlist2[i].AMPM2;
                            ssJepList.ActiveSheet.Cells[i, 4].Text = Waitlist2[i].NAME;
                        }
                    }
                }
            }
            if (sender == this.ssJepList)
            {
                long nWrtNo = 0;

                if (e.RowHeader == true) return;

                if (ssJepList.ActiveSheet.NonEmptyRowCount == 0) return;

                fn_Screen_Clear();

                txtWrtNo.Text = ssJepList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                nWrtNo = txtWrtNo.Text.To<long>();

                eTextBoxKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));

                tabControl1.TabIndex = 1;
                tabControl1.SelectedTab = tabChk;

                ////결과항목이 null 인 셀 포커스 이동
                SS2_FOCUS();
            }
            else if (sender == this.SS2)
            {
                string strResCode = "";
                string strData = "";

                if (e.Column != 2)
                {
                    return;                    
                }

                //SS2.ActiveSheet.SetActiveCell(e.Row, 2);
            }
        }


        /// <summary>
        /// //결과항목이 null 인 셀 포커스 이동
        /// </summary>
        void SS2_FOCUS()
        {   
            if (SS2.ActiveSheet.RowCount > 0)
            {
                SS2.ActiveSheet.SetActiveCell(0, (int)clsHcType.Instrument_Result.RESULT);
                //if (SS2.ActiveSheet.Cells[0, (int)clsHcType.Instrument_Result.RESULT].Text.Trim() == "")
                //{
                //    SS2.EditMode = true;
                //}
                SS2.Focus();
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text.Trim() == "")
                    {
                        SS2.ActiveSheet.SetActiveCell(i, (int)clsHcType.Instrument_Result.RESULT);
                        //SS2.ShowColumn(i, (int)clsHcType.Instrument_Result.RESULT, FarPoint.Win.Spread.HorizontalPosition.Nearest);
                        //if (SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text.Trim() == "")
                        //{
                        //    SS2.EditMode = true;
                        //}
                        SS2.Focus();
                        break;
                    }
                }
            }
        }

        void eTimerTick(object sender, EventArgs e)
        {
            if (sender == timer1)   //BP 결과 Read
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
            else if (sender == timer2)  //안압검사 결과 Read
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
            else if (sender == timer3)  //InBody 결과 Read
            {
                int nFileCnt = 0;
                string strPath = @"C:\LookinBody120\EMR\CSV\";
                string strFileName = "";
                string[] sFile = new string[10];
                int K = 0;
                int[] data = new int[10];

                string strPtno = "";
                string strHeight = "";
                string strWeight = "";
                string strWaist = "";

                if (!txtWrtNo.Text.IsNullOrEmpty()) return;

                sp.Spread_All_Clear(ssCSV);
                ssCSV.ActiveSheet.RowCount = 2;

                for (int i = 0; i < 10; i++)
                {
                    sFile[i] = "";
                }

                DirectoryInfo Dir = new DirectoryInfo(strPath);

                K = 0;
                foreach (FileInfo File in Dir.GetFiles())
                {
                    if (File.Extension.ToLower().CompareTo(".csv") == 0)
                    {
                        strFileName = File.Name.Substring(0, File.Name.Length - 4);
                        strPtno = VB.Left(strFileName, 8);

                        if (VB.Mid(strFileName, 10, 8) == clsPublic.GstrSysDate.Replace("-", ""))
                        {
                            K += 1;
                            nFileCnt += 1;
                            sFile[K - 0] = strFileName;
                        }
                    }
                }

                if (nFileCnt > 0)
                {
                    Array.Sort(sFile);
                    Array.Reverse(sFile);

                    strFileName = sFile[0];

                    using (FileStream fs = new FileStream(strPath + strFileName + ".csv", FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8, false))
                        {
                            int k = 0;
                            while (!sr.EndOfStream)
                            {
                                string s = sr.ReadLine();
                                string[] keys = s.Split(',');

                                ssCSV.ActiveSheet.Cells[k, 0].Text = keys[1].Replace("\"","");   // PTNO
                                ssCSV.ActiveSheet.Cells[k, 1].Text = keys[2].Replace("\"", "");   // HEIGHT
                                ssCSV.ActiveSheet.Cells[k, 2].Text = keys[14].Replace("\"", "");  // WEIGHT
                                ssCSV.ActiveSheet.Cells[k, 3].Text = keys[208].Replace("\"", ""); // WAIST
                                k += 1;
                            }
                        }
                    }
                    //txtWrtNo.Text = ssCSV.ActiveSheet.Cells[1, 0].Text;
                    txtWrtNo.Text = strPtno;
                    eTextBoxKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));

                    Thread.Sleep(200);

                    strHeight = ssCSV.ActiveSheet.Cells[1, 1].Text;
                    strWeight = ssCSV.ActiveSheet.Cells[1, 2].Text;
                    strWaist = ssCSV.ActiveSheet.Cells[1, 3].Text;

                    if (SS2.ActiveSheet.RowCount > 0)
                    {
                        for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                        {
                            if (SS2.ActiveSheet.Cells[i, 0].Text == "A101") //신장
                            {
                                SS2.ActiveSheet.Cells[i, 2].Text = strHeight;
                                SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                            }

                            if (SS2.ActiveSheet.Cells[i, 0].Text == "A102") //체중
                            {
                                SS2.ActiveSheet.Cells[i, 2].Text = strWeight;
                                SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                            }

                            if (SS2.ActiveSheet.Cells[i, 0].Text == "A115") //허리둘레
                            {
                                SS2.ActiveSheet.Cells[i, 2].Text = strWaist;
                                SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                            }
                        }
                    }
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";
            string strMyName = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();

            FstrTemp = "";
            //SS2_Sheet1.Columns[0].Visible = false;      //Code   
            SS2_Sheet1.Columns[3].Visible = false;      //help   
            SS2_Sheet1.Columns[4].Visible = false;      //결과코드
            SS2_Sheet1.Columns[5].Visible = false;      //판정
            SS2_Sheet1.Columns[7].Visible = false;      //결과값코드
            SS2_Sheet1.Columns[8].Visible = false;      //변경
            SS2_Sheet1.Columns[9].Visible = false;      //ROWID
            SS2_Sheet1.Columns[10].Visible = false;     //Result Type
            SS2_Sheet1.Columns[13].Visible = false;     //helpcode   

            SS3_Sheet1.Columns[7].Visible = false;      //결과값코드
            SS3_Sheet1.Columns[8].Visible = false;      //변경
            SS3_Sheet1.Columns[9].Visible = false;      //ROWID    

            ssChk_Sheet1.Columns[7].Visible = false;    //Part
            ssChk_Sheet1.Columns[8].Visible = false;    //접수번호

            sp.Spread_All_Clear(ssJepList);
            ssJepList.ActiveSheet.RowCount = 50;

            dtpSDate.Text = clsPublic.GstrSysDate;
            grpBP.Visible = false;
            lblBP.Text = "";

            if (clsType.User.IdNumber != "4349")
            {
                if (clsCompuInfo.gstrCOMIP != "192.168.41.68")
                {
                    Menu10.Visible = false; //스트레스검사
                }
            }

            //관리자작업창
            Menu16.Enabled = false;
            if (hicBcodeService.GetMenuSetAuthoritybyIdNumber("HIC_MENU권한_관리자작업", clsType.User.IdNumber.Trim()) > 0)
            {
                Menu16.Enabled = true;
            }

            //입력담당 선택
            //cboPart.Items.Clear();

            //lblDateTime.Text = "";
            //lblDateTime.Text = clsPublic.GstrSysDate + " " + clsVbfunc.GetYoIl(clsPublic.GstrSysDate) + " " + clsPublic.GstrSysTime;

            this.Text += " " + "☞작업자: " + clsType.User.UserName;

            FstrBDate = clsPublic.GstrSysDate;
            dtpSDate.Text = clsPublic.GstrSysDate;

            if (VB.Pstr(cboPart.Text, ".", 1) == "**") //전체
            {
                lblExTitle.Text = "**.전체";
            }

            fn_Screen_Clear();

            SS3.Visible = false;
            SS4.Visible = false;
            ssJepList.Visible = true;

            tabControl1.SelectedIndex = 1;

            //lblExTitle.Text = cboPart.Text;

            if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
            {
                HIC_EXCODE haroomlist = hicExcodeService.Read_HaRoom(VB.Pstr(cboPart.Text, ".", 1));

                if (!haroomlist.IsNullOrEmpty())
                {
                    FstrRoom = haroomlist.HAROOM;
                }
                else
                {
                    FstrRoom = "";
                }
            }
            else //파트가 접수(전체)일때
            {
                FstrRoom = "ALL";
            }

            if (clsHcVariable.GstrHeaInbodySendYN == "Y")
            {
                //chkInBodySend.Visible = true;
                chkInBodySend.Checked = true;
            }
            else
            {
                chkInBodySend.Visible = false;
                chkInBodySend.Checked = false;
            }

            txtWrtNo.Focus();

            if (VB.Pstr(lblExTitle.Text, ".", 1) != "**" && !lblExTitle.Text.IsNullOrEmpty())
            {
                if (cboPart.FindString("3.체성분") > -1)
                {
                    if (chkInBodySend.Checked == true)
                    {
                        if (blnExcelFileDownload == false)
                        {
                            fn_InBody_Connect();                           
                        }
                    }
                }

                if (strPart ==  "13")
                {
                    gDtSpecCode = lbExSQL.sel_EXAM_SPECODE_Code(clsDB.DbCon);//검체등 기본코드 메모리 Load
                    //gsWard = "TO";    //PC의병동코드(건강증진센터) //사용하는곳 없음.
                }

                //검사실 바코드 자동인쇄요청 PC 여부 설정
                FbExamBarCodeReq = false;
                BarCode_AutoPrint();

                timer2.Enabled = false;
                if (lblExTitle.Text == "8.안압 검사")
                {
                    //FstrBuffer = "";
                    ////Serial 통신
                    ////사용가능한 통신포트 를 찾아서 cboComPort에 담는다.
                    //cboComPort.Items.Clear();

                    //foreach (string comport in SerialPort.GetPortNames())                
                    //{
                    //    cboComPort.Items.Add(comport);
                    //}

                    //cboComPort.SelectedIndex = 0;

                    //if (null == m_sp)
                    //{
                    //    m_sp.PortName = cboComPort.Text; //"COM1";                 
                    //    m_sp.BaudRate = Convert.ToInt32("9600");
                    //    m_sp.Open();
                    //}
                    //else
                    //{
                    //    if (!m_sp.IsOpen)
                    //    {
                    //        m_sp.Open();
                    //    }
                    //}
                    timer2.Enabled = true;
                }
            }
            else
            {
                timer2.Enabled = false;
            }

            txtWrtNo.Focus();
        }

        /// <summary>
        /// InBody MDB 접속
        /// </summary>
        void fn_InBody_Connect()
        {
            Mdb.Connect_Binding();
            if (clsMdb.strMDBConnYN == "FAIL")
            {
                MessageBox.Show("InBody MDB 접속에 실패 하였습니다!" + "\r\n" + "InBody 메인 PC의 상태를 확인 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkInBodySend.Checked = false;
                timer3.Enabled = false;
                Mdb.DBClose();
            }
            else
            {
                fn_Chk_InBody();
                if (blnMdbSaveFlag == false)
                {
                    return;
                }
                timer3.Enabled = true;
            }
            Mdb.DBClose();
        }

        /// <summary>
        /// InBody 환자정보 존재 여부 확인
        /// </summary>
        bool fn_InBody_Read_PatInfo(string argUserId)
        {
            bool rtnVal = false;
            string strBirthDay = "";
            int result = 0;

            //Mdb.Connect_Binding();

            DataTable dt = new DataTable();

            string strSql = "SELECT USER_ID FROM USER_INFO1_TBL WHERE USER_ID ='" + argUserId + "'";
            DataSet ds = Mdb.GetDataSet(strSql);

            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// InBody 환자 정보 전송
        /// </summary>
        void fn_InBody_PatInfo_Send(string argPtNo, string argSName, string argSex, string argJumin)
        {
            string strBirthDay = "";
            string strSysdate = "";
            string SQL = "";
            int result = 0;

            strSysdate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + ":00";

            strBirthDay = ComFunc.GetBirthDate(VB.Left(argJumin, 6), VB.Mid(argJumin, 7, 7), "-");

            SQL = "INSERT INTO USER_INFO1_TBL                                                   ";
            SQL += "      (USER_ID, NAME, GENDER, AGE, BirthDay, USER_REG_DATE, UPDATE_DATE)    ";
            SQL += "VALUES (                                                                    ";
            SQL += "      '" + argPtNo + "'                                                     ";
            SQL += "    , '" + argSName + "'                                                    ";
            SQL += "    , '" + argSex + "'                                                      ";
            SQL += "    ,  " + hb.READ_HIC_AGE_GESAN(argJumin) + "                              ";
            SQL += "    , '" + strBirthDay + "'                                                 ";
            SQL += "    , '" + strSysdate + "'                                                  ";
            SQL += "    , '" + strSysdate + "'                                                  ";
            SQL += "      )                                                                     ";
            result = Mdb.ExcuteQuery(SQL);

            if (result < 0)
            {
                MessageBox.Show("InBody 장비로 환자 정보 저장 중 오류발생!, 전산팀으로 문의 바랍니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                blnMdbSaveFlag = false;
                return;
            }
        }

        ///// <summary>
        ///// InBody 최초 종검 수검자 인적 정보 Excel 파일 제공 => Direct로 MDB에 전송 하는 방법으로 변경(2020.11.12)
        ///// 2020.10.27 이상훈
        ///// </summary>
        //void fn_Chk_InBody_BAK()
        //{
        //    int nRow = 0;
        //    string strJumin = "";
        //    string strBirth = "";
        //    int nNewPatCnt = 0;

        //    sp.Spread_All_Clear(ssInbodyList);

        //    List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetJepsuListbyToDayAll();

        //    ssInbodyList.ActiveSheet.RowCount = list.Count;
        //    if (list.Count > 0)
        //    {   
        //        nRow = 0;
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            if (fn_InBody_Read_PatInfo(list[i].PTNO) == false)
        //            {
        //                nRow += 1;
        //                if (ssInbodyList.ActiveSheet.RowCount < nRow)
        //                {
        //                    ssInbodyList.ActiveSheet.RowCount = nRow;
        //                }

        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].SNAME.Trim();
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].PTNO.Trim();
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 2].Text = "150";
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SEX;
        //                //ssInbodyList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].BIRTHDAY;
        //                strBirth = list[i].BIRTHDAY;
        //                if (strBirth.IsNullOrEmpty())
        //                {
        //                    strJumin = clsAES.DeAES(list[i].JUMIN2);
        //                    ssInbodyList.ActiveSheet.Cells[nRow - 1, 4].Text = ComFunc.GetBirthDate(VB.Left(strJumin, 6), VB.Mid(strJumin, 7, 7), "-").Replace("-", ".");
        //                }
        //                else
        //                {
        //                    ssInbodyList.ActiveSheet.Cells[nRow - 1, 4].Text = strBirth.Replace("-", ".");
        //                }
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].AGE.To<string>();
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HPHONE == null ? "" : list[i].HPHONE.Replace("-", "");
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].TEL == null ? "" : list[i].TEL.Replace("-", "");
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].MAILCODE;
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].ADDRESS;
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].EMAIL;
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 11].Text = clsPublic.GstrSysDate.Replace("-", ".");
        //                ssInbodyList.ActiveSheet.Cells[nRow - 1, 12].Text = "";

        //                nNewPatCnt += 1;
        //            }
        //        }

        //        ssInbodyList.ActiveSheet.RowCount = nRow;

        //        //if (nNewPatCnt > 0)
        //        {
        //            string strPath = "C:\\ExcelFileDown\\";
        //            string strFileName = "C:\\ExcelFileDown\\" + "InBody 연동 신규 수검자 자명단_" + clsPublic.GstrSysDate;

        //            Dir_Check(strPath, "*.*");

        //            hc.Excel_File_Create(strPath, strFileName, ssInbodyList, ssInbodyList_Sheet1);

        //            blnExcelFileDownload = true;
        //        }
        //    }
        //}

        /// <summary>
        /// InBody 최초 종검 수검자 인적 정보 Excel 파일 제공
        /// 2020.10.27 이상훈
        /// </summary>
        void fn_Chk_InBody()
        {
            int nRow = 0;
            string strJumin = "";
            string strBirth = "";
            int nNewPatCnt = 0;

            sp.Spread_All_Clear(ssInbodyList);

            List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetJepsuListbyToDayAll();

            ssInbodyList.ActiveSheet.RowCount = list.Count;
            if (list.Count > 0)
            {
                nRow = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (fn_InBody_Read_PatInfo(list[i].PTNO) == false)
                    {
                        fn_InBody_PatInfo_Send(list[i].PTNO, list[i].SNAME, list[i].SEX, clsAES.DeAES(list[i].JUMIN2));
                        if (blnMdbSaveFlag == false)
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void Dir_Check(string sDirPath, string sExe = "*.*")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }
        }

        void fn_Screen_Display()
        {
            string strRowId = "";

            blnExitFlag = false;

            strToDate = dtpSDate.Value.ToShortDateString();
            strYYYY = strToDate.Substring(0, 4);
            strPart = VB.Pstr(cboPart.Text, ".", 1);

            chkCorrectedVision.Checked = false;
            chkCorrectedHearing.Checked = false;

            if (strPart == "W")
            {
                btnSave.Enabled = false;
            }

            if (!txtWrtNo.Text.Trim().IsNullOrEmpty())
            {
                FnWRTNO = txtWrtNo.Text.To<long>();
            }

            lblXray.Text = "";
            FnOLD_Height = 0;
            FnOLD_Weight = 0;
            strAllWrtno = "";
            
            if (FstrPartG.IsNullOrEmpty())
            {
                MessageBox.Show("검사항목이 설정되지 않았습니다." + "\r\n" + "PC환경설정에서 검사항목을 설정하여 주십시오.", "검사항목미설정", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                blnExitFlag = true;
                return;
            }

            lblResultReceivePosition.Text = "";
            lblResultReceivePosition.Text = heaJepsuService.GetResultReceivePositionbyWrtNo(FnWRTNO, dtpSDate.Text);

            if (strPart == "W")
            {
                btnSave.Enabled = false;
            }

            //삭제된것 체크
            if (hb.READ_JepsuSTS_Hea(FnWRTNO) == "D")
            {
                MessageBox.Show("접수번호(" + FnWRTNO + ")는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                blnExitFlag = true;
                return;
            }

            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 30;
            sp.SetfpsRowHeight(SS2, 35);

            FbUAScanSave = false;

            if (FnWRTNO == 0)
            {
                MessageBox.Show("접수번호가 없습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                blnExitFlag = true;
                return;
            }
            else
            { 
                ufn_Update_Patient_Call();
                if (blnExitFlag == true)
                {
                    return;
                }
                ufn_Screen_Injek_display();         //인적사항을 Display             
                if (blnExitFlag == true)
                {
                    return;
                }
                ufn_Screen_Exam_Items_display();    //검사항목을 Display                    
                ufn_Screen_OLD_Result_Display(FstrSDate);    //종전 2개의 결과 보여줌

                //바코드 자동발행 요청
                if (FbExamBarCodeReq == true)
                {
                    //clsDB.setBeginTran(clsDB.DbCon);
                    //2020.11.02 바코드 요청 History 추가
                    int result = hicBarcodeReqService.HIC_BARCODE_REQ_Insert_His(FnWRTNO, "2", clsType.User.IdNumber, clsCompuInfo.gstrCOMIP);

                    if (result < 0)
                    {
                        blnExitFlag = true;
                        return;
                    }

                    result = 0;

                    result = hicBarcodeReqService.HIC_BARCODE_REQ_Insert(FnWRTNO, "2");

                    if (result < 0)
                    {
                        blnExitFlag = true;
                        return;
                    }


                    //검체바코드 발행 (혈액ACT 파트에서만 작동) - KMC



                }

                ACTING_CHECK(FnWRTNO, strToDate, FnPano);
                
                ufn_Screen_SpcExam_Display();   //일특항목 표시

                //BP 인터페이스
                //Work_BP_Control();
            }
        }

        void Work_BP_Control()
        {
            long lngHandle = 0;
            long chlHandle = 0;
            long chlHandle2 = 0;
            long hCmd;
            string strWRTNO = "";

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

        bool WindowTextSet(long hWnd, string strText)
        {
            bool rtnVal = false;

            rtnVal = (SendMessage(hWnd, WM_SETTEXT, strText.Length, strText) != 0);

            return rtnVal;
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

        void ufn_Update_Patient_Call()
        {   
            //int result = heaSangdamWaitService.Update_Patient_Call(FnWRTNO, FstrRoom);
            int result = heaSangdamWaitService.Update_Patient_Call(FnWRTNO, FstrRoomG); 

            if (result < 0)
            {
                blnExitFlag = true;
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
                blnExitFlag = true;
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
                ssPatInfo.ActiveSheet.Cells[2, 1].Text = "";
                //ssPatInfo.ActiveSheet.Cells[3, 1].Text = PtInfolist.JEPSUJONG;
                ssPatInfo.ActiveSheet.Cells[3, 1].Text = heaSunapdtlService.GetMainSunapDtlCodeNameByWrtno(FnWRTNO);

                HEA_JEPSU Jepsulist = heaJepsuService.Read_Jepsu(FnPano, FstrSDate);

                if (!Jepsulist.IsNullOrEmpty())
                {
                    if (Jepsulist.RID != null)
                    {
                        lblSDate.Text = "최종수검일" + "\r\n" + Jepsulist.SDATE1;
                    }
                    else
                    {
                        lblSDate.Text = "검진첫회";
                    }
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

                ssPatInfo.ActiveSheet.Cells[2, 1].Text = strExams;

                //일반검진 내역 조회
                //HIC_JEPSU HicJepsulist = hicJepsuService.Read_Jepsu(FnPano, FstrSDate);
                FnHcWRTNO = 0;
                HIC_JEPSU HicJepsulist = hicJepsuService.GetWrtNobyPtNoJepDate(FstrPtno, strJepDate);

                if (!HicJepsulist.IsNullOrEmpty())
                {
                    FnHcWRTNO = HicJepsulist.WRTNO;
                }

                conHcPatInfo1.SetDisPlay("25420", "O", FstrSDate, FstrPtno, "TO", strGjJong);
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

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            List<EXAM_DISPLAY> ExamDspList = examDisplayService.Read_ExamList(FnWRTNO, FstrPartG, "1", null);

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

                ufn_Combo_Set(strResCode, nRow - 1);
                Application.DoEvents();

                if (strExCode == "ZD04")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].BackColor = Color.FromArgb(128, 255, 255);
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].BackColor = Color.FromArgb(255, 255, 255);
                }

                if (!strResCode.IsNullOrEmpty() && !strResult.IsNullOrEmpty())
                {
                    HIC_RESCODE ResCodeList = hicRescodeService.Read_ResCode_Single(strResCode, VB.Left(strResult, 2));

                    if (!ResCodeList.IsNullOrEmpty())
                    {
                        strCode = ResCodeList.CODE;
                        FstrCboCode = strCode;
                        strName = ResCodeList.NAME;
                        FstrCboName = strName;

                        clsSpread.gSdCboItemFindLeft(SS2, nRow - 1, (int)clsHcType.Instrument_Result.RESULT, 2, strCode); //Spread Combo Item 찾아서 매칭
                    }
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
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].CellType = txt;

                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = "";
                    }
                }

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
                if (!strResCode.IsNullOrEmpty())
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                if (ExamDspList[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text = "*";
                }

                if (ExamDspList[i].EXCODE == "A103")
                {   
                    strTemp = hm.Biman_Gesan(FnWRTNO, "HEA");                    

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
                    
                    if (!FstrCboCode.IsNullOrEmpty())
                    {
                        //clsSpread.gSdCboItemFindLeft(SS2, nRow - 1, (int)clsHcType.Instrument_Result.RESULT, 2, FstrCboCode); //Spread Combo Item 찾아서 매칭
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].CellType = txt;
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = strTemp;
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = VB.Pstr(strTemp, ".", 1);
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = strTemp;
                    }                    
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
                        strNomal = ExamDspList[i].MIN_M + "~" + ExamDspList[i].MAX_M;
                    }
                    else
                    {
                        strNomal = ExamDspList[i].MIN_F + "~" + ExamDspList[i].MAX_F;
                    }
                }
                else
                {
                    HEA_WOMEN WRefList = heaWomenService.Read_Women_Reference(FnWRTNO);

                    if (strExcode == "E908")
                    {
                        strNomal = WRefList.MIN_ONE + "~" + WRefList.MAX_ONE;
                    }
                    else if (strExcode == "E909")
                    {
                        strNomal = WRefList.MIN_TWO + "~" + WRefList.MAX_TWO;
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
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PART].Text = ExamDspList[i].HEAPART; 

                //판정결과를 다시 Check함(L=Low,H=High)                
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text = hb.Result_Panjeng(ExamDspList[i].EXCODE.Trim(), strResult, strNomal);

                if (SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text.Trim() == "L")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].BackColor = Color.FromArgb(250, 210, 220);
                }
                else if (SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PANJENG].Text.Trim() == "H")
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

        void ufn_Combo_Set(string strResCode, int nRow)
        {
            string strList = "";

            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            //자료를 READ
            List<HIC_RESCODE> ResCodeList = hicRescodeService.Read_ResCode(strResCode);

            string[] strComboCode = new string[ResCodeList.Count];

            for (int i = 0; i < ResCodeList.Count; i++)
            {
                strComboCode[i] = ResCodeList[i].CODENAME;
            }

            if (ResCodeList.Count > 1)
            {   
                combo.Items = strComboCode;
                combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                combo.MaxDrop = 6;
                combo.MaxLength = 100;
                combo.ListWidth = 200;
                combo.Editable = true;
                SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.RESULT].CellType = combo;

                SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.HELP].Text = "Y";
            }
        }

        /// <summary>
        /// 종전 2개의 결과 보여줌
        /// </summary>
        void ufn_Screen_OLD_Result_Display(string argJepDate)
        {   
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1).Text = "결과 1";
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT2).Text = "결과 2";            

            List<HEA_JEPSU> JepsuList = heaJepsuService.Read_Wrtno_SDate(FnPano, argJepDate);

            nREAD = JepsuList.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    if (i > 2)  break;

                    SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1 + i).Value = JepsuList[i].SDATE;

                    //List<HIC_RESULT> RsltList = hicResultService.Get_Results(JepsuList[i].WRTNO);
                    List<HEA_RESULT> RsltList = heaResultService.Get_Results(JepsuList[i].WRTNO);

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

            tabControl1.SelectedIndex = 1;            

            sp.Spread_All_Clear(ssChk);
            ssChk.ActiveSheet.RowCount = 20;

            strPart = dtpSDate.Text.Left(1);
            
            if (VB.Pstr(lblExTitle.Text, ".", 1) != "**")
            {   
                List<ACTING_CHECK> list = actingCheckService.ACTING_CHECKbyWrtNOGubun11(ArgWRTNO, ArgDate);

                nREAD = list.Count;
                sp.SetfpsRowHeight(ssChk, 32);
                ssChk.ActiveSheet.RowCount = nREAD;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        nRow1 += 1;
                        nRow += 1;

                        if (i == 0)
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HAROOM;
                            strOldHaRoom = list[i].HAROOM;
                            nRow2 = nRow;
                        }

                        ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HAROOM;
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

                        ssChk.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                        if (bColor == true)
                        {
                            ufn_Line_Color(nRow - 1);
                        }
                        //상태점검
                        //List<HIC_RESULT_ACTIVE> Rsltlist = hicResultActiveService.Read_Result(FnWRTNO, list[i].HEAPART);
                        List<HEA_RESULT> Rsltlist = heaResultService.Read_ResultAct(FnWRTNO, list[i].HEAPART);

                        if (Rsltlist.Count == 0)
                        {
                            //List<HIC_RESULT_ACTIVE> Actlist2 = hicResultActiveService.Read_Active(FnWRTNO, list[i].HEAPART);
                            List<HEA_RESULT> Actlist2 = heaResultService.Read_Active(FnWRTNO, list[i].HEAPART);
                            if (Actlist2.Count > 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "미검";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                            else
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "완료";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                                //ssChk.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                            }
                        }
                        else
                        {
                            //List<HIC_RESULT_ACTIVE> Rsltlist2 = hicResultActiveService.Read_Result2(FnWRTNO, list[i].HEAPART);
                            List<HEA_RESULT> Rsltlist2 = heaResultService.Read_Result(FnWRTNO, list[i].HEAPART);

                            if (Rsltlist2.Count > 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "미검";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                            else
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "완료";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                                //ssChk.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                            }
                        }

                        if (ArgWRTNO == 0)
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "";
                        }

                        //대기점검
                        List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait(ArgDate, list[i].HEAPART);

                        nRead2 = Waitlist.Count;
                        
                        if (nRead2 == 0)
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 2].Text = "0";
                        }
                        else
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 2].Text = nRead2.To<string>();
                        }

                        //검사실 대기인원
                        HEA_SANGDAM_WAIT ExamWaitlist = waitCheckService.Read_Exam_Wait(ArgDate, list[i].HAROOM);

                        if (!ExamWaitlist.IsNullOrEmpty())
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = ExamWaitlist.CNT.To<string>();

                            if (list[i].HAROOM == "99")
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                            }
                        }

                        if (list[i].HAROOM.IsNullOrEmpty())
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = "0";
                        }

                        ssChk.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].HEAPART;

                        if (i < list.Count - 1)
                        {
                            if (strOldHaRoom != list[i + 1].HAROOM)
                            {
                                if (nRow1 > 0)
                                {
                                    ssChk_Sheet1.AddSpanCell(nRow2 - 1, 3, nRow1, 1);
                                    ssChk_Sheet1.AddSpanCell(nRow2 - 1, 4, nRow1, 1);
                                    ssChk_Sheet1.AddSpanCell(nRow2 - 1, 6, nRow1, 1);
                                }
                                nRow1 = 0;
                            }
                        }
                    }

                    //현 검사실 대기명단
                    List<HEA_SANGDAM_WAIT> NowExamWaitlist = heaSangdamWaitService.Read_Now_Wait(FstrRoom);

                    if (NowExamWaitlist.Count > 0)
                    {
                        for (int i = 0; i < NowExamWaitlist.Count; i++)
                        {
                            ssChk.ActiveSheet.Cells[i, 5].Text = NowExamWaitlist[i].SNAME;
                            ssChk.ActiveSheet.Cells[i, 8].Text = NowExamWaitlist[i].WRTNO.To<string>();
                        }
                    }
                }
            }        
        }

        void ufn_Line_Color(int argRow)
        {
            ssChk.ActiveSheet.Cells[argRow, 0].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[argRow, 1].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[argRow, 2].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[argRow, 3].BackColor = Color.FromArgb(255, 255, 128);
        }

        /// <summary>
        /// 종합검진+일특 누락방지
        /// </summary>
        void ufn_Screen_SpcExam_Display()
        {
            //배치전 소음
            List<HIC_SUNAPDTL> UCodelist = hicSunapdtlService.Read_UCode(FstrPtno, FstrSDate);

            for (int i = 0; i <= 6; i++)
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
            List<HIC_RESULT> RsltList = hicResultService.Read_Result(FstrPtno, FstrSDate);

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
                        case "TH51":
                            strSpcExam[6] = "N"; //정밀청력
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
                            case "TH51":
                                strSpcExam[6] = "Y"; //정밀청력
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //종검에서 검사여부
            //List<HIC_RESULT> RsltList2 = hicResultService.Read_Result2(FnWRTNO);
            List<HEA_RESULT> RsltList2 = heaResultService.Read_Result(FnWRTNO);

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
                        strSpcExam[4] = "Y";
                        break;
                    default:
                        break;
                }
            }

            if (!strSpcExam[5].IsNullOrEmpty())
            {
                strSpcExam[3] = "";
            }


            if (VB.InStr(ssPatInfo.ActiveSheet.Cells[1, 3].Text, "■일특") == 0)
            {
                if (!strSpcExam[0].IsNullOrEmpty() || !strSpcExam[1].IsNullOrEmpty() || !strSpcExam[2].IsNullOrEmpty() || !strSpcExam[3].IsNullOrEmpty() || !strSpcExam[4].IsNullOrEmpty() || !strSpcExam[5].IsNullOrEmpty() || !strSpcExam[6].IsNullOrEmpty())
                {
                    strTemp = "■일특:";
                    for (int i = 0; i < 7; i++)
                    {
                        if (!strSpcExam[i].IsNullOrEmpty())
                        {
                            ssChk.ActiveSheet.RowCount += 1;
                            switch (i)
                            {
                                case 0:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "특수소변";
                                    strTemp += "특수소변,";
                                    break;
                                case 1:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "객담";
                                    strTemp += "객담,";
                                    break;
                                case 2:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "폐활량 3회";
                                    strTemp += "폐활량 3회,";
                                    break;
                                case 3:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "특수소음";
                                    strTemp += "특수소음,";
                                    break;
                                case 4:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "진동";
                                    strTemp += "진동,";
                                    break;
                                case 5:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "채배소음";
                                    strTemp += "채배소음,";
                                    break;
                                case 6:
                                    ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 0].Text = "청력정밀(1층)";
                                    strTemp += "청력정밀(1층),";
                                    break;
                                default:
                                    break;
                            }
                            ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 1].Text = "";
                            if (strSpcExam[i] == "Y")
                            {
                                ssChk.ActiveSheet.Cells[ssChk.ActiveSheet.RowCount - 1, 1].Text = "완료";
                            }
                        }
                    }

                    strTemp = VB.Left(strTemp, strTemp.Length - 1) + "■";

                    if (VB.InStr(ssPatInfo.ActiveSheet.Cells[1, 3].Text, "■일특:") > 0)
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 3].Text = strTemp;
                    }
                    else if (VB.InStr(ssPatInfo.ActiveSheet.Cells[1, 3].Text, strTemp) == 0)
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 3].Text = strTemp + ssPatInfo.ActiveSheet.Cells[1, 3].Text;
                    }
                }
            }

            if (strGbNaksang == "Y")
            {
                if (VB.InStr(ssPatInfo.ActiveSheet.Cells[1, 3].Text, "★낙상주의") == 0)
                {
                    ssPatInfo.ActiveSheet.Cells[1, 3].Text = "★낙상주의" + ssPatInfo.ActiveSheet.Cells[1, 3].Text;
                }
            }
        }

        void BarCode_AutoPrint()
        {
            List<HIC_BCODE> list = hicBcodeService.GetCodeNamebyBcode("EXAM_바코드인쇄요청PC", clsCompuInfo.gstrCOMIP);
            if (list.Count > 0)
            {
                FbExamBarCodeReq = true;
            }
            else
            {
                FbExamBarCodeReq = false;
            }
        }
    }
}
