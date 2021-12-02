using System;
using System.Data;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using ComNurLibB;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmNurseNotice.cs
    /// Description     : 간호부 공지사항
    /// Author          : 안정수
    /// Create Date     : 2018-02-09
    /// Update History  : 
    /// TODO : 파일 내려받는부분 구현 필요, 테스트 필요
    /// </summary>
    /// <history>  
    /// 기존 FrmCommonKnowView.frm(FrmCommonKnowView) 폼 frmNurseNotice.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\mtsEmr\FrmCommonKnowView.frm(FrmCommonKnowView) >> frmNurseNotice.cs 폼이름 재정의" />
    public partial class frmNurseNotice : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsIpdNr CIN = new clsIpdNr();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

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

        public frmNurseNotice(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        string mstrWRTNO = "";


        public frmNurseNotice(MainFormMessage pform, string strWRTNO)
        {
            InitializeComponent();

            mstrWRTNO = strWRTNO;

            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnFileDown.Click += new EventHandler(eBtnClick);

            this.btnPrint.Click += new EventHandler(eBtnPrint);
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
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                Set_Init();
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnFileDown)
            {
                btnFileDown_Click();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void Set_Init()
        {
            if (mstrWRTNO == "")
            {
                ComFunc.MsgBox("조회된 자료가 없습니다.");
                this.Close();
                return;
            }

            eGetData();
        }

        void btnFileDown_Click()
        {
            if (txtFileName.Text.Trim() == "")
            { return; }

            if (txtAppendNo.Text.Trim() == "")
            { return; }

            string strFile = "";
            string strLocal = "";
            string strServerNameT = "192.168.100.33";
            string strUserNameT = "pcnfs";
            string strPasswordT = "pcnfs1";
            string strHostDir = "/pcnfs/nurCKnow/";

            strFile = txtAppendNo.Text.Trim();

            Ftpedt FtpedtX = new Ftpedt();

            if (FtpedtX.FtpConnetBatch(strServerNameT, strUserNameT, strPasswordT) == false)
            {
                ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                return;
            }

            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Title = "저장";
            SaveFile.Filter = "모든 파일 (*.*)|*.*|한글 문서(*.HWP)|*.HWP|엑셀 문서(*.XLS)|*.XLS|워드 문서(*.DOC)|*.DOC";
            SaveFile.FileName = txtFileName.Text.Trim();
            SaveFile.ShowDialog();

            strLocal = SaveFile.FileName;

            if (strLocal == "")
            {
                ComFunc.MsgBox("저장 취소", "취소");
                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
                return;
            }

            if (FtpedtX.FtpDownloadEx(strServerNameT, strUserNameT, strPasswordT, strLocal, strHostDir + strFile, strHostDir) == false)
            {
                ComFunc.MsgBox("다운로드 실패", "종료");
                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
                return;
            }

            FtpedtX.FtpDisConnetBatch();
            FtpedtX = null;

            ComFunc.MsgBox("다운로드 완료");

        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTemp = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;


            strTemp = VB.InputBox("수간호사의 이름을 입력하여 주십시요.");

            ssPrt.ActiveSheet.Cells[4, 5].Text = strTemp;

            ssPrt.ActiveSheet.Cells[10, 4].Text = clsPublic.GstrSysDate;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            SPR.setSpdPrint(ssPrt, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Both);
        }

        void eGetData()
        {
            //string strBUCODE = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  WRTNO, CDATE, SABUN, BUCODE, TITLE, REMARK, REPEAT, IMPORTANCE, DELDATE, PHARMACY, DOC_SOURCE, AppendFileName, AppendNo ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_CADEX_CKNOW";
                SQL += ComNum.VBLF + "WHERE WRTNO = " + mstrWRTNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblTitle.Text = (dt.Rows[0]["PHARMACY"].ToString().Trim() == "1" ? "(★약)" : "") + dt.Rows[0]["TITLE"].ToString().Trim();
                    txtRemark.Rtf = dt.Rows[0]["Remark"].ToString().Trim();
                    lblDoc_Source.Text = dt.Rows[0]["DOC_SOURCE"].ToString().Trim();
                    txtFileName.Text = dt.Rows[0]["AppendFileName"].ToString().Trim();
                    txtAppendNo.Text = dt.Rows[0]["AppendNo"].ToString().Trim();

                    if (lblDoc_Source.Text != "")
                    {
                        lblDoc_Source.Text = "★ 근거문서 : " + lblDoc_Source.Text;
                    }

                    ssPrt.ActiveSheet.Cells[4, 3].Text = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
                    ssPrt.ActiveSheet.Cells[4, 5].Text = "";    //수간호사
                    ssPrt.ActiveSheet.Cells[4, 7].Text = "";    //서명

                    ssPrt.ActiveSheet.Cells[5, 3].Text = lblTitle.Text;
                    ssPrt.ActiveSheet.Cells[9, 2].Text = txtRemark.Text;
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
