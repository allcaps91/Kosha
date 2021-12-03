using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmFormList
    /// Description     : 서식지 검색
    /// Author          : 이현종
    /// Create Date     : 2020-02-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmFormList.frm) >> frmFormList.cs 폼이름 재정의" />
    /// 
    public partial class frmFormList : Form
    {
        public frmFormList()
        {
            InitializeComponent();
        }

        private void FrmBSTList_Load(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;
            txtTitle.Clear();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT FORMNO, FORMNAME";
                SQL += ComNum.VBLF + " FROM ADMIN.AEMRFORM A";
                SQL += ComNum.VBLF + " WHERE UPPER(FORMNAME) LIKE '%" + txtTitle.Text.Trim().ToUpper() + "%' ";
                //SQL += ComNum.VBLF + "   AND USECHECK = 1";
                SQL += ComNum.VBLF + " AND NOT EXISTS";
                SQL += ComNum.VBLF + " (SELECT FORMNO, SABUN FROM ADMIN.EMRFORM_SET_LIST SUB";
                SQL += ComNum.VBLF + " WHERE SABUN = " + clsType.User.IdNumber;
                SQL += ComNum.VBLF + " AND A.FORMNO = SUB.FORMNO)";
                SQL += ComNum.VBLF + " ORDER BY FORMNAME ASC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount =  dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Save_Data())
            {
                Close();
            }
        }



        /// <summary>
        ///  저장
        /// </summary>
        /// <returns></returns>
        bool Save_Data()
        {
            string SQL    = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    if (SS1_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        SQL = " INSERT INTO ADMIN.EMRFORM_SET_LIST(SABUN, FORMNO) VALUES ";
                        SQL += ComNum.VBLF + "(" + clsType.User.IdNumber + ",'" + SS1_Sheet1.Cells[i, 2].Text.Trim() + "')";

                        string SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }


                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetSearchData();
            }
        }
    }
}
