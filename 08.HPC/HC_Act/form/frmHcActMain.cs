using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcActMain.cs
/// Description     : 검사결과 등록 / 변경
/// Author          : 이상훈
/// Create Date     : 2019-08-20
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHaActMain.frm(FrmHaActMain)" />

namespace HC_Act
{
    public partial class frmHcActMain : Form
    {
        string mPara1 = "";
        HeaJepsuResultService heaJepsuResultService = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicBcodeService hicBcodeService = null;
        HeaResultService heaResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaResvExamService heaResvExamService = null;
        HeaJepsuService heaJepsuService = null;
        BasPcconfigService basPcconfigService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicDoctorService hicDoctorService = null;
        BasScheduleService basScheduleService = null;
        HicWaitRoomService hicWaitRoomService = null;
        HicJepsuResultService hicJepsuResultService = null;
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;
        XrayResultnewService xrayResultnewService = null;

        ComFunc cF = null;
        clsHcMain hm = null;
        clsHcFunc hc = null;
        clsHaBase hb = null;
        clsHcAct ha = null;
        clsHcVariable hv = null;
        clsHcFunc cHF = null;

        frmHcAct1 FrmHcAct1 = null;
        frmHaAct1 FrmHaAct1 = null;
        frmHicPcSet FrmHicPcSet = null;
        frmHcActRoomSeqSet FrmHcActRoomSeqSet = null;
        frmMirErrorList FrmMirErrorList = null;

        frmHcResultInputCheckList FrmHcResultInputCheckList = null;
        frmHcChestXrayCheckcs FrmHcChestXrayCheckcs = null;
        frmHcPendList FrmHcPendList = null;
        frmHcChartAcceptReg FrmHcChartAcceptReg = null;
        frmHcChartTransferView FrmHcChartTransferView = null;
        frmHcResultRptTransitionReport FrmHcResultRptTransitionReport = null;
        frmHcJochiList FrmHcJochiList = null;

        frmHaResultAutoSend FrmHaResultAutoSend = null;
        frmHcActRemarkMgmt FrmHcActRemarkMgmt = null;

        frmHcActExamRoomStatus FrmHcActExamRoomStatus = null;

        //자동액팅 변수
        string FstrBDate;
        string FstrJepDate;
        string FstrGjYear;
        string FstrSex;
        long FnAge;
        string FstrGbChul;
        long FnHeaWRTNO;
        string FstrTemp;
        string FstrXRayCodeList;

        long FnWrtno2;
        long FnRowNo;   // 메모리타자기 위치 저장용
        long FnClickRow;   //Help를 Click한 Row
        string FstrPartExam;   //파트별 입력할 검사항목
        string FstrGjJong;
        string FstrPtno;
        long FnPano;
        string FstrPartG;
        bool FbAct12;
        bool FbAct17;
        string FstrRetValue;
        string FstrHeaLastTime;
        int FnTimerCNT;

        #region //MainFormMessage

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }
        public void MsgUnloadForm(Form frm)
        {
        }
        public void MsgFormClear()
        {
        }
        public void MsgSendPara(string strPara)
        {
        }
        #endregion //MainFormMessage

        public frmHcActMain()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcActMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHcActMain(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuResultService = new HeaJepsuResultService();
            comHpcLibBService = new ComHpcLibBService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicBcodeService = new HicBcodeService();
            heaResultService = new HeaResultService();
            hicXrayResultService = new HicXrayResultService();
            heaResvExamService = new HeaResvExamService();
            heaJepsuService = new HeaJepsuService();
            basPcconfigService = new BasPcconfigService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicDoctorService = new HicDoctorService();
            basScheduleService = new BasScheduleService();
            hicWaitRoomService = new HicWaitRoomService();
            hicJepsuResultService = new HicJepsuResultService();
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            xrayResultnewService = new XrayResultnewService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.menuExit.Click += new EventHandler(eMenuClick);
            this.menuMeasurement.Click += new EventHandler(eMenuClick);
            this.menuEtc.Click += new EventHandler(eMenuClick);

            this.MenuEtc01.Click += new EventHandler(eMenuClick);       //일일검진마감
            this.MenuEtc02.Click += new EventHandler(eMenuClick);       //문진표 누락점검
            this.MenuEtc03.Click += new EventHandler(eMenuClick);       //결과입력 점검
            this.MenuEtc04.Click += new EventHandler(eMenuClick);       //수검상태 조회
            this.MenuEtc05.Click += new EventHandler(eMenuClick);       //건진청구 오류 수정의뢰
            this.MenuEtc06.Click += new EventHandler(eMenuClick);       //흉부촬영 점검
            this.MenuEtc07.Click += new EventHandler(eMenuClick);       //보류대장
            this.MenuEtc08.Click += new EventHandler(eMenuClick);       //차트인수 등록
            this.MenuEtc09.Click += new EventHandler(eMenuClick);       //차트인계 조회        
            this.MenuEtc10.Click += new EventHandler(eMenuClick);       //조치대상자 조회(성인) 
            this.MenuEtc11.Click += new EventHandler(eMenuClick);       //조치대상자 조회(학생)  

            this.menuSearch.Click += new EventHandler(eMenuClick);

            this.MenuSearch01.Click += new EventHandler(eMenuClick);    //미상담자 조회
            this.MenuSearch02.Click += new EventHandler(eMenuClick);    //미판정자 조회
            this.MenuSearch03.Click += new EventHandler(eMenuClick);    //수검자별 Acting 상태조회
            this.MenuSearch04.Click += new EventHandler(eMenuClick);    //공단자격[상실자] 접수내역
            this.MenuSearch05.Click += new EventHandler(eMenuClick);    //사업구분별 검진종류 접수 오류
            this.MenuSearch06.Click += new EventHandler(eMenuClick);    //사업구분별 검진종류 접수 오류
            this.MenuSearch07.Click += new EventHandler(eMenuClick);    //증번호 주낙대상 접수 내역
            this.MenuSearch08.Click += new EventHandler(eMenuClick);    //암검진 부담률 수납 오류 내역
            this.MenuSearch09.Click += new EventHandler(eMenuClick);    //당일 접수구분 변경 및 접수취소 조회

            this.menuRsltReport.Click += new EventHandler(eMenuClick);  //결과지본인수령관리대장
            this.menuPCEnvrionment.Click += new EventHandler(eMenuClick);
            this.MenuEnv01.Click += new EventHandler(eMenuClick);   //PC 환경설정
            this.MenuEnv02.Click += new EventHandler(eMenuClick);   //계측항목 순번 관리
            this.MenuEnv03.Click += new EventHandler(eMenuClick);   //참고사항 문구 관리 
            
            this.menuWaitStatus.Click += new EventHandler(eMenuClick);   //일반검진/종합검진 대기현황
            this.menuHeaResAutoSend.Click += new EventHandler(eMenuClick);        //종검결과 자동전송
            this.menuHicResSend.Click += new EventHandler(eMenuClick);        //일반검진 결과전송
            

            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timer2.Tick += new EventHandler(eTimerTick);
            this.timer3.Tick += new EventHandler(eTimerTick);
            this.timer4.Tick += new EventHandler(eTimerTick);

            this.MouseDoubleClick += new MouseEventHandler(eMouseDblClick);

            this.ContextMenuExit.Click += new EventHandler(eContextMenu);
            this.ContextMenuReturn.Click += new EventHandler(eContextMenu);
        }

        void eContextMenu(object sender, EventArgs e)
        {
            if (sender == ContextMenuExit)
            {
                eMenuClick(menuExit, new EventArgs());
            }
            else if (sender == ContextMenuReturn)
            {
                this.Visible = true;                    //폼 보이게...
                this.ShowInTaskbar = false;             //태스크바에서 안보이게..
                this.trayAutoSend.Visible = true;       //트레이아이콘 보이게..
            }
        }

        void eMouseDblClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Show();
            this.Visible = true;                    //폼 보이게
            this.ShowInTaskbar = true;              //태스크바에서 보이게
            this.trayAutoSend.Visible = true;       //트레이아이콘 보이게
        }

        void SetControl()
        {
            cF = new ComFunc();
            hm = new clsHcMain();
            hc = new clsHcFunc();
            hb = new clsHaBase();
            ha = new clsHcAct();
            hv = new clsHcVariable();
            cHF = new clsHcFunc();
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strTemp = "";
            int result = 0;
            string strDate = "";
            string strLastTime = "";
            string sLine = "";
            string strPath = "";

            this.Text += " " + "☞작업자: " + clsType.User.UserName;

            clsHcVariable.GstrHicPart = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");

            if (clsHcVariable.GstrHicPart.IsNullOrEmpty())
            {
                MessageBox.Show("일반검진/종합검진 Setting이 되어 있지 않습니다!!!" + "\r\n" + "부서 설정 후 프로그램이 종료됩니다!" + "\r\n" + "재시작 하십시오!", "PC Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                frmHicPcSet f = new frmHicPcSet();
                f.ShowDialog();
                this.Close();
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();
            cHF.SET_자료사전_VALUE();

            FstrTemp = this.Text;

            clsQuery.READ_PC_CONFIG(clsDB.DbCon);
            hc.Read_INI();

            List<BAS_PCCONFIG> Configlst = basPcconfigService.GetConfig(clsCompuInfo.gstrCOMIP);
            for (int i = 0; i < Configlst.Count; i++)
            {
                clsHcVariable.GstrExam[i] = Configlst[i].CODE;
            }

            strTemp = "";
            
            for (int i = 0; i < Configlst.Count; i++)
            {
                if (!clsHcVariable.GstrExam[i].IsNullOrEmpty())
                {
                    strTemp = "OK";
                    break;
                }
            }
            timer1.Enabled = false;

            if (clsHcVariable.GstrHicPart == "1")//종검
            {
                if (clsHcVariable.GstrHeaAutoActingYN == "Y")
                {
                    //this.ShowInTaskbar = true;              //태스크바에서 보이게
                    //this.trayAutoSend.Visible = true;       //트레이아이콘 보이게
                    //this.toolStripStatusLabel1.Text = "종검결과 자동전송";
                    //this.Hide();

                    pnlActPic.Visible = true;

                    pnlActPic.Left = (this.ClientSize.Width - pnlActPic.Width) / 6;
                    pnlActPic.Top = 800;
                    //pnlActPic.Top = (this.ClientSize.Height - pnlActPic.Height) / 2;

                    timer2.Enabled = true;
                    timer1.Enabled = true;
                    timer3.Enabled = true;

                    eTimerTick(timer1, new EventArgs());

                    Application.DoEvents();
                }
                else
                {
                    pnlActPic.Visible = false;

                    if (strTemp == "OK")
                    {
                        eMenuClick(menuMeasurement, new EventArgs());
                        timer1.Enabled = false;
                    }
                    else
                    {
                        if (FrmHaAct1 == null)
                        {
                            FrmHaAct1 = new frmHaAct1();
                            themTabForm(FrmHaAct1, this.panMain);
                        }
                        else
                        {
                            if (FormIsExist(FrmHaAct1) == true)
                            {
                                FormVisiable(FrmHaAct1);
                            }
                            else
                            {
                                if (FrmHaAct1 == null)
                                {
                                    FrmHaAct1 = new frmHaAct1();
                                }
                                themTabForm(FrmHaAct1, this.panMain);
                            }
                        }
                    }
                }
            }
            else if (clsHcVariable.GstrHicPart == "2")  //일반검진
            {
                if (clsPrint.gGetPrinterFind("접수증") == true)
                {
                    clsHcVariable.GnPrtNo1 = 1;
                }

                if (clsPrint.gGetPrinterFind("카드영수증") == true)
                {
                    clsHcVariable.GnPrtNo2 = 1;
                }

                if (clsPrint.gGetPrinterFind("신용카드") == true)
                {
                    clsHcVariable.GnPrtNo2 = 1;
                }

                for (int i = 0; i <= 10; i++)
                {
                    if (clsHcVariable.GstrExam[i] == "W")
                    {
                        strTemp = "OK";
                        break;
                    }
                }

                strDate = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString() + " 23:59";

                //어제까지 상담대기 자료를 삭제함
                result = hicSangdamWaitService.DeletebyEntTime(strDate);

                if (result < 0)
                {
                    MessageBox.Show("상담대기자 삭제중 오류발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //1일 1회 1층계층체중계PC 검진스케쥴을 읽음
                if (hicBcodeService.GetCountbyGuybunName("BAS_체중계연결PC", clsCompuInfo.gstrCOMIP) > 0)
                {
                    //using (StreamReader Sr = new StreamReader(System.Environment.CurrentDirectory + @"C:\SET_Doct_Schedule.txt", Encoding.Default))
                    //{
                        //while ((sLine = Sr.ReadLine()) != null)
                        //{
                        //    strLastTime += sLine + "\r\n";
                        //}

                        strLastTime = File.ReadAllText(@"C:\SET_Doct_Schedule.txt");

                        if (VB.Left(strLastTime, 10) != clsPublic.GstrSysDate)
                        {
                            fn_Set_Hic_Doct_Schedule();
                        }
                    //}
                }

                //일반건진 관리자 사번
                if (hicBcodeService.GetCountbyGubunCode("HIC_일반건진관리자사번", clsType.User.IdNumber) > 0)
                {
                    clsHcVariable.GbHicAdminSabun = true;
                }

                if (clsHcVariable.GbHicAdminSabun == false)
                {
                    MenuEtc01.Visible = false;
                    menuSearch.Visible = false;
                }

                if (hicBcodeService.GetHeaResultAutoSendPCbyGubun("HIC_종합검진결과자동전송실행PC", clsCompuInfo.gstrCOMIP) > 0)
                {
                    clsHcVariable.GstrHeaResultAutoSendPC = "Y";
                }
                else
                {
                    clsHcVariable.GstrHeaResultAutoSendPC = "";
                }

                if (clsHcVariable.GstrHeaResultAutoSendPC == "Y")   //종합검진 결과 자동전송 실행 PC 이면
                {
                    FstrBDate = clsPublic.GstrSysDate;
                    strPath = @"c:\cmc\종검결과자동전송.txt";
                    FstrHeaLastTime = File.ReadAllText(strPath);

                    if (VB.Pstr(FstrHeaLastTime, "{}", 1) != clsPublic.GstrSysDate)
                    {
                        FstrHeaLastTime = "";
                    }
                    FnTimerCNT = 0;

                    timer4.Enabled = true;
                    eTimerTick(timer4, new EventArgs());
                }
                else
                {
                    eMenuClick(menuMeasurement, new EventArgs());
                    //if (FrmHcAct1 == null)
                    //{
                    //    FrmHcAct1 = new frmHcAct1();
                    //    themTabForm(FrmHcAct1, this.panMain);
                    //}
                    //else
                    //{
                    //    if (FormIsExist(FrmHcAct1) == true)
                    //    {
                    //        FormVisiable(FrmHcAct1);
                    //    }
                    //    else
                    //    {
                    //        if (FrmHcAct1 == null)
                    //        {
                    //            FrmHcAct1 = new frmHcAct1();
                    //        }
                    //        themTabForm(FrmHcAct1, this.panMain);
                    //    }
                    //}
                }
            }
        }

        /// <summary>
        /// 오늘 검진 상담의사 스케쥴 자동 등록
        /// </summary>
        void fn_Set_Hic_Doct_Schedule()
        {
            int nREAD = 0;
            long nSabun = 0;
            string strRoom = "";
            string strDRCODE = "";
            string strAmUse = "";
            string strPmUse = "";
            string strAmSang = "";
            string strPmSang = "";
            int result = 0;
            string strPath = "";

            clsDB.setBeginTran(clsDB.DbCon);

            List<HIC_DOCTOR> list = hicDoctorService.GetItemAll();

            nREAD = list.Count;

            for (int i = 0; i < nREAD; i++)
            {
                strRoom = list[i].ROOM;
                strDRCODE = list[i].DRCODE;
                nSabun = list[i].SABUN;
                strAmUse = "N";
                strPmUse = "N";
                strAmSang = "N";
                strPmSang = "N";

                //오늘의 검진 스케쥴을 읽음
                BAS_SCHEDULE list2 = basScheduleService.GetGbJinGbJin2bySchDateDrCode(clsPublic.GstrSysDate, strDRCODE);

                if (list2.GBJIN == "1") { strAmUse = "Y"; }
                if (list2.GBJIN2 == "1") { strPmUse = "Y"; }
                if (list2.GBJIN == "C") { strAmSang = "Y"; strAmUse = "Y"; }
                if (list2.GBJIN2 == "C") { strPmSang = "Y"; strPmUse = "Y"; }

                HIC_WAIT_ROOM item = new HIC_WAIT_ROOM();

                item.AMUSE = strAmUse;
                item.PMUSE = strPmUse;
                item.AMSANG = strAmSang;
                item.PMSANG = strPmSang;
                item.ROOM = strRoom;

                result = hicWaitRoomService.UpdatebyRoom(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("HIC_WAIT_ROOM Update 중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            strPath = @"C:\SET_Doct_Schedule.txt";
            File.WriteAllText(strPath, clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime);
        }

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
        }

        /// <summary>
        /// Main 폼에서 폼이 로드된 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool FormIsExist(Form frm)
        {
            foreach (Control ctl in this.panMain.Controls)
            {
                if (ctl is Form)
                {
                    if (ctl.Name == frm.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuMeasurement)  //계측업무
            {
                pnlActPic.Visible = false;
                timer2.Enabled = false;
                timer3.Enabled = false;

                if (clsHcVariable.GstrHicPart == "1")   //종합검진
                {
                    if (cHF.OpenForm_Check("frmHaAct1") == true)
                    {
                        return;
                    }

                    FrmHaAct1 = new frmHaAct1(this);
                    themTabForm(FrmHaAct1, this.panMain);
                }
                else if (clsHcVariable.GstrHicPart == "2")  //일반검진
                {
                    if (cHF.OpenForm_Check("frmHcAct1") == true)
                    {
                        return;
                    }

                    FrmHcAct1 = new frmHcAct1();
                    themTabForm(FrmHcAct1, this.panMain);
                }
                //else
                //{
                //    MessageBox.Show("일반검진/종합검진 Setting이 되어 있지 않습니다!!!" + "\r\n" + "부서 설정 후 프로그램이 종료됩니다!" + "\r\n" + "재시작 하십시오!", "PC Setting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    frmHicPcSet f = new frmHicPcSet();
                //    f.ShowDialog();
                //    Application.Exit();
                //}
            }
            else if (sender == menuEtc)     //검진일일점검 - 사용무
            {

            }
            else if (sender == MenuEtc01)     //일일검진마감 - 사용무
            {

            }
            else if (sender == MenuEtc02)     //문진표 누락점검 - 사용무
            {

            }
            else if (sender == MenuEtc03)     //결과입력 점검 - 사용무
            {
                
            }
            else if (sender == MenuEtc04)     //수검상태 조회
            {
                FrmHcResultInputCheckList = new frmHcResultInputCheckList();
                FrmHcResultInputCheckList.ShowDialog(this);
            }
            else if (sender == MenuEtc05)     //건진청구 오류 수정의뢰
            {
                FrmMirErrorList = new frmMirErrorList();
                FrmMirErrorList.ShowDialog(this);
            }
            else if (sender == MenuEtc06)     //흉부촬영 점검
            {
                FrmHcChestXrayCheckcs = new frmHcChestXrayCheckcs();
                FrmHcChestXrayCheckcs.ShowDialog(this);
                
            }
            else if (sender == MenuEtc07)     //보류대장
            {
                FrmHcPendList = new frmHcPendList();
                FrmHcPendList.ShowDialog(this);
            }
            else if (sender == MenuEtc08)     //차트인수 등록
            {
                FrmHcChartAcceptReg = new frmHcChartAcceptReg();
                FrmHcChartAcceptReg.ShowDialog(this);
            }
            else if (sender == MenuEtc09)     //차트인계 조회      
            {
                FrmHcChartTransferView = new frmHcChartTransferView();
                FrmHcChartTransferView.ShowDialog(this);
            }
            else if (sender == MenuEtc10)     //조치대상자 조회(성인)   
            {
                FrmHcJochiList = new frmHcJochiList();
                FrmHcJochiList.ShowDialog(this);
            }
            else if (sender == MenuEtc11)     //조치대상자 조회(학생)      
            {
                FrmHcJochiList = new frmHcJochiList();
                FrmHcJochiList.ShowDialog(this);
            }

            else if (sender == menuSearch)  //조회업무 - 사용무
            {

            }
            else if (sender == MenuSearch01)  //미상담자 조회 - 사용무
            {

            }
            else if (sender == MenuSearch02)  //미판정자 조회 - 사용무
            {

            }
            else if (sender == MenuSearch03)  //수검자별 Acting 상태조회 - 사용무
            {

            }
            else if (sender == MenuSearch04)  //공단자격[상실자] 접수내역 - 사용무
            {

            }
            else if (sender == MenuSearch05)  //사업구분별 검진종류 접수 오류 - 사용무
            {

            }
            else if (sender == MenuSearch06)  //사업구분별 검진종류 접수 오류 - 사용무
            {

            }
            else if (sender == MenuSearch07)  //증번호 주낙대상 접수 내역 - 사용무
            {

            }
            else if (sender == MenuSearch08)  //암검진 부담률 수납 오류 내역 - 사용무
            {

            }
            else if (sender == MenuSearch09)  //당일 접수구분 변경 및 접수취소 조회 - 사용무
            {

            }
            else if (sender == menuRsltReport)  //결과지본인수령관리대장
            {
                ///TODO : 이상훈 (2020.09.21) - WBS에 없음
                FrmHcResultRptTransitionReport = new frmHcResultRptTransitionReport();
                FrmHcResultRptTransitionReport.StartPosition = FormStartPosition.CenterParent;
                FrmHcResultRptTransitionReport.ShowDialog(this);
                FrmHcResultRptTransitionReport.Dispose();
                FrmHcResultRptTransitionReport = null;
            }
            else if (sender == menuPCEnvrionment)  //환경설정
            {
            }    
            else if (sender == MenuEnv01)   //PC 환경설정
            {
                FrmHicPcSet = new frmHicPcSet();
                FrmHicPcSet.StartPosition = FormStartPosition.CenterParent;
                FrmHicPcSet.ShowDialog();
            }
            else if (sender == MenuEnv02)   ////계측항목 순번 관리
            {
                FrmHcActRoomSeqSet = new frmHcActRoomSeqSet();
                FrmHcActRoomSeqSet.StartPosition = FormStartPosition.CenterParent;
                FrmHcActRoomSeqSet.ShowDialog();
            }
            else if (sender == MenuEnv03)   //참고사항 문구 관리
            {
                FrmHcActRemarkMgmt = new frmHcActRemarkMgmt();
                FrmHcActRemarkMgmt.StartPosition = FormStartPosition.CenterParent;
                FrmHcActRemarkMgmt.ShowDialog();
            }
            else if (sender == menuWaitStatus)  //일반검진/종합검진 대기현황
            {
                FrmHcActExamRoomStatus = new frmHcActExamRoomStatus(clsHcVariable.GstrHicPart);
                FrmHcActExamRoomStatus.StartPosition = FormStartPosition.CenterParent;
                FrmHcActExamRoomStatus.ShowDialog(this);
            }
            else if (sender == menuHeaResAutoSend)
            {
                frmHaResultAutoSend f = new frmHaResultAutoSend();
                f.Show();
            }
            else if (sender == menuHicResSend)
            {
                string strBDate = VB.InputBox("자동액팅할 날짜는? (예: YYYY-MM-DD)", "자동액팅날짜선택", clsPublic.GstrSysDate);

                fn_Today_Acting_Update(strBDate);
                fn_Today_AutoActing(strBDate, "HIC");
            }
            else if (sender == menuExit)    //종료
            {
                this.Close();
                return;
            }
        }

        public void eTimerTick(object sender, EventArgs e)
        {
            string strDate = "";
            string strPtNo = "";

            Application.DoEvents();

            if (sender == timer1)
            {
                if (clsHcVariable.GstrHicPart == "1")//종검
                {
                    //3분마다 점검 (18)            
                    clsHcVariable.GnAutoTimerCnt += 1;
                    if (clsHcVariable.GnAutoTimerCnt < 18)
                    {
                        return;
                    }

                    if (pnlActPic.Visible == false)
                    {
                        pnlActPic.Visible = true;

                        pnlActPic.Left = (this.ClientSize.Width - pnlActPic.Width) / 6;
                        pnlActPic.Top = 800;
                        //pnlActPic.Top = (this.ClientSize.Height - pnlActPic.Height) /2;
                    }

                    timer1.Enabled = false;
                    clsHcVariable.GnAutoTimerCnt = 0;

                    timer2.Enabled = true;
                    timer1.Enabled = true;
                    timer3.Enabled = true;
                    Application.DoEvents();



                    //ComFunc.ReadSysDate(clsDB.DbCon);
                    strDate = clsPublic.GstrSysDate;
                    //종검자동액팅을 변경함
                    //당일검진_자동액팅(strDate);
                    fn_Today_AutoActing(strDate, "HEA");
                    //Auto_Acting_Update 종검자동액팅 Main 작업
                    fn_Auto_Acting_Update();
                    //예약 대장내시경,산부인과초음파,TCD 외래 자동접수
                    fn_YEYAK_OPD_MASTER_INSERT();

                    timer1.Enabled = true;
                }
            }
            else if (sender == timer2)
            {
                if (timer2.Enabled == true)
                {
                    pictureBox1.Location = new Point(3, 3);
                    pictureBox2.Location = new Point(3, 3);
                    pictureBox3.Location = new Point(3, 3);
                    pictureBox4.Location = new Point(3, 3);
                    pictureBox5.Location = new Point(3, 3);
                    pictureBox6.Location = new Point(3, 3);
                    pictureBox7.Location = new Point(3, 3);
                    pictureBox8.Location = new Point(3, 3);

                    if (pictureBox1.Visible == true)
                    {
                        pictureBox1.Visible = false;
                        pictureBox2.Visible = true;
                    }
                    else if (pictureBox2.Visible == true)
                    {
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = true;
                    }
                    else if (pictureBox3.Visible == true)
                    {
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = true;
                    }
                    else if (pictureBox4.Visible == true)
                    {
                        pictureBox4.Visible = false;
                        pictureBox5.Visible = true;
                    }
                    else if (pictureBox5.Visible == true)
                    {
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = true;
                    }
                    else if (pictureBox6.Visible == true)
                    {
                        pictureBox6.Visible = false;
                        pictureBox7.Visible = true;
                    }
                    else if (pictureBox7.Visible == true)
                    {
                        pictureBox7.Visible = false;
                        pictureBox8.Visible = true;
                    }
                    else if (pictureBox8.Visible == true)
                    {
                        pictureBox8.Visible = false;
                        pictureBox1.Visible = true;
                    }
                }
            }
            else if (sender == timer3)
            {
                if (timer2.Enabled == true)
                {
                    if (lblAutoSendGuide.ForeColor == Color.Blue)
                    {
                        lblAutoSendGuide.ForeColor = Color.White;
                    }
                    else
                    {
                        lblAutoSendGuide.ForeColor = Color.Blue;
                    }
                }
            }
            else if (sender == timer4)
            {
                string strYoil = "";
                bool bOK = false;
                bool bLastSend = false;
                string strPath = @"c:\cmc\종검결과자동전송.txt";

                //-------------------------------------------------------
                //  종합검진 결과 자동전송 실행
                //  작업시간(평일):   08:00, 13:00, 16:30
                //  작업시간(토요일): 08:00, 11:30
                //-------------------------------------------------------

                ComFunc.ReadSysDate(clsDB.DbCon);
                strYoil = cF.READ_YOIL(clsDB.DbCon, clsPublic.GstrSysDate);

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
                    File.WriteAllText(strPath, FstrHeaLastTime);

                    if (cHF.OpenForm_Check("frmHaResultAutoSend") == false)
                    {
                        FrmHaResultAutoSend = new frmHaResultAutoSend();
                        FrmHaResultAutoSend.StartPosition = FormStartPosition.CenterParent;
                        FrmHaResultAutoSend.ShowDialog(this);
                        FnTimerCNT = 0;
                    }
                }

                //평일: 16:30, 토요일: 11:30 마감 전송
                if (bLastSend == true)
                {
                    FnTimerCNT = 0;
                    fn_Today_Acting_Update(FstrBDate);
                }

                //5분마다 작업을 처리함
                FnTimerCNT += 1;
                if (FnTimerCNT > 5)
                {
                    FnTimerCNT = 0;
                    fn_Today_AutoActing(FstrBDate, "HIC");
                }

                //출장(뇨검사, 심전도, 흉부 자동액팅)
                if (string.Compare(clsPublic.GstrSysTime, "09:00") >= 0 && string.Compare(clsPublic.GstrSysTime, "09:10") <= 0)
                {
                    fn_Auto_Acting_Update_Chul();
                }

                timer4.Enabled = true;
            }
        }

        /// <summary>
        /// 당일검진결과_UPDATE (일반검진)
        /// </summary>
        /// <param name="argBDate"></param>
        void fn_Today_Acting_Update(string argBDate)
        {
            int nRead = 0;
            long nWrtNo = 0;
            string strPtNo = "";
            string strResult = "";
            int result = 0;

            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDateGbSts(argBDate);

            nRead = list.Count;

            clsDB.setBeginTran(clsDB.DbCon);
            for (int i = 0; i < nRead; i++)
            {
                FstrJepDate = list[i].JEPDATE;
                FstrGjYear = list[i].GJYEAR;
                FstrSex = list[i].SEX;
                FnAge = list[i].AGE;
                FstrGbChul = list[i].GBCHUL;
                nWrtNo = list[i].WRTNO.To<long>();
                strPtNo = string.Format("{0:00000000}", list[i].PTNO);

                //if (nWrtNo == 1092675)
                //{
                //    nWrtNo = nWrtNo;
                //}

                ////검사결과 완료 Flag 자동등록
                //구강검사(ZD00),(ZD01) 누락자는 "." 찍기
                HIC_RESULT list2 = hicResultService.GetExCodeResultbyWrtNoInExCode(nWrtNo);

                if (!list2.IsNullOrEmpty())
                {
                    //result = hicResultService.UpdateTeethbyWrtNoExCode(nWrtNo, clsType.User.IdNumber); 
                    result = hicResultService.UpdateTeethbyWrtNoExCode(nWrtNo, "111");

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("검사결과 일괄등록중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //출장검진은 체혈,X - Ray,폐활량 액팅 자동으로 달기
                if (FstrGbChul == "Y")
                {
                    if (hicResultService.GetCountByWrtnoInExCode(nWrtNo) > 0)
                    {
                        //결과를 저장
                        result = hicResultService.UpdateResultEntSabunEntTimeActivebyWrtNoExCode(nWrtNo, clsType.User.IdNumber);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("검사결과(출장체혈) 등록중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                List<XRAY_RESULTNEW> list3 = xrayResultnewService.GetItembyPaNoSeekDateXCode(strPtNo, argBDate, "HC341");

                if (list3.Count == 1)
                {
                    //if (string.Compare(list3[0].RESULT, "-1") > 0)
                    //{
                    //    strResult = "01";
                    //}
                    //else if (string.Compare(list3[0].RESULT, "-1.1") <= 0 && string.Compare(list3[0].RESULT, "-2.4") >= 0)
                    //{
                    //    strResult = "02";
                    //}
                    //else if (string.Compare(list3[0].RESULT, "-2.5") <= 0)
                    //{
                    //    strResult = "03";
                    //}


                    if (Convert.ToDouble(list3[0].RESULT) >= -1)
                    {
                        strResult = "01";
                    }
                    else if (Convert.ToDouble(list3[0].RESULT) <= -1.1 && Convert.ToDouble(list3[0].RESULT) >= -2.4)
                    {
                        strResult = "02";
                    }
                    else if (Convert.ToDouble(list3[0].RESULT) <= 2.5)
                    {
                        strResult = "03";
                    }

                    //골밀도검사 업데이트
                    if (hicResultService.GetCountbyWrtNoExCodeNoResult(nWrtNo, "TX07") > 0)
                    {
                        //결과를 저장
                        //result = hicResultService.UpdatebyWrtNoExCodeTX07(nWrtNo, strResult, clsType.User.IdNumber);
                        result = hicResultService.UpdatebyWrtNoExCodeTX07(nWrtNo, strResult, "111");

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("검사결과(골밀도검사) 등록중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                //비만도 계산 및 Update                   
                hm.Biman_Gesan(nWrtNo, "HIC");                  //체질량 자동계산 A117
                hm.Update_Audiometry(nWrtNo);                   //기도청력 시 기본청력 정상입력
                //hm.MDRD_GFR_Gesan(nWrtNo, FstrSex, FnAge, "HIC");      //CFR 자동계산 2009년부터 => MDRD_GFR_Gesan()로 변경
                hm.AIR3_AUTO(nWrtNo, "HIC");                    //AIR 3분법 자동계산
                hm.LDL_Gesan(nWrtNo);                           //LDL콜레스테롤 계산
                hm.TIBC_Gesan(nWrtNo);                          //TIBC총철결합능 계산

                //접수마스타의 상태를 변경
                hm.Result_EntryEnd_Check(nWrtNo);
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 당일검진결과_UPDATE (일반검진)
        /// </summary>
        /// <param name=></param>
        void fn_Auto_Acting_Update_Chul()
        {

            long nREAD = 0;
            long nPano = 0;
            long nWRTNO = 0;
            string strROWID = "";
            string strPtNo = "";
            string strJepDate = "";
            
            int result = 0;

            //----------------------------------
            //  출장검진 자동 액팅
            //----------------------------------
            List<HIC_JEPSU_RESULT> listAct = hicJepsuResultService.GetItembyGbChulExCode();

            clsDB.setBeginTran(clsDB.DbCon);

            nREAD = listAct.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strROWID = listAct[i].RID;
                strPtNo = listAct[i].PTNO;
                strJepDate = listAct[i].JEPDATE;
                nWRTNO = listAct[i].WRTNO;
                nPano = listAct[i].PANO;

                //결과:01 // 액팅Y
                result = hicResultService.UpdateResultActiveEntSabunEntTimebyRowId(clsType.User.IdNumber, strROWID);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("출장검진 자동 액팅중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    //접수마스타의 상태를 변경
                    //hm.Result_EntryEnd_Check(nWRTNO);
            }

            clsDB.setCommitTran(clsDB.DbCon);

        }


        /// <summary>
        /// 당일검진_자동액팅
        /// </summary>
        /// <param name="argDate"></param>
        void fn_Today_AutoActing(string argBDate, string argGubun)
        {
            int nREAD = 0;
            int nRead2 = 0;
            string strROWID = "";
            string strPtNo = "";
            long nPano = 0;
            string strJepDate = "";
            string strActExCode = "";
            string strExCode = "";
            string strActGbn = "";
            string strActOK = "";
            string strActValue = "";
            long nWRTNO = 0;
            string strFrDate = "";
            string strToDate = "";
            int result = 0;

            List<HEA_JEPSU_RESULT> list = heaJepsuResultService.GetItembySDate(argBDate, argGubun);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strROWID = list[i].RID;
                strPtNo = list[i].PTNO;
                strJepDate = list[i].JEPDATE;
                nWRTNO = list[i].WRTNO;
                nPano = list[i].PANO;
                strExCode = ha.READ_ExamCode2_XrayCode(list[i].CODE.Trim());       //검사코드
                if (!strExCode.IsNullOrEmpty()) { strExCode = strExCode.Trim(); }
                strActExCode = list[i].EXCODE;                              //액팅코드
                strActGbn = ha.READ_ActingCodeGubun(list[i].CODE.Trim());

                strFrDate = strJepDate;
                strToDate = DateTime.Parse(strJepDate).AddDays(1).ToShortDateString();

                strActOK = "";
                if (argGubun == "HEA")
                {
                    switch (strActGbn)
                    {
                        case "방사선":
                            if (comHpcLibBService.GetXrayDetailbyPanoSeekDateXCode(strPtNo, strFrDate, strToDate, strExCode) > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        case "위내시경":
                            if (comHpcLibBService.GetEndoJupMstbyPtnoRDate(strPtNo, strFrDate, strToDate, "2") > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        case "대장내시경":
                            if (comHpcLibBService.GetEndoJupMstbyPtnoRDate(strPtNo, strFrDate, strToDate, "3") > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        case "EKG":
                            if (comHpcLibBService.GetEtcJupMstbyPtnoBDate(strPtNo, strJepDate) > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (argGubun == "HIC")
                {
                    switch (strActGbn)
                    {
                        case "방사선":
                            if (comHpcLibBService.GetXrayDetailbyPanoSeekDateXCode(strPtNo, strFrDate, strToDate, strExCode) > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        case "위내시경":
                            if (comHpcLibBService.GetEndoJupMstbyPtnoRDate(strPtNo, strFrDate, strToDate, "2") > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        case "대장내시경":
                            if (comHpcLibBService.GetEndoJupMstbyPtnoRDate(strPtNo, strFrDate, strToDate, "3") > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        case "진단검사":
                            if (comHpcLibBService.GetExam_SpecMstCountbyPtNo(strPtNo, strJepDate) > 0)
                            {
                                strActOK = "OK";
                            }
                            break;
                        default:
                            break;
                    }
                }

                strActValue = "01";

                if (strActOK == "OK" && !strActValue.IsNullOrEmpty() && !strROWID.IsNullOrEmpty())
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = heaJepsuResultService.UpdateResultActiveEntSabunbyRowIdWrtNo(strActValue, "Y", clsType.User.IdNumber.To<long>(), strROWID, nWRTNO, argGubun);
                    
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("결과저장 중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (argGubun == "HEA")
                    {
                        //내시경 액팅완료시 대기순번에서 자동으로 완료처리
                        if (strActGbn == "위내시경" && strActOK == "OK" && strActValue != "")
                        {
                            result = heaSangdamWaitService.UpdateGbCallbyWrtNoGubun("Y", nWRTNO, "09");

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("결과저장 중 오류 발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else if (argGubun == "HIC")
                    {
                        //접수마스타의 상태를 변경
                        hm.Result_EntryEnd_Check(nWRTNO);
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }

            if (argGubun == "HIC")
            {
                //----------------------------------
                //  출장검진 채혈 자동 액팅
                //----------------------------------
                List<HIC_JEPSU_RESULT> listAct = hicJepsuResultService.GetItembyJepDateGbChulExCode(argBDate);

                clsDB.setBeginTran(clsDB.DbCon);

                nREAD = listAct.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strROWID = listAct[i].RID;
                    strPtNo = listAct[i].PTNO;
                    strJepDate = listAct[i].JEPDATE;
                    nWRTNO = listAct[i].WRTNO;
                    nPano = listAct[i].PANO;

                    //혈액검사 결과 여부
                    if (hicResultService.GetCountbyWrtNoExCodeNoResult(nWRTNO) > 0)
                    {
                        result = hicResultService.UpdateResultActiveEntSabunEntTimebyRowId(clsType.User.IdNumber, strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("혈액검사 결과 저장중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //접수마스타의 상태를 변경
                        hm.Result_EntryEnd_Check(nWRTNO);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        /// <summary>
        /// 종검자동액팅 Main 작업
        /// </summary>
        void fn_Auto_Acting_Update()
        {
            int nREAD = 0;
            long nCNT = 0;
            long nWRTNO = 0;
            string strPtNo = "";
            string strExCode = "";
            //List<string> strExCode = new List<string>();
            string strData = "";
            //List<string> strTemp = new List<string>();
            string strTemp = "";
            string strOK = "";
            int nACT_SET_CNT = 0;
            List<string>[] strACT_SET_Code = new List<string>[101];
            List<string>[] strACT_SET_DB = new List<string>[101];
            List<string>[] strACT_SET_Data = new List<string>[101];
            List<string> strTemp1 = new List<string>();

            //string[] strACT_SET_Code = new string[101];
            //string[] strACT_SET_DB =   new string[101];
            //string[] strACT_SET_Data = new string[101];

            for (int i = 0; i <= 100; i++)
            {
                strACT_SET_Code[i] = new List<string>();
                strACT_SET_DB[i] = new List<string>();
                strACT_SET_Data[i] = new List<string>();
            }

            //----------------------------------------------
            //  자동액팅 설정값을 읽어 변수에 저장함
            //----------------------------------------------
            List<HIC_BCODE> list = hicBcodeService.GetItembyGubun("HEA_종검자동액팅");

            nREAD = list.Count;
            nACT_SET_CNT = 0;
            for (int i = 0; i < nREAD; i++)
            {
                strTemp = list[i].NAME;
                if (VB.Pstr(strTemp, "{}", 1) != "" && VB.Pstr(strTemp, "{}", 1) != "")                
                {   
                    strACT_SET_Code[nACT_SET_CNT].Add(VB.Pstr(strTemp, "{}", 1));
                    strACT_SET_DB[nACT_SET_CNT].Add(VB.Pstr(strTemp, "{}", 2));
                    strData = VB.Pstr(strTemp, "{}", 3);
                    nCNT = VB.L(strData, ";");
                    //strTemp = "";
                    strTemp1.Clear();


                    for (int j = 0; j < nCNT; j++)
                    {
                        if (!VB.Pstr(strData, ";", j + 1).IsNullOrEmpty())
                        {
                            strACT_SET_Data[nACT_SET_CNT].Add(VB.Pstr(strData, ";", j + 1).Trim());
                        }
                    }

                    nACT_SET_CNT += 1;
                }
            }

            //----------------------------------------------
            //  액팅 항목중 미액팅만 검색함
            //----------------------------------------------
            List<HEA_JEPSU_RESULT> list2 = heaJepsuResultService.GetItembyPart("5");

            nREAD = list2.Count;
            for (int i = 0; i < nREAD; i++)
            {
                nWRTNO = list2[i].WRTNO;
                strPtNo = list2[i].PTNO;
                strExCode = list2[i].EXCODE;

                //뇌혈류초음파(TCD)
                if (strExCode == "A895")
                {
                    if (comHpcLibBService.GetEtcJupMstbyPtNoOrderCode(strPtNo, "USTCD") > 0)
                    {
                        fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                    }
                }

                //폐활량검사(ACT)
                if (strExCode == "A899")
                {
                    if (comHpcLibBService.GetEtcJupMstbyPtNoOrderCodeNotFilePath(strPtNo, "F6002") >= 1)
                    {
                        fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                    }
                }

                //폐활량3회검사(ACT)
                if (strExCode == "A920")
                {
                    if (comHpcLibBService.GetEtcJupMstbyPtNoOrderCodeNotFilePath(strPtNo, "F6002") >= 3)
                    {
                        fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                    }
                }

                //일반건진과 종검 동시에 한 경우
                if (strExCode == "A136")

                {
                    if (hicXrayResultService.GetCountbyPtNoXCode(strPtNo, "A142") > 0)
                    {
                        fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                    }
                }

                strOK = "";
                for (int j = 0; j < nACT_SET_CNT; j++)
                {
                    if (strACT_SET_Code[j].To<string>() == strExCode)
                    {
                        strOK = "OK";
                        if (strACT_SET_DB[j].Equals("1"))
                        {
                            if (comHpcLibBService.GetExam_SpecMstCountbyPtNoMasterCode(strPtNo, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j].Equals("2"))
                        {
                            if (comHpcLibBService.GetXray_DetailbyPtNoXCode(strPtNo, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j].Equals("3"))
                        {
                            if (comHpcLibBService.GetEndo_JupMstCountbyPtNoGbJob(strPtNo, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j].Equals("4"))
                        {
                            if (comHpcLibBService.GetEtc_JupMstbyPtNoOrderCode(strPtNo, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j].Equals("5"))
                        {
                            if (heaResultService.GetCountbyWrtNoExCode(nWRTNO, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j].Equals("6"))
                        {
                            if (comHpcLibBService.GetEtc_JupMstbyPtNoOrderCodeStartDate(strPtNo, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                        else if (strACT_SET_DB[j].Equals("7"))
                        {
                            if (heaSangdamWaitService.GetCountbyWrtNoGubun(nWRTNO, strACT_SET_Data[j]) > 0)
                            {
                                fn_HEA_Auto_Acting(nWRTNO, strExCode, "01");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 예약 대장내시경,산부인과초음파,TCD 외래 자동접수
        /// </summary>
        void fn_YEYAK_OPD_MASTER_INSERT()
        {
            int nREAD = 0;
            string strList = "";
            string strPANO = "";
            string StrRDate = "";
            string strSName = "";
            string strSDate = "";
            string strPtNo = "";
            string strSex = "";
            long nAge = 0;
            string[] strGbExam = { "02", "08", "13" };  //대장내시경,골반초음파,TCD
            int result = 0;

            List<HEA_RESV_EXAM> list = heaResvExamService.GetItembyRTimeGbExam(strGbExam);

            nREAD = list.Count;
            strList = ",";
            for (int i = 0; i < nREAD; i++)
            {
                strPANO = list[i].PANO.To<string>();
                strSDate = list[i].SDATE;
                StrRDate = list[i].RDATE.To<string>();

                if (VB.InStr(strList, "," + strPANO + ",") == 0)
                {
                    //외래 환자등록번호를 찾음
                    List<HEA_JEPSU> list2 = heaJepsuService.GetItembyPaNoSDate(strPANO.To<long>(), strSDate);

                    strSName = list2[0].SNAME;
                    strPtNo = list2[0].PTNO;
                    strSex = list2[0].SEX;
                    nAge = list2[0].AGE;

                    if (strPtNo != "")
                    {
                        if (comHpcLibBService.GetItemOpdMasterbyPaNo(strPtNo) == 0)
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            COMHPC item = new COMHPC();

                            item.PTNO = strPtNo;
                            item.DEPTCODE = "TO";
                            item.BI = "51";
                            item.SNAME = strSName;
                            item.SEX = strSex;
                            item.AGE = nAge;
                            item.JICODE = "";
                            item.DRCODE = "7102";
                            item.RESERVED = "0";
                            item.CHOJAE = "1";
                            item.GBGAMEK = "00";
                            item.GBSPC = "0";
                            item.JIN = "D";
                            item.SINGU = "0";
                            item.PART = "222";
                            item.BDATE = strSDate;
                            item.EMR = "0";
                            item.GBUSE = "Y";
                            item.MKSJIN = "D";

                            result = comHpcLibBService.InsertOpdMaster(item);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// HEA_Result에 자동엑팅 업데이트
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argExCode"></param>
        /// <param name="argResult"></param>
        void fn_HEA_Auto_Acting(long argWrtNo, string argExCode, string argResult)
        {
            int result = 0;

            result = heaResultService.UpdateResultbyWrtNoExCode(argResult, "X05", clsType.User.IdNumber, argWrtNo, argExCode);

            if (result < 0)
            {
                return;
            }
        }

        public void themTabForm(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
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
