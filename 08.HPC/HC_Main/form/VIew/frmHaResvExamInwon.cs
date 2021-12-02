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
/// Class Name      : HC_Main
/// File Name       : frmHaResvExamInwon.cs
/// Description     : 종검 검사별 예약정원 조회
/// Author          : 김민철
/// Create Date     : 2020-04-23
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm일자별정원현황(Frm일자별정원현황.frm)" />
namespace HC_Main
{
    public partial class frmHaResvExamInwon : Form
    {
        HeaCodeService heaCodeService = null;
        HeaResvSetService heaResvSetService = null;
        HeaJepsuService heaJepsuService = null;
        HeaResvLtdService heaResvLtdService = null;
        HeaResvExamService heaResvExamService = null;

        ComFunc CF = null;
        clsSpread cSpd = null;

        private Point mCurrentPosition = new Point(0, 0);

        public frmHaResvExamInwon()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetEvent()
        {
            this.btnExit.Click      += new EventHandler(eFormClose);
            this.btnClose.Click     += new EventHandler(eFormClose);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.SS1.CellClick      += new CellClickEventHandler(eSpdClick);
            this.panSub02.MouseMove += new MouseEventHandler(ePanMouseMove);
            this.panSub02.MouseDown += new MouseEventHandler(ePanMouseDown);
            this.panSub02.DragEnter += new DragEventHandler(ePanDragEnter);
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

        private void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                Display_ResvInfo_Detail(e.Row, e.Column, cboYYMM.Text);
            }
        }

        private void Display_ResvInfo_Detail(int argRow, int argCol, string argYYMM)
        {
            if (argCol > 2 && argCol < SS1.ActiveSheet.ColumnCount-1 && argRow >= 1)
            {
                cSpd.Spread_Clear_Simple(SS3);

                int nRow = 0;
                string strDate = VB.Left(argYYMM, 4) + "-" + VB.Mid(argYYMM, 6, 2);
                string strGbn = SS1.ActiveSheet.Cells[argRow, 0].Text.Trim();
                strDate = strDate + "-" + VB.Format(SS1.ActiveSheet.Columns[argCol].Label.To<int>(), "00");

                lblDate.Text = strDate;

                if (argRow == 1)
                {
                    //종검 일반예약 현황
                    List<HEA_JEPSU> lst = heaJepsuService.GetListSNameSTimeBySDate(strDate);
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
                }
                else
                {
                    //선택검사 일반예약 현황
                    List<HEA_RESV_EXAM> lst = heaResvExamService.GetItembyRTime(strDate, CF.DATE_ADD(clsDB.DbCon, strDate, 1), strGbn);
                    if (lst.Count > 0)
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            nRow += 1;
                            if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                            SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst[i].SNAME;
                            SS3.ActiveSheet.Cells[nRow - 1, 1].Text = lst[i].RTIME;
                        }
                    }
                }

                //회사 가예약 사항 Display
                List<HEA_RESV_LTD> lst2 = heaResvLtdService.GetListAmPmJanBySDateGubun(strDate, strGbn, argRow);
                if (lst2.Count > 0)
                {
                    for (int i = 0; i < lst2.Count; i++)
                    {
                        if (lst2[i].AMJAN > 0)
                        {
                            nRow += 1;
                            if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                            SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst2[i].LTDNAME;
                            SS3.ActiveSheet.Cells[nRow - 1, 1].Text = "오전";
                            SS3.ActiveSheet.Cells[nRow - 1, 2].Text = lst2[i].AMJAN.To<string>() + " 명";


                        }

                        if (lst2[i].PMJAN > 0)
                        {
                            nRow += 1;
                            if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                            SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst2[i].LTDNAME;
                            SS3.ActiveSheet.Cells[nRow - 1, 1].Text = "오후";
                            SS3.ActiveSheet.Cells[nRow - 1, 2].Text = lst2[i].PMJAN.To<string>() + " 명";
                        }
                        
                    }
                }

                //가예약현황
                List<HEA_RESV_SET> lst3 = heaResvSetService.GetListGaInwonBySDate(strDate, strGbn, argRow);
                if (lst3.Count > 0)
                {
                    for (int i = 0; i < lst3.Count; i++)
                    {
                        if (lst3[i].GAINWONAM > 0)
                        {
                            nRow += 1;
                            if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                            SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst3[i].EXAMNAME;
                            SS3.ActiveSheet.Cells[nRow - 1, 1].Text = "오전";
                            SS3.ActiveSheet.Cells[nRow - 1, 2].Text = lst3[i].GAINWONAM.To<string>() + " 명";


                        }

                        if (lst3[i].GAINWONPM > 0)
                        {
                            nRow += 1;
                            if (SS3.ActiveSheet.RowCount < nRow) { SS3.ActiveSheet.RowCount = nRow; }

                            SS3.ActiveSheet.Cells[nRow - 1, 0].Text = lst3[i].EXAMNAME;
                            SS3.ActiveSheet.Cells[nRow - 1, 1].Text = "오후";
                            SS3.ActiveSheet.Cells[nRow - 1, 2].Text = lst3[i].GAINWONPM.To<string>() + " 명";
                        }

                    }
                }

                panSub02.Visible = true;
            }
        }

        private void SetControl()
        {
            heaCodeService = new HeaCodeService();
            heaResvSetService = new HeaResvSetService();
            heaJepsuService = new HeaJepsuService();
            heaResvLtdService = new HeaResvLtdService();
            heaResvExamService = new HeaResvExamService();

            CF = new ComFunc();
            cSpd = new clsSpread();

            string strYYMM = DateTime.Now.ToShortDateString();

            Set_Spread(strYYMM);

            CF.ComboMonth_Set3(cboYYMM, 12, DateTime.Now.Year, DateTime.Now.Month);

            prgBar.Value = 0;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Set_Spread(cboYYMM.Text.Trim());
                Screen_Display(cboYYMM.Text.Trim());
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
            string strExName = string.Empty;
            string strDate = string.Empty;
            List<string> lstGbexam = new List<string>();

            Cursor.Current = Cursors.WaitCursor;

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(5, 2).To<int>());

            Set_Yoil(argYYMM);

            //종검 예약인원
            SS1.ActiveSheet.Cells[1, 1].Text = "종검 예약인원";
            SS1.ActiveSheet.Cells[1, 2].Text = "A/P";

            for (int i = 1; i <= nDays; i++)
            {
                strDate = argYYMM + "-" + VB.Format(i, "00");

                nCnt_Am = 0;nCnt_Pm = 0;
                HEA_RESV_SET item = heaResvSetService.GetSumInwonAMPMByGubun(strDate, "TT");

                if (!item.IsNullOrEmpty())
                {
                    if (VB.Right(strDate, 5) != "12-25")
                    {
                        nCnt_Am = item.AMINWON.To<int>(0);
                        nCnt_Pm = item.PMINWON.To<int>(0);
                    }
                }

                nTo_Am = 0; nTo_Pm = 0;
                List<HEA_JEPSU> list = heaJepsuService.GetCountAmPm2BySDate(strDate);

                if (list.Count > 0)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (VB.Right(strDate, 5) != "12-25")
                        {
                            if (list[j].AMPM2 == "1")
                            {
                                nTo_Am = list[j].WCNT.To<int>(0);
                            }
                            else
                            {
                                nTo_Pm = list[j].WCNT.To<int>(0);
                            }
                        }
                    }
                }

                //회사별 가예약
                HEA_RESV_LTD item3 = heaResvLtdService.GetSumAmPmJanByGubun(strDate, "00");

                if (!item3.IsNullOrEmpty())
                {
                    if (VB.Right(strDate, 5) != "12-25")
                    {
                        if (item3.AMJAN > 0)
                        {
                            nTo_Am = nTo_Am + item3.AMJAN.To<int>(0);
                        }
                        else if (item3.PMJAN > 0)
                        {
                            nTo_Pm = nTo_Pm + item3.PMJAN.To<int>(0);
                        }
                        
                    }
                }

                //개인별 가예약
                HEA_RESV_SET item4 = heaResvSetService.GetSumGaInwonAMPMByGubun(strDate, "00");

                if (!item4.IsNullOrEmpty())
                {
                    if (VB.Right(strDate, 5) != "12-25")
                    {
                        if (item4.AMINWON > 0 || item4.GAINWONAM >0)
                        {
                            nTo_Am = nTo_Am + item4.AMINWON.To<int>(0);
                            nTo_Am = nTo_Am + item4.GAINWONAM.To<int>(0);
                        }
                        else if (item4.PMINWON > 0 || item4.GAINWONPM > 0)
                        {
                            nTo_Pm = nTo_Pm + item4.PMINWON.To<int>(0);
                            nTo_Pm = nTo_Pm + item4.GAINWONPM.To<int>(0);
                        }
                    }
                }

                SS1.ActiveSheet.Cells[1, i + 2].Text = nTo_Am.To<string>() + "/" + nTo_Pm.To<string>();

                if (nTo_Am > nCnt_Am || nTo_Pm > nCnt_Pm)
                {
                    SS1.ActiveSheet.Cells[1, i + 2].BackColor = Color.LightPink;
                }
                else
                {
                    if (nTo_Am == nCnt_Am )
                    {
                        SS1.ActiveSheet.Cells[1, i + 2].BackColor = Color.FromArgb(237, 235, 169);
                    }
                    //SS1.ActiveSheet.Cells[1, i + 2].BackColor = Color.FromArgb(237, 235, 169);
                }

                nTot_Am1 = nTot_Am1 + nTo_Am;
                nTot_Pm1 = nTot_Pm1 + nTo_Pm;
            }

            SS1.ActiveSheet.Cells[1, SS1.ActiveSheet.ColumnCount - 1].Text = VB.Format(nTot_Am1, "#,##0") + "/" + VB.Format(nTot_Pm1, "#,##0");

            nTo_Am = 0; nTo_Pm = 0;

            //선택검사 예약인원
            Control[] controls = ComFunc.GetAllControls(this);
            foreach (Control ctl in controls)
            {
                if (ctl is CheckBox)
                {
                    if (((CheckBox)ctl).Checked == true)
                    {
                        lstGbexam.Add(VB.Left(((CheckBox)ctl).Text, 2));
                    }
                }
            }

            //검사항목 Display
            IList<HEA_CODE> lst2 = heaCodeService.GetItemByGubunGroupBy("13", lstGbexam);

            if (lst2.Count > 0)
            {
                SS1.ActiveSheet.RowCount = lst2.Count * 2 + 2;

                for (int i = 0; i < lst2.Count; i++)
                {
                    SS1.ActiveSheet.Cells[nRow * 2, 0].Text = lst2[i].GUBUN2.Trim();
                    SS1.ActiveSheet.Cells[nRow * 2, 1].Text = lst2[i].NAME.Trim();
                    SS1.ActiveSheet.Cells[nRow * 2, 2].Text = "AM";

                    SS1.ActiveSheet.AddSpanCell(nRow * 2, 1, 2, 1);

                    SS1.ActiveSheet.Cells[nRow * 2 + 1, 0].Text = lst2[i].GUBUN2.Trim();
                    SS1.ActiveSheet.Cells[nRow * 2 + 1, 2].Text = "PM";

                    nRow = nRow + 1;
                }
            }

            prgBar.Maximum = SS1.ActiveSheet.RowCount;
            prgBar.Value = 0;

            //검사항목별 인원현황 Display
            for (int i = 2; i < SS1.ActiveSheet.RowCount; i+=2)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text.Trim() != "")
                {
                    strExName = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                }

                //인원 조회
                nTot_Am1 = 0; nTot_Pm1 = 0;
                nTot_Am2 = 0; nTot_Pm2 = 0;
                for (int j = 1; j <= nDays; j++)
                {
                    strDate = argYYMM + "-" + VB.Format(j, "00");

                    if (strDate == "2021-01-09")
                    {
                        strDate = strDate;
                    }

                    //예약인원
                    HEA_RESV_SET item = heaResvSetService.GetSumInwonAMPMByGubun(strDate, strExName);
                    if (!item.IsNullOrEmpty())
                    {
                        nTo_Am = 0; nTo_Pm = 0;

                        if (VB.Right(strDate, 5) != "12-25")
                        {
                            nTo_Am = item.AMINWON.To<int>(0);
                            nTo_Pm = item.PMINWON.To<int>(0);
                        }
                    }
                    //가예약인원
                    HEA_RESV_SET item2 = heaResvSetService.GetSumGaInwonAMPMByGubun(strDate, strExName);
                    if (!item2.IsNullOrEmpty())
                    {
                        nGa_Am = 0; nGa_Pm = 0;

                        if (VB.Right(strDate, 5) != "12-25")
                        {
                            nGa_Am = item2.GAINWONAM.To<int>(0);
                            nGa_Pm = item2.GAINWONPM.To<int>(0);
                        }
                    }

                    nCnt_Am = 0;
                    nCnt_Pm = 0;
                    //오전, 오후별 예약인원
                    List<HEA_RESV_EXAM> list = heaResvExamService.GetCNTAMPMbyRTime(strDate, CF.DATE_ADD(clsDB.DbCon, strDate, 1), strExName);
                    if (list.Count > 0)
                    {
                        for (int k = 0; k < list.Count; k++)
                        {
                            if (VB.Right(strDate, 5) != "12-25")
                            {
                                switch (list[k].AMPM.To<string>("").Trim())
                                {
                                    case "A": nCnt_Am = list[k].CNT.To<int>(0); break;
                                    case "P": nCnt_Pm = list[k].CNT.To<int>(0); break;
                                    default: break;
                                }
                            }
                        }
                    }

                    //회사별 가예약
                    HEA_RESV_LTD item3 = heaResvLtdService.GetSumAmPmJanByGubun(strDate, strExName);

                    if (!item3.IsNullOrEmpty())
                    {
                        if (VB.Right(strDate, 5) != "12-25")
                        {
                            if (item3.AMJAN.To<int>(0) > 0) { nGa_Am += item3.AMJAN.To<int>(0); }
                            if (item3.PMJAN.To<int>(0) > 0) { nGa_Pm += item3.PMJAN.To<int>(0); }
                        }
                    }

                    SS1.ActiveSheet.Cells[i, j + 2].Text = nTo_Am.To<string>() + "/" + (nGa_Am + nCnt_Am).To<string>();

                    //정원오전
                    if ((nGa_Am + nCnt_Am) > 0)
                    {
                        if (nTo_Am == (nGa_Am + nCnt_Am))
                        {
                            SS1.ActiveSheet.Cells[i, j + 2].BackColor = Color.FromArgb(237, 235, 169);
                        }
                        else if (nTo_Am < (nGa_Am + nCnt_Am))
                        {
                            SS1.ActiveSheet.Cells[i, j + 2].BackColor = Color.LightPink;
                        }
                    }

                    SS1.ActiveSheet.Cells[i + 1, j + 2].Text = nTo_Pm.To<string>() + "/" + (nGa_Pm + nCnt_Pm).To<string>();

                    //정원오후
                    if ((nGa_Pm + nCnt_Pm) > 0)
                    {
                        if (nTo_Pm == (nGa_Pm + nCnt_Pm))
                        {
                            SS1.ActiveSheet.Cells[i + 1, j + 2].BackColor = Color.FromArgb(237, 235, 169);
                        }
                        else if (nTo_Pm < (nGa_Pm + nCnt_Pm))
                        {
                            SS1.ActiveSheet.Cells[i + 1, j + 2].BackColor = Color.LightPink;
                        }
                    }

                    //합계에 누적
                    nTot_Am1 = nTot_Am1 + nTo_Am;
                    nTot_Am2 = nTot_Am2 + nGa_Am + nCnt_Am;
                    nTot_Pm1 = nTot_Pm1 + nTo_Pm;
                    nTot_Pm2 = nTot_Pm2 + nGa_Pm + nCnt_Pm;
                }

                SS1.ActiveSheet.Cells[i, SS1.ActiveSheet.ColumnCount - 1].Text = VB.Format(nTot_Am1, "#,##0") + "/" + VB.Format(nTot_Am2, "#,##0");
                SS1.ActiveSheet.Cells[i + 1, SS1.ActiveSheet.ColumnCount - 1].Text = VB.Format(nTot_Pm1, "#,##0") + "/" + VB.Format(nTot_Pm2, "#,##0");

                prgBar.Value = i;
                Application.DoEvents();
            }

            prgBar.Value = prgBar.Maximum;

            Cursor.Current = Cursors.Default;
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

        private void Set_Spread(string argYYMM)
        {
            SS1.Initialize(new SpreadOption { RowHeight = 24 });
            SS1.AddColumn("검사구분", "",  42, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SS1.AddColumn("검사항목", "",  52, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("구분",     "",  30, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            int nDays = DateTime.DaysInMonth(argYYMM.Substring(0, 4).To<int>(), argYYMM.Substring(5, 2).To<int>());

            for (int i = 1; i <= nDays; i++)
            {
                SS1.AddColumn(i.To<string>(), "", 38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = true });
            }

            SS1.AddColumn("합계", "", 72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });

            Screen_Clear();
        }
    }
}
