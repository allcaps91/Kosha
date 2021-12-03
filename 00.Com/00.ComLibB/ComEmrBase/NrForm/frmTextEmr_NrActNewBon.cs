using ComBase;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    /// <summary>
    /// 신생아간호활동기록지
    /// FORMNO = 1769
    /// \mtsEmrf\frmTextEmr_NrActNewBon.frm
    /// </summary>
    public partial class frmTextEmr_NrActNewBon : Form, EmrChartForm
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

        int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,

        //bool mHeadVisible = true;   //해드를 보이게 할지 여부
        string mFLOWGB = "COL"; //서식작성 방향
        int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;

        //bool mLoading = false;

        /// <summary>
        /// 차트작성 일자.
        /// </summary>
        string mstrOrderDate = string.Empty;

          /// <summary>
        /// VB => txtEmrUseId => 작성자 아이디
        /// </summary>
        string mEmrUseId = string.Empty;

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
        //EmrForm pForm = null;

        public string mstrFormNo = "1769";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";

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

            //usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
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
            pSaveData();
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            if (pDelData())
            {
                ClearSp();
                GetChartHistory();
            }
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

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            if (mstrMode.Equals("W") == false)
                return;

            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            if (pAcp.medEndDate != "")
            {
                if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(pAcp.medEndDate) < VB.Val(strdtpDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(pAcp.medEndDate, "yyyyMMdd", null);
                }
            }
            else
            {
                if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(strCurDate) < VB.Val(strdtpDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(strCurDate, "yyyyMMdd", null);
                }
            }
        }

        #endregion

        #region //EmrChartForm        
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        /// <summary>
        /// 권한별 환경을 설정을 한다(작성화면 안보이게 등)
        /// </summary>
        private void SetUserAut()
        {
            if (clsType.User.AuAWRITE == "1")
            {
                panTopMenu.Visible = true;
                //panWrite.Visible = true;
            }
            else
            {
                panTopMenu.Visible = false;
                //panWrite.Visible = false;
            }

            ssUserChartIO.Visible = clsType.User.AuAMANAGE.Equals("1") || clsType.User.AuAWRITE.Equals("1");
        }

        /// <summary>
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetTopMenuLoad();
        }


        #endregion

        #region //Private Function 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            mstrEmrNo = "0";
            ClearForm();
            ClearSp();
            ClearChart();
        }

        // <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;

            if (pAcp.ptNo.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return VB.Val(mstrEmrNo);
            }

            if (usFormTopMenuEvent.txtMedFrTime.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                return VB.Val(mstrEmrNo);
            }

            #region 작성일자 팝업 알림창
            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string MsgDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            if (pAcp.inOutCls == "O")
            {
                if (pAcp.medFrDate != usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") && pAcp.medDeptCd.Equals("ER") == false)
                {
                    if (ComFunc.MsgBoxQEx(this, "작성일자가 외래 진료일이 아닙니다 계속 작성하시겠습니까?") == DialogResult.No)
                    {
                        return VB.Val(mstrEmrNo);
                    }
                }
            }
            else
            {
                if (pAcp.medEndDate != "")
                {
                    if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(pAcp.medEndDate) < VB.Val(strdtpDate)))
                    {
                        if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이 날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                        {
                            return VB.Val(mstrEmrNo);
                        }
                    }
                }
                else
                {
                    if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(strCurDate) < VB.Val(strdtpDate)))
                    {
                        if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이 날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                        {
                            return VB.Val(mstrEmrNo);
                        }
                    }
                }
            }
            #endregion

            string strDate = string.Format("{0} {1}:{2}",
                usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString(),
                VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2),
                VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2));

            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return VB.Val(mstrEmrNo);
            }

            if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (SaveIO() == true)
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");
                ClearSp();
                btnSearch.PerformClick();
            }

            return dblEmrNo;
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = DeleteProgress(mEmrUseId);

            return rtnVal;
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void pPrintForm()
        {
            clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                                         ssUserChartHisIO, "V", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, 20, mFLOWHEADCNT, "V");
        }


        #endregion //기록지 클리어, 저장, 삭제, 프린터

        #region //Public Function 외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            return pSaveEmrData();
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool DelDataMsg()
        {
            return pDelData();
        }

        /// <summary>
        /// 클리어
        /// </summary>
        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
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

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{ 
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
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
        public frmTextEmr_NrActNewBon()
        {
            InitializeComponent();
        }

        public frmTextEmr_NrActNewBon(string pMake, int pItemCnt, int pHeadCnt, FormFlowSheet[] pFormFlowSheet, FormFlowSheetHead[,] pFormFlowSheetHead)
        {
            InitializeComponent();
            mFLOWGB = pMake;
            mFLOWITEMCNT = pItemCnt;
            mFLOWHEADCNT = pHeadCnt;
            mFormFlowSheet = pFormFlowSheet;
            mFormFlowSheetHead = pFormFlowSheetHead;

            pInitFormTemplet();
        }

        public frmTextEmr_NrActNewBon(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;

            pInitFormTemplet();
        }


        /// <summary>
        /// 19-07-05 신규 생성자 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strOrderDate"></param>
        /// <param name="pEmrCallForm"></param>
        public frmTextEmr_NrActNewBon(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrOrderDate = strOrderDate;
            InitializeComponent();

            pInitFormTemplet();
        }

        public frmTextEmr_NrActNewBon(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();

            pInitFormTemplet();
        }

       #endregion //생성자

        #region 스프레드 enum
        public enum Order
        {
            /// <summary>
            /// 처방일
            /// </summary>
            Bdate,
            /// <summary>
            /// 처방코드
            /// </summary>
            OrderCode,
            /// <summary>
            /// 처방명
            /// </summary>
            OrderName,
            /// <summary>
            /// 일용량
            /// </summary>
            Content,
            /// <summary>
            /// 일투량
            /// </summary>
            RealQty,
            /// <summary>
            /// 용법/검체
            /// </summary>
            DosName,
            /// <summary>
            /// Mix
            /// </summary>
            GbGroup,
            /// <summary>
            /// 횟수
            /// </summary>
            GbDiv,
            /// <summary>
            /// 일수
            /// </summary>
            Nal,
            /// <summary>
            /// Sign
            /// </summary>
            Sign
        }
        #endregion

       
        private void frmTextEmr_NrActNewBon_Load(object sender, EventArgs e)
        {
            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            usFormTopMenuEvent.mbtnClear.Visible = true;
            usFormTopMenuEvent.mbtnPrint.Visible = true;
            usFormTopMenuEvent.mbtnSave.Visible = true;
            usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            if (mEmrCallForm != null && (((Form)mEmrCallForm).Name.Equals("frmEmrLibViewerNr") == false))
            {
                dtpOptSDate.Value = DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null);
                dtpOptEDate.Value = Convert.ToDateTime(pAcp.medEndDate.Length == 0 ? ComQuery.CurrentDateTime(clsDB.DbCon, "S") : ComFunc.FormatStrToDate(pAcp.medEndDate, "D"));
            }
            else
            {
                dtpOptSDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3);
                dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            }

            ClearForm();

            //btnPrint.Visible = false;

            if (mstrOrderDate.Length > 0 || mEmrCallForm == null)
                return;

            ClearChart();

            if (pAcp.ptNo.Length > 0)
            {
                GetChartHistory();
                if (ssUserChartHisIO_Sheet1.ColumnCount >= 4)
                {
                    GetLastValue(2);
                }
            }
        }

        void ClearForm()
        {
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            btnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");
            usFormTopMenuEvent.mbtnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");

            mstrEmrNo = "0";
            mEmrUseId = "";

            for(int i = 3; i < ssUserChartIO_Sheet1.RowCount; i++)
            {
                for(int j = 2; j < ssUserChartIO_Sheet1.ColumnCount; j++)
                {
                    if(j == 2)
                    {
                        switch(i)
                        {
                            case 4:
                                ssUserChartIO_Sheet1.Cells[i, j].Text = "배꼽소독";
                                break;
                            case 5:
                                ssUserChartIO_Sheet1.Cells[i, j].Text = "Size Bath";
                                break;
                            case 6:
                                ssUserChartIO_Sheet1.Cells[i, j].Text = "회음간호";
                                break;
                            case 7:
                                ssUserChartIO_Sheet1.Cells[i, j].Text = "린넨교환";
                                break;
                            default:
                                ssUserChartIO_Sheet1.Cells[i, j].Text = "";
                                break;
                        }
                    }
                    else
                    {
                        ssUserChartIO_Sheet1.Cells[i, j].Text = string.Empty;
                    }
                }
            }

            
                                                            
        }                                                   
                                                            
 
        bool SaveIO()
        {
            bool rtnVal = false;
            try
            {
                if(pAcp.inOutCls == "" || pAcp.ptNo == "" || pAcp.medDeptCd == "" ||
                   pAcp.medDrCd  == "" || pAcp.medFrDate == "")
                {
                    ComFunc.MsgBoxEx(this, "환자 정보가 정확하지 않습니다." + ComNum.VBLF + "확인 후 다시 시도 하십시오.");
                    return rtnVal;
                }

                if(VB.Val(mstrEmrNo) != 0)
                {
                    if(ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?") == DialogResult.No)
                    {
                        return rtnVal;
                    }

                    if(clsType.User.IdNumber != mEmrUseId)
                    {
                        ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                        return rtnVal;
                    }
                }

                string strHead = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
                string strChartX1 = "<chart>";
                string strChartX2 = "</chart>";

                string strCONTENTS = "(SELECT CONTENTS FROM ADMIN.EMRFORM WHERE FORMNO = " + VB.Val(mstrFormNo) + ")";
                string strUPDATENO = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(mstrFormNo)).ToString();

                Cursor.Current = Cursors.WaitCursor;

                if(VB.Val(mstrEmrNo) != 0)
                {
                    if(clsEmrQuery.DeleteEmrXmlData(mstrEmrNo) == false)
                    {
                        return rtnVal;
                    }
                }


                double dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");
                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
                string strChartTime = usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "").Trim();
                string strInOutCls = pAcp.inOutCls;

                string strXML = strHead + strChartX1;
                strXML += MkValue() + strChartX2;

                clsEmrQuery.SaveEmrXmlData(dblEmrNo.ToString(), mstrFormNo, strChartDate, strChartTime,
                               pAcp.acpNo, pAcp.ptNo, strInOutCls, pAcp.medFrDate, pAcp.medFrTime,
                               pAcp.medEndDate, pAcp.medEndTime, pAcp.medDeptCd, pAcp.medDrCd,
                               strXML, strUPDATENO);

                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void ClearChart()
        {
            MakeUserChart(mstrFormNo, 14);
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)).ToString("00:00");
        }

        void MakeUserChart(string strFormNo, int lngStartCol)
        {
            ssUserChart_Sheet1.ColumnCount = lngStartCol;
            ssUserChartHis_Sheet1.ColumnCount = lngStartCol + 2;
            ssUserChartHis_Sheet1.RowCount = 0;
            ssUserChartHis_Sheet1.RowCount = 1;
            ssFORMXML_Sheet1.ColumnCount = lngStartCol;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;

            CheckBoxCellType TypeCheckBox = new CheckBoxCellType();
            TextCellType TypeText = new TextCellType();
            //ComboBoxCellType TypeComBo = new ComboBoxCellType();

            SQL = SQL + ComNum.VBLF + "  SELECT A.FORMNO, A.FORMNAME1 FORMNAME,  ";
            SQL = SQL + ComNum.VBLF + "      B.ITEMNO, B.ITEMNAME, B.ITEMTYPE, B.ITEMHALIGN, B.ITEMVALIGN,";
            SQL = SQL + ComNum.VBLF + "      B.ITEMHEIGHT, B.ITEMWIDTH, B.MULTILINE, B.USEMACRO, B.CONTROLID, B.ITEMRMK, B.TAGHEAD, B.TAGTAIL";
            SQL = SQL + ComNum.VBLF + "      FROM ADMIN.EMRFORM A INNER JOIN ADMIN.EMROPTFORM B";
            SQL = SQL + ComNum.VBLF + "         ON A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "      WHERE A.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "      ORDER BY B.ITEMNO";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            try
            {
                ssUserChart_Sheet1.ColumnCount = lngStartCol + dt.Rows.Count;

                ssUserChartHis_Sheet1.ColumnCount = lngStartCol + 2 + dt.Rows.Count + 1; //사용자 추가
                ssUserChartHis_Sheet1.Columns[ssUserChartHis_Sheet1.ColumnCount - 1].Label = "작성자";

                ssFORMXML_Sheet1.ColumnCount = lngStartCol + dt.Rows.Count;
                ssFORMXML_Sheet1.RowCount = 2;

                ssItem_Sheet1.ColumnCount = lngStartCol + dt.Rows.Count;
                ssItem_Sheet1.RowCount = 1;

                //lngStartCol -= 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssUserChart_Sheet1.Columns[lngStartCol].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChart_Sheet1.Columns[lngStartCol].Width = (int)VB.Val(dt.Rows[i]["ITEMWIDTH"].ToString().Trim()) + 50;

                    if (ssUserChart_Sheet1.RowCount == 0)
                        continue;


                    switch (dt.Rows[i]["ITEMTYPE"].ToString().Trim())
                    {
                        case "EDIT":
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim() == "1" ? true : false;
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].CellType = TypeText;
                            break;
                        case "COMBO":
                            clsSpread.gSpreadComboDataSetEx(ssUserChart, 0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i,
                                dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'));
                            break;
                        case "CHECK":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim())
                    {
                        case "LEFT":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim())
                    {
                        case "TOP":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    if (dt.Rows[i]["USEMACRO"].ToString().Trim() == "1" && dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0)
                    {
                        ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                    }

                    ssFORMXML_Sheet1.Cells[0, lngStartCol + i].Text = dt.Rows[i]["TAGHEAD"].ToString().Trim();
                    ssFORMXML_Sheet1.Cells[1, lngStartCol + i].Text = dt.Rows[i]["TAGTAIL"].ToString().Trim();

                    ssItem_Sheet1.Columns[lngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssItem_Sheet1.Cells[0, lngStartCol + i].Text = dt.Rows[i]["CONTROLID"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        void MakeUserChartOne(string strFormNo, int lngStartCol)
        {
            ssFORMXML_Sheet1.ColumnCount = lngStartCol;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;
            
            CheckBoxCellType TypeCheckBox = new CheckBoxCellType();
            TextCellType TypeText = new TextCellType();
            ComboBoxCellType TypeComBo = new ComboBoxCellType();

            SQL = SQL + ComNum.VBLF + "  SELECT A.FORMNO, A.FORMNAME1 FORMNAME,  ";
            SQL = SQL + ComNum.VBLF + "      B.ITEMNO, B.ITEMNAME, B.ITEMTYPE, B.ITEMHALIGN, B.ITEMVALIGN,";
            SQL = SQL + ComNum.VBLF + "      B.ITEMHEIGHT, B.ITEMWIDTH, B.MULTILINE, B.USEMACRO, B.CONTROLID, B.ITEMRMK, B.TAGHEAD, B.TAGTAIL";
            SQL = SQL + ComNum.VBLF + "      FROM ADMIN.EMRFORM A INNER JOIN ADMIN.EMROPTFORM B";
            SQL = SQL + ComNum.VBLF + "         ON A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "      WHERE A.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "      ORDER BY B.ITEMNO";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if(sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if(dt.Rows.Count == 0)
            {
                return;
            }

            try
            {
                ssUserChart_Sheet1.ColumnCount = lngStartCol + dt.Rows.Count;
                ssFORMXML_Sheet1.ColumnCount = lngStartCol + dt.Rows.Count;
                ssFORMXML_Sheet1.RowCount = 2;

                ssItem_Sheet1.ColumnCount = lngStartCol + dt.Rows.Count;
                ssItem_Sheet1.RowCount = 1;

                //lngStartCol -= 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssUserChart_Sheet1.Columns[lngStartCol].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChart_Sheet1.Columns[lngStartCol].Width = (int) VB.Val(dt.Rows[i]["ITEMWIDTH"].ToString().Trim()) + 50;

                    if (ssUserChart_Sheet1.RowCount == 0)
                        continue;


                    switch(dt.Rows[i]["ITEMTYPE"].ToString().Trim())
                    {
                        case "EDIT":
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim() == "1" ? true : false;
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].CellType = TypeText;
                            break;
                        case "COMBO":
                            clsSpread.gSpreadComboDataSetEx(ssUserChart, 0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i,
                                dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'));
                            break;
                        case "CHECK":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim())
                    {
                        case "LEFT":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim())
                    {
                        case "TOP":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    if(dt.Rows[i]["USEMACRO"].ToString().Trim() == "1" && dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0 )
                    {
                        ssUserChart_Sheet1.Cells[0, lngStartCol + i, ssUserChart_Sheet1.RowCount - 1, lngStartCol + i].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                    }

                    ssFORMXML_Sheet1.Cells[0, lngStartCol + i].Text = dt.Rows[i]["TAGHEAD"].ToString().Trim();
                    ssFORMXML_Sheet1.Cells[1, lngStartCol + i].Text = dt.Rows[i]["TAGTAIL"].ToString().Trim();

                    ssItem_Sheet1.Columns[lngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssItem_Sheet1.Cells[0, lngStartCol + i].Text = dt.Rows[i]["CONTROLID"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex )
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }


        /// <summary>
        /// VB MkHaT
        /// </summary>
        /// <param name="xCol"></param>
        /// <param name="strV"></param>
        /// <returns></returns>
        string MkHaT(int xCol, string strV)
        {
            strV = ssFORMXML_Sheet1.Cells[0, xCol].Text.Trim() + strV;
            strV += ssFORMXML_Sheet1.Cells[1, xCol].Text.Trim();

            return strV;
        }

        /// <summary>
        /// VB MkValue
        /// </summary>
        /// <returns></returns>
        string MkValue()
        {
            string strVal = string.Empty;
            strVal += MkHaT(14, ssUserChartIO_Sheet1.Cells[1, 2].Text.Trim());
            int StartCol = 15;
            for(int i = 3; i < 38; i++)
            {
                strVal += MkHaT(StartCol, ssUserChartIO_Sheet1.Cells[i, 2].Text.Trim());
                strVal += MkHaT(StartCol + 1, ssUserChartIO_Sheet1.Cells[i, 3].Text.Trim());
                StartCol += 2;
            }

            return strVal;
        }


        string SET_GRADE()
        {
            string rtnVal = string.Empty;
            switch (clsType.User.BuseCode)
            {
                case "078200":
                case "078201":
                case "077502":
                case "077405":
                    rtnVal = "SIMSA";
                    break;
                case "044201":
                case "044200":
                    rtnVal = "WRITE";
                    break;
            }

            if (clsType.User.Sabun == "14472")
            {
                rtnVal= "SIMSA";
            }

            return rtnVal;
        }

        string ReadOptionDate(string argGrade, string ArgIO, string ArgDeptCode, string argUSEID, string argDATE)
        {
            string rtnVal = string.Empty;

            switch (argUSEID)
            {

                case "45924":
                case "22536":
                case "36522":
                case "44892":
                    rtnVal = VB.Left(pAcp.medFrDate, 4) + "-" +
                        VB.Mid(pAcp.medFrDate, 5, 2) + "-" +
                        VB.Right(pAcp.medFrDate, 2);
                return rtnVal;
            }

            if (argGrade != "SIMSA")
            {
                return argDATE;
            }

            OracleDataReader reader = null;

            try
            {
                string SQL = " SELECT NAL ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMR_OPTION_SETDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE IO = '" + ArgIO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND USEID = " + argUSEID;
                if (ArgIO != "I")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND USED = '1' ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal; 
                }

                if(reader.HasRows && reader.Read())
                {
                    if(reader.GetValue(0).ToString().Trim().Length == 0 ||
                       VB.IsNumeric(reader.GetValue(0).ToString().Trim()) == false)
                    {
                        rtnVal = argDATE;
                        reader.Dispose();
                        return rtnVal;
                    }

                    rtnVal = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).
                        AddDays(VB.Val(reader.GetValue(0).ToString().Trim()) * -1).ToShortDateString();
                }

                reader.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        bool DeleteProgress(string strUseId)
        {
            bool rtnVal = false;
            double dEmrNo = VB.Val(mstrEmrNo);

            if (dEmrNo == 0)
                return rtnVal;

            if (dEmrNo != 0)
            {
                if (strUseId.Trim() != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    return rtnVal;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                    return rtnVal;

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                    return rtnVal;

                if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?") == DialogResult.No)
                {
                    return rtnVal;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            rtnVal = clsEmrQuery.DeleteEmrXmlData(mstrEmrNo);
            Cursor.Current = Cursors.Default;

            return rtnVal;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetChartHistory();
        }

        void GetChartHistory()
        {
            ssUserChartHisIO_Sheet1.ColumnCount = 2;

            if (pAcp.ptNo == "")
                return;

            if (mstrFormNo.Trim() == "")
                return;


            #region 권한 버튼
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //초기값 세팅
            if (VB.Val(mstrEmrNo) <= 0)
            {
                usFormTopMenuEvent.mbtnPrint.Visible = false;
                usFormTopMenuEvent.mbtnPrintNull.Visible = true;
            }
            else if (VB.Val(mstrEmrNo) > 0)
            {
                if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                {
                    usFormTopMenuEvent.mbtnSave.Visible = false;
                    usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                    usFormTopMenuEvent.mbtnDelete.Visible = false;
                }
            }

            //사본발급 출력여부
            usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(mstrEmrNo);
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
            #endregion

            GetUchartHis(pAcp.ptNo, mstrFormNo, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                ssUserChartHis);

            if(ssUserChartHis_Sheet1.RowCount > 0)
            {
                ConVIoData();
            }

            DesignFunc.DefaultDuty(ssUserChartIO, 2);
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <param name="strPtno">등록번호</param>
        /// <param name="strFormNo">폼번호</param>
        /// <param name="strSDATE">시작일자</param>
        /// <param name="strEDATE">종료일자</param>
        /// <param name="objSpread">스프레드</param>
        void GetUchartHis(string strPtno, string strFormNo,string strSDATE,string strEDATE,FarPoint.Win.Spread.FpSpread objSpread)
        {
            if (mstrFormNo == "")
                return;

            if (pAcp.ptNo == "")
                return;

            objSpread.ActiveSheet.RowCount = 0;

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = "SELECT A.EMRNO,";
                SQL+= ComNum.VBLF + "    A.ACPNO, A.INOUTCLS, A.CHARTDATE, A.CHARTTIME, ";
                SQL+= ComNum.VBLF + "    A.MEDDEPTCD, A.MEDDRCD, A.USEID,U.NAME USENAME,";
                SQL+= ComNum.VBLF + "    A.MEDFRDATE, A.MEDFRTIME,  B.FORMNO, B.FORMNAME1 FORMNAME,  B.USERFORMNO";
                SQL+= ComNum.VBLF + "FROM ADMIN.EMRXML A, ADMIN.EMRFORM B, ADMIN.EMRXMLMST D, ADMIN.EMR_USERT U ";
                SQL+= ComNum.VBLF + "  WHERE D.FORMNO = " + strFormNo;
                SQL+= ComNum.VBLF + "      AND A.FORMNO = B.FORMNO ";
                SQL+= ComNum.VBLF + "      AND A.USEID = U.USERID ";
                SQL+= ComNum.VBLF + "      AND D.PTNO = '" + strPtno + "' ";
                SQL+= ComNum.VBLF + "      AND D.CHARTDATE >= '" + strSDATE + "' ";
                SQL+= ComNum.VBLF + "      AND D.CHARTDATE <= '" + strEDATE + "' ";
                SQL+= ComNum.VBLF + "      AND A.EMRNO = D.EMRNO ";
                SQL+= ComNum.VBLF + "  ORDER BY A.CHARTDATE DESC,  TRIM(A.CHARTTIME) DESC, A.EMRNO ASC";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                objSpread.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSpread.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["USEID"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 13].Text = "";
                    objSpread.ActiveSheet.Cells[i, 14].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                    objSpread.ActiveSheet.Cells[i, 15].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");

                    #region XML세팅
                    XmlDocument xml = new XmlDocument(); // XmlDocument 생성
                    xml.LoadXml(LoadXml(dt.Rows[i]["EMRNO"].ToString().Trim()));
                    #endregion

                    #region SetUserXmlValue
                    if(xml.DocumentElement != null)
                    {
                        foreach (XmlNode xn in xml.DocumentElement.ChildNodes)
                        {
                            SetUserXmlValue(xn, objSpread, i);
                        }
                    }
                    #endregion
                    objSpread.ActiveSheet.Cells[i, objSpread.ActiveSheet.ColumnCount - 1].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// XML 불러오는 함수
        /// </summary>
        /// <param name="strEmrNo">EMRNO</param>
        /// <returns></returns>
        string LoadXml(string strEmrNo)
        {
            OracleDataReader reader = null;
            string strXml = string.Empty;

            try
            {
                string SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + "SELECT CHARTXML";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + VB.Val(strEmrNo);

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strXml;
                }

                if (reader.HasRows  && reader.Read())
                {
                    strXml = reader.GetValue(0).ToString().Trim();
                    return strXml;
                }

                return strXml;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return strXml;
            }
        }

        /// <summary>
        /// XML 해당 아이템 찾아서 값 넣어주는 함수
        /// 
        /// </summary>
        /// <param name="a_objElement">XMLNODE </param>
        /// <param name="objSpread">스프레드 객체</param>
        /// <param name="lngRowX">로우</param>
        void SetUserXmlValue(XmlNode a_objElement, FarPoint.Win.Spread.FpSpread objSpread, int lngRowX)
        {
            string strItem = a_objElement.Name;
            string strValue = a_objElement.InnerText;
            int lngItemCol = 0;
            TextCellType textCellType = new TextCellType();

            for (int lngCol = 0; lngCol < ssItem_Sheet1.ColumnCount; lngCol++)
            {
                if(strItem == ssItem_Sheet1.Cells[0, lngCol].Text.Trim())
                {
                    lngItemCol = lngCol;
                    break;
                }
            }

            if (lngItemCol == 0)
                return;

            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].HorizontalAlignment = ssUserChart.ActiveSheet.Cells[0, lngItemCol].HorizontalAlignment;
            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].VerticalAlignment   = ssUserChart.ActiveSheet.Cells[0, lngItemCol].VerticalAlignment;
            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].CellType = ssUserChart.ActiveSheet.Cells[0, lngItemCol].CellType;

            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].Text = strValue;
            
            if(objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].CellType != null &&
               objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].CellType.ToString() == "TextCellType")
            {
                textCellType = (TextCellType) objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].CellType;
                objSpread.ActiveSheet.Rows[lngRowX].Height = textCellType.Multiline == true ? objSpread.ActiveSheet.Rows[lngRowX].GetPreferredHeight() + 10 : 22;
            }

            textCellType.Dispose();

        }

        string READ_DETAIL_FALL(string argPTNO, string argDATE, string ArgIPDNO, string ArgAge)
        {
            try
            {
                string strFall = string.Empty;
                string strBraden = string.Empty;

                string strTOTAL = string.Empty;
                string strExam  = string.Empty;
                string strCAUSE = string.Empty;
                string strDrug  = string.Empty;

                string strWARD_C = string.Empty;
                string strAGE_C  = string.Empty;

                string SQL = string.Empty;

                DataTable dt = null;

                SQL = " SELECT WARDCODE, AGE ";
                SQL += ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0 )
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return string.Empty;
                }

                if(dt.Rows.Count > 0)
                {
                    switch(dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                            strFall = "OK";
                            strWARD_C = "중환자실 재원 환자";
                            break;

                        case "NR":
                        case "IQ":
                            strFall = "OK";
                            strWARD_C = "신생아실 재원 환자";
                            break;
                    }

                    double dAge = VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                    if(dAge >= 70)
                    {
                        strFall = "OK";
                        strAGE_C = "70세 이상 환자";
                    }

                    if(dAge < 7)
                    {
                        strFall = "OK";
                        strAGE_C = "7세 미만 환자";
                    }
                }

                dt.Dispose();

                SQL = "  SELECT PANO, TOTAL ";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLMORSE_SCALE";
                SQL += ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL += ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + " AND TOTAL >= 51";
                SQL += ComNum.VBLF + "     AND ROWID = (";
                SQL += ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL += ComNum.VBLF + "  SELECT * FROM ADMIN.NUR_FALLMORSE_SCALE";
                SQL += ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL += ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL += ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";


                if(VB.Val(ArgAge) < 18)
                {
                    SQL = "  SELECT PANO, TOTAL ";
                    SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLHUMPDUMP_SCALE";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                    SQL += ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                    SQL += ComNum.VBLF + " AND (TOTAL >= 12 OR AGE < 7)";
                    SQL += ComNum.VBLF + "     AND ROWID = (";
                    SQL += ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL += ComNum.VBLF + "  SELECT * FROM ADMIN.NUR_FALLHUMPDUMP_SCALE";
                    SQL += ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL += ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL += ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL += ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                }

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return string.Empty;
                }

                if(dt.Rows.Count> 0)
                {
                    strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                    strFall = "OK";                   
                }

                dt.Dispose();

                StringBuilder strTemp = new StringBuilder();

                SQL = " SELECT * "                                            ;
                SQL += ComNum.VBLF + "  FROM ADMIN.NUR_FALL_WARNING"      ;
                SQL += ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO               ;
                SQL += ComNum.VBLF + "   AND (WARNING1 = '1'"                   ;
                SQL += ComNum.VBLF + "                  OR WARNING2 = '1'"      ;
                SQL += ComNum.VBLF + "                  OR WARNING3 = '1'"      ;
                SQL += ComNum.VBLF + "                  OR WARNING4 = '1'"      ;
                SQL += ComNum.VBLF + "                  OR DRUG_01 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_02 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_03 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_04 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_05 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_06 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_07 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_08 = '1'"       ;
                SQL += ComNum.VBLF + "                  OR DRUG_08_ETC <> '')";

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";

                    strCAUSE = "";

                    if (strAGE_C.Length == 0 && dt.Rows[0]["WARNING1"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 70세이상 ");
                    }

                    if (dt.Rows[0]["WARNING2"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 보행장애 ");
                    }

                    if (dt.Rows[0]["WARNING3"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 혼미 ");
                    }

                    if (dt.Rows[0]["WARNING4"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 어지럼증 ");
                    }

                    strCAUSE = strTemp.ToString().Trim();

                    strTemp.Clear();
                    strDrug = "";

                    if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 진정제 ") ;
                    }
                    if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 수면제 ") ;
                    }
                    if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 향정신성약물 ") ;
                    }
                    if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 항우울제 ") ;
                    }
                    if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 완하제 ") ;
                    }
                    if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 이뇨제 ") ;
                    }
                    if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 진정약물 ") ;
                    }
                    if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ " + dt.Rows[0]["DRUG_08_ETC"].ToString().Trim());
                    }

                    strDrug = strTemp.ToString().Trim();

                }

                dt.Dispose();


                if (strFall == "OK")
                {
                    strTemp.Clear();
                    if(strTOTAL.Length > 0)
                    {
                        strTemp.AppendLine(" => 낙상점수 : " + VB.Val(strTOTAL) + "점 ");
                    }


                    if (strWARD_C.Length > 0)
                    {
                        strTemp.AppendLine(strWARD_C);
                    }


                    if (strAGE_C.Length > 0)
                    {
                        strTemp.AppendLine(strAGE_C);
                    }


                    if (strCAUSE.Length > 0)
                    {
                        strTemp.AppendLine(strCAUSE);
                    }


                    if (strDrug.Length > 0)
                    {
                        strTemp.AppendLine("-위험약물");
                        strTemp.AppendLine(strDrug);
                    }
                }

                return strTemp.ToString().Trim();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return string.Empty;
            }
        }

        string READ_DETAIL_EVAL_FALL_NEW(string strIpdNo)
        {
            string rtnVal = string.Empty;

            try
            {
                string strDrug = string.Empty;
                string strDrug2 = string.Empty;

                string strCAUSE = string.Empty;

                string SQL = string.Empty;
                StringBuilder strTemp = new StringBuilder();

                DataTable dt = null;

                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALL_EVAL ";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + strIpdNo;

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return string.Empty;
                }

                if (dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                    {
                        if(strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "진정제 ";
                    }

                    if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "수면제 ";
                    }

                    if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "향정신성 약물 ";
                    }

                    if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "항우울제 ";
                    }

                    if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                    { 
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "완하제 ";
                    }

                    if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "이뇨제 ";
                    }

                    if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "진정약품 ";

                    }

                    if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                    {
                        if (strDrug.Length == 0)
                        {
                            strDrug = "낙상 초래 약물의 초기 투여 - ";
                        }

                        strDrug2 += "기타 ";
                    }

                    if (dt.Rows[0]["DRUG_08ETC"].ToString().Trim() == "1")
                    {
                        strDrug = strDrug + "(" + dt.Rows[0]["DRUG_08"].ToString().Trim() + ") ";
                    }

                    if (dt.Rows[0]["PAT_CHANGE"].ToString().Trim() == "1")
                    {
                        strTemp.Append("환자상태변화 ");
                    }

                    if (dt.Rows[0]["PAT_CHANGE2"].ToString().Trim() == "1")
                    {
                        strTemp.Append("주요상태변화:간성 혼수, 알콜 섬망, 발작 ");
                    }

                    if (dt.Rows[0]["TRANFOR"].ToString().Trim() == "1")
                    {
                        strTemp.Append("전동 ");
                    }

                    if (dt.Rows[0]["RELEX"].ToString().Trim() == "1")
                    {
                        strTemp.Append("진정 치료(검사) ");
                    }

                    if (dt.Rows[0]["OP"].ToString().Trim() == "1")
                    {
                        strTemp.Append("수술/시술 ");
                    }

                    if (dt.Rows[0]["FALL"].ToString().Trim() == "1")
                    {
                        strTemp.Append("낙상 발생 ");
                    }

                }

                rtnVal = strTemp.ToString().Trim();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// VB => ConVIoData
        /// </summary>
        void ConVIoData()
        {
            for (int xRow1 = 0; xRow1 < ssUserChartHis_Sheet1.RowCount; xRow1++)
            {
                for (int xCol1 = 17; xCol1 < ssUserChartHis_Sheet1.ColumnCount; xCol1++)
                {
                    SetssConV(xCol1, ssUserChartHis_Sheet1.Cells[xRow1, xCol1].Text.Trim());
                }

                //일자
                string strDate = ssUserChartHis_Sheet1.Cells[xRow1, 14].Text.Trim();
                //시간
                string strTime = ssUserChartHis_Sheet1.Cells[xRow1, 15].Text.Trim();
                //Duty
                string strDuty = ssUserChartHis_Sheet1.Cells[xRow1, 16].Text.Trim();
                //EMRNO
                string strEmrNo = ssUserChartHis_Sheet1.Cells[xRow1, 11].Text.Trim();
                //USEID
                string strEmrUseId = ssUserChartHis_Sheet1.Cells[xRow1, 12].Text.Trim();

                //일자
                ssConV_Sheet1.Cells[0, 0].Text = string.Format("{0} / {1}", strDate, strTime);
                ssConV_Sheet1.Cells[0, 1].Text = string.Format("{0} / {1}", strDate, strTime);

                //Duty
                ssConV_Sheet1.Cells[1, 0].Text = strDuty;
                ssConV_Sheet1.Cells[1, 1].Text = strDuty;

                //작성자
                ssConV_Sheet1.Cells[38, 0].Text = string.Format("{0} / {1}", ssUserChartHis_Sheet1.Cells[xRow1, 87].Text.Trim(), strTime);
                ssConV_Sheet1.Cells[38, 1].Text = string.Format("{0} / {1}", ssUserChartHis_Sheet1.Cells[xRow1, 87].Text.Trim(), strTime);

                //EMRNO
                ssConV_Sheet1.Cells[39, 0].Text = ssUserChartHis_Sheet1.Cells[xRow1, 11].Text.Trim();
                ssConV_Sheet1.Cells[39, 1].Text = ssUserChartHis_Sheet1.Cells[xRow1, 11].Text.Trim();

                //USEID
                ssConV_Sheet1.Cells[40, 0].Text = ssUserChartHis_Sheet1.Cells[xRow1, 12].Text.Trim();
                ssConV_Sheet1.Cells[40, 1].Text = ssUserChartHis_Sheet1.Cells[xRow1, 12].Text.Trim();
                SetIoHis();
            }

        }

        void SetssConV(int lngCol, string strValue)
        {
            if (lngCol == 18 - 1)
                ssConV_Sheet1.Cells[4 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 19 - 1)
                ssConV_Sheet1.Cells[4 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 20 - 1)
                ssConV_Sheet1.Cells[5 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 21 - 1)
                ssConV_Sheet1.Cells[5 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 22 - 1)
                ssConV_Sheet1.Cells[6 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 23 - 1)
                ssConV_Sheet1.Cells[6 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 24 - 1)
                ssConV_Sheet1.Cells[7 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 25 - 1)
                ssConV_Sheet1.Cells[7 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 26 - 1)
                ssConV_Sheet1.Cells[8 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 27 - 1)
                ssConV_Sheet1.Cells[8 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 28 - 1)
                ssConV_Sheet1.Cells[9 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 29 - 1)
                ssConV_Sheet1.Cells[9 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 30 - 1)
                ssConV_Sheet1.Cells[10 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 31 - 1)
                ssConV_Sheet1.Cells[10 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 32 - 1)
                ssConV_Sheet1.Cells[11 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 33 - 1)
                ssConV_Sheet1.Cells[11 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 34 - 1)
                ssConV_Sheet1.Cells[12 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 35 - 1)
                ssConV_Sheet1.Cells[12 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 36 - 1)
                ssConV_Sheet1.Cells[13 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 37 - 1)
                ssConV_Sheet1.Cells[13 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 38 - 1)
                ssConV_Sheet1.Cells[14 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 39 - 1)
                ssConV_Sheet1.Cells[14 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 40 - 1)
                ssConV_Sheet1.Cells[15 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 41 - 1)
                ssConV_Sheet1.Cells[15 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 42 - 1)
                ssConV_Sheet1.Cells[16 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 43 - 1)
                ssConV_Sheet1.Cells[16 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 44 - 1)
                ssConV_Sheet1.Cells[17 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 45 - 1)
                ssConV_Sheet1.Cells[17 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 46 - 1)
                ssConV_Sheet1.Cells[18 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 47 - 1)
                ssConV_Sheet1.Cells[18 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 48 - 1)
                ssConV_Sheet1.Cells[19 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 49 - 1)
                ssConV_Sheet1.Cells[19 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 50 - 1)
                ssConV_Sheet1.Cells[20 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 51 - 1)
                ssConV_Sheet1.Cells[20 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 52 - 1)
                ssConV_Sheet1.Cells[21 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 53 - 1)
                ssConV_Sheet1.Cells[21 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 54 - 1)
                ssConV_Sheet1.Cells[22 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 55 - 1)
                ssConV_Sheet1.Cells[22 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 56 - 1)
                ssConV_Sheet1.Cells[23 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 57 - 1)
                ssConV_Sheet1.Cells[23 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 58 - 1)
                ssConV_Sheet1.Cells[24 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 59 - 1)
                ssConV_Sheet1.Cells[24 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 60 - 1)
                ssConV_Sheet1.Cells[25 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 61 - 1)
                ssConV_Sheet1.Cells[25 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 62 - 1)
                ssConV_Sheet1.Cells[26 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 63 - 1)
                ssConV_Sheet1.Cells[26 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 64 - 1)
                ssConV_Sheet1.Cells[27 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 65 - 1)
                ssConV_Sheet1.Cells[27 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 66 - 1)
                ssConV_Sheet1.Cells[28 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 67 - 1)
                ssConV_Sheet1.Cells[28 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 68 - 1)
                ssConV_Sheet1.Cells[29 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 69 - 1)
                ssConV_Sheet1.Cells[29 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 70 - 1)
                ssConV_Sheet1.Cells[30 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 71 - 1)
                ssConV_Sheet1.Cells[30 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 72 - 1)
                ssConV_Sheet1.Cells[31 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 73 - 1)
                ssConV_Sheet1.Cells[31 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 74 - 1)
                ssConV_Sheet1.Cells[32 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 75 - 1)
                ssConV_Sheet1.Cells[32 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 76 - 1)
                ssConV_Sheet1.Cells[33 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 77 - 1)
                ssConV_Sheet1.Cells[33 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 78 - 1)
                ssConV_Sheet1.Cells[34 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 79 - 1)
                ssConV_Sheet1.Cells[34 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 80 - 1)
                ssConV_Sheet1.Cells[35 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 81 - 1)
                ssConV_Sheet1.Cells[35 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 82 - 1)
                ssConV_Sheet1.Cells[36 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 83 - 1)
                ssConV_Sheet1.Cells[36 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 84 - 1)
                ssConV_Sheet1.Cells[37 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 85 - 1)
                ssConV_Sheet1.Cells[37 - 1, 2 - 1].Text = strValue;
            else if (lngCol == 86 - 1)
                ssConV_Sheet1.Cells[38 - 1, 1 - 1].Text = strValue;
            else if (lngCol == 87 - 1)
                ssConV_Sheet1.Cells[38 - 1, 2 - 1].Text = strValue;
        }


        void SetIoHis()
        {
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            int lngMCol1 = ssUserChartHisIO_Sheet1.ColumnCount;
            ssUserChartHisIO_Sheet1.ColumnCount += 2;
            ssUserChartHisIO_Sheet1.Columns[ssUserChartHisIO_Sheet1.ColumnCount - 2, ssUserChartHisIO_Sheet1.ColumnCount - 1].Border = complexBorder1;


            for (int xCol1 = 0; xCol1 < ssConV_Sheet1.ColumnCount; xCol1++)
            {
                for (int xRow1 = 0; xRow1 < ssConV_Sheet1.RowCount; xRow1++)
                {
                    ssUserChartHisIO_Sheet1.Cells[xRow1, xCol1 == 1 ? lngMCol1 + 1 : lngMCol1].Text = ssConV_Sheet1.Cells[xRow1, xCol1].Text.Trim();
                    ssUserChartHisIO_Sheet1.Columns[lngMCol1].Width = ssUserChartHisIO_Sheet1.Columns[lngMCol1].GetPreferredWidth() + 5;

                }
            }

            ssUserChartHisIO_Sheet1.Cells[0, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, ssUserChartHisIO_Sheet1.ColumnCount - 1].CellType = new TextCellType();
            ssUserChartHisIO_Sheet1.Cells[0, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, ssUserChartHisIO_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssUserChartHisIO_Sheet1.Cells[0, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, ssUserChartHisIO_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssUserChartHisIO_Sheet1.Cells[38, ssUserChartHisIO_Sheet1.ColumnCount - 2].CellType = new TextCellType();
            ssUserChartHisIO_Sheet1.Cells[38, ssUserChartHisIO_Sheet1.ColumnCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssUserChartHisIO_Sheet1.Cells[38, ssUserChartHisIO_Sheet1.ColumnCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;


            ssUserChartHisIO_Sheet1.AddSpanCell(0, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, 2); //날짜
            ssUserChartHisIO_Sheet1.AddSpanCell(1, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, 2); //Duty
            ssUserChartHisIO_Sheet1.AddSpanCell(38, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, 2); //Sign

            ssUserChartHisIO_Sheet1.Columns[ssUserChartHisIO_Sheet1.ColumnCount - 1].Width = ssUserChartHisIO_Sheet1.Columns[ssUserChartHisIO_Sheet1.ColumnCount - 1].GetPreferredWidth() + 5;
        }


        string GetDrNm(string strDrCode, string strBdate)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;

            try
            {
                string SQL = "SELECT DRNAME FROM ADMIN.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "          WHERE DRCODE = '" + strDrCode + "'";
                SQL = SQL + ComNum.VBLF + "          AND ((GBOUT <> 'Y')";
                SQL = SQL + ComNum.VBLF + "              OR (GBOUT = 'Y' AND ";
                SQL = SQL + ComNum.VBLF + "                      (FDATE >= TO_DATE('" + strBdate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "                      AND (TDATE <= TO_DATE('" + strBdate + "','YYYY-MM-DD'))))";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if(reader.HasRows &&
                   reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PrintCopy("");
        }

        void PrintCopy(string PrintType)
        {
            DataTable dt = null;
            OracleDataReader reader = null;

            string strPatName = string.Empty;
            string strSex = string.Empty;
            string strAge = string.Empty;
            string strJumin = string.Empty;

            try
            {
                string SQL = " SELECT SNAME, SEX, JUMIN1, JUMIN2 ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    strPatName = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSex     = dt.Rows[0]["SEX"].ToString().Trim();
                    strAge     = ComFunc.AgeCalc(clsDB.DbCon,
                        dt.Rows[0]["JUMIN1"].ToString().Trim() +
                        dt.Rows[0]["JUMIN2"].ToString().Trim()).ToString();
                     strJumin = string.Format("{0}-{1}******",
                        dt.Rows[0]["JUMIN1"].ToString().Trim(),
                        VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1));

                }

                dt.Dispose();

                string strFormName = string.Empty;
                SQL = " SELECT FORMNAME ";
                SQL = SQL += ComNum.VBLF + " FROM ADMIN.AEMRFORM ";
                SQL = SQL += ComNum.VBLF + "WHERE FORMNO = " + VB.Val(mstrFormNo);

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strFormName = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

                //'Print Head 지정
                string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
                string strFont2 = @"/fn""바탕체"" /fz""8"" /fb0 /fi0 /fu0 /fk0 /fs2";
                string strHead1 = "/c/f1" + strFormName + "/f1/n/n";
                string strHead2 = "/n/l/f2" + "환자번호 : " + pAcp.ptNo +
                                  VB.Space(5) + "환자성명 : " + strPatName + VB.Space(5) + "성별/나이 : " + strSex + "/" + strAge + "세              주민번호 :  " + strJumin + "/n/n";
                string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(10) + "출력일자 : " +  ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) + VB.Space(10) + "출력자 : " + clsType.User.UserName;
                strFooter = strFooter + "/n/l/f2" + "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.";
                strFooter = strFooter + "/n/l/f2" + "This is an electronically authorized offical medical record";

                ssUserChartHisIO_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssUserChartHisIO_Sheet1.PrintInfo.Footer = strFooter;
                ssUserChartHisIO_Sheet1.PrintInfo.Margin.Left = 20;
                ssUserChartHisIO_Sheet1.PrintInfo.Margin.Right = 20;
                ssUserChartHisIO_Sheet1.PrintInfo.Margin.Top = 20;
                ssUserChartHisIO_Sheet1.PrintInfo.Margin.Bottom = 20;

                ssUserChartHisIO_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssUserChartHisIO_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssUserChartHisIO_Sheet1.PrintInfo.ShowBorder = true;
                ssUserChartHisIO_Sheet1.PrintInfo.ShowColor = false;
                ssUserChartHisIO_Sheet1.PrintInfo.ShowGrid = true;
                ssUserChartHisIO_Sheet1.PrintInfo.ShowShadows = false;
                ssUserChartHisIO_Sheet1.PrintInfo.UseMax = false;
                ssUserChartHisIO_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssUserChartHisIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;

                SQL = " SELECT PRTGB ";
                SQL = SQL += ComNum.VBLF + " FROM ADMIN.EMRFORM ";
                SQL = SQL += ComNum.VBLF + "WHERE FORMNO = " + VB.Val(mstrFormNo);

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(reader.HasRows && reader.Read())
                {
                    switch(reader.GetValue(0).ToString().Trim())
                    {
                        case "L":
                            ssUserChartHisIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                            break;
                        case "P":
                            ssUserChartHisIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                            break;
                        case "S":
                            ssUserChartHisIO_Sheet1.PrintInfo.UseSmartPrint = true;
                            break;
                    }
                }
                reader.Dispose();


                if (PrintType == "V")
                {
                    ssUserChartHisIO_Sheet1.PrintInfo.Preview = true;
                }

                ssUserChartHisIO.PrintSheet(0);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void SsUserChartHisIO_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssUserChartHisIO_Sheet1.RowCount == 0)
                return;

            if (e.Column <= 1)
                return;

            ClearSp();

            string strCHARTDATE = ComFunc.SptChar(ssUserChartHisIO_Sheet1.Cells[0, e.Column].Text.Trim(), 0, "/");
            string strCHARTTIME = ComFunc.SptChar(ssUserChartHisIO_Sheet1.Cells[0, e.Column].Text.Trim(), 1, "/");
            string strDuty = ssUserChartHisIO_Sheet1.Cells[1, e.Column].Text.Trim();

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(strCHARTDATE);
            usFormTopMenuEvent.dtMedFrDate.Enabled = false;
            usFormTopMenuEvent.txtMedFrTime.Text = strCHARTTIME;

            ssUserChartIO_Sheet1.Cells[1, 3].Text = strDuty;

            string strEmrNo = ssUserChartHisIO_Sheet1.Cells[39, e.Column].Text.Trim();
            mstrEmrNo = strEmrNo;
            mEmrUseId = ssUserChartHisIO_Sheet1.Cells[40, e.Column].Text.Trim();

            #region 권한 버튼
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //초기값 세팅
            if (VB.Val(mstrEmrNo) <= 0)
            {
                usFormTopMenuEvent.mbtnPrint.Visible = false;
                usFormTopMenuEvent.mbtnPrintNull.Visible = true;
            }
            else if (VB.Val(mstrEmrNo) > 0)
            {
                if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                {
                    usFormTopMenuEvent.mbtnSave.Visible = false;
                    usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                    usFormTopMenuEvent.mbtnDelete.Visible = false;
                }
            }

            //사본발급 출력여부
            usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(mstrEmrNo);
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
            #endregion

            int i = 0;
            int intCol = 0;

            for (int xCol1 = 0; xCol1 < ssUserChartHisIO_Sheet1.ColumnCount; xCol1++)
            {
                if (strEmrNo == ssUserChartHisIO_Sheet1.Cells[39, xCol1].Text.Trim())
                {
                    i++;
                    if (i == 1)
                    {
                        intCol = 2;
                    }
                    else if (i == 2)
                    {
                        intCol = 3;
                        i = 0;
                    }

                    for (int xRow1 = 3; xRow1 < 38; xRow1++)
                    {
                        ssUserChartIO_Sheet1.Cells[xRow1, intCol].Text = ssUserChartHisIO_Sheet1.Cells[xRow1, xCol1].Text.Trim();
                    }
                }
            }

        }

        void ClearSp()
        {
            for (int i = 3; i < ssUserChartIO_Sheet1.RowCount; i++)
            {
                for (int j = 3; j < ssUserChartIO_Sheet1.ColumnCount; j++)
                {
                    ssUserChartIO_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }

            DesignFunc.DefaultDuty(ssUserChartIO, 2);
            ssUserChartIO_Sheet1.Cells[4, 3].Text = "배꼽소독";
            ssUserChartIO_Sheet1.Cells[5, 3].Text = "Size Bath";
            ssUserChartIO_Sheet1.Cells[6, 3].Text = "회음간호";

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Text = VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)).ToString("00:00");
            mstrEmrNo = "0";
            mEmrUseId = string.Empty;
        }

        private void BtnSearchPreHis_Click(object sender, EventArgs e)
        {
            if (ssUserChartHisIO_Sheet1.ActiveColumnIndex <= 1)
                return;


            GetLastValue(ssUserChartHisIO_Sheet1.ActiveColumnIndex);
        }

        void GetLastValue(int Col)
        {
            ClearSp();

            string strDuty = ssUserChartHisIO_Sheet1.Cells[1, Col].Text.Trim();
            ssUserChartIO_Sheet1.Cells[1, 1].Text = strDuty;

            string strEmrNo = ssUserChartHisIO_Sheet1.Cells[39, Col].Text.Trim();

            int i = 0;
            int intCol = 0;

            for (int xCol1 = 0; xCol1 < ssUserChartHisIO_Sheet1.ColumnCount; xCol1++)
            {
                if (strEmrNo == ssUserChartHisIO_Sheet1.Cells[39, xCol1].Text.Trim())
                {
                    i++;
                    if (i == 1)
                    {
                        intCol = 2;
                    }
                    else if (i == 2)
                    {
                        intCol = 3;
                        i = 0;
                    }

                    for (int xRow1 = 3; xRow1 < 38; xRow1++)
                    {
                        ssUserChartIO_Sheet1.Cells[xRow1, intCol].Text = ssUserChartHisIO_Sheet1.Cells[xRow1, xCol1].Text.Trim();
                    }
                }
            }
        }


        private void FrmTextEmr_NrActFall_Resize(object sender, EventArgs e)
        {
            ssUserChartIO.Height = Height - panTopMenu.Height - panel1.Height - 100;

            ssUserChartHisIO.Left = ssUserChartIO.Visible ? 30 : 600;

            ssUserChartHisIO.Width  = Width - ssUserChartHisIO.Left;
            ssUserChartHisIO.Height = Height - panTopMenu.Height - panel1.Height - 100;

        }

        private void ssUserChartHisIO_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void ssUserChartIO_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == 2 && e.Row == 1)
            {
                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
                string strDuty = ssUserChartIO_Sheet1.Cells[e.Row, e.Column].Text.Trim();

                READ_JELLCO(pAcp.ptNo, pAcp.medFrDate, strChartDate, strDuty);
            }
        }

        void READ_JELLCO(string argPTNO, string ArgInDate, string argChartDate, string ArgGubun)
        {
            ssUserChartIO_Sheet1.Cells[11, 2].Text = "";
            ssUserChartIO_Sheet1.Cells[11, 3].Text = "";

            ssUserChartIO_Sheet1.Cells[12, 2].Text = "";
            ssUserChartIO_Sheet1.Cells[12, 3].Text = "";

            string strCHARTsDATE = string.Empty;
            string strCHARTeDATE = string.Empty;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strOK = string.Empty;

            string strTEMP1 = string.Empty;
            string strTEMP2 = string.Empty;
            string strTEMP3 = string.Empty;

            try
            {

                switch (ArgGubun)
                {
                    case "Day":
                        strCHARTsDATE = argChartDate + " " + "08:00";
                        strCHARTeDATE = argChartDate + " " + "15:29";
                        break;
                    case "Evening":
                        strCHARTsDATE = argChartDate + " " + "15:30";
                        strCHARTeDATE = argChartDate + " " + "22:59";
                        break;
                    case "Night":
                        strCHARTsDATE = Convert.ToDateTime(argChartDate).AddDays(-1).ToString("yyyy-MM-dd") + " " + "23:00";
                        strCHARTeDATE = argChartDate + " " + "07:59";
                        break;
                }


                #region 점검
                SQL = " SELECT TO_CHAR(CHARTDATE, 'YYYY-MM-DD') CHARTDATE, TO_CHAR(CHARTDATE,'HH24:MI') CHARTTIME, CHARTDATE SORTTIME, GUBUN1, GUBUN2, KEEP ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + ArgInDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= TO_DATE('" + strCHARTsDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= TO_DATE('" + strCHARTeDATE + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strOK = "OK";                   
                }
                else
                {
                    strOK = "NO";
                }

                dt.Dispose();
                #endregion

                SQL = " SELECT TO_CHAR(CHARTDATE, 'YYYY-MM-DD') CHARTDATE, TO_CHAR(CHARTDATE,'HH24:MI') CHARTTIME, CHARTDATE SORTTIME, GUBUN1, GUBUN2, KEEP ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + ArgInDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= TO_DATE('" + Convert.ToDateTime(argChartDate).AddDays(-30).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["GUBUN1"].ToString().Trim())
                    {
                        case "0":
                            strTEMP1 = "Arm";
                            strTEMP3 = strOK.Equals("OK") ? "START" : "KEEP";
                            break;
                        case "1":
                            strTEMP1 = "Leg";
                            strTEMP3 = strOK.Equals("OK") ? "START" : "KEEP";
                            break;
                        case "X":
                            strTEMP1 = "";
                            strTEMP3 = strOK.Equals("OK") ? "Remove" : "";
                            break;
                    }

                    switch (dt.Rows[0]["GUBUN2"].ToString().Trim())
                    {
                        case "0":
                            strTEMP2 = "Right";
                            break;
                        case "1":
                            strTEMP2 = "Left";
                            break;
                        case "2":
                            strTEMP2 = "Both";
                            break;
                        case "X":
                            strTEMP2 = "";
                            break;
                    }

                    ssUserChartIO_Sheet1.Cells[11, 2].Text = strTEMP1.Length == 0 && strOK == "NO" ? "" : "Peripheral";

                    ssUserChartIO_Sheet1.Cells[11, 3].Text = strTEMP1 + " " + strTEMP2;

                    ssUserChartIO_Sheet1.Cells[11, 2].Text = strTEMP3;
                    ssUserChartIO_Sheet1.Cells[11, 3].Text = strOK == "OK" ? dt.Rows[0]["CHARTDATE"].ToString().Trim() + " " +
                        dt.Rows[0]["CHARTTIME"].ToString().Trim() : "";
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }
    }
}
