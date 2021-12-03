using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewIpdSimSarCheck4.cs
    /// Description     : 등록번호별 퇴원자 심사조정 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs56.frm(FrmTongIpdSimSarCheck4.frm) >> frmPmpaViewIpdSimSarCheck4.cs 폼이름 재정의" />	
    public partial class frmPmpaViewIpdSimSarCheck4 : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();
        string GstrRetValue = "";

        public frmPmpaViewIpdSimSarCheck4()
        {
            InitializeComponent();
        }

        public frmPmpaViewIpdSimSarCheck4(string strRetValue)
        {
            InitializeComponent();
            GstrRetValue = strRetValue;
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
            string strInsu = "";
            bool PrePrint = true;

            strTitle = "등록번호별 퇴원자 심사조정액 조회";

            if (rdoInsu0.Checked == true)
            {
                strInsu = " [보험종류]: 건강보험,의료급여";
            }
            else if (rdoInsu1.Checked == true)
            {
                strInsu = " [보험종류]: 산재";
            }
            else if (rdoInsu2.Checked == true)
            {
                strInsu = " [보험종류]: 자보";
            }
            else
            {
                strInsu = " [보험종류]: 전체";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(strInsu, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
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

            if (txtPano.Text.Length != 8)
            {
                ComFunc.MsgBox("등록번호를 입력하세요");
                txtPano.Focus();
                return;
            }

            try
            {
                //개인별 청구 차액을 표시
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, PANO,";
                SQL = SQL + ComNum.VBLF + "       BI, SNAME, SUBI,";
                SQL = SQL + ComNum.VBLF + "       TOTAMT, SIMAMT, TOTAMT - SIMAMT TOTAMTA  ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "'";
                if (rdoInsu0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('1','2')"; //보험보호
                }
                else if (rdoInsu1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('3')";     //산재
                }
                else if (rdoInsu2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND SUBI IN ('4')";     //자보
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY OUTDATE DESC, ACTDATE DESC  ";

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
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["TOTAMTA"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["TOTAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SIMAMT"].ToString().Trim()).ToString("###,###,###,##0");
                }
                dt.Dispose();
                dt = null;
                ssView.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewIpdSimSarCheck4_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            lblSName.Text = "";

            if (GstrRetValue != "")
            {
                txtPano.Text = VB.Left(GstrRetValue, 8);
                switch (VB.Mid(GstrRetValue, 9, 2))
                {
                    case "31":
                        rdoInsu1.Checked = true;
                        break;
                    case "52":
                        rdoInsu2.Checked = true;
                        break;
                    default:
                        rdoInsu0.Checked = true;
                        break;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrRetValue = "";
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
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

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrRetValue = VB.Left(ssView_Sheet1.Cells[e.Row, 8].Text + VB.Space(10), 10);  //EDI접수일
            GstrRetValue += VB.Val(ssView_Sheet1.Cells[e.Row, 11].Text).ToString("0000000000");     //WRTNO
            GstrRetValue += VB.Val(ssView_Sheet1.Cells[e.Row, 4].Text).ToString("0000000000");      //조합부담

            if (GstrRetValue != "")
            {
                ComFunc.MsgBox(GstrRetValue);
            }

            this.Close();
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            lblSName.Text = "";
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            ssView.Enabled = false;
            if (txtPano.Text != "" && GstrRetValue != "")
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
                if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
                {
                    ComFunc.MsgBox(txtPano.Text + " - 등록되지 않은 등록번호입니다.");
                    txtPano.Focus();
                    return;
                }
                lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();

                Search_Data();

                GstrRetValue = "";
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
            if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
            {
                ComFunc.MsgBox(txtPano.Text + " - 등록되지 않은 등록번호입니다.");
                txtPano.Focus();
                return;
            }
            lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();
            btnSearch.Focus();
        }
    }
}
