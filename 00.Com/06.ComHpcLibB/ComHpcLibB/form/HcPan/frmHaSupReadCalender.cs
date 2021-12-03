using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaSupReadCalender.cs
/// Description     : 종검 기능검사 판독조회
/// Author          : 김민철
/// Create Date     : 2021-03-24
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종검예약보기(HaMain18-2.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaSupReadCalender :Form
    {
        int FnSign = 0;
        int FnCheckCol = 0;
        int FnCheckRow = 0;
        int FnLastDD = 0;
        int FnCurrDD = 0;

        string FstrCurDate  = string.Empty;   //현재일자
        string FstrGetSDate = string.Empty;   //시작일자
        string FstrGetLDate = string.Empty;   //종료일자

        clsSpread cSpd = null;
        ComFunc CF = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaResvExamService heaResvExamService = null;
        HeaResultService heaResultService = null;
        HicJepsuResultService hicJepsuResultService = null;

        public frmHaSupReadCalender()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            CF = new ComFunc();
            comHpcLibBService = new ComHpcLibBService();
            heaResvExamService = new HeaResvExamService();
            heaResultService = new HeaResultService();
            hicJepsuResultService = new HicJepsuResultService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLeft.Click += new EventHandler(eBtnClick);
            this.btnRight.Click += new EventHandler(eBtnClick);
            this.ssCal.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssCal)
            {
                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                cSpd.Spread_Clear_Simple(ssHol);

                int nRow = 0;
                string strORDERCODE = "";
                string strDate = VB.Pstr(ssCal.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), ">", 1);
                strDate = VB.Pstr(strDate, "<", 2);

                if (!strDate.IsNullOrEmpty())
                {
                    strDate = lblYear.Text + "-" + lblMonth.Text + "-" + VB.Format(strDate.To<int>(), "00");
                    lblCurDate.Text = strDate;
                    FstrCurDate = strDate;
                }

                List<string> strExCodes = new List<string> { "TX89", "TX84", "TX68", "TX44", "TZ16" };

                if (chkGB1.Checked && chkGB2.Checked && chkGB3.Checked)
                {
                    strExCodes.Add("TZ10");
                }

                if (!chkGB1.Checked && chkGB2.Checked && !chkGB3.Checked)
                {
                    strExCodes.Remove("TZ16");
                }

                if (!chkGB1.Checked && !chkGB2.Checked && chkGB3.Checked)
                {
                    strExCodes.Clear();
                    strExCodes.Add("TZ16");
                }

                if (chkGB1.Checked && !chkGB2.Checked && chkGB3.Checked)
                {
                    strExCodes.Clear();
                    strExCodes.Add("TZ16");
                }

                //종합건진
                List<HEA_RESV_EXAM> list1 = heaResvExamService.GetListByRTimeExCodeIN(strDate, strExCodes);
                if (list1.Count > 0)
                {
                    for (int j = 0; j < list1.Count; j++)
                    {
                        nRow += 1;
                        if (ssHol.ActiveSheet.RowCount < nRow) { ssHol.ActiveSheet.RowCount = nRow; }

                        ssHol.ActiveSheet.Cells[nRow - 1, 0].Text = list1[j].RTIME;
                        ssHol.ActiveSheet.Cells[nRow - 1, 1].Text = list1[j].SNAME;
                        ssHol.ActiveSheet.Cells[nRow - 1, 2].Text = list1[j].PTNO;
                        ssHol.ActiveSheet.Cells[nRow - 1, 3].Text = list1[j].EXAMNAME;

                        switch (list1[j].EXCODE)
                        {
                            case "TX84": strORDERCODE = "US22"; break;
                            case "TX44": strORDERCODE = "E6545"; break;
                            case "TX89": strORDERCODE = "E6543"; break;
                            case "TX68": strORDERCODE = "US-CADU1"; break;
                            case "TZ16": strORDERCODE = "USTCD"; break;
                            default: break;
                        }

                        //종검번호 찾기 (기본인적사항) ... 나이,성별,수검일자
                        string strACT = heaResultService.GetActiveByWrtnoExCode(list1[j].WRTNO, list1[j].EXCODE).To<string>("");

                        ssHol.ActiveSheet.Cells[nRow - 1, 4].Text = strACT == "Y" ? "◎" : "";
                        ssHol.ActiveSheet.Cells[nRow - 1, 5].Text = strORDERCODE;
                        ssHol.ActiveSheet.Cells[nRow - 1, 6].Text = list1[j].BDATE;
                    }
                }

                //일반건진
                List<HIC_JEPSU_RESULT> list2 = hicJepsuResultService.GetListJepDateExCodeIN(strDate, strExCodes);
                if (list2.Count > 0)
                {
                    for (int j = 0; j < list2.Count; j++)
                    {
                        nRow += 1;
                        if (ssHol.ActiveSheet.RowCount < nRow) { ssHol.ActiveSheet.RowCount = nRow; }

                        ssHol.ActiveSheet.Cells[nRow - 1, 0].Text = "HR";
                        ssHol.ActiveSheet.Cells[nRow - 1, 1].Text = list2[j].SNAME;
                        ssHol.ActiveSheet.Cells[nRow - 1, 2].Text = list2[j].PTNO;
                        ssHol.ActiveSheet.Cells[nRow - 1, 3].Text = list2[j].EXAMNAME;

                        switch (list2[j].EXCODE)
                        {
                            case "TX84": strORDERCODE = "US22"; break;
                            case "TX44": strORDERCODE = "E6545"; break;
                            case "TX89": strORDERCODE = "E6543"; break;
                            case "TX68": strORDERCODE = "US-CADU1"; break;
                            case "TZ16": strORDERCODE = "USTCD"; break;
                            default: break;
                        }

                        ssHol.ActiveSheet.Cells[nRow - 1, 5].Text = strORDERCODE;
                        ssHol.ActiveSheet.Cells[nRow - 1, 6].Text = list2[j].JEPDATE;
                    }
                }

                panList.Visible = true;
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnClose)
            {
                panList.Visible = false;
            }
            else if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnLeft)
            {
                Screen_Display("btnLeft");
            }
            else if (sender == btnRight)
            {
                Screen_Display("btnRight");
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            panList.Visible = false;
            Screen_Display_CurrentDay();
        }

        private void Screen_Display_CurrentDay()
        {
            int nLastDD = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int nCurrDD = DateTime.Now.Day;

            string strCurDate  = DateTime.Now.ToShortDateString();
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
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);        //기본달력 그리기 
            Data_Search(strGetSDate, strGetLDate, nStartCol);     //Data 조회
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
            string strHoliDay = string.Empty;

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

                strCurMonth = lblYear.Text + "-" + VB.Mid(FstrGetSDate, 6, 2) + "-" + VB.Format(i, "00");

                strHoliDay = comHpcLibBService.GetHoliDayByCurrentDay(strCurMonth);

                if (strHoliDay == "*")
                {
                    ssCal.ActiveSheet.Cells[nRow, nCol].ForeColor = Color.DarkRed;
                    ssCal.ActiveSheet.Cells[nRow, nCol].BackColor = Color.MistyRose;
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

        /// <summary>
        /// 달력에 예약인원 표시
        /// </summary>
        /// <param name="strGetSDate"></param>
        /// <param name="strGetLDate"></param>
        /// <param name="nStartCol"></param>
        private void Data_Search(string strGetSDate, string strGetLDate, int nStartCol)
        {
            int nRow = 0;
            int nCol = nStartCol;

            int nDay = 0;
            string strDate = string.Empty;
            
            string strHoliDay = string.Empty;
            string strLastDay = string.Empty;
            string strACT = string.Empty;

            List<string> strExCodes = new List<string>
            {
                "TX89",
                "TX84",
                "TX68",
                "TX44",
                "TZ16"
            };

            if (chkGB1.Checked && chkGB2.Checked && chkGB3.Checked)
            {
                strExCodes.Add("TZ10");
            }

            if (!chkGB1.Checked && chkGB2.Checked && !chkGB3.Checked)
            {
                strExCodes.Remove("TZ16");
            }

            if (!chkGB1.Checked && !chkGB2.Checked && chkGB3.Checked)
            {
                strExCodes.Clear();
                strExCodes.Add("TZ16");
            }

            if (chkGB1.Checked && !chkGB2.Checked && chkGB3.Checked)
            {
                strExCodes.Clear();
                strExCodes.Add("TZ16");
            }

            for (int i = 1; i < 32; i++)
            {
                if (nCol > 6)
                {
                    nRow += 1;
                    nCol = 0;
                    if (nRow > 4) { nRow = 0; }
                }

                nDay = VB.Replace(VB.Replace(ssCal.ActiveSheet.Cells[nRow, nCol].Text, "<", ""), ">", "").To<int>(0);

                if (nDay > 0)
                {
                    strDate = lblYear.Text + "-" + lblMonth.Text + "-" + VB.Format(nDay, "00");

                    ssCal.ActiveSheet.Cells[nRow, nCol].Text += ComNum.VBLF;

                    //종합건진
                    List<HEA_RESV_EXAM> list1 = heaResvExamService.GetListByRTimeExCodeIN(strDate, strExCodes);
                    if (list1.Count > 0)
                    {
                        for (int j = 0; j < list1.Count; j++)
                        {
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += "(" + list1[j].RTIME + ")";

                            //종검번호 찾기 (기본인적사항) ... 나이,성별,수검일자
                            strACT = heaResultService.GetActiveByWrtnoExCode(list1[j].WRTNO, list1[j].EXCODE).To<string>("");

                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += strACT == "Y" ? "○" : "Ｘ" + "  ";

                            if (chkGB3.Checked && !chkGB1.Checked && !chkGB2.Checked)
                            {
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list1[j].PTNO + " ";
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list1[j].SNAME + ComNum.VBLF;
                            }
                            else
                            {
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list1[j].SNAME + " ";
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list1[j].EXAMNAME + ComNum.VBLF;
                            }
                        }
                    }

                    //일반건진
                    List<HIC_JEPSU_RESULT> list2 = hicJepsuResultService.GetListJepDateExCodeIN(strDate, strExCodes);
                    if (list2.Count > 0)
                    {
                        for (int j = 0; j < list2.Count; j++)
                        {
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += "(HR)  ";

                            if (chkGB3.Checked && !chkGB1.Checked && !chkGB2.Checked)
                            {
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list2[j].PTNO + " ";
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list2[j].SNAME + ComNum.VBLF;
                            }
                            else
                            {
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list2[j].SNAME + " ";
                                ssCal.ActiveSheet.Cells[nRow, nCol].Text += list2[j].EXAMNAME + ComNum.VBLF;
                            }
                        }
                    }

                    //심전도만 선택했을 떄, EKG프로그램에서
                    List<COMHPC> list3 = comHpcLibBService.GetBasPatPoscoListByExamres15(strDate);

                    if (list3.Count > 0)
                    {
                        for (int j = 0; j < list3.Count; j++)
                        {
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += "(" + list3[j].RDATE + ") ";
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += list3[j].PTNO + " ";
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += list3[j].SNAME + "(P)" + ComNum.VBLF;
                        }
                    }
                }

                Row row;
                row = ssCal.ActiveSheet.Rows[nRow];
                float rowSize = row.GetPreferredHeight();
                if (rowSize < 100) { rowSize = 100; }
                row.Height = rowSize;

                nCol += 1;
            }
        }

        /// <summary>
        /// 달력 예약정보 표시 Main
        /// </summary>
        /// <param name="argBtn"></param>
        private void Screen_Display(string argBtn = "")
        {
            string strGetSDate = string.Empty;

            if (argBtn == "btnLeft")
            {
                strGetSDate = CF.DATE_ADD(clsDB.DbCon, FstrGetSDate, -1);
                strGetSDate = VB.Mid(strGetSDate, 1, 8) + "01";
            }
            else if (argBtn == "btnRight")
            {
                strGetSDate = CF.DATE_ADD(clsDB.DbCon, FstrGetLDate, 1);
            }
            else
            {
                strGetSDate = VB.Left(FstrGetLDate, 8) + "01";
            }

            Cursor.Current = Cursors.WaitCursor;

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
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);         //기본달력 그리기

            Data_Search(strGetSDate, strGetLDate, nStartCol);     //Data 조회

            Cursor.Current = Cursors.Default;
        }
    }
}
