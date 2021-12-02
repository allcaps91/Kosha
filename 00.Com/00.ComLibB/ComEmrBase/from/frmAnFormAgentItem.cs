using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace ComEmrBase
{
    public partial class frmAnFormAgentItem : Form
    {
        public frmAnFormAgentItem()
        {
            InitializeComponent();
        }

        private void AnFormAgentItem_Load(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                PanItemsAdd.Visible = true;

                SsAgentItems.ActiveSheet.RowCount = 0;
                SsAgentItems.ActiveSheet.ColumnCount = 0;
                SsAgentItems.ActiveSheet.ColumnCount = 5;

                SsAgentItems.ActiveSheet.Columns[0].Width = 100;
                SsAgentItems.ActiveSheet.Columns[1].Width = 150;
                SsAgentItems.ActiveSheet.Columns[2].Width = 50;
                SsAgentItems.ActiveSheet.Columns[3].Width = 30;

                SsAgentItems.ActiveSheet.Columns[0].Locked = true;

                SsAgentItems.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "아이템코드";
                SsAgentItems.ActiveSheet.ColumnHeader.Cells[0, 1].Text = "아이템명";
                SsAgentItems.ActiveSheet.ColumnHeader.Cells[0, 2].Text = "단위";
                SsAgentItems.ActiveSheet.ColumnHeader.Cells[0, 3].Text = "순서";
                SsAgentItems.ActiveSheet.ColumnHeader.Cells[0, 4].Text = "제외";

                SsAgentItems.ActiveSheet.Columns[4].CellType = new CheckBoxCellType();
                SsAgentItems.ActiveSheet.Columns[4].VerticalAlignment = CellVerticalAlignment.Center;
                SsAgentItems.ActiveSheet.Columns[4].HorizontalAlignment = CellHorizontalAlignment.Center;

                SsItemsAdd.ActiveSheet.RowCount = 0;
                SsItemsAdd.ActiveSheet.ColumnCount = 0;
                SsItemsAdd.ActiveSheet.ColumnCount = 3;

                SsItemsAdd.ActiveSheet.Columns[0].Width = 100;
                SsItemsAdd.ActiveSheet.Columns[1].Width = 200;
                SsItemsAdd.ActiveSheet.Columns[2].Width = 50;

                SsItemsAdd.ActiveSheet.Columns[0].Locked = true;

                SsItemsAdd.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "아이템코드";
                SsItemsAdd.ActiveSheet.ColumnHeader.Cells[0, 1].Text = "아이템명";
                SsItemsAdd.ActiveSheet.ColumnHeader.Cells[0, 2].Text = "단위";

                SsAgentItems.ActiveSheet.RowCount = 0;

                SQL = "";
                SQL = "SELECT BASCD, BASNAME, BASEXNAME, DISSEQNO, USECLS";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRBASCD            ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'         ";
                SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록항목_Agent' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DISSEQNO                   ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                }

                SsAgentItems.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SsAgentItems.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    SsAgentItems.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                    SsAgentItems.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["BASEXNAME"].ToString().Trim();
                    SsAgentItems.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DISSEQNO"].ToString().Trim();
                    SsAgentItems.ActiveSheet.Cells[i, 4].Value = (dt.Rows[i]["USECLS"].ToString().Trim() == "1" ? true : false);
                }

                dt.Dispose();
                dt = null;

                TxtAgentSearch.Focus();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnAgentSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SsItemsAdd.ActiveSheet.RowCount = 0;

                if (TxtAgentSearch.Text.Trim().Empty())
                {
                    ComFunc.MsgBoxEx(this, "코드명 또는 코드가 없습니다.");
                    TxtAgentSearch.Focus();
                    return;
                }

                SQL = "SELECT ITEMNO, ITEMNAME, ITEMUNIT";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRITEM-- EMR 아이템관리                                 ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                                                             ";
                SQL = SQL + ComNum.VBLF + "   AND(ITEMINDEXNM  LIKE '%" + TxtAgentSearch.Text.Trim().ToUpper() + "%' OR ITEMNAME LIKE '%" + TxtAgentSearch.Text.Trim().ToUpper() + "%')    ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ITEMNAME                                                       ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                SsItemsAdd.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SsItemsAdd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ITEMNO"].ToString();
                    SsItemsAdd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ITEMNAME"].ToString();
                    SsItemsAdd.ActiveSheet.Cells[i, 2].Text = (dt.Rows[i]["ITEMUNIT"].ToString().Trim().NotEmpty() ? dt.Rows[i]["ITEMUNIT"].ToString().Trim() : "cc/hr");
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void TxtAgentSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            BtnAgentSearch.PerformClick();
        }

        private void SsItemsAdd_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (SsItemsAdd.ActiveSheet.RowCount == 0)
                return;

            if (e.ColumnHeader == true)
                return;

            if (e.Column != 0)
                return;


            for (int i = 0; i < SsAgentItems.ActiveSheet.RowCount; i++)
            {
                if (SsAgentItems.ActiveSheet.Cells[i, 0].Text.Trim() == SsItemsAdd.ActiveSheet.Cells[e.Row, 0].Text.Trim())
                {
                    ComFunc.MsgBoxEx(this, "이미 설정 또는 선택된 항목입니다.");
                    return;
                }
            }

            SsAgentItems.ActiveSheet.RowCount = SsAgentItems.ActiveSheet.RowCount + 1;

            SsAgentItems.ActiveSheet.Cells[SsAgentItems.ActiveSheet.RowCount - 1, 0].Text = SsItemsAdd.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            SsAgentItems.ActiveSheet.Cells[SsAgentItems.ActiveSheet.RowCount - 1, 1].Text = SsItemsAdd.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            SsAgentItems.ActiveSheet.Cells[SsAgentItems.ActiveSheet.RowCount - 1, 2].Text = SsItemsAdd.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            SsAgentItems.ActiveSheet.Cells[SsAgentItems.ActiveSheet.RowCount - 1, 3].Text = SsAgentItems.ActiveSheet.RowCount.ToString();
            SsAgentItems.ActiveSheet.Cells[SsAgentItems.ActiveSheet.RowCount - 1, 4].Value = false;
        }

        private void BtnAgentClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAgentAdd_Click(object sender, EventArgs e)
        {
            BtnAgentClose.Enabled = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string BASCD = string.Empty;
            string BASNAME = string.Empty;
            string BASEXNAME = string.Empty;
            string DISSEQNO = string.Empty;
            bool USECLS = false;

            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < SsAgentItems.ActiveSheet.RowCount; i++)
                {

                    BASCD = SsAgentItems.ActiveSheet.Cells[i, 0].Text.Trim();
                    BASNAME = SsAgentItems.ActiveSheet.Cells[i, 1].Text.Trim();
                    BASEXNAME = SsAgentItems.ActiveSheet.Cells[i, 2].Text.Trim();
                    DISSEQNO = SsAgentItems.ActiveSheet.Cells[i, 3].Text.Trim();
                    USECLS = Convert.ToBoolean(SsAgentItems.ActiveSheet.Cells[i, 4].Value);

                    SQL = "";
                    SQL = "MERGE INTO KOSMOS_EMR.AEMRBASCD M";
                    SQL = SQL + ComNum.VBLF + "USING DUAL                                                                                        ";
                    SQL = SQL + ComNum.VBLF + "   ON(M.BSNSCLS = '기록지관리' AND M.UNITCLS = '마취기록항목_Agent' AND M.BASCD = '" + BASCD + "')  ";
                    SQL = SQL + ComNum.VBLF + "WHEN MATCHED THEN                                                                                 ";
                    SQL = SQL + ComNum.VBLF + "    UPDATE SET M.BASNAME = '" + BASNAME + "'                                                      ";
                    SQL = SQL + ComNum.VBLF + "             , M.BASEXNAME = '" + BASEXNAME + "'                                                  ";
                    SQL = SQL + ComNum.VBLF + "             , M.USECLS = '" + (USECLS == true ? "1" : "0") + "'                                  ";
                    SQL = SQL + ComNum.VBLF + "             , DISSEQNO = " + DISSEQNO.ToString() + "                                             ";
                    SQL = SQL + ComNum.VBLF + "WHEN NOT MATCHED THEN                                                                             ";
                    SQL = SQL + ComNum.VBLF + "    INSERT(M.BSNSCLS, M.UNITCLS, M.BASCD, M.BASNAME, M.BASEXNAME, M.APLFRDATE, M.APLENDDATE, M.USECLS, M.DISSEQNO)        ";
                    SQL = SQL + ComNum.VBLF + "    VALUES('기록지관리', '마취기록항목_Agent', '" + BASCD + "', '" + BASNAME + "', '" + BASEXNAME + "', '20191217', '20991231', '" + (USECLS == true ? "1" : "0") + "', " + DISSEQNO.ToString() + ")                  ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                this.Close();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }



        }
    }
}
