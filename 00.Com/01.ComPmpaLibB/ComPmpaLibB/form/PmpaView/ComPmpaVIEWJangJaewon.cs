using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : ComPmpaVIEWJangJaewon.cs
    /// Description     : 진료과장별 장기입원자 현황
    /// Author          : 김효성
    /// Create Date     : 2017-09-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\psmh\IPD\iviewa\IVIEWAH.FRM  >> ComPmpaVIEWJangJaewon.cs 폼이름 재정의" />	
    public partial class ComPmpaVIEWJangJaewon : Form
    {
        string GstrJobName = "";

        public ComPmpaVIEWJangJaewon(string strJobName)
        {
            GstrJobName = strJobName;

            InitializeComponent();
        }

        public ComPmpaVIEWJangJaewon()
        {
            InitializeComponent();
        }

        private void ComPmpaVIEWJangJaewon_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


            ssView_Sheet1.Columns[15].Visible = false;
            ssView_Sheet1.Columns[16].Visible = false;
            ssView_Sheet1.Columns[17].Visible = false;
            ssView_Sheet1.Columns[18].Visible = false;

            txtIlsu.Text = "30";
            lblDays.Text = "0 명";
            txtAmt.Text = "30000";

            SET_ComboDept();

        }

        private void SET_ComboDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PrintRanking,DeptCode ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode IN (";
                SQL = SQL + ComNum.VBLF + "  SELECT DrDept1 FROM BAS_DOCTOR WHERE DrDept1 NOT IN ('ER','R6','TO','HR','PT','OM','LM')  ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY DrDept1) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                cboDept.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strDoct = "";

            CboDoct.Items.Clear();
            CboDoct.Items.Add("****.전체");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DrName,DrCode FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DrDept1='" + cboDept.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUBSTR(DrCode,3,2) <> '99'";
                SQL = SQL + ComNum.VBLF + "  AND TOUR ='N'";
                SQL = SQL + ComNum.VBLF + "ORDER BY DrName ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDoct = dt.Rows[i]["DrCode"].ToString().Trim() + ".";
                    strDoct = strDoct + dt.Rows[i]["DrName"].ToString().Trim();
                    CboDoct.Items.Add(strDoct);
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                CboDoct.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Clear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            lblDays.Text = "0 명";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            int i = 0;
            int nRow = 0;
            int nIlsu = 0;
            string strOldDept = "";
            string strNewDept = "";
            string strOK = "";
            double nBonRate = 0;
            string strBonRate = "";
            int nTotAmt = 0;
            int nSetAmt = 0;
            string strPano = "";
            int nIpdNo = 0;
            int nTRSNo = 0;
            DataTable dt = null;
            DataTable dtfunc = null;
            DataTable dtfunc1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            ssView_Sheet1.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);
            ssView_Sheet1.SetColumnMerge(2, FarPoint.Win.Spread.Model.MergePolicy.Always);

            Clear();

            nIlsu = (int)VB.Val(txtIlsu.Text);
            nSetAmt = (int)VB.Val(txtAmt.Text);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.DEPTCODE, a.DRCODE, b.DRNAME, a.ROOMCODE,a.LastTrs,                   ";
                SQL = SQL + ComNum.VBLF + "        a.PANO, a.SNAME, a.ILSU, TO_CHAR(a.INDATE,'YYYY-MM-DD') INDATE,         ";
                SQL = SQL + ComNum.VBLF + "        a.AGE, a.SEX, a.BI,a.IPDNO                                             ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "BAS_DOCTOR b, " + ComNum.DB_PMPA + "bas_clinicdept c         ";
                SQL = SQL + ComNum.VBLF + "  WHERE  1=1                                                       ";
                SQL = SQL + ComNum.VBLF + "    AND  a.GBSTS IN ('0','2')                                                       ";
                SQL = SQL + ComNum.VBLF + "    AND a.OUTDATE IS NULL                                                          ";
                SQL = SQL + ComNum.VBLF + "    AND a.PANO <> '81000004'                                                       ";


                if (cboDept.Text != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.DeptCode ='" + cboDept.Text + "'                             ";
                }
                if (VB.Left(CboDoct.Text, 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.DrCode ='" + VB.Left(CboDoct.Text, 4) + "'                           ";
                }

                if (chk1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.bi NOT IN ( '31','52')                                              ";
                }

                if (nIlsu != 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.ILSU >= '" + nIlsu + "'                               ";
                }

                SQL = SQL + ComNum.VBLF + "    AND a.DRCODE   = b.DRCODE                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND a.DEPTCODE = c.DEPTCODE                                                    ";

                if (rdo0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   ORDER BY a.DrCode "; 
                }

                else if (rdo1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   ORDER BY a.RoomCode ";
                }
                else if (rdo2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   ORDER BY a.InDate ";
                }
                else if (rdo3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   ORDER BY a.SName";
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

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (ssView_Sheet1.RowCount > 0)
                {
                    ssView_Sheet1.Cells[0, 1].Text = dt.Rows[0]["deptcode"].ToString().Trim();
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOldDept = dt.Rows[0]["DeptCode"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    nIpdNo = (int)VB.Val(dt.Rows[i]["IpdNo"].ToString().Trim());
                    nTRSNo = (int)VB.Val(dt.Rows[i]["LastTrs"].ToString().Trim());

                    strOK = "";
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT BonRate FROM IPD_TRANS WHERE Trsno =" + nTRSNo + " ";
                    SqlErr = clsDB.GetDataTable(ref dtfunc, SQL, clsDB.DbCon);
                    strBonRate = "";
                    nBonRate = 0;
                    nTotAmt = 0;

                    if (dtfunc.Rows.Count > 0)
                    {
                        strOK = "OK";
                        strBonRate = dtfunc.Rows[0]["BonRate"].ToString().Trim();
                        nBonRate = (Convert.ToDouble(dtfunc.Rows[0]["BonRate"].ToString().Trim()) / 100);

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "  SELECT SUM(Amt1) TotAmt ";
                        SQL = SQL + ComNum.VBLF + "  FROM  " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
                        SQL = SQL + ComNum.VBLF + "    AND ActDate >=TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + "    AND NU <= 20 ";
                        SQL = SQL + ComNum.VBLF + "    AND NU NOT IN ('01','02','03') ";
                        SQL = SQL + ComNum.VBLF + "    AND BUN NOT IN ('74') ";
                        SQL = SQL + ComNum.VBLF + "    AND TRSNO =" + nTRSNo + " ";

                        SqlErr = clsDB.GetDataTable(ref dtfunc1, SQL, clsDB.DbCon);

                        nTotAmt = (int)VB.Val(dtfunc1.Rows[0]["TotAmt"].ToString().Trim());

                        if (nBonRate > 0)
                        {
                            nTotAmt = (int)(Convert.ToDouble(nTotAmt) * (nBonRate));
                        }

                        if (nSetAmt <= nTotAmt)
                        {
                            strOK = "";
                        }

                        if (nSetAmt == 0)
                        {
                            strOK = "OK";
                        }

                        if (nTotAmt == 0)
                        {
                            strOK = "";
                        }

                        dtfunc1.Dispose();
                        dtfunc1 = null;
                    }
                    else
                    {
                        strOK = "";
                    }
                    dtfunc.Dispose();
                    dtfunc = null;

                    if (strOK == "OK")
                    {
                        nRow = nRow + 1;
                        strNewDept = dt.Rows[i]["Deptcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 0].Value = true;
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["drname"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Roomcode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["ilsu"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["indate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 18].Text = "";

                        switch (dt.Rows[i]["bi"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "보험";
                                break;

                            case "21":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "보호";
                                break;
                            case "22":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "보호";
                                break;
                            case "23":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "보호";
                                break;
                            case "31":
                            case "32":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "산재";
                                break;
                            case "52":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "자보";
                                break;
                            case "51":
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "일반";
                                break;
                            default:
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = "기타";
                                break;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = Convert.ToDouble(nTotAmt).ToString("###,###,###,##0");
                        ssView_Sheet1.Cells[nRow - 1, 15].Text = nIpdNo.ToString();
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = nTRSNo.ToString();
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = "";

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT ROWID,GBYN,REMARK FROM " + ComNum.DB_PMPA + "WORK_IPD_JANGI";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO= '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND IPDNO=" + nIpdNo + " AND TRSNO =" + nTRSNo + " ";

                        SqlErr = clsDB.GetDataTable(ref dtfunc, SQL, clsDB.DbCon);

                        if (dtfunc.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 17].Text = dtfunc.Rows[0]["ROWID"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = dtfunc.Rows[0]["REMARK"].ToString().Trim();

                            if (dtfunc.Rows[0]["GBYN"].ToString().Trim() == "Y")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 11].Value = true;
                            }
                            else if (dtfunc.Rows[0]["GBYN"].ToString().Trim() == "N")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 12].Value = true;
                            }
                        }
                        dtfunc.Dispose();
                        dtfunc = null;
                    }
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                lblDays.Text = nRow + " 명";
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssView_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            if (e.Column == 11)
            {
                ssView_Sheet1.Cells[e.Row, 12].Text = "";
                ssView_Sheet1.Cells[e.Row, 18].Text = "Y";
            }
            else if (e.Column == 12)
            {
                ssView_Sheet1.Cells[e.Row, 11].Text = "";
                ssView_Sheet1.Cells[e.Row, 18].Text = "Y";
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            string strRemark = "";
            string strROWID = "";
            string strBDate = "";
            string strTDate = "";
            string strPano = "";
            int nIpdNo = 0;
            int nTRSNo = 0;
            string strGbYN = "";
            string strCHK = "";
            string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            strBDate = strDTP;
            strGbYN = "";

            strPano = ssView_Sheet1.Cells[e.Row, 4].Text;
            strTDate = ssView_Sheet1.Cells[e.Row, 13].Text;
            strRemark = ssView_Sheet1.Cells[e.Row, 14].Text;
            nIpdNo = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 15].Text);
            nTRSNo = (int)VB.Val(ssView_Sheet1.Cells[e.Row, 16].Text);
            strROWID = ssView_Sheet1.Cells[e.Row, 17].Text;
            strCHK = ssView_Sheet1.Cells[e.Row, 18].Text;

            if (e.Column == 11)
            {
                if (e.Column == 11 && Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, 11].Value) == true)
                {
                    strGbYN = "Y";
                    ComFunc.MsgBox("퇴원안되는 사유를 확인하세요", "확인");
                    return;
                }
            }

            if (e.Column == 12)
            {
                if (e.Column == 12 && Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, 12].Value) == true)
                {
                    strGbYN = "N";
                    ComFunc.MsgBox("퇴원안되는 사유를 반드시 입력하세요", "확인");
                    return;
                }
            }


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (strROWID == "")
                {
                    SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.WORK_IPD_JANGI ( BDATE,PANO,IPDNO,TRSNO,GBYN,REMARK,OUTDATE) ";
                    SQL = SQL + ComNum.VBLF + " VALUES (  TO_DATE('" + strBDate + "','YYYY-MM-DD') ,'" + strPano + "',";
                    SQL = SQL + ComNum.VBLF + "  " + nIpdNo + "," + nTRSNo + ",'" + strGbYN + "','" + strRemark + "',TO_DATE('" + strTDate + "','YYYY-MM-DD') ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }
                else if (strROWID != "" && strCHK == "Y")
                {
                    SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.WORK_IPD_JANGI SET ";
                    SQL = SQL + ComNum.VBLF + "  GBYN ='" + strGbYN + "',";
                    SQL = SQL + ComNum.VBLF + "  OUTDATE =TO_DATE('" + strTDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "  REMARK ='" + strRemark + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " UPDATE KOSMOS_PMPA.WORK_IPD_JANGI SET ";
                    SQL = SQL + ComNum.VBLF + "  GBYN ='" + strGbYN + "',";
                    SQL = SQL + ComNum.VBLF + "  OUTDATE =TO_DATE('" + strTDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "  REMARK ='" + strRemark + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB . setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            ssView_Sheet1.RowCount = 50;
            lblDays.Text = "0 명";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}
            Cursor.Current = Cursors.WaitCursor;

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFoot1 = "";
            string PrintDate = "";
            string JobMan = "";
            int i = 0;
            int nICNT = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) != true)
                {
                    ssView_Sheet1.Rows[i].Visible = false;
                }
            }

            nICNT = 0;

            if (chk0.Checked == true)
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "	SELECT COUNT(*) CNT ";
                    SQL = SQL + ComNum.VBLF + "	FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                    SQL = SQL + ComNum.VBLF + "	WHERE 1=1 ";
                    SQL = SQL + ComNum.VBLF + "	  AND (OUTDATE IS NULL OR OUTDATE ='') ";

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
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return;

                    }

                    //스프레드 출력문
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BSNSCLS"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;

                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
            }

            PrintDate = mdtp.ToString();
            JobMan = GstrJobName;

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strFont1 = "/fn\"굴림\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "진료과장별 장기입원환자 현황" + "/f1/n";   //제목 센터

            if (txtAmt.Text != "")
            {
                strHead2 = "/l/f2" + "재원일수: " + VB.Val(txtIlsu.Text) + " 일 이상   " + cboDept.Text + " [" + (VB.Pstr(CboDoct.Text, ".", 2)) + "]  본인부담금 " + txtAmt.Text + "원 이하";    //'원무과장님(김종은)요청으로 "(3일간)" 문구 제외시킴

                if (chk0.Checked == true)
                {
                    strHead2 = strHead2 + VB.Space(40) + "작업일자 : " + PrintDate + VB.Space(60) + "Page: " + "/p" + "/n" + VB.Space(40) + "※1일 본인부담금 란은, 진찰료,입원료,병실차액,식대는 본인부담금액에서 제외함." + VB.Space(21) + "현재 재원 " + nICNT + "명";
                }
                else
                {
                    strHead2 = strHead2 + VB.Space(40) + "작업일자 : " + PrintDate + VB.Space(60) + "Page: " + "/p" + "/n" + VB.Space(40) + "※1일 본인부담금 란은, 진찰료,입원료,병실차액,식대는 본인부담금액에서 제외함.";

                }
            }
            else
            {
                strHead2 = "/l/f2" + "재원일수: " + VB.Val(txtIlsu.Text) + " 일 이상   " + cboDept.Text + " [" + (VB.Pstr(CboDoct.Text, ".", 2)) + "]";

                if (chk0.Checked == true)
                {
                    strHead2 = strHead2 + VB.Space(50) + "작업일자 : " + PrintDate + VB.Space(60) + "Page: " + "/p" + "/n" + VB.Space(40) + "※1일 본인부담금 란은, 진찰료,입원료,병실차액,식대는 본인부담금액에서 제외함." + VB.Space(21) + "현재 재원 " + nICNT + "명";
                }
                else
                {
                    strHead2 = strHead2 + VB.Space(50) + "작업일자 : " + PrintDate + VB.Space(60) + "Page: " + "/p" + "/n" + "※1일 본인부담금 란은, 진찰료,입원료,병실차액,식대는 본인부담금액에서 제외함.";
                }
            }

            strFoot1 = "/n/l/f1" + "▶환자치료에 최선을 다하시는 과장님께 늘 감사드립니다." + "/n";
            strFoot1 = strFoot1 + "/l/f1" + "▶병실이 부족합니다. 장기재원자 관리에 협조를 바랍니다." + "/n";
            strFoot1 = strFoot1 + "/l/f1" + "▶퇴원예고제를 적극 시행해 주십시오." + "/n";
            strFoot1 = strFoot1 + "/l/f1" + "▶퇴원예정일 또는 퇴원이 안되는 사유를 기재하여 3일이내 원무과로 주시기 바랍니다." + "/n";
            strFoot1 = strFoot1 + "/c/f1" + "- 원무과장 -";

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Footer = strFont2 + strFoot1;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) != true)
                {
                    ssView_Sheet1.Rows[i].Visible = false;
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
