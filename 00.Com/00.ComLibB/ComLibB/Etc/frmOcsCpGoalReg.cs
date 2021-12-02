using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpGoalReg : Form
    {
        
        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;
        Form mCallForm = null;

        //응급실 간호 대기(Nedis) 환자 정보를 가지고 온다
        double mCPNO = 0;
        string mPTNO = "";
        string mPTNAME = "";
        string mBDATE = "";
        string mDEPTCODE = "";
        string mCPCODE = "";

        clsOrderEtc OE = null;
        clsOrderEtc.OCS_CP_RECORD OCR = new clsOrderEtc.OCS_CP_RECORD();
        frmOcsCpSelect frmOcsCpSelectX;

        public enum CpValue
        {
            CODE = 0,   //코드
            NAMEC,      //지표
            CPVALV,     //기준
            GUBUN,      //구분
            SCODE,      //기준코드
            VALGB,      //미해당
            NAMES,      //기준지표
            CPSTIME,    //기준시각
            BTNCS,        //연동
            CPETIME,    //시행시각
            BTNCC,        //연동
            CPVAL,      //값
            CPRSTN     //평가
        }

        public frmOcsCpGoalReg()
        {
            InitializeComponent();
        }

        public frmOcsCpGoalReg(Form pCallForm)
        {
            InitializeComponent();
            mCallForm = pCallForm;
        }

        public frmOcsCpGoalReg(double pCPNO, string pPTNO, string pPTNAME, string pBDATE, string pDEPTCODE, string pCPCODE)
        {
            InitializeComponent();

            mCPNO = pCPNO;
            mPTNO = pPTNO;
            mPTNAME = pPTNAME;
            mBDATE = pBDATE;
            mDEPTCODE = pDEPTCODE;
            mCPCODE = pCPCODE;
        }

        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            //if (this.Width > 900)
            //{
            //    this.Width = 774;
            //}
            //else
            //{
            //    this.Width = 1159;
            //}
        }

        private void frmOcsCpGoalReg_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            //this.Width = 774;
            ssCpList_Sheet1.RowCount = 0;
            OE = new clsOrderEtc(); //CP 관련 클래스

            ClearForm();

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon , "D"),"D");

            dtpSDate.Value = Convert.ToDateTime(strCurDate);
            dtpEDate.Value = dtpSDate.Value;

            cboCpName.Items.Clear();
            cboCpName.Items.Add("전 체");

            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "   BASCD, BASNAME ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_BASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = 'CP관리'  ";
            SQL = SQL + ComNum.VBLF + "    AND GRPCD = 'CP코드관리' ";
            SQL = SQL + ComNum.VBLF + "    AND BASNAME1 = 'ER' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY BASCD ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboCpName.Items.Add(dt.Rows[i]["BASNAME"].ToString().Trim() + VB.Space(100) + dt.Rows[i]["BASCD"].ToString().Trim());
                }
            }
            dt.Dispose();
            dt = null;

            cboCpName.SelectedIndex = 0;

            GetDataCpList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetDataCpList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                GetDataCpGoal(mCPNO, mCPCODE);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (DeleteData() == true)
            {
                GetDataCpGoal(mCPNO, mCPCODE);
            }
        }

        private void ssCpList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssCpList_Sheet1.RowCount == 0) return;
            if (e.ColumnHeader == true) return;

            ssCpListCellClick(e.Row);
        }

        private void ssCpListCellClick(int Row)
        {
            mBDATE = ssCpList_Sheet1.Cells[Row, 0].Text.Trim();
            mCPNO = VB.Val(ssCpList_Sheet1.Cells[Row, ssCpList_Sheet1.ColumnCount - 2].Text.Trim());
            mCPCODE = ssCpList_Sheet1.Cells[Row, ssCpList_Sheet1.ColumnCount - 1].Text.Trim();

            ClearForm();
            GetDataPatInfo(mCPNO);
            GetDataCpGoal(mCPNO, mCPCODE);
        }

        private void ssValue_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            if (ssValue_Sheet1.RowCount == 0) return;

            if (e.Column == (int)CpValue.VALGB)
            {
                if (mCPCODE != "CPCODE0005")
                {
                    return;
                }
                string strCPCODE = ssValue_Sheet1.Cells[e.Row, (int)CpValue.CODE].Text;
                if (strCPCODE == "CPPLN0013")
                {
                    if (Convert.ToBoolean(ssValue_Sheet1.Cells[e.Row, (int)CpValue.VALGB].Value) == true)
                    {
                        ssValue_Sheet1.Cells[e.Row + 1, (int)CpValue.CPSTIME].Text = "";
                        ssValue_Sheet1.Cells[e.Row + 1, (int)CpValue.CPETIME].Text = "";
                        ssValue_Sheet1.Cells[e.Row + 1, (int)CpValue.CPVAL].Text = "";
                        ssValue_Sheet1.Cells[e.Row + 1, (int)CpValue.CPRSTN].Text = "";
                        ssValue_Sheet1.Cells[e.Row + 1, (int)CpValue.VALGB].Value = true;
                        ssValue_Sheet1.Rows[e.Row + 1].Visible = false;
                    }
                    else 
                    {
                        ssValue_Sheet1.Cells[e.Row + 1, (int)CpValue.VALGB].Value = false;
                        ssValue_Sheet1.Rows[e.Row + 1].Visible = true;
                    }
                }
                return;
            }

            if (e.Column == (int)CpValue.BTNCS || e.Column == (int)CpValue.BTNCC)
            {
                string strCPCODE = ssValue_Sheet1.Cells[e.Row, (int)CpValue.CODE].Text;
                string strREFCODE = ssValue_Sheet1.Cells[e.Row, (int)CpValue.SCODE].Text;
                string strTYPE = ssValue_Sheet1.Cells[e.Row, (int)CpValue.GUBUN].Text;
                long lngRefVal = (long)VB.Val(ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPVALV].Text);

                if (strTYPE == "구분")
                {
                    return;
                }

                //ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPVAL].Text = ""; //지표값
                //ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPRSTN].Text = ""; //평가

                if (e.Column == (int)CpValue.BTNCS)
                {
                    ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPSTIME].Text = GetRefTime(strREFCODE); //기준시각
                }
                else if (e.Column == (int)CpValue.BTNCC)
                {
                    ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPETIME].Text = GetCpTime(strCPCODE); //실행시각
                }

                if (ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPSTIME].Text.Trim() != "" && ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPETIME].Text.Trim() != "")
                {
                    long CpVal = CalcCpVal(ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPETIME].Text, ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPSTIME].Text);
                    long CpCheck = CpVal - lngRefVal;

                    ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPVAL].Text = CpVal.ToString(); //지표값
                    if (CpCheck > 0)
                    {
                        ssValue_Sheet1.Cells[e.Row, (int)CpValue.CODE, e.Row, (int)CpValue.CPRSTN].ForeColor = System.Drawing.Color.Red;
                        ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPRSTN].Text = "+ " + CpCheck.ToString(); //평가
                    }
                    else
                    {
                        ssValue_Sheet1.Cells[e.Row, (int)CpValue.CPRSTN].Text = CpCheck.ToString(); //평가
                                                                                                    //ssValue_Sheet1.Cells[e.Row, 9].Text = ""; //평가
                    }
                }
            }
        }

        private void ssValue_EditModeOff(object sender, EventArgs e)
        {
            if (ssValue_Sheet1.RowCount == 0) return;
            int Row = ssValue_Sheet1.ActiveRowIndex;
            int Col = ssValue_Sheet1.ActiveColumnIndex;
            string strCPCODE = ssValue_Sheet1.Cells[Row, (int)CpValue.CODE].Text;
            string strREFCODE = ssValue_Sheet1.Cells[Row, (int)CpValue.SCODE].Text;
            string strTYPE = ssValue_Sheet1.Cells[Row, (int)CpValue.GUBUN].Text;
            long lngRefVal = (long)VB.Val(ssValue_Sheet1.Cells[Row, (int)CpValue.CPVALV].Text);

            if (strTYPE == "구분")
            {
                return;
            }

            if (Col == (int)CpValue.CPSTIME || Col == (int)CpValue.CPETIME)
            {
                ssValue_Sheet1.Cells[Row, (int)CpValue.CPVAL].Text = ""; //지표값
                ssValue_Sheet1.Cells[Row, (int)CpValue.CPRSTN].Text = ""; //평가
                if (ssValue_Sheet1.Cells[Row, (int)CpValue.CPSTIME].Text.Trim() != "" && ssValue_Sheet1.Cells[Row, (int)CpValue.CPETIME].Text.Trim() != "")
                {
                    long CpVal = CalcCpVal(ssValue_Sheet1.Cells[Row, (int)CpValue.CPETIME].Text, ssValue_Sheet1.Cells[Row, (int)CpValue.CPSTIME].Text);
                    long CpCheck = CpVal - lngRefVal;

                    ssValue_Sheet1.Cells[Row, (int)CpValue.CPVAL].Text = CpVal.ToString(); //지표값
                    if (CpCheck > 0)
                    {
                        ssValue_Sheet1.Cells[Row, (int)CpValue.CODE, Row, (int)CpValue.CPRSTN].ForeColor = System.Drawing.Color.Red;
                        ssValue_Sheet1.Cells[Row, (int)CpValue.CPRSTN].Text = "+ " + CpCheck.ToString(); //평가
                    }
                    else
                    {
                        ssValue_Sheet1.Cells[Row, (int)CpValue.CPRSTN].Text = CpCheck.ToString(); //평가
                                                                                //ssValue_Sheet1.Cells[Row, 9].Text = ""; //평가
                    }
                }
            }
            
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            if (ssValue_Sheet1.RowCount == 0) return;
            int i = 0;

            for (i = 0; i < ssValue_Sheet1.RowCount; i++)
            {
                string strCPCODE = ssValue_Sheet1.Cells[i, (int)CpValue.CODE].Text;
                string strREFCODE = ssValue_Sheet1.Cells[i, (int)CpValue.SCODE].Text;
                string strTYPE = ssValue_Sheet1.Cells[i, (int)CpValue.GUBUN].Text;
                long lngRefVal = (long)VB.Val(ssValue_Sheet1.Cells[i, (int)CpValue.CPVALV].Text);

                if (strTYPE == "구분")
                {
                    continue;
                }

                ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text = ""; //지표값
                ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text = ""; //평가

                ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Text = GetRefTime(strREFCODE); //기준시각
                ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Text = GetCpTime(strCPCODE); //실행시각
                if (ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Text.Trim() != "" && ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Text.Trim() != "")
                {
                    long CpVal = CalcCpVal(ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Text, ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Text);
                    long CpCheck = CpVal - lngRefVal;

                    ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text = CpVal.ToString(); //지표값
                    if (CpCheck > 0)
                    {
                        ssValue_Sheet1.Cells[i, (int)CpValue.CODE, i , (int)CpValue.CPRSTN].ForeColor = System.Drawing.Color.Red;
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text = "+ " + CpCheck.ToString(); //평가
                    }
                    else
                    {
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text = CpCheck.ToString(); //평가
                        //ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text = ""; //평가
                    }
                }
            }
        }

        #region //Function
        private void ClearForm()
        {
            lblPTNO.Text = "";
            lblPTNAME.Text = "";
            lblSex.Text = "";
            lblAge.Text = "";
            lblCP.Text = "";
            lblINDATE.Text = "";
            lblJUMIN.Text = "";
            lblDISDATE.Text = "";
            lblKTASDATE.Text = "";
            lblKTASLEVEL.Text = "";
            lblJINRESULT.Text = "";
            lblOUTDATE.Text = "";
            lblDEPT.Text = "";
            lblCALLDATE.Text = "";
            lblJINDATE.Text = "";
            lblDECIDE.Text = "";
            txtEMS.Text = "";
            txtEMSCALL.Text = "";

            ssValue_Sheet1.RowCount = 0;

            panEMS.Visible = false;
        }

        private void GetDataCpList()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssCpList_Sheet1.RowCount = 0;
            ClearForm();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string strSDate = dtpSDate.Value.ToString("yyyy-MM-dd");
                string strEDate = dtpEDate.Value.ToString("yyyy-MM-dd");
                string strCpCode = "";
                if (cboCpName.SelectedIndex != 0)
                {
                    strCpCode = VB.Right(cboCpName.Text.Trim(), 10).Trim();
                }
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "    R.CPNO, R.PTNO, R.PTNAME, R.GBIO, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(R.BDATE, 'YYYY-MM-DD') AS BDATE , ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS OUTTIME ,";
                SQL = SQL + ComNum.VBLF + "    N.SEX, N.AGE, N.DRNAME, N.HODEPT1, N.HODRNAME1, ";
                SQL = SQL + ComNum.VBLF + "    R.DEPTCODE, R.BI, R.CPCODE, R.AGE, R.SEX, ";
                SQL = SQL + ComNum.VBLF + "    R.STARTSABUN, U.USERNAME, ";
                SQL = SQL + ComNum.VBLF + "    B.BASNAME ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_RECORD R ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
                SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD B ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리' ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리' ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_USER U";
                SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE >= TO_DATE('" + strSDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND R.BDATE <= TO_DATE('" + strEDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND R.GBIO = 'E' ";
                if (cboCpName.SelectedIndex != 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND R.CPCODE = '" + strCpCode + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY  R.BDATE ASC, R.PTNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssCpList_Sheet1.RowCount = dt.Rows.Count;
                    ssCpList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCpList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEX"].ToString().Trim() + "/" + dt.Rows[i]["AGE"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["HODRNAME1"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, ssCpList_Sheet1.ColumnCount - 2].Text = dt.Rows[i]["CPNO"].ToString().Trim();
                        ssCpList_Sheet1.Cells[i, ssCpList_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["CPCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                //if (ssCpList_Sheet1.RowCount > 0)
                //{
                //    ssCpListCellClick(0);
                //}

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void GetDataPatInfo(double pCPNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (mCPCODE == "CPCODE0006")
            {
                panEMS.Visible = true;
            }
            else
            {
                panEMS.Visible = false;
            }

            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    R.CPNO, R.PTNO, R.PTNAME, R.GBIO, R.BDATE , ";
            SQL = SQL + ComNum.VBLF + "    R.EMS_CALL_DATE, R.EMS_CALL_TIME, R.EMS_DATE, R.EMS_TIME, ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(R.INTIME, 'YYYY-MM-DD HH24:MI') AS INTIME,  ";
            SQL = SQL + ComNum.VBLF + "    P.JUMIN1, P.JUMIN2,  ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS OUTTIME ,";
            SQL = SQL + ComNum.VBLF + "    N.SEX, N.AGE, ";
            SQL = SQL + ComNum.VBLF + "    N.DRNAME, "; //ER의사
            SQL = SQL + ComNum.VBLF + "    N.HODEPT1,"; //주과
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.BALDATE, 'YYYY-MM-DD HH24:MI') AS BALDATE,"; //발병일자
            SQL = SQL + ComNum.VBLF + "    N.KTASLEVL,"; //KTASLEVL
            SQL = SQL + ComNum.VBLF + "    N.OUTGBN,"; //진료결과(퇴원구분) 1.입원 2.귀가 3.DOA 4.사망 5.취소 6.후송 7.DAMA 8.OPD(ER후외래로)
            SQL = SQL + ComNum.VBLF + "    N.DEPTCODE AS MDEPTCODE,"; //주과
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOTIME1, 'YYYY-MM-DD HH24:MI') AS HOTIME1,"; //호출시각1
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HODATE1, 'YYYY-MM-DD HH24:MI') AS HODATE1,"; //대면시각
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOJINDATE1, 'YYYY-MM-DD HH24:MI') AS HOJINDATE1,"; //결정시각
            SQL = SQL + ComNum.VBLF + "    R.DEPTCODE, R.BI, R.CPCODE, R.AGE, R.SEX,  ";
            SQL = SQL + ComNum.VBLF + "    R.STARTSABUN, U.USERNAME,  ";
            SQL = SQL + ComNum.VBLF + "    B.BASNAME AS CPNAME ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_RECORD R  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_PATIENT P ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = P.PANO ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD B  ";
            SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD  ";
            SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'  ";
            SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_USER U ";
            SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER  ";
            SQL = SQL + ComNum.VBLF + "WHERE R.CPNO = " + pCPNO;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                lblPTNO.Text = dt.Rows[0]["PTNO"].ToString().Trim();
                lblPTNAME.Text = dt.Rows[0]["PTNAME"].ToString().Trim();
                lblSex.Text = dt.Rows[0]["SEX"].ToString().Trim();
                lblAge.Text = dt.Rows[0]["AGE"].ToString().Trim();
                lblCP.Text = dt.Rows[0]["CPNAME"].ToString().Trim();
                lblINDATE.Text = dt.Rows[0]["INTIME"].ToString().Trim();
                lblOUTDATE.Text = dt.Rows[0]["OUTTIME"].ToString().Trim();
                lblJUMIN.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1);
                lblDISDATE.Text = dt.Rows[0]["BALDATE"].ToString().Trim();
                lblKTASDATE.Text = KTASTime(dt.Rows[0]["PTNO"].ToString().Trim(), VB.Left(dt.Rows[0]["INTIME"].ToString().Trim(), 10).Replace("-", ""), VB.Right(dt.Rows[0]["INTIME"].ToString().Trim(), 5).Replace(":", ""));
                lblKTASLEVEL.Text = dt.Rows[0]["KTASLEVL"].ToString().Trim();
                //lblJINRESULT.Text = JinResult(dt.Rows[0]["OUTGBN"].ToString().Trim());
                lblJINRESULT.Text = JinResultQuery(dt.Rows[0]["PTNO"].ToString().Trim(), VB.Left(dt.Rows[0]["INTIME"].ToString().Trim(), 10).Replace("-", ""), VB.Right(dt.Rows[0]["INTIME"].ToString().Trim(), 5).Replace(":", ""));
                lblDEPT.Text = dt.Rows[0]["MDEPTCODE"].ToString().Trim();
                lblCALLDATE.Text = dt.Rows[0]["HOTIME1"].ToString().Trim();
                lblJINDATE.Text = dt.Rows[0]["HODATE1"].ToString().Trim();
                lblDECIDE.Text = dt.Rows[0]["HOJINDATE1"].ToString().Trim();
                if (dt.Rows[0]["EMS_DATE"].ToString().Trim() != "")
                {
                    txtEMS.Text = dt.Rows[0]["EMS_DATE"].ToString().Trim() + dt.Rows[0]["EMS_TIME"].ToString().Trim();
                }
                else
                {
                    txtEMS.Text = "";
                }
                if (dt.Rows[0]["EMS_CALL_DATE"].ToString().Trim() != "")
                {
                    txtEMSCALL.Text = dt.Rows[0]["EMS_CALL_DATE"].ToString().Trim() + dt.Rows[0]["EMS_CALL_TIME"].ToString().Trim();
                }
                else
                {
                    txtEMSCALL.Text = "";
                }
            }

            dt.Dispose();
            dt = null;
        }

        private bool SaveData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            if (ssValue_Sheet1.RowCount == 0)
            {
                ComFunc.MsgBox("저장할 데이타가 없습니다.");
                return false;
            }

            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strDateCall = VB.Left(txtEMSCALL.Text.Trim().Replace("-", "").Replace(":", ""), 8);
                string strTimeCall = VB.Right(txtEMSCALL.Text.Trim().Replace("-", "").Replace(":", ""), 4);
                string strFullDateCall = ComFunc.FormatStrToDate(strDateCall, "D") + " " + ComFunc.FormatStrToDate(strTimeCall, "M");

                if (txtEMSCALL.Text.Replace("-", "").Replace(":", "").Trim() != "")
                {
                    if (VB.IsDate(strFullDateCall) == false)
                    {
                        ComFunc.MsgBox("호출시간 날짜 형식이 틀립니다.");
                        txtEMS.Focus();
                        return false;
                    }
                }

                string strDate = VB.Left(txtEMS.Text.Trim().Replace("-", "").Replace(":", ""), 8);
                string strTime = VB.Right(txtEMS.Text.Trim().Replace("-", "").Replace(":", ""), 4);
                string strFullDate = ComFunc.FormatStrToDate(strDate, "D") + " " + ComFunc.FormatStrToDate(strTime, "M");

                if (txtEMS.Text.Replace("-", "").Replace(":", "").Trim() != "")
                {
                    if (VB.IsDate(strFullDate) == false)
                    {
                        ComFunc.MsgBox("날짜 형식이 틀립니다.");
                        txtEMS.Focus();
                        return false;
                    }
                }

                SQL = "UPDATE KOSMOS_OCS.OCS_CP_RECORD SET";
                SQL = SQL + ComNum.VBLF + "     EMS_CALL_DATE = '" + strDateCall + "', ";
                SQL = SQL + ComNum.VBLF + "     EMS_CALL_TIME = '" + strTimeCall + "', ";
                SQL = SQL + ComNum.VBLF + "     EMS_DATE = '" + strDate + "', ";
                SQL = SQL + ComNum.VBLF + "     EMS_TIME = '" + strTime + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE CPNO = " + mCPNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = "DELETE FROM " + ComNum.DB_MED + "OCS_CP_VALUE";
                SQL = SQL + ComNum.VBLF + "WHERE CPNO = " + mCPNO + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < ssValue_Sheet1.RowCount; i++)
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_MED + "OCS_CP_VALUE";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     CPNO, CODE, SCODE, VALGB, CPSTIME, CPETIME, CPVALV, CPDFTN, CPVALN, CPRSTN, IDNUMBER, INPDATE, INPTIME";
                    SQL = SQL + ComNum.VBLF + ")";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "'" + mCPNO + "',"; //CPNO
                    SQL = SQL + ComNum.VBLF + "'" + ssValue_Sheet1.Cells[i, (int)CpValue.CODE].Text.Trim() + "',"; //CODE
                    SQL = SQL + ComNum.VBLF + "'" + ssValue_Sheet1.Cells[i, (int)CpValue.SCODE].Text.Trim() + "',"; //SCODE
                    SQL = SQL + ComNum.VBLF + "'" + (Convert.ToBoolean(ssValue_Sheet1.Cells[i, (int)CpValue.VALGB].Value) == true ? "1" : "0") + "',"; //VALGB
                    SQL = SQL + ComNum.VBLF + "'" + ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Text.Trim() + "',"; //CPSTIME
                    SQL = SQL + ComNum.VBLF + "'" + ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Text.Trim() + "',"; //CPETIME
                    if (ssValue_Sheet1.Cells[i, (int)CpValue.GUBUN].Text.Trim() == "구분")
                    {
                        SQL = SQL + ComNum.VBLF + "'" + ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text.Trim() + "',"; //CPVALV
                        SQL = SQL + ComNum.VBLF + "NULL,"; //CPDFTN
                        SQL = SQL + ComNum.VBLF + "NULL,"; //CPVALN
                        SQL = SQL + ComNum.VBLF + "NULL,"; //CPRSTN
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "'" + "" + "',"; //CPVALV
                        SQL = SQL + ComNum.VBLF + "'" + (int)VB.Val(ssValue_Sheet1.Cells[i, (int)CpValue.CPVALV].Text.Trim()) + "',"; //CPDFTN
                        SQL = SQL + ComNum.VBLF + "'" + (int)VB.Val(ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text.Trim()) + "',"; //CPVALN
                        SQL = SQL + ComNum.VBLF + "'" + (int)VB.Val(ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text.Trim()) + "',"; //CPRSTN
                    }

                    SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "',"; //IDNUMBER
                    SQL = SQL + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "',"; //INPDATE
                    SQL = SQL + ComNum.VBLF + "'" + VB.Right(strCurDateTime, 6) + "'"; //INPTIME
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool DeleteData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            if (ComFunc.MsgBoxQ("등록된 자료를 삭제하시겠습니까?" + ComNum.VBLF + "삭제된 데이타는 복구할 수 없습니다.", "CP", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Cursor.Current = Cursors.Default;
                return false;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE FROM " + ComNum.DB_MED + "OCS_CP_VALUE";
                SQL = SQL + ComNum.VBLF + "WHERE CPNO = " + mCPNO + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private long CalcCpVal(string strCpdate, string strRefDate)
        {
            long CpVal = 0;
            try
            {
                CpVal = VB.DateDiff("n", strRefDate, strCpdate);
            }
            catch
            {

            }
            return CpVal;
        }

        private void GetDataCpGoal(double pCPNO, string pCPCODE)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssValue_Sheet1.RowCount = 0;
            CrearButton();

            FarPoint.Win.Spread.CellType.TextCellType rtnTextCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType rtnCheckBoxCellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType rtnComboBoxCellType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            try
            {

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    V.CPNO, V.CODE, V.SCODE, V.VALGB, V.CPSTIME, V.CPETIME, V.CPVALV, V.CPVALN, V.CPRSTN, V.IDNUMBER,  ";
                SQL = SQL + ComNum.VBLF + "    B.TYPE, B.DSPSEQ, C.BASNAME AS CPNAME, B.CPVALUE, B.INPUTGBC, B.INPUTGBS, ";
                SQL = SQL + ComNum.VBLF + "    C1.BASCD, C1.BASNAME AS SNAME, C1.VFLAG1 AS SVFLAG1 ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_VALUE V  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_OCS.OCS_CP_SUB B  ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = B.CODE  ";
                SQL = SQL + ComNum.VBLF + "    AND B.CPCODE = '" + pCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "    AND B.GUBUN = '06'   ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_OCS.OCS_CP_MAIN A  ";
                SQL = SQL + ComNum.VBLF + "    ON B.CPCODE = A.CPCODE  ";
                SQL = SQL + ComNum.VBLF + "    AND B.SDATE = A.SDATE  ";
                SQL = SQL + ComNum.VBLF + "    AND A.SDATE = (SELECT MAX(A1.SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN A1 ";
                SQL = SQL + ComNum.VBLF + "                 WHERE A1.SDATE <= '" + mBDATE.Replace("-", "") + "') ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD C  ";
                SQL = SQL + ComNum.VBLF + "    ON V.CODE = C.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCD = 'CP지표'  ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_BASCD C1  ";
                SQL = SQL + ComNum.VBLF + "    ON V.SCODE = C1.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND C1.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND C1.GRPCD = 'CP지표참조' ";
                SQL = SQL + ComNum.VBLF + "WHERE V.CPNO = " + pCPNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY B.DSPSEQ  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssValue_Sheet1.RowCount = dt.Rows.Count;
                    ssValue_Sheet1.SetRowHeight(-1, 36);
                    
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssValue_Sheet1.Cells[i, (int)CpValue.CODE].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.NAMEC].Text = dt.Rows[i]["CPNAME"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPVALV].Text = dt.Rows[i]["CPVALUE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.GUBUN].Text = dt.Rows[i]["TYPE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.SCODE].Text = dt.Rows[i]["SCODE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.VALGB].Value = (VB.Val(dt.Rows[i]["VALGB"].ToString().Trim()) == 1 ? true : false);
                        ssValue_Sheet1.Cells[i, (int)CpValue.NAMES].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Text = dt.Rows[i]["CPSTIME"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Text = dt.Rows[i]["CPETIME"].ToString().Trim();
                        if (dt.Rows[i]["TYPE"].ToString().Trim() == "구분")
                        {
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Locked = true;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].CellType = rtnTextCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].CellType = rtnTextCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].CellType = rtnComboBoxCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            string[] arryUserMcro = VB.Split(dt.Rows[i]["SVFLAG1"].ToString().Trim(), "^");
                            clsSpread.gSpreadComboDataSetEx1(ssValue, i, (int)CpValue.CPVAL, i, (int)CpValue.CPVAL, arryUserMcro, true);

                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text = dt.Rows[i]["CPVALV"].ToString().Trim();
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text = "";
                        }
                        else
                        {
                            if (dt.Rows[i]["INPUTGBS"].ToString().Trim() != "연동")
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].CellType = rtnTextCellType;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Locked = true;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Text = dt.Rows[i]["INPUTGBS"].ToString().Trim();
                            }
                            if (dt.Rows[i]["INPUTGBC"].ToString().Trim() != "연동")
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].CellType = rtnTextCellType;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Locked = true;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Text = dt.Rows[i]["INPUTGBC"].ToString().Trim();
                            }

                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].Text = dt.Rows[i]["CPVALN"].ToString().Trim();
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text = dt.Rows[i]["CPRSTN"].ToString().Trim();

                            if (VB.Val(ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text) > 0)
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text = "+ " + ssValue_Sheet1.Cells[i, (int)CpValue.CPRSTN].Text;
                                ssValue_Sheet1.Cells[i, (int)CpValue.CODE, i, (int)CpValue.CPRSTN].ForeColor = System.Drawing.Color.Red;
                            }

                            if (mCPCODE == "CPCODE0005")
                            {
                                string strCPCODE = ssValue_Sheet1.Cells[i, (int)CpValue.CODE].Text;
                                if (strCPCODE == "CPPLN0004")
                                {
                                    ssValue_Sheet1.Rows[i].Visible = false;
                                }
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }
                dt.Dispose();
                dt = null;


                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    A.CPCODE, A.SDATE, B.GUBUN, B.CODE, B.NAME, B.TYPE, B.SCODE, B.CPVALUE, B.INPUTGBC, B.INPUTGBS, B.DSPSEQ,  ";
                SQL = SQL + ComNum.VBLF + "    C.NFLAG1 , C1.BASNAME AS SNAME, C1.VFLAG1 AS SVFLAG1 ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_MAIN A  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_OCS.OCS_CP_SUB B  ";
                SQL = SQL + ComNum.VBLF + "    ON A.CPCODE = B.CPCODE  ";
                SQL = SQL + ComNum.VBLF + "    AND A.SDATE = B.SDATE  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GUBUN = '06'  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD C  ";
                SQL = SQL + ComNum.VBLF + "    ON B.CODE = C.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCD = 'CP지표' ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_BASCD C1  ";
                SQL = SQL + ComNum.VBLF + "    ON B.SCODE = C1.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND C1.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND C1.GRPCD = 'CP지표참조'   ";
                SQL = SQL + ComNum.VBLF + "WHERE A.CPCODE = '" + mCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.SDATE = (SELECT MAX(A1.SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN A1 ";
                SQL = SQL + ComNum.VBLF + "                 WHERE A1.SDATE <= '" + mBDATE.Replace("-", "") + "') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.DSPSEQ  "; //NFLAG4

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssValue_Sheet1.RowCount = dt.Rows.Count;
                    ssValue_Sheet1.SetRowHeight(-1, 36);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssValue_Sheet1.Cells[i, (int)CpValue.CODE].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.NAMEC].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPVALV].Text = dt.Rows[i]["CPVALUE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.GUBUN].Text = dt.Rows[i]["TYPE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.SCODE].Text = dt.Rows[i]["SCODE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.VALGB].Text = ""; //VALGB
                        ssValue_Sheet1.Cells[i, (int)CpValue.NAMES].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                        if (dt.Rows[i]["TYPE"].ToString().Trim() == "구분")
                        {
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Locked = true;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].CellType = rtnTextCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].CellType = rtnTextCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].CellType = rtnComboBoxCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                            string[] arryUserMcro = VB.Split(dt.Rows[i]["SVFLAG1"].ToString().Trim(), "^");
                            clsSpread.gSpreadComboDataSetEx1(ssValue, i, (int)CpValue.CPVAL, i, (int)CpValue.CPVAL, arryUserMcro, true);
                        }
                        else
                        {
                            if (dt.Rows[i]["INPUTGBS"].ToString().Trim() != "연동")
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].CellType = rtnTextCellType;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Locked = true;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Text = dt.Rows[i]["INPUTGBS"].ToString().Trim();
                            }
                            if (dt.Rows[i]["INPUTGBC"].ToString().Trim() != "연동")
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].CellType = rtnTextCellType;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Locked = true;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Text = dt.Rows[i]["INPUTGBC"].ToString().Trim();
                            }

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string KTASTime(string pPTNO, string pINDATE, string pINTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {

                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + " PTMIKTID, PTMIKPR, PTMIKTS, PTMIKTDT, PTMIKTTM, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(TO_DATE( PTMIKTDT||PTMIKTTM,'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') CONDATE,  ";
                SQL = SQL + ComNum.VBLF + " PTMIKJOB, PTMIKIDN, WRITESABUN, SEQNO, SEND, ROWID, WRITESABUN, REALSABUN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_KTAS ";
                SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + pPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + pINDATE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PTMIINTM = '" + pINTIME + "' ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY DECODE(SEQNO, 1, 1, 2) ASC, PTMIKTDT DESC, PTMIKTTM DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["CONDATE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);

                return rtnVal;
            }
        }

        private string JinResult(string pFlag)
        {
            //1.입원 2.귀가 3.DOA 4.사망 5.취소 6.후송 7.DAMA 8.OPD(ER후외래로)
            string rtnVal = "";

            switch(pFlag)
            {
                case "1":
                    rtnVal = "입원";
                    break;
                case "2":
                    rtnVal = "귀가";
                    break;
                case "3":
                    rtnVal = "DOA";
                    break;
                case "4":
                    rtnVal = "사망";
                    break;
                case "5":
                    rtnVal = "취소";
                    break;
                case "6":
                    rtnVal = "후송";
                    break;
                case "7":
                    rtnVal = "DAMA";
                    break;
                case "8":
                    rtnVal = "OPD";
                    break;
            }

            return rtnVal;
        }

        private string JinResultQuery(string pPTNO, string pINDATE, string pINTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {

                SQL = " SELECT "                                                                              ;
                SQL = SQL + ComNum.VBLF + "    PTMIEMRT "                                                     ;
                SQL = SQL + ComNum.VBLF + "FROM NUR_ER_EMIHPTMI  "                                            ;
                SQL = SQL + ComNum.VBLF + "WHERE SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI  "         ;
                SQL = SQL + ComNum.VBLF + "              WHERE PTMIIDNO = '" + pPTNO + "'  "                       ;
                SQL = SQL + ComNum.VBLF + "                AND PTMIINDT = '" + pINDATE + "'  "                       ;
                SQL = SQL + ComNum.VBLF + "                AND PTMIINTM = '" + pINTIME + "')  "                          ;
                SQL = SQL + ComNum.VBLF + "AND PTMIIDNO = '" + pPTNO + "'  ";
                SQL = SQL + ComNum.VBLF + "AND PTMIINDT = '" + pINDATE + "'  ";
                SQL = SQL + ComNum.VBLF + "AND PTMIINTM = '" + pINTIME + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                switch (VB.Left(dt.Rows[0]["PTMIEMRT"].ToString().Trim(), 1))
                {
                    case "1":
                        rtnVal = "귀가";
                        break;
                    case "2":
                        rtnVal = "전원";
                        break;
                    case "3":
                        rtnVal = "입원";
                        break;
                    case "4":
                        rtnVal = "사망";
                        break;
                    case "8":
                        rtnVal = "기타";
                        break;
                    case "9":
                        rtnVal = "기타";
                        break;
                    default:
                        rtnVal = "";
                        break;
                }

                if (dt.Rows[0]["PTMIEMRT"].ToString().Trim() != "")
                {
                    rtnVal = rtnVal + " (" + dt.Rows[0]["PTMIEMRT"].ToString().Trim() + ")";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);

                return rtnVal;
            }
        }

        #endregion //Function

        #region // 참조값과 실행값을 조회한다

        private string GetCpTime(string strCPCODE)
        {
            string rtnVal = "";
            DataTable dt = null;
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (strCPCODE == "CPPLN0001")
            {
                SQL = CpQuery_CPPLN0001();
            }
            else if (strCPCODE == "CPPLN0002")
            {
                SQL = CpQuery_CPPLN0002();
            }
            else if (strCPCODE == "CPPLN0003")
            {
                SQL = CpQuery_CPPLN0003();
            }
            else if (strCPCODE == "CPPLN0004")
            {
                SQL = CpQuery_CPPLN0004();
            }
            else if (strCPCODE == "CPPLN0005")
            {
                SQL = CpQuery_CPPLN0005();
            }
            else if (strCPCODE == "CPPLN0006")
            {
                SQL = CpQuery_CPPLN0006();
            }
            else if (strCPCODE == "CPPLN0007")
            {
                SQL = CpQuery_CPPLN0007();
            }
            else if (strCPCODE == "CPPLN0008")
            {
                SQL = CpQuery_CPPLN0008();
            }
            else if (strCPCODE == "CPPLN0009")
            {
                SQL = CpQuery_CPPLN0009();
            }
            else if (strCPCODE == "CPPLN0010")
            {
                SQL = CpQuery_CPPLN0010();
            }
            else if (strCPCODE == "CPPLN0011")
            {
                SQL = CpQuery_CPPLN0011();
            }
            else if (strCPCODE == "CPPLN0012")
            {
                SQL = CpQuery_CPPLN0012();
            }
            else if (strCPCODE == "CPPLN0013")
            {
                SQL = CpQuery_CPPLN0013();
            }
            else if (strCPCODE == "CPPLN0014")
            {
                SQL = CpQuery_CPPLN0014();
            }
            else if (strCPCODE == "CPPLN0015")
            {
                SQL = CpQuery_CPPLN0015();
            }
            else if (strCPCODE == "CPPLN0016")
            {
                SQL = CpQuery_CPPLN0016();
            }
            else if (strCPCODE == "CPPLN0017")
            {
                SQL = CpQuery_CPPLN0017();
            }
            if (SQL == "")
            {
                return rtnVal;
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }

            rtnVal = dt.Rows[0]["CPDATE"].ToString().Trim();
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        private string GetRefTime(string strREFCODE)
        {
            string rtnVal = "";
            DataTable dt = null;
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (strREFCODE == "CPREF0011")
            {
                SQL = RefQuery_CPREF0011();
            }
            else if (strREFCODE == "CPREF0012")
            {
                SQL = RefQuery_CPREF0012();
            }
            else if (strREFCODE == "CPREF0013")
            {
                SQL = RefQuery_CPREF0013();
            }
            else if (strREFCODE == "CPREF0014")
            {
                SQL = RefQuery_CPREF0014();
            }
            else if (strREFCODE == "CPREF0015")
            {
                SQL = RefQuery_CPREF0015();
            }
            else if (strREFCODE == "CPREF0016")
            {
                SQL = RefQuery_CPREF0016();
            }
            else if (strREFCODE == "CPREF0017")
            {
                SQL = RefQuery_CPREF0017();
            }
            else if (strREFCODE == "CPREF0018")
            {
                SQL = RefQuery_CPREF0018();
            }
            else if (strREFCODE == "CPREF0019")
            {
                SQL = RefQuery_CPREF0019();
            }
            else if (strREFCODE == "CPREF0020")
            {
                SQL = RefQuery_CPREF0020();
            }
            else if (strREFCODE == "CPREF0021")
            {
                SQL = RefQuery_CPREF0021();
            }

            if (SQL == "")
            {
                return rtnVal;
            }
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }

            rtnVal = dt.Rows[0]["REFDATE"].ToString().Trim();
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        #endregion // 참조값과 실행값을 조회한다

        #region //Cp Query

        //CP Activation time
        string CpQuery_CPPLN0001()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(STARTDATE, 1, 4) || '-' || SUBSTR(STARTDATE,5,2) || '-' || SUBSTR(STARTDATE,7,2) ";
            SQL = SQL + ComNum.VBLF + "    || ' ' || SUBSTR(STARTTIME, 1, 2) || ':' || SUBSTR(STARTTIME,3,2) AS CPDATE";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD";
            SQL = SQL + ComNum.VBLF + "WHERE  CPNO = " + mCPNO;
            return SQL;
        }

        //Brain CT time : 수기
        string CpQuery_CPPLN0002()
        {
            string SQL = "";    //Query문
            SQL = "";
            
            return SQL;
        }

        //Brain MR diffusion time : 수기
        string CpQuery_CPPLN0003()
        {
            string SQL = "";    //Query문
            SQL = "";
            return SQL;
        }

        //대면(진료)시행 time (협진)
        string CpQuery_CPPLN0004()
        {
            string SQL = "";    //Query문
            //HODEPT1
            //Stroke : 주과(NE/NS) 진료시간
            //소아 Seizure : 주과(PD) 진료시간
            //소아장중첩증 : 주과(GS) 진료시간

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HODATE1, 'YYYY-MM-DD HH24:MI') AS CPDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            if (mCPCODE == "CPCODE0001")
            {
                SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('NE','NS') ";
            }
            else if (mCPCODE == "CPCODE0003")
            {
                SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('PD') ";
            }
            else if (mCPCODE == "CPCODE0005")
            {
                SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('GS') ";
            }
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //IVT(actylass) time : TODO
        string CpQuery_CPPLN0005()
        {
            //KOSMOS_OCS.OCS_IORDER_ACT_ER
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(N.CHARTDATE, 1, 4) || '-' || SUBSTR(N.CHARTDATE, 5, 2) || '-' || SUBSTR(N.CHARTDATE, 7, 2) || ' ' ||          ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(N.CHARTTIME, 1, 2) || ':' || SUBSTR(N.CHARTTIME, 3, 2) AS CPDATE                                          ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.EMRXML_TUYAK N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PTNO ";
            SQL = SQL + ComNum.VBLF + "     AND TO_CHAR(R.BDATE, 'YYYYMMDD') = N.MEDFRDATE ";
            SQL = SQL + ComNum.VBLF + "     AND N.MEDDEPTCD = 'ER' ";
            SQL = SQL + ComNum.VBLF + "     AND N.IT2 IN ('TPA2', 'TPA5')";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;

            SQL = SQL + ComNum.VBLF + "UNION ALL";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(N.CHARTDATE, 1, 4) || '-' || SUBSTR(N.CHARTDATE, 5, 2) || '-' || SUBSTR(N.CHARTDATE, 7, 2) || ' ' ||          ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(N.CHARTTIME, 1, 2) || ':' || SUBSTR(N.CHARTTIME, 3, 2) AS CPDATE                                          ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRCHARTMST N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PTNO ";
            SQL = SQL + ComNum.VBLF + "     AND TO_CHAR(R.BDATE, 'YYYYMMDD') = N.MEDFRDATE ";
            SQL = SQL + ComNum.VBLF + "     AND N.MEDDEPTCD = 'ER' ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRCHARTROW CR ";
            SQL = SQL + ComNum.VBLF + "     ON CR.EMRNO = N.EMRNO";
            SQL = SQL + ComNum.VBLF + "     AND CR.EMRNOHIS = N.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "     AND CR.ITEMNO = 'I0000037685'";
            SQL = SQL + ComNum.VBLF + "     AND CR.ITEMVALUE IN ('TPA2', 'TPA5')";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //IAT 처치 time
        string CpQuery_CPPLN0006()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS CPDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //EKG time : 수기입력
        string CpQuery_CPPLN0007()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //ER Exit time
        string CpQuery_CPPLN0008()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS CPDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //호출 time : 호출시각1
        string CpQuery_CPPLN0009()
        {
            string SQL = "";    //Query문
            //Stroke : 주과(NE/NS) 호출시간
            //소아 Seizure : 주과(PD) 호출시간
            //UGI Bleeding : 주과(MG) 호출시간
            //소아장중첩증 : 주과(GS) 호출시간
            //골반골절 : 주과(ER) 호출시간
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOTIME1, 'YYYY-MM-DD HH24:MI') AS CPDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            //if (mCPCODE == "CPCODE0001")
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('NE','NS') ";
            //}
            //else if (mCPCODE == "CPCODE0003")
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('PD') ";
            //}
            //else if (mCPCODE == "CPCODE0004")
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('MG') ";
            //}
            //else if (mCPCODE == "CPCODE0005")
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('GS') ";
            //}
            //else if (mCPCODE == "CPCODE0006")
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND HODEPT1 IN ('ER') ";
            //}
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //GFS시행 time : 수기입력
        string CpQuery_CPPLN0010()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //초음파  time : 수기입력
        string CpQuery_CPPLN0011()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //Reduction   time : 수기입력
        string CpQuery_CPPLN0012()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //외과 협진 사유 : 선택
        string CpQuery_CPPLN0013()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //재실 time
        string CpQuery_CPPLN0014()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS CPDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //EMS arrival time : 수기입력
        string CpQuery_CPPLN0015()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //Transfer time : 전원선택시
        string CpQuery_CPPLN0016()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS CPDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //GFS결정 time : 수기입력
        string CpQuery_CPPLN0017()
        {
            string SQL = "";    //Query문
            SQL = "";
            return SQL;
        }

        #endregion

        #region //Ref Query

        //내원 time
        string RefQuery_CPREF0011()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.INTIME, 'YYYY-MM-DD HH24:MI') AS REFDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //호출 time
        string RefQuery_CPREF0012()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOTIME1, 'YYYY-MM-DD HH24:MI') AS REFDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //CP activation time
        string RefQuery_CPREF0013()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(STARTDATE, 1, 4) || '-' || SUBSTR(STARTDATE,5,2) || '-' || SUBSTR(STARTDATE,7,2) ";
            SQL = SQL + ComNum.VBLF + "    || ' ' || SUBSTR(STARTTIME, 1, 2) || ':' || SUBSTR(STARTTIME,3,2) AS REFDATE";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD";
            SQL = SQL + ComNum.VBLF + "WHERE  CPNO = " + mCPNO;
            return SQL;
        }

        //KTAS time
        string RefQuery_CPREF0014()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                                                                                             ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(R.REFDATE, 1, 4) || '-' || SUBSTR(R.REFDATE, 5, 2) || '-' || SUBSTR(R.REFDATE, 7, 2) || ' ' ||          ";
            SQL = SQL + ComNum.VBLF + "    SUBSTR(R.REFDATE, 9, 2) || ':' || SUBSTR(R.REFDATE, 11, 2) AS REFDATE                                          ";
            SQL = SQL + ComNum.VBLF + "FROM                                                                                                               ";
            SQL = SQL + ComNum.VBLF + "(                                                                                                                  ";
            SQL = SQL + ComNum.VBLF + "    SELECT                                                                                                         ";
            SQL = SQL + ComNum.VBLF + "        MIN(PTMIKTDT || PTMIKTTM) AS REFDATE                                                                       ";
            SQL = SQL + ComNum.VBLF + "    FROM  KOSMOS_OCS.OCS_CP_RECORD R                                                                               ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N                                                                        ";
            SQL = SQL + ComNum.VBLF + "         ON R.PTNO = N.PANO                                                                                        ";
            SQL = SQL + ComNum.VBLF + "         AND R.BDATE = N.JDATE                                                                                     ";
            SQL = SQL + ComNum.VBLF + "         AND R.INTIME = N.INTIME                                                                                   ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.NUR_ER_KTAS K                                                                           ";
            SQL = SQL + ComNum.VBLF + "         ON N.PANO = K.PTMIIDNO                                                                                    ";
            SQL = SQL + ComNum.VBLF + "         AND TO_CHAR(R.INTIME, 'YYYYMMDD') = K.PTMIINDT                                                            ";
            SQL = SQL + ComNum.VBLF + "         AND TO_CHAR(R.INTIME, 'HH24MI') = K.PTMIINTM                                                              ";
            SQL = SQL + ComNum.VBLF + "    WHERE  R.CPNO = " + mCPNO;
            SQL = SQL + ComNum.VBLF + "    ) R                                                                                                            ";
            return SQL;
        }

        //SMS호출 time
        string RefQuery_CPREF0015()
        {
            string SQL = "";    //Query문
            //SQL = "";
            //SQL = SQL + ComNum.VBLF + "SELECT ";
            //SQL = SQL + ComNum.VBLF + "    SUBSTR(R.SMSDATE, 1, 4) || '-' || SUBSTR(R.SMSDATE, 5, 2) || '-' || SUBSTR(R.SMSDATE, 7, 2) || ' '  || ";
            //SQL = SQL + ComNum.VBLF + "    SUBSTR(R.SMSTIME, 1, 2) || ':' || SUBSTR(R.SMSDATE, 3, 2) AS REFDATE ";
            //SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            //SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            //SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            //SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            //SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            //SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOTIME1, 'YYYY-MM-DD HH24:MI') AS REFDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;

            return SQL;
        }

        //GFS결정 time : 수기입력
        string RefQuery_CPREF0016()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //영상의학과 호출 time
        string RefQuery_CPREF0017()
        {
            string SQL = "";    //Query문
            
            SQL = " SELECT  "                                                                  ;
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(HOTIME1,'YYYY-MM-DD HH24:MI') REFDATE "                    ;
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R "                                    ;
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N  "                            ;
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  "                                            ;
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  "                                         ;
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  "                                       ;
            SQL = SQL + ComNum.VBLF + "     AND N.HODEPT1 = 'RD' "                                           ;
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            SQL = SQL + ComNum.VBLF + "UNION "                                                               ;
            SQL = SQL + ComNum.VBLF + "SELECT  "                                                             ;
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(HOTIME2,'YYYY-MM-DD HH24:MI') REFDATE "                    ;
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R "                                    ;
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N  "                            ;
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  "                                            ;
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  "                                         ;
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  "                                       ;
            SQL = SQL + ComNum.VBLF + "     AND N.HODEPT2 = 'RD' "                                           ;
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            SQL = SQL + ComNum.VBLF + "UNION "                                                               ;
            SQL = SQL + ComNum.VBLF + "SELECT  "                                                             ;
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(HOTIME3,'YYYY-MM-DD HH24:MI') REFDATE "                    ;
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R "                                    ;
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N  "                            ;
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  "                                            ;
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  "                                         ;
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  "                                       ;
            SQL = SQL + ComNum.VBLF + "     AND N.HODEPT3 = 'RD' "                                           ;
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            SQL = SQL + ComNum.VBLF + "UNION "                                                               ;
            SQL = SQL + ComNum.VBLF + "SELECT  "                                                             ;
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(HOTIME4,'YYYY-MM-DD HH24:MI') REFDATE "                    ;
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R "                                    ;
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N  "                            ;
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  "                                            ;
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  "                                         ;
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  "                                       ;
            SQL = SQL + ComNum.VBLF + "     AND N.HODEPT4 = 'RD' "                                           ;
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            SQL = SQL + ComNum.VBLF + "UNION "                                                               ;
            SQL = SQL + ComNum.VBLF + "SELECT  "                                                             ;
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(HOTIME5,'YYYY-MM-DD HH24:MI') REFDATE "                    ;
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R "                                    ;
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N  "                            ;
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  "                                            ;
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  "                                         ;
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  "                                       ;
            SQL = SQL + ComNum.VBLF + "     AND N.HODEPT5 = 'RD' "                                           ;
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;

            return SQL;
        }

        //초음파 time : 수기입력
        string RefQuery_CPREF0018()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //EMS호출 time : 수기입력
        string RefQuery_CPREF0019()
        {
            string SQL = "";    //Query문
            return SQL;
        }

        //전원결정 time : 전원선택시 주과 결정시각
        string RefQuery_CPREF0020()
        {
            string SQL = "";    //Query문
            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOJINDATE1,'YYYY-MM-DD HH24:MI')  AS REFDATE ";
            SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_CP_RECORD R";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "WHERE  R.CPNO = " + mCPNO;
            return SQL;
        }

        //EKG time : 수기입력
        string RefQuery_CPREF0021()
        {
            string SQL = "";    //Query문
            return SQL;
        }
        
        #endregion

        #region //사용안함
        //사용안함
        private string GeKTasTime(string strPTNO, string strINDATE, string strINTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(TO_DATE( PTMIKTDT||PTMIKTTM,'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') CONDATE ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_ER_KTAS ";
                SQL = SQL + ComNum.VBLF + "WHERE PTMIIDNO ='" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "     AND PTMIINDT ='" + strINDATE + "' ";
                SQL = SQL + ComNum.VBLF + "     AND PTMIINTM ='" + strINTIME + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(SEQNO, 1, 1, 2) ASC, PTMIKTDT DESC, PTMIKTTM DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["CONDATE"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        //사용안함
        private string GeKTasLevel(string strPTNO, string strINDATE, string strINTIME)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "     PTMIKTS ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_ER_KTAS ";
                SQL = SQL + ComNum.VBLF + "WHERE PTMIIDNO ='" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "     AND PTMIINDT ='" + strINDATE + "' ";
                SQL = SQL + ComNum.VBLF + "     AND PTMIINTM ='" + strINTIME + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(SEQNO, 1, 1, 2) DESC, PTMIKTDT DESC, PTMIKTTM DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["PTMIKTS"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }


        #endregion

        private void frmOcsCpGoalReg_FormClosed(object sender, FormClosedEventArgs e)
        {
            OE = null;
            if (frmOcsCpSelectX != null)
            {
                frmOcsCpSelectX.Dispose();
                frmOcsCpSelectX = null;
            }
            
            if (mCallForm != null)
            {
                rEventClosed();
            }
        }

        private void DispCpGaol(string pCPCODE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            ssValue_Sheet1.RowCount = 0;

            FarPoint.Win.Spread.CellType.TextCellType rtnTextCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType rtnCheckBoxCellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType rtnComboBoxCellType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            try
            {
                string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    A.CPCODE, A.SDATE, B.GUBUN, B.CODE, B.NAME, B.TYPE, B.SCODE, B.CPVALUE, B.INPUTGBC, B.INPUTGBS, B.DSPSEQ,  ";
                SQL = SQL + ComNum.VBLF + "    C.NFLAG1 , C1.BASNAME AS SNAME, C1.VFLAG1 AS SVFLAG1 ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_MAIN A  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_OCS.OCS_CP_SUB B  ";
                SQL = SQL + ComNum.VBLF + "    ON A.CPCODE = B.CPCODE  ";
                SQL = SQL + ComNum.VBLF + "    AND A.SDATE = B.SDATE  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GUBUN = '06'  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD C  ";
                SQL = SQL + ComNum.VBLF + "    ON B.CODE = C.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND C.GRPCD = 'CP지표' ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_BASCD C1  ";
                SQL = SQL + ComNum.VBLF + "    ON B.SCODE = C1.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND C1.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND C1.GRPCD = 'CP지표참조'   ";
                SQL = SQL + ComNum.VBLF + "WHERE A.CPCODE = '" + pCPCODE + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.SDATE = (SELECT MAX(A1.SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN A1 ";
                SQL = SQL + ComNum.VBLF + "                 WHERE A1.SDATE <= '" + strCurDate + "') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.DSPSEQ  "; //NFLAG4

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssValue_Sheet1.RowCount = dt.Rows.Count;
                    ssValue_Sheet1.SetRowHeight(-1, 36);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssValue_Sheet1.Cells[i, (int)CpValue.CODE].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.NAMEC].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.CPVALV].Text = dt.Rows[i]["CPVALUE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.GUBUN].Text = dt.Rows[i]["TYPE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.SCODE].Text = dt.Rows[i]["SCODE"].ToString().Trim();
                        ssValue_Sheet1.Cells[i, (int)CpValue.VALGB].Text = ""; //VALGB
                        ssValue_Sheet1.Cells[i, (int)CpValue.NAMES].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                        if (dt.Rows[i]["TYPE"].ToString().Trim() == "구분")
                        {
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPSTIME].Locked = true;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPETIME].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].CellType = rtnTextCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].CellType = rtnTextCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Locked = true;

                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].CellType = rtnComboBoxCellType;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            ssValue_Sheet1.Cells[i, (int)CpValue.CPVAL].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                            string[] arryUserMcro = VB.Split(dt.Rows[i]["SVFLAG1"].ToString().Trim(), "^");
                            clsSpread.gSpreadComboDataSetEx1(ssValue, i, (int)CpValue.CPVAL, i, (int)CpValue.CPVAL, arryUserMcro, true);
                        }
                        else
                        {
                            if (dt.Rows[i]["INPUTGBS"].ToString().Trim() != "연동")
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].CellType = rtnTextCellType;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Locked = true;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCS].Text = dt.Rows[i]["INPUTGBS"].ToString().Trim();
                            }
                            if (dt.Rows[i]["INPUTGBC"].ToString().Trim() != "연동")
                            {
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].CellType = rtnTextCellType;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Locked = true;
                                ssValue_Sheet1.Cells[i, (int)CpValue.BTNCC].Text = dt.Rows[i]["INPUTGBC"].ToString().Trim();
                            }

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void CrearButton()
        {
            btnStroke.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
            btnSTEMI.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
            btnSeizure.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
            btnUGI.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
            btnDouble.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
            btnBroken.Font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);
        }

        private void btnStroke_Click(object sender, EventArgs e)
        {
            CrearButton();
            btnStroke.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
            DispCpGaol("CPCODE0001");
        }

        private void btnSTEMI_Click(object sender, EventArgs e)
        {
            CrearButton();
            btnSTEMI.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
            DispCpGaol("CPCODE0002");
        }

        private void btnSeizure_Click(object sender, EventArgs e)
        {
            CrearButton();
            btnSeizure.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
            DispCpGaol("CPCODE0003");
        }

        private void btnUGI_Click(object sender, EventArgs e)
        {
            CrearButton();
            btnUGI.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
            DispCpGaol("CPCODE0004");
        }

        private void btnDouble_Click(object sender, EventArgs e)
        {
            CrearButton();
            btnDouble.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
            DispCpGaol("CPCODE0005");
        }

        private void btnBroken_Click(object sender, EventArgs e)
        {
            CrearButton();
            btnBroken.Font = new Font("맑은 고딕", 9.75f, FontStyle.Bold);
            DispCpGaol("CPCODE0006");
        }

        private void btnSaveEms_Click(object sender, EventArgs e)
        {
            if (UpdateDataEms() == true)
            {

            }
        }

        private bool UpdateDataEms()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return false; //권한 확인
            }

            if (txtEMS.Text.Replace("-", "").Replace(":", "").Trim() == "")
            {
                if (ComFunc.MsgBoxQ("도착시간이 비었습니다." + ComNum.VBLF + "저장하시겠습니까?","PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No )
                {
                    txtEMS.Focus();
                    return false;
                }
            }
            
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strDateCall = VB.Left(txtEMSCALL.Text.Trim().Replace("-", "").Replace(":", ""), 8);
            string strTimeCall = VB.Right(txtEMSCALL.Text.Trim().Replace("-", "").Replace(":", ""), 4);
            string strFullDateCall = ComFunc.FormatStrToDate(strDateCall, "D") + " " + ComFunc.FormatStrToDate(strTimeCall, "M");

            if (txtEMSCALL.Text.Replace("-", "").Replace(":", "").Trim() != "")
            {
                if (VB.IsDate(strFullDateCall) == false)
                {
                    ComFunc.MsgBox("호출시간 날짜 형식이 틀립니다.");
                    txtEMS.Focus();
                    return false;
                }
            }

            string strDate = VB.Left(txtEMS.Text.Trim().Replace("-","").Replace(":", ""), 8);
            string strTime = VB.Right(txtEMS.Text.Trim().Replace("-", "").Replace(":", ""), 4);
            string strFullDate = ComFunc.FormatStrToDate(strDate,"D") + " " + ComFunc.FormatStrToDate(strTime, "M");

            if (txtEMS.Text.Replace("-", "").Replace(":", "").Trim() != "")
            {
                if (VB.IsDate(strFullDate) == false)
                {
                    ComFunc.MsgBox("도착시간 날짜 형식이 틀립니다.");
                    txtEMS.Focus();
                    return false;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "UPDATE KOSMOS_OCS.OCS_CP_RECORD SET";
                SQL = SQL + ComNum.VBLF + "     EMS_CALL_DATE = '" + strDateCall + "', ";
                SQL = SQL + ComNum.VBLF + "     EMS_CALL_TIME = '" + strTimeCall + "', ";
                SQL = SQL + ComNum.VBLF + "     EMS_DATE = '" + strDate + "', ";
                SQL = SQL + ComNum.VBLF + "     EMS_TIME = '" + strTime + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE CPNO = " + mCPNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnEmr_Click(object sender, EventArgs e)
        {
            if (lblPTNO.Text.Trim() == "") return;
            
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    R.CPNO, R.PTNO, R.PTNAME, R.GBIO, TO_CHAR(R.BDATE, 'YYYY-MM-DD') AS BDATE,  R.EMS_DATE, R.EMS_TIME, ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(R.INTIME, 'YYYY-MM-DD HH24:MI') AS INTIME,  ";
            SQL = SQL + ComNum.VBLF + "    P.JUMIN1, P.JUMIN2,  ";
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS OUTTIME ,";
            SQL = SQL + ComNum.VBLF + "    N.SEX, N.AGE, ";
            SQL = SQL + ComNum.VBLF + "    N.DRNAME, "; //ER의사
            SQL = SQL + ComNum.VBLF + "    N.HODEPT1,"; //주과
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.BALDATE, 'YYYY-MM-DD HH24:MI') AS BALDATE,"; //발병일자
            SQL = SQL + ComNum.VBLF + "    N.KTASLEVL,"; //KTASLEVL
            SQL = SQL + ComNum.VBLF + "    N.OUTGBN,"; //진료결과(퇴원구분) 1.입원 2.귀가 3.DOA 4.사망 5.취소 6.후송 7.DAMA 8.OPD(ER후외래로)
            SQL = SQL + ComNum.VBLF + "    N.DEPTCODE AS MDEPTCODE,"; //주과
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOTIME1, 'YYYY-MM-DD HH24:MI') AS HOTIME1,"; //호출시각1
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HODATE1, 'YYYY-MM-DD HH24:MI') AS HODATE1,"; //대면시각
            SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.HOJINDATE1, 'YYYY-MM-DD HH24:MI') AS HOJINDATE1,"; //결정시각
            SQL = SQL + ComNum.VBLF + "    R.DEPTCODE, R.BI, R.CPCODE, R.AGE, R.SEX,  ";
            SQL = SQL + ComNum.VBLF + "    R.STARTSABUN, U.USERNAME,  ";
            SQL = SQL + ComNum.VBLF + "    B.BASNAME AS CPNAME, ";
            SQL = SQL + ComNum.VBLF + "    D.DRCODE ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_RECORD R  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_PATIENT P ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = P.PANO ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N ";
            SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO ";
            SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE ";
            SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD B  ";
            SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD  ";
            SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'  ";
            SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_USER U ";
            SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER  ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_OCS.OCS_DOCTOR D ";
            SQL = SQL + ComNum.VBLF + "    ON D.SABUN = U.SABUN  ";
            SQL = SQL + ComNum.VBLF + "WHERE R.CPNO = " + mCPNO;

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

            string BFDate = "";
            string Pano = "";
            string DeptCode = "";
            string DrCode = "";

            Pano = dt.Rows[0]["PTNO"].ToString().Trim();
            BFDate = dt.Rows[0]["BDATE"].ToString().Trim();
            DeptCode = "ER";
            DrCode = dt.Rows[0]["DRCODE"].ToString().Trim();

            dt.Dispose();
            dt = null;

            #region //Call EMR Old - 응급실
            string strJobEmrName = ""; //외래 : EMROORDER, 입원 : EMRIORDER, 응급실 : EMREORDER
            string strFrDate = "";
            string strInoutCls = "O"; //외래 입원 구분(외래 : O , 입원 : I , 응급실 : O
            string strPara = "";
            strJobEmrName = "EMREORDER";
            strFrDate = BFDate.Replace("-", "");
            strInoutCls = "O";
            bool RunEmrView = CheckExecEmrViewOld();
            if (RunEmrView == true)
            {
                strPara = strJobEmrName + "/" + strInoutCls + "/" + strFrDate + "/" + DeptCode + "/" + DrCode;
            }
            else
            {
                strPara = strJobEmrName + "," + strInoutCls + "," + strFrDate + "," + DeptCode + "," + DrCode;
            }
            clsVbEmr.EXECUTE_TextEmrViewEx(Pano, clsType.User.Sabun, strPara);
            #endregion
        }

        /// <summary>
        /// EMR Viewer 가 실행중인지 확인한다
        /// </summary>
        /// <returns></returns>
        private bool CheckExecEmrViewOld()
        {
            bool ActiveProc = false;
            System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
            if (ProcessEx.Length > 0)
            {
                System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("mhemrviewer");
                System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
                foreach (System.Diagnostics.Process Proc in Pro1)
                {
                    if (Proc.Id != CurPro.Id)
                    {
                        ActiveProc = true;
                    }
                }
            }
            return ActiveProc;
        }

        private void btnSMS_Click(object sender, EventArgs e)
        {
            OCR.CPNO = 0;
            OCR.CP_CODE = "";
            OCR.CP_ROWID = "";
            OCR.CP_CNT = 0;
            OCR.CP_SELECT = false;
            OCR.CP_NEW = false;

            if (lblPTNO.Text.Trim() == "")
            {
                return;
            }

            OCR.PtNo = lblPTNO.Text.Trim();
            //
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
            SQL = "";
            SQL = "SELECT";
            SQL = SQL + ComNum.VBLF + "     ROWID";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + OCR.PtNo + "' ";
            SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + VB.Left(lblINDATE.Text.Trim(), 10) + "', 'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = 'ER' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            OCR.OPD_ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            dt.Dispose();
            dt = null;

            OE.Read_ERPat_Info(ref OCR);

            dt = OE.Read_CP_ReCord_Chk(OCR);

            if (dt.Rows.Count > 1)
            {
                OCR.CP_SELECT = false;

                frmOcsCpSelectX = new frmOcsCpSelect(OCR.PtNo, OCR.ER_PATIENT_InDate, OCR.ER_PATIENT_InTime);
                frmOcsCpSelectX.rSetCpInto += new frmOcsCpSelect.SetCpInto(frmOcsCpSelect_frmOcsCpSelect);
                frmOcsCpSelectX.ShowDialog(this);

                if (OCR.CP_SELECT == true && OCR.CP_NEW == false)
                {
                    OE.Read_ERPat_Info(ref OCR);
                    frmOcsCpSmsInfo frm2 = new frmOcsCpSmsInfo(OCR.PtNo, OCR.OPD_ROWID, OCR.DeptCode, OCR.BDate, OCR.ER_PATIENT_InDate, OCR.ER_PATIENT_InTime, OCR.CPNO);
                    frm2.StartPosition = FormStartPosition.CenterParent;
                    frm2.ShowDialog(this);
                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요");
                }
            }
            else
            {
                frmOcsCpSmsInfo frm2 = new frmOcsCpSmsInfo(OCR.PtNo, OCR.OPD_ROWID, OCR.DeptCode, OCR.BDate, OCR.ER_PATIENT_InDate, OCR.ER_PATIENT_InTime, OCR.CPNO);
                frm2.StartPosition = FormStartPosition.CenterParent;
                frm2.ShowDialog(this);
            }
        }

        private void frmOcsCpSelect_frmOcsCpSelect(double pCPNO, bool pCP_SELECT, bool pCP_NEW)
        {
            frmOcsCpSelectX.Dispose();
            frmOcsCpSelectX = null;

            OCR.CPNO = pCPNO;
            OCR.CP_SELECT = pCP_SELECT;
            OCR.CP_NEW = pCP_NEW;
        }

        private void ssValue_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            if (ssValue_Sheet1.RowCount == 0) return;
            int Row = e.Row;
            int Col = e.Column;
            string strCPCODE = ssValue_Sheet1.Cells[Row, (int)CpValue.CODE].Text;
            string strREFCODE = ssValue_Sheet1.Cells[Row, (int)CpValue.SCODE].Text;
            string strTYPE = ssValue_Sheet1.Cells[Row, (int)CpValue.GUBUN].Text;
            long lngRefVal = (long)VB.Val(ssValue_Sheet1.Cells[Row, (int)CpValue.CPVALV].Text);

            if (strTYPE == "구분")
            {
                return;
            }

            if (Col == (int)CpValue.CPSTIME || Col == (int)CpValue.CPETIME)
            {
                ssValue_Sheet1.Cells[Row, (int)CpValue.CPVAL].Text = ""; //지표값
                ssValue_Sheet1.Cells[Row, (int)CpValue.CPRSTN].Text = ""; //평가
                if (ssValue_Sheet1.Cells[Row, (int)CpValue.CPSTIME].Text.Trim() != "" && ssValue_Sheet1.Cells[Row, (int)CpValue.CPETIME].Text.Trim() != "")
                {
                    long CpVal = CalcCpVal(ssValue_Sheet1.Cells[Row, (int)CpValue.CPETIME].Text, ssValue_Sheet1.Cells[Row, (int)CpValue.CPSTIME].Text);
                    long CpCheck = CpVal - lngRefVal;

                    ssValue_Sheet1.Cells[Row, (int)CpValue.CPVAL].Text = CpVal.ToString(); //지표값
                    if (CpCheck > 0)
                    {
                        ssValue_Sheet1.Cells[Row, (int)CpValue.CODE, Row, (int)CpValue.CPRSTN].ForeColor = System.Drawing.Color.Red;
                        ssValue_Sheet1.Cells[Row, (int)CpValue.CPRSTN].Text = "+ " + CpCheck.ToString(); //평가
                    }
                    else
                    {
                        ssValue_Sheet1.Cells[Row, (int)CpValue.CPRSTN].Text = CpCheck.ToString(); //평가
                                                                                                  //ssValue_Sheet1.Cells[Row, 9].Text = ""; //평가
                    }
                }
            }
        }
    }
}
