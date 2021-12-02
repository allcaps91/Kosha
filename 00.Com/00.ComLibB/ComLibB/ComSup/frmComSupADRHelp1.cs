using ComBase;
using System;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupADRHelp1.cs
    /// Description     : 약물이상반응의 중증도 분류 설명
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 약물이상반응의 중증도 분류 설명
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\FrmADRHelp1.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupADRHelp1 : Form
    {
        public frmComSupADRHelp1()
        {
            InitializeComponent();
        }

        private void frmComSupADRHelp1_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
