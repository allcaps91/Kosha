using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmSetup01 : Form
    {
        public frmSetup01()
        {
            InitializeComponent();
        }

        private void frmSetup01_Load(object sender, EventArgs e)
        {
            cboVal1.Items.Clear();
            cboVal1.Items.Add("Rt Arm");
            cboVal1.Items.Add("Lt Arm");
            cboVal1.Items.Add("Rt Leg");
            cboVal1.Items.Add("Lt Leg");
            cboVal1.SelectedIndex = 1;


            cboVal2.Items.Clear();
            cboVal2.Items.Add("고막");
            cboVal2.Items.Add("Axilla");
            cboVal2.Items.Add("Oral"  );
            cboVal2.Items.Add("Rectal");
            cboVal2.SelectedIndex = 1;

            cboWard.Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, clsType.User.BuseCode) + "." + clsType.User.BuseCode;

            ReadSetup();
        }

        void ReadSetup()
        {
            DataTable dt = null;
            string SQL = " SELECT BUSE, VAL1, VAL2 ";
            SQL += ComNum.VBLF + " FROM ADMIN.EMR_SETUP_01 ";
            SQL += ComNum.VBLF + " WHERE BUSE = '" + VB.Right(cboWard.Text.Trim(), 6) + "' ";

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                cboWard.Text = clsVbfunc.GetBASBuSe(clsDB.DbCon, dt.Rows[0]["BUSE"].ToString().Trim()) + "." + dt.Rows[0]["BUSE"].ToString().Trim();
                cboVal1.Text = dt.Rows[0]["VAL1"].ToString().Trim();
                cboVal2.Text = dt.Rows[0]["VAL2"].ToString().Trim();
            }


            dt.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = " DELETE ADMIN.EMR_SETUP_01 ";
                SQL += ComNum.VBLF + " WHERE BUSE = '" + VB.Right(cboWard.Text.Trim(), 6) + "' ";

                string SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                SQL = " INSERT INTO ADMIN.EMR_SETUP_01(BUSE, VAL1, VAL2) VALUES (";
                SQL += ComNum.VBLF + "'" + VB.Right(cboWard.Text.Trim(), 6) + "','" + cboVal1.Text.Trim() + "','" + cboVal2.Text.Trim() + "') ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);

                if (SqlErr.Length > 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                ComFunc.MsgBoxEx(this, "저장되었습니다");
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
