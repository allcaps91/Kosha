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
    /// File Name       : frmComSupEndsSCH01.cs
    /// Description     : 종검 대장내시경 스케쥴 보기
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\\\.frm() >> frmComSupEndsSCH01.cs 폼이름 재정의" />
    /// 
    public partial class frmComSupEndsSCH01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSupSpd ccomspd = new clsComSupSpd();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();


        string[] strDay = null;
        string[] strDayCnt = null;

        bool gShow = true;

        #endregion

        public frmComSupEndsSCH01(bool bShow =true)
        {
            InitializeComponent();

            gShow = bShow;

            if (bShow==false)
            {
                panheader4.Visible = false;
                panel1.Visible = false;
            }

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
            this.btnClose.Click += new EventHandler(eBtnEvent);

            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);


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

                ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
                ssList.TextTipDelay = 500;

                screen_clear();

                setCtrlData();

                //기본 조회
                GetData(clsDB.DbCon,ssList);

                if (gShow == false)
                {
                    ssList.ZoomFactor = (float)0.95;
                }

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
                string strSDate = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-" +  VB.Left(s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), 2);

                if (VB.Len(strSDate) !=10)
                {
                    panList.Visible = false;
                    ComFunc.MsgBox("날짜를 다시 선택하십시오!!"); ;
                    return;
                }

                panList.Visible = true;
                lblDate.Text = strSDate + " 예약현황";
                GetData2(clsDB.DbCon, ssList2, strSDate);
            
                
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
                e.TipText = s.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                e.ShowTip = true;
                
            }
            catch
            {

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

        public void screan_display(string argYYMM)
        {
            cboYYMM.Text = argYYMM;
            GetData(clsDB.DbCon,ssList);
        }

        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd)
        {
            string strTemp = (cboYYMM.Text.Trim().Replace("년", "").Replace("월", "")).Replace(" ", "").Trim();
            string strSDate = VB.Left(strTemp, 4) + "-" + VB.Mid(strTemp, 5, 2) + "-01";
            string strTDate = Convert.ToString(Convert.ToDateTime(strSDate).AddMonths(1).AddDays(-1).ToShortDateString());

            read_sysdate();

            int start = Convert.ToInt32(Convert.ToDateTime(strSDate).DayOfWeek);

            //달력세팅 및 데이타 표시
            disp_Calendar(pDbCon ,Spd, strSDate, strTDate, start);


        }

        void disp_Calendar(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate, int start)
        {
            int nRow = 0;
            int nCol = 0;
            int j = 0;
            string strTemp = "";
            string strPano = "";
            DataTable dt = null;
            DataTable dt2 = null;
            DataTable dt3 = null;
            DataTable dt4 = null;

            //일자별 건수체크
            strDayCnt = endsSql.read_DayCnt(pDbCon, argSDate, argTDate);

            //휴일체크
            strDay = sup.read_huil(pDbCon, argSDate, argTDate);

            //clear
            Spd.ActiveSheet.ClearRange(0, 0, (int)Spd.ActiveSheet.RowCount, (int)Spd.ActiveSheet.ColumnCount, true);

            nCol = start;

            for (int i = 0; i < Convert.ToInt16(VB.Right(argTDate, 2)); i++)
            {

                Spd.ActiveSheet.Cells[nRow, nCol].Text = ComFunc.SetAutoZero((i+1).ToString(),2);
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


                string strTemp2 = "";
                dt = endsSql.sel_HEA_RESV_EXAM(pDbCon, "03", VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2), "", " '01','02' ", "");
                if (dt.Rows.Count > 0)
                {
                    for( j = 0; j < dt.Rows.Count; j++)
                    {
                        strTemp2 += dt.Rows[j]["Pano"].ToString().Trim() + ",";
                    }
                }

                //쿼리실행            
                dt = endsSql.sel_HEA_RESV_EXAM(pDbCon, "02",VB.Left(argSDate, 8) + ComFunc.SetAutoZero((i + 1).ToString(), 2),""," '02' ","");

                if (dt.Rows.Count > 0)
                {
                    strTemp = ComFunc.SetAutoZero((i + 1).ToString(),2) + "   " + strDayCnt[i] + "\r\n";

                    for ( j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["GBEXAM"].ToString().Trim()=="02")
                        {
                            strPano = dt.Rows[j]["Pano"].ToString().Trim();

                            strTemp += "(" + dt.Rows[j]["RTIME2"].ToString().Trim() + ")";

                            if (strTemp2.Contains(strPano) == true)
                            {
                                strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(+) ";
                            }
                            else
                            {
                                strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(-) ";
                            }
                            //if (dt.Rows[j]["FC_HIC_VIP_CHK"].ToString().Trim() == "Y")//2018-10-29
                            //{
                            //    strTemp += "VIP ";
                            //}

                            dt2 = endsSql.sel_Hea_Wrtno(pDbCon, strPano);

                            if (dt2.Rows.Count > 0)
                            {
                                dt3 = endsSql.sel_Hea_VIPCHK(pDbCon, dt2.Rows[0]["WRTNO"].ToString().Trim());
                                if (dt3.Rows.Count > 0)
                                {
                                    if (VB.Left(dt3.Rows[0]["YName"].ToString().Trim(), 4) == "골드검진" || VB.Left(dt3.Rows[0]["YName"].ToString().Trim(), 4) == "VIP검")
                                    {
                                        strTemp += "VIP ";
                                    }
                                    else
                                    {
                                        dt4 = endsSql.sel_Hea_AMT(pDbCon, dt2.Rows[0]["WRTNO"].ToString().Trim());
                                        if (dt4.Rows.Count > 0)
                                        {
                                            strTemp += "VIP ";
                                        }
                                        dt4.Dispose();
                                        dt4 = null;
                                    }
                                }
                                dt3.Dispose();
                                dt3 = null;
                            }
                            dt2.Dispose();
                            dt2 = null;                      
                           

                            //if (dt.Rows.Count > j+1 && dt.Rows[j]["Pano"].ToString().Trim()== dt.Rows[j+1]["Pano"].ToString().Trim() && dt.Rows[j +1]["GBEXAM"].ToString().Trim() =="01") 
                            //{
                            //    strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(+) ";
                            //}
                            //else
                            //{
                            //    strTemp += dt.Rows[j]["SName"].ToString().Trim() + "(-) ";
                            //}

                            if (dt.Rows[j]["LtdCode"].ToString().Trim() == "951")
                            {
                                strTemp += "한수원 \r\n";
                            }
                            else if (dt.Rows[j]["LtdCode"].ToString().Trim() == "3858")
                            {
                                strTemp += "한전KPS \r\n";
                            }
                            else
                            {
                                strTemp += "\r\n";
                            }

                        }
                        
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

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argSDate)
        {

            int i = 0;
            int nRow = 0;            
            string strNEW = "";
            string strOLD = "";
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            string strTemp = "";
            dt = endsSql.sel_HEA_RESV_EXAM(pDbCon, "03", argSDate, "", " '01','02' ", "");
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strTemp += dt.Rows[i]["Pano"].ToString().Trim() + "," ; 
                }
            }

            //쿼리실행      
            dt = endsSql.sel_HEA_RESV_EXAM(pDbCon, "02", argSDate, "", " '02' ", "");

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["GbExam"].ToString().Trim() =="02")
                    {
                        strNEW = dt.Rows[i]["Pano"].ToString().Trim();
                                                
                        Spd.ActiveSheet.Cells[nRow, 0].Text = dt.Rows[i]["RTime2"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, 2].Text = strNEW;
                        Spd.ActiveSheet.Cells[nRow, 3].Text = "대장내시경";

                        if (strTemp.Contains(strNEW) == true)
                        {
                            Spd.ActiveSheet.Cells[nRow, 3].Text = "대장내시경,위내시경";
                        }

                        //if (dt.Rows.Count > i+1 && strNEW ==dt.Rows[i+1]["Pano"].ToString().Trim() && dt.Rows[i + 1]["GbExam"].ToString().Trim() =="01")
                        //{
                        //    Spd.ActiveSheet.Cells[nRow, 3].Text = "대장내시경,위내시경";
                        //}

                        if (dt.Rows[i]["LtdCode"].ToString().Trim() == "0951" || dt.Rows[i]["LtdCode"].ToString().Trim() == "3858")
                        {
                            Spd.ActiveSheet.Cells[nRow, 3].BackColor = label1.BackColor;
                        }


                        nRow++;
                       

                        strOLD = dt.Rows[i]["Pano"].ToString().Trim();

                    }

                    
                }
            }

            dt.Dispose();
            dt = null;

            Spd.ActiveSheet.RowCount = nRow;

            Cursor.Current = Cursors.Default;

            #endregion


        }   

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

    }

}
