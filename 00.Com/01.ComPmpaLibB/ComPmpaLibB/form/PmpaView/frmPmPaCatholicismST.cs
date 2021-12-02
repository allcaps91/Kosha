using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaCatholicismST
    /// Description     : 카톨릭 환자 입원자 명단
    /// Author          : 김효성
    /// Create Date     : 2017-08-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iviewa\iviewa.vbp(IVIEWAA.FRM) >> frmPmPaCatholicismST.cs 폼이름 재정의" />	


    public partial class frmPmPaCatholicismST : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaCatholicismST()
        {
            InitializeComponent();
        }

        private void frmPmPaCatholicismST_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFdate.Value = DateTime.Parse(strDTP).AddDays(-1);
            dtpTdate.Value = DateTime.Parse(strDTP);

            btnCancel.Enabled = false;
            btnPrint.Enabled = false;

        }

        private void btnView_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nRow = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;
            btnView.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT WardCode,RoomCode,Pano,Sname,Sex,Age,                                  ";
                SQL = SQL + ComNum.VBLF + "        DeptCode,Bi,TO_CHAR(InDate,'YYYY-MM-DD') InDate                        ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_Master                                                             ";
                SQL = SQL + ComNum.VBLF + "  WHERE InDate >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')                ";
                SQL = SQL + ComNum.VBLF + "    AND InDate <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI')  ";
                SQL = SQL + ComNum.VBLF + "    AND Religion = '1'                                                         ";
                SQL = SQL + ComNum.VBLF + "    AND GBSTS IN ('0','2')                                                          ";
                SQL = SQL + ComNum.VBLF + "    AND OUTDATE IS NULL                                                          ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY WardCode,RoomCode                                                   ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                nRow = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();

                    //성별을 Display

                    if (dt.Rows[i]["SEX"].ToString().Trim() == "M")
                    {
                        ssView_Sheet1.Cells[i, 4].Text = "남";
                    }

                    if (dt.Rows[i]["SEX"].ToString().Trim() != "M")
                    {
                        ssView_Sheet1.Cells[i, 4].Text = "여";
                    }

                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();

                    //진료과명칭
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT DeptNameK ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                    SQL = SQL + ComNum.VBLF + "WHERE DeptCode = '" + dt.Rows[i]["DeptCode"] + "' ";

                    SQL = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt2.Rows.Count == 1)
                    {
                        ssView_Sheet1.Cells[i, 6].Text = dt2.Rows[0]["DeptNamek"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["InDate"].ToString().Trim();

                    dt2.Dispose();
                    dt2 = null;

                    //환자종류
                    switch (dt.Rows[i]["BI"].ToString().Trim())
                    {
                        case "11":
                            ssView_Sheet1.Cells[i, 8].Text = "공무원";
                            break;
                        case "12":
                            ssView_Sheet1.Cells[i, 8].Text = "직장";
                            break;
                        case "13":
                            ssView_Sheet1.Cells[i, 8].Text = "지역";
                            break;
                        case "21":
                            ssView_Sheet1.Cells[i, 8].Text = "보호1종";
                            break;
                        case "22":
                            ssView_Sheet1.Cells[i, 8].Text = "보호2종";
                            break;
                        case "23":
                            ssView_Sheet1.Cells[i, 8].Text = "보호3종";
                            break;
                        case "24":
                            ssView_Sheet1.Cells[i, 8].Text = "행려환자";
                            break;
                        case "31":
                            ssView_Sheet1.Cells[i, 8].Text = "산재";
                            break;
                        case "32":
                            ssView_Sheet1.Cells[i, 8].Text = "산재공상";
                            break;
                        case "41":
                            ssView_Sheet1.Cells[i, 8].Text = "공단100%";
                            break;
                        case "42":
                            ssView_Sheet1.Cells[i, 8].Text = "직장100%";
                            break;
                        case "43":
                            ssView_Sheet1.Cells[i, 8].Text = "지역100%";
                            break;
                        case "44":
                            ssView_Sheet1.Cells[i, 8].Text = "가족계획";
                            break;
                        case "51":
                            ssView_Sheet1.Cells[i, 8].Text = "일반";
                            break;
                        case "52":
                            ssView_Sheet1.Cells[i, 8].Text = "자보";
                            break;
                        case "53":
                            ssView_Sheet1.Cells[i, 8].Text = "계약처";
                            break;
                        case "54":
                            ssView_Sheet1.Cells[i, 8].Text = "미확인";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 8].Text = "기타";
                            break;
                    }
                }
                dt.Dispose();
                dt = null;

                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;

            }
            catch (Exception ex)
            {
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "카톨릭 환자 입원자 명단" + "/f1/n";
            strHead2 = "/n/l/f2" + "작업기간 : " + dtpTdate.Value.ToString("yyyy-MM-dd");
            strHead2 = strHead2 + "/n" + "인쇄 일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A") + VB.Space(20) + "PAGE :" + "/p";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            btnView.Enabled = true;
            btnCancel.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
            ssView_Sheet1.RowCount = 30;
            ssView_Sheet1.RowCount = 0;
        }
    }
}
