using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class FrmMedPregnantOrder : Form
    {
        string SQL = "";
        DataTable dt = null;
        string SqlErr = ""; //에러문 받는 변수

        string strOrdCode = "";
        string strOrdName = "";
        int FnRow;

        public delegate void OKClick(int nPWeek, int nRow);
        public static event OKClick PregnantOrderClick;

        public FrmMedPregnantOrder()
        {
            InitializeComponent();
        }

        public FrmMedPregnantOrder(string sOrdCode, string sOrdName, int nRow)
        {
            InitializeComponent();

            strOrdCode = sOrdCode;
            strOrdName = sOrdName;
            FnRow = nRow;
        }

        private void FrmMedPregnantOrder_Load(object sender, EventArgs e)
        {
            txtOrdCode.Text = strOrdCode;
            lblOrdName.Text = strOrdName;
        }

        private void FrmMedPregnantOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txtPregnancyWeek.Text.Trim() == "")
            {
                MessageBox.Show("임신주수를 입력하시기 바랍니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPregnancyWeek.Focus();
                return;
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            string strCode = "";
            int nPWeek = 0;

            if (txtOrdCode.Text.Trim() == "")
            {
                MessageBox.Show("처방코드를 입력하시기 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOrdCode.Focus();
                return;
            }

            if (txtPregnancyWeek.Text.Trim() == "")
            {
                MessageBox.Show("임신주수를 입력하시기 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPregnancyWeek.Focus();
                return;
            }

            if (VB.IsNumeric(txtPregnancyWeek.Text) == false)
            {
                MessageBox.Show("임신주수는 숫자만 입력이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPregnancyWeek.Text = "";
                txtPregnancyWeek.Focus();
                return;
            }

            try
            {
                SQL = "";
                SQL += " SELECT SUCODE                                          \r";
                SQL += "   FROM ADMIN.OCS_ORDERCODE                        \r";
                SQL += "  WHERE ORDERCODE = '" + txtOrdCode.Text.Trim() + "'    \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strCode = dt.Rows[0]["SUCODE"].ToString().Trim();
                }
                else
                {
                    strCode = txtOrdCode.Text.Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            nPWeek = int.Parse(txtPregnancyWeek.Text);

            switch (strCode)
            {
                case "EB511-1":
                    if (nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 13주 이하만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB512-1":
                    if (nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 13주 이하만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB511H":
                    if (nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 13주 이하만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB511":
                    if (nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 13주 이하만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB512":
                    if (nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 13주 이하만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB513-1":
                    if (nPWeek < 11 || nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 11 ~ 13주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB514-1":
                    if (nPWeek < 11 || nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 11 ~ 13주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB513":
                    if (nPWeek < 11 || nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 11 ~ 13주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB514":
                    if (nPWeek < 11 || nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 11 ~ 13주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515-1":
                    if (nPWeek < 11 || nPWeek > 13)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 11 ~ 13주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB516-1":
                    if (nPWeek < 14 || nPWeek > 19)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 14 ~ 19주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515H":
                    if (nPWeek < 14 || nPWeek > 19)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 14 ~ 19주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515":
                    if (nPWeek < 14 || nPWeek > 19)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 14 ~ 19주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB516":
                    if (nPWeek < 14 || nPWeek > 19)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 14 ~ 19주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515A-1":
                    if (nPWeek < 20|| nPWeek > 35)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 20 ~ 35주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB516A-1":
                    if (nPWeek < 20 || nPWeek > 35)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 20 ~ 35주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515AH":
                    if (nPWeek < 20 || nPWeek > 35)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 20 ~ 35주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515A":
                    if (nPWeek < 20 || nPWeek > 35)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 20 ~ 35주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB516A":
                    if (nPWeek < 20 || nPWeek > 35)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 20 ~ 35주만 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515B-1":
                    if (nPWeek < 36)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 36주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB516B-1":
                    if (nPWeek < 36)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 36주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515BH":
                    if (nPWeek < 36)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 36주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB515B":
                    if (nPWeek < 36)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 36주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB516B":
                    if (nPWeek < 36)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 36주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB517-1":
                    if (nPWeek < 16)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 16주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB518-1":
                    if (nPWeek < 16)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 16주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB517":
                    if (nPWeek < 16)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 16주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                case "EB518":
                    if (nPWeek < 16)
                    {
                        MessageBox.Show("해당 처방코드는 임신주수 16주 이후 처방이 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
                default:
                    MessageBox.Show("해당 처방코드는 임산부 초음파 코드가 아닙니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
            }

            PregnantOrderClick(int.Parse(txtPregnancyWeek.Text), FnRow);

            this.Close();

        }
    }
}
