using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread.CellType;

namespace ComEmrBase
{
    public partial class frmEmrSugaLink : Form
    {
        string SelectFormNo = string.Empty;

        public frmEmrSugaLink()
        {
            InitializeComponent();
        }

        private void frmEmrSugaLink_Load(object sender, EventArgs e)
        {
            GetFormList();
        }

        #region 버튼
        private void btnSearchItem_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData())
            {
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                GetItemList(SelectFormNo);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DelData())
            {
                ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                GetItemList(SelectFormNo);
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region 함수
        private bool SaveData()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAdd = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = "DELETE ADMIN.AEMRSUGAMAPPING";
                SQL += ComNum.VBLF + "WHERE FORMNO = " + SelectFormNo;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAdd, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return rtnVal;
                }

                for(int i = 0; i < ssItem_Sheet1.RowCount; i++)
                {
                    if (ssItem_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                    {
                        continue;
                    }


                    string ItemCd = ssItem_Sheet1.Cells[i, 3].Text.Trim();
                    string ORDERGBN = ssItem_Sheet1.Cells[i, 5].Text.Trim().Length >= 3 ? ssItem_Sheet1.Cells[i, 5].Text.Trim().Substring(0, 3) : "";
                    string APPOINTMENT = ssItem_Sheet1.Cells[i, 6].Text.Trim().Length >= 3 ? ssItem_Sheet1.Cells[i, 6].Text.Trim().Substring(0, 3) : "";

                    if (APPOINTMENT.IsNullOrEmpty() || ORDERGBN.IsNullOrEmpty())
                        continue;

                    SQL = "INSERT INTO ADMIN.AEMRSUGAMAPPING";
                    SQL += ComNum.VBLF + "(FORMNO, ITEMCD, ORDERGBN, APPOINTMENT)";
                    SQL += ComNum.VBLF + "VALUES";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "    '" + SelectFormNo + "'";
                    SQL += ComNum.VBLF + " ,  '" + ItemCd +"'";
                    SQL += ComNum.VBLF + " ,  '" + ORDERGBN + "'";
                    SQL += ComNum.VBLF + " ,  '" + APPOINTMENT + "'";
                    SQL += ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAdd, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return rtnVal;
                    }
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private bool DelData()
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAdd = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (int i = 0; i < ssItem_Sheet1.RowCount; i++)
                {
                    if (ssItem_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                    {
                        continue;
                    }

                    string ItemCd = ssItem_Sheet1.Cells[i, 3].Text.Trim();

                    SQL = "DELETE ADMIN.AEMRSUGAMAPPING";
                    SQL += ComNum.VBLF + "WHERE FORMNO =  " + SelectFormNo;
                    SQL += ComNum.VBLF + "  AND ITEMCD = '" + ItemCd + "'";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAdd, clsDB.DbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return rtnVal;
                    }
                }

                rtnVal = true;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 기록지 리스트
        /// </summary>
        private void GetFormList()
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            #region 쿼리
            SQL = "SELECT FORMNO, FORMNAME";
            SQL += ComNum.VBLF + " FROM ADMIN.AEMRFORM";
            SQL += ComNum.VBLF + "WHERE FORMNO IN (3150, 1575) -- 임상관찰, 기본간호활동";
            SQL += ComNum.VBLF + "  AND UPDATENO > 0";
            SQL += ComNum.VBLF + "  AND USECHECK = 1";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            ssForm_Sheet1.RowCount = dt.Rows.Count;
            ssForm_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssForm_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNO"].ToString();
                ssForm_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString();
            }

            dt.Dispose();
            return;
        }

        /// <summary>
        /// 기록지 아이템 리스트
        /// </summary>
        private void GetItemList(string formNo)
        {
            ssItem_Sheet1.RowCount = 0;

            if (formNo.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "기록지를 선택해주세요.");
                return;
            }

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            #region 쿼리
            SQL = "SELECT   UNITCLS";
            SQL += ComNum.VBLF + "  , BASEXNAME";
            SQL += ComNum.VBLF + "  , BASNAME";
            SQL += ComNum.VBLF + "  , BASCD";
            SQL += ComNum.VBLF + "  , ORDERGBN";
            SQL += ComNum.VBLF + "  , APPOINTMENT";
            SQL += ComNum.VBLF + "  , B.ITEMCD";

            SQL += ComNum.VBLF + " FROM ADMIN.AEMRBASCD A";
            if (rdoSave.Checked)
            {
                SQL += ComNum.VBLF + "   INNER JOIN ADMIN.AEMRSUGAMAPPING B";
                SQL += ComNum.VBLF + "      ON B.FORMNO = " + formNo;
                SQL += ComNum.VBLF + "     AND B.ITEMCD = A.BASCD";
            }
            else
            {
                SQL += ComNum.VBLF + "    LEFT OUTER JOIN ADMIN.AEMRSUGAMAPPING B";
                SQL += ComNum.VBLF + "      ON B.FORMNO = " + formNo;
                SQL += ComNum.VBLF + "     AND B.ITEMCD = A.BASCD";
            }
            SQL += ComNum.VBLF + "WHERE A.BSNSCLS = '기록지관리'";

            if (formNo.Equals("3150"))
            {
                SQL += ComNum.VBLF + "  AND A.UNITCLS IN ('임상관찰', '특수치료', '섭취배설', '임상관찰') ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND A.UNITCLS = '간호활동항목'";
            }

            SQL += ComNum.VBLF + "ORDER BY A.VFLAG1, A.NFLAG1";
            #endregion

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            ssItem_Sheet1.RowCount = dt.Rows.Count;
            ssItem_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ComboBoxCellType comboBoxCell  = (ComboBoxCellType)ssItem_Sheet1.Columns[5].CellType;
            ComboBoxCellType comboBoxCell2 = (ComboBoxCellType)ssItem_Sheet1.Columns[6].CellType;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //ssItem_Sheet1.Cells[i, 0].Text = (!string.IsNullOrWhiteSpace(dt.Rows[i]["ITEMCD"].ToString())).ToString();
                ssItem_Sheet1.Cells[i, 1].Text = dt.Rows[i]["UNITCLS"].ToString();
                ssItem_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BASEXNAME"].ToString();
                ssItem_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BASCD"].ToString();
                ssItem_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BASNAME"].ToString();

                ssItem_Sheet1.Cells[i, 5].Text = comboBoxCell.Items.Where(d => d.NotEmpty() && d.Length >= 3 && d.Substring(0, 3).Equals(dt.Rows[i]["ORDERGBN"].ToString())).FirstOrDefault();
                ssItem_Sheet1.Cells[i, 6].Text = comboBoxCell2.Items.Where(d => d.NotEmpty() && d.Length >= 3 && d.Substring(0, 3).Equals(dt.Rows[i]["APPOINTMENT"].ToString())).FirstOrDefault();

                if (rdoSave.Checked == false && ssItem_Sheet1.Cells[i, 5].Text.NotEmpty())
                {
                    ssItem_Sheet1.Cells[i, 0].Text = "True";
                    ssItem_Sheet1.Rows[i].BackColor = System.Drawing.Color.LightPink;
                }
            }

            dt.Dispose();
            return;
        }

        #endregion

        private void ssForm_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SelectFormNo = string.Empty;
            SelectFormNo = ssForm_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow(ssForm, e.Column);
                return;
            }

            GetItemList(SelectFormNo);
        }

        private void rdoSave_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                GetItemList(SelectFormNo);
            }
        }

        private void ssItem_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssItem_Sheet1.Cells[e.Row, 0].Text = "True";
        }
    }
}
