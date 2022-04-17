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

namespace HC_OSHA
{
    public partial class FrmJikwonIljeng : Form
    {
        public FrmJikwonIljeng()
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
            if (nMM == 13) { nMM = 1; nYY++; }
            for (i = 0; i < 12; i++)
            {
                cboYYMM.Items.Add(nYY.ToString() + "-" + VB.Format(nMM, "00"));
                nMM--;
                if (nMM == 0)
                {
                    nMM = 12;
                    nYY--;
                }
            }
            cboYYMM.SelectedIndex = 1;

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = true, ColumnHeaderHeight = 40 });
            SSList.AddColumnText("사번", "", 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("성명", "", 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("01", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("02", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("03", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("04", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("05", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("06", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("07", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("08", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("09", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("10", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("11", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("12", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("13", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("14", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("15", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("16", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("17", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("18", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("19", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("20", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("21", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("22", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("23", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("24", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("25", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("26", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("27", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("28", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("29", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("30", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });
            SSList.AddColumnText("31", "", 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap = false, IsMulti = false });

            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = 100;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYYMM.Text.Trim() + "월 직원 일정표";
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();

        }

        private void btnExcel1_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\직원일정표.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Data_Search();
        }

        private void Data_Search()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string strYYMM = "";
            string strDate = "";
            int nYY = 0;
            int nMM = 0;
            int nDD = 0;
            string strLastDay = "";
            int nCol = 0;

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

            SQL = "SELECT USERID,NAME FROM HIC_USERS ";
            SQL = SQL + ComNum.VBLF + "WHERE INDATE<='" + strLastDay + "' ";
            SQL = SQL + ComNum.VBLF + "  AND (TESADATE IS NULL OR TESADATE>='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01') ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND USERID NOT IN ('1') ";
            SQL = SQL + ComNum.VBLF + "ORDER BY NAME,USERID ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            SSList.ActiveSheet.RowCount = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["USERID"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 6;

                    //당월의 스케쥴을 읽어 표시함
                    SQL = "SELECT VISITDATE,SCHEDULE FROM HIC_USER_SCHEDULE ";
                    SQL = SQL + ComNum.VBLF + "WHERE USERID='" + dt.Rows[i]["USERID"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND VISITDATE>='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01' ";
                    SQL = SQL + ComNum.VBLF + "  AND VISITDATE<='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-31' ";
                    SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE ";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (dt1.Rows.Count > 0)
                    {
                        for (int j=0;j< dt1.Rows.Count; j++)
                        {
                            nCol = Int32.Parse(VB.Right(dt1.Rows[j]["VISITDATE"].ToString().Trim(), 2));
                            SSList.ActiveSheet.Cells[i, nCol+1].Text = dt1.Rows[j]["SCHEDULE"].ToString().Trim();
                        }
                    }
                    dt1.Dispose();
                    dt1 = null;
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            string strYYMM = "";
            string strDate = "";
            string strLastDay = "";
            int intRowAffected = 0;
            string strUSERID = "";
            string strSchedule = "";

            strYYMM = cboYYMM.Text.Trim();
            if (strYYMM == "")
            {
                ComFunc.MsgBox("작업월을 선택하세요");
                return;
            }

            //당월의 직원일정을 모두 삭제함
            SQL = "DELETE FROM HIC_USER_SCHEDULE ";
            SQL = SQL + ComNum.VBLF + "WHERE VISITDATE>='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01' ";
            SQL = SQL + ComNum.VBLF + "  AND VISITDATE<='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-31' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("기존 자료 삭제시 오류가 발생함", "알림");
                Cursor.Current = Cursors.Default;
                return;
            }

            //당월의 일정을 저장함
            for (int i=0;i< SSList.ActiveSheet.RowCount; i++)
            {
                strUSERID = SSList.ActiveSheet.Cells[i, 0].Text.Trim();
                for (int j = 1; j <= 31; j++)
                {
                    strSchedule = SSList.ActiveSheet.Cells[i, j+1].Text.Trim();
                    if (strSchedule != "")
                    {
                        strDate = strYYMM + "-" + VB.Format(j, "00");

                        SQL = "INSERT INTO HIC_USER_SCHEDULE (USERID,VISITDATE,SCHEDULE,SWLicense) VALUES ";
                        SQL = SQL + ComNum.VBLF + "('" + strUSERID + "','" + strDate + "','" + strSchedule + "',";
                        SQL = SQL + ComNum.VBLF + "'" + clsType.HosInfo.SwLicense + "') ";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("자료 저장 시 오류가 발생함", "알림");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

            }

            ComFunc.MsgBox("저장 완료", "알림");
        }
    }
}