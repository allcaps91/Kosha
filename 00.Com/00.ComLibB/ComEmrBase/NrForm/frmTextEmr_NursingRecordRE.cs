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
    /// 회복실 간호기록지등록
    /// FORMNO = 2137
    /// \mtsEmrf\frmTextEmr_NursingRecordRE.frm
    /// </summary>
    public partial class frmTextEmr_NursingRecordRE : Form, EmrChartForm
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

        /// <summary>
        /// 차트작성 일자.
        /// </summary>
        string mstrOrderDate = string.Empty;

        /// <summary>
        /// VB => txtEmrUseId => 작성자 아이디
        /// </summary>
        string mEmrUseId = string.Empty;

        /// <summary>
        /// 문제목록 변수
        /// </summary>
        string mOption = string.Empty;

        /// <summary>
        /// 문제목록 변수2
        /// </summary>
        string mMACROGB = string.Empty;

        /// <summary>
        /// 문제목록 항목 인덱스
        /// </summary>
        double mlngMACROINDEX = 0;

        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;

        public string mstrFormNo = "2137";
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
            if(pDelData())
            {
                pClearForm();
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

        /// <summary>
        /// 작성일자 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            throw new NotImplementedException();
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
            ClearChart();
        }

        // <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;

            if (string.IsNullOrWhiteSpace(pAcp.ptNo))
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return VB.Val(mstrEmrNo);
            }

            //if (ChkTime.Checked)
            //{
            //    if (string.IsNullOrWhiteSpace(usFormTopMenuEvent.txtMedFrTime.Text.Trim()))
            //    {
            //        ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
            //        return VB.Val(mstrEmrNo);
            //    }

            //    if (string.IsNullOrWhiteSpace(txtNa.Text.Trim()))
            //    {
            //        ComFunc.MsgBoxEx(this, "DATA 설정해 주십시오.");
            //        return VB.Val(mstrEmrNo);
            //    }
            //}
            //else
            //{
            //    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "yyyyMMdd", null);
            //    usFormTopMenuEvent.txtMedFrTime.Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");
            //}

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
                DateTime dtpChartDate = DateTime.ParseExact(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", ""), 4), "yyyyMMdd HHmm", null);
                DateTime dtpSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                if (dtpChartDate > dtpSysDate)
                {
                    ComFunc.MsgBoxEx(this, "현재 시간 이후로 작성할 수 없습니다.");
                    return VB.Val(mstrEmrNo);
                }

                if (pAcp.medEndDate != "")
                {
                    if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(pAcp.medEndDate) < VB.Val(strdtpDate)))
                    {
                        ComFunc.MsgBoxEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다");
                        return VB.Val(mstrEmrNo);
                    }
                }
                else
                {
                    if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(strCurDate) < VB.Val(strdtpDate)))
                    {
                        ComFunc.MsgBoxEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다");
                        return VB.Val(mstrEmrNo);
                    }
                }
            }
            #endregion

            if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            string strDate = usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString() + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2) + ":" + VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2);
            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return VB.Val(mstrEmrNo);
            }

            if (string.IsNullOrWhiteSpace(txtNa.Text.Trim()) == false && SaveProgress())
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");

                usFormTopMenuEvent.dtMedFrDate.Enabled = true;
                usFormTopMenuEvent.txtMedFrTime.Enabled = true;
                mstrEmrNo = "0";
                txtNa.Clear();
                lblN.Text = "신규등록";
                GetChartHistory();
            }

            return dblEmrNo;
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = false;

            if (VB.Val(mstrEmrNo) == 0)
            {
                return rtnVal;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (mEmrUseId.Trim() != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    return rtnVal;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                {
                    return rtnVal;
                }

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                {
                    return rtnVal;
                }

                if (ComFunc.MsgBoxQEx(this, "기존내용을 삭제하시겠습니까?", "삭제", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return rtnVal;
                }
            }

            rtnVal = DeleteProgress();

            return rtnVal;
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void pPrintForm()
        {
            using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
            {
                frmEmrPrintOptionX.ShowDialog();

                if (clsFormPrint.mstrPRINTFLAG == "-1")
                {
                    return;
                }

                //PrintCopy("V");
            }
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
        public frmTextEmr_NursingRecordRE()
        {
            InitializeComponent();
        }

        public frmTextEmr_NursingRecordRE(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
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
        public frmTextEmr_NursingRecordRE(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        public frmTextEmr_NursingRecordRE(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        private void frmTextEmr_NursingRecordRE_Load(object sender, EventArgs e)
        {
            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            usFormTopMenuEvent.mbtnClear.Visible = false;
            usFormTopMenuEvent.mbtnPrint.Visible = false;
            usFormTopMenuEvent.mbtnSave.Visible = false;
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;

            SetWardList();

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");

            dtpOptSDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3);
            dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            cboBuse.Items.Add("");
            cboBuse.Items.Add("혈관조영실" + VB.Space(50) + "100570");
            cboBuse.Items.Add("인공신장실" + VB.Space(50) + "033108");

            rdoDept.Checked = true;

            pClearForm();

            MakeUserChart(mstrFormNo, 14);

            GetChartHistory();
        }

        #region MakeUserChart
        void MakeUserChart(string strFormNo, int lngStartCol)
        {
            ssUserChartHis_Sheet1.ColumnCount = lngStartCol + 2;
            ssUserChartHis_Sheet1.RowCount = 0;
            ssUserChartHis_Sheet1.RowCount = 1;

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
                ssUserChartHis_Sheet1.ColumnCount = lngStartCol + 2 + dt.Rows.Count + 1; //사용자 추가
                ssUserChartHis_Sheet1.Columns[ssUserChartHis_Sheet1.ColumnCount - 1].Label = "작성자";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].Width = dt.Rows[i]["ITEMNAME"].ToString().Trim().Equals("간호기록") ? 300 : 40;

                    if (ssUserChartHis_Sheet1.RowCount == 0)
                        continue;


                    switch (dt.Rows[i]["ITEMTYPE"].ToString().Trim().ToUpper())
                    {
                        case "EDIT":
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim().Equals("1");
                            TypeText.MaxLength = 320000; 
                            TypeText.WordWrap = TypeText.Multiline;
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].CellType = TypeText;
                            break;
                        case "COMBO":
                            clsSpread.gSpreadComboDataSetEx(ssUserChartHis, 0, lngStartCol + i + 2, ssUserChartHis_Sheet1.RowCount - 1, lngStartCol + i + 2,
                                dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'));
                            break;
                        case "CHECK":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim().ToUpper())
                    {
                        case "LEFT":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim().ToUpper())
                    {
                        case "TOP":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    if (dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0)
                    {
                        ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                        ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].Visible = true;
                    }

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
        #endregion

        #region Clear
        void ClearChart()
        {
            mstrEmrNo = "0";
            mEmrUseId = string.Empty;

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");

            txtNa.Clear();
            txtRoomR.Clear();
            lblN.Text = "신규등록";
            txtWardR.Text = pAcp.ward;
            txtRoomR.Text = pAcp.room;
        }

        #endregion

        #region 트리뷰
        void SetWardList()
        {
            cboWard.Items.Clear();

            OracleDataReader dataReader = null;

            string SQL = " SELECT A.MACROGB, MAX(B.NAME) AS NAME ";
            SQL += ComNum.VBLF + "    FROM ADMIN.EMRMACROETC A ";
            SQL += ComNum.VBLF + "    INNER JOIN ADMIN.EMR_CLINICT B ";
            SQL += ComNum.VBLF + "    ON A.MACROGB = B.CLINCODE ";
            SQL += ComNum.VBLF + "    GROUP BY A.MACROGB ";


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
                    cboWard.Items.Add(dataReader.GetValue(1).ToString().Trim() + VB.Space(30) + dataReader.GetValue(0).ToString().Trim());
                }
            }

            dataReader.Dispose();


            for(int i = 0; i < cboWard.Items.Count; i++)
            {
                if (clsType.User.BuseCode.Equals(cboWard.Items[i].ToString().Substring(cboWard.Items[i].ToString().LastIndexOf(' ')).Trim()))
                {
                    cboWard.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion

        #region GetChartHistory
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

            //기록지 정보를 세팅한다.
            SetFormInfo();

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

            ////사본발급 출력여부
            //usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(mstrEmrNo);
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
            #endregion


            GetUchartHis(pAcp.ptNo, mstrFormNo, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                ssUserChartHis);
        }

        private void SetFormInfo()
        {
            pForm = null;
            pForm = clsEmrChart.ClearEmrForm();
            pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo, mstrUpdateNo);
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        /// <param name="strPtno">등록번호</param>
        /// <param name="strFormNo">폼번호</param>
        /// <param name="strSDATE">시작일자</param>
        /// <param name="strEDATE">종료일자</param>
        /// <param name="objSpread">스프레드</param>
        void GetUchartHis(string strPtno, string strFormNo, string strSDATE, string strEDATE, FarPoint.Win.Spread.FpSpread objSpread)
        {
            if (mstrFormNo == "")
                return;

            if (pAcp.ptNo == "")
                return;

            objSpread.ActiveSheet.RowCount = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            //READ_INIT_FALL();

            try
            {
                //SQL = " SELECT A.EMRNO, "                                                                  ;
                //SQL += ComNum.VBLF + "    xmltype.getstringval(extract(CHARTXML,'/chart')) AS CHARTA,"    ;
                //SQL += ComNum.VBLF + "    A.ACPNO, A.INOUTCLS, A.CHARTDATE, A.CHARTTIME, "                ;
                //SQL += ComNum.VBLF + "    A.MEDDEPTCD, A.MEDDRCD, A.USEID,U.NAME USENAME,"                ;
                //SQL += ComNum.VBLF + "    A.MEDFRDATE, A.MEDFRTIME,  B.FORMNO, B.FORMNAME, B.USERFORMNO"  ;
                //SQL += ComNum.VBLF + "FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B "          ;
                //SQL += ComNum.VBLF + "      ON A.FORMNO = B.FORMNO "                                      ;
                //SQL += ComNum.VBLF + "    INNER JOIN ADMIN.BAS_PASS U"                              ;
                //SQL += ComNum.VBLF + "      ON A.USEID = U.IDNUMBER ";

                //if (cboBuse.Text.Length > 0)
                //{
                //    SQL += ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U2 ";
                //    SQL += ComNum.VBLF + "      ON A.USEID = U2.USERID";
                //    SQL += ComNum.VBLF + "   INNER JOIN ADMIN.BAS_BUSE E ";
                //    SQL += ComNum.VBLF + "      ON E.BUCODE = U2.BUSECODE ";
                //    SQL += ComNum.VBLF + "    AND E.BUCODE = " + VB.Right(cboBuse.Text.Trim(), 6);
                //}

                //SQL += ComNum.VBLF + "  WHERE EMRNO IN (SELECT EMRNO FROM ADMIN.EMRXMLMST WHERE FORMNO = " + strFormNo;
                //SQL += ComNum.VBLF + "      AND PTNO = '" + pAcp.ptNo + "' ";
                //SQL += ComNum.VBLF + "      AND CHARTDATE >= '" + strSDATE + "' ";
                //SQL += ComNum.VBLF + "      AND CHARTDATE <= '" + strEDATE + "') ";
                //SQL += ComNum.VBLF + "      AND U.PROGRAMID = '        ' ";
                //SQL += ComNum.VBLF + "  ORDER BY A.CHARTDATE DESC, TRIM(A.CHARTTIME) DESC, A.EMRNO ASC ";

                //string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                //if (sqlErr.Length > 0)
                //{
                //    ComFunc.MsgBoxEx(this, sqlErr);
                //    return;
                //}

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    return;
                //}

                //objSpread.ActiveSheet.RowCount = dt.Rows.Count;

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    objSpread.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                //    objSpread.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["USEID"].ToString().Trim();
                //    objSpread.ActiveSheet.Cells[i, 13].Text = "";
                //    objSpread.ActiveSheet.Cells[i, 14].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                //    objSpread.ActiveSheet.Cells[i, 15].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");

                //    #region XML세팅
                //    XmlDocument xml = new XmlDocument(); // XmlDocument 생성
                //    xml.LoadXml(LoadXml(dt.Rows[i]["EMRNO"].ToString().Trim()));
                //    #endregion

                //    #region SetUserXmlValue
                //    if (xml.DocumentElement != null)
                //    {
                //        foreach (XmlNode xn in xml.DocumentElement.ChildNodes)
                //        {
                //            SetUserXmlValue(xn, objSpread, i);
                //        }
                //    }
                //    #endregion
                //    objSpread.ActiveSheet.Cells[i, objSpread.ActiveSheet.ColumnCount - 1].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                //}

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     FORMNO, UPDATENO, FORMNAME, EMRNO, CHARTDATE, CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "     T.WARDCODE, ROOMCODE, PROBLEM, GUBUN, NURSERECODE, ";
                SQL = SQL + ComNum.VBLF + "     MEDDEPTCD, MEDDRCD, USEID, USENAME, ACPNO, INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "     MEDFRDATE, MEDFRTIME ";
                SQL = SQL + ComNum.VBLF + "FROM ";
                SQL = SQL + ComNum.VBLF + "( ";
                SQL = SQL + ComNum.VBLF + "SELECT B.FORMNO, B.UPDATENO, B.FORMNAME, A.EMRNO, ";
                SQL = SQL + ComNum.VBLF + "     A.CHARTDATE, A.CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta3') AS WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta4') AS ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta1') AS PROBLEM, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it1') AS GUBUN, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta2') AS NURSERECODE, ";
                SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.MEDDRCD, A.USEID, U.NAME USENAME, A.ACPNO, A.INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "     A.MEDFRDATE, A.MEDFRTIME ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U ";
                SQL = SQL + ComNum.VBLF + "      ON A.USEID = U.USERID ";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO IN( ";
                SQL = SQL + ComNum.VBLF + "    SELECT EMRNO FROM ADMIN.EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + "     WHERE FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + strSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + strEDATE + "') ";
                SQL = SQL + ComNum.VBLF + "UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT B.FORMNO, B.UPDATENO, B.FORMNAME, A.EMRNO, ";
                SQL = SQL + ComNum.VBLF + "     A.CHARTDATE, A.CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + "     N.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "     N.ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + "     N.PROBLEM, ";
                SQL = SQL + ComNum.VBLF + "     N.TYPE AS GUBUN, ";
                SQL = SQL + ComNum.VBLF + "     N.NRRECODE AS NURSERECODE, ";
                SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.MEDDRCD, A.CHARTUSEID USEID, U.NAME USENAME, A.ACPNO, A.INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "     A.MEDFRDATE, A.MEDFRTIME ";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST A INNER JOIN ADMIN.AEMRFORM B ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U ";
                SQL = SQL + ComNum.VBLF + "      ON A.CHARTUSEID = U.USERID ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.AEMRNURSRECORD N ";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO = N.EMRNO ";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = N.EMRNOHIS ";
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + strSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strEDATE + "' ";
                SQL = SQL + ComNum.VBLF + ") T ";
                if (cboBuse.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U2 ";
                    SQL = SQL + ComNum.VBLF + "      ON T.USEID = U2.USERID";
                    SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.BAS_BUSE E ";
                    SQL = SQL + ComNum.VBLF + "      ON E.BUCODE = U2.BUSECODE ";
                    SQL = SQL + ComNum.VBLF + "    AND E.BUCODE = " + VB.Right(cboBuse.Text, 6);
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY CHARTDATE DESC, TRIM(CHARTTIME) DESC, EMRNO ASC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
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


                string strChartDate = "";
                string strChartTime = "";

                objSpread.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strChartDate = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D");
                    strChartTime = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");

                    objSpread.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["USEID"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 13].Text = "";
                    objSpread.ActiveSheet.Cells[i, 14].Text = strChartDate;
                    objSpread.ActiveSheet.Cells[i, 15].Text = strChartTime;
                    objSpread.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["NURSERECODE"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 19].Text = dt.Rows[i]["USENAME"].ToString().Trim();

                    objSpread.ActiveSheet.SetRowHeight(i, Convert.ToInt32(objSpread.ActiveSheet.GetPreferredRowHeight(i)) + 16);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        #endregion

        #region XML 

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

                if (reader.HasRows && reader.Read())
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

            for (int lngCol = 0; lngCol < ssUserChartHis_Sheet1.ColumnCount; lngCol++)
            {
                if (ssUserChartHis_Sheet1.Columns[lngCol].Tag == null)
                    continue;

                if ( strItem == ssUserChartHis_Sheet1.Columns[lngCol].Tag.ToString().Trim())
                {
                    lngItemCol = lngCol;
                    break;
                }
            }

            if (lngItemCol == 0)
                return;

            //objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].HorizontalAlignment = ssUserChart.ActiveSheet.Cells[0, lngItemCol].HorizontalAlignment;
            //objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].VerticalAlignment = ssUserChart.ActiveSheet.Cells[0, lngItemCol].VerticalAlignment;
            //objSpread.ActiveSheet.Cells[lngRowX, lngItemCol + 2].CellType = ssUserChart.ActiveSheet.Cells[0, lngItemCol].CellType;

            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].Text = strValue;

            if (objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType != null &&
               objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType.ToString() == "TextCellType")
            {
                textCellType = (TextCellType)objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType;
                objSpread.ActiveSheet.Rows[lngRowX].Height = textCellType.Multiline == true ? objSpread.ActiveSheet.Rows[lngRowX].GetPreferredHeight() + 10 : 22;
            }
            else if(objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType == null)
            {
                textCellType.Multiline = true;
                textCellType.WordWrap = true;
                objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType = textCellType;
                objSpread.ActiveSheet.Rows[lngRowX].Height = objSpread.ActiveSheet.Rows[lngRowX].GetPreferredHeight() + 10;
            }

            textCellType.Dispose();

        }

        #endregion

        #region LoadTable
        void LoadTable()
        {
            trvMACROGrp.Nodes.Clear();

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL += ComNum.VBLF + "    SELECT A.MACROGB, A.MACROINDEX, A.MACROKEY, A.MACROPARENT, A.MACRONAME,";
                SQL += ComNum.VBLF + "          (SELECT MAX(O.MACROINDEX) FROM ADMIN.EMRMACROETCDTL O";
                SQL += ComNum.VBLF + "                  WHERE O.MACROGB = A.MACROGB AND O.MACROINDEX = A.MACROINDEX) AS DTLYN";
                SQL += ComNum.VBLF + "           FROM ADMIN.EMRMACROETC A";
                SQL += ComNum.VBLF + "          WHERE A.MACROGB = '" + mMACROGB +  "'";
                SQL += ComNum.VBLF + "          ORDER BY A.MACROPARENTV, A.SYSDSPINDEX";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if(dt.Rows[i]["MACROPARENT"].ToString().Trim().Equals("0_"))
                    {
                        trvMACROGrp.Nodes.Add(dt.Rows[i]["MACROKEY"].ToString().Trim(), dt.Rows[i]["MACRONAME"].ToString().Trim());
                    }
                    else
                    {
                        trvMACROGrp.Nodes.Find(dt.Rows[i]["MACROPARENT"].ToString().Trim(), true)[0]?.Nodes.Add(dt.Rows[i]["MACROKEY"].ToString().Trim(), dt.Rows[i]["MACRONAME"].ToString().Trim());
                    }


                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        #endregion

        #region 노드 클릭시
        private void trvMACROGrp_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ssNara_Sheet1.RowCount = 0;

            if (e.Node.Nodes.Count > 0)
                return;

            mlngMACROINDEX = VB.Val(ComFunc.SptChar(e.Node.Name, 1, "_"));

            txtProbR.Text = e.Node.Text.Trim();

            GetDtlInfo();
        }

        void GetDtlInfo()
        {
            ssNara_Sheet1.RowCount = 0;

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ";
                SQL += ComNum.VBLF + "  A.MACROCD, ";
                SQL += ComNum.VBLF + "  A.MACROSEQ, A.MACROTEXT, A.MACRODSP";
                SQL += ComNum.VBLF + " FROM ADMIN.EMRMACROETCDTL A";
                SQL += ComNum.VBLF + " WHERE A.MACROGB = '" + mMACROGB.Trim() + "'";
                SQL += ComNum.VBLF + "   AND A.MACROINDEX = " + mlngMACROINDEX;
                SQL += ComNum.VBLF + "   AND A.MACROCD = 'NARA'";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }


                ssNara_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssNara_Sheet1.Cells[i, 0].Text = "True";
                    ssNara_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MACROTEXT"].ToString().Trim();
                    ssNara_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MACRODSP"].ToString().Trim();
                    ssNara_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MACROCD"].ToString().Trim();
                    ssNara_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MACROSEQ"].ToString().Trim();

                    ssNara_Sheet1.Rows[i].Height = ssNara_Sheet1.Rows[i].GetPreferredHeight() + 10;
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
        #endregion

        #region 삭제
        bool DeleteProgress()
        {
            bool rtnVal = false;
            
            Cursor.Current = Cursors.WaitCursor;

            if (IsOldChartFormByEmrNo(mstrEmrNo) == true)
            {
                rtnVal = clsEmrQuery.DeleteEmrXmlData(mstrEmrNo);
            }
            else
            {
                rtnVal = clsEmrQuery.DeleteNurseRecord(clsDB.DbCon, mstrEmrNo);
            }

            Cursor.Current = Cursors.Default;

            return rtnVal;
        }

        #endregion

        void ClearForm()
        {
            mstrEmrNo = "0";
            mEmrUseId = "";

            txtNa.Clear();
            txtRoomR.Clear();
            lblN.Text = "신규등록";
            txtWardR.Text = "ER";
        }

        private void btnMcrReg_Click(object sender, EventArgs e)
        {
            clsEmrPublic.GstrNurCodeDAR = "";
            clsEmrPublic.GstrNurData.Clear();
            clsEmrPublic.GstrNurAction.Clear();
            clsEmrPublic.GstrNurResult.Clear();

            using (frmNursingRecordJindanOld frm = new frmNursingRecordJindanOld())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        #region result 버튼
        /// <summary>
        /// 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAplayR_Click(object sender, EventArgs e)
        {
            StringBuilder strString = new StringBuilder();
            for(int lngRow = 0; lngRow < ssNara_Sheet1.RowCount; lngRow++)
            {
                if(ssNara_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True"))
                {
                    strString.AppendLine(ssNara_Sheet1.Cells[lngRow, 1].Text.Trim());
                }
            }

            if(strString.Length > 0)
            {
                txtNa.Clear();
                txtNa.Text = strString.ToString();
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelR_Click(object sender, EventArgs e)
        {
            pDelData();
        }

        /// <summary>
        /// 새로입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearR_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQEx(this, "새로 입력 하시겠습니까?") == DialogResult.No)
                return;

            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            mstrEmrNo = "0";
            txtNa.Clear();
            lblN.Text = "신규등록";
            usFormTopMenuEvent.mbtnSave.Visible = true;
        }

        #endregion

        #region 좌측 하단 - 히스토리
        private void btnSearchHis_Click(object sender, EventArgs e)
        {
            GetChartHistory();
        }
        #endregion

        #region 라디오 이벤트
        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as RadioButton).Checked)
            {
                if (sender.Equals(rdoAll))
                {
                    mOption = "H";
                    mMACROGB = "WARD";
                }
                else if (sender.Equals(rdoDept))
                {
                    panWard.Visible = true;
                    mOption = "D";
                    mMACROGB = cboWard.Items.Count == 0 ? clsType.User.BuseCode : VB.Right(cboWard.Text.Trim(), 10).Trim();
                }
                else if (sender.Equals(rdoUse))
                {
                    panWard.Visible = false;
                    switch(clsType.User.Sabun)
                    {
                        case "14472":
                        case "16047":
                        case "22901":
                        case "28727":
                        case "28754":
                            mMACROGB = "21987";
                            break;
                        case "15317":
                        case "13662":
                            mMACROGB = "15317";
                            break;
                        default:
                            mMACROGB = clsType.User.IdNumber;
                            break;
                    }
                }
                LoadTable();
            }
        }

        #endregion
       
        private void ssUserChartHis_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblN.Text = "변경작업";

            #region 수정권한
            if(READ_Modify_Cert(ssUserChartHis, 12, e.Row, clsType.User.IdNumber))
            {
                usFormTopMenuEvent.mbtnSave.Visible = true;
                usFormTopMenuEvent.mbtnDelete.Visible = true;
            }
            else
            {
                usFormTopMenuEvent.mbtnSave.Visible = false;
                usFormTopMenuEvent.mbtnDelete.Visible = false;
            }
            #endregion


            if (string.IsNullOrWhiteSpace(ssUserChartHis_Sheet1.Cells[e.Row, 11].Text.Trim()))
            {
                return;
            }

            mstrEmrNo = ssUserChartHis_Sheet1.Cells[e.Row, 11].Text.Trim();
            mEmrUseId = ssUserChartHis_Sheet1.Cells[e.Row, 12].Text.Trim();
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssUserChartHis_Sheet1.Cells[e.Row, 14].Text.Trim());
            usFormTopMenuEvent.dtMedFrDate.Enabled = false;
            usFormTopMenuEvent.txtMedFrTime.Text = ssUserChartHis_Sheet1.Cells[e.Row, 15].Text.Trim();
            txtProbR.Clear();

            txtNa.Text = ssUserChartHis_Sheet1.Cells[e.Row, 18].Text.Trim().Replace("\n", ComNum.VBLF);
        }

        bool READ_Modify_Cert(FarPoint.Win.Spread.FpSpread spd, int ArgCol, int argROW, string argUserID)
        {
            return spd.ActiveSheet.Cells[argROW, ArgCol].Text.Trim().Equals(argUserID);
        }

        #region 기록지 저장 버튼
        private void btnSaveR_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(pAcp.ptNo))
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return;
            }

            if(ChkTime.Checked)
            {
                if(string.IsNullOrWhiteSpace(usFormTopMenuEvent.txtMedFrTime.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNa.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "DATA 설정해 주십시오.");
                    return;
                }
            }
            else
            {
                usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "yyyyMMdd", null);
                usFormTopMenuEvent.txtMedFrTime.Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");
            }


            string strDate = usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString() + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2) + ":" + VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2);
            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNa.Text.Trim()) == false && SaveProgress())
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");

                usFormTopMenuEvent.dtMedFrDate.Enabled = true;
                usFormTopMenuEvent.txtMedFrTime.Enabled = true;
                mstrEmrNo = "0";
                txtNa.Clear();
                lblN.Text = "신규등록";
                GetChartHistory();
                return;
            }
        }

        bool SaveProgress()
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
                    if (ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?") == DialogResult.No)
                    {
                        return rtnVal;
                    }

                    if (clsType.User.IdNumber != mEmrUseId)
                    {
                        ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                        return rtnVal;
                    }

                    // 차트백업 및 삭제
                    if (DeleteProgress() == false)
                    {
                        return rtnVal;
                    }
                }

                string strUPDATENO = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(mstrFormNo)).ToString();
                double dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");
                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
                string strChartTime = usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "");
                string strInOutCls = pAcp.inOutCls;

                Cursor.Current = Cursors.WaitCursor;

                if (pForm.FmOLDGB == 1)
                {
                    string strHead = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
                    string strChartX1 = "<chart>";
                    string strChartX2 = "</chart>";
                    string strXML = strHead + strChartX1;
                    StringBuilder strProgress = new StringBuilder();


                    #region 병동
                    strProgress.Append("<ta3 type=\"textArea\" label=\"병동");
                    strProgress.Append("\"><![CDATA[");
                    strProgress.Append(pAcp.ward);
                    strProgress.AppendLine("]]></ta3>");
                    #endregion

                    #region 호실
                    strProgress.Append("<ta4 type=\"textArea\" label=\"호실");
                    strProgress.Append("\"><![CDATA[");
                    strProgress.Append(pAcp.room);
                    strProgress.AppendLine("]]></ta4>");
                    #endregion

                    #region 간호기록
                    strProgress.Append("<ta2 type=\"textArea\" label=\"간호기록");
                    strProgress.Append("\"><![CDATA[");
                    strProgress.Append(txtNa.Text.Trim().Replace("'", "`"));
                    strProgress.AppendLine("]]></ta2>");
                    #endregion

                    strXML += ComNum.VBLF + strProgress.ToString().Trim() + strChartX2;

                    clsEmrQuery.SaveEmrXmlData(dblEmrNo.ToString(), mstrFormNo, strChartDate, strChartTime,
                                   pAcp.acpNo, pAcp.ptNo, strInOutCls, pAcp.medFrDate, pAcp.medFrTime,
                                   pAcp.medEndDate, pAcp.medEndTime, pAcp.medDeptCd, pAcp.medDrCd,
                                   strXML, strUPDATENO);
                }
                else
                {
                    string strSAVEGB = "1";
                    string strSAVECERT = "1";

                    dblEmrNo = clsEmrQuery.SaveNurseRecord(clsDB.DbCon, pAcp, mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                           clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "",
                                                           "", "", txtNa.Text.Trim().Replace("'", "`"), "", txtWardR.Text.Trim(), txtRoomR.Text.Trim());

                }

                clsEmrFunc.NowEmrCert(clsDB.DbCon, pAcp.medFrDate, pAcp.ptNo);

                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }
        #endregion

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdoAll_CheckedChanged(rdoDept, null);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ssNara_Sheet1.RowCount == 0)
                return;

            ssNara_Sheet1.Cells[0, 0, ssNara_Sheet1.RowCount - 1, 0].Text = chkAll.Checked ? "True" : "False";
        }

        private bool IsOldChartFormByEmrNo(string strEmrNo)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT '1' AS OLDGB               ";
                SQL += ComNum.VBLF + "  FROM ADMIN.EMRXML A        ";
                SQL += ComNum.VBLF + " WHERE EMRNO = '" + strEmrNo + "'  ";
                SQL += ComNum.VBLF + " UNION ALL                        ";
                SQL += ComNum.VBLF + "SELECT B.OLDGB                    ";
                SQL += ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A  ";
                SQL += ComNum.VBLF + " INNER JOIN ADMIN.AEMRFORM B ";
                SQL += ComNum.VBLF + "    ON A.FORMNO = B.FORMNO        ";
                SQL += ComNum.VBLF + "   AND A.UPDATENO = B.UPDATENO    ";
                SQL += ComNum.VBLF + " WHERE EMRNO = '" + strEmrNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["OLDGB"].ToString().Trim() == "1")
                    {
                        rtnVal = true;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
