using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupIjrm
{
    /// <summary>
    /// Class Name      : SupIjrm
    /// File Name       : frmIjrmVACC02.cs
    /// Description     : 접종내역조회
    /// Author          : 김홍록
    /// Create Date     : 2017-07-19
    /// Update History  : 
    /// </summary>
    /// <history>     
    /// </history>
    /// <seealso cref= "d:\psmh\Ocs\OpdOcs\ojumst\Frm접종내역조회.frm" />
    public partial class frmComSupIjrmVACC01 : Form
    {

        clsComSQL comSql = new clsComSQL();

        string gStrURL;
        string gStrNbnTyp;
        string gstrHldResNum;
        string gstrBabyType;
        string gStrPtno;

        /// <summary>생성자</summary>
        public frmComSupIjrmVACC01(string strPtno,string strNbnTyp = "", string strHldResNum = "", string strBabyType = "")
        {
            InitializeComponent();

            this.gStrPtno = strPtno;
            this.gStrNbnTyp = strNbnTyp;
            this.gstrHldResNum = strHldResNum;
            this.gstrBabyType = strBabyType;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Move += new EventHandler(eFromMove);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnSearchClick);

            this.webBr.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(eWebDocCompleted);

            this.ucSupComPtSearch1.ePSMH_UcSupComPtSearch_VALUE += new ComSupLibB.UcSupComPtSearch.PSMH_RETURN_VALUE(ePSMH_PTINFO);


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

        void eFromMove(object sender, EventArgs e)
        {
            Point p = new Point();

            p.X = this.Location.X + this.ucSupComPtSearch1.Location.X;
            p.Y = this.Location.Y + this.ucSupComPtSearch1.Location.Y + this.ucSupComPtSearch1.Height * 4;

            this.ucSupComPtSearch1.pPSMH_LPoint = p;

        }

        void setCtrl()
        {
            setCtrlText();
            setCtrlWebBr(true);

            this.eBtnSearchClick(this.btnSearch, new EventArgs());
        }

        void setCtrlText()
        {
            this.ucSupComPtSearch1.txtSearch_PtInfo.Text = this.gStrPtno;
            this.ucSupComPtSearch1.setSname();

        }

        void eBtnSearchClick(object sender, EventArgs e)
        {
            bool b = chkSearch();

            if (b == true)
            {
                setCtrlCircular(true);
                setCtrlWebBr(false);
            }
        }

        bool chkSearch()
        {

            if (string.IsNullOrEmpty(this.txtJUMIN1.Text) == true)
            {
                ComFunc.MsgBox("주민번호 앞 자리는 반드시 존재 해야 합니다.");
                return false;
            }

            if (string.IsNullOrEmpty(this.txtJUMIN2.Text) == true)
            {
                ComFunc.MsgBox("주민번호 뒷 자리는 반드시 존재 해야 합니다.");
                return false;
            }

            return true;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }            
        }

        void ePSMH_PTINFO(object sender, string pano, string sname)
        {
            if (string.IsNullOrEmpty(pano) == false)
            {
                DataTable dt = comSql.sel_BAS_PATIENT(clsDB.DbCon, pano);

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    this.txtJUMIN1.Text = dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.JUMIN1].ToString();

                    if (string.IsNullOrEmpty(dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.JUMIN3].ToString() ) == false)
                    {
                        this.txtJUMIN2.Text = clsAES.DeAES(dt.Rows[0][(int)clsComSQL.enmSel_BAS_PATIENT.JUMIN3].ToString());
                    }
                    else
                    {
                        this.txtJUMIN2.Text = "";
                        ComFunc.MsgBox("주민번호 뒷자리가 존재 하지 않습니다.");
                    }
                }
                else
                {
                    ComFunc.MsgBox("환자정보가 존재 하지 않습니다.");
                }
            }
            
        }

        void eWebDocCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Debug.WriteLine(e.Url.ToString());

            //if (e.Url.ToString().IndexOf("is.cdc.go.kr", 0) > -1)
            if (e.Url.ToString().IndexOf("is.kdca.go.kr", 0) > -1)
            {
                setCtrlCircular(false);
            }
        }

        void setCtrlWebBr(bool isClear)
        {
           
            if (isClear == true)
            {
                this.webBr.ObjectForScripting = true;
                this.webBr.Navigate("about:blank");
                //this.gStrURL = "https://is.cdc.go.kr/tprevent/client_metrics.asp?PatResNum=";
                this.gStrURL = "https://is.kdca.go.kr/tprevent/client_metrics.asp?PatResNum=";
            

            }
            else
            {
                //2017.09.19.김홍록 : 이전 버젼
                //this.gStrURL = "https://is.cdc.go.kr/tprevent/client_metrics.asp?PatResNum=" + txtJUMIN1.Text + txtJUMIN2.Text + "&NbnTyp=" + this.gStrNbnTyp + "&HldResNum=" + this.gstrHldResNum + "&BabyType=" + this.gstrBabyType;

                // VB Sample : https://is.cdc.go.kr/tprevent/client_metrics.asp?PatResNum=7008232574510&NbnTyp=&HldResNum=&BabyType=
                // C# Sample : https://is.cdc.go.kr/tprevent/client_metrics.asp?PatResNum=0809123702715&NbnTyp=&HldResNum=&BabyType=


                // ********  << 2017.09.19.김홍록 : 메일 내용 >>  ***********************************************************************************
                //
                //  안녕하세요.질병관리본부 예방접종관리과 유지보수 개발팀입니다.
                //  문의주신 내용에 대한 답변드립니다.
                    
                //  *URL
                //  개발 = http://is.picoit.co.kr/iris/index_ocs_docs.jsp
                //  운영 = https://is.cdc.go.kr/iris/index_ocs_docs.jsp
                //    *PARAMETER
                //        service     = getVcnInfo
                //        PatResNum   = 피접종자 주민등록번호
                //        NbnTyp      = 신생아여부(Y: 신생아일 경우)
                //        HldResNum   = 보호자주민등록번호(피접종자가 신생아일 경우 필수)
                //        BabyType    = 쌍둥이구분코드(1:첫째, 2:둘쨰)
                //        OrgCod      = 의료기관코드(8자리숫자)
                //        vType       = 화면타입(NULL: html타입, XML: xml타입)

                //    * ex) 개발서버 테스트용 입니다.(userKey: irr11111111)
                //    13자리 주민등록번호 :                http://is.picoit.co.kr/iris/index_ocs_docs.jsp?service=getVcnInfo&PatResNum=9911111111111&NbnTyp=&HldResNum
                //                    7자리 주민등록번호 : http://is.picoit.co.kr/iris/index_ocs_docs.jsp?service=getVcnInfo&PatResNum=1701023&NbnTyp=Y&HldResNum=9911111111111
                //    ※ 담당자: 민경원 대리(043 - 238 - 2913, super13oy@naver.com)
                //
                // *********************************************************************************************************************************************

                //this.gStrURL = "https://is.cdc.go.kr/iris/index_ocs_docs.jsp?service=getVcnInfo&PatResNum=" + txtJUMIN1.Text + txtJUMIN2.Text + "&NbnTyp=" + this.gStrNbnTyp + "&HldResNum=" + this.gstrHldResNum + "&BabyType=" + this.gstrBabyType;
                this.gStrURL = "https://is.kdca.go.kr/iris/index_ocs_docs.jsp?service=getVcnInfo&PatResNum=" + txtJUMIN1.Text + txtJUMIN2.Text + "&NbnTyp=" + this.gStrNbnTyp + "&HldResNum=" + this.gstrHldResNum + "&BabyType=" + this.gstrBabyType;
                this.webBr.Navigate(gStrURL);
            }

        }

        void setCtrlCircular(bool b)
        {
            this.circProgress.Visible = b;
            this.circProgress.IsRunning = b;
        }
    }
}
