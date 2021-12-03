using ComBase;
using System;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : Frm혈액사용예정일.cs
    /// Description     : 의사별 내시경건수 통계
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// <history>       
    /// 테스트 필요
    /// d:\psmh\Ocs\Frm혈액사용예정일.frm(Frm혈액사용예정일) => FrmMEdBloodUseDaySet.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Ocs\Frm혈액사용예정일.frm(Frm혈액사용예정일)
    /// </seealso>
    /// </summary>
    public partial class FrmMEdBloodUseDaySet : Form
    {
        public FrmMEdBloodUseDaySet()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnOK.Click += new EventHandler(eBtnEvent);
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                clsPublic.Gstr혈액사용예정일Date = "";
                this.Close();
            }

            else if (sender == this.btnOK)
            {
                //
                //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                btnOK_Click();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        void btnOK_Click()
        {
            if(dtpDate.Text == "")
            {
                ComFunc.MsgBox("날짜를 넣어주세요");
                dtpDate.Focus();
                return;
            }

            clsPublic.Gstr혈액사용예정일Date = dtpDate.Text;
            this.Close();
        }
    }
}
