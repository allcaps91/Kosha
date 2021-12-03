using System;
using System.Data;
using System.Windows.Forms;
using ComBase;


namespace ComLibB
{
    public partial class frmOcsCpSmsInfo : Form
    {
        clsOrderEtc OE = null;
        clsOrderEtc.OCS_CP_RECORD OCR = new clsOrderEtc.OCS_CP_RECORD();
        string mPtNo = "";
        string mOPD_ROWID = "";
        string mCP_DEPT = "";
        string mBDate = "";
        string mER_PATIENT_InDate = "";
        string mER_PATIENT_InTime = "";
        double mCPNO = 0;

        public frmOcsCpSmsInfo()
        {
            InitializeComponent();
        }

        public frmOcsCpSmsInfo(string pPtNo, string pMst_ROWID, string pCP_DEPT, string pBDate, 
                                string pER_PATIENT_InDate, string pER_PATIENT_InTime, double pCPNO)
        {
            InitializeComponent();
            mPtNo = pPtNo;
            mOPD_ROWID = pMst_ROWID;
            mCP_DEPT = pCP_DEPT;
            mBDate = pBDate;
            mER_PATIENT_InDate = pER_PATIENT_InDate;
            mER_PATIENT_InTime = pER_PATIENT_InTime;
            mCPNO = pCPNO;
        }

        private void frmOcsCpSmsInfo_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            SCREEN_CLEAR();

            OE = new clsOrderEtc();
            OCR.PtNo = mPtNo;
            OCR.OPD_ROWID = mOPD_ROWID;
            OCR.CP_DEPT = mCP_DEPT;
            OCR.BDate = mBDate;
            OCR.ER_PATIENT_InDate = mER_PATIENT_InDate;
            OCR.ER_PATIENT_InTime = mER_PATIENT_InTime;
            OCR.CPNO = mCPNO;

            Read_Pat_Info();
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SCREEN_CLEAR()
        {
            txtCallacti.Text = "";
            txtCalldeacti.Text = "";
            txtCallwarm.Text = "";
            txtCallact.Text = "";

            ssSMS_Sheet1.RowCount = 0;

            cboCpCode.Text = "";
            cboCpCode.Items.Clear();
        }

        private void Read_Pat_Info()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strTemp = "";

            lblPatInfo.Text = "";

            OE.Read_ERPat_Info(ref OCR);

            lblPatInfo.Text = OCR.CP_STS;

            if (OCR.ER_PATIENT_InDate == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //CP 등록 체크
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CPNO, Ptno, GbIO, DeptCode, Bi ";
                SQL = SQL + ComNum.VBLF + "     , CpCode, ROWID ";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE ";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(InTIME,'YYYY-MM-DD HH24:MI') AS InTIME ";
                SQL = SQL + ComNum.VBLF + "     , WarmDate, WarmTime, WarmSabun ";           //예비CP
                SQL = SQL + ComNum.VBLF + "     , StartDate, StartTime, StartSabun ";        //CP등록
                SQL = SQL + ComNum.VBLF + "     , ActDate, ActTime, ActSabun ";              //시술
                SQL = SQL + ComNum.VBLF + "     , DropDate, DropTime, DropSabun ";           //CP제외
                SQL = SQL + ComNum.VBLF + "     , CancerDate, CancerTime, CancerSabun ";     //CP중단
                SQL = SQL + ComNum.VBLF + "     , CallDate, CallTime, CallSabun ";           //의사콜
                SQL = SQL + ComNum.VBLF + "     ,Call_Warm_Date,Call_Warm_Time,Call_Warm_Sabun ";//           '콜
                SQL = SQL + ComNum.VBLF + "     ,Call_Acti_Date,Call_Acti_Time,Call_Acti_Sabun ";//           '콜
                SQL = SQL + ComNum.VBLF + "     ,Call_DeActi_Date,Call_DeActi_Time,Call_DeActi_Sabun ";//      '콜
                SQL = SQL + ComNum.VBLF + "     ,Call_Act_Date,Call_Act_Time,Call_Act_Sabun ";//              '콜
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_RECORD ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND PTNO = '" + OCR.PtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDate = TO_DATE('" + OCR.BDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND GbIO = 'E' ";
                SQL = SQL + ComNum.VBLF + "         AND InTime = TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI')  ";

                if (OCR.CPNO != 0)
                {
                    SQL = SQL + ComNum.VBLF + "   AND CPNO = " + OCR.CPNO + "  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND InTime = TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI')  ";
                }

                if (OCR.CP_SELECT == true)
                {
                    if (OCR.CP_NEW == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND CPNO = '1'  ";
                    }
                    else
                    {
                        if (OCR.CP_CNT > 1 && OCR.CPNO > 0)
                        {
                            SQL = SQL + ComNum.VBLF + "         AND CPNO = '" + OCR.CPNO + "' ";
                        }
                    }
                }
                else
                {
                    if (OCR.CP_NEW == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND CPNO = '1'  ";
                    }
                }

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
                    lblPatInfo.Text += " 미등록 )";
                }
                else
                {
                    OCR.CPNO = VB.Val(dt.Rows[0]["CPNO"].ToString().Trim());
                    OCR.CP_CODE = dt.Rows[0]["CPCODE"].ToString().Trim();

                    if (OCR.CP_CODE != "")
                    {
                        OCR.CP_Name = OE.READ_CP_NAME(OCR.CP_CODE, "ER");
                    }

                    OCR.OPD_BDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    OCR.BDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    OCR.CP_ROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    cboCpCode.Text = OCR.CP_Name;

                    if (dt.Rows[0]["STARTDATE"].ToString().Trim() != "")
                    {
                        strTemp = "activation +";
                    }

                    if (dt.Rows[0]["DROPDATE"].ToString().Trim() != "")
                    {
                        strTemp = "deactivation +";
                    }

                    if (dt.Rows[0]["WARMDATE"].ToString().Trim() != "")
                    {
                        strTemp += "예비 CP +";
                    }

                    if (dt.Rows[0]["ACTDATE"].ToString().Trim() != "")
                    {
                        strTemp += "시술 +";
                    }

                    if (dt.Rows[0]["Call_Warm_Date"].ToString().Trim() != "")
                    {
                        txtCallwarm.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_Warm_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_Warm_TIME"].ToString().Trim(), "M");
                    }

                    if (dt.Rows[0]["Call_Acti_Date"].ToString().Trim() != "")
                    {
                        txtCallacti.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_Acti_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_Acti_TIME"].ToString().Trim(), "M");
                    }

                    if (dt.Rows[0]["Call_DeActi_Date"].ToString().Trim() != "")
                    {
                        txtCalldeacti.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_DeActi_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_DeActi_TIME"].ToString().Trim(), "M");
                    }

                    if (dt.Rows[0]["Call_Act_Date"].ToString().Trim() != "")
                    {
                        txtCallact.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_Act_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_Act_Time"].ToString().Trim(), "M");
                    }

                    if (strTemp != "")
                    {
                        if (VB.Right(strTemp, 1) == "+")
                        {
                            strTemp = VB.Mid(strTemp, 1, strTemp.Length - 1);
                        }

                        lblPatInfo.Text += strTemp + ")";
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            READ_SMS_LIST(ssSMS, "73", OCR.PtNo, OCR.BDate,  OCR.CPNO);
        }

        private void READ_SMS_LIST(FarPoint.Win.Spread.FpSpread ssSpread, string strGubun, string strPano, string strDATE, double dblCpNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssSpread.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = " SELECT PANO,SNAME,HPHONE,SENDMSG,DeptCode ";
                SQL = SQL + ComNum.VBLF + " ,TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI') JOBDATE     ";
                SQL = SQL + ComNum.VBLF + " ,TO_CHAR(SENDTIME,'YYYY-MM-DD HH24:MI') SENDTIME   ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.ETC_SMS ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ENTDATE >=TO_DATE('" + strDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ENTDATE <= TO_DATE('" + Convert.ToDateTime(strDATE).AddDays(1).ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND Bigo ='" + dblCpNo + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY JOBDATE DESC ";

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
                    ssSpread.ActiveSheet.RowCount = dt.Rows.Count;
                    ssSpread.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssSpread.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
                        ssSpread.ActiveSheet.Cells[i, 1].Text = READ_SMS_DRNAME(dt.Rows[i]["HPHONE"].ToString().Trim());
                        ssSpread.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssSpread.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssSpread.ActiveSheet.Cells[i, 4].Text = clsVbfunc.GetBCodeCODE(clsDB.DbCon, "C#_CP_SMS_GUBUN", dt.Rows[i]["SNAME"].ToString().Trim());
                        ssSpread.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["JOBDATE"].ToString().Trim();
                        ssSpread.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                        ssSpread.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["SENDMSG"].ToString().Trim();
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

        private string READ_SMS_DRNAME(string argHp)
        {
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT d.HTEL, c.DRNAME SName ";
                SQL = SQL + ComNum.VBLF + " FROM   ADMIN.OCS_DOCTOR c,  ADMIN.INSA_MST d ";
                SQL = SQL + ComNum.VBLF + " WHERE c.SABUN = d.SABUN ";
                SQL = SQL + ComNum.VBLF + "  AND d.TOIDAY IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND c.Gbout  ='N' ";
                SQL = SQL + ComNum.VBLF + "  AND c.Grade  ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(c.drcode,3,2) <>'99'";
                SQL = SQL + ComNum.VBLF + "  AND c.drcode <>'0104' ";
                SQL = SQL + ComNum.VBLF + "  AND d.HTel ='" + argHp + "' ";

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
                rtnVal = dt.Rows[0]["SName"].ToString().Trim();
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


    }
}
