using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmAFBChkList : Form
    {
        string FstrRowid2 = "";

        string mstrEXEName = "";

        public frmAFBChkList()
        {
            InitializeComponent();
        }

        public frmAFBChkList(string strEXEName)
        {
            InitializeComponent();

            mstrEXEName = strEXEName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panMEMO.Visible = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread SP = new clsSpread();

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "항산균 검사 리스트";

            strHeader = SP.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SP.setSpdPrint_String(" 조회일자: " + dtpFDate.Text + "∼" + dtpTDate.Text, new Font("굴림체", 10, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Landscape, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SP.setSpdPrint(ssView1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                if (FstrRowid2 == "")
                {
                    SQL = "INSERT INTO KOSMOS_PMPA.BAS_OCSMEMO_INFECT2 (PTNO, SNAME, MEMO,  ENTDATE ) ";
                    SQL = SQL + "  VALUES ( '" + txtPano2.Text.Trim() + "', '" + txtSname2.Text.Trim() + "', '" + txtMemo2.Text.Trim().Replace("'", "`") + "' ,  SYSDATE)";

                }
                else

                if (txtMemo2.Text.Trim() == "")
                {
                    SQL = " DELETE KOSMOS_PMPA.BAS_OCSMEMO_INFECT2  ";
                    SQL = SQL + " WHERE ROWID = '" + FstrRowid2 + "' ";
                }
                else
                {
                    SQL = " UPDATE KOSMOS_PMPA.BAS_OCSMEMO_INFECT2 SET ";
                    SQL = SQL + "  MEMO = '" + txtMemo2.Text.Trim().Replace("'", "`") + "' ";
                    SQL = SQL + " WHERE ROWID = '" + FstrRowid2 + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("등록 완료", "확인");
                panMEMO.Visible = false;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            int i = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string strPano2 = "";
            string strSName = "";
            string strDept = "";
            string strDrname = "";
            string strBDATE = "";
            string strRDate = "";
            string strResult = "";
            string strSpecNo = "";
            string strBI = "";
            string strIO = "";
            string strJewon = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd");
            strPano = txtPano.Text.Trim();

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT RESULT, A.SPECNO, A.PANO, B.SPECCODE FROM KOSMOS_OCS.EXAM_RESULTC A ,   KOSMOS_OCS.EXAM_SPECMST B  ";
                SQL = SQL + "                   WHERE B.RESULTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + "                     AND B.RESULTDATE < TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + "                      AND B.BDATE <=TRUNC(SYSDATE)";

                if (strPano != "")
                {
                    SQL = SQL + "  AND B.PANO = '" + strPano + "' ";
                }

                SQL = SQL + "                     AND B.STATUS IN ('04', '05')  ";
                SQL = SQL + "                     AND B.TUBE IN ( '084','100')  ";

                if (mstrEXEName.ToUpper() == "IPDMED" || mstrEXEName.ToUpper() == "EXINFECT")
                {
                    SQL = SQL + "        AND A.MASTERCODE IN( 'MI36','MI364','MI03','MI031','MI363')   ";
                }
                else
                {
                    SQL = SQL + "        AND A.MASTERCODE IN( 'MI36','MI364','MI363')   ";
                }

                SQL = SQL + "        AND A.RESULT NOT LIKE 'NO GROWTH FOR%' ";
                SQL = SQL + "        AND A.RESULT NOT LIKE 'CONTAMINATION%' ";


                //'양성
                if (optGB0.Checked == true)
                {
                    SQL = SQL + " AND A.HCODE IN( 'M2398', 'M2362','M2363','M2364','M2365','M1032','M1032','M1033','M1034','M1035' )";// '2014-06-11;
                }

                //'음성
                if (optGB1.Checked == true)
                {
                    SQL = SQL + " AND A.HCODE IN ( 'M1031' ,'M2361') ";
                }

                SQL = SQL + "        AND B.SPECNO = A.SPECNO(+)   ";
                SQL = SQL + " ORDER BY A.RESULTDATE, A.PANO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = 0;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPano2 = dt.Rows[i]["PANO"].ToString().Trim();
                        strSpecNo = dt.Rows[i]["SPECNO"].ToString().Trim();
                        strResult = dt.Rows[i]["RESULT"].ToString().Trim();

                        SQL = "";
                        SQL = " SELECT A.BI,A.SNAME,A.DEPTCODE,A.IPDOPD,";
                        SQL = SQL + " TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                        SQL = SQL + " TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE, B.DRNAME  ";
                        SQL = SQL + "  FROM KOSMOS_OCS.EXAM_SPECMST A , KOSMOS_PMPA.BAS_DOCTOR B ";
                        SQL = SQL + " WHERE A.PANO = '" + strPano2 + "' ";
                        SQL = SQL + "   AND A.SPECNO = '" + strSpecNo + "' ";
                        SQL = SQL + "   AND A.DRCODE = B.DRCODE(+)";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strSName = dt1.Rows[0]["SNAME"].ToString().Trim();
                            strDept = dt1.Rows[0]["DEPTCODE"].ToString().Trim();
                            strBI = dt1.Rows[0]["BI"].ToString().Trim();
                            strIO = dt1.Rows[0]["IPDOPD"].ToString().Trim();
                            strBDATE = dt1.Rows[0]["BDATE"].ToString().Trim();
                            strRDate = dt1.Rows[0]["RESULTDATE"].ToString().Trim();
                            strDrname = dt1.Rows[0]["DRNAME"].ToString().Trim();
                        }
                        else
                        {
                            strSName = "";
                            strDept = "";
                            strBI = "";
                            strIO = "";
                            strBDATE = "";
                            strRDate = "";
                            strDrname = "";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strIO == "I")
                        {
                            SQL = " SELECT GBSTS FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                            SQL = SQL + " WHERE PANO = '" + strPano2 + "' ";
                            SQL = SQL + "   AND INDATE <= TO_DATE('" + Convert.ToDateTime(strBDATE).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + "   AND (OUTDATE >= TO_DATE('" + strBDATE + "','YYYY-MM-DD')    ";
                            SQL = SQL + "        OR OUTDATE IS NULL) ";
                            SQL = SQL + "   AND GBSTS <> '9' ";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["GBSTS"].ToString().Trim() == "7")
                                {
                                    strJewon = "퇴원";
                                }
                                else
                                {
                                    strJewon = "입원";
                                }
                            }
                            else
                            {
                                strJewon = "이상";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                        else
                        {
                            strJewon = "";
                        }

                        //    '외래,입원의 slip에 B4064 발생되면 표시해줌
                        //    '2009-08-07 입원외래 합쳐서 표시해줌

                        ssView1_Sheet1.RowCount = ssView1_Sheet1.RowCount + 1;

                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 0].Text = strPano2;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 1].Text = strSName;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 2].Text = strDept;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 3].Text = strDrname;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 4].Text = strBI;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 5].Text = strIO;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 6].Text = strBDATE;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 7].Text = strRDate;


                        SQL = " SELECT NAME FROM KOSMOS_OCS.EXAM_SPECODE WHERE CODE ='" + dt.Rows[i]["SPECCODE"].ToString().Trim() + "' AND GUBUN ='14' ";
                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 8].Text = dt1.Rows[0]["NAME"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 9].Text = strResult;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 10].Text = strSpecNo;
                        ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 11].Text = strJewon;

                        //    '결핵 신청서 등록여부
                        SQL = " SELECT ACTDATE,  SINDATE";
                        SQL = SQL + " FROM   KOSMOS_PMPA.NUR_STD_INFECT3 ";
                        SQL = SQL + " WHERE PANO = '" + strPano2 + "' ";
                        SQL = SQL + " ORDER BY ACTDATE DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {

                            if (dt1.Rows[0]["SINDATE"].ToString().Trim() == "")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].Text = "신고일자無";
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].Text = ComFunc.FormatStrToDate(dt1.Rows[0]["SINDATE"].ToString().Trim(),"D");

                                if (Convert.ToDateTime(ComFunc.FormatStrToDate(dt1.Rows[0]["SINDATE"].ToString().Trim(), "D")) < Convert.ToDateTime("2010-01-01"))
                                {
                                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].ForeColor = Color.FromArgb(255, 100, 100);
                                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 12].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;


                        //      SS1.Col = 14:

                        SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_OCSMEMO_INFECT2 ";
                        SQL = SQL + " WHERE PTNO = '" + strPano2 + "'";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 13].Text = "★";
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 13].Text = "";
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (mstrEXEName.ToUpper() == "IPDMED")
                        {
                        }
                        else
                        {
                            if (chkSunap.Checked == true)
                            {
                                SQL = "";
                                SQL = " SELECT 'O' GBN, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, ";
                                SQL = SQL + "                  TO_CHAR(BDATE,'YYYY-MM-DD') BDATE";
                                SQL = SQL + "  FROM KOSMOS_PMPA.OPD_SLIP ";
                                SQL = SQL + " WHERE BDATE >= TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                                SQL = SQL + "   AND ACTDATE <= TRUNC(SYSDATE)";
                                SQL = SQL + "   AND PANO ='" + strPano2 + "' ";
                                SQL = SQL + "   AND SUNEXT IN ( 'B4064' ,'B4065',  'B4054A') ";
                                SQL = SQL + "   GROUP BY  ACTDATE, BDATE ";
                                SQL = SQL + " UNION ALL ";
                                SQL = SQL + "SELECT 'I' GBN ,TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE ,";
                                SQL = SQL + "                       TO_CHAR(BDATE,'YYYY-MM-DD') BDATE";
                                SQL = SQL + "  FROM KOSMOS_PMPA.IPD_NEW_SLIP ";
                                SQL = SQL + " WHERE BDATE >= TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                                SQL = SQL + "   AND ACTDATE <= TRUNC(SYSDATE)";
                                SQL = SQL + "   AND PANO ='" + strPano2 + "' ";
                                SQL = SQL + "   AND SUNEXT IN ( 'B4064' ,'B4065',  'B4054A') ";
                                SQL = SQL + "   GROUP BY  ACTDATE, BDATE ";
                                SQL = SQL + "  ORDER BY 2 DESC      ";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount - 1, 14].Text = dt1.Rows[0]["ACTDATE"].ToString().Trim();
                                }

                                dt1.Dispose();
                                dt1 = null;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmAFBChkList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            panMEMO.Visible = false;
            panSMS.Visible = false;
            panSMS.Dock = DockStyle.Fill;

            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpFDate.Value = dtpTDate.Value.AddDays(-8);

            if (mstrEXEName.ToUpper() == "IPDMED")
            {
                if (ssView1_Sheet1.Columns.Get(14).Visible == false)
                {
                    chkSunap.Checked = false;
                }
                else
                {
                    chkSunap.Checked = true;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit1_Click(object sender, EventArgs e)
        {
            panSMS.Visible = false;
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView1_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }
            string strSpecNo = "";
            string strPano = "";
            string strSName = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            strPano = ssView1_Sheet1.Cells[e.Row, 0].Text.Trim();
            strSName = ssView1_Sheet1.Cells[e.Row, 1].Text.Trim();

            try
            {
                if (e.Column == 10)
                {
                    strSpecNo = ssView1_Sheet1.Cells[e.Row, 10].Text.Trim();

                    SQL = "";
                    SQL = "SELECT  JOBDATE , SABUN,   SPECNO,  PANO ,  HPHONE,  SMS , SENDTIME   ";
                    SQL = SQL + "  FROM KOSMOS_OCS.EXAM_SMSSEND ";
                    SQL = SQL + " WHERE SPECNO = '" + strSpecNo + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssView2_Sheet1.RowCount = dt.Rows.Count;
                        ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JOBDATE"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 0].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
                            ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["HPHONE"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 0].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());
                            ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SMS"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    if (mstrEXEName.ToUpper() == "IPDMED")
                    {
                        panSMS.Visible = true;
                    }
                }
                else if (e.Column == 13)
                {

                    txtPano2.Text = strPano;
                    txtSname2.Text = strSName;
                    txtMemo2.Text = "";

                    SQL = "";
                    SQL = " SELECT MEMO, ROWID  FROM KOSMOS_PMPA.BAS_OCSMEMO_INFECT2 ";
                    SQL = SQL + " WHERE PTNO = '" + strPano + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    FstrRowid2 = "";

                    if (dt.Rows.Count > 0)
                    {
                        txtMemo2.Text = dt.Rows[0]["MEMO"].ToString().Trim();
                        FstrRowid2 = dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    panMEMO.Visible = true;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }




























        }
    }
}
