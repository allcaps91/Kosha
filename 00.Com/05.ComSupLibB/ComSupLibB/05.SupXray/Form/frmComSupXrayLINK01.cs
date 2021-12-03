using ComBase;
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayLINK01.cs
    /// Description     : 영상의학 MVIEW 연동 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-12-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\etc\Mview\FrmMView.frm(FrmMView) >> frmComSupXrayLINK01.cs 폼이름 재정의" />
    public partial class frmComSupXrayLINK01 : Form, MainFormMessage 
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();        
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSQL comSql = new clsComSQL();
        clsComSup sup = new clsComSup();

        string gSabun = "";
        string gPass = "";

        #endregion

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        public frmComSupXrayLINK01()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupXrayLINK01(string argSabun,string argPass)
        {
            InitializeComponent();
            gSabun = argSabun;
            gPass = argPass;
            setEvent();
        }

        public frmComSupXrayLINK01(MainFormMessage pform, string argSabun, string argPass)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //로그인 모듈안에 권한 체크함
                if (mview_login(gSabun,gPass)==true)
                {
                    this.Close();
                    return;
                }
                else
                {
                    ComFunc.MsgBox("로그인실패!!");
                    this.Close();
                    return;
                }
            }
        }

        void eFormResize(object sender, EventArgs e)
        {
            
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        bool mview_login(string argSabun,string argPass)
        {
            if (argSabun =="" && argPass =="")               
            {
                clsPacs.PACS_Image_View_AES(clsType.User.IdNumber, clsSHA.SHA256(clsType.User.PasswordChar));
            }
            else
            {
                clsPacs.PACS_Image_View_AES(argSabun, argPass);
            }
            

            return true;
        }

    }
}
