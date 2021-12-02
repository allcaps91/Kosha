using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using HC_Core;
using HC_Measurement.Dto;
using HC_Measurement.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_Measurement
{
    public partial class frmHcChkSchedule :CommonForm
    {
        int FnSign = 0;
        int FnCheckCol = 0;
        int FnCheckRow = 0;

        int FnLastDD = 0;
        int FnCurrDD = 0;
        string FstrCurDate  = string.Empty;   //현재일자
        string FstrGetSDate = string.Empty;   //시작일자
        string FstrGetLDate = string.Empty;   //종료일자
        string FstrGubun = string.Empty;

        ComFunc CF = null;
        clsSpread cSpd = null;

        HicChukMstNewService hicChukMstNewService = null;

        public frmHcChkSchedule()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load                  += new EventHandler(eFormLoad);
            this.btnExit.Click         += new EventHandler(eBtnClick);
            this.btnSearch.Click       += new EventHandler(eBtnClick);
            this.btnPrint.Click        += new EventHandler(eBtnClick);
            this.btnLeft.Click         += new EventHandler(eBtnClick);
            this.btnRight.Click        += new EventHandler(eBtnClick);
            this.ssCal.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == ssCal)
            {
                string strDate = VB.Pstr(ssCal.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), ">", 1);
                strDate = VB.Pstr(strDate, "<", 2);

                if (!strDate.IsNullOrEmpty())
                {
                    strDate = lblYear.Text + "-" + lblMonth.Text + "-" + VB.Format(strDate.To<int>(), "00");
                    string strGubun = "1";     //1.출장,2.내원 (기본출장)

                    //frmHcScheduleEntry frm = new frmHcScheduleEntry(strDate, strGubun);
                    //frm.ShowDialog();
                }
            }
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();

            hicChukMstNewService = new HicChukMstNewService();

            ssCal.Initialize(new SpreadOption { ColumnHeaderHeight = 20, RowHeightAuto = true, RowHeight = 131 });
            ssCal.AddColumn("일",     "",  40, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left,  VAligen = CellVerticalAlignment.Top });  //, CellVerticalAlignment.Top
            ssCal.AddColumn("월 MON", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("화 TUE", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("수 WED", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("목 THU", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("금 FRI", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("토 SAT", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.ActiveSheet.RowCount = 5;

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                FstrGubun = rdoView1.Checked ? "1" : "2";

                SS_Calendar_Clear();
                SS_Calendar_Show(FnLastDD, FnSign, FnCurrDD);                  //기본달력 그리기
                Data_Search(FstrGetSDate, FstrGetLDate, FstrGubun, FnSign);
            }
            else if (sender == btnLeft)
            {
                Screen_Display(btnLeft);
            }
            else if (sender == btnRight)
            {
                Screen_Display(btnRight);
            }

            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strHeader = cSpd.setSpdPrint_String(  lblMonth.Text + "월 측정팀 출장 일정표     "+ VB.Space(50) + "인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 12), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, true, 0.80f);
                cSpd.setSpdPrint(ssCal, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        private void Screen_Display(Button argBtn)
        {
            string strGetSDate = string.Empty;

            if (argBtn.Name == "btnLeft")
            {
                strGetSDate = CF.DATE_ADD(clsDB.DbCon, FstrGetSDate, -1);
                strGetSDate = VB.Mid(strGetSDate, 1, 8) + "01";
            }
            else if (argBtn.Name == "btnRight")
            {
                strGetSDate = CF.DATE_ADD(clsDB.DbCon, FstrGetLDate, 1);
            }

            int nLastDD = DateTime.DaysInMonth(VB.Left(strGetSDate, 4).To<int>(), VB.Mid(strGetSDate, 6, 2).To<int>());
            int nCurrDD = DateTime.Now.Day;


            string strGetLDate = VB.Mid(strGetSDate, 1, 8) + VB.Format(nLastDD, "00");

            int nStartCol = Convert.ToInt32(Convert.ToDateTime(strGetSDate).DayOfWeek);   //요일 시작

            FnSign = nStartCol;             //시작 Col
            FnLastDD = nLastDD;
            FnCurrDD = nCurrDD;

            FstrGetSDate = strGetSDate;     //시작일자
            FstrGetLDate = strGetLDate;     //종료일자
            FstrGubun = rdoView1.Checked ? "1" : "2";

            lblYear.Text = VB.Left(strGetSDate, 4);
            lblMonth.Text = VB.Mid(strGetSDate, 6, 2);

            SS_Calendar_Clear();
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);                  //기본달력 그리기
            Data_Search(strGetSDate, strGetLDate, FstrGubun, nStartCol);     //Data 조회
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            int nLastDD = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int nCurrDD = DateTime.Now.Day;
            
            string strCurDate  = DateTime.Now.ToShortDateString();
            string strGetSDate = VB.Mid(strCurDate, 1, 8) + "01";
            string strGetLDate = VB.Mid(strCurDate, 1, 8) + VB.Format(nLastDD, "00");
            
            int nStartCol = Convert.ToInt32(Convert.ToDateTime(strGetSDate).DayOfWeek);   //요일 시작

            string strGubun = "1";     //1.예비조사, 2.환경측정

            FnSign = nStartCol;             //시작 Col
            FnLastDD = nLastDD;
            FnCurrDD = nCurrDD;

            FstrCurDate = strCurDate;       //현재일자
            FstrGetSDate = strGetSDate;     //시작일자
            FstrGetLDate = strGetLDate;     //종료일자
            FstrGubun = strGubun;

            lblYear.Text = DateTime.Now.Year.ToString();
            lblMonth.Text = VB.Format(DateTime.Now.Month, "00");

            SS_Calendar_Clear();
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);                  //기본달력 그리기
            Data_Search(strGetSDate, strGetLDate, strGubun, nStartCol);     //Data 조회
        }

        private void SS_Calendar_Clear()
        {
            cSpd.setSpdCellColor(ssCal, 0, 0, ssCal.ActiveSheet.RowCount - 1, ssCal.ActiveSheet.ColumnCount - 1, Color.White);
            cSpd.Spread_Clear(ssCal, ssCal.ActiveSheet.RowCount, ssCal.ActiveSheet.ColumnCount);
        }

        /// <summary>
        /// 기본달력 그리기
        /// </summary>
        /// <param name="nLastDD">해당월끝일</param>
        /// <param name="nStartCol">요일</param>
        /// <param name="nCurrDD">현재일자</param>
        private void SS_Calendar_Show(int ArgLastDD, int ArgStartCol, int ArgCurDD)
        {
            int nRow = 0;
            int nCol = ArgStartCol;
            string strRemark = string.Empty;

            for (int i = 1; i <= ArgLastDD; i++)
            {
                ssCal.ActiveSheet.Cells[nRow, nCol].Text = "<" + i.To<string>() + ">";

                if (nCol == 0)      //일요일
                {
                    ssCal.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.DarkRed;
                }
                else if (nCol == 6)  //토요일
                {
                    ssCal.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.DarkBlue;
                }
                else
                {
                    ssCal.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.Black;
                }

                if (i == ArgCurDD)
                {
                    FnCheckCol = nCol;
                    FnCheckRow = nRow;
                }

                nCol += 1;

                if (nCol > 6)
                {
                    nRow += 1;
                    nCol = 0;
                }

                if (nRow > 4) { nRow = 0; }
            }
        }

        private void Data_Search(string ArgStartDate, string ArgLastDate, string ArgGubun, int nStartCol)
        {
            int sDD = 0;
            int eDD = 0;
            int nRow = 0;
            int nCol = nStartCol;

            string strDate1 = string.Empty;
            string strDate2 = string.Empty;
            string strCurDate = string.Empty;

            string strTemp = "";
            
            int K = 0;
            int nHeight = 80;

            Cursor.Current = Cursors.WaitCursor;

            nCol = nStartCol;

            //for (int i = 0; i < 7; i++)
            //{
            //    if (ssCal.ActiveSheet.Cells[0, i].Text == "<1>")
            //    {
            //        nCol = i;
            //        FnSign = i;
            //    }
            //}

            List<HIC_CHUKMST_NEW> list = hicChukMstNewService.GetListByDateGubun(ArgStartDate, ArgLastDate, ArgGubun);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (ArgGubun == "1")
                    {
                        strDate1 = list[i].SDATE.To<string>("");
                        strDate2 = list[i].EDATE.To<string>("");
                        sDD = list[i].SDATE.To<string>("").Substring(8, 2).To<int>(0);
                        eDD = list[i].EDATE.To<string>("").Substring(8, 2).To<int>(0);
                    }
                    else
                    {
                        strDate1 = list[i].BDATE.To<string>("");
                        strDate2 = list[i].BDATE.To<string>("");
                        sDD = list[i].BDATE.To<string>("").Substring(8, 2).To<int>(0);
                        eDD = list[i].BDATE.To<string>("").Substring(8, 2).To<int>(0);
                    }

                    nRow = (sDD + nCol) / 7;
                    nCol = Find_Calender_Start_Pos(sDD, nRow);
                    
                    if (sDD == eDD)
                    {
                        strTemp = list[i].WRTNO.To<string>("") + "]" + " " + list[i].LTDNAME + ComNum.VBLF;
                        ssCal.ActiveSheet.Cells[nRow, nCol].Text += strTemp;
                    }
                    else
                    {
                        

                        for (int j = sDD; j <= eDD; j++)
                        {
                            strTemp = list[i].WRTNO.To<string>("") + "]" + " " + list[i].LTDNAME + ComNum.VBLF;
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += strTemp;

                            nCol += 1;
                        }
                    }

                    Size size = ssCal.ActiveSheet.GetPreferredCellSize(nRow, nCol);
                    size.Height = size.Height + 15;
                    if (nHeight < size.Height)
                    {
                        ssCal.ActiveSheet.Rows[nRow].Height = size.Height;
                        nHeight = size.Height;
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private int Find_Calender_Start_Pos(int sDD, int nRow)
        {
            int rtnVal = 0;

            for (int i = 0; i < 7; i++)
            {
                if (sDD == VB.Pstr(VB.Pstr(ssCal.ActiveSheet.Cells[nRow, i].Text, ">", 1), "<", 2).To<int>(0))
                {
                    rtnVal = i;
                    break;
                }
            }

            return rtnVal;
        }
    }
}
