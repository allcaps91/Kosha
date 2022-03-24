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
    public partial class FrmChuljangIlji : Form
    {
        public FrmChuljangIlji()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            DateTime dtDate = DateTime.Now;

            for (i = 0; i < 180; i++)
            {
                //월요일만 표시함
                int nYoil = (int)dtDate.DayOfWeek;

                if (nYoil==1)
                {
                    cboDate.Items.Add(dtDate.ToString("yyyy-MM-dd"));
                }
                dtDate = dtDate.AddDays(-1);
                if (dtDate.ToString("yyyy-MM-dd") == "2022-02-20") break;
            }
            cboDate.SelectedIndex = 0;

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = false, ColumnHeaderHeight = 40 });
            SSList.AddColumnText("구분", "", 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("이름", "", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("1", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("2", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("3", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("4", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("5", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("6", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });

            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = 100;
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
            string strStartDate = "";
            string strEndDate = "";
            string strDateList = "";

            string strOldData = "";
            string strNewData = "";
            int nRow = 0;
            int nCol = 0;
            string strVisit = "";
            bool bOK = false;

            strDate = cboDate.Text.Trim();
            if (strDate == "")
            {
                ComFunc.MsgBox("작업일을 선택하세요");
                return;
            }

            //주간 날짜를 표시
            SSList.ActiveSheet.RowCount = 100;
            DateTime dtDate = Convert.ToDateTime(strDate);
            strDateList = "";
            strStartDate = strDate.Replace("-", "");
            nCol = 1;
            for (int i = 1; i <= 7; i++)
            {
                strDate = dtDate.ToString("yyyy-MM-dd");
                if (VB.Left(clsVbfunc.GetYoIl(strDate), 1) != "일")
                {
                    strEndDate = strDate.Replace("-", "");

                    //날짜별 칼럼번호를 저장
                    nCol++;
                    strDateList += dtDate.ToString("yyyyMMdd") + "=" + nCol + ",";

                    SSList_Sheet1.ColumnHeader.Cells[0, nCol].Text = strDate + " (";
                    SSList_Sheet1.ColumnHeader.Cells[0, nCol].Text += VB.Left(clsVbfunc.GetYoIl(strDate), 1) + ")";
                    dtDate = dtDate.AddDays(1);
                }
            }

            //의사 상태보고서 날짜를 읽음
            SQL = "SELECT A.DOCTORNAME,A.VISITDATE,B.NAME AS LTDNAME ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_REPORT_DOCTOR A,HC_SITE_VIEW B ";
            SQL = SQL + ComNum.VBLF + "WHERE A.VISITDATE>='" + strStartDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.VISITDATE<='" + strEndDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND A.DOCTORNAME IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.DOCTORNAME,A.VISITDATE,LTDNAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strOldData = "";
            nRow = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData ="1{}" + dt.Rows[i]["DOCTORNAME"].ToString().Trim();

                    if (strOldData=="")
                    {
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "의사";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["DOCTORNAME"].ToString().Trim();
                    }
                    else if (strNewData != strOldData)
                    {
                        SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["DOCTORNAME"].ToString().Trim();
                    }

                    //칼럼번호를 찾음
                    nCol = 0;
                    for (int j = 1; j <= 7; j++)
                    {
                        strDate = VB.Pstr(strDateList, ",", j);
                        if (dt.Rows[i]["VISITDATE"].ToString().Trim() == VB.Pstr(strDate, "=", 1))
                        {
                            nCol = Int32.Parse(VB.Pstr(strDate, "=", 2));
                            break;
                        }
                    }

                    if (nCol > 0)
                    {
                        strVisit = dt.Rows[i]["LTDNAME"].ToString().Trim();

                        if (SSList.ActiveSheet.Cells[nRow - 1, nCol].Text.Trim() == "")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, nCol].Text = strVisit;
                        }
                        else
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, nCol].Text += "\r" + strVisit;
                        }
                    }
                    strOldData = strNewData;
                }
                SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
            }
            dt.Dispose();
            dt = null;

            //간호사 상태보고서 날짜를 읽음
            SQL = "SELECT A.NURSENAME,A.VISITDATE,B.NAME AS LTDNAME ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_REPORT_NURSE A,HC_SITE_VIEW B ";
            SQL = SQL + ComNum.VBLF + "WHERE A.VISITDATE>='" + strStartDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.VISITDATE<='" + strEndDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND A.NURSENAME IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.NURSENAME,A.VISITDATE,LTDNAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strOldData = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = "2{}" + dt.Rows[i]["NURSENAME"].ToString().Trim();

                    if (strOldData=="")
                    {
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "간호사";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["NURSENAME"].ToString().Trim();
                    }
                    else if (strNewData != strOldData)
                    {
                        SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["NURSENAME"].ToString().Trim();
                    }

                    //칼럼번호를 찾음
                    nCol = 0;
                    for (int j = 1; j <= 7; j++)
                    {
                        strDate = VB.Pstr(strDateList, ",", j);
                        if (dt.Rows[i]["VISITDATE"].ToString().Trim() == VB.Pstr(strDate, "=", 1))
                        {
                            nCol = Int32.Parse(VB.Pstr(strDate, "=", 2));
                            break;
                        }
                    }

                    if (nCol > 0)
                    {
                        strVisit = dt.Rows[i]["LTDNAME"].ToString().Trim();

                        if (SSList.ActiveSheet.Cells[nRow - 1, nCol].Text.Trim() == "")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, nCol].Text = strVisit;
                        }
                        else
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, nCol].Text += "\r" + strVisit;
                        }
                    }
                    strOldData = strNewData;
                }
                SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
            }
            dt.Dispose();
            dt = null;

            //산업위생 상태보고서 날짜를 읽음
            SQL = "SELECT A.ENGINEERNAME,A.VISITDATE,B.NAME AS LTDNAME ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_REPORT_ENGINEER A,HC_SITE_VIEW B ";
            SQL = SQL + ComNum.VBLF + "WHERE A.VISITDATE>='" + strStartDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.VISITDATE<='" + strEndDate + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ENGINEERNAME IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.ENGINEERNAME,A.VISITDATE,LTDNAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strOldData = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = "3{}" + dt.Rows[i]["ENGINEERNAME"].ToString().Trim();

                    if (strOldData=="")
                    {
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "산업위생";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["ENGINEERNAME"].ToString().Trim();
                    }
                    else if (strNewData != strOldData)
                    {
                        SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
                        nRow++;
                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["ENGINEERNAME"].ToString().Trim();
                    }

                    //칼럼번호를 찾음
                    nCol = 0;
                    for (int j = 1; j <= 7; j++)
                    {
                        strDate = VB.Pstr(strDateList, ",", j);
                        if (dt.Rows[i]["VISITDATE"].ToString().Trim() == VB.Pstr(strDate, "=", 1))
                        {
                            nCol = Int32.Parse(VB.Pstr(strDate, "=", 2));
                            break;
                        }
                    }

                    if (nCol > 0)
                    {
                        strVisit = dt.Rows[i]["LTDNAME"].ToString().Trim();

                        if (SSList.ActiveSheet.Cells[nRow - 1, nCol].Text.Trim() == "")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, nCol].Text = strVisit;
                        }
                        else
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, nCol].Text += "\r" + strVisit;
                        }
                    }
                    strOldData = strNewData;
                }
                SSList_Sheet1.Rows[nRow - 1].Height = SSList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
            }
            dt.Dispose();
            dt = null;

            if (SSList.ActiveSheet.Cells[0, 4].Text.Trim() == "") SSList.ActiveSheet.Cells[0, 4].Text = "-";
            if (SSList.ActiveSheet.Cells[0, 5].Text.Trim() == "") SSList.ActiveSheet.Cells[0, 5].Text = "-";
            if (SSList.ActiveSheet.Cells[0, 6].Text.Trim() == "") SSList.ActiveSheet.Cells[0, 6].Text = "-";
            if (SSList.ActiveSheet.Cells[0, 7].Text.Trim() == "") SSList.ActiveSheet.Cells[0, 7].Text = "-";

            SSList.ActiveSheet.RowCount = nRow;

            BtnPrint.Enabled = true;
            btnExcel.Enabled = true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.HEAD_APPROVAL);
            sp.Title =  "주간 출장 업무일지(" + cboDate.Text.Trim() + ")";
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\주간출장업무일지.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
