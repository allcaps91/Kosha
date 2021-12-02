using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\nurse\nrstd\nrstd.vbp\Frm교육관리조회4.frm >> frmEducationDetaillList.cs 폼이름 재정의" />

    public partial class frmEducationDetaillList : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmEducationDetaillList()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEducationDetaillList_Load(object sender, EventArgs e)
        {
            SS_Display(clsPublic.GstrRetValue);
        }

        private void SS_Display(string ArgSabun)
        {
            int i = 0;
            double nSum = 0;
            string strSabun = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            strSabun = VB.Val(VB.Pstr(ArgSabun, "^^", 1)).ToString();
            nSum = 0;

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 1;

            Cursor.Current = Cursors.WaitCursor;

            strSabun = "40848";

            try
            {
                SQL = "";
                SQL = " SELECT WRTNO,SABUN,SNAME,BUCODE,IPSADATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate, ";
                SQL = SQL + ComNum.VBLF + " JIKJONG,EDUJONG,EDUNAME, ";
                SQL = SQL + ComNum.VBLF + "    OptTime,EDUTIME,MAN, OptPlace,PLACE,JUMSU,GUBUN,ENTDATE,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM NUR_EDU_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE (TRIM(Sabun) ='" + VB.Val(strSabun).ToString("00000") + "' OR TRIM(SABUN) = '" + strSabun + "') ";
                SQL = SQL + ComNum.VBLF + "  AND FRDATE >= TO_DATE('" + (VB.Pstr(ArgSabun, "^^", 2)) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND FRDATE <= TO_DATE('" + (VB.Pstr(ArgSabun, "^^", 3)) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ((GUBUN = '2' AND SIGN = '1') OR GUBUN = '1')";
                SQL = SQL + ComNum.VBLF + " Order By FrDate ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                    //'개인정보읽기
                    SQL = "";
                    SQL = "SELECT c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.INSA_MST a,";
                    SQL = SQL + ComNum.VBLF + "      KOSMOS_PMPA.BAS_BUSE b,";
                    SQL = SQL + ComNum.VBLF + "      KOSMOS_ADM.INSA_CODE c ";
                    SQL = SQL + ComNum.VBLF + "WHERE  a.IpsaDay<=TO_DATE('" + strDTP + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + strDTP + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.Sabun  ='" + VB.Val(dt.Rows[i]["sabun"].ToString().Trim()).ToString("0000") + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.Buse=b.BuCode(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND a.Jik=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND c.Gubun='2' ";// '직책
                    SQL = SQL + ComNum.VBLF + "ORDER BY c.Code, a.Sabun, a.KorName ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        SS1_Sheet1.Cells[i, 2].Text = dt1.Rows[0]["BuseName"].ToString().Trim();

                        switch (dt1.Rows[0]["EDUJONG"].ToString().Trim())
                        {
                            case "1":
                                SS1_Sheet1.Cells[i, 3].Text = "병동";
                                break;
                            case "2":
                                SS1_Sheet1.Cells[i, 3].Text = "감염";
                                break;
                            case "3":
                                SS1_Sheet1.Cells[i, 3].Text = "QI";
                                break;
                            case "4":
                                SS1_Sheet1.Cells[i, 3].Text = "CS";
                                break;
                            case "5":
                                SS1_Sheet1.Cells[i, 3].Text = "CPR";
                                break;
                            case "6":
                                SS1_Sheet1.Cells[i, 3].Text = "학술";
                                break;
                            case "7":
                                SS1_Sheet1.Cells[i, 3].Text = "직무";
                                break;
                            case "8":
                                SS1_Sheet1.Cells[i, 3].Text = "전직원";
                                break;
                            case "9":
                                SS1_Sheet1.Cells[i, 3].Text = "특강";
                                break;
                            case "10":
                                SS1_Sheet1.Cells[i, 3].Text = "연수";
                                break;
                            case "11":
                                SS1_Sheet1.Cells[i, 3].Text = "10대";
                                break;
                            case "12":
                                SS1_Sheet1.Cells[i, 3].Text = "보수";
                                break;
                            case "13":
                                SS1_Sheet1.Cells[i, 3].Text = "Report";
                                break;
                            case "14":
                                SS1_Sheet1.Cells[i, 3].Text = "강사";
                                break;
                            case "15":
                                SS1_Sheet1.Cells[i, 3].Text = "프리셉터";
                                break;
                            case "16":
                                SS1_Sheet1.Cells[i, 3].Text = "Cyber";
                                break;
                            case "17":
                                SS1_Sheet1.Cells[i, 3].Text = "승진자";
                                break;
                            default:
                                SS1_Sheet1.Cells[i, 3].Text = "기타";
                                break;
                        }
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["EduName"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Jumsu"].ToString().Trim();
                        nSum = nSum + VB.Val(dt.Rows[i]["Jumsu"].ToString().Trim());

                        switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                        {
                            case "1":
                                SS1_Sheet1.Cells[i, 6].Text = "개인";
                                break;
                            case "2":
                                SS1_Sheet1.Cells[i, 6].Text = "전체";
                                break;
                        }
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["FrDate"].ToString().Trim();

                        if (dt.Rows[i]["ToDate"].ToString().Trim() != "")
                        {
                            SS1_Sheet1.Cells[i, 7].Text = SS1_Sheet1.Cells[i, 7].Text + "~" + dt.Rows[i]["ToDate"].ToString().Trim();
                        }

                        switch (dt.Rows[i]["OptTime"].ToString().Trim())
                        {
                            case "0":
                                SS1_Sheet1.Cells[i, 8].Text = "10분";
                                break;
                            case "1":
                                SS1_Sheet1.Cells[i, 8].Text = "30분내";
                                break;
                            case "2":
                                SS1_Sheet1.Cells[i, 8].Text = "1시간내";
                                break;
                            case "3":
                                SS1_Sheet1.Cells[i, 8].Text = "90분";
                                break;
                            case "4":
                                SS1_Sheet1.Cells[i, 8].Text = "2시간";
                                break;
                        }

                        if (dt.Rows[i]["EDUTime"].ToString().Trim() != "")
                        {
                            SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["EDUTime"].ToString().Trim();
                        }

                        switch (dt.Rows[i]["OptPlace"].ToString().Trim())
                        {
                            case "0":
                                SS1_Sheet1.Cells[i, 9].Text = "마리아홀";
                                break;
                            case "1":
                                SS1_Sheet1.Cells[i, 9].Text = "466호실";
                                break;
                        }

                        if (dt.Rows[i]["Place"].ToString().Trim() != "")
                        {
                            SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Place"].ToString().Trim();
                        }
                        SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Men"].ToString().Trim();

                    }

                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 4].Text = "합계";
                    SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 5].Text = nSum.ToString();



                    dt1.Dispose();
                    dt1 = null;
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

        }

        private void SS1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            clsPublic.GstrHelpCode = "";

            clsPublic.GstrHelpCode = SS1_Sheet1.Cells[e.Row, 0].Text;

            //TODO
            //Frm교육관리등록2.Show 1

            clsPublic.GstrHelpCode = "";
        }
    }
}
