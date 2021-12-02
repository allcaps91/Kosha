using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmChartColorRank
    /// Description     : 서식지 표시 색 및 정렬 순서 변경
    /// Author          : 이현종
    /// Create Date     : 2019-09-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmChartColorRank.frm) >> frmChartColorRank.cs 폼이름 재정의" />
    /// 
    public partial class frmChartColorRank : Form
    {
        //public delegate void CloseEvent();
        //public event CloseEvent rClosed;

        public frmChartColorRank()
        {
            InitializeComponent();
        }

        private void FrmBSTList_Load(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
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

                SQL = " SELECT A.FORMNO, A.FORMNAME,  B.COLOR, B.RANKING, B.ROWID";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRFORM A";
                SQL += ComNum.VBLF + "   LEFT OUTER JOIN KOSMOS_EMR.EMRFORM_SET B";
                SQL += ComNum.VBLF + "     ON  A.FORMNO = B.FORMNO";
                SQL += ComNum.VBLF + "     AND B.SABUN = "  + clsType.User.IdNumber;
                SQL += ComNum.VBLF + " WHERE EXISTS";
                SQL += ComNum.VBLF + " (";
                SQL += ComNum.VBLF + "   SELECT FORMNO ";
                SQL += ComNum.VBLF + "     FROM KOSMOS_EMR.EMRFORM_SET_LIST";
                SQL += ComNum.VBLF + "    WHERE SABUN = " + clsType.User.IdNumber;
                SQL += ComNum.VBLF + "      AND FORMNO = A.FORMNO";
                SQL += ComNum.VBLF + " )";
                SQL += ComNum.VBLF + "   AND A.UPDATENO = 1";
                SQL += ComNum.VBLF + "  ORDER BY TO_NUMBER(B.RANKING) ASC, A.FORMNAME ASC ";

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
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["COLOR"].ToString().Trim().Equals("1") ? "True" : "False";
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RANKING"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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
            Save_Data();
       
            GetSearchData();
        }



        /// <summary>
        ///  저장
        /// </summary>
        /// <returns></returns>
        bool Save_Data()
        {
            string SQL    = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    string strCODE = SS1_Sheet1.Cells[i, 0].Text.Trim();
                    string strCHECK = SS1_Sheet1.Cells[i, 3].Text.Trim().Equals("True") ? "1" : "0";
                    string strRANKING = SS1_Sheet1.Cells[i, 4].Text.Trim();
                    string strROWID = SS1_Sheet1.Cells[i, 5].Text.Trim();

                    if (strROWID.Length > 0)
                    {
                        SQL = " DELETE KOSMOS_EMR.EMRFORM_SET ";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }

                    SQL = " INSERT INTO KOSMOS_EMR.EMRFORM_SET (";
                    SQL += ComNum.VBLF + " FORMNO, SABUN, WRITEDATE, RANKING, COLOR) VALUES (";
                    SQL += ComNum.VBLF + "'" + strCODE + "'," + clsType.User.IdNumber + ", SYSDATE, '" + strRANKING + "','" + strCHECK + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
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

        private void BtnSaveForm_Click(object sender, EventArgs e)
        {
            using(frmFormList frm = new frmFormList())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
            GetSearchData();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
