using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class FrmWardCheck : Form
    {
        string SQL;
        DataTable dt = null;
        //DataTable dt1 = null;
        //DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        //int intRowAffected = 0; //변경된 Row 받는 변수

        public FrmWardCheck()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsPublic.GstrWardCode = "";
            clsPublic.GstrICUWard = "";
            
            VB.SaveSetting("TWIN", "NURVIEW", "WARDCODE", cboWard.Text.Trim());

            clsPublic.GstrWardCode = VB.Left(cboWard.Text.Trim(), 2);
            clsPublic.GstrICUWard = VB.Mid(cboWard.Text.Trim(), 4, 4);
            this.Close();
        }

        private void FrmWardCheck_Load(object sender, EventArgs e)
        {
            try
            {
                SQL = "";
                SQL += " SELECT WardCode FROM ADMIN.BAS_WARD  \r";
                SQL += "  WHERE WardCode <> 'IU'                    \r";
                SQL += "    AND USED = 'Y'                          \r";
                SQL += "  ORDER BY WardCode                         \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon); ;
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboWard.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                }

                //cboWard.Items.Add("IU(SICU");
                //cboWard.Items.Add("IU(MICU");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("AN");
                cboWard.Items.Add("EN");
                cboWard.Items.Add("EX");
                cboWard.Items.Add("HD");
                cboWard.Items.Add("OP");
                cboWard.Items.Add("OPD");
                cboWard.Items.Add("CSR");
                //cboWard.Items.Add("OR");        //2019-02-14

                cboWard.SelectedIndex = 0;

                for (int i = 0; i < cboWard.Items.Count; i++)
                {
                    cboWard.SelectedIndex = i;
                    if (clsPublic.GstrICUWard == "SICU")
                    {
                        cboWard.SelectedIndex = 12;
                        break;
                    }

                    if (clsPublic.GstrICUWard == "MICU")
                    {
                        cboWard.SelectedIndex = 13;
                        break;
                    }
                    cboWard.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
