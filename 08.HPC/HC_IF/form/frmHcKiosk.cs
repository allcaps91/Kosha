using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC_IF.Dto;
using HC_Main;
using HC_Main.Model;
using HC_Main.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace HC_IF
{
    public partial class frmHcKiosk :Form
    {
        List<HIC_KIOSK> YEYAK = new List<HIC_KIOSK>();
        frmHaKiosk fHaKsk = null;

        int FnTimer2 = 0;
        int FnTimer3 = 0;
        bool FbMsgFlag = false;
        string FstrMsg = "";
        string FstrRetValue = "";
        int FnCnt = 0;
        string FstrJobGbn = "";
        string FstrHuilCheck = "";
        
        DevComponents.DotNetBar.ButtonX[] btnNames = new DevComponents.DotNetBar.ButtonX[50];

        int nYeyakTCnt = 0;
        int nYeyakTPage = 0;
        int nYeyakPage = 0;
        long nSEQ_Yeyak = 0;
        long nSEQ_Ilban = 0;
        string FstrSName = "", FstrJumin = "", FstrJuso = "", FstrTel = "", FstrHPhone = "", FstrLtdCode = "", FstrLtdName = "", FstrMunjin = "";
        bool FbClick = false;

        ComFunc CF = null;
        clsHaBase cHB = null;
        clsHcFunc cHF = null;
        clsHcMain cHcMain = null;
        clsHcMainFunc cHMF = null;

        HicCodeService hicCodeService = null;
        BasPatientService basPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        HicSunapService hicSunapService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicWaitService hicWaitService = null;
        HicBcodeService hicBcodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicCancerResv2Service hicCancerResv2Service = null;
        HicCancerResv1Service hicCancerResv1Service = null;
        HicPatientService hicPatientService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HicSunapWorkService hicSunapWorkService = null;
        HicSunapdtlWorkService hicSunapdtlWorkService = null;
        HeaJepsuService heaJepsuService = null;
        WorkNhicService workNhicService = null;
        GroupCodeExamDisplayService groupCodeExamDisplayService = null;
        HicGroupexamGroupcodeExcodeService hicGroupexamGroupcodeExcodeService = null;

        string strInChulPath = @"c:\cmc\exe\inchul_t.exe";

        public frmHcKiosk()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cHB = new clsHaBase();
            cHF = new clsHcFunc();
            cHcMain = new clsHcMain();
            cHMF = new clsHcMainFunc();

            hicCodeService = new HicCodeService();
            basPatientService = new BasPatientService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            hicSunapService = new HicSunapService();
            hicSunapdtlService = new HicSunapdtlService();
            hicWaitService = new HicWaitService();
            hicBcodeService = new HicBcodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicCancerResv2Service = new HicCancerResv2Service();
            hicCancerResv1Service = new HicCancerResv1Service();
            hicPatientService = new HicPatientService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicSunapWorkService = new HicSunapWorkService();
            hicSunapdtlWorkService = new HicSunapdtlWorkService();
            heaJepsuService = new HeaJepsuService();
            workNhicService = new WorkNhicService();
            groupCodeExamDisplayService = new GroupCodeExamDisplayService();
            hicGroupexamGroupcodeExcodeService = new HicGroupexamGroupcodeExcodeService();
        }

        private void SetEvents()
        {
            this.Load           += new EventHandler(eFormLoad);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click  += new EventHandler(eBtnClick);
            this.btnYes.Click   += new EventHandler(eBtnClick);
            this.btnConfirm.Click += new EventHandler(eBtnClick);
            this.Num0.Click     += new EventHandler(eNumBtnClick);
            this.Num1.Click     += new EventHandler(eNumBtnClick);
            this.Num2.Click     += new EventHandler(eNumBtnClick);
            this.Num3.Click     += new EventHandler(eNumBtnClick);
            this.Num4.Click     += new EventHandler(eNumBtnClick);
            this.Num5.Click     += new EventHandler(eNumBtnClick);
            this.Num6.Click     += new EventHandler(eNumBtnClick);
            this.Num7.Click     += new EventHandler(eNumBtnClick);
            this.Num8.Click     += new EventHandler(eNumBtnClick);
            this.Num9.Click     += new EventHandler(eNumBtnClick);

            this.txtJumin.TextChanged += new EventHandler(eTxtChanged);

            this.btnNameUp.Click     += new EventHandler(eBtnClick);
            this.btnNameDown.Click   += new EventHandler(eBtnClick);

            this.timer1.Tick         += new EventHandler(eTimer_Timer);
            this.timer2.Tick         += new EventHandler(eTimer_Timer);
            this.timer3.Tick         += new EventHandler(eTimer_Timer);
            this.timer4.Tick         += new EventHandler(eTimer_Timer);
            this.timerResv.Tick      += new EventHandler(eTimer_Timer);
            this.timerShutdown.Tick  += new EventHandler(eTimer_Timer);

            this.picLogo.Click += new EventHandler(ePicDblClick);
            //this.picLogo.DoubleClick += new EventHandler(ePicDblClick);
            this.panSubMsg.Click     += new EventHandler(ePanClick);
            this.lblGubun.DoubleClick += new EventHandler(eLblDblClick);
        }

        private void eTxtChanged(object sender, EventArgs e)
        {
            if (txtJumin.TextLength >= 13)
            {
                Data_Save();
            }
        }

        private void eLblDblClick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            timerResv.Stop();
            timerShutdown.Stop();

            //FrmViewMain.Show

            this.Close();
            return;   
        }

        private void eBtnNameClick(object sender, EventArgs e)
        {
            int Index = 0;
            int inx = 0;

            try
            {
                Index = VB.Pstr(((DevComponents.DotNetBar.ButtonX)sender).Name, "Name", 2).To<int>();
                if (FbClick) { return; }

                timer1.Stop();
                txtJumin.Text = "";

                inx = ((nYeyakPage - 1) * 50) + Index;

                frmHcKiosk_Verify.rSetGstrValue += new frmHcKiosk_Verify.SetGstrValue(eVerify_Message_NameClick);
                frmHcKiosk_Verify fHV = new frmHcKiosk_Verify("예약", YEYAK[inx].RID);
                fHV.StartPosition = FormStartPosition.Manual;
                fHV.Location = new System.Drawing.Point(140, 390);
                fHV.ShowDialog();
                frmHcKiosk_Verify.rSetGstrValue -= new frmHcKiosk_Verify.SetGstrValue(eVerify_Message_NameClick);

                timer1.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show("eBtnNameClick Error " + ex.Message);
            }
            
        }

        private void eVerify_Message_NameClick(string strJumin)
        {
            try
            {
                if (!strJumin.IsNullOrEmpty())
                {
                    FbClick = true;
                    txtJumin.Text = strJumin;
                    Data_Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("eVerify_Message_NameClick Error " + ex.Message);
            }
        }

        /// <summary>
        /// 출근등록 프로그램 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ePicDblClick(object sender, EventArgs e)
        {
            if (sender == picLogo)
            {
                Process Proc = new Process(); //외부프로그램 실행

                Process[] procs = Process.GetProcessesByName("Inchul_t");

                if (procs.Length > 0)
                {
                    // 중복실행시 처리
                    procs[0].Kill();
                }

                if (!File.Exists(strInChulPath))
                {
                    ComFunc.MsgBox(@"c:\cmc\exe 폴더에 inchul_t.exe 파일이 없습니다.", "파일누락");
                    return;
                }
                else
                {
                    timer1.Stop();
                    Proc.StartInfo.FileName = strInChulPath;
                    Proc.Start();
                }
            }
        }

        private void ePanClick(object sender, EventArgs e)
        {
            Job_TimerResv();
        }

        private void eTimer_Timer(object sender, EventArgs e)
        {
            try
            {
                if (sender == timer1)
                {
                    this.ActiveControl = txtJumin;
                    txtJumin.SelectionStart = txtJumin.TextLength;
                    //txtJumin.Focus();
                }
                else if (sender == timer2)
                {
                    FnTimer2 = FnTimer2 + 1;
                    if (FnTimer2 >= 10)
                    {
                        panMsg.Visible = false;
                        timer2.Stop();
                        timer1.Start();
                        FnTimer2 = 0;
                    }
                }
                else if (sender == timer3)
                {
                    FnTimer3 = FnTimer3 + 1;
                    if (FnTimer3 >= 120)
                    { 
                        if (txtJumin.Text.Trim() == "")
                        {
                            timer3.Stop();
                            FnTimer3 = 0;
                            Display_Yeyak();
                            timer3.Start();
                        }
                    }
                }
                else if (sender == timer4)
                {
                    //---------------------------------------
                    //  12:30 자동으로 종검키오스크로 전환
                    //---------------------------------------
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    clsPublic.GstrIpAddress = clsVbfunc.GetIpAddressOeacle(clsDB.DbCon);

                    if (clsPublic.GstrIpAddress == "192.168.41.39")
                    {
                        if (string.Compare(clsPublic.GstrSysTime, "12:25") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:35") <= 0)
                        {
                            foreach (Form Form in Application.OpenForms) //떠있는지 체크
                            {
                                if (Form.Name == "frmHcKiosk") { Form.Close();  break; }
                            }

                            foreach (Form Form in Application.OpenForms) //떠있는지 체크
                            {
                                if (Form.Name == "frmHaKiosk") { Form.Close(); break; }
                            }

                            timer4.Enabled = false;
                            fHaKsk = new frmHaKiosk();
                            fHaKsk.StartPosition = FormStartPosition.CenterScreen;
                            fHaKsk.ShowDialog();
                            cHF.fn_ClearMemory(fHaKsk);
                        }
                    }
                }
                else if (sender == timerResv)
                {
                    Job_TimerResv();
                }
                else if (sender == timerShutdown)
                {
                    TimerShutdown_Timer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("eTimer_Timer Error " + ComNum.VBLF + ex.Message);
            }
        }

        /// <summary>
        /// TimerShutdown_Timer()
        /// </summary>
        private void TimerShutdown_Timer()
        {
            string strDate = string.Empty;

            try
            {
                strDate = DateTime.Now.ToShortDateString();

                //---------------------------------------
                //  12:30 자동으로 종검키오스크로 전환
                //---------------------------------------
                ComFunc.ReadSysDate(clsDB.DbCon);

                #region 키오스크 Main 프로그램에서 실행하도록 변경
                clsPublic.GstrIpAddress = clsVbfunc.GetIpAddressOeacle(clsDB.DbCon);

                //if (clsPublic.GstrIpAddress == "192.168.41.39")
                //{
                //    if (string.Compare(clsPublic.GstrSysTime, "12:25") >= 0 && string.Compare(clsPublic.GstrSysTime, "12:35") <= 0)
                //    {
                //        if (File.Exists(strHaKioskPath))
                //        {
                //            #region  기존 프로그램 처리 코드
                //            //Process[] procs = Process.GetProcessesByName("hakiosk.exe");
                //            //if (procs.Length > 0) { procs[0].Kill(); }
                //            //Process Proc = new Process();       //외부프로그램 실행
                //            //Proc.StartInfo.FileName = strHaKioskPath;
                //            //Proc.Start();
                //            #endregion

                //            //TODO : 종검 키오스크 실행
                //            this.Close();
                //        }

                //    }
                //}
                #endregion

                //------------------------------------
                //  PC 자동종료 설정값을 읽음
                //------------------------------------
                string strYoil = CF.READ_YOIL(clsDB.DbCon, strDate);
                string strStopTIme = hicBcodeService.GetCodeNamebyGubunCode("BAS_표시장비자동종료시간", clsPublic.GstrIpAddress);

                if (strYoil == "토요일")
                {    
                    strStopTIme = VB.Pstr(strStopTIme, "{}", 2);
                }
                else
                {
                    strStopTIme = VB.Pstr(strStopTIme, "{}", 1);
                }

                //종료시간 설정값이 없으면
                if (strStopTIme.IsNullOrEmpty()) { return; }

                //설정한 종료시간이 되면 PC를 종료함
                if (string.Compare(clsPublic.GstrSysTime, strStopTIme) >= 0)
                {
                    timerShutdown.Stop();
                    Process.Start("shutdown.exe"); 
                }

                //------------------------------------
                //  공휴일은 자동종료함
                //------------------------------------
                if (FstrHuilCheck != "Y")
                {
                    FstrHuilCheck = "Y";
                    if (cHB.HIC_DATE_HUIL_Check(strDate))
                    {
                        timerShutdown.Stop();
                        Process.Start("shutdown.exe");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TimerShutdown Error " + ComNum.VBLF + ex.Message);
            }
        }

        private void Job_TimerResv()
        {
            int i = 0, j = 0, nREAD = 0;
            string strJumin = string.Empty, strDate = string.Empty, strTime = string.Empty;
            long[,] nCNT = new long[4, 7];

            timerResv.Stop();

            //변수를 Clear
            //1)정원 2)당일예약 3)여유1 4)도착인원 5)여유2 6)미도착
            for (i = 1; i < 4; i++)
            {
                for (j = 1; j < 7; j++) { nCNT[i, j] = 0; }
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            strDate = DateTime.Now.ToShortDateString();
            //strDate = DateTime.Now.AddDays(-4).ToShortDateString();
            strTime = clsPublic.GstrSysTime;

            //정원을 읽음
            HIC_CANCER_RESV1 iHCR1 = hicCancerResv1Service.GetItembyJobDate(strDate);

            if (!iHCR1.IsNullOrEmpty())
            {
                if (DateTime.Now.Hour < 12)
                {
                    nCNT[1, 1] = iHCR1.GFS;
                    nCNT[2, 1] = iHCR1.GFSH;
                    nCNT[3, 1] = iHCR1.UGI;
                }
                else
                {
                    nCNT[1, 1] = iHCR1.GFS1;
                    nCNT[2, 1] = iHCR1.GFSH1;
                    nCNT[3, 1] = iHCR1.UGI1;
                }
            }

            string strAMPM = "AM";
            //string strFDate = DateTime.Now.AddDays(-4).ToShortDateString();
            //string strTDate = DateTime.Now.AddDays(-4).ToShortDateString();
            string strFDate = DateTime.Now.ToShortDateString();
            string strTDate = DateTime.Now.ToShortDateString();

            if (DateTime.Now.Hour < 12)
            {
                strAMPM = "AM";
                strTDate = strTDate + " 12:00";
            }
            else
            {
                strAMPM = "PM";
                strFDate = strFDate + " 12:01";
                strTDate = strTDate + " 23:59";
            }

            //암검진 예약현황 및 도착현황을 읽음
            List<HIC_CANCER_RESV2> lstHCR2 = hicCancerResv2Service.GetItembyRTime3(strAMPM, strFDate, strTDate);

            if (lstHCR2.Count > 0)
            {
                for (i = 0; i < lstHCR2.Count; i++)
                {
                    //2)당일예약 인원수
                    if (lstHCR2[i].GBGFS == "Y") {  nCNT[1, 2] += 1; }
                    if (lstHCR2[i].GBGFSH == "Y") { nCNT[2, 2] += 1; }
                    if (lstHCR2[i].GBUGI == "Y") {  nCNT[3, 2] += 1; }


                    strJumin = lstHCR2[i].JUMIN2.To<string>("").Trim();

                    //대기순번표 발행여부
                    string strHWRID = hicWaitService.GetRowidByJobDateJumin2(strDate, strJumin);

                    if (!strHWRID.IsNullOrEmpty())
                    {
                        //4)도착 인원수
                        if (lstHCR2[i].GBGFS == "Y") {  nCNT[1, 4] += 1; }
                        if (lstHCR2[i].GBGFSH == "Y") { nCNT[2, 4] += 1; }
                        if (lstHCR2[i].GBUGI == "Y") {  nCNT[3, 4] += 1; }
                    }
                    else
                    {
                        //5)미도착 인원수
                        if (lstHCR2[i].GBGFS == "Y") {  nCNT[1, 6] += 1; }
                        if (lstHCR2[i].GBGFSH == "Y") { nCNT[2, 6] += 1; }
                        if (lstHCR2[i].GBUGI == "Y")  { nCNT[3, 6] += 1; }
                    }
                }
            }

            for (i = 1; i < 4; i++)
            {
                nCNT[i, 3] = nCNT[i, 1] - nCNT[i, 2]; //여유1
                nCNT[i, 5] = nCNT[i, 1] - nCNT[i, 4]; //여유2
                nCNT[i, 6] = nCNT[i, 2] - nCNT[i, 4]; //미도착

                ss1.ActiveSheet.Cells[i - 1, 1].Text = nCNT[i, 1].ToString("#0");   //정원
                ss1.ActiveSheet.Cells[i - 1, 2].Text = nCNT[i, 3].ToString("#0");   //예약인원
                ss1.ActiveSheet.Cells[i - 1, 3].Text = nCNT[i, 4].ToString("#0");   //도착인원
                ss1.ActiveSheet.Cells[i - 1, 4].Text = nCNT[i, 5].ToString("#0");   //여유
            }

            timerResv.Start();

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save();
                Application.DoEvents();
                txtJumin.Focus();
            }
            else if (sender == btnCancel)
            {
                txtJumin.Text = "";
                Application.DoEvents();
                txtJumin.Focus();
            }
            else if (sender == btnNameUp)
            {
                Display_Yeyak_Buttons(btnNameUp);
            }
            else if (sender == btnNameDown)
            {
                Display_Yeyak_Buttons(btnNameDown);
            }
            else if (sender == btnYes)
            {
                panHeaInfo.Visible = false;
            }
            else if (sender == btnConfirm)
            {
                frmHcKiosk_ResultView frm = new frmHcKiosk_ResultView();
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new System.Drawing.Point(280, 390);
                frm.ShowDialog();
            }
        }

        private void Display_Yeyak_Buttons(Button argBtn)
        {
            int inx = 0, nStart = 0, nEnd = 0;

            if (argBtn.Name == "btnNameDown")
            {
                nYeyakPage = nYeyakPage + 1;
            }
            else if (argBtn.Name == "btnNameUp")
            {
                nYeyakPage = nYeyakPage - 1;
            }
            
            nStart = ((nYeyakPage - 1) * 50) + 1;
            nEnd = nStart + 49;
            if (nEnd > nYeyakTCnt) { nEnd = nYeyakTCnt; }

            if (argBtn.Name == "btnNameDown")
            {
                btnNameUp.Visible = true;
            }
            else if (argBtn.Name == "btnNameUp")
            {
                if (nYeyakPage >= 2)
                {
                    btnNameUp.Visible = true;
                }
                else
                {
                    btnNameUp.Visible = false;
                }
            }

            if (nEnd < nYeyakTCnt)
            {
                btnNameDown.Visible = true;
            }
            else
            {
                btnNameDown.Visible = false;
            }

            inx = 0;
            for (int i = nStart - 1; i < nEnd; i++)
            {
                DevComponents.DotNetBar.ButtonX ctlSName = (Controls.Find("SName" + inx.ToString(), true)[0] as DevComponents.DotNetBar.ButtonX);
                //ctlSName.Visible = true;
                ctlSName.Enabled = true;
                ctlSName.Text = YEYAK[i].SNAME + "<br></br><font size ='18'>" + YEYAK[i].AGE + "</font>";
                inx += 1;
            }

            //빈칸은 화면에 보이지 않게 함
            for (int i = inx; i < 50; i++)
            {
                DevComponents.DotNetBar.ButtonX ctlSName = (Controls.Find("SName" + i.ToString(), true)[0] as DevComponents.DotNetBar.ButtonX);
                ctlSName.Text = "";
                ctlSName.Enabled = false;
                //ctlSName.Visible = false;
            }
        }

        private void eNumBtnClick(object sender, EventArgs e)
        {
            if (txtJumin.TextLength < 13)
            {
                txtJumin.Text += ((DevComponents.DotNetBar.ButtonX)sender).Text;
            }
            //else
            //{
            //    Data_Save();
            //}
        }

        private void Data_Save()
        {
            int nLen = 0;
            long nSeqNo = 0;
            long nPano = 0;
            int nAge = 0;

            string strJumin = string.Empty;    string strJepsu   = string.Empty;
            string strCurDate = string.Empty;  string strAutoJep = string.Empty;
            string strRePrint = string.Empty;  string strGjJong  = string.Empty;
            string strPtNo = string.Empty;     
            string strGbYeyak = string.Empty;
            string strSuDate = string.Empty;
            string strJong = string.Empty;
            string strChasu = string.Empty;
            string strUcodes = string.Empty;
            string strROWID = string.Empty;
            string strGBGFS = string.Empty;
            string strGBCT = string.Empty;
            string strAm = string.Empty;

            List<string> strTemp = new List<string>();
            FstrMunjin = "";

            try
            {
                //strCurDate = DateTime.Now.AddDays(-4).ToShortDateString();
                strCurDate = DateTime.Now.ToShortDateString();

                btnSave.Enabled = false;

                if (txtJumin.Text.Trim() == "")
                {
                    btnSave.Enabled = true;
                    FbClick = false;
                    return;
                }

                //병원 등록번호를 입력한 경우 처리
                if (txtJumin.Text.Trim().Length <= 8)
                {
                    string strPano = VB.Format(VB.Val(txtJumin.Text), "00000000");
                    timer1.Stop();
                    txtJumin.Text = "";

                    frmHcKiosk_Verify.rSetGstrValue += new frmHcKiosk_Verify.SetGstrValue(eVerify_Message_Save);
                    frmHcKiosk_Verify fHV = new frmHcKiosk_Verify("등록번호", strPano);
                    fHV.StartPosition = FormStartPosition.Manual;
                    fHV.Location = new System.Drawing.Point(140, 390);
                    fHV.ShowDialog();
                    frmHcKiosk_Verify.rSetGstrValue -= new frmHcKiosk_Verify.SetGstrValue(eVerify_Message_Save);
                    return;
                }

                Button_Enabled(false);

                ComFunc.ReadSysDate(clsDB.DbCon);

                strJumin = txtJumin.Text.Trim();
                nLen = strJumin.Length;

                if (nLen != 13)
                {
                    FstrMsg = "주민등록번호 13자리를 정확하게 입력하세요";
                    txtJumin.Text = "";
                    Msg_Screen_Display();
                    Button_Enabled(true);
                    FbClick = false;
                    btnSave.Enabled = true;
                    return;
                }

                //주민번호 오류 점검
                if (cHF.JuminNo_Check_New(VB.Left(strJumin, 6), VB.Right(strJumin, 7)).Equals("ERROR"))
                {
                    FstrMsg = "주민등록번호 13자리를 정확하게 입력하세요";
                    txtJumin.Text = "";
                    Msg_Screen_Display();
                    Button_Enabled(true);
                    FbClick = false;
                    btnSave.Enabled = true;
                    return;
                }

                //숫자만 입력하였는지 점검
                for (int i = 1; i <= nLen; i++)
                {
                    switch (VB.Mid(strJumin, i, 1))
                    {
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":   break;
                        default:
                            FstrMsg = "주민등록번호를 숫자로 입력하세요";
                            txtJumin.Text = "";
                            Msg_Screen_Display();
                            Button_Enabled(true);
                            FbClick = false;
                            btnSave.Enabled = true;
                            return;
                    }
                }

                //종검 예약자는 접수 불가
                if (!heaJepsuService.GetRowidBySDateKioskPano(strCurDate, clsAES.AES(strJumin)).IsNullOrEmpty())
                {
                    timer1.Stop();
                    panHeaInfo.Visible = true;
                    txtJumin.Text = "";
                    Button_Enabled(true);
                    timer1.Start();
                    btnSave.Enabled = true;
                    FbClick = false;
                    return;
                }

                //오늘 이미 접수를 하였는지 점검
                nSeqNo = hicWaitService.GetSeqNoByJobDateJumin(strCurDate, clsAES.AES(strJumin));
                if (nSeqNo > 0)
                {
                    strRePrint = "OK";
                }

                panWait.Visible = true;
                Application.DoEvents();

                //나이를 계산함
                nAge = cHB.READ_HIC_AGE_GESAN2(strJumin).To<int>(0);

                //건진 환자마스타에 등록된 수검자인지 점검
                nPano = 0;
                FstrJumin = strJumin;
                strPtNo = "";

                HIC_PATIENT iHP = hicPatientService.GetItembyJumin2NotInSName(clsAES.AES(strJumin), clsHcVariable.B04_NOT_PATIENT);

                if (!iHP.IsNullOrEmpty())
                {
                    strTemp.Clear();
                    strTemp.Add(iHP.SNAME);             //성명
                    strTemp.Add(iHP.JUSO1);             //주소1
                    strTemp.Add(iHP.JUSO2);             //주소2
                    strTemp.Add(iHP.TEL);               //전화번호
                    strTemp.Add(iHP.HPHONE);            //휴대폰번호
                    strTemp.Add(iHP.LTDNAME.To<string>(" "));           //근무처
                    strTemp.Add(strJumin);              //주민번호

                    nPano = iHP.PANO;           //건진번호
                    strPtNo = iHP.PTNO;         //등록번호

                    FstrSName = iHP.SNAME;
                    FstrJuso = iHP.JUSO1 + " " + iHP.JUSO2;
                    FstrTel = iHP.TEL;
                    FstrHPhone = iHP.HPHONE;
                    FstrLtdCode = iHP.LTDCODE.To<string>("").Trim();
                    //개인정보동의서 1회만 받음
                    //if (iHP.GBPRIVACY.To<string>("").Trim() == "")
                    if (iHP.GBPRIVACY_NEW.To<string>("").Trim() == "")
                    {
                        FstrMunjin += "개인정보(X),";
                    }
                    else
                    {
                        FstrMunjin += "개인정보(O),";
                    }
                }
                else
                {
                    strTemp.Clear();
                    FstrMunjin += "개인정보(X),";
                }

                //병원 환자마스타에 등록이 되었는지 점검
                if (strTemp.Count == 0)
                {
                    BAS_PATIENT iBPT = basPatientService.GetItembyJumin1Jumin3NotInSName(VB.Left(strJumin, 6), clsAES.AES(VB.Right(strJumin, 7)), clsHcVariable.B04_NOT_PATIENT);

                    if (!iBPT.IsNullOrEmpty())
                    {
                        strTemp.Add(iBPT.SNAME);            //성명
                        strTemp.Add(iBPT.JUSO);             //주소1
                        strTemp.Add(iBPT.ROADDETAIL);       //주소2
                        strTemp.Add(iBPT.TEL);              //전화번호
                        strTemp.Add(iBPT.HPHONE);           //휴대폰번호
                        strTemp.Add(" ");                   //근무처
                        strTemp.Add(strJumin);              //주민번호

                        strPtNo = iBPT.PANO;
                        FstrSName = iBPT.SNAME;
                        FstrJuso = "";
                        FstrTel = iBPT.TEL;
                        FstrHPhone = iBPT.HPHONE;
                        FstrLtdCode = "";
                    }
                    else
                    {
                        strTemp.Clear();
                    }
                }

                if (strTemp.Count == 0)
                {
                    strTemp.Add(" ");
                    strTemp.Add(" ");
                    strTemp.Add(" ");
                    strTemp.Add(" ");
                    strTemp.Add(" ");
                    strTemp.Add(" ");
                    strTemp.Add(strJumin);
                }

                //인터넷문진 여부를 읽음
                if (strPtNo != "")
                {
                    if (!hicIeMunjinNewService.GetItembyPtNoMunDate(strPtNo, VB.Left(strCurDate, 4) + "-01-01").IsNullOrEmpty())
                    {
                        FstrMunjin += "I문진O,";
                    }
                    else
                    {
                        FstrMunjin += "I문진X,";
                    }
                }
                else
                {
                    //신환은 인터넷문진표 여부 확인이 불가능함
                    FstrMunjin += "I문진?,";
                }

                //2차재검 가접수 대상자인지 점검 -> 바로 접수함
                strGbYeyak = "";
                string strYear = "";
                if (nPano > 0)
                {
                    if (string.Compare(VB.Right(strCurDate, 5), "02-01") >= 0)
                    {
                        strYear = DateTime.Now.Year.To<string>("");
                    }
                    else
                    {
                        strYear = (DateTime.Now.Year - 1).To<string>("");
                    }
                    
                    HIC_JEPSU_WORK iHJW = hicJepsuWorkService.GetSecondJongItemByPanoGjYear(nPano, strYear, "1");

                    if (!iHJW.IsNullOrEmpty())
                    {
                        strSuDate = iHJW.JEPDATE;
                        strJong   = iHJW.GJJONG;
                        strChasu  = iHJW.GJCHASU;
                        strUcodes = iHJW.UCODES;
                        strROWID  = iHJW.RID;
                        //재발행은 2차재검 접수를 안함
                        if (strRePrint == "")
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            if (!INSERT_JEPSU_Rtn(iHJW))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                panWait.Visible = false;
                                Button_Enabled(true);
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                        strJepsu   = "OK";
                        strGbYeyak = "2"; //2차재검
                        strAutoJep = "Y";
                        strGjJong  = strJong;
                        //2차 청력재검 문진표
                        if (iHJW.SEXAMS.Contains("J231")) { FstrMunjin += "청력2차,"; }
                        if (iHJW.SEXAMS.Contains("9231")) { FstrMunjin += "청력2차,"; }
                    }
                }

                ////2020-07-02(16/28종 2차 재검표시)
                //if (nPano > 0)
                //{
                //    if (string.Compare(VB.Right(strCurDate, 5), "02-01") >= 0)
                //    {
                //        strYear = DateTime.Now.Year.To<string>("");
                //    }
                //    else
                //    {
                //        strYear = (DateTime.Now.Year - 1).To<string>("");
                //    }

                //    HIC_JEPSU iHJ = hicJepsuService.GetSecondJongItemByPanoGjYear(nPano, strYear);

                //    if (!iHJ.IsNullOrEmpty())
                //    {
                //        if (iHJ.GJJONG == "16")
                //        {
                //            FstrMunjin += "재검(16종),";
                //        }
                //        else if (iHJ.GJJONG == "28")
                //        {
                //            FstrMunjin += "재검(28종),";
                //        }
                //    }
                //}

                //가접수는 검진종류를 인쇄
                if (nPano > 0)
                {
                    if (string.Compare(VB.Right(strCurDate, 5), "02-01") >= 0)
                    {
                        strYear = DateTime.Now.Year.To<string>("");
                    }
                    else
                    {
                        strYear = (DateTime.Now.Year - 1).To<string>("");
                    }

                    //HIC_JEPSU_WORK iHJW = hicJepsuWorkService.GetSecondJongItemByPanoGjYear(nPano, strYear, "2");
                    List<HIC_JEPSU_WORK> iHJW = hicJepsuWorkService.GetListItemByPanoGjYear(nPano, strYear, "2");

                    if (!iHJW.IsNullOrEmpty())
                    {
                        for (int i = 0; i < iHJW.Count; i++)
                        {
                            strGbYeyak = "3";
                            strSuDate = iHJW[i].JEPDATE;
                            strJong = iHJW[i].GJJONG;
                            strChasu = iHJW[i].GJCHASU;
                            strUcodes = iHJW[i].UCODES;
                            FstrLtdCode = iHJW[i].LTDCODE.To<string>("");

                            if (string.Compare(strJong, "11") >= 0 && string.Compare(strJong, "14") <= 0)
                            {
                                if (strUcodes.IsNullOrEmpty())
                                {
                                    FstrMunjin += "1차,";
                                }
                                else
                                {
                                    FstrMunjin += "일특,";
                                }
                            }
                            else if (string.Compare(strJong, "41") >= 0 && string.Compare(strJong, "43") <= 0)
                            {
                                if (strUcodes == "")
                                {
                                    FstrMunjin += "생애1차,";
                                }
                                else
                                {
                                    FstrMunjin += "생특,";
                                }
                            }
                            else
                            {
                                switch (strJong)
                                {
                                    case "16": FstrMunjin += "재검(16종),"; break;
                                    case "21": FstrMunjin += "채용,"; break;
                                    case "22": FstrMunjin += "채배,"; break;
                                    case "23": FstrMunjin += "특수,"; break;
                                    case "24": FstrMunjin += "배치전,"; break;
                                    case "25": FstrMunjin += "수시,"; break;
                                    case "26": FstrMunjin += "임시,"; break;
                                    case "28": FstrMunjin += "재검(28종),"; break;
                                    case "30": FstrMunjin += "채배,"; break;
                                    case "49": FstrMunjin += "채+방,"; break;
                                    case "51": FstrMunjin += "방사선,"; break;
                                    case "56": FstrMunjin += "학생,"; break;
                                    case "69": FstrMunjin += "69종,"; break;
                                    default: FstrMunjin += "가접수,"; break;
                                }
                            }

                            if (!strUcodes.IsNullOrEmpty() && (strUcodes.Contains("V01") || strUcodes.Contains("V02")))
                            {
                                FstrMunjin += "야간,";
                            }
                        }
                    }
                }

                //string strFDate = DateTime.Now.AddDays(-4).ToShortDateString();
                //string strTDate = DateTime.Now.AddDays(-3).ToShortDateString();
                string strFDate = DateTime.Now.ToShortDateString();
                string strTDate = DateTime.Now.AddDays(1).ToShortDateString();

                strGBGFS = "";
                strGBCT = "";
                strAm = "";
                //암검진 예약자
                HIC_CANCER_RESV2 iHCR2 = hicCancerResv2Service.GetItembyJumin(clsAES.AES(strJumin), strFDate, strTDate);

                if (!iHCR2.IsNullOrEmpty())
                {
                    strGbYeyak = "1"; //암검진예약
                    //if (iHCR2.GBBOHUM == "Y")
                    //{
                    //    if (FstrMunjin.IsNullOrEmpty() && !FstrMunjin.Contains("1차")) { FstrMunjin += "1차,"; }
                    //}

                    if (iHCR2.GBBOHUM == "Y") { FstrMunjin += "1차,"; }
                    if (iHCR2.GBGFS == "Y")   { FstrMunjin += "GFS,"; }
                    if (iHCR2.GBGFSH == "Y")  { FstrMunjin += "GFSH,"; }
                    if (iHCR2.GBUGI == "Y")   { FstrMunjin += "UGI,"; }
                    if (iHCR2.GBCT == "Y")    { FstrMunjin += "폐암,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBAM1 == "Y")    { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBAM2 == "Y")    { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBAM3 == "Y")    { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBAM4 == "Y")    { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBUGI == "Y")    { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBGFS == "Y")    { FstrMunjin += "암검진,"; strGBGFS = "Y"; } 
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBMAMMO == "Y")  { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBRECUTM == "Y") { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBSONO == "Y")   { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBGFSH == "Y")   { FstrMunjin += "암검진,"; strGBGFS = "Y"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBWOMB == "Y")   { FstrMunjin += "암검진,"; }
                    //if (!FstrMunjin.Contains("암검진,") && iHCR2.GBCT == "Y")     { FstrMunjin += "암검진,"; strGBCT = "Y"; }
                    if (iHCR2.GBAM1 == "Y") { strAm = "OK"; }
                    if (iHCR2.GBAM2 == "Y") { strAm = "OK"; }
                    if (iHCR2.GBAM3 == "Y") { strAm = "OK"; }
                    if (iHCR2.GBAM4 == "Y") { strAm = "OK"; }
                    if (iHCR2.GBUGI == "Y") { strAm = "OK"; }
                    if (iHCR2.GBGFS == "Y") { strAm = "OK"; strGBGFS = "Y"; }
                    if (iHCR2.GBMAMMO == "Y") {  strAm = "OK"; }
                    if (iHCR2.GBRECUTM == "Y") { strAm = "OK"; }
                    if (iHCR2.GBSONO == "Y") { strAm = "OK"; }
                    if (iHCR2.GBGFSH == "Y") { strAm = "OK"; strGBGFS = "Y"; }
                    if (iHCR2.GBWOMB == "Y") { strAm = "OK"; }
                    if (iHCR2.GBCT == "Y") { strAm = "OK"; strGBCT = "Y"; }

                    if (strAm == "OK") { FstrMunjin += "암검진,"; }
                }

                //접수번호를 생성

                if (strRePrint == "")
                {
                    nSeqNo = hicWaitService.GetMaxSeqNoByGbYeyakAge(strCurDate, strGbYeyak, nAge, strGBGFS, strGBCT, strJong);

                    if (nSeqNo == 0)
                    {
                        //nSeqNo = 301;   //당일접수

                        //if (strGbYeyak == "1" && (strGBGFS == "Y"|| strGBCT == "Y"))
                        //{
                        //    nSeqNo = 101;
                        //}
                        //else if (strGbYeyak == "1" || strGbYeyak == "2" || strGbYeyak == "3")
                        //{
                        //    nSeqNo = 151;
                        //}

                        ////학생검진
                        //if (nAge <= 17) { nSeqNo = 501; }


                        //2021년 부터 대기순번표 재조정 (종합검진 1-100  /  위암+재검 101-999  /  당일접수 1001-1999  / 학생검진 2001-2999)
                        nSeqNo = 1001;   //당일접수

                        if (strGbYeyak == "1" && (strGBGFS == "Y" || strGBCT == "Y"))
                        {
                            nSeqNo = 101;
                        }
                        //재검(16종)
                        else if (strJong == "16" || strJong == "28")
                        {
                            nSeqNo = 201;
                        }
                        else if (strGbYeyak == "1" || strGbYeyak == "2" || strGbYeyak == "3")
                        {
                            nSeqNo = 301;
                        }

                        //학생검진
                        if (nAge <= 17) { nSeqNo = 2001; }

                    }
                    else
                    {
                        nSeqNo += 1;
                    }

                    HIC_WAIT nHW = new HIC_WAIT
                    {
                        JOBDATE = strCurDate,
                        SEQNO = nSeqNo,
                        JUMIN = VB.Left(strJumin, 7) + "******",
                        JUMIN2 = clsAES.AES(strJumin),
                        SNAME = strTemp[0].To<string>(""),
                        GBJOB = "1",
                        GBYEYAK = strGbYeyak,
                        GBAUTOJEP = strAutoJep,
                        GJJONG = strGjJong,
                        JEPTIME = clsPublic.GstrSysTime,
                        CALLTIME = "",
                        ENDTIME = "",
                        GBBUSE = "2"
                    };

                    clsDB.setBeginTran(clsDB.DbCon);

                    if (!hicWaitService.InsertData(nHW))
                    {
                        MessageBox.Show("HIC_WAIT 대기순번 등록시 오류 발생", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        timer1.Stop();
                        Button_Enabled(true);
                        timer1.Start();
                        btnSave.Enabled = true;
                        panWait.Visible = false;
                        FbClick = false;
                        Application.DoEvents();
                        txtJumin.Focus();
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    //2015-05-22 가접수 검진자격조회 대상자에 등록
                    if (FstrSName.Trim() != "")
                    {
                        if (workNhicService.GetRowidByJumin2Rtime(clsAES.AES(strJumin)).IsNullOrEmpty())
                        {
                            WORK_NHIC item2 = new WORK_NHIC
                            {
                                GUBUN = "H",
                                SNAME = FstrSName,
                                JUMIN = VB.Left(FstrJumin, 7) + "******",
                                JUMIN2 = clsAES.AES(strJumin),
                                PANO = strPtNo,
                                GBSTS = "0",
                                YEAR = DateTime.Now.Year.To<string>("")
                            };

                            clsDB.setBeginTran(clsDB.DbCon);

                            int result = workNhicService.InsertData(item2);
                            if (result <= 0)
                            {
                                MessageBox.Show("WORK_NHIC 자격조회 대상 등록시 오류 발생", "오류");
                                clsDB.setRollbackTran(clsDB.DbCon);
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }

                //접수증을 인쇄함
                strTemp.Add(FstrMunjin);    //문진표 종류
                Jepsu_Print(nSeqNo, strTemp, nAge, strPtNo);

                txtJumin.Text = "";
                FstrMsg = "대기순번표를 가져가세요.";
                Msg_Screen_Display();
                Display_Yeyak();
                Job_TimerResv();
                Button_Enabled(true);

                btnSave.Enabled = true;

                FbClick = false;
                panWait.Visible = false;
                Application.DoEvents();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data_Save Error " + ex.Message);
                timer1.Stop();
                Button_Enabled(true);
                timer1.Start();
                btnSave.Enabled = true;
                panWait.Visible = false;
                FbClick = false;
                Application.DoEvents();
                txtJumin.Focus();
                return;
            }
        }

        /// <summary>
        /// 대기순번표 출력
        /// </summary>
        /// <param name="nSeqNo"></param>
        /// <param name="strTemp">0. 이름, 1. 주소, 2.상세주소, 3.전화번호, 4.휴대폰번호, 5.근무처, 6.주민번호, 7.문진표종류</param>
        /// <param name="nAge"></param>
        /// <param name="nPano"></param>
        private void Jepsu_Print(long nSeqNo, List<string> strTemp, int nAge, string strPtno)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();

            string strLtdName = CF.TextBox_2_MultiLine(strTemp[5], 15);
            string strLtdName1 = VB.Trim(VB.Pstr(strLtdName, "{{@}}", 1));
            string strLtdName2 = VB.Trim(VB.Pstr(strLtdName, "{{@}}", 2));
            string strLtdName3 = VB.Trim(VB.Pstr(strLtdName, "{{@}}", 3));

            string strJuso = CF.TextBox_2_MultiLine(strTemp[1] + strTemp[2], 30);
            string strJuso1 = VB.Trim(VB.Pstr(strJuso, "{{@}}", 1));
            string strJuso2 = VB.Trim(VB.Pstr(strJuso, "{{@}}", 2));
            string strJuso3 = VB.Trim(VB.Pstr(strJuso, "{{@}}", 3));

            string strMunjin = CF.TextBox_2_MultiLine(strTemp[7], 30);
            string sstrMunjin1 = VB.Trim(VB.Pstr(strMunjin, "{{@}}", 1));
            string sstrMunjin2 = VB.Trim(VB.Pstr(strMunjin, "{{@}}", 2));
            string sstrMunjin3 = VB.Trim(VB.Pstr(strMunjin, "{{@}}", 3));

            SS_Clear_ssPtr(ssPrt);

            ssPrt.ActiveSheet.Cells[0, 2].Text = nSeqNo.To<string>("");     //대기번호
            ssPrt.ActiveSheet.Cells[2, 2].Text = strTemp[0];                //수검자명
            ssPrt.ActiveSheet.Cells[4, 2].Text = VB.Left(strTemp[6], 6) + "-" + VB.Mid(strTemp[6], 7, 1) + "(만 " + nAge + "세)";    //주민번호
            ssPrt.ActiveSheet.Cells[5, 2].Text = strTemp[3];                //전화번호
            ssPrt.ActiveSheet.Cells[6, 2].Text = strTemp[4];                //휴대폰번호
            ssPrt.ActiveSheet.Cells[7, 2].Text = strLtdName1;               //근무처 1 Line
            ssPrt.ActiveSheet.Cells[8, 2].Text = strLtdName2;               //근무처 2 Line
            ssPrt.ActiveSheet.Cells[10, 0].Text = "▶결과지 받을 주소[" + strPtno + "]";   //결과지 받을 주소 []
            ssPrt.ActiveSheet.Cells[11, 0].Text = strJuso1;   //주소1
            ssPrt.ActiveSheet.Cells[12, 0].Text = strJuso2;   //주소2
            ssPrt.ActiveSheet.Cells[13, 0].Text = strJuso3;   //주소3
            ssPrt.ActiveSheet.Cells[16, 0].Text = sstrMunjin1;   //받으실 문진표 종류1
            ssPrt.ActiveSheet.Cells[17, 0].Text = sstrMunjin2;   //받으실 문진표 종류2
            Application.DoEvents();
            string strPrintName = CP.getPmpaBarCodePrinter("접수증"); 
            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 0, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);

            SPR.setSpdPrint(ssPrt, false, setMargin, setOption, "", "", strPrintName);

            ComFunc.Delay(1500);

        }

        private void SS_Clear_ssPtr(FpSpread ssPrt)
        {
            ssPrt.ActiveSheet.Cells[0, 3].Text = "";    //대기번호
            ssPrt.ActiveSheet.Cells[2, 3].Text = "";    //수검자명
            ssPrt.ActiveSheet.Cells[4, 3].Text = "";    //주민번호
            ssPrt.ActiveSheet.Cells[5, 3].Text = "";    //전화번호
            ssPrt.ActiveSheet.Cells[6, 3].Text = "";    //휴대폰번호
            ssPrt.ActiveSheet.Cells[7, 3].Text = "";    //근무처 1 Line
            ssPrt.ActiveSheet.Cells[8, 3].Text = "";    //근무처 2 Line
            ssPrt.ActiveSheet.Cells[10, 0].Text = "";   //결과지 받을 주소 []
            ssPrt.ActiveSheet.Cells[11, 0].Text = "";   //주소1
            ssPrt.ActiveSheet.Cells[12, 0].Text = "";   //주소2
            ssPrt.ActiveSheet.Cells[13, 0].Text = "";   //주소3
            ssPrt.ActiveSheet.Cells[16, 0].Text = "";   //받으실 문진표 종류1
            ssPrt.ActiveSheet.Cells[17, 0].Text = "";   //받으실 문진표 종류2
            ssPrt.ActiveSheet.Cells[20, 0].Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();//대기표 출력일자
        }

        private bool INSERT_JEPSU_Rtn(HIC_JEPSU_WORK iHJW)
        {
            string strGjBangi = string.Empty;
            string strGbDental = string.Empty;
            List<HIC_RESULT> lstHRES = new List<HIC_RESULT>();

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                strGjBangi = "1";
                //if (string.Compare(VB.Right(DateTime.Now.AddDays(-4).ToShortDateString(), 5), "07-01") >= 0) { strGjBangi = "2"; }
                if (string.Compare(VB.Right(DateTime.Now.ToShortDateString(), 5), "07-01") >= 0) { strGjBangi = "2"; }

                //신규번호 생성
                HIC_JEPSU nHJ = new HIC_JEPSU
                {
                    WRTNO = cHB.Read_New_JepsuNo(),
                    GWRTNO = cHB.Read_New_JepsuGWrtNo(),
                    GJBANGI = strGjBangi,
                    GBEXAM = "N",
                    GBCHUL = "N",
                    JOBSABUN = 111,
                    GBSTS = "1",
                    AUTOJEP = "Y",
                    JONGGUMYN = "0"
                };

                if (heaJepsuService.GetCountbyPtNoSDate(iHJW.PTNO, DateTime.Now.ToShortDateString()) > 0) { nHJ.JONGGUMYN = "1"; }

                //포항성모병원 직원은 웹결과지 자동 신청(채용검진은 제외)
                if (iHJW.LTDCODE == 483 && iHJW.GJJONG != "21")
                {
                    nHJ.WEBPRINTREQ = DateTime.Now;
                }

                //접수마스타에 INSERT
                if (!hicJepsuService.SelectWorkJepsuInsert(nHJ, iHJW.RID))
                {
                    return false;
                }

                //가접수 내용이 있을경우 삭제
                if (!hicJepsuWorkService.DeleteByRowid(iHJW.RID)) { return false; }

                //HIC_SUNAP 생성
                if (!hicSunapService.InsertSelectBySuDatePano(nHJ.WRTNO, iHJW.JEPDATE, iHJW.PANO, iHJW.GJJONG)) { return false; }

                //기존의 자료가 있으면 삭제함
                if (!hicSunapWorkService.DeleteByPanoSuDate(iHJW.JEPDATE, iHJW.PANO)) { return false; }

                //HIC_SUNAPDTL 생성
                if (!hicSunapdtlService.InsertSelectBySunapDtlWork(nHJ.WRTNO, iHJW)) { return false; }

                //기존의 자료가 있으면 삭제함
                if (!hicSunapdtlWorkService.DeletebyPaNoSuDate(iHJW)) { return false; }

                //HIC_RESULT 생성
                List<GROUPCODE_EXAM_DISPLAY> lstGED = groupCodeExamDisplayService.GetExamListByWrtno(nHJ.WRTNO);

                if (lstGED.Count > 0)
                {
                    GROUPCODE_EXAM_DISPLAY GED1 = lstGED.Find(x => x.EXCODE == "ZD00");
                    GROUPCODE_EXAM_DISPLAY GED2 = lstGED.Find(x => x.EXCODE == "ZD01");

                    if (!GED1.IsNullOrEmpty())
                    {
                        if (GED1.EXCODE == "ZD00") { strGbDental = "Y"; }
                    }

                    if (!GED2.IsNullOrEmpty())
                    {
                        if (GED2.EXCODE == "ZD01") { strGbDental = "Y"; }
                    }

                    //if (GED1.EXCODE == "ZD00" || GED2.EXCODE == "ZD01") { strGbDental = "Y"; }

                    HIC_RESULT dHRES = null;
                    
                    for (int i = 0; i < lstGED.Count; i++)
                    {
                        if (lstHRES.Find(x => x.EXCODE == lstGED[i].EXCODE).IsNullOrEmpty())    //중복검사 제외
                        {
                            dHRES = new HIC_RESULT
                            {
                                EXCODE      = lstGED[i].EXCODE,
                                GROUPCODE   = lstGED[i].GROUPCODE,
                                PART        = VB.Trim(lstGED[i].ENTPART),
                                RESCODE     = lstGED[i].RESCODE,
                                WRTNO       = nHJ.WRTNO
                            };

                            lstHRES.Add(dHRES);
                        }
                    }

                    for (int i = 0; i < lstHRES.Count; i++)
                    {
                        if (!hicResultService.InsertData(lstHRES[i]))
                        {
                            MessageBox.Show("검사항목 INSERT 시 오류 발생", "오류");
                            return false;
                        }
                    }
                }

                HIC_JEPSU rHJ = hicJepsuService.GetItemByWRTNO(nHJ.WRTNO);

                if (!rHJ.IsNullOrEmpty())
                {
                    if (!cHMF.HIC_NEW_SANGDAM_INSERT(rHJ))  //상담테이블
                    {
                        MessageBox.Show("신규상담항목 자동발생 시 오류가 발생함.", "오류");
                        return false;
                    }
                }

                //EMR 생성
                if (iHJW.PANO != 999)
                {
                    clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, iHJW.PTNO, DateTime.Now.ToShortDateString(), "HR", "HR", "정상", "99916");
                }

                //문진 대상항목 설정
                if (!cHcMain.Munjin_ITEM_SET(nHJ.WRTNO))
                {
                    MessageBox.Show("문진 대상항목 UpDate 시 오류가 발생함", "오류");
                    return false;
                }

                //2차인경우 1차판정의사 Update(2차판정은 1차 판정의사가 하기 위해)
                if (!cHcMain.UPDATE_FirstPanjeng_DrNo(nHJ.WRTNO))
                {
                    MessageBox.Show("2차검진자 1차판정의사 면허번호 UpDate 시 오류가 발생함", "오류");
                    return false;
                }

                //문진테이블 생성
                if (!rHJ.IsNullOrEmpty())
                {
                    rHJ.GBDENTAL = strGbDental;
                    rHJ.UCODES = iHJW.UCODES;
                    if (!cHMF.HIC_NEW_MUNITEM_INSERT(rHJ))  //검진1차,특수,구강,암의 판정테이블
                    {
                        MessageBox.Show("신규문진항목 자동발생시 오류가 발생함.", "오류");
                        return false;
                    }
                }
                

                //토요일 등 휴일가산
                if (cHB.HIC_Huil_GasanDay(DateTime.Now.ToShortDateString()))
                {
                    if (!Huil_JinCode_ADD(nHJ.WRTNO, DateTime.Now.ToShortDateString()))
                    {
                        MessageBox.Show("휴일가산 수가 자동발생시 오류가 발생함.", "오류");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("INSERT_JEPSU_Rtn Error " + ex.Message);
                return false;                
            }
        }

        private bool Huil_JinCode_ADD(long nWRTNO, string argDate)
        {
            int nCNT = 0, nCNT2 = 0;

            try
            {
                List<HIC_SUNAPDTL> list = hicSunapdtlService.GetAllbyWrtNo(nWRTNO);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CODE.To<string>("").Trim() == "1601") { nCNT += 1; }
                        else if (list[i].CODE.To<string>("").Trim() == "1701") { nCNT += 1; }
                        else if (list[i].CODE.To<string>("").Trim() == "1801") { nCNT += 1; }
                        else if (list[i].CODE.To<string>("").Trim() == "4401") { nCNT += 1; }
                        else if (list[i].CODE.To<string>("").Trim() == "4501") { nCNT += 1; }
                        else if (list[i].CODE.To<string>("").Trim() == "4601") { nCNT += 1; }
                        else if (list[i].CODE.To<string>("").Trim() == "1117") { nCNT2 += 1; }
                    }
                }

                //2차재검 가산 진찰료가 없으면 대상이 아님
                if (nCNT == 0) { return true; }
                //이미 가산코드가 있으면 대상이 아님
                if (nCNT2 > 0) { return true; }

                bool bOK = false;
                long nAMT = 0;
                long nAmtNo = 0;
                long nPrice = 0;
                string strGroupGbSuga = string.Empty;
                string strGbSuga = string.Empty;

                //휴일가산 금액을 읽음
                List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst = hicGroupexamGroupcodeExcodeService.GetListByCode("1117", null);
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        strGroupGbSuga = lst[i].GBSUGA.To<string>("").Trim();      //그룹
                        strGbSuga = lst[i].SUGAGBN.To<string>("").Trim();          //검사항목
                                                                                   //묶음코드에 수가적용구분이 없으면 그룹코드의 구분으로 적용함.
                        if (strGbSuga == "") { strGbSuga = strGroupGbSuga; }

                        // Amt1 = 보험수가의 80%
                        // Amt2 = 보험수가의 100%
                        // Amt3 = 보험수가의 125%
                        // Amt4 = 일반+특검 차액
                        // Amt5 = 임의수가

                        nAmtNo = strGbSuga.To<int>();
                        //전년도 건진사업이면 Old수가를 적용함
                        bOK = false;
                        if (string.Compare(DateTime.Now.Year.To<string>(""), VB.Left(DateTime.Now.ToShortDateString(), 4)) < 0)
                        {
                            if (string.Compare(lst[i].SUDATE, VB.Left(DateTime.Now.ToShortDateString(), 4) + "-01-01") >= 0) { bOK = true; }
                            //전년도 자료를 수정한 경우는 제외
                            if (VB.Left(argDate, 4) == DateTime.Now.Year.To<string>("")) { bOK = false; }
                        }

                        if (bOK)
                        {
                            nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }
                        else
                        {
                            if (string.Compare(argDate, lst[i].SUDATE) > 0)
                            {
                                nPrice = lst[i].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                            }
                            else
                            {
                                nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                            }
                        }

                        nAMT += nPrice;
                    }
                }

                //HIC_SUNAPDTL에 INSERT
                HIC_SUNAPDTL iHSDTL = new HIC_SUNAPDTL();
                iHSDTL.WRTNO = nWRTNO;
                iHSDTL.CODE = "1117";
                iHSDTL.UCODE = "";
                iHSDTL.AMT = nAMT;
                iHSDTL.GBSELF = "01";

                if (!hicSunapdtlService.InsertData(iHSDTL))
                {
                    return false;
                }

                //HIC_SUNAP에 UPDATE
                HIC_SUNAP iHS = hicSunapService.GetHicSunapByWRTNO(nWRTNO);

                if (iHS.IsNullOrEmpty())
                {
                    iHS.TOTAMT += nAMT;
                    iHS.JOHAPAMT += nAMT;

                    if (!hicSunapService.UpdateTotAmtJhpAmtbyRowid(iHS))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void eVerify_Message_Save(string strJumin)
        {
            try
            {
                if (!strJumin.IsNullOrEmpty())
                {
                    txtJumin.Text = strJumin;
                    timer1.Start();
                }
                else
                {
                    timer1.Start();
                    btnSave.Enabled = true;
                    FbClick = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("eVerify_Message_Save Error " + ex.Message);
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            string strDate = DateTime.Now.ToShortDateString();
            //string strDate = DateTime.Now.AddDays(-4).ToShortDateString();

            try
            {
                for (int i = 0; i < 50; i++)
                {
                    btnNames[i] = (Controls.Find("SName" + i.ToString(), true)[0] as DevComponents.DotNetBar.ButtonX);
                    btnNames[i].Click += new EventHandler(eBtnNameClick);
                }

                panHeaInfo.Visible = false;
                txtJumin.Text = "";
                FnTimer3 = 0;

                panSubMsg.Text = "공단검진 예약하신 본인의 이름을 누르시면 대기순번표가 인쇄 됩니다.";

                //예약자 최종 접수번호
                nSEQ_Yeyak = hicWaitService.GetMaxSeqnoByJobDate(strDate, "예약");

                //일반 최종 접수번호
                nSEQ_Ilban = hicWaitService.GetMaxSeqnoByJobDate(strDate, "일반");
                if (nSEQ_Ilban < 100) { nSEQ_Ilban = 100; }

                FstrJobGbn = "HIC";

                Display_Yeyak();

                Job_TimerResv();

                timer1.Start();
                timerResv.Start();
                timerShutdown.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 암검진 예약자 명단을 표시함
        /// </summary>
        private void Display_Yeyak()
        {
            int i = 0, nCNT = 0, nMaxCnt = 0, nAge = 0;
            string strJumin = "", strJumin2 = "", strSex = "", strOK = "";

            HIC_KIOSK vYEYAK = new HIC_KIOSK();

            YEYAK.Clear();

            //string strFDate = DateTime.Now.AddDays(-4).ToShortDateString();
            //string strTDate = DateTime.Now.AddDays(-3).ToShortDateString();
            string strFDate = DateTime.Now.ToShortDateString();
            string strTDate = DateTime.Now.AddDays(1).ToShortDateString();

            //암검진 예약자
            List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetItembyRTime2(strFDate, strTDate);

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                nCNT = 0;

                for (i = 0; i < list.Count; i++)
                {
                    strOK = "OK";
                    strJumin2 = list[i].JUMIN2.To<string>("").Trim();

                    //오늘 이미 접수를 하였는지 점검
                    string strRowid = hicWaitService.GetRowidByJobDateJumin2(strFDate, strJumin2);

                    if (!strRowid.IsNullOrEmpty()) { strOK = ""; }

                    if (strOK == "OK")
                    {
                        nCNT += 1;

                        if (nCNT <= 200)
                        {
                            strJumin = clsAES.DeAES(strJumin2);
                            string strMidJumin = VB.Mid(strJumin, 7, 1);
                            if (strMidJumin == "1" || strMidJumin == "3" || strMidJumin == "0" || strMidJumin == "5" || strMidJumin == "7")
                            {
                                strSex = "남";
                            }
                            else
                            {
                                strSex = "여";
                            }

                            nAge = ComFunc.AgeCalcEx(strJumin, strFDate);

                            vYEYAK = new HIC_KIOSK
                            {
                                YNO     = nCNT,
                                SNAME   = list[i].SNAME,
                                AGE     = nAge.To<string>("0") + "세/" + strSex,
                                PTNO    = list[i].PANO,
                                JUMIN   = strJumin,
                                RID     = list[i].ROWID
                            };

                            YEYAK.Add(vYEYAK);
                        }
                    }

                }
            }

            nYeyakTCnt = nCNT;
            nYeyakTPage = (int)Math.Truncate(nCNT / 50.0) + 1;
            nYeyakPage = 1;
            nMaxCnt = nCNT;
            if (nMaxCnt >= 50) { nMaxCnt = 50; }

            if (nCNT <= 50)
            {
                btnNameUp.Visible = false;
                btnNameDown.Visible = false;
            }
            else
            {
                btnNameUp.Visible = false;
                btnNameDown.Visible = true;
            }

            for (i = 0; i < 50; i++)
            {
                DevComponents.DotNetBar.ButtonX ctlSName = (Controls.Find("SName" + i.ToString(), true)[0] as DevComponents.DotNetBar.ButtonX);
                if (i < YEYAK.Count)
                {
                    ctlSName.Text = YEYAK[i].SNAME + "<br></br><font size ='18'>" + YEYAK[i].AGE + "</font>";
                    ctlSName.Enabled = true;
                }
                else
                {
                    ctlSName.Text = "";
                    ctlSName.Enabled = false;
                    //ctlSName.Visible = false;
                }
                            
            }

        }

        private void Msg_Screen_Display()
        {
            FnTimer2 = 0;
            panMsg.Text = FstrMsg;
            timer1.Stop();
            timer2.Start();
            panMsg.Visible = true;
            Application.DoEvents();
        }

        private void Button_Enabled(bool bFlag)
        {
            for (int i = 0; i < 10; i++)
            {
                DevComponents.DotNetBar.ButtonX ctlSName = (Controls.Find("Num" + i.ToString(), true)[0] as DevComponents.DotNetBar.ButtonX);
                ctlSName.Enabled = bFlag;
            }

            btnCancel.Enabled = bFlag;
            btnSave.Enabled = bFlag;
        }
    }
}
