using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ComBase;
using FarPoint.Win.Spread.CellType;
using Microsoft.VisualBasic;
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// 상처간호기록지, 욕창예방 간호활동기록지
    /// FORMNO = 2574
    /// \mtsEmrf\frmTextEmr_NrActiveTotalCare.frm
    /// </summary>
    public partial class frmTextEmrVtChart : Form, EmrChartForm
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

        public string mstrFormNo = "2476";
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

            if (clsType.User.AuAPRINTIN.Equals("1"))
            {
                btnPrint.Visible = true;
            }

            ssUserChart.Visible = clsType.User.AuAMANAGE.Equals("1") || clsType.User.AuAWRITE.Equals("1");
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

            if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            string strDate = string.Format("{0} {1}:{2}",
                usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString(),
                VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2),
                VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2));

            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return VB.Val(mstrEmrNo);
            }

            if (SaveUserChart() == true)
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");
                ClearSp();
                btnSearch.PerformClick();
            }

            return VB.Val(mstrEmrNo);
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
                                         ssUserChartHis, "V", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, 20, mFLOWHEADCNT, "V");
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
        public frmTextEmrVtChart()
        {
            InitializeComponent();
        }

        public frmTextEmrVtChart(string pMake, int pItemCnt, int pHeadCnt, FormFlowSheet[] pFormFlowSheet, FormFlowSheetHead[,] pFormFlowSheetHead)
        {
            InitializeComponent();
            mFLOWGB = pMake;
            mFLOWITEMCNT = pItemCnt;
            mFLOWHEADCNT = pHeadCnt;
            mFormFlowSheet = pFormFlowSheet;
            mFormFlowSheetHead = pFormFlowSheetHead;

            pInitFormTemplet();
        }

        public frmTextEmrVtChart(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
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
        public frmTextEmrVtChart(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        public frmTextEmrVtChart(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        #region 변수
        private string gUserGrade = string.Empty;
        #endregion
        
        private void frmTextEmr_NrActiveTotalCare_Load(object sender, EventArgs e)
        {
            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            usFormTopMenuEvent.mbtnClear.Visible = true;
            //usFormTopMenuEvent.mbtnPrint.Visible = true;
            usFormTopMenuEvent.mbtnSave.Visible = true;
            usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            if (mEmrCallForm != null && (((Form)mEmrCallForm).Name.Equals("frmEmrLibViewerNr") == false))
            {
                dtpOptSDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));
                dtpOptEDate.Value = Convert.ToDateTime(pAcp.medEndDate.Length == 0 ? ComQuery.CurrentDateTime(clsDB.DbCon, "S") : ComFunc.FormatStrToDate(pAcp.medEndDate, "D"));
            }
            else
            {
                dtpOptSDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3);
                dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            }

            //btnPrint.Visible = false;

            usFormTopMenuEvent.mbtnPrint.Visible  = clsType.User.AuAPRINTIN.Equals("1");
            btnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");

            if (mstrOrderDate.Length > 0 || mEmrCallForm == null)
                return;

            ClearChart();

            ssUserChartHis_Sheet1.RowCount = 0;
            MakeUserChart(mstrFormNo, 14);
            #region 틀고정
            ssUserChartHis_Sheet1.FrozenRowCount = 16;
            #endregion

            if (pAcp.ptNo.Length > 0)
            {
                GetChartHistory();
                GetDcur(pAcp.ptNo, usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString());
            }
        }



        void GetDcur(string strPtNo, string strChartDate)
        {
            OracleDataReader dataReader = null;

            string SQL = " SELECT Total ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE B ";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER M";
            SQL = SQL + ComNum.VBLF + "      ON B.IPDNO = M.IPDNO";
            SQL = SQL + ComNum.VBLF + " AND M.PANO = '" + strPtNo + "'";
            SQL = SQL + ComNum.VBLF + " AND B.ACTDATE = TO_DATE('" + strChartDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + " AND M.Amset4 <> '3'";
            SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000'";
            SQL = SQL + ComNum.VBLF + " AND M.Pano <> '81000004'";

            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dataReader.HasRows)
            {
                while(dataReader.Read())
                {
                    if (ssUserChart_Sheet1.Columns.Count > 0)
                    {
                        ssUserChart_Sheet1.Cells[15, 0].Text = dataReader.GetValue(0).ToString().Trim() == "0" ? "" : dataReader.GetValue(0).ToString().Trim();
                    }
                }
            }
            dataReader.Dispose();
        }


        void ClearForm()
        {
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;

            mstrEmrNo = "0";

            for(int i = 3; i < ssUserChart_Sheet1.RowCount; i++)
            {
                for(int j = 3; j < ssUserChart_Sheet1.ColumnCount; j++)
                {
                    ssUserChart_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }
        }
        
      
        bool SaveUserChart()
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

                    if (clsType.User.IdNumber != mEmrUseId)
                    {
                        ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                        return rtnVal;
                    }
                }

                string strHead = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
                string strChartX1 = "<chart>";
                string strChartX2 = "</chart>";

                string strCONTENTS = "(SELECT CONTENTS FROM KOSMOS_EMR.EMRFORM WHERE FORMNO = " + VB.Val(mstrFormNo) + ")";
                string strUPDATENO = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(mstrFormNo)).ToString();

                Cursor.Current = Cursors.WaitCursor;

                if(VB.Val(mstrEmrNo) != 0)
                {
                    if(clsEmrQuery.DeleteEmrXmlData(mstrEmrNo) == false)
                    {
                        return rtnVal;
                    }
                }


                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
                string strChartTime = usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "").Trim();
                string strInOutCls = pAcp.inOutCls;

                string strTAGHEAD = string.Empty;
                string strTAGTAIL = string.Empty;

                for (int lngCol = 0; lngCol < ssUserChart_Sheet1.ColumnCount; lngCol++)
                {
                    double dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");
                    string strXML = strHead + strChartX1;

                    for (int lngRow = 14; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
                    {
                        strTAGHEAD = ssFORMXML_Sheet1.Cells[lngRow, 0].Text.Trim();
                        strTAGTAIL = ssFORMXML_Sheet1.Cells[lngRow, 1].Text.Trim();

                        strXML += strTAGHEAD + ssUserChart_Sheet1.Cells[lngRow, lngCol].Text.Trim().Replace("'", "`") + strTAGTAIL;

                    }
                    strXML += strChartX2;

                    if(clsEmrQuery.SaveEmrXmlData(dblEmrNo.ToString(), mstrFormNo, strChartDate, strChartTime,
                    pAcp.acpNo, pAcp.ptNo, strInOutCls, pAcp.medFrDate, pAcp.medFrTime,
                    pAcp.medEndDate, pAcp.medEndTime, pAcp.medDeptCd, pAcp.medDrCd,
                    strXML, strUPDATENO))
                    {
                        mstrEmrNo = dblEmrNo.ToString();
                    }
                }


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
            ssUserChart_Sheet1.ColumnCount = 0;
            ssUserChart_Sheet1.ColumnCount = 1;
            ssUserChart_Sheet1.Columns[0].Locked = false;

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)).ToString("00:00");
        }

        void MakeUserChart(string strFormNo, int lngStartRow)
        {
            ssUserChart_Sheet1.RowCount = lngStartRow;
            ssUserChartHis_Sheet1.RowCount = lngStartRow + 2;
            ssUserChartHis_Sheet1.ColumnCount = 0;
            ssUserChartHis_Sheet1.ColumnCount = 1;
            ssFORMXML_Sheet1.RowCount = lngStartRow;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;

            CheckBoxCellType TypeCheckBox = new CheckBoxCellType();
            TextCellType TypeText = new TextCellType();
            ComboBoxCellType TypeComBo = new ComboBoxCellType();

            SQL = SQL + ComNum.VBLF + "  SELECT A.FORMNO, A.FORMNAME1 FORMNAME,  ";
            SQL = SQL + ComNum.VBLF + "      B.ITEMNO, B.ITEMNAME, B.ITEMTYPE, B.ITEMHALIGN, B.ITEMVALIGN,";
            SQL = SQL + ComNum.VBLF + "      B.ITEMHEIGHT, B.ITEMWIDTH, B.MULTILINE, B.USEMACRO, B.CONTROLID, B.ITEMRMK, B.TAGHEAD, B.TAGTAIL";
            SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_EMR.EMRFORM A INNER JOIN KOSMOS_EMR.EMROPTFORM B";
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
                ssUserChart_Sheet1.RowCount = lngStartRow + dt.Rows.Count;

                ssUserChartHis_Sheet1.RowCount = lngStartRow + 2 + dt.Rows.Count + 1; //사용자 추가
                ssUserChartHis_Sheet1.Rows[0, 13].Visible = false;
                ssUserChartHis_Sheet1.Rows[14].Label = "작성일자";
                ssUserChartHis_Sheet1.Rows[15].Label = "작성시간";
                ssUserChartHis_Sheet1.Rows[0, ssUserChartHis_Sheet1.RowCount - 1].Locked = false;
                ssUserChartHis_Sheet1.Rows[ssUserChartHis_Sheet1.RowCount - 1].Label = "작성자";

                ssFORMXML_Sheet1.RowCount = lngStartRow + dt.Rows.Count;
                ssFORMXML_Sheet1.ColumnCount = 2;

                ssItem_Sheet1.RowCount = lngStartRow + dt.Rows.Count;
                ssItem_Sheet1.ColumnCount = 1;

                //lngStartCol -= 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region ssUserChart
                    ssUserChart_Sheet1.Rows[lngStartRow + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChart_Sheet1.Rows[lngStartRow + i].Height = (int)VB.Val(dt.Rows[i]["ITEMHEIGHT"].ToString().Trim()) + 10;

                    ssUserChart_Sheet1.Columns[0].Width = 200;


                    if (ssUserChart_Sheet1.ColumnCount == 0)
                        continue;


                    switch (dt.Rows[i]["ITEMTYPE"].ToString().Trim().ToUpper())
                    {
                        case "EDIT":
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim() == "1" ? true : false;
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].CellType = TypeText;
                            break;
                        case "COMBO":
                            clsSpread.gSpreadComboDataSetEx(ssUserChart, lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1,
                                dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'));
                            break;
                        case "CHECK":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim().ToUpper())
                    {
                        case "LEFT":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim().ToUpper())
                    {
                        case "TOP":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    if (dt.Rows[i]["USEMACRO"].ToString().Trim() == "1" && dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0)
                    {
                        ssUserChart_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChart_Sheet1.ColumnCount - 1].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                    }
                    #endregion

                    #region ssUserChartHis
                    ssUserChartHis_Sheet1.Rows[lngStartRow + 2 + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChartHis_Sheet1.Rows[lngStartRow + 2 + i].Height = (int)VB.Val(dt.Rows[i]["ITEMHEIGHT"].ToString().Trim()) + 10;

                    ssUserChartHis_Sheet1.Columns[0].Width = 200;

                    if (ssUserChartHis_Sheet1.ColumnCount == 0)
                        continue;


                    switch (dt.Rows[i]["ITEMTYPE"].ToString().Trim().ToUpper())
                    {
                        case "EDIT":
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim() == "1" ? true : false;
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].CellType = TypeText;
                            break;
                        case "COMBO":
                            clsSpread.gSpreadComboDataSetEx(ssUserChartHis, lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1,
                                dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'));
                            break;
                        case "CHECK":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim().ToUpper())
                    {
                        case "LEFT":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim().ToUpper())
                    {
                        case "TOP":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    if (dt.Rows[i]["USEMACRO"].ToString().Trim() == "1" && dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0)
                    {
                        ssUserChartHis_Sheet1.Cells[lngStartRow + i, 0, lngStartRow + i, ssUserChartHis_Sheet1.ColumnCount - 1].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                    }
                    #endregion

                    ssFORMXML_Sheet1.Cells[lngStartRow + i, 0].Text = dt.Rows[i]["TAGHEAD"].ToString().Trim();
                    ssFORMXML_Sheet1.Cells[lngStartRow + i, 1].Text = dt.Rows[i]["TAGTAIL"].ToString().Trim();

                    ssItem_Sheet1.Rows[lngStartRow + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssItem_Sheet1.Cells[lngStartRow + i, 0].Text = dt.Rows[i]["CONTROLID"].ToString().Trim();
                }


                switch(mstrFormNo)
                {
                    case "1725":
                        ssUserChart_Sheet1.Cells[25, 0, 34, ssUserChart_Sheet1.ColumnCount - 1].BackColor =
                            ColorTranslator.FromOle((int) new Int32Converter().ConvertFromString("0xBFFFEF"));
                        break;
                    case "1573":
                        ssUserChart_Sheet1.Cells[26, 0, 35, ssUserChart_Sheet1.ColumnCount - 1].BackColor =
                            ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("0xBFFFEF"));
                        break;
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
            DesignFunc.DefaultDuty(ssUserChart, 0, 14);
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

            objSpread.ActiveSheet.ColumnCount = 0;

            string SQL = string.Empty;
            string strOldDate = string.Empty;
            Color dblColorInfo = Color.Empty;
            DataTable dt = null;

            try
            {
                SQL = "SELECT A.EMRNO,";
                SQL += ComNum.VBLF + "    CHARTXML, ";
                SQL += ComNum.VBLF + "    0 ACPNO, A.INOUTCLS, A.CHARTDATE, A.CHARTTIME, ";
                SQL += ComNum.VBLF + "    A.MEDDEPTCD, A.MEDDRCD, A.USEID,U.NAME USENAME,";
                SQL += ComNum.VBLF + "    A.MEDFRDATE, A.MEDFRTIME,  B.FORMNO, B.FORMNAME1 FORMNAME,  B.USERFORMNO";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML A, KOSMOS_EMR.EMRFORM B, KOSMOS_EMR.EMRXMLMST D, KOSMOS_EMR.EMR_USERT U ";
                SQL += ComNum.VBLF + "  WHERE D.FORMNO = " + strFormNo;
                SQL += ComNum.VBLF + "    AND D.PTNO = '" + strPtno + "' ";
                SQL += ComNum.VBLF + "    AND D.CHARTDATE >= '" + strSDATE + "' ";
                SQL += ComNum.VBLF + "    AND D.CHARTDATE <= '" + strEDATE + "' ";
                SQL += ComNum.VBLF + "    AND A.FORMNO = B.FORMNO ";
                SQL += ComNum.VBLF + "    AND A.USEID = U.USERID ";
                SQL += ComNum.VBLF + "    AND A.EMRNO = D.EMRNO ";
                SQL += ComNum.VBLF + "  ORDER BY A.CHARTDATE DESC, TRIM(A.CHARTTIME) DESC";

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

                objSpread.ActiveSheet.ColumnCount = dt.Rows.Count;                
                objSpread.ActiveSheet.Columns[0].Border = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
                if(objSpread.ActiveSheet.ColumnCount > 1)
                {
                    objSpread.ActiveSheet.Columns[1, objSpread.ActiveSheet.ColumnCount - 1].Border = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
                }

                objSpread.ActiveSheet.Columns[0, objSpread.ActiveSheet.ColumnCount - 1].Width = 150;


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if(strOldDate != dt.Rows[i]["CHARTDATE"].ToString().Trim())
                    {
                        dblColorInfo = dblColorInfo == Color.White ? ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("0xBFFFEF")) : Color.White;
                        strOldDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    }

                    objSpread.ActiveSheet.Columns[i].BackColor = dblColorInfo;


                    objSpread.ActiveSheet.Cells[11, i].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[12, i].Text = dt.Rows[i]["USEID"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[13, i].Text = "";
                    objSpread.ActiveSheet.Cells[14, i].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                    objSpread.ActiveSheet.Cells[15, i].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");

                    #region XML세팅
                    XmlDocument xml = new XmlDocument(); // XmlDocument 생성
                    xml.LoadXml(dt.Rows[i]["CHARTXML"].ToString().Trim());
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
                    objSpread.ActiveSheet.Cells[objSpread.ActiveSheet.RowCount - 1, i].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                }

                dt.Dispose();


                switch (mstrFormNo)
                {
                    case "1725":
                        objSpread.ActiveSheet.Cells[27, 0, 36, objSpread.ActiveSheet.ColumnCount - 1].BackColor =
                            ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("0xBFFFEF"));
                        break;
                    case "1573":
                        objSpread.ActiveSheet.Cells[34, 0, 43, objSpread.ActiveSheet.ColumnCount - 1].BackColor =
                            ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("0xBFFFEF"));
                        break;
                }

                objSpread.ActiveSheet.Rows[objSpread.ActiveSheet.Rows.Count - 1].Label = "Sign";
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// XML 해당 아이템 찾아서 값 넣어주는 함수
        /// 
        /// </summary>
        /// <param name="a_objElement">XMLNODE </param>
        /// <param name="objSpread">스프레드 객체</param>
        /// <param name="lngColX">칼럼</param>
        void SetUserXmlValue(XmlNode a_objElement, FarPoint.Win.Spread.FpSpread objSpread, int lngColX)
        {
            string strItem = a_objElement.Name;
            string strValue = a_objElement.InnerText;
            int lngItemRow = 0;
            TextCellType textCellType = new TextCellType();

            for (int lngRow = 0; lngRow < ssItem_Sheet1.RowCount; lngRow++)
            {
                if(strItem == ssItem_Sheet1.Cells[14 + lngRow, 0].Text.Trim())
                {
                    lngItemRow = 14 + lngRow;
                    break;
                }
            }

            if (lngItemRow == 0)
                return;

            objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].HorizontalAlignment = ssUserChart.ActiveSheet.Cells[lngItemRow, 0].HorizontalAlignment;
            objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].VerticalAlignment   = ssUserChart.ActiveSheet.Cells[lngItemRow, 0].VerticalAlignment;
            objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType = ssUserChart.ActiveSheet.Cells[lngItemRow, 0].CellType;


            if (objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType != null &&
                objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType.ToString() == "TextCellType")
            {
                textCellType.Multiline = true;
                objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType = textCellType;
            }
            else if (objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType != null &&
                     objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType.ToString() != "TextCellType")
            {
                textCellType.Multiline = false;
                objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].CellType = textCellType;
            }

            objSpread.ActiveSheet.Cells[lngItemRow + 2, lngColX].Text = strValue;

            textCellType.Dispose();

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PrintCopy("V");
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
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strPatName = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strAge = ComFunc.AgeCalc(clsDB.DbCon,
                        dt.Rows[0]["JUMIN1"].ToString().Trim() +
                        dt.Rows[0]["JUMIN2"].ToString().Trim()).ToString();
                    strJumin = string.Format("{0}-{1}******",
                       dt.Rows[0]["JUMIN1"].ToString().Trim(),
                       VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1));

                }

                dt.Dispose();

                string strFormName = string.Empty;
                SQL = " SELECT FORMNAME ";
                SQL = SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRFORM ";
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
                string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(10) + "출력일자 : " + ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) + VB.Space(10) + "출력자 : " + clsType.User.UserName;
                strFooter = strFooter + "/n/l/f2" + "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.";
                strFooter = strFooter + "/n/l/f2" + "This is an electronically authorized offical medical record";

                ssUserChartHis_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssUserChartHis_Sheet1.PrintInfo.Footer = strFooter;
                ssUserChartHis_Sheet1.PrintInfo.Margin.Left = 20;
                ssUserChartHis_Sheet1.PrintInfo.Margin.Right = 20;
                ssUserChartHis_Sheet1.PrintInfo.Margin.Top = 20;
                ssUserChartHis_Sheet1.PrintInfo.Margin.Bottom = 20;

                ssUserChartHis_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssUserChartHis_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssUserChartHis_Sheet1.PrintInfo.ShowBorder = true;
                ssUserChartHis_Sheet1.PrintInfo.ShowColor = false;
                ssUserChartHis_Sheet1.PrintInfo.ShowGrid = true;
                ssUserChartHis_Sheet1.PrintInfo.ShowShadows = false;
                ssUserChartHis_Sheet1.PrintInfo.UseMax = false;
                ssUserChartHis_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssUserChartHis_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;

                SQL = " SELECT PRTGB ";
                SQL = SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRFORM ";
                SQL = SQL += ComNum.VBLF + "WHERE FORMNO = " + VB.Val(mstrFormNo);

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    switch (reader.GetValue(0).ToString().Trim())
                    {
                        case "L":
                            ssUserChartHis_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                            break;
                        case "P":
                            ssUserChartHis_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                            break;
                        case "S":
                            ssUserChartHis_Sheet1.PrintInfo.UseSmartPrint = true;
                            break;
                    }
                }
                reader.Dispose();


                if (PrintType == "V")
                {
                    ssUserChartHis_Sheet1.PrintInfo.Preview = true;
                }

                ssUserChartHis.PrintSheet(0);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void SsUserChartHisIO_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssUserChartHis_Sheet1.RowCount == 0)
                return;

            string strEmrNo = ssUserChartHis_Sheet1.Cells[11, e.Column].Text.Trim();
            if (strEmrNo.Length == 0)
                return;
            
            mstrEmrNo = strEmrNo;

            mEmrUseId = ssUserChartHis_Sheet1.Cells[12, e.Column].Text.Trim();

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

            string strCHARTDATE = ssUserChartHis_Sheet1.Cells[14, e.Column].Text.Trim();
            string strCHARTTIME = ssUserChartHis_Sheet1.Cells[15, e.Column].Text.Trim();
            string strDuty = ssUserChartHis_Sheet1.Cells[1, e.Column].Text.Trim();

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(strCHARTDATE);
            usFormTopMenuEvent.dtMedFrDate.Enabled = false;
            usFormTopMenuEvent.txtMedFrTime.Text = strCHARTTIME;

            for(int lngRow = 16; lngRow < ssUserChartHis_Sheet1.RowCount - 2; lngRow++)
            {
                ssUserChart_Sheet1.Cells[lngRow - 2, 0].Text = ssUserChartHis_Sheet1.Cells[lngRow, e.Column].Text.Trim();
            }

        }

        void ClearSp()
        {
            for (int i = 3; i < ssUserChart_Sheet1.RowCount; i++)
            {
                for (int j = 3; j < ssUserChart_Sheet1.ColumnCount; j++)
                {
                    ssUserChart_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }

            DesignFunc.DefaultDuty(ssUserChart, 0, 14);

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Text = VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)).ToString("00:00");
            mstrEmrNo = "0";
            mEmrUseId = string.Empty;
        }

        private void BtnSearchPreHis_Click(object sender, EventArgs e)
        {
            mstrEmrNo = string.Empty;
            mEmrUseId = string.Empty;

            if (ssUserChartHis_Sheet1.ColumnCount <= 0)
                return;

            ssUserChartHis_Sheet1.Cells[0, 0, ssUserChartHis_Sheet1.RowCount - 1, ssUserChartHis_Sheet1.ColumnCount - 1].BackColor = ColorTranslator.FromWin32(Information.QBColor(15));
            ssUserChartHis_Sheet1.Cells[0, ssUserChartHis_Sheet1.ActiveColumnIndex, ssUserChartHis_Sheet1.RowCount - 1, ssUserChartHis_Sheet1.ActiveColumnIndex].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("0xBFFFEF"));

            if (ssUserChartHis_Sheet1.Cells[11, ssUserChartHis_Sheet1.ActiveColumnIndex].Text.Trim().Length == 0)
                return;

            for(int lngRow = 14; lngRow < ssUserChartHis_Sheet1.RowCount - 1; lngRow++)
            {
                ssUserChart_Sheet1.Cells[lngRow - 2, 0].Text = ssUserChartHis_Sheet1.Cells[lngRow, ssUserChartHis_Sheet1.ActiveColumnIndex].Text.Trim();
            }

        }

        private void FrmTextEmr_NrActFall_Resize(object sender, EventArgs e)
        {
            ssUserChart.Height = Height - panTopMenu.Height - 100;

            ssUserChartHis.Left = usFormTopMenuEvent.Visible == false ? 30 : 582;

            ssUserChartHis.Width  = Width - ssUserChartHis.Left - 7;
            ssUserChartHis.Height = Height - panTopMenu.Height - ssUserChartHis.Top - 5;

        }

        private void ssUserChart_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0)
                return;

            if (e.Row == 20 || e.Row == 30 || e.Row == 40)
            {
            }
            else
            {
                return;
            }

            int intV1 = 0;
            if (e.Row == 20)
            {
                intV1 = 0;
            }
            else if (e.Row == 30)
            {
                intV1 = 10;
            }
            else if (e.Row == 40)
            {
                intV1 = 20;
            }

            double lngValue = 0;
            if (!string.IsNullOrWhiteSpace(ssUserChart_Sheet1.Cells[18 + intV1, 0].Text.Trim()))
            {
                ssUserChart_Sheet1.Cells[intV1 + 18, 0].Text = ssUserChart_Sheet1.Cells[intV1 + 18, 0].Text.Replace("x", "*");
                ssUserChart_Sheet1.Cells[intV1 + 18, 0].Text = ssUserChart_Sheet1.Cells[intV1 + 18, 0].Text.Replace("X", "*");
                string[] valValue = ssUserChart_Sheet1.Cells[e.Row, 0].Text.Split('*');

                StringBuilder strTEMP = new StringBuilder();
                if (VB.IsNumeric(valValue[0]) == false)
                {
                    for (int i = 0; i < valValue[0].Length; i++)
                    {
                        strTEMP.Append(VB.IsNumeric(valValue[0].Substring(i, 1)) ? valValue[0].Substring(i, 1) : "");
                    }

                    valValue[0] = strTEMP.ToString().Trim();
                }



                strTEMP.Clear();
                if (valValue.Length > 1)
                {
                    if (VB.IsNumeric(valValue[1]) == false)
                    {
                        for (int i = 0; i < valValue[1].Length; i++)
                        {
                            strTEMP.Append(VB.IsNumeric(valValue[1].Substring(i, 1)) ? valValue[1].Substring(i, 1) : "");
                        }

                        valValue[1] = strTEMP.ToString().Trim();
                    }
                }

                lngValue = valValue.Length > 1 ? VB.Val(valValue[0]) * VB.Val(valValue[1]) : VB.Val(valValue[0]);
            }

            if (lngValue > 24)
                lngValue = 10;
            else if (lngValue <= 24 && lngValue >= 12.1)
                lngValue = 9;
            else if (lngValue <= 12 && lngValue >= 8.1)
                lngValue = 8;
            else if (lngValue <= 8 && lngValue >= 4.1)
                lngValue = 7;
            else if (lngValue <= 4 && lngValue >= 3.1)
                lngValue = 6;
            else if (lngValue <= 3 && lngValue >= 2.1)
                lngValue = 5;
            else if (lngValue <= 2 && lngValue >= 1.1)
                lngValue = 4;
            else if (lngValue <= 1 && lngValue >= 0.7)
                lngValue = 3;
            else if (lngValue <= 0.6 && lngValue >= 0.3)
                lngValue = 2;
            else if (lngValue < 0.3 && lngValue > 0)
                lngValue = 1;
            else if (lngValue == 0)
                lngValue = 0;


            switch (ssUserChart_Sheet1.Cells[19 + intV1, 0].Text.Trim())
            {
                case "가피":
                    lngValue += 4;
                    break;
                case "부육":
                    lngValue += 3;
                    break;
                case "육아조직":
                    lngValue += 2;
                    break;
                case "상피세포":
                    lngValue += 1;
                    break;
            }

            switch (ssUserChart_Sheet1.Cells[20 + intV1, 0].Text.Trim())
            {
                case "소량":
                    lngValue += 1;
                    break;
                case "중정도":
                    lngValue += 2;
                    break;
                case "다량":
                    lngValue += 3;
                    break;
            }

            ssUserChart_Sheet1.Cells[25 + intV1, 0].Text = lngValue.ToString();
        }
    }
}
