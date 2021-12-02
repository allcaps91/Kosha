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
    public partial class frmComSupFnExSCH03 : Form
    {
        /// <summary>
        /// Class Name      : ComSupLibB.SupFnEx
        /// File Name       : frmComSupFnExSCH03.cs
        /// Description     : 종검 검사 스케쥴 보기
        /// Author          : 윤조연
        /// Create Date     : 2017-08-10
        /// Update History  :  
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\haMain18-2.frm(frm종검예약보기) >> frmComSupFnExSCH03.cs 폼이름 재정의" /> 
        /// 

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
        int gPart = 0;
  
        #endregion

        public frmComSupFnExSCH03()
        {
            InitializeComponent();
            
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            //txtMonth.Text = VB.Left(cpublic.strSysDate, 4) + "년 " + VB.Mid(cpublic.strSysDate, 6, 2) + "월";

            sup.setYYYYMM(cboYYMM, cpublic.strSysDate, 24, 12, 12, "-");

            if (gPart == 1)
            {
                chkJob1.Checked = true;
            }
            else if (gPart == 2)
            {
                chkJob2.Checked = true;
            }

        }

        void setCtrlInit(PsmhDb pDbCon)
        {
            clsCompuInfo.SetComputerInfo();
            DataTable dt = ComQuery.Select_BAS_PCCONFIG(pDbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_생리기능검사");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                //파트설정로드
                if (dt.Rows[0]["VALUEV"].ToString() == clsComSupFnEx.enm_FnExPart.PART1.ToString())
                {
                    gPart = (int)clsComSupFnEx.enm_FnExPart.PART1;                    
                }
                else if (dt.Rows[0]["VALUEV"].ToString() == clsComSupFnEx.enm_FnExPart.PART2.ToString())
                {
                    gPart = (int)clsComSupFnEx.enm_FnExPart.PART2;
                }
                else
                {
                    gPart = 0;
                }
            }
            else
            {
                gPart = 0;
            }
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
            this.btnClose.Click += new EventHandler(eBtnEvent);

            this.btnPrint.Click += new EventHandler(eBtnEvent);
            
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

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
                setSpd(ssList2);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlInit(clsDB.DbCon);

                screen_clear();

                setCtrlData();

                //기본 조회
                GetData(clsDB.DbCon,ssList);
            }
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnClose)
            {
                panList.Visible = false;
            }
            else if (sender == this.btnPrint)
            {
                //인쇄
                ePrint();

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

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.RowHeader == true || e.Column < 1)
            {
                return;
            }

            FpSpread s = (FpSpread)sender;

            if (sender == ssList)
            {
                string strTemp = (cboYYMM.Text.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
                string strSDate = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" + VB.Left(s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), 2);

                panList.Visible = true;
                lblDate.Text = strSDate + " 예약현황";
                GetData2(clsDB.DbCon,ssList2, strSDate);


            }
            else if (sender == ssList2)
            {
                #region 예전 VB DLL 연동 부분 주석처리 2019-09-19 KMC
                //string strTemp = "";
                //strTemp += clsComSup.setP(lblDate.Text.Trim(), " ", 1) + "@@"; //날짜
                //strTemp += s.ActiveSheet.Cells[e.Row, 1].Text.Trim() + "@@"; //이름
                //strTemp += s.ActiveSheet.Cells[e.Row, 2].Text.Trim() + "@@"; //pano
                //strTemp += s.ActiveSheet.Cells[e.Row, 3].Text.Trim() + "@@"; //검사명칭
                //strTemp += s.ActiveSheet.Cells[e.Row, 5].Text.Trim() + "@@"; //오더코드
                //strTemp += s.ActiveSheet.Cells[e.Row, 6].Text.Trim() + "@@"; //bdate
                //if (s.ActiveSheet.Cells[e.Row, 0].Text.Trim() != "HR")
                //{
                //    strTemp += "TO@@"; //과
                //}
                //else
                //{
                //    strTemp += s.ActiveSheet.Cells[e.Row, 0].Text.Trim() + "@@"; //과
                //}
                ////종검결과입력 연동
                //toResult_show(strTemp); 
                #endregion

                //VB DLL 폼 컨버전하여 연동시킴 2019-09-19 김민철
                string strTemp1 = clsComSup.setP(lblDate.Text.Trim(), " ", 1); //날짜
                string strTemp2 = s.ActiveSheet.Cells[e.Row, 1].Text.Trim(); //이름
                string strTemp3 = s.ActiveSheet.Cells[e.Row, 2].Text.Trim(); //pano
                string strTemp4 = s.ActiveSheet.Cells[e.Row, 3].Text.Trim(); //검사명칭
                string strTemp5 = s.ActiveSheet.Cells[e.Row, 5].Text.Trim(); //오더코드
                string strTemp6 = s.ActiveSheet.Cells[e.Row, 6].Text.Trim(); //bdate
                string strTemp7 = s.ActiveSheet.Cells[e.Row, 0].Text.Trim() != "HR" ? "TO" : s.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                frmComSupFnExSET02 f = new frmComSupFnExSET02(strTemp4, strTemp2, strTemp3, strTemp7, strTemp1, strTemp6, strTemp5);
                f.ShowDialog();

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

            strTitle = "종합검진 스케쥴 " + "(" + cboYYMM.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        string Gubun2Code()
        {
            string strTemp = "";

            if (chkJob1.Checked == true)
            {
                strTemp += " 'TZ16'  ";
            }
            if (chkJob2.Checked == true)
            {
                if (strTemp != "")
                {
                    strTemp += " ,'TX84','TX68','TX89','TX44'  ";
                }
                else
                {
                    strTemp += " 'TX84','TX68','TX89','TX44'  ";
                }
            }
            
            if (strTemp =="")
            {
                strTemp = " 'TX89','TX84','TX68','TX44','TZ16' ";
            }

            return strTemp;
        }

        string Gubun2CodeOLD()
        {
            //사용안함

            string strTemp = "";

            if (chkJob1.Checked == true)
            {
                strTemp += " 'TX89','TX44','TX84','TX68','TZ16'  ";
            }
            if (chkJob2.Checked == true)
            {
                if (strTemp != "")
                {
                    strTemp += " ,'TX84','TX68','TX89','TX44'  ";
                }
                else
                {
                    strTemp += " 'TX84','TX68','TX89','TX44'  ";
                }
            }
            //if (chkJob3.Checked == true)
            //{
            //    if (strTemp != "")
            //    {
            //        strTemp += " ,'TZ16'  ";
            //    }
            //    else
            //    {
            //        strTemp += " 'TZ16'  ";
            //    }
            //}
            if (strTemp == "")
            {
                strTemp = " 'TX89','TX84','TX68','TX44','TZ16' ";
            }

            return strTemp;
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

        void setSpd(FarPoint.Win.Spread.FpSpread Spd)
        {
            Spd.ActiveSheet.Columns.Get(5).Visible = false;
            Spd.ActiveSheet.Columns.Get(6).Visible = false;
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
            return VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";

        }

        void disp_Calendar(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate, int start)
        {
            int nRow = 0;
            int nCol = 0;
            string strTemp = "";            
            DataTable dt = null;

            string strExCode = Gubun2Code();
            
            if (strExCode =="")
            {
                ComFunc.MsgBox("검사조건을 선택후 조회하십시오!!");
                return;

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

                //쿼리실행            
                dt = fnExSql.sel_HEA_RESV_EXAM(pDbCon, "UNION", VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2),"",strExCode, chkJob1.Checked.ToString());

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        
                        strTemp += "(" + dt.Rows[j]["RDate2"].ToString().Trim() + ")";

                        if (dt.Rows[j]["GBN"].ToString().Trim() == "HR")
                        {
                            //strTemp += "(HR)";
                            strTemp = "";
                        }
                        else if (dt.Rows[j]["GBN"].ToString().Trim() == "PS")
                        {
                            //strTemp += "(P)";
                        }
                        else
                        {
                            
                            if (dt.Rows[j]["ACTIVE"].ToString().Trim() == "Y")
                            {
                                strTemp += "○";
                            }
                            else
                            {
                                strTemp += "Ｘ";
                            }                            
                            
                        }

                        
                        if (dt.Rows[j]["GBN"].ToString().Trim() == "PS")
                        {
                            strTemp += dt.Rows[j]["Ptno"].ToString().Trim() + "  ";
                            strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(P)" + "\r\n";
                            if (dt.Rows[j]["ExamName"].ToString().Trim() != "")
                            {
                                strTemp += dt.Rows[j]["ExamName"].ToString().Trim() + "\r\n";
                            }
                        }
                        else if (dt.Rows[j]["GBN"].ToString().Trim() == "HR")
                        {
                            if (dt.Rows[j]["ExamName"].ToString().Trim() != "")
                            {
                                strTemp += dt.Rows[j]["ExamName"].ToString().Trim() + " ";
                            }
                            strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(HR)" + "\r\n";
                        }
                        else
                        {
                            strTemp += dt.Rows[j]["SName"].ToString().Trim() + "  ";
                            if (dt.Rows[j]["ExamName"].ToString().Trim() != "")
                            {
                                strTemp += dt.Rows[j]["ExamName"].ToString().Trim() + "\r\n";
                            }
                        }
                        
                        

                        //if ( chkJob1.Checked == false && chkJob2.Checked == false)
                        //{
                        //    strTemp += dt.Rows[j]["Ptno"].ToString().Trim() + "  ";
                        //    if (dt.Rows[j]["GBN"].ToString().Trim() == "PS")
                        //    {
                        //        strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(P)" + "\r\n";
                        //    }
                        //    else
                        //    {
                        //        strTemp += dt.Rows[j]["SName"].ToString().Trim() + "\r\n";
                        //    }                        
                        //}
                        //else
                        //{
                        //    strTemp += dt.Rows[j]["SName"].ToString().Trim() + "  ";
                        //    strTemp += dt.Rows[j]["ExamName"].ToString().Trim() + "\r\n";
                        //}                        

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


                Spd.Enabled = true;

            }

        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate)
        {

            int i = 0;            
            DataTable dt = null;

            string strExCode = Gubun2Code();

            if (strExCode == "")
            {
                ComFunc.MsgBox("검사조건을 선택후 조회하십시오!!");
                return;

            }

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = fnExSql.sel_HEA_RESV_EXAM( pDbCon, "UNION", argSDate, "", strExCode, "False");

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["GBN"].ToString().Trim() =="HR")
                    {
                        Spd.ActiveSheet.Cells[i, 0].Text = "HR";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["RDate2"].ToString().Trim();
                    }
                    
                    Spd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ExamName"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, 4].Text = "";
                    if (dt.Rows[i]["ACTIVE"].ToString().Trim() == "Y")
                    {
                        Spd.ActiveSheet.Cells[i, 4].Text = "○";
                    }
                    Spd.ActiveSheet.Cells[i, 5].Text = ExCode2OrderCode(dt.Rows[i]["ExCode"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["JepDate"].ToString().Trim();


                }
            }

            dt.Dispose();
            dt = null;

           
            Cursor.Current = Cursors.Default;

            #endregion


        }

        string ExCode2OrderCode(string argCode)
        {
            string str = "";

            if (argCode =="TX84")
            {
                str = "US22";
            }
            else if (argCode == "TX44")
            {
                str = "E6545";
            }
            else if (argCode == "TX89")
            {
                str = "E6543";
            }
            else if (argCode == "TX68")
            {
                str = "US-CADU1";
            }
            else if (argCode == "TZ16")
            {
                str = "USTCD";
            }
            else
            {
                str = "";
            }

            return str;
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList);
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
    }
}
