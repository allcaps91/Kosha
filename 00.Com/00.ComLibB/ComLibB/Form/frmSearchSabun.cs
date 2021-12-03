using FarPoint.Win.Spread;
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

/// <summary>
/// Description : 사원번호 조회
/// Author : 김형범
/// Create Date : 2017.06.19
/// </summary>
/// <history>
/// 완료
/// 2017-07-03 dt1 정보가 없을때 return 하는 문제 수정 -> null 값으로 뿌리도록 - 박성완
/// </history>
namespace ComLibB
{
    /// <summary> 사원번호 찾기 </summary>
    public partial class frmSearchSabun : Form
    {
        public delegate void GetSabunData(string strSabun);
        public event GetSabunData rGetSabunData;

        public delegate void EventClose();
        public event EventClose rEventClose;

        /// <summary> 사원번호 찾기 </summary>
        public frmSearchSabun()
        {
            InitializeComponent();
        }

        void frmSearchSabun_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtFind.Text = "";
            ssClear(ssView);
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            rEventClose();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int intRow = 0;

            string strBuseName = "";
            string strJikName = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            try
            {
                if (txtFind.Text.Trim() == "")
                {
                    ComFunc.MsgBox("찾으실 이름이 공란입니다.", "오류");
                    txtFind.Focus();
                    return;
                }

                ssView_Sheet1.RowCount = 20;
                ssClear(ssView);

                SQL = "";
                SQL = "SELECT Sabun,KorName,Buse,Jik,TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(ToiDay,'YYYY-MM-DD') ToiDay,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(BalDay,'YYYY-MM-DD') BalDay ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + "WHERE KorName LIKE '" + txtFind.Text.Trim() + "%' ";

                if (chkToi.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND ToiDay IS NULL ";
                }

                if (optSabun.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Sabun ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY KorName,Sabun ";
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

                intRow = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (intRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = intRow;
                    }

                    ssView_Sheet1.Cells[intRow, 0].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    ssView_Sheet1.Cells[intRow, 1].Text = dt.Rows[i]["KorName"].ToString().Trim();

                    strBuseName = "";
                    strJikName = "";

                    SQL = "";
                    SQL = "SELECT Name FROM BAS_BUSE ";
                    SQL = SQL + ComNum.VBLF + "WHERE BuCode = '" + dt.Rows[i]["Buse"].ToString().Trim() + "' ";


                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (dt1.Rows.Count == 0)
                    {
                        strBuseName = "";
                    }
                    else
                    {
                        strBuseName = dt1.Rows[0]["Name"].ToString().Trim();
                    }
                    dt1.Dispose();
                    dt1 = null;


                    SQL = "";
                    SQL = "SELECT Name FROM " + ComNum.DB_ERP + "INSA_CODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE Gubun = '2' ";
                    SQL = SQL + ComNum.VBLF + "  AND Code = '" + dt.Rows[i]["Jik"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if ( dt1.Rows.Count == 0)
                    {
                        strJikName = "";
                    }
                    else
                    {
                        strJikName = dt1.Rows[0]["Name"].ToString().Trim() as string;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    ssView_Sheet1.Cells[intRow, 2].Text = strBuseName as string;
                    ssView_Sheet1.Cells[intRow, 3].Text = strJikName as string;
                    ssView_Sheet1.Cells[intRow, 4].Text = dt.Rows[i]["IpsaDay"].ToString().Trim() as string;
                    ssView_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["ToiDay"].ToString().Trim() as string;
                    ssView_Sheet1.Cells[intRow, 6].Text = dt.Rows[i]["BalDay"].ToString().Trim() as string;

                    intRow = intRow + 1;
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = intRow;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Spread Clear
        /// </summary>
        /// <param name="SpreadName"></param>
        public void ssClear(FpSpread SpreadName)
        {
            SpreadName.ActiveSheet.RowCount = 1;
            SpreadName.ActiveSheet.Cells[0, 0, SpreadName.ActiveSheet.Rows.Count - 1, SpreadName.ActiveSheet.ColumnCount - 1].Text = "";
            SpreadName.ActiveSheet.SetActiveCell(0, 0);
        }

        void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
            
            rGetSabunData(ssView_Sheet1.Cells[e.Row, 0].Text.Trim());
        }

        void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
