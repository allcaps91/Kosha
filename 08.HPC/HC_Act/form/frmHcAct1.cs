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
using System.IO.Ports;
using ComLibB;
using System.Threading;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcAct1.cs
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
    public partial class frmHcAct1 : BaseForm
    {

        List<HIC_BCODE> lstBlnfo = new List<HIC_BCODE>();    //채혈안내문

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
        HicResultExCodeService hicResultExCodeService = null;
        EtcJupmstService etcJupmstService = null;

        ComFunc cF = new ComFunc();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcAct hcact = new clsHcAct();
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsHaBase ha = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();
        clsSpread sp = null;
        clsHcFunc hf = new clsHcFunc();

        //Serial 통신
        SerialPort m_sp = new SerialPort();
        delegate void SetTextCallBack(string opt);

        frmHcPendListReg FrmHcPendListReg = null;
        frmHcActPFTMunjin FrmHcActPFTMunjin = null;
        frmHcActMemoSave FrmHcActMemoSave = null;
        frmHcAdmin_Job FrmHcAdmin_Job = null;
        frmHcActRemarkMgmt FrmHcActRemarkMgmt = null;
        frmHcMemo FrmHcMemo = null;
        frmViewResult FrmViewResult = null;
        frmPftSomoTong FrmPftSomoTong = null;
        frmEkgScanList FrmEkgScanList = null;
        string strRemarkItem = "";

        //FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

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
        string fstrJepDate1 = ""; //바코드날짜
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
        double FnOLD_Waist;       //종전 허리둘게

        double FnOLD_TZ08;       //종전 악력(좌)
        double FnOLD_TZ09;       //종전 악력(우)

        double FnOLD_A104;       //종전 시력(좌)
        double FnOLD_A105;       //종전 시력(우)

        ////////////////////////////////////////////////

        ////////////////////////////////////////////////
        ///Screen_Display 변수
        ///////////////////////////////////////////////
        string strBlood = "";
        string strSex = "";
        string strPart = "";

        int nREAD = 0;
        int nRow = 0;
        string strExCode = "";
        string strHName = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strNomal = "";
        string strGbHelp = "";

        string strResultTmp = "";   //결과 임시저장소

        long nHeaPano = 0;
        string strXrayno = "";

        string strToDate = "";
        string strTemp = "";
        string strYYYY = "";
        string[] strSpcExam = new string[7];

        int nCNT = 0;
        string strJumin;
        string str낙상주의;
        List<long> strAllWrtno = new List<long>();

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

        //string strCODE = "";
        string strNewPan = "";
        string strGb2Audio = "";
        int nHeight = 0;
        double nCha = 0;

        //접수마스타의 상태 설정용 변수
        int nNullCNT = 0;
        string strExamRemark = "";
        string strDispOk = "";  //키/몸무게 종전 기록과 5 이상 차이 여부
        string strWaistDispOk = "";  //허리둘게 종전 기록과 5 이상 차이 여부
        string strTZ0809 = ""; //악력(좌,우)
        string strA104A105 = ""; //시력(좌,우)
        string strPointDispOk = ""; //".." 입력 제한
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

        bool boolSort = false;
        bool blnExitFlag = false;

        //string FStatus = "";

        public frmHcAct1()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        void SetControl()
        {
            sp = new clsSpread();

            hc.Read_INI(CboPart);

            for (int i = 0; i < CboPart.Items.Count; i++)
            {
                CboPart.SelectedIndex = i;
                FstrPartG.Add(VB.Pstr(CboPart.Text, ".", 1));
            }

            SheetView shv = SS2.ActiveSheet;
            InputMap im = new InputMap();
            Keystroke k = new Keystroke(Keys.Enter, Keys.None);
            im = SS2.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(k, SpreadActions.MoveToNextRow);
            im = SS2.GetInputMap(InputMapMode.WhenFocused);
            im.Put(k, SpreadActions.MoveToNextRow);
            //if (CboPart.Items.Count > 0)
            //{
            //    CboPart.SelectedIndex = 0;
            //}
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
            patientInfoService = new PatientInfoService();
            hicResultExCodeService = new HicResultExCodeService();
            etcJupmstService = new EtcJupmstService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSang.Click += new EventHandler(eBtnClick);
            this.btnPatSearch.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);
            
            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timer2.Tick += new EventHandler(eTimerTick);
            this.timer3.Tick += new EventHandler(eTimerTick);
            this.timerHeight.Tick += new EventHandler(eTimerTick);
            this.timerUA.Tick += new EventHandler(eTimerTick);

            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTextBoxKeyPress);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTextBoxKeyPress); 

            this.tabControl1.Click += new EventHandler(eTabClick);

            this.ssChk.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssChk.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssJepList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssJepList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpreadClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.EditModeOn += new EventHandler(eSpreadEditModeOn);
            this.SS2.EditModeOff += new EventHandler(eSpreadEditModeOff);
            this.SS2.Change += new ChangeEventHandler(eSpreadChange);
            this.SS2.KeyDown += new KeyEventHandler(eSpreadKeyDown);
            this.SS2.KeyUp += new KeyEventHandler(eSpreadKeyUp);
            this.SS2.LeaveCell += new LeaveCellEventHandler(eSpreadLeaveCell);
            
            this.chkCorrectedVision.Click += new EventHandler(eCheckBoxClick);
            this.chkCorrectedHearing.Click += new EventHandler(eCheckBoxClick);

            this.btnRemarkInput.Click += new EventHandler(eBtnClick);

            this.Menu01.Click += new EventHandler(eMenuClick);  //방사선번호
            this.Menu02.Click += new EventHandler(eMenuClick);  //접수자명단조회   => 사용무
            this.Menu03.Click += new EventHandler(eMenuClick);  //검사실번호
            this.Menu04.Click += new EventHandler(eMenuClick);  //과거결과조회
            this.Menu05.Click += new EventHandler(eMenuClick);  //상담완료일괄정리   => 사용무
            this.Menu06.Click += new EventHandler(eMenuClick);  //방사선자동액팅
            this.Menu07.Click += new EventHandler(eMenuClick);  //청력검사
            this.Menu08.Click += new EventHandler(eMenuClick);  //보류대장
            this.Menu09.Click += new EventHandler(eMenuClick);  //팀파노검사
            this.Menu10.Click += new EventHandler(eMenuClick);  //폐활량검사표
            this.Menu11.Click += new EventHandler(eMenuClick);  //관리자작업
            this.Menu12.Click += new EventHandler(eMenuClick);  //검사결과 메모입력
            this.Menu13_01.Click += new EventHandler(eMenuClick);  //EMR
            this.Menu13_02.Click += new EventHandler(eMenuClick);  //OCS 결과
            this.Menu14.Click += new EventHandler(eMenuClick);  //EMR
            this.Menu15.Click += new EventHandler(eMenuClick);  //OCS 결과
            this.Menu16_01.Click += new EventHandler(eMenuClick);  //상담,검사실번호설정
            this.Menu16_02.Click += new EventHandler(eMenuClick);  //상담 및 판정 통보일변경
            this.Menu17.Click += new EventHandler(eMenuClick);  //폐활량소모통계
            this.Menu18.Click += new EventHandler(eMenuClick);  //EKG명단조회

            this.cboComPort.SelectedIndexChanged += new EventHandler(eCboChage);
        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == Menu01)   //방사선번호
            {
                fn_menuList();
            }
            else if (sender == Menu02)  //접수자명단조회 => 사용무
            {
                fn_menuList();
                
            }
            else if (sender == Menu03)  //검사실번호
            {
                frmRoomNoSet frm = new frmRoomNoSet();
                frm.ShowDialog();
            }
            else if (sender == Menu04)  //과거결과조회
            {
                frmHcPanPersonResult frm = new frmHcPanPersonResult("frmHcAct1", FstrPtno, FstrName);
                frm.ShowDialog();
            }
            else if (sender == Menu05)  //상담완료일괄정리  ==> 사용무(2020-10-05 요청)
            {
                //frmCounselMissCheck frm = new frmCounselMissCheck();
                //frm.ShowDialog();
            }
            else if (sender == Menu06)  //방사선자동액팅
            {
                frmXrayAutoActing frm = new frmXrayAutoActing();
                frm.ShowDialog();
            }
            else if (sender == Menu07)  //청력검사
            {
                string strPath = "";
                string strREC = "";
                string strJong = "";
                string strData = "";
                string strWRTNO = "";

                strJong = FstrGjJong;
                strWRTNO = txtWrtNo.Text;

                List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetHNamebyWrtNo(strWRTNO);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (strData.IsNullOrEmpty())
                        {
                            strData = list[i].HNAME;
                        }
                        else
                        {
                            strData += list[i].HNAME + ","; 
                        }
                    }
                }

                strREC = "";

                strPath = @"c:\Audio_Interface\WorkList.txt";

                strREC = ssPatInfo.ActiveSheet.Cells[0, 1].Text + "\t" + FstrPtno + "\t"; //성명,등록번호
                strREC += VB.Left(ssPatInfo.ActiveSheet.Cells[0, 3].Text, 6) + "\t"; //생년월일
                strREC += FstrSex + "\t" + FnAge + "\t" + "HR" + "\t" + "청력검사" + "\t" + FnWRTNO;
                strREC += "\t" + clsType.User.UserName + "\t" + hb.READ_GjJong_Name(strJong) + "\t" + strData;

                DirectoryInfo dir = new DirectoryInfo(@"c:\Audio_Interface");
                if (dir.Exists == false)
                {
                    dir.Create();
                }

                clsVbfunc.WriteFile(strPath, strREC);

                Fstr청력인터페이스 = "OK";
                timer1.Enabled = true;

                MessageBox.Show("청력검사 인터페이스 저장 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == Menu08)  //보류대장
            {
                if (FnWRTNO > 0)
                {
                    FrmHcPendListReg = new frmHcPendListReg(FnWRTNO, "1");
                    FrmHcPendListReg.ShowDialog(this);
                }
            }
            else if (sender == Menu09)  //팀파노검사
            {
                string strFileName = "";

                //기존화일 삭제
                strFileName = @"c:\cmc\";

                DirectoryInfo Dir = new DirectoryInfo(strFileName);

                if (Dir.Exists == false)
                {
                    Dir.Create();
                }
                else
                {
                    FileInfo[] File = Dir.GetFiles("*.jpg", SearchOption.AllDirectories);

                    foreach (FileInfo file in File)
                    {
                        file.Delete();
                    }
                }

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDateOnlyDeptCode(FstrPtno, fstrJepDate1, "Y", "6", "HR");

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        hf.AudioFILE_DBToFile1(list[i].ROWID, "1", i);
                    }
                }
            }
            else if (sender == Menu10)  //폐활량검사표
            {
                if (txtWrtNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("수검자가 선택 되지 않았습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FrmHcActPFTMunjin = new frmHcActPFTMunjin("HCACT", "HIC", txtWrtNo.Text.To<long>(), FstrPtno, txtWrtNo.Text.To<long>());
                FrmHcActPFTMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcActPFTMunjin.ShowDialog(this);
            }
            else if (sender == Menu11)  //관리자작업
            {
                //if (txtWrtNo.Text.IsNullOrEmpty())
                //{
                //    MessageBox.Show("수검자가 선택 되지 않았습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                FrmHcAdmin_Job = new frmHcAdmin_Job();
                FrmHcAdmin_Job.StartPosition = FormStartPosition.CenterParent;
                FrmHcAdmin_Job.ShowDialog(this);
            }
            else if (sender == Menu12)
            {
                //FrmHcActMemoSave = new frmHcActMemoSave(txtWrtNo.Text.To<long>());
                //FrmHcActMemoSave.StartPosition = FormStartPosition.CenterParent;
                //FrmHcActMemoSave.ShowDialog(this);

                FrmHcMemo = new frmHcMemo(FstrPtno);
                FrmHcMemo.StartPosition = FormStartPosition.CenterParent;
                FrmHcMemo.ShowDialog(this);
            }
            else if (sender == Menu13_01)   //EMR_사용X
            {
                clsVbEmr.EXECUTE_NewTextEmrView(FstrPtno);
            }
            else if (sender == Menu13_02)   //OCS결과_사용X
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == Menu14)   //EMR
            {
                clsVbEmr.EXECUTE_NewTextEmrView(FstrPtno);
            }
            else if (sender == Menu15)   //OCS
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == Menu16_01)   //상담,검사실번호 설정
            {
                frmRoomNoSet frm = new frmRoomNoSet();
                frm.ShowDialog();
            }
            else if (sender == Menu16_02)   //상담 및 판정통보일변경
            {
                FrmHcAdmin_Job = new frmHcAdmin_Job();
                FrmHcAdmin_Job.StartPosition = FormStartPosition.CenterParent;
                FrmHcAdmin_Job.ShowDialog(this);
            }
            else if (sender == Menu17)   //폐활량 물품 통계
            {
                FrmPftSomoTong = new frmPftSomoTong();
                FrmPftSomoTong.ShowDialog(this);
            }
            else if (sender == Menu18)   //EKG명단조회
            {
                FrmEkgScanList = new frmEkgScanList();
                FrmEkgScanList.ShowDialog(this);
            }


        }

        void eCboChage(object sender, EventArgs e)
        {
            if (sender == cboComPort)
            {
                if (cboComPort.Text.Trim() == "")
                {
                    return;
                }

                fn_SerialPort_Connect();
            }
        }

        void fn_Screen_Clear()
        {              
            txtWrtNo.Text = "";
            FstrPtno = "";
            FstrName = "";
            FnClickRow = 0;
            FnWrtno2 = 0;
            FnHeaWRTNO = 0;
            lblXray.Text = "";
            FbAutoSave = false;
            Fstr청력인터페이스 = "";
            timer1.Enabled = false;
            FnOLD_Height = 0;
            FnOLD_Weight = 0;
            FnOLD_Waist = 0;
            FnOLD_TZ08 = 0;
            FnOLD_TZ09 = 0;
            FnOLD_A104 = 0;
            FnOLD_A105 = 0;

            lblResultReceivePosition.Text = "";

            sp.Spread_All_Clear(ssList1);

            Menu01.Enabled = false;
            txtXrayNo.Text = "";
            ssList1.Enabled = true;
            btnSave.Enabled = true;

            //인적사항
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[1, 3].Text = "";
            ssPatInfo.ActiveSheet.Cells[2, 1].Text = "";
            ssPatInfo.ActiveSheet.Cells[3, 1].Text = "";

            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 40;
            sp.SetfpsRowHeight(SS2, 35);
            sp.Spread_All_Clear(ssChk);
            ssChk.ActiveSheet.RowCount = 20;
            sp.SetfpsRowHeight(ssChk, 30);
            btnSave.Enabled = true;
            timer1.Enabled = false;
            
            Menu01.Enabled = false;
            txtXrayNo.Text = "";
            ssList1.Enabled = true;
            Menu08.Enabled = false;
            Menu10.Enabled = false;

            timer1.Enabled = false;
            timer3.Enabled = false;
            lblBpH.Text = "0";
            lblBpL.Text = "0";
            txtSName.Text = "";

            conHcPatInfo1.SetDisPlay("25420", "O", clsPublic.GstrSysDate, "", "", "");
            panInfo.Visible = false;
        }

        void eTabClick(object sender, EventArgs e)
        {
            if (sender == tabControl1)
            {
                if (tabControl1.SelectedTab == tabJepsu)
                {
                    //fn_menuList();
                    if (FstrWaitRoom == "10")
                    {
                        txtWrtNo.Focus();
                        txtWrtNo.Select();
                        return;
                    }
                }
                else if (tabControl1.SelectedTab == tabChk)
                {   
                    FnWRTNO = txtWrtNo.Text.To<long>();
                    ACTING_CHECK(FnWRTNO, dtpHicJepDate.Text);                    
                    //ACTING_CHECK_NEW(FnWRTNO, dtpHicJepDate.Text);
                }
                txtWrtNo.Focus();
                txtWrtNo.Select();
            }
        }

        void fn_menuList()
        {
            int nRead = 0;
            int nRead2 = 0;
            long nWrtNo = 0;
            long nHeaPano = 0;
            string strTemp = "";
            string strSDate = "";
            string strGbChul = "";
            string strPtno = "";
            string sSName = "";

            strSDate = dtpFDate.Text;
            tabControl1.TabIndex = 0;
            tabControl1.SelectedTab = tabJepsu;

            ssJepList.Visible = true;

            Cursor.Current = Cursors.WaitCursor;

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

            sSName = txtSName.Text.Trim();

            if (ssJepList.ActiveSheet.RowCount == 0) ssJepList.ActiveSheet.RowCount = 30;
           
            if (!FstrWaitRoom.IsNullOrEmpty())
            {
                List<HIC_JEPSU> list = hicJepsuService.GetWrtnobyGubun(FstrWaitRoom, sSName);

                nRead = list.Count;
                progressBar1.Maximum = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();

                    nWrtNo = list[i].WRTNO;

                    if (nWrtNo > 0)
                    {
                        //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                        List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo, "2");

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
                                ssJepList.ActiveSheet.Cells[i, 2].Text = "X";
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[i, 2].Text = "○";
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
                    progressBar1.Value = i + 1;
                }
            }
            else
            {
                List<HIC_JEPSU> list = hicJepsuService.GetWrtnobyJepDate(dtpHicJepDate.Text, strGbChul, sSName);

                nRead = list.Count;
                ssJepList.ActiveSheet.RowCount = nRead;
                progressBar1.Maximum = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    ssJepList.ActiveSheet.Cells[i, 0].Text = list[i].WRTNO.To<string>();

                    nWrtNo = list[i].WRTNO;

                    if (nWrtNo > 0)
                    {
                        //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                        List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo, "2");

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
                                ssJepList.ActiveSheet.Cells[i, 2].Text = "X";
                            }
                            else
                            {
                                ssJepList.ActiveSheet.Cells[i, 2].Text = "○";
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
                    progressBar1.Value = i + 1;
                }
            }

            Cursor.Current = Cursors.Default;
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

                fn_Screen_Clear();
                txtWrtNo.Focus();
                txtWrtNo.Select();
            }
            else if (sender == btnPatSearch)
            {
                fn_menuList();
            }
            else if (sender == btnRemarkInput)
            {
                FrmHcActRemarkMgmt = new frmHcActRemarkMgmt("INPUT");
                FrmHcActRemarkMgmt.rSetGstrValue += new frmHcActRemarkMgmt.SetGstrValue(Remark_Value);
                FrmHcActRemarkMgmt.ShowDialog();
                FrmHcActRemarkMgmt.rSetGstrValue -= new frmHcActRemarkMgmt.SetGstrValue(Remark_Value);

                if (!strRemarkItem.IsNullOrEmpty())
                {
                    if (ssPatInfo.ActiveSheet.Cells[1, 3].Text.Trim() == "")
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 3].Text = strRemarkItem;
                    }
                    else
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 3].Text += "," + strRemarkItem;
                    }
                }
            }
            else if (sender == btnSave)
            {
                string strRoomCd = "";
                string strCode = "";

                string strMinM = "";
                string strMaxM = "";
                string strMinF = "";
                string strMaxF = "";
                string strAudioMsg = "";

                if (txtWrtNo.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("접수번호가 공란입니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtWrtNo.Focus();
                    txtWrtNo.Select();
                    return;
                }

                btnSave.Enabled = false;

                if (Police_Result_Check() == false)
                {
                    MessageBox.Show("경찰공무원 채용검진의 청력검사 결과를 수치로 입력해주세요!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
                    return;
                }


                //결과출력 확인
                HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(FnWRTNO);
                if(item.PRTSABUN > 0)
                {
                    MessageBox.Show("결과지 출력완료된 수검자입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
                    return;
                }





                //자료에 오류가 있는지 점검함
                strDispOk = "";
                strWaistDispOk = "";
                strPointDispOk = "";
                strTZ0809 = "";
                strA104A105 = "";

                int returnVal1 = 0;

                for (int i = 0; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strCode = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    if (!strResult.IsNullOrEmpty())
                    {

                        //남녀참고치
                        strMinM = "";
                        strMaxM = "";
                        strMinF = "";
                        strMaxF = "";

                        HIC_EXCODE item1 = hicExcodeService.FindOne(strCode);
                        if (!item1.IsNullOrEmpty())
                        {
                            strMinM = item1.MIN_M;
                            strMaxM = item1.MAX_M;
                            strMinF = item1.MIN_F;
                            strMaxF = item1.MAX_F;
                        }

                        if (strCode == "A108" || strCode == "A109")
                        {
                            if (VB.IsNumeric(strResult) == false) //혈압은 숫자만 가능함
                            {
                                MessageBox.Show(i + 1 + "번줄 혈압은 숫자만 가능합니다!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                btnSave.Enabled = true;
                                return;
                            }

                            //혈압 소수점 불가
                            if (int.TryParse(strResult, out returnVal1) == false)
                            {
                                MessageBox.Show(i + 1 + "번줄 혈압결과에 소수점 포함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                btnSave.Enabled = true;
                                return;
                            }

                            if  (strCode == "A108")    //혈압(최고)
                            {
                                if (Convert.ToInt32(strResult) > 250 || Convert.ToInt32(strResult) < 60)
                                {
                                    MessageBox.Show("혈압결과를 확인하세요!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    btnSave.Enabled = true;
                                    return;
                                }
                            }

                            if (strCode == "A109")     //혈압(최저)
                            {
                                if (Convert.ToInt32(strResult) > 250 || Convert.ToInt32(strResult) < 40)
                                {
                                    MessageBox.Show("혈압결과를 확인하세요!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    btnSave.Enabled = true;
                                    return;
                                }
                            }
                        }

                        if (strCode == "A111" || strCode == "A112") //요단백
                        {
                            strResCode = VB.Pstr(SS2.ActiveSheet.Cells[i, 2].Text.Trim(), ".", 1);
                            switch (strResCode)
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
                                    btnSave.Enabled = true;
                                    return;
                            }
                        }

                        //일반검진 시력조건 해당 값만 저장
                        if (strCode == "A104" || strCode == "A105")
                        {
                            if (!strResult.IsNullOrEmpty())
                            {
                                strResultTmp = strResult.Replace("(", "");
                                strResultTmp = strResultTmp.Replace(")", "");
                                
                                if (strResultTmp != "0.1" && strResultTmp != "0.15" && strResultTmp != "0.2" && strResultTmp != "0.3" && strResultTmp != "0.4" && strResultTmp != "0.5" && strResultTmp != "0.6" && strResultTmp != "0.7" && strResultTmp != "0.8" && strResultTmp != "0.9" && strResultTmp != "1.0" && strResultTmp != "1.2" && strResultTmp != "1.5" && strResultTmp != "2.0" && strResultTmp != "9.9")
                                {
                                    MessageBox.Show("시력 기준값이 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    btnSave.Enabled = true;
                                    return;
                                }
                            }
                        }
                        
                        //키,몸무게,허리둘레 체크로직 추가
                        if (strCode == "A101")     //신장
                        {
                            if (Convert.ToDouble(strResult) > 200 || Convert.ToDouble(strResult) < 100)
                            {
                                if (Convert.ToDouble(strResult) != 999.9)
                                {
                                    MessageBox.Show("신장값을 확인하세요!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    btnSave.Enabled = true;
                                    return;
                                }
                            }
                        }
                        if (strCode == "A102")     //체중
                        {
                            if (Convert.ToDouble(strResult) > 200 || Convert.ToDouble(strResult) < 10)
                            {
                                if (Convert.ToDouble(strResult) != 999.9)
                                {
                                    MessageBox.Show("체중값을 확인하세요!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    btnSave.Enabled = true;
                                    return;
                                }
                            }
                        }
                        if (strCode == "A109")     //허리둘레
                        {
                            if (Convert.ToDouble(strResult) > 150 || Convert.ToDouble(strResult) < 50)
                            {
                                if (Convert.ToDouble(strResult) != 999.9)
                                {
                                    MessageBox.Show("허리둘레값을 확인하세요!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    btnSave.Enabled = true;
                                    return;
                                }
                            }
                        }

                        //키,몸무게 종전과 5이상 차이가 발생하면 메세지 표시
                        if (strCode == "A101" && FnOLD_Height != 0)
                        {
                            nCha = FnOLD_Height - strResult.To<double>();
                            if (Math.Abs(nCha) >= 5)
                            {
                                strDispOk = "OK";
                            }
                        }
                        else if (strCode == "A102" && FnOLD_Weight != 0)
                        {
                            nCha = FnOLD_Weight - strResult.To<double>();
                            if (Math.Abs(nCha) >= 5)
                            {
                                strDispOk = "OK";
                            }
                        }

                        //악력(좌) 종전과 차이가 발생하면 메시지 표시
                        if (strCode == "TZ08" && FnOLD_TZ08 != 0 && !FnOLD_TZ08.IsNullOrEmpty())
                        {
                            nCha = FnOLD_TZ08 - strResult.To<double>();
                            if (Math.Abs(nCha) >= 5)
                            {
                                strTZ0809 = "OK";
                            }
                        }

                        //악력(우) 종전과 차이가 발생하면 메시지 표시
                        if (strCode == "TZ09" && FnOLD_TZ09 != 0 && !FnOLD_TZ09.IsNullOrEmpty())
                        {
                            nCha = FnOLD_TZ09 - strResult.To<double>();
                            if (Math.Abs(nCha) >= 5)
                            {
                                strTZ0809 = "OK";
                            }
                        }

                        //시력(좌) 종전과 차이가 발생하면 메시지 표시
                        if (strCode == "A104" && FnOLD_A104 != 0 && !FnOLD_A104.IsNullOrEmpty())
                        {
                            strResult = strResult.Replace("(", "");
                            strResult = strResult.Replace(")", "");
                            nCha = FnOLD_A104 - strResult.To<double>();
                            if (Math.Abs(nCha) >= 0.5)
                            {
                                strA104A105 = "OK";
                            }
                        }
                        //시력(우) 종전과 차이가 발생하면 메시지 표시
                        if (strCode == "A105" && FnOLD_A105 != 0 && !FnOLD_A105.IsNullOrEmpty())
                        {
                            strResult = strResult.Replace("(", "");
                            strResult = strResult.Replace(")", "");
                            nCha = FnOLD_A105 - strResult.To<double>();
                            if (Math.Abs(nCha) >= 0.5)
                            {
                                strA104A105 = "OK";
                            }
                        }

                        //노인신체기능검사
                        if (strCode == "A118") { strNOIN1 = strResult; }
                        if (strCode == "A119") { strNOIN2 = strResult; }
                        if (strCode == "A120") { strNOIN3 = strResult; }

                        if (strCode == "A118" && VB.IsNumeric(strResult) == false) //혈압은 숫자만 가능함
                        {
                            MessageBox.Show("숫자만 입력 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }

                        if (strCode == "A120" && VB.IsNumeric(strResult) == false) //혈압은 숫자만 가능함
                        {
                            MessageBox.Show("숫자만 입력 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }

                        if (strCode == "A120" && strResult == "0")
                        {
                            MessageBox.Show("보행장애결과가 0입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }

                        //허리둘레 종전과 10Cm 이상 차이가 발생하면 메세지 표시
                        //A115
                        if (strCode == "A115" && FnOLD_Waist != 0 && !FnOLD_Waist.IsNullOrEmpty())
                        {
                            nCha = FnOLD_Waist - strResult.To<double>();
                            if (Math.Abs(nCha) >= 10)
                            {
                                strWaistDispOk = "OK";
                            }
                        }

                        if (SS2.ActiveSheet.Cells[i, 2].Text.IndexOf("..") > 0)
                        {   
                            strPointDispOk = "OK";
                        }


                        //순음정력검사 대상 체크로직
                        if (hicSunapdtlService.GetCountbyWrtNoCode(FnWRTNO, "J231") == 0 && strAudioMsg == "")
                        {
                            if (strCode == "TH13" || strCode == "TH14" || strCode == "TH15" || strCode == "TH23" || strCode == "TH24" || strCode == "TH25")     //청력
                            {
                                if (FstrSex == "M")
                                {
                                    if (strResult != "X" && strResult != ".")
                                    {
                                        if (Convert.ToDouble(strResult) < Convert.ToDouble(strMinM) || Convert.ToDouble(strResult) > Convert.ToDouble(strMaxM))
                                        {
                                            strAudioMsg = "OK";
                                            MessageBox.Show("순음청력검사 대상자입니다!!!", "접수안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                                else if (FstrSex == "F")
                                {
                                    if (strResult != "X" && strResult != ".")
                                    {
                                        if (Convert.ToDouble(strResult) < Convert.ToDouble(strMinF) || Convert.ToDouble(strResult) > Convert.ToDouble(strMaxF))
                                        {
                                            strAudioMsg = "OK";
                                            MessageBox.Show("순음청력검사 대상자입니다!!!", "접수안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //정밀청력 5의배수만 등록
                for (int i = 0; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strCode = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strPart = SS2.ActiveSheet.Cells[i, 14].Text.Trim();
                   
                    if (!strResult.IsNullOrEmpty())
                    {
                        if (strPart == "A" && strResult != "0" && (strResult.To<double>() % 5) != 0)
                        {
                            if(strCode != "TH90" && strCode != "TH91")
                            {
                                MessageBox.Show("정밀청력 값이 5의배수가 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                btnSave.Enabled = true;
                                return;
                            } 
                        }
                    }

                    if(strResult =="..")
                    {
                        MessageBox.Show("코드: "+strCode + " 정밀청력 값이 '..' 로 입력되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnSave.Enabled = true;
                        return;
                    }
                }

                //노인신체기능검사
                if (!strNOIN1.IsNullOrEmpty())
                {  
                    if (strNOIN1.To<int>() >= 21)
                    {
                        if (VB.Left(strNOIN2, 2) == "01")
                        {
                            MessageBox.Show("보행장애 검사결과를 확인해주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }
                    }
                }

                //키,몸무게 종전과 5이상 차이가 발생하면 메세지 표시
                if (strDispOk == "OK")
                {
                    strMsg = "키 또는 몸무게 종전 결과값과 5 이상 차이가 발생함.";
                    strMsg += "저장을 하시겠습니까?";
                    if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        btnSave.Enabled = true;
                        return;
                    }
                }

                //허리둘레 종전과 5이상 차이가 발생하면 메세지 표시
                if (strWaistDispOk == "OK")
                {
                    strMsg = "허리둘레 종전 결과값과 10 이상 차이가 발생함.";
                    strMsg += "저장을 하시겠습니까?";
                    if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        btnSave.Enabled = true;
                        return;
                    }
                }

                //악력 입력제한
                if (strTZ0809 == "OK")
                {
                    strMsg = "악력 종전 결과값과 5 이상 차이가 발생함.";
                    strMsg += "저장을 하시겠습니까?";
                    if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        btnSave.Enabled = true;
                        return;
                    }
                }

                //시력
                if (strA104A105 == "OK")
                {
                    strMsg = "시력 종전 결과값과 0.5 이상 차이가 발생함.";
                    strMsg += "저장을 하시겠습니까?";
                    if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        btnSave.Enabled = true;
                        return;
                    }
                }
                
                //".." 입력제한
                if (strPointDispOk == "OK")
                {
                    strMsg = "결과 입력이 잘못 되었습니다!";
                    strMsg += "저장을 하시겠습니까?";
                    if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        btnSave.Enabled = true;
                        return;
                    }
                }

                //BP 전송
                if (chkBP.Checked == true)
                {
                    //fn_Work_BP_Data();
                }

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strCode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CODE].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESULT].Text.Trim();
                    strPanjeng = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.PANJENG].Text.Trim();
                    strResCode = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESCODE].Text.Trim();
                    strChange = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.CHANGE].Text.Trim();
                    strROWID = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.ROWID].Text.Trim();
                    strResType = SS2.ActiveSheet.Cells[i, (int)clsHcType.Instrument_Result.RESTYPE].Text.Trim();

                    strNewPan = hm.ExCode_Result_Panjeng(strCode, strResult, FstrSex, dtpFDate.Text, VB.Left(dtpFDate.Text, 4)).Trim();

                    if (!strResCode.IsNullOrEmpty())
                    {
                        strResult = VB.Pstr(strResult, ".", 1);
                    }

                    //2020.11.04 요잠혈 02번 입력 금지
                    if (strCode == "A113" && VB.Pstr(strResult, ".", 1) == "02")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("요잠혈은 02.± 입력 불가합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SS2.ActiveSheet.SetActiveCell(i, 2);
                        btnSave.Enabled = true;
                        return;
                    }

                    if (strChange == "Y" || !strResult.IsNullOrEmpty())
                    {
                        //History에 INSERT
                        int result = hicResultHisService.Result_History_Insert(clsType.User.IdNumber, strResult.Replace("'", "`"), strROWID, "");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }

                        int result1 = hicResultHisService.Result_Update(strResult.Replace("'", "`"), strNewPan, strResCode, clsType.User.IdNumber, strROWID, strCode);                        

                        if (result1 < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnSave.Enabled = true;
                            return;
                        }
                    }
                }

                //참고사항 저장
                strExamRemark = ssPatInfo.ActiveSheet.Cells[1, 3].Text.Trim();

                int result2 = hicJepsuService.UpdateExamRemarkbyWrtNo(strExamRemark, FnWRTNO);

                if (result2 < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("참고사항 저장중 오류 발생!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //비만도 계산 및 Update
                if (FstrGjJong == "56")
                {
                    hm.Biman_Gesan(FnWRTNO, "HIC");
                }
                else
                {
                    hm.Biman_Gesan(FnWRTNO, "HIC");    //체질량 자동계산 A117
                }

                hm.Update_Audiometry(FnWRTNO);  //기도청력 시 기본청력 정상입력

                //hm.GFR_Gesan_Life(FnWRTNO, FstrSex, FnAge); //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                //hm.MDRD_GFR_Gesan(FnWRTNO, FstrSex, FnAge, "HIC");  //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                hm.AIR3_AUTO(FnWRTNO, "HIC");                      //AIR 3분법 자동계산
                hm.AIR6_AUTO(FnWRTNO, "HIC");                      //AIR 6분법 자동계산
                hm.LDL_Gesan(FnWRTNO);                      //LDL콜레스테롤 계산
                hm.TIBC_Gesan(FnWRTNO);                     //TIBC총철결합능 계산

                //접수마스타의 상태를 변경
                hm.Result_EntryEnd_Check(txtWrtNo.Text.To<long>());

                //62종 혈액종검 체혈액팅
                hm.Update_Blood_Acting(FnPano, FnWRTNO, dtpHicJepDate.Text);
                //다음방 자동 지정
                if (FstrWaitRoom == "10" || FstrWaitRoom == "14")
                {
                    hcact.WAIT_NextRoom_SET(FnWRTNO, FnPano, FstrWaitRoom);
                }
                timer1.Enabled = false;

                //fn_Screen_Display();
                fn_Screen_Clear();

                tabControl1.SelectedTab = tabChk;
                eTabClick(tabControl1, new EventArgs());

                btnSave.Enabled = true;

                txtWrtNo.Focus();
                txtWrtNo.Select();

                timer1.Enabled = false;
                timer3.Enabled = false;
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
            else if (sender == btnClose)
            {
                panInfo.Visible = false;
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
            List<string> sCodes = new List<string>();
            bool bOK = false;

            sCodes.Clear();

            sCodes.Add("2116");
            sCodes.Add("2122");
            sCodes.Add("2121");

            if (txtWrtNo.Text.Trim().IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 공란입니다!", "필수항목 누락", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            nWrtNo = txtWrtNo.Text.To<long>();

            HIC_SUNAPDTL listSunapDtl = hicSunapdtlService.GetSunapDtlbyWrtNo(nWrtNo, sCodes);

            if (listSunapDtl.IsNullOrEmpty())
            {
                rtnVal = true;
                return rtnVal;
            }

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCode = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                if (!strResult.IsNullOrEmpty())
                {
                    switch (strCode)
                    {
                        case "A106":
                        case "A107":
                        case "TH12":
                        case "TH22":
                            bOK = true;
                            break;
                        default:
                            bOK = false;
                            break;
                    }

                    if (bOK == true)
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
            rtnVal = true;
            return rtnVal;
        }

        void eTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                string strPtno = "";
                string strTemp = "";
                string strSDate = "";
                long nWrtNo = 0;
                
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

                    FbUAScanSave = false;

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

                    if (chkBP.Checked == true)
                    {
                        timer1.Enabled = true;
                        //BP 인터페이스
                        //Work_BP_Control();
                    }

                    if (chkAudio.Checked == true)
                    {
                        timer3.Enabled = true;
                    }

                    fn_ViewClick();

                    if (!FstrWaitRoom.IsNullOrEmpty())
                    {
                        nWrtNo = txtWrtNo.Text.To<long>();

                        clsDB.setBeginTran(clsDB.DbCon);

                        int result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(nWrtNo, FstrWaitRoom);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                        clsDB.setCommitTran(clsDB.DbCon);
                    }
                    
                    //소변검사 자동저장
                    if (FbUAScanSave == true)
                    {
                        FbUAScanSave = false;
                        timerUA.Enabled = true;                        
                    }

                    //결과항목이 null 인 셀 포커스 이동
                    SS2_FOCUS();


                    //채혈안내문
                    HIC_JEPSU item = hicJepsuService.GetItembyWrtNo(Convert.ToInt32(txtWrtNo.Text));

                    if (!item.IsNullOrEmpty())
                    {
                        if (!item.UCODES.IsNullOrEmpty())
                        {
                            for (int i = 0; i < lstBlnfo.Count; i++)
                            {
                                if (item.UCODES.Trim().Contains(lstBlnfo[i].CODE.To<string>("").Trim()))
                                {
                                    MessageBox.Show("채혈안내문 필요함", "확인");
                                    break;
                                }

                            }
                        }
                    }
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

        void fn_ViewClick()
        {
            Menu10.Enabled = false;

            //폐활량검사 액팅방이고 액팅이 안된경우(일반검진이 있는 경우에만)
            //if (!clsHcVariable.GstrPFTSN.IsNullOrEmpty() && FnHcWRTNO > 0)
            if (!clsHcVariable.GstrPFTSN.IsNullOrEmpty())
            {
                string sRowId = hicResSpecialService.Read_Res_Special(txtWrtNo.Text.To<long>());

                if (!sRowId.IsNullOrEmpty())
                {
                    HIC_RESULT RsltList = hicResultService.Read_Result3(txtWrtNo.Text.To<long>());

                    if (!RsltList.IsNullOrEmpty())
                    {
                        Menu10.Enabled = true;

                        if (RsltList.RESULT.IsNullOrEmpty())
                        {
                            eMenuClick(Menu10, new EventArgs());
                        }
                    }
                }
                //2019-11-07(준종합검진 계념으로 추가검사경우 체크로직)
                else
                {
                    HIC_RESULT list = hicResultService.GetResultbyWrtNoPart(txtWrtNo.Text.To<long>(), "7");

                    if (!list.IsNullOrEmpty())
                    {
                        Menu10.Enabled = true;
                        if (list.RESULT.IsNullOrEmpty())
                        {
                            eMenuClick(Menu10, new EventArgs());
                        }
                    }
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

                strPart = ssChk.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                {
                    ssChk.ActiveSheet.Cells[i, 4].Text = "";
                }

                //해당 검사실 대기명단
                List<HIC_SANGDAM_WAIT> Wait_List = hicSangdamWaitService.Read_Sangdam_Wait_List(strPart);                

                for (int i = 0; i < Wait_List.Count; i++)
                {
                    ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List[i].SNAME;
                    ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List[i].WRTNO.To<string>();
                }
            }
            else if (sender == this.SS2)
            {
                string strResCode = "";

                if (e.Column == 2)
                {
                    if (SS2.ActiveSheet.NonEmptyRowCount == 0) return;

                    strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                    if (strResCode.IsNullOrEmpty())
                    {
                        sp.Spread_All_Clear(ssList1);
                        FnClickRow = -1;
                        return;
                    }

                    FnClickRow = e.Row;

                    sp.Spread_All_Clear(ssList1);
                    Application.DoEvents();

                    //자료를 Read
                    List<HIC_RESCODE> list = hicRescodeService.GetCodeNamebyResCode(strResCode);

                    ssList1.ActiveSheet.RowCount = list.Count;
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ssList1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                            ssList1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                        }
                    }
                }
                else if (e.Column == 2)
                {
                    strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                    if (strResCode.IsNullOrEmpty())
                    {
                        sp.Spread_All_Clear(ssList1);
                        FnClickRow = -1;
                        return;
                    }
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
            if (sender == SS2)
            {
                if (SS2.ActiveSheet.NonEmptyRowCount == 0) return;

                int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
                int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;
                string strResCode = "";

                if (nCol != 2) return;

                strResCode = SS2.ActiveSheet.Cells[nRow, 7].Text.Trim();
                if (strResCode.IsNullOrEmpty())
                {
                    sp.Spread_All_Clear(ssList1);
                    FnClickRow = -1;
                    return;
                }

                //자료를 READ
                List<HIC_RESCODE> list = hicRescodeService.GetCodeNamebyBindGubun(strResCode);

                ssList1.ActiveSheet.RowCount = list.Count;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ssList1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                        ssList1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                    }
                }
                else
                {
                    sp.Spread_All_Clear(ssList1);
                }
            }
        }

        void eSpreadEditModeOff(object sender, EventArgs e)
        {
            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;

            if (SS2.InputDeviceType.ToString() == "Keyboard")
            {
                if (nRow == SS2.ActiveSheet.RowCount - 1)
                {
                    SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.CHANGE].Text = "Y";
                    if (MessageBox.Show("결과값을 저장하시겠습니까?", "확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        eBtnClick(btnSave, new EventArgs());
                    }
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

            if (txtWrtNo.Text.IsNullOrEmpty()) return;

            int nRow = this.SS2.ActiveSheet.ActiveRow.Index;
            int nCol = this.SS2.ActiveSheet.ActiveColumn.Index;

            if (sender == SS2)
            {
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

                strCode = SS2.ActiveSheet.Cells[nRow, 0].Text;

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
                    lblGuide.Text = "F1: 측정불가 F5: 정상 F6: 비정상 F7: 교정 F8: 적록색약 F9: 9.9 F10: 미실시";

                    switch (e.KeyCode)
                    {
                        case Keys.F1:
                            strResult = "측정불가";
                            break;
                        case Keys.F5:
                            strResult = "정상";
                            break;
                        case Keys.F6:
                            strResult = "비정상";
                            break;
                        case Keys.F7:
                            strResult = "교정";
                            break;
                        case Keys.F8:
                            strResult = "적록색약";
                            break;
                        case Keys.F9:
                            strResult = "9.9";
                            break;
                        case Keys.F10:
                            strResult = "미실시";
                            break;
                        default:
                            break;
                    }
                }

                strResCode = SS2.ActiveSheet.Cells[nRow, 7].Text.Trim();
                strResType = SS2.ActiveSheet.Cells[nRow, 10].Text.Trim();

                if (!strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text = strResult;
                    SS2.ActiveSheet.Cells[(int)FnRowNo, 8].Text = "Y";

                    FnRowNo += 1;
                    if (FnRowNo > SS2.ActiveSheet.RowCount)
                    {
                        FnRowNo = SS2.ActiveSheet.RowCount;
                    }
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
                }
            }
        }

        void eSpreadKeyUp(object sender, KeyEventArgs e)
        {
            string strCode = "";
            bool bKey1 = false;
            bool bKey2 = false;

            if (txtWrtNo.Text.IsNullOrEmpty()) return;

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
                    SS2.Focus();
                    SS2.EditMode = true;
                }
            }
            else if (strTemp == "2")
            {
                //문자일경우
                if (VB.IsNumeric(SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text) == true)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
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

            if (txtWrtNo.Text.IsNullOrEmpty()) return;

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

            if (txtWrtNo.Text.IsNullOrEmpty()) return;

            strCode = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            nData = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim().To<double>();
            strData = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            SS2.ActiveSheet.Cells[e.Row, 8].Text = "Y";

            //숫자값을 사용하는 코드인지 확인
            if (hicExcodeService.GetResultTypebyCode(strCode.Trim()) == "1")
            {
                if (nData > 999.9)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                    return;
                }

                if (SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text.IndexOf("..") > 0)
                {
                    MessageBox.Show("결과 입력이 잘못 되었습니다!", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
                    SS2.Focus();
                    SS2.EditMode = true;
                }

                switch (strCode.Trim())
                {
                    case "A108":    //혈압(최고)
                        if (nData > 250 || nData < 60)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                        }
                        break;
                    case "A109":    //혈압(최저)
                        if (nData > 250 || nData < 40)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                        }
                        break;
                    case "A114":    //소변Ph
                        if (nData > 8 || nData < 6)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                        }
                        break;
                    case "A104":    //시력(좌)
                    case "A105":    //시력(우)
                    case "C203":    //교정시력(좌)
                    case "C204":    //교정시력(우)
                        nData = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim().Replace("(", "").Replace(")", "").To<double>();
                        if ((nData > 2 || nData < 0.1) && strData != ".")
                        {
                            if (nData != 9.9)
                            {
                                MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                            }
                        }
                        break;
                    case "A115":    //허리둘레
                        if (nData > 150 || nData < 50 )
                        {
                            if ( nData != 999.9)
                            {
                                MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                            }
                        }
                        break;
                    case "ZD04":    //흉부둘레
                        if (nData > 150 || nData < 50)
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                        }
                        break;
                    case "A101":    //신장
                        if (nData > 200 || nData < 100)
                        {
                            if (nData != 999.9)
                            {
                                MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                            }
                        }
                        break;
                    case "A102":    //체중
                        if (nData > 200 || nData < 10)
                        {
                            if (nData != 999.9)
                            {
                                MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                            }
                        }
                        break;
                    case "TZ08":
                    case "TZ09":
                    default:
                        break;
                }
            }
            else
            {
                switch (strCode.Trim())
                {
                    //기도청력 "." 입력금지
                    case "TH21":
                    case "TH22":
                    case "TH23":
                    case "TH24":
                    case "TH25":
                    case "TH26":
                    case "TH11":
                    case "TH12":
                    case "TH13":
                    case "TH14":
                    case "TH15":
                    case "TH16":
                        if (VB.Left(SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim(), 1) == "." || SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim() == "," ||
                            SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim() == "/" || SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim() == "*")
                        {
                            MessageBox.Show("결과 입력이 잘못 되었습니다", "결과값 입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            long nAge;
            long nPaNo;
            string strPart;
            string strSName;
            string strSex;
            string strGjJong;
            string strRoom;
            string strGb;
            long nWrtNo = 0;

            FpSpread o = (FpSpread)sender;

            if (sender == this.ssChk)
            {
                if (ssChk.ActiveSheet.NonEmptyRowCount == 0) return;

                //if (e.Column == 3)
                //{
                //    if (txtWrtNo.Text.Trim().IsNullOrEmpty())
                //    {
                //        txtWrtNo.Focus();
                //        return;
                //    }

                //    HIC_JEPSU Wait_List = hicJepsuService.Read_Jepsu_Wait_List(txtWrtNo.Text.To<long>(), dtpHicJepDate.Text);

                //    if (Wait_List.IsNullOrEmpty())
                //    {
                //        MessageBox.Show(txtWrtNo.Text + " 접수내역이 없습니다", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //        txtWrtNo.Focus();
                //        return;
                //    }

                //    strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                //    //해당방에 등록되어 있는지 확인
                //    HIC_SANGDAM_WAIT SDReg_List = hicSangdamWaitService.Read_Sangdam_Wait_RegList(txtWrtNo.Text.To<long>(), strPart);

                //    if (!SDReg_List.IsNullOrEmpty())
                //    {
                //        if (!SDReg_List.RID.IsNullOrEmpty())
                //        {
                //            if (SDReg_List.GBCALL == "Y")
                //            {
                //                MessageBox.Show(SDReg_List.GUBUN + " 번방의 검사가 이미 완료되었습니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //            }
                //            else
                //            {
                //                MessageBox.Show(SDReg_List.GUBUN + " 번방에 이미 등록되어 있습니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //            }
                //            return;
                //        }
                //    }

                //    //다른검사실에 등록되어 있는지 확인
                //    HIC_SANGDAM_WAIT SDEtcReg_List = hicSangdamWaitService.Read_Sangdam_EtcRoomReg(txtWrtNo.Text.To<long>(), strPart);

                //    if (!SDEtcReg_List.IsNullOrEmpty())
                //    {
                //        if (!SDEtcReg_List.RID.IsNullOrEmpty())
                //        {
                //            if (!SDEtcReg_List.CALLTIME.IsNullOrEmpty())
                //            {
                //                MessageBox.Show(SDEtcReg_List.GUBUN + " 번방에서 검사중입니다!!", "등록불가!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //                return;
                //            }
                //            else
                //            {
                //                //기존의 자료가 있으면 삭제함
                //                int result = hicSangdamWaitService.Delete_Sangdam_PreData(txtWrtNo.Text.To<long>(), strPart);

                //                if (result < 0)
                //                {
                //                    MessageBox.Show("타 검사실 대기순번 삭제시 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                    return;
                //                }
                //            }
                //        }
                //    }

                //    //해당방에 등록
                //    HIC_JEPSU listJepsu = hicJepsuService.GetSexAgeGjJongbyWrtNo(txtWrtNo.Text.To<long>());

                //    if (!listJepsu.IsNullOrEmpty())
                //    {
                //        strSName = listJepsu.SNAME;
                //        strSex = listJepsu.SEX;
                //        nAge = listJepsu.AGE;
                //        strGjJong = listJepsu.GJJONG;
                //        nPaNo = listJepsu.PANO;

                //        HIC_SANGDAM_WAIT View_List2 = hicSangdamWaitService.Read_Sangdam_View2(strPart);

                //        HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();
                            
                //        item.WRTNO = FnWRTNO;
                //        item.SNAME = strSName;
                //        item.SEX = strSex;
                //        item.AGE = nAge;
                //        item.GJJONG = strGjJong;
                //        item.GBCALL = "";
                //        item.GUBUN = FstrRoom;
                //        item.WAITNO = View_List2.WAITNO;
                //        item.PANO = nPaNo;

                //        int result = hicSangdamWaitService.Insert_Sangdam_Wait3(item);

                //        if (result < 0)
                //        {
                //            MessageBox.Show("상담대기 순번등록 중 오류 발생!!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            return;
                //        }

                //        FstrRoom2 = strPart;
                //    }

                //    for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                //    {
                //        strRoom = ssChk.ActiveSheet.Cells[i, 6].Text.Trim();
                //        ssChk.ActiveSheet.Cells[i, 3].Text = "";

                //        HIC_SANGDAM_WAIT WrtNoCnt_List = hicSangdamWaitService.Read_Sangdam_WrtNoCnt(strRoom);

                //        if (strRoom != "99")
                //        {
                //            ssChk.ActiveSheet.Cells[i, 3].Text = WrtNoCnt_List.CNT.To<string>();
                //        }
                //    }

                //    //현 검사실 대기명단
                //    List<HIC_SANGDAM_WAIT> Wait_List1 = hicSangdamWaitService.Read_Sangdam_Wait_List(strPart);

                //    for (int i = 0; i < Wait_List1.Count; i++)
                //    {
                //        ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List1[i].SNAME;
                //        ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List1[i].WRTNO.To<string>();
                //    }
                //}
                //else if (e.Column == 5)
                //{
                //    if (VB.Val(ssChk.ActiveSheet.Cells[e.Row, 8].Text) == 0)
                //    {
                //        txtWrtNo.Focus();
                //        return;
                //    }
                //    txtWrtNo.Text = ssChk.ActiveSheet.Cells[e.Row, 8].Text;
                //    txtWrtNo.Focus();

                //    eTextBoxKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
                //    return;
                //}
                //else
                //{
                    if (rdoJepsuGubun2.Checked == true)
                    {
                        strGb = "1";
                    }
                    else if (rdoJepsuGubun3.Checked == true)
                    {
                        strGb = "2";
                    }
                    else
                    {
                        strGb = "";
                    }

                    strPart = ssChk.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                    tabControl1.TabIndex = 0;
                    tabControl1.SelectedTab = tabJepsu;
                    sp.Spread_All_Clear(ssJepList);
                    ssJepList.ActiveSheet.RowCount = 30;

                    if (e.Column == 1)
                    {
                        strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                        //List<HIC_SANGDAM_WAIT> Waitlist1 = hicSangdamWaitService.Read_Sangdam_View3(strPart);
                        List<HIC_JEPSU_RESULT> Waitlist1 = hicJepsuResultService.GetItembyJepDatePart(dtpHicJepDate.Text, strPart, strGb);

                        ssJepList.ActiveSheet.RowCount = Waitlist1.Count;

                        for (int i = 0; i < Waitlist1.Count; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = Waitlist1[i].WRTNO.To<string>();
                            ssJepList.ActiveSheet.Cells[i, 1].Text = Waitlist1[i].SNAME;
                        }
                    }
                    else
                    {
                        strPart = ssChk.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                        List<HIC_JEPSU_RESULT> Waitlist2 = hicJepsuResultService.GetItembyJepDatePart(dtpHicJepDate.Text, strPart, strGb);
                        ssJepList.ActiveSheet.RowCount = Waitlist2.Count;
                        for (int i = 0; i < Waitlist2.Count; i++)
                        {
                            ssJepList.ActiveSheet.Cells[i, 0].Text = Waitlist2[i].WRTNO.To<string>();
                            nWrtNo = Waitlist2[i].WRTNO.To<long>();
                            ssJepList.ActiveSheet.Cells[i, 1].Text = Waitlist2[i].SNAME;
                            ssJepList.ActiveSheet.Cells[i, 2].Text = "";
                            ssJepList.ActiveSheet.Cells[i, 4].Text = Waitlist2[i].SEX;
                            ssJepList.ActiveSheet.Cells[i, 5].Text = Waitlist2[i].NAME;

                            //상태점검(신체계측 분류 검사코드 중 액팅이 안된것 찾기)
                            List<HIC_RESULT_ACTIVE> listActive = hicResultActiveService.GetActivebyWrtno(nWrtNo, "2");

                            if (listActive.Count > 0)
                            {
                                for (int j = 0; j < listActive.Count; j++)
                                {
                                    if (listActive[j].ACTIVE == "N" || listActive[j].ACTIVE.IsNullOrEmpty())
                                    {
                                        strTemp = "OK";
                                        break;
                                    }
                                }

                                if (strTemp == "OK")
                                {
                                    ssJepList.ActiveSheet.Cells[i, 2].Text = "X";
                                }
                                else
                                {
                                    ssJepList.ActiveSheet.Cells[i, 2].Text = "○";
                                }
                                strTemp = "";
                            }
                            //종검 마스타에서 종검번호를 찾음
                            nHeaPano = hicPatientService.GetPanobyPtno(Waitlist2[i].PTNO);

                            if (nHeaPano > 0)
                            {
                                FnHeaWRTNO = heaJepsuService.GetWrtNobyHeaPaNo(nHeaPano, dtpFDate.Text);

                                if (FnHeaWRTNO > 0)
                                {
                                    ssJepList.ActiveSheet.Cells[i, 3].Text = "○";
                                }
                            }
                        }
                    }
                //}
            }
            else if (sender == this.ssJepList)
            {
                if (e.RowHeader == true) return;

                if (ssJepList.ActiveSheet.NonEmptyRowCount == 0) return;

                fn_Screen_Clear();

                txtWrtNo.Text = ssJepList.ActiveSheet.Cells[e.Row, 0].Text;
                nWrtNo = txtWrtNo.Text.To<long>();

                eTextBoxKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));

                if (chkBP.Checked == true)
                {
                    timer1.Enabled = true;
                    //BP 인터페이스
                    //Work_BP_Control();
                }

                tabControl1.TabIndex = 1;
                tabControl1.SelectedTab = tabChk;

                //결과항목이 null 인 셀 포커스 이동
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
        ///결과항목이 null 인 셀 포커스 이동
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
            if (sender == timer1)
            {
                //TODO : 이상훈 (2020.09.09) BP 관련 인터페이스 확인 필요   
            }
            else if (sender == timer2)
            {
                if (FstrWaitRoom != "10")
                {
                    return;
                }

                FnTimerList += 1;
                if (FnTimerList >= 10)
                {
                    FnTimerList = 0;
                    fn_menuList();
                }

                if (FbAutoSave == true)
                {
                    FnTimerSave += 1;
                    lblXray.Text = FnTimerSave.To<string>();
                    if (FnTimerSave >= 5)
                    {
                        FbAutoSave = false;
                        FnTimerSave = 0;
                        eMenuClick(Menu06, new EventArgs());
                        txtWrtNo.Text = "";
                    }
                }
                else
                {
                    lblXray.Text = "";
                    FnTimerSave = 0;
                }

                if (FstrWaitRoom == "10")
                {
                    txtWrtNo.Focus();
                    txtWrtNo.Select();
                }
            }
            else if (sender == timer3)
            {
                if (Fstr청력인터페이스 == "OK")
                {
                    //"c:\Audio_Interface\Result.txt"
                    timer3.Enabled = false;
                    DirectoryInfo dir = new DirectoryInfo(@"c:\Audio_Interface\Result.txt");
                    if (dir.Exists == true)
                    {
                        fn_Audio_Interface();
                        return;
                    }

                    timer3.Enabled = true;
                    return;
                }
            }
            else if (sender == timerHeight)
            {
                double nHeight = 0;
                double nWeight = 0;
                double nBiman = 0;
                string strTemp = "";

                timerHeight.Enabled = false;

                Thread.Sleep(100);

                strTemp = VB.Pstr(FstrComm1, ",", 1);
                if (string.Compare(VB.Left(strTemp, 1), "0") < 0 || string.Compare(VB.Left(strTemp, 1), "9") > 0) { strTemp = VB.Right(strTemp, strTemp.Length - 1); }
                if (string.Compare(VB.Left(strTemp, 1), "0") < 0 || string.Compare(VB.Left(strTemp, 1), "9") > 0) { strTemp = VB.Right(strTemp, strTemp.Length - 1); }
                nHeight = strTemp.To<double>();
                nWeight = VB.Pstr(FstrComm1, ",", 2).To<double>();

                Thread.Sleep(100);

                //소숫점1자리이하는 버럼 처리 요청
                nHeight = Math.Truncate(nHeight * 10) / 10;
                nWeight = Math.Truncate(nWeight * 10) / 10;

                for (int i = 0; i < SS2_Sheet1.NonEmptyRowCount; i++)
                {
                    if (SS2.ActiveSheet.Cells[i, 0].Text == "A101")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = nHeight.To<string>();
                        SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                    }

                    if (SS2.ActiveSheet.Cells[i, 0].Text == "A102")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = nWeight.To<string>();
                        SS2.ActiveSheet.Cells[i, 8].Text = "Y";
                    }
                }

                FstrComm1 = "";
                
            }
            else if (sender == timerUA) //소변검사 자동 저장
            {
                timerUA.Enabled = false;
                eBtnClick(btnSave, new EventArgs());
            }
        }

        /// <summary>
        /// 청력검사 인터페이스
        /// </summary>
        void fn_Audio_Interface()
        {
            int nCNT = 0;
            string strFile = "";
            string strREC = "";
            long nWRTNO = 0;
            string strSname = "";
            string strPtno = "";
            string strExCode = "";
            string strResult = "";
            string strGBn = "";
            string strTemp = "";

            const string EXCODE_CONV_LA = "    ,    ,TH11,    ,TH12,    ,TH13,TH14,TH15,TH16,TH17"; //기도청력(왼쪽)
            const string EXCODE_CONV_RA = "    ,    ,TH21,    ,TH22,    ,TH23,TH24,TH25,TH26,TH27"; //도청력(우쪽)
            const string EXCODE_CONV_LB = "    ,    ,TH51,    ,TH52,    ,TH53,TH54,TH55,TH56,    "; //전도청력(왼쪽)
            const string EXCODE_CONV_RB = "    ,    ,TH61,    ,TH62,    ,TH63,TH64,TH65,TH66,    "; //전도청력(우쪽)
            const string EXCODE_CONV_LAM = "   ,    ,TH67,    ,TH68,    ,TH69,TH70,TH71,TH72,    "; //도청력(왼쪽)음차폐
            const string EXCODE_CONV_RAM = "   ,    ,TH73,    ,TH74,    ,TH75,TH76,TH77,TH78,    "; //도청력(우쪽)음차폐
            const string EXCODE_CONV_LBM = "   ,    ,TH79,    ,TH80,    ,TH81,TH82,TH83,TH84,    "; //전도청력(왼쪽)음차폐
            const string EXCODE_CONV_RBM = "   ,    ,TH85,    ,TH86,    ,TH87,TH88,TH89,TH95,    "; //골전도청력(우쪽)음차폐

            strFile = clsVbfunc.GetFile(@"c:\Audio_Interface\Result.txt");
            strREC = VB.Pstr(strFile, "\r\n", 1);
            strSname = VB.Pstr(strREC, ",", 1);
            strPtno = VB.Pstr(strREC, ",", 2);
            nWRTNO = VB.Pstr(strREC, ",", 8).To<long>();

            nCNT = VB.L(strFile, "\r\n").To<int>();

            for (int i = 2; i <= nCNT; i++)
            {
                strREC = VB.Pstr(strFile, "\r\n", i);
                if (!strREC.IsNullOrEmpty())
                {
                    strGBn = VB.Pstr(strREC, ",", 1);
                    switch (strGBn)
                    {
                        case "LA":
                            strTemp = EXCODE_CONV_LA;
                            break;
                        case "RA":
                            strTemp = EXCODE_CONV_RA;
                            break;
                        case "LB":
                            strTemp = EXCODE_CONV_LB;
                            break;
                        case "RB":
                            strTemp = EXCODE_CONV_RB;
                            break;
                        case "LAM":
                            strTemp = EXCODE_CONV_LAM;
                            break;
                        case "RAM":
                            strTemp = EXCODE_CONV_RAM;
                            break;
                        case "LBM":
                            strTemp = EXCODE_CONV_LBM;
                            break;
                        case "RBM":
                            strTemp = EXCODE_CONV_RBM;
                            break;
                        default:
                            break;
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

            sp.Spread_All_Clear(ssJepList);
            ssJepList.ActiveSheet.RowCount = 50;
            sp.SetfpsRowHeight(ssJepList, 22);

            this.Text += " " + "☞작업자: " + clsType.User.UserName;

            FstrBDate = clsPublic.GstrSysDate;
            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpHicJepDate.Text = clsPublic.GstrSysDate;
                        
            fn_Screen_Clear();

            SS3.Visible = false;
            SS4.Visible = false;
            lblWaitRoom.Text = "";
            lblWaitRoom.Visible = false;
            Menu16_01.Enabled = false;
            Menu16_02.Enabled = false;
            

            if (clsHcVariable.GbHicAdminSabun == true || hicBcodeService.GetMenuSetAuthoritybyIdNumber("HIC_MENU권한_검사실번호", clsType.User.IdNumber) > 0)
            {
                //Menu03.Enabled = true;
                Menu16_01.Enabled = true;
                
            }

            if (hicBcodeService.GetMenuSetAuthoritybyIdNumber("HIC_MENU권한_관리자작업", clsType.User.IdNumber.Trim()) > 0)
            {
                //Menu11.Enabled = true;
                Menu16_02.Enabled = true;
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

            if (FnWRTNO > 0)
            {
                txtWrtNo.Text = FnWRTNO.To<string>();
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

            //검사실 바코드 자동인쇄요청 PC 여부 설정
            FbExamBarCodeReq = false;
            BarCode_AutoPrint();

            //검사실코드 및 명칭을 읽음
            List<HIC_BCODE> list3 = hicBcodeService.GetCodeNamebyBcode1("BAS_표시장비검사실PC", clsCompuInfo.gstrCOMIP);

            FstrWaitRoom = "";
            FstrWaitName = "";

            if (list3.Count > 0)
            {
                strData = list3[0].NAME;
                if (VB.Pstr(strData, "{}", 1) == "일반")
                {
                    FstrWaitRoom = string.Format("{0:00}", int.Parse(VB.Pstr(strData, "{}", 2)));
                    FstrWaitName = VB.Pstr(strData, "{}", 3);
                    this.Text += " 【검사실:" + FstrWaitRoom + "." + FstrWaitName + "】";
                    this.tabJepsu.Text = "대기자명단";
                    btnSang.Text = "대기등록";
                }
            }

            List<BAS_BCODE> list4 = basBcodeService.GetCodeNamebyBCode("HIC_촬영기사", "");
            cboExId.SetItems(list4, "NAME", "CODE", "", "", AddComboBoxPosition.Top);

            List<BAS_BCODE> list6 = basBcodeService.GetCodeNamebyBCode("HIC_촬영기사", clsType.User.Sabun);
            if (list6.Count == 1)
            {
                cboExId.FindString(list6[0].CODE + "." + list6[0].NAME);
            }

            lblXray.Text = "";
            FnTimerList = 0;
            FbAutoSave = false;
            FnTimerSave = 0;
            timer2.Enabled = false;

            if (FstrWaitRoom == "10") //흉부촬영실
            {
                timer2.Enabled = true;
                ssList1.Visible = false;
                ssList2.Visible = true;
                lblXrayGisa.Visible = true;
                cboExId.Visible = true;
                eMenuClick(Menu06, new EventArgs());
            }
            else
            {
                ssList1.Visible = true;
                ssList2.Visible = false;
                lblXrayGisa.Visible = false;
                cboExId.Visible = false;
            }

            timerHeight.Enabled = false;
            FstrComm1 = "";
            if (FbWeightPc == true)
            {
                //Serial 통신
                fn_Serial_Set();
            }

            //fn_menuList();
            txtWrtNo.Focus();
            txtWrtNo.Select();

            HIC_EXCODE haroomlist = hicExcodeService.Read_HcRoom(VB.Pstr(CboPart.Text, ".", 1));




            if (!haroomlist.IsNullOrEmpty())
            {
                FstrRoom = haroomlist.HCROOM;
            }
            else
            {
                FstrRoom = "";
            }

            //채혈안내문유해인자
            for (int i = 0; i< CboPart.Items.Count; i++ )
            {
                if (VB.Pstr(CboPart.Items[i].ToString(), ".", 1) == "8")
                {
                    lstBlnfo = hicBcodeService.GetCodebyGubun("HIC_채혈안내문유해인자");
                }
            }
            //if (VB.Pstr(CboPart.Text, ".", 1) == "8")
            //{
            //    lstBlnfo = hicBcodeService.GetCodebyGubun("HIC_채혈안내문유해인자");
            //}
           

        }

        /// <summary>
        /// Serial 통신
        /// </summary>
        void fn_Serial_Set()
        {
            //사용가능한 통신포트 를 찾아서 cboComPort에 담는다.
            cboComPort.DataSource = SerialPort.GetPortNames(); //연결 가능한 시리얼포트 이름을 콤보박스에 가져오기 
            //cboComPort.SelectedIndex = 0;
        }

        void fn_SerialPort_Connect()
        {
            if (!m_sp.IsOpen)
            {
                //MessageBox.Show(cboComPort.Text);
                //m_sp.PortName = cboComPort.Text;
                m_sp.PortName = "COM3";
                m_sp.BaudRate = 9600;
                //m_sp.BaudRate = 19200;
                m_sp.DataBits = 8;
                m_sp.StopBits = StopBits.One;
                m_sp.Parity = Parity.None;
                m_sp.Handshake = Handshake.None;
                m_sp.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                m_sp.Open();
                //MessageBox.Show(cboComPort.Text + " 가 Open 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!m_sp.IsOpen)
                {
                    m_sp.Open();
                }
            }
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(SerialReceived));

            if (FstrComm1.IndexOf(",") >= 5)
            {
                timerHeight.Enabled = true;
                eTimerTick(timerHeight, new EventArgs());
            }
        }

        private void SerialReceived(object s, EventArgs e)  //여기에서 수신 데이타를 사용자의 용도에 따라 처리한다.
        {
            //int ReceiveData = m_sp.ReadByte();                    //시리얼 포트에 수신된 데이타를 ReceiveData 읽어오기  
            //FstrComm1 += string.Format("{0:X2}", ReceiveData);    //int 형식을 string형식으로 변환하여 출력

            //int ReceiveData = m_sp.ReadChar();                    //시리얼 포트에 수신된 데이타를 ReceiveData 읽어오기
            //FstrComm1 += string.Format("{0:X2}", ReceiveData);    //int 형식을 string형식으로 변환하여 출력

            //string ReceiveData = m_sp.ReadLine();                 //시리얼 포트에 수신된 데이타를 ReceiveData 읽어오기
            //FstrComm1 += ReceiveData;                             

            string ReceiveData = m_sp.ReadExisting();             //시리얼 포트에 수신된 데이타를 ReceiveData 읽어오기
            Thread.Sleep(100);
            FstrComm1 += ReceiveData;
            Thread.Sleep(100);
        }

        void fn_Screen_Display()
        {
            string strRowId = "";

            blnExitFlag = false;

            strToDate = dtpHicJepDate.Value.ToShortDateString();
            strYYYY = strToDate.Substring(0, 4);
            strPart = VB.Pstr(CboPart.Text, ".", 1);

            chkCorrectedVision.Checked = false;
            chkCorrectedHearing.Checked = false;

            sp.Spread_All_Clear(ssList1);

            if (strPart == "W")
            {
                ssList1.Enabled = false;
                btnSave.Enabled = false;
            }

            if (!txtWrtNo.Text.IsNullOrEmpty())
            {
                FnWRTNO = txtWrtNo.Text.To<long>();
            }
            
            lblXray.Text = "";
            FnOLD_Height = 0;
            FnOLD_Weight = 0;
            strAllWrtno.Clear();

            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show("접수번호(" + FnWRTNO + ")는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                blnExitFlag = true;
                return;
            }

            SS2.ActiveSheet.RowCount = 20;
            FbUAScanSave = false;

            if (FstrPartG.IsNullOrEmpty())
            {
                MessageBox.Show("검사항목이 설정되지 않았습니다." + "\r\n" + "PC환경설정에서 검사항목을 설정하여 주십시오.", "검사항목미설정", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                blnExitFlag = true;
                return;
            }

            lblResultReceivePosition.Text = "";
            lblResultReceivePosition.Text = hicJepsuService.GetResultReceivePositionbyWrtNo(FnWRTNO, dtpHicJepDate.Text);

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

                ufn_Screen_Injek_display();         //인적사항을 Display

                if (FstrGjJong == "55") { panInfo.Visible = true; }

                if (blnExitFlag == true)
                {              
                    return;
                }
                ufn_Screen_Exam_Items_display();    //검사항목을 Display
                ufn_Screen_OLD_Result_Display(FstrSDate);    //종전 2개의 결과 보여줌

                //방사선 직촬번호 입력
                strRowId = hicResultService.GetXrayNoByWrtno(FnWRTNO);

                if (!strRowId.IsNullOrEmpty())
                {
                    if (!txtXrayNo.Text.Trim().IsNullOrEmpty())
                    {
                        //int result = hicResultService.Update_ResultbyRowId(txtXrayNo.Text.Trim(), long.Parse(clsType.User.IdNumber), strRowId

                        int result = hicResultService.Update_ResultbyRowId(txtXrayNo.Text.Trim(), 111, strRowId);
                        if (result < 0)
                        {
                            MessageBox.Show("XRAY 번호 등록중 오류 발생", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            blnExitFlag = true;
                            return;
                        }
                    }
                }

                //바코드 자동발행 요청
                if (FbExamBarCodeReq == true)
                {
                    ////2020.11.02 바코드 요청 History 추가
                    //int result = hicBarcodeReqService.HIC_BARCODE_REQ_Insert_HIC_His(FnPano, dtpHicJepDate.Text, clsType.User.IdNumber, clsCompuInfo.gstrCOMIP);

                    //if (result < 0)
                    //{
                    //    blnExitFlag = true;
                    //    return;
                    //}

                    //result = 0;

                    //result = hicBarcodeReqService.HIC_BARCODE_REQ_Insert_HIC(FnPano, dtpHicJepDate.Text);
                    int result = hicBarcodeReqService.HIC_BARCODE_REQ_Insert_HIC(FnPano, fstrJepDate1);


                    if (result < 0)
                    {
                        blnExitFlag = true;
                        return;
                    }

                    //검체바코드 발행 (혈액ACT 파트에서만 작동) - KMC



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

                //흉부촬영실은 액팅점검 안함
                if (FstrWaitRoom != "10")
                {
                    ACTING_CHECK(FnWRTNO, strToDate, FnPano);
                    //ACTING_CHECK_NEW(FnWRTNO, strToDate, FnPano);
                }

                //BP 인터페이스
                //Work_BP_Control();

                Menu08.Enabled = true;

                //키,몸무게 인터페이스
                if (FbWeightPc == true)
                {  
                    m_sp.Close();
                    m_sp.Open();
                    FstrComm1 = "";
                }
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
            fstrJepDate1 = "";
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
                FnPano = PtInfolist.PANO.To<long>();
                FstrJumin = PtInfolist.JUMIN;
                FstrSDate = PtInfolist.JEPDATE;
                FstrGongDan = PtInfolist.GONGDAN;
                strGbNaksang = PtInfolist.GBNAKSANG;

                fstrJepDate1 = PtInfolist.JEPDATE;

                FstrGjYear = PtInfolist.GJYEAR;
                strSex = PtInfolist.SEX;
                strJumin = clsAES.DeAES(PtInfolist.JUMIN2);
                strJepDate = PtInfolist.JEPDATE;
                strGjJong = PtInfolist.GJJONG;
                FstrGjJong = strGjJong;
                strGbExam = PtInfolist.GBEXAM;

                if (strJumin.IsNullOrEmpty())
                {
                    MessageBox.Show("주민등록번호가 존재하지 않습니다. 수검자 정보를 확인 바랍니다! ", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    blnExitFlag = true;
                    return;
                }

                ssPatInfo.ActiveSheet.Cells[0, 1].Text = PtInfolist.SNAME;                
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = strJumin.Substring(0, 6) + "-" + strJumin.Substring(6, 1) + "******";
                ssPatInfo.ActiveSheet.Cells[1, 1].Text = PtInfolist.SEX + "/" + PtInfolist.AGE;
                ssPatInfo.ActiveSheet.Cells[1, 3].Text = PtInfolist.ACTMEMO;                
                ssPatInfo.ActiveSheet.Cells[2, 1].Text = "";
                ssPatInfo.ActiveSheet.Cells[3, 1].Text = PtInfolist.JEPSUJONG;

                strExams = "";

                str낙상주의 = PtInfolist.GBNAKSANG;

                //암여부 확인
                HIC_JEPSU list = hicJepsuService.GetGbAmbyPaNoJepDate(FnPano, strJepDate);

                if (!list.IsNullOrEmpty())
                {
                    for (int i = 1; i < VB.L(list.GBAM, ","); i++)
                    {
                        if (VB.Pstr(list.GBAM, ",", i) == "1")
                        {
                            switch (i)
                            {
                                case 1:
                                case 2:
                                    strTemp += "위장,";
                                    break;
                                case 3:
                                    strTemp += "대장,";
                                    break;
                                case 4:
                                    strTemp += "간,";
                                    break;
                                case 5:
                                    strTemp += "유방,";
                                    break;
                                case 6:
                                    strTemp += "자궁,";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (!strTemp.IsNullOrEmpty())
                {
                    strTemp = VB.Left(strTemp, strTemp.Length - 1);
                    ssPatInfo.ActiveSheet.Cells[2, 1].Text = "☞" + strTemp;
                }

                if (str낙상주의 == "Y")
                {
                    //ssPatInfo.ActiveSheet.Cells[2, 1].Text = "★낙상주의 " + ssPatInfo.ActiveSheet.Cells[2, 1].Text;
                    string sTmp = ssPatInfo.ActiveSheet.Cells[2, 1].Text;
                    ssPatInfo.ActiveSheet.Cells[2, 1].Text = "★낙상주의 " + sTmp;                    
                }

                //검사 참고사항을 표시함
                List<HIC_JEPSU> list2 = hicJepsuService.GetExamRemarkbyPanoJepDate(FnPano, strJepDate);

                strTemp = "";
                if (list2.Count > 0)
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        if (!list2[i].EXAMREMARK.IsNullOrEmpty())
                        {
                            if (VB.InStr(strTemp, list2[i].EXAMREMARK) == 0)
                            {
                                strTemp += list2[i].EXAMREMARK + ","; 
                            }
                        }
                    }
                    if (!strTemp.IsNullOrEmpty())
                    {
                        ssPatInfo.ActiveSheet.Cells[1, 3].Text = strTemp;
                    }
                }

                //방사선 번호 찾기
                strXrayno = "";
                txtXrayNo.Text = "";
                strXrayno = PtInfolist.XRAYNO;
                if (!strXrayno.IsNullOrEmpty())
                {
                    strTemp = hicXrayResultService.GetXrayNobyXrayNo(strXrayno);
                    if (!strTemp.IsNullOrEmpty())
                    {
                        Menu01.Enabled = true;
                    }
                    else
                    {
                        Menu01.Enabled = false;
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
                List<HIC_JEPSU> list3 = hicJepsuService.GetWrtnoByPano_All(FnPano, strJepDate);

                for (int i = 0; i < list3.Count; i++)
                {
                    strAllWrtno.Add(list3[i].WRTNO);
                }

                conHcPatInfo1.SetDisPlay("25420", "O", FstrSDate, FstrPtno, "HR", strGjJong);
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

            List<EXAM_DISPLAY> ExamDspList = examDisplayService.Read_ExamList(FnWRTNO, FstrPartG, "2", strAllWrtno);

            nREAD = ExamDspList.Count;
            nRow = 0;
            strFlag = "N";

            for (int i = 0; i < nREAD; i++)
            {
                strResult = ExamDspList[i].RESULT;
                strResCode = ExamDspList[i].RESCODE;
                strResultType = ExamDspList[i].RESULTTYPE;
                strGbCodeUse = ExamDspList[i].GBCODEUSE;
                strExCode = ExamDspList[i].EXCODE;
                strHName = ExamDspList[i].HNAME;
                nRow += 1;

                if (nRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = nRow;
                }

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].Text = strExCode;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.EXAMNAME].Text = strHName;

                if (strExCode == "ZD04")
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].BackColor = Color.FromArgb(128, 255, 255);
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.CODE].BackColor = Color.FromArgb(255, 255, 255);
                }

                ufn_Combo_Set(strResCode, nRow - 1);

                if (!strResCode.IsNullOrEmpty() && !strResult.IsNullOrEmpty())
                {
                    HIC_RESCODE ResCodeList = hicRescodeService.Read_ResCode_Single(strResCode, strResult);

                    if (!ResCodeList.IsNullOrEmpty())
                    {
                        strCode = ResCodeList.CODE;
                        strName = ResCodeList.NAME;
                        clsSpread.gSdCboItemFindLeft(SS2, nRow - 1, (int)clsHcType.Instrument_Result.RESULT, 2, strCode); //Spread Combo Item 찾아서 매칭
                    }
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = strResult;
                }

                //SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].ForeColor = Color.FromArgb(0, 0, 0);
                ////A103(비만도)는 자동계산(입력금지)
                //if (strGbCodeUse == "N" || strExCode == "A103")
                //{
                //    if (strExCode != "A151" && strExCode != "TH01" && strExCode != "TH02")
                //    {
                //        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
                //        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.HELPBTN].CellType = txt;

                //        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.HELPBTN].Text = "";
                //    }
                //}

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.EXAMNAME].Text = ExamDspList[i].HNAME;
                //SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].Text = strResult;
                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].ForeColor = Color.FromArgb(0, 0, 0);

                #region 2017-02-06 소변컵 스캔 PC에서 결과가 공란은 자동으로 정상으로 표시
                if (FbUAScanPc == true && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULT].ForeColor = Color.FromArgb(0, 0, 255);
                    switch (strExCode)
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

                #region (비만도 자동계산함)
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

                #region 자동계산은 선택못함
                switch (strExCode)
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
                
                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.RESULTCODE].Text = ha.READ_ResultName(strResCode, strResult);
                    }
                }

                if (FstrSex == "M")
                {
                    strNomal = ExamDspList[i].MIN_M + "~" + ExamDspList[i].MAX_M;
                }
                else
                {
                    strNomal = ExamDspList[i].MIN_F + "~" + ExamDspList[i].MAX_F;
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

                SS2.ActiveSheet.Cells[nRow - 1, (int)clsHcType.Instrument_Result.PART].Text = ExamDspList[i].ENTPART;
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
                combo.MaxDrop = ResCodeList.Count;
                combo.MaxLength = 150;
                combo.ListWidth = 150;
                combo.Editable = true;
                SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.RESULT].CellType = combo;

                SS2.ActiveSheet.Cells[nRow, (int)clsHcType.Instrument_Result.HELP].Text = "Y";
            }
        }

        void ufn_Combo_Set_Remark(string strGubun)
        {
            string strList = "";

            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            //자료를 READ
            List<HIC_BCODE> list = hicBcodeService.GetItembyGubun(strGubun);

            if (list.Count > 0)
            {
                string[] arrCodeName = list.GetStringArray("CODENAME");
                combo.Items = arrCodeName;
                combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                combo.MaxDrop = 6;
                combo.MaxLength = 150;
                combo.ListWidth = 150;
                combo.Editable = false;
                SS2.ActiveSheet.Cells[1, 3].CellType = combo;
            }
        }

        /// <summary>
        /// 종전 2개의 결과 보여줌
        /// </summary>
        void ufn_Screen_OLD_Result_Display(string argJepDate)
        {
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1).Text = "결과 1";
            SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT2).Text = "결과 2";

            List<HIC_JEPSU> JepsuList = hicJepsuService.Read_Wrtno_SDate(FnPano, argJepDate);

            nREAD = JepsuList.Count;

            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    if (i > 1) break;

                    SS2.ActiveSheet.ColumnHeader.Cells.Get(0, (int)clsHcType.Instrument_Result.PRERESULT1 + i).Value = JepsuList[i].JEPDATE;

                    List<HIC_RESULT> RsltList = hicResultService.Get_Results(JepsuList[i].WRTNO);

                    for (int j = 0; j < RsltList.Count; j++)
                    {
                        strExCode = RsltList[j].EXCODE;
                        for (int k = 0; k < SS2.ActiveSheet.RowCount; k++)
                        {
                            if (SS2.ActiveSheet.Cells[k, (int)clsHcType.Instrument_Result.CODE].Text.Trim() == strExCode)
                            {
                                SS2.ActiveSheet.Cells[k, (int)clsHcType.Instrument_Result.PRERESULT1 + i].Text = RsltList[j].RESULT;
                            }
                        }
                        //키,몸무게 종전값 보관
                        if (FnOLD_Height == 0 && strExCode == "A101")
                        {
                            FnOLD_Height = RsltList[j].RESULT.To<double>();
                        }
                        if (FnOLD_Weight == 0 && strExCode == "A102")
                        {
                            FnOLD_Weight = RsltList[j].RESULT.To<double>();
                        }
                        if (FnOLD_Waist == 0 && strExCode == "A115")
                        {
                            FnOLD_Waist = RsltList[j].RESULT.To<double>();
                        }

                        //2020-10-26
                        if (FnOLD_TZ08 == 0 && strExCode == "TZ08")
                        {
                            FnOLD_TZ08 = RsltList[j].RESULT.To<double>();
                        }
                        if (FnOLD_TZ09 == 0 && strExCode == "TZ09")
                        {
                            FnOLD_TZ09 = RsltList[j].RESULT.To<double>();
                        }

                        if (FnOLD_A104 == 0 && strExCode == "A104")
                        {
                            if (!RsltList[j].RESULT.IsNullOrEmpty())
                            {   
                                RsltList[j].RESULT = RsltList[j].RESULT.Replace("(", "");
                                RsltList[j].RESULT = RsltList[j].RESULT.Replace(")", "");
                            }
                            FnOLD_A104 = RsltList[j].RESULT.To<double>();
                        }
                        if (FnOLD_A105 == 0 && strExCode == "A105")
                        {
                            if (!RsltList[j].RESULT.IsNullOrEmpty())
                            {
                                RsltList[j].RESULT = RsltList[j].RESULT.Replace("(", "");
                                RsltList[j].RESULT = RsltList[j].RESULT.Replace(")", "");
                            }
                            FnOLD_A105 = RsltList[j].RESULT.To<double>();
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

            //string strGbWait = "";
            List<string> strGbWait = new List<string>();

            string strOK = "";
            string strExName = "";

            bool boolSort = false;

            sp.Spread_All_Clear(ssChk);
            ssChk.ActiveSheet.RowCount = 20;
            
            if (actingCheckService.GetCodebyWrtno(ArgWRTNO) == "3116")
            {
                strChk1 = "OK";
            }

            strPart = dtpFDate.Text.Left(1);
            
            string strGB = "";
            
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

            if (hicJepsuService.GetCountbyPaNoJepDate(ArgPano, ArgDate) > 1)
            {
                //당일 다른검진이 있으면 모두 표시
                strJong = "Y";
            }

            if (strJong.IsNullOrEmpty())
            {
                strJong = "N";
            }

            List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_HIC(ArgWRTNO, ArgDate, ArgPano, strChk1, strJong);

            nREAD = list.Count;
            sp.SetfpsRowHeight(ssChk, 32);
            ssChk.ActiveSheet.RowCount = nREAD;
            if (list.Count > 0)
            {   
                for (int i = 0; i < nREAD; i++)
                {
                    nRow1 += 1;
                    nRow += 1;

                    ssChk.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                    ssChk.ActiveSheet.Cells[i, 3].Text = list[i].ENTPART;

                    Application.DoEvents();

                    //청력(특수) 및 폐기능검사 대기인원수 표시
                    strGbWait.Clear();
                    if (list[i].NAME == "특수청력")
                    {
                        strGbWait.Add("12");
                        strGbWait.Add("13");
                    }
                    if (list[i].NAME == "폐활량")
                    {
                        strGbWait.Add("06");
                        strGbWait.Add("07");
                    }
                    //if (list[i].NAME == "자궁암검사")
                    if (list[i].NAME == "자궁경부암")
                    {
                        strGbWait.Add("14");
                    }
                    if (list[i].NAME == "구강상담")
                    {
                        strGbWait.Add("08");
                        strGbWait.Add("09");
                    }
                    if (list[i].NAME == "심전도")
                    {
                        strGbWait.Add("11");
                    }

                    //2021-08-23
                    if (list[i].NAME == "일반청력")
                    {
                        strGbWait.Add("05");
                    }
                    if (list[i].NAME == "흉부촬영")
                    {
                        strGbWait.Add("10");
                    }
                    if (list[i].NAME == "일반상담")
                    {
                        strGbWait.Add("15");
                        strGbWait.Add("16");
                        strGbWait.Add("17");
                        strGbWait.Add("18");
                    }

                    //상태점검
                    ssChk.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                    if (bColor == true)
                    {
                        ufn_Line_Color();
                    }
                    //상태점검
                    List<HIC_RESULT_ACTIVE> Actlist = hicResultActiveService.Read_Active_Hic(FnWRTNO, ArgPano, ArgDate, strJong, list[i].ENTPART);
                    if (Actlist.Count > 0)
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "미검";
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                    }
                    else
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "완료";
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                    }

                    if (ArgWRTNO == 0)
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "";
                    }

                    //대기점검
                    List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait_Hic(ArgDate, list[i].ENTPART, strGB);

                    nRead2 = Waitlist.Count;

                    if (nRead2 == 0)
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 2].Text = "0";
                    }
                    else
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 2].Text = nRead2.To<string>();
                    }

                    //청력(특수) 및 폐기능검사 대기인원수를 표시함
                    if (strGbWait.Count > 0 && rdoJepsuGubun2.Checked == true)
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 2].Text += "/" + waitCheckService.Read_Exam_Wait_Hic(strGbWait).To<string>();
                    }
                }
            }
        }

        void ACTING_CHECK_NEW(long ArgWRTNO, string ArgDate, long ArgPano = 0)
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
            //string strGbWait = "";
            List<string> strGbWait = new List<string>();
            string strOK = "";
            string strExName = "";
            bool boolSort = false;
            string strJepsuGubun = "";

            //ssJepList.Visible = false;
            tabControl1.SelectedIndex = 1;

            sp.Spread_All_Clear(ssChk);
            ssChk.ActiveSheet.RowCount = 20;

            strPart = dtpHicJepDate.Text.Left(1);

            if (rdoJepsuGubun1.Checked == true)
            {
                strJepsuGubun = "";
            }
            else if (rdoJepsuGubun2.Checked == true)
            {
                strJepsuGubun = "1";
            }
            else if (rdoJepsuGubun3.Checked == true)
            {
                strJepsuGubun = "2";
            }

            //List<ACTING_CHECK> list = actingCheckService.ACTING_CHECKbyWrtNOGubun11(ArgWRTNO, ArgDate);
            List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_HIC(ArgWRTNO, ArgDate, ArgPano, strChk1, strJong);

            nREAD = list.Count;
            sp.SetfpsRowHeight(ssChk, 32);
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
                    strGbWait.Clear();
                    if (list[i].NAME == "청력(특수)")
                    {
                        strGbWait.Add("12");
                        strGbWait.Add("13");
                    }
                    if (list[i].NAME == "폐활량")
                    {
                        strGbWait.Add("06");
                        strGbWait.Add("07");
                    }
                    if (list[i].NAME == "자궁암검사")
                    {
                        strGbWait.Add("124");
                    }
                    if (list[i].NAME == "구강상담")
                    {
                        strGbWait.Add("08");
                        strGbWait.Add("09");
                    }

                    if (i == 0)
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].ENTPART;
                        strOldHaRoom = list[i].ENTPART;
                        nRow2 = nRow;
                    }

                    ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].ENTPART;
                    if (strOldHaRoom != list[i].ENTPART)
                    {
                        strOldHaRoom = list[i].ENTPART;
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
                    //if (bColor == true)
                    //{
                    //    ufn_Line_Color_New(nRow - 1);
                    //}

                    //상태점검
                    List<HIC_RESULT_ACTIVE> Rsltlist = hicResultActiveService.Read_Result(ArgWRTNO, list[i].ENTPART);

                    if (Rsltlist.Count == 0)
                    {
                        List<HIC_RESULT_ACTIVE> Actlist2 = hicResultActiveService.Read_Active(ArgWRTNO, list[i].ENTPART);
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
                        List<HIC_RESULT_ACTIVE> Rsltlist2 = hicResultActiveService.Read_Result2(ArgWRTNO, list[i].ENTPART);

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
                    List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait_Hic(ArgDate, list[i].ENTPART, strJepsuGubun);

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
                    HIC_SANGDAM_WAIT ExamWaitlist = hicSangdamWaitService.Read_Exam_Wait(ArgDate, list[i].HCROOM);

                    if (!ExamWaitlist.IsNullOrEmpty())
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = ExamWaitlist.CNT.To<string>();

                        if (list[i].HCROOM == "99")
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                        }
                    }
                    ssChk.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].ENTPART;

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

                    //청력(특수) 및 폐기능검사 대기인원수를 표시함
                    if (strGbWait.Count > 0 && rdoJepsuGubun2.Checked == true)
                    {
                        ssChk.ActiveSheet.Cells[nRow - 1, 2].Text += "/" + waitCheckService.Read_Exam_Wait_Hic(strGbWait).To<string>();
                    }
                }

                //현 검사실 대기명단
                //List<HIC_SANGDAM_WAIT> NowExamWaitlist = hicSangdamWaitService.Read_Now_Wait(FstrRoom);

                //if (NowExamWaitlist.Count > 0)
                //{
                //    for (int i = 0; i < NowExamWaitlist.Count; i++)
                //    {
                //        ssChk.ActiveSheet.Cells[i, 5].Text = NowExamWaitlist[i].SNAME;
                //        ssChk.ActiveSheet.Cells[i, 8].Text = NowExamWaitlist[i].WRTNO.To<string>();
                //    }
                //}
            }            
        }

        void ufn_Line_Color_New(int argRow)
        {
            ssChk.ActiveSheet.Cells[argRow, 0].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[argRow, 1].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[argRow, 2].BackColor = Color.FromArgb(255, 255, 128);
            ssChk.ActiveSheet.Cells[argRow, 3].BackColor = Color.FromArgb(255, 255, 128);
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
                strTemp = list[i].NAME;
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
            if (!listAct.IsNullOrEmpty() && listAct.Count > 0)
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
            txtWrtNo.Focus();
            txtWrtNo.Select();
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

                strExCode = hcact.READ_ExamCode2XrayCode(list[i].CODE);       //검사코드
                strActExCode = list[i].EXCODE;   //액팅코드
                strResCode = list[i].RESCODE;
                strActGbn = hcact.READ_ActingCodeGubun(list[i].CODE);

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
            List<string> strExCode = new List<string>();

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
                        strExCode.Clear();
                        strExCode.Add("TX07");

                        int result = hicResultService.Update_ResultbyWrtNo(item, strExCode, "");

                        if (result < 0)
                        {
                            MessageBox.Show("검사결과 등록중 오류 발생! (골밀도검사)", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                //비만도 계산 및 Update                   
                hm.Biman_Gesan(FnWRTNO, "HIC");                 //체질량 자동계산 A117
                hm.Update_Audiometry(FnWRTNO);                  //기도청력 시 기본청력 정상입력
                //hm.MDRD_GFR_Gesan(FnWRTNO, FstrSex, FnAge, "HIC");     //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                hm.AIR3_AUTO(FnWRTNO, "HIC");                   //AIR 3분법 자동계산
                hm.LDL_Gesan(FnWRTNO);                          //LDL콜레스테롤 계산
                hm.TIBC_Gesan(FnWRTNO);                         //TIBC총철결합능 계산

                //접수마스타의 상태를 변경
                hm.Result_EntryEnd_Check(FnWRTNO);
            }
        }

        private void Remark_Value(string strRemark)
        {
            strRemarkItem = strRemark;
        }
    }
}
