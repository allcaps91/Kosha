using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupFnExABR02.cs
    /// Description     : 기능검사 ABR 스케쥴 보기
    /// Author          : 윤조연
    /// Create Date     : 2017-06-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 ABR 스케쥴 조회 명단폼 frmComSupFnExSCH02.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\ekg\FrmABRSchedule.frm(FrmABRSchedule) >> frmComSupFnExSCH02.cs 폼이름 재정의" />
    public partial class frmComSupFnExSCH02 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupFnExSQL fnExSql = new clsComSupFnExSQL();
        

        string[] strDay = null;

        string gPart = ""; //01:ABR, 02:외과 유방

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

        public frmComSupFnExSCH02()
        {
            InitializeComponent();
            gPart = "01";
            setEvent();

        }

        public frmComSupFnExSCH02(string argPart)
        {
            InitializeComponent();
            gPart = argPart;
            setEvent();

        }

        public frmComSupFnExSCH02(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;
            gPart = "01";
            setEvent();
        }

        public frmComSupFnExSCH02(MainFormMessage pform,string argPart)
        {
            InitializeComponent();

            this.mCallForm = pform;
            gPart = argPart;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            //txtMonth.Text = VB.Left( cpublic.strSysDate,4) +"년 " + VB.Mid(  cpublic.strSysDate,6,2)  + "월";

            sup.setYYYYMM(cboYYMM, cpublic.strSysDate, 24, 12, 12, "-");


            //생성자에 따라 타이틀 표시
            if (gPart =="01")
            {
                lblTitle.Text = "ABR 스케쥴 조회";
                btnSave.Text = "ABR 검사 스케쥴 등록";
            }
            else if (gPart == "02")
            {
                lblTitle.Text = "외과 유방검사 스케쥴 조회";
                btnSave.Text = "외과 유방검사 스케쥴 등록";
            }
            else
            {
                lblTitle.Text = "ABR 스케쥴 조회";
                btnSave.Text = "ABR 검사 스케쥴 등록";
            }
            


        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnSearch2.Click += new EventHandler(eBtnEvent);
            this.btnSearch3.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);

            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);

            //this.ssList.CellDoubleClick += ssList_CellDoubleClick;


            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboEvent);

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

                screen_clear();

                setCtrlData();

                //기본 조회
                GetData(clsDB.DbCon, ssList);
            }
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnPrint)
            {
                //인쇄
                ePrint();

            }
            else if (sender == this.btnSave)
            {
                //저장
                frmComSupFnExSET01 f = new frmComSupFnExSET01(gPart);
                f.ShowDialog();
                GetData(clsDB.DbCon,ssList);
            }
            else if (sender == this.btnSearch1)
            {
                GetData(clsDB.DbCon,ssList);
            }
            else if (sender == this.btnSearch3)
            {
                setMonth(true);
                GetData(clsDB.DbCon,ssList);
            }
            else if (sender == this.btnSearch2)
            {
                setMonth(false);
                GetData(clsDB.DbCon,ssList);
            }

        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "이비인후과 ABR 스케쥴 "+"(" + cboYYMM.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false,(float)0.92);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            
        }

        void screen_clear()
        {

            read_sysdate();

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

            //clear
            ssList.ActiveSheet.ClearRange(0, 0, (int)ssList.ActiveSheet.RowCount, (int)ssList.ActiveSheet.ColumnCount, true);

        }

        void setMonth(bool bADD )
        {
            
            string strMonth = cboYYMM.Text.Trim().Replace("년","").Replace("월","").Replace(" ","").Trim();
            string strYYYY = VB.Left( strMonth,4);
            string strMM = "";
            strMM = ComFunc.SetAutoZero((bADD==true ? Convert.ToInt16(VB.Right(strMonth, 2)) +1 : Convert.ToInt16(VB.Right(strMonth, 2)) - 1).ToString(),2);
            if (strMM =="13")
            {
                strYYYY = (Convert.ToInt16( strYYYY) + 1).ToString();
                strMM = "01";

            }
            else if (strMM == "00")
            {
                strYYYY = (Convert.ToInt16(strYYYY) - 1 ).ToString();
                strMM = "12";
            }

            cboYYMM.Text = strYYYY + "년 " + strMM + "월";
                        

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {   
            string strTemp = (cboYYMM.Text.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
            string strSDate = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";
            string strTDate = Convert.ToString(Convert.ToDateTime(strSDate).AddMonths(1).AddDays(-1).ToShortDateString());

            read_sysdate();

            int start =  Convert.ToInt32(Convert.ToDateTime(strSDate).DayOfWeek);

            //달력세팅 및 데이타 표시
            disp_Calendar(pDbCon, Spd, strSDate, strTDate,start);                    


        }              
                
        void disp_Calendar(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argSDate, string argTDate,int start)
        {
            int nRow = 0;
            int nCol = 0;
            string strTemp = "";
            DataTable dt = null;

            //휴일체크
            strDay = sup.read_huil( pDbCon, argSDate, argTDate);            
                       
            //clear
            Spd.ActiveSheet.ClearRange(0, 0, (int)Spd.ActiveSheet.RowCount, (int)Spd.ActiveSheet.ColumnCount, true);

            nCol = start;

            for (int i = 0; i <  Convert.ToInt16(VB.Right( argTDate,2)) ; i++)
            {

                Spd.ActiveSheet.Cells[nRow, nCol].Text = (i+1).ToString();
                Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                Spd.ActiveSheet.Cells[nRow, nCol].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                if (strDay[i]=="토")
                {
                    Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(40, 30, 230);
                }
                if (strDay[i] == "일")
                {
                    Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(255,0,0);
                }
                if (strDay[i] == "*")
                {
                    Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }

                //쿼리실행            
                dt = fnExSql.sel_ExamABRSch2(pDbCon, gPart, VB.Left( argSDate,8)+ ComFunc.SetAutoZero( (i+1).ToString(),2));

                if (dt.Rows.Count>0)
                {
                    strTemp +=(i + 1).ToString() + "\r\n";

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {                        
                        strTemp += "(" + dt.Rows[j]["RDate3"].ToString().Trim() + ")";
                        strTemp += dt.Rows[j]["SName"].ToString().Trim() + " ";
                        strTemp += dt.Rows[j]["EXAMNAME"].ToString().Trim() + "\r\n";
                                                
                    }

                    Spd.ActiveSheet.Cells[nRow, nCol].Text = strTemp;                    
                }

                dt = null;
                strTemp = "";

                nCol++;
                if (nCol > 6)
                {
                    nRow++;
                    nCol = 0;
                }
                if (nRow > 4) nRow = 0;
                

            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
        
    }
}
