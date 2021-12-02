using System;
using ComBase;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComSupLibB.Com;

namespace ComSupLibB
{
    public partial class frmDietOrderNewMng : Form
    {
        private string mstrPtNo = "";
        private string mstrGuBun = "";
        private string mstrFDate = "";
        private string mstrTDate = "";
        private string mstrWard = "";

        public frmDietOrderNewMng()
        {
            InitializeComponent();
        }

        public frmDietOrderNewMng(string strPtNo, string strGuBun, string strFDate, string strTDate)
        {
            InitializeComponent();

            mstrPtNo = strPtNo;
            mstrGuBun = strGuBun;
            mstrFDate = strFDate;
            mstrTDate = strTDate;
        }

        public frmDietOrderNewMng(string strPtNo, string strGuBun, string strFDate, string strTDate, string strWard)
        {
            InitializeComponent();

            mstrPtNo = strPtNo;
            mstrGuBun = strGuBun;
            mstrFDate = strFDate;
            mstrTDate = strTDate;
            mstrWard = strWard;
        }

        private void frmDietOrderNewMng_Load(object sender, EventArgs e)
        {
            clsComSup sup = new clsComSup();

            //if (mstrWard != "")
            //{
            //    frmDietOrderNew frmDietOrderNewX = new frmDietOrderNew("SUB", mstrPtNo, mstrWard);
            //    frmHistory2 frmHistory2X = new frmHistory2("SUB", mstrPtNo, mstrGuBun, mstrFDate, mstrTDate);
            //    sup.setCtrlLoad(panel1, frmDietOrderNewX);
            //    sup.setCtrlLoad(panel2, frmHistory2X);
            //}
            //else
            //{
            ComFunc cf = new ComFunc();

            if (cf.Read_Bcode_Name(clsDB.DbCon, "DIET_식이처방_NEW_사용", "USE") == "Y")
            {
                frmDietOrder2020 frmDietOrderNewX = new frmDietOrder2020("SUB", mstrPtNo);
                frmHistory2 frmHistory2X = new frmHistory2("SUB", mstrPtNo, mstrGuBun, mstrFDate, mstrTDate);
                sup.setCtrlLoad(panel1, frmDietOrderNewX);
                sup.setCtrlLoad(panel2, frmHistory2X);
            }
            else
            {
                frmDietOrderNew frmDietOrderNewX = new frmDietOrderNew("SUB", mstrPtNo);
                frmHistory2 frmHistory2X = new frmHistory2("SUB", mstrPtNo, mstrGuBun, mstrFDate, mstrTDate);
                sup.setCtrlLoad(panel1, frmDietOrderNewX);
                sup.setCtrlLoad(panel2, frmHistory2X);
            }
            //}

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
