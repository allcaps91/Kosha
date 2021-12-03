using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using System.Xml;

namespace ComSupLibB
{
    public partial class frmComSupLbExSEND03 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        string gStrURL = "";
        byte[] gPostData = null;

        public delegate void EventExit();
        public event EventExit rEventExit;

        public delegate void SendText(string strText);
        public event SendText rSendText;

        #endregion


        #region MainFormMessage InterFace

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

        #endregion

        public frmComSupLbExSEND03(MainFormMessage pform, string pStrUrl, byte[] pPostData)
        {
            InitializeComponent();
            this.mCallForm = pform;

            gStrURL = pStrUrl;
            gPostData = pPostData;

            setEvent();
        }

        public frmComSupLbExSEND03(string pStrUrl, byte[] pPostData)
        {
            InitializeComponent();
            
            gStrURL = pStrUrl;
            gPostData = pPostData;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(eWebDocCompleted);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //else
            {
                //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                //ComFunc.ReadSysDate(clsDB.DbCon);

                if(gPostData == null)
                {
                    this.Close();
                    return;
                }

                else
                {
                    setCtrlWebBr();
                }                
            }
        }

        void setCtrlWebBr()
        {
            this.webBrowser1.ObjectForScripting = true;
            this.webBrowser1.Navigate("about:blank");

            try
            {
                this.webBrowser1.Navigate(gStrURL, "", gPostData, "Content-Type: application/x-www-form-urlencoded");
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox("질병관리본부 접속 오류(인터넷) 입니다. 화면 종료 후 다시 시도 하세요 " + ex.ToString());
                this.Close();
                return;
            }
            
        }

        void eWebDocCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(e.Url.ToString().IndexOf("https://152", 0) > -1)
            {
                

                HtmlDocument doc = this.webBrowser1.Document;
                string body = doc.Body.InnerText;                

                if (VB.Left(body, 2) == "\r\n")
                {
                    ComFunc.MsgBox("경고창에서 '아니오'를 클릭하셨습니다. 화면 종료 후 전송버튼 클릭 후 '예'를 눌러주십시오");
                    rSendText("False");
                    this.Close();
                    return;
                }

                else
                {
                    //string Text = VB.Mid(body, 198, 4);

                    int nTextIndex = body.IndexOf("<code_dt>");
                    string Text = VB.Mid(body, nTextIndex + 10, 4);

                    if(VB.Right(Text, 1) == "<")
                    {
                        Text = VB.Left(Text, 3);
                    }

                    //등록성공일 경우
                    if (Text == "201" || Text == "200" || Text == "2001")
                    {                        
                        rSendText("True");
                        this.Close();
                        return;
                    }

                    else
                    {                        
                        //ComFunc.MsgBox("등록실패!!" + "\r\n" + "화면 종료 후 다시 시도 하세요");                        
                        rSendText(Text);
                        this.Close();
                        return;
                    }
                }


                

                //HtmlElement he = doc.GetElementById(strTmp);
                //s = he.GetAttribute("value");
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }         
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
    }
}
