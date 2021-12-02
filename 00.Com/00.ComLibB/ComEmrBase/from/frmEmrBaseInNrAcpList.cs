using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseInNrAcpList : Form
    {
        EmrPatient AcpEmr = null;

        //기록지 관련 : 작성된 기록지 호출
        public delegate void SetEmrAcpInfo(EmrPatient tAcp);
        public event SetEmrAcpInfo rSetEmrAcpInfo;

        /// <summary>
        /// 공용 굵은 폰트
        /// </summary>
        Font fontBold = null;

        public frmEmrBaseInNrAcpList()
        {
            InitializeComponent();
        }

        public frmEmrBaseInNrAcpList(EmrPatient _strPtno)
        {
            //건진 내시경 EMR 환자 자동 표시 용으로..... .... ..... ... .. . . . ..2021-04-27
            InitializeComponent();
            //strPTNO = _strPtno;
            AcpEmr = _strPtno;
        }

        private void frmEmrBaseInNrAcpList_Load(object sender, EventArgs e)
        {
            panNr.Dock = DockStyle.Fill;
            panEr.Dock = DockStyle.Fill;
            panEr.Visible = false;
            ssAipPatList_Sheet1.RowCount = 0;

            fontBold = new Font("굴림", 8, FontStyle.Bold);

            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            dtpORDDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCurDate, "D"));

            ComBoSet();
            SetSpreadFilter();

            panelHC.Visible = false;
            ssAipPatList_Sheet1.RowHeader.Visible = false;
            if (clsEmrQueryEtc.CheckHCBuse(clsType.User.BuseCode) == true)
            {
                controlInitHC("");
                if (AcpEmr != null)
                {
                    if (AcpEmr.ptNo != "")
                    {
                        txtHCPTNO.Text = AcpEmr.ptNo;
                        dtpHCDATE.Text = ComFunc.ChgDate(AcpEmr.medFrDate);
                    }
                }
                
            }

            GetPatList();
        }

        void controlInitHC(string arg)
        {
            //사용자가 건진일 경우 숨기는 콘트롤
            panel1.Visible = false;
            panel2.Visible = false;
            panelHC.Visible = true;
            chkEMR.Visible = false;
            btnPrintList.Visible = false;
            cboWard.Items.Clear();
            cboWard.Items.Add("HR");
            cboWard.SelectedIndex = 0;
            cboWard.Enabled = false;

            ssAipPatList_Sheet1.RowHeader.Visible = true;

            ssAipPatList_Sheet1.Columns[0].Visible = false;
            ssAipPatList_Sheet1.Columns[4].Visible = false;
            ssAipPatList_Sheet1.Columns[5].Visible = false;
            ssAipPatList_Sheet1.Columns[9].Visible = true;
            ssAipPatList_Sheet1.Columns[9].Width = 25;
            ssAipPatList_Sheet1.ColumnHeader.Cells[0, 9].Text = "진료과";
            ssAipPatList_Sheet1.Columns[10].Visible = false;
            ssAipPatList_Sheet1.Columns[11].Visible = false;

            ssAipPatList_Sheet1.ColumnCount = ssAipPatList_Sheet1.ColumnCount + 3;

            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 3).Label = "진정검사전";
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 3).Font = new System.Drawing.Font("굴림", 8F);
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 3).Locked = true;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 3).Width = 51F;

            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 2).Label = "모니터/진정평가";
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 2).Font = new System.Drawing.Font("굴림", 8F);
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 2).Locked = true;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 2).Width = 51F;

            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 1).Label = "회복평가/퇴실교육";
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 1).Font = new System.Drawing.Font("굴림", 8F);
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 1).Locked = true;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssAipPatList_Sheet1.Columns.Get(ssAipPatList_Sheet1.ColumnCount - 1).Width = 51F;

        }

       private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWard.Text.Trim() == "ER")
            {
                chkEROut.Visible = true;
                chkERLastDay.Visible = true;
                chkERNotComplete.Visible = true;
                ssAipPatList_Sheet1.ColumnHeader.Cells[0, 0].Text = "중증";
                panEr.Visible = true;
            }
            else
            {
                chkEROut.Visible = false;
                chkERLastDay.Visible = false;
                chkERNotComplete.Visible = false;
                ssAipPatList_Sheet1.ColumnHeader.Cells[0, 0].Text = "호실";
                panEr.Visible = false;
            }


            if (cboWard.Text.Trim().Equals("OP") || cboWard.Text.Trim().Equals("AG"))
            {
                cboJob.Items.Clear();
                cboJob.Items.Add("1.입원수술자명단");
                cboJob.Items.Add("2.외래수술자명단");
                cboJob.SelectedIndex = 0;
                cboDr.SelectedIndex = 0;
            }
            else
            {
                cboJob.Items.Clear();
                cboJob.Items.Add("1.재원자명단");
                cboJob.Items.Add("2.당일입원자");
                cboJob.Items.Add("F.어제입원자");
                cboJob.Items.Add("O.당일수술/시술환자");
                cboJob.Items.Add("3.퇴원예고자");
                cboJob.Items.Add("4.당일퇴원자");
                cboJob.Items.Add("5.응급실");
                cboJob.Items.Add("6.병동이실환자");
                cboJob.Items.Add("7.자가약대상자");
                cboJob.SelectedIndex = 0;
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strDeptCd = VB.Right(cboDept.Text.Trim(), 6).Trim();

            if (strDeptCd == "0")
            {
                strDeptCd = "";
            }
            cboDr.Items.Clear();
            cboDr.Items.Add("전  체" + VB.Space(50) + "0");

            try
            {
                SQL = " SELECT DRNAME,DRCODE FROM KOSMOS_OCS.OCS_DOCTOR  ";
                SQL = SQL + ComNum.VBLF + "WHERE GBOUT = 'N' ";
                if (strDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + strDeptCd + "' ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
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
                    cboDr.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["DRCODE"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                cboDr.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        #region //공통함수

        // 스프레드 필터기능
        private void SetSpreadFilter()
        {
            clsSpread methodSpd = new clsSpread();

            methodSpd.setSpdFilter(ssAipPatList, 0, AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(ssAipPatList, 1, AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(ssAipPatList, 2, AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(ssAipPatList, 3, AutoFilterMode.EnhancedContextMenu, true);

            //methodSpd.setSpdFilter(ssAipPatList, 4, AutoFilterMode.EnhancedContextMenu, true);
        }

        private void ComBoSet()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboNurAct.Items.Add("");
            cboNurAct.Items.Add("욕창고위험군" + VB.Space(50) + "욕창");
            cboNurAct.Items.Add("낙상고위험군" + VB.Space(50) + "낙상");
            cboNurAct.Items.Add("혈당측정" + VB.Space(50) + "NC1008");
            cboNurAct.Items.Add("측정 및 관찰 측정(섭취배설량 측정)" + VB.Space(50) + "NC1322");
            cboNurAct.Items.Add("측정 및 관찰 측정(신장 또는 체중측정)" + VB.Space(50) + "NC1321");
            cboNurAct.Items.Add("처치준비 및 보조(Simple  Dressing  )" + VB.Space(50) + "NC1269");
            cboNurAct.Items.Add("처치준비 및 보조(Sore Dressing 및 기록 )" + VB.Space(50) + "NC1268");
            cboNurAct.Items.Add("배설간호 (배뇨CAPD 투석액 교환)" + VB.Space(50) + "NC1272");
            cboNurAct.Items.Add("수술간호 (수술후 활력측정)" + VB.Space(50) + "NC1406");
            cboNurAct.Items.Add("측정 및 관찰 상태(통증관리) " + VB.Space(50) + "NC1289");
            cboNurAct.SelectedIndex = 0;

            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.SelectedIndex = 0;                       

            //진료과 세팅
            cboDept.Items.Clear();
            cboDept.Items.Add("전  체" + VB.Space(50) + "0");

            #region 쿼리
            SQL = ComNum.VBLF + "ORDER BY PRTGRD";
            dt = clsEmrQuery.GetMedDeptInfo(clsDB.DbCon, SQL);

            if (dt == null)
            {
                return;
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DEPTKORNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
                }
            }
            dt.Dispose();
            dt = null;
            cboDept.SelectedIndex = 0;

            int sIndex = -1;

            cboWard.Items.Add("전체");

            SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            string WardCodes = string.Empty;
            if (VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                WardCodes = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                    //if (dt.Rows[i]["MATCH_CODE"].ToString().Trim().Equals(clsType.User.BuseCode))
                    if (dt.Rows[i]["WardCode"].ToString().Trim().Equals(WardCodes))
                    {
                        sIndex = i;
                    }
                }
            }

            dt.Dispose();
            #endregion

            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("OP");
            cboWard.Items.Add("AG");
            cboWard.Items.Add("ENDO");
            cboWard.Items.Add("외래수혈");
            cboWard.Items.Add("혈액종양내과");
            cboWard.Items.Add("CT");
            cboWard.Items.Add("MRI");
            cboWard.Items.Add("RI");
            cboWard.Items.Add("SONO");
            cboWard.Items.Add("TO");
            cboWard.Items.Add("HR");

            cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;
            cboWard.Text = clsType.User.BuseCode.Equals("033124") ? "AG" : WardCodes;

            if (WardCodes.Trim().Equals("40"))
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("40");
                cboWard.Items.Add("4H");
                cboWard.SelectedIndex = 0;
                cboWard.Enabled = true;
            }
            else if (WardCodes.Trim().Equals("4H"))
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("4H");
                cboWard.Items.Add("40");
                cboWard.SelectedIndex = 0;
                cboWard.Enabled = true;
            }
            else if (WardCodes.Trim().Equals("OP") || WardCodes.Trim().Equals("AG"))
            {
                cboWard.Enabled = true;
            }

            bool Manager = NURSE_Manager_Check(clsType.User.IdNumber);

            if (Manager == true || clsVbfunc.GetBCodeCODE(clsDB.DbCon, "NUR_간호부관리자사번IP", "").Equals(clsCompuInfo.gstrCOMIP))
            {
                cboWard.Enabled = true;
            }

            if (cboWard.Text.Trim().Equals("OP") || cboWard.Text.Trim().Equals("AG"))
            {
                cboJob.Items.Clear();
                cboJob.Items.Add("1.입원수술자명단");
                cboJob.Items.Add("2.외래수술자명단");
                cboJob.SelectedIndex = 0;
            }
            else
            {
                cboJob.Items.Clear();
                cboJob.Items.Add("1.재원자명단");
                cboJob.Items.Add("2.당일입원자");
                cboJob.Items.Add("F.어제입원자");
                cboJob.Items.Add("O.당일수술/시술환자");
                cboJob.Items.Add("3.퇴원예고자");
                cboJob.Items.Add("4.당일퇴원자");
                cboJob.Items.Add("5.응급실");
                cboJob.Items.Add("6.병동이실환자");
                cboJob.Items.Add("7.자가약대상자");
                cboJob.Items.Add("L.C-Line 유지 환자");
                cboJob.Items.Add("M.항암제투여자");
                cboJob.SelectedIndex = 0;
            }

            #region 외래 일경우
            if (Manager == false && (clsType.User.BuseCode.Equals("033110") || clsType.User.BuseCode.Equals("044510")))
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("ENDO");
                cboWard.Items.Add("ENDO(ERCP)");
                cboWard.Items.Add("외래수혈");
                cboWard.Items.Add("혈액종양내과");
                cboWard.Items.Add("CT");
                cboWard.Items.Add("MRI");
                cboWard.Items.Add("RI");
                cboWard.Items.Add("SONO");
                cboWard.Enabled = true;
            }
            #endregion

            if (Manager == false && (clsType.User.BuseCode.Equals("044510") || clsType.User.BuseCode.Equals("044520")))
            {
                cboWard.Items.Clear();
                cboWard.Items.Add("ENDO");
                cboWard.SelectedIndex = 0;
            }

        }

        private bool NURSE_Manager_Check(string strSABUN)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "SELECT Code FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE Gubun='NUR_간호부관리자사번' ";
            SQL = SQL + ComNum.VBLF + "   AND Code= " + VB.Val(strSABUN) + " ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL    ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return false;
            }

            dt.Dispose();
            dt = null;
            return true;
        }

        private string READ_BAS_BCODE(string ArgGubun, string ArgCode)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            SQL = "";
            SQL = " SELECT NAME ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + "   WHERE GUBUN ='" + ArgGubun + "' ";
            SQL = SQL + ComNum.VBLF + "     AND CODE ='" + ArgCode + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return "";
            }
            rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        private bool READ_ER_GUBUN(string arg)
        {
            bool rtnVal = false;

            if (VB.InStr(1, arg, "ACS") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }

            if (VB.InStr(1, arg, "CVA") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }
            if (VB.InStr(1, arg, "TRA") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }
            if (VB.InStr(1, arg, "DIS") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }
            if (VB.InStr(1, arg, "A") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }
            if (VB.InStr(1, arg, "C") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }
            if (VB.InStr(1, arg, "T") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }
            if (VB.InStr(1, arg, "D") > 0)
            {
                rtnVal = true;
                return rtnVal;
            }

            return rtnVal;
        }

        private bool READ_COMPLETE(string argPTNO, string argMEDFRDATE, string argFORMNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            bool rtnVal = false;

            if (argPTNO == "")
            {
                argMEDFRDATE = VB.Replace(argMEDFRDATE, "-", "");
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT FORMNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_COMPLETE ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + argMEDFRDATE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = '" + argFORMNO + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                    return rtnVal;
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
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string Pat_TewonName_Chk(long ArgIPDNO)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_TEWON_NAMESEND  ";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + ArgIPDNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = "";

                if (dt1.Rows.Count > 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    rtnVal = "OK";
                    return rtnVal;
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string Read_ICU_Bed_Name(string arg, string argShort = "")
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT NAME, CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_ICU_침상번호' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE = '" + arg + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt1.Rows.Count > 0)
                {
                    if (argShort == "1")
                    {
                        rtnVal = dt1.Rows[0]["NAME"].ToString().Trim();
                        if (rtnVal.IndexOf("격리") >= 0)
                        {
                            rtnVal = rtnVal.Replace("격리", "격");
                        }

                        rtnVal = string.Format("{0})", rtnVal);
                    }
                    else
                    {
                        rtnVal = dt1.Rows[0]["NAME"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }
            
        #endregion

        #region //리스트 조회

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetPatList();
        }

        private void btnSearchOpt_Click(object sender, EventArgs e)
        {
            GetPatList();
        }

        private void GetPatList()
        {
            ssAipPatList_Sheet1.RowCount = 0;

            ssAipPatList_Sheet1.ColumnHeader.Cells[0, 4].Text = "DRG";
            ssAipPatList_Sheet1.ColumnHeader.Cells[0, 5].Text = "의사명";
            ssAipPatList_Sheet1.Columns[4].Width = 22;

            TextCellType txtCellType = new TextCellType();
            ssAipPatList_Sheet1.Columns[5].CellType = txtCellType;

            if (cboWard.Text.Trim() == "HD")
            {
                GetPatListHd();
            }
            else if (cboWard.Text.Trim() == "ER")
            {
                //ssAipPatList_Sheet1.Columns[0].Width = 6;

                ImageCellType imgCellType = new ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.Stretch;
                ssAipPatList_Sheet1.Columns[5].CellType = imgCellType;
                ssAipPatList_Sheet1.Columns[4].Label = " ";
                ssAipPatList_Sheet1.Columns[4].Width = 1;
                ssAipPatList_Sheet1.Columns[5].Label = "감염";
                GetPatListER();
            }
            else if (cboWard.Text.Trim() == "OP" || cboWard.Text.Trim() == "AG")
            {
                GetPatListOP();
            }
            else if (cboWard.Text.Trim() == "외래수혈")
            {
                GetPatListOOSu();
            }
            else if (cboWard.Text.Trim() == "혈액종양내과")
            {
                GetPatListCANCER();
            }
            else if (cboWard.Text.Trim() == "ENDO")
            {
                GetPatListENDO();
            }
            else if (cboWard.Text.Trim() == "ENDO(ERCP)")
            {
                GetPatListENDO_ERCP();
            }
            else if (cboWard.Text.Trim() == "CT")
            {
                GetPatListXRAY("CT");
            }
            else if (cboWard.Text.Trim() == "MRI")
            {
                GetPatListXRAY("MRI");
            }
            else if (cboWard.Text.Trim() == "RI")
            {
                GetPatListXRAY("RI");
            }
            else if (cboWard.Text.Trim() == "SONO")
            {
                GetPatListXRAY("SONO");
            }
            else if (cboWard.Text.Trim() == "TO")
            {
                GetPatListTO();
            }
            else if (cboWard.Text.Trim() == "HR")
            {
                //건진내시경EMR의 경우에 사용
                GetPatListHR();
            }
            else
            {
                if (VB.Left(cboJob.Text.Trim(), 1) == "6")
                {
                    GetPatListTran();
                }
                else if (VB.Left(cboJob.Text.Trim(), 1) == "7")
                {
                    GetPatListSelfMed();
                }
                else
                {
                    GetPatListIpd();
                }
            }
        }

        /// <summary>
        /// 일반검진 리스트
        /// </summary>
        private void GetPatListHR()
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strFormno = "";
            string strOrdername = "";

            ssAipPatList_Sheet1.RowCount = 0;

            //string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            //string strCurDate = VB.Left(strCurDateTime, 8);
            //string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') AS TDATE, PANO, A.SNAME, A.SEX, AGE,  'O' AS IPDOPD, A.DEPTCODE, ";
            SQL = SQL + ComNum.VBLF + "  (";
            SQL = SQL + ComNum.VBLF + "         SELECT LISTAGG(FORMNO, ',') WITHIN GROUP(ORDER BY FORMNO ASC) AS FORMNO ";
            SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_EMR.AEMRCHARTMST SUB ";
            SQL = SQL + ComNum.VBLF + "          WHERE FORMNO IN ('2431', '2429', '2433') ";
            SQL = SQL + ComNum.VBLF + "            AND SUB.PTNO = A.PANO ";
            SQL = SQL + ComNum.VBLF + "            AND SUB.MEDDEPTCD = A.DEPTCODE ";
            SQL = SQL + ComNum.VBLF + "            AND SUB.MEDFRDATE = TO_CHAR(A.BDATE, 'YYYYMMDD') ";
            SQL = SQL + ComNum.VBLF + "    ) FORMNO, ";
            SQL = SQL + ComNum.VBLF + "  (SELECT ORDERNAME FROM KOSMOS_OCS.OCS_ORDERCODE WHERE ORDERCODE = B.ORDERCODE AND ROWNUM = 1) ORDERNAME ";
            SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_OCS.ENDO_JUPMST B ";
            SQL = SQL + ComNum.VBLF + "  WHERE A.BDATE = TO_DATE('" + dtpHCDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE IN('TO','HR') ";
            SQL = SQL + ComNum.VBLF + "    AND B.BUSE = '044500' ";
            //SQL = SQL + ComNum.VBLF + "    AND B.GBSUNAP IN ('1','7') ";
            if (txtHCPTNO.Text.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = '" + txtHCPTNO.Text.Trim() + "' ";
            }
            if (rbtnHC1_1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND B.GBSUNAP = '1'";
            }
            else if (rbtnHC1_2.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND B.GBSUNAP = '7'";
            }
            SQL = SQL + ComNum.VBLF + "    AND A.BDATE = B.JDATE ";
            SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE = B.DEPTCODE ";
            SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PTNO ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY SNAME ";
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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
            ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 4].Text = "";
                ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                ssAipPatList_Sheet1.Cells[i, 6].Text = "";
                ssAipPatList_Sheet1.Cells[i, 7].Text = "";
                ssAipPatList_Sheet1.Cells[i, 8].Text = "";
                ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 11].Text = "";
                ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                strFormno = dt.Rows[i]["FORMNO"].ToString().Trim();

                if (strFormno.Contains("2429"))
                { ssAipPatList_Sheet1.Cells[i, ssAipPatList_Sheet1.ColumnCount - 3].Text = "●"; }
                if (strFormno.Contains("2431"))
                { ssAipPatList_Sheet1.Cells[i, ssAipPatList_Sheet1.ColumnCount - 2].Text = "●"; }
                if (strFormno.Contains("2433"))
                { ssAipPatList_Sheet1.Cells[i, ssAipPatList_Sheet1.ColumnCount - 1].Text = "●"; }

                strOrdername = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                if (strOrdername.Contains("수면"))
                {
                    ssAipPatList_Sheet1.Cells[i, 1].BackColor = Color.LightYellow;
                    ssAipPatList_Sheet1.Cells[i, 2].BackColor = Color.LightYellow;
                    ssAipPatList_Sheet1.Cells[i, 3].BackColor = Color.LightYellow;
                }
            }
            dt.Dispose();
            dt = null;
        }


        /// <summary>
        /// 종검 리스트
        /// </summary>
        private void GetPatListTO()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssAipPatList_Sheet1.RowCount = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            SQL = "";
            SQL = " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') AS TDATE, PTNO PANO, SNAME, SEX, AGE,  'O' AS IPDOPD";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HEA_JEPSU";
            SQL = SQL + ComNum.VBLF + "  WHERE SDATE = TO_DATE('" + strToDate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "ORDER BY WRTNO, SNAME ";

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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
            ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 4].Text = "";
                ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                ssAipPatList_Sheet1.Cells[i, 6].Text = "";
                ssAipPatList_Sheet1.Cells[i, 7].Text = "";
                ssAipPatList_Sheet1.Cells[i, 8].Text = "";
                ssAipPatList_Sheet1.Cells[i, 9].Text = "TO";
                ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 11].Text = "";
                ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void GetPatListHd()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssAipPatList_Sheet1.RowCount = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);
            string strJob = VB.Left(cboJob.Text.Trim(), 1);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            SQL = "";
            SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') AS TDATE, PANO, SNAME, SEX, AGE,  'O' AS IPDOPD";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            SQL = SQL + ComNum.VBLF + "  WHERE DEPTCODE = 'HD'";
            SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
            SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + dtpORDDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "UNION ALL    ";
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(TDATE,'YYYY-MM-DD') AS TDATE, PANO, SNAME, SEX, AGE, IPDOPD ";
            SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "TONG_HD_DAILY ";
            SQL = SQL + ComNum.VBLF + "WHERE TDATE = TO_DATE('" + dtpORDDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "AND IPDOPD = 'I'";
            SQL = SQL + ComNum.VBLF + "ORDER BY IPDOPD , SNAME ";

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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
            ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 4].Text = "";
                ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                ssAipPatList_Sheet1.Cells[i, 6].Text = "";
                ssAipPatList_Sheet1.Cells[i, 7].Text = "";
                ssAipPatList_Sheet1.Cells[i, 8].Text = "";
                ssAipPatList_Sheet1.Cells[i, 9].Text = "HD";
                ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 11].Text = "";
                ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void GetPatListER()
        {
            DataTable dt = null;
            string SQL      = string.Empty;    //Query문
            string SqlErr   = string.Empty; //에러문 받는 변수

            ssAipPatList_Sheet1.RowCount = 0;

            string strFDate = dtpORDDATE.Value.AddDays(-1).ToString("yyyy-MM-dd");
            string strTDate = dtpORDDATE.Value.ToString("yyyy-MM-dd");
            string strTDate2 = dtpORDDATE.Value.AddDays(-2).ToString("yyyy-MM-dd");

            SQL = "";
            SQL = " SELECT 'O' AS IPDOPD,  Pano as Ptno,SName,Sex,Age,";
            SQL = SQL + ComNum.VBLF + " DrCode,Bi,Chojae GbChojae, DeptCode,GbSpc,TO_CHAR(JTime,'YYYY-MM-DD HH24:MI') JTime1,         ";
            SQL = SQL + ComNum.VBLF + "Jin, Bunup, PNEUMONIA, ERPATIENT, TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, OCSJIN, TOI_GUBUN, ER_NUM   ";
            SQL = SQL + ComNum.VBLF + ", " + ComNum.DB_MED + "FC_EXAM_INFECT_MASTER_IMG_EX(K.PANO, K.BDATE) FC_INFECT ";    //감염   
            SQL = SQL + ComNum.VBLF + ",  (SELECT NAME                       ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE          ";
            SQL = SQL + ComNum.VBLF + "   WHERE GUBUN ='ETC_응급실환자구역'       ";
            SQL = SQL + ComNum.VBLF + "     AND CODE = K.ER_NUM              ";
            SQL = SQL + ComNum.VBLF + "   ) AS ETC_ER                        ";

            SQL = SQL + ComNum.VBLF + " , CASE WHEN EXISTS(                               ";
            SQL = SQL + ComNum.VBLF + "  SELECT 1                                         ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_COMPLETE                      ";
            SQL = SQL + ComNum.VBLF + " WHERE PTNO = K.PANO                   ";
            SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = TO_CHAR(K.BDATE, 'YYYYMMDD')    ";
            SQL = SQL + ComNum.VBLF + "   AND FORMNO = 2049                               ";
            SQL = SQL + ComNum.VBLF + " 	) THEN 1 END READ_COMPLETE                    ";


            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER K  ";
            if (chkERUse.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "WHERE BDate >= TO_DATE('" + dtpESDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + dtpEEDATE.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                if (optER1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SNAME LIKE '%" + txtSNAME.Text.Trim() + "%' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtSNAME.Text.Trim() + "' ";
                }

                if (rdoRoom1.Checked == true)  //전체
                {
                }
                else if (rdoRoom2.Checked == true)  //'관찰
                {
                    SQL = SQL + ComNum.VBLF + "     AND K.ER_NUM IN ('71','72','73','74','61','62','63','64','65','66','67','68','69','70','43','44','45','46','91','92','99','98','03') ";
                }
                else if (rdoRoom3.Checked == true)  //'중증
                {
                    SQL = SQL + ComNum.VBLF + "     AND K.ER_NUM IN ('81','82','83','84','85','86','75','76','77','78') ";
                }
                else if (rdoRoom4.Checked == true)  //'소아
                {
                    SQL = SQL + ComNum.VBLF + "     AND K.ER_NUM IN ('52','53','54','55','56','57','A0','A1','A2','A3','A4','A5','A6','A7','A8') ";
                }
                else if (rdoRoom5.Checked == true)  //'격리
                {
                    SQL = SQL + ComNum.VBLF + "     AND K.ER_NUM IN ('47','48','49','58','59') ";
                }

            }
            else
            {
                if (chkERLastDay.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BDate >= TO_DATE('" + strTDate2 + "', 'YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE BDate = TO_DATE('" + strTDate + "', 'YYYY-MM-DD')";
                }
            }
            SQL = SQL + ComNum.VBLF + "AND DeptCode IN ('EM' ,'ER')      ";
            SQL = SQL + ComNum.VBLF + "AND Jin IN ('0','1','2','3','4','5','6','F','R','S')  ";
            SQL = SQL + ComNum.VBLF + "AND EXISTS (  ";
            SQL = SQL + ComNum.VBLF + "              SELECT a.Pano,b.SName,TO_CHAR(a.JDate,'YYYY-MM-DD') JDate,  ";
            SQL = SQL + ComNum.VBLF + "               TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate,a.EntSabun  ";
            SQL = SQL + ComNum.VBLF + "               FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "BAS_PATIENT b   ";
            SQL = SQL + ComNum.VBLF + "              WHERE a.JDate = K.BDATE ";
            SQL = SQL + ComNum.VBLF + "                AND a.Pano=b.Pano(+)   ";

            if (chkERUse.Checked == true)
            {
            }
            else
            {
                if (chkEROut.Checked == true)
                {
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                AND A.OutTime IS NULL ";
                }
            }

            SQL = SQL + ComNum.VBLF + "                AND A.PANO = K.Pano )  ";
            if (chkERNotComplete.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " AND NOT EXISTS ";
                SQL = SQL + ComNum.VBLF + " (SELECT * ";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_EMR.EMR_COMPLETE E";
                SQL = SQL + ComNum.VBLF + "  WHERE E.MEDFRDATE = TO_CHAR(K.BDATE,'YYYYMMDD')";
                SQL = SQL + ComNum.VBLF + "       AND K.PANO = E.PTNO) ";
            }

            //조회기간중 입원인 환자
            if (VB.Left(cboJob.Text.Trim(), 1) == "2")
            {
                SQL = SQL + ComNum.VBLF + " AND OCSJIN = '#' ";
                SQL = SQL + ComNum.VBLF + " AND PANO IN ( SELECT PANO FROM  " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "                WHERE GBSTS = '0' ";
                SQL = SQL + ComNum.VBLF + "                AND INDATE BETWEEN TO_DATE('" + strFDate + "', 'YYYY-MM-DD')  AND TO_DATE('" + strTDate + "', 'YYYY-MM-DD') ) ";
            }
            SQL = SQL + ComNum.VBLF + "   ORDER BY DECODE(ERPATIENT, NULL, 2, 1)";

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
                Cursor.Current = Cursors.Default;
                return;
            }

            ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
            ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ETC_ER"].ToString().Trim();
                //ssAipPatList_Sheet1.Cells[i, 0].Text = READ_BAS_BCODE("ETC_응급실환자구역", dt.Rows[i]["ER_NUM"].ToString().Trim());

                //if (ssAipPatList_Sheet1.Cells[i, 0].Text == "" || VB.Len(ssAipPatList_Sheet1.Cells[i, 0].Text) == 2)
                //{
                switch (dt.Rows[i]["ERPATIENT"].ToString().Trim())
                {
                    case "A":
                        ssAipPatList_Sheet1.Cells[i, 0].Text = "ACS)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                        break;
                    case "C":
                        ssAipPatList_Sheet1.Cells[i, 0].Text = "CVA)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                        break;
                    case "T":
                        ssAipPatList_Sheet1.Cells[i, 0].Text = "TRA)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                        break;
                    case "D":
                        ssAipPatList_Sheet1.Cells[i, 0].Text = "DIS)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                        break;
                }
                //}
                //else
                //{
                //    switch (dt.Rows[i]["ERPATIENT"].ToString().Trim())
                //    {
                //        case "A":
                //            ssAipPatList_Sheet1.Cells[i, 0].Text = "A)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                //            break;
                //        case "C":
                //            ssAipPatList_Sheet1.Cells[i, 0].Text = "C)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                //            break;
                //        case "T":
                //            ssAipPatList_Sheet1.Cells[i, 0].Text = "T)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                //            break;
                //        case "D":
                //            ssAipPatList_Sheet1.Cells[i, 0].Text = "D)" + ssAipPatList_Sheet1.Cells[i, 0].Text;
                //            break;
                //    }
                //}

                string strPtNo = dt.Rows[i]["Ptno"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 1].Text = strPtNo;
                ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();

                ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex1(clsDB.DbCon, strPtNo) + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TOI_GUBUN"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                ssAipPatList_Sheet1.Cells[i, 6].Text = Convert.ToDateTime(dt.Rows[i]["JTime1"].ToString().Trim()).ToString("yyyy-MM-dd");

                ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                //SET_Infection("", strPtNo, dtpORDDATE.Checked, 6, ssAipPatList.Row, ssAipPatList, Picture1, imgInfect);
                ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 10].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");

                ssAipPatList_Sheet1.Cells[i, 5].Value = InfactResource(dt.Rows[i]["Fc_infect"].ToString().Trim()); //감염

                //SET_Infection(i, strPtNo, ssAipPatList_Sheet1.Cells[i, 10].Text);

                if (READ_ER_GUBUN(ssAipPatList_Sheet1.Cells[i, 0].Text.Trim()))
                {
                    ssAipPatList_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 255, 0);
                    ssAipPatList_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 255, 0);
                }

                //if (strPtNo == "03586070")
                //{
                //    strPtNo = strPtNo;
                //}

                if (dt.Rows[i]["READ_COMPLETE"].ToString().Trim().Equals("1"))
                {
                    ssAipPatList_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(200, 200, 255);
                    ssAipPatList_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 200, 255);
                    ssAipPatList_Sheet1.Cells[i, 4].BackColor = Color.FromArgb(200, 200, 255);
                }
                else
                {
                    ssAipPatList_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(255, 255, 255);
                    ssAipPatList_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(255, 255, 255);
                    ssAipPatList_Sheet1.Cells[i, 4].BackColor = Color.FromArgb(255, 255, 255);
                }
            }
            dt.Dispose();
            dt = null;
        }

        #region 감염정보 이미지 LOad
        /// <summary>
        /// 감염정보 이미지 Load
        /// </summary>
        /// <param name="sGubun"></param>
        /// <returns></returns>
        private object InfactResource(string sGubun)
        {
            // 1    2    3    4    5    6    7
            //혈액 접촉 격리 공기 비말  보호 해외
            object rtnVal = null;
            if (sGubun == "0000001")
            {
                rtnVal = Properties.Resources.I0000001;
            }
            else if (sGubun == "0000010")
            {
                rtnVal = Properties.Resources.I0000010;
            }
            else if (sGubun == "0000011")
            {
                rtnVal = Properties.Resources.I0000011;
            }
            else if (sGubun == "0000100")
            {
                rtnVal = Properties.Resources.I0000100;
            }
            else if (sGubun == "0000101")
            {
                rtnVal = Properties.Resources.I0000101;
            }
            else if (sGubun == "0000110")
            {
                rtnVal = Properties.Resources.I0000110;
            }
            else if (sGubun == "0000111")
            {
                rtnVal = Properties.Resources.I0000111;
            }
            else if (sGubun == "0001000")
            {
                rtnVal = Properties.Resources.I0001000;
            }
            else if (sGubun == "0001001")
            {
                rtnVal = Properties.Resources.I0001001;
            }
            else if (sGubun == "0001010")
            {
                rtnVal = Properties.Resources.I0001010;
            }
            else if (sGubun == "0001011")
            {
                rtnVal = Properties.Resources.I0001011;
            }
            else if (sGubun == "0001100")
            {
                rtnVal = Properties.Resources.I0001100;
            }
            else if (sGubun == "0001101")
            {
                rtnVal = Properties.Resources.I0001101;
            }
            else if (sGubun == "0001110")
            {
                rtnVal = Properties.Resources.I0001110;
            }
            else if (sGubun == "0001111")
            {
                rtnVal = Properties.Resources.I0001111;
            }
            else if (sGubun == "0010000")
            {
                rtnVal = Properties.Resources.I0010000;
            }
            else if (sGubun == "0010001")
            {
                rtnVal = Properties.Resources.I0010001;
            }
            else if (sGubun == "0010010")
            {
                rtnVal = Properties.Resources.I0010010;
            }
            else if (sGubun == "0010011")
            {
                rtnVal = Properties.Resources.I0010011;
            }
            else if (sGubun == "0010100")
            {
                rtnVal = Properties.Resources.I0010100;
            }
            else if (sGubun == "0010101")
            {
                rtnVal = Properties.Resources.I0010101;
            }
            else if (sGubun == "0010110")
            {
                rtnVal = Properties.Resources.I0010110;
            }
            else if (sGubun == "0010111")
            {
                rtnVal = Properties.Resources.I0010111;
            }
            else if (sGubun == "0011000")
            {
                rtnVal = Properties.Resources.I0011000;
            }
            else if (sGubun == "0011001")
            {
                rtnVal = Properties.Resources.I0011001;
            }
            else if (sGubun == "0011010")
            {
                rtnVal = Properties.Resources.I0011010;
            }
            else if (sGubun == "0011011")
            {
                rtnVal = Properties.Resources.I0011011;
            }
            else if (sGubun == "0011100")
            {
                rtnVal = Properties.Resources.I0011100;
            }
            else if (sGubun == "0011101")
            {
                rtnVal = Properties.Resources.I0011101;
            }
            else if (sGubun == "0011110")
            {
                rtnVal = Properties.Resources.I0011110;
            }
            else if (sGubun == "0011111")
            {
                rtnVal = Properties.Resources.I0011111;
            }
            else if (sGubun == "0100000")
            {
                rtnVal = Properties.Resources.I0100000;
            }
            else if (sGubun == "0100001")
            {
                rtnVal = Properties.Resources.I0100001;
            }
            else if (sGubun == "0100010")
            {
                rtnVal = Properties.Resources.I0100010;
            }
            else if (sGubun == "0100011")
            {
                rtnVal = Properties.Resources.I0100011;
            }
            else if (sGubun == "0100100")
            {
                rtnVal = Properties.Resources.I0100100;
            }
            else if (sGubun == "0100101")
            {
                rtnVal = Properties.Resources.I0100101;
            }
            else if (sGubun == "0100110")
            {
                rtnVal = Properties.Resources.I0100110;
            }
            else if (sGubun == "0100111")
            {
                rtnVal = Properties.Resources.I0100111;
            }
            else if (sGubun == "0101000")
            {
                rtnVal = Properties.Resources.I0101000;
            }
            else if (sGubun == "0101001")
            {
                rtnVal = Properties.Resources.I0101001;
            }
            else if (sGubun == "0101010")
            {
                rtnVal = Properties.Resources.I0101010;
            }
            else if (sGubun == "0101011")
            {
                rtnVal = Properties.Resources.I0101011;
            }
            else if (sGubun == "0101100")
            {
                rtnVal = Properties.Resources.I0101100;
            }
            else if (sGubun == "0101101")
            {
                rtnVal = Properties.Resources.I0101101;
            }
            else if (sGubun == "0101110")
            {
                rtnVal = Properties.Resources.I0101110;
            }
            else if (sGubun == "0101111")
            {
                rtnVal = Properties.Resources.I0101111;
            }
            else if (sGubun == "0110000")
            {
                rtnVal = Properties.Resources.I0110000;
            }
            else if (sGubun == "0110001")
            {
                rtnVal = Properties.Resources.I0110001;
            }
            else if (sGubun == "0110010")
            {
                rtnVal = Properties.Resources.I0110010;
            }
            else if (sGubun == "0110011")
            {
                rtnVal = Properties.Resources.I0110011;
            }
            else if (sGubun == "0110100")
            {
                rtnVal = Properties.Resources.I0110100;
            }
            else if (sGubun == "0110101")
            {
                rtnVal = Properties.Resources.I0110101;
            }
            else if (sGubun == "0110110")
            {
                rtnVal = Properties.Resources.I0110110;
            }
            else if (sGubun == "0110111")
            {
                rtnVal = Properties.Resources.I0110111;
            }
            else if (sGubun == "0111000")
            {
                rtnVal = Properties.Resources.I0111000;
            }
            else if (sGubun == "0111001")
            {
                rtnVal = Properties.Resources.I0111001;
            }
            else if (sGubun == "0111010")
            {
                rtnVal = Properties.Resources.I0111010;
            }
            else if (sGubun == "0111011")
            {
                rtnVal = Properties.Resources.I0111011;
            }
            else if (sGubun == "0111100")
            {
                rtnVal = Properties.Resources.I0111100;
            }
            else if (sGubun == "0111101")
            {
                rtnVal = Properties.Resources.I0111101;
            }
            else if (sGubun == "0111110")
            {
                rtnVal = Properties.Resources.I0111110;
            }
            else if (sGubun == "0111111")
            {
                rtnVal = Properties.Resources.I0111111;
            }
            else if (sGubun == "1000000")
            {
                rtnVal = Properties.Resources.I1000000;
            }
            else if (sGubun == "1000001")
            {
                rtnVal = Properties.Resources.I1000001;
            }
            else if (sGubun == "1000010")
            {
                rtnVal = Properties.Resources.I1000010;
            }
            else if (sGubun == "1000011")
            {
                rtnVal = Properties.Resources.I1000011;
            }
            else if (sGubun == "1000100")
            {
                rtnVal = Properties.Resources.I1000100;
            }
            else if (sGubun == "1000101")
            {
                rtnVal = Properties.Resources.I1000101;
            }
            else if (sGubun == "1000110")
            {
                rtnVal = Properties.Resources.I1000110;
            }
            else if (sGubun == "1000111")
            {
                rtnVal = Properties.Resources.I1000111;
            }
            else if (sGubun == "1001000")
            {
                rtnVal = Properties.Resources.I1001000;
            }
            else if (sGubun == "1001001")
            {
                rtnVal = Properties.Resources.I1001001;
            }
            else if (sGubun == "1001010")
            {
                rtnVal = Properties.Resources.I1001010;
            }
            else if (sGubun == "1001011")
            {
                rtnVal = Properties.Resources.I1001011;
            }
            else if (sGubun == "1001100")
            {
                rtnVal = Properties.Resources.I1001100;
            }
            else if (sGubun == "1001101")
            {
                rtnVal = Properties.Resources.I1001101;
            }
            else if (sGubun == "1001110")
            {
                rtnVal = Properties.Resources.I1001110;
            }
            else if (sGubun == "1001111")
            {
                rtnVal = Properties.Resources.I1001111;
            }
            else if (sGubun == "1010000")
            {
                rtnVal = Properties.Resources.I1010000;
            }
            else if (sGubun == "1010001")
            {
                rtnVal = Properties.Resources.I1010001;
            }
            else if (sGubun == "1010010")
            {
                rtnVal = Properties.Resources.I1010010;
            }
            else if (sGubun == "1010011")
            {
                rtnVal = Properties.Resources.I1010011;
            }
            else if (sGubun == "1010100")
            {
                rtnVal = Properties.Resources.I1010100;
            }
            else if (sGubun == "1010101")
            {
                rtnVal = Properties.Resources.I1010101;
            }
            else if (sGubun == "1010110")
            {
                rtnVal = Properties.Resources.I1010110;
            }
            else if (sGubun == "1010111")
            {
                rtnVal = Properties.Resources.I1010111;
            }
            else if (sGubun == "1011000")
            {
                rtnVal = Properties.Resources.I1011000;
            }
            else if (sGubun == "1011001")
            {
                rtnVal = Properties.Resources.I1011001;
            }
            else if (sGubun == "1011010")
            {
                rtnVal = Properties.Resources.I1011010;
            }
            else if (sGubun == "1011011")
            {
                rtnVal = Properties.Resources.I1011011;
            }
            else if (sGubun == "1011100")
            {
                rtnVal = Properties.Resources.I1011100;
            }
            else if (sGubun == "1011101")
            {
                rtnVal = Properties.Resources.I1011101;
            }
            else if (sGubun == "1011110")
            {
                rtnVal = Properties.Resources.I1011110;
            }
            else if (sGubun == "1011111")
            {
                rtnVal = Properties.Resources.I1011111;
            }
            else if (sGubun == "1100000")
            {
                rtnVal = Properties.Resources.I1100000;
            }
            else if (sGubun == "1100001")
            {
                rtnVal = Properties.Resources.I1100001;
            }
            else if (sGubun == "1100010")
            {
                rtnVal = Properties.Resources.I1100010;
            }
            else if (sGubun == "1100011")
            {
                rtnVal = Properties.Resources.I1100011;
            }
            else if (sGubun == "1100100")
            {
                rtnVal = Properties.Resources.I1100100;
            }
            else if (sGubun == "1100101")
            {
                rtnVal = Properties.Resources.I1100101;
            }
            else if (sGubun == "1100110")
            {
                rtnVal = Properties.Resources.I1100110;
            }
            else if (sGubun == "1100111")
            {
                rtnVal = Properties.Resources.I1100111;
            }
            else if (sGubun == "1101000")
            {
                rtnVal = Properties.Resources.I1101000;
            }
            else if (sGubun == "1101001")
            {
                rtnVal = Properties.Resources.I1101001;
            }
            else if (sGubun == "1101010")
            {
                rtnVal = Properties.Resources.I1101010;
            }
            else if (sGubun == "1101011")
            {
                rtnVal = Properties.Resources.I1101011;
            }
            else if (sGubun == "1101100")
            {
                rtnVal = Properties.Resources.I1101100;
            }
            else if (sGubun == "1101101")
            {
                rtnVal = Properties.Resources.I1101101;
            }
            else if (sGubun == "1101110")
            {
                rtnVal = Properties.Resources.I1101110;
            }
            else if (sGubun == "1101111")
            {
                rtnVal = Properties.Resources.I1101111;
            }
            else if (sGubun == "1110000")
            {
                rtnVal = Properties.Resources.I1110000;
            }
            else if (sGubun == "1110001")
            {
                rtnVal = Properties.Resources.I1110001;
            }
            else if (sGubun == "1110010")
            {
                rtnVal = Properties.Resources.I1110010;
            }
            else if (sGubun == "1110011")
            {
                rtnVal = Properties.Resources.I1110011;
            }
            else if (sGubun == "1110100")
            {
                rtnVal = Properties.Resources.I1110100;
            }
            else if (sGubun == "1110101")
            {
                rtnVal = Properties.Resources.I1110101;
            }
            else if (sGubun == "1110110")
            {
                rtnVal = Properties.Resources.I1110110;
            }
            else if (sGubun == "1110111")
            {
                rtnVal = Properties.Resources.I1110111;
            }
            else if (sGubun == "1111000")
            {
                rtnVal = Properties.Resources.I1111000;
            }
            else if (sGubun == "1111001")
            {
                rtnVal = Properties.Resources.I1111001;
            }
            else if (sGubun == "1111010")
            {
                rtnVal = Properties.Resources.I1111010;
            }
            else if (sGubun == "1111011")
            {
                rtnVal = Properties.Resources.I1111011;
            }
            else if (sGubun == "1111100")
            {
                rtnVal = Properties.Resources.I1111100;
            }
            else if (sGubun == "1111101")
            {
                rtnVal = Properties.Resources.I1111101;
            }
            else if (sGubun == "1111110")
            {
                rtnVal = Properties.Resources.I1111110;
            }
            else if (sGubun == "1111111")
            {
                rtnVal = Properties.Resources.I1111111;
            }

            return rtnVal;
        }
        #endregion


        private void SET_Infection(int Row, string strPtno, string strIndate)
        {
            OracleDataReader reader = null;

            try
            {
                string SQL = " SELECT KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(A.PANO, " + "TO_DATE('" + strIndate  + "', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_PATIENT A";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPtno + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                }

                ////혈액
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 1, 1) == "1")
                //{
                //    lblBLOOD1.Visible = false;
                //    picBLOOD1.Visible = true;
                //    picBLOOD1.Image = Properties.Resources.I00100;
                //}

                ////접촉1
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 2, 1) == "1")
                //{
                //    lblCONTACT1.Visible = false;
                //    picCONTACT1.Visible = true;
                //    picCONTACT1.Image = Properties.Resources.I01000;
                //}

                ////접촉2(격리)
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 3, 1) == "1")
                //{
                //    lblCONTACT1.Visible = false;
                //    picCONTACT1.Visible = true;
                //    picCONTACT1.Image = Properties.Resources.I00001;
                //}

                ////공기
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 4, 1) == "1")
                //{
                //    lblAIR1.Visible = false;
                //    picAIR1.Visible = true;
                //    picAIR1.Image = Properties.Resources.I10000;
                //}

                ////비말
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 5, 1) == "1")
                //{
                //    lblBIMAL1.Visible = false;
                //    picBIMAL1.Visible = true;
                //    picBIMAL1.Image = Properties.Resources.I00010;
                //}

                ////보호
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 6, 1) == "1")
                //{
                //    lblProtect1.Visible = false;
                //    picProtect1.Visible = true;
                //    picProtect1.Image = Properties.Resources.보호;
                //}

                ////해외
                //if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 7, 1) == "1")
                //{
                //    lblForegin1.Visible = false;
                //    picForegin1.Visible = true;
                //    picForegin1.Image = Properties.Resources.해외;
                //}

                if (reader.HasRows && reader.Read())
                {
                    string strInfect = reader.GetValue(0).ToString().Trim();
                    if (strInfect.Substring(0, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.혈액;
                    }
                    else if (strInfect.Substring(1, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.접촉1;
                    }
                    else if (strInfect.Substring(2, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.접촉2_격리;
                    }
                    else if (strInfect.Substring(3, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.공기;
                    }
                    else if (strInfect.Substring(4, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.비말;
                    }
                    else if (strInfect.Substring(5, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.보호;
                    }
                    else if (strInfect.Substring(6, 1).Equals("1"))
                    {
                        ImageCellType imgCellType = new ImageCellType();
                        ssAipPatList_Sheet1.Cells[Row, 5].CellType = imgCellType;
                        ssAipPatList_Sheet1.Cells[Row, 5].Value = Properties.Resources.해외;
                    }
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private void GetPatListOP()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssAipPatList_Sheet1.RowCount = 0;

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);
            string strJob = VB.Left(cboJob.Text.Trim(), 1);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = dtpORDDATE.Value.ToString("yyyy-MM-dd");
            //(VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            SQL = "";

            if (strJob.Equals("2"))
            {
                SQL = " SELECT 'O' ROOMCODE, M.Pano, M.SName, M.Sex, M.Age, TO_CHAR(M.BDATE,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + "  '' GBSTS, '' GBDRG, M.DeptCode, M.DrCode, D.DrName, M.EMR";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(B.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY A.FORMNO, A.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM B                                            ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO   = B.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND B.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_ERP.HR_EMP_BASIS E                                        ";
                SQL = SQL + ComNum.VBLF + "      ON A.CHARTUSEID = E.EMP_ID                                               ";
                SQL = SQL + ComNum.VBLF + "     AND E.JOBKIND_CD = '41'                                                   ";
                SQL = SQL + ComNum.VBLF + "WHERE A.MEDFRDATE  =  TO_CHAR(M.BDATE, 'YYYYMMDD')                             ";
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO       = M.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND A.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.FORMNO, A.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";

                SQL = SQL + ComNum.VBLF + "  FROM   KOSMOS_PMPA.OPD_MASTER  M,";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_PMPA.BAS_PATIENT P,";
                SQL = SQL + ComNum.VBLF + "  KOSMOS_PMPA.BAS_DOCTOR d";
                SQL = SQL + ComNum.VBLF + "  WHERE M.PANO IN (";
                SQL = SQL + ComNum.VBLF + "                                     SELECT PANO";
                SQL = SQL + ComNum.VBLF + "                                     From KOSMOS_PMPA.ORAN_MASTER";
                SQL = SQL + ComNum.VBLF + "                                    WHERE OPDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";

                if( cboWard.Text.Trim() == "AG")
                {
                    SQL = SQL + ComNum.VBLF + "                                     AND GBANGIO = 'Y' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                     AND GBANGIO = 'N' ";
                }

                if (VB.Left(cboDept.Text, 10).Trim() != "전  체")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + VB.Right(cboDept.Text, 6).Trim() + "' ";
                }

                if (VB.Left(cboDr.Text, 10).Trim() != "전  체")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + VB.Right(cboDr.Text, 5).Trim() + "' ";
                }

                SQL = SQL + ComNum.VBLF + "                                      AND M.DEPTCODE = DEPTCODE";
                SQL = SQL + ComNum.VBLF + "                                 )";
                SQL = SQL + ComNum.VBLF + " AND (M.BDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR M.BDate>= TO_DATE('" + strToDate + "','YYYY-MM-DD') )";
                SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000'";
                SQL = SQL + ComNum.VBLF + " AND M.Pano=P.Pano(+)";
                SQL = SQL + ComNum.VBLF + " AND M.DrCode=D.DrCode(+)";
                SQL = SQL + ComNum.VBLF + " AND M.REP <> '#'";
                SQL = SQL + ComNum.VBLF + " ORDER BY SName, INDATE DESC";
            }
            else
            {
                SQL = " SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts, M.GBDRG,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate, M.DeptCode,M.DrCode,D.DrName,M.AmSet1, ";
                SQL = SQL + ComNum.VBLF + " M.AmSet4,M.AmSet6,M.AmSet7, M.EMR   ";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(B.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY A.FORMNO, A.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM B                                            ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO   = B.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND B.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_ERP.HR_EMP_BASIS E                                        ";
                SQL = SQL + ComNum.VBLF + "      ON A.CHARTUSEID = E.EMP_ID                                               ";
                SQL = SQL + ComNum.VBLF + "     AND E.JOBKIND_CD = '41'                                                   ";
                SQL = SQL + ComNum.VBLF + "WHERE A.MEDFRDATE  =  TO_CHAR(M.INDATE, 'YYYYMMDD')                            ";
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO       = M.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND A.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.FORMNO, A.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M,          ";
                SQL = SQL + ComNum.VBLF + " KOSMOS_PMPA.BAS_PATIENT P,          ";
                SQL = SQL + ComNum.VBLF + " KOSMOS_PMPA.BAS_DOCTOR  D   ";
                SQL = SQL + ComNum.VBLF + " WHERE M.PANO IN (   ";
                SQL = SQL + ComNum.VBLF + "                                    SELECT PANO  ";
                SQL = SQL + ComNum.VBLF + "                                    FROM KOSMOS_PMPA.ORAN_MASTER  ";
                SQL = SQL + ComNum.VBLF + "                                    WHERE OPDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";

                if (cboWard.Text.Trim() == "AG")
                {
                    SQL = SQL + ComNum.VBLF + "                                     AND GBANGIO = 'Y' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "                                     AND GBANGIO = 'N' ";
                }

                if (VB.Left(cboDept.Text, 10).Trim() != "전  체")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + VB.Right(cboDept.Text, 6).Trim() + "' ";
                }

                if (VB.Left(cboDr.Text, 10).Trim() != "전  체")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + VB.Right(cboDr.Text, 5).Trim() + "' ";
                }

                SQL = SQL + ComNum.VBLF + "                                )   ";
                //SQL = SQL + ComNum.VBLF + "AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.GBSTS <> '7') OR M.OutDate>= TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                SQL = SQL + ComNum.VBLF + "AND (M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR M.OutDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ) ";
                SQL = SQL + ComNum.VBLF + "AND M.Pano < '90000000'   ";
                SQL = SQL + ComNum.VBLF + "AND M.GbSTS <> '9'    ";
                SQL = SQL + ComNum.VBLF + "AND M.Pano=P.Pano(+)    ";
                SQL = SQL + ComNum.VBLF + "AND M.DrCode=D.DrCode(+)  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY M.RoomCode,M.SName, M.Indate DESC   ";
            }



            //SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') AS TDATE, PANO, SNAME, SEX, AGE,  'O' AS IPDOPD";
            //SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER";
            //SQL = SQL + ComNum.VBLF + "  WHERE DEPTCODE = 'HD'";
            //SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
            //SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + dtpORDDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            //SQL = SQL + ComNum.VBLF + "UNION ALL    ";
            //SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(TDATE,'YYYY-MM-DD') AS TDATE, PANO, SNAME, SEX, AGE, IPDOPD ";
            //SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "TONG_HD_DAILY ";
            //SQL = SQL + ComNum.VBLF + "WHERE TDATE = TO_DATE('" + dtpORDDATE.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            //SQL = SQL + ComNum.VBLF + "AND IPDOPD = 'I'";
            //SQL = SQL + ComNum.VBLF + "ORDER BY IPDOPD , SNAME ";

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
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }

            ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
            ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "[DRG]": "");
                ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                ssAipPatList_Sheet1.Cells[i, 9].Text = ssAipPatList_Sheet1.Cells[i, 7].Text;
                ssAipPatList_Sheet1.Cells[i, 10].Text = ssAipPatList_Sheet1.Cells[i, 6].Text;
                NOT_CERT_CHART_LIST(i, dt.Rows[i]["NOTE"].ToString().Trim());
                ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //TEXT EMR대상자
                {
                    ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                    ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Blue;
                }
                if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7") //퇴원완료자
                {
                    ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                    ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void GetPatListOOSu()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strDate = dtpORDDATE.Value.ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE, 'YYYY-MM-DD') AS TDATE, PANO, SNAME, SEX, AGE,  'O' AS IPDOPD, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER M  ";
                SQL = SQL + ComNum.VBLF + "WHERE  JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')   ";
                SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "      AND EXISTS ( SELECT *  ";
                SQL = SQL + ComNum.VBLF + "                              FROM " + ComNum.DB_MED + "EXAM_BLOODCROSSM C  ";
                SQL = SQL + ComNum.VBLF + "                                 WHERE C.PANO = M.PANO  ";
                SQL = SQL + ComNum.VBLF + "                                 AND TRUNC(C.JUBSUDATE) = TRUNC(M.BDATE)  ";
                SQL = SQL + ComNum.VBLF + "                                 AND C.GBSTATUS <> '3'  )  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY IPDOPD , SNAME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = "";
                        ssAipPatList_Sheet1.Cells[i, 6].Text = "";
                        ssAipPatList_Sheet1.Cells[i, 7].Text = "";
                        ssAipPatList_Sheet1.Cells[i, 8].Text = "";
                        ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim(); //"HD";
                        ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetPatListENDO()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, GBDRG, B.DRNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(BB.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY AA.FORMNO, AA.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST AA                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM BB                                            ";
                SQL = SQL + ComNum.VBLF + "      ON AA.FORMNO   = BB.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND AA.UPDATENO = BB.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND BB.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "WHERE AA.MEDFRDATE  =  TO_CHAR(A.INDATE, 'YYYYMMDD')                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.PTNO       = A.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND AA.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY AA.FORMNO, AA.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND GBSTS <> '7') OR OUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "    AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBJOB NOT IN ('4')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'I')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT 0 ROOMCODE, PANO, SNAME, AGE, SEX, '' GBDRG, B.DRNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'O' GUBUN";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(BB.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY AA.FORMNO, AA.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST AA                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM BB                                            ";
                SQL = SQL + ComNum.VBLF + "      ON AA.FORMNO   = BB.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND AA.UPDATENO = BB.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND BB.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "WHERE AA.MEDFRDATE  =  TO_CHAR(A.BDATE, 'YYYYMMDD')                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.PTNO       = A.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND AA.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY AA.FORMNO, AA.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  Where a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBJOB NOT IN ('4')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'O')";
                
                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN, SNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "[DRG]" : "";// dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        NOT_CERT_CHART_LIST(i, dt.Rows[i]["NOTE"].ToString().Trim()); 
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        if (clsEmrQueryEtc.ReadHCEndoGubun(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["INDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim()) == "수면")
                        {
                            ssAipPatList_Sheet1.Cells[i, 1].BackColor = Color.LightYellow;
                            ssAipPatList_Sheet1.Cells[i, 2].BackColor = Color.LightYellow;
                            ssAipPatList_Sheet1.Cells[i, 3].BackColor = Color.LightYellow;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetPatListENDO_ERCP()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, GBDRG, B.DRNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(BB.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY AA.FORMNO, AA.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST AA                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM BB                                            ";
                SQL = SQL + ComNum.VBLF + "      ON AA.FORMNO   = BB.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND AA.UPDATENO = BB.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND BB.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "WHERE AA.MEDFRDATE  =  TO_CHAR(A.INDATE, 'YYYYMMDD')                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.PTNO       = A.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND AA.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY AA.FORMNO, AA.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND GBSTS <> '7') OR OUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "    AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBJOB  IN ('4')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'I')";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT 0 ROOMCODE, PANO, SNAME, AGE, SEX, '' GBDRG, B.DRNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'O' GUBUN";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(BB.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY AA.FORMNO, AA.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST AA                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM BB                                            ";
                SQL = SQL + ComNum.VBLF + "      ON AA.FORMNO   = BB.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND AA.UPDATENO = BB.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND BB.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "WHERE AA.MEDFRDATE  =  TO_CHAR(A.BDATE, 'YYYYMMDD')                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.PTNO       = A.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND AA.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND AA.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY AA.FORMNO, AA.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  Where a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT A1.PTNO";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "ENDO_JUPMST A1,";
                SQL = SQL + ComNum.VBLF + "          " + ComNum.DB_MED + "OCS_ORDERCODE b1";
                SQL = SQL + ComNum.VBLF + "    Where A1.OrderCode = b1.OrderCode";
                SQL = SQL + ComNum.VBLF + "          AND A1.BDATE < TRUNC (SYSDATE + 1)";
                SQL = SQL + ComNum.VBLF + "          AND (B1.SLIPNO = '0044' OR B1.SLIPNO = '0064' OR B1.SLIPNO = '0105')";
                SQL = SQL + ComNum.VBLF + "          AND (A1.BUSE IS NULL OR A1.BUSE = '056104')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE >= TO_DATE ('" + strToDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.RDATE < TO_DATE ('" + strNextDate + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBSUNAP IN ('1', '7')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBJOB  IN ('4')";
                SQL = SQL + ComNum.VBLF + "          AND A1.GBIO = 'O')";

                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN, SNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "[DRG]" : "";// dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        NOT_CERT_CHART_LIST(i, dt.Rows[i]["NOTE"].ToString().Trim());
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        if (clsEmrQueryEtc.ReadHCEndoGubun(dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["INDATE"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim()) == "수면")
                        {
                            ssAipPatList_Sheet1.Cells[i, 1].BackColor = Color.LightYellow;
                            ssAipPatList_Sheet1.Cells[i, 2].BackColor = Color.LightYellow;
                            ssAipPatList_Sheet1.Cells[i, 3].BackColor = Color.LightYellow;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        private void GetPatListXRAY(string Gubun)
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strGUBUN = string.Empty;


            switch (Gubun.Trim())
            {
                case "CT":
                    strGUBUN = "4";
                    break;
                case "MRI":
                    strGUBUN = "5";
                    break;
                case "RI":
                    strGUBUN = "6";
                    break;
                case "SONO":
                    strGUBUN = "3";
                    break;
            }

            ssAipPatList_Sheet1.RowCount = 0;


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, GBDRG, B.DRNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND A.GBSTS <> '7') OR OUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "    AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "                   SELECT PANO ";
                SQL = SQL + ComNum.VBLF + "                     FROM KOSMOS_PMPA.XRAY_DETAIL";
                SQL = SQL + ComNum.VBLF + "                    WHERE SEEKDATE >= TO_DATE('" + strToDate + " 00:00:00','YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "                      AND SEEKDATE <= TO_DATE('" + strNextDate + " 23:59:59','YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "                      AND XJONG IN ('" + strGUBUN + "'))";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT 0 ROOMCODE, PANO, SNAME, AGE, SEX, '' GBDRG, B.DRNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'O' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  Where a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND PANO IN (";
                SQL = SQL + ComNum.VBLF + "                   SELECT PANO ";
                SQL = SQL + ComNum.VBLF + "                     FROM KOSMOS_PMPA.XRAY_DETAIL";
                SQL = SQL + ComNum.VBLF + "                    WHERE SEEKDATE >= TO_DATE('" + strToDate + " 00:00:00','YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "                      AND SEEKDATE <= TO_DATE('" + strNextDate + " 23:59:59','YYYY-MM-DD HH24:MI:SS')";
                SQL = SQL + ComNum.VBLF + "                      AND XJONG IN ('" + strGUBUN + "'))";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN, SNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }


        private void GetPatListCANCER()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strGUBUN = string.Empty;

            ssAipPatList_Sheet1.RowCount = 0;


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, GBDRG, B.DRNAME, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'I' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  WHERE a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "    AND ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND A.GBSTS <> '7') OR OUTDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE = 'MO' ";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT 0 ROOMCODE, PANO, SNAME, AGE, SEX, '' GBDRG, B.DRNAME, TO_CHAR(BDATE,'YYYY-MM-DD') INDATE, DEPTCODE, A.DRCODE, 'O' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "  Where a.DrCode = b.DrCode";
                SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "  AND A.DEPTCODE = 'MO' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN, SNAME";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetPatListTran()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            string strDeptCd = VB.Trim(VB.Right(cboDept.Text, 6));
            string strDrCd = VB.Trim(VB.Right(cboDr.Text, 5));
            if (strDeptCd == "0")
            {
                strDeptCd = "";
            }
            if (strDrCd == "0")
            {
                strDrCd = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + " TRUNC(M.InDate) AS InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                SQL = SQL + ComNum.VBLF + " TRUNC(M.OutDate) AS OutDate,";
                SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7, M.EMR ";

                SQL = SQL + ComNum.VBLF + " , CASE WHEN EXISTS(                                       ";
                SQL = SQL + ComNum.VBLF + "    SELECT 1                                               ";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_PMPA.NUR_TEWON_NAMESEND       ";
                SQL = SQL + ComNum.VBLF + "                  WHERE IPDNO = M.IPDNO                    ";
                SQL = SQL + ComNum.VBLF + "                    AND (DelDate IS NULL OR DelDate ='')   ";
                SQL = SQL + ComNum.VBLF + " ) THEN 'ⓝ' || M.SNAME  ELSE M.SNAME                                    ";
                SQL = SQL + ComNum.VBLF + " END TEWONNAME                                             ";

                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_DOCTOR  D, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANSFOR T";
                switch (VB.Trim(cboWard.Text))
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' ";
                        break;
                    case "ND":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + VB.Trim(cboWard.Text) + "' ";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "  AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                SQL = SQL + ComNum.VBLF + "  AND M.GbSTS <> '9' ";
                SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
                switch (VB.Trim(cboWard.Text))
                {
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + " AND ((T.FRWARD <> T.TOWARD) OR (T.FRWARD = 'IU' AND T.FRROOM <> T.TOROOM))";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + " AND ((T.FRWARD <> T.TOWARD) OR (T.FRWARD = 'IU' AND T.FRROOM <> T.TOROOM))";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "  AND T.FRWARD <> T.TOWARD";
                        break;
                }
                SQL = SQL + ComNum.VBLF + "  AND M.WARDCODE = T.TOWARD";
                SQL = SQL + ComNum.VBLF + "  AND T.TRSDATE >= TO_DATE('" + strToDate + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND T.TRSDATE <= TO_DATE('" + strToDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND T.PANO = M.PANO";
                if (chkEMR.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.EMR = '1' ";
                }
                if (cboNurAct.Text != "")
                {
                    if (VB.Right(cboNurAct.Text.Trim(), 8).Trim() == "낙상")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(M.PANO, SYSDATE) = 'Y' ";
                    }
                    else if (VB.Right(cboNurAct.Text.Trim(), 8).Trim() == "욕창")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND KOSMOS_OCS.FC_READ_WARNING_BRADEN(M.IPDNO) > 0 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS (SELECT 1 FROM " + ComNum.DB_MED + "OCS_IORDER O INNER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE C ";
                        SQL = SQL + ComNum.VBLF + "                                 ON O.ORDERCODE = C.ORDERCODE AND O.SLIPNO IN ('A7','A6','A5','0100') ";
                        SQL = SQL + ComNum.VBLF + "                                 AND O.ORDERSITE NOT IN ('E', 'N', 'ERO') ";
                        SQL = SQL + ComNum.VBLF + "                       WHERE M.PANO = O.PTNO ";
                        SQL = SQL + ComNum.VBLF + "                            AND O.GBSTATUS NOT IN ('D-', 'D') ";
                    }

                    if (VB.Trim(VB.Right(cboNurAct.Text, 8)) == "NC1289")
                    {
                        SQL = SQL + ComNum.VBLF + "                            AND O.SUCODE IN ('NC1289', 'NC1320') "; //측정 및 관찰 상태(통증관리) 코드가 두개임
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                            AND O.SUCODE = '" + VB.Trim(VB.Right(cboNurAct.Text, 8)) + "' ";
                    }
                    SQL = SQL + ComNum.VBLF + "                            AND O.BDATE = TO_DATE( '" + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString() + "', 'YYYY-MM-DD') )";
                }
                if (strDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND M.DEPTCODE =  '" + strDeptCd + "'";
                }
                if (strDrCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND M.DRCODE = '" + strDrCd + "'";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + "          TRUNC(M.InDate), M.Ilsu,M.IpdNo,M.GbSts, TRUNC(M.OutDate),";
                SQL = SQL + ComNum.VBLF + "          M.DEPTCODE , M.DRCODE, d.DrName, M.AmSet1, M.AmSet4, M.AmSet6, M.AmSet7, M.EMR";
                SQL = SQL + ComNum.VBLF + " ORDER BY M.RoomCode,M.SName, TRUNC(M.InDate) DESC  ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = nRead;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["TEWONNAME"].ToString().Trim();
                        //ssAipPatList_Sheet1.Cells[i, 2].Text = (Pat_TewonName_Chk(Convert.ToInt64(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()))) == "OK" ? "ⓝ" : "") + dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                        if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //TEXT EMR대상자
                        {
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Blue;
                        }
                        if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7") //퇴원완료자
                        {
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetPatListSelfMed()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            string strDeptCd = VB.Trim(VB.Right(cboDept.Text, 6));
            string strDrCd = VB.Trim(VB.Right(cboDr.Text, 5));
            if (strDeptCd == "0")
            {
                strDeptCd = "";
            }
            if (strDrCd == "0")
            {
                strDrCd = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + " TRUNC(M.InDate) AS InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                SQL = SQL + ComNum.VBLF + " TRUNC(M.OutDate) AS OutDate,";
                SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7, M.EMR ";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(B.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY A.FORMNO, A.UPDATENO)    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A                                                ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRFORM B                                            ";
                SQL = SQL + ComNum.VBLF + "      ON A.FORMNO   = B.FORMNO                                                 ";
                SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO                                               ";
                SQL = SQL + ComNum.VBLF + "     AND B.FORMTYPE IN ('0', '4')                                              ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_ERP.HR_EMP_BASIS E                                        ";
                SQL = SQL + ComNum.VBLF + "      ON A.CHARTUSEID = E.EMP_ID                                               ";
                SQL = SQL + ComNum.VBLF + "     AND E.JOBKIND_CD = '41'                                                   ";
                SQL = SQL + ComNum.VBLF + "WHERE A.MEDFRDATE  =  TO_CHAR(TRUNC(M.InDate), 'YYYYMMDD')                            ";
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO       = M.PANO                                                    ";
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO > 0                                                             ";
                SQL = SQL + ComNum.VBLF + "  AND A.SAVECERT = '0'                                                         ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.FORMNO, A.UPDATENO                                      ";
                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                     ";

                SQL = SQL + ComNum.VBLF + " , CASE WHEN EXISTS(                                       ";
                SQL = SQL + ComNum.VBLF + "    SELECT 1                                               ";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_PMPA.NUR_TEWON_NAMESEND       ";
                SQL = SQL + ComNum.VBLF + "                  WHERE IPDNO = M.IPDNO                    ";
                SQL = SQL + ComNum.VBLF + "                    AND (DelDate IS NULL OR DelDate ='')   ";
                SQL = SQL + ComNum.VBLF + " ) THEN 'ⓝ' || M.SNAME  ELSE M.SNAME                                    ";
                SQL = SQL + ComNum.VBLF + " END TEWONNAME                                             ";

                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_DOCTOR  D, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_EMR + "EMR_CADEX_SELFMED S    ";
                switch (VB.Trim(cboWard.Text))
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' ";
                        break;
                    case "ND":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + VB.Trim(cboWard.Text) + "' ";
                        break;
                }
                SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.PANO = S.PTNO ";
                SQL = SQL + ComNum.VBLF + "  AND M.INDATE >= TO_DATE(S.MEDFRDATE || ' 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "  AND M.INDATE <= TO_DATE(S.MEDFRDATE || ' 23:59','YYYY-MM-DD HH24:MI') ";
                if (chkEMR.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.EMR = '1' ";
                }
                if (cboNurAct.Text != "")
                {
                    if (VB.Right(cboNurAct.Text.Trim(), 8).Trim() == "낙상")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(M.PANO, SYSDATE) = 'Y' ";
                    }
                    else if (VB.Right(cboNurAct.Text.Trim(), 8).Trim() == "욕창")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND KOSMOS_OCS.FC_READ_WARNING_BRADEN(M.IPDNO) > 0 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS (SELECT 1 FROM " + ComNum.DB_MED + "OCS_IORDER O INNER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE C ";
                        SQL = SQL + ComNum.VBLF + "                                 ON O.ORDERCODE = C.ORDERCODE AND O.SLIPNO IN ('A7','A6','A5','0100') ";
                        SQL = SQL + ComNum.VBLF + "                                 AND O.ORDERSITE NOT IN ('E', 'N', 'ERO') ";
                        SQL = SQL + ComNum.VBLF + "                       WHERE M.PANO = O.PTNO ";
                        SQL = SQL + ComNum.VBLF + "                            AND O.GBSTATUS NOT IN ('D-', 'D') ";

                        if (VB.Trim(VB.Right(cboNurAct.Text, 8)) == "NC1289")
                        {
                            SQL = SQL + ComNum.VBLF + "                            AND O.SUCODE IN ('NC1289', 'NC1320') "; //측정 및 관찰 상태(통증관리) 코드가 두개임.
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                            AND O.SUCODE = '" + VB.Trim(VB.Right(cboNurAct.Text, 8)) + "' ";
                        }
                        SQL = SQL + ComNum.VBLF + "                            AND O.BDATE = TO_DATE( '" + VB.Format(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "@@@@-@@-@@") + "', 'YYYY-MM-DD') )";
                    }
                }
                if (strDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND M.DEPTCODE =  '" + strDeptCd + "'";
                }
                if (strDrCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND M.DRCODE = '" + strDrCd + "'";
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + " TRUNC(M.InDate), M.Ilsu, M.IpdNo, M.GbSts, TRUNC(M.OutDate), ";
                SQL = SQL + ComNum.VBLF + " M.DeptCode, M.DrCode, D.DrName, M.AmSet1, M.AmSet4, M.AmSet6, M.AmSet7, M.EMR ";
                SQL = SQL + ComNum.VBLF + " ORDER BY M.RoomCode,M.SName, TRUNC(M.InDate) DESC  ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["TEWONNAME"].ToString().Trim();
                        //ssAipPatList_Sheet1.Cells[i, 2].Text = (Pat_TewonName_Chk(Convert.ToInt64(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()))) == "OK" ? "ⓝ" : "") + dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text = clsVbfunc.READ_AGE_GESAN_Ex(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                        NOT_CERT_CHART_LIST(i, dt.Rows[i]["NOTE"].ToString().Trim());

                        if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //TEXT EMR대상자
                        {
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Blue;
                        }
                        if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7") //퇴원완료자
                        {
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetPatListIpd()
        {
            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJob = VB.Left(cboJob.Text.Trim(), 1);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);

            string strPriDate = (VB.DateAdd("d", -1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strToDate = (VB.DateAdd("d", 0, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");
            string strNextDate = (VB.DateAdd("d", 1, ComFunc.FormatStrToDate(strCurDate, "D"))).ToString("yyyy-MM-dd");

            string strDeptCd = VB.Trim(VB.Right(cboDept.Text, 6));
            string strDrCd = VB.Trim(VB.Right(cboDr.Text, 5));

            if (strDeptCd == "0")
            {
                strDeptCd = "";
            }

            if (strDrCd == "0")
            {
                strDrCd = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT  /*+rule */M.WardCode,M.RoomCode,M.Pano, M.Sex,M.Age,M.Bi,M.PName,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate, 'YYYY-MM-DD') AS InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate, 'YYYY-MM-DD') AS OutDate, M.GBDRG, ";
                SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7, M.EMR, M.BEDNUM ";
                SQL = SQL + ComNum.VBLF + " ,(                                                                            ";
                //SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(MAX(B.FORMNAME), '\r\n') WITHIN GROUP(ORDER BY A.FORMNO, A.UPDATENO)        ";
                //SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRCHARTMST A                                                           ";
                //SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_EMR.AEMRFORM B                                                       ";
                //SQL = SQL + ComNum.VBLF + "         ON A.FORMNO   = B.FORMNO                                                            ";
                //SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = B.UPDATENO                                                          ";
                //SQL = SQL + ComNum.VBLF + "        AND B.FORMTYPE IN ('0', '4')                                                         ";
                //SQL = SQL + ComNum.VBLF + "      INNER JOIN KOSMOS_ERP.HR_EMP_BASIS E                                                   ";
                //SQL = SQL + ComNum.VBLF + "         ON A.CHARTUSEID = E.EMP_ID                                                          ";
                //SQL = SQL + ComNum.VBLF + "        AND E.JOBKIND_CD = '41'                                                              ";
                //SQL = SQL + ComNum.VBLF + "   WHERE A.MEDFRDATE  =  TO_CHAR(M.InDate, 'YYYYMMDD')                                       ";
                //SQL = SQL + ComNum.VBLF + "     AND A.PTNO       = M.PANO                                                               ";
                //SQL = SQL + ComNum.VBLF + "     AND A.FORMNO > 0                                                                        ";
                //SQL = SQL + ComNum.VBLF + "     AND A.SAVECERT = '0'                                                                    ";
                //SQL = SQL + ComNum.VBLF + "   GROUP BY A.FORMNO, A.UPDATENO                                                             ";

                SQL = SQL + ComNum.VBLF + "  SELECT LISTAGG(CERTNAME, '\r\n') WITHIN GROUP(ORDER BY CERTNAME) ";
                SQL = SQL + ComNum.VBLF + "  FROM ( ";
                SQL = SQL + ComNum.VBLF + "      SELECT B.FORMNAME || ' ' || K.YNAME || ' ' || E.EMP_NM CERTNAME, A.PTNO, A.MEDFRDATE  ";
                SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_EMR.AEMRCHARTMST A, KOSMOS_EMR.AEMRFORM B, KOSMOS_ERP.HR_EMP_BASIS E, KOSMOS_PMPA.BAS_BUSE K  ";
                SQL = SQL + ComNum.VBLF + "      WHERE A.FORMNO = B.FORMNO  ";
                SQL = SQL + ComNum.VBLF + "        AND A.UPDATENO = B.UPDATENO  ";
                SQL = SQL + ComNum.VBLF + "        AND B.FORMTYPE IN('0', '4')  ";
                SQL = SQL + ComNum.VBLF + "        AND A.CHARTUSEID = E.EMP_ID  ";
                SQL = SQL + ComNum.VBLF + "        AND E.DEPT_CD = K.BUCODE  ";
                SQL = SQL + ComNum.VBLF + "        AND E.JOBKIND_CD = '41'  ";
                SQL = SQL + ComNum.VBLF + "        AND A.FORMNO > 0  ";
                SQL = SQL + ComNum.VBLF + "        AND A.SAVECERT = '0'  ";
                SQL = SQL + ComNum.VBLF + "      GROUP BY  B.FORMNAME || ' ' || K.YNAME || ' ' || E.EMP_NM, A.PTNO, A.MEDFRDATE )  ";
                SQL = SQL + ComNum.VBLF + " WHERE MEDFRDATE  =  TO_CHAR(M.InDate, 'YYYYMMDD')                                       ";
                SQL = SQL + ComNum.VBLF + "   AND PTNO       = M.PANO                                                               ";

                SQL = SQL + ComNum.VBLF + " ) AS NOTE                                                                                   ";

                SQL = SQL + ComNum.VBLF + " ,	CASE WHEN M.WARDCODE IN ('33', '35') THEN    ";
                SQL = SQL + ComNum.VBLF + " 	(                                            ";
                SQL = SQL + ComNum.VBLF + " 		SELECT REPLACE(NAME, '격리', '격') || ')'  ";
                SQL = SQL + ComNum.VBLF + " 		  FROM KOSMOS_PMPA.BAS_BCODE             ";
                SQL = SQL + ComNum.VBLF + " 		 WHERE GUBUN = 'NUR_ICU_침상번호'           ";
                SQL = SQL + ComNum.VBLF + " 		   AND CODE  = M.BEDNUM                  ";
                SQL = SQL + ComNum.VBLF + " 		   AND DELDATE IS NULL                   ";
                SQL = SQL + ComNum.VBLF + " 	)                                            ";
                SQL = SQL + ComNum.VBLF + " END ICU_BED_NAME                                 ";

                SQL = SQL + ComNum.VBLF + " , CASE WHEN EXISTS(                                       ";
                SQL = SQL + ComNum.VBLF + "    SELECT 1                                               ";
                SQL = SQL + ComNum.VBLF + "                 FROM KOSMOS_PMPA.NUR_TEWON_NAMESEND       ";
                SQL = SQL + ComNum.VBLF + "                  WHERE IPDNO = M.IPDNO                    ";
                SQL = SQL + ComNum.VBLF + "                    AND (DelDate IS NULL OR DelDate ='')   ";
                SQL = SQL + ComNum.VBLF + " ) THEN 'ⓝ' || M.SNAME  ELSE M.SNAME                      ";
                SQL = SQL + ComNum.VBLF + " END TEWONNAME                                             ";

                SQL = SQL + ComNum.VBLF + " FROM   " + ComNum.DB_PMPA + "IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT P, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_DOCTOR  D ";
                switch ((cboWard.Text).Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' ";
                        break;
                    case "ND":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + (cboWard.Text).Trim() + "' ";
                        break;
                }

                //작업분류
                if (strJob == "1" || strJob == "L" || strJob == "M") //재원자
                {
                    SQL = SQL + ComNum.VBLF + " AND ((M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.GBSTS <> '7') OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";

                    if (strJob == "L")
                    {
                        SQL = SQL + ComNum.VBLF + " AND EXISTS";
                        SQL = SQL + ComNum.VBLF + "  (SELECT IPDNO";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL MST";
                        SQL = SQL + ComNum.VBLF + "   WHERE  EXISTS (";
                        SQL = SQL + ComNum.VBLF + "   SELECT ACTDATE, SEQNO FROM (";
                        SQL = SQL + ComNum.VBLF + "   SELECT MAX(ACTDATE) ACTDATE, SEQNO";
                        SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                        SQL = SQL + ComNum.VBLF + "   GROUP BY SEQNO ) SUB";
                        SQL = SQL + ComNum.VBLF + "   WHERE MST.ACTDATE = SUB.ACTDATE";
                        SQL = SQL + ComNum.VBLF + "        AND MST.SEQNO = SUB.SEQNO)";
                        SQL = SQL + ComNum.VBLF + "        AND MST.IPDNO = M.IPDNO";
                        SQL = SQL + ComNum.VBLF + "        AND STATUS IN ('삽입','유지'))";
                    }
                    else if (strJob == "M")
                    {
                        SQL = SQL + ComNum.VBLF + " AND EXISTS ( SELECT PANO FROM KOSMOS_PMPA.IPD_NEW_SLIP SUB ";
                        SQL = SQL + ComNum.VBLF + "  WHERE M.IPDNO = SUB.IPDNO";
                        SQL = SQL + ComNum.VBLF + "       AND SUB.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "       AND SUB.SUNEXT IN (";
                        SQL = SQL + ComNum.VBLF + "              SELECT SUNEXT";
                        SQL = SQL + ComNum.VBLF + "                FROM KOSMOS_PMPA.BAS_SUN";
                        SQL = SQL + ComNum.VBLF + "              WHERE GBANTICAN = 'Y')) ";
                    }
                }
                else if (strJob == "2") //당일입원자
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.InDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.Pano <> '81000004' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.GbSTS <> '9' ";
                }
                else if (strJob == "F") //어제입원자
                {
                    SQL = SQL + ComNum.VBLF + "  AND M.InDate >= TO_DATE('" + Convert.ToDateTime(strToDate).AddDays(-1).ToShortDateString() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime >= TO_DATE('" + Convert.ToDateTime(strToDate).AddDays(-1).ToShortDateString() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + Convert.ToDateTime(strNextDate).AddDays(-1).ToShortDateString() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.Pano <> '81000004' ";
                    SQL = SQL + ComNum.VBLF + "  AND M.GbSTS <> '9' ";
                }
                else if (strJob == "3") //퇴원예고
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate>=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts NOT IN ('7','9') ";
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND (M.ROutDate IS NULL OR M.ROutDate>=TRUNC(SYSDATE) ) ";
                }
                else if (strJob == "4") //당일퇴원
                {
                    SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '7' "; //퇴원수납완료
                }
                else if (strJob == "O") //당일수술시술환자
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.OutDate = TO_DATE('" + strToDate + "','YYYY-MM-DD') OR (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND M.GBSTS <> '7'))";
                    SQL = SQL + ComNum.VBLF + " AND EXISTS ( ";
                    SQL = SQL + ComNum.VBLF + "               SELECT * ";
                    SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_PMPA + "ORAN_MASTER OPSUB";
                    SQL = SQL + ComNum.VBLF + "                WHERE OPDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "                  AND M.PANO = OPSUB.PANO) ";
                }
                else if (strJob == "5") //응급실경유입원 1-3일전
                {
                    SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                    SQL = SQL + ComNum.VBLF + " AND (M.Ilsu >= 1 AND M.Ilsu<=3) ";
                    SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('3','4','5') ";
                    SQL = SQL + ComNum.VBLF + " AND M.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND M.ROutDate>=TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                }
                SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";

                if (chkEMR.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.EMR = '1' ";
                }

                if (cboTeam.Text.Trim() != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                    SQL = SQL + ComNum.VBLF + " (SELECT * FROM " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE T";
                    SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                    SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + (cboTeam.Text).Trim() + "')";
                }

                if (cboNurAct.Text.Trim() != "")
                {
                    if (VB.Right(cboNurAct.Text.Trim(), 8).Trim() == "낙상")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(M.PANO, SYSDATE) = 'Y' ";
                    }
                    else if (VB.Right(cboNurAct.Text.Trim(), 8).Trim() == "욕창")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND KOSMOS_OCS.FC_READ_WARNING_BRADEN(M.IPDNO) > 0 ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS (SELECT 1 FROM " + ComNum.DB_MED + "OCS_IORDER O INNER JOIN " + ComNum.DB_MED + "OCS_ORDERCODE C ";
                        SQL = SQL + ComNum.VBLF + "                                 ON O.ORDERCODE = C.ORDERCODE AND O.SLIPNO IN ('A7','A6','A5','0100') ";
                        SQL = SQL + ComNum.VBLF + "                                 AND O.ORDERSITE NOT IN ('E', 'N', 'ERO') ";
                        SQL = SQL + ComNum.VBLF + "                       WHERE M.PANO = O.PTNO ";
                        SQL = SQL + ComNum.VBLF + "                            AND O.GBSTATUS NOT IN ('D-', 'D') ";

                        if (VB.Right(cboNurAct.Text, 8).Trim() == "NC1289")
                        {
                            SQL = SQL + ComNum.VBLF + "                            AND O.SUCODE IN ('NC1289', 'NC1320') "; //측정 및 관찰 상태(통증관리) 코드가 두개임.
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                            AND O.SUCODE = '" + VB.Right(cboNurAct.Text, 8).Trim() + "' ";
                        }
                        SQL = SQL + ComNum.VBLF + "                            AND O.BDATE = TO_DATE( '" + Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString() + "', 'YYYY-MM-DD') )";
                    }

                }


                if (strDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND M.DEPTCODE =  '" + strDeptCd + "'";
                }

                if (strDrCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "AND M.DRCODE = '" + strDrCd + "'";
                }

                //SORT
                if ((cboWard.Text).Trim() == "33" || (cboWard.Text).Trim() == "35")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY M.RoomCode, M.BEDNUM, M.SName, M.Indate DESC  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY M.RoomCode,M.SName, M.Indate DESC  ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssAipPatList_Sheet1.RowCount = dt.Rows.Count;
                ssAipPatList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (cboWard.Text.Trim() == "33" || cboWard.Text.Trim() == "35")
                        {
                            ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ICU_BED_NAME"].ToString().Trim();
                            //ssAipPatList_Sheet1.Cells[i, 0].Text = Read_ICU_Bed_Name(dt.Rows[i]["BEDNUM"].ToString().Trim(), "1") + dt.Rows[i]["RoomCode"].ToString().Trim();
                        }
                        else
                        {
                            ssAipPatList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        }

                        ssAipPatList_Sheet1.Cells[i, 1].Text  = dt.Rows[i]["Pano"].ToString().Trim();
                        //ssAipPatList_Sheet1.Cells[i, 2].Text  = (Pat_TewonName_Chk(Convert.ToInt64(VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim()))) == "OK" ? "ⓝ" : "") + dt.Rows[i]["Sname"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["TEWONNAME"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 3].Text  = clsVbfunc.READ_AGE_GESAN_Ex1(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim()) + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 4].Text  = dt.Rows[i]["GBDRG"].ToString().Trim() == "D" ? "[DRG]" : "";
                        ssAipPatList_Sheet1.Cells[i, 5].Text  = dt.Rows[i]["DrName"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 6].Text  = dt.Rows[i]["InDate"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 7].Text  = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssAipPatList_Sheet1.Cells[i, 8].Text  = dt.Rows[i]["DrCode"].ToString().Trim();
                        NOT_CERT_CHART_LIST(i, dt.Rows[i]["NOTE"].ToString().Trim());
                        ssAipPatList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                        if (dt.Rows[i]["EMR"].ToString().Trim() == "1") //TEXT EMR대상자
                        {
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Blue;
                        }
                        if (dt.Rows[i]["GBSTS"].ToString().Trim() == "7") //퇴원완료자
                        {
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].Font = fontBold;
                            ssAipPatList_Sheet1.Cells[i, 0, i, ssAipPatList_Sheet1.ColumnCount - 1].ForeColor = Color.Red;
                        }
                    }
                }
                
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 임시저장 차트 목록(정형화)
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="strInDate"></param>
        /// <param name="strPano"></param>
        private void NOT_CERT_CHART_LIST(int Row, string NOTE)
        {
            int Col = 3;
            if (NOTE.IsNullOrEmpty())
            { 
                ssAipPatList_Sheet1.SetStickyNoteStyleInfo(Row, Col, null);
                return;
            }

            Font font = new Font("굴림", 10);
      
            ssAipPatList_Sheet1.Cells[Row, Col].NoteStyle = NoteStyle.PopupStickyNote;
            ssAipPatList_Sheet1.Cells[Row, Col].NoteIndicatorPosition = NoteIndicatorPosition.TopLeft;
            ssAipPatList_Sheet1.Cells[Row, Col].NoteIndicatorColor = Color.Pink;
            ssAipPatList_Sheet1.Cells[Row, Col].NoteIndicatorSize = new Size(10, 20);
            ssAipPatList_Sheet1.Cells[Row, Col].Note = NOTE;
            FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
            //nsinfo = ssAipPatList_Sheet1.GetStickyNoteStyleInfo(Row, Col);
            //nsinfo.BackColor = Color.Red;
            nsinfo.ForeColor = Color.Black;
            nsinfo.Width = TextRenderer.MeasureText(NOTE.Split(new string[] { "\r\n", "\n"}, StringSplitOptions.None).OrderByDescending(d => d.Length).ToArray()[0], font).Width; //가장 긴 텍스트 길이에 맞춰서 너비 설정
            nsinfo.ShapeOutlineColor = Color.Red;
            nsinfo.ShapeOutlineThickness = 1;
            nsinfo.ShadowOffsetX = 3;
            nsinfo.ShadowOffsetY = 3;
            nsinfo.Height = 100;

            ssAipPatList_Sheet1.SetStickyNoteStyleInfo(Row, Col, nsinfo);
         
        }

        #endregion //리스트 조회

        #region //환자정보 전달

        private void ssAipPatList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssAipPatList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssAipPatList, e.Column);
                return;
            }

            ssAipPatListCellDoubleClick(e.Row, e.Column);
        }

        private void ssAipPatListCellDoubleClick(int Row, int Col)
        {
            string strGb = ssAipPatList_Sheet1.Cells[Row, 9].Text.Trim();

        

            if (strGb == "HD" ||
                strGb == "HR" ||
                strGb == "TO" ||
                cboWard.Text.Equals("혈액종양내과") ||
                cboWard.Text.Equals("외래수혈") ||
                cboWard.Text.Equals("ENDO") ||
                cboWard.Text.Equals("CT") ||
                cboWard.Text.Equals("MRI") ||
                ((cboWard.Text.Equals("OP") || cboWard.Text.Equals("AG")) && VB.Left(cboJob.Text, 1).Equals("2")))
            {
                SetEmrPatInfoHd(Row, Col);
            }
            else if (strGb == "ER")
            {
                SetEmrPatInfoER(Row, Col);
            }
            else
            {
                SetEmrPatInfoIpd(Row, Col);
            }
        }

        private void SetEmrPatInfoHd(int Row, int Col)
        {
            string strPtNo = ssAipPatList_Sheet1.Cells[Row, 1].Text.Trim();
            string strInoutCls = ssAipPatList_Sheet1.Cells[Row, 0].Text.Trim();
            string strMedFrDate = ssAipPatList_Sheet1.Cells[Row, 10].Text.Trim();
            string strDeptCd = ssAipPatList_Sheet1.Cells[Row, 9].Text.Trim();
            string strDrCd = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (strInoutCls == "O" && cboWard.Text == "HD")
                {
                    SQL = "";
                    SQL = "SELECT 'O' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = 'HD'";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                }
                else if (strInoutCls == "O" && cboWard.Text == "외래수혈")
                {
                    SQL = "";
                    SQL = "SELECT 'O' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = '" + strDeptCd + "' ";
                }
                else if (strInoutCls == "O" && cboWard.Text == "혈액종양내과")
                {
                    SQL = "";
                    SQL = "SELECT 'O' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = 'MO'  ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                }
                else if (strInoutCls == "O" && ((cboWard.Text == "ENDO")  || cboWard.Text == "HR" || cboWard.Text == "TO"))
                {
                    SQL = "";
                    SQL = "SELECT 'O' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = '" + strDeptCd + "' ";
                }
                else if (strInoutCls == "O" && (cboWard.Text == "CT" || cboWard.Text == "MRI"))
                {
                    SQL = "";
                    SQL = "SELECT 'O' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = '" + strDeptCd + "' ";
                }
                else if (strInoutCls == "O" && (cboWard.Text == "OP" || cboWard.Text == "AG"))
                {
                    SQL = "";
                    SQL = "SELECT 'O' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = '" + strDeptCd + "' ";
                }
                else
                {
                    SQL = "";
                    SQL = "SELECT 'I' AS INOUTCLS, PANO AS PTNO, DEPTCODE AS MEDDEPTCD, TO_CHAR(INDATE, 'YYYYMMDD') AS MEDFRDTAE, DRCODE AS MEDDRCD, '' AS MEDENDDATE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "      AND TO_DATE(INDATE) <= TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "      AND (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR OUTDATE >= TO_DATE('" + strMedFrDate + "','YYYY-MM-DD'))";
                    SQL = SQL + ComNum.VBLF + "      AND GBSTS <> '9'";
                }

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
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strPtNo = dt.Rows[0]["PTNO"].ToString().Trim();
                strInoutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                strMedFrDate = dt.Rows[0]["MEDFRDTAE"].ToString().Trim();
                strDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                strDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();

                dt.Dispose();
                dt = null;

                AcpEmr = clsEmrChart.ClearPatient();
                AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInoutCls, strMedFrDate, strDeptCd);
                if (AcpEmr == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                rSetEmrAcpInfo(AcpEmr);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SetEmrPatInfoER(int Row, int Col)
        {
            string strPtNo = ssAipPatList_Sheet1.Cells[Row, 1].Text.Trim();
            string strInoutCls = "";
            string strMedFrDate = ssAipPatList_Sheet1.Cells[Row, 10].Text.Trim();
            string strDeptCd = "";
            string strDrCd = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "  SELECT 'O' AS INOUTCLS, Pano AS PTNO , SName, TO_CHAR(BDATE, 'YYYYMMDD') AS MEDFRDTAE,  DRCODE AS MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      DEPTCODE AS MEDDEPTCD, '' AS MEDENDDATE";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "    AND BDate  = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND DeptCode IN ('EM' ,'ER')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY BDATE ASC";

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
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strPtNo = dt.Rows[0]["PTNO"].ToString().Trim();
                strInoutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                strMedFrDate = dt.Rows[0]["MEDFRDTAE"].ToString().Trim();
                strDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                strDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();

                dt.Dispose();
                dt = null;

                AcpEmr = clsEmrChart.ClearPatient();
                AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInoutCls, strMedFrDate, strDeptCd);
                if (AcpEmr == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                rSetEmrAcpInfo(AcpEmr);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void SetEmrPatInfoIpd(int Row, int Col)
        {
            string strPtNo = ssAipPatList_Sheet1.Cells[Row, 1].Text.Trim();
            string strInoutCls = "I";
            string strMedFrDate = ssAipPatList_Sheet1.Cells[Row, 6].Text.Trim().Replace("-","").Substring(0, 8);
            string strDeptCd = ssAipPatList_Sheet1.Cells[Row, 7].Text.Trim();
            //string strDrCd = "";

            try
            {
                AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInoutCls, strMedFrDate, strDeptCd);
                if (AcpEmr == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

                rSetEmrAcpInfo(AcpEmr);
            }
            catch (Exception ex)
            {
                Log.Error("SetEmrPatInfoIpd 접수정보 전달", ex.StackTrace);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, "SetEmrPatInfoIpd 접수정보 전달", clsDB.DbCon); //에러로그 저장
            }
        }


        #endregion //환자정보 전달

        private void btnPrintList_Click(object sender, EventArgs e)
        {
            if (ssAipPatList_Sheet1.RowCount == 0)
                return;

            string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
            string strFont2 = @"/fn""바탕체"" /fz""12"" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strHead1 = @"/c/f1" + "환자리스트" + "/f1/n/n";

            ssAipPatList_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2;
            ssAipPatList_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            //ssAipPatList_Sheet1.PrintInfo.Margin.Left = 40;
            //ssAipPatList_Sheet1.PrintInfo.Margin.Right  = 40;
            ssAipPatList_Sheet1.PrintInfo.Margin.Top    = 30;
            ssAipPatList_Sheet1.PrintInfo.Margin.Bottom = 10;

            ssAipPatList_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssAipPatList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssAipPatList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssAipPatList_Sheet1.PrintInfo.ShowBorder = true;
            ssAipPatList_Sheet1.PrintInfo.ShowColor = false;
            ssAipPatList_Sheet1.PrintInfo.ShowGrid = true;
            ssAipPatList_Sheet1.PrintInfo.ShowShadows = false;
            ssAipPatList_Sheet1.PrintInfo.UseMax = false;
            ssAipPatList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssAipPatList.PrintSheet(0);
        }

        private void rdoRoom1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                GetPatList();
            }
        }

        private void ssAipPatList_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssAipPatList, e.Column);
                return;
            }
        }
    }
}
