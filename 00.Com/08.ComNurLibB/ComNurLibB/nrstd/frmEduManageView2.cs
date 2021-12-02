using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduManageView2.cs
    /// Description     : 부서별교육내역조회화면
    /// Author          : 박창욱
    /// Create Date     : 2018-01-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리조회2.frm(Frm교육관리조회2.frm) >> frmEduManageView2.cs 폼이름 재정의" />	
    public partial class frmEduManageView2 : Form
    {
        string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmEduManageView2()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (rdoGbn10.Checked == true)
            {
                strTitle = "부서별 교육건수현황";
            }
            else
            {
                strTitle = "부서별 교육점수현황";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년월 : " + cboYYMM.Text + "월", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            //DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int MM = 0;
            int nRow = 0;
            double[,] nTOT = new double[3, 14];
            string strFrDate = "";
            string strToDate = "";
            string strOldData = "";
            string strNewData = "";
            string strBuName = "";

            ComFunc cf = new ComFunc();

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            //누적할 변수를 Clear
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 14; j++)
                {
                    nTOT[i, j] = 0;
                }
            }

            strFrDate = VB.Left(cboYYMM.Text, 4) + "-01-01";
            strToDate = cf.READ_LASTDAY(clsDB.DbCon, VB.Left(cboYYMM.Text, 4) + "-" + VB.Right(strSysDate, 5));

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region View2

                //ssView2_Sheet1.RowCount = 2;

                //SQL = "";
                //SQL = "   SELECT c.Name BuName, B.KORNAME, B.JIK, B.SABUN, B.KUNDAY, SUM(EDUTIME) EDUTIME";
                //SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_MST a, " + ComNum.DB_PMPA + "BAS_BUSE c, " + ComNum.DB_ERP + "INSA_MST B";
                //SQL = SQL + ComNum.VBLF + " WHERE A.FRDate>=TO_DATE('" + strFrDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "   AND a.FRDate<=TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                //SQL = SQL + ComNum.VBLF + "   AND TRUNC (A.SABUN) = B.SABUN";
                //if (cboBuse.Text.Trim() != "")
                //{
                //    SQL = SQL + ComNum.VBLF + "   AND B.BUSE = '" + VB.Right(cboBuse.Text, 6) + "' ";
                //}
                //if (rdoGbn0.Checked == true)
                //{
                //    SQL = SQL + ComNum.VBLF + "   AND a.Gubun='1' ";
                //}
                //else
                //{
                //    SQL = SQL + ComNum.VBLF + "   AND (a.Gubun='2' AND SIGN = '1') ";
                //}
                //SQL = SQL + ComNum.VBLF + "   AND a.BuCode = c.BuCode";
                //SQL = SQL + ComNum.VBLF + " GROUP BY c.Name, B.KORNAME, B.JIK, B.SABUN, B.KUNDAY";
                //SQL = SQL + ComNum.VBLF + " ORDER BY B.JIK, B.SABUN ";

                //SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    Cursor.Current = Cursors.Default;
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    return;
                //}

                //if (dt.Rows.Count > 0)
                //{
                //    ssView2_Sheet1.RowCount = dt.Rows.Count + 2;
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        ssView2_Sheet1.Cells[i + 2, 0].Text = (i + 1).ToString();
                //        ssView2_Sheet1.Cells[i + 2, 1].Text = dt.Rows[i]["BUNAME"].ToString().Trim();
                //        ssView2_Sheet1.Cells[i + 2, 2].Text = dt.Rows[i]["KORNAME"].ToString().Trim();

                //        SQL = "";
                //        SQL = "SELECT NAME";
                //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_CODE ";
                //        SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                //        SQL = SQL + ComNum.VBLF + "   AND CODE = '" + dt.Rows[i]["JIK"].ToString().Trim() + "' ";

                //        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                //        if (SqlErr != "")
                //        {
                //            Cursor.Current = Cursors.Default;
                //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //            return;
                //        }

                //        if (dt1.Rows.Count > 0)
                //        {
                //            ssView2_Sheet1.Cells[i + 2, 3].Text = dt1.Rows[0]["NAME"].ToString().Trim();
                //        }
                //        else
                //        {
                //            ssView2_Sheet1.Cells[i + 2, 3].Text = "";
                //        }

                //        dt1.Dispose();
                //        dt1 = null;

                //        ssView2_Sheet1.Cells[i + 2, 4].Text = dt.Rows[i]["SABUN"].ToString().Trim();
                //        ssView2_Sheet1.Cells[i + 2, 5].Text = dt.Rows[i]["KUNDAY"].ToString().Trim();
                //        ssView2_Sheet1.Cells[i + 2, 6].Text = dt.Rows[i]["EDUTIME"].ToString().Trim();

                //        SQL = "";
                //        SQL = "SELECT B.GROUPNAME, SUM(A.EDUTIME) EDUTIME";
                //        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_MST A, " + ComNum.DB_PMPA + "NUR_EDU_CODE_SET B";
                //        SQL = SQL + ComNum.VBLF + " WHERE  A.EDUJONG = SUBSTR(B.CODE, 1, 2)";
                //        SQL = SQL + ComNum.VBLF + "   AND A.FRDate>=TO_DATE('" + strFrDate + "','YYYY-MM-DD') ";
                //        SQL = SQL + ComNum.VBLF + "   AND a.FRDate<=TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                //        SQL = SQL + ComNum.VBLF + "   AND TRUNC (A.SABUN) = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "' ";
                //        if (rdoGbn0.Checked == true)
                //        {
                //            SQL = SQL + ComNum.VBLF + "   AND a.Gubun='1' ";
                //        }
                //        else
                //        {
                //            SQL = SQL + ComNum.VBLF + "   AND (a.Gubun='2' AND SIGN = '1') ";
                //        }
                //        SQL = SQL + ComNum.VBLF + " GROUP BY B.GROUPNAME";

                //        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                //        if (SqlErr != "")
                //        {
                //            Cursor.Current = Cursors.Default;
                //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //            return;
                //        }

                //        if (dt1.Rows.Count > 0)
                //        {
                //            for (j = 0; j < dt1.Rows.Count; j++)
                //            {
                //                switch (dt1.Rows[j]["GROUPNAME"].ToString().Trim())
                //                {
                //                    case "필수":
                //                        ssView2_Sheet1.Cells[i + 2, 7].Text = dt1.Rows[j]["EDUTIME"].ToString().Trim();
                //                        break;
                //                    case "직무":
                //                        ssView2_Sheet1.Cells[i + 2, 8].Text = dt1.Rows[j]["EDUTIME"].ToString().Trim();
                //                        break;
                //                    case "병동":
                //                        ssView2_Sheet1.Cells[i + 2, 9].Text = dt1.Rows[j]["EDUTIME"].ToString().Trim();
                //                        break;
                //                    case "사이버":
                //                        ssView2_Sheet1.Cells[i + 2, 10].Text = dt1.Rows[j]["EDUTIME"].ToString().Trim();
                //                        break;
                //                    case "기타":
                //                        ssView2_Sheet1.Cells[i + 2, 11].Text = dt1.Rows[j]["EDUTIME"].ToString().Trim();
                //                        break;
                //                }
                //            }
                //        }

                //        dt1.Dispose();
                //        dt1 = null;
                //    }
                //}

                //dt.Dispose();
                //dt = null;

                #endregion


                //자료를 Select
                SQL = "";
                if (rdoGbn10.Checked == true)
                {
                    SQL = "SELECT a.BuCode,c.Name BuName,TO_CHAR(a.SDate,'MM') SDate, COUNT(*) CNT ";
                }
                else if (rdoGbn11.Checked == true)
                {
                    SQL = "SELECT a.BuCode,c.Name BuName,TO_CHAR(a.SDate,'MM') SDate,SUM(TO_NUMBER(JumSu)) CNT ";
                }
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_MST a, " + ComNum.DB_PMPA + "BAS_BUSE c , " + ComNum.DB_ERP + "INSA_MST B";
                SQL = SQL + ComNum.VBLF + " WHERE a.FRDate>=TO_DATE('" + strFrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.FRDate<=TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  TRUNC(A.SABUN) = B.SABUN";
                if (cboBuse.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.BUSE = '" + VB.Right(cboBuse.Text, 6) + "' ";
                }
                if (rdoGbn0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND a.Gubun='1' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND (a.Gubun='2' AND SIGN = '1') ";
                }
                SQL = SQL + ComNum.VBLF + "   AND a.BuCode=c.BuCode ";
                SQL = SQL + ComNum.VBLF + " GROUP BY a.BuCode,c.Name,TO_CHAR(a.SDate,'MM') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.BuCode,c.Name,TO_CHAR(a.SDate,'MM') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                strOldData = "";
                strBuName = "";
                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["BuCode"].ToString().Trim();
                    if (strOldData != strNewData)
                    {
                        if (nTOT[1, 13] != 0)
                        {
                            nRow += 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = " " + strBuName;
                            for (j = 1; j < 14; j++)
                            {
                                ssView_Sheet1.Cells[nRow - 1, j].Text = nTOT[1, j].ToString("#,##0");
                                nTOT[1, j] = 0;
                            }
                        }
                        strOldData = strNewData;
                        strBuName = dt.Rows[i]["BuName"].ToString().Trim();
                    }

                    MM = (int)VB.Val(dt.Rows[i]["SDate"].ToString().Trim());
                    nTOT[1, MM] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    nTOT[1, 13] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    nTOT[2, MM] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    nTOT[2, 13] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                if (nTOT[1, 13] != 0)
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = " " + strBuName;
                    for (j = 1; j < 14; j++)
                    {
                        ssView_Sheet1.Cells[nRow - 1, j].Text = nTOT[1, j].ToString("#,##0");
                        nTOT[1, j] = 0;
                    }
                }

                //전체합계
                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[nRow - 1, 0].Text = " ** 합계 **";
                for (j = 1; j < 14; j++)
                {
                    ssView_Sheet1.Cells[nRow - 1, j].Text = nTOT[2, j].ToString("#,##0");
                }

                btnPrint.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmEduManageView2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strYYMM = "";

            strYYMM = VB.Left(strSysDate, 4);

            cboYYMM.Items.Clear();
            for (i = 1; i < 11; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년도");
                strYYMM = (VB.Val(strYYMM) - 1).ToString();
            }
            cboYYMM.SelectedIndex = 0;

            btnPrint.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT A.MATCH_CODE BUSE, B.NAME BUNAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_CODE A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.MATCH_CODE = B.BUCODE ";
                SQL = SQL + ComNum.VBLF + "   AND A.SUBUSE = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY SUBRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    return;
                }

                cboBuse.Items.Clear();
                cboBuse.Items.Add(" ");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboBuse.Items.Add(dt.Rows[i]["BuName"].ToString().Trim() + "." + dt.Rows[i]["Buse"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboBuse.Items.Add("정형외과(일반).100251");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
