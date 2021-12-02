using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupFnExSCH01.cs
    /// Description     : 산부인과 영상의학검사 스케쥴 
    /// Author          : 윤조연
    /// Create Date     : 2017-07-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 ABR 스케쥴 조회 명단폼 frmComSupFnExSCH01.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\FrmSchedule_OG.frm(FrmSchedule_OG) >> frmComSupFnExSCH01.cs 폼이름 재정의" />
    public partial class frmComSupFnExSCH01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupFnExSQL fnExSql = new clsComSupFnExSQL();
       

        string[] strDay = null;

        string gDept = "";

        #endregion

        public frmComSupFnExSCH01(string argDept)
        {
            InitializeComponent();

            gDept = argDept;                        

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            //txtMonth.Text = VB.Left(cpublic.strSysDate, 4) + "년 " + VB.Mid(cpublic.strSysDate, 6, 2) + "월";

            sup.setYYYYMM(cboYYMM, cpublic.strSysDate, 24, 12, 12, "-");

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnSearch2.Click += new EventHandler(eBtnEvent);
            this.btnSearch3.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);

            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboEvent);

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

                if (gDept != "OG")
                {
                    this.Close();
                    return;
                }

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
                string sDate = read_sDate(cboYYMM.Text.Trim());
                string tDate = Convert.ToString(Convert.ToDateTime(sDate).AddMonths(1).AddDays(-1).ToShortDateString());
                frmComSupEXSET01 f = new frmComSupEXSET01(sDate,tDate, "XRAY");
                f.ShowDialog();

                screen_display();
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

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            FpSpread s = (FpSpread)sender;

            string sDate = VB.Left(read_sDate(cboYYMM.Text.Trim()), 8) + VB.Left(s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), 2);
            
            frmComSupEXSET01 f = new frmComSupEXSET01(sDate, "", "XRAY");

            f.ShowDialog();

            screen_display();



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

            strTitle = "산부인과 검사 스케쥴 " + "(" + cboYYMM.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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

        void setMonth(bool bADD)
        {

            string strMonth = cboYYMM.Text.Trim().Replace("년", "").Replace("월", "").Replace(" ", "").Trim();
            string strYYYY = VB.Left(strMonth, 4);
            string strMM = "";
            strMM = ComFunc.SetAutoZero((bADD == true ? Convert.ToInt16(VB.Right(strMonth, 2)) + 1 : Convert.ToInt16(VB.Right(strMonth, 2)) - 1).ToString(), 2);
            if (strMM == "13")
            {
                strYYYY = (Convert.ToInt16(strYYYY) + 1).ToString();
                strMM = "01";

            }
            else if (strMM == "00")
            {
                strYYYY = (Convert.ToInt16(strYYYY) - 1).ToString();
                strMM = "12";
            }

            cboYYMM.Text = strYYYY + "년 " + strMM + "월";


        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {            
            string strSDate = read_sDate(cboYYMM.Text.Trim());
            string strTDate = Convert.ToString(Convert.ToDateTime(strSDate).AddMonths(1).AddDays(-1).ToShortDateString());

            read_sysdate();

            int start = Convert.ToInt32(Convert.ToDateTime(strSDate).DayOfWeek);

            //달력세팅 및 데이타 표시
            disp_Calendar(pDbCon, Spd, strSDate, strTDate, start);


        }

        string read_sDate(string argYYMM)
        {
            string strTemp = (argYYMM.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
            return  VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";
            
        }

        void disp_Calendar(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate, int start)
        {
            int nRow = 0;
            int nCol = 0;
            string strTemp = "";
            string strJob = "";
            DataTable dt = null;

            if (optJob0.Checked==true)
            {
                strJob = "0";
            }
            else if (optJob1.Checked == true)
            {
                strJob = "1";
            }
            else if (optJob2.Checked == true)
            {
                strJob = "2";
            }

            //휴일체크
            strDay = sup.read_huil(pDbCon, argSDate, argTDate);

            //clear
            Spd.ActiveSheet.ClearRange(0, 0, (int)Spd.ActiveSheet.RowCount, (int)Spd.ActiveSheet.ColumnCount, true);

            nCol = start;

            for (int i = 0; i < Convert.ToInt16(VB.Right(argTDate, 2)); i++)
            {

                Spd.ActiveSheet.Cells[nRow, nCol].Text = ComFunc.SetAutoZero((i + 1).ToString(),2);
                Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                Spd.ActiveSheet.Cells[nRow, nCol].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                if (strDay[i] == "토")
                {
                    Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(40, 30, 230);
                }
                if (strDay[i] == "일")
                {
                    Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }
                if (strDay[i] == "*")
                {
                    Spd.ActiveSheet.Cells[nRow, nCol].ForeColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }

                //쿼리실행            
                dt = fnExSql.sel_ETC_SCHEDULE_OG(pDbCon, strJob, VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2));

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["DelDate"].ToString().Trim()!="")
                        {
                            strTemp += "(" + dt.Rows[j]["sTime"].ToString().Trim() + ")";
                            strTemp += dt.Rows[j]["Pano"].ToString().Trim() + " ★취소★ ";
                            strTemp += dt.Rows[j]["Remark"].ToString().Trim() ;
                            strTemp += " " + dt.Rows[j]["ENTDATE"].ToString().Trim() + "\r\n";
                        }
                        else 
                        {
                            strTemp += "(" + dt.Rows[j]["sTime"].ToString().Trim() + ")";
                            strTemp += dt.Rows[j]["Pano"].ToString().Trim() + " ";
                            strTemp += dt.Rows[j]["Remark"].ToString().Trim() ;
                            strTemp += " " + dt.Rows[j]["ENTDATE"].ToString().Trim() + "\r\n";
                        }


                    }

                    Spd.ActiveSheet.Cells[nRow, nCol].Text += "\r\n" + strTemp;
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

        void screen_display()
        {
            GetData(clsDB.DbCon,ssList);
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
    }

}
