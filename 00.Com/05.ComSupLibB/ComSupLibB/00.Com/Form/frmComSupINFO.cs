using System;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupINFO.cs
    /// Description     : 메모놓기
    /// Author          : 김홍록
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// </summary>
    /// <history>   
    /// 기존 FrmEkgMemo.frm(FrmEkgMemo) 폼 frmComSupEkgMemo.cs 으로 변경함
    /// </history>
    /// <seealso cref= "신규" />
    public partial class frmComSupINFO : Form
    {

        string gStrTitle = string.Empty;
        string gStrTitleSub = string.Empty;
        string gStrContaint = string.Empty;

        public frmComSupINFO(string strTitle, string strTitleSub, string strContaint)
        {
            InitializeComponent();

            this.gStrTitle = strTitle;
            this.gStrTitleSub = strTitleSub;
            this.gStrContaint = strContaint;           

            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }


        void eFormLoad(object sender, EventArgs e)
        {
            this.setCtrlTitle();                    
        }

        void eBtnClick(object sender, EventArgs e)
        {
            this.Close();
        }


        void setCtrlTitle()
        {
            this.lblTitle.Text      = this.gStrTitle;
            this.lblTitleSub0.Text  = this.gStrTitleSub;

            this.txtInfo.Text = this.gStrContaint;            

        }
        
    }
}
