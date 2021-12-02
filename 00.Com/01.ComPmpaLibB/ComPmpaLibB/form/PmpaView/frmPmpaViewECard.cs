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

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewECard.cs
    /// Description     : 인터넷 카드 조회
    /// Author          : 안정수
    /// Create Date     : 2017-08-24
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\oumsad\Frm인터넷카드조회.frm(Frm인터넷카드조회) => frmPmpaViewECard.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm인터넷카드조회.frm(Frm인터넷카드조회)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewECard : Form
    {
        string mstr신용카드결제사 = "";
        public frmPmpaViewECard()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewECard(string Gstr신용카드결제사)
        {
            InitializeComponent();
            setEvent();
            mstr신용카드결제사 = Gstr신용카드결제사;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.optCard0.CheckedChanged += new EventHandler(eBtnEvent);
            this.optCard1.CheckedChanged += new EventHandler(eBtnEvent);
            this.optCard2.CheckedChanged += new EventHandler(eBtnEvent);
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

            optCard0.Checked = true;

            if (mstr신용카드결제사 == "1")
            {
                optCard1.Checked = true;
            }
            else
            {
                optCard0.Checked = true;
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.optCard0)
            {
                webBrowser1.Navigate("http://store.ikoces.com");
            }

            else if (sender == this.optCard1)
            {
                webBrowser1.Navigate("http://www.starfriend.co.kr/starplus");
            }

            else if (sender == this.optCard2)
            {
                webBrowser1.Navigate("https://bo.ubpay.com:446/uboffice/index.jsp");             
            }

        }

    }
}
