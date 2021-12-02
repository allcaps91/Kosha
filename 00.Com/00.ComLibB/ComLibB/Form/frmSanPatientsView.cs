using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSanPatientsView
    /// File Name : frmSanPatientsView.cs
    /// Title or Description : 산재환자 현황
    /// Author : 박창욱
    /// Create Date : 2017-06-02
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>  
    /// <history>  
    /// VB\Frm산재환자현황.frm(Frm산재환자현황) -> frmSanPatientsView.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\Frm산재환자현황.frm(Frm산재환자현황)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busanid\\busanid.vbp
    /// </vbp>
    public partial class frmSanPatientsView : Form
    {
        public frmSanPatientsView()
        {
            InitializeComponent();
        }

        private void frmSanPatientsView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.Columns[10].Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int k = 0;
            int nRead = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string strRoomCode = "";
            string strSname = "";
            string strGubun = "";
            string strOutDate = "";
            string strindate = "";
            string strDate1 = "";   //재해일자
            string striLLs1 = "";   //승인상병명1
            string striLLs2 = "";   //승인상병명2
            string strODate1 = "";  //외래시작일자
            string strODate2 = "";  //외래종료일자
            string strIDate1 = "";  //입원시작일자
            string strIDate2 = "";  //입원종료일자
            string strJumin = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            strYYMM = dtpDate.Value.ToString("yyyyMM");
            strFDate = dtpDate.Value.ToString("yyyy-MM-01");
            strTDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(dtpDate.Value.ToString().Trim(), 4)),
                                         Convert.ToInt32(VB.Mid(dtpDate.Value.ToString().Trim(), 6, 2)));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = " SELECT PANO FROM MIR_SANTONG ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM  = '" + strYYMM + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count <= 0)
                {
                    #region GoSub Data_Select
                    SQL = "";
                    SQL = " CREATE OR REPLACE VIEW VIEW_IPD_VIEW ";
                    SQL = SQL + ComNum.VBLF + " (PANO, SNAME, ROOMCODE, INDATE, OUTDATE, GUBUN) AS ";
                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.ROOMCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, 'T' ";
                    SQL = SQL + ComNum.VBLF + "  FROM IPD_TRANS A, IPD_NEW_MASTER B  ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                    SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NOT NULL";

                    SQL = SQL + ComNum.VBLF + "  UNION ALL ";

                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.ROOMCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, 'J' ";
                    SQL = SQL + ComNum.VBLF + "   FROM IPD_TRANS A, IPD_NEW_MASTER B";
                    SQL = SQL + ComNum.VBLF + " WHERE A.INDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                    SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NULL";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = " SELECT PANO, SNAME, ROOMCODE, INDATE, OUTDATE, GUBUN FROM VIEW_IPD_VIEW ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 0)
                    {
                        dt1.Dispose();
                        dt1 = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    nRead = dt1.Rows.Count;

                    ssView_Sheet1.RowCount = 2;
                    ssView_Sheet1.RowCount = nRead * 2 + 2;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    k = 3;
                    j = 1;

                    for (i = 0; i < nRead; i++)
                    {
                        striLLs1 = "";
                        striLLs2 = "";
                        strODate1 = "";
                        strODate2 = "";
                        strIDate1 = "";
                        strIDate2 = "";
                        strOutDate = "";
                        strindate = "";
                        strRoomCode = "";

                        strPano = dt1.Rows[i]["PANO"].ToString().Trim();
                        strSname = dt1.Rows[i]["SNAME"].ToString().Trim();
                        strRoomCode = dt1.Rows[i]["ROOMCODE"].ToString().Trim();
                        strindate = dt1.Rows[i]["INDATE"].ToString().Trim();
                        strOutDate = dt1.Rows[i]["OUTDATE"].ToString().Trim();
                        strGubun = dt1.Rows[i]["GUBUN"].ToString().Trim();
                        if (strSname == "김봉구") { strGubun = "T"; }

                        if (strGubun == "T")    //퇴원
                        {
                            if (VB.Val(strindate) < VB.Val(strFDate))
                            {
                                strIDate1 = strFDate;
                            }
                            else
                            {
                                strIDate1 = strFDate;
                            }
                            strIDate2 = strOutDate;
                        }

                        else      //입원
                        {
                            if (VB.Val(strindate) < VB.Val(strFDate))
                            {
                                strIDate1 = strFDate;
                            }
                            else
                            {
                                strIDate1 = strindate;
                            }
                            strIDate2 = strTDate;
                        }

                        //재해일자 Select
                        SQL = "";
                        SQL = " SELECT TO_CHAR(DATE1,'YYYY-MM-DD') DATE1, JUMIN1||'-'||JUMIN2 JUMIN , JUMIN1, JUMIN3  FROM BAS_SANID ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            dt2.Dispose();
                            dt2 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        strDate1 = dt2.Rows[0]["DATE1"].ToString().Trim();

                        if (dt2.Rows[0]["JUMIN3"].ToString().Trim() != "")
                        {
                            strJumin = dt2.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt2.Rows[0]["JUMIN3"].ToString().Trim());
                        }
                        else
                        {
                            strJumin = dt2.Rows[0]["JUMIN"].ToString().Trim();
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //승인상병명
                        SQL = "";
                        SQL = " SELECT RANK,ILLNAME FROM MIR_SANILLS";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY RANK ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            switch (j)
                            {
                                case 0:
                                    striLLs1 = dt2.Rows[0]["ILLNAME"].ToString().Trim();
                                    break;
                                case 1:
                                    striLLs1 = dt2.Rows[1]["ILLNAME"].ToString().Trim();
                                    break;
                                case 2:
                                    striLLs1 = dt2.Rows[1]["ILLNAME"].ToString().Trim() + " 외 " + (dt2.Rows.Count - 2) + "건";
                                    break;
                            }
                            if (j == 2) { break; }
                        }

                        dt2.Dispose();
                        dt2 = null;

                        //외래통원기간
                        SQL = "";
                        SQL = " SELECT TO_CHAR(MIN(ACTDATE),'YYYY-MM-DD') MINDATE, TO_CHAR(MAX(ACTDATE),'YYYY-MM-DD') MAXDATE ";
                        SQL = SQL + ComNum.VBLF + " FROM OPD_MASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BI  = '31' ";
                        SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + strPano + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        strODate1 = dt2.Rows[0]["MINDATE"].ToString().Trim();
                        strODate2 = dt2.Rows[0]["MAXDATE"].ToString().Trim();

                        dt2.Dispose();
                        dt2 = null;

                        ssView_Sheet1.Cells[k - 1, 0].Text = j.ToString();
                        ssView_Sheet1.Cells[k - 1, 1].Text = strSname;
                        ssView_Sheet1.Cells[k - 1, 2].Text = Convert.ToDateTime(strDate1).ToString("MM-dd"); ;
                        if (VB.Len(striLLs1) > 29)
                        {
                            ssView_Sheet1.Cells[k - 1, 3].Text = striLLs1;
                            ssView_Sheet1.Cells[k - 1, 3].Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        }
                        else if (VB.Len(striLLs1) >= 25 && VB.Len(striLLs1) <= 29)
                        {
                            ssView_Sheet1.Cells[k - 1, 3].Text = striLLs1;
                            ssView_Sheet1.Cells[k - 1, 3].Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        }
                        else
                        {
                            ssView_Sheet1.Cells[k - 1, 3].Text = striLLs1;
                        }
                        ssView_Sheet1.Cells[k - 1, 4].Text = dtpDate.Value.ToString("MM-dd");
                        ssView_Sheet1.Cells[k - 1, 5].Text = strRoomCode;
                        ssView_Sheet1.Cells[k, 1].Text = strJumin;
                        if (VB.Len(striLLs2) >= 30)
                        {
                            ssView_Sheet1.Cells[k, 3].Text = striLLs2;
                            ssView_Sheet1.Cells[k, 3].Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        }
                        else if (VB.Len(striLLs2) >= 25 && VB.Len(striLLs2) <= 29)
                        {
                            ssView_Sheet1.Cells[k, 3].Text = striLLs2;
                            ssView_Sheet1.Cells[k, 3].Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                        }
                        else
                        {
                            ssView_Sheet1.Cells[k, 3].Text = striLLs2;
                        }

                        if (strODate1 == strODate2)
                        {
                            if (strODate1.Trim() != "")
                            {
                                ssView_Sheet1.Cells[k, 4].Text = Convert.ToDateTime(strODate1).ToString("MM-dd");
                            }
                        }
                        else
                        {
                            if (strODate1.Trim() != "" && strODate2.Trim() != "")
                            {
                                ssView_Sheet1.Cells[k, 4].Text = Convert.ToDateTime(strODate1).ToString("MM-dd") + " ~ " + Convert.ToDateTime(strODate2).ToString("MM-dd");
                            }
                        }
                        ssView_Sheet1.Cells[k, 10].Text = strPano;
                   
                        k = k + 2;
                        j = j + 1;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    SQL = "";
                    SQL = " SELECT PANO, SNAME, TO_CHAR(MIN(ACTDATE),'YYYY-MM-DD') MINDATE, TO_CHAR(MAX(ACTDATE),'YYYY-MM-DD') MAXDATE ";
                    SQL = SQL + ComNum.VBLF + " FROM OPD_MASTER ";
                    SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BI  = '31' ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY PANO, SNAME ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nRead = dt1.Rows.Count;
                    for (i = 0; i < nRead-1; i++)
                    {
                        striLLs1 = "";
                        striLLs2 = "";
                        strODate1 = "";
                        strODate2 = "";
                        strIDate1 = "";
                        strIDate2 = "";
                        strOutDate = "";
                        strindate = "";
                        strRoomCode = "";
                        strPano = dt1.Rows[i]["PANO"].ToString().Trim();
                        strSname = dt1.Rows[i]["SNAME"].ToString().Trim();
                        strODate1 = dt1.Rows[i]["MINDATE"].ToString().Trim();
                        strODate2 = dt1.Rows[i]["MAXDATE"].ToString().Trim();

                        SQL = "";
                        SQL = " SELECT PANO FROM VIEW_IPD_VIEW ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt2.Rows.Count == 0)
                        {
                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 2;
                            k = ssView_Sheet1.RowCount - 1;
                            //재해일자 Select
                            SQL = "";
                            SQL = " SELECT TO_CHAR(DATE1,'YYYY-MM-DD') DATE1, JUMIN1|| JUMIN2 JUMIN, JUMIN1, JUMIN3  FROM BAS_SANID ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }
                            if (dt3.Rows.Count > 0)
                            {
                                strDate1 = dt3.Rows[0]["DATE1"].ToString().Trim();

                                if (dt3.Rows[0]["JUMIN3"].ToString().Trim() != "")
                                {
                                    strJumin = dt3.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt3.Rows[0]["JUMIN3"].ToString().Trim());
                                }
                                else
                                {
                                    strJumin = dt3.Rows[0]["JUMIN"].ToString().Trim();
                                }
                            }
                            dt3.Dispose();
                            dt3 = null;

                            //승인상병명
                            SQL = "";
                            SQL = " SELECT RANK,ILLNAME FROM MIR_SANILLS";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY RANK ";
                            SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            for (j = 0; j < dt3.Rows.Count; j++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        striLLs1 = dt3.Rows[0]["ILLNAME"].ToString().Trim();
                                        break;
                                    case 1:
                                        striLLs2 = dt3.Rows[1]["ILLNAME"].ToString().Trim();
                                        break;
                                    case 2:
                                        striLLs2 = dt3.Rows[1]["ILLNAME"].ToString().Trim() + " 외 " + (dt3.Rows.Count - 2) + "건";
                                        break;
                                }
                                if (j == 2) { break; }
                            }
                            dt3.Dispose();
                            dt3 = null;

                            ssView_Sheet1.Cells[k - 1, 0].Text = j.ToString();
                            ssView_Sheet1.Cells[k - 1, 1].Text = strSname;
                            ssView_Sheet1.Cells[k - 1, 2].Text = Convert.ToDateTime(strDate1).ToString("MM-dd");
                            if (VB.Len(striLLs1) > 29)
                            {
                                ssView_Sheet1.Cells[k - 1, 3].Text = striLLs1;
                                ssView_Sheet1.Cells[k - 1, 3].Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                            }
                            else if (VB.Len(striLLs1) >= 25 && VB.Len(striLLs1) <= 29)
                            {
                                ssView_Sheet1.Cells[k - 1, 3].Text = striLLs1;
                                ssView_Sheet1.Cells[k - 1, 3].Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                            }
                            else
                            {
                                ssView_Sheet1.Cells[k - 1, 3].Text = striLLs1;
                            }

                            ssView_Sheet1.Cells[k, 1].Text = strJumin;
                            if (VB.Len(striLLs2) >= 30)
                            {
                                ssView_Sheet1.Cells[k, 3].Text = striLLs2;
                                ssView_Sheet1.Cells[k, 3].Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                            }
                            else if (VB.Len(striLLs1) >= 25 && VB.Len(striLLs1) <= 29)
                            {
                                ssView_Sheet1.Cells[k, 3].Text = striLLs2;
                                ssView_Sheet1.Cells[k, 3].Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                            }
                            else
                            {
                                ssView_Sheet1.Cells[k, 3].Text = striLLs2;
                            }

                            if (strODate1 == strODate2)
                            {
                                if (strODate1.Trim() != "")
                                {
                                    ssView_Sheet1.Cells[k, 4].Text = Convert.ToDateTime(strODate1).ToString("MM-dd");
                                }
                            }
                            else
                            {
                                if (strODate1.Trim() != "" && strODate2.Trim() != "")
                                {
                                    ssView_Sheet1.Cells[k, 4].Text = Convert.ToDateTime(strODate1).ToString("MM-dd") + " ~ " + Convert.ToDateTime(strODate2).ToString("MM-dd");
                                }
                            }

                            ssView_Sheet1.Cells[k, 10].Text = strPano;

                            j = j + 1;
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                    

                    SQL = "";
                    SQL = "DROP VIEW VIEW_IPD_VIEW";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }



                #endregion

                else
                {
                    #region GoSub Data_Select_SanTong

                    SQL = "";
                    SQL = "SELECT SNAME,JUMIN,TO_CHAR(JEADATE,'YYYY-MM-DD') JEANDATE ,TO_CHAR(OUDATE,'YYYY-MM-DD') OUDATE, ";
                    SQL = SQL + ComNum.VBLF + " ILLS1,ILLS2,INGAGAN,TOGAGAN,ROOM,GanName, PANO, ";
                    SQL = SQL + ComNum.VBLF + " HuAmt, SAAMT, REMARK11, REMARK12, REMARK21, REMARK22, BIGO11, BIGO12, ROWID ";
                    SQL = SQL + ComNum.VBLF + " FROM MIR_SANTONG ";
                    SQL = SQL + ComNum.VBLF + " WHERE YYMM  = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY ROWID ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt2.Rows.Count == 0)
                    {
                        dt2.Dispose();
                        dt2 = null;
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    k = 3;
                    ssView_Sheet1.RowCount = 2;
                    ssView_Sheet1.RowCount = dt2.Rows.Count * 2 + 2;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt2.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[k - 1, 0].Text = (i + 1).ToString();
                        ssView_Sheet1.Cells[k - 1, 1].Text = dt2.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 2].Text = dt2.Rows[i]["JEANDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 3].Text = dt2.Rows[i]["ILLS1"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 4].Text = dt2.Rows[i]["INGAGAN"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 5].Text = dt2.Rows[i]["ROOM"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 6].Text = VB.Val(dt2.Rows[i]["HUAMT"].ToString().Trim()).ToString("###,###,###");
                        ssView_Sheet1.Cells[k - 1, 7].Text = VB.Val(dt2.Rows[i]["REMARK11"].ToString().Trim()).ToString("###,###,###");
                        ssView_Sheet1.Cells[k - 1, 8].Text = dt2.Rows[i]["REMARK21"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 9].Text = dt2.Rows[i]["BIGO11"].ToString().Trim();
                        ssView_Sheet1.Cells[k - 1, 10].Text = dt2.Rows[i]["ROWID"].ToString().Trim();

                        ssView_Sheet1.Cells[k, 1].Text = clsAES.DeAES(dt2.Rows[i]["JUMIN"].ToString().Trim());
                        ssView_Sheet1.Cells[k, 2].Text = dt2.Rows[i]["OUDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 3].Text = dt2.Rows[i]["ILLS2"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 4].Text = dt2.Rows[i]["TOGAGAN"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 5].Text = dt2.Rows[i]["GANNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 6].Text = VB.Val(dt2.Rows[i]["SAAMT"].ToString().Trim()).ToString("###,###,###");
                        ssView_Sheet1.Cells[k, 7].Text = dt2.Rows[i]["REMARK12"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 8].Text = dt2.Rows[i]["REMARK22"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 9].Text = dt2.Rows[i]["BIGO12"].ToString().Trim();
                        ssView_Sheet1.Cells[k, 10].Text = dt2.Rows[i]["PANO"].ToString().Trim();


                        k = k + 2;
                    }

                    dt2.Dispose();
                    dt2 = null;

                    #endregion
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.RowCount = 50;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int k = 0;
            string strYYMM = "";
            string strPano = "";
            string strSname = "";
            string strJumin = "";
            string strJeaDate = "";
            string strOUDate = "";
            string striLLs1 = "";
            string striLLs2 = "";
            string strInGiGan = "";
            string strToGiGan = "";
            string strRoom = "";
            string strGanName = "";
            string strHuAmt = "";
            string strSaAmt = "";
            string strRemark11 = "";
            string strRemark12 = "";
            string strRemark21 = "";
            string strRemark22 = "";
            string strBigo11 = "";
            string strBigo12 = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            //strYYMM = VB.Left(dtpDate.Value.ToString().Trim(), 4) + VB.Mid(dtpDate.Value.ToString().Trim(), 6, 2);
            strYYMM = dtpDate.Value.ToString("yyyyMM");

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 2; i < ssView_Sheet1.RowCount; i = i + 2)
                {
                    strSname = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strJeaDate = ssView_Sheet1.Cells[i, 2].Text.Trim();
                    striLLs1 = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strInGiGan = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strRoom = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strHuAmt = VB.Val(ssView_Sheet1.Cells[i, 6].Text).ToString("#########").Trim();
                    strRemark11 = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strRemark21 = ssView_Sheet1.Cells[i, 8].Text.Trim();
                    strBigo11 = ssView_Sheet1.Cells[i, 9].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 10].Text;

                    strJumin = clsAES.AES(VB.Replace(ssView_Sheet1.Cells[i + 1, 1].Text.Trim(), "-", ""));
                    strOUDate = ssView_Sheet1.Cells[i + 1, 2].Text.Trim();
                    striLLs2 = ssView_Sheet1.Cells[i + 1, 3].Text.Trim();
                    strToGiGan = ssView_Sheet1.Cells[i + 1, 4].Text.Trim();
                    strGanName = ssView_Sheet1.Cells[i + 1, 5].Text.Trim();
                    strSaAmt = VB.Val(ssView_Sheet1.Cells[i + 1, 6].Text).ToString("#########").Trim();
                    strRemark12 = ssView_Sheet1.Cells[i + 1, 7].Text.Trim();
                    strRemark22 = ssView_Sheet1.Cells[i + 1, 8].Text.Trim();
                    strBigo12 = ssView_Sheet1.Cells[i + 1, 9].Text.Trim();
                    strPano = ssView_Sheet1.Cells[i + 1, 10].Text.Trim();

                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL = " INSERT INTO MIR_SANTONG (YYMM,PANO,SNAME,JUMIN,JEADATE,OUDATE,ILLS1,ILLS2,INGAGAN,";
                        SQL = SQL + ComNum.VBLF + " TOGAGAN,ROOM,GanName , HuAmt, SAAMT, REMARK11, REMARK12, ";
                        SQL = SQL + ComNum.VBLF + " REMARK21, REMARK22, BIGO11, BIGO12) VALUES ( ";
                        SQL = SQL + ComNum.VBLF + " '" + strYYMM + "', '" + strPano + "', '" + strSname + "', '" + strJumin + "', ";
                        SQL = SQL + ComNum.VBLF + " TO_DATE('" + strJeaDate + "','YYYY-MM-DD'), TO_DATE('" + strOUDate + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + " '" + striLLs1 + "', '" + striLLs2 + "','" + strInGiGan + "', '" + strToGiGan + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strRoom + "', '" + strGanName + "', '" + strHuAmt + "', '" + strSaAmt + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strRemark11 + "', '" + strRemark12 + "', '" + strRemark21 + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strRemark22 + "', '" + strBigo11 + "', '" + strBigo12 + "') ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        SQL = "";
                        SQL = " UPDATE MIR_SANTONG SET ";
                        SQL = SQL + ComNum.VBLF + " OUDATE = TO_DATE('" + strOUDate + "', 'YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + " HUAMT = '" + strHuAmt + "', GANNAME = '" + strGanName + "', ";
                        SQL = SQL + ComNum.VBLF + " SAAMT = '" + strSaAmt + "', REMARK11  = '" + strRemark11 + "', ";
                        SQL = SQL + ComNum.VBLF + " REMARK12  = '" + strRemark12 + "', REMARK21  = '" + strRemark21 + "', ";
                        SQL = SQL + ComNum.VBLF + " REMARK22  = '" + strRemark22 + "', BIGO11  = '" + strBigo11 + "', ";
                        SQL = SQL + ComNum.VBLF + " BIGO12  = '" + strBigo12 + "' , JUMIN = '" + strJumin + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";

            //Print Head 지정
            strFont1 = "/fn\"SYSTEM\" /fz\"20\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = strHead1 + " / c" + dtpDate.Value.ToString().Trim() + " 산재환자";

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
