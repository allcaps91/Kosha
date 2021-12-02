using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupHELP04.cs
    /// Description     : 진단검사의학과 HELP
    /// Author          : 김홍록
    /// Create Date     : 2018-02-13
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "exmain\FrmMsg.frm" />
    public partial class frmComSupLbExHELP04 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        string gStrMasterCode;

        public frmComSupLbExHELP04(string strMasterCode)
        {
            InitializeComponent();

            this.gStrMasterCode = strMasterCode;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);            

        }

        void setCtrl()
        {
            this.timer1.Start();
            this.lblTEXT.Text = this.gStrMasterCode;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            byte[] colorBytes = new byte[3];

            rand.NextBytes(colorBytes);

            Color randomColor = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);

            this.lblTEXT.ForeColor = randomColor;
            
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
