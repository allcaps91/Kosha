using ComBase; //기본 클래스
using ComBase.Controls;
using Microsoft.Win32;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrChartNew : Form, EmrChartForm
    {
        #region API
        //670
        //int mintHeight = 0;
        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);
        #endregion

        #region 변수 
        //통증평가 폼 
        frmAcheDetail frmAche = null;
        /// <summary>
        /// 현재 폼 한개 더 보 여주기 위한 변수
        /// </summary>
        frmEmrChartNew fEmrChartNewForm = null;

        /// <summary>
        /// 검사결과 매핑 폼(마취기록지용)
        /// </summary>
        frmAnFormExam frmAnFormExamX = null;

        //private mtsPanel15.mPanel panTopMenu;
        //private mtsPanel15.mPanel panChart;

        /// <summary>
        /// 템플릿 화면인지 여부
        /// </summary>
        bool misUserTemplet = false; //
        public string mstrMACRONO = string.Empty;
        /// <summary>
        /// 차트작성 0, 기록지등록에서 호출 1,
        /// </summary>
        int mCallFormGb = 0;  //
        /// <summary>
        /// 처방에서 저장 호출 여부
        /// </summary>
        bool isReciveOrderSave = false; //
        /// <summary>
        /// 작성전 값을 저장하여 변경여부를 체크 한다.
        /// </summary>
        string mstrXmlInit = string.Empty; //
        /// <summary>
        /// 작성전 값을 저장하여 변경여부를 체크 한다. : 이미지
        /// </summary>
        clsEmrType.EmrXmlImageInit[] mEmrXmlImageInit = null; //
        /// <summary>
        /// 상용구 클릭시 커서 이동 때문에 필요함.
        /// </summary>
        int lastScrollValue = 0;

        /// <summary>
        /// 퇴실결과지 관련 상용구 설정
        /// </summary>
        string tempMacro = string.Empty;

        /// <summary>
        /// 의료정보팀 권한 작성용 플래그
        /// </summary>
        string strSaveFlag = string.Empty;

        /// <summary>
        /// 로딩 다되었는지 확인.
        /// </summary>
        public bool mLoading = false;

        /// <summary>
        /// 신규 기록지 히스토리 보는 폼
        /// </summary>
        frmEmrNewHisView frmEmrNewHisViewX = null;
        #endregion

        #region 수혈기록지 이중확인 관련\
        /// <summary>
        /// 수혈기록지 이중확인 폼
        /// </summary>
        frmEmrLogin frmEmrLoginX = null;

        /// <summary>
        /// 이중확인 사람 부서 컨트롤
        /// </summary>
        string mstrConfirmBuse = string.Empty;
        /// <summary>
        /// 이중확인 사람 이름 컨트롤
        /// </summary>
        string mstrConfirmName = string.Empty;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;
        FormXml[] mFormXml = null;
        //FormXml[] mFormXmlInit = null;
        public string mstrFormNo = "0";
        public string mstrUpdateNo = "0";
        public string mstrFormText = string.Empty;
        public string mstrEmrNo = "0";  //961 131641  //963 735603
        public string mstrMode = "W";
        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region // 상용구 관련 변수 선언
        Control mControl = null;    //일반 텍스트
        frmEmrMacrowordProg frmMacrowordProgEvent;

        Control mCalControl = null; //달력 띄우기
        frmEmrCaledar frmEmrCaledarEvent;

        FarPoint.Win.Spread.FpSpread ssMacroWord;
        FarPoint.Win.Spread.SheetView ssMacroWord_Sheet1;

        /// <summary>
        /// 상용구 폼.
        /// </summary>
        frmEmrBaseSympOld fEmrMacro = null;
        #endregion // 상용구 관련 모듈

        #region // V/S 폼
        /// <summary>
        /// V/S폼
        /// </summary>
        frmEmrBaseVitalSign frmEmrVitalSignX = null;
        #endregion // 상용구 관련 모듈

        #region // 인공신장실 인터페이스  폼
        /// <summary>
        /// 인공신장실 인터페이스  폼
        /// </summary>
        frmHemodialysisInterface fEmrHemodialysisInterface = null;
        #endregion // 상용구 관련 모듈

        #region // 진단명 검색 관련 변수 선언
        /// <summary>
        /// 진단코드 텍스트박스
        /// </summary>
        Control mDiagCode = null;
        /// <summary>
        /// 진단명 텍스트박스(신규 서식)
        /// </summary>
        Control mDiagName = null;
        /// <summary>
        /// 진단명 스프레드
        /// </summary>
        FarPoint.Win.Spread.FpSpread ssDiag;
        /// <summary>
        /// 진단명 스프레드 시트
        /// </summary>
        FarPoint.Win.Spread.SheetView ssDiag_Sheet1;

        /// <summary>
        /// 상병 선택값
        /// </summary>
        string strDiagCode = string.Empty;
        #endregion 

        #region // 스프레드 관련 변수 선언
        /// <summary>
        /// 수술시간 스프레드
        /// </summary>
        FarPoint.Win.Spread.FpSpread ssOpList;
        /// <summary>
        /// 수술 스프레드 시트
        /// </summary>
        FarPoint.Win.Spread.SheetView ssOpList_Sheet1;
        #endregion // 상용구 관련 모듈

        #region //EmrChartForm

        /// <summary>
        /// 외부호출 : 차트 저장
        /// </summary>
        /// <param name="strFlag"></param>
        /// <returns></returns>
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;

            if (CheckChartChangeData() == "")
            {
                dblEmrNo = VB.Val(mstrEmrNo);
                return dblEmrNo;
            }
            if (ComFunc.MsgBoxQEx(this, "변경된 내용이 있습니다." + ComNum.VBLF + "저장 하시겠습니까?") == DialogResult.No)
            {
                dblEmrNo = VB.Val(mstrEmrNo);
                return dblEmrNo;
            }
            isReciveOrderSave = true;
            dblEmrNo = SaveData(strFlag, true);
            isReciveOrderSave = false;
            return dblEmrNo;
        }

        /// <summary>
        /// 외부호출 : 기록지 삭제
        /// </summary>
        /// <returns></returns>
        public bool DelDataMsg()
        {
            bool rtnVal = false;
            rtnVal = pDelData();
            return rtnVal;
        }

        /// <summary>
        /// 외부호출 : 기록지 클리어
        /// </summary>
        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
            InitMibi();
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            SetInitChatValue();
        }

        /// <summary>
        /// 외부호출 : 템플릿 불러오기 : 서식생성기
        /// </summary>
        /// <param name="dblMACRONO"></param>
        public void SetUserFormMsg(double dblMACRONO)
        {
            pSetUserForm(dblMACRONO);
        }

        /// <summary>
        /// 외부호출 : 템플릿 저장 : 서식 생성기
        /// </summary>
        /// <param name="dblMACRONO"></param>
        /// <returns></returns>
        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            rtnVal = pSaveUserForm(dblMACRONO);
            return rtnVal;
        }

        /// <summary>
        /// 외부호출 : 이전내역 불러오기
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <param name="strOldGb"></param>
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {

        }

        /// <summary>
        /// 외부호출 : 서식지 출력
        /// </summary>
        /// <param name="strPRINTFLAG"></param>
        /// <returns></returns>
        public int PrintFormMsg(string strPRINTFLAG)
        {
            //DisChargerReCordVisible();

            int rtnVal = 0;
            if (strPRINTFLAG == "N")
            {
                using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
                {
                    frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterParent;
                    frmEmrPrintOptionX.ShowDialog(this);
                }
            }
            
            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            if (mEmrCallForm != null && ((Form) mEmrCallForm).Name.Equals("frmEmrJobChartCopy") == false 
                && clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return rtnVal;
            }

            List<Control> lstDisabled = ComFunc.GetAllControls(panChart).Where(d => d.Enabled == false).ToList();
            if (lstDisabled.Count > 0)
            {
                foreach(Control control in lstDisabled)
                {
                    control.Enabled = true;
                }
            }

            #region 혈액투석 기록지 조회 관련 숨기기
            if (pForm.FmFORMNO == 1577 || pForm.FmFORMNO == 2466)
            {
                Control control = Controls.Find("PanSearch", true).FirstOrDefault();
                if(control != null)
                {
                    control.Visible = false;
                }
            }
            #endregion
            
            #region 진료기록지 -  이미지, 기타  숨기기
            if (pForm.FmGRPFORMNO >= 1000 && pForm.FmGRPFORMNO <= 1012 || pForm.FmGRPFORMNO == 1075)
            {
                Control control = panChart.Controls.Find("GI0000029790", true).FirstOrDefault();
                if (control != null)
                {
                    List<Control> controls = ComFunc.GetAllControls(control).Where(d => d is PictureBox && d.Tag != null && VB.IsNumeric(d.Tag)).ToList();
                    if (controls.Count == 0)
                    {
                        control.Visible = false;
                    }
                }
             

                control = panChart.Controls.Find("GI0000001067", true).FirstOrDefault();
                if (control != null)
                {
                    List<Control> controls = ComFunc.GetAllControls(control).Where(d => d is TextBox && string.IsNullOrWhiteSpace(d.Text.Trim()) == false).ToList();
                    if (controls.Count == 0)
                    {
                        control.Visible = false;
                    }
                }
            }
            #endregion

            if (pForm.FmFORMNO == 1647 && pAcp.medEndDate.Length > 0)
            {
                rtnVal = clsFormPrint.PrintRtf(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panCoading, richTextBox1,  "C");
            }
            else
            {
                rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo,  panChart, "C");            
            }

            return rtnVal;
        }

        /// <summary>
        /// 외부호출 : 서식지를 이미지로 변환
        /// </summary>
        /// <param name="strPRINTFLAG"></param>
        /// <returns></returns>
        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        /// <summary>
        /// 상단 버튼 모음 로드
        /// </summary>
        private void SetTopMenuLoad()
        {
            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetSaveTemp += new usFormTopMenu.SetSaveTemp(usFormTopMenuEvent_SetSaveTemp);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rSetPrintNull += new usFormTopMenu.SetPrintNull(usFormTopMenuEvent_SetPrintNull);
            usFormTopMenuEvent.rSetComplete += new usFormTopMenu.SetComplete(usFormTopMenuEvent_SetComplete);
            usFormTopMenuEvent.rSetCoading += new usFormTopMenu.SetCoading(usFormTopMenuEvent_SetCoading);
            usFormTopMenuEvent.rSetHisSearch += new usFormTopMenu.SetHisSearch(usFormTopMenuEvent_SetHisSearch);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------

        }

        /// <summary>
        /// 작성시간 패널 보이게
        /// </summary>
        /// <param name="mkText"></param>
        private void usFormTopMenuEvent_SetTimeCheckShow(ComboBox mkText)
        {
            mMaskBox = mkText;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = mMaskBox.Top - 5;
            usTimeSetEvent.Left = mMaskBox.Left;
            usTimeSetEvent.BringToFront();
        }

        /// <summary>
        /// 작성시간 변경
        /// </summary>
        /// <param name="strText"></param>
        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usFormTopMenuEvent.txtMedFrTime.Text = strText;
        }

        /// <summary>
        /// T 버튼 클릭 이벤트
        /// </summary>
        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        /// <summary>
        /// 인증저장 버튼 클릭 이벤트
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        private void usFormTopMenuEvent_SetSave(string strFrDate, string strFrTime)
        {
            #region 2021-634 전산업무 의뢰서 처리(입퇴원 요약지 검수완료)
            if (pForm.FmFORMNO == 1647 && ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>() >= "2021-07-01 06:00:00".To<DateTime>() && 
                usFormTopMenuEvent.mbtnComplete.Text.Equals("검수완료") && clsType.User.AuAMANAGE.Equals("1") == false)
            {
                ComFunc.MsgBoxEx(this, "완료된차트입니다.\r\n수정하시려면 의료정보팀 연락요망 (T.8041)");
                return;
            }
            #endregion

            //변경하시겠습니까 메시지 체크 여부를 위해서 추가함.
            string strTempEmrNo = mstrEmrNo;
            mstrEmrNo = SaveData("0", true).ToString();

            #region 오늘 작성된 내역 전자인증 다시
            if (pAcp != null)
            {
                clsEmrFunc.NowEmrCert(clsDB.DbCon, pAcp.medFrDate, pAcp.ptNo);
            }
            #endregion

            //if ((VB.Val(mstrEmrNo) > 0 && (pForm.FmOLDGB == 1) && strTempEmrNo.Equals(mstrEmrNo) == false) ||
            //    (pForm.FmOLDGB == 0 &&  strTempEmrNo.Equals(mstrEmrNo)) ||
            //     VB.Val(strTempEmrNo) == 0 && VB.Val(mstrEmrNo) > 0)
            //{
            //    //frmEmrBaseContinuView
            //    if (mEmrCallForm != null && ((mEmrCallForm as Form).Name.Equals("frmEmrLibViewerNr") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBaseChartView") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBaseChartWrite") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBloodInfo") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBaseContinuView")))
            //    {
            //        LoadEmrChartInfo();
            //    }
            //    else
            //    {
            //        mstrEmrNo = "0";
            //    }
            //}
        }

        /// <summary>
        /// 임시저장 버튼 클릭 이벤트
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        private void usFormTopMenuEvent_SetSaveTemp(string strFrDate, string strFrTime)
        {
            string strTempEmrNo = mstrEmrNo;
            mstrEmrNo = SaveData("0", false).ToString();

            //if ((VB.Val(mstrEmrNo) > 0 && (pForm.FmOLDGB == 1) && strTempEmrNo.Equals(mstrEmrNo) == false) ||
            //    (pForm.FmOLDGB == 0 && strTempEmrNo.Equals(mstrEmrNo)) ||
            //     VB.Val(strTempEmrNo) == 0 && VB.Val(mstrEmrNo) > 0)
            //{
            //    if (mEmrCallForm != null && ((mEmrCallForm as Form).Name.Equals("frmEmrLibViewerNr") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBaseChartView") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBaseChartWrite") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBloodInfo") ||
            //        (mEmrCallForm as Form).Name.Equals("frmEmrBaseContinuView")))
            //    {
            //        LoadEmrChartInfo();
            //    }
            //    else
            //    {
            //        mstrEmrNo = "0";
            //    }
            //}
        }

        /// <summary>
        /// 삭제 버튼 클릭 이벤트
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
        }

        /// <summary>
        /// Clear 버튼 클릭 이벤트
        /// </summary>
        private void usFormTopMenuEvent_SetClear()
        {
            if (CheckChartChangeData() != "")
            {
                if (ComFunc.MsgBoxQEx(this, "변경된 내용이 있습니다." + ComNum.VBLF + "Clear 하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
            }
            mstrEmrNo = "0";
            pClearForm();
            InitMibi();
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            SetInitChatValue();
            usFormTopMenuEvent.mbtnSaveTemp.Visible = pForm.FmOLDGB != 1;
        }

        /// <summary>
        /// 출력버튼 클릭 이벤트
        /// </summary>
        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }

        /// <summary>
        /// 출력버튼(빈서식지) 클릭 이벤트
        /// </summary>
        private void usFormTopMenuEvent_SetPrintNull()
        {
            //pClearForm();

            if (pForm.FmFORMNO == 1647 && ComFunc.MsgBoxQEx(this, "출력하시겠습니까?") == DialogResult.No)
                return;


            using(frmEmrChartNew frmPrtNull = new frmEmrChartNew(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), pAcp, "0", "V"))
            {
                frmPrtNull.Show();
                frmPrtNull.pClearForm();
                frmPrtNull.pPrintFormNull();
            }

            //pPrintFormNull();

            ////관리자일경우 출력후 작성내역 로드
            //if(clsType.User.AuAMANAGE == "1")
            //{
            //    LoadEmrChartInfo();
            //}
        }

        /// <summary>
        /// 미검수, 검수완료 버튼 이벤트
        /// </summary>
        private void usFormTopMenuEvent_SetComplete()
        {
            pSetComplete();
        }

        /// <summary>
        /// 코딩
        /// </summary>
        private void usFormTopMenuEvent_SetCoading()
        {
            pSetCoading();
        }

        /// <summary>
        /// 코딩
        /// </summary>
        private void usFormTopMenuEvent_SetHisSearch()
        {
            if (frmEmrNewHisViewX != null)
            {
                frmEmrNewHisViewX.BringToFront();
                frmEmrNewHisViewX.Show();
                return;
            }

            frmEmrNewHisViewX = new frmEmrNewHisView(pAcp, pForm, mstrEmrNo);
            frmEmrNewHisViewX.StartPosition = FormStartPosition.CenterParent;
            frmEmrNewHisViewX.FormClosed += frmEmrNewHisViewX_FormClosed;
            frmEmrNewHisViewX.Show(this);
        }

        private void frmEmrNewHisViewX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrNewHisViewX != null)
            {
                frmEmrNewHisViewX.Dispose();
                frmEmrNewHisViewX = null;
                return;
            }
        }

        /// <summary>
        /// 닫기 버튼 클릭 이벤트
        /// </summary>
        private void usFormTopMenuEvent_EventClosed()
        {
            this.Close();
            //아무것도 하지 않는다.
        }

        /// <summary>
        /// 작성일자 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            //필요시만 코딩함
        }

        #endregion

        #region //폼생성후 기본 이벤트 매핑
        private void SetControlEvents()
        {
            pAddOpListSpd();
            pAddDiagSpd();
            pAddMacroSpd();
            SetEventToControl();
            SetControlInitValue();
        }

        /// <summary>
        /// 이벤트를 매핑한다
        /// </summary>
        private void SetEventToControl()
        {
            //이미지 작성
            pAddEventImage(this);
            //텍스트 박스에 상용구 이벤트를 세팅한다
            pAddEventToText(this);
            //Radio 패널 열기 닫기
            pAddEventCheckAndRdio(this);
            //Button에 이벤트 달기
            pAddEventButton(this);
            ////모든 컨트롤 임시로 이름 등등 표시하도록
            pAdClickMassage(this);
        }

        private void pAdClickMassage(Control objParent)
        {
            List<Control> controls = FormFunc.GetAllControls(this).Where(d => d is Panel).ToList();
            foreach (Control control in controls)
            {
                ((Panel)control).Click += new System.EventHandler(Panel_Click);
            }
        }

        /// <summary>
        /// Panel Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_Click(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
            ssDiag.Visible = false;
            //ComFunc.MsgBoxEx(this, ((Panel)sender).Name);
        }

        /// <summary>
        /// 기본값을 세팅을 한다
        /// </summary>
        private void SetControlInitValue()
        {
            if (mFormXml == null) return;

            return;

            //for (int i = 0; i < mFormXml.Length; i++)
            //{
            //    Control[] tx = null;
            //    tx = this.Controls.Find(mFormXml[i].strCONTROLNAME, true);
            //    if (tx != null)
            //    {
            //        if (tx.Length > 0)
            //        {
            //            if (tx[0] is mtsPanel15.mPanel)
            //            {

            //            }
            //            if (tx[0] is Panel)
            //            {

            //            }
            //            if (tx[0] is GroupBox)
            //            {

            //            }
            //            if (tx[0] is Button)
            //            {

            //            }
            //            if (tx[0] is Label)
            //            {

            //            }
            //            if (tx[0] is TextBox)
            //            {

            //            }
            //            if (tx[0] is CheckBox)
            //            {
            //                if (mFormXml[i].strCHECKED.ToUpper() == "TRUE")
            //                {
            //                    ((CheckBox)tx[0]).Checked = true;
            //                }
            //            }
            //            if (tx[0] is RadioButton)
            //            {
            //                if (mFormXml[i].strCHECKED.ToUpper() == "TRUE")
            //                {
            //                    ((RadioButton)tx[0]).Checked = true;
            //                }
            //            }
            //            if (tx[0] is PictureBox)
            //            {
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 이미지 이벤트
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventImage(Control objParent)
        {
            Control[] controls = FormFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is PictureBox)
                {
                    ((PictureBox)control).DoubleClick += new System.EventHandler(PictureBox_DoubleClick);
                }
            }
        }

        /// <summary>
        /// 체크박스, 레디오 버튼
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventCheckAndRdio(Control objParent)
        {
            Control[] controls = FormFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is RadioButton)
                {
                    ((RadioButton)control).CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
                }
                if (control is CheckBox)
                {
                    ((CheckBox)control).CheckedChanged += new System.EventHandler(CheckBox_CheckedChanged);
                }
            }
        }

        /// <summary>
        /// Button 이벤트
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventButton(Control objParent)
        {
            Control[] controls = FormFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    ((Button)control).Click += new System.EventHandler(Button_Click);
                }
            }
        }

        /// <summary>
        /// PictureBox 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
                return;

            string strTag = string.Empty;

            if (((PictureBox)sender).Tag != null)
            {
                strTag = ((PictureBox)sender).Tag.ToString();
            }

            lastScrollValue = panChart.AutoScrollPosition.Y < 0 ? panChart.AutoScrollPosition.Y * -1 : panChart.AutoScrollPosition.Y;

            //clsEmrFunc.SetImageEvent(this, (PictureBox)sender, strTag, mstrFormNo, mstrUpdateNo, mstrMode, mstrEmrNo, mEmrCallForm);
            Image baseImage = ((PictureBox)sender).Image == null || ((PictureBox) sender).BackColor == Color.LightGray ? FindAndImage(((PictureBox)sender).Name) : ((PictureBox)sender).Image;
            clsEmrFunc.SetImageEvent(this, (PictureBox)sender, strTag, mstrFormNo, mstrUpdateNo, mstrMode, mstrEmrNo, baseImage, mEmrCallForm);
            panChart.VerticalScroll.Value = lastScrollValue;
        }

        private Image FindAndImage(string strConName)
        {
            Image Pic = null;
            for (int i = 0; i < mFormXml.Length; i++)
            {
                if (mFormXml[i].strCONTROLNAME == strConName)
                {
                    Pic = mFormXml[i].imgIMAGE;
                    break;
                }
            }
            return Pic;
        }

        /// <summary>
        /// RadioButton Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Tag == null || mLoading == false)
            {
                return;
            }

            string strTag = ((RadioButton)sender).Tag.ToString();

            if (strTag.Trim() == "") return;

            if (((RadioButton)sender).Checked == true)
            {
                //입퇴원 요약지 상병명 검색옵션 19-08-08 추가
                if(strTag == "DIAGRDO")
                {
                    strDiagCode = ((RadioButton)sender).Name;
                    return;
                }

                #region 퇴원간호 계획지 전원 연동
                if (VB.Val(mstrEmrNo) == 0 && pForm.FmOLDGB != 1 && pForm.FmFORMNO == 966 && ((RadioButton)sender).Name.Equals("I0000035173_1") && mLoading)
                {
                    //추후관리 - 타병원
                    Control control = panChart.Controls.Find("I0000035211_1", true).FirstOrDefault();
                    if (control != null)
                    {
                        (control as RadioButton).Checked = true;
                    }

                    //퇴원후 거주장소 - 타병원
                    control = panChart.Controls.Find("I0000035221", true).FirstOrDefault();
                    if (control != null)
                    {
                        (control as CheckBox).Checked = true;
                    }
                }
                #endregion


                #region 내시경(진정)검사전
                if (VB.Val(mstrEmrNo) == 0 && pForm.FmOLDGB != 1 && pForm.FmFORMNO == 2429 && ((RadioButton)sender).Name.Equals("I0000034261") && mLoading)
                {
                    Control control = panChart.Controls.Find("I0000011889", true).FirstOrDefault();
                    if (control != null)
                    {
                        (control as RadioButton).Checked = true;
                    }
                }

                if (VB.Val(mstrEmrNo) == 0 && pForm.FmOLDGB != 1 && pForm.FmFORMNO == 2429 && ((RadioButton)sender).Name.Equals("I0000034263") && mLoading)
                {
                    Control control = panChart.Controls.Find("I0000011890", true).FirstOrDefault();
                    if (control != null)
                    {
                        (control as RadioButton).Checked = true;
                    }
                }

                #endregion

                clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (RadioButton)sender, strTag, pAcp, "");
                //clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (RadioButton)sender, strTag, p, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
            }
        }

        /// <summary>
        /// CheckBox Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Tag == null || mLoading == false)
            {
                return;
            }

            #region 입퇴원 요약지 체크박스 라디오 버튼 처럼
            if (pForm.FmFORMNO == 1647 || pForm.FmFORMNO == 1598)
            {
                List<Control> controls = FormFunc.GetAllControls(panChart).Where(c => c is CheckBox).ToList();
                #region 전체 체크박스 
                for (int i = 0; i < controls.Count; i++)
                {
                    ((CheckBox)controls[i]).CheckedChanged -= CheckBox_CheckedChanged;
                }
                #endregion

                CheckBox chk = (CheckBox) sender;
                if (chk.Checked == true)
                {
                    #region 퇴원시 환자상태
                    if (chk.Text.Equals("완쾌") || chk.Text.Equals("경쾌") || chk.Text.Equals("호전안됨") ||
                        chk.Text.Equals("치료안함") || chk.Text.Equals("진단뿐") || chk.Text.Equals("가망없는 퇴원") ||
                        chk.Text.Equals("48시간이후사망") || chk.Text.Equals("48시간이내사망"))
                    {
                        #region 완쾌, 경쾌, 호전안됨, 치료안함, 진단뿐, 가망없는퇴원, 48시간이후사망, 48시간이내사망
                        //for (int i = 1; i < 9; i++)
                        //{
                        //    controls = panChart.Controls.Find("ik" + i, true);
                        //    if (controls.Length > 0)
                        //    {
                        //        ((CheckBox)controls[0]).Checked = false;
                        //    }
                        //}
                        
                        foreach(Control control in controls.Where(c => c.Text.Equals("완쾌") || c.Text.Equals("경쾌") || c.Text.Equals("호전안됨") ||
                        c.Text.Equals("치료안함") || c.Text.Equals("진단뿐") || c.Text.Equals("가망없는 퇴원") ||
                        c.Text.Equals("48시간이후사망") || c.Text.Equals("48시간이내사망")))
                        {
                            ((CheckBox)control).Checked = false;
                        }

                        #endregion
                        chk.Checked = true;

                        if (chk.Text.Equals("48시간이후사망") || chk.Text.Equals("48시간이내사망"))
                        {
                            //for (int i = 9; i < 15; i++)
                            //{
                            //    controls = panChart.Controls.Find("ik" + i, true);
                            //    if (controls.Length > 0)
                            //    {
                            //        ((CheckBox)controls[0]).Checked = false;
                            //        ((CheckBox)controls[0]).Enabled = false;
                            //    }
                            //}

                            foreach (Control control in controls.Where(c => 
                            c.Text.Equals("퇴원지시후") || c.Text.Equals("DAMA") || c.Text.Equals("탈원") || c.Text.Equals("전원") ||
                            c.Text.Equals("없음") || c.Text.Equals("있음")))
                            {
                                ((CheckBox)control).Checked = false;
                                ((CheckBox)control).Enabled = false;
                            }

                        }
                        else
                        {
                            //for (int i = 9; i < 15; i++)
                            //{
                            //    controls = panChart.Controls.Find("ik" + i, true);
                            //    if (controls.Length > 0)
                            //    {
                            //        ((CheckBox)controls[0]).Enabled = true;
                            //    }
                            //}

                            foreach (Control control in controls.Where(c =>
                                c.Text.Equals("퇴원지시후") || c.Text.Equals("DAMA") || c.Text.Equals("탈원") || c.Text.Equals("전원") ||
                                c.Text.Equals("없음") || c.Text.Equals("있음")))
                            {
                                ((CheckBox)control).Enabled = true;
                            }
                        }
                    }

                    #endregion

                    #region 퇴원형태
                     //if (chk.Name.Equals("ik9") || chk.Name.Equals("ik10") || chk.Name.Equals("ik11") ||
                     //   chk.Name.Equals("ik12"))
                    if (chk.Text.Equals("퇴원지시후") || chk.Text.Equals("DAMA") || chk.Text.Equals("탈원") || chk.Text.Equals("전원"))
                    {
                        #region 퇴원지시후, DAMA, 탈원, 전원
                        //for (int i = 9; i < 13; i++)
                        //{
                        //    controls = panChart.Controls.Find("ik" + i, true);
                        //    if (controls.Length > 0)
                        //    {
                        //        ((CheckBox)controls[0]).Checked = false;
                        //    }
                        //}

                        foreach (Control control in controls.Where(c => c.Text.Equals("퇴원지시후") || c.Text.Equals("DAMA") || c.Text.Equals("탈원") || c.Text.Equals("전원")))
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        #endregion

                        chk.Checked = true;
                    }
                    #endregion

                    #region 추후치료계획
                     if (chk.Text.Equals("없음") || chk.Text.Equals("있음"))
                    {
                        #region 없음, 있음.
                        //for (int i = 13; i < 15; i++)
                        //{
                        //    controls = panChart.Controls.Find("ik" + i, true);
                        //    if (controls.Length > 0)
                        //    {
                        //        ((CheckBox)controls[0]).Checked = false;
                        //    }
                        //}
                        foreach (Control control in controls.Where(c => c.Text.Equals("없음") || c.Text.Equals("있음")))
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        #endregion

                        if (chk.Text.Equals("없음"))
                        {
                            Control control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "it2" : "I0000013189_2", true).First();
                            if (control != null)
                            {
                                ((TextBox)control).Clear();
                            }
                        }

                        chk.Checked = true;
                    }
                    #endregion


                
                }
                else if(chk.Text.Equals("48시간이후사망") || chk.Text.Equals("48시간이내사망"))
                {
                    //for (int i = 9; i < 15; i++)
                    //{
                    //    controls = panChart.Controls.Find("ik" + i, true);
                    //    if (controls.Length > 0)
                    //    {
                    //        ((CheckBox)controls[0]).Enabled = true;
                    //    }
                    //}

                    foreach (Control control in controls.Where(c =>
                        c.Text.Equals("퇴원지시후") || c.Text.Equals("DAMA") || c.Text.Equals("탈원") || c.Text.Equals("전원") ||
                        c.Text.Equals("없음") || c.Text.Equals("있음")))
                    {
                        ((CheckBox)control).Enabled = true;
                    }
                }


                //#region 전체 체크박스 
                //for (int i = 1; i < 15; i++)
                //{
                //    controls = panChart.Controls.Find("ik" + i, true);
                //    if (controls.Length > 0)
                //    {
                //        ((CheckBox)controls[0]).CheckedChanged += CheckBox_CheckedChanged;
                //    }
                //}
                //#endregion

                #region 전체 체크박스 
                for (int i = 0; i < controls.Count; i++)
                {
                    ((CheckBox)controls[i]).CheckedChanged += CheckBox_CheckedChanged;
                }
                #endregion
            }
            #endregion

            string strTag = ((CheckBox)sender).Tag.ToString();

            if (strTag.Trim() == "") return;

            clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (CheckBox)sender, strTag, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag == null || mLoading == false)
            {
                return;
            }

            string strTag = ((Button)sender).Tag.ToString();

            if (strTag.Trim().Length == 0)
                return;

            ssDiag.Visible = false;
            #region 수술등 여러 정보 넣는 함수
            //GetTransferHistory 전과(전출)기록지 읽는 부분 강제 설정
            // P/G 테스트 후 FORMNO = 2323, AEMRFORMXML.USERFUNC값 변경 예정
            if (strTag.Equals("GETOPLIST") || strTag.Equals("GetTransferHistory"))
            {
                if (ssOpList.Visible)
                {
                    ssOpList.Visible = false;
                    return;
                }

                switch (pForm.FmFORMNO)
                {
                    case 2074:
                        clsEmrFunc.setChartFormValue(clsDB.DbCon, this, pAcp);
                        break;
                    case 2737:
                        clsEmrFunc.setChartFormValue10(clsDB.DbCon, this, pAcp, pForm);
                        break;
                    case 2605:
                    case 2676:
                        clsEmrFunc.setChartFormValue2605(clsDB.DbCon, this, pAcp, pForm);
                        break;
                    case 2277:
                        clsEmrFunc.setChartFormValue4(clsDB.DbCon, this, pAcp);
                        break;
                    case 2305: //신생아 간호정보조사지
                    case 2306: //신생아 출생정보 기록지 (분만실용)
                        string strPtno = VB.InputBox("아기의 엄마 등록번호를 입력하십시요.");
                        if (VB.IsNumeric(strPtno) == false)
                        {
                            ComFunc.MsgBoxEx(this, "숫자만 입력하세요!!");
                            return;
                        }

                        string strCDate = VB.InputBox("산모의 간호정보조사지 작성일자를 입력하세요. 예)20200101");
                        if (Regex.IsMatch(strCDate, @"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$") == false)
                        {
                            ComFunc.MsgBoxEx(this, "날짜 형식을 맞춰주세요 예)20200101");
                            return;
                        }

                        clsEmrFunc.setChartFormValueBaby_New(clsDB.DbCon, this, VB.Val(strPtno).ToString("00000000"), strCDate, pForm);
                        break;
                    //case "2323": 
                    // clsEmrFunc => GetTransferHistory 코딩 되어있음.
                        //clsEmrFunc.setChartFormValue8(clsDB.DbCon, this, pAcp);
                        //break;
                    case 2280:
                        if (pForm.FmOLDGB == 1)
                        {
                            clsEmrFunc.setChartFormValue3(clsDB.DbCon, this, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), pForm);
                        }
                        else
                        {
                            clsEmrFunc.OpControl(clsDB.DbCon, this, panChart, (Button)sender, ssOpList, pAcp, pForm);
                        }
                        break;
                    case 2323:
                        clsEmrFunc.OpControl(clsDB.DbCon, this, panChart, (Button)sender, ssOpList, pAcp, pForm);
                        break;
                    case 2307: //신생아 출생정보 기록지 (신생아용)
                        strPtno = VB.InputBox("아기의 엄마 등록번호를 입력하십시요.");
                        if (VB.IsNumeric(strPtno) == false)
                        {
                            ComFunc.MsgBoxEx(this, "숫자만 입력하세요!!");
                            return;
                        }
                        //if (pForm.FmOLDGB == 1)
                        //{
                            clsEmrFunc.setChartFormValue6(clsDB.DbCon, this, VB.Val(strPtno).ToString("00000000"), usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), pForm);
                        //}
                        //else
                        //{
                        //    clsEmrFunc.setChartFormValueBaby_New(clsDB.DbCon, this, VB.Val(strPtno).ToString("00000000"), usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), pForm);
                        //}
                        break;                    
                    default:
                        clsEmrFunc.OpControl(clsDB.DbCon, this, panChart, (Button)sender, ssOpList, pAcp, pForm);
                        break;
                }
                return;
            }
            #endregion

            #region 신규 기록지 열기
            else if(strTag.IndexOf("Set_NewForm_Show") != -1)
            {
                string strFormNo = strTag.Substring(strTag.LastIndexOf(":") + 1);
                EmrForm showForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, strFormNo);

                if (showForm != null)
                {
                    MedicalRecordMst((Button)sender, showForm);
                }
            }
            #endregion

            #region 상용구
            else if (strTag.IndexOf("Set_Form_Macro") != -1)
            {
                if (frmEmrVitalSignX != null)
                {
                    frmEmrVitalSignX.Dispose();
                    frmEmrVitalSignX = null;
                }

                string strControlNm = strTag.Substring(strTag.IndexOf(":") + 1);

                Application.DoEvents();

                fEmrMacro = new frmEmrBaseSympOld(clsType.User.IdNumber, clsType.User.DeptCode, clsType.User.IdNumber, strControlNm, "ProgressNote");
                fEmrMacro.rEventMakeText += new frmEmrBaseSympOld.EventMakeText(frmEmrBaseSympOld_EventMakeText);
                fEmrMacro.FormClosed += FEmrMacro_FormClosed;
                fEmrMacro.Owner = this;
                fEmrMacro.BringToFront();
                fEmrMacro.Show();
                return;
            }
            #endregion

            #region 바이탈
            else if (strTag.IndexOf("Set_Form_V/S") != -1)
            {
                if (frmEmrVitalSignX != null)
                {
                    frmEmrVitalSignX.Dispose();
                    frmEmrVitalSignX = null;
                }

                frmEmrVitalSignX = new frmEmrBaseVitalSign();
                clsPublic.GstrHelpCode = pAcp == null ? "" : pAcp.ptNo;
                frmEmrVitalSignX.FormClosed += fEmrChartFlow_FormClosed;
                frmEmrVitalSignX.Owner = this;
                frmEmrVitalSignX.StartPosition = FormStartPosition.CenterScreen;
                frmEmrVitalSignX.BringToFront();
                frmEmrVitalSignX.Show();
                return;
            }
            #endregion

            #region 전동 기록지 정보 폼
            else if (strTag.IndexOf("Set_Form_TransRecordingInfo") != -1)
            {
                using (frmTransRecordingInfo frmTransRecordingInfo = new frmTransRecordingInfo(this, pAcp))
                {
                    frmTransRecordingInfo.StartPosition = FormStartPosition.CenterParent;
                    frmTransRecordingInfo.ShowDialog(this);
                }
                return;
            }
            #endregion

            #region 통증 평가 
            else if (strTag.Equals("Set_Form_frmAche"))
            {
                if (frmAche != null)
                {
                    frmAche.Dispose();
                    frmAche = null;
                }

                frmAche = new frmAcheDetail(pAcp.acpNoIn, "");
                frmAche.FormClosed += FrmAche_FormClosed; ;
                frmAche.Owner = this;
                frmAche.BringToFront();
                frmAche.Show();
                return;
            }
            #endregion

            #region 수혈기록지 로그인
            else if (strTag.IndexOf("Set_Blood_Login") != -1)
            {
                if (frmEmrLoginX != null)
                {
                    frmEmrLoginX.Dispose();
                    frmEmrLoginX = null;
                }
                
                if (pForm.FmFORMNO == 1965)
                {
                    Control control = panChart.Controls.Find("I0000009925", true).FirstOrDefault();
                    if (control != null)
                    {
                        string tmp = strTag.Substring(strTag.IndexOf(":") + 1);
                        mstrConfirmBuse = tmp.Split(',')[0];
                        mstrConfirmName = tmp.Split(',')[1];

                        string BloodNo = control.Text.Trim();

                        Screen screen = Screen.FromControl(this);
                        string strComponent = string.Empty;

                        control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                        if (control != null && control.Tag != null)
                        {
                            strComponent = control.Tag.ToString();
                        }

                        frmEmrLoginX = new frmEmrLogin(pAcp.ptNo, BloodNo.Trim(), strComponent);
                        frmEmrLoginX.Location = new Point(screen.WorkingArea.Right - frmEmrLoginX.Width - 80, 220);
                        frmEmrLoginX.StartPosition = FormStartPosition.Manual;
                        frmEmrLoginX.FormClosed += FrmEmrLoginX_FormClosed;
                        frmEmrLoginX.rSendUserInfo += FrmEmrLoginX_rSendUserInfo;
                        frmEmrLoginX.Owner = this;
                        frmEmrLoginX.BringToFront();
                        frmEmrLoginX.Show();
                    }
                }
                else if (pForm.FmFORMNO == 3535)
                {
                    List<Control> controls = FormFunc.GetAllControls(panChart).Where(d => d.Name.IndexOf("I0000009925") != -1 && d.Text.NotEmpty()).OrderBy(d => d.Name).ToList();
                    if (controls != null)
                    {
                        string tmp = strTag.Substring(strTag.IndexOf(":") + 1);
                        mstrConfirmBuse = tmp.Split(',')[0];
                        mstrConfirmName = tmp.Split(',')[1];

                        Screen screen = Screen.FromControl(this);
                        StringBuilder BloodNo = new StringBuilder();

                        for(int i = 0; i < controls.Count; i++)
                        {
                            if (i == controls.Count -1)
                            {
                                BloodNo.AppendLine("'" + controls[i].Text.Trim() + "'");
                            }
                            else
                            {
                                BloodNo.AppendLine("'" + controls[i].Text.Trim() + "', ");
                            }
                        }

                        string strComponent = string.Empty;
                        Control control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                        if (control != null && control.Tag != null)
                        {
                            strComponent = control.Tag.ToString();
                        }

                        frmEmrLoginX = new frmEmrLogin(pAcp.ptNo, BloodNo.ToString().Trim(), strComponent);
                        frmEmrLoginX.Location = new Point(screen.WorkingArea.Right - frmEmrLoginX.Width - 80, 220);
                        frmEmrLoginX.StartPosition = FormStartPosition.Manual;
                        frmEmrLoginX.FormClosed += FrmEmrLoginX_FormClosed;
                        frmEmrLoginX.rSendUserInfo += FrmEmrLoginX_rSendUserInfo;
                        frmEmrLoginX.Owner = this;
                        frmEmrLoginX.BringToFront();
                        frmEmrLoginX.Show();
                    }
                }

                return;
            }
            #endregion

            #region 혈액투석 기록지 이전내역
            else if (strTag.IndexOf("Set_Form_ChartHis") != -1)
            {
                using (frmEmrChartHisList fEmrChartHisList = new frmEmrChartHisList(this, pAcp.ptNo, pAcp.acpNo, pForm.FmFORMNO.ToString(), pForm.FmFORMNAME, ""))
                {
                    fEmrChartHisList.StartPosition = FormStartPosition.CenterParent;
                    fEmrChartHisList.ShowDialog(this);
                }
                return;
            }
            #endregion

            #region 검사결과 가져오기
            else if (strTag.IndexOf("Set_FormPatInfo_ExamResult") != -1)
            {
                if (frmAnFormExamX != null)
                {
                    frmAnFormExamX.Dispose();
                    frmAnFormExamX = null;
                }

                frmAnFormExamX = new frmAnFormExam(pAcp.ptNo, "", "", "", "", "", "", "", "", "", "");
                {
                    frmAnFormExamX.StartPosition = FormStartPosition.CenterScreen;
                    frmAnFormExamX.rGetPatientInfo += FrmAnFormExamX_rGetPatientInfo;
                    frmAnFormExamX.rEventClosed += FrmAnFormExamX_rEventClosed;
                    frmAnFormExamX.ShowDialog(this);
                }

                return;
            }
            #endregion

            clsEmrFunc.SetControlEvent(clsDB.DbCon, this, (Button)sender, strTag, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
        }

        private void FrmAnFormExamX_rEventClosed()
        {
            if (frmAnFormExamX != null)
            {
                frmAnFormExamX.Dispose();
                frmAnFormExamX = null;
            }
        }

        private void FrmAnFormExamX_rGetPatientInfo(string Hb, string Hct, string Plt, string Wbc, string Na, string K, string GOT, string GPT, string PT, string PTT)
        {
            if (frmAnFormExamX != null)
            {
                frmAnFormExamX.Dispose();
                frmAnFormExamX = null;
            }

            Control control = panChart.Controls.Find("I0000001822", true).FirstOrDefault();
            if (control != null) //HB
            {
                control.Text = Hb;
            }

            control = panChart.Controls.Find("I0000001825", true).FirstOrDefault();
            if (control != null) //Hct
            {
                control.Text = Hct;
            }

            control = panChart.Controls.Find("I0000003609", true).FirstOrDefault();
            if (control != null) //Plt
            {
                control.Text = Plt;
            }

            control = panChart.Controls.Find("I0000002083", true).FirstOrDefault();
            if (control != null) //Wbc
            {
                control.Text = Wbc;
            }

            control = panChart.Controls.Find("I0000007170", true).FirstOrDefault();
            if (control != null) //PT
            {
                control.Text = PT;
            }

            control = panChart.Controls.Find("I0000002582", true).FirstOrDefault();
            if (control != null) //PTT
            {
                control.Text = PTT;
            }

            control = panChart.Controls.Find("I0000033617", true).FirstOrDefault();
            if (control != null) //GOT
            {
                control.Text = GOT;
            }

            control = panChart.Controls.Find("I0000033618", true).FirstOrDefault();
            if (control != null) //GPT
            {
                control.Text = GPT;
            }

            #region Blood Type
            OracleDataReader reader = null;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_BloodType(pAcp);
            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                control = panChart.Controls.Find("I0000011819", true).FirstOrDefault();
                if (control != null) //Blood Type
                {
                    control.Text = reader.GetValue(0).ToString().Trim();
                }
            }

            reader.Dispose();
            #endregion
        }

        private void FrmEmrLoginX_rSendUserInfo(string strName, string strBuse)
        {
            Control control = panChart.Controls.Find(mstrConfirmName, true).FirstOrDefault();
            if (control != null)
            {
                control.Text = strName;
            }

            control = panChart.Controls.Find(mstrConfirmBuse, true).FirstOrDefault();
            if (control != null)
            {
                control.Text = strBuse;
            }

            if (frmEmrLoginX != null)
            {
                frmEmrLoginX.Dispose();
                frmEmrLoginX = null;
            }
        }

        private void FrmEmrLoginX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrLoginX != null)
            {
                frmEmrLoginX.Dispose();
                frmEmrLoginX = null;
            }
        }

        /// <summary>
        /// 해당 내원 기간내에 기록지 있으면 스프레드에 표시
        /// </summary>
        /// <param name="parentCtl"></param>
        /// <param name="emrForm"></param>
        private void MedicalRecordMst(Control parentCtl, EmrForm emrForm)
        {
            string SQL = FormPatInfoQuery.Query_FormPatInfo_Mst(pAcp, emrForm);
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            #region
            ssOpList.Left = parentCtl.Left - 80;
            ssOpList.Top = parentCtl.Top + 90;
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 1)
            {
                if (fEmrChartNewForm != null)
                {
                    fEmrChartNewForm.Dispose();
                    fEmrChartNewForm = null;
                }

                fEmrChartNewForm = new frmEmrChartNew(dt.Rows[0]["FORMNO"].ToString().Trim(), dt.Rows[0]["UPDATENO"].ToString().Trim(),
                    pAcp, dt.Rows[0]["EMRNO"].ToString().Trim(), "V");
                fEmrChartNewForm.StartPosition = FormStartPosition.CenterParent;
                fEmrChartNewForm.FormClosed += FEmrChartNewForm_FormClosed;
                fEmrChartNewForm.Show(this);
            }
            else if (dt.Rows.Count > 1)
            {
                ssOpList.Visible = true;
                ssOpList.ActiveSheet.RowCount = dt.Rows.Count;
                ssOpList.ActiveSheet.SetRowHeight(-1, 20);

                ssOpList.ActiveSheet.Columns[0].Label = "차트일자";
                ssOpList.ActiveSheet.Columns[1].Label = "차트시간";
                ssOpList.ActiveSheet.Columns[2].Label = "작성자";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssOpList.ActiveSheet.Cells[i, 0].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                    ssOpList.ActiveSheet.Cells[i, 0].Tag = dt.Rows[i]["FORMNO"].ToString().Trim();
                    ssOpList.ActiveSheet.Cells[i, 1].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                    ssOpList.ActiveSheet.Cells[i, 1].Tag = dt.Rows[i]["UPDATENO"].ToString().Trim();
                    ssOpList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                    ssOpList.ActiveSheet.Cells[i, 2].Tag = dt.Rows[i]["EMRNO"].ToString().Trim();
                }

                ssOpList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
                ssOpList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

                ssOpList.Parent = panChart;
                ssOpList.Visible = true;
                ssOpList.BringToFront();
            }

            dt.Dispose();
        }

        private void FEmrChartNewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrChartNewForm != null)
            {
                fEmrChartNewForm.Dispose();
                fEmrChartNewForm = null;
            }
        }

        private void FEmrHemodialysisInterface_rSendInterface(string strData)
        {

        }

        private void FrmAche_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmAche != null)
            {
                frmAche.Dispose();
                frmAche = null;
            }
        }


        private void FEmrHemodialysisInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrHemodialysisInterface != null)
            {
                fEmrHemodialysisInterface.Dispose();
                fEmrHemodialysisInterface = null;
            }
        }

        private void fEmrChartFlow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrVitalSignX != null)
            {
                frmEmrVitalSignX.Dispose();
                frmEmrVitalSignX = null;
            }
        }


        #region 경과기록지 상용구 버튼
        private void frmEmrBaseSympOld_EventMakeText(int intOption, string strMacro)
        {
            TextBox txt = (TextBox) panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta1" : "I0000000981", true)[0];
            if (intOption == 0)
            {
                txt.Text = "";
                txt.Text = strMacro;
            }
            else
            {
                //txtProgress.Text = txtProgress.Text + " " + strMacro;
                int selstart = txt.SelectionStart;
                int intMacro = strMacro.Length;
                txt.Text = txt.Text.Insert(selstart, " " + strMacro);
                txt.Focus();
                txt.SelectionStart = selstart + intMacro + 1;
            }

            //GetMeCroTitle();
        }

        private void FEmrMacro_FormClosed(object sender, FormClosedEventArgs e)
        {
            fEmrMacro.Dispose();
            fEmrMacro = null;
        }

        //private void frmEmrBaseSympOld_EventClosed()
        //{
        //    fEmrMacro.Dispose();
        //    fEmrMacro = null;
        //    //GetMeCroTitle();
        //}

        private void GetMeCro(TextBox txtProgress, string SYSMPNAME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SYSMPNAME, SYSMPRMK";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRSYSMP";
                SQL = SQL + ComNum.VBLF + " WHERE SYSMPNAME = '" + SYSMPNAME.Replace("'", "`") + "'";
                SQL = SQL + ComNum.VBLF + "   AND SYSMPGB = '" + clsType.User.DrCode + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt.Rows.Count ==  1)
                {
                    int selstart = txtProgress.TextLength;
                    txtProgress.Text = txtProgress.Text.Insert(selstart, " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim()  + " " );
                    txtProgress.Focus();
                    txtProgress.SelectionStart = txtProgress.TextLength + 2;
                    txtProgress.SelectionLength = 0;
                }
                else
                {
                    clsEmrFunc.DspControlF12(clsDB.DbCon, this, panChart, dt, ssMacroWord, txtProgress);
                }

                //txtProgress.Text = txtProgress.Text + " " + dt.Rows[0]["SYSMPRMK"].ToString().Trim();
                

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        #endregion

        /// <summary>
        /// TextBox Event
        /// </summary>
        /// <param name="objParent"></param>
        private void pAddEventToText(Control objParent)
        {
            Control[] controls = FormFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is TextBox)
                {
                    string strTag = string.Empty;


                    if (((TextBox)control).Tag != null)
                    {
                        strTag = ((TextBox)control).Tag.ToString();  
                        if(strTag.Equals("TIME"))
                        {
                            ((TextBox)control).MaxLength = 5;
                            ((TextBox)control).PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(pTextBox_PreviewKeyDown);
                            ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyDown);
                            ((TextBox)control).Leave += new System.EventHandler(pTextBox_Leave);
                        }
                        else if (strTag == "DATE")
                        {
                            ((TextBox)control).PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(pTextBox_PreviewKeyDown);
                            ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyDown);
                            ((TextBox)control).Click += new System.EventHandler(pCalTextBox_Click);
                            ((TextBox)control).DoubleClick += new System.EventHandler(pCalTextBox_DoubleClick);
                        }
                        else if(strTag == "DIAGSPD")
                        {
                            ((TextBox)control).PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(pTextBox_PreviewKeyDown);
                            ((TextBox)control).KeyUp += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyUp);
                            ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                            ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);
                            ((TextBox)control).Enter += new System.EventHandler(pTextBox_Enter);
                            //((TextBox)control).Leave += new System.EventHandler(pTextBox_Leave);
                        }
                        else if(strTag == "Set_FormPatInfo_Er_Trans")
                        {
                            ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pCalTextBox_KeyDown);
                            ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                            ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);
                        }
                        else
                        {
                            ((TextBox)control).PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(pTextBox_PreviewKeyDown);
                            ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyDown);
                            ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                            ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);

                            if (((TextBox)control).Multiline == true)
                            {
                                ((TextBox)control).ScrollBars = ScrollBars.None;
                                ((TextBox)control).MouseWheel += ChartTextControlMouseWheel;
                                ((TextBox)control).TextChanged += new System.EventHandler(pTextBox_TextChanged);
                            }
                        }
                    }
                    else
                    {
                        ((TextBox)control).PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(pTextBox_PreviewKeyDown);
                        ((TextBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(pTextBox_KeyDown);
                        ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                        ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);
                        if (((TextBox)control).Multiline == true)
                        {
                            ((TextBox)control).ScrollBars = ScrollBars.None;
                            ((TextBox)control).MouseWheel += ChartTextControlMouseWheel;
                            ((TextBox)control).TextChanged += new System.EventHandler(pTextBox_TextChanged);
                        }
                    }
                }
            }
        }

        //=================

        /// <summary>
        /// 텍스트 박스 마우스 휠 이벤트 
        /// </summary>
        private void ChartTextControlMouseWheel(object sender, MouseEventArgs e)
        {
            //long maxValue = PanelHeight - ChartHeight;

            if (e.Delta <= 0)
            {
                if (panChart.VerticalScroll.Value + 100 < panChart.VerticalScroll.Maximum)
                {
                    panChart.VerticalScroll.Value += 100;
                }
                //else
                //{
                //    pnlChart.VerticalScroll.Value -= 0;
                //}
            }
            else
            {
                if (panChart.VerticalScroll.Value - 100 <= 0)
                {
                    panChart.VerticalScroll.Value = 0;
                }
                else
                {
                    panChart.VerticalScroll.Value -= 100;
                }
            }

            Console.WriteLine(e.Delta);
        }



        private void pTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
            {
                return;
            }

            ssMacroWord.Visible = false;

            string strTag = (sender as TextBox).Tag.ToString();

            if ((sender as TextBox).Multiline == false && (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return))
            {
                if (strTag.Equals("TIME"))
                {
                    clsEmrFunc.AutoTimeText((sender as TextBox));
                    //return;
                }
                else if(strTag.Equals("GetSabunName"))
                {
                    (sender as TextBox).Text = clsVbfunc.GetInSaName(clsDB.DbCon, (sender as TextBox).Text.Trim());
                }
                else if(strTag.IndexOf("Set_NextFocus") != -1)
                {
                    clsEmrFunc.Set_NextFocus(panChart, strTag);
                    return;
                }
                else if (strTag.IndexOf("Set_GetSabunBuseName") != -1)
                {
                    clsEmrFunc.Set_GetSabunBuseName(clsDB.DbCon, this, strTag);
                    return;
                }
                else if (strTag.Equals("DIAGSPD") && pForm.FmOLDGB == 0)
                {
                    TextBox text = (sender as TextBox);
                    if (text.Name.IndexOf("_") != -1 && (text.Name.IndexOf("I0000036383") != -1 || text.Name.IndexOf("I0000031714") != -1))
                    {
                        double tIndex = VB.Val(text.Name.Split('_')[1]);
                        Control control = panChart.Controls.Find(text.Name.Split('_')[0] + "_" +  (tIndex + 1), true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Focus();
                            return;
                        }

                        if (IsDiagCode(text) == false)
                        {
                            return;
                        }
                    }
                }

                SelectNextControl((Control) sender , true, true, true, false);
            }

            if (e.KeyCode == Keys.Tab)
            {
                SetFocusNextControl(((TextBox)sender).Name);
                return;
            }
        }

        private void pCalTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (mLoading && string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
            {
                return;
            }

            ssMacroWord.Visible = false;
            mCalControl = (TextBox)sender;
        }

        private void pCalTextBox_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
                return;

            mCalControl = (TextBox)sender;
            ssMacroWord.Visible = false;
        }

        private void pCalTextBox_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
                return;


            mCalControl = (TextBox)sender;
            ssMacroWord.Visible = false;

            if (frmEmrCaledarEvent == null)
            {
                frmEmrCaledarEvent = new frmEmrCaledar();
                frmEmrCaledarEvent.rSetClalendaInfo += new frmEmrCaledar.SetClalendaInfo(frmEmrCaledarEvent_SetClalendaInfo);
                frmEmrCaledarEvent.rEventClosed += new frmEmrCaledar.EventClosed(frmEmrCaledarEvent_EventClosed);
            }
            frmEmrCaledarEvent.ShowDialog();
        }

        private void pTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tx = (TextBox)sender;
            if (tx.Tag == null) return;
            
            if (tx.Tag.ToString().Trim().ToUpper() == "FALSE") return;
            if (tx.Tag.ToString().Trim() == "")
            {
                if (tx.Height <= 40)
                {
                    return;
                }
            }

            int OldHeight = tx.Height;

            ChengeHeight(tx);

            if (mLoading == false || panChart.AutoScrollPosition.IsEmpty)
                return;

            try
            {
                if (tx.Height > OldHeight && panChart.VerticalScroll.Maximum >= panChart.VerticalScroll.Value + (tx.Height - OldHeight))
                {
                    panChart.VerticalScroll.Value += tx.Height - OldHeight;
                }
            }
            catch { }
        }

        private float GetSize(TextBox cTextBox)
        {
            Font font = new Font("맑은 고딕", cTextBox.Font.Size);
            Image fakeImage = new Bitmap(1, 1); //As we cannot use CreateGraphics() in a class library, so the fake image is used to load the Graphics.
            Graphics graphics = Graphics.FromImage(fakeImage);
            SizeF size = graphics.MeasureString("Hello", font);

            return size.Height;
        }

        /// <summary>
        /// TextBox Height 조절
        /// 컨트롤은 Dock FIll 이 되어 있어야함
        /// </summary>
        /// <param name="cTextBox"></param>
        private void ChengeHeight(TextBox cTextBox)
        {
            Control pCon = cTextBox.Parent;
            //Control MaxParentCon = pCon.Parent.Name.Equals("panChart") ? pCon : FindMaxParentCon(pCon);

            //int pConHeight = (int)VB.Val(mFormXml.Where(ct => ct.strCONTROLNAME.Equals(pCon.Name)).ToList()[0].strSIZEHEIGHT);
            //int MaxParentConHeight = (int)VB.Val(mFormXml.Where(ct => ct.strCONTROLNAME.Equals(MaxParentCon.Name)).ToList()[0].strSIZEHEIGHT);

            int beforeTextBoxHeight = cTextBox.Height;

            //if (pCon.Parent.Name == "panChart")
            //{
            //    MaxParentCon = pCon;
            //}
            //else
            //{
            //    MaxParentCon = FindMaxParentCon(pCon);
            //}

            int afterTextBoxHeight = CalcHeght(cTextBox);
            //int PContHeight = pCon.Height;

            #region 이전 
            //if (afterTextBoxHeight < 40)
            //{
            //    afterTextBoxHeight = 40;
            //    if (MaxParentCon != pCon)
            //    {
            //        if (pCon.Dock == DockStyle.Top)
            //        {
            //            pCon.Height = pCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
            //        }
            //    }
            //    MaxParentCon.Height = MaxParentCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
            //}
            //else
            //{
            //    if (afterTextBoxHeight > beforeTextBoxHeight)
            //    {
            //        if (MaxParentCon != pCon)
            //        {
            //            if (pCon.Dock == DockStyle.Top)
            //            {
            //                pCon.Height = pCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
            //            }
            //        }
            //        MaxParentCon.Height = MaxParentCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
            //    }
            //    else if (afterTextBoxHeight < beforeTextBoxHeight)
            //    {
            //        if (MaxParentCon != pCon)
            //        {
            //            if (pCon.Dock == DockStyle.Top)
            //            {
            //                pCon.Height = pCon.Height - (beforeTextBoxHeight - afterTextBoxHeight);
            //            }
            //        }
            //        MaxParentCon.Height = MaxParentCon.Height - (beforeTextBoxHeight - afterTextBoxHeight);
            //    }
            //}
            #endregion

            #region new 

            //Size size = TextRenderer.MeasureText(cTextBox.Text, cTextBox.Font, cTextBox.Size, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis);
            int pTextHeight = (int)VB.Val(mFormXml.Where(ct => ct.strCONTROLNAME.Equals(cTextBox.Name)).ToList().First().strSIZEHEIGHT);
            pTextHeight = pTextHeight == 0 ? 44 : pTextHeight;

            cTextBox.SuspendLayout();
            while (true)
            {
                if (afterTextBoxHeight < pTextHeight)
                {
                    afterTextBoxHeight = pTextHeight;
                    pCon.Height += (afterTextBoxHeight - beforeTextBoxHeight);
                }
                else
                {
                    if (afterTextBoxHeight > beforeTextBoxHeight)
                    {
                        pCon.Height += (afterTextBoxHeight - beforeTextBoxHeight) + 30;
                    }
                    else if (afterTextBoxHeight < beforeTextBoxHeight)
                    {
                        pCon.Height -= (beforeTextBoxHeight - afterTextBoxHeight) - 30;
                    }
                    //else if (beforeTextBoxHeight < (size.Height)) 
                    //{
                    //    pCon.Height += (size.Height - beforeTextBoxHeight);
                    //}
                }

                if (pCon.Parent == null || pCon.Parent.Equals(panChart))
                {
                    break;
                }
                pCon = pCon.Parent;
            }

            cTextBox.ResumeLayout();
            #endregion
        }

        private Control FindMaxParentCon(Control cCon)
        {
            Control pCon = cCon.Parent;

            if (pCon.Parent.Name.Equals("panChart") == false)
            {
                pCon = FindMaxParentCon(pCon);
            }
            return pCon;
        }

        /// <summary>
        /// TextBox 사이즈 측정 
        /// 버거 땜시 하드코딩해서 사이즈 마춤
        /// </summary>
        /// <param name="cTextBox"></param>
        /// <returns></returns>
        private int CalcHeght(TextBox cTextBox)
        {
            var numberOfLines = SendMessage(cTextBox.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0);
            //float intHeight = GetSize(cTextBox);

            int FontHeight = cTextBox.Font.Height;
            double fHeight = 0;

            //if (numberOfLines <= 4)
            //{
            //    fHeight = cTextBox.Font.Height;
            //}
            //else if (numberOfLines > 4 && numberOfLines <= 10)
            //{
            //    fHeight = FontHeight - 1;
            //}
            //else if (numberOfLines > 10 && numberOfLines <= 20)
            //{
            //    fHeight = FontHeight - 1.3;
            //}
            //else if (numberOfLines > 20 && numberOfLines <= 24)
            //{
            //    fHeight = FontHeight - 1.5;
            //}
            //else if (numberOfLines > 24 && numberOfLines <= 30)
            //{
            //    fHeight = FontHeight - 1.6;
            //}
            //else if (numberOfLines > 30 && numberOfLines <= 50)
            //{
            //    fHeight = FontHeight - 1.8;
            //}
            //else if (numberOfLines > 50 && numberOfLines <= 180)  //80
            //{
            //    fHeight = FontHeight - 1.9;
            //}
            //else if (numberOfLines > 180)  //80
            //{
            //    fHeight = FontHeight - 2;
            //}

            if (numberOfLines <= 4)
            {
                fHeight = cTextBox.Font.Height;
            }
            else if (numberOfLines > 4 && numberOfLines <= 10)
            {
                fHeight = FontHeight - 2;
            }
            else if (numberOfLines > 10 && numberOfLines <= 20)
            {
                fHeight = FontHeight - 2.5;
            }
            else if (numberOfLines > 20 && numberOfLines <= 30)
            {
                fHeight = FontHeight - 2.5;
            }
            else if (numberOfLines > 30 && numberOfLines <= 40)
            {
                fHeight = FontHeight - 2.5;
            }
            else if (numberOfLines > 40 && numberOfLines <= 50)
            {
                fHeight = FontHeight - 2.5;
            }
            else if (numberOfLines > 50 && numberOfLines <= 60)
            {
                fHeight = FontHeight - 2.6;
            }
            else if (numberOfLines > 60 && numberOfLines <= 90)
            {
                fHeight = FontHeight - 2.7;
            }
            else if (numberOfLines > 90 && numberOfLines <= 130)  
            {
                fHeight = FontHeight - 2.9;
            }
            else if (numberOfLines > 130 && numberOfLines <= 400)  
            {
                fHeight = FontHeight - 2.9;
            }
            else if (numberOfLines > 400)
            {
                fHeight = FontHeight - 3.0;
            }

            int cHeight = (int)(fHeight * numberOfLines) + 2;
            return cHeight;
        }

        private void frmEmrCaledarEvent_SetClalendaInfo(string strDate)
        {
            frmEmrCaledarEvent.Dispose();
            frmEmrCaledarEvent = null;

            if (strDate.Trim() == "")
            {
                return;
            }
            mCalControl.Text = strDate;
        }

        private void frmEmrCaledarEvent_EventClosed()
        {
            frmEmrCaledarEvent.Dispose();
            frmEmrCaledarEvent = null;
        }
        //=================

        private void pTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            ssMacroWord.Visible = false;
            mControl = (TextBox)sender;

            string strText = string.Empty;
            if (e.KeyCode == Keys.F2)
            {
                //strText = clsEngToKor.UDF_Eng2Han(strText);
                strText = ((TextBox)sender).SelectedText.Trim();
                strText = EngHanConv.Eng2Kor(strText);
                ((TextBox)sender).SelectedText = strText;
            }
            else if (e.KeyCode == Keys.F3)
            {
                strText = ((TextBox)sender).SelectedText.Trim();
                strText = EngHanConv.Kor2Eng(strText);
                ((TextBox)sender).SelectedText = strText;
            }
            //19-08-12 TF팀 요구사항으로 추가함.
            else if(e.KeyCode == Keys.F5)
            {
                ((TextBox)sender).Clear();
            }
            //19-09-07- OS 황성현 레지던트 요청.
            else if (e.KeyCode == Keys.F12)
            {
                ssMacroWord.Visible = false;
                ssDiag.Visible = false;

                if (mControl.Text.IndexOf(" ") == -1)
                    return;

                string sText = mControl.Text.Substring(mControl.Text.TrimEnd().LastIndexOf(" ")).Trim();

                mControl.Text = mControl.Text.Substring(0, mControl.Text.LastIndexOf(" "));

                GetMeCro((TextBox)mControl, sText);
            }
        }

        private void pTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (ssDiag.Visible == true && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                ssDiag.Focus();
                //ssDiag_Sheet1.SetActiveCell(0, 0);
                return;
            }

            mDiagCode = (TextBox)sender;

            Control DiagPOA = null;

            //신규 서식
            if (pForm.FmOLDGB == 0)
            {
                DiagPOA = mDiagCode.Parent.Controls.OfType<TextBox>().Where(t => t.Left >= 500).First();
                mDiagName = (sender as TextBox).Parent.Controls.OfType<TextBox>().Where(t => t.Width >= 300).First();
                mDiagCode = (sender as TextBox).Parent.Controls.OfType<TextBox>().Where(t => t.Left <= 30).First();

            }

            Control NowControl = pForm.FmOLDGB == 1 ? mDiagCode : (sender as TextBox);

            if ( NowControl.Text.Length < 2)
                return;

            ssMacroWord.Visible = false;
            ssDiag.Visible = false;

            if (DiagPOA != null && string.IsNullOrWhiteSpace(DiagPOA.Text))
            {
                DiagPOA.Text = "Y";
            }

            clsEmrFunc.DiagControl(clsDB.DbCon, panChart, NowControl.Text, ssDiag, NowControl, NowControl.Name.Equals("I0000028889") || NowControl.Name.IndexOf("I0000036383") != -1 ? "Code" : strDiagCode);
        }

        private void pTextBox_Click(object sender, EventArgs e)
        {
            if (ssMacroWord.Visible)
            {
                ssMacroWord.Visible = false;
                return;
            }

            if (ssDiag.Visible)
            {
                ssDiag.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
                return;

            lastScrollValue = panChart.AutoScrollPosition.Y < 0 ? panChart.AutoScrollPosition.Y * -1 : panChart.AutoScrollPosition.Y;

            if (((TextBox)sender).Focused == false)
            {
                return;
            }

            mControl = (TextBox)sender;

            clsEmrFunc.DspControl(clsDB.DbCon, this, panChart, mstrFormNo, ssMacroWord, mControl);
            lastScrollValue = panChart.AutoScrollPosition.Y < 0 ? panChart.AutoScrollPosition.Y * -1 : panChart.AutoScrollPosition.Y;
        }

        private void pTextBox_DoubleClick(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
            ssDiag.Visible = false;

            if (string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
                return;

            mControl = (TextBox)sender;
 
            pLoadMacro(mControl);
            return;
        }

        private void pTextBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).Select(0, 0);
            return;
        }

        private void pTextBox_Leave(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
            ssDiag.Visible = false;

            if (((TextBox)sender).Tag == null || string.IsNullOrEmpty(strSaveFlag) && clsType.User.AuAMANAGE.Equals("1"))
                return;

            if ((sender as TextBox).Tag.ToString().Equals("TIME"))
            {
                clsEmrFunc.AutoTimeText((sender as TextBox));
                return;
            }
            return;
        }

        /// <summary>
        /// TextBox 멀티라인 높이 조절 : 사용안함
        /// </summary>
        private void pMultiTextHeigh()
        {
            for (int i = 0; i < mFormXml.Length; i++)
            {
                if (mFormXml[i].strCONTROTYPE != "System.Windows.Forms.TextBox")
                {
                    continue;
                }

                if (mFormXml[i].strMULTILINE.ToUpper() != "TRUE")
                {
                    continue;
                }

                if (mFormXml[i].strAUTOHEIGH.ToUpper() != "TRUE")
                {
                    continue;
                }

                Control[] tx = null;
                TextBox cTextBox = null;

                tx = this.Controls.Find(mFormXml[i].strCONTROLNAME, true);
                if (tx != null)
                {
                    if (tx.Length > 0)
                    {
                        if (tx[0] is TextBox)
                        {
                            cTextBox = (TextBox)tx[0];
                            if (cTextBox.Text.Trim() == "")
                            {
                                if (cTextBox.Height > 60)
                                {
                                    SetMultiTexBoxtLoop(cTextBox, 60);
                                }
                            }
                            else
                            {
                                int TextHeight = (int)(cTextBox.Lines.Length * cTextBox.Font.Size);
                                int TextBoxHeight = cTextBox.Height;
                                if (TextHeight < TextBoxHeight)
                                {
                                    SetMultiTexBoxtLoop(cTextBox, TextHeight + 10);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 멀티라인 텍스트 박스 높이를 조절한다
        /// </summary>
        /// <param name="cCont"></param>
        /// <param name="pCont"></param>
        /// <param name="Height"></param>
        private void SetMultiTexBoxtLoop(Control cCont, int Height)
        {
            return;
            //cCont.Height = Height;
            //if (cCont.Dock == DockStyle.Fill)
            //{
            //    return;
            //}
            //Control pCont = null;
            //pCont = cCont.Parent;
            //if (pCont.Name == "panChart")
            //{
            //    return;
            //}
            //pCont.Height = pCont.Height - Height;
            //return;

            //int pHeght = 0;
            //pHeght = pCont.Height - Height;

            //SetMultiTexBoxtLoop(pCont, Height + 2);
        }

        /// <summary>
        /// Next Tab로 포커스 이동
        /// </summary>
        /// <param name="strConNm"></param>
        private void SetFocusNextControl(string strConNm)
        {
            return;

            //for (int i = 0; i < mFormXml.Length; i++)
            //{
            //    if (mFormXml[i].strCONTROLNAME == strConNm)
            //    {
            //        int intTab = (int)VB.Val(mFormXml[i].strTABORDER);
            //        for (int j = 0; j < mFormXml.Length; j++)
            //        {
            //            if (((int)VB.Val(mFormXml[j].strTABORDER) == intTab + 1) && mFormXml[j].strCONTROTYPE == "System.Windows.Forms.TextBox")
            //            {
            //                Control[] tx = null;

            //                tx = panChart.Controls.Find(mFormXml[j].strCONTROLNAME, true);
            //                if (tx != null)
            //                {
            //                    if (tx.Length > 0)
            //                    {
            //                        ((TextBox)tx[0]).Focus();
            //                        return;
            //                    }
            //                }
            //                else
            //                {
            //                    return;
            //                }
            //                break;
            //            }
            //        }
            //        break;
            //    }
            //}
        }

        #endregion //폼생성후 기본 이벤트 매핑

        #region //폼생성후 상용구관련 : 스프래드 생성등

        /// <summary>
        /// 수술/검사 등등 스프레드 생성.
        /// </summary>
        private void pAddOpListSpd()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color333635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text397635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();

            ssOpList = new FarPoint.Win.Spread.FpSpread();
            ssOpList_Sheet1 = new FarPoint.Win.Spread.SheetView();

            // ssMacroWord
            this.ssOpList.AccessibleDescription = string.Empty;
            this.ssOpList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssOpList.Location = new System.Drawing.Point(3, 6);
            this.ssOpList.Name = "ssOpList";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            this.ssOpList.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2});
            this.ssOpList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssOpList_Sheet1});
            this.ssOpList.Size = new System.Drawing.Size(250, 200);
            this.ssOpList.TabIndex = 105;
            this.ssOpList.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssOpList.TextTipAppearance = tipAppearance1;
            this.ssOpList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssOpList.Visible = false;
            this.ssOpList.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssOpList_CellDoubleClick);
            //this.ssDiag.Leave += new System.EventHandler(this.ssMacroWord_Leave);
            // 
            // ssMacroWord_Sheet1
            // 
            this.ssOpList_Sheet1.Reset();
            this.ssOpList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssOpList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssOpList_Sheet1.ColumnCount = 4;
            ssOpList_Sheet1.RowCount = 1;
            this.ssOpList_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssOpList_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssOpList_Sheet1.ColumnHeader.Visible = true;
            this.ssOpList_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssOpList_Sheet1.DefaultStyleName = "Text397635194368298125000";
            this.ssOpList_Sheet1.GrayAreaBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssOpList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssOpList_Sheet1.RowHeader.Visible = false;
            this.ssOpList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            this.ssOpList_Sheet1.Visible = true;
            this.ssOpList.Visible = false;

            ssOpList_Sheet1.ColumnCount = 4;
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.Multiline = false;
            TypeText.MaxLength = 255;

            ssOpList_Sheet1.Columns[0].CellType = TypeText;
            ssOpList_Sheet1.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssOpList_Sheet1.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssOpList_Sheet1.Columns[0].Width = 60;
            ssOpList_Sheet1.Columns[0].Locked = true;
            ssOpList_Sheet1.Columns[0].Label = pForm.FmFORMNO == 2465 && pForm.FmOLDGB == 1 ? "검사일자" : "수술일자";
            //ssSpd.ActiveSheet.Columns[0].Visible = true;

            ssOpList_Sheet1.Columns[1].CellType = TypeText;
            ssOpList_Sheet1.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssOpList_Sheet1.Columns[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssOpList_Sheet1.Columns[1].Width = 60;
            ssOpList_Sheet1.Columns[1].Locked = true;
            ssOpList_Sheet1.Columns[1].Label = pForm.FmFORMNO == 2465 && pForm.FmOLDGB == 1 ? "감염" : "시작시간";

            ssOpList_Sheet1.Columns[2].CellType = TypeText;
            ssOpList_Sheet1.Columns[2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssOpList_Sheet1.Columns[2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssOpList_Sheet1.Columns[2].Width = 60;
            ssOpList_Sheet1.Columns[2].Locked = true;
            ssOpList_Sheet1.Columns[2].Label = pForm.FmFORMNO == 2465 && pForm.FmOLDGB == 1 ? "" : "종료시간";

            ssOpList_Sheet1.Columns[3].CellType = TypeText;
            ssOpList_Sheet1.Columns[3].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssOpList_Sheet1.Columns[3].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssOpList_Sheet1.Columns[3].Width = 60;
            ssOpList_Sheet1.Columns[3].Locked = true;
            ssOpList_Sheet1.Columns[3].Label = "저장";

            Controls.Add(this.ssOpList);
        }

        private void ssOpList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string str1 = ssOpList_Sheet1.Cells[e.Row, 0].Text.Trim();
            string str2 = ssOpList_Sheet1.Cells[e.Row, 1].Text.Trim();
            string str3 = ssOpList_Sheet1.Cells[e.Row, 2].Text.Trim();
            string strVal = ssOpList_Sheet1.Cells[e.Row, 2].Tag != null ? ssOpList_Sheet1.Cells[e.Row, 2].Tag.ToString().Trim() : string.Empty;
            string strVal2 = ssOpList_Sheet1.Cells[e.Row, 1].Tag != null ? ssOpList_Sheet1.Cells[e.Row, 1].Tag.ToString().Trim() : string.Empty;

            string strCerti = ssOpList_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (string.IsNullOrEmpty(str1)) return;

            if (strCerti.Equals("임시"))
            {
                ComFunc.MsgBoxEx(this, "인증 저장된 항목을 선택하세요.");
                return;
            }

            if (ssOpList_Sheet1.Cells[e.Row, 0].Tag != null)
            {
                if (fEmrChartNewForm != null)
                {
                    fEmrChartNewForm.Dispose();
                    fEmrChartNewForm = null;
                }

                fEmrChartNewForm = new frmEmrChartNew(ssOpList_Sheet1.Cells[e.Row, 0].Tag.ToString(), ssOpList_Sheet1.Cells[e.Row, 1].Tag.ToString(), pAcp, strVal, "V");
                fEmrChartNewForm.StartPosition = FormStartPosition.CenterParent;
                fEmrChartNewForm.FormClosed += FEmrChartNewForm_FormClosed;
                fEmrChartNewForm.Show(this);
                return;
            }

            switch(mstrFormNo)
            {

                case "2264":
                case "2242":
                case "2265":
                case "1939":
                case "2289":
                case "1258":
                case "2084":
                case "2068":
                case "2150":
                case "1947":
                case "1570":
                case "2236":
                case "2073":
                case "2130":
                case "2144": 
                    clsEmrFunc.setChartFormValue2(pForm, panChart, str1, str2, str3);
                      break;
                case "2308":
                case "2309":
                case "2610":
                    if (pForm.FmOLDGB == 1)
                    {
                        clsEmrFunc.setChartFormValue7(panChart, str1, str2, str3);
                    }
                    else if(pForm.FmFORMNO == 2610)
                    {
                        clsEmrFunc.setChartFormValue2610_New(clsDB.DbCon, panChart, pAcp.ptNo, str1, strVal2);
                    }
                    break;
                case "2611":
                    if (pForm.FmOLDGB == 1)
                    {
                        clsEmrFunc.setChartFormValue7(panChart, str1, str2, str3);
                    }
                    else
                    {
                        clsEmrFunc.setChartFormValue2611_New(clsDB.DbCon, panChart, pAcp.ptNo, str1, strVal2);
                    }
                    break;
                case "1544":
                    clsEmrFunc.setChartFormValue1544_New(clsDB.DbCon, panChart, pAcp.ptNo, str1, strVal2);
                    break;
                case "2465":
                    if (pForm.FmOLDGB != 1)
                    {
                        clsEmrFunc.setChartFormValue2465_New(clsDB.DbCon, panChart, pAcp.ptNo, str1, strVal2);
                    }
                    break;
                case "2463":
                    clsEmrFunc.setChartFormValue9(clsDB.DbCon, panChart, pAcp.ptNo, str1, pForm);
                    break;
                case "2467":
                case "2636":
                    clsEmrFunc.setChartFormValue2467(clsDB.DbCon, panChart, strVal, strVal2, pForm);
                    break;
                case "1808":
                    clsEmrFunc.setChartFormValue1808(clsDB.DbCon, panChart, strVal, strVal2, pForm);
                    break;
                case "2618":
                    clsEmrFunc.setChartFormValue2618(clsDB.DbCon, pForm, panChart, pAcp.ptNo , str1, strVal2);
                    break;
                case "2644":
                    clsEmrFunc.setChartFormValue2644_New(clsDB.DbCon, panChart, pAcp.ptNo, str1, strVal2);
                    break;
                case "2280":
                    clsEmrFunc.setChartFormValue3(clsDB.DbCon, this, pAcp, "", pForm, strVal);
                    break;
                case "2323":
                    clsEmrFunc.GetTransferHistory2(clsDB.DbCon, this, pAcp, "", pForm, strVal);
                    break;
            }


            ssOpList.Visible = false;

            if (panChart.VerticalScroll.Value == lastScrollValue)
                return;

            panChart.VerticalScroll.Value = lastScrollValue;
        }

        /// <summary>
        /// 진단명 검색 스프레드 생성.
        /// </summary>
        private void pAddDiagSpd()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color333635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text397635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();

            ssDiag = new FarPoint.Win.Spread.FpSpread();
            ssDiag_Sheet1 = new FarPoint.Win.Spread.SheetView();

            // ssMacroWord
            this.ssDiag.AccessibleDescription = string.Empty;
            this.ssDiag.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssDiag.Location = new System.Drawing.Point(3, 6);
            this.ssDiag.Name = "ssDiag";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            this.ssDiag.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2});
            this.ssDiag.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssDiag_Sheet1});
            this.ssDiag.Size = new System.Drawing.Size(62, 20);
            this.ssDiag.TabIndex = 105;
            this.ssDiag.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssDiag.TextTipAppearance = tipAppearance1;
            this.ssDiag.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssDiag.Visible = false;
            this.ssDiag.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssDiag_CellDoubleClick);
            this.ssDiag.KeyDown += SsDiag_KeyDown;
            //this.ssDiag.Leave += new System.EventHandler(this.ssMacroWord_Leave);
            // 
            // ssMacroWord_Sheet1
            // 
            this.ssDiag_Sheet1.Reset();
            this.ssDiag_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssDiag_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssDiag_Sheet1.ColumnCount = 1;
            ssDiag_Sheet1.RowCount = 1;
            this.ssDiag_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssDiag_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssDiag_Sheet1.ColumnHeader.Visible = false;
            this.ssDiag_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssDiag_Sheet1.DefaultStyleName = "Text397635194368298125000";
            this.ssDiag_Sheet1.GrayAreaBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssDiag_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssDiag_Sheet1.RowHeader.Visible = false;
            this.ssDiag_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;

            Controls.Add(this.ssDiag);
        }

        private void SsDiag_KeyDown(object sender, KeyEventArgs e)
        {
            if (ssDiag_Sheet1.RowCount == 0)
                return;

            if(e.KeyCode != Keys.Enter)
            {
                return;
            }

            ssDiag.Visible = false;

            string DiagCode = Regex.Replace(ssDiag_Sheet1.Cells[ssDiag_Sheet1.ActiveRowIndex, 0].Text.Trim(), @"[^ a-zA-Z0-9]", "");
            string DiagName = ssDiag_Sheet1.Cells[ssDiag_Sheet1.ActiveRowIndex, 1].Text.Trim();

            if (pForm.FmOLDGB == 1)
            {
                mDiagCode.Text = DiagCode + "  :" + DiagName;
            }
            else
            {
                mDiagCode.Text = DiagCode;
                mDiagName.Text = DiagName;
            }

            Control DiagPOA = mDiagCode.Parent.Controls.OfType<TextBox>().Where(t => t.Left >= 500).First();
            if (DiagPOA != null && !string.IsNullOrWhiteSpace(DiagCode) && FormPatInfoFunc.Set_FormPatInfo_IsPoaException(clsDB.DbCon, DiagCode))
            {
                DiagPOA.Text = "E";
            }
            else
            {
                DiagPOA.Text = "Y";
            }

            if (panChart.VerticalScroll.Value == lastScrollValue)
                return;

            panChart.VerticalScroll.Value = lastScrollValue;
        }

        private void ssDiag_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        { 
            ssDiag.Visible = false;
            string DiagCode = Regex.Replace(ssDiag_Sheet1.Cells[e.Row, 0].Text.Trim(), @"[^ a-zA-Z0-9]", "");
            string DiagName = ssDiag_Sheet1.Cells[e.Row, 1].Text.Trim();

            if (pForm.FmOLDGB == 1)
            {
                mDiagCode.Text = DiagCode + "  :" + DiagName;
            }
            else
            {
                if (IsDiagCode(mDiagCode) == false)
                {
                    return;
                }

                mDiagCode.Text = DiagCode;
                mDiagName.Text = DiagName;

                if (DiagName.IndexOf("::   ") != -1)
                {
                    mDiagName.Text = DiagName.Substring(DiagName.IndexOf("::   ") + ("::   ").Length);
                }

                Control DiagPOA = mDiagCode.Parent.Controls.OfType<TextBox>().Where(t => t.Left >= 500).First();
                if (DiagPOA != null && !string.IsNullOrWhiteSpace(DiagCode) && FormPatInfoFunc.Set_FormPatInfo_IsPoaException(clsDB.DbCon, DiagCode))
                {
                    DiagPOA.Text = "E";
                }
                else
                {
                    DiagPOA.Text = "Y";
                }
            }

            if (panChart.VerticalScroll.Value == lastScrollValue)
                return;

            panChart.VerticalScroll.Value = lastScrollValue;
        }

        private bool IsDiagCode(Control DiagCode)
        {
            bool rtnVal = false;

            if (string.IsNullOrWhiteSpace(DiagCode.Text))
                return true;

            #region 주진단, 부진단 같은 항목 있을시 저장 X
            string strMainCode = string.Empty;
            Control panDiag = panChart.Controls.Find("I0000028889", true).FirstOrDefault();
            if (panDiag != null)
            {
                strMainCode = panDiag.Text.Trim();
            }

            panDiag = panChart.Controls.Find("GI0000031714", true).FirstOrDefault();
            Control DualCode = FormFunc.GetAllControls(panDiag)
                .Where(
                d => d is TextBox &&
                d.Name.IndexOf("I0000036383") != -1 &&
                d.Equals(DiagCode) == false &&
                d.Text.Equals(DiagCode.Text) || !string.IsNullOrWhiteSpace(strMainCode) && d.Text.Equals(strMainCode)).FirstOrDefault();

            if (panDiag != null && DualCode != null)
            {
                DiagCode.Text = "";
                ComFunc.MsgBoxEx(this, "주진단 혹은 부진단에 같은 항목이 있습니다. 확인해주세요");
                return rtnVal;
            }
            #endregion

            rtnVal = true;
            return rtnVal;
        }

        private void pAddMacroSpd()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color333635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text397635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();

            ssMacroWord = new FarPoint.Win.Spread.FpSpread();
            ssMacroWord_Sheet1 = new FarPoint.Win.Spread.SheetView();

            // ssMacroWord
            this.ssMacroWord.AccessibleDescription = string.Empty;
            this.ssMacroWord.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMacroWord.Location = new System.Drawing.Point(3, 6);
            this.ssMacroWord.Name = "ssMacroWord";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            this.ssMacroWord.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2});
            this.ssMacroWord.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMacroWord_Sheet1});
            this.ssMacroWord.Size = new System.Drawing.Size(62, 20);
            this.ssMacroWord.TabIndex = 105;
            this.ssMacroWord.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMacroWord.TextTipAppearance = tipAppearance1;
            this.ssMacroWord.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssMacroWord.Visible = false;
            this.ssMacroWord.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMacroWord_CellClick);
            this.ssMacroWord.Leave += new System.EventHandler(this.ssMacroWord_Leave);
            // 
            // ssMacroWord_Sheet1
            // 
            this.ssMacroWord_Sheet1.Reset();
            this.ssMacroWord_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMacroWord_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssMacroWord_Sheet1.ColumnCount = 2;
            ssMacroWord_Sheet1.RowCount = 10;
            this.ssMacroWord_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.ColumnHeader.Visible = false;
            this.ssMacroWord_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.DefaultStyleName = "Text397635194368298125000";
            this.ssMacroWord_Sheet1.GrayAreaBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssMacroWord_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMacroWord_Sheet1.RowHeader.Visible = false;
            this.ssMacroWord_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;

            Controls.Add(this.ssMacroWord);
        }



        private void pLoadMacro(Control mControl)
        {
            string strConIndex = string.Empty;
            strConIndex = clsXML.IsArryCon(mControl);

            if (frmMacrowordProgEvent == null)
            {
                string strUserGb = string.Empty;
                switch(clsEmrPublic.gstrMcrAllFlag)
                {
                    //전체
                    case "1":
                        strUserGb = "A";
                        break;
                    //과별
                    case "2":
                        strUserGb = "D";
                        break;
                    //유저
                    case "3":
                        strUserGb = "U";
                        break;
                }

                frmMacrowordProgEvent = new frmEmrMacrowordProg(mControl.Name, strConIndex, mstrFormNo, "200", mstrFormText, strUserGb);
                frmMacrowordProgEvent.rSetMacro += new frmEmrMacrowordProg.SetMacro(frmMacrowordProgEvent_SetMacro);
                frmMacrowordProgEvent.rEventClosed += new frmEmrMacrowordProg.EventClosed(frmMacrowordProgEvent_EventClosed);
            }
            frmMacrowordProgEvent.ShowDialog();
        }

        private void frmMacrowordProgEvent_SetMacro(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx)
        {
            string strConIndex = string.Empty;
            strConIndex = clsXML.IsArryCon(mControl);

            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;

            if (clsEmrPublic.gstrMcrAddFlag == "1")
            {
                clsEmrFunc.MacroSpace(clsDB.DbCon, mControl, strMacro);
            }
            else
            {
                mControl.Text = strMacro;
            }

            if (mstrFormNo == "963")
                return;


            if (panChart.VerticalScroll.Value == lastScrollValue)
                return;

            panChart.VerticalScroll.Value = lastScrollValue;
        }

        private void frmMacrowordProgEvent_EventClosed()
        {
            frmMacrowordProgEvent.Dispose();
            frmMacrowordProgEvent = null;
        }


        private void ssMacroWord_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssMacroWord.Visible = false;
            string strMacro = ssMacroWord_Sheet1.Cells[e.Row, 1].Text;
            strMacro = VB.Replace(VB.Replace(strMacro, "<pre>", ""), "</pre>", "");

            clsApi.SuspendDrawing(panChart);

            if (pForm.FmFORMNO != 3541)
            {
                if (clsEmrPublic.gstrMcrAddFlag == "1")
                {
                    clsEmrFunc.MacroSpace(clsDB.DbCon, mControl, strMacro);
                }
                else
                {
                    mControl.Text = strMacro;
                }
            }
            else
            {
                mControl.Text = strMacro;
                if (mControl.Width >= 150 && mControl.Tag != null && mControl.Tag.ToString().IndexOf(":") != -1) //Scale definition
                {
                    Control ScoreText = panChart.Controls.Find(mControl.Tag.ToString().Split(':')[1], true).FirstOrDefault();
                    if (ScoreText != null && strMacro.Trim().IndexOf(":") != -1)
                    {                        
                        ScoreText.Text = strMacro.Split(':')[0].Equals("0") || strMacro.Split(':')[0].Equals("UN") ? "" : strMacro.Split(':')[0];
                    }
                }
            }


            //panChart.AutoScroll = false;

            //panChart.BringToFront();
            if (panChart.VerticalScroll.Value != lastScrollValue)
            {
                panChart.VerticalScroll.Value = lastScrollValue;
                //mControl.Focus();
            }

            //panChart.AutoScroll = true;


            //panChart.Refresh();

            clsApi.ResumeDrawing(panChart);
        }

        private void ssMacroWord_Leave(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
        }


        #endregion //폼생성후 상용구관련 : 스프래드 생성등

        #region //Private Function 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 기록지 정보를 세팅한다
        /// </summary>
        private void SetFormInfo()
        {
            pForm = null;
            pForm = clsEmrChart.ClearEmrForm();
            pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo, mstrUpdateNo);
        }

        /// <summary>
        /// 기록지 신규 작성을 위해 클리어
        /// </summary>
        public void pClearForm()
        {
            //모든 컨트롤을 초기화 한다.
            ComFunc.SetAllControlClearEx(this);
            //시간 세팅을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));

            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;

            if (mFormXml == null || mLoading == false || panChart.Controls.Count == 0)
                return;

            if (mFormXml.Any(d => d.strCONTROTYPE.Equals("System.Windows.Forms.PictureBox")) == false)
                return;

            List<FormXml> mPicture = mFormXml.Where(d => d.strCONTROTYPE.Equals("System.Windows.Forms.PictureBox")).ToList();
            if (mPicture.Count > 0)
            {
                for (int i = 0; i < mPicture.Count; i++)
                {
                    Control control = panChart.Controls.Find(mPicture[i].strCONTROLNAME, true).First();
                    if (control != null)
                    {
                        if ((control as PictureBox).Image != null)
                        {
                            if (mPicture[i].imgIMAGE == null)
                            {
                                (control as PictureBox).Image.Dispose();
                                (control as PictureBox).Image = null;
                                (control as PictureBox).ImageLocation = null;
                                (control as PictureBox).Update();
                                //(control as PictureBox).Refresh();
                            }
                            else
                            {
                                //if ((control as PictureBox).Image != null)
                                //{
                                //    (control as PictureBox).Image.Dispose();
                                //}

                                (control as PictureBox).Image = mPicture[i].imgIMAGE;
                                (control as PictureBox).Update();
                            }
                            (control as PictureBox).Tag = string.Empty;
                            (control as PictureBox).BackColor = Color.White;
                        }
                    }
                }
            }

            pClearFormExcept();
        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {

        }

        /// <summary>
        /// 사용자 템플릿 서식을 불러온다
        /// </summary>
        /// <param name="dblMACRONO"></param>
        private void pSetUserForm(double dblMACRONO)
        {
            clsXML.LoadDataUserChartRow(clsDB.DbCon, this, dblMACRONO.ToString(), false);
        }

        /// <summary>
        /// 이전 작성내역 로드
        /// </summary>
        private void LoadEmrChartInfo()
        {
            if (pForm == null)
            {
                return;
            }

            if (pForm.FmOLDGB == 1)
            {
                clsOldChart.LoadDataXMLOldChart(this, mstrEmrNo, false, true, usFormTopMenuEvent.dtMedFrDate, usFormTopMenuEvent.txtMedFrTime);
            }
            else
            {
                //변경 내역용
                if (mstrMode.Equals("H"))
                {
                    clsXML.LoadDataChartHisRow(clsDB.DbCon, this, mstrEmrNo, usFormTopMenuEvent.dtMedFrDate, usFormTopMenuEvent.txtMedFrTime);
                }
                else
                {
                    clsXML.LoadDataChartRow(clsDB.DbCon, this, mstrEmrNo, false, true, usFormTopMenuEvent.dtMedFrDate, usFormTopMenuEvent.txtMedFrTime);
                }

                GetImageLoad();
            }
        }

        /// <summary>
        /// 초진 관련 필수 입력사항 점검
        /// </summary>
        /// <returns></returns>
        private bool CheckFirstVisit()
        {
            bool rtnVal = false;

            switch (pForm.FmFORMNO)
            {
                case 2474:
                case 2475:
                case 2672:
                case 2692:
                case 2390:
                case 2746:
                    break;
                default:
                    rtnVal = true;
                    return rtnVal;
            }

            if (mstrUpdateNo.Equals("1") == false)
            {
                rtnVal = true;
                return rtnVal;
            }


            #region 통증평가 라디오 체크
            Control[] controls = null;
            int intCnt = 0;
            bool bPainYn = false;

            string OtherText = string.Empty;

            switch (pForm.FmFORMNO)
            {
                case 2474:
                case 2475:
                case 2672:
                    OtherText = "it4";
                    controls = panChart.Controls.Find("ir5", true);
                    if (controls.Length > 0 && ((RadioButton)controls[0]).Checked == false)
                    {
                        intCnt += 1;
                    }

                    controls = panChart.Controls.Find("ir6", true);
                    if (controls.Length > 0) 
                    {
                        bPainYn = ((RadioButton)controls[0]).Checked;
                        if (bPainYn == false)
                        {
                            intCnt += 1;
                        }
                    }

                    controls = panChart.Controls.Find("ir7", true);
                    if (controls.Length > 0 && ((RadioButton)controls[0]).Checked == false)
                    {
                        intCnt += 1;
                    }

                    break;
                case 2692:
                case 2390:
                case 2746:
                    if(pForm.FmFORMNO == 2692)
                    {
                        OtherText = "it5";
                    }
                    else if (pForm.FmFORMNO == 2390)
                    {
                        OtherText = "it20";
                    }
                    else
                    {
                        OtherText = "it3";
                    }

                    controls = panChart.Controls.Find("ir1", true);
                    if (controls.Length > 0 && ((RadioButton)controls[0]).Checked == false)
                    {
                        intCnt += 1;
                    }

                    controls = panChart.Controls.Find("ir2", true);
                    if (controls.Length > 0)
                    {
                        bPainYn = ((RadioButton)controls[0]).Checked;
                        if (bPainYn == false)
                        {
                            intCnt += 1;
                        }
                    }

                    controls = panChart.Controls.Find("ir3", true);
                    if (controls.Length > 0 && ((RadioButton)controls[0]).Checked == false)
                    {
                        intCnt += 1;
                    }
  
                    break;
            }

            if (intCnt == 3)
            {
                ComFunc.MsgBoxEx(this, "통증평가 항목은 필수입력 항목입니다.");
                return rtnVal;
            }
            #endregion

            #region 예 눌렀을경우 다 입력했는지 점검
            if(bPainYn == true)
            {
                controls = panChart.Controls.Find("panCheck", true);
                if (controls.Length > 0)
                {
                    //카운트 초기화
                    intCnt = 0;
                    foreach(Control control in ComFunc.GetAllControls(controls[0]))
                    {
                        if (control is TextBox)
                        {
                            if (control.Name.Equals(OtherText) == false && control.Text.Trim().Length == 0)
                            {
                                ComFunc.MsgBoxEx(this, "부위, 기간, 빈도 중에 입력안하신 항목을 입력해주세요.");
                                return rtnVal;
                            }
                        }
                        else if(control is CheckBox)
                        {
                            if (((CheckBox) control).Checked)
                            {
                                intCnt += 1;
                            }
                        }
                    }

                    if(intCnt == 0)
                    {
                        ComFunc.MsgBoxEx(this, "통증 양상 항목에 체크를 해주세요.");
                        return rtnVal;
                    }
                }
            }

            #endregion

            rtnVal = true;
            return rtnVal;
        }

        /// <summary>
        /// 입퇴원 요약지 필수 입력사항 점검
        /// </summary>
        /// <returns></returns>
        private bool CheckDischargeData()
        {
            bool rtnVal = false;

            if(pForm.FmFORMNO == 1647 || pForm.FmFORMNO == 1598)
            {
                Control[] controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "di1" : "I0000013294", true);
                if(controls.Length > 0 && string.Concat(controls[0].Text.Trim().Where(c => !char.IsWhiteSpace(c))).Length == 0)
                {
                    ComFunc.MsgBoxEx(this, "주진단명은 필수 입력 항목입니다.");
                    return rtnVal;
                }

                if (pForm.FmOLDGB == 0 )
                {
                    controls = panChart.Controls.Find("I0000036381", true);
                    if (controls.Length > 0 && string.Concat(controls[0].Text.Trim().Where(c => !char.IsWhiteSpace(c))).Length == 0)
                    {
                        ComFunc.MsgBoxEx(this, "주진단 POA는 필수 입력 항목입니다.");
                        return rtnVal;
                    }
                }
                

                #region 주증상 및 현병력
                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta2" : "I0000031713", true);
                if (controls.Length > 0 && string.IsNullOrWhiteSpace(controls[0].Text))
                {
                    ComFunc.MsgBoxEx(this, "주증상 및 현병력은 필수 입력 항목입니다.");
                    panChart.ScrollControlIntoView(controls[0]);
                    return rtnVal;
                }
                #endregion

                #region 진단명, 진단코드 POA 중에 1개라도 빠진 항목이 있는지  점검 - 주석처리
                Control panDiag = panChart.Controls.Find("GI0000031714", true).FirstOrDefault();
                if (panDiag != null)
                {
                    var lstCtrl = FormFunc.GetAllControls(panDiag)
                        .Where(d => d.Name.IndexOf("I0000036383") == -1 && d.Name.IndexOf("_") != -1)
                        .GroupBy(d => d.Name.Split('_')[1])
                        .Select(dd =>
                        new
                        {
                            Idx = dd.Key,
                            NotEmptyCount = dd.Count(ds => ds is TextBox && string.IsNullOrWhiteSpace(ds.Text.Trim()) == false)
                        }
                        );

                    if (lstCtrl.Count(d => d.NotEmptyCount > 0 && d.NotEmptyCount != 2) > 0)
                    {
                        ComFunc.MsgBoxEx(this, "부진단명 혹은 부진단 POA를 입력해주세요.");
                        return rtnVal;
                    }
                }
                #endregion

                #region di2~di9 부진단명 특수문자 검사
                if (clsType.User.DrCode.Length > 0 && pForm.FmOLDGB == 1)
                {
                    for (int i = 2; i < 10; i++)
                    {
                        controls = panChart.Controls.Find("di" + i, true);
                        if (controls.Length > 0 && controls[0].Text.IndexOf(":") > -1)
                        {
                            string strDiagCode = controls[0].Text.Trim();
                            string strDiagName = strDiagCode.Substring(strDiagCode.IndexOf(":") + 1);
                            strDiagCode = strDiagCode.Substring(0, strDiagCode.IndexOf(":"));

                            //알파벳, 숫자로 이루어진게 아니면 
                            //다른 문자열 삭제
                            if (Regex.IsMatch(strDiagCode, @"^[a-zA-Z]+[0-9]*$") == false)
                            {
                                strDiagCode = Regex.Replace(strDiagCode.Trim(), @"[^ a-zA-Z0-9]", "");
                                controls[0].Text = strDiagCode + "  :" + strDiagName;
                                controls[0].Text = controls[0].Text.Trim().Replace(Environment.NewLine, "");
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                #endregion

                #region ik1~8 체크
                bool PanCheck = false;
                bool NoCheck = false;
                List<string> lstChk = new List<string>
                {
                    {pForm.FmOLDGB == 1 ? "ik1" : "I0000013224" },
                    {pForm.FmOLDGB == 1 ? "ik2" : "I0000012982" },
                    {pForm.FmOLDGB == 1 ? "ik3" : "I0000013381" },
                    {pForm.FmOLDGB == 1 ? "ik4" : "I0000012743" },
                    {pForm.FmOLDGB == 1 ? "ik7" : "I0000031724" },
                    {pForm.FmOLDGB == 1 ? "ik5" : "I0000029916" },
                    {pForm.FmOLDGB == 1 ? "ik8" : "I0000031725" },
                    {pForm.FmOLDGB == 1 ? "ik6" : "I0000027971" }
                };

                for(int i = 0; i < lstChk.Count; i++)
                {
                    controls = panChart.Controls.Find(lstChk[i], true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            //48시간 이후 사망, 48시간 이내 사망 체크일경우
                            if(controls[0].Text.Equals("48시간이후사망") || controls[0].Text.Equals("48시간이내사망"))
                            {
                                NoCheck = true;
                            }

                            PanCheck = true;
                            break;
                        }
                    }
                }

                if(PanCheck == false)
                {
                    panChart.ScrollControlIntoView(controls[0]);
                    ComFunc.MsgBoxEx(this, "퇴원 시 환자 상태는 필수 입력 항목입니다.");
                    return rtnVal;
                }
                #endregion

                #region //ik9 ~ i12 점검
                PanCheck = false;
                if(NoCheck == false)
                {
                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik9" : "I0000013350", true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            PanCheck = true;
                        }
                    }

                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik10" : "I0000010975", true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            PanCheck = true;
                        }
                    }

                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "iK11" : "I0000013270", true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            PanCheck = true;
                        }
                    }

                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik12" : "I0000000391_1", true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            PanCheck = true;
                        }
                    }
                }

                if (NoCheck == false && PanCheck == false)
                {
                    panChart.ScrollControlIntoView(controls[0]);
                    ComFunc.MsgBoxEx(this, "퇴원 형태는 필수 입력 항목입니다.");
                    return rtnVal;
                }
                #endregion

                #region //ik13 ~ i14 점검
                PanCheck = false;
                if (NoCheck == false)
                {
                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik13" : "I0000000091", true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            PanCheck = true;
                        }
                    }

                    controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik14" : "I0000013189_1", true);
                    if (controls.Length > 0)
                    {
                        if (((CheckBox)controls[0]).Checked == true)
                        {
                            PanCheck = true;
                        }
                    }
                }

                if (NoCheck == false && PanCheck == false)
                {
                    panChart.ScrollControlIntoView(controls[0]);
                    ComFunc.MsgBoxEx(this, "추후 치료 계획은 필수 입력항목입니다.");
                    return rtnVal;
                }
                #endregion

                #region 경과요약
                if (pForm.FmOLDGB == 0)
                {
                    controls = panChart.Controls.Find("I0000007396", true);
                    if (controls.Length > 0 && string.IsNullOrWhiteSpace(controls[0].Text))
                    {
                        ComFunc.MsgBoxEx(this, "경과요약은 필수 입력항목입니다.");
                        panChart.ScrollControlIntoView(controls[0]);
                        return rtnVal;
                    }
                }

                #endregion


            }

            rtnVal = true;

            return rtnVal;
        }

        /// <summary>
        /// DATA 점검
        /// </summary>
        /// <returns></returns>
        private bool CheckData(bool blnCertYn, ref Control ActiveControl)
        {
            bool rtnVal = true;

            InitMibi();

            //인증저장인 경우는 미비체크를 한다
            if (blnCertYn == true)
            {
                if (mFormXml == null)
                    return rtnVal;

                var mibiCon = from mFormXmlLinq in mFormXml
                              where mFormXmlLinq.strMIBI == "Y"
                              select mFormXmlLinq;

                foreach (FormXml mFormXmlLinq in mibiCon)
                {
                    bool IsComp = false;
                    Control[] tx = null;
                    Control obj = null; //mtsPanel15.mPanel

                    if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.Panel" || mFormXmlLinq.strCONTROTYPE == "mtsPanel15.mPanel")
                    {
                        IsComp = CheckMibiItem(mFormXmlLinq.strCONTROLNAME);
                        if (IsComp == false)
                        {
                            tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                            if (tx.Length > 0 && tx[0].Visible == true)
                            {
                                obj = (Panel)tx[0];
                                ((Panel)obj).BackColor = Color.Pink;
                                if (ActiveControl == null)
                                {
                                    ActiveControl = obj;
                                }
                                rtnVal = false;
                                SetMibiItemBg(mFormXmlLinq.strCONTROLNAME);
                            }
                        }
                    }
                    else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.TextBox")
                    {
                        tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                        if (tx.Length > 0 && tx[0].Visible == true)
                        {
                            obj = (TextBox)tx[0];
                            if (((TextBox)obj).Text.Trim() == "")
                            {
                                ((TextBox)obj).BackColor = Color.Pink;
                                if (ActiveControl == null)
                                {
                                    ActiveControl = obj;
                                }
                                rtnVal = false;
                            }
                        }
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 아이템별 미비 체크
        /// </summary>
        /// <param name="strConName"></param>
        /// <returns></returns>
        private bool CheckMibiItem(string strConName)
        {
            bool rtnVal = false;

            var mibiCon = from mFormXmlLinq in mFormXml
                          where mFormXmlLinq.strCONTROLPARENT == strConName
                          select mFormXmlLinq;

            foreach (FormXml mFormXmlLinq in mibiCon)
            {
                Control[] tx = null;
                Control obj = null;
                if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.Panel" || mFormXmlLinq.strCONTROTYPE == "mtsPanel15.mPanel")
                {
                    rtnVal = CheckMibiItem(mFormXmlLinq.strCONTROLNAME);
                    if (rtnVal == true)
                    {
                        return rtnVal;
                    }
                }
                else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.TextBox")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (TextBox)tx[0];
                        if (((TextBox)obj).Text.Trim() != "")
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                    }
                }
                else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.CheckBox")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (CheckBox)tx[0];
                        if (((CheckBox)obj).Checked == true)
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                    }
                }
                else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.RadioButton")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (RadioButton)tx[0];
                        if (((RadioButton)obj).Checked == true)
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 미비가 있을 경우 배경색을 변경
        /// </summary>
        /// <param name="strConName"></param>
        private void SetMibiItemBg(string strConName)
        {
            var mibiCon = from mFormXmlLinq in mFormXml
                          where mFormXmlLinq.strCONTROLPARENT == strConName
                          select mFormXmlLinq;

            foreach (FormXml mFormXmlLinq in mibiCon)
            {
                Control[] tx = null;
                Control obj = null;
                if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.Panel")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (Panel)tx[0];
                        ((Panel)obj).BackColor = Color.Pink;
                    }

                    SetMibiItemBg(mFormXmlLinq.strCONTROLNAME);
                }
                else if (mFormXmlLinq.strCONTROTYPE == "mtsPanel15.mPanel")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)tx[0];
                        ((mtsPanel15.mPanel)obj).BackColor = Color.Pink;
                    }

                    SetMibiItemBg(mFormXmlLinq.strCONTROLNAME);
                }
                else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.TextBox")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (TextBox)tx[0];
                        if (((TextBox)obj).Text.Trim() == "")
                        {
                            obj.BackColor = Color.Pink;
                        }
                    }
                }
                else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.CheckBox")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (CheckBox)tx[0];
                        if (((CheckBox)obj).Checked == false)
                        {
                            obj.BackColor = Color.Pink;
                        }
                    }
                }
                else if (mFormXmlLinq.strCONTROTYPE == "System.Windows.Forms.RadioButton")
                {
                    tx = panChart.Controls.Find(mFormXmlLinq.strCONTROLNAME, true);
                    if (tx.Length > 0)
                    {
                        obj = (RadioButton)tx[0];
                        if (((RadioButton)obj).Checked == false)
                        {
                            obj.BackColor = Color.Pink;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 미비된 색을 원 상태로 되돌림
        /// </summary>
        private void InitMibi()
        {
            List<Control> controls = ComFunc.GetAllControls(panChart).Where(d => d.BackColor == Color.Pink).ToList();
            foreach (Control objControl in controls)
            {
                objControl.BackColor = Color.White;
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData(bool blnCertYn)
        {
            double dblEmrNo = 0;
            string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "");
            
            if (pForm.FmOLDGB == 1)
            {
                clsEmrType.EmrXmlImage[] pEmrXmlImage = new clsEmrType.EmrXmlImage[0];
                string strXML = clsXML.SaveDataToXmlOld(this, false, panChart, ref pEmrXmlImage, mEmrXmlImageInit, mstrFormNo);
                dblEmrNo = pSaveOldEmrData(strChartDate, strChartTime, strXML, pEmrXmlImage, mEmrXmlImageInit);
                mstrXmlInit = strXML;
            }
            else
            {
                dblEmrNo = pSaveNewEmrData(strChartDate, strChartTime, blnCertYn);
            }
                
            //전자인증 : 옮김

            return dblEmrNo;
        }

        /// <summary>
        /// 저장 : 신규서식
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="blnCertYn"></param>
        /// <returns></returns>
        private double pSaveNewEmrData(string strChartDate, string strChartTime, bool blnCertYn)
        {
            double dblEmrNo = 0;

            string strSAVEGB = "1";
            string strSAVECERT = "0";
            if (blnCertYn == true)
            {
                strSAVECERT = "1";
            }
            dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, pAcp, this, false, this,
                                                                mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                                clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", strSaveFlag);                

            return dblEmrNo;
        }

        /// <summary>
        /// 저장 : 이전서식
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strXML"></param>
        /// <param name="pEmrXmlImage"></param>
        /// <returns></returns>
        private double pSaveOldEmrData(string strChartDate, string strChartTime, string strXML, clsEmrType.EmrXmlImage[] pEmrXmlImage, clsEmrType.EmrXmlImageInit[] pEmrXmlImageInit)
        {
            double dblEmrNo = clsXMLOld.gSaveEmrXml(clsDB.DbCon, pAcp, mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime, strXML, pEmrXmlImage, pEmrXmlImageInit, strSaveFlag);
            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool pDelData()
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                #region 2021-634 전산업무 의뢰서 처리(입퇴원 요약지 검수완료)
                if (pForm.FmFORMNO == 1647 && ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>() >= "2021-07-01 06:00:00".To<DateTime>() &&
                    usFormTopMenuEvent.mbtnComplete.Text.Equals("검수완료"))
                {
                    ComFunc.MsgBoxEx(this, "완료된차트입니다.\r\n수정하시려면 의료정보팀 연락요망 (T.8041)");
                    return false;
                }
                #endregion

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                {
                    return false;
                }

                if (pForm.FmOLDGB == 1)
                {
                    if (ComFunc.MsgBoxQEx(this, "정말로 삭제하시겠습니까?") == DialogResult.No)
                    {
                        return false;
                    }

                    if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                    {
                        return false;
                    }
                    if (clsXML.gDeleteEmrXmlOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber, mstrFormNo) == true)
                    {
                        ComFunc.MsgBoxEx(this, "삭제하였습니다.");

                        mstrEmrNo = "0";
                        if (isReciveOrderSave == false)
                        {
                            //처방저장시에는 다시 조회 하지 않는다
                            pClearForm();
                            InitMibi();
                            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
                            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
                            SetInitChatValue();

                            if (mEmrCallForm != null)
                            {
                                mEmrCallForm.MsgDelete();
                            }
                        }
                    }
                }
                else
                {
                    if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                    {
                        return false;
                    }
                    if (clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == true)
                    {
                        #region DataMapping 삭제
                        switch (pForm.FmFORMNO)
                        {
                            case 1965:
                                Control control = panChart.Controls.Find("I0000009925", true).FirstOrDefault();
                                if (control != null && control.Text.Trim().Length > 0)
                                {
                                    string strBloodNo = control.Text.Trim();
                                    control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                                    if (control != null && control.Tag != null)
                                    {
                                        clsEmrQuery.DeleteDataMappingEx(clsDB.DbCon, this, pForm, strBloodNo + control.Tag.ToString(), 0);
                                        clsEmrQuery.DeleteDataMapping(clsDB.DbCon, this, pForm, VB.Val(mstrEmrNo), string.Empty, 0);
                                    }
                                }
                                break;
                            case 3535:
                                List<Control> controls = FormFunc.GetAllControls(panChart).Where(d => d.Name.IndexOf("I0000009925") != -1 && d.Text.NotEmpty()).ToList();
                                if (controls != null)
                                {
                                    foreach(Control control1 in controls)
                                    {
                                        string strBloodNo = control1.Text.Trim();
                                        string CtrlNumber = control1.Name.Split('_')[1];
                                        control = panChart.Controls.Find("I0000037484_" + CtrlNumber, true).FirstOrDefault();
                                        if (control != null && control.Tag != null)
                                        {
                                            clsEmrQuery.DeleteDataMappingEx(clsDB.DbCon, this, pForm, strBloodNo + control.Tag.ToString(), 0);
                                            clsEmrQuery.DeleteDataMapping(clsDB.DbCon, this, pForm, VB.Val(mstrEmrNo), string.Empty, 0);
                                        }
                                    }
                                    
                                }
                                break;
                        }
                        #endregion
                        mstrEmrNo = "0";
                        if (isReciveOrderSave == false)
                        {
                            //처방저장시에는 다시 조회 하지 않는다
                            pClearForm();
                            InitMibi();
                            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
                            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
                            SetInitChatValue();
                            if (mEmrCallForm != null)
                            {
                                mEmrCallForm.MsgDelete();
                            }
                        }
                    }
                }
            }

            //usFormTopMenuEvent.mbtnSaveTemp.Visible = pForm.FmOLDGB != 1;

            return true;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {           
            if(pForm.FmFORMNO == 2278 && (clsType.User.DeptCode.Equals("MN") || clsType.User.DeptCode.Equals("MR")) && clsType.User.DrCode.Length > 0)
            {
                clsFormPrint.mstrPRINTFLAG = VB.Val(mstrEmrNo) > 0 ?  "0" : "1";
            }
            else if (pForm.FmFORMNO >= 3577 && pForm.FmFORMNO <= 3588 && clsType.User.DeptCode.Equals("MR") && clsType.User.DrCode.Length > 0)
            {
                clsFormPrint.mstrPRINTFLAG = "0";
            }
            //2492(치료계획 설명서(작성용)(신규)), 3611(항암 치료 동의서 (작성용))
            else if (pForm.FmFORMNO == 2492 || pForm.FmFORMNO == 3611)
            {
                clsFormPrint.mstrPRINTFLAG = VB.Val(mstrEmrNo) > 0 ? "0" : "1";
            }
            else if (pForm.FmFORMNO == 2593 && pForm.FmUPDATENO >= 2)
            {
                Control[] panPrint = panChart.Controls.Find("panPrint1", true);
                if (panPrint.Length > 0)
                {
                    panPrint[0].Height = 300;
                }
            }
            else if (mstrMode.Equals("W"))
            {
                //frmEmrPrintOption 기록실은 창이안뜨게 요청 
                clsFormPrint.mstrPRINTFLAG = "0";
            }
            else
            {
                using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
                {
                    frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterParent;
                    frmEmrPrintOptionX.ShowDialog(this);
                }

                if (clsFormPrint.mstrPRINTFLAG == "-1")
                {
                    return;
                }
            }

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return;
            }

            List<Control> lstDisabled = ComFunc.GetAllControls(panChart).Where(d => d.Enabled == false).ToList();
            if (lstDisabled.Count > 0)
            {
                foreach (Control control in lstDisabled)
                {
                    control.Enabled = true;
                }
            }

            #region 혈액투석 기록지 조회 관련 숨기기
            if (pForm.FmFORMNO == 1577 || pForm.FmFORMNO == 2466)
            {
                Control control = Controls.Find("PanSearch", true).FirstOrDefault();
                if (control != null)
                {
                    control.Visible = false;
                }
            }
            #endregion

            #region 진료기록지 -  이미지, 기타  숨기기
            if (pForm.FmGRPFORMNO >= 1000 && pForm.FmGRPFORMNO <= 1012 || pForm.FmGRPFORMNO == 1075)
            {
                Control control = panChart.Controls.Find("GI0000029790", true).FirstOrDefault();
                if (control != null)
                {
                    List<Control> controls = ComFunc.GetAllControls(control).Where(d => d is PictureBox && d.Tag != null && VB.IsNumeric(d.Tag)).ToList();
                    if (controls.Count == 0)
                    {
                        control.Visible = false;
                    }
                }


                control = panChart.Controls.Find("GI0000001067", true).FirstOrDefault();
                if (control != null)
                {
                    List<Control> controls = ComFunc.GetAllControls(control).Where(d => d is TextBox && string.IsNullOrWhiteSpace(d.Text.Trim()) == false).ToList();
                    if (controls.Count == 0)
                    {
                        control.Visible = false;
                    }
                }
            }
            #endregion

            //2492(치료계획 설명서(작성용)(신규)), 3611(항암 치료 동의서 (작성용))
            if (pForm.FmFORMNO == 2492 || pForm.FmFORMNO == 3611)
            {
                clsFormPrint.PrintFormAgreement(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panChart, "C", false);
            }
            else if (pForm.FmFORMNO == 1647 || pForm.FmFORMNO == 1598)
            {
                clsFormPrint.PrintRtf(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panCoading, richTextBox1, mstrMode.Equals("H") ? "C2" : "C");

            }
            else
            {
                clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panChart, mstrMode.Equals("H") ? "C2" : "C");
            }

            if (pForm.FmFORMNO == 2593 && pForm.FmUPDATENO >= 2)
            {
                Control[] panPrint = panChart.Controls.Find("panPrint1", true);
                if (panPrint.Length > 0)
                {
                    panPrint[0].Height = 10;
                }
            }
        }

        /// <summary>
        /// 빈서식지(기록지)를 출력한다.
        /// </summary>
        public void pPrintFormNull()
        {
            //using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
            //{
            //    frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterParent;
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{
            //    return;
            //}

            clsFormPrint.mstrPRINTFLAG = "0"; //원외출력으로 변경

            //처방전 대리수령 신청서 무조건 원외로 
            if (pForm.FmFORMNO == 2761)
            {
                clsFormPrint.mstrPRINTFLAG = "0"; //원외출력으로 변경
            }
            else if (pForm.FmGRPFORMNO >= 1050 && pForm.FmGRPFORMNO <= 1055 || pForm.FmGRPFORMNO == 1066 || pForm.FmGRPFORMNO == 1068)
            {
                clsFormPrint.mstrPRINTFLAG = "0"; //원외출력으로 변경
                clsFormPrint.PrintFormAgreement(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, "0", panChart, "C", pForm.FmGRPFORMNO == 1050);
                return;
            }

            clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panChart, "C");
        }

        /// <summary>
        /// 기록지 미검수, 검수완료
        /// </summary>
        public void pSetComplete()
        {
            if(clsType.User.AuAMANAGE != "1")
            {
                return;
            }

            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if(usFormTopMenuEvent.mbtnComplete.Text == "미검수")
                {
                    bool writeForm = false;

                    #region 미비기록 데이터 조회..
                    OracleDataReader reader = null;

                    SQL = " SELECT * ";
                    SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRMIBI";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    //SQL += ComNum.VBLF + "  AND MEDDEPTCD = '" + pAcp.medDeptCd + "'";
                    SQL += ComNum.VBLF + "  AND MEDfrdate = '" + pAcp.medFrDate + "'";
                    SQL += ComNum.VBLF + "  AND PTNO = '" + pAcp.ptNo + "'";
                    SQL += ComNum.VBLF + "  AND MIBICLS = '1'";
                    SQL += ComNum.VBLF + "  AND MIBIFNDATE IS NULL";
                    //SQL += ComNum.VBLF + "  AND TO_NUMBER(CONCAT(MIBIINDATE, MIBIINTIME)) >= NVL(CONCAT(WRITEDATE, WRITETIME), 0)";

                     

                    string SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                        return;
                    }

                    if (reader.HasRows)
                    {
                        writeForm = true;
                    }

                    reader.Dispose();

                    #endregion

                    if (writeForm)
                    {
                        ComFunc.MsgBoxEx(this, "미비기록 있음 확인요망.");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                        
                    }


                    #region INSERT

                    SQL = "INSERT INTO KOSMOS_EMR.EMRXML_COMPLETE";
                    //SQL += ComNum.VBLF + " EMRNO, EMRNOHIS, CDATE, CSABUN, PTNO, MEDFRDATE, INDATE) VALUES (";
                    SQL += ComNum.VBLF + "( EMRNO, EMRNOHIS, CDATE, CSABUN, PTNO, MEDFRDATE, INDATE)";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "   EMRNO";
                    SQL += ComNum.VBLF + " , EMRNOHIS";
                    SQL += ComNum.VBLF + " , SYSDATE";
                    SQL += ComNum.VBLF + " , " + clsType.User.IdNumber;
                    SQL += ComNum.VBLF + " , PTNO";
                    SQL += ComNum.VBLF + " , MEDFRDATE";
                    SQL += ComNum.VBLF + " , TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST";
                    SQL += ComNum.VBLF + "WHERE FORMNO = 1647";
                    SQL += ComNum.VBLF + "  AND MEDFRDATE = '" + pAcp.medFrDate + "'";
                    SQL += ComNum.VBLF + "  AND PTNO = '" + pAcp.ptNo + "'";
                    SQL += ComNum.VBLF + "  AND EMRNO = " + mstrEmrNo;
                    SQL += ComNum.VBLF + "UNION ALL ";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "   EMRNO";
                    SQL += ComNum.VBLF + " , 0";
                    SQL += ComNum.VBLF + " , SYSDATE";
                    SQL += ComNum.VBLF + " , " + clsType.User.IdNumber;
                    SQL += ComNum.VBLF + " , PTNO";
                    SQL += ComNum.VBLF + " , MEDFRDATE";
                    SQL += ComNum.VBLF + " , TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXMLMST";
                    SQL += ComNum.VBLF + "WHERE FORMNO = 1647";
                    SQL += ComNum.VBLF + "  AND MEDFRDATE = '" + pAcp.medFrDate + "'";
                    SQL += ComNum.VBLF + "  AND PTNO = '" + pAcp.ptNo + "'";
                    SQL += ComNum.VBLF + "  AND EMRNO = " + mstrEmrNo;

                    sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        return;
                    }
                    #endregion

                    usFormTopMenuEvent.mbtnComplete.Text = "검수완료";
                    usFormTopMenuEvent.mbtnComplete.ForeColor = Color.FromArgb(0, 0, 255);
                }
                else
                {

                    if (ComFunc.MsgBoxQEx(this, "미검수 상태로 전환 하시겠습니까?", "확인") == DialogResult.No)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    #region INSERT, DELETE
                    SQL = "INSERT INTO KOSMOS_EMR.EMRXML_COMPLETE_HISTORY(";
                    SQL += ComNum.VBLF + " EMRNO, CDATE, CSABUN, DELDATE, DELSABUN, MEDFRDATE, PTNO, INDATE) ";
                    SQL += ComNum.VBLF + " SELECT EMRNO, CDATE, CSABUN, SYSDATE, " + clsType.User.IdNumber + ", MEDFRDATE, PTNO, INDATE";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_COMPLETE ";
                    SQL += ComNum.VBLF + "  WHERE EMRNO = " + mstrEmrNo;

                    sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        return;
                    }

                    SQL = " DELETE KOSMOS_EMR.EMRXML_COMPLETE ";
                    SQL += ComNum.VBLF + "  WHERE PTNO = '" + pAcp.ptNo + "'";
                    SQL += ComNum.VBLF + "    AND MEDFRDATE = '" + pAcp.medFrDate + "'";

                    sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        return;
                    }
                    #endregion

                    usFormTopMenuEvent.mbtnComplete.Text = "미검수";
                    usFormTopMenuEvent.mbtnComplete.ForeColor = Color.FromArgb(255, 0, 0);
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }


        /// <summary>
        /// 권한(코딩)
        /// </summary>
        public void pSetCoading()
        {
            #region 입퇴원 요약보기 추후 주석해제
            if ((pForm.FmFORMNO == 1647 || pForm.FmFORMNO == 1598) && VB.Val(mstrEmrNo) > 0)
            {
                if (panCoading.Visible)
                {
                    panCoading.Visible = false;
                    panChart.Visible = true;
                }
                else
                {
                    panCoading.Visible = true;
                    panChart.Visible = false;
                }
            }
            #endregion


            ssMacroWord.Visible = false;
            ssDiag.Visible = false;

            if (usFormTopMenuEvent.mbtnSave.Visible)
            {
                usFormTopMenuEvent.BackColor = Color.White;
                usFormTopMenuEvent.mbtnSave.Visible = false;
                strSaveFlag = string.Empty;
                pLockStart();
            }
            else
            {
                usFormTopMenuEvent.BackColor = Color.FromArgb(255, 255, 192);
                usFormTopMenuEvent.mbtnSave.Visible = true;
                strSaveFlag = "SAVE";
                pLockEnd();
            }
        }

        /// <summary>
        /// 사용자 템플릿 저장
        /// </summary>
        /// <returns></returns>
        public bool pSaveUserForm(double dblMACRONO)
        {
            //TODO
            bool rtnVal = false;
            rtnVal = clsEmrQuery.SaveDataAEMRUSERCHARTFORMROW(clsDB.DbCon, this, false, this, dblMACRONO);
            return rtnVal;
        }

        /// <summary>
        /// 컨트롤 입력 및 수정 안되도록
        /// </summary>
        public void pLockStart()
        {
            List<Control> controls = ComFunc.GetAllControls(panChart).Where(d => d is CheckBox || d is RadioButton || d is TextBox).ToList();
            foreach(Control control in controls)
            {
                #region 체크 클릭시 변경 안되게 수정.
                if (control is CheckBox)
                {
                    ((CheckBox)control).AutoCheck = false;
                }
                #endregion

                #region 라디오 클릭시 변경 안되게 수정.
                else if (control is RadioButton)
                {
                    ((RadioButton) control).AutoCheck = false;
                }
                #endregion

                #region 텍스트박스 잠구기
                else if (control is TextBox)
                {
                    ((TextBox)control).ReadOnly = true;
                }
                #endregion
            }
        }

        /// <summary>
        /// 컨트롤 입력 및 수정 잠금 해제
        /// </summary>
        public void pLockEnd()
        {
            List<Control> controls = ComFunc.GetAllControls(panChart).Where(d => d is CheckBox || d is RadioButton || d is TextBox).ToList();
            foreach (Control control in controls)
            {
                #region 텍스트 이벤트 살리기
                if (control is TextBox)
                {
                    ((TextBox)control).ReadOnly = false;
                    control.BackColor = Color.White;
                }
                #endregion

                #region 체크 살리기
                else if (control is CheckBox)
                {
                    ((CheckBox)control).AutoCheck = true;
                }
                #endregion

                #region 라디오 살리기
                else if (control is RadioButton)
                {
                    ((RadioButton)control).AutoCheck = true;
                }
                #endregion
            }
        }

        /// <summary>
        /// 이미지 기본 초기 이미지로
        /// </summary>
        public void SetImgDefault()
        {
            if (mFormXml == null || mLoading == false || panChart.Controls.Count == 0)
                return;

            List<FormXml> mPicture = mFormXml.Where(d => d.strCONTROTYPE.Equals("System.Windows.Forms.PictureBox")).ToList();
            if (mPicture.Count > 0)
            {
                for (int i = 0; i < mPicture.Count; i++)
                {
                    Control control = panChart.Controls.Find(mPicture[i].strCONTROLNAME, true).First();
                    if (control != null)
                    {
                        if ((control as PictureBox).Image != null)
                        {
                            (control as PictureBox).Image = mPicture[i].imgIMAGE;
                            (control as PictureBox).Tag = string.Empty;
                            (control as PictureBox).BackColor = Color.White;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 차트 작성시 최초 값을 저장
        /// </summary>
        public void SetInitChatValue()
        {
            mstrXmlInit = null;
            mstrXmlInit = clsXML.SaveDataToXml(this, true, panChart, false); //이미지 제외 컨트롤 값 저장

            mEmrXmlImageInit = null;
            SaveDataToXml(this, true, panChart, ref mEmrXmlImageInit, true); //이미지 컨트롤 값 저장
        }

        /// <summary>
        /// 차트의 초기값을 저장한다 : 이미지
        /// </summary>
        /// <param name="frmXmlForm"></param>
        /// <param name="isSpcPanel"></param>
        /// <param name="pControl"></param>
        /// <param name="pEmrXmlImageInit"></param>
        private void SaveDataToXml(Control frmXmlForm, bool isSpcPanel, Control pControl, ref clsEmrType.EmrXmlImageInit[] pEmrXmlImageInit, bool Start)
        {
            try
            {
                if (isSpcPanel == true)
                {
                    if (pControl == null)
                    {
                        ComFunc.MsgBoxEx(this, "선택된 컨테이너가 존재하지 않습니다.");
                        return;
                    }
                }
                
                Control[] controls = null;
                string strXml = clsXML.GetXML((long)VB.Val(mstrFormNo));

                if (isSpcPanel == true)
                {
                    controls = ComFunc.GetAllControls(pControl);
                }
                else
                {
                    controls = ComFunc.GetAllControls(frmXmlForm);
                }


                string strConName = "";

                foreach (Control objControl in controls)
                {
                    strConName = objControl.Name;
                    if (objControl is PictureBox)
                    {
                        if (pEmrXmlImageInit == null)
                        {
                            pEmrXmlImageInit = new clsEmrType.EmrXmlImageInit[0];
                        }

                        Array.Resize<clsEmrType.EmrXmlImageInit>(ref pEmrXmlImageInit, pEmrXmlImageInit.Length + 1);
                        pEmrXmlImageInit[pEmrXmlImageInit.Length - 1].Pic = objControl;
                        pEmrXmlImageInit[pEmrXmlImageInit.Length - 1].Img = ((PictureBox)objControl).Image;
                        pEmrXmlImageInit[pEmrXmlImageInit.Length - 1].ContNm = ((PictureBox)objControl).Name;
                        pEmrXmlImageInit[pEmrXmlImageInit.Length - 1].ImageNo = "";
                        if (Start)
                        {
                            pEmrXmlImageInit[pEmrXmlImageInit.Length - 1].ImageNo = objControl.Tag == null || objControl.Tag != null && objControl.Tag.ToString().Length == 0 ? clsXML.GetImageNo(strXml, strConName) : ((PictureBox)objControl).Tag.ToString();
                        }
                        ((PictureBox)objControl).Tag = "";
                    }
                }
                
                return ;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message.ToString(), "PSMH");
                return ;
            }
        }

        #endregion //기록지 클리어, 저장, 삭제, 프린터

        #region //Public Function 외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터


        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
            InitMibi();
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
            SetInitChatValue();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            SetFormInfo();

            SetTopMenuLoad();

            mFormXml = FormDesignQuery.GetDataFormXml(mstrFormNo, mstrUpdateNo);
            if (mFormXml == null)
            {
                return;
            }

            if (mFormXml != null)
            {
                for (int i = 0; i < mFormXml.Length; i++)
                {
                    if (mFormXml[i].strCONTROLPARENT == "Form1")
                    {
                        mFormXml[i].strCONTROLPARENT = "panChart";
                    }

                    if (mFormXml[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                    {
                        mFormXml[i].strCONTROTYPE = "mtsPanel15.mPanel";
                    }

                    #region 경과기록지 수정, 작성시에만 보이게
                    if (mFormXml[i].strCONTROLNAME == "PanMacro" && mFormXml[i].strVISIBLED == "False")
                    {
                        mFormXml[i].strVISIBLED = "True";
                    }
                    #endregion
                }

                FormLoadControl.LoadControl(this, mFormXml, "panChart");
            }

            //  컨트롤
            List<Control> list = new List<Control>();
            GetControls(this, ref list);

            //  컨트롤의 Top이 낮고 Left가 작은 기준으로 컨트롤 정렬
            var cList = list.ToList().OrderBy(r => r.Top).ThenBy(r => r.Left).ToList();
            cList.ForEach(x => x.TabIndex = cList.IndexOf(x) + 1);

            SetControlEvents();
        }

        /// <summary>
        /// TabIndex를위해 컨트롤 전체 담기
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="list"></param>
        private void GetControls(Control ctrl, ref List<Control> list)
        {
            foreach(Control c in ctrl.Controls)
            {
                if(c.HasChildren)
                {
                    GetControls(c,ref list);
                }

                list.Add(c);
            }
        }


        /// <summary>
        /// 입퇴원 요약지 하드코딩.
        /// </summary>
        private void DisChargerReCordVisible(bool ViewVisible = true)
        {
            #region 입퇴원 요약지 관련 처리
            //차트복사일때 버튼 숨기게.
            if ((pForm.FmFORMNO == 1647 && 
                mEmrCallForm != null && ((Form)mEmrCallForm).Name == "frmEmrJobChartCopy" ) ||
                pForm.FmFORMNO == 1647)
            {
                Control[] controls;

                //사용자면 무조건 수정모드
                if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber))
                {
                    return;
                }
                else
                {
                    richTextBox1.Visible = true;
                    panChart.Visible = false;
                }

                //btnSaveVisible.Top = 5;
                //btnSaveVisible.Left = 450;
                //btnSaveVisible.BringToFront();

                #region 입퇴원정보 하드코딩

                if (richTextBox1.IsDisposed)
                    return;

                richTextBox1.Clear();
                if(pAcp != null)
                {
                    //입퇴원 정보
                    DateTime dtpFrDate = DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null);
                    DateTime dtpEndDate = DateTime.ParseExact(pAcp.medEndDate.Length == 0 ? ComQuery.CurrentDateTime(clsDB.DbCon, "D") : pAcp.medEndDate, "yyyyMMdd", null);

                    string strTemp = string.Format("[입퇴원 정보] 재원일수: {0}일", (dtpEndDate - dtpFrDate).TotalDays + 1);
                    RichTxtBold(strTemp, "굴림체", 11, false, true);

                    string strDrName = string.Empty;
                    string strDeptCode = string.Empty;

                    FormPatInfoFunc.GetInDrName(clsDB.DbCon, pAcp, ref strDrName, ref strDeptCode);

                    //입원정보
                    int intStart = richTextBox1.TextLength;
                    if (strDrName.Length > 0 && strDrName != pAcp.medDrName)
                    {
                        strTemp = string.Format("입원일: {0}  입원과: {1}  주치의: {2}", pAcp.medFrDate, strDeptCode, strDrName);
                    }

                    else
                    {
                        strTemp = string.Format("입원일: {0}  입원과: {1}  주치의: {2}", pAcp.medFrDate, pAcp.medDeptKorName, pAcp.medDrName);
                    }


                    RichTextAdd(strTemp);
                    #region 입퇴원요약지 요약보기 추후 주석해제
                    if (ViewVisible)
                    {
                        panChart.Visible = false;
                        panCoading.Visible = true;
                    }
                    #endregion

                    //19-10-07 퇴원과와 입원과가 같은데 입원의사랑 퇴원의사가 다른경우 색깔 표시 및 진하게.
                    if (mEmrCallForm != null && ((Form)mEmrCallForm).Name.Equals("frmEmrJobChartCopy") == false && strDrName.Length > 0 && strDeptCode == pAcp.medDeptKorName && strDrName != pAcp.medDrName && clsType.User.AuAMANAGE == "1")
                    {
                        using (Font bFont = new Font("굴림체", 11, FontStyle.Bold))
                        {
                            richTextBox1.Select(intStart, strTemp.Length);
                            richTextBox1.SelectionFont = bFont;
                        }
                    }

                    //퇴원정보
                    strTemp = string.Format("퇴원일: {0}  퇴원과: {1}  주치의: {2}", pAcp.medEndDate, pAcp.medDeptKorName, pAcp.medDrName);
                    RichTextAdd(strTemp);
                }
                
                #endregion

                StringBuilder strBuilder = new StringBuilder();

                #region 라벨 숨기기 - 주석처리
                //if (clsType.User.AuAMANAGE == "1")
                //{
                //    for (int i = 1; i < 11; i++)
                //    {
                //        controls = panChart.Controls.Find("lblInfo" + i, true);
                //        if (controls.Length > 0)
                //        {
                //            controls[0].Dispose();
                //        }
                //    }
                //}
                #endregion

                #region 주증상 및 현병력
                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta2" : "I0000031713", true);
                if (controls.Length > 0 && controls[0].Text.Length > 0)
                {
                    RichTxtBold("[주증상 및 현병력]");
                    //GetSortContent(controls[0].Text, ref strBuilder);
                    RichTextAdd(controls[0].Text);
                    //RichTextAdd("");
                }
                #endregion

                string strName = string.Empty;
                int division = 75;


                #region 주진단명
                string POA = string.Empty;
                controls = panChart.Controls.Find("I0000036381", true);
                if (controls.Length > 0)
                {
                    POA = controls[0].Text.Trim();
                }

                string MainCode = string.Empty;
                controls = panChart.Controls.Find("I0000028889", true);
                if (controls.Length > 0)
                {
                    MainCode = controls[0].Text.Trim();
                }

                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "di1" : "I0000013294", true);
                if (controls.Length > 0)
                {
                    if(string.Concat(controls[0].Text.Trim().Where(c => !char.IsWhiteSpace(c))).Length > 0)
                    {

                        RichTxtBold("[주진단명]");
                        if (pForm.FmOLDGB == 0)
                        {
                            strName = controls[0].Text;

                            RichTextAdd(("   POA").PadRight(10) + ("진단코드").PadRight(40) + ("진단명"), "Times New Roman", 11, true);
                            if (controls[0].Text.Length <= division)
                            {
                                RichTxtBold(POA.PadLeft(6).PadRight(13) + (MainCode).PadRight(16 - MainCode.Length) + controls[0].Text, "Times New Roman", 12, false, false);
                            }
                            else
                            {
                                #region 이전
                                //strName = controls[0].Text;
                                //subLength = (strName.Length / division).To<int>() + 1;
                                //sLength = 0;
                                //for(int i = 0; i < subLength; i++)
                                //{
                                //    strName = controls[0].Text.Substring(sLength, controls[0].Text.Length - sLength >= division ? division : controls[0].Text.Length - sLength);
                                //    if (i == 0)
                                //    {
                                //        RichTxtBold(POA.PadLeft(6).PadRight(14) + (MainCode).PadRight(21 - MainCode.Length) + strName, "Times New Roman", 12, false, false);
                                //        sLength = division;
                                //    }
                                //    else
                                //    {
                                //        sLength += controls[0].Text.Length - sLength >= division ? division : controls[0].Text.Length - sLength;
                                //        RichTxtBold("".PadRight(36) + strName, "Times New Roman", 12, false, false);
                                //    }
                                //}
                                #endregion

                                #region 잘라서 넣기
                                string[] strArr = strName.Split(' ');
                                StringBuilder strDiag = new StringBuilder();

                                bool newLine = false;
                                using (Font subFont = new Font("Times New Roman", 12, FontStyle.Bold))
                                {
                                    for (int s = 0; s < strArr.Length; s++)
                                    {
                                        strDiag.Append(strArr[s]).Append(" ");
                                        //Size strWidth = TextRenderer.MeasureText(strDiag.ToString().Trim(), subFont);
                                        Size strWidth = TextRenderer.MeasureText(strDiag.ToString().Trim(), subFont, new Size(500, 20), TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                        if (strWidth.Width >= 400 || s == strArr.Length - 1)
                                        {
                                            if (newLine == false)
                                            {
                                                strBuilder.Append((POA.PadLeft(6).PadRight(13) + (MainCode).PadRight(16 - MainCode.Length) + strDiag.ToString().Trim() + "\n"));
                                                newLine = true;
                                            }
                                            else
                                            {
                                                strBuilder.Append(("").PadRight(38) + strDiag.ToString().Trim() + "\n");
                                            }

                                            strDiag.Clear();
                                        }
                                    }
                                    RichTxtBold("     " + strBuilder.ToString().Trim(), "Times New Roman", 12, false, false);

                                }
                                #endregion


                            }
                        }
                        else
                        {
                            RichTxtBold("   " + controls[0].Text, "Times New Roman", 12, false, false);
                        }
                    }
                    else
                    {
                        RichTxtBold("[주진단명 오류]");
                    }
                }
                #endregion

                #region 부진단명 처리
                POA = string.Empty;
                MainCode = string.Empty;
                //strBuilder.Clear();

                if (ComFunc.GetAllControls(panChart).Where(d => d.Name.Equals("di1") == false && d.Name.IndexOf(pForm.FmOLDGB == 1 ? "di" : "I0000031714_") != -1 && !string.IsNullOrWhiteSpace(d.Text)).Any())
                {
                    RichTxtBold("[부진단명]");
                    if (pForm.FmOLDGB == 0)
                    {
                        RichTextAdd(("   POA").PadRight(10) + ("진단코드").PadRight(40) + ("진단명"), "Times New Roman", 11, true);
                        //RichTxtBold("     " + strBuilder.ToString().Trim(), "Times New Roman", 12, true, false);
                    }
                    else
                    {
                        //RichTxtBold("   " + strBuilder.ToString().Trim(), "Times New Roman", 12, false, false);
                    }

                    for (int i = (pForm.FmOLDGB == 1 ? 2 : 1); i < (pForm.FmOLDGB == 1 ? 10 : 21); i++)
                    {
                        if (pForm.FmOLDGB == 0)
                        {
                            POA = string.Empty;
                            controls = panChart.Controls.Find("I0000036382_" + i, true);
                            if (controls.Length > 0)
                            {
                                POA = controls[0].Text.Trim();
                            }

                            MainCode = string.Empty;
                            controls = panChart.Controls.Find("I0000036383_" + i, true);
                            if (controls.Length > 0)
                            {
                                MainCode = controls[0].Text.Trim();
                            }
                        }

                        controls = panChart.Controls.Find((pForm.FmOLDGB == 1 ? "di" : "I0000031714_") + i, true);
                        if (controls.Length > 0)
                        {
                            controls[0].Text = controls[0].Text.Trim().Replace(Environment.NewLine, "");
                            if (controls[0].Text.Length > 0)
                            {
                                if (pForm.FmOLDGB == 0)
                                {
                                    strName = controls[0].Text;

                                    if (strName.Length <= division)
                                    {
                                        RichTxtBold(POA.PadLeft(6).PadRight(13) + (MainCode).PadRight(16 - MainCode.Length) + controls[0].Text, "Times New Roman", 12, false, false);
                                        RichTxtBoldLine();

                                    }
                                    else
                                    {
                                        //첫번째 모든 정보
                                        #region 잘라서 넣기
                                        string[] strArr = strName.Split(' ');
                                        StringBuilder strDiag = new StringBuilder();

                                        bool newLine = false;
                                        using (Font subFont = new Font("Times New Roman", 12, FontStyle.Bold))
                                        {
                                            for (int s = 0; s < strArr.Length; s++)
                                            {
                                                strDiag.Append(strArr[s]).Append(" ");
                                                //Size strWidth = TextRenderer.MeasureText(strDiag.ToString().Trim(), subFont);
                                                Size strWidth = TextRenderer.MeasureText(strDiag.ToString().Trim(), subFont, new Size(500, 20), TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                                if (strWidth.Width >= 400 || s == strArr.Length - 1)
                                                {
                                                    if (newLine == false)
                                                    {
                                                        RichTxtBold((POA.PadLeft(6).PadRight(13) + (MainCode).PadRight(16 - MainCode.Length) + strDiag.ToString().Trim()), "Times New Roman", 12, false, false);
                                                        //strBuilder.Append((POA.PadLeft(6).PadRight(12) + (MainCode).PadRight(20 - MainCode.Length) + strDiag.ToString().Trim() + "\n"));
                                                        newLine = true;
                                                    }
                                                    else
                                                    {
                                                        RichTxtBold(("").PadRight(38) + strDiag.ToString().Trim(), "Times New Roman", 12, false, false);
                                                        //strBuilder.Append(("").PadRight(34) + strDiag.ToString().Trim() + "\n");
                                                    }
                                                    strDiag.Clear();
                                                }
                                            }
                                        }

                                        RichTxtBoldLine();
                                        #endregion
                                    }

                                }
                                else
                                {
                                    strBuilder.Append("   " + controls[0].Text.Trim() + "\n");
                                }
                            }
                        }
                    }
                    if (pForm.FmOLDGB == 1)
                    {
                        RichTxtBold("   " + strBuilder.ToString().Trim() + "\n", "Times New Roman", 12, false, false);
                    }
                }

                if (strBuilder.Length > 0)
                {
                    
                }
                #endregion

                #region 수술 및 처치명 있을때만 보임.
                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta6" : "I0000031715", true);
                if (controls.Length > 0 && controls[0].Text.Length > 0)
                {
                    RichTxtBold("[수술 및 처치명]");
                    //GetSortContent(controls[0].Text, ref strBuilder, 2);
                    //RichTextAdd(strBuilder.ToString(), "Times New Roman", 11, true);
                    RichTextAdd(controls[0].Text, "Times New Roman", 11, true);

                    //RichTextAdd("");
                }
                #endregion

                #region 퇴원시 환자상태
                string ctlName = string.Empty;
                RichTxtBold("[퇴원시 환자상태]");

                Control control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik1" : "I0000013224", true).FirstOrDefault();
                if (control != null)
                { 
                    richTextBox1.AppendText((((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " " );
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik2" : "I0000012982", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText(" " + (((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " " );
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik3" : "I0000013381", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText(" " + (((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " ");
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik4" : "I0000012743", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText(" " + (((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " ");
                    richTextBox1.AppendText("\n");
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik7" : "I0000029916", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText((((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " ");
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik5" : "I0000027971", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText((((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " ");
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik8" : "I0000031724", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText(" " + (((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " ");
                }

                control = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ik6" : "I0000031725", true).FirstOrDefault();
                if (control != null)
                {
                    richTextBox1.AppendText(" " + (((CheckBox)control).Checked ? "\u25A3 " : "\u25A1 ") + control.Text + " ");
                    richTextBox1.AppendText("\n");
                }
                #endregion

                #region 퇴원형태
                RichTxtBold("[퇴원형태]");
                List<string> Checkes = new List<string>
                {
                    {pForm.FmOLDGB == 1 ? "ik9" : "I0000013350"},
                    {pForm.FmOLDGB == 1 ? "ik10" : "I0000010975"},
                    {pForm.FmOLDGB == 1 ? "ik11" : "I0000013270"},
                    {pForm.FmOLDGB == 1 ? "ik12" : "I0000000391_1"}
                };

                //for (int i = 9; i < 13; i++)
                for (int i = 0; i < Checkes.Count; i++)
                {
                    //controls = panChart.Controls.Find("ik" + i, true);
                    controls = panChart.Controls.Find(Checkes[i], true);
                    if (controls.Length > 0)
                    {
                        richTextBox1.AppendText((i > 0 ? " " : "") + (((CheckBox)controls[0]).Checked ? "\u25A3 " : "\u25A1 ") + controls[0].Text + " " );
                    }
                }


                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "it1" : "I0000000391_2", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length > 0)
                {
                    RichTextAdd("   " + controls[0].Text.Trim());
                }
                else
                {
                    RichTextAdd("");
                }

                #endregion

                #region 추후치료계획
                RichTxtBold("[추후치료계획]");
                Checkes.Clear();
                Checkes.Add(pForm.FmOLDGB == 1 ? "ik13" : "I0000000091");
                Checkes.Add(pForm.FmOLDGB == 1 ? "ik14" : "I0000013189_1");

                //for (int i = 13; i < 15; i++)
                for (int i = 0; i < Checkes.Count; i++)
                {
                    //controls = panChart.Controls.Find("ik" + i, true);
                    controls = panChart.Controls.Find(Checkes[i], true);
                    if (controls.Length > 0)
                    {
                        richTextBox1.AppendText((i > 0 ? " " : "") + (((CheckBox)controls[0]).Checked ? "\u25A3 " : "\u25A1 ") + controls[0].Text + " ");
                    }
                }

                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "it2" : "I0000013189_2", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length > 0)
                {
                    RichTextAdd("   " + controls[0].Text.Trim());
                }

                #endregion

                #region 경과요약 - 신규 기록지
                if (pForm.FmOLDGB == 0)
                {
                    controls = panChart.Controls.Find("I0000007396", true);
                    if (controls.Length > 0 && controls[0].Text.Length > 0)
                    {
                        RichTxtBold("[경과요약]");
                        //GetSortContent(controls[0].Text, ref strBuilder);
                        //RichTextAdd(strBuilder.ToString(), "굴림체", 10, true);

                        RichTextAdd(controls[0].Text, "굴림체", 10, true);
                    }
                }

                #endregion

                #region 전과정보
                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta10" : "I0000031718", true);
                if (controls.Length > 0 && controls[0].Text.Length > 0)
                {
                    RichTxtBold("[전과정보]");
                    //GetSortContent(controls[0].Text, ref strBuilder, 2);
                    //RichTextAdd(strBuilder.ToString());

                    RichTextAdd(controls[0].Text);

                    //RichTextAdd("");
                }
                #endregion

                #region 경과요약 및 검사결과(이전), 검사결과(신규)
                if (pForm.FmOLDGB == 1)
                {
                    controls = panChart.Controls.Find("ta7", true);
                    if (controls.Length > 0 && controls[0].Text.Length > 0)
                    {
                        RichTxtBold("[경과요약 및 검사결과]");
                        //GetSortContent(controls[0].Text, ref strBuilder);
                        //RichTextAdd(strBuilder.ToString(), "굴림체", 10, true);

                        RichTextAdd(controls[0].Text, "굴림체", 10, true);
                    }
                }
                else
                {
                    controls = panChart.Controls.Find("I0000000972", true);
                    if (controls.Length > 0 && controls[0].Text.Length > 0)
                    {
                        RichTxtBold("[검사결과]");
                        //GetSortContent(controls[0].Text, ref strBuilder);
                        //RichTextAdd(strBuilder.ToString(), "굴림체", 10, true);

                        RichTextAdd(controls[0].Text, "굴림체", 10, true);
                    }
                }
               
                #endregion

                #region Lab
                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta8" : "I0000012610", true);
                if (controls.Length > 0 && controls[0].Text.Length > 0)
                {
                    RichTxtBold("● Lab");
                    //GetSortContent(controls[0].Text, ref strBuilder);
                    //RichTextAdd(strBuilder.ToString(), "굴림체", 10, true);

                    RichTextAdd(controls[0].Text, "굴림체", 10, true);
                }
                #endregion

                #region 퇴원약
                controls = panChart.Controls.Find(pForm.FmOLDGB == 1 ? "ta9" : "I0000011150", true);
                if (controls.Length > 0 && controls[0].Text.Length > 0)
                {
                    RichTxtBold("[퇴원약]");
                    //GetSortContent(controls[0].Text, ref strBuilder);
                    //RichTextAdd(strBuilder.ToString(), "굴림체", 10, true);

                    RichTextAdd(controls[0].Text, "굴림체", 10, true);
                }
                #endregion
            }
            #endregion
        }

        #region 리치텍스트박스 및 입퇴원 요약지 하드코딩 관련 함수
        /// <summary>
        /// 리치텍스트 볼드
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="FontName"></param>
        private void RichTxtBold(string strTitle, string FontName = "굴림체", float fontSize = 11, bool LineHeader = true, bool LineSpace = true)
        {
            using (Font bFont = new Font(FontName, fontSize, FontStyle.Bold))
            {
                int intStart = richTextBox1.TextLength;

                if (LineHeader)
                {
                    richTextBox1.AppendText(new string(' ', 30));
                    richTextBox1.Select(intStart, 30);
                    using (Font font = new Font("굴림체", 2))
                    {
                        richTextBox1.SelectionFont = font;
                    }
                    richTextBox1.AppendText("\n");
                }

                intStart = richTextBox1.TextLength;
                richTextBox1.AppendText(strTitle);
                richTextBox1.Select(intStart, strTitle.Length + 2);
                richTextBox1.SelectionFont = bFont;
                if(strTitle.Equals("[주진단명 오류]"))
                {
                    richTextBox1.SelectionColor = Color.Red;
                }

                if(LineSpace)
                {
                    richTextBox1.Select(intStart > 1 ? intStart - 1 : 0 ,  strTitle.Length + 2);
                    richTextBox1.SelectionCharOffset = 4;
                    richTextBox1.SelectionLength = 0;
                }

                richTextBox1.SelectionLength = 0;
                richTextBox1.AppendText("\n");
            }
        }

        /// <summary>
        /// 리치텍스트 볼드2
        /// </summary>
        private void RichTxtBoldLine()
        {
            using (Font bFont = new Font("굴림체", 3, FontStyle.Bold))
            {
                int intStart = richTextBox1.TextLength;
                string strTitle = "   ";

                intStart = richTextBox1.TextLength;
                richTextBox1.AppendText(strTitle);
                richTextBox1.Select(intStart, strTitle.Length + 2);
                richTextBox1.SelectionFont = bFont;

                richTextBox1.Select(intStart > 1 ? intStart - 1 : 0, strTitle.Length + 2);
                richTextBox1.SelectionCharOffset = 1;
                richTextBox1.SelectionLength = 0;

                richTextBox1.SelectionLength = 0;
                richTextBox1.AppendText("\n");
            }
        }


        /// <summary>
        /// 리치텍스트 추가
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="LineSpace"></param>
        private void RichTextAdd(string Content, string FontName = "굴림체", float FontSize = 11, bool changeFont = false)
        {
            int Start = richTextBox1.TextLength;
            richTextBox1.AppendText(Content);
            richTextBox1.AppendText("\n");
            if (changeFont)
            {
                richTextBox1.Select(Start, Content.Length + 1);
                using(Font font = new Font(FontName, FontSize))
                {
                    richTextBox1.SelectionFont = font;
                }
                richTextBox1.SelectionLength = 0;
            }
        }
        #endregion

        /// <summary>
        /// 작성내역을 세팅한다
        /// </summary>
        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //히스토리 보는 용도로 들어왔을경우
            usFormTopMenuEvent.mbtnHisSearch.Visible = pForm.FmOLDGB != 1 && clsType.User.AuAMANAGE.Equals("1");

            #region 입퇴원 요약지  코드 항상 대문자 입력 
            if (pForm.FmOLDGB == 0 && (pForm.FmFORMNO == 1647 || pForm.FmFORMNO == 1598))
            {
                Control panDiag = panChart.Controls.Find("GI0000031714", true).FirstOrDefault();
                if (panDiag != null)
                {
                    Control main = panChart.Controls.Find("I0000028889", true).FirstOrDefault();
                    if (main != null)
                    {
                        main.ImeMode = ImeMode.Disable;
                        (main as TextBox).CharacterCasing = CharacterCasing.Upper;
                    }

                    List<Control> controls = FormFunc.GetAllControls(panDiag).Where(d => d is TextBox).Where(c => c.Name.IndexOf("I0000036383") != -1).ToList();
                    foreach (Control control in controls)
                    {
                        control.ImeMode = ImeMode.Disable;
                        (control as TextBox).CharacterCasing = CharacterCasing.Upper;
                    }
                }
            }
            #endregion

            //초기값 세팅
            if (VB.Val(mstrEmrNo) <= 0)
            {
                // usFormTopMenuEvent.mbtnPrint.Visible = false;
                usFormTopMenuEvent.mbtnPrint.Visible =  clsType.User.AuAPRINTIN.Equals("1");
 
                usFormTopMenuEvent.mbtnPrintNull.Visible = true;
                usFormTopMenuEvent.mbtnSaveTemp.Visible = pForm.FmOLDGB != 1;

                if (mFormXml != null)
                {
                    clsEmrFunc.SetControlInitValue(clsDB.DbCon, this, mFormXml, pForm, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"));
                }

                //LoadEmrChartInfo();

                #region 입퇴원 요약지 부진단명 가져오기 
                if (pForm.FmOLDGB == 0 && (pForm.FmFORMNO == 1647 || pForm.FmFORMNO == 1598) && pAcp != null)
                {
                    clsEmrFunc.Set_FormPatInfo_SubDisease_Eng(clsDB.DbCon, pAcp, this, ":I0000036383:1:20");
                }
                #endregion

                #region 응급실 퇴실기록지 퇴실구분 자동 입력
                if (pForm.FmFORMNO == 3129)
                {
                    clsEmrFunc.setChartFormValue3129_1(clsDB.DbCon, this, pAcp, pForm);
                }
                #endregion

                if (pForm.FmFORMNO == 2279 && pAcp.medDeptCd == "ER")
                {
                    clsEmrFunc.setChartFormValue2279_1(clsDB.DbCon, this, pAcp, pForm);
                }
                
                if (pForm.FmFORMNO == 3612 )
                {
                    Control obj = panChart.Controls.Find("I0000001378", true).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Text = pAcp.ptName;
                    }
                    
                    obj = panChart.Controls.Find("I0000030739", true).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Text = pAcp.ptName;
                    }

                    obj = panChart.Controls.Find("I0000017119", true).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.Text = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + Convert.ToInt16(VB.Mid(clsPublic.GstrSysDate, 6, 2)).ToString() + "월 " + Convert.ToInt16(VB.Right(clsPublic.GstrSysDate, 2)).ToString() + "일";
                    }
                }

                // 피부사정기록지 XML버전일때만
                if (pForm.FmFORMNO == 2348 && pForm.FmOLDGB == 1)
                {
                    #region 피부사정 자동 점수 입력
                    DataTable dt = clsEmrFunc.Set_FormPatInfo_BRADEN(clsDB.DbCon, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string strGUBUN = dt.Rows[0]["GUBUN"].ToString().Trim();
                        string strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();

                        Control obj = panChart.Controls.Find("it282", true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Text = strTOTAL;
                        }

                        #region 데이터 넣기
                        obj = panChart.Controls.Find("it5", true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Text = dt.Rows[0]["JUMSU1"].ToString().Trim();
                        }

                        obj = panChart.Controls.Find("it6", true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Text = dt.Rows[0]["JUMSU2"].ToString().Trim();
                        }

                        obj = panChart.Controls.Find("it7", true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Text = dt.Rows[0]["JUMSU3"].ToString().Trim();
                        }

                        obj = panChart.Controls.Find("it8", true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Text = dt.Rows[0]["JUMSU4"].ToString().Trim();
                        }

                        obj = panChart.Controls.Find("it9", true).FirstOrDefault();
                        if (obj != null)
                        {
                            obj.Text = dt.Rows[0]["JUMSU5"].ToString().Trim();
                        }
                        #endregion

                        switch (strGUBUN)
                        {
                            #region BRADEN
                            case "BRADEN":
                                obj = panChart.Controls.Find("it10", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[0]["JUMSU6"].ToString().Trim();
                                }

                                obj = panChart.Controls.Find("it284", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = "미해당";
                                }

                                obj = panChart.Controls.Find("it285", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = "미해당";
                                }

                                obj = panChart.Controls.Find("ik2013", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    (obj as CheckBox).Checked = true;
                                }

                                obj = panChart.Controls.Find("it283", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    if (VB.Val(strTOTAL) < 9)
                                    {
                                        obj.Text = "최고위험군";
                                    }
                                    else if (VB.Val(strTOTAL) >= 10 && VB.Val(strTOTAL) <= 12)
                                    {
                                        obj.Text = "고위험군";
                                    }
                                    else if (VB.Val(strTOTAL) >= 13 && VB.Val(strTOTAL) <= 14)
                                    {
                                        obj.Text = "중증도위험군";

                                    }
                                    else if (VB.Val(strTOTAL) >= 15 && VB.Val(strTOTAL) <= 18)
                                    {
                                        obj.Text = "저위험군";

                                    }
                                    else
                                    {
                                        obj.Text = "주기적인 욕창발생요인 점검";
                                    }
                                }
                                break;
                            #endregion
                            #region CHILD
                            case "CHILD":
                                obj = panChart.Controls.Find("it10", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = "미해당";
                                }

                                obj = panChart.Controls.Find("it284", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[0]["JUMSU6"].ToString().Trim();
                                }

                                obj = panChart.Controls.Find("it285", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[0]["JUMSU7"].ToString().Trim();
                                }

                                obj = panChart.Controls.Find("ik2014", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    (obj as CheckBox).Checked = true;
                                }

                                obj = panChart.Controls.Find("it283", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    if (VB.Val(strTOTAL) <= 16)
                                    {
                                        obj.Text = "욕창위험군";
                                    }
                                    else
                                    {
                                        obj.Text = "주기적인 욕창발생요인 점검";
                                    }
                                }
                                break;
                            #endregion
                            #region BABY
                            case "BABY":
                                obj = panChart.Controls.Find("it10", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = "미해당";
                                }

                                obj = panChart.Controls.Find("it284", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[0]["JUMSU6"].ToString().Trim();
                                }

                                obj = panChart.Controls.Find("it285", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[0]["JUMSU7"].ToString().Trim();
                                }

                                obj = panChart.Controls.Find("ik2015", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    (obj as CheckBox).Checked = true;
                                }
                                break;
                            #endregion
                        }
                    }

                    if (dt != null)
                    {
                        dt.Dispose();
                    }
                    #endregion
                }
                //퇴원간호 계획지 XML버전일때만
                else if (pForm.FmFORMNO == 966)
                {
                    if (pForm.FmOLDGB == 1)
                    {
                        #region 컨트롤 찾아서 데이터 넣기(이전 서식)
                        DataTable dt = clsEmrFunc.Set_FormPatInfo_OPD_RESERVED(clsDB.DbCon, pAcp);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string sDate = dt.Rows[0]["DATE3"].ToString().Trim();

                            //일자
                            Control obj = panChart.Controls.Find("dt2", true).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.Text = Convert.ToDateTime(sDate).ToShortDateString();
                            }

                            //시간
                            obj = panChart.Controls.Find("it6", true).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.Text = Convert.ToDateTime(sDate).ToString("HH:mm");
                            }

                            //진료과
                            obj = panChart.Controls.Find("it5", true).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.Text = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                            }

                            //진료의
                            obj = panChart.Controls.Find("it4", true).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                            }

                            if (dt.Rows.Count > 1)
                            {
                                sDate = dt.Rows[1]["DATE3"].ToString().Trim();
                                obj = panChart.Controls.Find("dt3", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = Convert.ToDateTime(sDate).ToShortDateString();
                                }

                                //시간
                                obj = panChart.Controls.Find("it7", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = Convert.ToDateTime(sDate).ToString("HH:mm");
                                }

                                //진료과
                                obj = panChart.Controls.Find("it8", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[1]["DEPTNAMEK"].ToString().Trim();
                                }

                                //진료의
                                obj = panChart.Controls.Find("it9", true).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Text = dt.Rows[1]["DRNAME"].ToString().Trim();
                                }
                            }
                        }

                        if (dt != null)
                        {
                            dt.Dispose();
                        }
                        #endregion
                    }
                }
                //혈액투석기록지 가장 최근내용 가져오기
                else if (pForm.FmFORMNO == 1577 || pForm.FmFORMNO == 2466 )
                {
                    #region 컨트롤 찾아서 데이터 넣기
                    Control control = null;
                    string MaxEmrNo = clsEmrFunc.Set_FormPatInfo_LASTEMRNO(clsDB.DbCon, pAcp, pForm, mstrFormNo);
                    if (pForm.FmOLDGB == 1)
                    {
                        clsOldChart.LoadDataXMLOldChart(this, MaxEmrNo, false, true, usFormTopMenuEvent.dtMedFrDate, usFormTopMenuEvent.txtMedFrTime);

                        control = Controls.Find("ta1", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = "";
                        }

                        control = Controls.Find("ta2", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = "";
                        }

                        control = Controls.Find("txtSearch", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                        }

                        //투석 횟수 + 1
                        control = Controls.Find("it2", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = (VB.Val(control.Text) + 1).ToString();
                        }
                    }
                    else
                    {
                        clsXML.LoadDataChartRow(clsDB.DbCon, this, MaxEmrNo, false, true, usFormTopMenuEvent.dtMedFrDate, usFormTopMenuEvent.txtMedFrTime);

                        control = Controls.Find("I0000035477", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = "";
                        }

                        control = Controls.Find("I0000033726", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = "";
                        }

                        control = Controls.Find("I0000030654", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = "";
                        }

                        control = Controls.Find("I0000030619", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = "";
                        }
                    }
                    #endregion

                    usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                    usFormTopMenuEvent.dtMedFrDate.Enabled = true;
                    usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
                }
                //입퇴원 요약지 신규서식지
                else if(pForm.FmFORMNO == 1647 && pForm.FmOLDGB != 1)
                {

                }
            }
            else if (VB.Val(mstrEmrNo) > 0)
            {
                #region 검수완료 관련 로직 수정(2021-06-28) // 07-01부터 적용
                //if (clsType.User.AuAMANAGE.Equals("1") && pForm.FmFORMNO == 1647)
                if (pForm.FmFORMNO == 1647)
                {
                    if (clsEmrFunc.READ_CHART_COMPLETE(clsDB.DbCon, pAcp, pForm))
                    {
                        usFormTopMenuEvent.mbtnComplete.Text = "검수완료";
                        usFormTopMenuEvent.mbtnComplete.ForeColor = Color.FromArgb(0, 0, 255);
                    }
                    else
                    {
                        usFormTopMenuEvent.mbtnComplete.Text = "미검수";
                        usFormTopMenuEvent.mbtnComplete.ForeColor = Color.FromArgb(255, 0, 0);
                    }

                    if (ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>() < "2021-07-01 06:00:00".To<DateTime>())
                    {
                        if (clsType.User.AuAMANAGE.Equals("1"))
                        {
                            usFormTopMenuEvent.mbtnComplete.Visible = true;
                        }
                    }
                    else
                    {
                        usFormTopMenuEvent.mbtnComplete.Visible = clsType.User.AuAMANAGE.Equals("1") || clsType.User.AuAMANAGE.Equals("1") == false && usFormTopMenuEvent.mbtnComplete.Text.Equals("검수완료");
                    }

                  
                }
                #endregion

                #region 신규 간호정보조사지 환자정보 FrTime 수정.
                if (pForm.FmFORMNO == 2678 && pForm.FmOLDGB == 0)
                {
                    clsEmrQuery.ErPatientFrTime(clsDB.DbCon, mstrEmrNo, ref pAcp);
                }
                #endregion

                //EMRNO가 있으면 기록 정보를 세팅을 한다.
                //  폴더생성
                LoadEmrChartInfo();
                if (pForm.FmOLDGB == 1)
                {
                    //if (pForm.FmFORMNO == 1647)
                    //{
                    //    btnSaveVisible.Visible = true;
                    //}
                    //혈액투석기록지 가장 최근내용 가져오기
                    if (pForm.FmFORMNO == 1577 && pForm.FmOLDGB == 1 || pForm.FmFORMNO == 2466 && pForm.FmOLDGB == 1)
                    {
                        Control control = Controls.Find("txtSearch", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
                        }
                    }

                    if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                    {
                        usFormTopMenuEvent.mbtnSave.Visible = false;
                        usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                        usFormTopMenuEvent.mbtnDelete.Visible = false;
                        //btnSaveVisible.Visible = false;
                    }

                    usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                }
                else
                {
                    //인공신장실, 수혈기록지  아닐때
                    if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                    {
                        usFormTopMenuEvent.mbtnSave.Visible = false;
                        usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                        usFormTopMenuEvent.mbtnDelete.Visible = false;
                    }

                    if (pForm.FmFORMNO == 1577 && clsType.User.BuseCode.Equals("033108") && clsType.User.IsNurse.Equals("OK") && clsType.User.AuAWRITE.Equals("1"))
                    {
                        usFormTopMenuEvent.mbtnSave.Visible = mstrMode.Equals("W");
                        usFormTopMenuEvent.mbtnSaveTemp.Visible = mstrMode.Equals("W");
                    }

                    if ((pForm.FmFORMNO == 1965 || pForm.FmFORMNO == 3535) && clsType.User.IsNurse.Equals("OK") && clsType.User.AuAWRITE.Equals("1"))
                    {
                        usFormTopMenuEvent.mbtnSave.Visible = mstrMode.Equals("W");
                        usFormTopMenuEvent.mbtnSaveTemp.Visible = mstrMode.Equals("W");
                        //usFormTopMenuEvent.mbtnDelete.Visible = mstrMode.Equals("W");
                    }  
                }

                DisChargerReCordVisible();
            }

            //사본발급 출력여부
            usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(mstrEmrNo);
            //if (usFormTopMenuEvent.lblPrntYn.Visible)
            //{
            //    usFormTopMenuEvent.mbtnSave.Visible = false;
            //    usFormTopMenuEvent.mbtnDelete.Visible = false;
            //}
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = pForm.FmOLDGB != 1;

            #region //환자정보 마스크 기능
            if (clsType.User.AuAMASK == "1")
            {
                Control[] controls = ComFunc.GetAllControls(panChart);
                foreach (Control c in controls)
                {
                    if (c is TextBox)
                    {
                        switch (VB.Left(c.Name.Trim(), 11))
                        {
                            case "I0000000360":
                            case "I0000000361":
                            case "I0000022077":
                            case "I0000032542":
                            case "I0000032606":
                            case "I0000033869":
                                //주민번호
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000022079":
                                //여권번호
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000022078":
                                //운전면허번호
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            //외국인등록번호 카드번호 계좌번호

                            case "I0000001142":
                            case "I0000032601":
                            case "I0000009884":
                                //등록번호
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000000306":
                            case "I0000031385":
                            case "I0000004637":
                            case "I0000016506":
                            case "I0000033907":
                            case "I0000034209":
                            case "I0000001378":
                            case "I0000013061":
                            case "I0000004638":
                            case "I0000022581":
                            case "I0000022525":
                            case "I0000019639":
                            case "I0000026253":
                            case "I0000029264":
                            case "I0000032643":
                            case "I0000035326":
                                //이름
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000028960":
                            case "I0000034207":
                            case "I0000034211":
                            case "I0000020326":
                            case "I0000020327":
                            case "I0000022071":
                            case "I0000000399":
                            case "I0000031464":
                            case "I0000032539":
                            case "I0000032620":
                            case "I0000032621":
                            case "I0000032636":
                            case "I0000032637":
                            case "I0000032638":
                            case "I0000032639":
                                //전화번호
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000000634":
                            case "I0000001721":
                                //핸드폰
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000022072":
                                //E-MAIL
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                            case "I0000032033":
                            case "I0000032605":
                            case "I0000033743":
                            case "I0000036193":
                            case "I0000000362":
                                //주소
                                if ((c as TextBox).Text.Trim() != "")
                                {
                                    (c as TextBox).Text = VB.Left((c as TextBox).Text, 1).PadRight((c as TextBox).Text.Length - 1, '*');
                                }
                                break;
                        }
                    }
                }
            }
            #endregion //환자정보 마스크 기능
        }

        /// <summary>
        /// 19-10-21 전산실 요청
        /// 퇴원자 퇴원이후 30일
        /// 외래 당일만 수정 가능하게.
        /// </summary>
        private bool ModificationTerm()
        {
            bool rtnVal = false;
            DateTime SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            //관리자거나 ER환자는 입력가능하게.
            if (clsType.User.AuAMANAGE.Equals("1") || pAcp.medDeptCd.Equals("ER"))
            {
                rtnVal = true;
                return rtnVal;
            }

            DateTime FrDate = DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null);

            if (pAcp.inOutCls.Equals("I"))
            {
                if(pAcp.medEndDate.Length == 0)
                {
                    rtnVal = true;
                }
                else
                {
                    //퇴원일 있을경우 현재시간 - 30일 
                    DateTime EndDate = DateTime.ParseExact(pAcp.medEndDate, "yyyyMMdd", null);
                    rtnVal = (SysDate.Date - EndDate.Date).TotalDays <= 30;
                }
            }
            else if (pAcp.inOutCls.Equals("O"))
            {
                //현재 서버시간이 내원당일인지
                rtnVal = SysDate.Date.Equals(FrDate.Date);
            }

            return rtnVal;
        }

        /// <summary>
        /// 사용자 템플릿 작성내역을 불러온다
        /// </summary>
        private void pSetEmrInfoTemplet()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            //clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            clsXML.LoadDataUserChartRow(clsDB.DbCon, this, mstrMACRONO, false);
        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        /// <summary>
        /// 기록지 내용이 변경되었는지 확인한다. : 최초 로드시에 체크
        /// </summary>
        /// <returns></returns>
        public string CheckChartChangeData()
        {
            if (this.Visible == false || Disposing == true)
            {
                return "";
            }

            string rtnVal = string.Empty;

            #region 빈 서식지 인지 확인
            int EmptyCnt = 0;

            List<Control> controls = ComFunc.GetAllControls(panChart).Where(Ctrl => Ctrl is TextBox).ToList();

            foreach (Control objControl in controls)
            {
                if ((objControl is TextBox))
                {
                    if (string.IsNullOrWhiteSpace(((TextBox)objControl).Text.Trim()))
                    {
                        EmptyCnt++;
                    }
                }
            }

            //모든 텍스트가 비어있으면 빠져나감.
            if (controls.Count > 0 && EmptyCnt == controls.Count)
            {
                return rtnVal;
            }
            #endregion

            string strXML = clsXML.SaveDataToXml(this, true, panChart, false);

            if (strXML != mstrXmlInit)
            {
                rtnVal = pForm.FmFORMNAME + " 변경된 내용이 있습니다.";
            }
            else
            {
                clsEmrType.EmrXmlImageInit[] pEmrXmlImageInit = null;
                SaveDataToXml(this, true, panChart, ref pEmrXmlImageInit, false); //이미지 컨트롤 값 저장

                //19-08-19 이미지비교 제대로 안되서 함수 추가함.

                if (mEmrXmlImageInit != null && pEmrXmlImageInit != null)
                {
                    for (int i = 0; i < mEmrXmlImageInit.Length; i++)
                    {
                        //if (clsEmrFunc.CompareBitmapsFast((Bitmap) pEmrXmlImageInit[i].Img, (Bitmap)mEmrXmlImageInit[i].Img) ==false ) 
                        if (pEmrXmlImageInit[i].Img != null && mEmrXmlImageInit[i].Img != null &&
                            clsEmrFunc.ImageCompare((Bitmap) pEmrXmlImageInit[i].Img, (Bitmap) mEmrXmlImageInit[i].Img) == false
                            ||
                            pEmrXmlImageInit[i].Img == null && mEmrXmlImageInit[i].Img != null ||
                            pEmrXmlImageInit[i].Img != null && mEmrXmlImageInit[i].Img == null) 
                        {
                            rtnVal = pForm.FmFORMNAME + " 변경된 내용이 있습니다.";
                            break;
                        }
                    } 
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double SaveData(string strFlag, bool blnCertYn)
        {
            double dblEmrNo = 0;
            
            if (VB.Val(mstrEmrNo) != 0)
            {
                if (pForm.FmOLDGB == 1)
                {
                    if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false && strSaveFlag.Length == 0) return VB.Val(mstrEmrNo);                    

                }
                else
                {
                    if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false && strSaveFlag.Length == 0 &&
                        pForm.FmFORMNO != 1577 && pForm.FmFORMNO != 1965 && pForm.FmFORMNO != 3535 &&
                        clsType.User.BuseCode.Equals("033108") == false)
                    {
                        return VB.Val(mstrEmrNo);
                    }
                }
            }

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
            {
                return VB.Val(mstrEmrNo);
            }

            Control ChkControl = null;
            if (CheckData(blnCertYn, ref ChkControl) == false)
            {
                ComFunc.MsgBoxEx(this, "필수 입력사항을 입력해주세요.");
                panChart.ScrollControlIntoView(ChkControl);
                return VB.Val(mstrEmrNo);
            }
                                  
            #region 작성일자 팝업 알림창
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            if (pAcp.inOutCls == "O")
            {
                if (pForm.FmFORMNO != 963 && pForm.FmFORMNO != 2432 && pForm.FmFORMNO != 2433 && 
                    pForm.FmFORMNO != 2434 && pForm.FmFORMNO != 1965 && pForm.FmFORMNO != 3535)
                {
                    if (pAcp.medFrDate != usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") && pAcp.medDeptCd.Equals("ER") == false)
                    {
                        ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 외래진료일내 작성요망");
                        return VB.Val(mstrEmrNo);
                    }
                }
                else 
                {
                    if (pAcp.medFrDate != usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") && ComFunc.MsgBoxQEx(this, "외래진료일과 챠트 작성일이 다릅니다. 계속 진행하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return VB.Val(mstrEmrNo);
                    }
                }

            }
            else
            {
                if (mstrFormNo.Equals("1647") == true)
                {
                    //퇴원요약지
                    #region 전산업무의뢰서 2019-1442 사망자는 작성일자 제한 풀어줌
                    if (clsEmrQueryOld.IsDeath(pAcp.ptNo) == false)
                    {
                        if (pAcp.medEndDate != "")
                        {
                            if ((VB.Val(pAcp.medFrDate) > VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))) || (VB.Val(pAcp.medEndDate) < VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))))
                            {
                                ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 입원기간내 작성요망");
                                return VB.Val(mstrEmrNo);
                            }
                        }
                        else
                        {
                            if ((VB.Val(pAcp.medFrDate) > VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))) || (VB.Val(strCurDate) < VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))))
                            {
                                ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 입원기간내 작성요망");
                                return VB.Val(mstrEmrNo);
                            }
                        }
                    }
                    #endregion
                }
                else
                {                 
                    if (mstrFormNo.Equals("963") == false)
                    {
                        if (pAcp.medEndDate != "")
                        {
                            if ((VB.Val(pAcp.medFrDate) > VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))) || (VB.Val(pAcp.medEndDate) < VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))))
                            {
                                ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 입원기간내 작성요망");
                                return VB.Val(mstrEmrNo);
                            }
                        }
                        else
                        {
                            if ((VB.Val(pAcp.medFrDate) > VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))) || (VB.Val(strCurDate) < VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))))
                            {
                                ComFunc.MsgBoxEx(this, "작성일자 오류입니다. 입원기간내 작성요망");
                                return VB.Val(mstrEmrNo);
                            }
                        }
                    }
                }
            }
            #endregion

            //입퇴원 요약지 점검
            if (CheckDischargeData() == false)
            {
                return VB.Val(mstrEmrNo);
            }

            #region 빈 서식지 인지 확인
            //모든 텍스트가 비어있으면 저장 못하게.
            List<Control> controls = ComFunc.GetAllControls(panChart).Where(Ctrl => Ctrl is TextBox).ToList();
            if (controls.Count > 0 && (controls.Where(c => string.IsNullOrWhiteSpace(c.Text.Trim())).Count()) == controls.Count)
            {
                return VB.Val(mstrEmrNo);
            }

            Control Img = panChart.Controls.Find("I0000029770", true).FirstOrDefault();
            if (pForm.FmFORMNO == 1232 && Img != null && Img.BackColor == Color.LightGray)
            {
                ComFunc.MsgBoxEx(this, "이미지를 저장해주세요.");
                return VB.Val(mstrEmrNo);
            }
            #endregion

            if (VB.Val(clsType.User.IdNumber) == 0)
            {
                ComFunc.MsgBox("정상적인 접근이 아닙니다. 다시 저장해주세요.");
                return VB.Val(mstrEmrNo);
            }

            //초진기록지 점검
            //19-10-28 해제
            //if (CheckFirstVisit() == false)
            //{
            //    return VB.Val(mstrEmrNo);
            //}

            #region //EMR 저장시 메시지 박스 기본 버튼(예1, 아니오0)
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2;
            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
                string strEmrGb = reg.GetValue("EmrSaveMsg", "0").ToString();
                reg.Close();
                reg.Dispose();

                defaultButton = strEmrGb.Equals("1") ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, "메시지박스 옵션를 불러오는중 오류가 발생했습니다." + ComNum.VBLF +
                    ex.Message);
            }
            #endregion

            if (VB.Val(mstrEmrNo) != 0 && strSaveFlag.Length == 0 && mEmrCallForm != null && ((Form)mEmrCallForm).Name == "frmEmrBaseEmrCertify" == false ||
                VB.Val(mstrEmrNo) != 0 && strSaveFlag.Length == 0 && mEmrCallForm == null)
            {
                if (ComFunc.MsgBoxQEx(this, "기존 내용을 변경하시겠습니까?", "EMR", defaultButton) == DialogResult.No)
                {
                    return VB.Val(mstrEmrNo);
                }
            }

            #region 간호기록 입원일시 응급실 출발일시 10분 초과 점검
            string strInDate = string.Empty;
            string strInTime = string.Empty;
            Control control = null;
            switch (pForm.FmFORMNO)
            {
                case 2294:
                case 2295:
                case 2296:
                case 2305:
                case 2311:
                case 2356:
                    string strErFrDate = FormPatInfoFunc.Set_FormPatInfo_ER_TRANS_TIME(clsDB.DbCon, pAcp);
                    if (pForm.FmUPDATENO == 1)
                    {
                        #region 이전 기로ㅓㄱ지
                        switch (pForm.FmFORMNO)
                        {
                            case 2294:
                            case 2296:
                            case 2305:
                            case 2311:
                                control = panChart.Controls.Find("dt2", true).FirstOrDefault();
                                if (control != null)
                                {
                                    strInDate = control.Text.Trim();
                                }

                                control = panChart.Controls.Find("it4", true).FirstOrDefault();
                                if (control != null)
                                {
                                    strInTime = control.Text.Trim();
                                }

                                break;
                            case 2295:
                                control = panChart.Controls.Find("dt3", true).FirstOrDefault();
                                if (control != null)
                                {
                                    strInDate = control.Text.Trim();
                                }

                                control = panChart.Controls.Find("it4", true).FirstOrDefault();
                                if (control != null)
                                {
                                    strInTime = control.Text.Trim();
                                }
                                break;
                            case 2356:
                                control = panChart.Controls.Find("dt2", true).FirstOrDefault();
                                if (control != null)
                                {
                                    strInDate = control.Text.Trim();
                                }

                                control = panChart.Controls.Find("it3", true).FirstOrDefault();
                                if (control != null)
                                {
                                    strInTime = control.Text.Trim();
                                }
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        control = panChart.Controls.Find("I0000029882", true).FirstOrDefault();
                        if (control != null)
                        {
                            strInDate = control.Text.Trim();
                        }

                        control = panChart.Controls.Find("I0000033484", true).FirstOrDefault();
                        if (control != null)
                        {
                            strInTime = control.Text.Trim();

                            if (strInTime.Length > 2)
                            {
                                string strHour = strInTime.Substring(0, 2);
                                if (strInTime.Length >= 2 && Regex.IsMatch(strHour, @"[0-9]") && strHour.To<double>() >= 24)
                                {
                                    ComFunc.MsgBoxEx(this, "입원 시간은 23시를 넘을 수 없습니다\r\n다시 확인 해주세요.", "오류");
                                    return VB.Val(mstrEmrNo);
                                }

                                if (strInTime.Length >= 2 && Regex.IsMatch(strInTime, "^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$") == false)
                                {
                                    control.Text = Regex.Replace(strInTime, @"[^0-9]", "");
                                    clsEmrFunc.AutoTimeText(control);
                                }
                            }
                        }
                    }
                    

                    strInDate = strInDate.Replace(" ", "");
                    strInDate = strInDate.Replace("년", "");
                    strInDate = strInDate.Replace("월", "");
                    strInDate = strInDate.Replace("일", "");
                    string strIpWonDate = strInDate + " " + strInTime;
                     

                    if (string.IsNullOrWhiteSpace(strInDate) == false && string.IsNullOrWhiteSpace(strInTime) == false && string.IsNullOrWhiteSpace(strErFrDate) == false)
                    {
                        DateTime dtpIpwon = Convert.ToDateTime(strIpWonDate);
                        DateTime dtpErDate = Convert.ToDateTime(strErFrDate);


                        TimeSpan timeSpan = (dtpIpwon - dtpErDate);
                        //if (VB.Val(FormPatInfoFunc.Set_FormPatInfo_TIMECOMPARE(clsDB.DbCon, strIpWonDate, strErFrDate)) > 10)
                        if (timeSpan.TotalMinutes > 10)
                        {
                            if (timeSpan.TotalMinutes > 60 * 24)
                            {
                                ComFunc.MsgBoxEx(this, "입원일시를 다시 확인하세요\r\n응급실 출발일시와의 차이 : " + timeSpan.TotalMinutes  + "분");
                                return VB.Val(mstrEmrNo);
                            }

                            if (ComFunc.MsgBoxQEx(this, "입원일시는 응급실 출발일시에서 10분을 초과 할 수 없습니다.\r\n" +
                                                   "★ 해당환자의 응급실 출발 일시 : " + strErFrDate + "\r\n" +
                                                   "확인하시기 바랍니다.입원일시를 수정하시겠습니까 ?") == DialogResult.No)
                            {
                                return VB.Val(mstrEmrNo);
                            }
                        }
                        else if (dtpIpwon.Date == dtpErDate.Date && 
                                 dtpIpwon.Hour == dtpErDate.Hour &&
                                 dtpIpwon.Minute == dtpErDate.Minute)
                        {
                            ComFunc.MsgBoxEx(this, "입원일시와 응급실 출발일시시가 동일 합니다.\r\n" +
                                                "★ 해당환자의 응급실 출발 일시 : " + strErFrDate + "\r\n" +
                                                "확인하시기 바랍니다.");
                            return VB.Val(mstrEmrNo);
                        }
                        else if (dtpIpwon.Date != usFormTopMenuEvent.dtMedFrDate.Value.Date)
                        {
                            if (ComFunc.MsgBoxQEx(this, "입원일자와 작성일자가 다릅니다.\r\n" +
                                                    "입원일자 : " + dtpIpwon.ToString("yyyy-MM-dd")  + "\r\n" +
                                                    "작성일자 : " + usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd")  + "\r\n" +
                                                    "다시 한번 확인하시겠습니까?") == DialogResult.No)
                            {
                                return VB.Val(mstrEmrNo);
                            }
                        }
                        else if (timeSpan.TotalMinutes < 0)
                        {
                            ComFunc.MsgBoxEx(this, "입원일시를 다시 확인하세요\r\n응급실 출발일시보다 작습니다.");
                            return VB.Val(mstrEmrNo);
                        }
                    }
                    break;
            }
            #endregion

            #region 간호정보 조사지 사생활 보호 업데이트
            RadioButton rdo = null;
            if (pForm.FmOLDGB == 1)
            {
                switch (pForm.FmFORMNO)
                {
                    case 2294:
                    case 2295:
                    case 2356:
                        rdo = panChart.Controls.Find("ir4", true).FirstOrDefault() as RadioButton;
                        if (rdo != null && (rdo as RadioButton).Checked)
                        {
                            FormPatInfoFunc.Set_FormPatInfo_Secret(clsDB.DbCon, pAcp);
                        }
                        break;
                    case 2311:
                        rdo = panChart.Controls.Find("ir2", true).FirstOrDefault() as RadioButton;
                        if (rdo != null && (rdo as RadioButton).Checked)
                        {
                            FormPatInfoFunc.Set_FormPatInfo_Secret(clsDB.DbCon, pAcp);
                        }


                        break;
                }
            }
            else
            {
                switch (pForm.FmFORMNO)
                {
                    case 2294:
                    case 2295:
                    case 2356:
                    case 2311:
                        rdo = panChart.Controls.Find("I0000034749", true).FirstOrDefault() as RadioButton;
                        if (rdo != null && (rdo as RadioButton).Checked)
                        {
                            FormPatInfoFunc.Set_FormPatInfo_Secret(clsDB.DbCon, pAcp);
                        }
                        break;
                }
            }
   
            #endregion

            #region 수혈기록지 30분 이내 점검
            if ((pForm.FmFORMNO == 1965 || pForm.FmFORMNO == 3535) && pForm.FmOLDGB != 1)
            {
                string Component = string.Empty;
                control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                if (control != null)
                {
                    Component = control.Text.Trim();
                }

                #region 응급실, 마취과 아닐때만
                if (clsType.User.BuseCode.Equals("033103") == false && clsType.User.BuseCode.Equals("033109") == false && Component.IndexOf("PLT/C(농축혈소판)") == -1)
                {
                    #region 불출일시
                    control = panChart.Controls.Find("I0000009935", true).FirstOrDefault();
                    if (control != null)
                    {
                        strInDate = control.Text.Trim();
                    }

                    control = panChart.Controls.Find("I0000037483", true).FirstOrDefault();
                    if (control != null)
                    {
                        strInTime = control.Text.Trim();
                    }
                    #endregion

                    string strBloodDate = strInDate + " " + strInTime;

                    #region 수혈 시작일시
                    control = panChart.Controls.Find("I0000037486", true).FirstOrDefault();
                    if (control != null)
                    {
                        strInDate = control.Text.Trim();
                    }

                    control = panChart.Controls.Find("I0000037487", true).FirstOrDefault();
                    if (control != null)
                    {
                        strInTime = control.Text.Trim();
                    }
                    #endregion

                    string strStartDate = strInDate + " " + strInTime;

                    DateTime dateTimeTemp;
                    if (DateTime.TryParse(strBloodDate, out dateTimeTemp) && DateTime.TryParse(strStartDate, out dateTimeTemp))
                    {
                        if ((dateTimeTemp - Convert.ToDateTime(strBloodDate)).TotalMinutes > 30)
                        {
                            if (ComFunc.MsgBoxQEx(this, "혈액불출일시에서 30분을 초과 할 수 없습니다.\r\n" +
                                                   "★ 해당환자의 혈액 불출 일시 : " + strBloodDate + "\r\n" +
                                                   "확인하시기 바랍니다. 수혈시작일시를 수정하시겠습니까 ?") == DialogResult.No)
                            {
                                return VB.Val(mstrEmrNo);
                            }
                        }
                    }
                }
                #endregion

                //1 Unit 투여량
                control = panChart.Controls.Find("I0000037510", true).FirstOrDefault();
                if (control != null && pForm.FmFORMNO == 1965)
                {
                    strInTime = control.Text.Trim();

                    //총 투여량
                    control = panChart.Controls.Find("I0000013528", true).FirstOrDefault();
                    if (control != null && VB.Val(control.Text.Trim()) > VB.Val(strInTime))
                    {
                        ComFunc.MsgBoxEx(this, "총 투여량은 1 Unit 투여량을 넘을 수 없습니다.");
                        return VB.Val(mstrEmrNo);
                    }
                }                
            }
            #endregion

            dblEmrNo = pSaveEmrData(blnCertYn);

            if (dblEmrNo == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
            else
            {
                #region DataMapping 저장
                if (dblEmrNo > 0)
                {
                    switch (pForm.FmFORMNO)
                    {
                        case 1965:
                            control = panChart.Controls.Find("I0000009925", true).FirstOrDefault();
                            if (control != null && control.Text.Trim().Length > 0)
                            {
                                string strBloodNo = control.Text.Trim();
                                control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                                if (control != null && control.Tag != null)
                                {
                                    clsEmrQuery.SaveDataMapping(clsDB.DbCon, this, pForm, dblEmrNo,  strBloodNo + control.Tag.ToString(), 0);
                                }
                            }
                            break;
                        case 3535:
                            List<Control> controls2 = FormFunc.GetAllControls(panChart).Where(d => d.Name.IndexOf("I0000009925") != -1 && d.Text.NotEmpty()).ToList();
                            if (controls2 != null)
                            {
                                foreach (Control control1 in controls2)
                                {
                                    string strBloodNo = control1.Text.Trim();
                                    string CtrlNumber = control1.Name.Split('_')[1];
                                    control = panChart.Controls.Find("I0000037484_" + CtrlNumber, true).FirstOrDefault();
                                    if (control != null && control.Tag != null)
                                    {
                                        clsEmrQuery.SaveDataMapping(clsDB.DbCon, this, pForm, dblEmrNo, strBloodNo + control.Tag.ToString(), 0);
                                    }
                                }
                            }
                            break;
                    }
                }
                #endregion

                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                mstrEmrNo = Convert.ToString(dblEmrNo);

                if (dblEmrNo != 0)
                {
                    ////전자인증 : 신규서식지만
                    //if (blnCertYn == true)
                    //{
                    //    if (pForm.FmOLDGB != 1)
                    //    {
                    //        //bool blnCert = clsEmrQuery.SaveDataAEMRCHARTCERTY(this, false, this, dblEmrNo, null);
                    //        //if (blnCert == false)
                    //        //{
                    //        //    ComFunc.MsgBoxEx(this, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                    //        //}
                    //    }
                    //}
                }

                if (isReciveOrderSave == false)
                {
                    //처방저장시에는 다시 조회 하지 않는다
                    //pSetEmrInfo();
                    if (mEmrCallForm != null)
                    {                        
                        strFlag = pForm.FmFORMNAME;
                        if ((mEmrCallForm as Form).Name.Equals("frmEmrBaseContinuView") ||
                            (mEmrCallForm as Form).Name.Equals("frmNrActingItemNew1") ||
                            (mEmrCallForm as Form).Name.Equals("frmEmrBaseProgressOcsNew"))
                        {
                            mEmrCallForm.MsgSave(strFlag);
                        }
                    }
                }
                
                DisChargerReCordVisible(false);

                //권한으로 설정 안했을때만.
                if (strSaveFlag.Length == 0)
                {
                    mstrEmrNo = "0";
                    pClearForm();
                    InitMibi();

                    clsEmrFunc.usBtnHide(usFormTopMenuEvent);
                    clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
                    SetInitChatValue();

                    if (pForm.FmFORMNO == 2278 && (clsType.User.DeptCode.Equals("MN") || clsType.User.DeptCode.Equals("MR")) && clsType.User.DrCode.Length > 0 ||
                        pForm.FmFORMNO == 2492 || pForm.FmFORMNO == 3611 ||
                        (pForm.FmFORMNO >= 3577 && pForm.FmFORMNO <= 3588 && clsType.User.DeptCode.Equals("MR") && clsType.User.DrCode.Length > 0))
                    {
                        usFormTopMenuEvent.mbtnPrint.Visible = true;
                        usFormTopMenuEvent.mbtnPrintNull.Visible = false;
                    }
                }
                else
                {
                    pSetCoading();
                }

                //사본발급 출력여부
                usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(dblEmrNo.ToString());
               
                //frmEmrBaseContinuView
                if (mEmrCallForm != null && ((mEmrCallForm as Form).Name.Equals("frmEmrLibViewerNr") ||
                    (mEmrCallForm as Form).Name.Equals("frmEmrBaseChartView") ||
                    (mEmrCallForm as Form).Name.Equals("frmEmrBaseChartWrite") ||
                    (mEmrCallForm as Form).Name.Equals("frmEmrBloodInfo") ||
                    (mEmrCallForm as Form).Name.Equals("frmEmrBaseContinuView") ||
                    (mEmrCallForm as Form).Name.Equals("frmEmrBaseProgressOcsNew"))
                    )
                {
                    mstrEmrNo = dblEmrNo.ToString();
                    LoadEmrChartInfo();
                }
                else
                {
                    mstrEmrNo = "0";
                    return 0;
                    //dblEmrNo = 0;
                }
            }

            return dblEmrNo;
        }

        /// <summary>
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetFormInfo();

            SetTopMenuLoad();

            mFormXml = FormDesignQuery.GetDataFormXml(mstrFormNo, mstrUpdateNo);
            if (mFormXml == null)
            {
                return;
            }

            if (mFormXml != null)
            {
                for (int i = 0; i < mFormXml.Length; i++)
                {
                    if (mFormXml[i].strCONTROLPARENT == "Form1")
                    {
                        mFormXml[i].strCONTROLPARENT = "panChart";
                    }

                    if (mFormXml[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                    {
                        mFormXml[i].strCONTROTYPE = "mtsPanel15.mPanel";
                    }

                }

                FormLoadControl.LoadControl(this, mFormXml, "panChart");
            }

            //SetControlEvents();
        }

        /// <summary>
        /// 폼에 이전내역을 세팅한다
        /// </summary>
        public void LoadForm()
        {
            pClearForm();
            pSetEmrInfo();
            SetInitChatValue();

            //  서식지 이미지
            GetFormImages();
            //pMultiTextHeigh();
        }

        /// <summary>
        /// 서식지 이미지 가져오기
        /// </summary>
        private void GetFormImages()
        {
            if(VB.Val(mstrEmrNo) > 0)
            {
                return;
            }

            DataTable dt = clsEmrQuery.GetFormImages(mstrFormNo, mstrUpdateNo);
            if(dt != null)
            {
                string basePath = Path.Combine(clsEmrType.EmrSvrInfo.EmrClient, "EmrImageTmp", "New");
                string folderName = string.Concat(mstrFormNo, "_", mstrUpdateNo, "_");
                foreach (DataRow row in dt.Rows)
                {
                    string itemName = row["CONTROLNAME"].ToString();
                    string tempForldName = Path.Combine(basePath, string.Concat(folderName, itemName));

                    if(Directory.Exists(tempForldName))
                    {
                        foreach(FileInfo file in new DirectoryInfo(tempForldName).GetFiles())
                        {
                            file.Delete();
                        }

                        Directory.Delete(tempForldName);
                    }
                    Directory.CreateDirectory(tempForldName);

                    //  ClOB -> Byte[] 변경
                    byte[] b = Convert.FromBase64String(row["IMAGE"].ToString());
                    using (FileStream image = File.Create(Path.Combine(tempForldName, string.Concat(row["CONTROLNAME"].ToString(), ".jpg"))))
                    {
                        image.Write(b, 0, b.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 사용자 템플릿 내용을 세팅한다
        /// </summary>
        public void LoadFormTemplet()
        {
            pClearForm();
            pSetEmrInfoTemplet();
            //pMultiTextHeigh();
        }

        #endregion

        #region //생성자
        public frmEmrChartNew()
        {
            InitializeComponent();
            //pInitForm();
        }

        public frmEmrChartNew(FormXml[] pFormXml)
        {
            mFormXml = pFormXml;
            InitializeComponent();
            pInitForm();
            LoadForm();
        }

        public frmEmrChartNew(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            //김현욱(2021-04-21) 나중에 주석 제거요망
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();
            pInitForm();
            LoadForm();
        }

        public frmEmrChartNew(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            InitializeComponent();
            pInitForm();
            LoadForm();
        }

        public frmEmrChartNew(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)  //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;
            InitializeComponent();
            pInitForm();
            LoadForm();
        }

        public frmEmrChartNew(string strFormNo, string strUpdateNo, string strEmrNo, FormXml[] pFormXml)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mFormXml = pFormXml;
            InitializeComponent();
            pInitForm();
            LoadForm();
        }

        /// <summary>
        /// 사용자 템플릿
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="isUserTemplet"></param>
        public frmEmrChartNew(string strFormNo, string strUpdateNo, bool isUserTemplet, string strMACRONO)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            misUserTemplet = isUserTemplet;
            mstrMACRONO = strMACRONO;
            InitializeComponent();
            pInitFormTemplet();
            LoadFormTemplet();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEmrChartNew_Load(object sender, EventArgs e)
        {
            this.Width = 700;

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            //clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            //clsEmrFunc.DeleteImageJobFoldAll();
            string strDir = "C:\\PSMHEXE\\EmrImageTmp\\New";
            if (Directory.Exists(strDir) == false)
            {
                Directory.CreateDirectory(strDir);
            }

            strDir = "C:\\PSMHEXE\\EmrImageTmp\\Update";
            if (Directory.Exists(strDir) == false)
            {
                Directory.CreateDirectory(strDir);
            }

            #region 서식생성기 에서 호출시 테스트 환자정보 자동 설정
            if (mCallFormGb == 1)
            {
                pAcp = clsEmrChart.ClearPatient();
                //pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "81000004", "I", ComQuery.CurrentDateTime(clsDB.DbCon, "D"));
            }
            #endregion

            //템플릿 저장화면에서 호출할 경우
            if (misUserTemplet == true)
            {
                panTopMenu.Visible = false; 
            }
            else
            {
                //LoadForm();
            }

            mLoading = true;

            if (mEmrCallForm != null && ((Form) mEmrCallForm).Name == "frmEmrBaseProgressOcsNew")
            {
                Width = 785;
                usFormTopMenuEvent.mbtnExit.Visible = true;
            }

            if (clsType.User.AuAMANAGE.Equals("1") && mstrMode.Equals("V"))
            {
                usFormTopMenuEvent.mbtnExit.Visible = true;
                pLockStart();
            }

            if (VB.Val(mstrEmrNo) > 0  && pForm.FmOLDGB == 0 && 
                usFormTopMenuEvent.mbtnSaveTemp.Visible && clsEmrQuery.EmrCertSave(clsDB.DbCon, mstrEmrNo))
            {
                usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                //인증저장 했으면 임시저장못하게 막기
            }

            

            #region 퇴실결과지는 무조건 추가로
            if (pForm.FmFORMNO == 3129 && mstrMode.Equals("W"))
            {
                tempMacro = clsEmrPublic.gstrMcrAddFlag;
                clsEmrPublic.gstrMcrAddFlag = "0";
            }
            #endregion

            if (pForm.FmFORMNO == 1647 && VB.Val(mstrEmrNo) == 0 && 
                FormPatInfoFunc.Set_FormPatInfo_DischargeRecordCnt(clsDB.DbCon, pAcp) > 0 )
            {
                ComFunc.MsgBoxEx(this, "이미 작성된 퇴원요약지가 있습니다 차트조회에서 퇴원요약지를 수정해주세요.");
            }

            #region 입퇴원 요약지 - 퇴원간호 계획지 에서 퇴원약 없음 체크시 자동 체크.
            if (VB.Val(mstrEmrNo) == 0 && pForm.FmOLDGB == 0 && pForm.FmFORMNO == 1647 && pAcp != null)
            {
                Control txtOutDrug = panChart.Controls.Find("I0000011150", true).FirstOrDefault();
                if (txtOutDrug != null && string.IsNullOrWhiteSpace(txtOutDrug.Text))
                {
                    clsEmrFunc.Set_FormPatInfo_OutDrugCheck(clsDB.DbCon, pAcp, this);
                }
            }

            #endregion

            #region EM Note ER 접수정보 아니면 메시지
            if (pForm.FmFORMNO == 2605 && VB.Val(mstrEmrNo) > 0 && pAcp.inOutCls.Equals("I") && usFormTopMenuEvent.mbtnSave.Visible)
            {
                EmrPatient tmpAcp =clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, pAcp.ptNo, "O", pAcp.medFrDate, "ER");
                if (tmpAcp != null)
                {
                    pAcp = tmpAcp;
                }
                else
                {
                    tmpAcp = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, mstrEmrNo);
                    if (tmpAcp != null)
                    {
                        pAcp = tmpAcp;
                    }
                }
            }
            #endregion


            if (pForm.FmGRPFORMNO >= 1050 && pForm.FmGRPFORMNO <= 1055 || pForm.FmGRPFORMNO == 1066 || pForm.FmGRPFORMNO == 1068)
            {
                //2492 치료계획 설명서(작성용), 3611 항암 치료 동의서 (작성용)
                if (pForm.FmFORMNO != 2492 && pForm.FmFORMNO != 3611)
                {
                    usFormTopMenuEvent.mbtnDelete.Visible = false;
                    usFormTopMenuEvent.mbtnSave.Visible = false;
                    usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                }
                panChart.Dock = DockStyle.Left;
                panChart.Width = 710;
            }


            if (pForm.FmFORMNO == 2278 && (clsType.User.DeptCode.Equals("MN") || clsType.User.DeptCode.Equals("MR")) && clsType.User.DrCode.Length > 0 ||
                pForm.FmFORMNO == 2492 || pForm.FmFORMNO == 3611 ||
                (pForm.FmFORMNO >= 3577 && pForm.FmFORMNO <= 3588 && clsType.User.DeptCode.Equals("MR") && clsType.User.DrCode.Length > 0)
                )
            {
                usFormTopMenuEvent.mbtnPrint.Visible = true;
                usFormTopMenuEvent.mbtnPrintNull.Visible = false;
                return;
            }

            if (pForm == null || pForm != null && pForm.FmOLDGB == 1)
                return;

            #region 신규서식지: 체크박스, 라디오, 텍스트박스에 내용이 있거나 그림이 업로드 되었는데 부모 패널이 숨겨져 있으면 보이게
            if (pForm.FmOLDGB != 1 && VB.Val(mstrEmrNo) > 0)
            {
                #region 작성시에 숨겼던것들 보이면 안되기에 이벤트를 그대로 다시 태운다..
                var controls = FormFunc.GetAllControls(panChart)
                    .Where(p =>
                    (p is CheckBox && (p as CheckBox).Checked && p.Tag != null) || (p is RadioButton && (p as RadioButton).Checked && p.Tag != null))
                    .Where(p =>
                    p.Tag.ToString().IndexOf("Set_ControlVisible") != -1 ||
                    p.Tag.ToString().IndexOf("Set_CheckValVisible") != -1 ||
                    p.Tag.ToString().IndexOf("Set_AutoPanelVisible") != -1 ||
                    p.Tag.ToString().IndexOf("Set_CheckControlVisible") != -1).ToList();

                foreach (Control control in controls)
                {
                    if (control is CheckBox)
                    {
                        CheckBox_CheckedChanged(control, null);
                    }
                    else
                    {
                        RadioButton_CheckedChanged(control, null);
                    }
                }
                #endregion

                controls = FormFunc.GetAllControls(panChart).Where(p =>
                (p is CheckBox && (p as CheckBox).Checked) ||
                (p is RadioButton && (p as RadioButton).Checked) ||
                (p is TextBox && p.Text.Length > 0) ||
                (p is PictureBox && p.Tag != null && VB.IsNumeric(p.Tag))).ToList();//.OrderByDescending(d => d.Name).ToList();

                foreach (Control control in controls)
                {
                    control.Visible = true;
                    if (control.Parent.Visible == false)
                    {
                        Console.WriteLine(control.Parent.Name);
                        if (control.Parent.Parent != null && control.Parent.Parent.Visible == false)
                        {
                            control.Parent.Parent.Visible = true;
                        }
                        clsEmrFunc.PanelAutoSize(control.Parent, true);
                    }
                }
            }
            #endregion

            //사용자면 무조건 수정모드
            if (pForm.FmFORMNO == 1647 && VB.Val(mstrEmrNo) > 0)
            {
                if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber))
                {
                    richTextBox1.Visible = false;
                    panChart.Visible = true;
                }
                else
                {
                    richTextBox1.Visible = true;
                    panChart.Visible = false;
                }

                return;
            }


            DataTable dt = null;
            OracleDataReader reader = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            #region 신규작성 이고 감염정보 라디오가 있다면 감염 여부를 확인하고 자동으로 체크
            Control RdoInfect = panChart.Controls.Find("I0000035366", true).FirstOrDefault();
            if (VB.Val(mstrEmrNo) == 0 && RdoInfect != null && pAcp != null && pForm.FmFORMNO != 2467 && pForm.FmFORMNO != 2280 && pForm.FmFORMNO != 1808)
            {
                SQL = FormPatInfoQuery.Query_FormPatInfo_INFECT(pAcp);

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (reader.GetValue(0).ToString().Trim().IndexOf("1") != -1)
                    {
                        (RdoInfect as RadioButton).Checked = true;
                        clsEmrFunc.Set_FormPatInfo_INFECT(clsDB.DbCon, this, pAcp, "Set_FormPatInfo_INFECT:GI0000019843_2");
                    }
                }
                reader.Dispose();

            }
            #endregion

            #region 신규 작성 + 알러지 체크 후 표시
            RdoInfect = panChart.Controls.Find("I0000034276", true).FirstOrDefault();
            if (VB.Val(mstrEmrNo) == 0 && pAcp != null && pForm.FmFORMNO != 2467 && pForm.FmFORMNO != 2280 && pForm.FmFORMNO != 1808)
            {
                clsEmrFunc.Set_FormPatInfo_AUTO_ALLERGY(clsDB.DbCon, panChart, pAcp);
            }
            #endregion

            #region 신규 작성 + 퇴원 간호 계획지
            if (VB.Val(mstrEmrNo) == 0 && pForm.FmOLDGB != 1 && (pForm.FmFORMNO == 966 || pForm.FmFORMNO == 1832) && pAcp != null)
            {
                reader = null;
                SQL = FormPatInfoQuery.Query_FormPatInfo_OPD_RESERVED(pAcp);

                Control control = panChart.Controls.Find("GI0000029099_2", true).FirstOrDefault();

                //해당 패널 안에서 텍스트만 이름순으로 정렬해서 뽑아낸다.
                List<Control> textBoxes = ComFunc.GetAllControls(control).Where(Ctl => Ctl is TextBox).OrderBy(Ctl => VB.Val(Ctl.Name.Substring(Ctl.Name.LastIndexOf("_") + 1))).ToList();

                try
                {
                    string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBox(sqlErr);
                        return;
                    }

                    if (reader.HasRows)
                    {
                        control = panChart.Controls.Find("I0000035170_1", true).FirstOrDefault();
                        if (control != null)
                        {
                            (control as RadioButton).Checked = true;
                        }

                        int ArrCnt = 0;
                        while (reader.Read())
                        {
                            if (ArrCnt >= 5 || ArrCnt >= textBoxes.Count)
                                break;

                            string rtnVal = string.Empty;
                            textBoxes[ArrCnt].Text = string.Format("{0}/{1}/{2}", reader.GetValue(0).ToString().Trim(), Convert.ToDateTime(reader.GetValue(1).ToString().Trim()).ToString("yyyy-MM-dd HH:mm"), reader.GetValue(3).ToString().Trim());
                            ArrCnt += 1;
                        }

                        control.Visible = true;
                        clsEmrFunc.PanelAutoSize(textBoxes[ArrCnt - 1].Parent, true);

                    }
                    //else
                    //{
                    //    control = panChart.Controls.Find("I0000035171", true).FirstOrDefault();
                    //}
                    reader.Dispose();

                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                }
            }
            #endregion

            #region 신규 작성 + 내시경(진정) 검사 전 기록지(씬규) - 2429(폼번호)
            if (pForm.FmFORMNO == 2429 && (mstrEmrNo.Equals("0") || mstrEmrNo.IsNullOrEmpty()))
            {
                Control[] controls;
                if (pAcp.sex.Equals("여"))
                {
                    controls = panChart.Controls.Find("I0000034952", true);
                    if (controls.Length > 0)
                    {
                        if (controls[0] is CheckBox)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                        else if (controls[0] is RadioButton)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }
                    }
                }
                string strPastMedicalHistory = clsEmrQueryEtc.CheckPastMedicalHistory(pAcp.ptNo, pAcp.medFrDate);
                //재원기간 중 간호정보조사지 내역의 과거병력 읽어와서 처리
                if (strPastMedicalHistory != "")
                {
                    controls = panChart.Controls.Find("I0000034263", true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as RadioButton).Checked = true;
                    }

                    controls = panChart.Controls.Find("I0000006088", true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as TextBox).Text = strPastMedicalHistory;
                    }
                    
                    controls = panChart.Controls.Find("I0000011890", true);

                    if (controls.Length > 0)
                    {
                        (controls[0] as RadioButton).Checked = true;
                    }

                }
            }
            #endregion

            
            if (clsEmrQueryEtc.CheckHCBuse(clsType.User.BuseCode) == true)
            {
                #region 신규 작성 + 건진직원일 경우 자동 설정 관련(2429, 2433)
                Control[] controls;

                if (pForm.FmFORMNO == 2429 && (mstrEmrNo.Equals("0") || mstrEmrNo.IsNullOrEmpty()))
                {
                    //아래는 건진내시경환자일 경우에만

                    if (pAcp.medDeptCd == "TO")
                    {
                        controls = panChart.Controls.Find("I0000033962", true);     //종합검진
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                    else if (pAcp.medDeptCd == "HR")
                    {
                        controls = panChart.Controls.Find("I0000000007", true);     //신체검진
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }

                    controls = panChart.Controls.Find("I0000000418", true);     //몸무게
                    if (controls != null)
                    {
                        controls[0].Text = clsEmrQueryEtc.ReadHCWeight(pAcp.ptNo, pAcp.medFrDate);
                    }


                    controls = panChart.Controls.Find("I0000033972", true);     //금식상태 시간
                    if (controls != null)
                    {
                        controls[0].Text = "8hr";
                    }

                    controls = panChart.Controls.Find("I0000034280", true);     //치아상태 양호
                    if (controls != null)
                    {
                        (controls[0] as CheckBox).Checked = true;
                    }

                    controls = panChart.Controls.Find("I0000011889", true);     //CLASS1
                    if (controls != null)
                    {
                        (controls[0] as RadioButton).Checked = true;
                    }
                    
                }


                if (pForm.FmFORMNO == 2433 && (mstrEmrNo.Equals("0") || mstrEmrNo.IsNullOrEmpty()))
                {
                    if(clsEmrQueryEtc.ReadHCEndoGubun(pAcp.ptNo, pAcp.medFrDate, pAcp.medDeptCd) == "수면")
                    {
                        controls = panChart.Controls.Find("I0000035273", true);     //진정평가 해당
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000033926", true);     //명령에 의해 네, 팔 다리 움직임
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035150", true);     //호흡 - 심호흡 및 기침 가능
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000034135", true);     //진정 전 비교 혈압의 ±20%미만 차이
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000033934", true);     //의식상태 - 완전깨어 지남력 있음
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000033937", true);     //산소포화도 - 분홍색, 산소포화도 92%이상
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035140", true);     //퇴실시 환자교육 - 환자 
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000034293", true);     //생검 미시행
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035145", true);     //퇴실시 환자교육 - 보호자 동반 퇴실
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035146", true);     //퇴실시 환자교육 - 자가운전 금지 
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035144", true);     //퇴실시 환자교육 - 식사/안정
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035147", true);     //퇴실시 환자교육 - 낙상예방
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035270", true);     //퇴실시 환자교육 - 검사 후 관리 리플렛 제공
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000027499", true);     //합계
                        if (controls != null)
                        {
                            controls[0].Text = "10";
                        }

                        
                    }
                    else
                    {
                        controls = panChart.Controls.Find("I0000033923", true);     //진정평가 미해당
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035140", true);     //퇴실시 환자교육 - 환자 
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000034293", true);     //생검 미시행
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035144", true);     //퇴실시 환자교육 - 식사/안정
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035147", true);     //퇴실시 환자교육 - 낙상예방
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035270", true);     //퇴실시 환자교육 - 검사 후 관리 리플렛 제공
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }

                }
                #endregion
            }
            else
            {
                #region 신규작성 + 건진 이외에 (2433)

                if (pForm.FmFORMNO == 2433 && (mstrEmrNo.Equals("0") || mstrEmrNo.IsNullOrEmpty()))
                {
                    Control[] controls;

                    if (clsEmrQueryEtc.ReadHCEndoGubun(pAcp.ptNo, pAcp.medFrDate, pAcp.medDeptCd) == "수면")
                    {
                        controls = panChart.Controls.Find("I0000035273", true);     //진정평가 해당
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035140", true);     //퇴실시 환자교육 - 환자 
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000034293", true);     //생검 미시행
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035145", true);     //퇴실시 환자교육 - 보호자 동반 퇴실
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035146", true);     //퇴실시 환자교육 - 자가운전 금지 
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035144", true);     //퇴실시 환자교육 - 식사/안정
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035147", true);     //퇴실시 환자교육 - 낙상예방
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035270", true);     //퇴실시 환자교육 - 검사 후 관리 리플렛 제공
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                    }
                    else
                    {
                        controls = panChart.Controls.Find("I0000033923", true);     //진정평가 미해당
                        if (controls != null)
                        {
                            (controls[0] as RadioButton).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035140", true);     //퇴실시 환자교육 - 환자 
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035144", true);     //퇴실시 환자교육 - 식사/안정
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035147", true);     //퇴실시 환자교육 - 낙상예방
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }

                        controls = panChart.Controls.Find("I0000035270", true);     //퇴실시 환자교육 - 검사 후 관리 리플렛 제공
                        if (controls != null)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                } 
                #endregion
            }

            GetImageLoad();
        }

        private void GetImageLoad()
        {
            if (mstrEmrNo.To<int>() == 0)
                return;

            #region 이미지 관련

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = string.Empty;
                SQL += "SELECT EMRNOHIS                             \r\n";
                if (mstrMode == "H")
                {
                    SQL += "  FROM KOSMOS_EMR.AEMRCHARTMSTHIS       \r\n";
                    SQL += " WHERE EMRNO > 0                        \r\n";
                    SQL += "   AND EMRNOHIS =  " + mstrEmrNo + "\r\n";
                }
                else
                {
                    SQL += "  FROM KOSMOS_EMR.AEMRCHARTMST          \r\n";
                    SQL += " WHERE EMRNO = " + mstrEmrNo + "        \r\n";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    string emrNohis = string.Empty;
                    emrNohis = dt.Rows[0]["EMRNOHIS"].ToString();

                    dt.Dispose();
                    dt = null;

                    #region 차트복사
                    if (mEmrCallForm != null && ((Form)mEmrCallForm).Name.Equals("frmEmrJobChartCopy"))
                    {
                        SQL = "";
                        SQL += "SELECT EMRNO                            \r\n";
                        SQL += "     , EMRNOHIS                         \r\n";
                        SQL += "     , FORMNO                           \r\n";
                        SQL += "     , UPDATENO                         \r\n";
                        SQL += "     , DRAW                             \r\n";
                        SQL += "     , FILENAME                         \r\n";
                        SQL += "     , IMAGE                            \r\n";
                        SQL += "     , IMAGENAME                        \r\n";
                        SQL += "     , ITEMNAME                         \r\n";

                        if (mstrMode == "H")
                        {
                            SQL += "  FROM KOSMOS_EMR.AEMRCHARTDRAWHIS          \r\n";
                            SQL += " WHERE EMRNO    > 0  \r\n";
                            SQL += "   AND EMRNOHIS = " + emrNohis + "    \r\n";
                        }
                        else
                        {
                            SQL += "  FROM KOSMOS_EMR.AEMRCHARTDRAW          \r\n";
                            SQL += " WHERE EMRNO    = " + mstrEmrNo + "   \r\n";
                            SQL += "   AND EMRNOHIS = " + emrNohis + "    \r\n";
                        }

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }

                        string basePath = @"C:\PSMHEXE\EmrImageTmp\Update";
                        foreach (DataRow row in dt.Rows)
                        {
                            string itemName = row["ITEMNAME"].ToString();
                            string emrNo    = row["EMRNO"].ToString();
                            string dtlName  = row["FILENAME"].ToString();
                            string imgName  = row["IMAGENAME"].ToString();
                            string dtlFile  = row["DRAW"].ToString();
                            string imgFile  = row["IMAGE"].ToString();

                            string folder = Path.Combine(basePath, string.Concat(emrNo, "_", itemName));
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            }
                            else
                            {
                                DirectoryInfo dir = new DirectoryInfo(folder);
                                foreach (FileInfo file in dir.GetFiles())
                                {
                                    file.Delete();
                                }
                            }


                            if (!string.IsNullOrWhiteSpace(dtlFile))
                            {
                                byte[] dtlFileByte = Convert.FromBase64String(dtlFile);
                                //  dtl byte -> 파일로 변환
                                using (FileStream file = File.Create(Path.Combine(folder, dtlName)))
                                {
                                    file.Write(dtlFileByte, 0, dtlFileByte.Length);
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(imgFile))
                            {
                                byte[] imgFileByte = Convert.FromBase64String(imgFile);
                                //  img byte -> 이미지로 변환
                                using (FileStream file = File.Create(Path.Combine(folder, imgName)))
                                {
                                    file.Write(imgFileByte, 0, imgFileByte.Length);
                                }

                                //  이미지 데이터로 전환
                                Image image = null;
                                using (MemoryStream ms = new MemoryStream(imgFileByte))
                                {
                                    image = Image.FromStream(ms);
                                }

                                //  이미지 데이터 PictureBox 로드
                                Control[] ctrls = panChart.Controls.Find(itemName, true);
                                foreach (Control ctrl in ctrls)
                                {
                                    (ctrl as PictureBox).Image = image;
                                }
                            }
                        }


                        dt.Dispose();
                        dt = null;

                    }
                    #endregion

                    #region 그외
                    else
                    {
                        SQL = "";
                        SQL += "SELECT EMRNO                                    \r\n";
                        SQL += "     , EMRNOHIS                                 \r\n";
                        SQL += "     , FORMNO                                   \r\n";
                        SQL += "     , UPDATENO                                 \r\n";
                        SQL += "     , DRAW                                     \r\n";
                        SQL += "     , FILENAME                                 \r\n";
                        SQL += "     , IMAGE                                    \r\n";
                        SQL += "     , IMAGENAME                                \r\n";
                        SQL += "     , ITEMNAME                                 \r\n";
                        if (mstrMode == "H")
                        {
                            SQL += "  FROM KOSMOS_EMR.AEMRCHARTDRAWHIS          \r\n";
                            SQL += " WHERE EMRNO    > 0                         \r\n";
                            SQL += "   AND EMRNOHIS = " + emrNohis + "          \r\n";
                        }
                        else
                        {
                            SQL += "  FROM KOSMOS_EMR.AEMRCHARTDRAW             \r\n";
                            SQL += " WHERE EMRNO    = " + mstrEmrNo + "         \r\n";
                            SQL += "   AND EMRNOHIS = " + emrNohis + "          \r\n";
                        }

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            Thread thread = new Thread(new ThreadStart(delegate ()
                            {
                                if (InvokeRequired)
                                {
                                    #region 쓰레드
                                    BeginInvoke(new Action(delegate ()
                                    {
                                        if (dt == null)
                                            return;

                                        string basePath = @"C:\PSMHEXE\EmrImageTmp\Update";
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            string itemName = row["ITEMNAME"].ToString();
                                            string emrNo = row["EMRNO"].ToString();
                                            string dtlName = row["FILENAME"].ToString();
                                            string imgName = row["IMAGENAME"].ToString();
                                            string dtlFile = row["DRAW"].ToString();
                                            string imgFile = row["IMAGE"].ToString();

                                            string folder = Path.Combine(basePath, string.Concat(emrNo, "_", itemName));
                                            if (!Directory.Exists(folder))
                                            {
                                                Directory.CreateDirectory(folder);
                                            }
                                            else
                                            {
                                                DirectoryInfo dir = new DirectoryInfo(folder);
                                                foreach (FileInfo file in dir.GetFiles())
                                                {
                                                    file.Delete();
                                                }
                                            }


                                            if (!string.IsNullOrWhiteSpace(dtlFile))
                                            {
                                                byte[] dtlFileByte = Convert.FromBase64String(dtlFile);
                                                //  dtl byte -> 파일로 변환
                                                using (FileStream file = File.Create(Path.Combine(folder, dtlName)))
                                                {
                                                    file.Write(dtlFileByte, 0, dtlFileByte.Length);
                                                }
                                            }

                                            if (!string.IsNullOrWhiteSpace(imgFile))
                                            {
                                                byte[] imgFileByte = Convert.FromBase64String(imgFile);
                                                //  img byte -> 이미지로 변환
                                                using (FileStream file = File.Create(Path.Combine(folder, imgName)))
                                                {
                                                    file.Write(imgFileByte, 0, imgFileByte.Length);
                                                }

                                                //  이미지 데이터로 전환
                                                Image image = null;
                                                using (MemoryStream ms = new MemoryStream(imgFileByte))
                                                {
                                                    image = Image.FromStream(ms);
                                                }

                                                //  이미지 데이터 PictureBox 로드
                                                Control[] ctrls = panChart.Controls.Find(itemName, true);
                                                foreach (Control ctrl in ctrls)
                                                {
                                                    (ctrl as PictureBox).Image = image;
                                                }
                                            }
                                        }
                                        dt.Dispose();
                                        dt = null;
                                    }));
                                    #endregion
                                }
                                else
                                {
                                    #region 쓰레드
                                    if (dt == null)
                                        return;

                                    string basePath = @"C:\PSMHEXE\EmrImageTmp\Update";
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        string itemName = row["ITEMNAME"].ToString();
                                        string emrNo = row["EMRNO"].ToString();
                                        string dtlName = row["FILENAME"].ToString();
                                        string imgName = row["IMAGENAME"].ToString();
                                        string dtlFile = row["DRAW"].ToString();
                                        string imgFile = row["IMAGE"].ToString();

                                        string folder = Path.Combine(basePath, string.Concat(emrNo, "_", itemName));
                                        if (!Directory.Exists(folder))
                                        {
                                            Directory.CreateDirectory(folder);
                                        }
                                        else
                                        {
                                            DirectoryInfo dir = new DirectoryInfo(folder);
                                            foreach (FileInfo file in dir.GetFiles())
                                            {
                                                file.Delete();
                                            }
                                        }


                                        if (!string.IsNullOrWhiteSpace(dtlFile))
                                        {
                                            byte[] dtlFileByte = Convert.FromBase64String(dtlFile);
                                            //  dtl byte -> 파일로 변환
                                            using (FileStream file = File.Create(Path.Combine(folder, dtlName)))
                                            {
                                                file.Write(dtlFileByte, 0, dtlFileByte.Length);
                                            }
                                        }

                                        if (!string.IsNullOrWhiteSpace(imgFile))
                                        {
                                            byte[] imgFileByte = Convert.FromBase64String(imgFile);
                                            //  img byte -> 이미지로 변환
                                            using (FileStream file = File.Create(Path.Combine(folder, imgName)))
                                            {
                                                file.Write(imgFileByte, 0, imgFileByte.Length);
                                            }

                                            //  이미지 데이터로 전환
                                            Image image = null;
                                            using (MemoryStream ms = new MemoryStream(imgFileByte))
                                            {
                                                image = Image.FromStream(ms);
                                            }

                                            //  이미지 데이터 PictureBox 로드
                                            Control[] ctrls = panChart.Controls.Find(itemName, true);
                                            foreach (Control ctrl in ctrls)
                                            {
                                                (ctrl as PictureBox).Image = image;
                                            }
                                        }
                                    }
                                    dt.Dispose();
                                    dt = null;
                                    #endregion
                                }


                            }));

                            thread.IsBackground = true;
                            thread.Start();
                        }
                    }
                    #endregion
                }
            }
            catch//(Exception ex)
            {

            }

            //thread.Join();
            #endregion

        }


        private void Frm_rEventBloodComponent(Control control, string Code, Form frm)
        {
            if (control == null || frm == null || string.IsNullOrWhiteSpace(Code))
            {
                Close();
            }

            control.Tag = Code;

            frm.Close();
            frm.Dispose();
        }
        #endregion //생성자

        private void btnSaveVisible_Click(object sender, EventArgs e)
        {
            panCoading.Visible = false;
            panChart.Visible = true;
        }

        private void frmEmrChartNew_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void frmEmrChartNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pForm.FmFORMNO == 3129 && mstrMode.Equals("W"))
            {
                clsEmrPublic.gstrMcrAddFlag = tempMacro;
            }

            if (frmAche != null)
            {
                frmAche.Dispose();
                frmAche = null;
            }

            if (frmAnFormExamX != null)
            {
                frmAnFormExamX.Dispose();
                frmAnFormExamX = null;
            }

            if (fEmrChartNewForm != null)
            {
                fEmrChartNewForm.Dispose();
                fEmrChartNewForm = null;
            }

            if (frmEmrLoginX != null)
            {
                frmEmrLoginX.Dispose();
                frmEmrLoginX = null;
            }

            if (frmMacrowordProgEvent != null)
            {
                frmMacrowordProgEvent.Dispose();
                frmMacrowordProgEvent = null;
            }
            if (frmEmrCaledarEvent != null)
            {
                frmEmrCaledarEvent.Dispose();
                frmEmrCaledarEvent = null;
            }
            if (fEmrMacro != null)
            {
                fEmrMacro.Dispose();
                fEmrMacro = null;
            }
            if (frmEmrVitalSignX != null)
            {
                frmEmrVitalSignX.Dispose();
                frmEmrVitalSignX = null;
            }

            if (fEmrHemodialysisInterface != null)
            {
                fEmrHemodialysisInterface.Dispose();
                fEmrHemodialysisInterface = null;
            }

        }
    }
}
