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

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaJengSanYearEndWeb.cs
    /// Description     : 연말정산 제출자료 웹 확인
    /// Author          : 이정현
    /// Create Date     : 2018-08-16
    /// <history> 
    /// 연말정산 제출자료 웹 확인
    /// </history>
    /// <seealso>
    /// PSMH\OPD\jengsan\Frm연말정산웹확인.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\jengsan\jengsan.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpaJengSanYearEndWeb : Form
    {
        public frmPmpaJengSanYearEndWeb()
        {
            InitializeComponent();
        }

        private void frmPmpaJengSanYearEndWeb_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.yesone.go.kr/cdata/index.jsp");
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.yesone.go.kr/cdata/index.jsp");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
