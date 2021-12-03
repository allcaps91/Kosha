using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPanoMisu.cs
    /// Description     : 등록번호별 미수내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs73.frm(FrmPanoMisuView.frm) >> frmPmpaViewPanoMisu.cs 폼이름 재정의" />	
    public partial class frmPmpaViewPanoMisu : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();
        string GstrRetValue = "";

        public frmPmpaViewPanoMisu()
        {
            InitializeComponent();
        }

        public frmPmpaViewPanoMisu(string strRetValue)
        {
            InitializeComponent();
            GstrRetValue = strRetValue;
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
            int nRow = 0;
            double nAmt = 0;

            if (txtPano.Text.Length != 8)
            {
                ComFunc.MsgBox("등록번호를 입력하세요");
                txtPano.Focus();
                return;
            }

            btnSearch.Enabled = false;
            ssView.Enabled = true;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                //자료조회
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate, IpdOpd, TongGbn,";
                SQL = SQL + ComNum.VBLF + "       MirYYMM, Amt2, TO_CHAR(FromDate,'YYYYMMDD') FromDate,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(ToDate,'YYYYMMDD') ToDate, Remark, DeptCode,";
                SQL = SQL + ComNum.VBLF + "       ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND MisuID = '" + VB.Val(txtPano.Text).ToString("00000000") + "' ";
                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd='I' ";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd='O' ";
                }
                if (rdoInsu0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Class='05' ";
                }
                if (rdoInsu1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND Class='07' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY BDate DESC,IpdOpd ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
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
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["MirYYMM"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["TongGbn"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = nAmt.ToString("###,###,###,##0");   //미수금액
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["FromDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["ToDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                btnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewPanoMisu_Load(object sender, EventArgs e)
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
                if (VB.Right(GstrRetValue, 1) == "O")
                {
                    rdoIO1.Checked = true;
                }
                else
                {
                    rdoIO0.Checked = true;
                }
                txtPano.Text = VB.Left(GstrRetValue, 8);
                switch (VB.Mid(GstrRetValue, 9, 2))
                {
                    case "31":
                        rdoInsu0.Checked = true;
                        break;
                    case "52":
                        rdoInsu1.Checked = true;
                        break;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrRetValue = "";
            this.Close();
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

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrRetValue = VB.Left(ssView_Sheet1.Cells[e.Row, 0].Text + VB.Space(10), 10);  //발생일자

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
                    ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
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

            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

            if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
            {
                ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                txtPano.Focus();
                return;
            }
            lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();
            btnSearch.Focus();
        }
    }
}
