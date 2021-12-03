using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmDateSET : Form
    {
        public frmDateSET()
        {
            InitializeComponent();
        }

        private void frmDateSET_Load(object sender, EventArgs e)
        {
            GeSearchData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GeSearchData();
        }

        void GeSearchData()
        {
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            DataTable dt = null;

            SSO_Sheet1.RowCount = 0;
            SSI_Sheet1.RowCount = 0;

            #region 입원
            SQL = " SELECT B.NAL, B.SDATE, B.EDATE, B.USED, B.ROWID ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_OPTION_SETDATE B";
            SQL += ComNum.VBLF + "  WHERE B.USEID = " + clsType.User.IdNumber;
            SQL += ComNum.VBLF + "   AND B.IO = 'I'";

            sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if(sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                return;
            }

            if(dt.Rows.Count > 0)
            {
                SSI_Sheet1.RowCount = dt.Rows.Count;
                SSI_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSI_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAL"].ToString().Trim();
                    SSI_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    SSI_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                    SSI_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USED"].ToString().Trim();
                    SSI_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }
            else
            {
                SSI_Sheet1.RowCount = 1;
            }
            dt.Dispose();
            #endregion

            #region 외래
            SQL = " SELECT A.DEPTCODE, B.NAL, B.SDATE, B.EDATE, B.USED, B.ROWID";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_CLINICDEPT A, KOSMOS_EMR.EMR_OPTION_SETDATE B";
            SQL += ComNum.VBLF + " WHERE A.DEPTCODE NOT IN ('II','R6','HD','PT','AN')";
            SQL += ComNum.VBLF + "   AND A.DEPTCODE = B.DEPTCODE(+)";
            SQL += ComNum.VBLF + "   AND B.USEID(+) = " + clsType.User.IdNumber;
            SQL += ComNum.VBLF + "   AND B.IO(+) = 'O'";
            SQL += ComNum.VBLF + " ORDER BY  PrintRanking";

            sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                SSO_Sheet1.RowCount = dt.Rows.Count;
                SSO_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSO_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SSO_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAL"].ToString().Trim();
                    SSO_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    SSO_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                    SSO_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USED"].ToString().Trim().Equals("1").ToString();
                    SSO_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }
            dt.Dispose();
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                #region 입원
                for (int i = 0; i < SSI_Sheet1.RowCount; i++)
                {
                    string strDeptCode = SSI_Sheet1.Cells[i, 0].Text.Trim();
                    string strNAL   = SSI_Sheet1.Cells[i, 1].Text.Trim();
                    string strSDate = SSI_Sheet1.Cells[i, 2].Text.Trim();
                    string strEDATE = SSI_Sheet1.Cells[i, 3].Text.Trim();
                    string strUSED  = SSI_Sheet1.Cells[i, 4].Text.Trim().Equals("True") ? "1" : "0";
                    string strROWID = SSI_Sheet1.Cells[i, 5].Text.Trim();

                    if(string.IsNullOrWhiteSpace(strROWID))
                    {
                        SQL = " INSERT INTO KOSMOS_EMR.EMR_OPTION_SETDATE(";
                        SQL += ComNum.VBLF + " USEID, IO, DEPTCODE, SDATE, ";
                        SQL += ComNum.VBLF + " EDATE, WRITEDATE, USED, NAL) VALUES (";
                        SQL += ComNum.VBLF +  clsType.User.IdNumber + ",'I','" + strDeptCode + "', TO_DATE('" + strSDate + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + "TO_DATE('" + strEDATE + "','YYYY-MM-DD'), SYSDATE, '" + strUSED + "','" + strNAL + "') ";
                    }
                    else
                    {
                        SQL = " UPDATE KOSMOS_EMR.EMR_OPTION_SETDATE SET ";
                        SQL += ComNum.VBLF + " SDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + " EDATE = TO_DATE('" + strEDATE + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + " USED = '" + strUSED + "', ";
                        SQL += ComNum.VBLF + " NAL = '" + strNAL + "', ";
                        SQL += ComNum.VBLF + " WRITEDATE = SYSDATE";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    }

                    sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if(sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }
                }
                #endregion


                #region 외래
                for (int i = 0; i < SSO_Sheet1.RowCount; i++)
                {
                    string strDeptCode = SSO_Sheet1.Cells[i, 0].Text.Trim();
                    string strNAL = SSO_Sheet1.Cells[i, 1].Text.Trim();
                    string strSDate = SSO_Sheet1.Cells[i, 2].Text.Trim();
                    string strEDATE = SSO_Sheet1.Cells[i, 3].Text.Trim();
                    string strUSED = SSO_Sheet1.Cells[i, 4].Text.Trim().Equals("True") ? "1" : "0";
                    string strROWID = SSO_Sheet1.Cells[i, 5].Text.Trim();

                    if (string.IsNullOrWhiteSpace(strROWID))
                    {
                        SQL = " INSERT INTO KOSMOS_EMR.EMR_OPTION_SETDATE(";
                        SQL += ComNum.VBLF + " USEID, IO, DEPTCODE, SDATE, ";
                        SQL += ComNum.VBLF + " EDATE, WRITEDATE, USED, NAL) VALUES (";
                        SQL += ComNum.VBLF + clsType.User.IdNumber + ",'O','" + strDeptCode + "', TO_DATE('" + strSDate + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + "TO_DATE('" + strEDATE + "','YYYY-MM-DD'), SYSDATE, '" + strUSED + "','" + strNAL + "') ";
                    }
                    else
                    {
                        SQL = " UPDATE KOSMOS_EMR.EMR_OPTION_SETDATE SET ";
                        SQL += ComNum.VBLF + " SDATE = TO_DATE('" + strSDate + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + " EDATE = TO_DATE('" + strEDATE + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + " USED = '" + strUSED + "', ";
                        SQL += ComNum.VBLF + " NAL = '" + strNAL + "', ";
                        SQL += ComNum.VBLF + " WRITEDATE = SYSDATE";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    }

                    sqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }
                }
                #endregion


                clsDB.setCommitTran(clsDB.DbCon);
                Close();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
