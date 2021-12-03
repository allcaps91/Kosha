using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;

namespace ComNurLibB
{
    public partial class frmOpdNrRsvChange2 : Form
    {
        private string strPtno = "";

        public frmOpdNrRsvChange2()
        {
            InitializeComponent();
        }

        public frmOpdNrRsvChange2(string strPtno)
        {
            InitializeComponent();
            this.strPtno = strPtno;
        }

        private void frmOpdNrRsvChange2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = strPtno;

        }




    }
}
