using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmChartLog
    /// Description     : 챠트 작성 로그 조회
    /// Author          : 이현종
    /// Create Date     : 2019-08-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmChartLog.frm) >> frmChartLog.cs 폼이름 재정의" />
    /// 
    public partial class frmChartLog : Form
    {

        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        public frmChartLog()
        {
            InitializeComponent();
        }

        private void FrmChartLog_Load(object sender, EventArgs e)
        {
            txtFormName.Clear();
            lblF23.Text = "";

            txtPTNO.Clear();
            lblName.Text = "";

            dtpSDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpEDATE.Value = dtpSDATE.Value;

            SS1_Sheet1.RowCount = 0;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (lblFormNo.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "서식지 명칭을 정확히 입력하시기 바랍니다.");
                return;
            }

            if (lblName.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "등록번호를 정확히 입력하시기 바랍니다.");
                return;
            }

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
                SQL = " SELECT A.FORMNAME, B.CHARTDATE, B.CHARTTIME, B.WRITEDATE, B.WRITETIME, B.USEID, B.ROWID, '1' GUBUN, M.KORNAME"                ;
                SQL += ComNum.VBLF + "  FROM ADMIN.EMRFORM A, ADMIN.EMRXMLMST B, ADMIN.INSA_MST M"                                                   ;
                SQL += ComNum.VBLF + " WHERE B.PTNO = '"  + txtPTNO.Text.Trim() + "'"                                                          ;
                SQL += ComNum.VBLF + "   AND B.FORMNO = " + lblFormNo.Text;
                SQL += ComNum.VBLF + "   AND B.CHARTDATE = '" + dtpSDATE.Value.ToString("yyyyMMdd") + "'"                                  ;
                SQL += ComNum.VBLF + "   AND A.FORMNO = B.FORMNO"                                                                            ;
                SQL += ComNum.VBLF + "   AND M.SABUN3 = B.USEID"                                                                            ;
                SQL += ComNum.VBLF + " UNION ALL";
                SQL += ComNum.VBLF + " SELECT A.FORMNAME, B.CHARTDATE, B.CHARTTIME, B.WRITEDATE, B.WRITETIME, B.USEID, B.ROWID, '2' GUBUN, M.KORNAME";
                SQL += ComNum.VBLF + "  FROM ADMIN.EMRFORM A, ADMIN.EMRXMLHISTORY B, ADMIN.INSA_MST M";
                SQL += ComNum.VBLF + " WHERE B.PTNO = '"  + txtPTNO.Text.Trim() + "'"                                                          ;
                SQL += ComNum.VBLF + "   AND B.FORMNO = " + lblFormNo.Text                                                           ;
                SQL += ComNum.VBLF + "   AND B.CHARTDATE = '" + dtpSDATE.Value.ToString("yyyyMMdd") +  "'"                                  ;
                SQL += ComNum.VBLF + "   AND A.FORMNO = B.FORMNO"                                                                            ;
                SQL += ComNum.VBLF + "   AND M.SABUN3 = B.USEID"                                                                            ;
                SQL += ComNum.VBLF + "   ORDER BY GUBUN ASC, CHARTDATE DESC, CHARTTIME DESC, WRITEDATE DESC, WRITETIME DESC";

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
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["CHARTTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WRITETIME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["USEID"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GUBUN"].ToString().Trim();

                        if (SS1_Sheet1.Cells[i, 8].Text.Trim().Equals("1"))
                        {
                            SS1_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 255);
                        }
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (rClosed == null)
            {
                Close();
                return;
            }
            rClosed();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;


            #region 쿼리
            string SQL = string.Empty;
            OracleDataReader reader = null;

            try
            {
                SQL = "SELECT COUNT(*) CNT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "    WHERE GUBUN = 'EMR_권역평가' ";
                SQL = SQL + ComNum.VBLF + "      AND CODE = '인증' ";
                SQL = SQL + ComNum.VBLF + "      AND NAME = 'Y'";

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

                if (SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Equals("0"))
                {
                    reader.Dispose();
                    return;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion

            if(SS1_Sheet1.Cells[e.Row, 8].Text.Trim().Equals("1"))
            {
                string strRowid = SS1_Sheet1.Cells[e.Row, 7].Text.Trim();
                string strUseId = SS1_Sheet1.Cells[e.Row, 6].Text.Trim();

                VB.Shell(@"c:\cmc\exe\certchk.exe EMR^^" + strUseId + "^^" + strRowid + "^^");
            }
        }

        private void TxtFormName_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                #region 쿼리
                string SQL = string.Empty;
                OracleDataReader reader = null;

                try
                {
                    SQL = "SELECT FORMNO, FORMNAME";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.AEMRFORM ";
                    SQL = SQL + ComNum.VBLF + " WHERE UPPER(FORMNAME) LIKE '%" + txtFormName.Text.Trim().ToUpper() + "%'";

                    string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        lblFormNo.Text = reader.GetValue(0).ToString().Trim();
                        txtFormName.Text = reader.GetValue(1).ToString().Trim();
                    }

                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                }
                #endregion
            }
        }

        private void TxtPTNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPTNO.Text = ComFunc.SetAutoZero(txtPTNO.Text, 8);

                #region 쿼리
                string SQL = string.Empty;
                OracleDataReader reader = null;

                try
                {
                    SQL = "SELECT PANO, SNAME";
                    SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPTNO.Text + "'";

                    string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        txtPTNO.Text = reader.GetValue(0).ToString().Trim();
                        lblName.Text = reader.GetValue(1).ToString().Trim();
                    }

                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                }
                #endregion
            }
        }
    }
}
