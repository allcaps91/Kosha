using System;
using ComBase;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrPrintFlowSheet : Form, EmrChartForm
    {
        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region //폼에서 사용하는 변수
        //const string mDirection = "V";   //기록지 작성방향(H: 옆으로, V:아래로)
        //const bool mHeadVisible = true;   //해드를 보이게 할지 여부
        //const int mintHeadCol = 4;  //해드 칼럼 수(작성, 조회 공통)
        //const int mintHeadRow = 2;  //해드 줄 수 (조회시에)

        //string mstrFormName = "";
        //string mstrPrintType = "";

        int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,

        //bool mHeadVisible = true;   //해드를 보이게 할지 여부
        string mFLOWGB = "COL"; //서식작성 방향
        int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;
        int mFLOWINPUTSIZE = 0;

        //int mRow = 0;
        //int mCol = 0;
        FormFlowSheet[] mFormFlowSheet = null;
        FormFlowSheetHead[,] mFormFlowSheetHead = null;

        FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
        FarPoint.Win.Spread.CellType.ComboBoxCellType TypeComboBox = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
        FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;

        public string mstrFormNo = "2185";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";
        public string mstrPrtSeq = string.Empty;

        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        private void SetTopMenuLoad()
        {
            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            //usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------
        }

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
            //pSaveData();
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            //pDelData();
        }

        private void usFormTopMenuEvent_SetClear()
        {
            mstrEmrNo = "0";
            //pClearForm();
        }

        private void usFormTopMenuEvent_SetPrint()
        {
            //pPrintForm();
        }

        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            //필요시만 코딩함
        }

        #endregion

        #region // 상용구 관련 변수 선언
        //private Control mControl = null;
        //private frmMacrowordProg frmMacrowordProgEvent;

        //private Control mCalControl = null; //달력 띄우기
        //private frmEmrCaledar frmEmrCaledarEvent;

        //private FarPoint.Win.Spread.FpSpread ssMacroWord;
        //private FarPoint.Win.Spread.SheetView ssMacroWord_Sheet1;
        #endregion // 상용구 관련 모듈

        #region //EmrChartForm
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            bool rtnVal = false;
            //rtnVal = pDelData();
            return rtnVal;
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            //pClearForm();
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            return rtnVal;
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {

        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //SetPrintVisible();

            if (mFLOWGB == "ROW")
            {
                rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, ssView_Sheet1.RowCount - 3, mFLOWHEADCNT, "P");
            }
            else
            {
                rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Portrait, mFLOWGB, -1, ssView_Sheet1.ColumnCount - 3, mFLOWHEADCNT, "P");
            }

            pInitFormSpc();

            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        
        #region //생성자
        public frmEmrPrintFlowSheet()
        {
            InitializeComponent();
        }

        public frmEmrPrintFlowSheet(string pMake, int pItemCnt, int pHeadCnt, FormFlowSheet[] pFormFlowSheet, FormFlowSheetHead[,] pFormFlowSheetHead)
        {
            InitializeComponent();
            mFLOWGB = pMake;
            mFLOWITEMCNT = pItemCnt;
            mFLOWHEADCNT = pHeadCnt;
            mFormFlowSheet = pFormFlowSheet;
            mFormFlowSheetHead = pFormFlowSheetHead;

            pInitFormTemplet();
        }

        public frmEmrPrintFlowSheet(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;

            pInitFormTemplet();
        }

        public frmEmrPrintFlowSheet(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrEmrNo = strEmrNo;
            InitializeComponent();

            pInitFormTemplet();
        }

        /// <summary>
        /// 차트복사 => 작성일자 조회 위해서 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strChartDate"></param>
        /// <param name="pEmrCallForm"></param>
        public frmEmrPrintFlowSheet(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strPrtSeq, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrEmrNo = strEmrNo;
            mstrPrtSeq = strPrtSeq;
            InitializeComponent();

            pInitFormTemplet();
        }

        private void frmEmrPrintFlowSheet_Load(object sender, EventArgs e)
        {
            //clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            //if (mCallFormGb != 1)
            //{
            //    SetUserAut();
            //}

            //usFormTopMenuEvent.mbtnClear.Visible = true;
            //usFormTopMenuEvent.mbtnPrint.Visible = true;
            //usFormTopMenuEvent.mbtnSave.Visible = true;
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = true;


            //자동조회
            QuerySpdList();
            //mbtnSearchAll.PerformClick();
        }

        #endregion //생성자


        #region //Private Function

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
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetFormInfo();

            if (mstrFormNo != "0")
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
                mFLOWGB = pForm.FmFLOWGB;
            }

            if (mFormFlowSheetHead != null)
            {
                SetWriteSpd();
            }

        }

        /// <summary>
        /// 권한별 환경을 설정을 한다(작성화면 안보이게 등)
        /// </summary>
        private void SetUserAut()
        {
            if (clsType.User.AuAPRINTIN == "1")
            {
                mbtnPrint.Visible = true;
            }
            else
            {
                mbtnPrint.Visible = true;
            }
        }

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.ColumnCount = 0;
            //mRow = -2;
            //mCol = -2;
        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        private void pInitFormSpc()
        {
            //int i = 0;
            //if (mFLOWGB == "ROW")
            //{
            //    for (i = 0; i < mFLOWHEADCNT; i++)
            //    {
            //        if (mHeadVisible == true)
            //        {
            //            if (i == 0)
            //            {
            //                ssView_Sheet1.Columns[i].Visible = false;
            //            }
            //            else
            //            {
            //                ssView_Sheet1.Columns[i].Visible = true;
            //            }
            //        }
            //        else
            //        {
            //            ssView_Sheet1.Columns[i].Visible = false;
            //        }
            //    }
            //}
            //else
            //{
            //    for (i = 0; i < mFLOWHEADCNT; i++)
            //    {
            //        if (mHeadVisible == true)
            //        {
            //            if (i == 0)
            //            {
            //                ssView_Sheet1.Rows[i].Visible = false;
            //            }
            //            else
            //            {
            //                ssView_Sheet1.Rows[i].Visible = true;
            //            }
            //        }
            //        else
            //        {
            //            ssView_Sheet1.Rows[i].Visible = false;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 작성 스프래드 세팅
        /// </summary>
        private void SetWriteSpd()
        {
            pClearForm();

            if (mFLOWGB == "ROW") //세로방식(아래로 작성)
            {
                ssView_Sheet1.RowCount = mFLOWITEMCNT + clsEmrNum.FLOWVIWADD;
                ssView_Sheet1.RowHeader.ColumnCount = mFLOWHEADCNT;
                ssView_Sheet1.ColumnCount = 1;
                DesignFunc.SetInitSpd(ssView, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, "V");
                DesignFunc.SetHead(ssView_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead, "V");
            }
            else //가로방식(옆으로 작성)
            {
                ssView_Sheet1.ColumnCount = mFLOWITEMCNT + clsEmrNum.FLOWVIWADD;
                ssView_Sheet1.ColumnHeader.RowCount = mFLOWHEADCNT;
                ssView_Sheet1.RowCount = 1;
                DesignFunc.SetInitSpd(ssView, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, "V");
                DesignFunc.SetHead(ssView_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead, "V");
            }
        }

        /// <summary>
        /// 스프래드에 출력여부 표시하거나 숨기기
        /// </summary>
        private void SetPrintVisible()
        {
            //int i = 0;
            //if (mFLOWGB == "ROW")
            //{
            //    for (i = 1; i < mFLOWHEADCNT; i++)
            //    {
            //        ssView_Sheet1.Columns[i].Visible = true;
            //    }
            //}
            //else
            //{
            //    for (i = 1; i < mFLOWHEADCNT; i++)
            //    {
            //        ssView_Sheet1.Rows[i].Visible = true;
            //    }
            //}
        }

        /// <summary>
        /// 작성내역을 세팅한다
        /// </summary>
        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            //TODO
            //LoadEmrChartInfo();

        }

        /// <summary>
        /// 사용자 템플릿 작성내역을 불러온다
        /// </summary>
        private void pSetEmrInfoTemplet()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            //clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //TODO
            //clsXML.LoadDataUserChartRow(clsDB.DbCon, this, mstrMACRONO, false);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = false;

            return rtnVal;
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void pPrintForm()
        {
            mbtnPrint.Enabled = false;
            SetPrintVisible();

            if (mFLOWGB == "ROW")
            {
                clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Portrait, mFLOWGB, -1, ssView_Sheet1.RowCount - 3, mFLOWHEADCNT, "A");
            }
            else
            {
                clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, ssView_Sheet1.ColumnCount - 3, mFLOWHEADCNT, "A");
            }

            pInitFormSpc();

            mbtnPrint.Enabled = true;
        }

        #endregion //Private Function

        #region //차트조회

        private void QuerySpdList()
        {
            if (string.IsNullOrEmpty(mstrPrtSeq))
            {
                clsEmrQuery.QuerySpdList(clsDB.DbCon, pAcp, mstrFormNo, mstrUpdateNo, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                         ssView,
                        mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, chkDesc.Checked);
            }
            else
            {
                clsEmrQuery.QuerySpdList2(clsDB.DbCon, pAcp, mstrFormNo, mstrEmrNo, mstrUpdateNo, mstrPrtSeq,
                                        ssView,
                                        mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, chkDesc.Checked);

            }
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            QuerySpdList();
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            pPrintForm();
        }
        #endregion //차트조회

    }
}
