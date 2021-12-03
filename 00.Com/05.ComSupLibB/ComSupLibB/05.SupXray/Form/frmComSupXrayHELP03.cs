using ComBase; //기본 클래스
using System;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayHELP03.cs
    /// Description     : 영상의학과 수동접수시 부위선택 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-07-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\xumain29.frm (FrmViewSlipSub) >> frmComSupXrayHELP03.cs 폼이름 재정의" />
    public partial class frmComSupXrayHELP03 : Form
    {
        #region 클래스 선언 및 etc....

        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupXray.SupXrayBase XBase = new clsComSupXray.SupXrayBase();

        #endregion

        public frmComSupXrayHELP03()
        {
            InitializeComponent();

            setEvent();

        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
                        

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

                screen_clear();
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSearch)
            {
                setData();
            }

        }

        void screen_clear()
        {
            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }
            }
        }

        void setData()
        {

            XBase.GstrBuwiHelp = "";

            Control[] controls = ComFunc.GetAllControls(this);
            foreach (Control ctl in controls)
            {
                if(ctl is CheckBox)
                {
                    if(((CheckBox)ctl).Checked == true)
                    {
                        XBase.GstrBuwiHelp += ctl.Text +",";
                    }
                }
                else if (ctl is RadioButton)
                {
                    if (((RadioButton)ctl).Checked == true)
                    {
                        XBase.GstrBuwiHelp += ctl.Text + ",";
                    }
                }

            }

            this.Close();

        }
    }
}
