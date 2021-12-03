using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmPanoSearch.cs
    /// Description     : 등록번호 및 퇴원일자 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정, 권한 확인하는 부분 추가
    /// <history>       
    /// D:\타병원\PSMHH\mid\midout\midout06.frm(FrmPanoSearch) => frmPanoSearch.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\mid\midout\midout06.frm(FrmPanoSearch)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\mid\midout\midout.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    /// </summary>
    public partial class frmPanoSearch : Form
    {
        public delegate void SetPanoTDate(string strPano, string strTDate);
        public event SetPanoTDate rSetPanoTDate;

        public frmPanoSearch()
        {
            InitializeComponent();
        }

        void frmPanoSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optPano.Checked = true;
            txtData.Text = "";
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            if (optPano.Checked == true)
            {
                txtData.Text = txtData.Text.PadLeft(8, '0');
                GetData();
            }
            else if (optName.Checked == true)
            {
                GetData();
            }
        }

        void GetData()
        {
            int i = 0;

            string strODate = "";
            string strIDate = "";
            string strTdept = "";
            string strName = "";
            string strPano = "";
            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;
            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    Pano, SName, Tdept,TO_CHAR(InDate,'YYYY-MM-DD') IDate,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(OutDate,'YYYY-MM-DD') ODate, CANC  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MID_SUMMARY";
                if (optPano.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + txtData.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY OutDate DESC";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE Sname LIKE '%" + txtData.Text.Trim() + "%' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Pano,OutDate DESC";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strName = dt.Rows[i]["SName"].ToString().Trim();
                    strTdept = dt.Rows[i]["TDept"].ToString().Trim();
                    strODate = dt.Rows[i]["ODate"].ToString().Trim();
                    strIDate = dt.Rows[i]["IDate"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 0].Text = strPano;
                    ssView_Sheet1.Cells[i, 1].Text = strName;
                    ssView_Sheet1.Cells[i, 2].Text = strTdept;
                    ssView_Sheet1.Cells[i, 3].Text = strIDate;
                    ssView_Sheet1.Cells[i, 4].Text = strODate;
                    switch (dt.Rows[i]["CANC"].ToString().Trim())
                    {
                        case "2":
                        case "3":
                        case "6":
                            ssView_Sheet1.Cells[i, 4].BackColor = Color.Pink;
                            break;
                    }
                    
                }

                dt.Dispose();
                dt = null;

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }

        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            string strPano = "";
            string strTDate = "";

            strPano = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            strTDate = ssView_Sheet1.Cells[e.Row, 4].Text.Trim();

            rSetPanoTDate(strPano, strTDate);

            this.Close();
        }

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (optPano.Checked == true)
                {
                    txtData.Text = txtData.Text.PadLeft(8, '0');
                    GetData();
                }
                else if (optName.Checked == true)
                {
                    GetData();
                }
            }
        }

        private void optCH_CheckedChanged(object sender, EventArgs e)
        {
            txtData.Text = "";
            ssView_Sheet1.RowCount = 0;
        }
    }
}
