using ComBase;
using ComBase.Controls;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmOcsCpDrop : Form
    {
        string strCPNO = string.Empty;

        public frmOcsCpDrop()
        {
            InitializeComponent();
        }

        public frmOcsCpDrop(string strCPNO)
        {
            InitializeComponent();
            this.strCPNO = strCPNO;
        }

        private void frmOcsCpDrop_Load(object sender, EventArgs e)
        {
            GetDataCpCode();
        }

        private void GetDataCpCode()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = string.Empty;
            string SqlErr = string.Empty;

            DataTable dt = null;
            int i = 0;

            cboSayu.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = string.Empty;
                //SQL = "SELECT";
                //SQL = SQL + ComNum.VBLF + " DISPSEQ,   BASCD, BASNAME";
                //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BASCD";
                //SQL = SQL + ComNum.VBLF + "  WHERE GRPCDB = 'CP관리'";
                //SQL = SQL + ComNum.VBLF + "    AND GRPCD  = 'CP제외기준'";
                //SQL = SQL + ComNum.VBLF + "ORDER BY DISPSEQ";

                SQL = string.Empty;
                SQL = "SELECT                                                                 ";
                SQL = SQL + ComNum.VBLF + " B.DSPSEQ,   NAME                                  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_CP_MAIN A           ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_MED + "OCS_CP_RECORD R   ";
                SQL = SQL + ComNum.VBLF + "   ON A.CPCODE = R.CPCODE                          ";
                SQL = SQL + ComNum.VBLF + "  AND R.CPNO   = " + strCPNO + "                   ";

                SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_MED + "OCS_CP_SUB B      ";
                SQL = SQL + ComNum.VBLF + "   ON A.CPCODE = B.CPCODE                          ";
                SQL = SQL + ComNum.VBLF + "  AND A.SDATE  = B.SDATE                           ";
                SQL = SQL + ComNum.VBLF + "  AND B.GUBUN  = '03' -- 구분자(01:자격,02:중단사유,03:제외기준,04:진단코드,05:수술코드,06:지표내용,07:동의서)                          ";
                if (clsType.User.IsNurse.Equals("OK"))
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.REMARK  = '간호사' --타병원 전원";
                }
                SQL = SQL + ComNum.VBLF + "WHERE A.SDATE  = (SELECT MAX(SDATE) FROM KOSMOS_OCS.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.DSPSEQ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSayu.Items.Add(dt.Rows[i]["DSPSEQ"].To<int>().ToString("00") +
                                                  "." + dt.Rows[i]["NAME"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboSayu.SelectedIndex = 0;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (clsType.User.DrCode.NotEmpty() &&
                ComFunc.MsgBoxQEx(this, string.Format("제외하실경우 {0}일 이후의 'CP'처방중에\r\n픽업이 안된 오더만 자동 D/C 됩니다 제외하시겠습니까?", dtpDate.Value.ToString("yyyy-MM-dd"))) == DialogResult.No)
                return;

            if (Save_Data() == true)
            {
                ComFunc.MsgBox("제외하였습니다.");
                Close();
            }
            return;
        }

        bool Save_Data()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string Code = cboSayu.Text.Substring(0, cboSayu.Text.IndexOf("."));

            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "UPDATE " + ComNum.DB_MED + "OCS_CP_RECORD";
                SQL += ComNum.VBLF + "  SET";
                SQL += ComNum.VBLF + "  DROPGB = '01',";
                SQL += ComNum.VBLF + "  DROPCD = '" + Code + "', ";
                //SQL += ComNum.VBLF + "  CANCERREMARK = '" + txtSayu.Text.Trim().Replace("'", "`") + "',";
                SQL += ComNum.VBLF + "  DROPDATE    = '" + dtpDate.Value.ToString("yyyyMMdd") + "', ";
                SQL += ComNum.VBLF + "  DROPTIME    = TO_CHAR(SYSDATE, 'HH24MISS'), ";
                SQL += ComNum.VBLF + "  DROPSABUN   = '" + clsType.User.Sabun + "'";
                SQL += ComNum.VBLF + "  WHERE CPNO  = " + strCPNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
