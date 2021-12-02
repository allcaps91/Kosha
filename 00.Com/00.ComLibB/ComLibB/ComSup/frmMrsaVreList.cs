using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupInfc
    /// File Name       : frmMrsaVreList
    /// Description     : 다제내성균 관리대상자  명단(MRSA.VRE)
    /// Author          : 전상원
    /// Create Date     : 2017-03-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " PSMH\nurse\nrinfo\nrinfo.vbp(FrmMrsaVre명단.frm) >> frmMrsaVreList.cs 폼이름 재정의" />
    public partial class frmMrsaVreList : Form
    {
        public frmMrsaVreList()
        {
            InitializeComponent();
        }

        private void frmMrsaVreList_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComFunc CF = new ComFunc();

            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, strSysDate, -42);
            dtpTDate.Text = strSysDate;

            if (clsPublic.GstrHelpCode != "") 
            {
                this.Text = VB.Pstr(clsPublic.GstrHelpCode, "{}", 3);
            }
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            int j = 0;
            int k = 0;
            int nRead = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string strSName = "";
            string strSpecCode = "";
            string strSpecName = "";
            string strRDate = "";
            string strChk = "";
            string strIO = "";
            string strRoom = "";

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            if (rdoGB_0.Checked == true)
            {
                #region Select_MRSA
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = " SELECT A.PANO,A.SNAME,A.SPECCODE, B.NAME,                           ";
                    SQL = SQL + ComNum.VBLF + "MAX(TO_CHAR(A.RDATE,'YYYY-MM-DD')) RDATE                       ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION A,                             ";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_MED + "EXAM_SPECODE B                                ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = B.CODE                                     ";
                    SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='14'                                           ";
                    if (rdoBun_2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD = 'O' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD = 'I' ";
                    }
                    SQL = SQL + ComNum.VBLF + "   AND A.MRSA  = '*'                                       ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO,A.SNAME,A.SPECCODE,B.NAME                 ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    nRead = dt.Rows.Count;
                    ssView_Sheet1.RowCount = 0;

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            strSName = dt.Rows[i]["SNAME"].ToString().Trim();
                            strSpecCode = dt.Rows[i]["SPECCODE"].ToString().Trim();
                            strSpecName = dt.Rows[i]["NAME"].ToString().Trim();
                            strRDate = dt.Rows[i]["RDATE"].ToString().Trim();
                            strIO = "";

                            if (rdoBun_2.Checked == true)
                            {
                                strIO = "외래";
                                strRoom = "OPD";
                            }
                            else
                            {
                                SQL = "";
                                SQL = " SELECT ROOMCODE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                                SQL = SQL + ComNum.VBLF + " WHERE OUTDATE IS NULL ";
                                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    strIO = "입원";
                                    strRoom = dt1.Rows[0]["ROOMCODE"].ToString().Trim();
                                }
                                else
                                {
                                    strIO = "퇴원";

                                    SQL = "";
                                    SQL = " SELECT ROOM FROM " + ComNum.DB_MED + "EXAM_INFECTION ";
                                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND RDATE = TO_DATE('" + strRDate + "','YYYY-MM-DD') ";

                                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    strRoom = dt2.Rows[0]["ROOM"].ToString().Trim();

                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }

                            SQL = "";
                            SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,MRSA                                 ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                            ";
                            SQL = SQL + ComNum.VBLF + " WHERE RDATE > TO_DATE('" + strRDate + "','YYYY-MM-DD')    ";
                            SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                            ";
                            SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                            SQL = SQL + ComNum.VBLF + "   AND (MRSA <> '*' OR MRSA IS NULL) ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),MRSA                 ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                strChk = "OK";
                            }
                            else if (dt1.Rows.Count <= 2)
                            {
                                strChk = "OK";
                            }
                            else if (dt1.Rows.Count >= 3)
                            {
                                strChk = "NO";
                            }

                            dt1.Dispose();
                            dt1 = null;

                            k = 0;
                            if (rdoBun_0.Checked == true)
                            {
                                if (strIO == "입원")
                                {
                                    if (strChk == "OK")
                                    {
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPano;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strRoom;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strSpecName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Right(strRDate, 5);

                                        SQL = "";
                                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,MRSA                   ";
                                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                            ";
                                        SQL = SQL + ComNum.VBLF + " WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                                        SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')   ";
                                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                            ";
                                        SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),MRSA                 ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dt2.Rows.Count >= 11)
                                        {
                                            for (j = dt2.Rows.Count - 10; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["MRSA"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                                k = k + 1;
                                            }
                                        }
                                        else
                                        {
                                            for (j = 0; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["MRSA"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                            }
                            else if (rdoBun_1.Checked == true)
                            {
                                if (strIO == "퇴원")
                                {
                                    if (strChk == "OK")
                                    {
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPano;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strRoom;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strSpecName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Right(strRDate, 5);

                                        SQL = "";
                                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,MRSA";
                                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION";
                                        SQL = SQL + ComNum.VBLF + " WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                                        SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')   ";
                                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                            ";
                                        SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),MRSA                 ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dt2.Rows.Count >= 11)
                                        {
                                            for (j = dt2.Rows.Count - 10; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["MRSA"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                                k = k + 1;
                                            }
                                        }
                                        else
                                        {
                                            for (j = 0; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["MRSA"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                            }
                            else
                            {
                                if (strIO == "외래")
                                {
                                    if (strChk == "OK")
                                    {
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPano;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strRoom;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strSpecName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Right(strRDate, 5);

                                        SQL = "";
                                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,MRSA                   ";
                                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                            ";
                                        SQL = SQL + ComNum.VBLF + " WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')   ";
                                        SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')   ";
                                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                            ";
                                        SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                    ";
                                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),MRSA                 ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dt2.Rows.Count >= 11)
                                        {
                                            for (j = dt2.Rows.Count - 10; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["MRSA"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                                k = k + 1;
                                            }
                                        }
                                        else
                                        {
                                            for (j = 0; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["MRSA"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                    }

                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                #endregion
            }
            else
            {
                #region Select_VRE
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = " SELECT A.PANO,A.SNAME,A.SPECCODE, B.NAME,                           ";
                    SQL = SQL + ComNum.VBLF + "MAX(TO_CHAR(A.RDATE,'YYYY-MM-DD')) RDATE                       ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION A,                             ";
                    SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_MED + "EXAM_SPECODE B                                ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')     ";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECCODE = B.CODE                                     ";
                    SQL = SQL + ComNum.VBLF + "   AND B.GUBUN ='14'                                           ";
                    if (rdoBun_2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD = 'O' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD = 'I' ";
                    }
                    SQL = SQL + ComNum.VBLF + "   AND A.VRE  = '*'                                        ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO,A.SNAME,A.SPECCODE,B.NAME                     ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        nRead = dt.Rows.Count;
                        ssView_Sheet1.RowCount = 0;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            strSName = dt.Rows[i]["SNAME"].ToString().Trim();
                            strSpecCode = dt.Rows[i]["SPECCODE"].ToString().Trim();
                            strSpecName = dt.Rows[i]["NAME"].ToString().Trim();
                            strRDate = dt.Rows[i]["RDATE"].ToString().Trim();
                            strIO = "";

                            if (rdoBun_2.Checked == true)
                            {
                                strIO = "외래";
                                strRoom = "OPD";
                            }
                            else
                            {
                                SQL = "";
                                SQL = " SELECT ROOMCODE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                                SQL = SQL + ComNum.VBLF + " WHERE OUTDATE IS NULL ";
                                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    strIO = "입원";
                                    strRoom = dt1.Rows[0]["ROOMCODE"].ToString().Trim();
                                }
                                else
                                {
                                    strIO = "퇴원";
                                    SQL = "";
                                    SQL = " SELECT ROOM FROM " + ComNum.DB_MED + "EXAM_INFECTION ";
                                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "' ";
                                    SQL = SQL + ComNum.VBLF + "   AND RDATE = TO_DATE('" + strRDate + "','YYYY-MM-DD') ";

                                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    strRoom = dt2.Rows[0]["ROOM"].ToString().Trim();

                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }

                            SQL = "";
                            SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE                   ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                           ";
                            SQL = SQL + ComNum.VBLF + " WHERE RDATE > TO_DATE('" + strRDate + "','YYYY-MM-DD')   ";
                            SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')  ";
                            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                           ";
                            SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                   ";
                            SQL = SQL + ComNum.VBLF + "   AND (MRSA <> '*' OR MRSA IS NULL) ";
                            SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                strChk = "OK";
                            }
                            else if (dt1.Rows.Count <= 2)
                            {
                                strChk = "OK";
                            }
                            else if (dt1.Rows.Count >= 3)
                            {
                                strChk = "NO";
                            }

                            dt1.Dispose();
                            dt1 = null;

                            k = 0;

                            if (rdoBun_0.Checked == true)
                            {
                                if (strIO == "입원")
                                {
                                    if (strChk == "OK")
                                    {
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPano;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strRoom;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strSpecName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Right(strRDate, 5);

                                        SQL = "";
                                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE                   ";
                                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                           ";
                                        SQL = SQL + ComNum.VBLF + " WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')  ";
                                        SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')  ";
                                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                           ";
                                        SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                   ";
                                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dt2.Rows.Count >= 11)
                                        {
                                            for (j = dt2.Rows.Count - 10; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["VRE"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                                k = k + 1;
                                            }
                                        }
                                        else
                                        {
                                            for (j = 0; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["VRE"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                            }
                            else if (rdoBun_1.Checked == true)
                            {
                                if (strIO == "퇴원")
                                {
                                    if (strChk == "OK")
                                    {
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPano;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strRoom;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strSpecName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Right(strRDate, 5);

                                        SQL = "";
                                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE";
                                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION";
                                        SQL = SQL + ComNum.VBLF + " WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')  ";
                                        SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')  ";
                                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                           ";
                                        SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                   ";
                                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dt2.Rows.Count >= 11)
                                        {
                                            for (j = dt2.Rows.Count - 10; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["VRE"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                                k = k + 1;
                                            }
                                        }
                                        else
                                        {
                                            for (j = 0; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["VRE"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                            }
                            else
                            {
                                if (strIO == "외래")
                                {
                                    if (strChk == "OK")
                                    {
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strPano;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strSName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strRoom;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strSpecName;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = VB.Right(strRDate, 5);

                                        SQL = "";
                                        SQL = " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE,VRE                   ";
                                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTION                           ";
                                        SQL = SQL + ComNum.VBLF + " WHERE RDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')  ";
                                        SQL = SQL + ComNum.VBLF + "   AND RDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')  ";
                                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "'                           ";
                                        SQL = SQL + ComNum.VBLF + "   AND SPECCODE = '" + strSpecCode + "'                   ";
                                        SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD'),VRE                 ";

                                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                            return;
                                        }

                                        if (dt2.Rows.Count >= 11)
                                        {
                                            for (j = dt2.Rows.Count - 10; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["VRE"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                                k = k + 1;
                                            }
                                        }
                                        else
                                        {
                                            for (j = 0; j < dt2.Rows.Count; j++)
                                            {
                                                if (dt2.Rows[j]["VRE"].ToString().Trim() == "*")
                                                {
                                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].BackColor = Color.FromArgb(255, 201, 255);
                                                }
                                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, j + 5].Text = VB.Right(dt2.Rows[j]["RDATE"].ToString().Trim(), 5);
                                            }
                                        }

                                        dt2.Dispose();
                                        dt2 = null;
                                    }
                                }
                            }
                        }
                    }
                    
                    dt.Dispose();
                    dt = null;

                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                    }

                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                #endregion
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            if (rdoBun_0.Checked == true)
            {
                strTitle = "다제내성균 환자명단(재원)";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("작업일자 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDate + " " + strSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else if (rdoBun_1.Checked == true)
            {
                strTitle = "다제내성균 환자명단(퇴원)";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("작업일자 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDate + " " + strSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else
            {
                strTitle = "다제내성균 환자명단(외래)";

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += CS.setSpdPrint_String("작업일자 : " + dtpFDate.Text + " ~ " + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDate + " " + strSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
