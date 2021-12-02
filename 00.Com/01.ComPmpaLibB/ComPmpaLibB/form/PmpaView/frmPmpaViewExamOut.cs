using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewExamOut.cs
    /// Description     : 심사자별 퇴원건수 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iusent\Frm심사자퇴원조회.frm(Frm심사자퇴원조회.frm) >> frmPmpaViewExamOut.cs 폼이름 재정의" />	
    public partial class frmPmpaViewExamOut : Form
    {
        public frmPmpaViewExamOut()
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
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;
            string strOK = "";
            string strGbSTS = "";
            string strSname = "";   //최초심사 성명
            long nSabun = 0;        //최초심사 사번
            string strSName2 = "";  //심사수정

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //IPD_TRANS 단위로 DISPLAY

                SQL = "";
                SQL = " SELECT B.IPDNO, B.Pano,a.Sname,a.RoomCode,";
                SQL = SQL + "        TO_CHAR(A.InDate,'YYYY-MM-DD') RealInDate,";
                SQL = SQL + "        TO_CHAR(B.InDate,'YY/MM/DD') InDate,";
                SQL = SQL + "        B.GbSTS,TO_CHAR(B.OutDate,'YY/MM/DD') OutDate,";
                SQL = SQL + "        b.DeptCode,b.DrCode,TO_CHAR(B.RoutDate,'YY/MM/DD HH24:MI') RoutDate,";
                SQL = SQL + "        TO_CHAR(B.SunapTime,'YY/MM/DD HH24:MI') SunapTime,";
                SQL = SQL + "        B.GbSTS, b.TRSNO,b.Bi,b.GbIPD,b.AmSet3, B.AmSet5,B.AmSet4,a.GbSuDay,";
                SQL = SQL + "        B.OGPDBUN,b.OGPDBUNdtl, B.VCODE,b.SimsaSabun , B.GBCHECKLIST";
                SQL = SQL + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "IPD_TRANS b";
                SQL = SQL + "  WHERE 1 = 1";
                SQL = SQL + "    AND b.ActDATE =  TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + "    AND B.GbSTS IN ('7')";
                SQL = SQL + "    AND a.IPDNO = b.IPDNO(+)";
                SQL = SQL + "    AND b.GbIPD IN ('1','9')";
                SQL = SQL + " ORDER BY B.ROUTDATE ASC , B.PANO, B.OutDate,a.RoomCode,B.IPDNO,b.TRSNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
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

                nRow = 0;
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    //최초심사자체크
                    SQL = "";
                    SQL = " SELECT SIMSA_SNAME,SIMSA_Sabun ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Trsno =" + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                    SQL = SQL + ComNum.VBLF + "   AND (Simsa_No IS NULL  OR Simsa_No  ='') ";
                    SQL = SQL + ComNum.VBLF + "   AND  (GbSts ='5' OR Flag ='Y') ";
                    SQL = SQL + ComNum.VBLF + "   AND Simsa_Sabun in (";
                    SQL = SQL + ComNum.VBLF + " SELECT sabun FROM " + ComNum.DB_ERP + "INSA_MST ";
                    SQL = SQL + ComNum.VBLF + " WHERE Buse ='078201' ) ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY Simsa_OK";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    strSname = "";
                    if (dt1.Rows.Count > 0)
                    {
                        strSname = dt1.Rows[0]["SIMSA_SNAME"].ToString().Trim();
                        nSabun = (long)VB.Val(dt1.Rows[0]["Simsa_Sabun"].ToString().Trim());
                        if (txtSabun.Text.Trim() != "")
                        {
                            if (VB.Val(txtSabun.Text) == VB.Val(dt1.Rows[0]["Simsa_Sabun"].ToString().Trim()))
                            {
                                strOK = "OK";
                            }
                            else
                            {
                                strOK = "";
                            }
                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    if (txtSabun.Text.Trim() == "")
                    {
                        strOK = "OK";
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        ssView_Sheet1.RowCount = nRow;
                        strGbSTS = dt.Rows[i]["GbSTS"].ToString().Trim();

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["OutDate"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = "";

                        if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "지병" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "P" || dt.Rows[i]["OGPDBUN"].ToString().Trim() == "O")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "면제";
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        //Y268 뇌출혈 추가
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" ||
                                 dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || dt.Rows[i]["VCODE"].ToString().Trim() == "V268")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" || dt.Rows[i]["VCODE"].ToString().Trim() == "V247" ||
                                 dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증화상E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" || dt.Rows[i]["VCODE"].ToString().Trim() == "V247" ||
                                 dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증화상F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" ||
                                 dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증화상+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "H")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "희귀H";
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "V")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "희귀V";
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "C")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "차상";
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                        {
                            if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = "차상E+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = "차상E" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                        {
                            if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = "차상F+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = "차상F" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                        }
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["TRSNO"].ToString().Trim();

                        ssView_Sheet1.Cells[nRow - 1, 11].Text = strSname;

                        if (strSname == "")
                        {
                            //최종심사자
                            SQL = "";
                            SQL = "SELECT SIMSA_SNAME,SIMSA_Sabun ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                            SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND Trsno =" + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                            SQL = SQL + ComNum.VBLF + "   AND (Simsa_No IS NULL  OR Simsa_No  ='') ";
                            SQL = SQL + ComNum.VBLF + "   AND (GbSts ='5' OR Flag ='Y')  ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY EntDate  ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = dt1.Rows[0]["SIMSA_SNAME"].ToString().Trim();
                                nSabun = (long)VB.Val(dt1.Rows[0]["SIMSA_Sabun"].ToString().Trim());
                            }
                            dt1.Dispose();
                            dt1 = null;


                            //보조심사자
                            SQL = "";
                            SQL = " SELECT SIMSA_SNAME ";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                            SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                            SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND Trsno =" + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                            SQL = SQL + ComNum.VBLF + "   AND (Simsa_No IS NULL  OR Simsa_No  ='') ";
                            SQL = SQL + ComNum.VBLF + "   AND (GbSts ='5' OR Flag ='Y')  ";
                            SQL = SQL + ComNum.VBLF + "   AND SIMSA_Sabun <> " + nSabun + " ";
                            SQL = SQL + ComNum.VBLF + " ORDER BY EntDate  ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            strSName2 = "";
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = "";
                            if (dt1.Rows.Count > 0)
                            {
                                strSName2 += dt1.Rows[0]["SIMSA_SNAME"].ToString().Trim() + ",";
                            }
                            dt1.Dispose();
                            dt1 = null;

                            if (VB.Len(strSName2) > 1)
                            {
                                strSName2 = VB.Mid(strSName2, 1, VB.Len(strSName2) - 1);
                            }

                            ssView_Sheet1.Cells[nRow - 1, 12].Text = strSName2;
                        }
                        else
                        {
                            //보조심사자
                            SQL = "";
                            SQL = " SELECT SIMSA_SNAME ";
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                            SQL = SQL + ComNum.VBLF + "  AND PANO  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Trsno =" + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                            SQL = SQL + ComNum.VBLF + "  AND (Simsa_No IS NULL  OR Simsa_No  ='') ";
                            SQL = SQL + ComNum.VBLF + "  AND (GbSts ='5' OR Flag ='Y')  ";
                            SQL = SQL + ComNum.VBLF + "  AND SIMSA_Sabun <> " + nSabun + " ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY EntDate  ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            strSName2 = "";
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = "";
                            if (dt1.Rows.Count > 0)
                            {
                                strSName2 += dt1.Rows[0]["SIMSA_SNAME"].ToString().Trim() + ",";
                            }
                            dt1.Dispose();
                            dt1 = null;

                            if (VB.Len(strSName2) > 1)
                            {
                                strSName2 = VB.Mid(strSName2, 1, VB.Len(strSName2) - 1);
                            }

                            ssView_Sheet1.Cells[nRow - 1, 12].Text = strSName2;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["SunapTime"].ToString().Trim();
                    }
                }

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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

        private void frmPmpaViewExamOut_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //this.Close();
            //return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpOutDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            txtSabun.Text = "";
        }

    }
}
