using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSanPatientsViewExamination
    /// File Name : frmSanPatientsViewExamination.cs
    /// Title or Description : 산재환자현황 현지심사대상자
    /// Author : 박창욱
    /// Create Date : 2017-06-02
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>
    /// <history>  
    /// VB\Frm산재환자현황_01.frm(Frm산재환자현황_01) -> frmSanPatientsViewExamination.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busanid\Frm산재환자현황_01.frm(Frm산재환자현황_01)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busanid\\busanid.vbp
    /// </vbp>
    public partial class frmSanPatientsViewExamination : Form
    {
        public frmSanPatientsViewExamination()
        {
            InitializeComponent();
        }

        private void frmSanPatientsViewExamination_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int k = 0;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            strFDate = "2013-09-15";
            strTDate = clsPublic.GstrSysDate;

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.ColumnCount = 30;

            try
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, JUMIN1, JUMIN2  FROM BAS_SANID_P";
                SQL = SQL + ComNum.VBLF + " ORDER BY SNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 500;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                k = 0;
                strPano = "";
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD - 1; i++)
                {
                    if(strPano != dt.Rows[i]["Pano"].ToString().Trim())
                    {
                        k = k + 1;
                    }
                    ssView_Sheet1.Cells[k - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[k - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[k - 1, 2].Text = dt.Rows[i]["Jumin1"].ToString().Trim();
                    ssView_Sheet1.Cells[k - 1, 3].Text = dt.Rows[i]["Jumin2"].ToString().Trim();

                    if (chkPhysical.Checked == true)
                    {
                        SQL = "";
                        SQL = " SELECT ACTDATE fROM ETC_PTORDER";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BI = '31' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                      

                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[k - 1, 10].Text = dt1.Rows[0]["ACTDATE"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    if(chkOutPatient.Checked == true)
                    {
                        SQL = "";
                        SQL = " SELECT ACTDATE, DEPTCODE FROM OPD_MASTER";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BI = '31' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY ACTDATE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[k - 1, 8].Text = dt1.Rows[0]["ACTDATE"].ToString().Trim();
                            ssView_Sheet1.Cells[k - 1, 9].Text = dt1.Rows[0]["DEPTCODE"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;

                    }

                    if (chkInPatient.Checked == true)
                    {
                        
                        SQL = "";
                        SQL = " SELECT A.PANO, B.SNAME, B.ROOMCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                        SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, 'T', B.WARDCODE,  B.DEPTCODE ";
                        SQL = SQL + ComNum.VBLF + "  FROM IPD_TRANS A, IPD_NEW_MASTER B  ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')";
                        SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                        SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NOT NULL";

                        SQL = SQL + ComNum.VBLF + "  UNION ALL ";

                        SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.ROOMCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                        SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, 'J', B.WARDCODE,  B.DEPTCODE  ";
                        SQL = SQL + ComNum.VBLF + "   FROM IPD_TRANS A, IPD_NEW_MASTER B ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')";
                        SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                        SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NULL";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                      

                        if (dt1.Rows.Count > 0)
                        {
                            if(chkTotal.Checked == true)
                            {
                                for (j = 0; j < dt1.Rows.Count - 1; j++)
                                {
                                    ssView_Sheet1.Cells[k - 1, 4].Text = dt1.Rows[j]["INDATE"].ToString().Trim();
                                    ssView_Sheet1.Cells[k - 1, 5].Text = dt1.Rows[j]["WARDCODE"].ToString().Trim();
                                    ssView_Sheet1.Cells[k - 1, 6].Text = dt1.Rows[j]["ROOMCODE"].ToString().Trim();
                                    ssView_Sheet1.Cells[k - 1, 7].Text = dt1.Rows[j]["DEPTCODE"].ToString().Trim();

                                    if (j < dt1.Rows.Count) { k = k + 1; }
                                }
                            }
                            else
                            {
                                ssView_Sheet1.Cells[k - 1, 4].Text = dt1.Rows[j]["INDATE"].ToString().Trim();
                                ssView_Sheet1.Cells[k - 1, 5].Text = dt1.Rows[j]["WARDCODE"].ToString().Trim();
                                ssView_Sheet1.Cells[k - 1, 6].Text = dt1.Rows[j]["ROOMCODE"].ToString().Trim();
                                ssView_Sheet1.Cells[k - 1, 7].Text = dt1.Rows[j]["DEPTCODE"].ToString().Trim();
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int k = 0;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPano = "";
            string strDate1 = "";  //재해일자
            string striLLs1 = "";  //승인상병명1
            string striLLs2 = "";  //승인상병명2
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            strFDate = "2013-07-01";
            strTDate = "2016-07-31";

            ssView_Sheet1.ColumnCount = 10;

            try
            {
                SQL = "";
                SQL = " SELECT PANO, SNAME, JUMIN1, JUMIN2  FROM BAS_SANID_P";
                SQL = SQL + ComNum.VBLF + " ORDER BY SNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 500;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                k = 0;
                strPano = "";
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD - 1; i++)
                {
                    if(strPano != dt.Rows[i]["Pano"].ToString().Trim())
                    {
                        k = k + 1;
                        if(ssView_Sheet1.RowCount < k)
                        {
                            ssView_Sheet1.RowCount += 1;
                        }
                    }
                    ssView_Sheet1.Cells[k - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[k - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[k - 1, 2].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + 
                                                         dt.Rows[i]["Jumin2"].ToString().Trim();
                    ssView_Sheet1.Rows[k - 1].BackColor = Color.FromArgb(166, 251, 249);

                    //재해일자 SELECT
                    SQL = "";
                    SQL = " SELECT TO_CHAR(DATE1,'YYYY-MM-DD') DATE1, JUMIN1||'-'||JUMIN2 JUMIN , JUMIN1, JUMIN3  FROM BAS_SANID ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";

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
                        return;
                    }
                    strDate1 = dt1.Rows[0]["DATE1"].ToString().Trim();

                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[k - 1, 3].Text = strDate1;

                    //승인상병명
                    SQL = "";
                    SQL = " SELECT RANK,ILLNAME FROM MIR_SANILLS";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWNUM <= 2 ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (j = 0; j < dt1.Rows.Count - 1; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                striLLs1 = dt1.Rows[0]["ILLNAME"].ToString().Trim();
                                break;
                            case 1:
                                striLLs1 = dt1.Rows[1]["ILLNAME"].ToString().Trim();
                                break;
                            case 2:
                                striLLs1 = dt1.Rows[1]["ILLNAME"].ToString().Trim() + " 외 " + (dt1.Rows.Count -2) + "건";
                                break;
                        }
                        if (j == 2) { break; }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[k - 1, 7].Text = striLLs1 + " " + striLLs2;


                    //입통원 구분
                    SQL = "";
                    SQL = " SELECT A.PANO, B.SNAME, B.ROOMCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, 'T', B.WARDCODE,  B.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "  FROM IPD_TRANS A, IPD_NEW_MASTER B  ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND (";
                    SQL = SQL + ComNum.VBLF + "        (A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "         OR ";
                    SQL = SQL + ComNum.VBLF + "        (A.INDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') AND A.INDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD'))";
                    SQL = SQL + ComNum.VBLF + "       ) ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                    SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NOT NULL";
                    SQL = SQL + ComNum.VBLF + "   AND A.AMT50 <> 0 ";
                    SQL = SQL + ComNum.VBLF + "  UNION ALL ";

                    SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, B.ROOMCODE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                    SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, 'J', B.WARDCODE,  B.DEPTCODE  ";
                    SQL = SQL + ComNum.VBLF + "   FROM IPD_TRANS A, IPD_NEW_MASTER B ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "   AND A.GBIPD NOT IN ('D')";
                    SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                    SQL = SQL + ComNum.VBLF + "   AND B.OUTDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "   AND A.AMT50 <> 0 ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        for (j = 0; j < dt1.Rows.Count; j++)
                        {
                            k = k + 1;
                            if(ssView_Sheet1.RowCount < k)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            }
                            ssView_Sheet1.Cells[k - 1, 4].Text = "입원";
                            ssView_Sheet1.Cells[k - 1, 5].Text = dt1.Rows[j]["INDATE"].ToString().Trim() + " ~ " +
                                                                 dt1.Rows[j]["OUTDATE"].ToString().Trim();
                            ssView_Sheet1.Cells[k - 1, 6].Text = dt1.Rows[j]["ROOMCODE"].ToString().Trim();
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    //외래
                    SQL = "";
                    SQL = " SELECT PANO ,TO_CHAR(MIN(ACTDATE),'YYYY/MM/DD') MINDATE, TO_CHAR(MAX(ACTDATE),'YYYY/MM/DD') MAXDATE FROM OPD_MASTER";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "    GROUP BY PANO ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        k = k + 1;
                        if (ssView_Sheet1.RowCount < k)
                        {
                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        }
                        ssView_Sheet1.Cells[k - 1, 4].Text = "외래";
                        ssView_Sheet1.Cells[k - 1, 5].Text = dt1.Rows[0]["MINDATE"].ToString().Trim() + " ~ " +
                                                             dt1.Rows[0]["MAXDATE"].ToString().Trim();
                    }
                    dt1.Dispose();
                    dt1 = null;

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);   //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 500;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";

            //Print Head 지정
            strFont1 = "/fn\"SYSTEM\" /fz\"20\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = strHead1 + "/c" + " 산재 현지환자 목록";

            //Print Body
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n";
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
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
