using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDailyChangeRoom.cs
    /// Description     : 선택진료 관련 의사변경 및 전실전과 명단
    /// Author          : 박창욱
    /// Create Date     : 2017-09-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm일자별전실전과내역2.frm(Frm일자별전실전과내역2.frm) >> frmPmpaViewDailyChangeRoom.cs 폼이름 재정의" />	
    public partial class frmPmpaViewDailyChangeRoom : Form
    {
        public frmPmpaViewDailyChangeRoom()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            int nRead = 0;
            string strPano = "";
            string strBi = "";
            string strSname = "";
            string strInDate = "";
            string strFrRoom = "";
            string strToRoom = "";
            string strList = "";
            string strTemp = "";
            string strOK = "";
            string strDrcode = "";
            long nIPDNO = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            strDrcode = "";
            if (txtDrCode.Text.Length == 4)
            {
                strDrcode = txtDrCode.Text;
            }

            if (dtpFDate.Value < Convert.ToDateTime("2011-06-01"))
            {
                ComFunc.MsgBox("2011-06-01 부터 선택진료시행입니다." + ComNum.VBLF + ComNum.VBLF + "이전 자료는 정확한 자료가 아닙니다.");
            }

            try
            {
                SQL = "";
                SQL = " SELECT Pano,FrWard,FrRoom,FrDept,FrDoctor,ToWard,ToRoom,ToDept,ToDoctor,IPDNO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(TrsDate,'YYYY-MM-DD HH24:MI') TrsDate,'전실전과' AS Gubun";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND TrsDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND TrsDate < TO_DATE('" + dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND FrDoctor <> ToDoctor";
                if (strDrcode != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND ( FrDoctor IN ( SELECT DRCODE FROM KOSMOS_PMPA.BAS_DOCTOR WHERE GBCHOICE='Y' AND DrCode ='" + strDrcode + "' ) OR";
                    SQL = SQL + ComNum.VBLF + "        ToDoctor IN ( SELECT DRCODE FROM KOSMOS_PMPA.BAS_DOCTOR WHERE GBCHOICE='Y' AND DrCode ='" + strDrcode + "') )";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND ( FrDoctor IN ( SELECT DRCODE FROM KOSMOS_PMPA.BAS_DOCTOR WHERE GBCHOICE='Y' ) OR";
                    SQL = SQL + ComNum.VBLF + "        ToDoctor IN ( SELECT DRCODE FROM KOSMOS_PMPA.BAS_DOCTOR WHERE GBCHOICE='Y') ) ";
                }
                SQL = SQL + ComNum.VBLF + "   AND Pano <> '81000004'";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;

                nRow = 0;

                for (i = 0; i < nRead; i++)
                {
                    strPano = VB.Val(dt.Rows[i]["Pano"].ToString().Trim()).ToString("00000000");
                    nIPDNO = (long)VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim());

                    strOK = "";

                    if (ComFunc.READ_IPD_NEW_MASTER_INDATE_CHK(clsDB.DbCon, nIPDNO) == "OK")
                    {
                        strOK = "OK";
                    }

                    if (chkIp.Checked == false)
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;

                        SQL = "";
                        SQL = " SELECT Sname,Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND Pano = '" + strPano + "'";
                        SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + nIPDNO + "";
                        SQL = SQL + ComNum.VBLF + "    AND AmSet6 != '*'";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strSname = dt1.Rows[0]["Sname"].ToString().Trim();
                            strBi = dt1.Rows[0]["Bi"].ToString().Trim();
                            strInDate = dt1.Rows[0]["InDate"].ToString().Trim();
                        }
                        else
                        {
                            strSname = "";
                            strBi = "";
                            strInDate = "";
                        }
                        dt1.Dispose();
                        dt1 = null;

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano.Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = strSname.Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = strBi.Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["FrWard"].ToString().Trim() + " " + dt.Rows[i]["FrRoom"].ToString().Trim() + " " +
                                                                dt.Rows[i]["FrDept"].ToString().Trim() + "(" + dt.Rows[i]["FrDoctor"].ToString().Trim() + ") " +
                                                                clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["FrDoctor"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["ToWard"].ToString().Trim() + " " + dt.Rows[i]["FrRoom"].ToString().Trim() + " " +
                                                                dt.Rows[i]["ToDept"].ToString().Trim() + "(" + dt.Rows[i]["ToDoctor"].ToString().Trim() + ") " +
                                                                clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["ToDoctor"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["TrsDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;

                SQL = "";
                SQL = " SELECT SUBSTR(REMARK,1,8) PANO,'' FrWard,'' FrRoom,'' FrDept,'' ToWard,'' ToRoom,'' ToDept, IPDNO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(JobTime,'YYYY-MM-DD HH24:MI') TrsDate,Remark,'의사변경' AS Gubun";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_JOBHISTORY";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND JobTime >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND  JobTime < TO_DATE('" + dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {

                    nRead = dt.Rows.Count;
                    ssView_Sheet1.RowCount += nRead;
                    for (i = 0; i < nRead; i++)
                    {
                        strTemp = dt.Rows[i]["Remark"].ToString().Trim();
                        nIPDNO = (long)VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim());
                        strPano = VB.Pstr(strTemp, " ", 1).Trim();
                        strSname = VB.Pstr(strTemp, " ", 2).Trim();
                        strList = "(" + VB.Pstr(strTemp, "(", 2).Trim();
                        strFrRoom = VB.Mid(strList.Trim(), 2, 4);
                        strToRoom = VB.Mid(strList.Trim(), 8, 4);

                        strOK = "";
                        SQL = "";
                        SQL = " SELECT DrCode FROM " + ComNum.DB_PMPA + "BAS_DOCTOR WHERE ( DRCODE ='" + strFrRoom + "' OR DRCODE ='" + strToRoom + "' ) AND GBCHOICE='Y' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strOK = "OK";
                        }
                        dt1.Dispose();
                        dt1 = null;

                        if (strOK == "OK" && strDrcode != "")
                        {
                            if (strDrcode == strFrRoom || strDrcode == strToRoom)
                            {
                                strOK = "OK";
                            }
                            else
                            {
                                strOK = "";
                            }
                        }

                        if (chkIp.Checked == true)
                        {
                            if (ComFunc.READ_IPD_NEW_MASTER_INDATE_CHK(clsDB.DbCon, nIPDNO) == "OK" && strOK == "OK")
                            {
                                strOK = "OK";
                            }
                            else
                            {
                                strOK = "";
                            }
                        }


                        if (strOK == "OK" && strPano != "81000004")
                        {
                            nRow += 1;
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano.Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = strSname.Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = "";
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = strList + "(" + strFrRoom + ") " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strFrRoom);
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = strList + "(" + strToRoom + ") " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strToRoom);
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["TrsDate"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;

                //SQL 에러
                SQL = "";
                SQL = " SELECT  TO_CHAR(ActDate,'YYYY-MM-DD') TrsDate,SQL,'변경에러!' AS Gubun";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_SQLERROR";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND ActDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND ActDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND SQL LIKE 'INSERT INTO ETC_JobH%'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    ssView_Sheet1.RowCount += nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        strTemp = dt.Rows[i]["SQL"].ToString().Trim();
                        strPano = VB.Left(VB.Pstr(strTemp, "`", 6), 8);
                        strSname = VB.Pstr(VB.Pstr(strTemp, "`", 6), " ", 2);
                        strFrRoom = VB.Left(VB.Pstr(strTemp, "(", 3).Trim(), 4);
                        strToRoom = VB.Mid(VB.Pstr(strTemp, "(", 3).Trim(), 7, 4);
                        strOK = "";

                        SQL = "";
                        SQL = " SELECT DrCode FROM " + ComNum.DB_PMPA + "BAS_DOCTOR WHERE ( DRCODE ='" + strFrRoom + "' OR DRCODE ='" + strToRoom + "' ) AND GBCHOICE='Y' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strOK = "OK";
                        }
                        dt1.Dispose();
                        dt1 = null;

                        if (strOK == "OK" && strDrcode != "")
                        {
                            if (strDrcode == strFrRoom || strDrcode == strToRoom)
                            {
                                strOK = "OK";
                            }
                            else
                            {
                                strOK = "";
                            }
                        }

                        if (strOK == "OK" && strPano != "81000004")
                        {
                            nRow += 1;
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = strPano.Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = strSname.Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = "";
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = strList + "(" + strFrRoom + ") " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strFrRoom);
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = strList + "(" + strToRoom + ") " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strToRoom);
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["TrsDate"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            string strFont1 = "";
            string strHead1 = "";

            Cursor.Current = Cursors.WaitCursor;

            //Print Head
            strFont1 = "/fn\"굴림체\" /fz\"20\" /fs1";
            strHead1 = "/c";

            strHead1 = strHead1 + "선택진료 의사변경 명부" + "/n";
            ssPrint_Sheet1.Cells[3, 0].Text = "전실전과일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd");

            //Print Body
            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].ColumnSpan = 2;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].ColumnSpan = 3;

                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 6].Text;
            }

            ssPrint_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssPrint_Sheet1.PrintInfo.Margin.Left = 0;
            ssPrint_Sheet1.PrintInfo.Margin.Right = 0;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 120;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 100;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = false;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint.PrintSheet(0);

            Cursor.Current = Cursors.Default;

        }

        private void frmPmpaViewDailyChangeRoom_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            if (clsType.User.Sabun == "4349")
            {
                txtDrCode.Visible = true;
                lblDrCode.Visible = true;
            }
        }
    }
}
