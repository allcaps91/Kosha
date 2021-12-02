using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmPaVIEWSpecialJewoncar.cs
    /// Description     : 조합별 년간통계 조회
    /// Author          : 김효성
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\IPD\iument\iument.vbp\Frm재원자차량조회(Frm재원자차량조회.FRM)  >> frmPmPaVIEWSpecialJewoncar.cs 폼이름 재정의" />	

    public partial class frmPmPaVIEWSpecialJewoncar : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaVIEWSpecialJewoncar()
        {
            InitializeComponent();
        }

        private void frmPmPaVIEWSpecialJewoncar_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            dtpFDate.Value.AddDays(-15);
            dtpTdate.Value = Convert.ToDateTime(strDTP);
            txtcar.Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nRow = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Sname,IPDNO,DEPTCODE,WARDCODE,ROOMCODE,CAR,     ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') INDATE,                  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(OUTDate,'YYYY-MM-DD') OUTDATE                 ";
                SQL = SQL + ComNum.VBLF + "  From " + ComNum.DB_PMPA + "IPD_CAR                                         ";
                SQL = SQL + ComNum.VBLF + " Where 1=1";
                SQL = SQL + ComNum.VBLF + "   AND InDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND InDate < TO_DATE('" + dtpTdate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (txtPano.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "' ";
                }
                if (txtcar.Text != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND CAR LIKE '%" + txtcar.Text + "%' ";
                }
                if (chkall.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND OUTDate IS NOT NULL ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["CAR"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = Convert.ToInt32(txtPano).ToString("00000000");
            }
        }
    }
}
