using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_Main
{
    public partial class frmHaTimeInwon : Form
    {

        ComFunc CF = null;
        clsSpread cSpd = null;
        clsHcFunc cHF = null;

        private Point mCurrentPosition = new Point(0, 0);

        HeaJepsuService heaJepsuService = null;
        HeaResvSetTimeService heaResvSetTimeService = null;


        public frmHaTimeInwon()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.btnExit.Click += new EventHandler(eFormClose);
            this.btnClose.Click += new EventHandler(eFormClose);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSetting.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.panSub02.MouseMove += new MouseEventHandler(ePanMouseMove);
            this.panSub02.MouseDown += new MouseEventHandler(ePanMouseDown);
            this.panSub02.DragEnter += new DragEventHandler(ePanDragEnter);
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cSpd = new clsSpread();



            heaJepsuService = new HeaJepsuService();
            heaResvSetTimeService = new HeaResvSetTimeService();


            string strYYMM = DateTime.Now.ToShortDateString();

            Set_Spread(strYYMM);

            CF.ComboMonth_Set3(cboYYMM, 12, DateTime.Now.Year, DateTime.Now.Month);

            prgBar.Value = 0;

        }

        private void ePanDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ePanMouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                mCurrentPosition = new Point(-e.X, -e.Y);
            }
        }

        private void ePanMouseMove(object sender, MouseEventArgs e)
        {
            if (sender == panSub02)
            {
                if (e.Button == MouseButtons.Left)
                {
                    panSub02.Location = new Point(panSub02.Location.X + (mCurrentPosition.X + e.X), panSub02.Location.Y + (mCurrentPosition.Y + e.Y));
                }
            }
        }


        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Set_Spread(cboYYMM.Text.Trim());
                Screen_Display(cboYYMM.Text.Trim());
            }
            else if ( sender == btnSetting)
            {
                frmHaSetTime_Count frm = new frmHaSetTime_Count();
                frm.ShowDialog();
                cHF.fn_ClearMemory(frm);
                
            }
        }

        private void Screen_Clear()
        {
            panSub02.Visible = false;
            cSpd.Spread_Clear_Simple(SS1, 3);
        }

        private void Screen_Display(string argYYMM)
        {
            int nRow = 1;
            int nTo_Am = 0;
            int nTo_Pm = 0;
            int nTot_Am1 = 0;
            int nTot_Pm1 = 0;
            int nTot_Am2 = 0;
            int nTot_Pm2 = 0;
            int nCnt_Am = 0;
            int nCnt_Pm = 0;
            int nGa_Am = 0;
            int nGa_Pm = 0;

            string strSTime = "";
            string strSTimeCnt = "";
            string strTemp = "";


            long nInwon = 0;
            long nGaInwon = 0;

            string strExName = string.Empty;
            string strDate = string.Empty;
            List<string> lstGbexam = new List<string>();

            Cursor.Current = Cursors.WaitCursor;
            SS1.ActiveSheet.RowCount = 0;
            SS1.ActiveSheet.RowCount = 3;

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(5, 2).To<int>());

            Set_Yoil(argYYMM);
            SS1.ActiveSheet.Cells[1, 1].Text = "종검 예약인원";

            for (int i = 1; i <= nDays; i++)
            {

                strDate = argYYMM + "-" + VB.Format(i, "00");


                List <HEA_RESV_SET_TIME> list = heaResvSetTimeService.GetSumInwonAMPMByGubun(strDate, "00");
                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (SS1.ActiveSheet.RowCount < list.Count +1) { SS1.ActiveSheet.RowCount = list.Count+1; }


                        nInwon = list[j].INWON;
                        nGaInwon = list[j].GAINWON;
                        strSTime = list[j].STIME;

                        int nCNT = heaJepsuService.GetCountBySDateSTime(strDate, strSTime);
                        strTemp = nInwon+ nGaInwon + "/" + nCNT.To<string>("0");
                        SS1.ActiveSheet.Cells[j+1, 2].Text = strSTime;
                        SS1.ActiveSheet.Cells[j + 1, i + 2].Text = strTemp;

                        if (nInwon + nGaInwon == nCNT)
                        {
                            SS1.ActiveSheet.Cells[j + 1, i + 2].BackColor = Color.FromArgb(237, 235, 169);
                        }
                        else if (nInwon + nGaInwon < nCNT)
                        {
                            SS1.ActiveSheet.Cells[j + 1, i + 2].BackColor = Color.LightPink;
                        }
                    }
                }
            }
        }

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                Display_ResvInfo_Detail(e.Row, e.Column, cboYYMM.Text);
            }
        }

        private void Display_ResvInfo_Detail(int argRow, int argCol, string argYYMM)
        {
            if (argCol > 2 && argCol < SS1.ActiveSheet.ColumnCount - 1 && argRow >= 1)
            {
                cSpd.Spread_Clear_Simple(SS3);

                int nRow = 0;
                string strDate = VB.Left(argYYMM, 4) + "-" + VB.Mid(argYYMM, 6, 2);
                string strGbn = SS1.ActiveSheet.Cells[argRow, 0].Text.Trim();
                string strStime = "";

                strDate = strDate + "-" + VB.Format(SS1.ActiveSheet.Columns[argCol].Label.To<int>(), "00");
                strStime = SS1.ActiveSheet.Cells[argRow, 2].Text.Trim();
                lblDate.Text = strDate;


                //종검 일반예약 현황
                List<HEA_JEPSU> lst = heaJepsuService.GetListSNameBySDateSTime(strDate, strStime);
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        nRow += 1;
                        if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                        SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst[i].SNAME;
                        SS3.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].STIME;
                    }
                }
                
                panSub02.Visible = true;
            }

        }


        private void Set_Spread(string argYYMM)
        {
            SS1.Initialize(new SpreadOption { RowHeight = 24 });
            SS1.AddColumn("검사구분", "", 42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("검사항목", "", 52, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("시간", "", 60, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(5, 2).To<int>());

            for (int i = 1; i <= nDays; i++)
            {
                SS1.AddColumn(i.To<string>(), "", 38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            }

            SS1.AddColumn("합계", "", 72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            Screen_Clear();
        }

        private void eFormClose(object sender, EventArgs e)
        {
            if (sender == btnClose)
            {
                panSub02.Visible = false;
            }
            else if (sender == btnExit)
            {
                this.Hide();
                return;
            }
        }

        private void Set_Yoil(string argYYMM)
        {
            string strDate = string.Empty;
            string strYoil = string.Empty;
            string strLastDay = CF.READ_LASTDAY(clsDB.DbCon, argYYMM + "-01");
            int nLastDay = VB.Right(strLastDay, 2).To<int>();

            UnaryComparisonConditionalFormattingRule unary;

            unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
            unary.BackColor = Color.LightSteelBlue;

            //일자별 요일을 SET
            for (int i = 1; i <= nLastDay; i++)
            {
                strDate = argYYMM + "-" + VB.Format(i, "00");
                strYoil = CF.READ_YOIL(clsDB.DbCon, strDate);

                SS1.ActiveSheet.Cells[0, i + 2].Text = VB.Left(strYoil, 1);

                if (VB.Left(strYoil, 1) == "토")
                {
                    unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                    unary.BackColor = Color.LightSteelBlue;
                    SS1.ActiveSheet.SetConditionalFormatting(-1, i + 2, unary);
                }
                else if (VB.Left(strYoil, 1) == "일")
                {
                    unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "*", false);
                    unary.BackColor = Color.MistyRose;
                    SS1.ActiveSheet.SetConditionalFormatting(-1, i + 2, unary);
                }
            }
        }
    }
}
