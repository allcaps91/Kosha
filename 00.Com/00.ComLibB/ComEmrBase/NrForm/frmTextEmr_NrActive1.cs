using ComBase;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    /// <summary>
    /// 기본간호활동
    /// FORMNO = 1575
    /// \mtsEmrf\frmTextEmr_NrActive1.frm
    /// </summary>
    public partial class frmTextEmr_NrActive1 : Form, EmrChartForm
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
        int mlngEtcCol = 0;
        int mlngEtcRow = 0;
        int mlngOutCol = 0;
        int mlngOutRow = 0;

        //bool mLoading = false;

        /// <summary>
        /// 차트작성 일자.
        /// </summary>
        string mstrOrderDate = string.Empty;

        /// <summary>
        /// VB => txtEmrUseId => 작성자 아이디
        /// </summary>
        string mEmrUseId = string.Empty;

        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        //EmrForm pForm = null;

        public string mstrFormNo = "1575";
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
            if (mstrMode.Equals("W") == false || pAcp.medDeptCd.Equals("ER"))
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

            if (clsType.User.AuAPRINTIN == "1")
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = true;
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
                                         ssUserChartHisIO, "V", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, "ROW", -1, 20, 3, "V");
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
        public frmTextEmr_NrActive1()
        {
            InitializeComponent();
        }

        public frmTextEmr_NrActive1(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
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
        public frmTextEmr_NrActive1(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        public frmTextEmr_NrActive1(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

 
        private void frmTextEmr_NrActFallBaby_Load(object sender, EventArgs e)
        {
            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            #region 틀고정
            ssUserChartHisIO_Sheet1.FrozenRowCount = 3;
            #endregion

            usFormTopMenuEvent.mbtnClear.Visible = true;
            usFormTopMenuEvent.mbtnPrint.Visible = true;
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

            ClearForm();

            btnSearchPT.Visible = READ_PTORDER();

            if (clsEmrPublic.gUserGrade.Equals("SIMSA"))
            {
                ssUserChartHisIO_Sheet1.Protect = false;
            }

            //btnPrint.Visible = false;

            if (mstrOrderDate.Length > 0 || mEmrCallForm == null)
                return;

            ClearChart();

            ssUserChartHis_Sheet1.RowCount = 0;
            if (pAcp.ptNo.Length > 0)
            {              
                GetChartHistory();
                ClearSp();

                GetDcur(pAcp.ptNo, usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString());
                if (ssUserChartHisIO_Sheet1.ColumnCount >= 3)
                {
                    GetLastValue(2, 0);
                }

                DesignFunc.DefaultDuty(ssUserChartIO, 2);
                ComboSelChagne(2, 1);

                READ_MSG1(pAcp.ptNo);

                string strFall = clsVbfunc.READ_WARNING_FALL(clsDB.DbCon, pAcp.ptNo, ComQuery.CurrentDateTime(clsDB.DbCon, "D"), -1, pAcp.age);
                string strBraden = READ_WARNING_BRADEN(pAcp.ptNo, ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "", pAcp.age, pAcp.ward, "");

                if (strBraden == "욕창위험")
                {
                    ssUserChartIO_Sheet1.Cells[3, 2].Text = ssUserChartIO_Sheet1.Cells[3, 2].Text.IndexOf("(고위험군)") == -1 ? ssUserChartIO_Sheet1.Cells[3, 2].Text + "(고위험군)" : ssUserChartIO_Sheet1.Cells[3, 2].Text;
                }
                else
                {
                    ssUserChartIO_Sheet1.Cells[3, 2].Text = "욕창점수";
                }

                if (strFall == "낙상위험" )
                {
                    ssUserChartIO_Sheet1.Cells[4, 2].Text = ssUserChartIO_Sheet1.Cells[4, 2].Text.IndexOf("(고위험군)") == -1 ? ssUserChartIO_Sheet1.Cells[4, 2].Text + "(고위험군)" : ssUserChartIO_Sheet1.Cells[4, 2].Text;
                }
                else
                {
                    ssUserChartIO_Sheet1.Cells[4, 2].Text = "낙상점수";
                }
            }
        }


        void GetDcur(string strPtNo, string strChartDate)
        {
            OracleDataReader dataReader = null;

            #region 1
            string SQL = "  SELECT Total, B.ACTDATE, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) ENTDATE";
            SQL = SQL + ComNum.VBLF + "   FROM ADMIN.NUR_BRADEN_SCALE B INNER JOIN ADMIN.IPD_NEW_MASTER M  ";
            SQL = SQL + ComNum.VBLF + "                    ON B.IPDNO = M.IPDNO  ";
            SQL = SQL + ComNum.VBLF + "                  AND M.PANO = '" + strPtNo + "' ";
            SQL = SQL + ComNum.VBLF + "                  AND TRUNC(B.ACTDATE) =  TO_DATE('" + strChartDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + " UNION ALL        ";
            SQL = SQL + ComNum.VBLF + "  SELECT Total, B.ACTDATE, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) ENTDATE";
            SQL = SQL + ComNum.VBLF + "   FROM ADMIN.NUR_BRADEN_SCALE_CHILD B INNER JOIN ADMIN.IPD_NEW_MASTER M  ";
            SQL = SQL + ComNum.VBLF + "                    ON B.IPDNO = M.IPDNO  ";
            SQL = SQL + ComNum.VBLF + "                  AND M.PANO = '" + strPtNo + "' ";
            SQL = SQL + ComNum.VBLF + "                  AND TRUNC(B.ACTDATE) =  TO_DATE('" + strChartDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + " UNION ALL        ";
            SQL = SQL + ComNum.VBLF + "  SELECT Total, B.ACTDATE, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) ENTDATE";
            SQL = SQL + ComNum.VBLF + "   FROM ADMIN.NUR_BRADEN_SCALE_BABY B INNER JOIN ADMIN.IPD_NEW_MASTER M  ";
            SQL = SQL + ComNum.VBLF + "                    ON B.IPDNO = M.IPDNO  ";
            SQL = SQL + ComNum.VBLF + "                  AND M.PANO = '" + strPtNo + "' ";
            SQL = SQL + ComNum.VBLF + "                  AND TRUNC(B.ACTDATE) =  TO_DATE('" + strChartDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, ENTDATE DESC ";

            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    ssUserChartIO_Sheet1.Cells[3, 3].Text = dataReader.GetValue(0).ToString().Trim() == "" ? "0" : dataReader.GetValue(0).ToString().Trim();
                }
            }
            dataReader.Dispose();
            #endregion

            #region 2
            if (VB.Val(pAcp.age) >= 18)
            {
                SQL = "  SELECT Total   ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.NUR_FALLMORSE_SCALE B INNER JOIN ADMIN.IPD_NEW_MASTER M  ";
                SQL = SQL + ComNum.VBLF + "                    ON B.IPDNO = M.IPDNO  ";
                SQL = SQL + ComNum.VBLF + "                  AND M.PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "                  AND TRUNC(B.ACTDATE) =  TO_DATE('" + strChartDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY B.ACTDATE DESC, DECODE(B.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), B.ENTDATE) DESC ";
            }
            else
            {
                SQL = "  SELECT Total   ";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.NUR_FALLHUMPDUMP_SCALE B INNER JOIN ADMIN.IPD_NEW_MASTER M  ";
                SQL = SQL + ComNum.VBLF + "                    ON B.IPDNO = M.IPDNO  ";
                SQL = SQL + ComNum.VBLF + "                  AND M.PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "                  AND TRUNC(B.ACTDATE) =  TO_DATE('" + strChartDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY B.ACTDATE DESC, DECODE(B.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), B.ENTDATE) DESC ";
            }

            sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    ssUserChartIO_Sheet1.Cells[4, 3].Text = dataReader.GetValue(0).ToString().Trim() == "" ? "0" : dataReader.GetValue(0).ToString().Trim();
                }
            }
            dataReader.Dispose();
            #endregion
        }


        bool READ_PTORDER()
        {
            string SQL = string.Empty;
            bool rtnVal = false;
            DataTable dt = null;

            SSPT_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE , TO_CHAR(CTIME, 'YYYY-MM-DD HH24:MI') CTIME, A.SUCODE, ORDERNAME";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_PTORDER A, ADMIN.OCS_ORDERCODE B";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND GBIO = 'I'";
                SQL = SQL + ComNum.VBLF + "   AND PANO= '" + pAcp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.SUCODE = B.SUCODE";
                SQL = SQL + ComNum.VBLF + " GROUP BY ACTDATE, CTIME, A.SUCODE, ORDERNAME";


                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SQL, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                SSPT_Sheet1.RowCount = dt.Rows.Count;
                SSPT_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i =0; i < dt.Rows.Count; i++)
                {
                    SSPT_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    SSPT_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    SSPT_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    SSPT_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CTIME"].ToString().Trim();

                }

                dt.Dispose();

                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
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

            for (int i = 3; i < ssUserChartIO_Sheet1.RowCount; i++)
            {
                for(int j = 2; j < ssUserChartIO_Sheet1.ColumnCount; j++)
                {
                    ssUserChartIO_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }
        }


        void READ_MSG1(string strPtno)
        {
            OracleDataReader dataReader = null;


            //jellco 간호활동 들어가간것 여부 확인
            string SQL = " SELECT PTNO";
            SQL = SQL + ComNum.VBLF + " From ADMIN.OCS_IORDER";
            SQL = SQL + ComNum.VBLF + " WHERE BDATE = TRUNC(SYSDATE)  ";
            SQL = SQL + ComNum.VBLF + " AND ORDERCODE IN (";
            SQL = SQL + ComNum.VBLF + " SELECT ORDERCODE FROM ADMIN.OCS_ORDERCODE";
            SQL = SQL + ComNum.VBLF + " WHERE SUCODE = 'KK059'";
            SQL = SQL + ComNum.VBLF + " AND SLIPNO = 'A6')";
            SQL = SQL + ComNum.VBLF + " AND SLIPNO = 'A6'";
            SQL = SQL + ComNum.VBLF + " AND PTNO = '" + strPtno + "' ";

            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dataReader.HasRows)
            {
                using (frmMSG_IV frm = new frmMSG_IV())
                {
                    frm.ShowDialog();
                }
            }

            dataReader.Dispose();


            //ICE BAG 간호활동 들어간것
            SQL = " SELECT PTNO";
            SQL = SQL + ComNum.VBLF + " From ADMIN.OCS_IORDER";
            SQL = SQL + ComNum.VBLF + " WHERE BDATE = TRUNC(SYSDATE)  ";
            SQL = SQL + ComNum.VBLF + " AND ORDERCODE IN (";
            SQL = SQL + ComNum.VBLF + " SELECT ORDERCODE FROM ADMIN.OCS_ORDERCODE";
            SQL = SQL + ComNum.VBLF + " WHERE SUCODE = 'IBC'";
            SQL = SQL + ComNum.VBLF + " AND SLIPNO = 'A6')";
            SQL = SQL + ComNum.VBLF + " AND SLIPNO = 'A6'";
            SQL = SQL + ComNum.VBLF + " AND PTNO = '" + strPtno + "' ";

            sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dataReader.HasRows)
            {
                using (frmMSG_ICE frm = new frmMSG_ICE())
                {
                    frm.ShowDialog();
                }
            }

            dataReader.Dispose();
        }

       

        string READ_WARNING_BRADEN(string strPtno, string strDate, string strIpdNo, string strAge, string strWard, string strDate2)
        {
            OracleDataReader dataReader = null;
            DataTable dt = null;
            string SQL = string.Empty;
            string sqlErr = string.Empty;

            string strBraden = string.Empty;
            string strOK = string.Empty;

            if (strIpdNo.Length == 0)
            {
                SQL = "SELECT IPDNO, WARDCODE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_MASTER     ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return strBraden;
                }

                if (dt.Rows.Count == 0)
                    return strBraden;

                strIpdNo = dt.Rows[0]["IPDNO"].ToString().Trim();
                strWard = dt.Rows[0]["WARDCODE"].ToString().Trim();

                dt.Dispose();

            }

            if(!VB.IsNumeric(strAge))
            {
                SQL = " SELECT AGE";
                SQL = SQL + ComNum.VBLF + " From ADMIN.IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY INDATE DESC";

                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return strBraden;
                }

                if (dataReader.HasRows && dataReader.Read())
                {
                    strAge = dataReader.GetValue(0).ToString().Trim();
                }

                dataReader.Dispose();
            }

            if (strAge.Length == 0)
                return strBraden;

            string strGUBUN = string.Empty;

            if (strWard == "NR" || strWard == "ND" || strWard == "IQ")
            {
                strGUBUN = "신생아";
            }
            else if(VB.Val(strAge) < 5)
            {
                strGUBUN = "소아";
            }
            else
            {
                strGUBUN = string.Empty;
            }

            if(strGUBUN.Length == 0)
            {
                SQL = " SELECT A.PANO, A.TOTAL, A.AGE "                                                                  ;
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE A"                                                ;
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + strIpdNo                                                        ;
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + strPtno + "' "                                                  ;
                if( strDate2.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                }


                SQL =SQL + ComNum.VBLF + "     AND A.TOTAL <= 18"                                                              ;
                SQL =SQL + ComNum.VBLF + "     AND A.ROWID = ("                                                                ;
                SQL =SQL + ComNum.VBLF + "   SELECT ROWID FROM ("                                                              ;
                SQL =SQL + ComNum.VBLF + "  SELECT * FROM ADMIN.NUR_BRADEN_SCALE"                                        ;
                SQL =SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')"                           ;
                SQL =SQL + ComNum.VBLF + "       AND IPDNO = " + strIpdNo                                                      ;
                SQL =SQL + ComNum.VBLF + "       AND PANO = '" + strPtno + "' "                                                ;
                SQL =SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)"                               ;
                SQL =SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return strBraden;
                }

                if (dt.Rows.Count > 0)
                {
                    double dAge = VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                    double dTotal = VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim());
                    if ((dAge >= 60 && dTotal <= 18) || 
                        dAge < 60 && dTotal <=16)
                    {
                        strBraden = "OK";
                    }
                }

                dt.Dispose();
            }
            else if(strGUBUN == "소아")
            {
                SQL = " SELECT TOTAL";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE_CHILD A";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + strIpdNo;
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + strPtno + "' ";
                if (strDate2.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                }


                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return strBraden;
                }

                if (dataReader.HasRows && dataReader.Read() && VB.Val(dataReader.GetValue(0).ToString().Trim()) <= 16)
                {
                    strOK = "OK";
                }

                dataReader.Dispose();
            }
            else if (strGUBUN == "신생아")
            {
                SQL = " SELECT TOTAL";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE_BABY A";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + strIpdNo;
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + strPtno + "' ";
                if (strDate2.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                }


                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return strBraden;
                }

                if (dataReader.HasRows && dataReader.Read() && VB.Val(dataReader.GetValue(0).ToString().Trim()) <= 20)
                {
                    strBraden = "OK";
                }

                dataReader.Dispose();
            }

            if(strBraden.Length == 0)
            {
                SQL = " SELECT *";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + strIpdNo;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( ";
                SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                SQL = SQL + ComNum.VBLF + "      )";

                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return strBraden;
                }

                if (dataReader.HasRows)
                {
                    strBraden = "OK";
                }

                dataReader.Dispose();
            }

            return strBraden == "OK" ? "욕창위험" : string.Empty;
        }

        void GetLastValue(int lngCol, int lngRow)
        {
            if(lngCol <= 1)
                return;

            string strDuty = ssUserChartHisIO_Sheet1.Cells[1, lngCol].Text.Trim();
            ssUserChartIO_Sheet1.Cells[1, 1].Text = strDuty;

            string strEmrNo = ssUserChartHisIO_Sheet1.Cells[46, lngCol].Text.Trim();

            int i = 0;
            int intCol = 0;

            for(int xCol1 = 0; xCol1 < ssUserChartHisIO_Sheet1.ColumnCount; xCol1++)
            {
                if(strEmrNo == ssUserChartHisIO_Sheet1.Cells[46, xCol1].Text.Trim())
                {
                    i += 1;
                    if(i == 1)
                    {
                        intCol = 2;
                    }
                    else if(i == 2)
                    {
                        intCol = 3;
                        i = 0;
                    }

                    for(int xRow = 3; xRow < 45; xRow++)
                    {
                        if(intCol == 3)
                        {
                            if(xRow == 3 || xRow == 4)
                            {

                            }
                            else
                            {
                                ssUserChartIO_Sheet1.Cells[xRow, intCol].Text = ssUserChartHisIO_Sheet1.Cells[xRow, xCol1].Text.Trim();
                            }
                        }
                        else
                        {
                            ssUserChartIO_Sheet1.Cells[xRow, intCol].Text = ssUserChartHisIO_Sheet1.Cells[xRow, xCol1].Text.Trim();
                        }
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
            ssUserChart_Sheet1.ColumnCount = lngStartCol;
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

            strVal += MkHaT(14, ssUserChartIO_Sheet1.Cells[1, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(15, ssUserChartIO_Sheet1.Cells[3, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(16, ssUserChartIO_Sheet1.Cells[3, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(17, ssUserChartIO_Sheet1.Cells[4, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(18, ssUserChartIO_Sheet1.Cells[4, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(19, ssUserChartIO_Sheet1.Cells[5, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(20, ssUserChartIO_Sheet1.Cells[5, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(21, ssUserChartIO_Sheet1.Cells[6, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(22, ssUserChartIO_Sheet1.Cells[6, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(23, ssUserChartIO_Sheet1.Cells[7, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(24, ssUserChartIO_Sheet1.Cells[7, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(25, ssUserChartIO_Sheet1.Cells[8, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(26, ssUserChartIO_Sheet1.Cells[8, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(27, ssUserChartIO_Sheet1.Cells[9, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(28, ssUserChartIO_Sheet1.Cells[9, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(29, ssUserChartIO_Sheet1.Cells[10, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(30, ssUserChartIO_Sheet1.Cells[10, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(31, ssUserChartIO_Sheet1.Cells[11, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(32, ssUserChartIO_Sheet1.Cells[11, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(33, ssUserChartIO_Sheet1.Cells[12, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(34, ssUserChartIO_Sheet1.Cells[12, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(35, ssUserChartIO_Sheet1.Cells[13, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(36, ssUserChartIO_Sheet1.Cells[13, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(37, ssUserChartIO_Sheet1.Cells[14, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(38, ssUserChartIO_Sheet1.Cells[14, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(39, ssUserChartIO_Sheet1.Cells[15, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(40, ssUserChartIO_Sheet1.Cells[15, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(41, ssUserChartIO_Sheet1.Cells[16, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(42, ssUserChartIO_Sheet1.Cells[16, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(43, ssUserChartIO_Sheet1.Cells[17, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(44, ssUserChartIO_Sheet1.Cells[17, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(45, ssUserChartIO_Sheet1.Cells[18, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(46, ssUserChartIO_Sheet1.Cells[18, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(47, ssUserChartIO_Sheet1.Cells[19, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(48, ssUserChartIO_Sheet1.Cells[19, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(49, ssUserChartIO_Sheet1.Cells[20, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(50, ssUserChartIO_Sheet1.Cells[20, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(51, ssUserChartIO_Sheet1.Cells[21, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(52, ssUserChartIO_Sheet1.Cells[21, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(53, ssUserChartIO_Sheet1.Cells[22, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(54, ssUserChartIO_Sheet1.Cells[22, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(55, ssUserChartIO_Sheet1.Cells[23, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(56, ssUserChartIO_Sheet1.Cells[23, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(57, ssUserChartIO_Sheet1.Cells[24, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(58, ssUserChartIO_Sheet1.Cells[24, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(59, ssUserChartIO_Sheet1.Cells[25, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(60, ssUserChartIO_Sheet1.Cells[25, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(61, ssUserChartIO_Sheet1.Cells[26, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(62, ssUserChartIO_Sheet1.Cells[26, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(63, ssUserChartIO_Sheet1.Cells[27, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(64, ssUserChartIO_Sheet1.Cells[27, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(65, ssUserChartIO_Sheet1.Cells[28, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(66, ssUserChartIO_Sheet1.Cells[28, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(67, ssUserChartIO_Sheet1.Cells[29, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(68, ssUserChartIO_Sheet1.Cells[29, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(69, ssUserChartIO_Sheet1.Cells[30, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(70, ssUserChartIO_Sheet1.Cells[30, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(71, ssUserChartIO_Sheet1.Cells[31, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(72, ssUserChartIO_Sheet1.Cells[31, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(73, ssUserChartIO_Sheet1.Cells[32, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(74, ssUserChartIO_Sheet1.Cells[32, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(75, ssUserChartIO_Sheet1.Cells[33, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(76, ssUserChartIO_Sheet1.Cells[33, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(77, ssUserChartIO_Sheet1.Cells[34, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(78, ssUserChartIO_Sheet1.Cells[34, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(79, ssUserChartIO_Sheet1.Cells[35, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(80, ssUserChartIO_Sheet1.Cells[35, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(81, ssUserChartIO_Sheet1.Cells[36, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(82, ssUserChartIO_Sheet1.Cells[36, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(83, ssUserChartIO_Sheet1.Cells[37, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(84, ssUserChartIO_Sheet1.Cells[37, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(85, ssUserChartIO_Sheet1.Cells[38, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(86, ssUserChartIO_Sheet1.Cells[38, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(87, ssUserChartIO_Sheet1.Cells[39, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(88, ssUserChartIO_Sheet1.Cells[39, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(89, ssUserChartIO_Sheet1.Cells[40, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(90, ssUserChartIO_Sheet1.Cells[40, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(91, ssUserChartIO_Sheet1.Cells[41, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(92, ssUserChartIO_Sheet1.Cells[41, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(93, ssUserChartIO_Sheet1.Cells[42, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(94, ssUserChartIO_Sheet1.Cells[42, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(95, ssUserChartIO_Sheet1.Cells[43, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(96, ssUserChartIO_Sheet1.Cells[43, 3].Text) + ComNum.VBLF;
            strVal += MkHaT(97, ssUserChartIO_Sheet1.Cells[44, 2].Text) + ComNum.VBLF;
            strVal += MkHaT(98, ssUserChartIO_Sheet1.Cells[44, 3].Text) + ComNum.VBLF;

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
                usFormTopMenuEvent.mbtnSave.Visible = true;
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


        /// <summary>
        /// VB => ConVIoData
        /// </summary>
        void ConVIoData()
        {

            string strPreDate = string.Empty;
            string strPreDuty = string.Empty;
            int intColorInfo = 1;

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
                ssConV_Sheet1.Cells[45, 0].Text = ssUserChartHis_Sheet1.Cells[xRow1, ssUserChartHis_Sheet1.ColumnCount - 1].Text.Trim();
                ssConV_Sheet1.Cells[45, 1].Text = ssUserChartHis_Sheet1.Cells[xRow1, ssUserChartHis_Sheet1.ColumnCount - 1].Text.Trim();

                //EMRNO
                ssConV_Sheet1.Cells[46, 0].Text = ssUserChartHis_Sheet1.Cells[xRow1, 11].Text.Trim();
                ssConV_Sheet1.Cells[46, 1].Text = ssUserChartHis_Sheet1.Cells[xRow1, 11].Text.Trim();

                //USEID
                ssConV_Sheet1.Cells[47, 0].Text = ssUserChartHis_Sheet1.Cells[xRow1, 12].Text.Trim();
                ssConV_Sheet1.Cells[47, 1].Text = ssUserChartHis_Sheet1.Cells[xRow1, 12].Text.Trim();

                if(strPreDate != strDate)
                {
                    intColorInfo *= -1;
                }

                SetIoHis(intColorInfo);

                strPreDate = strDate;
                strPreDuty = strDuty;
            }

            TextCellType textCellType = new TextCellType();
            textCellType.Static = true;
            ssUserChartHisIO_Sheet1.Cells[0, 0, ssUserChartHisIO_Sheet1.RowCount - 1, ssUserChartHisIO_Sheet1.ColumnCount - 1].CellType = textCellType;
            ssUserChartHisIO_Sheet1.Cells[0, 0, ssUserChartHisIO_Sheet1.RowCount - 1, ssUserChartHisIO_Sheet1.ColumnCount - 1].Locked = false;

        }

        void SetssConV(int lngCol, string strValue)
        {
            lngCol += 1;

            if (lngCol == 18)
                ssConV_Sheet1.Cells[3, 0].Text = strValue;
            else if (lngCol == 19)
                ssConV_Sheet1.Cells[3, 1].Text = strValue;
            else if (lngCol == 20)
                ssConV_Sheet1.Cells[4, 0].Text = strValue;
            else if (lngCol == 21)
                ssConV_Sheet1.Cells[4, 1].Text = strValue;
            else if (lngCol == 22)
                ssConV_Sheet1.Cells[5, 0].Text = strValue;
            else if (lngCol == 23)
                ssConV_Sheet1.Cells[5, 1].Text = strValue;
            else if (lngCol == 24)
                ssConV_Sheet1.Cells[6, 0].Text = strValue;
            else if (lngCol == 25)
                ssConV_Sheet1.Cells[6, 1].Text = strValue;
            else if (lngCol == 26)
                ssConV_Sheet1.Cells[7, 0].Text = strValue;
            else if (lngCol == 27)
                ssConV_Sheet1.Cells[7, 1].Text = strValue;
            else if (lngCol == 28)
                ssConV_Sheet1.Cells[8, 0].Text = strValue;
            else if (lngCol == 29)
                ssConV_Sheet1.Cells[8, 1].Text = strValue;
            else if (lngCol == 30)
                ssConV_Sheet1.Cells[9, 0].Text = strValue;
            else if (lngCol == 31)
                ssConV_Sheet1.Cells[9, 1].Text = strValue;
            else if (lngCol == 32)
                ssConV_Sheet1.Cells[10, 0].Text = strValue;
            else if (lngCol == 33)
                ssConV_Sheet1.Cells[10, 1].Text = strValue;
            else if (lngCol == 34)
                ssConV_Sheet1.Cells[11, 0].Text = strValue;
            else if (lngCol == 35)
                ssConV_Sheet1.Cells[11, 1].Text = strValue;
            else if (lngCol == 36)
                ssConV_Sheet1.Cells[12, 0].Text = strValue;
            else if (lngCol == 37)
                ssConV_Sheet1.Cells[12, 1].Text = strValue;
            else if (lngCol == 38)
                ssConV_Sheet1.Cells[13, 0].Text = strValue;
            else if (lngCol == 39)
                ssConV_Sheet1.Cells[13, 1].Text = strValue;
            else if (lngCol == 40)
                ssConV_Sheet1.Cells[14, 0].Text = strValue;
            else if (lngCol == 41)
                ssConV_Sheet1.Cells[14, 1].Text = strValue;
            else if (lngCol == 42)
                ssConV_Sheet1.Cells[15, 0].Text = strValue;
            else if (lngCol == 43)
                ssConV_Sheet1.Cells[15, 1].Text = strValue;
            else if (lngCol == 44)
                ssConV_Sheet1.Cells[16, 0].Text = strValue;
            else if (lngCol == 45)
                ssConV_Sheet1.Cells[16, 1].Text = strValue;
            else if (lngCol == 46)
                ssConV_Sheet1.Cells[17, 0].Text = strValue;
            else if (lngCol == 47)
                ssConV_Sheet1.Cells[17, 1].Text = strValue;
            else if (lngCol == 48)
                ssConV_Sheet1.Cells[18, 0].Text = strValue;
            else if (lngCol == 49)
                ssConV_Sheet1.Cells[18, 1].Text = strValue;
            else if (lngCol == 50)
                ssConV_Sheet1.Cells[19, 0].Text = strValue;
            else if (lngCol == 51)
                ssConV_Sheet1.Cells[19, 1].Text = strValue;
            else if (lngCol == 52)
                ssConV_Sheet1.Cells[20, 0].Text = strValue;
            else if (lngCol == 53)
                ssConV_Sheet1.Cells[20, 1].Text = strValue;
            else if (lngCol == 54)
                ssConV_Sheet1.Cells[21, 0].Text = strValue;
            else if (lngCol == 55)
                ssConV_Sheet1.Cells[21, 1].Text = strValue;
            else if (lngCol == 56)
                ssConV_Sheet1.Cells[22, 0].Text = strValue;
            else if (lngCol == 57)
                ssConV_Sheet1.Cells[22, 1].Text = strValue;
            else if (lngCol == 58)
                ssConV_Sheet1.Cells[23, 0].Text = strValue;
            else if (lngCol == 59)
                ssConV_Sheet1.Cells[23, 1].Text = strValue;
            else if (lngCol == 60)
                ssConV_Sheet1.Cells[24, 0].Text = strValue;
            else if (lngCol == 61)
                ssConV_Sheet1.Cells[24, 1].Text = strValue;
            else if (lngCol == 62)
                ssConV_Sheet1.Cells[25, 0].Text = strValue;
            else if (lngCol == 63)
                ssConV_Sheet1.Cells[25, 1].Text = strValue;
            else if (lngCol == 64)
                ssConV_Sheet1.Cells[26, 0].Text = strValue;
            else if (lngCol == 65)
                ssConV_Sheet1.Cells[26, 1].Text = strValue;
            else if (lngCol == 66)
                ssConV_Sheet1.Cells[27, 0].Text = strValue;
            else if (lngCol == 67)
                ssConV_Sheet1.Cells[27, 1].Text = strValue;
            else if (lngCol == 68)
                ssConV_Sheet1.Cells[28, 0].Text = strValue;
            else if (lngCol == 69)
                ssConV_Sheet1.Cells[28, 1].Text = strValue;
            else if (lngCol == 70)
                ssConV_Sheet1.Cells[29, 0].Text = strValue;
            else if (lngCol == 71)
                ssConV_Sheet1.Cells[29, 1].Text = strValue;
            else if (lngCol == 72)
                ssConV_Sheet1.Cells[30, 0].Text = strValue;
            else if (lngCol == 73)
                ssConV_Sheet1.Cells[30, 1].Text = strValue;
            else if (lngCol == 74)
                ssConV_Sheet1.Cells[31, 0].Text = strValue;
            else if (lngCol == 75)
                ssConV_Sheet1.Cells[31, 1].Text = strValue;
            else if (lngCol == 76)
                ssConV_Sheet1.Cells[32, 0].Text = strValue;
            else if (lngCol == 77)
                ssConV_Sheet1.Cells[32, 1].Text = strValue;
            else if (lngCol == 78)
                ssConV_Sheet1.Cells[33, 0].Text = strValue;
            else if (lngCol == 79)
                ssConV_Sheet1.Cells[33, 1].Text = strValue;
            else if (lngCol == 80)
                ssConV_Sheet1.Cells[34, 0].Text = strValue;
            else if (lngCol == 81)
                ssConV_Sheet1.Cells[34, 1].Text = strValue;
            else if (lngCol == 82)
                ssConV_Sheet1.Cells[35, 0].Text = strValue;
            else if (lngCol == 83)
                ssConV_Sheet1.Cells[35, 1].Text = strValue;
            else if (lngCol == 84)
                ssConV_Sheet1.Cells[36, 0].Text = strValue;
            else if (lngCol == 85)
                ssConV_Sheet1.Cells[36, 1].Text = strValue;
            else if (lngCol == 86)
                ssConV_Sheet1.Cells[37, 0].Text = strValue;
            else if (lngCol == 87)
                ssConV_Sheet1.Cells[37, 1].Text = strValue;
            else if (lngCol == 88)
                ssConV_Sheet1.Cells[38, 0].Text = strValue;
            else if (lngCol == 89)
                ssConV_Sheet1.Cells[38, 1].Text = strValue;
            else if (lngCol == 90)
                ssConV_Sheet1.Cells[39, 0].Text = strValue;
            else if (lngCol == 91)
                ssConV_Sheet1.Cells[39, 1].Text = strValue;
            else if (lngCol == 92)
                ssConV_Sheet1.Cells[40, 0].Text = strValue;
            else if (lngCol == 93)
                ssConV_Sheet1.Cells[40, 1].Text = strValue;
            else if (lngCol == 94)
                ssConV_Sheet1.Cells[41, 0].Text = strValue;
            else if (lngCol == 95)
                ssConV_Sheet1.Cells[41, 1].Text = strValue;
            else if (lngCol == 96)
                ssConV_Sheet1.Cells[42, 0].Text = strValue;
            else if (lngCol == 97)
                ssConV_Sheet1.Cells[42, 1].Text = strValue;
            else if (lngCol == 98)
                ssConV_Sheet1.Cells[43, 0].Text = strValue;
            else if (lngCol == 99)
                ssConV_Sheet1.Cells[43, 1].Text = strValue;
            else if (lngCol == 100)
                ssConV_Sheet1.Cells[44, 0].Text = strValue;
            else if (lngCol == 101)
                ssConV_Sheet1.Cells[44, 1].Text = strValue;
        }


        void SetIoHis(int intColorInfo)
        {
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            int lngMCol1 = ssUserChartHisIO_Sheet1.ColumnCount;
            ssUserChartHisIO_Sheet1.ColumnCount += 2;
            ssUserChartHisIO_Sheet1.Columns[ssUserChartHisIO_Sheet1.ColumnCount - 2, ssUserChartHisIO_Sheet1.ColumnCount - 1].Border = complexBorder1;


            for (int xCol1 = 0; xCol1 < ssConV_Sheet1.ColumnCount; xCol1++)
            {
                for (int xRow1 = 0; xRow1 < ssConV_Sheet1.RowCount; xRow1++)
                {
                    if (xCol1 == 0)
                    {
                        ssUserChartHisIO_Sheet1.Cells[xRow1, lngMCol1].Text = ssConV_Sheet1.Cells[xRow1, xCol1].Text.Trim();
                        ssUserChartHisIO_Sheet1.Columns[lngMCol1].Width = ssUserChartHisIO_Sheet1.Columns[lngMCol1].GetPreferredWidth() + 5;
                    }
                    else if(xCol1 == 1)
                    {
                        ssUserChartHisIO_Sheet1.Cells[xRow1, lngMCol1 + 1].Text = ssConV_Sheet1.Cells[xRow1, xCol1].Text.Trim();
                        ssUserChartHisIO_Sheet1.Columns[lngMCol1 + 1].Width = ssUserChartHisIO_Sheet1.Columns[lngMCol1 + 1].GetPreferredWidth() + 5;
                    }
                    else
                    {
                        ssUserChartHisIO_Sheet1.Cells[xRow1, lngMCol1].Text = ssConV_Sheet1.Cells[xRow1, xCol1].Text.Trim();
                        ssUserChartHisIO_Sheet1.Columns[lngMCol1].Width = ssUserChartHisIO_Sheet1.Columns[lngMCol1].GetPreferredWidth() + 5;
                    }
                }
            }

            ssUserChartHisIO_Sheet1.Columns[ssUserChartHisIO_Sheet1.ColumnCount - 2, ssUserChartHisIO_Sheet1.ColumnCount - 1].BackColor = intColorInfo == -1 ? System.Drawing.Color.White :
                                            ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("0xBFFFEF"));

            ssUserChartHisIO_Sheet1.AddSpanCell(0, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, 2);
            ssUserChartHisIO_Sheet1.AddSpanCell(1, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, 2);
            ssUserChartHisIO_Sheet1.AddSpanCell(45, ssUserChartHisIO_Sheet1.ColumnCount - 2, 1, 2);


            ssUserChartHisIO_Sheet1.Cells[0, ssUserChartHisIO_Sheet1.ColumnCount - 2].CellType = new TextCellType();
            ssUserChartHisIO_Sheet1.Cells[0, ssUserChartHisIO_Sheet1.ColumnCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssUserChartHisIO_Sheet1.Cells[0, ssUserChartHisIO_Sheet1.ColumnCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssUserChartHisIO_Sheet1.Cells[1, ssUserChartHisIO_Sheet1.ColumnCount - 2].CellType = new TextCellType();
            ssUserChartHisIO_Sheet1.Cells[1, ssUserChartHisIO_Sheet1.ColumnCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssUserChartHisIO_Sheet1.Cells[1, ssUserChartHisIO_Sheet1.ColumnCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            ssUserChartHisIO_Sheet1.Cells[45, ssUserChartHisIO_Sheet1.ColumnCount - 2].CellType = new TextCellType();
            ssUserChartHisIO_Sheet1.Cells[45, ssUserChartHisIO_Sheet1.ColumnCount - 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssUserChartHisIO_Sheet1.Cells[45, ssUserChartHisIO_Sheet1.ColumnCount - 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

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
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_PATIENT ";
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
                string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(10) + "출력일자 : " + ComQuery.CurrentDateTime(clsDB.DbCon, "D").Substring(0, 10) + VB.Space(10) + "출력자 : " + clsType.User.UserName;
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

            ssUserChartIO_Sheet1.Cells[1, 1].Text = strDuty;

            mEmrUseId = ssUserChartHisIO_Sheet1.Cells[47, e.Column].Text.Trim();

            string strEmrNo = ssUserChartHisIO_Sheet1.Cells[46, e.Column].Text.Trim();
            mstrEmrNo = strEmrNo;
            int i = 0;
            int intCol = 0;


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


            for (int xCol1 = 0; xCol1 < ssUserChartHisIO_Sheet1.ColumnCount; xCol1++)
            {
                if(strEmrNo == ssUserChartHisIO_Sheet1.Cells[46, xCol1].Text.Trim())
                {
                    i += 1;
                    if(i == 1)
                    {
                        intCol = 2;
                    }
                    else if(i == 2)
                    {
                        intCol = 3;
                        i = 0;
                    }

                    for (int xRow1 = 3; xRow1 < 45; xRow1++)
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
                for (int j = 2; j < ssUserChartIO_Sheet1.ColumnCount; j++)
                {
                    ssUserChartIO_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }

            DesignFunc.DefaultDuty(ssUserChartIO, 2);
            ssUserChartIO_Sheet1.Cells[3, 2].Text = "욕창점수";
            ssUserChartIO_Sheet1.Cells[4, 2].Text = "낙상점수";
            ssUserChartIO_Sheet1.Cells[5, 2].Text = "수면";
            ssUserChartIO_Sheet1.Cells[6, 2].Text = "활동";
            ssUserChartIO_Sheet1.Cells[14, 2].Text = "욕창유무";
            ssUserChartIO_Sheet1.Cells[21, 2].Text = "식욕";


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

            ClearSp();
            GetLastValue(ssUserChartHisIO_Sheet1.ActiveColumnIndex, 0);
        }

        private void FrmTextEmr_NrActFall_Resize(object sender, EventArgs e)
        {
            ssUserChartIO.Height = Height - panTopMenu.Height - panel1.Height - 100;

            ssUserChartHisIO.Left = ssUserChartIO.Visible ? 30 : 600;

            ssUserChartHisIO.Width  = Width - ssUserChartHisIO.Left;
            ssUserChartHisIO.Height = Height - panTopMenu.Height - panel1.Height - 100;

        }

        private void BtnSearchPT_Click(object sender, EventArgs e)
        {
            if(panPT.Visible == false)
            {
                READ_PTORDER();
                panPT.Visible = true;
            }
            else
            {
                panPT.Visible = false;
            }
        }

        private void SSPT_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SSPT_Sheet1.RowCount == 0)
                return;

            string strCTime = SSPT_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (strCTime.Length == 0)
                return;

            ssUserChartIO_Sheet1.Cells[7, 2].Text = "물리치료";
            ssUserChartIO_Sheet1.Cells[7, 3].Text = VB.Right(strCTime, 5);

            panPT.Visible = false;
        }


        void ComboSelChagne(int Col, int Row)
        {
            if (Col == 2 && Row == 1)
            {
                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString();
                string strDuty = ssUserChartIO_Sheet1.Cells[Row, Col].Text.Trim();

                ssUserChartIO_Sheet1.Cells[20, 2].Text = "";
                ssUserChartIO_Sheet1.Cells[22, 2].Text = "";

                READ_JELLCO(pAcp.ptNo, pAcp.medFrDate, strChartDate, strDuty);
                READ_NEBULIZER(pAcp.ptNo, pAcp.medFrDate, strChartDate, strDuty);
                READ_CATHETER(pAcp.ptNo, pAcp.medFrDate, strChartDate, strDuty);

                string strSql = " SELECT D.DIETDAY, D.BUN,  D.DIETNAME, D.QTY ";
                strSql = strSql + ComNum.VBLF + "   FROM ADMIN.DIET_NEWORDER D ";
                strSql = strSql + ComNum.VBLF + "WHERE D.ACTDATE = TO_DATE('" + strChartDate + "', 'YYYY-MM-DD') ";
                strSql = strSql + ComNum.VBLF + "     AND D.PANO = '" + pAcp.ptNo + "' ";
                strSql = strSql + ComNum.VBLF + "     AND D.BUN IN ('01','02','03','04') ";

                DataTable dt = null;
                string SqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                try
                {
                    string strDiet1 = string.Empty;
                    string strDiet2 = string.Empty;
                    string strDiet3 = string.Empty;

                    string strDiet1Etc = string.Empty;
                    string strDiet2Etc = string.Empty;
                    string strDiet3Etc = string.Empty;

                    double strDiet1Qty = 0;
                    double strDiet2Qty = 0;
                    double strDiet3Qty = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BUN"].ToString().Trim() != "04")
                        {
                            switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                            {
                                case "1":
                                    strDiet1 = dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    strDiet1Qty = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                    break;
                                case "2":
                                    strDiet2 = dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    strDiet2Qty = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                    break;
                                case "3":
                                    strDiet3 = dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    strDiet3Qty = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                    break;
                            }
                        }
                        else
                        {
                            switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                            {
                                case "1":
                                    strDiet1Etc = strDiet1 + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    break;
                                case "2":
                                    strDiet2Etc = strDiet2 + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    break;
                                case "3":
                                    strDiet3Etc = strDiet3 + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    break;

                            }
                        }
                    }

                    dt.Dispose();

                    if (strDuty == "Day")
                    {
                        ssUserChartIO_Sheet1.Cells[20, 2].Text =
                        (ssUserChartIO_Sheet1.Cells[20, 2].Text.Length == 0 ? strDiet1 : ssUserChartIO_Sheet1.Cells[20, 2].Text) +
                        (strDiet1Qty > 100 ? "(" + strDiet1Qty + ")" : "");

                        ssUserChartIO_Sheet1.Cells[22, 2].Text =
                        (ssUserChartIO_Sheet1.Cells[22, 2].Text.Length == 0 ? strDiet2 : ssUserChartIO_Sheet1.Cells[22, 2].Text) +
                        (strDiet2Qty > 100 ? "(" + strDiet2Qty + ")" : "");

                        ssUserChartIO_Sheet1.Cells[23, 2].Text = strDiet1Etc.Length != 0 ? "아)" + strDiet1Etc : "";
                        ssUserChartIO_Sheet1.Cells[23, 2].Text = ssUserChartIO_Sheet1.Cells[23, 2].Text + " " +
                            (strDiet2Etc.Length != 0 ? "점)" + strDiet2Etc : "");

                    }
                    else if (strDuty == "Evening")
                    {
                        ssUserChartIO_Sheet1.Cells[20, 2].Text =
                        (ssUserChartIO_Sheet1.Cells[20, 2].Text.Length == 0 ? strDiet3 : ssUserChartIO_Sheet1.Cells[20, 2].Text) +
                        (strDiet3Qty > 100 ? "(" + strDiet3Qty + ")" : "");

                        ssUserChartIO_Sheet1.Cells[23, 2].Text = strDiet3Etc.Length != 0 ? "저)" + strDiet3Etc : "";
                    }
                }

                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                }
            }
        }

        void READ_JELLCO(string argPTNO, string ArgInDate, string argChartDate, string ArgGubun)
        {
            ssUserChartIO_Sheet1.Cells[35, 2].Text = "";
            ssUserChartIO_Sheet1.Cells[35, 3].Text = "";

            ssUserChartIO_Sheet1.Cells[36, 2].Text = "";
            ssUserChartIO_Sheet1.Cells[36, 3].Text = "";

            string strCHARTsDATE = string.Empty;
            string strCHARTeDATE = string.Empty;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strOK = string.Empty;

            string strTEMP1 = string.Empty;
            string strTEMP2 = string.Empty;
            string strTEMP3 = string.Empty;

            ArgInDate = DateTime.ParseExact(ArgInDate, "yyyyMMdd", null).ToString("yyyy-MM-dd");

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
                        strCHARTsDATE = Convert.ToDateTime(argChartDate).AddDays(-1).ToString("yyyy-MM-dd") + " 23:00";
                        strCHARTeDATE = argChartDate + " " + "07:59";
                        break;
                }

                #region OK 확인
                SQL = " SELECT TO_CHAR(CHARTDATE, 'YYYY-MM-DD') CHARTDATE, TO_CHAR(CHARTDATE,'HH24:MI') CHARTTIME, CHARTDATE SORTTIME, GUBUN1, GUBUN2, KEEP ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_JELLCO ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + ArgInDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= TO_DATE('" + strCHARTsDATE + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= TO_DATE('" + strCHARTeDATE + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
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
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= TO_DATE('" + Convert.ToDateTime(argChartDate).AddDays(-7).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= TO_DATE('" + strCHARTsDATE + "','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORTTIME DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if(dt.Rows.Count > 0 )
                {
                   switch (dt.Rows[0]["GUBUN1"].ToString().Trim())
                    {
                        case "0":
                            strTEMP1 = "Arm";
                            strTEMP3 = dt.Rows[0]["KEEP"].ToString().Trim() == "0" ? "START" : "KEEP";
                            break;
                        case "1":
                            strTEMP1 = "Leg";
                            strTEMP3 = dt.Rows[0]["KEEP"].ToString().Trim() == "0" ? "START" : "KEEP";
                            break;
                        case "X":
                            strTEMP1 = "";
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
                }

                ssUserChartIO_Sheet1.Cells[35, 2].Text = strTEMP1.Length == 0 && strOK == "NO" ? "" : "Peripheral";

                ssUserChartIO_Sheet1.Cells[35, 3].Text = strTEMP1 + " " + strTEMP2;

                ssUserChartIO_Sheet1.Cells[36, 2].Text = strTEMP3;
                ssUserChartIO_Sheet1.Cells[36, 3].Text = strOK == "OK" ? dt.Rows[0]["CHARTDATE"].ToString().Trim() + " " +
                    dt.Rows[0]["CHARTTIME"].ToString().Trim() : "";

                dt.Dispose();
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }


        }

        void READ_NEBULIZER(string argPTNO, string ArgInDate, string argChartDate, string ArgGubun)
        {
            string strBDate = string.Empty;
            string strChartDate = string.Empty;
            string strChartTime = string.Empty;
            string strCODE = "'";

            string strCHARTsDATE = string.Empty;
            string strCHARTeDATE = string.Empty;


            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            string strTEMP1 = string.Empty;
            string strTEMP2 = string.Empty;
            string strTEMP3 = string.Empty;

            try
            {


                SQL = " SELECT CODE";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_흡입치료챠트연동코드'";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        strCODE += reader.GetValue(0).ToString().Trim() + "','";
                    }

                    strCODE = VB.Mid(strCODE, 1, strCODE.Length - 2);
                }

                reader.Dispose();

                ssUserChartIO_Sheet1.Cells[34, 2].Text = "";
                ssUserChartIO_Sheet1.Cells[34, 3].Text = "";

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
                        strCHARTsDATE = Convert.ToDateTime(argChartDate).AddDays(-1).ToShortDateString() + " " + "23:00";
                        strCHARTeDATE = argChartDate + " " + "07:59";
                        break;
                }

                SQL = "  SELECT A.ACTTIME"                                                                                                     ;
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_IORDER_ACT A,"                                                                       ;
                SQL = SQL + ComNum.VBLF + "   (SELECT C.PTNO, TRIM(extractValue(chartxml, '//it1')) BDATE, extractValue(chartxml, '//it2') SUCODE"    ;
                SQL = SQL + ComNum.VBLF + "     FROM ADMIN.EMRXML B, ADMIN.EMRXMLMST C"                                                     ;
                SQL = SQL + ComNum.VBLF + "     WHERE C.PTNO = '" + argPTNO + "'"                                                                     ;
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE >= '" + Convert.ToDateTime(argChartDate).AddDays(-1).ToString("yyyyMMdd") + "' "                                ;
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE <= '" + argChartDate.Replace("-", "") + "' "                                             ;
                SQL = SQL + ComNum.VBLF + "          AND C.FORMNO = 1796"                                                                             ;
                SQL = SQL + ComNum.VBLF + "          AND B.EMRNO = C.EMRNO"                                                                           ;
                SQL = SQL + ComNum.VBLF + "          AND TRIM(extractValue(chartxml, '//it2')) IN ('SALS','PULMI','ABEN-2A','MYST4A')) B"             ;
                SQL = SQL + ComNum.VBLF + "     WHERE Trim(a.PtNo) = b.PtNo"                                                                          ;
                SQL = SQL + ComNum.VBLF + "          AND B.BDATE = TO_CHAR(A.BDATE, 'YYYYMMDD')"                                                      ;
                SQL = SQL + ComNum.VBLF + "          AND TRIM(A.SUCODE) = B.SUCODE"                                                                   ;
                SQL = SQL + ComNum.VBLF + "          AND A.PTNO = '" + argPTNO + "'"                                                                  ;
                SQL = SQL + ComNum.VBLF + "          AND A.BDATE = TO_DATE('"  + strBDate + "','YYYY-MM-DD')"                                          ;
                SQL = SQL + ComNum.VBLF + "    AND ACTTIME >= TO_DATE('" + strCHARTsDATE + "','YYYY-MM-DD HH24:MI') "                                 ;
                SQL = SQL + ComNum.VBLF + "    AND ACTTIME <= TO_DATE('" + strCHARTeDATE + "','YYYY-MM-DD HH24:MI') "                                 ;
                SQL = SQL + ComNum.VBLF + "          AND A.SUCODE IN ('SALS','PULMI','ABEN-2A','MYST4A')"                                             ;
                SQL = SQL + ComNum.VBLF + "          AND A.DOSCODE IN ( SELECT DOSCODE FROM ADMIN.OCS_ODOSAGE"                                   ;
                SQL = SQL + ComNum.VBLF + "     WHERE UPPER(DOSNAME) LIKE '%INHAL%')"                                                                 ;
                SQL = SQL + ComNum.VBLF + "          ORDER BY ACTTIME ASC";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    ssUserChartIO_Sheet1.Cells[34, 2].Text = "Nebulizer";
                    ssUserChartIO_Sheet1.Cells[34, 3].Text = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        void READ_CATHETER(string argPTNO, string ArgInDate, string argChartDate, string ArgGubun)
        {
            ssUserChartIO_Sheet1.Cells[30, 2].Text = "";
            ssUserChartIO_Sheet1.Cells[30, 3].Text = "";

            string strCHARTsDATE = string.Empty;
            string strCHARTeDATE = string.Empty;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strOK = string.Empty;

            string strTEMP1 = string.Empty;
            string strTEMP2 = string.Empty;
            string strTEMP3 = string.Empty;

            string strInDate = VB.Left(ArgInDate, 4) + "-" + VB.Mid(ArgInDate, 5, 2) + "-" + VB.Right(ArgInDate, 2);

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
                        strCHARTsDATE = Convert.ToDateTime(argChartDate).AddDays(-1).ToString("yyyyMMdd") + " " + "23:00";
                        strCHARTeDATE = argChartDate + " " + "07:59";
                        break;
                }


                SQL = " SELECT CATH_O2, CATH_CLINE, CATH_FOLEY, CATH_DRESSINGDEPT, CATH_ETC ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_CATHETER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WRITEDATE DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    string strO2 = dt.Rows[0]["CATH_O2"].ToString().Trim();
                    string strCLINE = dt.Rows[0]["CATH_CLINE"].ToString().Trim();
                    string strFOLEY = dt.Rows[0]["CATH_FOLEY"].ToString().Trim();
                    string strDRESSINGDEPT = dt.Rows[0]["CATH_DRESSINGDEPT"].ToString().Trim();
                    string strETC = dt.Rows[0]["CATH_ETC"].ToString().Trim();

                    if (strO2 == "1")
                    {
                        ssUserChartIO_Sheet1.Cells[30, 2].Text = ReadO2Remark(argPTNO); //ReadO2Remark(argPTNO)
                        ssUserChartIO_Sheet1.Cells[30, 3].Text = "";
                    }

                    if (strCLINE == "1")
                    {
                        ssUserChartIO_Sheet1.Cells[35, 2].Text = ssUserChartIO_Sheet1.Cells[35, 2].Text.Length == 0 ? "Line종류선택요망!!!"
                            : ssUserChartIO_Sheet1.Cells[35, 2].Text.Trim();
                    }

                    if (strFOLEY == "1")
                    {
                        ssUserChartIO_Sheet1.Cells[39, 2].Text = ssUserChartIO_Sheet1.Cells[39, 2].Text.Length == 0 ? "F-Cath"
                            : ssUserChartIO_Sheet1.Cells[39, 2].Text.Trim();
                    }


                    if (strETC.Length > 0)
                    {
                        if(strFOLEY == "1")
                        {
                            ssUserChartIO_Sheet1.Cells[40, 2].Text = ssUserChartIO_Sheet1.Cells[40, 2].Text.Length == 0 ? strETC
                            : ssUserChartIO_Sheet1.Cells[40, 2].Text.Trim();
                        }
                        else
                        {
                            ssUserChartIO_Sheet1.Cells[39, 2].Text = ssUserChartIO_Sheet1.Cells[39, 2].Text.Length == 0 ? strETC
                            : ssUserChartIO_Sheet1.Cells[39, 2].Text.Trim();
                        }
                    }

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

        string ReadO2Remark(string argPTNO)
        {
            string rtnVal = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;


            try
            {

                SQL = "  SELECT REMARK";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "   WHERE BDATE =TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "     AND PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND REMARK IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "     AND ( REMARK LIKE '%산소%' OR REMARK LIKE 'O2 %'  OR REMARK LIKE '% O2%' OR REMARK LIKE '%O₂%') ";
                SQL = SQL + ComNum.VBLF + "     AND GBSTATUS NOT IN ('D','D-')";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        rtnVal += reader.GetValue(0).ToString().Trim() + ComNum.VBLF;
                    }
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        private void BtnExitFlu_Click(object sender, EventArgs e)
        {
            panEtc.Visible = false;
        }

        private void BtnApplyOut_Click(object sender, EventArgs e)
        {
            if(ssOut_Sheet1.Cells[0, 0].Text.Trim().Length == 0)
            {
                ComFunc.MsgBoxEx(this, "구분을 선택해 주십시오.");
                return;
            }

            string strOut = ssOut_Sheet1.Cells[0, 0].Text.Trim();
            strOut += " | " + ssOut_Sheet1.Cells[0, 1].Text.Trim();
            strOut += " | " + ssOut_Sheet1.Cells[0, 2].Text.Trim();

            ssUserChartIO_Sheet1.Cells[mlngOutRow, mlngOutCol].Text = strOut;
            panOut.Visible = false;
        }

        private void BtnCancelClear_Click(object sender, EventArgs e)
        {
            ssOut_Sheet1.Cells[0, 1, 0, 2].Text = string.Empty;
        }

        private void BtnExitOut_Click(object sender, EventArgs e)
        {
            panOut.Visible = false;
        }

        private void SsCath_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ssCath_Sheet1.Cells[0, 0].Text.Trim()))
                return;

            ssUserChartIO_Sheet1.Cells[mlngEtcRow, mlngEtcCol].Text = ssCath_Sheet1.Cells[0, 0].Text.Trim();
            panEtc.Visible = false;
        }

        private void SsTraction_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ssTraction_Sheet1.Cells[0, 0].Text.Trim()))
                return;

            ssUserChartIO_Sheet1.Cells[mlngEtcRow, mlngEtcCol].Text = ssTraction_Sheet1.Cells[0, 0].Text.Trim();
            panEtc.Visible = false;
        }

        private void SsUserChartIO_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            string[] strArrChr;

            if (e.Row >= 39 && e.Row <= 42)
            {
                if(e.Column == 2)
                {
                    if (e.Row == 39)
                        return;

                    mlngEtcCol = 2;
                    mlngEtcRow = e.Row;
                    panEtc.Visible = true;
                }
                else if(e.Column == 3)
                {
                    ssOut_Sheet1.Cells[0, 0, 0, 2].Text = "";

                    mlngOutCol = 3;
                    mlngOutRow = e.Row;
                    strArrChr = ssUserChartIO_Sheet1.Cells[e.Row, e.Column].Text.Trim().Split('|');

                    if (strArrChr.Length >= 0)
                    {
                        ssOut_Sheet1.Cells[0, 0].Text = strArrChr[0].Trim();

                    }
                    if (strArrChr.Length > 1)
                    {
                        ssOut_Sheet1.Cells[0, 1].Text = strArrChr[1].Trim();
                    }
                    if (strArrChr.Length > 2)
                    {
                        ssOut_Sheet1.Cells[0, 2].Text = strArrChr[2].Trim();
                    }

                    panOut.Visible = true;
                }
            }
            else if((e.Row == 11 || e.Row == 12 ||
                    e.Row >= 15 && e.Row <= 19 ||
                    e.Row == 24 || e.Row == 26 ||
                    e.Row == 28 || e.Row == 32 ||
                    e.Row == 36 || e.Row == 38 ||
                    e.Row == 43 || e.Row == 44) && e.Button == MouseButtons.Right)
            {
                if (e.Column != 3)
                    return;

                using (frmEmrTimeTable frmEmrTime = new frmEmrTimeTable(e.Row, e.Column))
                {
                    frmEmrTime.StartPosition = FormStartPosition.CenterParent;
                    frmEmrTime.rSendTime += FrmEmrTime_rSendTime;
                    frmEmrTime.ShowDialog(this);
                }
            }
        }

        private void FrmEmrTime_rSendTime(int Row, int Col, string Time)
        {
            ssUserChartIO_Sheet1.Cells[Row, Col].Text = Time;
        }

        private void ssUserChartIO_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (clsEmrPublic.gUserGrade == "SIMSA")
                return;


            ComboSelChagne(e.Column, e.Row);
        }

        private void btnSum_Click(object sender, EventArgs e)
        {
            btnSum.Text = btnSum.Text.Equals("늘리기") ? "줄이기" : "늘리기";

            for (int i = 3; i < 24; i++)
            {
                ssUserChartHisIO_Sheet1.Rows[i].Visible = btnSum.Text.Equals("줄이기");
            }
        }
    }
}
