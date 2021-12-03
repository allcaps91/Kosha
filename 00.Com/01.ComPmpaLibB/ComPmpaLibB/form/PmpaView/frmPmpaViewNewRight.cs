using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewNewRight.cs
    /// Description     : 자격조회 [ 주민번호1 + 증기호 ]
    /// Author          : 안정수
    /// Create Date     : 2017-08-23
    /// Update History  : 2017-11-02
    /// <history>       
    /// 실제 테스트 필요
    /// d:\psmh\OPD\oumsad\Frm신자격.frm(Frm신자격) => frmPmpaViewNewRight.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm신자격.frm(Frm신자격)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewNewRight : Form
    {
        ComFunc CF = new ComFunc();
        string mstrHelpCode = "";

        public frmPmpaViewNewRight()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewNewRight(string GstrHelpCode)
        {
            InitializeComponent();
            setEvent();
            mstrHelpCode = GstrHelpCode;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btn_nhic.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
            string strSname = "";
            string strJumin1 = "";
            string strGKiho = "";

            txtSname.Text = "";
            txtJumin1.Text = "";
            txtBiGkiho.Text = "";

            if (mstrHelpCode != "")
            {
                strSname = VB.Pstr(mstrHelpCode, ",", 1).Trim();
                strJumin1 = VB.Pstr(mstrHelpCode, ",", 2).Trim();
                strGKiho = VB.Pstr(mstrHelpCode, ",", 3).Trim();

                txtSname.Text = strSname;
                txtJumin1.Text = strJumin1;
                txtBiGkiho.Text = strGKiho;
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnCancel)
            {
                btnCancel_Click();
            }

            else if (sender == this.btn_nhic)
            {
                btn_nhic_Click();
            }
        }

        void btn_nhic_Click()
        {
            if (txtSname.Text == "")
            {
                ComFunc.MsgBox("이름을 넣고 자격조회를 하십시오. ");
                return;
            }

            if (txtJumin1.Text == "")
            {
                ComFunc.MsgBox("주민번호1을 넣고 자격조회를 하십시오. ");
                return;
            }

            if (VB.Len(txtJumin1.Text) != 6)
            {
                ComFunc.MsgBox("주민번호 자릿수가 6자리를 확인후 자격조회를 하십시오. ");
                return;
            }

            if (txtBiGkiho.Text == "")
            {
                ComFunc.MsgBox("보험증번호를 넣고 자격조회를 하십시오. ");
                return;
            }

            clsPublic.GstrHelpCode = txtSname.Text.Trim() + "," + txtJumin1.Text.Trim() + "," + txtBiGkiho.Text.Trim();
            this.Close();
        }

        void btnCancel_Click()
        {
            clsPublic.GstrHelpCode = "";
            this.Close();
        }

        void txtSname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtJumin1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtBiGkiho_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btn_nhic.Focus();
            }
        }
    }
}
