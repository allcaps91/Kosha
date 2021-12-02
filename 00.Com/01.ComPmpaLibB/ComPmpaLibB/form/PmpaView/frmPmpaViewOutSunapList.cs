using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewIOSunapList.cs
    /// Description     : 퇴원수남점검리스트
    /// Author          : 김해수
    /// Create Date     : 2018-08-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\IPD\iument\Frm퇴원수납점검리스트.frm(Frm퇴원수납점검리스트) >> frmPmpaViewIOSunapList.cs 폼이름 재정의" />
    public partial class frmPmpaViewOutSunapList : Form
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();

        // Thread thread;
        // FpSpread spd;	

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
        #endregion
        public frmPmpaViewOutSunapList()
        {
            InitializeComponent();
            setEvent();
        }
        public frmPmpaViewOutSunapList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }
        void setCtrlData()
        {
            setCombo();
        }
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            ////탭버튼클릭 이벤트
            //this.tab1.Click += new EventHandler(eTabClick);
            //this.tab2.Click += new EventHandler(eTabClick);
            //this.tab3.Click += new EventHandler(eTabClick);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            //this.btnSearch2.Click += new EventHandler(eBtnSearch);
            //this.btnSearch5.Click += new EventHandler(eBtnSearch);
            //this.btnSearch6.Click += new EventHandler(eBtnSearch);
            //this.btnSearch8.Click += new EventHandler(eBtnSearch);
            //this.btnSearch9.Click += new EventHandler(eBtnSearch);

            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnSave2.Click += new EventHandler(eBtnSave);
            //this.btnSave3.Click += new EventHandler(eBtnSave);
            //this.btnSave5.Click += new EventHandler(eBtnSave);
            //this.btnSave_Session.Click += new EventHandler(eBtnSave);
            //this.btnMemo.Click += new EventHandler(eBtnSave);

            this.btnPrint.Click += new EventHandler(eBtnPrint);  //스프레드 방식 출력
            //this.btnPrint2.Click += new EventHandler(eBtnPrint);
            //this.btnPrint3.Click += new EventHandler(eBtnPrint); //Print.print 출력방식



            ////명단 더블클릭 이벤트
            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            //this.ssList.EditModeOff += new EventHandler(eSpreadEditOff);

            //this.cboGubun.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboPart.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboAmPm.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboIO.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            //this.cboSleep.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.txtSearch.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.dtpDate.ValueChanged += new EventHandler(eDtpValueChanged);


        }
        void setCombo()
        {

        }
        void setTxtTip()
        {

        }
        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //설정세팅
            //}
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
                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

            }
        }
        void eFormResize(object sender, EventArgs e)
        {
            //setCtrlProgress();
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
        }
        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
            }
        }
        void screen_clear(string Job = "")
        {
            read_sysdate();

            if (Job == "")
            {
                dtpDate.Text = cpublic.strSysDate;
            }
            else if (Job == "Clear")
            {
                ssList.ActiveSheet.Rows.Count = 0;
            }
        }
        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, dtpDate.Text.Trim());
        }
        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate)
        {
            DataTable dt = null;

            #region //데이터 셋 읽어 자료 표시
            int i = 0;
            string nAmt = "";

            screen_clear("Clear");

            dt = cSQL.sel_VIEW01_Check(pDbCon, argDate);

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nAmt = dt.Rows[i]["Amt"].ToString().Trim();

                    if (nAmt != "0" && nAmt != null)
                    {
                        Spd.ActiveSheet.Rows.Count = Spd.ActiveSheet.Rows.Count + 1;

                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.Rows.Count - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.Rows.Count - 1, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.Rows.Count - 1, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.Rows.Count - 1, 3].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.Rows.Count - 1, 4].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        Spd.ActiveSheet.Cells[Spd.ActiveSheet.Rows.Count - 1, 5].Text = ComFunc.vbFormat(nAmt, "#,##0");
                    }
                }
            }

            dt.Dispose();
            dt = null;
            #endregion

            return;
        }
        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                ePrint();
            }
        }
        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            // strTitle = "내시경 예약자 명단 " + "(" + dtpFDate.Text.Trim() + ")";
            strTitle = "퇴원수납 점검 리스트";

            //strSubTitle += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);
            strSubTitle += SPR.setSpdPrint_String("작업일자:" + dtpDate.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, false);

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
            //strFooter = "";

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 100, 0, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion
        }
    }
}
