using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmNPChartBatch
    /// Description     : 병동별 NP챠트 조회 권한 일괄 등록
    /// Author          : 이현종
    /// Create Date     : 2019-08-29
    /// Update History  : 
    /// </summary> 20-03-12 FM 권한 추가
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmNPChartBatch.frm) >> frmNPChartBatch.cs 폼이름 재정의" />
    /// 
    public partial class frmNPChartBatch : Form
    {
        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        string strDept = string.Empty;

        public frmNPChartBatch(string sDept)
        {
            strDept = sDept.IndexOf("NP") != -1 ? "NP" : "FM";
            InitializeComponent();
        }

        private void FrmNPChartBatch_Load(object sender, EventArgs e)
        {
            Text = "병동별 " + strDept + "챠트 조회 권한 일괄 등록";
            lblTitle.Text = strDept + "챠트 조회 권한 일괄 등록";

            GetSearhData();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearhData();
        }

        void GetSearhData()
        {

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT CODE, PRINTRANKING, B.BUSECODE, A.MATCH_CODE";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_CODE A, KOSMOS_EMR.EMR_USERT B";
                SQL += ComNum.VBLF + " WHERE A.GUBUN = '2'";
                SQL += ComNum.VBLF + "   AND A.MATCH_CODE = B.BUSECODE(+)";
                SQL += ComNum.VBLF + "   AND B." + strDept + "VIEW(+) = '*'";
                SQL += ComNum.VBLF + "   AND SUBUSE = '1' "; //'병동에만 적용(ER제외)
                SQL += ComNum.VBLF + "   AND A.CODE NOT IN ('ER') ";
                SQL += ComNum.VBLF + " GROUP BY CODE, PRINTRANKING, B.BUSECODE, A.MATCH_CODE ";
                SQL += ComNum.VBLF + " ORDER BY PRINTRANKING";

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

                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BUSECODE"].ToString().Trim().Length > 0 ? "Y" : "N";
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MATCH_CODE"].ToString().Trim();
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
            if(ComFunc.MsgBoxQEx(this, "적용하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            if (SS1_Sheet1.RowCount == 0)
                return;

            clsDB.setBeginTran(clsDB.DbCon);

            string SQL = string.Empty;
            try
            {
                for (int i = 0; i < SS1_Sheet1.RowCount; i++)
                {
                    string strBuse = SS1_Sheet1.Cells[i, 2].Text.Trim();
                    SQL = " UPDATE KOSMOS_EMR.EMR_USERT ";
                    if(SS1_Sheet1.Cells[i, 1].Text.Trim() == "Y")
                    {
                        SQL += ComNum.VBLF + " SET " + strDept + "VIEW = '*' ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + " SET " + strDept + "VIEW = NULL ";
                    }

                    SQL += ComNum.VBLF + " WHERE BUSECODE = '" + strBuse + "' ";

                    int RowAffected = 0;
                    string sqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                    if(sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBox(sqlErr);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                GetSearhData();
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }
    }
}
