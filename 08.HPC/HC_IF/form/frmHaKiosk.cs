using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using HC_IF.Dto;
using HC_Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace HC_IF
{
    public partial class frmHaKiosk :Form
    {
        frmHcKiosk fHcKsk = null;

        DevComponents.DotNetBar.ButtonX[] btnNames = new DevComponents.DotNetBar.ButtonX[80];

        List<HEA_KIOSK> lstHAKIOSK = new List<HEA_KIOSK>();
        List<HEA_SUNAPDTL> haSUNAPDTL = new List<HEA_SUNAPDTL>();
        HEA_SUNAP haSUNAP = null;

        long FnGwrtno = 0;
        string FstrWRTNO = "";
        string FstrCommit = "";
        string FstrHuilCheck = "";
        string FstrJumin = "";
        string FstrBirth = "";
        string FstrNewExams = "";
        
        bool FbClick = false;
        int FnIndex = 0;

        clsHcFunc cHF = null;
        ComFunc CF = null;
        clsHaBase cHB = null;
        clsHcMain cHcMain = null;
        clsHcOrderSend cHOS = null;
        clsHcMainFunc cHMF = null;
        HicBcodeService hicBcodeService = null;
        HicPatientService hicPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HeaResultService heaResultService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        HeaSunapService heaSunapService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HeaJepsuSunapdtlService heaJepsuSunapdtlService = null;
        HeaJepsuService heaJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HeaGamcodeService heaGamcodeService = null;
        HeaGroupcodeService heaGroupcodeService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicWaitService hicWaitService = null;
        HicJepsuResultService hicJepsuResultService = null;
        HeaResvExamService heaResvExamService = null;
        XrayDetailService xrayDetailService = null;

        string strInChulPath = @"c:\cmc\exe\inchul_t.exe";

        public frmHaKiosk()
        {
            InitializeComponent();
            SetEvents();
            SetControl();
        }

        private void SetControl()
        {
            cHF = new clsHcFunc();
            CF = new ComFunc();
            cHB = new clsHaBase();
            cHcMain = new clsHcMain();
            cHOS = new clsHcOrderSend();
            cHMF = new clsHcMainFunc();
            hicBcodeService = new HicBcodeService();
            hicPatientService = new HicPatientService();
            hicJepsuService = new HicJepsuService();
            hicJepsuWorkService = new HicJepsuWorkService();
            heaResultService = new HeaResultService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            heaSunapService = new HeaSunapService();
            heaSunapdtlService = new HeaSunapdtlService();
            heaJepsuSunapdtlService = new HeaJepsuSunapdtlService();  
            heaJepsuService = new HeaJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            heaGamcodeService = new HeaGamcodeService();
            heaGroupcodeService = new HeaGroupcodeService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicWaitService = new HicWaitService();
            hicJepsuResultService = new HicJepsuResultService();
            heaResvExamService = new HeaResvExamService();
            xrayDetailService = new XrayDetailService();
        }

        private void SetEvents()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.timer1.Tick        += new EventHandler(eTimer_Timer);
            this.timer2.Tick        += new EventHandler(eTimer_Timer);
            this.timerStop.Tick     += new EventHandler(eTimer_Timer);

            this.picLogo.Click      += new EventHandler(ePicDblClick);
            this.lblGubun.DoubleClick += new EventHandler(eLblDblClick);
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
                    timer2.Stop();
                    Proc.StartInfo.FileName = strInChulPath;
                    Proc.Start();
                }
            }
        }

        private void eLblDblClick(object sender, EventArgs e)
        {
            timer2.Stop();
            timerStop.Stop();

            this.Close();
            return;
        }

        private void eTimer_Timer(object sender, EventArgs e)
        {
            if (sender == timer1)
            {
                bool bShut = true;

                for (int i = 0; i < 80; i++)
                {
                    if (btnNames[i].Text.Trim() != "")
                    {
                        bShut = false;
                        break;
                    }
                }

                if (bShut)
                {
                    
                    //---------------------------------------
                    //  12:30 자동으로 종검키오스크로 전환
                    //---------------------------------------
                    ComFunc.ReadSysDate(clsDB.DbCon);
                    clsPublic.GstrIpAddress = clsVbfunc.GetIpAddressOeacle(clsDB.DbCon);

                    if (clsPublic.GstrIpAddress == "192.168.41.39")
                    {
                        foreach (Form Form in Application.OpenForms) //떠있는지 체크
                        {
                            if (Form.Name == "frmHcKiosk") { Form.Close(); break; }
                        }

                        foreach (Form Form in Application.OpenForms) //떠있는지 체크
                        {
                            if (Form.Name == "frmHaKiosk") { Form.Close(); break; }
                        }

                        timer1.Enabled = false;

                        fHcKsk = new frmHcKiosk();
                        fHcKsk.StartPosition = FormStartPosition.CenterScreen;
                        fHcKsk.ShowDialog();
                        cHF.fn_ClearMemory(fHcKsk);
                    }
                }
            }
            else if (sender == timer2)
            {
                timer2.Stop();

                for (int i = 0; i < 80; i++)
                {
                    btnNames[i].Enabled = false;
                    btnNames[i].Text = "";
                }

                Screen_Display();

                timer2.Start();
            }
            else if (sender == timerStop)
            {
                TimerShutdown_Timer();
            }
        }

        private void TimerShutdown_Timer()
        {
            string strDate = string.Empty;

            try
            {
                strDate = DateTime.Now.ToShortDateString();
                clsPublic.GstrIpAddress = clsVbfunc.GetIpAddressOeacle(clsDB.DbCon);
                ComFunc.ReadSysDate(clsDB.DbCon);

                //------------------------------------
                //  PC 자동종료 설정값을 읽음
                //------------------------------------
                string strYoil = CF.READ_YOIL(clsDB.DbCon, strDate);
                string strStopTIme = hicBcodeService.GetCodeNamebyGubunCode("BAS_표시장비자동종료시간", clsPublic.GstrIpAddress);

                if (strYoil == "토")
                {
                    strStopTIme = VB.Pstr(strStopTIme, "{}", 2);
                }
                else if (strYoil == "일")
                {
                    strStopTIme = "00:00";
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
                    timerStop.Stop();
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
                        timerStop.Stop();
                        Process.Start("shutdown.exe");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TimerShutdown Error " + ComNum.VBLF + ex.Message);
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            for (int i = 0; i < 80; i++)
            {
                btnNames[i] = (Controls.Find("SName" + i.ToString(), true)[0] as DevComponents.DotNetBar.ButtonX);
                btnNames[i].Click += new EventHandler(eBtnNameClick);
            }

            panMsg.Text = "본인의 이름을 누르시면 대기순번표가 인쇄 됩니다.";
            Amt_Clear();
            Screen_Clear();
            Screen_Display();
        }

        private void Screen_Display()
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            lstHAKIOSK.Clear();

            string strSDate = DateTime.Now.ToShortDateString();
            string strAmPm2 = string.Compare(clsPublic.GstrSysTime, "12:30") >= 0 ? "2" : "1";
            string strGbVip = string.Empty;

            HEA_KIOSK haKIOSK = null;           //FstrInfo, FstrInfo2  종검접수증 출력정보

            try
            {
                //strSDate = "2021-07-17"; strAmPm2 = "1";
                List<HEA_JEPSU_PATIENT> lstJepPat = heaJepsuPatientService.GetListBySDateAMPM2(strSDate, strAmPm2);

                if (lstJepPat.Count > 0)
                {
                    for (int i = 0; i < lstJepPat.Count; i++)
                    {
                        haKIOSK = new HEA_KIOSK();

                        if (!lstJepPat[i].SNAME.IsNullOrEmpty())
                        {
                            btnNames[i].Enabled = true;
                            btnNames[i].Text = lstJepPat[i].SNAME;
                        }

                        //골드검진,VIP검진 표시
                        strGbVip = "";
                        if (lstJepPat[i].GJJONG == "11" || lstJepPat[i].GJJONG == "12")
                        {
                            if (!heaSunapdtlService.CheckVipByWRTNOLikeCodeName(lstJepPat[i].WRTNO).IsNullOrEmpty())
                            {
                                strGbVip = "Y";
                            }
                        }

                        //검진비가 150만원이상이면 VIP검진
                        if (strGbVip != "Y")
                        {
                            if (heaSunapdtlService.GetSumAmtByWRTNO(lstJepPat[i].WRTNO) > 1500000)
                            {
                                strGbVip = "Y";
                            }
                        }

                        if (strGbVip == "Y")
                        {
                            btnNames[i].TextColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            btnNames[i].TextColor = System.Drawing.Color.Black;
                        }

                        //이름, 주민번호, 종검번호, 접수번호, 병원번호, 성별, 회사코드, 주소, 휴대폰번호
                        //  1       2         3         4         5      6       7       8         9
                        //종검접수증 출력 정보 공용사용
                        haKIOSK.SNAME   = lstJepPat[i].SNAME;
                        haKIOSK.JUMIN   = clsAES.DeAES(lstJepPat[i].JUMIN2);
                        haKIOSK.PANO    = lstJepPat[i].PANO.To<long>(0);
                        haKIOSK.WRTNO   = lstJepPat[i].WRTNO;
                        haKIOSK.PTNO    = lstJepPat[i].PTNO;
                        haKIOSK.SEX     = lstJepPat[i].SEX.To<string>("");
                        haKIOSK.AGE     = lstJepPat[i].AGE.To<long>(0);
                        haKIOSK.TEL     = lstJepPat[i].TEL.To<string>("");
                        haKIOSK.LTDCODE = lstJepPat[i].LTDCODE.To<long>(0);
                        haKIOSK.LTDNAME = lstJepPat[i].LTDNAME.To<string>("");
                        haKIOSK.JUSO    = lstJepPat[i].JUSO1.To<string>("") + " " + lstJepPat[i].JUSO2.To<string>("");
                        haKIOSK.HPHONE  = lstJepPat[i].HPHONE.To<string>("");
                        haKIOSK.GJJONG  = lstJepPat[i].GJJONG.To<string>("");
                        haKIOSK.GBSTS   = lstJepPat[i].GBSTS.To<string>("");
                        haKIOSK.INDX    = i;

                        lstHAKIOSK.Add(haKIOSK);

                        if (lstJepPat[i].GBSTS != "0")
                        {
                            btnNames[i].Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Screen_Clear()
        {
            for (int i = 0; i < 80; i++)
            {
                btnNames[i].Enabled = false;
                btnNames[i].Text = "";
            }

            lstHAKIOSK.Clear();

            FstrCommit = "";
        }

        private void Amt_Clear()
        {
            haSUNAP = new HEA_SUNAP();
        }

        private void eBtnNameClick(object sender, EventArgs e)
        {
            int Index = 0;
            string strOK = string.Empty;

            HEA_KIOSK haKIOSK = null;

            //클릭후 접수중이면 처리를 안함
            if (FbClick) { return; }

            Index = VB.Pstr(((DevComponents.DotNetBar.ButtonX)sender).Name, "Name", 2).To<int>();
            //빈칸을 클릭하면 처리를 안함
            if (!lstHAKIOSK.IsNullOrEmpty() && lstHAKIOSK[Index].SNAME.IsNullOrEmpty()) { return; }

            timer2.Enabled = false;
            btnNames[Index].Enabled = false;
            FnIndex = Index;

            haKIOSK = new HEA_KIOSK();

            haKIOSK = lstHAKIOSK[Index];   //선택한 검진자 인적정보 , 영수증 출력정보

            //주민등록번호로 생일을 구함
            FstrJumin = haKIOSK.JUMIN;

            //생년월일이 날짜형식이 아니면 접수를 안함(LYJ)
            FstrBirth = ComFunc.GetBirthDate(VB.Left(FstrJumin, 6), VB.Right(FstrJumin, 7), "-");
            strOK = "OK";

            if (FstrBirth.Length != 10) { strOK = ""; }
            if (FstrBirth.Trim() == "19--") { strOK = ""; }
            if (VB.IsDate(FstrBirth) == false) { strOK = ""; }

            //주민등록번호 오류 점검
            if (strOK != "OK")
            {
                FstrBirth = "";
                timer2.Enabled = true;
                btnNames[Index].Enabled = true;
                panMsg.Text = "본인의 이름을 누르시면 대기순번표가 인쇄 됩니다.";
                Application.DoEvents();
                return;
            }

            frmHcKiosk_Verify.rSetHaGstrValue += new frmHcKiosk_Verify.SetHaGstrValue(eVerify_Message_NameClick);
            frmHcKiosk_Verify fHV = new frmHcKiosk_Verify(haKIOSK);
            fHV.StartPosition = FormStartPosition.Manual;
            fHV.Location = new System.Drawing.Point(140, 390);
            fHV.ShowDialog();
            frmHcKiosk_Verify.rSetHaGstrValue -= new frmHcKiosk_Verify.SetHaGstrValue(eVerify_Message_NameClick);

            timer2.Enabled = true;
            panMsg.Text = "본인의 이름을 누르시면 대기순번표가 인쇄 됩니다.";
            Application.DoEvents();

        }

        private void eVerify_Message_NameClick(HEA_KIOSK argKIOSK)
        {
            try
            {
                if (!argKIOSK.SNAME.IsNullOrEmpty())
                {
                    FbClick = true;
                    panMsg.Text = "잠시 후 대기순번표가 인쇄됩니다. 대기순번표를 가져가세요..";
                    Application.DoEvents();

                    Read_SunapDTL(argKIOSK.WRTNO);
                    Data_Save(argKIOSK);

                    FbClick = false;
                }
                else
                {
                    btnNames[argKIOSK.INDX].Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("eVerify_Message_NameClick Error " + ex.Message);
            }
        }

        private void Data_Save(HEA_KIOSK argKIOSK)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                FnGwrtno = 0;
                FnGwrtno = cHB.Read_New_JepsuGWrtNo(); 

                if (!heaJepsuService.UpDateGbSTSCDate(argKIOSK.WRTNO, clsPublic.GstrSysTime, FnGwrtno))
                {
                    MessageBox.Show("종검 접수등록시 에러발생.", "Error");
                    clsDB.setCommitTran(clsDB.DbCon);
                    return;
                }
                
                //Exam_Acting_Update
                List<HIC_RESULT_EXCODE> haResult = hicResultExCodeService.GetItemHicHeabyWrtNoOrderbyPanjengPartExCode("HEA", argKIOSK.WRTNO);
                if (haResult.Count > 0)
                {
                    HEA_RESULT hRES = null;

                    for (int i = 0; i < haResult.Count; i++)
                    {
                        hRES = new HEA_RESULT
                        {
                            ACTPART = haResult[i].HEAPART,
                            PART = haResult[i].PART,
                            RESCODE = haResult[i].RESCODE.To<string>(""),
                            EXCODE = haResult[i].EXCODE,
                            WRTNO = argKIOSK.WRTNO,
                            RESULT = ""
                        };

                        if (!heaResultService.UpDatePartResCodeByExCodeWrtno(hRES))
                        {
                            MessageBox.Show("종검 결과 UPDATE시 에러발생.", "Error");
                            clsDB.setCommitTran(clsDB.DbCon);
                            return;
                        }
                    }
                }

                //수납금액계산
                if (!Account_Hea_Sunap(argKIOSK))
                {
                    MessageBox.Show("종검 수납금액 계산시 에러발생.", "Error");
                    clsDB.setCommitTran(clsDB.DbCon);
                    return;
                }

                //종검접수 정보 읽기
                HEA_JEPSU iHJ = heaJepsuService.GetItemByWrtno(argKIOSK.WRTNO);
                if (!iHJ.IsNullOrEmpty())
                {
                    //방사선오더 자동 발생
                    if (iHJ.PANO != 999)
                    {
                        COMHPC eOS = new COMHPC
                        {
                            WRTNO   = argKIOSK.WRTNO,
                            PANO    = argKIOSK.PANO,
                            SDATE   = iHJ.SDATE,
                            JEPDATE = iHJ.JEPDATE,
                            SNAME   = iHJ.SNAME,
                            JUMIN   = argKIOSK.JUMIN,
                            PTNO    = iHJ.PTNO,
                            SEX     = argKIOSK.SEX,
                            AGE     = argKIOSK.AGE,
                            JOBSABUN = 222,
                            DRCODE = "7102",
                            DEPTCODE = "TO",
                            LTDCODE = argKIOSK.LTDCODE
                        };

                        if (!eOS.PTNO.IsNullOrEmpty())
                        {
                            if (!cHOS.EXAM_ORDER_SEND(eOS, "TO", ""))
                            {
                                MessageBox.Show("Xray Order 전송중 오류가 발생함", "오류");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            //내시경 간호기록지 및 외래접수 생성
                            if (!cHcMain.HIC_ENDOCHART_INSERT(argKIOSK.WRTNO, iHJ.SDATE, iHJ.PTNO, "TO", iHJ.PANO))
                            {
                                MessageBox.Show("내시경기록지 생성시 오류가 발생함.", "오류");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            if (!cHMF.Jin_Support_Data_Send_TO(iHJ, "1", iHJ.SDATE))
                            {
                                MessageBox.Show("기능검사 접수시 오류가 발생함.", "오류");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, iHJ.PTNO, DateTime.Now.ToShortDateString(), "TO", "TO", "정상", "99917");
                        }
                    }
                }

                //MRI

                string strRowid_Xray = "";
                strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(iHJ.PTNO, "HI135A", iHJ.SDATE, "TO");
                //Xray_Detail MRI 구분변경
                if (!strRowid_Xray.IsNullOrEmpty())
                {
                    xrayDetailService.InsertHis(strRowid_Xray);
                    xrayDetailService.UpDate_GBRESERVED(strRowid_Xray, "1");    //UPDATE XRAY_DETAIL
                }

                Amt_Clear();

                //일반건진 가접수 내역 접수로 전환
                if (iHJ.GONGDAN == "Y")
                {
                    if (!Read_Hic_GaJepsu(argKIOSK.PTNO, VB.Left(iHJ.SDATE, 4)))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //GWRTNO UPDATE
                    if (!hicJepsuService.UpDateGWRTNOByPtnoJepDate(argKIOSK.PTNO, iHJ.SDATE, FnGwrtno))
                    {
                        MessageBox.Show("일반검진 접수시 GWRTNO UPDATE오류!!", "Error");
                        clsDB.setCommitTran(clsDB.DbCon);
                        return;
                    }
                }

                //GBHEAENDO UPDATE(2021-05-31)
                if (heaJepsuService.GetEndoGbnbyPtNo(argKIOSK.PTNO) == "1")
                {

                    if (!hicJepsuService.UpDateGBHEAENDO(argKIOSK.PTNO, "Y"))
                    {
                        MessageBox.Show("일반검진접수 내시경실 UPDATE오류!", "오류");
                        return;
                    }
                }

                //종검수검자 대기순번표 업데이트 및 인쇄
                if (!Print_HEA_WaitPager(argKIOSK, iHJ))
                {
                    MessageBox.Show("종검 대기순번 등록시 오류가 발생함.", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //2015-07-21 일특 액팅코드 INSERT
                if (!Hic_Spc_Acting_Insert(argKIOSK.PTNO, argKIOSK.WRTNO))
                {
                    MessageBox.Show("일특 액팅코드 INSERT시 오류가 발생함.", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //clsDB.setRollbackTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data_Save Error " + ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
        }

        private bool Hic_Spc_Acting_Insert(string argPtno, long nWRTNO)
        {
            string[] strSpcExam = new string[9];

            try
            {
                List<HIC_JEPSU_RESULT> lst = hicJepsuResultService.GetListSpcExamByPtno(argPtno);
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].EXCODE == "MU11" || lst[i].EXCODE == "MU15" || lst[i].EXCODE == "MU12" || lst[i].EXCODE == "MU74")
                        {
                            strSpcExam[1] = "Y"; //소변(특수)
                        }
                        if (lst[i].EXCODE == "LM10")
                        {
                            strSpcExam[2] = "Y"; //객담
                        }
                        if (lst[i].EXCODE == "TR11")
                        {
                            strSpcExam[3] = "Y"; //폐활량3회
                        }
                        if (lst[i].EXCODE == "A992")
                        {
                            strSpcExam[4] = "Y"; //분변검사
                        }
                        if (lst[i].EXCODE == "A993")
                        {
                            strSpcExam[5] = "Y"; //유방촬영
                        }
                        if (lst[i].EXCODE == "A803")
                        {
                            strSpcExam[6] = "Y"; //PAP
                        }
                    }
                }

                string strExCode = string.Empty;
                string strActPart = string.Empty;

                for (int i = 1; i <= 6; i++)
                {
                    if (strSpcExam[i] == "Y")
                    {
                        switch (i)
                        {
                            case 1: strExCode = "A903"; strActPart = "1"; break;  //특수소변
                            case 2: strExCode = "A902"; strActPart = "G"; break;  //객담
                            case 3: strExCode = "A920"; strActPart = "7"; break;  //폐활량3회검사(ACT)
                            case 4: strExCode = "A992"; strActPart = "J"; break;  //분변검사
                            case 5: strExCode = "A993"; strActPart = "K"; break;  //유방촬영
                            case 6: strExCode = "A803"; strActPart = "O"; break;  //PAP
                            default: strExCode = ""; break;
                        }

                        if (!strExCode.IsNullOrEmpty())
                        {
                            if (heaResultService.GetRowidByOneExcodeWrtno(strExCode, nWRTNO).IsNullOrEmpty())
                            {
                                HEA_RESULT item = new HEA_RESULT
                                {
                                    WRTNO = nWRTNO,
                                    EXCODE = strExCode,
                                    PART = "5",
                                    RESCODE = "X05",
                                    PANJENG = "",
                                    RESULT = "",
                                    ACTPART = strActPart
                                };

                                if (!heaResultService.InSert(item))
                                {
                                    MessageBox.Show("종검 결과입력 DB에 INSERT 오류.", "확인");
                                    return false;
                                }
                            }
                        }
                        
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Print_HEA_WaitPager(HEA_KIOSK argKIOSK, HEA_JEPSU iHJ)
        {
            string strJuso = string.Empty;
            string strIEMunjin = string.Empty;
            string strCurDate = DateTime.Now.ToShortDateString();
            string strGjWrtno = string.Empty;

            List<string> strTemp = new List<string>();

            try
            {
                HIC_PATIENT iHP = hicPatientService.GetPatInfoByPtno(argKIOSK.PTNO);

                if (iHP.IsNullOrEmpty())
                {
                    MessageBox.Show("수검자 정보가 없습니다.", "확인");
                    return false;
                }

                strJuso = iHP.JUSO1 + " ";
                if (!iHP.JUSO2.IsNullOrEmpty()) { strJuso += iHP.JUSO2; }

                //인터넷문진 여부를 읽음
                strIEMunjin = "";
                if (iHP.PTNO != "")
                {
                    string strDate = DateTime.Now.AddDays(-60).ToShortDateString();
                    if (!hicIeMunjinNewService.GetItembyPtNoMunDate(iHP.PTNO, strDate, "HEA").IsNullOrEmpty())
                    {
                        strIEMunjin += "▶IE문진";
                    }
                }

                //일반검진 접수번호
                strGjWrtno = "일반검진 접수번호:";
                List<HIC_JEPSU> hcJEP = hicJepsuService.GetListByPtnoJepDate(argKIOSK.PTNO, iHJ.SDATE);
                if (hcJEP.Count > 0)
                {
                    for (int i = 0; i < hcJEP.Count; i++)
                    {
                        strGjWrtno += hcJEP[i].GJJONG.To<string>("") + "종 ";
                        strGjWrtno += hcJEP[i].WRTNO.To<string>("") + ", ";
                    }
                }


                long nCount = 0;
                string strEndo = "";
                string strSTime = "";
                string strYoil = CF.READ_YOIL(clsDB.DbCon, iHJ.SDATE);

                //대장내시경확인
                HEA_RESV_EXAM item1 = heaResvExamService.GetCountbyPaNo1(iHP.PANO, "02", iHJ.SDATE);
                if(strYoil == "토요일")
                {
                    strSTime = "07:50";
                    if (!item1.IsNullOrEmpty() && iHJ.STIME == "07:50")
                    {
                        strEndo = "OK";
   
                    }
                }
                else
                {
                    strSTime = "07:20";
                    if (!item1.IsNullOrEmpty() && iHJ.STIME == "07:20")
                    {
                        strEndo = "OK";
                    }
                }
                

                HEA_RESV_EXAM item2 = heaResvExamService.GetCountBySdateGbexam(iHJ.SDATE, "02", strSTime);
                if(!item2.IsNullOrEmpty()) { nCount = item2.CNT + 1; }


                //대기순번을 발급
                long nSeqNo = hicWaitService.GetMaxSeqnoByJobDateGbBuse(strCurDate, "1", strEndo, nCount);
                //long nSeqNo = hicWaitService.GetMaxSeqnoByJobDateGbBuse("2021-07-17", "1", strEndo, nCount);
                if (strEndo=="" && nSeqNo == 0)
                {
                    nSeqNo = nCount - 1;
                }

                nSeqNo += 1;



                HIC_WAIT item = new HIC_WAIT
                {
                    JOBDATE = strCurDate,
                    SEQNO = nSeqNo,
                    JUMIN = VB.Left(clsAES.DeAES(iHP.JUMIN2), 7) + "******",
                    JUMIN2 = iHP.JUMIN2,
                    SNAME = iHP.SNAME,
                    GBJOB = "1",
                    GBYEYAK = "",
                    GBAUTOJEP = "",
                    GJJONG = "83",
                    JEPTIME = clsPublic.GstrSysTime,
                    CALLTIME = "",
                    ENDTIME = "",
                    GBBUSE = "1"
                };

                if (!hicWaitService.InsertData(item))
                {
                    MessageBox.Show("종검 대기순번표 DB에 저장 오류.", "확인");
                    return false;
                }

                strTemp.Clear();
                strTemp.Add(iHP.SNAME);                     //성명
                strTemp.Add(iHP.JUSO1);                     //주소1
                strTemp.Add(iHP.JUSO2);                     //주소2
                strTemp.Add(iHP.TEL);                       //전화번호
                strTemp.Add(iHP.HPHONE);                    //휴대폰번호
                strTemp.Add(iHP.LTDNAME.To<string>(" "));   //근무처
                strTemp.Add(clsAES.DeAES(iHP.JUMIN2));      //주민번호
                strTemp.Add(strIEMunjin);                   //문진종류
                strTemp.Add(strGjWrtno);                    //일반검진 종류/접수번호

                Jepsu_Print(nSeqNo, strTemp, (int)argKIOSK.AGE, argKIOSK.PTNO);

                return true;
            }
            catch (Exception)
            {
                return false;
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

            string strHcGjWrtno = CF.TextBox_2_MultiLine(strTemp[8], 30);
            string strHcGjWrtno1 = VB.Trim(VB.Pstr(strHcGjWrtno, "{{@}}", 1));
            string strHcGjWrtno2 = VB.Trim(VB.Pstr(strHcGjWrtno, "{{@}}", 2));

            SS_Clear_ssPtr(ssPrt);

            ssPrt.ActiveSheet.Cells[0, 2].Text = nSeqNo.To<string>("");     //대기번호
            ssPrt.ActiveSheet.Cells[2, 2].Text = strTemp[0];                //수검자명
            ssPrt.ActiveSheet.Cells[4, 2].Text = VB.Left(strTemp[6], 6) + "-" + VB.Mid(strTemp[6], 7, 1) + "******";    //주민번호
            ssPrt.ActiveSheet.Cells[5, 2].Text = strTemp[3];                //전화번호
            ssPrt.ActiveSheet.Cells[6, 2].Text = strTemp[4];                //휴대폰번호
            ssPrt.ActiveSheet.Cells[7, 2].Text = strLtdName1;               //근무처 1 Line
            ssPrt.ActiveSheet.Cells[8, 2].Text = strLtdName2;               //근무처 2 Line
            ssPrt.ActiveSheet.Cells[10, 0].Text = "▶결과지 받을 주소[" + strPtno + "]";   //결과지 받을 주소 []
            ssPrt.ActiveSheet.Cells[11, 0].Text = strJuso1;   //주소1
            ssPrt.ActiveSheet.Cells[12, 0].Text = strJuso2;   //주소2
            ssPrt.ActiveSheet.Cells[13, 0].Text = strJuso3;   //주소3
            ssPrt.ActiveSheet.Cells[15, 0].Text = strMunjin;  //인터넷문진표
            ssPrt.ActiveSheet.Cells[16, 0].Text = strHcGjWrtno1;  //일반검진 접수번호
            ssPrt.ActiveSheet.Cells[17, 0].Text = strHcGjWrtno2;  //일반검진 접수번호

            Application.DoEvents();
            string strPrintName = CP.getPmpaBarCodePrinter("접수증");
            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 0, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);

            SPR.setSpdPrint(ssPrt, false, setMargin, setOption, "", "", strPrintName);

            ComFunc.Delay(1200);

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
            ssPrt.ActiveSheet.Cells[15, 0].Text = "";   //인터넷문진표 정보
            ssPrt.ActiveSheet.Cells[16, 0].Text = "";   //일반검진 접수번호
            ssPrt.ActiveSheet.Cells[17, 0].Text = "";   //일반검진 접수번호
            ssPrt.ActiveSheet.Cells[20, 0].Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();//대기표 출력일자
        }

        public bool Read_Hic_GaJepsu(string argPtno, string argYear)
        {
            try
            {
                List<HIC_JEPSU_WORK> lstHJW = hicJepsuWorkService.GetItemByPtnoYear(argPtno, argYear);
                if (lstHJW.Count > 0)
                {
                    for (int i = 0; i < lstHJW.Count; i++)
                    {
                        if (!cHMF.INSERT_HIC_JEPSU(lstHJW[i]))    //일반검진 접수
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Account_Hea_Sunap(HEA_KIOSK argKIOSK)
        {
            try
            {
                haSUNAP = new HEA_SUNAP();

                if (!Read_Sunap_Detail(argKIOSK))
                {
                    return false;
                }

                if (!Report_NewAmt_Print(argKIOSK))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Read_Sunap_Detail(HEA_KIOSK argKIOSK)
        {
            long nHcGamAmt = 0;
            long nGamAmt = 0;
            long nAmt = 0;
            long nTotAmt = 0, nLtdAmt = 0, nBoninAmt = 0, nLtdSAmt = 0;
            long nHalinAmt = 0, nIpGumAmt = 0;

            string strBuRate = string.Empty;
            string strGamGye = string.Empty;
            string strSelf = string.Empty;
            string strCode = string.Empty;

            int nLtdRate = 0, nBonRate = 0;

            try
            {
                //수납 항목을 SELECT(묶음코드)
                List<HEA_JEPSU_SUNAPDTL> lstHJSD = heaJepsuSunapdtlService.GetGamInfoByWrtno(argKIOSK.WRTNO);

                if (lstHJSD.Count > 0 && !lstHJSD.IsNullOrEmpty())
                {
                    nHcGamAmt = lstHJSD[0].GAMAMT.To<long>(0);
                    strBuRate = lstHJSD[0].BURATE.To<string>("");
                    strGamGye = lstHJSD[0].GAMCODE.To<string>("");

                    nGamAmt = 0;
                }

                for (int i = 0; i < haSUNAPDTL.Count; i++)
                {
                    if (haSUNAPDTL[i].CODE == "YY001")
                    {
                        nGamAmt = haSUNAPDTL[i].AMT;
                        break;
                    }
                }

                FstrNewExams = "";

                for (int i = 0; i < lstHJSD.Count; i++)
                {
                    nAmt = lstHJSD[i].AMT;
                    strSelf = lstHJSD[i].GBSELF;
                    strCode = lstHJSD[i].CODE;
                    FstrNewExams += strCode + ",";

                    if (strSelf == "") { strSelf = strBuRate; }

                    //감액코드는 적용대상이 아니므로 0원 처리
                    if (strCode == "YY001") { nAmt = 0; }

                    switch (strSelf)
                    {
                        case "1": nLtdRate = 0; nBonRate = 100; break;   //본인100%
                        case "2": nLtdRate = 100; nBonRate = 0; break;   //회사100%
                        case "3": nLtdRate = 50; nBonRate = 50; break;   //회사,본인50%"
                        case "4": nLtdRate = 0; nBonRate = 0; break;   //회사,본인 입부부담"
                        default: nLtdRate = 0; nBonRate = 0; break;
                    }

                    nTotAmt += nAmt;

                    if (nLtdRate != 0) { nLtdAmt += (long)Math.Truncate(nAmt * nLtdRate / 100.0); }
                    if (nBonRate != 0) { nBoninAmt += (long)Math.Truncate(nAmt * nBonRate / 100.0); }

                    if (strSelf == "4")
                    {
                        nBoninAmt += lstHJSD[i].BONINAMT;
                        nLtdAmt += lstHJSD[i].LTDAMT;
                    }
                }

                //본인,회사 입부부담
                if (strBuRate == "4")
                {
                    ////ComboBuRate.ListIndex = Val(strBuRate) - 1
                    //haSUNAP.LTDSAMT = lstHJSD[0].LTDSAMT;

                    //if (haSUNAP.LTDSAMT != 0)
                    //{
                    //    nLtdSAmt = haSUNAP.LTDSAMT;
                    //    haSUNAP.LTDAMT = haSUNAP.LTDSAMT;
                    //}

                    //nLtdAmt = nLtdSAmt;
                    //nBoninAmt = nTotAmt - nLtdAmt;

                    //haSUNAP.LTDAMT = nLtdAmt;             //회사부담금
                    //haSUNAP.BONINAMT = nBoninAmt;         //본인부담금

                    //haSUNAP.LTDAMT = nLtdAmt;             //회사부담금
                    //haSUNAP.BONINAMT = nBoninAmt;         //본인부담금
                }

                //감액코드
                if (strGamGye.Trim() != "")
                {
                    haSUNAP.HALINGYE = strGamGye;
                    Account_Process_HalinAmt(argKIOSK);

                    if (strGamGye == "942")
                    {
                        haSUNAP.HALINAMT = nHcGamAmt;
                    }
                }

                haSUNAP.TOTAMT = nTotAmt;
                haSUNAP.LTDAMT = nLtdAmt;
                haSUNAP.BONINAMT = nBoninAmt;

                FstrCommit = "";

                if (heaSunapService.GetRowidByWrtno(argKIOSK.WRTNO).IsNullOrEmpty())
                {
                    nHalinAmt = haSUNAP.BONINAMT;
                    nIpGumAmt = nTotAmt - nLtdAmt - nHalinAmt;

                    if (nIpGumAmt < 0) { nIpGumAmt = 0; }

                    haSUNAP.CHAAMT = nIpGumAmt;

                    if (haSUNAP.CHAAMT == 0)
                    {
                        FstrCommit = "OK";
                    }
                    else if (nBoninAmt > 0)
                    {
                        FstrCommit = "OK";
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void Account_Process_HalinAmt(HEA_KIOSK argKIOSK)
        {
            string strGamCode = string.Empty;
            string strOK = string.Empty;
            string strCode = string.Empty;
            string strHalin = string.Empty;
            string strGbSelect = string.Empty;

            long nAmt = 0;
            long nGamAmt = 0;

            int nRate = 0;

            long nHalAmt_M = 0;
            long nHalAmt_F = 0;

            try
            {
                strGamCode = haSUNAP.HALINGYE;

                for (int i = 0; i < haSUNAPDTL.Count; i++)
                {
                    strCode = haSUNAPDTL[i].CODE;
                    nAmt = haSUNAPDTL[i].AMT;

                    if (strCode == "YY001")
                    {
                        nGamAmt = nAmt;
                        break;
                    }
                }

                if (strGamCode == "")
                {
                    haSUNAP.HALINAMT = nGamAmt;
                    return;
                }

                haSUNAP.HALINAMT = 0;

                //감액조회
                HEA_GAMCODE iHGAM = heaGamcodeService.GetItemByCode(strGamCode);
                if (!iHGAM.IsNullOrEmpty())
                {
                    nRate = (int)iHGAM.RATE;
                    nHalAmt_M = iHGAM.AMT1;
                    nHalAmt_F = iHGAM.AMT2;
                }

                for (int i = 0; i < haSUNAPDTL.Count; i++)
                {
                    strCode = haSUNAPDTL[i].CODE;
                    nAmt = haSUNAPDTL[i].AMT;
                    strHalin = haSUNAPDTL[i].GBHALIN;

                    //감액코드 적용전 기본코드에만 적용시키기로 함
                    strGbSelect = "";
                    HEA_GROUPCODE iHGRPCD = heaGroupcodeService.GetItemByCode(strCode);
                    if (iHGRPCD.IsNullOrEmpty())
                    {
                        strGbSelect = iHGRPCD.GBSELECT;
                    }

                    //기본코드 금액에서 감액코드 금액을 뺌
                    if (strGbSelect != "Y")
                    {
                        nAmt = nAmt - nGamAmt;
                    }

                    if (strCode != "YY001")
                    {
                        if (strHalin == "1")
                        {
                            if (nRate > 0)
                            {
                                haSUNAP.HALINAMT = haSUNAP.HALINAMT + (long)Math.Truncate(nAmt * (nRate / 100.0));
                            }
                            else
                            {
                                if (argKIOSK.SEX == "M")
                                {
                                    haSUNAP.HALINAMT += nHalAmt_M;
                                }
                                else
                                {
                                    haSUNAP.HALINAMT += nHalAmt_F;
                                }
                            }
                        }
                    }
                    else
                    {
                        nGamAmt = nAmt;
                    }
                }

                haSUNAP.HALINAMT = haSUNAP.HALINAMT + nGamAmt;      //할인금액(YY001)

                haSUNAP.CHAAMT = haSUNAP.BONINAMT - haSUNAP.HALINAMT;

                if (haSUNAP.CHAAMT < 0)
                {
                    haSUNAP.CHAAMT = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool Report_NewAmt_Print(HEA_KIOSK argKIOSK)
        {
            try
            {
                if (FstrCommit == "OK")
                {
                    haSUNAP.WRTNO = argKIOSK.WRTNO;
                    haSUNAP.PANO = argKIOSK.PANO;
                    haSUNAP.SEQNO = 1;
                    haSUNAP.EXAMCODES = FstrNewExams;
                    haSUNAP.JOBSABUN = 222;
                    haSUNAP.CARDSEQNO = 0;

                    //수납영수증 상세내역
                    if (!heaSunapService.Insert(haSUNAP))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void Read_SunapDTL(long nWRTNO)
        {
            haSUNAPDTL.Clear();

            haSUNAPDTL = heaSunapdtlService.GetListByWRTNO(nWRTNO);
        }
    }
}
