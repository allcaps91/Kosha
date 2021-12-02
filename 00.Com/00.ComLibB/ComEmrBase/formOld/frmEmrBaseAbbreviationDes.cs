using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmEmrBaseAbbreviationDes.cs
    /// Description     : 금지약어화면
    /// Author          : 이정현
    /// Create Date     : 2018-05-23
    /// <history> 
    /// 금지약어화면
    /// </history>
    /// <seealso>
    /// PSMH\mid\midu000\Frm금지약어.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\mid\midu000.vbp
    /// </vbp>
    /// </summary>
    public partial class frmEmrBaseAbbreviationDes : Form
    {
        public frmEmrBaseAbbreviationDes()
        {
            InitializeComponent();
        }

        private void frmEmrBaseAbbreviationDes_Load(object sender, EventArgs e)
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
