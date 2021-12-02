using ComBase;
using ComDbB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using ComMedLibB;
using ComLibB;
using SupXray;
using System;
using System.Data;
using System.Windows.Forms;

namespace HC_Main
{
    public partial class frmMDI_Main : Form, MainFormMessage
    {
        string mPara1 = "";

        frmHcJepMain frmHCMain = null;
        frmHaJepMain frmHaMain = null;
        frmHcJepsuView frmHJView = null;
        frmHaJepsuView frmHaJView = null;
        frmHcAllGaJepsu FrmHcAllGaJepsu = null;
        frmHcNewPatient FrmHcNewPatient = null;
        frmHcNightMunjin FrmHcNightMunjin = null;
        frmHcLiverCMunjin FrmHcLiverCMunjin = null;
        frmHcEmrConset_Rec FrmHcEmrConset_Rec = null;
        frmHcSecondList FrmHcSecondList = null;
        ComHpcLibB.frmHcPanPersonResult FrmHcPanPersonResult = null;
        frmHcAmReserve FrmHcAmReserve = null;
        frmHcSchedule FrmHcSchedule = null;
        frmHcCardTransView FrmHcCardTransView = null;
        frmHcGaJepsuVIew FrmHcGaJepsuVIew = null;
        frmHcGaJepsuVIew_Print FrmHcGaJepsuVIew_Print = null;
        frmHcResultView FrmHcResultView = null;
        frmHyangApprove_List FrmHyangApprove_List = null;
        frmHcJochiList FrmHcJochiList = null;
        frmWaitSeqReg FrmWaitSeqReg = null;
        frmHcResultInputCheckList FrmHcResultInputCheckList = null;
        frmAmJepsu_List FrmAmJepsu_List = null;
        frmHcCharttrans_Insert FrmHcCharttrans_Insert = null;
        frmHcDoubleChartArrangement FrmHcDoubleChartArrangement = null;
        frmHcDoubleChartClear FrmHcDoubleChartClear = null;
        frmHcIEMunjin FrmHcIEMunjin = null;
        frmHcIEMunjinVIew FrmHcIEMunjinVIew = null;
        frmHcEntryCardDaou FrmHcEntryCardDaou = null;
        frmHcMstCardDaou FrmHcMstCardDaou = null;
        frmHcAlimTaliSendView FrmHcAlimTaliSendView = null;
        frmHcWaitList FrmHcWaitList = null;
        frmHcGaJepsuIntMunjinYN FrmHcGaJepsuIntMunjinYN = null;
        frmHcOutPacsOrdCreate FrmHcOutPacsOrdCreate = null;
        frmHcLtdCode FrmHcLtdCode = null;
        frmHaCheckList FrmHaCheckList = null;
        frmHcScheduleEntry FrmHcScheduleEntry = null;
        frmHcMemo frmHcMemo = null;
        frmRegisteredMailFileCreate frmRegMailFile = null;
        frmHcEmrConsentView frmHcEmrConsentView = null;
        frmHaResvCalendar frmHaRsvCal = null;
        frmHaResvExamInwon frmHaRsvExInwon = null;
        frmHaAddExamNote frmHaAddNote = null;
        frmHcExcelList frmHcExcelLst = null;
        frmHaExcelImport frmHaExcelImport = null;
        frmMirErrorList FrmMirErrorList = null;
        frmSupXrayVIEW12 FrmSupXrayVIEW12 = null;
        frmHaTimeInwon frmHaTimeInwon = null;
        frmHcPanJindanSet FrmHcPanJindanSet = null;


        clsHcFunc cHF = null;
        INIFile INI = new INIFile();
        clsSpread sp = new clsSpread();

        HIC_PATIENT Hpatient = null;
        HicWaitService hicWaitService = null;

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

        public frmMDI_Main()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmMDI_Main(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmMDI_Main(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cHF = new clsHcFunc();
            frmHCMain = new frmHcJepMain();
            frmHaMain = new frmHaJepMain();
            frmHJView = new frmHcJepsuView();
            frmHaJView = new frmHaJepsuView();
            frmHaRsvCal = new frmHaResvCalendar();
            frmHaRsvExInwon = new frmHaResvExamInwon();
            frmHaAddNote = new frmHaAddExamNote();
            hicWaitService = new HicWaitService();
            frmHcExcelLst = new frmHcExcelList();
            frmHaTimeInwon = new frmHaTimeInwon();
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

        private void SetEvent()
        {
            this.Load                += new EventHandler(eFormLoad);
            this.Activated           += new EventHandler(eFormActivated);
            this.FormClosed          += new FormClosedEventHandler(eFormClosed);
            this.FormClosing         += new FormClosingEventHandler(eFormClosing);
            //접수업무               
            this.menuJep01.Click     += new EventHandler(eMenuClick);    //검진접수
            this.menuJep02.Click     += new EventHandler(eMenuClick);    //일괄가접수
            this.menuJep03.Click     += new EventHandler(eMenuClick);    //신규수검자 번호생성
            //this.menuJep04.Click     += new EventHandler(eMenuClick);    //생활습관 도구표
            this.menuJep05.Click     += new EventHandler(eMenuClick);    //야간문진표 작성
            this.menuJep06.Click     += new EventHandler(eMenuClick);    //C형간염문진표 작성
            this.menuJep07.Click     += new EventHandler(eMenuClick);    //종검접수
            this.menuJep08.Click     += new EventHandler(eMenuClick);    //전자동의서

            //수검자관리             
            this.menuPat01.Click     += new EventHandler(eMenuClick);    //수검자 인적사항 수정
            this.menuPat02.Click     += new EventHandler(eMenuClick);    //2차검진 명단작성
            this.menuPat03.Click     += new EventHandler(eMenuClick);    //검진자 개인 HISTORY
            this.menuPat04.Click     += new EventHandler(eMenuClick);    //수검자 메모
            //예약관리               
            this.menuResv01.Click    += new EventHandler(eMenuClick);    //암검진 예약등록
            this.menuResv02.Click    += new EventHandler(eMenuClick);    //검진 예약스케쥴 조회
            this.menuResv03.Click    += new EventHandler(eMenuClick);    //종검 예약달력
            this.menuResv04.Click    += new EventHandler(eMenuClick);    //종검 회사별 가예약 관리
            this.menuResv05.Click    += new EventHandler(eMenuClick);    //종검 회사별 계약코드 관리
            this.menuResv06.Click    += new EventHandler(eMenuClick);    //종검 일자별 정원 현황
            this.menuResv07.Click    += new EventHandler(eMenuClick);    //종검 추가검사 관리 노트
            this.menuResv08.Click    += new EventHandler(eMenuClick);    //종검예약 Excel 명단 조회
            this.menuResv09.Click    += new EventHandler(eMenuClick);    //종검예약 Excel 명단 저장
            this.menuResv10.Click    += new EventHandler(eMenuClick);    //출장검진 예약등록
            this.menuResv11.Click    += new EventHandler(eMenuClick);    //종검 시간별 정원 현황
            //접수/수납조회          
            this.menuJSView01.Click  += new EventHandler(eMenuClick);    //검진비 수납집계표
            this.menuJSView02.Click  += new EventHandler(eMenuClick);    //카드 거래내역 조회
            //this.menuJSView03.Click  += new EventHandler(eMenuClick);    //카드사별 수입집계표
            this.menuJSView04.Click  += new EventHandler(eMenuClick);    //검진 접수명단
            this.menuJSView05.Click  += new EventHandler(eMenuClick);    //종검 수검자 관리
            this.menuJSView06.Click  += new EventHandler(eMenuClick);    //가접수명단조회
            this.menuJSView07.Click += new EventHandler(eMenuClick);    //가접수명단조회

            //기타조회
            this.menuEtcView01.Click += new EventHandler(eMenuClick);   //개인별 검사결과 조회
            this.menuEtcView02.Click += new EventHandler(eMenuClick);   //향정처방 조회
            //this.menuEtcView03.Click += new EventHandler(eMenuClick);   //직원가족 성인병 검진대상자 조회
            //this.menuEtcView04.Click += new EventHandler(eMenuClick);   //암종류상세조회
            this.menuEtcView05.Click += new EventHandler(eMenuClick);   //조치대상자조회
            //this.menuEtcView06.Click += new EventHandler(eMenuClick);   //상담대기순번조회
            this.menuEtcView07.Click += new EventHandler(eMenuClick);   //암접수자 조회
            this.menuEtcView08.Click += new EventHandler(eMenuClick);   //등기우편 명단
            this.menuEtcView09.Click += new EventHandler(eMenuClick);   //전자동의서 조회
            this.menuEtcView10.Click += new EventHandler(eMenuClick);   //MRI예약 조회
            this.menuEtcView11.Click += new EventHandler(eMenuClick);   //건강진단구분변경

            //기타업무
            //this.menuETC01.Click     += new EventHandler(eMenuClick);    //문진표인쇄
            this.menuETC02.Click     += new EventHandler(eMenuClick);    //검사결과 입력 체크리스트
            this.menuETC03.Click     += new EventHandler(eMenuClick);    //종합검진 수검자 체크리스트
            this.menuETC04.Click     += new EventHandler(eMenuClick);    //알림톡전송내역
            this.menuETC05.Click     += new EventHandler(eMenuClick);    //챠트인계 등록
            this.menuETC06.Click += new EventHandler(eMenuClick);    //챠트인계 등록
            //this.menuETC06.Click     += new EventHandler(eMenuClick);    //2차검진명단작성
            this.menuETC07.Click     += new EventHandler(eMenuClick);    //일반건진 이중챠트 정리작업
            //this.menuETC08.Click     += new EventHandler(eMenuClick);    //건진대상자 SMS 발송
            //this.menuETC09.Click     += new EventHandler(eMenuClick);    //카드승인방법 Set
            this.menuETC10.Click     += new EventHandler(eMenuClick);    //인터넷문진표 연계
            this.menuETC11.Click += new EventHandler(eMenuClick);       //가접수명단 문진표 조회
            this.menuETC12.Click += new EventHandler(eMenuClick);       //출장검진PACS오더생성
            this.menuETC13.Click += new EventHandler(eMenuClick);       //회사코드
            this.menuETC14.Click += new EventHandler(eMenuClick);       //공단청구오류수정의뢰
            //결제등록
            this.menuPayMent01.Click += new EventHandler(eMenuClick);    //수동카드
            //this.menuPayMent02.Click += new EventHandler(eMenuClick);    //현금영수증
            this.menuPayMent03.Click += new EventHandler(eMenuClick);    //카드등록
            
            //종료
            this.menuExit.Click     += new EventHandler(eMenuClick);
            //this.menuTest.Click += new EventHandler(eMenuClick);

            frmHcJepMain.rSetJepsuView += new frmHcJepMain.SetJepsuView(eJepMainView);
            frmHaJepMain.rSetJepsuView += new frmHaJepMain.SetJepsuView(eHaJepMainView);
            ComHpcLibB.frmHcPanPersonResult.rSetDelegateJep += new ComHpcLibB.frmHcPanPersonResult.SetDelegateJepsuForm(eDelegateJepForm);
            frmHaResvCalendar.rSetExInwonView += new frmHaResvCalendar.SetExInwonView(eHaExInwonFormView);
        }

        private void eHaExInwonFormView(object sender, EventArgs e)
        {
            if (frmHaRsvExInwon == null)
            {
                frmHaResvExamInwon frmHaRsvExInwon = new frmHaResvExamInwon();
                frmHaRsvExInwon.StartPosition = FormStartPosition.CenterScreen;
                frmHaRsvExInwon.Show();
            }
            else
            {
                FormVisiable(frmHaRsvExInwon);
            }
        }

        private void eDelegateJepForm(object sender, EventArgs e)
        {
            if (((Control)sender).Name == "btnRef1")
            {
                eMenuClick(menuJep01, e);
            }
            else if (((Control)sender).Name == "btnRef2")
            {
                eMenuClick(menuJep07, e);
            }
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {
            frmHcJepMain.rSetJepsuView -= new frmHcJepMain.SetJepsuView(eJepMainView);
            frmHaJepMain.rSetJepsuView -= new frmHaJepMain.SetJepsuView(eHaJepMainView);
            ComHpcLibB.frmHcPanPersonResult.rSetDelegateJep -= new ComHpcLibB.frmHcPanPersonResult.SetDelegateJepsuForm(eDelegateJepForm);
            frmHaResvCalendar.rSetExInwonView -= new frmHaResvCalendar.SetExInwonView(eHaExInwonFormView);
            frmHCMain.eDisevent(null, null);
            frmHaMain.eDisevent(null, null);
        }

        private void eHaJepMainView(object sender, EventArgs e)
        {
            if (frmHaJView == null)
            {
                frmHaJepsuView frmHaJView = new frmHaJepsuView();
                frmHaJView.StartPosition = FormStartPosition.CenterScreen;
                frmHaJView.ShowDialog();
                cHF.fn_ClearMemory(frmHaJView);
            }
            else
            {
                FormVisiable(frmHaJView);
            }
        }

        private void eJepMainView(object sender, EventArgs e)
        {
            if (frmHJView == null)
            {
                frmHcJepsuView frmHJView = new frmHcJepsuView();
                frmHJView.StartPosition = FormStartPosition.CenterScreen;
                frmHJView.ShowDialog();
                cHF.fn_ClearMemory(frmHJView);
            }
            else
            {
                FormVisiable(frmHJView);
            }
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)
            {
                this.Close();
                return;
            }
            //접수업무
            else if (sender == menuJep01)   //검진접수
            {
                if (frmHCMain == null)
                {
                    themTabForm(frmHCMain, this.panMain);
                }
                else
                {
                    if (FormIsExist(frmHCMain) == true)
                    {
                        FormVisiable(frmHCMain);
                    }
                    else
                    {
                        frmHCMain = new frmHcJepMain();
                        themTabForm(frmHCMain, this.panMain);
                    }
                }
            }
            else if (sender == menuJep07)   //종검접수
            {
                if (frmHaMain == null)
                {
                    themTabForm(frmHaMain, this.panMain);
                }
                else
                {
                    if (FormIsExist(frmHaMain) == true)
                    {
                        FormVisiable(frmHaMain);
                    }
                    else
                    {
                        frmHaMain = new frmHaJepMain();
                        themTabForm(frmHaMain, this.panMain);
                    }
                }
            }
            
            
            else if (sender == menuJep02)   //일괄가접수
            {
                if (cHF.OpenForm_Check("frmHcAllGaJepsu") == true)
                {
                    return;
                }

                FrmHcAllGaJepsu = new frmHcAllGaJepsu();
                FrmHcAllGaJepsu.StartPosition = FormStartPosition.CenterScreen;
                FrmHcAllGaJepsu.Show();
                //cHF.fn_ClearMemory(FrmHcAllGaJepsu);
            }
            else if (sender == menuJep03)   //신규수검자 번호생성
            {
                if (cHF.OpenForm_Check("frmHcNewPatient") == true)
                {
                    return;
                }

                FrmHcNewPatient = new frmHcNewPatient();
                FrmHcNewPatient.ShowDialog();
                cHF.fn_ClearMemory(FrmHcNewPatient);
            }
            else if (sender == menuJep05)   //야간문진표 작성
            {
                if (cHF.OpenForm_Check("frmHcNightMunjin") == true)
                {
                    return;
                }

                FrmHcNightMunjin = new frmHcNightMunjin();
                FrmHcNightMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcNightMunjin.ShowDialog();
                cHF.fn_ClearMemory(FrmHcNightMunjin);
            }

            else if (sender == menuJep06)   //C형간염문진표 작성(2020-09-07 신규추가)
            {
                if (cHF.OpenForm_Check("frmHcLiverCMunjin") == true)
                {
                    return;
                }

                FrmHcLiverCMunjin = new frmHcLiverCMunjin();
                FrmHcLiverCMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcLiverCMunjin.ShowDialog();
                cHF.fn_ClearMemory(FrmHcLiverCMunjin);
            }

            else if (sender == menuJep08)   //전자동의서
            {
                if (cHF.OpenForm_Check("frmHcEmrConset_Rec") == true)
                {
                    return;
                }

                FrmHcEmrConset_Rec = new frmHcEmrConset_Rec(0,"NUR");
                FrmHcEmrConset_Rec.StartPosition = FormStartPosition.CenterScreen;
                FrmHcEmrConset_Rec.ShowDialog();
                cHF.fn_ClearMemory(FrmHcEmrConset_Rec);
            }

            //수검자관리
            else if (sender == menuPat01)   //수검자 인적사항 관리
            {
                if (cHF.OpenForm_Check("frmHcPatientModify") == true)
                {
                    return;
                }

                frmHcPatientModify frm = new frmHcPatientModify();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == menuPat02)   //2차검진 명단작성
            {
                if (cHF.OpenForm_Check("frmHcSecondList") == true)
                {
                    return;
                }

                FrmHcSecondList = new frmHcSecondList();
                FrmHcSecondList.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSecondList.ShowDialog();
                cHF.fn_ClearMemory(FrmHcSecondList);
            }
            else if (sender == menuPat03)   //검진자 개인 HISTORY
            {
                if (cHF.OpenForm_Check("frmHcPanPersonResult") == true)
                {
                    return;
                }

                FrmHcPanPersonResult = new ComHpcLibB.frmHcPanPersonResult("frmHcJepMain");
                FrmHcPanPersonResult.StartPosition = FormStartPosition.CenterScreen;
                FrmHcPanPersonResult.ShowDialog();
                cHF.fn_ClearMemory(FrmHcPanPersonResult);
            }
            else if (sender == menuPat04)   //수검자 메모
            {
                if (cHF.OpenForm_Check("frmHcMemo") == true)
                {
                    return;
                }

                frmHcMemo = new frmHcMemo();
                frmHcMemo.StartPosition = FormStartPosition.CenterScreen;
                frmHcMemo.ShowDialog();
                cHF.fn_ClearMemory(frmHcMemo);
            }
            //예약관리
            else if (sender == menuResv01)  //암검진 예약등록
            {
                if (cHF.OpenForm_Check("frmHcAmReserve") == true)
                {
                    return;
                }

                FrmHcAmReserve = new frmHcAmReserve();
                FrmHcAmReserve.StartPosition = FormStartPosition.CenterScreen;                
                FrmHcAmReserve.Show();
            }
            else if (sender == menuResv10)  //출장검진 예약등록
            {
                if (cHF.OpenForm_Check("frmHcScheduleEntry") == true)
                {
                    return;
                }

                FrmHcScheduleEntry = new frmHcScheduleEntry(DateTime.Now.ToShortDateString(), "1");
                FrmHcScheduleEntry.StartPosition = FormStartPosition.CenterScreen;
                FrmHcScheduleEntry.ShowDialog();
                cHF.fn_ClearMemory(FrmHcScheduleEntry);
            }
            else if (sender == menuResv02)  //검진 예약스케쥴 조회
            {
                if (cHF.OpenForm_Check("frmHcSchedule") == true)
                {
                    return;
                }

                FrmHcSchedule = new frmHcSchedule();
                FrmHcSchedule.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchedule.ShowDialog();
                cHF.fn_ClearMemory(FrmHcSchedule);
            }
            else if (sender == menuResv03)  //종검예약달력
            {
                if (frmHaRsvCal == null)
                {
                    frmHaResvCalendar frmHaRsvCal = new frmHaResvCalendar();
                    frmHaRsvCal.StartPosition = FormStartPosition.CenterScreen;
                    frmHaRsvCal.ShowDialog();
                    cHF.fn_ClearMemory(frmHaRsvCal);
                }
                else
                {
                    FormVisiable(frmHaRsvCal);
                }
            }
            else if (sender == menuResv04)  //종검 회사별 가예약 관리
            {
                if (cHF.OpenForm_Check("frmHaLtdGaResv") == true)
                {
                    return;
                }

                frmHaLtdGaResv frm = new frmHaLtdGaResv();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == menuResv05)  //종검 회사별 계약코드 관리
            {
                if (cHF.OpenForm_Check("frmHaLtdExamCompare") == true)
                {
                    return;
                }

                frmHaLtdExamCompare frm = new frmHaLtdExamCompare();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == menuResv06)  //종검 일자별 인원 현황
            {
                if (frmHaRsvExInwon == null)
                {
                    frmHaResvExamInwon frmHaRsvExInwon = new frmHaResvExamInwon();
                    frmHaRsvExInwon.StartPosition = FormStartPosition.CenterScreen;
                    frmHaRsvExInwon.Show();
                }
                else
                {
                    FormVisiable(frmHaRsvExInwon);
                }
            }
            else if (sender == menuResv07)  //추가검사 관리노트
            {
                if (cHF.OpenForm_Check("frmHaAddExamNote") == true)
                {
                    FormVisiable(frmHaAddNote);
                    return;
                }

                frmHaAddExamNote frm = new frmHaAddExamNote();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == menuResv08)  //엑셀명단 조회
            {
                if (frmHcExcelLst == null)
                {
                    frmHcExcelList frmHcExcelLst = new frmHcExcelList();
                    frmHcExcelLst.StartPosition = FormStartPosition.CenterScreen;
                    frmHcExcelLst.Show();
                }
                else
                {
                    FormVisiable(frmHcExcelLst);
                }
            }
            else if (sender == menuResv09)   //엑셀명단 저장
            {
                if (cHF.OpenForm_Check("frmHaExcelImport") == true)
                {
                    return;
                }

                frmHaExcelImport = new frmHaExcelImport();
                frmHaExcelImport.StartPosition = FormStartPosition.CenterScreen;
                frmHaExcelImport.ShowDialog();
                cHF.fn_ClearMemory(frmHaExcelImport);

            }

            else if (sender == menuResv11)  //종검 시간별 인원 현황
            {
                if (frmHaTimeInwon == null)
                {
                    frmHaTimeInwon = new frmHaTimeInwon();
                    frmHaTimeInwon.StartPosition = FormStartPosition.CenterScreen;
                    frmHaTimeInwon.Show();
                }
                else
                {
                    FormVisiable(frmHaTimeInwon);
                }
            }
            //접수/수납조회         
            else if (sender == menuJSView01)    //검진비 수납집계표
            {
                if (cHF.OpenForm_Check("frmHcSunapList") == true)
                {
                    return;
                }

                frmHcSunapList frm = new frmHcSunapList();
                //frm.ShowDialog();
                frm.Show();
                //cHF.fn_ClearMemory(frm);
            }
            else if (sender == menuJSView02)    //카드 거래내역 조회
            {
                if (cHF.OpenForm_Check("frmHcCardTransView") == true)
                {
                    return;
                }

                FrmHcCardTransView = new frmHcCardTransView();
                FrmHcCardTransView.StartPosition = FormStartPosition.CenterScreen;
                //FrmHcCardTransView.ShowDialog();
                FrmHcCardTransView.Show();
                //cHF.fn_ClearMemory(FrmHcCardTransView);
            }
            else if (sender == menuJSView04)    //검진 접수자 명단
            {
                if (frmHJView == null)
                {
                    frmHcJepsuView frmHJView = new frmHcJepsuView();
                    frmHJView.StartPosition = FormStartPosition.CenterScreen;
                    frmHJView.ShowDialog();
                    cHF.fn_ClearMemory(frmHJView);
                }
                else
                {
                    FormVisiable(frmHJView);
                }

            }            
            else if (sender == menuJSView05)    //종검 접수(예약) 명단
            {
                if (frmHaJView == null)
                {
                    frmHaJepsuView frmHaJView = new frmHaJepsuView();
                    frmHaJView.StartPosition = FormStartPosition.CenterScreen;
                    frmHaJView.ShowDialog();
                    cHF.fn_ClearMemory(frmHaJView);
                }
                else
                {
                    FormVisiable(frmHaJView);
                }
            }
            else if (sender == menuJSView06)    //가접수명단조회
            {
                if (cHF.OpenForm_Check("frmHcGaJepsuVIew") == true)
                {
                    return;
                }

                FrmHcGaJepsuVIew = new frmHcGaJepsuVIew();
                FrmHcGaJepsuVIew.StartPosition = FormStartPosition.CenterScreen;
                FrmHcGaJepsuVIew.ShowDialog();
                cHF.fn_ClearMemory(FrmHcGaJepsuVIew);
            }
            else if (sender == menuJSView07)    //가접수명단조회
            {
                if (cHF.OpenForm_Check("frmHcGaJepsuVIew_Print") == true)
                {
                    return;
                }

                FrmHcGaJepsuVIew_Print = new frmHcGaJepsuVIew_Print();
                FrmHcGaJepsuVIew_Print.StartPosition = FormStartPosition.CenterScreen;
                FrmHcGaJepsuVIew_Print.ShowDialog();
                cHF.fn_ClearMemory(FrmHcGaJepsuVIew_Print);
            }
            //기타조회   
            else if (sender == menuEtcView01)   //개인별 검사결과 조회
            {
                if (cHF.OpenForm_Check("frmHcResultView") == true)
                {
                    return;
                }

                FrmHcResultView = new frmHcResultView("HIC", 0);
                FrmHcResultView.StartPosition = FormStartPosition.CenterScreen;
                FrmHcResultView.ShowDialog();
                cHF.fn_ClearMemory(FrmHcResultView);
            }
            else if (sender == menuEtcView02)   //향정처방 조회
            {
                if (cHF.OpenForm_Check("frmHyangApprove_List") == true)
                {
                    return;
                }

                FrmHyangApprove_List = new frmHyangApprove_List();
                FrmHyangApprove_List.StartPosition = FormStartPosition.CenterScreen;
                FrmHyangApprove_List.ShowDialog();
                cHF.fn_ClearMemory(FrmHyangApprove_List);
            }
            else if (sender == menuEtcView05)   //조치대상자조회
            {
                if (FrmHcJochiList == null)
                {
                    frmHcJochiList FrmHcJochiList = new frmHcJochiList();
                    FrmHcJochiList.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcJochiList.Show();
                    //cHF.fn_ClearMemory(FrmHcJochiList);
                }
                else
                {
                    FormVisiable(FrmHcJochiList);
                }
            }
            else if (sender == menuEtcView07)   //암접수자 조회
            {
                if (cHF.OpenForm_Check("frmAmJepsu_List") == true)
                {
                    return;
                }

                FrmAmJepsu_List = new frmAmJepsu_List();
                FrmAmJepsu_List.StartPosition = FormStartPosition.CenterScreen;
                FrmAmJepsu_List.ShowDialog();
                cHF.fn_ClearMemory(FrmAmJepsu_List);
            }
            else if (sender == menuEtcView08)   //등기우편명단
            {
                if (cHF.OpenForm_Check("frmRegisteredMailFileCreate") == true)
                {
                    return;
                }

                frmRegMailFile = new frmRegisteredMailFileCreate();
                frmRegMailFile.StartPosition = FormStartPosition.CenterScreen;
                frmRegMailFile.ShowDialog();
                cHF.fn_ClearMemory(frmRegMailFile);
            }
            else if (sender == menuEtcView09)   //전자동의서 조회
            {
                if (cHF.OpenForm_Check("frmHcConsentformView") == true)
                {
                    return;
                }

                frmHcEmrConsentView = new frmHcEmrConsentView();
                frmHcEmrConsentView.StartPosition = FormStartPosition.CenterScreen;
                frmHcEmrConsentView.ShowDialog();
                cHF.fn_ClearMemory(frmHcEmrConsentView);
            }
            else if (sender == menuEtcView10)
            {
                FrmSupXrayVIEW12 = new frmSupXrayVIEW12();
                FrmSupXrayVIEW12.StartPosition = FormStartPosition.CenterScreen;
                FrmSupXrayVIEW12.ShowDialog();
                cHF.fn_ClearMemory(FrmSupXrayVIEW12);
            }

            else if (sender == menuEtcView11)
            {
                FrmHcPanJindanSet = new frmHcPanJindanSet();
                FrmHcPanJindanSet.StartPosition = FormStartPosition.CenterScreen;
                FrmHcPanJindanSet.ShowDialog();
                cHF.fn_ClearMemory(FrmHcPanJindanSet);
            }

            else if (sender == menuETC02)   //검사결과 입력 체크리스트
            {
                if (FrmHcResultInputCheckList == null)
                {
                    frmHcResultInputCheckList FrmHcResultInputCheckList = new frmHcResultInputCheckList();
                    FrmHcResultInputCheckList.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcResultInputCheckList.Show();
                }
                else
                {
                    FormVisiable(FrmHcResultInputCheckList);
                }
            }
            else if (sender == menuETC03)    //종합검진 수검자 체크리스트
            {
                if (FrmHaCheckList == null)
                {
                    frmHaCheckList FrmHaCheckList = new frmHaCheckList();
                    FrmHaCheckList.StartPosition = FormStartPosition.CenterScreen;
                    FrmHaCheckList.Show();
                }
                else
                {
                    FormVisiable(FrmHaCheckList);
                }

            }
            else if (sender == menuETC04)   //알림톡전송내역
            {
                if (cHF.OpenForm_Check("frmHcAlimTaliSendView") == true)
                {
                    return;
                }

                FrmHcAlimTaliSendView = new frmHcAlimTaliSendView();
                FrmHcAlimTaliSendView.StartPosition = FormStartPosition.CenterScreen;
                FrmHcAlimTaliSendView.ShowDialog();
                cHF.fn_ClearMemory(FrmHcAlimTaliSendView);
            }
            else if (sender == menuETC05)   //챠트인계 등록
            {
                if (cHF.OpenForm_Check("frmHcCharttrans_Insert") == true)
                {
                    return;
                }

                FrmHcCharttrans_Insert = new frmHcCharttrans_Insert();
                FrmHcCharttrans_Insert.StartPosition = FormStartPosition.CenterScreen;
                FrmHcCharttrans_Insert.ShowDialog();
                cHF.fn_ClearMemory(FrmHcCharttrans_Insert);
            }
            else if (sender == menuETC06)   //등록번호 이중챠트 수동정리
            {

                if (cHF.OpenForm_Check("frmHcDoubleChartClear") == true)
                {
                    return;
                }

                FrmHcDoubleChartClear = new frmHcDoubleChartClear();
                FrmHcDoubleChartClear.StartPosition = FormStartPosition.CenterScreen;
                FrmHcDoubleChartClear.ShowDialog();
                cHF.fn_ClearMemory(FrmHcDoubleChartClear);
            }
            else if (sender == menuETC07)   //일반건진 이중챠트 정리작업
            {
                if (cHF.OpenForm_Check("frmHcDoubleChartArrangement") == true)
                {
                    return;
                }

                FrmHcDoubleChartArrangement = new frmHcDoubleChartArrangement();
                FrmHcDoubleChartArrangement.StartPosition = FormStartPosition.CenterScreen;
                FrmHcDoubleChartArrangement.ShowDialog();
                cHF.fn_ClearMemory(FrmHcDoubleChartArrangement);
            }
            else if (sender == menuETC10)   //인터넷문진표 연계
            {
                if (cHF.OpenForm_Check("frmHcIEMunjin") == true)
                {
                    return;
                }

                FrmHcIEMunjin = new frmHcIEMunjin();
                FrmHcIEMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcIEMunjin.ShowDialog();
                cHF.fn_ClearMemory(FrmHcIEMunjin);
            }
            else if (sender == menuETC11)   //가접수명단 문진표 조회
            {
                if (cHF.OpenForm_Check("frmHcGaJepsuIntMunjinYN") == true)
                {
                    return;
                }

                FrmHcGaJepsuIntMunjinYN = new frmHcGaJepsuIntMunjinYN();
                FrmHcGaJepsuIntMunjinYN.StartPosition = FormStartPosition.CenterScreen;
                FrmHcGaJepsuIntMunjinYN.ShowDialog();
                cHF.fn_ClearMemory(FrmHcGaJepsuIntMunjinYN);
            }
            else if (sender == menuETC12)   //출장검진PACS오더생성
            {
                if (cHF.OpenForm_Check("frmHcOutPacsOrdCreate") == true)
                {
                    return;
                }

                FrmHcOutPacsOrdCreate = new frmHcOutPacsOrdCreate();
                FrmHcOutPacsOrdCreate.StartPosition = FormStartPosition.CenterScreen;
                FrmHcOutPacsOrdCreate.ShowDialog();
                cHF.fn_ClearMemory(FrmHcOutPacsOrdCreate);
            }

            else if (sender == menuETC13) //회사코드
            {
                if (cHF.OpenForm_Check("frmHcLtdCode") == true)
                {
                    return;
                }

                FrmHcLtdCode = new frmHcLtdCode();
                FrmHcLtdCode.StartPosition = FormStartPosition.CenterScreen;
                FrmHcLtdCode.ShowDialog();
                cHF.fn_ClearMemory(FrmHcLtdCode);
            }
            else if (sender == menuETC14) //공단청구오류수정의뢰
            {
                if (cHF.OpenForm_Check("FrmMirErrorList") == true)
                {
                    return;
                }

                FrmMirErrorList = new frmMirErrorList("frmHcJepMain");
                FrmMirErrorList.StartPosition = FormStartPosition.CenterScreen;
                FrmMirErrorList.Show();
                //cHF.fn_ClearMemory(FrmMirErrorList);
            }

            //결제등록
            else if (sender == menuPayMent01)   //카드/현금영수증 승인/취소
            {
                if (cHF.OpenForm_Check("frmHcEntryCardDaou") == true)
                {
                    return;
                }

                FrmHcEntryCardDaou = new frmHcEntryCardDaou();
                FrmHcEntryCardDaou.StartPosition = FormStartPosition.CenterScreen;
                FrmHcEntryCardDaou.ShowDialog();
                cHF.fn_ClearMemory(FrmHcEntryCardDaou);
            }
            //else if (sender == menuPayMent02)   //현금영수증 => 사용무
            //{

            //}
            else if (sender == menuPayMent03)   //카드등록
            {
                if (cHF.OpenForm_Check("frmHcMstCardDaou") == true)
                {
                    return;
                }

                FrmHcMstCardDaou = new frmHcMstCardDaou();
                FrmHcMstCardDaou.StartPosition = FormStartPosition.CenterScreen;
                FrmHcMstCardDaou.ShowDialog();
                cHF.fn_ClearMemory(FrmHcMstCardDaou);
            }
            
            //else if (sender == menuChul03)  //출장검진용 가접수 DB파일 생성 => 사용무
            //{

            //}
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

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            cHF.SET_자료사전_VALUE();
            clsDur.Set_HiraDur();

            if (frmHCMain == null)
            {
                frmHCMain = new frmHcJepMain();
                themTabForm(frmHCMain, this.panMain);
            }
            else
            {
                if (FormIsExist(frmHCMain) == true)
                {
                    FormVisiable(frmHCMain);
                }
                else
                {
                    if (frmHCMain == null)
                    {
                        frmHCMain = new frmHcJepMain();
                    }
                    themTabForm(frmHCMain, this.panMain);
                }
            }

            if (frmHaMain == null)
            {
                frmHaMain = new frmHaJepMain();
                themTabForm(frmHaMain, this.panMain);
            }
            else
            {
                if (FormIsExist(frmHaMain) == true)
                {
                    FormVisiable(frmHaMain);
                }
                else
                {
                    if (frmHaMain == null)
                    {
                        frmHaMain = new frmHaJepMain();
                    }
                    themTabForm(frmHaMain, this.panMain);
                }
            }  
        }

        /// <summary>
        /// Description : MDI내 폼 활성화시 기본값 세팅
        /// Author : 김민철
        /// Create Date : 2020.03.02
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pForm"></param>
        void SetMDIFormTitle(PsmhDb pDbCon, string pForm)
        {
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            if (pForm == "frmBoard" || pForm == "frmSplash")
            {
                return;
            }

            SQL = "";
            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    PROJECTNAME, FORMNAME, FORMNAME1, FORMAUTH, FORMAUTHTEL ";
            SQL = SQL + ComNum.VBLF + "FROM BAS_PROJECTFORM ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNAME = '" + pForm + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string[] arryAssName = VB.Split(dt.Rows[0]["PROJECTNAME"].ToString().Trim(), ".");
            string strAssName = arryAssName[0];
            strAssName = strAssName + ".dll";

            string strPROJECTNAME = dt.Rows[0]["PROJECTNAME"].ToString().Trim();
            string strFORMNAME = dt.Rows[0]["FORMNAME"].ToString().Trim();
            string strFORMAUTH = dt.Rows[0]["FORMAUTH"].ToString().Trim();
            string strFORMAUTHTEL = dt.Rows[0]["FORMAUTHTEL"].ToString().Trim();
            dt.Dispose();
            dt = null;

            string strUpdateIniFile = @"C:\PSMHEXE\PSMHAutoUpdate.ini";
            clsIniFile myIniFile = new clsIniFile(strUpdateIniFile);
            double dblVerClt = myIniFile.ReadValue("DEFAULT_UPDATE_LIST", strAssName, 0);

            //lblTitle.Text = "재원심사 메인 : ";
            //lblTitle.Text += strFORMNAME + " (" + strFORMAUTH + " ☎ " + strFORMAUTHTEL + ")"
            //            + VB.Space(6) + " (" + strPROJECTNAME + " : Ver " + dblVerClt.ToString() + ")";
            //lblTitle.ForeColor = System.Drawing.Color.White;
            return;
        }

        void ePost_value_HPAT(HIC_PATIENT item)
        {
            Hpatient = item;
        }

    }
}
