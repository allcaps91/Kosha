using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public partial class frmMcrtJobViewFav : Form
    {
        //Messgae Send
        public delegate void SendMsg(string strMsg);
        public event SendMsg rSendMsg;
        
        string strMcrt = ""; //진단서 종류  GstrMC
        string strFav = ""; //상용구분류    GstrFAV

        public frmMcrtJobViewFav()
        {
            InitializeComponent();
        }

        public frmMcrtJobViewFav(string strMcrt , string strFav)
        {
            InitializeComponent();
            this.strMcrt = strMcrt;
            this.strFav = strFav;
        }

        private void frmMcrtJobViewFav_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssFav_Sheet1.RowCount = 0;
            txtFavText.Text = "";

            /// 사용자별 폼의 저장(C), 조회(R), 수정(U), 삭제(D), 출력(P)
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }
            GetDataList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssFav_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtFavText.Text = ssFav_Sheet1.Cells[e.Row, 1].Tag.ToString();
        }

        private void ssFav_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (txtFavText.Text.Trim() == "")
            {
                ComFunc.MsgBox("선택된 상용구가 없습니다.");
                return;
            }
            rSendMsg(txtFavText.Text.Trim());
            Close();
        }

        private void ssFav_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                strFav = txtFavText.Text;
                Close();
            }
        }

        private void GetDataList()
        {
            string SQL = "";
            string SqlErr = "";
            int i = 0;
            DataTable dt = null;

            ssFav_Sheet1.RowCount = 0;
            try
            {

                SQL = " SELECT  TITLENAME ";
                SQL = SQL + ComNum.VBLF +  " FROM " + ComNum.DB_MED + "OCS_MCTITLE ";
                SQL = SQL + ComNum.VBLF + " WHERE  (MCCLASS = '" + strMcrt + "'  AND  FAVCLASS = '00') ";
                SQL = SQL + ComNum.VBLF + " OR     (MCCLASS = '" + strMcrt + "'  AND  FAVCLASS = '" + strFav + "') ";
                SQL = SQL + ComNum.VBLF + " AND     RowNum = 2 ";
                SQL = SQL + ComNum.VBLF + " ORDER   BY  FAVCLASS ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }
 
                //if(dt.Rows.Count == 2)
                //{
                //    lblTitleSub0.Text = "상용구도움말 : " + dt.Rows[0]["TITLENAME"].ToString().Trim();
                //    lblTitleSub0.Text = lblTitleSub0.Text + " - [" + dt.Rows[1]["TITLENAME"] + "]";
                //}

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT  FAVSEQ, FAVTITLE, FAVTEXT ";
                SQL = SQL + ComNum.VBLF + " FROM    " + ComNum.DB_MED + "OCS_MCFAV";
                SQL = SQL + ComNum.VBLF + " WHERE   MCCLASS  = '" + strMcrt + "' ";
                SQL = SQL + ComNum.VBLF + " AND     FAVCLASS = '" + strFav + "' ";
                //TODO
                SQL = SQL + ComNum.VBLF + " AND     DRSABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' ";
                //SQL = SQL + ComNum.VBLF + " AND     DRSABUN = '35679'";

                SQL = SQL + ComNum.VBLF + " ORDER   BY FAVSEQ ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("등록된 상용구가 없습니다.", "상용구 검색");
                    Close();
                    return;
                }

                ssFav_Sheet1.RowCount = dt.Rows.Count;
                ssFav_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssFav_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FAVSEQ"].ToString().Trim() + ". " + dt.Rows[i]["FAVTITLE"].ToString().Trim();
                    ssFav_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FAVTEXT"].ToString();
                    ssFav_Sheet1.Cells[i, 1].Tag = dt.Rows[i]["FAVTEXT"].ToString();
                }

                dt.Dispose();
                dt = null;
                ssFav_Sheet1.ActiveRowIndex = 0;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
            }
        }

    }
}
