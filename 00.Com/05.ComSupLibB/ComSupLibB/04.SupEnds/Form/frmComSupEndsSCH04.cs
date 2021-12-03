using ComLibB;
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsSCH04.cs
    /// Description     : 대장내시경 스케쥴 등록 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-09-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2개폼 연동 및 통합
    /// </history>
    /// <seealso cref= "\ocs\endo\endres_new\Frm대장스케쥴_new.frm(Frm대장스케쥴_new+종검스케쥴로드) >> frmComSupEndsSCH04.cs 폼이름 재정의" />
    /// 
    public partial class frmComSupEndsSCH04 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupEndsSpd cendsSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();

        clsComSupEndsSQL.cEndo_JupMst cEndo_Jupmst = null;
        clsComSupEndsSQL.cENDO_DSCHEDULE cENDO_DSCHEDULE = null;

        frmComSupEndsSCH01 frmComSupEndsSCH01x = null; //종검달력연동

        clsSupSCHArray[] cSCH = null; // 일자정보 배열

        int TCOL = 4;
        enum DrSchCol { Dept, DrName, DrCode, Change };

        int TCOL2 = 1;
        enum DrCol { Date };



        string[] strDay = null;
        int[,,] nDocCnt = null;
        int[,,] nSchCnt = null;

        int[,] nDocCnt1 = null;

        string gDoctors = "";
        string gDoctors2 = "";
        string gDrCodes = "";
        int gDrCnt = 0;

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

        public frmComSupEndsSCH04()
        {
            InitializeComponent();

            setEvent();
        }

        //폼 생성자 추가
        public frmComSupEndsSCH04(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;

            setEvent();
        }
        
        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            //txtMonth.Text = VB.Left(cpublic.strSysDate, 4) + "년 " + VB.Mid(cpublic.strSysDate, 6, 2) + "월";

            sup.setYYYYMM(cboYYMM,cpublic.strSysDate, 24,12,12,"-");

            setCombo();

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnExit2.Click += new EventHandler(eBtnClick);

            this.btnSearch1.Click += new EventHandler(eBtnSearch);
            this.btnSearch2.Click += new EventHandler(eBtnSearch);
            this.btnSearch3.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnSave2.Click += new EventHandler(eBtnSave);

            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.btnPrint2.Click += new EventHandler(eBtnPrint);


            this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList2.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            this.ssList3.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);

            this.chkCNT.CheckedChanged += new EventHandler(eChkChanged);

            this.cboAmPm.SelectedIndexChanged += new EventHandler(eCboSelChanged);

        }

        void setTxtTip()
        {
            //툴팁
            ssList2.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList2.TextTipDelay = 500;

            ssList3.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList3.TextTipDelay = 500;
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
                //
                cendsSpd.sSpd_enmPanoList(ssList3, cendsSpd.sSpdPanoList, cendsSpd.nSpdPanoList, 10, 0);
                cendsSpd.sSpd_enmDrList2(ssList4, cendsSpd.sSpdDrList2, cendsSpd.nSpdDrList2, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


                //툴팁
                setTxtTip();


                frmComSupEndsSCH01x = new frmComSupEndsSCH01(false);
                sup.setCtrlLoad(panBody, frmComSupEndsSCH01x);

                screen_clear();

                setCtrlData();

                //기본 조회
                screen_display();
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
            }
            else if (sender == this.btnExit2)
            {
                panShowHide(false);
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                screen_display();
            }
            else if (sender == this.btnSearch3)
            {
                setMonth(true);
                screen_display();
            }
            else if (sender == this.btnSearch2)
            {
                setMonth(false);
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                frmComSupEndsSCH02 f = new frmComSupEndsSCH02(); 
                f.ShowDialog();
            }
            else if (sender == this.btnSave2)
            {
                frmComSupEndsRESV01 f = new frmComSupEndsRESV01();
                f.ShowDialog();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //인쇄
                ePrint("A",ssList1);
            }
            else if (sender == this.btnPrint2)
            {
                //인쇄
                ePrint("B",ssList2);
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

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

            if (sender == this.ssList1)
            {
                if ( e.ColumnHeader == true)
                {
                    string sDay = ComFunc.SetAutoZero(s.ActiveSheet.ColumnHeader.Cells.Get(0, e.Column).Text, 2);
                    string sDate = VB.Left(read_sDate(cboYYMM.Text.Trim()),8) + sDay;
                    if (sDay !="")
                    {
                        setSpread2(clsDB.DbCon,ssList2, ssList3, ssList4, sDate, gDrCnt + 1);
                        panShowHide(true);
                    }
                    

                }
                else
                {
                    ComFunc.MsgBox("요일을 더블클릭하면 상세 내역 표시!!");
                }
                
            }

        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (s.ActiveSheet.RowCount <= -1)
            {
                return;
            }

            if (e.RowHeader == true || e.Row < 0)
            {
                return;
            }

           
            try
            {

                if (sender == ssList2)
                {
                    e.TipText = s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                    e.ShowTip = true;
                }
                else if (sender == ssList3)
                {
                    e.TipText = s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                    e.ShowTip = true;
                }
                else
                {
                    e.TipText = s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                    e.ShowTip = true;
                }
                
            }
            catch
            {

            }
           

        }

        void eChkChanged(object sender,EventArgs e)
        {
            screen_display();
        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            ComboBox o = (ComboBox)sender;
            //조회
            try
            {
                if (sender == this.cboAmPm)
                {
                    if (o.SelectedItem.ToString() != null)
                    {
                        setSpdColHide(cboAmPm.Text.Trim(),ssList1);
                    }
                }
                

            }
            catch
            {

            }

        }

        void ePrint(string argJob, FarPoint.Win.Spread.FpSpread Spd)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (argJob =="A")
            {
                strTitle = cboYYMM.Text.Trim() + " 대장내시경 의사별 스케쥴";
                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("내시경실   :" + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                
            }
            else if (argJob == "B")
            {
                strTitle = "일자별 대장내시경 의사별 스케쥴";
                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("내시경실    출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                
            }

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, true, false, (float)0.89);

            SPR.setSpdPrint(Spd, PrePrint, setMargin, setOption, strHeader, strFooter);

                        

        }        

        void readDrinfo(PsmhDb pDbCon)
        {
            gDoctors = "";
            gDrCodes = "";
            gDrCnt = 0;

            DataTable dt = endsSql.sel_BAS_DOCTOR(pDbCon, "", "", " a.endo_seq, a.printranking ");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gDrCnt++;
                    gDoctors += dt.Rows[i]["DrName"].ToString().Trim() + "." + (i+1) +"," ;
                    gDoctors2 +=  (i + 1) + "." + dt.Rows[i]["DrName"].ToString().Trim() +  ",";
                    gDrCodes += dt.Rows[i]["DrCode"].ToString().Trim() + ",";
                }
                
            }
            
        }

        void screen_clear()
        {

            read_sysdate();

            panShowHide(false);

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

            //clear
            //ssList1.ActiveSheet.ClearRange(0, 0, (int)ssList1.ActiveSheet.RowCount, (int)ssList1.ActiveSheet.ColumnCount, true);

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

            //txtMonth.Text = strYYYY + "년 " + strMM + "월";
            cboYYMM.Text = strYYYY + "년 " + strMM + "월";


        }

        void setCombo()
        {
            cboAmPm.Items.Clear();
            cboAmPm.Items.Add("*.전체");
            cboAmPm.Items.Add("1.오전");
            cboAmPm.Items.Add("2.오후");
            cboAmPm.SelectedIndex = 0;
            
        }

        void SetArray(PsmhDb pDbCon, string argDrCode, string argDate1, string argDate2)
        {
            DataTable dt = null;

            cSCH = new clsSupSCHArray[0];

            dt = sup.sel_BasSch(pDbCon,"00", argDrCode, argDate1, argDate2); //초기세팅은 의사스케쥴 참조

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Array.Resize<clsSupSCHArray>(ref cSCH, cSCH.Length + 1);
                    cSCH[i] = new clsSupSCHArray();
                    cSCH[i].Day = dt.Rows[i]["ILJA"].ToString().Trim();
                    cSCH[i].Jin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                    cSCH[i].Jin2 = dt.Rows[i]["GbJin2"].ToString().Trim();

                }
            }

            dt = sup.sel_BasSch(pDbCon, "01", argDrCode, argDate1, argDate2);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    cSCH[i].DJin1 = dt.Rows[i]["GbJin"].ToString().Trim();
                    cSCH[i].DJin2 = dt.Rows[i]["GbJin2"].ToString().Trim();

                }
            }

        }

        void setSpread(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate,int argRow)
        {
            int i = 0;
            string yoil = "";
            int nCnt = 0;
            string strTDate = Convert.ToDateTime(VB.Left(Convert.ToDateTime(argDate).AddMonths(1).ToShortDateString(), 8) + "01").AddDays(-1).ToShortDateString();

            //휴일체크
            strDay = sup.read_huil(pDbCon, argDate, strTDate);

            nCnt = Convert.ToInt16(VB.Right(strTDate, 2));

            Spd.ActiveSheet.RowCount = 0;
            Spd.ActiveSheet.RowCount = argRow;

            Spd.ActiveSheet.ColumnCount = 0;
            Spd.ActiveSheet.ColumnCount = (nCnt * 3) + TCOL;

            methodSpd.setColAlign(Spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColStyle(Spd, -1, -1, clsSpread.enmSpdType.Label);

            Spd.VerticalScrollBarWidth = 10;
            Spd.HorizontalScrollBarHeight = 10;

            Spd.ActiveSheet.ColumnHeader.RowCount = 3;

            Spd.ActiveSheet.Columns[3, Spd.ActiveSheet.ColumnCount - 1].Width = 30;

            //컬럼 고정 및 색상
            Spd.ActiveSheet.FrozenColumnCount = 2;
            Spd.ActiveSheet.Columns[0, (Spd.ActiveSheet.RowCount - 1)].BackColor = Color.Empty;
            int col2 = 2;
            if ((col2 > 0))
            {
                Spd.ActiveSheet.Columns[0, (col2 - 1)].BackColor = Color.LightGray;
            }


            Spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 2, Spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.Dept, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.Dept].Value = "과";
            Spd.ActiveSheet.Columns[0].Width = 25;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.DrName, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.DrName].Value = "의사";
            Spd.ActiveSheet.Columns[1].Width = 50;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.DrCode, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.DrCode].Value = "의사코드";
            Spd.ActiveSheet.Columns[2].Visible = false;

            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrSchCol.Change, 3, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrSchCol.Change].Value = "수정";
            Spd.ActiveSheet.Columns[3].Visible = false;


            for (i = 0; i < nCnt; i++)
            {

                if (cpublic.strSysDate.CompareTo(VB.Left(argDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2)) <= 0)
                {
                    methodSpd.setColStyle(Spd, -1, (i * 2) + TCOL + i, clsSpread.enmSpdType.Text);
                    methodSpd.setColStyle(Spd, -1, (i * 2) + TCOL + i + 1, clsSpread.enmSpdType.Text);
                }

                yoil = clsVbfunc.GetYoIl(date2cbo(argDate, i + 1));

                Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (i * 2) + TCOL + i, 1, 3);
                Spd.ActiveSheet.ColumnHeader.Cells[0, (i * 2) + TCOL + i].Value = i + 1;

                Spd.ActiveSheet.AddColumnHeaderSpanCell(1, (i * 2) + TCOL + i, 1, 3);
                Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 2) + TCOL + i].Value = VB.Left(yoil, 1);
                Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + i].Value = "AM";
                Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 1 + i].Value = "PM";

                Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 2 + i].Value = "야";
                Spd.ActiveSheet.Columns[(i * 2) + TCOL + 2 + i].Visible = false;

                if (yoil == "일요일" || strDay[i] == "*")
                {
                    Spd.ActiveSheet.ColumnHeader.Cells[0, (i * 2) + TCOL + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 2) + TCOL + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 1 + i].BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.ColumnHeader.Cells[2, (i * 2) + TCOL + 2 + i].BackColor = sup.SCH_Huil;
                
                    Spd.ActiveSheet.Columns.Get((i * 2) + TCOL + i).BackColor = sup.SCH_Huil;
                    Spd.ActiveSheet.Columns.Get((i * 2) + TCOL + i+1).BackColor = sup.SCH_Huil;
                }


            }

        }

        void setSpread2(PsmhDb pDbCon, FpSpread Spd, FpSpread Spd2, FpSpread Spd3,string argDate, int argCnt)
        {
            int i = 0;
            int j = 0;
            int nPCnt = 0;
            int nRowCnt = 0;
            int nDr = 0;
            DataTable dt = null;
            string strGbn = "";
            string strPano   = "";
            string strSName = "";
            string strRDate = "";
            string strTime = "";
            string strDrName = "";
            string strRemark = "";
            string strNEW = "";
            string strOLD = "";
            string strTemp = "";
            int nDay = Convert.ToInt16(VB.Right(argDate,2));

            nDocCnt1 = new int[gDrCnt+1,2];

            Spd.ActiveSheet.RowCount = 0;
            Spd.ActiveSheet.RowCount = 20;

            Spd.ActiveSheet.ColumnCount = 0;
            Spd.ActiveSheet.ColumnCount = (argCnt * 4) + TCOL2 +1;  //1더준건 출력관련
            Spd.ActiveSheet.Columns[(argCnt * 4) + TCOL2].Visible = false;



            //clear
            ssList2.ActiveSheet.ClearRange(0, 0, (int)ssList2.ActiveSheet.RowCount, (int)ssList2.ActiveSheet.ColumnCount, true);

            methodSpd.setColAlign(Spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColStyle(Spd, -1, -1, clsSpread.enmSpdType.Label);
            
            Spd.VerticalScrollBarWidth = 10;
            Spd.HorizontalScrollBarHeight = 10;
            
            Spd.ActiveSheet.ColumnHeader.RowCount = 2;
                      
            //컬럼 고정 및 색상
            Spd.ActiveSheet.FrozenColumnCount = 1;
            Spd.ActiveSheet.Columns[0, (Spd.ActiveSheet.RowCount - 1)].BackColor = Color.Empty;

            int col2 = 1;
            if ((col2 > 0))
            {
                Spd.ActiveSheet.Columns[0, (col2 - 1)].BackColor = Color.LightGray;
            }
           

            Spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 1, Spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;


            Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (int)DrCol.Date, 2, 1);
            Spd.ActiveSheet.ColumnHeader.Cells[0, (int)DrCol.Date].Value = "일자";
            Spd.ActiveSheet.Columns[0].Width = 70;
            
            for (i = 0; i < argCnt; i++)
            {             
                Spd.ActiveSheet.AddColumnHeaderSpanCell(0, (i * 3) + TCOL2 + i, 1, 4);
                if (i==0)
                {
                    Spd.ActiveSheet.ColumnHeader.Cells[0, (i * 3) + TCOL2 + i].Value = "의사미지정";
                }
                else
                {
                    Spd.ActiveSheet.ColumnHeader.Cells[0, (i * 3) + TCOL2 + i].Value =  clsComSup.setP(clsComSup.setP(gDoctors2,i.ToString(),2),",",1).Replace(".","");
                }                
                                         
                Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 3) + TCOL2 + i + 0].Value = "시간";
                Spd.ActiveSheet.Columns[(i * 3) + TCOL2 + i + 0].Width = 35;
                
                Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 3) + TCOL2 + i + 1].Value = "환자명";
                Spd.ActiveSheet.Columns[(i * 3) + TCOL2 + i + 1].Width = 45;
                
                Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 3) + TCOL2 + i + 2].Value = "등록번호";
                Spd.ActiveSheet.Columns[(i * 3) + TCOL2 + i + 2].Width = 55;
                
                Spd.ActiveSheet.ColumnHeader.Cells[1, (i * 3) + TCOL2 + i + 3].Value = "참고사항";
                Spd.ActiveSheet.Columns[(i * 3) + TCOL2 + i + 3].Width = 60;
                
            }

            //포스코 위 체크
            strTemp = "";
            dt = endsSql.sel_ENDO_JUPMST_sch_posco(pDbCon, argDate, argDate);
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strTemp += dt.Rows[i]["Pano"].ToString().Trim() + ",";
                }
            }

            //쿼리실행            
            dt = endsSql.sel_ENDO_JUPMST_sch2(pDbCon, argDate, argDate);

            //의사별 리스트
            Spd.ActiveSheet.RowCount = 50;
            
            //일자 개인별 리스트 시트
            Spd2.ActiveSheet.RowCount = 0;

            //의사별 오전오후 건수관련 시트
            Spd3.ActiveSheet.RowCount = 0;
            Spd3.ActiveSheet.RowCount = gDrCnt;
            
            if (ComFunc.isDataTableNull(dt) == false)
            {
                Spd2.ActiveSheet.RowCount = dt.Rows.Count;
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strGbn = dt.Rows[i]["Gbn"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strSName = dt.Rows[i]["SName"].ToString().Trim();
                    strRDate = dt.Rows[i]["SDate"].ToString().Trim();
                    strDrName = dt.Rows[i]["RDrName"].ToString().Trim();
                    strRemark = dt.Rows[i]["Remark"].ToString().Trim();

                    strTime = VB.Right(strRDate, 5);
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.mm].Text = VB.Mid(argDate, 6, 2);
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.dd].Text = VB.Mid(argDate, 9, 2);
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.time].Text = strTime;
                    if (strGbn == "1" || strGbn == "9")
                    {
                        if (strDrName =="")
                        {
                            nDr = 0;
                        }
                        else
                        {
                            if (gDoctors.Contains(strDrName) == true)
                            {
                                nDr = Convert.ToInt16(clsComSup.setP(clsComSup.setP(clsComSup.setP(gDoctors, strDrName, 2), ",", 1), ".", 2));
                            }
                            else
                            {
                                nDr = 0;
                            }
                            
                        }                        

                        if (strTime.CompareTo("12:30") <= 0)
                        {
                            //건수누적
                            nDocCnt1[nDr,0] += 1;
                        }
                        else
                        {
                            //건수누적
                            nDocCnt1[nDr,1] += 1;
                        }

                        Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.DrName].Text = strDrName;

                        if (strGbn == "1")
                        {
                            Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.SName].Text = strSName;
                        }
                        else
                        {
                            Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.SName].Text = "[가]" + strSName;
                        }
                            
                        Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    }                    
                    else
                    {
                        nPCnt++;
                        Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.SName].Text = "[P]" + strSName;
                        Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.Remark].Text = "대장";
                        if (strTemp.Contains(strPano) == true )
                        {
                            Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.Remark].Text = "대장,위";
                        }
                    }


                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmPanoList.Pano].Text = strPano;

                    nRowCnt = 0;

                    if (strGbn == "1" || strGbn == "9")
                    {
                        if (strDrName!="")
                        {
                            for (j = 0; j < Spd.ActiveSheet.ColumnCount; j++)
                            {
                     
                                if (Spd.ActiveSheet.Cells[nRowCnt, (nDr * 4) + TCOL2 ].Text !="")
                                {
                                    nRowCnt++;                                    
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[nRowCnt, 0].Text = argDate;                                    
                                    Spd.ActiveSheet.Cells[nRowCnt, (nDr * 4) + TCOL2].Text = strTime;                                    
                                    Spd.ActiveSheet.Cells[nRowCnt, (nDr * 4) + TCOL2 + 1].Text = strSName;
                                    Spd.ActiveSheet.Cells[nRowCnt, (nDr * 4) + TCOL2 + 1].BackColor = Color.FromArgb(128, 255, 128);
                                    Spd.ActiveSheet.Cells[nRowCnt, (nDr * 4) + TCOL2 + 2].Text = strPano;
                                    Spd.ActiveSheet.Cells[nRowCnt, (nDr * 4) + TCOL2 + 3].Text = strGbn == "9" ? "[가예약]" : strRemark;

                                    Spd.ActiveSheet.Cells[nRowCnt,Spd.ActiveSheet.ColumnCount-1].Text = "1"; //출력때문 값넣어줌

                                    
                                    break;
                                }

                            }
                        }
                        else
                        {
                            for (j = 0; j < Spd.ActiveSheet.ColumnCount; j++)
                            {
                                if (Spd.ActiveSheet.Cells[nRowCnt, TCOL2].Text != "")
                                {
                                    nRowCnt++;
                                }
                                else
                                {
                                    Spd.ActiveSheet.Cells[nRowCnt, 0 ].Text = argDate;
                                    Spd.ActiveSheet.Cells[nRowCnt, 1].Text = strTime;
                                    Spd.ActiveSheet.Cells[nRowCnt, 2].Text = strSName;
                                    Spd.ActiveSheet.Cells[nRowCnt, 2].BackColor = Color.FromArgb(128, 255, 128);
                                    Spd.ActiveSheet.Cells[nRowCnt, 3].Text = strPano;
                                    Spd.ActiveSheet.Cells[nRowCnt, 4].Text = strRemark;
                            
                                    break;
                                }
                            }
                        }
                    }
                                        

                }

            }

            //최종 의사별 리스트 ROW 체크
            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;

          
            //의사별 오전오후 건수 표시
            for (i = 1; i <= gDrCnt; i++)
            {
                Spd3.ActiveSheet.Cells[i - 1, 0].Text = clsComSup.setP(clsComSup.setP(gDoctors, ",", i), ".", 1);
                Spd3.ActiveSheet.Cells[i - 1, 1].Text = nDocCnt1[i-1, 0].ToString() + "/" + nDocCnt[i -1, 0, nDay-1];
                if (nDocCnt1[i, 0] >= nDocCnt[i - 1, 0, nDay-1])
                {
                    Spd3.ActiveSheet.Cells[i - 1, 1].BackColor = Color.LightPink;
                }
                Spd3.ActiveSheet.Cells[i - 1, 2].Text = nDocCnt1[i-1, 1].ToString() + "/" + nDocCnt[i -1, 1, nDay-1];
                if (nDocCnt1[i, 1] >= nDocCnt[i - 1, 1, nDay-1])
                {
                    Spd3.ActiveSheet.Cells[i - 1, 2].BackColor = Color.LightPink;
                }
            }

            Spd3.ActiveSheet.Columns.Get(0).BackColor = Color.LightGray;
                      
            
            //종검 스케쥴 인원 ADD            
            strTemp = "";
            dt = endsSql.sel_HEA_RESV_EXAM(pDbCon, "03", argDate, "", " '01','02' ", "");
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strTemp += dt.Rows[i]["Pano"].ToString().Trim() + ",";
                }
            }
            
            //쿼리실행      
            dt = endsSql.sel_HEA_RESV_EXAM(pDbCon, "02", argDate, "", " '02' ", "");

            nRowCnt = Spd.ActiveSheet.RowCount+1;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount += dt.Rows.Count +1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i==0)
                    {
                        Spd.ActiveSheet.Cells.Get(nRowCnt-1, 1).ColumnSpan = 4;                        
                        Spd.ActiveSheet.Cells[nRowCnt - 1, 1].Text = "종검명단 " + dt.Rows.Count + "건 ";
                    }                    
                    if (dt.Rows[i]["GbExam"].ToString().Trim() == "02")
                    {
                        strNEW = dt.Rows[i]["Pano"].ToString().Trim();
                                                
                        strTime = dt.Rows[i]["RTime2"].ToString().Trim();
                        strSName = dt.Rows[i]["SName"].ToString().Trim();
                        strPano = strNEW;
                        strRemark = "대장";
                       
                        if (strTemp.Contains(strPano) == true)
                        {
                            strRemark = "대장,위";
                        }

                        for (j = 0; j < Spd.ActiveSheet.RowCount; j++)
                        {
                            if (Spd.ActiveSheet.Cells[nRowCnt, 1].Text != "")
                            {
                                nRowCnt++;
                            }
                            else
                            {
                                Spd.ActiveSheet.Cells[nRowCnt, 0].Text = argDate;
                                Spd.ActiveSheet.Cells[nRowCnt, 1].Text = strTime;
                                Spd.ActiveSheet.Cells[nRowCnt, 2].Text = strSName;
                                Spd.ActiveSheet.Cells[nRowCnt, 2].BackColor = Color.FromArgb(128, 255, 128);
                                Spd.ActiveSheet.Cells[nRowCnt, 3].Text = strPano;
                                Spd.ActiveSheet.Cells[nRowCnt, 4].Text = strRemark;
                                
                                break;
                            }
                        }


                        strOLD = dt.Rows[i]["Pano"].ToString().Trim();
                    }
                }

             }
                        

            //포스코 ADD
            if (nPCnt > 0)
            {                
                nRowCnt = Spd.ActiveSheet.RowCount + 1;
                Spd.ActiveSheet.RowCount += nPCnt + 1;
                Spd.ActiveSheet.Cells.Get(nRowCnt - 1, 1).ColumnSpan = 4;
                Spd.ActiveSheet.Cells[nRowCnt - 1, 1].Text = "포스코 " + nPCnt + "건 ";
                                

                for (i = 0; i <= Spd2.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    if ( clsComSup.setL(Spd2.ActiveSheet.Cells[i,4].Text.Trim(),"[P]") > 1 )
                    {   
                        Spd.ActiveSheet.Cells[nRowCnt, 0].Text = argDate;
                        Spd.ActiveSheet.Cells[nRowCnt, 1].Text = Spd2.ActiveSheet.Cells[i, 2].Text.Trim();
                        Spd.ActiveSheet.Cells[nRowCnt, 2].Text = Spd2.ActiveSheet.Cells[i, 4].Text.Trim().Replace("[P]",""); 
                        Spd.ActiveSheet.Cells[nRowCnt, 3].Text = Spd2.ActiveSheet.Cells[i, 5].Text.Trim(); 
                        Spd.ActiveSheet.Cells[nRowCnt, 4].Text = Spd2.ActiveSheet.Cells[i, 6].Text.Trim();

                        nRowCnt++;
                    }

                    
                }
                    
            }            

            //최종 의사별 리스트 ROW 맞춤
            Spd.ActiveSheet.RowCount = Spd.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
            
        }

        void GetData(PsmhDb pDbCon, string argSDate,string argTDate)
        {
            
            string[] str = gDoctors.Split(',');
            
            setSpread(pDbCon, ssList1, argSDate, gDrCnt);

            nDocCnt = new int[str.Length-1, 2, Convert.ToInt16(VB.Right(argTDate, 2))];
            nSchCnt = new int[str.Length - 1, 2, Convert.ToInt16(VB.Right(argTDate, 2))];

            read_sysdate();

            read_schCnt(pDbCon,argSDate, argTDate);

            int start = Convert.ToInt32(Convert.ToDateTime(argSDate).DayOfWeek);

            //달력세팅 및 데이타 표시
            disp_Calendar(pDbCon, argSDate, argTDate, start);
            
        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate,string argTDate)
        {
            int i = 0;
            int j = 0;


            DataTable dt = null;
            int nLastDay = 0;
            string strDrCode = "";
            int nDr = 0;
            string strDrName = "";
            

            nLastDay = Convert.ToInt16(VB.Right(argTDate, 2));

            string strOrdby = " a.endo_seq  ,B.PrintRanking, A.DrDept1  ,decode(a.drcode,'1102',1,'1104',2,'1114',3,'1113', 4, '1108',5 ,'1111', 6,'1120',7 ,'1119',8, '1107', 9, A.PrintRanking )  ";

            dt = endsSql.sel_BAS_DOCTOR(pDbCon, "", "", strOrdby);

            if (ComFunc.isDataTableNull(dt) == false)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();

                    SetArray(pDbCon, strDrCode, argDate, argTDate);

                    Spd.ActiveSheet.Cells[i, TCOL + j].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)DrSchCol.Dept].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)DrSchCol.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)DrSchCol.DrCode].Text = strDrCode;

                    if (cSCH != null && cSCH.Length > 0)
                    {
                        for (j = 0; j < nLastDay; j++)
                        {
                            if (chkCNT.Checked ==true)
                            {
                                strDrName = dt.Rows[i]["DrName"].ToString().Trim();
                                nDr = Convert.ToInt16(clsComSup.setP(clsComSup.setP(clsComSup.setP(gDoctors, strDrName, 2), ",", 1), ".", 2)) - 1;

                                Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = nDocCnt[nDr, 0, j] + "/" + nSchCnt[nDr, 0, j];
                                if (cSCH[j].Jin1 == "3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_3;
                                }
                                else if (cSCH[j].Jin1 == "6" || cSCH[j].Jin1 == "9")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = Color.Pink;
                                }
                                if (nDocCnt[nDr, 0, j] > 0 && nDocCnt[nDr, 0, j] >= nSchCnt[nDr, 0, j])
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.ENDO_1;
                                }

                                Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = nDocCnt[nDr, 1, j] + "/" + nSchCnt[nDr, 1, j];
                                if (cSCH[j].Jin2 == "3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_3;
                                }
                                else if (cSCH[j].Jin2 == "6" || cSCH[j].Jin2 == "9")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = Color.Pink;
                                }
                                if (nDocCnt[nDr, 1, j] > 0 && nDocCnt[nDr, 1, j] >= nSchCnt[nDr, 1, j])
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.ENDO_1;
                                }
                            }
                            else
                            {
                                
                                if ( strDay[j] != "*")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].Value = sup.sch2Name(cSCH[j].Jin1);

                                    if (cSCH[j].Jin1 == "1")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_1;
                                    }
                                    else if (cSCH[j].Jin1 == "2")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_2;
                                    }
                                    else if (cSCH[j].Jin1 == "3")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_3;
                                    }
                                    else if (cSCH[j].Jin1 == "9" || cSCH[j].Jin1 == "6")
                                    {
                                        //Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_9;
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = Color.Pink;
                                    }
                                    else
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_White;
                                    }

                                }

                                
                                if ( strDay[j] != "*")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = sup.sch2Name(cSCH[j].Jin2);

                                    if (cSCH[j].Jin2 == "1")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_1;
                                    }
                                    else if (cSCH[j].Jin2 == "2")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_2;
                                    }
                                    else if (cSCH[j].Jin2 == "3")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_3;
                                    }
                                    else if (cSCH[j].Jin2 == "9" || cSCH[j].Jin2 == "6")
                                    {
                                        //Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_9;
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = Color.Pink;
                                    }
                                    else
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_White;
                                    }
                                }
                            }

                            
                        }
                    }

                }

            }

        }

        void setSpdColHide(string Job, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
                        
            int nLastDay = Convert.ToInt16(VB.Right(Convert.ToString(Convert.ToDateTime(read_sDate(cboYYMM.Text.Trim())).AddMonths(1).AddDays(-1).ToShortDateString()),2));

            if (Spd.ActiveSheet.ColumnCount < TCOL )
            {
                return;
            }

            try
            {
                for (i = 0; i < nLastDay; i++)
                {
                    Spd.ActiveSheet.Columns[(i * 2) + TCOL + i].Visible = true;
                    Spd.ActiveSheet.Columns[(i * 2) + TCOL + i + 1].Visible = true;
                }

                if (Job == "1.오전")
                {
                    for (i = 0; i < nLastDay; i++)
                    {
                        Spd.ActiveSheet.Columns[(i * 2) + TCOL + i + 1].Visible = false;
                    }
                }
                else if (Job == "2.오후")
                {
                    for (i = 0; i < nLastDay; i++)
                    {
                        Spd.ActiveSheet.Columns[(i * 2) + TCOL + i].Visible = false;
                    }
                }
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            
        }

        string read_sDate(string argYYMM)
        {
            string strTemp = (argYYMM.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
            return VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";

        }

        void read_schCnt(PsmhDb pDbCon, string argSDate,string argTDate)
        {
            int nDay = 0;
            int nDr = 0;
            string strDrName = "";

            cENDO_DSCHEDULE = new clsComSupEndsSQL.cENDO_DSCHEDULE();

            cENDO_DSCHEDULE.Date1 = argSDate;
            cENDO_DSCHEDULE.Date2 = argTDate;

            DataTable dt = endsSql.sel_ENDO_DSCHEDULE(pDbCon, cENDO_DSCHEDULE);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nDay = Convert.ToInt16(dt.Rows[i]["nDay"].ToString().Trim())-1;
                    strDrName = dt.Rows[i]["FC_DrName"].ToString().Trim();
                    if (strDrName !="" && gDoctors.Contains(strDrName) ==true)
                    {
                        nDr = Convert.ToInt16(clsComSup.setP(clsComSup.setP(clsComSup.setP(gDoctors, strDrName, 2), ",", 1), ".", 2))-1 ;
                        if (dt.Rows[i]["GbJin"].ToString().Trim()!="")
                        {
                            nSchCnt[nDr, 0, nDay] += Convert.ToInt16(dt.Rows[i]["GbJin"].ToString().Trim()); //오전
                        }
                        if (dt.Rows[i]["GbJin2"].ToString().Trim() != "")
                        {
                            nSchCnt[nDr, 1, nDay] += Convert.ToInt16(dt.Rows[i]["GbJin2"].ToString().Trim()); //오후
                        }
                            
                    }
                    
                }
            }


        }                

        string date2cbo(string argDate, int argDay = 1)
        {
            return VB.Left(argDate, 7) + "-" + ComFunc.SetAutoZero(argDay.ToString(), 2);
        }                

        void disp_Calendar(PsmhDb pDbCon, string argSDate, string argTDate, int start)
        {
            int nRow = 0;
            int nCol = 0;
            string strGbn = "";
            string strTime = "";
            string strDrName = "";
            string strSName = "";
            string strTemp = "";
            string strJob = "";
            string strRes = "";
            //string strRemark = "";
            //string strResult = "";
            string strTempD = "";
            int nDr = 0;

            DataTable dt = null;

            if (optSearch1.Checked == true)
            {
                strJob = "0";
            }
            else if (optSearch2.Checked == true)
            {
                strJob = "1";
            }
                       
            //휴일체크
            strDay = sup.read_huil(pDbCon, argSDate, argTDate);

            
            nCol = start;

            for (int i = 0; i < Convert.ToInt16(VB.Right(argTDate, 2)); i++)
            {
                
                #region  //쿼리실행  당일대장 결과 완료자 변수담기    
                cEndo_Jupmst = new clsComSupEndsSQL.cEndo_JupMst();
                cEndo_Jupmst.Date1 = VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2);
                cEndo_Jupmst.Date2 = VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2);
                cEndo_Jupmst.GbJob = "3";
                cEndo_Jupmst.Res = "0";
                cEndo_Jupmst.ResultDate = "결과";
                dt = endsSql.sel_Endo_JupMst(pDbCon, cEndo_Jupmst);
                strTempD = "";
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        strTempD += dt.Rows[j]["Ptno"].ToString().Trim() +",";
                    }
                }
                #endregion

                //쿼리실행            
                dt = endsSql.sel_ENDO_JUPMST_sch(pDbCon, strJob, VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2), VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2),"1");

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        strGbn = dt.Rows[j]["Gbn"].ToString().Trim();
                        strRes = dt.Rows[j]["Res"].ToString().Trim();
                        strTime = VB.Right(dt.Rows[j]["SDate"].ToString().Trim(),5);                        
                        strSName = dt.Rows[j]["SName"].ToString().Trim();
                        strDrName = dt.Rows[j]["RDrName"].ToString().Trim();                       
                        //strResult = "";
                        
                        if (strGbn != "2" && strDrName != "" && strRes == "1")
                        {
                            strTemp += "[" + strDrName + "]";

                            //strResult = "";
                            if ( j < dt.Rows.Count && strGbn == "1")
                            {
                                if (strTempD.Contains(dt.Rows[j]["Pano"].ToString().Trim()) ==true )
                                {
                                    //strResult = "OK";
                                }  
                            }
                        }

                        if (strRes == "1" || (strGbn =="1" && strRes == ""))
                        {
                            
                            if (strDrName != "" && gDoctors.Contains(strDrName) ==true && (strGbn == "1" || strGbn == "9"))
                            {
                                nDr = Convert.ToInt16(clsComSup.setP(clsComSup.setP(clsComSup.setP(gDoctors, strDrName, 2), ",", 1), ".", 2)) - 1;

                                if (strTime.CompareTo("12:30") <= 0)
                                {
                                    //건수누적
                                    nDocCnt[nDr, 0, i] += 1;
                                }
                                else
                                {
                                    //건수누적
                                    nDocCnt[nDr, 1, i] += 1;
                                }


                            }

                        }  
            
                    }
                                
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
            ssList1.Enabled = false;

            string strSDate = read_sDate(cboYYMM.Text.Trim());
            string strTDate = Convert.ToString(Convert.ToDateTime(strSDate).AddMonths(1).AddDays(-1).ToShortDateString());

            readDrinfo(clsDB.DbCon);

            GetData(clsDB.DbCon, strSDate,strTDate);

            GetData2(clsDB.DbCon, ssList1, strSDate,strTDate);

            frmComSupEndsSCH01x.screan_display(cboYYMM.Text.Trim());//

            ssList1.Enabled = true;

            screen_clear();

        }

        void panShowHide(bool bShow =false)
        {
            if(bShow==true)
            {
                panList.Visible = true;
                spList.Visible = true;
            }
            else if (bShow == false)
            {
                panList.Visible = false;
                spList.Visible = false;
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
