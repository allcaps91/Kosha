using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class FrmMedMsgGsAdd : Form
    {
        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string strOrderCode;
        string strGbn;

         public FrmMedMsgGsAdd()
        {
            InitializeComponent();
        }

        public FrmMedMsgGsAdd(string sOrderCode, string sGbn)
        {
            InitializeComponent();

            strOrderCode = sOrderCode;
            strGbn = sGbn;
        }

        private void FrmMedMsgGsAdd_Load(object sender, EventArgs e)
        {
            try
            {
                SQL = "";
                SQL += " SELECT ORDERNAME                           \r";
                SQL += "   FROM ADMIN.OCS_ORDERCODE            \r";
                SQL += "  WHERE ORDERCODE = '" + strOrderCode + "'  \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    lblOrderCode.Text = strOrderCode;
                    lblOrderName.Text = dt.Rows[0]["ORDERNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            btnGS.Visible = false;
            btnCS.Visible = false;

            switch (strGbn)
            {
                case "1":
                    btnGS.Visible = true;
                    btnGS.Focus();
                    break;
                case "2":
                    btnCS.Visible = true;
                    btnCS.Focus();
                    break;
                case "3":
                    btnGS.Visible = true;
                    btnCS.Visible = true;
                    btnGS.Focus();
                    break;
                default:
                    MessageBox.Show("해당수가 오류, 전산팀 연락 요망!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
            }
        }

        private void btnEtc_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = "0";
            this.Close();
        }

        private void btnGS_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = "1";
            this.Close();
        }

        private void btnCS_Click(object sender, EventArgs e)
        {
            clsPublic.GstrHelpCode = "2";
            this.Close();
        }
    }
}
