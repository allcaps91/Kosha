using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmCertTest_PhohangSM : Form
    {

        //인사마스트에서 CERTPASS 가 있을 경우만 들어간다.


        public frmCertTest_PhohangSM()
        {
            InitializeComponent();
        }

        private void frmCertTest_PhohangSM_Load(object sender, EventArgs e)
        {

        }

        private void APIINIT_Click(object sender, EventArgs e)
        {
            clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0);
        }

        private void APIRELEASE_Click(object sender, EventArgs e)
        {
            clsCertWork.API_RELEASE();
        }

        private void IDC_BUTTON_ROAMING2_Click(object sender, EventArgs e)
        {

        }

        private void IDC_BUTTON_ROAMING_Click(object sender, EventArgs e)
        {

        }

        private void btnCertAll_Click(object sender, EventArgs e)
        {
            //double rtnVal = clsCertWork.CertWork(clsDB.DbCon, "29519", clsCertWork.EXAM_VERIFY, "02487371", "테스트 데이타입니다.");
            //if (rtnVal == 0)
            //{
            //    ComFunc.MsgBoxEx(this, "전자인증중 문제가 발생했습니다");
            //}
        }

        private void btnSearchSabun_Click(object sender, EventArgs e)
        {
            string SID = "";
            string CERTPASS = "";
            clsCertWork.GetCertIdAndPassword(clsDB.DbCon, txtSABUN.Text.Trim(), ref SID, ref CERTPASS);

            IDC_EDIT_ID.Text = SID;
            IDC_EDIT_CERTPASS.Text = CERTPASS;
            
        }
    }
}
