using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewExamCondition.cs
    /// Description     : 심사처리상황 History
    /// Author          : 박창욱
    /// Create Date     : 2017-09-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-09-04 박창욱 : 유사한 폼 통합
    /// </history>
    /// <seealso cref= "\IPD\iusent\Frm심사처리상황.frm(Frm심사처리상황.frm) >> frmPmpaViewExamCondition.cs 폼이름 재정의" />	
    /// <seealso cref= "\IPD\ipdSim2\Frm심사처리상황.frm(Frm심사처리상황.frm) >> frmPmpaViewExamCondition.cs 폼이름 재정의" />	
    public partial class frmPmpaViewExamCondition : Form
    {
        public frmPmpaViewExamCondition()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        void SearchData()
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
            string strGbSTS = "";

            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssView_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //IPD_TRANS 단위로 DISPLAY
                SQL = "";
                SQL = " SELECT B.IPDNO, B.Pano,a.Sname,a.RoomCode,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') RealInDate,  ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(B.InDate,'YY/MM/DD') InDate,  ";
                SQL = SQL + ComNum.VBLF + "        B.GbSTS,TO_CHAR(B.OutDate,'YY/MM/DD') OutDate, ";
                SQL = SQL + ComNum.VBLF + "        b.DeptCode,b.DrCode,TO_CHAR(B.RoutDate,'YY/MM/DD HH24:MI') RoutDate, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(B.SunapTime,'YY/MM/DD HH24:MI') SunapTime, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(B.ROUTDATE,'YY/MM/DD HH24:MI') ROUTDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(B.SIMSATime,'YY/MM/DD HH24:MI') SIMSATime, ";
                SQL = SQL + ComNum.VBLF + "        B.GbSTS, b.TRSNO,b.Bi,b.GbIPD,b.AmSet3, B.AmSet5,B.AmSet4,a.GbSuDay, ";
                SQL = SQL + ComNum.VBLF + "        B.OGPDBUN,b.OGPDBUNdtl, B.VCODE,b.SimsaSabun , B.GBCHECKLIST ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "IPD_TRANS b ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND b.OUTDATE >=  TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND A.OUTDATE <= TO_DATE('" + dtpOutDateTo.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GbSTS IN ('1','2','3','4','5','6','7') ";
                SQL = SQL + ComNum.VBLF + "    AND a.IPDNO = b.IPDNO(+) ";
                SQL = SQL + ComNum.VBLF + "    AND b.GbIPD IN ('1','9') ";
                if (txtPano.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.Pano ='" + txtPano.Text + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO NOT IN ('02782747') ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY B.ROUTDATE ASC , B.PANO, B.OutDate,a.RoomCode,B.IPDNO,b.TRSNO ";

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

                nRow = 0;
                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    string strSname = ""; //에러문 받는 변수
                    string strOK = ""; //에러문 받는 변수
                    long nSabun = 0; //에러문 받는 변수
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
                    if (strOK !="OK")
                    {
                        continue;
                    }
                    nRow += 1;
                    ssView_Sheet1.RowCount = nRow;
                    strGbSTS = dt.Rows[i]["GbSTS"].ToString().Trim();

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["OutDate"].ToString().Trim();
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
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" &&( dt.Rows[i]["VCODE"].ToString().Trim() == "V247" ||
                             dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250"))
                    {
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = "중증화상E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" &&( dt.Rows[i]["VCODE"].ToString().Trim() == "V247" ||
                             dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250"))
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


                    //최종심사자
                    SQL = "";
                    SQL = "SELECT SIMSA_SNAME ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Trsno =" + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                    SQL = SQL + ComNum.VBLF + "   AND (GbSTS ='5' OR Flag ='Y' ) ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC ";
                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt1.Rows[0]["SIMSA_SNAME"].ToString().Trim();
                    }
                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["SunapTime"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", strGbSTS);


                    //심사완료 CHECK
                    SQL = "";
                    SQL = "SELECT SIMSA_SNAME ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Trsno =" + dt.Rows[i]["TRSNO"].ToString().Trim() + " ";
                    SQL = SQL + ComNum.VBLF + "   AND GbSTS ='5' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC ";
                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count == 1)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].BackColor = Color.FromArgb(255, 255, 0);
                    }
                    else if (dt1.Rows.Count >= 2)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].BackColor = Color.FromArgb(255, 202, 202);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].BackColor = Color.FromArgb(255, 255, 255);
                    }
                    dt1.Dispose();
                    dt1 = null;


                    //간호부에서 퇴원 예고 등록자 표시
                    SQL = "";
                    SQL = " SELECT TO_CHAR(ROUTDATE,'YYYY-MM-DD') ROUTDATE, ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(ROUTENTTIME ,'YYYY-MM-DD') ROUTENTDATE,";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(ROUTENTTIME ,'HH24:MI') RTIME,";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(ROUTENTTIME ,'YYYY-MM-DD HH24:MI') ROUTENTDATENEW";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_MASTER";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND IPDNO  = '" + dt.Rows[i]["TRSNO"].ToString().Trim() + "' ";
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
                        if (Convert.ToDateTime(dt1.Rows[0]["ROutDate"].ToString().Trim()) > Convert.ToDateTime(dt1.Rows[0]["ROUTENTDATE"].ToString().Trim()))
                        {
                            if (Convert.ToDateTime(dt1.Rows[0]["ROutDate"].ToString().Trim()) < Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + " 17:31"))
                            {
                                ssView_Sheet1.Cells[nRow - 1, 14].Text = "예고(" + dt.Rows[i]["GBCHECKLIST"].ToString().Trim() + ")";
                                ssView_Sheet1.Cells[nRow - 1, 14].BackColor = Color.Red;
                            }
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["ROUTDATE"].ToString().Trim();


                    SQL = "";
                    SQL = "SELECT IPDNO,TRSNO,PANO,BI,SNAME,TO_CHAR(INDATE,'YYYY/MM/DD') INDATE,TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE,";
                    SQL = SQL + ComNum.VBLF + "       GBSTS,FLAG,TO_CHAR(SIMSA_OK,'YY/MM/DD HH24:MI') SIMSA_OK,TO_CHAR(SIMSA_NO,'MM/DD HH24:MI') SIMSA_NO,";
                    SQL = SQL + ComNum.VBLF + "       SIMSA_SNAME , SIMSA_SABUN ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                    SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND Trsno = '" + dt.Rows[i]["TRSNO"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND GBSTS ='5' ";
                    SQL = SQL + ComNum.VBLF + "   AND SIMSA_SNAME IN ('김순옥','정희정','심경순','이민주','이향숙','김준수','우영란','김진숙','이현경')   ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC ";
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
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = dt1.Rows[0]["SIMSA_OK"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 16].BackColor = Color.Blue;
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["SIMSATIME"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;
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

        private void frmPmpaViewExamCondition_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtPano.Text = "";
            dtpOutDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            string strPano = "";
            long nIPDNO = 0;
            long nTRSNO = 0;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            strPano = ssView_Sheet1.Cells[e.Row, 0].Text;
            nIPDNO = (long)VB.Val(ssView_Sheet1.Cells[e.Row, 9].Text);
            nTRSNO = (long)VB.Val(ssView_Sheet1.Cells[e.Row, 10].Text);

            if (strPano != "" && nTRSNO > 0)
            {
                Simsa_History_Display(strPano, nIPDNO, nTRSNO);
            }
            else
            {
                ComFunc.MsgBox("환자를 선택 후 조회하세요.");
            }
        }

        //심사자 History
        void Simsa_History_Display(string argPano, long argIpdNo, long argTRSNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;

            ssHistory_Sheet1.RowCount = 0;
            ssHistory_Sheet1.RowCount = 1;

            try
            {
                SQL = "";
                SQL = "SELECT IPDNO,TRSNO,PANO,BI,SNAME,TO_CHAR(INDATE,'YYYY/MM/DD') INDATE,TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE,";
                SQL = SQL + ComNum.VBLF + "       GBSTS,FLAG,TO_CHAR(SIMSA_OK,'MM/DD HH24:MI') SIMSA_OK,TO_CHAR(SIMSA_NO,'MM/DD HH24:MI') SIMSA_NO,";
                SQL = SQL + ComNum.VBLF + "       SIMSA_SNAME , SIMSA_SABUN ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Trsno =" + argTRSNO + " ";
                SQL = SQL + ComNum.VBLF + " ORDER BY EntDate DESC ";

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
                ssHistory_Sheet1.RowCount = nRead;
                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        if (dt.Rows[i]["Simsa_OK"].ToString().Trim() != "")
                        {
                            ssHistory_Sheet1.Cells[i, 0].Text = "완료";
                            ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Simsa_OK"].ToString().Trim();
                            ssHistory_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        if (dt.Rows[i]["Simsa_NO"].ToString().Trim() != "")
                        {
                            ssHistory_Sheet1.Cells[i, 0].Text = "작업";
                            ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Simsa_NO"].ToString().Trim();
                            ssHistory_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        if (dt.Rows[i]["Flag"].ToString().Trim() == "Y")
                        {
                            ssHistory_Sheet1.Cells[i, 0].Text = "심사중";
                            ssHistory_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Simsa_OK"].ToString().Trim();
                            ssHistory_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 255, 128);
                        }

                        ssHistory_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Simsa_SName"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 3].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", dt.Rows[i]["GbSTS"].ToString().Trim());
                        ssHistory_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 7].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssHistory_Sheet1.Cells[i, 8].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    }
                }

                ssHistory_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
        }
    }
}
