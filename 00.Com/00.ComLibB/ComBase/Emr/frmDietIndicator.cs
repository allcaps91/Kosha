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

namespace ComBase
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDietIndicator.cs
    /// Description     : 영양초기평가/재평가 기본지표
    /// Author          : 박창욱
    /// Create Date     : 2018-05-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\diet\dietorder\Frm영양평가지표.frm(Frm영양평가지표.frm) >> frmDietIndicator.cs 폼이름 재정의" />	
    public partial class frmDietIndicator : Form
    {
        public frmDietIndicator()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDietIndicator_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }
    }
}
