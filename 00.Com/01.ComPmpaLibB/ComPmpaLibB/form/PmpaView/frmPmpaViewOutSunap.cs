using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOutSunap.cs
    /// Description     : 퇴원수납내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm퇴원수납내역.FRM(Frm퇴원수납내역.frm) >> frmPmpaViewOutSunap.cs 폼이름 재정의" />	
    public partial class frmPmpaViewOutSunap : Form
    {
        string GstrSysDate = "";

        public frmPmpaViewOutSunap()
        {
            InitializeComponent();
        }

        private void frmPmpaViewOutSunap_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            GstrSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFDate.Value = Convert.ToDateTime(GstrSysDate);
            dtpTDate.Value = Convert.ToDateTime(GstrSysDate);

            cboGubun.Items.Clear();
            cboGubun.Items.Add("전체"); //ALL
            cboGubun.Items.Add("보험(전체)");//11-15, 41-45
            cboGubun.Items.Add("보험(공단)");//11
            cboGubun.Items.Add("보험(직장)");//12
            cboGubun.Items.Add("보험(지역)");//13
            cboGubun.Items.Add("의료급여");//21-25
            cboGubun.Items.Add("산재"); //31
            cboGubun.Items.Add("공상"); //32
            cboGubun.Items.Add("산재공상");//33
            cboGubun.Items.Add("보험계약");//45
            cboGubun.Items.Add("일반"); //51,54
            cboGubun.Items.Add("TA보험"); //52
            cboGubun.Items.Add("일반계약");//53
            cboGubun.Items.Add("TA일반"); //55
            cboGubun.SelectedIndex = 0;

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

            strTitle = "신 자 감 액 명 단";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록기간 : ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("증빙서류 : ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;

            if (dtpFDate.Value > Convert.ToDateTime(GstrSysDate))
            {
                ComFunc.MsgBox("작업일자가 오늘보다 이후입니다.");
                dtpFDate.Value = Convert.ToDateTime(GstrSysDate);
                dtpFDate.Focus();
                return;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.Pano, b.SName, a.Bi,";
                SQL = SQL + ComNum.VBLF + "       a.DeptCode, a.DrCode, c.DrName,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, a.Ilsu,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate, a.GbIPD, a.GbGamek,";
                SQL = SQL + ComNum.VBLF + "       a.SangAmt, a.Amt50, a.Amt51,";
                SQL = SQL + ComNum.VBLF + "       a.Amt52, a.Amt53, a.Amt54,";
                SQL = SQL + ComNum.VBLF + "       a.Amt55, a.Amt56, a.Amt57,";
                SQL = SQL + ComNum.VBLF + "       a.Amt58";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS a, " + ComNum.DB_PMPA + "BAS_PATIENT b, " + ComNum.DB_PMPA + "BAS_DOCTOR c";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND a.ActDate>=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.ActDate<=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND a.Amt50 <> 0 ";
                //환자구분
                switch (cboGubun.Text.Trim())
                {
                    case "전체":
                        ;
                        break;
                    case "보험(전체)":
                        SQL = SQL + ComNum.VBLF + " AND ((a.Bi>='11' AND a.Bi<='15') OR (a.Bi>='41' AND a.Bi<='45'))  ";
                        break;
                    case "보험(공단)":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '11' ";
                        break;
                    case "보험(직장)":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '12' ";
                        break;
                    case "보험(지역)":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '13' ";
                        break;
                    case "의료급여":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi>='21' AND a.Bi<='25' ";
                        break;
                    case "산재":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '31' ";
                        break;
                    case "공상":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '32' ";
                        break;
                    case "산재공상":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '33' ";
                        break;
                    case "보험계약":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '45' ";
                        break;
                    case "일반":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi IN ('51','54) ";
                        break;
                    case "TA보험":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '52' ";
                        break;
                    case "일반계약":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '53' ";
                        break;
                    case "TA일반":
                        SQL = SQL + ComNum.VBLF + " AND a.Bi = '55' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + "  AND a.Pano=b.Pano(+)";
                SQL = SQL + ComNum.VBLF + "  AND a.DrCode=c.DrCode(+)";
                if (rdoSort0.Checked == true) //성명순
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY ActDate,b.SName,a.Pano ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY ActDate,a.DeptCode,a.DrCode ";
                }

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
                nRow = 0;

                for (i = 0; i < nRead; i++)
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Ilsu"].ToString().Trim();

                    ssView_Sheet1.Cells[nRow - 1, 9].Text = "";
                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = "◎";
                    }

                    ssView_Sheet1.Cells[nRow - 1, 10].Text = "";
                    if (dt.Rows[i]["SangAmt"].ToString().Trim() != "0")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = "◎";
                    }

                    if (dt.Rows[i]["GbGamek"].ToString().Trim() != "00")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbGamek"].ToString().Trim();
                    }

                    ssView_Sheet1.Cells[nRow - 1, 12].Text = VB.Val(dt.Rows[i]["Amt50"].ToString().Trim()).ToString("#,##0");   //총진료비
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Val(dt.Rows[i]["Amt53"].ToString().Trim()).ToString("#,##0");   //조합부담
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt.Rows[i]["Amt54"].ToString().Trim()).ToString("#,##0");   //감액
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = VB.Val(dt.Rows[i]["Amt51"].ToString().Trim()).ToString("#,##0");   //중간납대체
                    ssView_Sheet1.Cells[nRow - 1, 16].Text = VB.Val(dt.Rows[i]["Amt56"].ToString().Trim()).ToString("#,##0");   //개인미수

                    if (VB.Val(dt.Rows[i]["Amt58"].ToString().Trim()) == 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = VB.Val(dt.Rows[i]["Amt57"].ToString().Trim()).ToString("#,##0");   //퇴원금
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 17].Text = (VB.Val(dt.Rows[i]["Amt58"].ToString().Trim()) * (-1)).ToString("#,##0");   //환불금
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


    }
}
