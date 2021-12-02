using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaResvCalendar.cs
/// Description     : 2차 대상자 명단 조회
/// Author          : 김민철
/// Create Date     : 2020-06-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm종합검진예약달력(Frm종합검진예약달력.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaResvCalendar : Form
    {
        public delegate void rSendMsg(string strPano, string strSDate);
        public static event rSendMsg rSndMsg;

        public delegate void SetExInwonView(object sender, EventArgs e);
        public static event SetExInwonView rSetExInwonView;

        int FnSign = 0;
        int FnCheckCol = 0;
        int FnCheckRow = 0;
        int FnCol = -1;
        int FnRow = -1;
        int FnLastDD = 0;
        int FnCurrDD = 0;
        int FnSS2_ColCnt = 0;
        string FstrCurDate  = string.Empty;   //현재일자
        string FstrGetSDate = string.Empty;   //시작일자
        string FstrGetLDate = string.Empty;   //종료일자
        string FstrSS2ExList = string.Empty;    //SS2 헤더설정용
        string FstrGbExam = string.Empty;

        bool bolSort = false;

        ComFunc CF = null;
        clsSpread cSpd = null;
        clsHcFunc cHF = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaJepsuService heaJepsuService = null;
        HeaResvSetService heaResvSetService = null;
        HeaResvLtdService heaResvLtdService = null;
        HeaCodeService heaCodeService = null;
        HeaResvExamService heaResvExamService = null;
        HicPatientService hicPatientService = null;
        HeaJepsuResultService heaJepsuResultService = null;
        HeaResultService heaResultService = null;

        UnaryComparisonConditionalFormattingRule unary;
        Color bColor;
        ToolTip toolTip = null;
        Thread thread = null;

        public frmHaResvCalendar()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load                    += new EventHandler(eFormLoad);
            this.Activated               += new EventHandler(eFormActivated);
            this.btnExit.Click           += new EventHandler(eBtnClick);
            this.btnLeft.Click           += new EventHandler(eBtnClick);
            this.btnRight.Click          += new EventHandler(eBtnClick);
            this.btnSave.Click           += new EventHandler(eBtnClick);
            this.btnSet.Click            += new EventHandler(eBtnClick);
            this.btnExamInwon.Click      += new EventHandler(eHcExamInwonFormView);
            this.btnResvLtd.Click        += new EventHandler(eBtnClick);

            this.ssCal.CellDoubleClick   += new CellClickEventHandler(eSpdDblClick);
            this.SS_Exam.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
            this.SS2.CellClick           += new CellClickEventHandler(eSpdCellClick);
            this.SS3.CellClick           += new CellClickEventHandler(eSpdCellClick);
            this.SS4.CellClick           += new CellClickEventHandler(eSpdCellClick);
            this.SS5.CellClick           += new CellClickEventHandler(eSpdCellClick);
            this.SS2.CellDoubleClick     += new CellClickEventHandler(eSpdDblClick);
            this.SS3.CellDoubleClick     += new CellClickEventHandler(eSpdDblClick);
        }

        private void eFormActivated(object sender, EventArgs e)
        {
            //if (!bShow) { Refresh_Form_Display(); }
            //bShow = false;
        }

        private void eHcExamInwonFormView(object sender, EventArgs e)
        {
            rSetExInwonView(sender, e);
        }

        private void eSpdCellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow((FpSpread)sender, e.Column, ref bolSort, true);
            }
            else
            {
                if (sender == SS2)
                {
                    if (e.RowHeader || e.Column < (FnSS2_ColCnt - 1))
                    {
                        return;
                    }
                    else
                    {
                        //수검일자와 검사일자가 다른경우 툴팁으로 안내
                        long nPano = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim().To<long>(0);
                        string strSDate = SS2.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                        string strGubn1 = SS2.ActiveSheet.Columns.Get(e.Column).Label.Trim();
                        string strMarking = SS2.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();

                        if (!strMarking.IsNullOrEmpty())
                        {
                            string strRTime = heaResvExamService.GetRTimeByPanoSDateGubun1(nPano, strSDate, strGubn1);

                            if (strRTime.IsNullOrEmpty())
                            {
                                strRTime = "예약일정 없음";
                            }

                            toolTip.SetToolTip((Control)sender, strRTime);
                        }
                    }
                }
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            long nTotal = 0;
            long nCount_M = 0;
            long nCount_F = 0;

            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == ssCal)
            {
                Cursor.Current = Cursors.WaitCursor;

                cSpd.Spread_Clear_Simple(SS_Exam);
                cSpd.Spread_Clear_Simple(SS2);
                SS_Etc_Spread_Clear();

                if (FnRow > -1 && FnCol > -1)
                {
                    ssCal.ActiveSheet.Cells[FnRow, FnCol].BackColor = bColor;
                }

                string strDate = VB.Pstr(ssCal.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), ">", 1);
                strDate = VB.Pstr(strDate, "<", 2);

                if (!strDate.IsNullOrEmpty())
                {
                    strDate = lblYear.Text + "-" + lblMonth.Text + "-" + VB.Format(strDate.To<int>(), "00");
                    lblCurDate.Text = strDate;
                    FstrCurDate = strDate;
                }

                FnRow = e.Row;
                FnCol = e.Column;
                bColor = ssCal.ActiveSheet.Cells[e.Row, e.Column].BackColor;

                List<HEA_JEPSU> list = heaJepsuService.GetCountSexBySDate(strDate);

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].SEX == "M")
                        {
                            nCount_M = list[i].WCNT;
                        }
                        else
                        {
                            nCount_F = list[i].WCNT;
                        }
                    }

                    nTotal = nCount_F + nCount_M;

                    lblCount.Text = " 총원: " + nTotal.To<string>() + "명     ";
                    lblCount.Text += "M: " + nCount_M.To<string>() + "명   ";
                    lblCount.Text += "F: " + nCount_F.To<string>() + "명";

                    //2021-10-21
                    txtRemark.Text = comHpcLibBService.GetHeaSRevRemarkBySDate(FstrCurDate);

                    Daily_Exam_Count(FstrCurDate);  //선택검사 인원 현황
                    Insert_Exam_Data(FstrCurDate);  //해당일자 예약 및 수검자 명단

                    //2021-10-21
                    //Display_Endo_Resv(FstrCurDate); //해당일자 내시경 명단 Remark Display
                }
                else
                {

                    lblCount.Text = " 총원: 0명     ";
                    lblCount.Text += "M: 0명   ";
                    lblCount.Text += "F: 0명";

                    //txtRemark.Text = "";
                    //2021-10-21
                    txtRemark.Text = comHpcLibBService.GetHeaSRevRemarkBySDate(FstrCurDate);

                    Daily_Exam_Count(FstrCurDate);  //선택검사 인원 현황
                    Insert_Exam_Data(FstrCurDate);  //해당일자 예약 및 수검자 명단

                    //2021-10-21
                    //Display_Endo_Resv(FstrCurDate); //해당일자 내시경 명단 Remark Display
                }

                Cursor.Current = Cursors.Default;
            }
            else if (sender == SS_Exam)
            {
                Cursor.Current = Cursors.WaitCursor;

                FstrGbExam = SS_Exam.ActiveSheet.Cells[e.Row, 0].Text.Trim().PadLeft(2, '0');
                lblExamList.Text = SS_Exam.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                Display_Exam_Reserved();

                Cursor.Current = Cursors.Default;
            }
            else if (sender == SS2)
            {
                string strPano = SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                string strSDate = VB.Left(SS2.ActiveSheet.Cells[e.Row, 4].Text.Trim(), 10);

                rSndMsg(strPano, strSDate);

                this.Hide();
                return;
            }
            else if (sender == SS3)
            {
                string strName = SS3.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                string strTime = SS3.ActiveSheet.Cells[e.Row, 1].Text.Trim();

                txtRemark.Text += ComNum.VBLF;
                txtRemark.Text += strName + "(" + strTime + ")";
            }
        }

        private void Display_Exam_Reserved()
        {
            int nRow = 0;

            SS_Etc_Spread_Clear();

            if (FstrGbExam == "00")
            {
                //종검예약자 명단 조회
                SS3.DataSource = heaJepsuService.GetListSNameSTimeBySDate(FstrCurDate);
            }
            else
            {
                //선택검사 예약자 명단 조회
                List<HEA_RESV_EXAM> lst = heaResvExamService.GetItembyRTime(FstrCurDate, CF.DATE_ADD(clsDB.DbCon, FstrCurDate, 1), FstrGbExam);
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        nRow += 1;
                        if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                        SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst[i].SNAME;
                        SS3.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].RTIME;
                        SS3.ActiveSheet.Cells[nRow - 1, 2].Text = Convert.ToDateTime(lst[i].SDATE).ToShortDateString();
                        if (Convert.ToDateTime(lst[i].SDATE).ToShortDateString() != FstrCurDate)
                        {
                            SS3.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.Pink;
                        }
                        else
                        {
                            SS3.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.White;
                        }
                    }
                }
            }

            //회사 가예약 사항 Display
            SS5.DataSource = heaResvLtdService.GetListAmPmJanBySDateGubun(FstrCurDate, FstrGbExam, 0);

            //가예약 사항 Display
            SS4.DataSource = heaResvSetService.GetListGaInwonBySDate(FstrCurDate, FstrGbExam, 0);
            SS4.AddRows(5);

        }

        private void SetControl()
        {
            bColor = new Color();
            CF = new ComFunc();
            cSpd = new clsSpread();
            cHF = new clsHcFunc();

            toolTip = new ToolTip();

            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            comHpcLibBService = new ComHpcLibBService();
            heaJepsuService = new HeaJepsuService();
            heaResvSetService = new HeaResvSetService();
            heaResvLtdService = new HeaResvLtdService();
            heaCodeService = new HeaCodeService();
            heaResvExamService = new HeaResvExamService();
            hicPatientService = new HicPatientService();
            heaJepsuResultService = new HeaJepsuResultService();
            heaResultService = new HeaResultService();

            SS_Exam.ActiveSheet.SetRowHeight(-1, 22);
            
            SS2.Initialize(new SpreadOption { RowHeight = 22, RowHeaderVisible = true });
            SS2.AddColumn("종검번호",   "", 64, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("등록번호",   "", 68, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("수검자명",   "", 68, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("AMPM",       "", 30, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("수검일자",   "", 78, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn("검사일자",   "", 78, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("수면",       "", 38, new SpreadCellTypeOption { IsEditble = false });

            FnSS2_ColCnt = 7;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.FromArgb(255, 255, 221);
            SS2.ActiveSheet.SetConditionalFormatting(-1, 1, unary);

            //예약명단
            SS3.Initialize(new SpreadOption { RowHeight = 22, ColumnHeaderHeight = 38, RowHeaderVisible = true });
            SS3.AddColumn("예약자명", nameof(HEA_RESV_EXAM.SNAME), 70, new SpreadCellTypeOption { IsEditble = false });
            SS3.AddColumn("예약시간", nameof(HEA_RESV_EXAM.RTIME), 44, new SpreadCellTypeOption { IsEditble = false });
            SS3.AddColumn("수검일자", nameof(HEA_RESV_EXAM.SDATE), 78, new SpreadCellTypeOption { IsEditble = false });

            //회사가예약
            SS5.Initialize(new SpreadOption { RowHeight = 22, RowHeaderVisible = false, ColumnHeaderHeight = 38 });
            SS5.AddColumn("회사명", nameof(HEA_RESV_LTD.LTDNAME),  82, new SpreadCellTypeOption { IsEditble = false });
            SS5.AddColumn("오전",   nameof(HEA_RESV_LTD.AMJAN),    38, new SpreadCellTypeOption { IsEditble = false });
            SS5.AddColumn("오후",   nameof(HEA_RESV_LTD.PMJAN),    38, new SpreadCellTypeOption { IsEditble = false });
            SS5.AddColumn("담당자", nameof(HEA_RESV_LTD.JOBNAME),  72, new SpreadCellTypeOption { IsEditble = false });
            SS5.AddColumn("사번",   nameof(HEA_RESV_LTD.ENTSABUN), 92, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            
            //개인가예약
            SS4.Initialize(new SpreadOption { RowHeight = 22, ColumnHeaderHeight = 38 });
            SS4.AddColumnCheckBox("삭제", "", 30, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += frmHaResvCalendar_ButtonClick;
            SS4.AddColumn("예약자명", nameof(HEA_RESV_SET.EXAMNAME),  84, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS4.AddColumn("오전",     nameof(HEA_RESV_SET.GAINWONAM), 30, new SpreadCellTypeOption { });
            SS4.AddColumn("오후",     nameof(HEA_RESV_SET.GAINWONPM), 30, new SpreadCellTypeOption { });
            SS4.AddColumn("사번",     nameof(HEA_RESV_SET.ENTSABUN),  92, new SpreadCellTypeOption { IsVisivle = false });
            SS4.AddColumn("담당자",   nameof(HEA_RESV_SET.JOBNAME),   70, new SpreadCellTypeOption { IsEditble = false });
            SS4.AddColumn("ROWID",    nameof(HEA_RESV_SET.RID),       92, new SpreadCellTypeOption { IsVisivle = false });
            
        }

        private void frmHaResvCalendar_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            HEA_RESV_SET code = SS4.GetRowData(e.Row) as HEA_RESV_SET;
            SS4.DeleteRow(e.Row);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Hide();
                return;
            }
            else if (sender == btnLeft)
            {
                Screen_Display(btnLeft);
            }
            else if (sender == btnRight)
            {
                Screen_Display(btnRight);
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnSet)
            {
                frmHaSetExam_Count frm = new frmHaSetExam_Count();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
            else if (sender == btnResvLtd)
            {
                frmHaLtdGaResv frm = new frmHaLtdGaResv();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
            }
        }

        private void Data_Save()
        {
            int result = 0;
            string strRowid = comHpcLibBService.GetRowidBySRev(FstrCurDate);

            if (!strRowid.IsNullOrEmpty())
            {
                result = comHpcLibBService.UpDateSRev(strRowid, txtRemark.Text.Trim());
            }
            else
            {
                result = comHpcLibBService.InsertSRev(FstrCurDate, txtRemark.Text.Trim());
            }

            if (result <= 0)
            {
                MessageBox.Show("오류발생", "오류");
            }

            if (FstrGbExam == "") { return; }

            //가예약인원 업데이트
            IList<HEA_RESV_SET> list = SS4.GetEditbleData<HEA_RESV_SET>();

            if (list.Count > 0)
            {
                if (heaResvSetService.Save(list, FstrCurDate, FstrGbExam))
                {
                    MessageBox.Show("저장하였습니다");
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다. ");
                }
            }

            //현재상태 Screen_Display 
            cSpd.Spread_Clear_Simple(SS4);
            SS4.DataSource = heaResvSetService.GetListGaInwonBySDate(FstrCurDate, FstrGbExam, 0);
            SS4.AddRows(5);

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Display_CurrentDay();
        }

        private void Refresh_Form_Display()
        {
            try
            {
                //스레드 시작
                thread = new Thread(tProcess);
                thread.Start();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
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

            lblCurDate.Text = FstrCurDate;

            SS_Calendar_Clear();
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);        //기본달력 그리기
            Data_Search(strGetSDate, strGetLDate, nStartCol);     //Data 조회

            cSpd.Spread_Clear_Simple(SS_Exam);
            cSpd.Spread_Clear_Simple(SS2);
            SS_Etc_Spread_Clear();


            Daily_Exam_Count(FstrCurDate);
            Insert_Exam_Data(FstrCurDate);

            txtRemark.Text = comHpcLibBService.GetHeaSRevBySDate(FstrCurDate);

            //2021-10-21
            //Display_Endo_Resv(FstrCurDate);
            Display_Calender_Color(nCurrDD, true);
        }

        private void SS_Etc_Spread_Clear()
        {
            cSpd.Spread_Clear_Simple(SS3);
            cSpd.Spread_Clear_Simple(SS4);
            cSpd.Spread_Clear_Simple(SS5);
        }

        /// <summary>
        /// 달력 예약정보 표시 Main
        /// </summary>
        /// <param name="argBtn"></param>
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
            SS_Etc_Spread_Clear();
            SS_Calendar_Show(nLastDD, nStartCol, nCurrDD);         //기본달력 그리기
            Data_Search(strGetSDate, strGetLDate,  nStartCol);     //Data 조회
            Display_Calender_Color(nCurrDD, false);

            Cursor.Current = Cursors.Default;
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
            int GetDD = 0;
            long[] nGetMCount_AM = new long[32];
            long[] nGetMCount_PM = new long[32];
            long[] nGetFCount_AM = new long[32];
            long[] nGetFCount_PM = new long[32];

            string strSDate = string.Empty;
            string strHoliDay = string.Empty;
            string strLastDay = string.Empty;

            //일자별 예약인원 누적
            IList<HEA_JEPSU> list = heaJepsuService.GetCountSDateSexAmPm2BySDate(strGetSDate, strGetLDate);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GetDD = VB.Right(list[i].SDATE, 2).To<int>();

                    if (list[i].AMPM2 == "1")
                    {
                        if (list[i].SEX == "M")
                        {
                            nGetMCount_AM[GetDD] = list[i].WCNT;
                        }
                        else
                        { 
                            nGetFCount_AM[GetDD] = list[i].WCNT;
                        } 
                    }
                    else
                    {
                        if (list[i].SEX == "M")
                        {
                            nGetMCount_PM[GetDD] = list[i].WCNT;
                        }
                        else
                        {
                            nGetFCount_PM[GetDD] = list[i].WCNT;
                        }
                    }
                }
            }

            //가예약 사항 Display
            IList<HEA_RESV_SET> list2 = heaResvSetService.GetSumGaInwonSDateBySDateGbResv(strGetSDate, strGetLDate, "00");

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    GetDD = VB.Right(list2[i].SDATE, 2).To<int>();

                    if (list[i].SEX == "M")
                    {
                        nGetMCount_AM[GetDD] += list2[i].GAINWONAM;
                        nGetMCount_PM[GetDD] += list2[i].GAINWONPM;
                    }
                    else
                    {
                        nGetFCount_AM[GetDD] += list2[i].GAINWONAM;
                        nGetFCount_PM[GetDD] += list2[i].GAINWONPM;
                    }
                }
            }

            for (int i = 1; i < 32; i++)
            {
                if (nCol > 6)
                {
                    nRow += 1;
                    nCol = 0;
                    if (nRow > 4) { nRow = 0; }
                }


                if (nGetMCount_AM[i] > 0 || nGetMCount_PM[i] > 0 || nGetFCount_AM[i] > 0 || nGetFCount_PM[i] > 0)
                {
                    //회사별 가예약 잔여인원을 합산함

                    strSDate = VB.Left(strGetSDate, 8) + VB.Format(i, "00");
                    HEA_RESV_LTD item = heaResvLtdService.GetSumAmPmJanByGubun(strSDate, "00");

                    if (!item.IsNullOrEmpty())
                    {
                        ssCal.ActiveSheet.Cells[nRow, nCol].Text += VB.Space(1);
                        ssCal.ActiveSheet.Cells[nRow, nCol].Text += (nGetMCount_AM[i] + nGetFCount_AM[i] + item.AMJAN).To<string>() + "/";
                        ssCal.ActiveSheet.Cells[nRow, nCol].Text += (nGetMCount_PM[i] + nGetFCount_PM[i] + item.PMJAN).To<string>() + "명" + ComNum.VBLF + ComNum.VBLF;
                        ssCal.ActiveSheet.Cells[nRow, nCol].Text += "AM: M" + VB.Format(nGetMCount_AM[i], "00") + " F" + VB.Format(nGetFCount_AM[i], "00") + ComNum.VBLF;
                        ssCal.ActiveSheet.Cells[nRow, nCol].Text += "PM: M" + VB.Format(nGetMCount_PM[i], "00") + " F" + VB.Format(nGetFCount_PM[i], "00") + ComNum.VBLF;
                    }
                }
                else
                {
                    strLastDay = clsVbfunc.LastDay(int.Parse(VB.Left(strGetSDate, 4)), int.Parse(VB.Mid(strGetSDate, 6, 2))); //월의 마지막날
                    if (Convert.ToInt32(VB.Right(strLastDay,2)) > i)
                    {
                        strHoliDay = "";
                        strHoliDay = comHpcLibBService.GetHoliDayByCurrentDay(VB.Left(strGetSDate, 8) + VB.Format(i, "00"));

                        if (strHoliDay != "*" && nCol != 0)
                        {
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += VB.Space(1);
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += "0/0명" + ComNum.VBLF + ComNum.VBLF;
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += "AM: M" + "00" + " F" + "00" + ComNum.VBLF;
                            ssCal.ActiveSheet.Cells[nRow, nCol].Text += "PM: M" + "00" + " F" + "00" + ComNum.VBLF;
                        }
                    }
                }

                nCol += 1;
            }
        }

        private void Daily_Exam_Count(string fstrCurDate)
        {
            string strGubun = string.Empty;
            int nColSize = 34;

            FstrSS2ExList = "";

            strGubun = "00";
            SS_Exam.ActiveSheet.RowCount = 5;
            SS_Exam.ActiveSheet.Cells[0, 0].Text = "00";
            SS_Exam.ActiveSheet.Cells[0, 1].Text = " 종합검진";

            Display_SSExam(0, fstrCurDate, strGubun);

            IList<HEA_CODE> list = heaCodeService.GetItemByGubunGroupBy("13", null, "CAL");

            if (list.Count > 0)
            {
                SS_Exam.ActiveSheet.RowCount = list.Count + 1;

                for (int i = 0; i < list.Count; i++)
                {
                    strGubun = list[i].GUBUN2.Trim();
                    if (SS2.ActiveSheet.ColumnCount < (i + FnSS2_ColCnt + 1))
                    {
                        if (list[i].GUBUN1.Length > 3)
                        {
                            nColSize = 40;
                        }
                        else
                        {
                            nColSize = 34;
                        }

                        SS2.AddColumn(list[i].GUBUN1, "", nColSize, new SpreadCellTypeOption { IsEditble = false });  
                    }

                    FstrSS2ExList += "'" + list[i].GUBUN1 + "',";

                    SS_Exam.ActiveSheet.Cells[i + 1, 0].Text = strGubun;
                    SS_Exam.ActiveSheet.Cells[i + 1, 1].Text = " " + list[i].NAME;

                    Display_SSExam(i + 1, fstrCurDate, strGubun);
                }

                if (VB.Right(FstrSS2ExList, 1) == ",")
                {
                    FstrSS2ExList = FstrSS2ExList.Substring(0, FstrSS2ExList.Length - 1);
                }

            }
        }

        private void Display_SSExam(int argRow, string argCurDate, string argGubun)
        {
            long nTo_Am = 0;
            long nTo_Pm = 0;
            long nGa_Am = 0;
            long nGa_Pm = 0;
            long nCnt_Am = 0;
            long nCnt_Pm = 0;

            HEA_RESV_SET item1 = heaResvSetService.GetSumInwonAMPMByGubun(argCurDate, argGubun);

            if (!item1.IsNullOrEmpty())
            {
                nTo_Am = item1.AMINWON;
                nTo_Pm = item1.PMINWON;
            }

            //개인별 가예약
            HEA_RESV_SET item2 =heaResvSetService.GetSumGaInwonAMPMByGubun(argCurDate, argGubun);

            if (!item2.IsNullOrEmpty())
            {
                nGa_Am += item2.GAINWONAM;
                nGa_Pm += item2.GAINWONPM;
            }

            //회사별 가예약
            HEA_RESV_LTD item3 = heaResvLtdService.GetSumAmPmJanByGubun(argCurDate, argGubun);

            if (!item3.IsNullOrEmpty())
            {
                nGa_Am += item3.AMJAN;
                nGa_Pm += item3.PMJAN;
            }

            if (argGubun == "00")
            {
                List<HEA_JEPSU> lst1 = heaJepsuService.GetCountAmPm2BySDate(argCurDate);
                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++)
                    {
                        if (lst1[i].AMPM2 == "1")
                        {
                            nCnt_Am += lst1[i].WCNT;
                        }
                        else if (lst1[i].AMPM2 == "2")
                        {
                            nCnt_Pm += lst1[i].WCNT;
                        }
                    }
                }
            }
            else
            {
                List<HEA_RESV_EXAM> lst1 = heaResvExamService.GetCNTAMPMbyRTime(argCurDate, CF.DATE_ADD(clsDB.DbCon, argCurDate, 1), argGubun);
                if (lst1.Count > 0)
                {
                    for (int i = 0; i < lst1.Count; i++)
                    {
                        if (lst1[i].AMPM == "A")
                        {
                            nCnt_Am += lst1[i].CNT;
                        }
                        else if (lst1[i].AMPM == "P")
                        {
                            nCnt_Pm += lst1[i].CNT;
                        }
                    }
                }
            }

            SS_Exam.ActiveSheet.Cells[argRow, 2].Text = nTo_Am.To<string>() + "/" + (nGa_Am + nCnt_Am).To<string>();
            SS_Exam.ActiveSheet.Cells[argRow, 3].Text = nTo_Pm.To<string>() + "/" + (nGa_Pm + nCnt_Pm).To<string>();
            SS_Exam.ActiveSheet.Cells[argRow, 4].Text = nGa_Am.To<string>();
            SS_Exam.ActiveSheet.Cells[argRow, 5].Text = nGa_Pm.To<string>();
            SS_Exam.ActiveSheet.Cells[argRow, 6].Text = (nTo_Am + nTo_Pm).To<string>() + "/" + (nGa_Am + nGa_Pm + nCnt_Am + nCnt_Pm).To<string>();

            //정원오전
            if (nTo_Am == (nGa_Am + nCnt_Am))
            {
                SS_Exam.ActiveSheet.Cells[argRow, 2].BackColor = Color.LemonChiffon;
            }
            else if (nTo_Am < (nGa_Am + nCnt_Am))
            {
                SS_Exam.ActiveSheet.Cells[argRow, 2].BackColor = Color.MistyRose;
            }
            else
            {
                SS_Exam.ActiveSheet.Cells[argRow, 2].BackColor = Color.White;
            }

            //정원오후
            if (nTo_Pm == (nGa_Pm + nCnt_Pm))
            {
                SS_Exam.ActiveSheet.Cells[argRow, 3].BackColor = Color.LemonChiffon;
            }
            else if (nTo_Pm < (nGa_Pm + nCnt_Pm))
            {
                SS_Exam.ActiveSheet.Cells[argRow, 3].BackColor = Color.MistyRose;
            }
            else
            {
                SS_Exam.ActiveSheet.Cells[argRow, 3].BackColor = Color.White;
            }

            //총원
            if ((nTo_Am + nTo_Pm) == (nGa_Am + nGa_Pm + nCnt_Am + nCnt_Pm))
            {
                SS_Exam.ActiveSheet.Cells[argRow, 6].BackColor = Color.LemonChiffon;
            }
            else if ((nTo_Am + nTo_Pm) < (nGa_Am + nGa_Pm + nCnt_Am + nCnt_Pm))
            {
                SS_Exam.ActiveSheet.Cells[argRow, 6].BackColor = Color.MistyRose;
            }
            else
            {
                SS_Exam.ActiveSheet.Cells[argRow, 6].BackColor = Color.White;
            }
        }

        /// <summary>
        /// 해당일자 예약 및 수검자 명단 (PIVOT 으로 튜닝)
        /// </summary>
        /// <param name="fstrCurDate"></param>
        private void Insert_Exam_Data(string argCurDate)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            int nRow = 0;
            string strPtno = "";
            string strFDate = argCurDate;
            string strTDate = CF.DATE_ADD(clsDB.DbCon, argCurDate, 1);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT *  FROM  (  ";
            SQL += ComNum.VBLF + "           SELECT a.PANO, a.PTNO, a.SNAME, d.GUBUN1, DECODE(a.AMPM2, '1', 'A', 'P') AS AMPM, TO_CHAR(a.SDATE, 'YYYY-MM-DD') SDATE     ";
            SQL += ComNum.VBLF + "                , TO_CHAR(c.RTIME, 'YYYY-MM-DD') RTIME                                                                                ";
            SQL += ComNum.VBLF + "                , DECODE(b.EXCODE, 'TX20', '수면', 'TX41', '수면', 'TX64', '수면', '') AS EXCODE                                      ";
            SQL += ComNum.VBLF + "             FROM KOSMOS_PMPA.HEA_JEPSU a,     KOSMOS_PMPA.HEA_RESULT b,                                                              ";
            SQL += ComNum.VBLF + "                  KOSMOS_PMPA.HEA_RESV_EXAM c, KOSMOS_PMPA.HEA_CODE d                                                                 ";
            SQL += ComNum.VBLF + "            WHERE 1 = 1                                                                                                               ";
            SQL += ComNum.VBLF + "              AND (a.SDATE= TO_DATE('" + strFDate + "', 'YYYY-MM-DD')                                                                 ";
            SQL += ComNum.VBLF + "                   OR(    c.RTIME >= TO_DATE('" + strFDate + "', 'YYYY-MM-DD')                                                        ";
            SQL += ComNum.VBLF + "                      AND c.RTIME <  TO_DATE('" + strTDate + "', 'YYYY-MM-DD')))                                                      ";
            SQL += ComNum.VBLF + "              AND a.DELDATE IS NULL                                                                                                   ";
            SQL += ComNum.VBLF + "              AND c.DELDATE IS NULL                                                                                                   ";
            SQL += ComNum.VBLF + "              AND d.GUBUN = '13'                                                                                                      ";
            SQL += ComNum.VBLF + "              AND a.WRTNO = b.WRTNO(+)                                                                                                ";
            SQL += ComNum.VBLF + "              AND a.PANO = c.PANO                                                                                                     ";
            SQL += ComNum.VBLF + "              AND a.SDATE = c.SDATE                                                                                                   ";
            SQL += ComNum.VBLF + "              AND b.EXCODE = c.EXCODE                                                                                                 ";
            SQL += ComNum.VBLF + "              AND b.EXCODE = d.CODE(+)                                                                                                ";
            SQL += ComNum.VBLF + "              AND b.EXCODE IN (SELECT CODE FROM HEA_CODE WHERE GUBUN = '13' GROUP BY CODE)                                            ";
            SQL += ComNum.VBLF + "              UNION                                                                                                                   ";
            SQL += ComNum.VBLF + "              SELECT PANO, PTNO, SNAME, '' AS GUBUN1, DECODE(AMPM2, '1', 'A', 'P') AS AMPM, TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE        ";
            SQL += ComNum.VBLF + "              , TO_CHAR(SDATE, 'YYYY-MM-DD') RTIME                , '' AS EXCODE                                                      ";
            SQL += ComNum.VBLF + "              FROM KOSMOS_PMPA.HEA_JEPSU                                                                                              ";
            SQL += ComNum.VBLF + "              WHERE 1 = 1                                                                                                             ";
            SQL += ComNum.VBLF + "              AND SDATE = TO_DATE('" + strFDate + "', 'YYYY-MM-DD')                                                                   ";
            SQL += ComNum.VBLF + "              AND DELDATE IS NULL                                                                                                     ";
            SQL += ComNum.VBLF + " )                                                                                                                                    ";
            SQL += ComNum.VBLF + " PIVOT                                                                                                                                ";
            SQL += ComNum.VBLF + " (                                                                                                                                    ";
            SQL += ComNum.VBLF + "     COUNT(GUBUN1)                                                                                                                    ";
            SQL += ComNum.VBLF + "     FOR GUBUN1 IN (" + FstrSS2ExList + ")                                                                                            ";
            SQL += ComNum.VBLF + " )   ";
            SQL += ComNum.VBLF + " ORDER BY SNAME, RTIME   ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (strPtno == dt.Rows[i]["PTNO"].ToString().Trim())
                    {
                        for (int j = 6; j < SS2.ActiveSheet.ColumnCount; j++)
                        {
                            if (dt.Rows[i][j].To<int>(0) >= 1)
                            {
                                //검사일자와 수검일자가 서로 다른 검사의 경우 표시
                                if (dt.Rows[i]["SDATE"].ToString().To<string>("").Trim() != dt.Rows[i]["RTIME"].ToString().To<string>("").Trim())
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, j].Text = "●";
                                }
                                else
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, j].Text = "◎";
                                }
                            }
                        }
                    }
                    else
                    {
                        strPtno = dt.Rows[i]["PTNO"].ToString().Trim();

                        nRow += 1;
                        if (SS2.ActiveSheet.RowCount < nRow)
                        {
                            SS2.ActiveSheet.RowCount = nRow;
                        }

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j > (FnSS2_ColCnt - 1))
                            {
                                if (argCurDate != dt.Rows[i]["SDATE"].ToString().To<string>("").Trim())
                                {
                                    SS2.ActiveSheet.Cells[nRow - 1, j].Text = dt.Rows[i][j].ToString().To<string>("0") == "0" ? "" : "◆";
                                }
                                else
                                {
                                    if (dt.Rows[i]["SDATE"].ToString().To<string>("").Trim() != dt.Rows[i]["RTIME"].ToString().To<string>("").Trim())
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, j].Text = dt.Rows[i][j].ToString().To<string>("0") == "0" ? "" : "●";
                                    }
                                    else
                                    {
                                        SS2.ActiveSheet.Cells[nRow - 1, j].Text = dt.Rows[i][j].ToString().To<string>("0") == "0" ? "" : "◎";
                                    }
                                }
                                
                            }
                            else
                            {
                                SS2.ActiveSheet.Cells[nRow - 1, j].Text = dt.Rows[i][j].ToString().To<string>("");
                            }
                        }

                    }

                }
            }

            dt.Dispose();
            dt = null;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "●", false);
            unary.BackColor = Color.PaleTurquoise;
            SS2.ActiveSheet.SetConditionalFormatting(-1, 6, -1, SS2.ActiveSheet.ColumnCount, unary);

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "◆", false);
            unary.BackColor = Color.Thistle;
            SS2.ActiveSheet.SetConditionalFormatting(-1, 6, -1, SS2.ActiveSheet.ColumnCount, unary);

        }

        /// <summary>
        /// 해당일자 내시경 명단 Remark Display
        /// </summary>
        /// <param name="fstrCurDate"></param>
        private void Display_Endo_Resv(string argCurDate)
        {
            string strOK = "";

            txtRemark.Text += ComNum.VBLF;

            List<HEA_JEPSU> list = heaJepsuService.GetListEndoByRTime(argCurDate);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "";
                    if (VB.L(txtRemark.Text, list[i].PTNO) < 2)
                    {
                        if (!heaResvExamService.GetRowidByPanoExcode(list[i].PANO, "", argCurDate).IsNullOrEmpty())
                        {
                            strOK = "OK";
                        }

                        txtRemark.Text += list[i].SNAME + "(";

                        if (strOK == "OK")
                        {
                            txtRemark.Text += "+,";
                        }
                        else
                        {
                            txtRemark.Text += "-,";
                        }

                        txtRemark.Text += list[i].PTNO.Trim() + ", " + list[i].RTIME.Trim() + ")" + ComNum.VBLF;
                    }
                }
            }
        }

        private void Display_Calender_Color(int nCurrDD, bool nCurrMon)
        {
            long nCNT = 0;
            long nTotal = 0;
            string strDay = string.Empty;
            string strDate = string.Empty;
            string strTemp1 = string.Empty;
            string strTemp2 = string.Empty;
            long AmCnt  = 0;
            long PmCnt  = 0;
            long GAmCnt = 0;
            long GPmCnt = 0;

            for (int i = 0; i < ssCal.ActiveSheet.RowCount; i++)
            {
                for (int j = 0; j < ssCal.ActiveSheet.ColumnCount; j++)
                {
                    strTemp1 = ""; strTemp2 = "";

                    strDate = lblYear.Text + "-" + lblMonth.Text + "-";

                    if (ssCal.ActiveSheet.Cells[i, j].Text.Trim() != "")
                    {
                        strDay = VB.Pstr(ssCal.ActiveSheet.Cells[i, j].Text, ">", 1);
                        strDay = VB.Pstr(strDay, "<", 2);
                        strDay = strDay.PadLeft(2, '0');

                        if (strDay.Trim() != "")
                        {
                            strDate += strDay;

                            //종검수검 정원체크(오전+오후)
                            nTotal = heaResvSetService.GetSumAmPmInwonBySDate(strDate, "TT");

                            //종검수검 인원체크
                            nCNT = heaJepsuService.GetJepCountBySDate(strDate);

                            if (nTotal > 0 && nTotal <= nCNT)
                            {
                                ssCal.ActiveSheet.Cells[i, j].BackColor = Color.MistyRose;
                                strTemp1 = "OK";
                            }

                            nTotal = 0; nCNT = 0;
                            AmCnt = 0;  PmCnt = 0; GAmCnt = 0; GPmCnt = 0;
                            //대장내시경 정원체크
                            HEA_RESV_SET item = heaResvSetService.GetSumInwonAMPMByGubun(strDate, "02");
                            if (!item.IsNullOrEmpty())
                            {
                                AmCnt = item.AMINWON;
                                PmCnt = item.PMINWON;
                                GAmCnt = item.GAINWONAM;
                                GPmCnt = item.GAINWONPM;
                                nTotal = AmCnt + PmCnt;
                            }

                            //대장내시경 정원체크
                            nCNT = heaResvExamService.GetCountbyPano(strDate, CF.DATE_ADD(clsDB.DbCon, strDate, 1), "02");
                            nCNT += GAmCnt + GPmCnt;

                            //대장내시경 회사가예약
                            HEA_RESV_LTD item2 = heaResvLtdService.GetSumAmPmJanByGubun(strDate, "02");
                            if (!item2.IsNullOrEmpty())
                            {
                                nCNT += item2.AMJAN;
                                nCNT += item2.PMJAN;
                            }

                            if (nTotal > 0 && nTotal <= nCNT)
                            {
                                ssCal.ActiveSheet.Cells[i, j].BackColor = Color.LemonChiffon;
                                strTemp2 = "OK";
                            }

                            //대장+마감
                            if (strTemp1 == "OK" && strTemp2 == "OK")
                            {
                                ssCal.ActiveSheet.Cells[i, j].BackColor = Color.LightBlue;
                            }

                            //해당일자 색깔 표시
                            if (strDay.To<int>() == nCurrDD && nCurrMon)
                            {
                                ssCal.ActiveSheet.Cells[i, j].BackColor = Color.DeepSkyBlue;
                            }
                        }

                    }
                    
                }
            }
        }

        private string chk_ExamRTimeBySDate(long nPano, string argExCode, string argCurDate)
        {
            string rtnVal = string.Empty;
            string strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(nPano, argExCode, argCurDate);

            if (!strRTime.IsNullOrEmpty())
            {
                if (strRTime != argCurDate)
                {
                    rtnVal = "OK";
                }
            }

            return rtnVal;
        }

        delegate void threadFormDelegate();
        delegate void threadProcessDelegate(bool b);

        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }

        void tProcess()
        {
            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.BeginInvoke(new System.Action(() => this.Enabled = false));

            this.Invoke(new threadFormDelegate(Screen_Display_CurrentDay));

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.BeginInvoke(new System.Action(() => this.Enabled = true));
        }
    }
}
