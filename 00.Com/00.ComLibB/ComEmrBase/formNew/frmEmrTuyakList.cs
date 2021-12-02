using ComBase;
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrTuyakList : Form, EmrChartForm
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

        string mEmrUseId = string.Empty;

        int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,

        //bool mHeadVisible = true;   //해드를 보이게 할지 여부
        /// <summary>
        /// 차트복사 신청번호
        /// </summary>
        string mstrPrtReqNo = string.Empty;
        /// <summary>
        /// 차트복사 과
        /// </summary>
        string mstrDeptCode = string.Empty;

        bool ErCoordinator = false;

        Font fontBold = new Font("굴림체", 9, FontStyle.Bold);
        Font fontRegular = new Font("굴림체", 9, FontStyle.Regular);

        FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
        FarPoint.Win.Spread.CellType.ComboBoxCellType TypeComboBox = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
        FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;

        public string mstrFormNo = "1796";
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
            usFormTopMenuEvent.rSetSaveTemp += new usFormTopMenu.SetSaveTemp(usFormTopMenuEvent_SetSaveTemp);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

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
            pSaveEmrData();
        }

        private void usFormTopMenuEvent_SetSaveTemp(string strFrDate, string strFrTime)
        {
            pSaveEmrData();
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            if(pDelData())
            {
                ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                pClearForm();
                GetSearchData(null);
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

        #region // 상용구 관련 변수 선언
        //private Control mControl = null;
        //private frmMacrowordProg frmMacrowordProgEvent;

        //private Control mCalControl = null; //달력 띄우기
        //private frmEmrCaledar frmEmrCaledarEvent;

        //private FarPoint.Win.Spread.FpSpread ssMacroWord;
        //private FarPoint.Win.Spread.SheetView ssMacroWord_Sheet1;
        #endregion // 상용구 관련 모듈

        #region //EmrChartForm

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }
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

        public int PrintFormMsg(string strPRINTFLAG)
        {
            return pPrintForm();
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region //생성자
        public frmEmrTuyakList()
        {
            InitializeComponent();
        }

        public frmEmrTuyakList(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;

            pInitFormTemplet();
        }

        public frmEmrTuyakList(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            if (mstrEmrNo.IndexOf("|") != -1)
            {
                mstrDeptCode = mstrEmrNo.Split('|')[0];
                mstrEmrNo = mstrEmrNo.Split('|')[1];
            }
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();

            pInitFormTemplet();
        }


        /// <summary>
        /// 19-07-09 신규 생성자 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strPRTREQNO"></param>
        /// <param name="pEmrCallForm"></param>
        public frmEmrTuyakList(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strPRTREQNO, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrPrtReqNo = strPRTREQNO;
            InitializeComponent();

            pInitFormTemplet();
        }
        #endregion //생성자

        #region 스프레드 enum
        private enum OCSTUYAKLIST
        {
            /// <summary>
            /// 작성일자
            /// </summary>
            ChartDate,
            /// <summary>
            /// 시간
            /// </summary>
            ChartTime,
            /// <summary>
            /// 처방일자
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
            /// 용법 및 검체
            /// </summary>
            DosName,
            /// <summary>
            /// 횟수
            /// </summary>
            Qty,
            /// <summary>
            /// 일투량
            /// </summary>
            RealQty,
            /// <summary>
            /// 비고
            /// </summary>
            Remark,
            /// <summary>
            /// 일련번호
            /// </summary>
            SerialNumber,
            /// <summary>
            /// Acting
            /// </summary>
            Acting,
            /// <summary>
            /// 간호사 참고사항
            /// </summary>
            NurseNote,
            /// <summary>
            /// 작성자
            /// </summary>
            WriteName,
            /// <summary>
            /// 사본발급
            /// </summary>
            CopyYN
        }

        private enum OCSTUYAK
        {
            /// <summary>
            /// 처방일자
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
            /// 용법 및 검체
            /// </summary>
            DosName,
            /// <summary>
            /// 일용량
            /// </summary>
            Content,
            /// <summary>
            /// 일투량(횟수)
            /// </summary>
            RealQty,
            /// <summary>
            /// 비고
            /// </summary>
            Remark,
            /// <summary>
            /// 일련번호
            /// </summary>
            SerialNumber,
            /// <summary>
            /// Acting
            /// </summary>
            Acting,
            /// <summary>
            /// 간호사 참고사항
            /// </summary>
            NurseNote
        }
        #endregion

        private void frmEmrTuyakList_Load(object sender, EventArgs e)
        {
            SetCombo();

            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            clsEmrPublic.bOrderAsc = chkAsc.Checked;

            //usFormTopMenuEvent.mbtnClear.Visible = true;
            //usFormTopMenuEvent.mbtnPrint.Visible = true;
            //usFormTopMenuEvent.mbtnSave.Visible = true;
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            if (mstrMode == "P")
            {
                ssView_Sheet1.Columns[(int)OCSTUYAKLIST.CopyYN].Visible = false;
                mstrPrtReqNo = mstrEmrNo;
            }


            if (mstrPrtReqNo != "" )
            {
                panTopMenu.Visible = false;
                dtpFrDate.Enabled = false;
                dtpEndDate.Enabled = false;
                spcChart.Panel1Collapsed = true;
                spcChart.Panel1MinSize = 0;

                ssView_Sheet1.Columns[4].Width += 140;
                ssView_Sheet1.Columns[8].Width = 40;
                ssView_Sheet1.Columns[11].Width = 60;
                btnHis.Visible = false;
                btnSearchCode.Visible = false;
                GetSearchData(null, true);
                return;
            }


            if (mEmrCallForm != null && (((Form)mEmrCallForm).Name.Equals("frmEmrLibViewerNr") == false))
            {
                dtpFrDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));
                if (pAcp.inOutCls == "O")
                {
                    if (pAcp.medDeptCd == "ER")
                    {
                        dtpEndDate.Value = dtpFrDate.Value.AddDays(+1);
                    }
                    else
                    {
                        dtpEndDate.Value = dtpFrDate.Value;
                    }
                }
                else
                {
                    dtpEndDate.Value = Convert.ToDateTime(pAcp.medEndDate != "" ? ComFunc.FormatStrToDate(pAcp.medEndDate, "D") : ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }
            }
            else
            {
                dtpFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3);
                dtpEndDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            }

            pClearForm();

            #region //19-12-02 심사팀 필터기능 추가
            if (clsType.User.BuseCode.Equals("078201"))
            {
                for (int i = 0; i < ssView.ActiveSheet.Columns.Count; i++)
                {
                    ssView.ActiveSheet.Columns.Get(i).AllowAutoFilter = true;
                }
                ssView.ActiveSheet.AutoFilterMode = AutoFilterMode.EnhancedContextMenu;
            }
            #endregion

            GetSearchData(null);
        }

        #region //차트조회

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            GetSearchData(sender, mstrPrtReqNo != "" ? true :false);
        }

        private void BtnSearchCode_Click(object sender, EventArgs e)
        {
            GetSearchData(sender);
        }

        /// <summary>
        /// 조회함수(True: 차트복사, False: 기본 조회)
        /// </summary>
        /// <param name="Copy">True: 차트복사, False: 기본 조회</param>
        void GetSearchData(object sender, bool Copy = false)
        {
            //If START_TUYAK Then

            DataTable dt = null;
            ssView_Sheet1.RowCount = 0;

            ErCoordinator = clsEmrFunc.IsErCoordinator(clsDB.DbCon);
            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);
            string strSearhCode = string.Empty;

            string strOldDate = string.Empty;
            Color dblColorInfo = Color.White;

            if (sender == btnSearchCode)
            {
               strSearhCode = VB.InputBox("검색하실 코드를 입력하세요.", "입력");
            }



            StringBuilder SQL = new StringBuilder();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (Copy)
                {
                    SQL.AppendLine("  SELECT A.EMRNO, A.USEID, (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(A.USEID, '0')) USENAME, WRITEDATE, WRITETIME,");
                    SQL.AppendLine("A.CHARTDATE, A.CHARTTIME, 1796 FORMNO, '투약기록지' FORMNAME, 1796 USERFORMNO, ");
                    SQL.AppendLine("  it1 , it2, it3, it4, it5, it6, it7, it8, it9, it10,");
                    SQL.AppendLine("  (SELECT BUN FROM KOSMOS_OCS.OCS_ORDERCODE WHERE TRIM(SUCODE) = A.IT2 AND ROWNUM = 1) AS BUN");
                    SQL.AppendLine("  , '' AS PRNTYN");
                    SQL.AppendLine("  FROM KOSMOS_EMR.EMRXML_TUYAK A");
                    SQL.AppendLine("    INNER JOIN KOSMOS_EMR.EMRPRTREQ E ");
                    SQL.AppendLine("       ON E.PRTREQNO = " + mstrPrtReqNo);
                    SQL.AppendLine("       AND E.EMRNO = A.EMRNO");
                    SQL.AppendLine("       AND E.SCANYN = 'T'" );
                    SQL.AppendLine("   WHERE A.MEDDEPTCD = '" + mstrDeptCode + "'");

                    if (chkAsc.Checked)
                    {
                        SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME)");
                    }
                    else
                    {
                        SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME) DESC");
                    }
                }
                else
                {
                    SQL.AppendLine(" SELECT A.EMRNO, A.WRITEDATE, A.WRITETIME, A.USEID, A.CHARTDATE, A.CHARTTIME, 1796 FORMNO, '투약기록지' FORMNAME,  1796 USERFORMNO");
                    SQL.AppendLine(", IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10, ");
                    SQL.AppendLine("  (SELECT BUN FROM KOSMOS_OCS.OCS_ORDERCODE WHERE TRIM(SUCODE) = A.IT2 AND ROWNUM = 1) AS BUN");
                    SQL.AppendLine(", CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_EMR.EMRPRTREQ WHERE EMRNO = A.EMRNO AND SCANYN = 'T') THEN '사 본' ELSE '' END PRNTYN");
                    SQL.AppendLine(", (SELECT NAME FROM KOSMOS_EMR.EMR_USERT WHERE USERID = LTRIM(A.USEID, '0')) AS USENAME");
                    SQL.AppendLine("FROM KOSMOS_EMR.EMRXML_TUYAK A");
                    SQL.AppendLine("   WHERE A.FORMNO = 1796");
                    SQL.AppendLine("     AND A.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("     AND A.MEDFRDATE = '" + pAcp.medFrDate + "' ");

                    if (pAcp.inOutCls == "O")
                    { 
                        if (pAcp.medDeptCd == "RA" || (pAcp.medDeptCd == "MD" && (pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("     AND A.MEDDEPTCD = 'MD'");
                            SQL.AppendLine("     AND A.MEDDRCD IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("     AND A.MEDDEPTCD = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("     AND A.MEDDRCD NOT IN ('1107','1125')");
                        }
                    }
                    else
                    {
                        SQL.AppendLine("     AND A.INOUTCLS IN ('I') ");
                    }

                    if (pAcp.inOutCls == "O" && pAcp.medDeptCd == "HD")
                    {
                        SQL.AppendLine("     AND A.INOUTCLS IN ('O','I') ");
                    }
                    else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "HD")
                    {
                        SQL.AppendLine("     AND A.INOUTCLS IN ('O') ");
                    }

                    if (mViewNpChart == false)
                    {
                        SQL.AppendLine("     AND A.MEDDEPTCD <> 'NP'");
                    }

                    if(strSearhCode.Length > 0)
                    {
                        SQL.AppendLine("     AND IT2 LIKE '%" + strSearhCode.ToUpper() +"%'");
                    }

                    SQL.AppendLine("     AND A.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ");
                    SQL.AppendLine("     AND A.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ");

                    if(chkAsc.Checked)
                    {
                        SQL.AppendLine("  ORDER BY (CHARTDATE || CHARTTIME)");
                    }
                    else
                    {
                        SQL.AppendLine("  ORDER BY (CHARTDATE || CHARTTIME) DESC");
                    }
                }
        

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                string strTime = string.Empty;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strTime = StrToTime(dt.Rows[i]["ChartTime"].ToString().Trim());
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.ChartDate].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.ChartDate].Tag = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.ChartTime].Text =
                        VB.Left(strTime, 2) + ":" +
                        VB.Mid(strTime, 3, 2);
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.ChartTime].Tag = dt.Rows[i]["WRITETime"].ToString().Trim();

                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.Bdate].Text = dt.Rows[i]["IT1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.OrderCode].Text = dt.Rows[i]["IT2"].ToString().Trim();

                    if (mEmrCallForm != null && (((Form)mEmrCallForm).Name.Equals("frmEmrLibViewerNr") == false))
                    {
                        switch (dt.Rows[i]["BUN"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                                ssView_Sheet1.Rows[i].ForeColor = Color.FromArgb(240, 50, 50);
                                break;
                            case "20":
                                ssView_Sheet1.Rows[i].ForeColor = Color.FromArgb(50, 50, 240);
                                break;
                            default:
                                ssView_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 0);
                                break;
                        }
                    }

                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.OrderName].Text = dt.Rows[i]["IT3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.DosName].Text = dt.Rows[i]["IT4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.Qty].Text = dt.Rows[i]["IT5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.RealQty].Text = dt.Rows[i]["IT6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.Remark].Text = dt.Rows[i]["IT7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.SerialNumber].Text = dt.Rows[i]["IT8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.Acting].Text = dt.Rows[i]["IT9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.NurseNote].Text = dt.Rows[i]["IT10"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.WriteName].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.WriteName].Tag = dt.Rows[i]["USEID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.CopyYN].Text = dt.Rows[i]["PRNTYN"].ToString().Trim();
                    ssView_Sheet1.Cells[i, (int)OCSTUYAKLIST.CopyYN].Tag = dt.Rows[i]["EMRNO"].ToString().Trim();

                    #region 색변경
                    if (Copy == false && (clsType.User.BuseCode.Equals("044201") && clsType.User.AuAMANAGE == "1" || clsType.User.BuseCode.Equals("078201") || ErCoordinator == true))
                    {
                        if (!strOldDate.Equals(dt.Rows[i]["CHARTDATE"].ToString()))
                        {
                            dblColorInfo = dblColorInfo == Color.White ? ComNum.SPSELCOLOR : Color.White;
                            strOldDate = dt.Rows[i]["CHARTDATE"].ToString();
                        }

                        ssView_Sheet1.Rows[i].BackColor = dblColorInfo;
                    }
                    #endregion

                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private string StrToTime(string strTime)
        {
            strTime = VB.Left(strTime, 4);
            DateTime dateTime;
            if (DateTime.TryParseExact(strTime, "HHmm", null, System.Globalization.DateTimeStyles.None, out dateTime) == false)
            {
                return "0" + strTime;
            }
            else
            {
                return strTime;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            pPrintForm();
        }
        #endregion //차트조회

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

            SetTopMenuLoad();

            //if (mstrFormNo != "0")
            //{
            //    FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFormFlowSheet, ref mFormFlowSheetHead);
            //}
        }

        /// <summary>
        /// 권한별 환경을 설정을 한다(작성화면 안보이게 등)
        /// </summary>
        private void SetUserAut()
        {
            if (clsType.User.AuAWRITE == "1")
            {
                panTopMenu.Visible = true;
                panWrite.Visible = true;
            }
            else
            {
                panTopMenu.Visible = false;
                panWrite.Visible = false;
            }

            if (clsType.User.AuAPRINTIN == "1")
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }

            panWrite.Visible = mstrMode.Equals("W");
            panTopMenu.Visible = panWrite.Visible;

            usFormTopMenuEvent.mbtnClear.Visible = true;
            usFormTopMenuEvent.mbtnPrintNull.Visible = true;
            usFormTopMenuEvent.mbtnSave.Visible = true;
            usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            spcChart.Panel1Collapsed = !panWrite.Visible;
            spcChart.Panel1MinSize = spcChart.Panel1Collapsed ? 0 : spcChart.Panel1MinSize;
        }

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            ssWrite_Sheet1.RowCount = 1;
            ssWrite_Sheet1.Cells[0, 0, 0, ssWrite_Sheet1.ColumnCount - 1].Text = "";

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
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
                return dblEmrNo;
            }

            if (usFormTopMenuEvent.txtMedFrTime.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                return dblEmrNo;
            }

            string strDate = string.Format("{0} {1}:{2}",
                usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString(),
                VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2),
                VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2));

            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return dblEmrNo;
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

            if (SaveChart() == true)
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");
                pClearForm();
                btnSearchAll.PerformClick();
            }

            return dblEmrNo;
        }

        /// <summary>
        /// 저장 
        /// </summary>
        /// <returns></returns>
        bool SaveChart()
        {
            bool rtnVal = false;
            try
            {
                if (pAcp.inOutCls == "" || pAcp.ptNo == "" || pAcp.medDeptCd == "" ||
                   pAcp.medDrCd == "" || pAcp.medFrDate == "")
                {
                    ComFunc.MsgBoxEx(this, "환자 정보가 정확하지 않습니다." + ComNum.VBLF + "확인 후 다시 시도 하십시오.");
                    return rtnVal;
                }

                if (VB.Val(mstrEmrNo) != 0)
                {
                    if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                        return rtnVal;

                    if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                        return rtnVal;

                    if (ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?") == DialogResult.No)
                    {
                        return rtnVal;
                    }

                    if (clsType.User.IdNumber != VB.Val(mEmrUseId).ToString())
                    {
                        ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                        return rtnVal;
                    }
                }

                Cursor.Current = Cursors.WaitCursor;
                if (VB.Val(mstrEmrNo) != 0)
                {
                    if (clsEmrQuery.DeleteEmrXmlData(mstrEmrNo) == false)
                    {
                        return rtnVal;
                    }
                }

                rtnVal = clsEmrQuery.Insert_EMR_TuYak(ssWrite, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "").Trim(), pAcp);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
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
        /// 스프래드에 출력여부 표시하거나 숨기기
        /// </summary>
        private void SetPrintVisible()
        {
      
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
            if (VB.Val(mstrEmrNo) == 0)
                return false;

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (mEmrUseId.Trim() != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    return false;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                    return false;

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                    return false;

                if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?") == DialogResult.No)
                {
                    return false;
                }
            }

            return clsEmrQuery.DeleteEmrXmlData(mstrEmrNo);
        }

        /// <summary>
        /// 출력
        /// </summary>
        private int pPrintForm()
        {
            int rtnVal = 0;
            string strMode = "A";
            if (mstrPrtReqNo != "")
            {
                strMode = "P";
                clsFormPrint.mstrPRINTFLAG = "0"; //원외출력으로 변경
            }

            ssView_Sheet1.Columns[(int)OCSTUYAKLIST.CopyYN].Visible = false;

            btnPrint.Enabled = false;

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Font = fontBold;

            List<int> lstVal = new List<int>();

            #region 출력전 배경색 로우 저장

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (ssView_Sheet1.Rows[i].BackColor == ComNum.SPSELCOLOR)
                {
                    lstVal.Add(i);
                }
            }
            ssView_Sheet1.Rows[0, ssView_Sheet1.RowCount - 1].BackColor = Color.White;
            #endregion

            rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                        ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, "COL", -1, ssView_Sheet1.ColumnCount - 3, 0, strMode);

            ComFunc.Delay(1000);

            #region 출력후 배경색 되돌리기
            for (int i = 0; i < lstVal.Count; i++)
            {
                ssView_Sheet1.Rows[lstVal[i]].BackColor = ComNum.SPSELCOLOR;
            }
            #endregion

            pInitFormSpc();

            btnPrint.Enabled = true;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Font = fontRegular;
            ssView_Sheet1.Columns[(int)OCSTUYAKLIST.CopyYN].Visible = true;

            return rtnVal;
        }

        #endregion //Private Function
                
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
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            //SetFormInfo(); //TODO

            SetTopMenuLoad();

        }

 
        /// <summary>
        /// 폼에 이전내역을 세팅한다
        /// </summary>
        public void LoadForm()
        {
            pClearForm();
            pSetEmrInfo();
            //pMultiTextHeigh();
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
            string SQL = " SELECT NAL ";

            try
            {
                SQL = string.Empty;
                SQL = " SELECT NAL ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_OPTION_SETDATE ";
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
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (reader.GetValue(0).ToString().Trim().Length == 0 ||
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
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 비고 콤보박스 설정
        /// </summary>
        void SetCombo()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT ITEMRMK";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMROPTFORM";
                SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = 1796";
                SQL = SQL + ComNum.VBLF + "    AND CONTROLID = 'it7'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(reader.HasRows && reader.Read())
                {
                    string[] strCombo = reader.GetValue(0).ToString().Split('^');
                    clsSpread.gSpreadComboDataSetEx(ssWrite, 0, (int)OCSTUYAKLIST.Remark, 0, (int)OCSTUYAKLIST.Remark, strCombo);
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }

        private void SsView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
                return;

            for(int i = 2; i < ssView_Sheet1.Columns.Count - 1; i++)
            {
                ssWrite_Sheet1.Cells[0, i].Text = ssView_Sheet1.Cells[e.Row, i].Text.Trim();
            }

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssView_Sheet1.Cells[e.Row, (int)OCSTUYAKLIST.ChartDate].Text.Trim());
            usFormTopMenuEvent.txtMedFrTime.Text = ssView_Sheet1.Cells[e.Row, (int)OCSTUYAKLIST.ChartTime].Text.Trim();

            mEmrUseId = ssView_Sheet1.Cells[e.Row, (int)OCSTUYAKLIST.WriteName].Tag.ToString();
            mstrEmrNo = ssView_Sheet1.Cells[e.Row, (int)OCSTUYAKLIST.CopyYN].Tag.ToString().Trim();
            if (VB.Val(mEmrUseId).ToString().Equals(clsType.User.IdNumber))
            {
                usFormTopMenuEvent.mbtnSave.Visible = true;
                //usFormTopMenuEvent.mbtnSaveTemp.Visible = true;
                usFormTopMenuEvent.mbtnDelete.Visible = true;
            }
            else
            {
                usFormTopMenuEvent.mbtnSave.Visible = false;
                usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                usFormTopMenuEvent.mbtnDelete.Visible = false;
            }
        }

        private void SsView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (clsType.User.BuseCode == "044201" || ErCoordinator == true)
            {
                lblServerDate.Text = string.Format("{0} \r\n{1}", ssView_Sheet1.Cells[e.Row, (int)OCSTUYAKLIST.ChartDate].Tag, VB.Val(ssView_Sheet1.Cells[e.Row, (int)OCSTUYAKLIST.ChartTime].Tag.ToString()).ToString("00:00:00"));
                lblServerDate.Visible = true;
            }
            else
            {
                lblServerDate.Visible = false;
            }
        }

        private void btnHis_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0 || ssView_Sheet1.ActiveRowIndex < 0)
                return;

            pClearForm();

            mEmrUseId = string.Empty;
            mstrEmrNo = string.Empty;

            for (int i = 2; i < ssView_Sheet1.Columns.Count - 1; i++)
            {
                ssWrite_Sheet1.Cells[0, i].Text = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, i].Text.Trim();
            }
        }

        private void chkAsc_CheckedChanged(object sender, EventArgs e)
        {
            clsEmrPublic.bOrderAsc = chkAsc.Checked;
        }
    }
}
