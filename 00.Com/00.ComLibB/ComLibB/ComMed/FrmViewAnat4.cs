using ComBase;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : 마약처방전 주요증상
    /// Author : 이상훈
    /// Create Date : 2018.01.15
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmViewAnat4.frm"/>
    public partial class FrmViewAnat4 : Form
    {
        string strMayakSuCode;

        public FrmViewAnat4(string sMayakSuCode)
        {
            InitializeComponent();

            strMayakSuCode = sMayakSuCode;
        }

        private void FrmViewAnat4_Load(object sender, EventArgs e)
        {
            this.Location = new Point(300, 300);

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            lblMayakTitle.Text = "";
            if (clsOrdFunction.GstrMayakSuCode != "")
            {
                lblMayakTitle.Text = "마약수가 : " + clsOrdFunction.GstrMayakSuCode;

                rtxtRequest.Text = clsOrdFunction.GstrMayakRemark;
            }

            rtxtRequest.Focus();
            rtxtRequest.Select();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (rtxtRequest.Text.Trim().Length < 4)
            {
                MessageBox.Show("주요증상을 5글자 이상 입력하십시오!!!", "주용증상", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                rtxtRequest.Focus();
                return;
            }

            clsOrdFunction.GstrMayakRemark = rtxtRequest.Text.Replace("'", "`");
            clsOrdFunction.GstrMayakSuCode = "";
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
