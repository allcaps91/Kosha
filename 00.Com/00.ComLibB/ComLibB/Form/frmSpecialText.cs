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

/// <summary>
/// Description : 특수문자 선택
/// Author : 김형범
/// Create Date : 2017.06.16
/// </summary>
/// <history>
/// 부모폼에서 실행 확인 필요
/// </history>
namespace ComLibB
{
    /// <summary> 특수문자 </summary>
    public partial class frmSpecialText : Form
    {
        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendText(string strText);
        public event SendText rSendText;

        /// <summary> 특수문자 </summary>
        public frmSpecialText()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            rEventExit();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            rSendText(txtText.Text);
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtText.Text = ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            btnExit.Enabled = true;
            btnSelect.Enabled = true;
        }

        void frmSpecialText_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    //rEventExit(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
        }
    }
}
