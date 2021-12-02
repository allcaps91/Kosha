using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpSms : Form
    {
        private clsOrderEtc OE = null;
        private clsOrderEtc.OCS_CP_RECORD OCR = new clsOrderEtc.OCS_CP_RECORD();
        private double GdblCPNO = 0;

        public frmOcsCpSms(double dblCPNO = 0)
        {
            InitializeComponent();

            GdblCPNO = dblCPNO;
        }
        
        private void frmOcsCpSms_Load(object sender, EventArgs e)
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
            OE.Clear_OCS_CP_RECORD(ref OCR);

            OCR.PtNo = clsOrdFunction.Pat.PtNo;
            OCR.OPD_ROWID = clsOrdFunction.Pat.Mst_ROWID;
            OCR.CP_DEPT = clsOrdFunction.Pat.DeptCode;
            OCR.CPNO = GdblCPNO;

            setCombo();
            Read_Pat_Info();

            if (OCR.CP_DEPT != "")
            {
                READ_DEPT_Doctor(OCR.CP_DEPT.ToUpper().Trim(), true);
            }
        }

        private void SCREEN_CLEAR()
        {
            txtMsg.Text = "";

            lblCallacti.Text = "";
            lblCalldeacti.Text = "";
            lblCallwarm.Text = "";
            lblCallact.Text = "";

            rdoCPWarm.Checked = false;
            rdoCPActi.Checked = false;
            rdoCPdeActi.Checked = false;
            rdoCPact.Checked = false;

            btnCPcallacti.Enabled = false;
            btnCPcalldeacti.Enabled = false;
            btnCPcallwarm.Enabled = false;
            btnCPcallact.Enabled = false;

            ssView1_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 0;
            ssSMS_Sheet1.RowCount = 0;

            cboCpCode.Text = "";
            cboCpCode.Items.Clear();
        }

        private void setCombo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboDept.Text = "";
            cboDept.Items.Clear();
            cboDept.Items.Add("");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CODE, NAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "         AND GUBUN = 'C#_CP_SMS_DEPT' ";
                SQL = SQL + ComNum.VBLF + "         AND (DELDATE IS NULL OR DELDATE = '') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Sort, Code ";

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
                        cboDept.Items.Add(dt.Rows[i]["CODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;

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

                if (VB.Val(OCR.ER_PATIENT_CPNO) != 0)
                {
                    SQL = SQL + ComNum.VBLF + "   AND CPNO = " + OCR.ER_PATIENT_CPNO + "  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND InTime = TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI')  ";
                }
                
                if (OCR.CPNO > 0)
                {
                    SQL = SQL + ComNum.VBLF + "         AND CPNO = '" + OCR.CPNO + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND CPNO = '1' ";
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
                    cboCpCode.Enabled = false;

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
                        lblCallwarm.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_Warm_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_Warm_TIME"].ToString().Trim(), "M") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["Call_Warm_Sabun"].ToString().Trim()) + ")";
                    }

                    if (dt.Rows[0]["Call_Acti_Date"].ToString().Trim() != "")
                    {
                        lblCallacti.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_Acti_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_Acti_TIME"].ToString().Trim(), "M") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["Call_Acti_Sabun"].ToString().Trim()) + ")";
                    }

                    if (dt.Rows[0]["Call_DeActi_Date"].ToString().Trim() != "")
                    {
                        lblCalldeacti.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_DeActi_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_DeActi_TIME"].ToString().Trim(), "M") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["Call_DeActi_Sabun"].ToString().Trim()) + ")";
                    }

                    if (dt.Rows[0]["Call_Act_Date"].ToString().Trim() != "")
                    {
                        lblCallact.Text = ComFunc.FormatStrToDate(dt.Rows[0]["Call_Act_Date"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[0]["Call_Act_TIME"].ToString().Trim(), "M") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["Call_Act_Sabun"].ToString().Trim()) + ")";
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

        private void READ_DEPT_Doctor(string strDept, bool bolXray)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView1_Sheet1.RowCount = 0;

            if (strDept == "") { return; }

            if (OCR.CP_ROWID == "")
            {
                ComFunc.MsgBox("CP 등록 후 문자 작업을 하세요!!");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     '1' AS GBN, d.HTEL, c.DRNAME AS SName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR c, " + ComNum.DB_ERP + "INSA_MST d ";
                SQL = SQL + ComNum.VBLF + "     WHERE c.SABUN = d.SABUN ";
                SQL = SQL + ComNum.VBLF + "         AND d.TOIDAY IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND c.Gbout = 'N' ";
                SQL = SQL + ComNum.VBLF + "         AND c.Grade = '1' ";
                SQL = SQL + ComNum.VBLF + "         AND SUBSTR(c.drcode,3,2) <> '99'"; //전체과 제외
                SQL = SQL + ComNum.VBLF + "         AND c.drcode <> '0104' "; //김중구소장 제외
                SQL = SQL + ComNum.VBLF + "         AND c.DEPTCODE = '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "         AND d.HTel IS NOT NULL "; //폰있는것만
                SQL = SQL + ComNum.VBLF + "ORDER BY 1, c.DRCODE ";

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
                    ssView1_Sheet1.RowCount = dt.Rows.Count + 1;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["HTEL"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = strDept;
                    }

                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = strDept;
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

        private void txtMsg_TextChanged(object sender, EventArgs e)
        {
            lblCnt.Text = txtMsg.Text.Length + "/80";

            if (txtMsg.Text.Length > 80)
            {
                ComFunc.MsgBox("80자 이상 문자는 전송이 안됩니다.");
            }
        }

        private void rdoCP_CheckedChanged(object sender, EventArgs e)
        {
            string strSTS = "";

            btnCPcallwarm.Enabled = false;
            btnCPcallacti.Enabled = false;
            btnCPcalldeacti.Enabled = false;
            btnCPcallact.Enabled = false;

            if (((RadioButton)sender).Checked == true)
            {
                if (rdoCPWarm.Checked == true)
                {
                    btnCPcallwarm.Enabled = true;

                    txtMsg.Text = OCR.PtNo + " " + OCR.sName + " " + OCR.Sex + "/" + OCR.Age + " " + OCR.CP_Name + " 예비CP 대상자입니다";

                    strSTS = "00";
                }
                else if (rdoCPActi.Checked == true)
                {
                    btnCPcallacti.Enabled = true;

                    txtMsg.Text = OCR.PtNo + " " + OCR.sName + " " + OCR.Sex + "/" + OCR.Age + " " + OCR.CP_Name + " CP대상 환자입니다";

                    strSTS = "01";
                }
                else if (rdoCPdeActi.Checked == true)
                {
                    btnCPcalldeacti.Enabled = true;

                    txtMsg.Text = OCR.PtNo + " " + OCR.sName + " " + OCR.Sex + "/" + OCR.Age + " " + OCR.CP_Name + " CP대상 해제합니다.";

                    strSTS = "02";
                }
                else if (rdoCPact.Checked == true)
                {
                    btnCPcallact.Enabled = true;

                    txtMsg.Text = OCR.PtNo + " " + OCR.sName + " " + OCR.Sex + "/" + OCR.Age + " " + OCR.CP_Name + " CP 대상으로 IAT 시행합니다";

                    strSTS = "03";
                }

                READ_SMS_LIST(ssSMS, "73", OCR.PtNo, OCR.BDate, strSTS, OCR.CPNO);
            }
        }

        private void READ_SMS_LIST(FarPoint.Win.Spread.FpSpread ssSpread, string strGubun, string strPano, string strDATE, string strSTS, double dblCpNo)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssSpread.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                
                SQL = " SELECT PANO,SNAME,HPHONE,SENDMSG,DeptCode " ;
                SQL = SQL + ComNum.VBLF + " ,TO_CHAR(JOBDATE,'YYYY-MM-DD HH24:MI') JOBDATE     " ;
                SQL = SQL + ComNum.VBLF + " ,TO_CHAR(SENDTIME,'YYYY-MM-DD HH24:MI') SENDTIME   " ;
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_SMS " ;
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 " ;
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='" + strGubun + "' " ;
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + strPano + "' " ;
                SQL = SQL + ComNum.VBLF + "   AND ENTDATE >=TO_DATE('" + strDATE + "','YYYY-MM-DD') " ;
                SQL = SQL + ComNum.VBLF + "         AND ENTDATE <= TO_DATE('" + Convert.ToDateTime(strDATE).AddDays(1).ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND SName ='" + strSTS + "' ";
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
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_DOCTOR c,  KOSMOS_ADM.INSA_MST d ";
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

        private void btnCPcallwarm_Click(object sender, EventArgs e)
        {
            if (rdoCPWarm.Checked == false)
            {
                ComFunc.MsgBox("CP구분을 선택하여 작업하세요.");
                return;
            }

            if (lblCallwarm.Text.Trim() != "")
            {
                ComFunc.MsgBox("이미 등록되어 있습니다.");
                return;
            }

            if (OE.CP_ER_Save(ref OCR, "SMS 예비 CP", "") == true)
            {
                lblCallwarm.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
            }
        }

        private void btnCPcallacti_Click(object sender, EventArgs e)
        {
            if (rdoCPActi.Checked == false)
            {
                ComFunc.MsgBox("CP구분을 선택하여 작업하세요.");
                return;
            }

            if (lblCallacti.Text.Trim() != "")
            {
                ComFunc.MsgBox("이미 등록되어 있습니다.");
                return;
            }

            if (OE.CP_ER_Save(ref OCR, "SMS CP activation", "") == true)
            {
                lblCallacti.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
            }
        }

        private void btnCPcalldeacti_Click(object sender, EventArgs e)
        {
            if (rdoCPdeActi.Checked == false)
            {
                ComFunc.MsgBox("CP구분을 선택하여 작업하세요.");
                return;
            }

            if (lblCalldeacti.Text.Trim() != "")
            {
                ComFunc.MsgBox("이미 등록되어 있습니다.");
                return;
            }

            if (OE.CP_ER_Save(ref OCR, "SMS CP deactivation", "") == true)
            {
                lblCalldeacti.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
            }
        }

        private void btnCPcallact_Click(object sender, EventArgs e)
        {
            if (rdoCPact.Checked == false)
            {
                ComFunc.MsgBox("CP구분을 선택하여 작업하세요.");
                return;
            }

            if (lblCallact.Text.Trim() != "")
            {
                ComFunc.MsgBox("이미 등록되어 있습니다.");
                return;
            }

            if (OE.CP_ER_Save(ref OCR, "SMS 시술", "") == true)
            {
                lblCallact.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            READ_DEPT_Doctor(cboDept.Text.ToUpper().Trim(), false);
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            int i = 0;

            for (i = 0; i < ssView1_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView1_Sheet1.Cells[i, 0].Value) == true)
                {
                    ssView2_Sheet1.RowCount = ssView2_Sheet1.RowCount + 1;
                    ssView2_Sheet1.SetRowHeight(ssView2_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 0].Value = true;
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 1].Text = ssView1_Sheet1.Cells[i, 1].Text.Trim();
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 2].Text = ssView1_Sheet1.Cells[i, 2].Text.Trim();
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 3].Text = ssView1_Sheet1.Cells[i, 3].Text.Trim();

                    ssView1_Sheet1.Cells[i, 0].Value = false;
                }
            }
        }

        private void btnSend2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strTxtTel = "";
            string strTxtRetTel = "";
            string strTxtMsg = "";
            string strRTime = "";
            string strDept = "";
            string strYYMM = "";
            string strGubun = "";
            string strSysDateTime = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            int intCNT = 0;

            if (OCR.CP_ROWID == "")
            {
                ComFunc.MsgBox("먼저 CP등록후 작업하세요");
                return;
            }

            if (rdoCPWarm.Checked == false && rdoCPActi.Checked == false 
                && rdoCPdeActi.Checked == false && rdoCPact.Checked == false)
            {
                ComFunc.MsgBox("CP구분을 선택하여 작업하세요.");
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            strGubun = set_CP_Gubun();

            //=>ADD
            if (strGubun == "00")
            {
                if (lblCallwarm.Text.Trim() != "" )
                {
                    ComFunc.MsgBox("이미 등록되어 있습니다.");
                    return;
                }

                if (OE.CP_ER_Save(ref OCR, "SMS 예비 CP", "") == true)
                {
                    lblCallwarm.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
                }
            }
            else if (strGubun == "01")
            {
                if (lblCallacti.Text.Trim() != "")
                {
                    ComFunc.MsgBox("이미 등록되어 있습니다.");
                    return;
                }

                if (OE.CP_ER_Save(ref OCR, "SMS CP activation", "") == true)
                {
                    lblCallwarm.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
                }
            }
            else if (strGubun == "02")
            {
                if (lblCalldeacti.Text.Trim() != "")
                {
                    ComFunc.MsgBox("이미 등록되어 있습니다.");
                    return;
                }

                if (OE.CP_ER_Save(ref OCR, "SMS CP deactivation", "") == true)
                {
                    lblCallwarm.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
                }
            }
            else if (strGubun == "03")
            {
                if (lblCallact.Text.Trim() != "")
                {
                    ComFunc.MsgBox("이미 등록되어 있습니다.");
                    return;
                }

                //시술/수술 등록 ADD2
                OE.CP_ER_Save(ref OCR, "시술", "");

                if (OE.CP_ER_Save(ref OCR, "SMS 시술", "") == true)
                {
                    lblCallwarm.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + " (" + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun) + ")";
                }
            }
            //<= ADD

            for (i = 0; i < ssView2_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView2_Sheet1.Cells[i, 0].Value) == true)
                {
                    intCNT++;
                }
            }

            if (intCNT == 0)
            {
                ComFunc.MsgBox("선택한 건수가 없습니다.. 선택후 SMS 전송하십시오!!");
                return;
            }

            strYYMM = Convert.ToDateTime(strSysDateTime).ToString("yyyyMM");

            if (txtMsg.Text.Trim() == "")
            {
                ComFunc.MsgBox("문자메세지가 공란입니다.");
                return;
            }

            if (txtRetTel.Text.Trim() == "")
            {
                ComFunc.MsgBox("회신번호가 공란입니다.");
                return;
            }

            strTxtMsg = txtMsg.Text.Trim();

            if (strTxtMsg.Length > 80)
            {
                ComFunc.MsgBox("메세지는 80자까지만 가능합니다.");
                return;
            }

            strRTime = strSysDateTime;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView2_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strDept = ssView2_Sheet1.Cells[i, 3].Text.Trim();
                        strTxtTel = ssView2_Sheet1.Cells[i, 1].Text.Trim();

                        if (strTxtTel != "")
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS";
                            SQL = SQL + ComNum.VBLF + "     (Pano, SName, JobDate, Hphone, Gubun, DeptCode, ";
                            SQL = SQL + ComNum.VBLF + "     Rettel, SendMsg, Bigo, EntSabun, EntDate)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.PtNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strRTime + "','YYYY-MM-DD HH24:MI:ss'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTxtTel + "', ";
                            SQL = SQL + ComNum.VBLF + "         '73', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDept + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + txtRetTel.Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTxtMsg + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.CPNO + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS_CP ";
                            SQL = SQL + ComNum.VBLF + "     (Pano, SName, JobDate, Hphone, Gubun, DeptCode, ";
                            SQL = SQL + ComNum.VBLF + "     Rettel, SendMsg, Bigo, EntSabun, EntDate)";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.PtNo + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                            SQL = SQL + ComNum.VBLF + "         TO_DATE('" + strRTime + "','YYYY-MM-DD HH24:MI:ss'), ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTxtTel + "', ";
                            SQL = SQL + ComNum.VBLF + "         '73', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strDept + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + txtRetTel.Text.Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + strTxtMsg + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + OCR.CPNO + "', ";
                            SQL = SQL + ComNum.VBLF + "         " + clsType.User.Sabun + ", ";
                            SQL = SQL + ComNum.VBLF + "         SYSDATE ";
                            SQL = SQL + ComNum.VBLF + "     )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            ssView2_Sheet1.Cells[i, 0].Value = false;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                READ_SMS_LIST(ssSMS, "73", OCR.PtNo, OCR.BDate, strGubun, OCR.CPNO);

                ComFunc.MsgBox("CP SMS 전송이 완료되었습니다.");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strGubun = "";

            strGubun = set_CP_Gubun();

            if (strGubun == "XX")
            {
                ComFunc.MsgBox("구분을 선택 후 작업하세요.");
                return;
            }

            READ_SMS_LIST(ssSMS, "73", OCR.PtNo, OCR.BDate, strGubun, OCR.CPNO);
        }

        private string set_CP_Gubun()
        {
            string rtnVal = "";

            if (rdoCPWarm.Checked == true) { rtnVal = "00"; }
            else if (rdoCPActi.Checked == true) { rtnVal = "01"; }
            else if (rdoCPdeActi.Checked == true) { rtnVal = "02"; }
            else if (rdoCPact.Checked == true) { rtnVal = "03"; }
            else { rtnVal = "XX"; }

            return rtnVal;
        }
    }
}
