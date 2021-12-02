using ComBase;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    /// <summary>
    /// 응급실 간호기록지등록
    /// FORMNO = 2049
    /// \mtsEmrf\frmTextEmr_NursingRecordER.frm
    /// </summary>
    public partial class frmTextEmr_NursingRecordER : Form, EmrChartForm
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

        /// <summary>
        /// 폰트
        /// </summary>
        Font font = new Font("나눔고딕", 9f);

        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;

        public string mstrFormNo = "2049";
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
            usFormTopMenuEvent.txtMedFrTime.KeyDown += txtMedFrTime_KeyDown;
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);


            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------
        }

        private void txtMedFrTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                txtResult.Focus();
            }
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

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            //필요시만 코딩함
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

            if (ChkTime.Checked)
            {
                if (string.IsNullOrWhiteSpace(usFormTopMenuEvent.txtMedFrTime.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                    return VB.Val(mstrEmrNo);
                }

                if (string.IsNullOrWhiteSpace(txtResult.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "DATA 설정해 주십시오.");
                    return VB.Val(mstrEmrNo);
                }
            }
            else
            {
                usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "yyyyMMdd", null);
                usFormTopMenuEvent.txtMedFrTime.Text = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToString("HH:mm");
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

            string strDate = usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString() + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2) + ":" + VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2);
            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return VB.Val(mstrEmrNo);
            }

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (string.IsNullOrWhiteSpace(txtResult.Text.Trim()) == false && SaveProgress())
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");

                usFormTopMenuEvent.dtMedFrDate.Enabled = true;
                usFormTopMenuEvent.txtMedFrTime.Enabled = true;
                mstrEmrNo = "0";
                txtResult.Clear();
                lblR.Text = "신규등록";
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
        public frmTextEmr_NursingRecordER()
        {
            InitializeComponent();
        }

        public frmTextEmr_NursingRecordER(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
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
        public frmTextEmr_NursingRecordER(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        public frmTextEmr_NursingRecordER(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        private void frmTextEmr_NursingRecordER_Load(object sender, EventArgs e)
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
            ReadMemo();
            READ_SWR();
            READ_Ventilator();
            READ_NOT_CONVERT_BST(pAcp.ptNo, pAcp.medFrDate, dtpOptEDate.Value.ToString("yyyyMMdd"));
            GetBSTList();   //BST
            GetSearchData();//미시행
        }


        private string READ_NOT_CONVERT_BST(string argPTNO, string argINDATE, string argDate)
        {
            string strOK = "";
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수



            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "  SELECT MIN(SUBSTR(MEASURE_DT, 1, 8)) EXAMDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_INTERFACE_BST ";
                SQL = SQL + ComNum.VBLF + " WHERE PATIENT_ID LIKE '" + argPTNO + "%' ";
                SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT >= '" + argINDATE.Replace("-", "").Trim() + "000000' ";
                SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT <= '" + argDate.Replace("-", "").Trim() + "999999' ";
                SQL = SQL + ComNum.VBLF + "   AND EMR IS NULL ";
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
                    strOK = dt.Rows[0]["EXAMDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strOK != "")
                {
                    MessageBox.Show("★ 검사일 : " + strOK + ComNum.VBLF + " 저장하지 않은 BST 인터페이스 결과가 있습니다. 확인하시기 바랍니다.", "확인");

                    using (frmBSTInterface frm = new frmBSTInterface())
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog(this);
                    }
                }

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
                ssUserChartHis_Sheet1.ColumnCount = lngStartCol + 2 + dt.Rows.Count + 1; //사용자 추가
                ssUserChartHis_Sheet1.Columns[ssUserChartHis_Sheet1.ColumnCount - 1].Label = "작성자";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChartHis_Sheet1.Columns[lngStartCol + i + 2].Width = (int)VB.Val(dt.Rows[i]["ITEMWIDTH"].ToString().Trim()) + 320;
                    ssUserChartHis_Sheet1.Columns[lngStartCol + i].Font = font;

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

                dt.Dispose();

                txtResult.Width = (int) ssUserChartHis_Sheet1.Columns[16].Width ;
                txtResult.Font = this.Font;
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

            txtResult.Clear();
            txtRoomR.Clear();
            lblR.Text = "신규등록";
            txtWardR.Text = "ER";
        }

        #endregion

        #region 트리뷰
        void SetWardList()
        {
            cboWard.Items.Clear();

            OracleDataReader dataReader = null;

            string SQL = " SELECT A.MACROGB, MAX(B.NAME) AS NAME ";
            SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMRMACROETC A ";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CLINICT B ";
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
                if(clsType.User.BuseCode.Equals(cboWard.Items[i].ToString().Substring(cboWard.Items[i].ToString().LastIndexOf(' ')).Trim()))
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

            if (READ_COMPLETE())
            {
                lbComplete.Visible = true;
                btnComplete.Text = "완료취소";
            }
            else
            {
                lbComplete.Visible = false;
                btnComplete.Text = "입력완료";
            }

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
                //SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML A INNER JOIN KOSMOS_EMR.EMRFORM B "          ;
                //SQL += ComNum.VBLF + "      ON A.FORMNO = B.FORMNO "                                      ;
                //SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.BAS_PASS U"                              ;
                //SQL += ComNum.VBLF + "      ON A.USEID = U.IDNUMBER ";

                //if (cboBuse.Text.Length > 0)
                //{
                //    SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_USERT U2 ";
                //    SQL += ComNum.VBLF + "      ON A.USEID = U2.USERID";
                //    SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_BUSE E ";
                //    SQL += ComNum.VBLF + "      ON E.BUCODE = U2.BUSECODE ";
                //    SQL += ComNum.VBLF + "    AND E.BUCODE = " + VB.Right(cboBuse.Text.Trim(), 6);
                //}

                //SQL += ComNum.VBLF + "  WHERE EMRNO IN (SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = " + strFormNo;
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
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML A INNER JOIN KOSMOS_EMR.EMRFORM B ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.BAS_PASS U ";
                SQL = SQL + ComNum.VBLF + "      ON A.USEID     = U.IDNUMBER ";
                SQL = SQL + ComNum.VBLF + "     AND U.PROGRAMID = '        '";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO IN( ";
                SQL = SQL + ComNum.VBLF + "    SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST ";
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
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A INNER JOIN KOSMOS_EMR.AEMRFORM B ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.BAS_PASS U ";
                SQL = SQL + ComNum.VBLF + "      ON A.CHARTUSEID = U.IDNUMBER ";
                SQL = SQL + ComNum.VBLF + "     AND U.PROGRAMID  = '        '";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRNURSRECORD N ";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO = N.EMRNO ";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = N.EMRNOHIS ";
                SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + strSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + strEDATE + "' ";
                SQL = SQL + ComNum.VBLF + ") T ";
                if (cboBuse.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_USERT U2 ";
                    SQL = SQL + ComNum.VBLF + "      ON T.USEID = U2.USERID";
                    SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_BUSE E ";
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
                    objSpread.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["NURSERECODE"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["USENAME"].ToString().Trim();

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

            if (objSpread.ActiveSheet.Columns[lngItemCol].CellType != null &&
               objSpread.ActiveSheet.Columns[lngItemCol].CellType.ToString() == "TextCellType")
            {
                textCellType = (TextCellType)objSpread.ActiveSheet.Columns[lngItemCol].CellType;
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

        #region ReadMemo
        void ReadMemo()
        {
            ssErMemo.ActiveSheet.RowCount = 0;
            ssErMemo.ActiveSheet.RowCount = 1;

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = " SELECT MEMO, TO_CHAR(WRITEDATE,'YYYY-MM-DD') WRITEDATE, WRITESABUN, B.KORNAME, A.ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_MEMO A";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_ADM.INSA_MST B";
                SQL += ComNum.VBLF + "      ON A.WRITESABUN = B.SABUN3";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + pAcp.ptNo + "' ";
                SQL += ComNum.VBLF + "   AND WRITEDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + " 00:00','YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "   AND WRITEDATE <= TO_DATE('" + dtpOptEDate.Value.ToShortDateString() + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "  AND FORMNO = " + mstrFormNo;
                SQL += ComNum.VBLF + "  ORDER BY WRITEDATE DESC";

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

                ssErMemo.ActiveSheet.RowCount = dt.Rows.Count + 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssErMemo.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                    ssErMemo.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    ssErMemo.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                    ssErMemo.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssErMemo.ActiveSheet.Rows[i].Height = ssErMemo.ActiveSheet.Rows[i].GetPreferredHeight() + 5;
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
        #endregion

        #region READ_SWR
        void READ_SWR()
        {
            ssSWR.ActiveSheet.RowCount = 0;

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = " SELECT CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//it7') it7,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//it8') it8,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//it11') it11,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//it12') it12,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//it14') it14,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//it16') it16";
                SQL += ComNum.VBLF + "   FROM KOSMOS_EMR.EMRXML";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + pAcp.ptNo + "'";
                SQL += ComNum.VBLF + "      AND CHARTDATE >= '" + dtpOptSDate.Value.AddDays(-1).ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "      AND CHARTDATE <= '" + dtpOptEDate.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "      AND FORMNO = 1969";
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + " SELECT CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000002018') AS it7,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000001765') AS it8,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000014815') AS it11,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000002009') AS it12,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000001811') AS it14,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000008708') AS it16";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + pAcp.ptNo + "'";
                SQL += ComNum.VBLF + "   AND CHARTDATE >= '" + dtpOptSDate.Value.AddDays(-1).ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "   AND CHARTDATE <= '" + dtpOptEDate.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "   AND FORMNO IN(1969)";

                SQL += ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                ssSWR.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string strChartDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    string strChartTime = dt.Rows[i]["CHARTTIME"].ToString().Trim();
                    strChartDate = VB.Right(strChartDate, 4);
                    strChartDate = VB.Left(strChartDate, 2) + "/" + VB.Right(strChartDate, 2);


                    strChartTime = VB.Left(strChartTime, 4);
                    strChartTime = VB.Left(strChartTime, 2) + ":" + VB.Right(strChartTime, 2);

                    ssSWR.ActiveSheet.Cells[i, 0].Text = strChartDate;
                    ssSWR.ActiveSheet.Cells[i, 1].Text = strChartTime;
                    ssSWR.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["it7"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["it8"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["it11"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["it12"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["it14"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["it16"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    ssSWR.ActiveSheet.Cells[i, 8].Text = VB.Left(ssSWR.ActiveSheet.Cells[i, 8].Text, 4) + "-" + VB.Mid(ssSWR.ActiveSheet.Cells[i, 8].Text, 5, 2) + "-" + VB.Right(ssSWR.ActiveSheet.Cells[i, 8].Text, 2);
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
        #endregion

        #region READ_Ventilator
        void READ_Ventilator()
        {
            ssVentilator.ActiveSheet.RowCount = 0;

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {                                                                        ;
                SQL = " SELECT CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL += ComNum.VBLF + "    ta1,";
                SQL += ComNum.VBLF + "    ta2,";
                SQL += ComNum.VBLF + "    ta3,";
                SQL += ComNum.VBLF + "    ta4,";
                SQL += ComNum.VBLF + "    ta5,";
                SQL += ComNum.VBLF + "    ta6,";
                SQL += ComNum.VBLF + "    ta7,";
                SQL += ComNum.VBLF + "    ta8,";
                SQL += ComNum.VBLF + "    ta9,";
                SQL += ComNum.VBLF + "    ta10,";
                SQL += ComNum.VBLF + "    ta11,";
                SQL += ComNum.VBLF + "    ta12,";
                SQL += ComNum.VBLF + "    ta13,";
                SQL += ComNum.VBLF + "    ta14,";
                SQL += ComNum.VBLF + "    ta15";
                SQL += ComNum.VBLF + " FROM(";
                SQL += ComNum.VBLF + " SELECT CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL += ComNum.VBLF + "    EXTRACTVALUE(CHARTXML, '//ta1') ta1,";
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta2') ta2,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta3') ta3,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta4') ta4,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta5') ta5,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta6') ta6,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta7') ta7,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta8') ta8,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta9') ta9,"        ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta10') ta10,"      ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta11') ta11,"      ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta12') ta12,"      ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta13') ta13,"      ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta14') ta14,"      ;
                SQL += ComNum.VBLF +  "    EXTRACTVALUE(CHARTXML, '//ta15') ta15"       ;
                SQL += ComNum.VBLF +  "   FROM KOSMOS_EMR.EMRXML"                       ;
                SQL += ComNum.VBLF +  " WHERE PTNO = '" + pAcp.ptNo + "'"                 ;
                SQL += ComNum.VBLF +  "      AND CHARTDATE >= '" + dtpOptSDate.Value.AddDays(-1).ToString("yyyyMMdd") + "'"       ;
                SQL += ComNum.VBLF +  "      AND CHARTDATE <= '" + dtpOptEDate.Value.ToString("yyyyMMdd") + "'"       ;
                SQL += ComNum.VBLF +  "      AND FORMNO = 2598"                       ;

                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + " SELECT CHARTDATE, TRIM(CHARTTIME) CHARTTIME,";
                SQL += ComNum.VBLF + "    '' ta1,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMCD = 'I0000031111_1') AS ta2,";
                SQL += ComNum.VBLF + "    '' ta3,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037245') AS ta4,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037247') AS ta5,";
                SQL += ComNum.VBLF + "    '' ta6,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037248') AS ta7,";
                SQL += ComNum.VBLF + "    '' ta8,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037249') AS ta9,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037250') AS ta10,";
                SQL += ComNum.VBLF + "    '' ta11,";
                SQL += ComNum.VBLF + "    '' ta12,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037857') AS ta13,";
                SQL += ComNum.VBLF + "    '' ta14,";
                SQL += ComNum.VBLF + "    (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037254') AS ta15";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + pAcp.ptNo + "'";
                SQL += ComNum.VBLF + "   AND CHARTDATE >= '" + dtpOptSDate.Value.AddDays(-1).ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "   AND CHARTDATE <= '" + dtpOptEDate.Value.ToString("yyyyMMdd") + "'";
                SQL += ComNum.VBLF + "   AND FORMNO IN(1969)";
                SQL += ComNum.VBLF + " )";
                SQL += ComNum.VBLF + " WHERE (";
                SQL += ComNum.VBLF + "       ta1 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta2 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta3 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta4 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta5 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta6 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta7 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta8 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta9 IS NOT NULL  OR ";
                SQL += ComNum.VBLF + "       ta10 IS NOT NULL OR";
                SQL += ComNum.VBLF + "       ta11 IS NOT NULL OR";
                SQL += ComNum.VBLF + "       ta12 IS NOT NULL OR";
                SQL += ComNum.VBLF + "       ta13 IS NOT NULL OR";
                SQL += ComNum.VBLF + "       ta14 IS NOT NULL OR";
                SQL += ComNum.VBLF + "       ta15 IS NOT NULL";
                SQL += ComNum.VBLF + "       )";

                SQL += ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";

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

                ssVentilator.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string strChartDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    string strChartTime = dt.Rows[i]["CHARTTIME"].ToString().Trim();
                    strChartDate = VB.Right(strChartDate, 4);
                    strChartDate = VB.Left(strChartDate, 2) + "/" + VB.Right(strChartDate, 2);


                    strChartTime = VB.Left(strChartTime, 4);
                    strChartTime = VB.Left(strChartTime, 2) + ":" + VB.Right(strChartTime, 2);

                    ssVentilator.ActiveSheet.Cells[i, 0].Text = strChartDate;
                    ssVentilator.ActiveSheet.Cells[i, 1].Text = strChartTime;
                    ssVentilator.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ta1"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ta2"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ta3"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ta4"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["ta5"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["ta6"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["ta7"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ta8"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["ta9"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["ta10"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["ta11"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["ta12"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["ta13"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["ta14"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["ta15"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                    ssVentilator.ActiveSheet.Cells[i, 17].Text = VB.Left(ssVentilator.ActiveSheet.Cells[i, 17].Text, 4) + "-" +
                        VB.Mid(ssVentilator.ActiveSheet.Cells[i, 17].Text, 5, 2) + "-" + VB.Right(ssVentilator.ActiveSheet.Cells[i, 17].Text, 2);
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
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
                SQL += ComNum.VBLF + "          (SELECT MAX(O.MACROINDEX) FROM KOSMOS_EMR.EMRMACROETCDTL O";
                SQL += ComNum.VBLF + "                  WHERE O.MACROGB = A.MACROGB AND O.MACROINDEX = A.MACROINDEX) AS DTLYN";
                SQL += ComNum.VBLF + "           FROM KOSMOS_EMR.EMRMACROETC A";
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
            ssResult_Sheet1.RowCount = 0;

            if (e.Node.Nodes.Count > 0)
                return;

            mlngMACROINDEX = VB.Val(ComFunc.SptChar(e.Node.Name, 1, "_"));

            txtProbR.Text = e.Node.Text.Trim();

            GetDtlInfo();
        }

        void GetDtlInfo()
        {
            ssResult_Sheet1.RowCount = 0;

            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ";
                SQL += ComNum.VBLF + "  A.MACROCD, ";
                SQL += ComNum.VBLF + "  A.MACROSEQ, A.MACROTEXT, A.MACRODSP";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRMACROETCDTL A";
                SQL += ComNum.VBLF + " WHERE A.MACROGB = '" + mMACROGB.Trim() + "'";
                SQL += ComNum.VBLF + "   AND A.MACROINDEX = " + mlngMACROINDEX;
                SQL += ComNum.VBLF + "   AND A.MACROCD = 'RESULT'";

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


                ssResult_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssResult_Sheet1.Cells[i, 0].Text = "True";
                    ssResult_Sheet1.Cells[i, 1].Text = dt.Rows[i]["MACROTEXT"].ToString().Trim();
                    ssResult_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MACRODSP"].ToString().Trim();
                    ssResult_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MACROCD"].ToString().Trim();
                    ssResult_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MACROSEQ"].ToString().Trim();

                    ssResult_Sheet1.Rows[i].Height = ssResult_Sheet1.Rows[i].GetPreferredHeight() + 10;
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

            txtResult.Clear();
            txtRoomR.Clear();
            lblR.Text = "신규등록";
            txtWardR.Text = "ER";
        }

        void GetSearchData()
        {
            //frmOrderCheck 참조
            

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            string strFDate = dtpOptSDate.Value.ToString("yyyy-MM-dd");
            string strTDate = dtpOptEDate.Value.ToString("yyyy-MM-dd");

            string strPano = "";
            string strERROR = "";
            string strORDERCODE = "";
            string strSucode = "";
            string strOrderName = "";
            string strOrderNo = "";
            string strBDATE = "";

            int i = 0;
            int j = 0;
            int nCnt = 0;
            ssErCheck_Sheet1.RowCount = 0;

            DateTime DT = dtpOptSDate.Value;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
               
                SQL = " SELECT Pano,SName,Bi,DrCode,Age,DeptCode,      (SELECT NAME ";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "      WHERE GUBUN = 'ETC_응급실환자구역' ";
                SQL = SQL + ComNum.VBLF + "        AND CODE = ER_NUM) ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(BDate,'YYYY-MM-DD')  BDate , ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(ActDate,'YYYY-MM-DD') ActDate  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER  ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  BDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  Pano < '90000000' ";
                SQL = SQL + ComNum.VBLF + "   AND  Pano <> '81000004' ";
                SQL = SQL + ComNum.VBLF + "   AND  PANO = '" + pAcp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND  DeptCode ='ER'  ";   //'응급실만"
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDate,SName ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strERROR = "N";

                    if (strPano == "01689167")
                    {
                        //strPano = strPano;
                    }

                    DT = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim());
                    DT = DT.AddDays(+2);

                    SQL = " SELECT A.BUN, A.ORDERCODE, A.SUCODE, A.ORDERNO, B.ORDERNAME, B.ORDERNAMES, ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.ACTDATE ,'YYYY-MM-DD') ACTDATE, BDATE, A.ORDERNO  ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A ,KOSMOS_OCS.OCS_ORDERCODE  B ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + ComFunc.FormatStrToDateTime(dt.Rows[i]["BDATE"].ToString().Trim(), "D") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + DT.ToShortDateString() + "','YYYY-MM-DD') ";
                 
                    SQL = SQL + ComNum.VBLF + "   AND GBIOE IN ('E','EI')  ";
                    SQL = SQL + ComNum.VBLF + "   AND ((A.Bun >='52' AND A.BUN <='73') or(A.Bun='44') ) ";

                    SQL = SQL + ComNum.VBLF + "   AND A.Ptno ='" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND (A.GBSTATUS IS NULL OR A.GBSTATUS NOT IN ('D','D-')) ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE = B.ORDERCODE";
                    SQL = SQL + ComNum.VBLF + " ORDER BY  Ptno ";


                   

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            strORDERCODE = dt2.Rows[j]["ORDERCODE"].ToString().Trim();
                            strSucode = dt2.Rows[j]["SUCODE"].ToString().Trim();
                            strOrderName = dt2.Rows[j]["ORDERNAME"].ToString().Trim() + " " + dt2.Rows[j]["ORDERNAMES"].ToString().Trim();
                            strOrderNo = dt2.Rows[j]["ORDERNO"].ToString().Trim();

                            if (strOrderNo == "35868056")
                            {
                            }

                            if (VB.Val(dt2.Rows[j]["BUN"].ToString().Trim()) >= 52 && VB.Val(dt2.Rows[j]["BUN"].ToString().Trim()) <= 64)
                            {//진단검사의학과
                                if (strOrderNo == "166961447")
                                {
                                    //i = i;
                                }
                                SQL = " SELECT PANO ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_SPECMST ";
                                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ComFunc.FormatStrToDateTime(dt2.Rows[j]["BDATE"].ToString().Trim(), "D") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND STATUS = '00' ";
                                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt2.Rows[j]["ORDERNO"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                strERROR = "";
                                if (dt3.Rows.Count > 0) strERROR = "Y";
                            }
                            else if (VB.Val(dt2.Rows[j]["BUN"].ToString().Trim()) == 44)
                            {//EKG
                                if (strOrderNo == "166961447")
                                {
                                    //i = i;
                                }
                                SQL = " SELECT PTNO ";
                                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.ETC_JUPMST ";
                                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ComFunc.FormatStrToDateTime(dt2.Rows[j]["BDATE"].ToString().Trim(), "D") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND GBFTP  is null ";
                                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt2.Rows[j]["ORDERNO"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                strERROR = "";
                                if (dt3.Rows.Count > 0) strERROR = "Y";
                            }

                            else
                            {//영상의학과
                                SQL = " SELECT PANO";
                                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.XRAY_DETAIL ";
                                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ComFunc.FormatStrToDateTime(dt2.Rows[j]["BDATE"].ToString().Trim(), "D") + "','YYYY-MM-DD') ";
                                //2018-10-24 SUCODE 와 XCODE 가 다른 경우가 발생하여 SUCODE 비교는 제외함.
                                //SQL = SQL + ComNum.VBLF + "   AND XCODE = '" + dt2.Rows[j]["SUCODE"].ToString().Trim() + "' "                            ;
                                SQL = SQL + ComNum.VBLF + "   AND GBRESERVED = '7' ";
                                SQL = SQL + ComNum.VBLF + "   AND ORDERNO = '" + dt2.Rows[j]["ORDERNO"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                strERROR = "";
                                if (dt3.Rows.Count == 0) strERROR = "Y";
                               
                            }

                            dt3.Dispose();
                            dt3 = null;

                            if (strERROR == "Y")
                            {
                                ssErCheck_Sheet1.RowCount = ssErCheck_Sheet1.RowCount + 1;

                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 0].Text = strPano;
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["Age"].ToString().Trim();

                                SQL = " SELECT DECODE(ROOMCODE,'234','MICU','233','SICU',WARDCODE) WARDCODE ";
                                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND TRUNC(INDATE) = TO_DATE('" + ComFunc.FormatStrToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim(), "D") + "','YYYY-MM-DD') ";

                            
                                SqlErr = clsDB.GetDataTableEx(ref dt3, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (dt3.Rows.Count > 0)
                                {
                                    ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 3].Text = dt3.Rows[0]["WARDCODE"].ToString().Trim();
                                }

                                dt3.Dispose();
                                dt3 = null;

                                if (dt.Rows[i]["DeptCode"].ToString().Trim() == "ER")
                                {
                                    ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                                }

                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                                strBDATE = ComFunc.FormatStrToDateTime(dt.Rows[i]["BDate"].ToString().Trim(), "D");

                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 6].Text = VB.Right(strBDATE,2);
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 7].Text = strORDERCODE;
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 8].Text = strSucode;
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 9].Text = strOrderName;
                                ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 10].Text = strOrderNo;
                               // ssErCheck_Sheet1.Cells[ssErCheck_Sheet1.RowCount - 1, 11].Text = readNoPay(strPano, strBDATE, strOrderNo);

                            }

                            strERROR = "N";
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void GetBSTList()
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            { 
                ssBST_Sheet1.RowCount = 0;

                SQL = "SELECT MEASURE_DT, B.VALUE, SUBSTR(B.PATIENT_ID,1,8) PATIENT_ID ,TO_CHAR(B.EMR, 'YYYY-MM-DD') EMR";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER I  ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_OCS.EXAM_INTERFACE_BST B";
                SQL = SQL + ComNum.VBLF + "      ON MEASURE_DT >=  '" + dtpOptSDate.Value.ToString("yyyyMMdd") + "000000'";
                SQL = SQL + ComNum.VBLF + "      AND MEASURE_DT <= '" + dtpOptEDate.Value.ToString("yyyyMMdd") + "235959'";
                SQL = SQL + ComNum.VBLF + "      AND B.WARD LIKE 'ER%'";
                SQL = SQL + ComNum.VBLF + "      AND SUBSTR(B.PATIENT_ID,1,8) = '" + pAcp.ptNo + "'";
                //SQL = SQL + ComNum.VBLF + "     WHERE I.ACTDATE = '" + dtpExamDate.Value.ToString("yyyy-MM-dd") + "'";
                SQL = SQL + ComNum.VBLF + "     AND I.ACTDATE >= TO_CHAR(SYSDATE-2, 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "     AND I.DEPTCODE = 'ER'";
                SQL = SQL + ComNum.VBLF + "     AND I.PANO = '" + pAcp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "     ORDER BY MEASURE_DT DESC";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssBST_Sheet1.RowCount = dt.Rows.Count;
                    ssBST_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime dtpDT = DateTime.ParseExact(dt.Rows[i]["MEASURE_DT"].ToString(), "yyyyMMddHHmmss", null);

                        ssBST_Sheet1.Cells[i, 0].Locked = !string.IsNullOrWhiteSpace(dt.Rows[i]["EMR"].ToString().Trim());
                        ssBST_Sheet1.Cells[i, 1].Text = dtpDT.ToString("yyyy-MM-dd");
                        ssBST_Sheet1.Cells[i, 2].Text = dtpDT.ToString("HH:mm");
                        ssBST_Sheet1.Cells[i, 2].Tag = dtpDT.ToString("yyyyMMddHHmmss");
                        ssBST_Sheet1.Cells[i, 3].Text = dt.Rows[i]["VALUE"].ToString().Trim();
                        //ssChart_Sheet1.Cells[i, 3].Tag  = dt.Rows[i]["SPECNO"].ToString().Trim();
                        ssBST_Sheet1.Cells[i, 3].Tag = dt.Rows[i]["PATIENT_ID"].ToString().Trim();
                        ssBST_Sheet1.Cells[i, 4].Text = dt.Rows[i]["EMR"].ToString().Trim();

                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "BST Interface GetBSTList()", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
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
            for(int lngRow = 0; lngRow < ssResult_Sheet1.RowCount; lngRow++)
            {
                if(ssResult_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True"))
                {
                    strString.AppendLine(ssResult_Sheet1.Cells[lngRow, 1].Text.Trim());
                }
            }

            if(strString.Length > 0)
            {
                txtResult.AppendText((string.IsNullOrWhiteSpace(txtResult.Text) ? "" :  ComNum.VBLF) + strString.ToString());
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
            txtResult.Clear();
            lblR.Text = "신규등록";
            usFormTopMenuEvent.mbtnSave.Visible = true;
        }

        #endregion

        #region 좌측 하단 - 히스토리
        private void btnSearchHis_Click(object sender, EventArgs e)
        {
            GetChartHistory();
        }
        #endregion


        #region 좌측 V/S, 메모 등
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReadMemo();
        }

        /// <summary>
        /// 메모 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MemoSave())
            {
                ReadMemo();
            }
        }

        /// <summary>
        /// 메모 저장
        /// </summary>
        /// <returns></returns>
        bool MemoSave()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for(int i = 0; i < ssErMemo_Sheet1.RowCount; i++)
                {
                    string strMEMO = ssErMemo_Sheet1.Cells[i, 1].Text.Trim().Replace("'", "`");
                    string strROWID = ssErMemo_Sheet1.Cells[i, 4].Text.Trim();
                    string strChange = ssErMemo_Sheet1.Cells[i, 5].Text.Trim();

                    if (strChange.Equals("Y"))
                    {
                        if(string.IsNullOrWhiteSpace(strROWID))
                        {
                            SQL = " INSERT INTO KOSMOS_EMR.EMR_MEMO( ";
                            SQL += ComNum.VBLF + " PTNO, MEMO, WRITEDATE, WRITESABUN, FORMNO) VALUES ";
                            SQL += ComNum.VBLF + "('" + pAcp.ptNo + "','" + strMEMO + "', SYSDATE, " + clsType.User.IdNumber + "," + mstrFormNo + ") ";

                            sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if(sqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, sqlErr);
                                return rtnVal;
                            }
                        }
                        else
                        {
                            SQL = " UPDATE KOSMOS_EMR.EMR_MEMO SET ";
                            SQL += ComNum.VBLF + " MEMO = '" + strMEMO + "'";
                            SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                            if (sqlErr.Length > 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, sqlErr);
                                return rtnVal;
                            }
                        }
                    
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            return rtnVal;
        }

        /// <summary>
        /// 메모삭제
        /// </summary>
        /// <returns></returns>
        bool MemoDelete()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < ssErMemo_Sheet1.RowCount; i++)
                {
                    string strROWID = ssErMemo_Sheet1.Cells[i, 4].Text.Trim();

                    if (ssErMemo_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        SQL = " DELETE KOSMOS_EMR.EMR_MEMO ";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";

                        sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                        if (sqlErr.Length > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, sqlErr);
                            return rtnVal;
                        }

                    }
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            return rtnVal;
        }

        /// <summary>
        /// 메모 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MemoDelete())
            {
                ReadMemo();
            }
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
                    //panWard.Visible = true;
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

        private void ssErMemo_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Column == 1)
            {
                ssErMemo_Sheet1.Cells[e.Row, 5].Text = "Y";
                ssErMemo_Sheet1.Rows[e.Row].Height = ssErMemo_Sheet1.Rows[e.Row].GetPreferredHeight() + 5;
            }
        }

        private void ssUserChartHis_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            lblR.Text = "변경작업";

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

            txtResult.Text = ssUserChartHis_Sheet1.Cells[e.Row, 16].Text.Trim().Replace("\n", ComNum.VBLF);
        }

        bool READ_Modify_Cert(FarPoint.Win.Spread.FpSpread spd, int ArgCol, int argROW, string argUserID)
        {
            return spd.ActiveSheet.Cells[argROW, ArgCol].Text.Trim().Equals(argUserID);
        }

        private void ssSWR_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssSWR_Sheet1.RowCount == 0)
                return;

            ChkTime.Checked = true;

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssSWR_Sheet1.Cells[e.Row, 8].Text.Trim());
            usFormTopMenuEvent.txtMedFrTime.Text = ssSWR_Sheet1.Cells[e.Row, 1].Text.Trim();

            string strData = ssSWR_Sheet1.Cells[e.Row, 2].Text.Trim() + ssSWR_Sheet1.Cells[e.Row, 3].Text.Trim();
            StringBuilder strVS = new StringBuilder();

            if (strData.Length > 0)
            {
                strVS.Append("▶혈압 : ").Append(ssSWR_Sheet1.Cells[e.Row, 2].Text.Trim()).Append("/");
                strVS.AppendLine(ssSWR_Sheet1.Cells[e.Row, 3].Text.Trim());
            }

            if (ssSWR_Sheet1.Cells[e.Row, 4].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶맥박 : " + ssSWR_Sheet1.Cells[e.Row, 4].Text.Trim());
            }

            if (ssSWR_Sheet1.Cells[e.Row, 5].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶자가호흡 : " + ssSWR_Sheet1.Cells[e.Row, 5].Text.Trim());
            }

            if (ssSWR_Sheet1.Cells[e.Row, 6].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶체온 : " + ssSWR_Sheet1.Cells[e.Row, 6].Text.Trim());
            }

            if (ssSWR_Sheet1.Cells[e.Row, 7].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶SpO2 : " + ssSWR_Sheet1.Cells[e.Row, 7].Text.Trim());
            }

            if (txtResult.TextLength > 0)
            {
                txtResult.AppendText(ComNum.VBLF);
            }
            txtResult.AppendText(strVS.ToString().Trim());
        }

        private void ssVentilator_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssVentilator_Sheet1.RowCount == 0)
                return;

            ChkTime.Checked = true;

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssVentilator_Sheet1.Cells[e.Row, 17].Text.Trim());
            usFormTopMenuEvent.txtMedFrTime.Text = ssVentilator_Sheet1.Cells[e.Row, 1].Text.Trim();

            string strData = ssVentilator_Sheet1.Cells[e.Row, 2].Text.Trim() + ssVentilator_Sheet1.Cells[e.Row, 3].Text.Trim();
            StringBuilder strVS = new StringBuilder();

            if (ssVentilator_Sheet1.Cells[e.Row, 2].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶환기양식 : " + ssVentilator_Sheet1.Cells[e.Row, 2].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 3].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶Mode : " + ssVentilator_Sheet1.Cells[e.Row, 3].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 4].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶Weaning Mode : " + ssVentilator_Sheet1.Cells[e.Row, 4].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 5].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶Fio2 : " + ssVentilator_Sheet1.Cells[e.Row, 5].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 6].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶R.R : " + ssVentilator_Sheet1.Cells[e.Row, 6].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 7].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶Vt : " + ssVentilator_Sheet1.Cells[e.Row, 7].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 8].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶I : " + ssVentilator_Sheet1.Cells[e.Row, 8].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 9].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶E : " + ssVentilator_Sheet1.Cells[e.Row, 9].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 10].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶PEEP : " + ssVentilator_Sheet1.Cells[e.Row, 10].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 11].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶Ins.Press. : " + ssVentilator_Sheet1.Cells[e.Row, 11].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 12].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶MV stop 시간  : " + ssVentilator_Sheet1.Cells[e.Row, 12].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 13].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶MV Reapply : " + ssVentilator_Sheet1.Cells[e.Row, 13].Text.Trim());
            }
            if (ssVentilator_Sheet1.Cells[e.Row, 14].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶Dr's  : " + ssVentilator_Sheet1.Cells[e.Row, 14].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 15].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶처방경로 : " + ssVentilator_Sheet1.Cells[e.Row, 15].Text.Trim());
            }

            if (ssVentilator_Sheet1.Cells[e.Row, 16].Text.Trim().Length > 0)
            {
                strVS.AppendLine("▶상태 : " + ssVentilator_Sheet1.Cells[e.Row, 16].Text.Trim());
            }

            if(txtResult.TextLength > 0)
            {
                txtResult.AppendText(ComNum.VBLF);
            }
            txtResult.AppendText(strVS.ToString().Trim());
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

                if (string.IsNullOrWhiteSpace(txtResult.Text.Trim()))
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

            if (string.IsNullOrWhiteSpace(txtResult.Text.Trim()) == false && SaveProgress())
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");

                usFormTopMenuEvent.dtMedFrDate.Enabled = true;
                usFormTopMenuEvent.txtMedFrTime.Enabled = true;
                mstrEmrNo = "0";
                txtResult.Clear();
                lblR.Text = "신규등록";
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

                    strProgress.Append("<");
                    strProgress.Append(ssUserChartHis_Sheet1.Columns[16].Tag.ToString());
                    strProgress.Append(" type=\"textArea\" label=\"간호기록");
                    strProgress.Append("\"><![CDATA[");
                    strProgress.Append(txtResult.Text.Trim().Replace("'", "`"));
                    strProgress.Append("]]><");
                    strProgress.Append("/");
                    strProgress.Append(ssUserChartHis_Sheet1.Columns[16].Tag.ToString());
                    strProgress.Append(">");

                    strXML += strProgress.ToString().Trim() + strChartX2;

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
                                                           "", "", txtResult.Text.Trim().Replace("'", "`"), "", txtWardR.Text.Trim(), txtRoomR.Text.Trim());

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

        private void btnComplete_Click(object sender, EventArgs e)
        {
            COMPLETE_YN(btnComplete.Text.Equals("입력완료") ? "Y" : "N");

            if (READ_COMPLETE())
            {
                lbComplete.Visible = true;
                btnComplete.Text = "완료취소";
            }
            else
            {
                lbComplete.Visible = false;
                btnComplete.Text = "입력완료";
            }
        }

        /// <summary>
        /// 입력완료 추가/삭제
        /// </summary>
        /// <param name="argYN"></param>
        void COMPLETE_YN(string argYN)
        {
            if (pAcp == null)
                return;

            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;


            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
              
                switch(argYN)
                {
                    case "Y":
                        SQL = " INSERT INTO KOSMOS_EMR.EMR_COMPLETE(";
                        SQL += ComNum.VBLF + " MEDFRDATE, MEDFRTIME, MEDDEPTCD, MEDDRCD, ";
                        SQL += ComNum.VBLF + " PTNO, FORMNO, INOUTCLS, WRITEDATE, ";
                        SQL += ComNum.VBLF + " WRITETIME, USERID) VALUES (";
                        SQL += ComNum.VBLF + "'" + pAcp.medFrDate + "','1200','','',";
                        SQL += ComNum.VBLF + "'" + pAcp.ptNo + "','" + mstrFormNo + "','I','" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "', ";
                        SQL += ComNum.VBLF + "'" + ComQuery.CurrentDateTime(clsDB.DbCon, "T") + "','" + clsType.User.IdNumber + "')";
                        break;
                    case "N":
                        SQL = " DELETE KOSMOS_EMR.EMR_COMPLETE";
                        SQL += ComNum.VBLF + " WHERE PTNO = '" + pAcp.ptNo + "' ";
                        SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + pAcp.medFrDate + "' ";
                        SQL += ComNum.VBLF + "   AND FORMNO = '" + mstrFormNo + "' ";
                        break;
                }

                sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 입력완료 확인
        /// </summary>
        /// <returns></returns>
        bool READ_COMPLETE()
        {
            bool rtnVal = false;
            OracleDataReader dataReader = null;

            string SQL = " SELECT FORMNO";
            SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMR_COMPLETE ";
            SQL += ComNum.VBLF + "    WHERE PTNO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + "      AND MEDFRDATE = '" + pAcp.medFrDate + "'";
            SQL += ComNum.VBLF + "      AND FORMNO = " + mstrFormNo;


            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return rtnVal;
            }

            if (dataReader.HasRows && dataReader.Read())
            {
                rtnVal = true;
            }

            dataReader.Dispose();

            return rtnVal;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ssResult_Sheet1.RowCount == 0)
                return;

            ssResult_Sheet1.Cells[0, 0, ssResult_Sheet1.RowCount - 1, 0].Text = chkAll.Checked ? "True" : "False";
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
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXML A        ";
                SQL += ComNum.VBLF + " WHERE EMRNO = '" + strEmrNo + "'  ";
                SQL += ComNum.VBLF + " UNION ALL                        ";
                SQL += ComNum.VBLF + "SELECT B.OLDGB                    ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A  ";
                SQL += ComNum.VBLF + " INNER JOIN KOSMOS_EMR.AEMRFORM B ";
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

        private void ssBST_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssBST_Sheet1.RowCount == 0)
                return;

            //usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssBST_Sheet1.Cells[e.Row, 17].Text.Trim());
            //usFormTopMenuEvent.txtMedFrTime.Text = ssBST_Sheet1.Cells[e.Row, 1].Text.Trim();

            //string strData = ssBST_Sheet1.Cells[e.Row, 2].Text.Trim() + ssBST_Sheet1.Cells[e.Row, 3].Text.Trim();
            StringBuilder strBST = new StringBuilder();

            //if (ssBST_Sheet1.Cells[e.Row, 1].Text.Trim().Length > 0)
            //{
            //    strBST.AppendLine("▶BST측정일자 : " + ssBST_Sheet1.Cells[e.Row, 1].Text.Trim());
            //}

            //if (ssBST_Sheet1.Cells[e.Row, 2].Text.Trim().Length > 0)
            //{
            //    strBST.AppendLine("▶측정시간 : " + ssBST_Sheet1.Cells[e.Row, 2].Text.Trim());
            //}

            if (ssBST_Sheet1.Cells[e.Row, 3].Text.Trim().Length > 0)
            {
                strBST.AppendLine("▶BST : " + ssBST_Sheet1.Cells[e.Row, 3].Text.Trim() + "mg/dl") ;
            }


            if (txtResult.TextLength > 0)
            {
                txtResult.AppendText(ComNum.VBLF);
            }
            txtResult.AppendText(strBST.ToString().Trim());
        }
    }
}
