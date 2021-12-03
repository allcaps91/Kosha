using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrPrintOption : Form
    {
        public frmEmrPrintOption()
        {
            InitializeComponent();
            
        }

        private void frmEmrPrintOption_Load(object sender, EventArgs e)
        {
            //TODO
            ////심사실
            //string strSql = "";
            //DataTable dt = null;

            //strSql = " SELECT BASCD ";
            //strSql = strSql + clsComNum.VBLF + "FROM " + clsPublic.EMRDB + "AEMRBASCD ";
            //strSql = strSql + clsComNum.VBLF + "WHERE BSNSCLS = '권한관리' ";
            //strSql = strSql + clsComNum.VBLF + "    AND UNITCLS = 'TOP' ";
            //strSql = strSql + clsComNum.VBLF + "    AND BASEXNAME = '보험심사팀' ";
            //strSql = strSql + clsComNum.VBLF + "    AND BASCD = '" + clsType.gEmrUseInfo.strUseId + "' ";
            //dt = clsDB.GetDataTableREx(strSql);

            //if (dt == null)
            //{
            //    clsComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    return;
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    mbtnOut.Enabled = false;
            //    mbtnIn.Enabled = false;
            //    mbtnSimsa.Enabled = true;
            //    mbtnSimsa.Focus();
            //    blnSimsa = true;
            //}
            //else
            //{
            //    dt.Dispose();
            //    dt = null;
            //    if (clsType.gEmrUseInfo.AuAPRINTOUT == "1")
            //    {
            //        mbtnOut.Enabled = true;
            //    }
            //    if (clsType.gEmrUseInfo.AuAPRINTSIM == "1")
            //    {
            //        mbtnSimsa.Enabled = true;
            //    }
            //    mbtnIn.Focus();
            //}
            


            if (clsType.User.AuAPRINTOUT == "1")
            {
                btnOut.Enabled = true;
            }

            if (clsType.User.AuAPRINTSIM == "1")
            {
                btnSimsa.Enabled = true;
                btnSimsa.Focus();
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            clsFormPrint.mstrPRINTFLAG = "1";
            this.Close();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            clsFormPrint.mstrPRINTFLAG = "0";
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clsFormPrint.mstrPRINTFLAG = "-1";
            this.Close();
        }

        private void btnSimsa_Click(object sender, EventArgs e)
        {
            clsFormPrint.mstrPRINTFLAG = "2";
            this.Close();
        }

        private void frmEmrPrintOption_Activated(object sender, EventArgs e)
        {
            if (clsType.User.AuAPRINTSIM == "1")
            {
                btnSimsa.Focus();
            }
        }
    }
}
