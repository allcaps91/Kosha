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
    /// File Name       : frmPmpaViewMessage.cs
    /// Description     : 확인화면
    /// Author          : 안정수
    /// Create Date     : 2017-10-02
    /// Update History  : 2017-10-30
    /// TODO : 실제 테스트 필요
    /// <history>       
    /// d:\psmh\OPD\jengsan\Frm메시지.frm(Frm메시지) => frmPmpaViewMessage.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jengsan\Frm메시지.frm(Frm메시지)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMessage : Form
    {
        string mstrRetValue = "";
        public frmPmpaViewMessage()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewMessage(string GstrRetValue)
        {
            InitializeComponent();
            setEvent();
            mstrRetValue = GstrRetValue;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
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

            txtSname.Text = "";
            txtJumin1.Text = "";
            txtJumin2.Text = "";
            
            if(mstrRetValue != "")
            {
                txtSname.Text = VB.Pstr(mstrRetValue, "^^", 1);
                txtJumin1.Text = VB.Pstr(mstrRetValue, "^^", 2);
                txtJumin2.Text = VB.Left(VB.Pstr(mstrRetValue, "^^", 3), 3) + "****";
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnCancel)
            {
                mstrRetValue = "";
                this.Close();
                return;
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                mstrRetValue = "OK";
                this.Close();
                return;
            }
        }     

    }
}
