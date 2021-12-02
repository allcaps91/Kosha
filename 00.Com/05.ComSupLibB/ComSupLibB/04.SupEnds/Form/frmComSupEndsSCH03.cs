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

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsSCH03.cs
    /// Description     : 대장내시경 스케쥴 등록 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-08-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endres_new\Frm대장스케쥴_new.frm(Frm대장스케쥴_new) >> frmComSupEndsSCH03.cs 폼이름 재정의" />
    /// 
    public partial class frmComSupEndsSCH03 : Form
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
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();

        clsComSupEndsSQL.cEndo_JupMst cEndo_Jupmst = null;
        clsComSupEndsSQL.cENDO_DSCHEDULE cENDO_DSCHEDULE = null;

        clsSupSCHArray[] cSCH = null; // 일자정보 배열

        const int TCOL = 4;
        enum DrSchCol { Dept, DrName, DrCode, Change };



        string[] strDay = null;
        int[,,] nDocCnt = null;
        int[,,] nSchCnt = null;

        string gDoctors = "";
        string gDrCodes = "";
        int gDrCnt = 0;

        #endregion

        public frmComSupEndsSCH03()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            txtMonth.Text = VB.Left(cpublic.strSysDate, 4) + "년 " + VB.Mid(cpublic.strSysDate, 6, 2) + "월";



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
            this.btnSave2.Click += new EventHandler(eBtnEvent);

            this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList1.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);

            this.chkCNT.CheckedChanged += new EventHandler(eChkChanged);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboEvent);

        }

        void setTxtTip()
        {
            //툴팁
            ssList1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList1.TextTipDelay = 500;
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

                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //기본 조회
                screen_display();
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
            else if (sender == this.btnSearch1)
            {
                screen_display();
            }
            else if (sender == this.btnSave)
            {
                frmComSupEndsSCH02 f = new frmComSupEndsSCH02();
                f.ShowDialog();
            }
            else if (sender == this.btnSave2)
            {
                frmComSupEndsRESV01 f = new frmComSupEndsRESV01();
                f.ShowDialog();
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

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            FpSpread s = (FpSpread)sender;

            //string sDate = VB.Left(read_sDate(txtMonth.Text.Trim()), 8) + VB.Left(s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), 2);

            //frmComSupEXSET01 f = new frmComSupEXSET01(sDate, "", "XRAY");

            //f.ShowDialog();

            //screen_display();



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
                e.TipText = ssList1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                e.ShowTip = true;
            }
            catch
            {

            }
           

        }

        void eChkChanged(object sender,EventArgs e)
        {
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

            strTitle = "대장내시경 스케쥴 " + "(" + txtMonth.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);

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
                    gDrCodes += dt.Rows[i]["DrCode"].ToString().Trim() + ",";
                }
                
            }
            
        }

        void screen_clear()
        {

            read_sysdate();

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

            //clear
            ssList1.ActiveSheet.ClearRange(0, 0, (int)ssList1.ActiveSheet.RowCount, (int)ssList1.ActiveSheet.ColumnCount, true);

        }

        void setMonth(bool bADD)
        {

            string strMonth = txtMonth.Text.Trim().Replace("년", "").Replace("월", "").Replace(" ", "").Trim();
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

            txtMonth.Text = strYYYY + "년 " + strMM + "월";


        }
        
        void SetArray(PsmhDb pDbCon, string argDrCode, string argDate1, string argDate2)
        {
            DataTable dt = null;

            cSCH = new clsSupSCHArray[0];

            dt = sup.sel_BasSch(pDbCon, "00", argDrCode, argDate1, argDate2); //초기세팅은 의사스케쥴 참조

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

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argSDate,string argTDate)
        {
            
            string[] str = gDoctors.Split(',');
            
            setSpread(pDbCon, ssList2, argSDate, gDrCnt);

            nDocCnt = new int[str.Length-1, 2, Convert.ToInt16(VB.Right(argTDate, 2))];
            nSchCnt = new int[str.Length - 1, 2, Convert.ToInt16(VB.Right(argTDate, 2))];

            read_sysdate();

            read_schCnt(pDbCon, argSDate, argTDate);

            int start = Convert.ToInt32(Convert.ToDateTime(argSDate).DayOfWeek);

            //달력세팅 및 데이타 표시
            disp_Calendar(pDbCon, Spd, argSDate, argTDate, start);
            
        }

        void GetData2(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd, string argDate,string argTDate)
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
                                if (nDocCnt[nDr, 0, j] > 0 && nDocCnt[nDr, 0, j] >= nSchCnt[nDr, 0, j])
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_1;
                                }

                                Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].Value = nDocCnt[nDr, 1, j] + "/" + nSchCnt[nDr, 1, j];
                                if (cSCH[j].Jin2 == "3")
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_3;
                                }
                                if (nDocCnt[nDr, 1, j] > 0 && nDocCnt[nDr, 1, j] > nSchCnt[nDr, 1, j])
                                {
                                    Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_1;
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
                                    else if (cSCH[j].Jin1 == "9")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j].BackColor = sup.SCH_9;
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
                                    else if (cSCH[j].Jin2 == "9")
                                    {
                                        Spd.ActiveSheet.Cells[i, (j * 2) + TCOL + j + 1].BackColor = sup.SCH_9;
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
                    if (strDrName !="")
                    {
                        nDr = Convert.ToInt16(clsComSup.setP(clsComSup.setP(clsComSup.setP(gDoctors, strDrName, 2), ",", 1), ".", 2))-1 ;

                        nSchCnt[nDr, 0, nDay] += Convert.ToInt16(dt.Rows[i]["GbJin"].ToString().Trim()); //오전
                        nSchCnt[nDr, 1, nDay] += Convert.ToInt16(dt.Rows[i]["GbJin2"].ToString().Trim()); //오후
                    }
                    
                }
            }


        }                

        string date2cbo(string argDate, int argDay = 1)
        {
            return VB.Left(argDate, 7) + "-" + ComFunc.SetAutoZero(argDay.ToString(), 2);
        }                

        void disp_Calendar(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate, int start)
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
            string strRemark = "";
            string strResult = "";
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

            Spd.Enabled = false;

            //휴일체크
            strDay = sup.read_huil(pDbCon, argSDate, argTDate);

            //clear
            Spd.ActiveSheet.ClearRange(0, 0, (int)Spd.ActiveSheet.RowCount, (int)Spd.ActiveSheet.ColumnCount, true);

            nCol = start;

            for (int i = 0; i < Convert.ToInt16(VB.Right(argTDate, 2)); i++)
            {

                Spd.ActiveSheet.Cells[nRow, nCol].Text = ComFunc.SetAutoZero((i + 1).ToString(), 2);
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
                        strRemark = dt.Rows[j]["Remark"].ToString().Trim();
                        strResult = "";
                        
                        if (strGbn != "2" && strDrName != "" && strRes == "1")
                        {
                            strTemp += "[" + strDrName + "]";

                            strResult = "";
                            if ( j < dt.Rows.Count && strGbn == "1")
                            {
                                if (strTempD.Contains(dt.Rows[j]["Pano"].ToString().Trim()) ==true )
                                {
                                    strResult = "OK";
                                }  
                            }
                        }

                        if (strRes =="1")
                        {

                            if (strRemark!="")
                            {
                                strRemark = " [참고]";
                            }

                            if (strDrName !="" && (strGbn == "1" || strGbn == "9"))
                            {
                                nDr = Convert.ToInt16(clsComSup.setP(clsComSup.setP( clsComSup.setP(gDoctors,strDrName,2),",",1),".",2))-1 ;

                                if (strTime.CompareTo("12:30") < 0)
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

                            if (strGbn =="1")
                            {                                    
                                if (strResult != "")
                                {
                                    strTemp += strSName + " " + strTime + strRemark +"(○)" + "\r\n"; ;
                                }
                                else
                                {
                                    strTemp += strSName + " " + strTime + strRemark + "\r\n"; ;
                                }
                                    
                            }
                            else if (strGbn == "2" && strSName != "")
                            {
                                strTemp += "[P]"+strSName + " " + strTime + "\r\n"; ;
                            }
                            else if (strGbn == "3" && strSName != "")
                            {
                                strTemp += "[H]" + strSName + " " + strTime + "\r\n"; ;
                            }
                            else if (strGbn == "4" && strSName != "")
                            {
                                if (dt.Rows[j]["ResultDate"].ToString().Trim() == "Y")
                                {
                                    strTemp += "[T]" + strSName + " " + strTime + " [+]" + "\r\n"; ;
                                }
                                else
                                {
                                    strTemp += "[T]" + strSName + " " + strTime +  "\r\n"; ;
                                }
                            }
                            else if (strGbn == "9" && strSName != "")
                            {
                                strTemp += "[가]" + strSName + " " + strTime + "\r\n"; ;
                            }
                                
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

            Spd.Enabled = true;

        }

        void screen_display()
        {
            ssList1.Enabled = false;
            ssList2.Enabled = false;

            string strSDate = read_sDate(txtMonth.Text.Trim());
            string strTDate = Convert.ToString(Convert.ToDateTime(strSDate).AddMonths(1).AddDays(-1).ToShortDateString());

            readDrinfo(clsDB.DbCon);            
            GetData(clsDB.DbCon,ssList1, strSDate,strTDate);

            GetData2(clsDB.DbCon,ssList2, strSDate,strTDate);

            ssList1.Enabled = true;
            ssList2.Enabled = true;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
    }
    
}
