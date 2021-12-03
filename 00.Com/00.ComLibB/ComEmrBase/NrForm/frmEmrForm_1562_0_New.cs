using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrForm_1562_0_New : Form, EmrChartForm, FormEmrMessage
    {

        #region // 폼에 사용하는 변수를 코딩하는 부분
        //private frmEmrInitList frmEmrInitListEvent;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "1562";
        public string mstrUpdateNo = "0";
        public string mstrFormText = string.Empty;
        public EmrPatient p = null;
        public EmrForm pForm = null;
        public string mstrEmrNo = "0";
        public string mstrGBN = "NEW";
        public string mstrMode = "W";
        /// <summary>
        /// 조무사 작성용
        /// </summary>
        public string mstrDrSabun = string.Empty;

        string mFLOWGB = string.Empty; //서식작성 방향
        int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;
        int mFLOWINPUTSIZE = 0;

        FormFlowSheet[] mFormFlowSheet = null;
        FormFlowSheetHead[,] mFormFlowSheetHead = null;
        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region // TopMenu관련 이벤트 처리 함수

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

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usFormTopMenuEvent.txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void usFormTopMenuEvent_SetSave(string strFrDate, string strFrTime)
        {
            double dblEmrNo = 0;
            dblEmrNo = pSaveData();

            #region 오늘 작성된 내역 전자인증 다시
            if (p != null)
            {
                clsEmrFunc.NowEmrCert(clsDB.DbCon, p.medFrDate, p.ptNo);
            }
            #endregion

            if (dblEmrNo > 0)
            {
                this.Close();
            }
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
        }
        private void usFormTopMenuEvent_SetClear()
        {
            mstrEmrNo = "0";
            pClearForm();
        }
        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }
        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        #endregion

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            //텍스트 박스에 상용구 이벤트를 세팅한다

            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------

            pClearForm();
            pSetEmrInfo();
        }

        /// <summary>
        /// 기록지 정보를 세팅한다
        /// </summary>
        private void SetFormInfo()
        {
            pForm = null;
            pForm = clsEmrChart.ClearEmrForm();
            pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo, mstrUpdateNo);
        }

        private void pSetEmrInfo()
        {
            //clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            ////권한에 따라서 버튼을 세팅을 한다. 
            //clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            usFormTopMenuEvent.mbtnPrint.Visible = false;
            usFormTopMenuEvent.mbtnSave.Visible = true;
            usFormTopMenuEvent.mbtnSaveTemp.Visible = true;
            usFormTopMenuEvent.mbtnDelete.Visible = true;


            if (mstrFormNo != "0")
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
            }

            SetFormInfo();

            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            pLoadEmrChartInfo();

        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            double dblEmrNo = 0;

            dblEmrNo = pSaveEmrData();
            if (dblEmrNo == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
            else
            {
                txtUSEID.Clear();
                mstrEmrNo = "0";
                QueryChartList();
            }
            return dblEmrNo;
        }

        #endregion

        #region //EmrChartForm  
        public double SaveDataMsg(string strFlag)
        {
            return 0;

        }

        public bool DelDataMsg()
        {
            return false;

        }

        public void ClearFormMsg()
        {
            return;

        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            return;
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            return false;
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            return 0;

        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            return 0;
        }

        #endregion

        #region FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {
        }

        public void MsgDelete()
        {
        }

        public void MsgClear()
        {
        }

        public void MsgPrint()
        {
        }
        #endregion

        //==========================================================================================//
        //=============================== 아래부터 코딩을 하면 됨 ===================================//
        //=========================================================================================//

        #region // 기록지 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            //모든 컨트롤을 초기화 한다.
            ssWrite_Sheet1.RowCount = 0;
            ssWrite_Sheet1.RowCount = 1;
            ssWrite_Sheet1.SetRowHeight(-1, 24);
            //ssList_Sheet1.RowCount = 0;

            mstrEmrNo = string.Empty;
            mstrGBN = "NEW";

            //시간 세팅을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            usFormTopMenuEvent.mbtnTime.Visible = true;
            //자격에따라서 버튼을 설정을 한다.
            pClearFormExcept();
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            //mstrEmrNo = "1455366";
            if (VB.Val(mstrEmrNo) == 0)
            {
                return;
            }
            //출력한 기록지는 수정이 안되도록 막는다.
            //bool blnPRNYN = clsEmrQuery.GetPrnYnInfo(mstrEmrNo);
            //if (blnPRNYN == true)
            //{
            //    clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            //}

            QueryChartList(mstrEmrNo);
            //clsXML.LoadDataXML(this, mstrEmrNo, false);
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
                if (txtUSEID.Text != mstrDrSabun)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    return false;
                }
            }

            if (gDeleteEmrXml(mstrEmrNo, mstrDrSabun) == true)
            {
                mstrEmrNo = "0";
                pClearForm();
                QueryChartList();
            }
            return true;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {

        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            if (clsType.User.AuAWRITE == "1")
            {
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnClear");
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnSave");
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnDelete");
            }
            // 내원일을 세팅한다.
            //clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);
        }

        #endregion
        class GV
        {
            public string Code = "";
            public string Y = "";
            public double X = 0;
        }


        public frmEmrForm_1562_0_New()
        {
            InitializeComponent();
        }

        public frmEmrForm_1562_0_New(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmEmrForm_1562_0_New(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        /// <summary>
        /// 조무사 작성용
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strDrSabun"></param>
        public frmEmrForm_1562_0_New(string strFormNo, string strUpdateNo, EmrPatient po, string strDrSabun)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrMode = "W";
            mstrDrSabun = VB.Val(strDrSabun).ToString();
        }

        private void frmEmrForm_1562_0_New_Load(object sender, EventArgs e)
        {
            
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "N"); //폼 기본값 세팅 등    

            ssList.Dock = DockStyle.Fill;

            ssList_Sheet1.RowCount = 0;
            //clsSpread.gSpreadEnter_NextColumn(ssWrite);
            pInitForm();
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;

            btnPDForm.Visible = clsType.User.DeptCode.Equals("PD");

            SetUserAut();

            if (clsType.User.IsCoNurse.Equals("OK") && string.IsNullOrWhiteSpace(mstrDrSabun))
            {
                ComFunc.MsgBoxEx(this, "정상적인 접근이 아닙니다.");
                Close();
                return;
            }

            if (string.IsNullOrWhiteSpace(mstrDrSabun))
            {
                mstrDrSabun = clsType.User.IdNumber;
            }

            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"),"D");
            dtpEndDate.Value = Convert.ToDateTime(strCurDate);

            if (p.inOutCls == "I")
            {
                dtpFrDate.Value = VB.DateAdd("D", -7, strCurDate);
            }
            else
            {
                dtpFrDate.Value = VB.DateAdd("m", -12, strCurDate);
            }

            ssWrite_Sheet1.Cells[0, 0].Text = "정규";
            ssWrite_Sheet1.Cells[0, 3].Text = "Rt Arm";
            ssWrite_Sheet1.Cells[0, 7].Text = "고막";
            clsSpread.gSpreadEnter_NextCol(ssWrite);

            lblPatInfo.Text = p.ptName + "[" + p.ptNo + "] " + p.sex + "/" + p.age;

            QueryChartList();

            ssWrite_Sheet1.SetActiveCell(0, 1);
            ssWrite.Focus();
            Application.DoEvents();

            //SendPatInfoWeb();


        }


        private void SetUserAut()
        {
            //if (clsType.User.AuAWRITE == "1")
            //{
                panTopMenu.Visible = true;
                panChart.Visible = true;
            //}
            //else
            //{
            //    panTopMenu.Visible = false;
            //    panChart.Visible = false;
            //}
            //if (clsType.User.AuAPRINT == "1")
            //{
            //    mbtnPrint.Visible = true;
            //}
            //else
            //{
            //    mbtnPrint.Visible = true;
            //}
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            QueryChartList();
        }

        private void QueryChartList(string EmrNo = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수

            if (string.IsNullOrWhiteSpace(EmrNo))
            {
                ssList_Sheet1.RowCount = 0;
            }

            Cursor.Current = Cursors.WaitCursor;
            #region XML
            SQL = " SELECT  'OLD' AS GBN, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it1')   AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it2')   AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it3')   AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it4')   AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it5')   AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it6')   AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it7')   AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it8')   AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it9')   AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it14')  AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it10')  AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it11')  AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it12')  AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it13')  AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it121') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it150') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it274') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.USEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime,        ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN EXISTS (SELECT 1 FROM ADMIN.EMRPRTREQ WHERE EMRNO = A.EMRNO AND SCANYN = 'T') THEN '사 본' END PRNTYN";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B ";
            SQL = SQL + ComNum.VBLF + " WHERE a.EMRNO = b.EMRNO ";
            SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND B.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
            #endregion

            #region 신규
            SQL = SQL + ComNum.VBLF + " UNION ALL";
            SQL = SQL + ComNum.VBLF + " SELECT  'NEW' AS GBN, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         '' AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000024733') AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002018') AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001765') AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037575') AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000014815') AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002009') AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001811') AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035464') AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000008708') AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418') AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002') AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000018853') AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000029454') AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000017712') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000010747') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001311') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTUSEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime,        ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN A.PRNTYN = 'Y' THEN '사 본' END PRNTYN";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + " WHERE FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
            #endregion

            if (chkDesc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE ASC, CHARTTIME ASC";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);

            if (dt == null)
            {
                ComFunc.MsgBoxEx(this,  "조회중 문제가 발생했습니다");
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

            ssList_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D");
                ssList_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");

                ssList_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["COL2"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 3].Text = (dt.Rows[i]["COL3"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["COL4"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 5].Text = (dt.Rows[i]["COL5"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 6].Text = (dt.Rows[i]["COL6"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["COL7"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 8].Text = (dt.Rows[i]["COL8"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 9].Text = (dt.Rows[i]["COL9"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["COL14"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 11].Text = (dt.Rows[i]["COL10"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 12].Text = (dt.Rows[i]["COL11"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 13].Text = (dt.Rows[i]["COL12"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 14].Text = (dt.Rows[i]["COL13"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 15].Text = (dt.Rows[i]["COL21"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 16].Text = (dt.Rows[i]["COL150"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 17].Text = (dt.Rows[i]["COL274"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 18].Text = clsVbfunc.GetInSaName(clsDB.DbCon, (dt.Rows[i]["USEID"].ToString() + "").Trim());
                ssList_Sheet1.Cells[i, 19].Text = (dt.Rows[i]["PRNTYN"].ToString()).Equals("사 본").ToString();
                ssList_Sheet1.Cells[i, 20].Text = (dt.Rows[i]["EMRNO"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 20].Tag  = (dt.Rows[i]["GBN"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 21].Text = (dt.Rows[i]["USEID"].ToString() + "").Trim();

                ssList_Sheet1.SetRowHeight(i, 24);

                double rtnVal = VB.Val(ssList_Sheet1.Cells[i, 4].Text);
                if (rtnVal >= 140 || rtnVal < 80) // 혈압
                {
                    ssList_Sheet1.Cells[i, 4].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 5].Text);
                if (rtnVal >= 90 || rtnVal < 60) // 혈압
                {
                    ssList_Sheet1.Cells[i, 5].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 7].Text);
                if (rtnVal >= 100 || rtnVal < 60) //맥박
                {
                    ssList_Sheet1.Cells[i, 7].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 8].Text);
                if (rtnVal >= 21 || rtnVal < 12) //호흡
                {
                    ssList_Sheet1.Cells[i, 8].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 9].Text);
                if (rtnVal >= 37.5 || rtnVal < 36.5) //체온
                {
                    ssList_Sheet1.Cells[i, 9].ForeColor = Color.Red;
                }
            }


            dt.Dispose();
            dt = null;

            ssList_Sheet1.SetRowHeight(-1, 24);
            Cursor.Current = Cursors.Default;
        }


        private bool gDeleteEmrXml(string strEmrNo, string strDelSabun)
        {

            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);



                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                if (VB.Val(strEmrNo) > 0)
                {

                    #region XML 과거기록 백업
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME, UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, ";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', UPDATENO, ";
                    SQL = SQL + ComNum.VBLF + "       '" + mstrDrSabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion


                    #region //신규 과거기록 백업
                    SqlErr = clsEmrQuery.SaveChartMastHis(clsDB.DbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", strDelSabun);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    #endregion
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "삭제되었습니다.");
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pOldSaveEmrData()
        {
            double dblEmrNo = 0;
            string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "");

            string strXML = "";
            string strXmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";
            string strXmlTail = "\r\n" + "</chart>";
            string strChart = "";

            //strChart = strChart + ComNum.VBLF + "<it1 type = \"inputText\" label = \"Duty\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 0].Text.Trim() + "]]></it1>";
            strChart = strChart + ComNum.VBLF + "<it1 type = \"inputText\" label = \"Duty\"><![CDATA[]]></it1>";
            strChart = strChart + ComNum.VBLF + "<it2 type = \"inputText\" label = \"구분\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 0].Text.Trim() + "]]></it2>";
            strChart = strChart + ComNum.VBLF + "<it3 type = \"inputText\" label = \"혈압(Sys)\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 1].Text.Trim() + "]]></it3>";
            strChart = strChart + ComNum.VBLF + "<it4 type = \"inputText\" label = \"혈압(Dia)\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 2].Text.Trim() + "]]></it4>";
            strChart = strChart + ComNum.VBLF + "<it5 type = \"inputText\" label = \"혈압측정위치\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 3].Text.Trim() + "]]></it5>";
            strChart = strChart + ComNum.VBLF + "<it6 type = \"inputText\" label = \"맥박\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 4].Text.Trim() + "]]></it6>";
            strChart = strChart + ComNum.VBLF + "<it7 type = \"inputText\" label = \"호흡\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 5].Text.Trim() + "]]></it7>";
            strChart = strChart + ComNum.VBLF + "<it8 type = \"inputText\" label = \"체온\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 6].Text.Trim() + "]]></it8>";
            strChart = strChart + ComNum.VBLF + "<it9 type = \"inputText\" label = \"체온측정위치\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 7].Text.Trim() + "]]></it9>";
            strChart = strChart + ComNum.VBLF + "<it14 type = \"inputText\" label = \"SpO2\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 8].Text.Trim() + "]]></it14>";
            strChart = strChart + ComNum.VBLF + "<it10 type = \"inputText\" label = \"체중\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 9].Text.Trim() + "]]></it10>";
            strChart = strChart + ComNum.VBLF + "<it11 type = \"inputText\" label = \"신장\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 10].Text.Trim() + "]]></it11>";
            strChart = strChart + ComNum.VBLF + "<it12 type = \"inputText\" label = \"배둘레\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 11].Text.Trim() + "]]></it12>";
            strChart = strChart + ComNum.VBLF + "<it13 type = \"inputText\" label = \"FHR\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 12].Text.Trim() + "]]></it13>";
            strChart = strChart + ComNum.VBLF + "<it121 type = \"inputText\" label = \"머리둘레\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 13].Text.Trim() + "]]></it121>";
            strChart = strChart + ComNum.VBLF + "<it150 type = \"inputText\" label = \"가슴둘레\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 14].Text.Trim() + "]]></it150>";
            strChart = strChart + ComNum.VBLF + "<it274 type = \"inputText\" label = \"비고\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 15].Text.Trim() + "]]></it274>";

            strXML = strXmlHead + strChart + strXmlTail;

            //EmrPatient pTmp = null;
            //pTmp = clsEmrChart.ClearPatient();

            //if (VB.Val(mstrEmrNo) == 0)
            //{
            //    pTmp = p;
            //}
            //else
            //{
            //    pTmp = clsEmrChart.SetEmrPatInfoXml(mstrEmrNo);
            //}

            dblEmrNo = gSaveEmrXml(mstrEmrNo, strChartDate, strChartTime, strXML);

            if (dblEmrNo != 0)
            {
                //TODO 전자인증
                //if (clsType.gHosInfo.strEmrCertUseYn == "1")
                //{
                //    dblEmrCertNo = clsEmrCerti.SaveEmrCert(Convert.ToString(dblEmrNo), mstrEmrNo, strXML, strChartDate, strChartTime);
                //    if (dblEmrCertNo == 0)
                //    {
                //        MessageBox.Show(new Form() { TopMost = true }, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                //    }
                //}
            }
            return dblEmrNo;
        }

        private double gSaveEmrXml(string mstrEmrNo, string strChartDate, string strChartTime, string strXML)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            double dblEmrNo = 0;
            double dblEmrHisNo = 0;
            string strUPDATENO = "1";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (mstrEmrNo != "")
                {
                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "ADMIN.EMRXMLHISNO");
                    string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                    strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,HISTORYWRITEDATE,HISTORYWRITETIME, UPDATENO,EMRSIGNED,EMRXMLHASH)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', UPDATENO, EMRSIGNED, EMRXMLHASH";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(mstrEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(mstrEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(mstrEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                }


                dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,EMRSIGNED,EMRXMLHASH) VALUES (";
                SQL = SQL + ComNum.VBLF + "     " + dblEmrNo + ",";
                SQL = SQL + ComNum.VBLF + "     " + VB.Val("1562") + ",";
                SQL = SQL + ComNum.VBLF + "     '" + VB.Val(mstrDrSabun) + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "     " + VB.Val(p.acpNo) + ",";
                SQL = SQL + ComNum.VBLF + "     '" + p.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medFrDate + "',";     //'strMedFrDate
                SQL = SQL + ComNum.VBLF + "     '" + p.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "     :1,";
                SQL = SQL + ComNum.VBLF + "     '',";
                SQL = SQL + ComNum.VBLF + "     " + strUPDATENO + ",'','')";

                clsDB.ExecuteXmlQuery(SQL, strXML, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return 0;
                }

                SQL = "INSERT INTO ADMIN.EMRXMLMST (EmrNo,PtNo,GbEmr,FormNo,UseID,ChartDate,ChartTime,";
                SQL = SQL + ComNum.VBLF + " InOutCls,MedFrDate,MedFrTime,MedEndDate,MedEndTime,MedDeptCd,MedDrCd,";
                SQL = SQL + ComNum.VBLF + " WriteDate,WriteTime) ";
                SQL = SQL + ComNum.VBLF + " SELECT EmrNo,PtNo,'1',FormNo,UseID,ChartDate,ChartTime,InOutCls,";
                SQL = SQL + ComNum.VBLF + "        MedFrDate,MedFrTime,MedEndDate,MedEndTime,MedDeptCd,MedDrCd,";
                SQL = SQL + ComNum.VBLF + "        writeDate,writeTime ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.EMRXML ";
                SQL = SQL + ComNum.VBLF + "  WHERE EmrNo=" + dblEmrNo + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return 0;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "저장되었습니다.");
                return dblEmrNo;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return dblEmrNo;
            }
        }

        private double pSaveEmrData()
        {
            double rtnVal = VB.Val(mstrEmrNo);

            if (p.inOutCls == "" || p.ptNo == "" || p.medDeptCd == "" ||
                   p.medDrCd == "" || p.medFrDate == "")
            {
                ComFunc.MsgBoxEx(this, "환자 정보가 정확하지 않습니다." + ComNum.VBLF + "확인 후 다시 시도 하십시오.");
                return rtnVal;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (ComFunc.MsgBoxQEx(this, "기존내용을 변경하시겠습니까?") == DialogResult.No)
                {
                    return rtnVal;
                }

                if (mstrDrSabun != VB.Val(txtUSEID.Text.Trim()).ToString())
                {
                    ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                    return rtnVal;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                    return rtnVal;

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                    return rtnVal;
            }

            double dblEmrNo = 0;
            if (mstrGBN.Equals("NEW"))
            {
                string strSAVEGB = "1";
                string strSAVECERT = "1";  //if (blnCertYn == true)
                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
                string strChartTime = usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "").Trim();

                dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, p, this, false, this,
                                                                    mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                                    mstrDrSabun, mstrDrSabun, strSAVEGB, strSAVECERT, "0", string.Empty, ssWrite, "COL");

                if (dblEmrNo > 0)
                {
                    mstrEmrNo = dblEmrNo.ToString();
                    rtnVal = dblEmrNo;
                }
            }
            else if (mstrGBN.Equals("OLD"))
            {
                dblEmrNo = pOldSaveEmrData();
                mstrEmrNo = dblEmrNo.ToString();
                rtnVal = dblEmrNo;
            }
           

            return rtnVal;

        }

        private void LoadFlowData(string strEmrNo)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.WaitCursor;

            ssWrite_Sheet1.RowCount = 0;
            ssWrite_Sheet1.RowCount = 1;
            ssWrite_Sheet1.SetRowHeight(-1, 24);

            #region XML
            SQL = " SELECT  'OLD' AS GBN, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it1')   AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it2')   AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it3')   AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it4')   AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it5')   AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it6')   AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it7')   AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it8')   AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it9')   AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it14')  AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it10')  AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it11')  AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it12')  AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it13')  AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it121') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it150') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it274') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.USEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime,        ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN EXISTS (SELECT 1 FROM ADMIN.EMRPRTREQ WHERE EMRNO = A.EMRNO AND SCANYN = 'T') THEN '사 본' END PRNTYN";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B ";
            SQL = SQL + ComNum.VBLF + " WHERE a.EMRNO = b.EMRNO ";
            SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND B.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND B.EMRNO = " + strEmrNo;

            #endregion

            #region 신규
            SQL = SQL + ComNum.VBLF + " UNION ALL";
            SQL = SQL + ComNum.VBLF + " SELECT  'NEW' AS GBN, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         '' AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000024733') AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002018') AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001765') AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037575') AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000014815') AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002009') AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001811') AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035464') AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000008708') AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418') AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002') AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000018853') AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000029454') AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000017712') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000010747') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001311') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTUSEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime,        ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN A.PRNTYN = 'Y' THEN '사 본' END PRNTYN";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + " WHERE FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND EMRNO = " + strEmrNo;
            #endregion

            if (chkDesc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE ASC, CHARTTIME ASC";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (dt == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

            string strPRNYN = dt.Rows[0]["PRNTYN"].ToString();
            txtDAEmrNo.Text = mstrEmrNo;
            txtCHARTDATE.Text = (dt.Rows[0]["CHARTDATE"].ToString() + "").Trim();
            txtCHARTTIME.Text = (dt.Rows[0]["CHARTTIME"].ToString() + "").Trim();
            txtUSEID.Text = VB.Val(dt.Rows[0]["USEID"].ToString().Trim()).ToString();
            mstrGBN = (dt.Rows[0]["GBN"].ToString() + "").Trim();

            p = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, p.ptNo,
                dt.Rows[0]["INOUTCLS"].ToString().Trim(),
                dt.Rows[0]["MEDFRDATE"].ToString().Trim(),
                dt.Rows[0]["MEDDEPTCD"].ToString().Trim());

            if (p == null)
            {
                usFormTopMenuEvent.mbtnSave.Visible = false;
                usFormTopMenuEvent.mbtnDelete.Visible = false; 
                ComFunc.MsgBoxEx(this, "접수 정보를 찾을 수 없습니다.");
                return;
            }    

            //ssWrite_Sheet1.Cells[0, 0].Text = (dt.Rows[0]["COL1"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 0].Text = (dt.Rows[0]["COL2"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 1].Text = (dt.Rows[0]["COL3"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 2].Text = (dt.Rows[0]["COL4"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 3].Text = (dt.Rows[0]["COL5"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 4].Text = (dt.Rows[0]["COL6"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 5].Text = (dt.Rows[0]["COL7"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 6].Text = (dt.Rows[0]["COL8"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 7].Text = (dt.Rows[0]["COL9"].ToString() + "").Trim();

            ssWrite_Sheet1.Cells[0, 8].Text = (dt.Rows[0]["COL14"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 9].Text = (dt.Rows[0]["COL10"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 10].Text = (dt.Rows[0]["COL11"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 11].Text = (dt.Rows[0]["COL12"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 12].Text = (dt.Rows[0]["COL13"].ToString() + "").Trim();

            ssWrite_Sheet1.Cells[0, 13].Text = (dt.Rows[0]["COL21"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 14].Text = (dt.Rows[0]["COL150"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 15].Text = (dt.Rows[0]["COL274"].ToString() + "").Trim();


            dt.Dispose();
            dt = null;

            //Progress Note
            usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(txtCHARTDATE.Text, "yyyyMMdd", null);
            usFormTopMenuEvent.txtMedFrTime.Text =VB.Val(VB.Left(txtCHARTTIME.Text, 4)).ToString("00:00");

            usFormTopMenuEvent.dtMedFrDate.Enabled = false;

            if (mstrDrSabun != txtUSEID.Text)
            {
                usFormTopMenuEvent.mbtnSave.Visible = false;
                usFormTopMenuEvent.mbtnDelete.Visible = false;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(strPRNYN))
                {
                    usFormTopMenuEvent.mbtnSave.Visible = false;
                    usFormTopMenuEvent.mbtnDelete.Visible = false;
                }
                else
                {
                    usFormTopMenuEvent.mbtnSave.Visible = true;
                    usFormTopMenuEvent.mbtnDelete.Visible = true;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }
            string strEmrNo = ssList_Sheet1.Cells[e.Row, 20].Text;
            pClearForm();
            mstrEmrNo = strEmrNo;
            LoadFlowData(mstrEmrNo);
        }

        private void ssWrite_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssWrite_Sheet1.RowCount == 0) return;
            if (ssWrite_Sheet1.ActiveColumnIndex == ssWrite_Sheet1.ColumnCount) return;
            ssWrite_Sheet1.SetActiveCell(ssWrite_Sheet1.ActiveRowIndex, ssWrite_Sheet1.ActiveColumnIndex + 1);
        }

        private void ssWrite_EditModeOff(object sender, EventArgs e)
        {
            if (ssWrite_Sheet1.ActiveColumnIndex == ssWrite_Sheet1.ColumnCount) return;
            ssWrite_Sheet1.ActiveColumnIndex = ssWrite_Sheet1.ActiveColumnIndex + 1;
        }

        private void btnSearchData_Click(object sender, EventArgs e)
        {
            panGraph.Dock = DockStyle.Fill;
            panGraph.Visible = false;
            ssList.Dock = DockStyle.Fill;
            ssList.Visible = true;
            QueryChartList();
        }

        private void btnSearchGraph_Click(object sender, EventArgs e)
        {
            panGraph.Dock = DockStyle.Fill;
            panGraph.Visible = true;
            ssList.Dock = DockStyle.Fill;
            ssList.Visible = false;

            GetVitalGraph();
        }

        #region Grape관련

        private void mbtnSearchAll2_Click(object sender, EventArgs e)
        {
            GetVitalGraph();
        }

        private double WheelValue = 0;

        private void GetVitalGraph()
        {

            //데이터 조회
            if (p == null) return;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            List<GV> GVLsit = new List<GV>();
            List<string> XList = new List<string>();

            bool blnData = false;
            int i = 0;

            chartVital.Series.Clear();
            chartVital.Titles.Clear();
            chartVital.ChartAreas.Clear();

            chartVital.ChartAreas.Add("Default");
            chartVital.Titles.Add("Vital Sign");
            chartVital.Titles[0].Font = new Font("굴림", 16F, FontStyle.Bold);

            if ((chkSBP.Checked == false) && (chkPR.Checked == false) && (chkRR.Checked == false) && (chkBT.Checked == false))
            {
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                #region XML
                SQL = " SELECT  'OLD' AS GBN, ";
                SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
                SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
                SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it3')   AS I0000002018, ";
                SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it4')   AS I0000001765, ";
                SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it6')   AS I0000014815, ";
                SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it7')   AS I0000002009, ";
                SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it8')   AS I0000001811 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B ";
                SQL = SQL + ComNum.VBLF + " WHERE a.EMRNO = b.EMRNO ";
                SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "   AND B.PTNO = '" + p.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
                #endregion

                #region 신규
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT  'NEW' AS GBN, ";
                SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
                SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
                SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002018') AS I0000002018, ";
                SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001765') AS I0000001765, ";
                SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000014815') AS I0000014815, ";
                SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002009') AS I0000002009, ";
                SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM ADMIN.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001811') AS I0000001811 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " WHERE FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + p.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
                #endregion

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string strDateTime = VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4).Insert(2, "-") + "\r\n" + VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4).Insert(2, ":");

                    if (XList.IndexOf(strDateTime) == -1)
                    {
                        XList.Add(strDateTime);
                    }

                    if (chkSBP.Checked == true)
                    {
                        if (VB.Val(dt.Rows[i]["I0000002018"].ToString().Trim()) > 0)
                        {
                            blnData = true;

                            if (VB.Val(dt.Rows[i]["I0000002018"].ToString().Trim()) > 100)
                            {
                                GVLsit.Add(new GV()
                                {
                                    Code = "SBP"
                                    ,
                                    X = VB.Val(dt.Rows[i]["I0000002018"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                            else
                            {
                                GVLsit.Add(new GV()
                                {
                                    Code = "SBP2"
                                    ,
                                    X = VB.Val(dt.Rows[i]["I0000002018"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }

                        if (VB.Val(dt.Rows[i]["I0000001765"].ToString().Trim()) > 0)
                        {
                            blnData = true;

                            GVLsit.Add(new GV()
                            {
                                Code = "DBP"
                                ,
                                X = VB.Val(dt.Rows[i]["I0000001765"].ToString().Trim())
                                ,
                                Y = strDateTime
                            }
                            );
                        }
                    }

                    if (chkPR.Checked == true)
                    {
                        if (VB.Val(dt.Rows[i]["I0000014815"].ToString().Trim()) > 0)
                        {
                            blnData = true;

                            GVLsit.Add(new GV()
                            {
                                Code = "맥박"
                                ,
                                X = VB.Val(dt.Rows[i]["I0000014815"].ToString().Trim())
                                ,
                                Y = strDateTime
                            }
                            );
                        }
                    }

                    if (chkRR.Checked == true)
                    {
                        if (VB.Val(dt.Rows[i]["I0000002009"].ToString().Trim()) > 0)
                        {
                            blnData = true;

                            GVLsit.Add(new GV()
                            {
                                Code = "호흡"
                                ,
                                X = VB.Val(dt.Rows[i]["I0000002009"].ToString().Trim())
                                ,
                                Y = strDateTime
                            }
                            );
                        }   
                    }

                    if (chkBT.Checked == true)
                    {
                        if (VB.Val(dt.Rows[i]["I0000001811"].ToString().Trim()) > 0)
                        {
                            blnData = true;

                            GVLsit.Add(new GV()
                            {
                                Code = "체온"
                                ,
                                X = VB.Val(dt.Rows[i]["I0000001811"].ToString().Trim())
                                ,
                                Y = strDateTime
                            }
                            );
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (blnData == false) return;

                chartVital.ChartAreas["Default"].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 5, 85, 90);
                chartVital.ChartAreas["Default"].InnerPlotPosition = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(13, 5, 90, 90);

                if (chkSBP.Checked == true)
                {
                    chartVital.Series.Add("SBP");
                    chartVital.Series["SBP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["SBP"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP.png";
                    chartVital.Series["SBP"].IsValueShownAsLabel = false;

                    chartVital.Series.Add("DBP");
                    chartVital.Series["DBP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["DBP"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\DBP.png";
                    chartVital.Series["DBP"].IsValueShownAsLabel = false;

                    chartVital.Series.Add("SBP2");
                    chartVital.Series["SBP2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["SBP2"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP2.png";
                    chartVital.Series["SBP2"].IsValueShownAsLabel = false;
                    chartVital.Series["SBP2"].IsVisibleInLegend = false;
                }

                if (chkPR.Checked == true)
                {
                    chartVital.Series.Add("맥박");
                    chartVital.Series["맥박"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["맥박"].BorderWidth = 2;
                    chartVital.Series["맥박"].Color = Color.IndianRed;
                    chartVital.Series["맥박"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["맥박"].MarkerColor = Color.IndianRed;
                    chartVital.Series["맥박"].MarkerSize = 6;

                }

                if (chkRR.Checked == true)
                {
                    chartVital.Series.Add("호흡");
                    chartVital.Series["호흡"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["호흡"].BorderWidth = 2;
                    chartVital.Series["호흡"].Color = Color.Gold;
                    chartVital.Series["호흡"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["호흡"].MarkerColor = Color.Gold;
                    chartVital.Series["호흡"].MarkerSize = 6;

                }

                if (chkBT.Checked == true)
                {
                    chartVital.Series.Add("체온");
                    chartVital.Series["체온"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["체온"].BorderWidth = 2;
                    chartVital.Series["체온"].Color = Color.Blue;
                    chartVital.Series["체온"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["체온"].MarkerColor = Color.Blue;
                    chartVital.Series["체온"].MarkerSize = 6;

                }

                chartVital.Series.Add("주의선");
                chartVital.Series["주의선"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartVital.Series["주의선"].BorderWidth = 2;
                chartVital.Series["주의선"].Color = Color.Orange;
                chartVital.Series["주의선"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;


                // X축 그리기

                XList.Sort();

                foreach (string DateTiem in XList)
                {
                    if (GVLsit.Where(d => d.Y == DateTiem).Any())
                    {
                        List<GV> list = GVLsit.Where(d => d.Y == DateTiem).ToList();

                        foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chartVital.Series)
                        {
                            if (list.Where(d => d.Code == series.Name).Any())
                            {
                                series.Points.AddY(list.Where(d => d.Code == series.Name).First().X);
                            }
                            else
                            {
                                if (series.Name == "주의선")
                                {
                                    series.Points.AddY(100);
                                    continue;
                                }

                                series.Points.AddY(double.NaN);
                                series.Points[series.Points.Count - 1].IsEmpty = true;
                            }
                        }

                        chartVital.Series[0].Points[chartVital.Series[0].Points.Count - 1].AxisLabel = DateTiem;

                    }
                }

                chartVital.ChartAreas["Default"].AxisX.Interval = 1;
                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                chartVital.ChartAreas["Default"].AxisY.Minimum = 40; //30
                chartVital.ChartAreas["Default"].AxisY.Maximum = 250; //250
                chartVital.ChartAreas["Default"].Position.X = 12;
                chartVital.ChartAreas["Default"].InnerPlotPosition.X = 2;
                chartVital.ChartAreas["Default"].AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.ScrollBar.Enabled = true;
                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;

                chartVital.Series["주의선"].ChartArea = "Default";


                CheckBox[] checkAee = new CheckBox[4];

                checkAee[0] = chkSBP;
                checkAee[1] = chkPR;
                checkAee[2] = chkRR;
                checkAee[3] = chkBT;


                int f = 1;


                foreach (CheckBox box in checkAee)
                {
                    if (box.Checked == true)
                    {

                        if (box == chkPR)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 30;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 150;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.IndianRed;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.IndianRed;

                            }

                            f += 1;

                        }

                        if (box == chkRR)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 1;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 1;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 10;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 40;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Gold;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Gold;
                            }

                            f += 1;

                        }

                        if (box == chkBT)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 0.5;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 0.5;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 34.0;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 45.0;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Blue;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Blue;
                            }

                            f += 1;

                        }

                    }
                }

                if (chkSBP.Checked == false)
                {
                    f = f - 1;
                }

                int PositionX = 0; // Y축범위 가로 간격

                if (chartVital.Width <= 970)
                {
                    PositionX = 4;
                }
                else
                {
                    PositionX = 3;
                }

                chartVital.ChartAreas["Default"].Position.X = PositionX * f;


                f = 1;

                foreach (CheckBox box in checkAee)
                {
                    if (box.Checked == true)
                    {
                        if (box == chkRR)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Gold)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["호흡"], PositionX * f, 2);
                            f += 1;
                        }

                        if (box == chkPR)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.IndianRed)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["맥박"], PositionX * f, 2);
                            f += 1;
                        }

                        if (box == chkBT)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Blue)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["체온"], PositionX * f, 2);
                            f += 1;
                        }
                    }
                }

                chartVital.ChartAreas["Default"].AxisY.LineWidth = 2;

                if (chartVital.ChartAreas.Count > 0)
                {
                    for (int k = 1; k < chartVital.ChartAreas.Count; k++)
                    {
                        if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "맥박")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 10;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 10;
                            chartVital.ChartAreas[k].AxisY.Minimum = 30;
                            chartVital.ChartAreas[k].AxisY.Maximum = 150;
                        }
                        else if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "체온")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 0.5;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 34.0;
                            chartVital.ChartAreas[k].AxisY.Maximum = 45.0;
                        }
                        else if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "호흡")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 1;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 10;
                            chartVital.ChartAreas[k].AxisY.Maximum = 40;
                        }
                    }
                }

                WheelValue = 21;

                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

                if (chartVital.Series[0].Points.Count > WheelValue)
                {
                    chartVital.ChartAreas["Default"].AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                }

                foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
                {
                    if (item == chartVital.ChartAreas["Default"])
                        continue;

                    item.AxisX.ScrollBar.Enabled = true;
                    item.AxisX.ScaleView.Zoomable = true;
                    item.AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.None;
                    item.AxisX.ScrollBar.ChartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    item.AxisX.ScrollBar.LineColor = Color.Transparent;
                    item.AxisX.ScaleView.Zoom(0, WheelValue);

                    if (chartVital.Series[0].Points.Count > WheelValue)
                    {
                        item.AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                    }

                }

                //=========
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);

            }

            Cursor.Current = Cursors.Default;

        }

        private void chartVital_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum < WheelValue + 1)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum;
                }
                else
                {
                    WheelValue = WheelValue + 1;
                }
            }

            if (e.Delta > 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum > WheelValue)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum;
                }
                else
                {
                    WheelValue = WheelValue - 1;
                }
            }

            chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

            foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            {
                if (item == chartVital.ChartAreas["Default"])
                    continue;

                item.AxisX.ScaleView.Zoom(0, WheelValue);
            }

        }

        private void chartVital_DoubleClick(object sender, EventArgs e)
        {
            //chartVital.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();

            //foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            //{
            //    if (item == chartVital.ChartAreas["Default"])
            //        continue;

            //    item.AxisX.ScaleView.ZoomReset();
            //}
        }

        public void CreateYAxis(System.Windows.Forms.DataVisualization.Charting.Chart chart, System.Windows.Forms.DataVisualization.Charting.ChartArea area,
            System.Windows.Forms.DataVisualization.Charting.Series series, float axisOffset, float labelsSize)
        {
            // Create new chart area for original series
            System.Windows.Forms.DataVisualization.Charting.ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;


            series.ChartArea = areaSeries.Name;

            if (series.Name == "체온"
                || series.Name == "맥박"
                || series.Name == "호흡")
            {

                // Create new chart area for axis
                System.Windows.Forms.DataVisualization.Charting.ChartArea areaAxis = chart.ChartAreas.Add("AxisY-" + series.ChartArea);
                areaAxis.BackColor = Color.Transparent;
                areaAxis.BorderColor = Color.Transparent;
                areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
                areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

                // Create a copy of specified series
                System.Windows.Forms.DataVisualization.Charting.Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
                seriesCopy.ChartType = series.ChartType;
                foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint point in series.Points)
                {
                    seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
                }

                // Hide copied series
                seriesCopy.IsVisibleInLegend = false;
                seriesCopy.Color = Color.Transparent;
                seriesCopy.BorderColor = Color.Transparent;
                seriesCopy.ChartArea = areaAxis.Name;

                // Disable drid lines & tickmarks
                areaAxis.AxisX.LineWidth = 0;
                areaAxis.AxisX.MajorGrid.Enabled = false;
                areaAxis.AxisX.MajorTickMark.Enabled = false;
                areaAxis.AxisX.LabelStyle.Enabled = false;
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
                areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

                if (series.Name == "체온")
                {
                    areaAxis.AxisY.LineColor = Color.Blue;
                    areaAxis.AxisY.InterlacedColor = Color.Blue;
                }
                else if (series.Name == "맥박")
                {
                    areaAxis.AxisY.LineColor = Color.IndianRed;
                    areaAxis.AxisY.InterlacedColor = Color.IndianRed;
                }
                else if (series.Name == "호흡")
                {
                    areaAxis.AxisY.LineColor = Color.Gold;
                    areaAxis.AxisY.InterlacedColor = Color.Gold;
                }

                areaAxis.AxisY.LineWidth = 2;

                // Adjust area position
                areaAxis.Position.X = axisOffset;
                areaAxis.InnerPlotPosition.X = labelsSize;
            }

        }


        #endregion Grape관련

        private void btnPDForm_Click(object sender, EventArgs e)
        {
            using(frmEmrHealthGraph frm = new frmEmrHealthGraph(mstrFormNo, mstrUpdateNo, p, "0", "W", "", "", this))
            {
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog(this);
            }
        }
    }
}
