using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmDrSchedule_View.cs
/// Description     : 건강증진센터 진료과장 진료일정표
/// Author          : 김경동
/// Create Date     : 2020-06-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건진진료일정표(Frm건진진료일정표.frm)" />

namespace ComHpcLibB
{
    public partial class frmDrSchedule_View : Form
    {

        int FnSign = 0;
        int FnCheckCol = 0;
        int FnCheckRow = 0;

        int FnLastDD = 0;
        int FnCurrDD = 0;
        string FstrCurDate = string.Empty;   //현재일자
        string FstrGetSDate = string.Empty;   //시작일자
        string FstrGetLDate = string.Empty;   //종료일자
        string FstrGubun = string.Empty;

        ComFunc CF = null;
        clsSpread cSpd = null;
        //HeaDayService heaDayService = null;
        //HicChulresvService hicChulresvService = null;
        HicDoctorService hicDoctorService = null;
        BasScheduleService basScheduleService = null;
        BasDoctorService basDoctorService = null;


        public frmDrSchedule_View()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLeft.Click += new EventHandler(eBtnClick);
            this.btnRight.Click += new EventHandler(eBtnClick);
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();
            //heaDayService = new HeaDayService();
            //hicChulresvService = new HicChulresvService();
            hicDoctorService = new HicDoctorService();
            basScheduleService = new BasScheduleService();
            basDoctorService = new BasDoctorService();

            ssCal.Initialize(new SpreadOption { ColumnHeaderHeight = 20, RowHeightAuto = true, RowHeight = 131 });
            ssCal.AddColumn("일", "", 40, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });  //, CellVerticalAlignment.Top
            ssCal.AddColumn("월 MON", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("화 TUE", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("수 WED", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("목 THU", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("금 FRI", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.AddColumn("토 SAT", "", 197, new SpreadCellTypeOption { IsEditble = false, IsMulti = true, Aligen = CellHorizontalAlignment.Left, VAligen = CellVerticalAlignment.Top });
            ssCal.ActiveSheet.RowCount = 5;
        }



        private void eFormLoad(object sender, EventArgs e)
        {
            int nLastDD = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int nCurrDD = DateTime.Now.Day;

            string strCurDate = DateTime.Now.ToShortDateString();
            string strGetSDate = VB.Mid(strCurDate, 1, 8) + "01";
            string strGetLDate = VB.Mid(strCurDate, 1, 8) + VB.Format(nLastDD, "00");

            int nStartCol = Convert.ToInt32(Convert.ToDateTime(strGetSDate).DayOfWeek);   //요일 시작



            FnSign = nStartCol;             //시작 Col
            FnLastDD = nLastDD;
            FnCurrDD = nCurrDD;

            FstrCurDate = strCurDate;       //현재일자
            FstrGetSDate = strGetSDate;     //시작일자
            FstrGetLDate = strGetLDate;     //종료일자

            lblYear.Text = DateTime.Now.Year.ToString();
            lblMonth.Text = VB.Format(DateTime.Now.Month, "00");

            SS_Calendar_Clear();
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);                  //기본달력 그리기
            //Data_Search(strGetSDate, strGetLDate, strGubun, nStartCol);     //Data 조회
        }

        private void eBtnClick(object sender, EventArgs e)
        {

            if (sender == btnSearch)
            {

                SS_Calendar_Clear();
                SS_Calendar_Show(FnLastDD, FnSign, FnCurrDD);                  //기본달력 그리기
                Data_Search(FstrGetSDate, FstrGetLDate, FnSign);
            }
            else if (sender == btnLeft)
            {
                Screen_Display(btnLeft);
            }
            else if (sender == btnRight)
            {
                Screen_Display(btnRight);
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
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
            string strCurMonth = string.Empty;
            string strRemark = string.Empty;

            for (int i = 1; i <= ArgLastDD; i++)
            {
                ssCal.ActiveSheet.Cells[nRow, nCol].Text = "<" + i.To<string>() + ">";

                if (nCol == 0)      //일요일
                {
                    ssCal.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.DarkRed;
                    ssCal.ActiveSheet.Cells[nRow, nCol].BackColor = Color.Pink;
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

                strCurMonth = lblYear.Text + "-" + VB.Mid(FstrGetSDate, 6, 2) + "-" + VB.Format(i, "00");

                //strRemark = heaDayService.GetRemarkByDate(strCurMonth);

                if (!strRemark.IsNullOrEmpty())
                {
                    ssCal.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.DarkRed;
                    ssCal.ActiveSheet.Cells[nRow, nCol].BackColor = Color.MistyRose;
                    ssCal.ActiveSheet.Cells[nRow, nCol].Text += ComNum.VBLF + strRemark;
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

            lblYear.Text = VB.Left(strGetSDate, 4);
            lblMonth.Text = VB.Mid(strGetSDate, 6, 2);

            SS_Calendar_Clear();
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);        //기본달력 그리기
            Data_Search(strGetSDate, strGetLDate, nStartCol);     //Data 조회

        }


        private void Data_Search(string ArgStartDate, string ArgLastDate, int nStartCol)
        {
            int GetDD = 0;
            int nRow = 0;
            int nCol = nStartCol;

            string strTemp = "";
            string strTemp1 = "a";
            int K = 0;
            int nHeight = 80;

            int nDD = 0;
            int nColStart = 0;
            List<string> strDrList = new List<string>();
            string strOLD = "";
            string strNEW = "";
            string strDRNAME = "";
            string strSche = "";
            string[] strGuntea = new string[31];



            Cursor.Current = Cursors.WaitCursor;



            List<HIC_DOCTOR> list = hicDoctorService.GetListbyReday(ArgStartDate);
            strDrList.Add("1109");
            //strDrList = "1109',";
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strDrList.Add(list[i].DRCODE.Trim());
                    //strDrList = strDrList + "'" + list[i].DRCODE + "',";
                }
                
            }

            //if (VB.Right(strDrList, 2) == "',") { strDrList = VB.Left(strDrList, VB.Len(strDrList) - 2); }

            List<BAS_SCHEDULE> list2 = basScheduleService.GetItembySchDateDrcodes(ArgStartDate, ArgLastDate, strDrList);
            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {


                    nDD = Convert.ToInt32(list2[i].ILJA);
                    strNEW = list2[i].DRCODE;

                    if (strOLD != strNEW)
                    {
                        strOLD = strNEW;
                        BAS_DOCTOR item = basDoctorService.GetItembyDrCord(strNEW);

                        strDRNAME = item.DRNAME;
                    }

                    if (nDD == 2)
                    {
                        nDD = nDD;
                        strDRNAME = strDRNAME;
                    }


                    strSche = list2[i].GBJIN + list2[i].GBJIN2;
                    if (((string.Compare(VB.Left(strSche, 1), "5") >= 0 && VB.Left(strSche,1) != "6")) && (string.Compare(VB.Left(strSche, 1), "8") <= 0))

                    {
                        switch (VB.Left(strSche,1))
                        {
                            case "5":
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 학회";
                                break;
                            case "6":
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 휴가";
                                break;
                            case "7":
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 출장";
                                break;
                            case "8":
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 기타";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (list2[i].GBDAY == "1")
                        {
                            if (strSche == "99")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " OFF";
                            }
                            else if (VB.Left(strSche, 1) == "9")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " AOFF";
                            }
                            else if (VB.Right(strSche, 1) == "9")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " POFF";
                            }
                            else if (VB.Left(strSche, 1) == "9")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " AOFF";
                            }
                            else if (strSche == "66")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 휴가";
                            }
                            else if (VB.Left(strSche, 1) == "6")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 오전반휴";
                            }
                            else if (VB.Right(strSche, 1) == "6")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " 오후반휴";
                            }
                        }
                        else if (list2[i].GBDAY == "2")
                        {
                            if (VB.Left(strSche, 1) == "9")
                            {
                                strGuntea[nDD] = strGuntea[nDD] + ComNum.VBLF + strDRNAME + " OFF";
                            }

                        }

                    }
                }
              
                //해당월 1일자 위치 찾음
                for (int i = 0; i < 6; i++)
                {

                   if (VB.Left(ssCal.ActiveSheet.Cells[0, i].Text, 2) == "<1")
                   {
                        nColStart = i-2;
                        FnSign = nColStart;
                        break;
                   }

                }

                nRow = 0;
                nCol = nColStart;
                for (int i = 0; i< 30; i++)
                {
                    nCol = nCol+1;
                    if (nCol > 6 )
                    {
                        nRow = nRow+1;
                        nCol = 0;
                        if (nRow > 4) { nRow = 0; }
                    }

                    ssCal.ActiveSheet.Cells[nRow, nCol].Text = ssCal.ActiveSheet.Cells[nRow, nCol].Text + strGuntea[i];


                }


            }




            

            Cursor.Current = Cursors.Default;
        }
    }
}
