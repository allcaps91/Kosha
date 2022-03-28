using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Enums;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.Core.Common.Interface;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace HC_OSHA.form.Schedule
{
    public partial class FrmVisitMonth : Form
    {
        public FrmVisitMonth()
        {
            InitializeComponent();
            Screen_Set();
            Data_Search();
        }
        private void Screen_Set()
        {
            int i = 0;
            string strYYMM = DateTime.Now.ToString("yyyyMM");
            int nYY = 0;
            int nMM = 0;

            nYY = Int32.Parse(VB.Left(strYYMM, 4));
            nMM = Int32.Parse(VB.Right(strYYMM, 2));

            nMM++;
            if (nMM==13) { nMM = 1; nYY++; }
            for (i = 0; i < 24; i++)
            {
                cboYYMM.Items.Add(nYY.ToString() + "-" + VB.Format(nMM,"00"));
                nMM--;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY--;
                }
            }
            cboYYMM.SelectedIndex = 1;

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = true, ColumnHeaderHeight = 40 });
            SSList.AddColumnText("구분", "", 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("이름", "", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("01", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("02", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("03", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("04", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("05", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("06", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("07", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("08", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("09", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("10", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("11", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("12", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("13", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("14", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("15", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("16", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("17", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("18", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("19", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("20", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("21", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("22", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("23", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("24", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("25", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("26", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("27", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("28", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("29", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("30", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("31", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });

            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = 100;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYYMM.Text.Trim() + "월 보건관리 일정표";
            //sp.Period = "회사명: " + TxtLtdcode.Text + "          " + "근로자: " + TxtName.Text;
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\보건관리일정표.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Data_Search();
        }

        private void Data_Search()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strYYMM = "";
            string strDate = "";
            int nYY = 0;
            int nMM = 0;
            int nDD = 0;
            string strLastDay = "";
            string strOldData = "";
            string strNewData = "";
            string strView = "";
            int nRow = 0;
            int nCol = 0;
            string strVisit = "";
            bool bOK = false;

            strYYMM = cboYYMM.Text.Trim();
            if (strYYMM == "")
            {
                ComFunc.MsgBox("작업월을 선택하세요");
                return;
            }

            //월별 제목줄 설정
            SSList.ActiveSheet.RowCount = 50;
            nYY = Int32.Parse(VB.Left(strYYMM, 4));
            nMM = Int32.Parse(VB.Right(strYYMM, 2));
            strLastDay = clsVbfunc.LastDay(nYY, nMM);
            nDD = Int32.Parse(VB.Right(strLastDay, 2));
            for (int i = 1; i <= 31; i++)
            {
                if (i <= nDD)
                {
                    strDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-" + VB.Format(i, "00");
                    SSList_Sheet1.ColumnHeader.Cells[0, i + 1].Text = VB.Right(strYYMM, 2) + "/" + VB.Format(i, "00") + " (";
                    SSList_Sheet1.ColumnHeader.Cells[0, i + 1].Text += VB.Left(clsVbfunc.GetYoIl(strDate), 1) + ")";
                    SSList.ActiveSheet.Rows[i + 1].Visible = true;
                }
                else
                {
                    SSList_Sheet1.ColumnHeader.Cells[0, i + 1].Text = " ";
                    SSList.ActiveSheet.Rows[i + 1].Visible = false;
                }
            }

            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = 100;

            SQL = "SELECT DECODE(C.ROLE,'DOCTOR','1','NURSE','2','3') AS ROLE,C.USERID,C.NAME, ";
            SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.VISITRESERVEDATE,'yyyyMMdd') VISITRESERVEDATE,";
            SQL = SQL + ComNum.VBLF + "   A.VISITSTARTTIME,A.VISITMANAGERID,B.NAME AS LTDNAME ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_SCHEDULE A,HC_SITE_VIEW B,HIC_USERS C ";
            SQL = SQL + ComNum.VBLF + "WHERE TO_CHAR(A.VISITRESERVEDATE,'YYYY-MM')='" + strYYMM + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.VISITUSERID = C.USERID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY ROLE,C.NAME,A.VISITRESERVEDATE,A.VISITSTARTTIME,LTDNAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strOldData = "";
            nRow = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["ROLE"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[i]["USERID"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[i]["NAME"].ToString().Trim() + "{}";

                    if (VB.Pstr(strNewData, "{}", 1) != VB.Pstr(strOldData, "{}", 1))
                    {
                        if (strOldData != "") SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 4;
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow, 0].Text = "";
                        if (dt.Rows[i]["ROLE"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "의사";
                        if (dt.Rows[i]["ROLE"].ToString().Trim() == "2") SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "간호사";
                        if (dt.Rows[i]["ROLE"].ToString().Trim() == "3") SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "산업위생";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    }
                    else if (strNewData != strOldData)
                    {
                        SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 4;
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    }
                    nDD = Int32.Parse(VB.Right(dt.Rows[i]["VISITRESERVEDATE"].ToString().Trim(), 2));
                    strVisit = "";
                    if (dt.Rows[i]["ROLE"].ToString().Trim() != "1" && dt.Rows[i]["VISITMANAGERID"].ToString().Trim() != "") strVisit += "★";
                    if (dt.Rows[i]["VISITSTARTTIME"].ToString().Trim() != "") strVisit = dt.Rows[i]["VISITSTARTTIME"].ToString().Trim() + " ";
                    strVisit += dt.Rows[i]["LTDNAME"].ToString().Trim();
                    //if (strVisit.Length > 14) strVisit = strVisit.Substring(0, 14);

                    if (SSList.ActiveSheet.Cells[nRow - 1, nDD + 1].Text.Trim() == "")
                    {
                        SSList.ActiveSheet.Cells[nRow - 1, nDD + 1].Text = strVisit;
                    }
                    else
                    {
                        //SSList.ActiveSheet.Cells[nRow-1, nDD + 1].Text += ComNum.VBLF + strVisit;
                        SSList.ActiveSheet.Cells[nRow - 1, nDD + 1].Text += "\r" + strVisit;
                    }
                    strOldData = strNewData;
                }
                SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 2;
            }
            dt.Dispose();
            dt = null;
            SSList.ActiveSheet.RowCount = nRow;

            //일정이 없는 날 칼럼넓이를 좁게
            for (int i = 2; i <= 32; i++)
            {
                bOK = false;
                for (int j = 0; j < nRow; j++)
                {
                    if (SSList.ActiveSheet.Cells[j, i].Text.Trim() != "") { bOK = true; break; }
                }
                if (bOK == false)
                {
                    SSList_Sheet1.ColumnHeader.Columns[i].Width = 45;
                }
                else
                {
                    SSList_Sheet1.ColumnHeader.Columns[i].Width = 160;
                }
            }

        }

        private void SSList_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void panSearch_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
