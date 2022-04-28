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
    public partial class FrmVisitMonth2 : Form
    {
        public FrmVisitMonth2()
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
            for (i = 0; i < 24; i++)
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

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = false, ColumnHeaderHeight = 30 });
            SSList.AddColumnText("일자", "", 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("요일", "", 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            for (i = 0; i < 30; i++)
            {
                SSList.AddColumnText(" ", "", 124, IsReadOnly.Y, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsSort = false, WordWrap = true, IsMulti = true });
            }
            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = 31;
        }


        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYYMM.Text.Trim() + "월 보건관리 일정표";
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();

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
            nYY = Int32.Parse(VB.Left(strYYMM, 4));
            nMM = Int32.Parse(VB.Right(strYYMM, 2));
            strLastDay = clsVbfunc.LastDay(nYY, nMM);
            nDD = Int32.Parse(VB.Right(strLastDay, 2));
            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = nDD;

            //시트 Clear
            for (int j = 1; j < SSList.ActiveSheet.ColumnCount; j++)
            {
                if (j>2)
                {
                    SSList_Sheet1.ColumnHeader.Columns[j].Width = 124;
                    SSList_Sheet1.ColumnHeader.Columns[j].Visible = true;
                }
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    SSList.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            //요일 표시 
            for (int i = 1; i <= nDD; i++)
            {
                strDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-" + VB.Format(i, "00");
                SSList.ActiveSheet.Cells[i - 1, 0].Text = VB.Format(i, "00") + "일";
                SSList.ActiveSheet.Cells[i - 1, 1].Text = VB.Left(clsVbfunc.GetYoIl(strDate), 1);
            }

            SQL = "SELECT DECODE(C.ROLE,'DOCTOR','1','NURSE','2','3') AS ROLE,C.USERID,C.NAME, ";
            SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.VISITRESERVEDATE,'yyyyMMdd') VISITRESERVEDATE,";
            SQL = SQL + ComNum.VBLF + "   A.VISITSTARTTIME,A.VISITMANAGERNAME,B.NAME AS LTDNAME ";
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
            nCol = 1;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strNewData = dt.Rows[i]["ROLE"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[i]["USERID"].ToString().Trim() + "{}";
                    strNewData += dt.Rows[i]["NAME"].ToString().Trim() + "{}";

                    if (strNewData != strOldData)
                    {
                        nCol++;
                        SSList_Sheet1.ColumnHeader.Cells[0,nCol].Text = dt.Rows[i]["NAME"].ToString().Trim();

                        //당월의 스케쥴을 읽어 표시함
                        if (strNewData != strOldData)
                        {
                            SQL = "SELECT VISITDATE,SCHEDULE FROM HIC_USER_SCHEDULE ";
                            SQL = SQL + ComNum.VBLF + "WHERE USERID='" + VB.Pstr(strNewData, "{}", 2) + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND VISITDATE>='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01' ";
                            SQL = SQL + ComNum.VBLF + "  AND VISITDATE<='" + VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-31' ";
                            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                            if (dt1.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt1.Rows.Count; j++)
                                {
                                    nDD = Int32.Parse(VB.Right(dt1.Rows[j]["VISITDATE"].ToString().Trim(), 2));
                                    SSList.ActiveSheet.Cells[nDD - 1, nCol].Text = "<" + dt1.Rows[j]["SCHEDULE"].ToString().Trim() + ">";
                                }
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                    nDD = Int32.Parse(VB.Right(dt.Rows[i]["VISITRESERVEDATE"].ToString().Trim(), 2));
                    strVisit = "";
                    if (dt.Rows[i]["ROLE"].ToString().Trim() != "1" && dt.Rows[i]["VISITMANAGERNAME"].ToString().Trim() != "") strVisit += "▶";
                    if (dt.Rows[i]["VISITSTARTTIME"].ToString().Trim() != "") strVisit += dt.Rows[i]["VISITSTARTTIME"].ToString().Trim() + " ";
                    strVisit += dt.Rows[i]["LTDNAME"].ToString().Trim();

                    if (SSList.ActiveSheet.Cells[nDD - 1, nCol].Text.Trim() == "")
                    {
                        SSList.ActiveSheet.Cells[nDD - 1, nCol].Text = strVisit;
                    }
                    else
                    {
                        SSList.ActiveSheet.Cells[nDD - 1, nCol].Text += "\r\n" + strVisit;
                    }
                    strOldData = strNewData;
                }
            }
            dt.Dispose();
            dt = null;

            //자료가 없는 칼럼 표시 제외
            //SSList.ActiveSheet.ColumnCount = nCol+1;
            for (int i=nCol+1;i< SSList.ActiveSheet.ColumnCount; i++)
            {
                SSList_Sheet1.Columns[i].Width = 0;
                SSList_Sheet1.Columns[i].Visible = false;
            }

            //셀의 높이를 조정
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 4;
                SSList_Sheet1.Rows[i].Visible = true;
            }

            //일정이 없는 날 칼럼높이를 좁게
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                bOK = false;
                for (int j = 1; j < SSList.ActiveSheet.ColumnCount; j++)
                {
                    if (SSList.ActiveSheet.Cells[i, j].Text.Trim() != "") { bOK = true; break; }
                }
                if (bOK == false)
                {
                    SSList_Sheet1.Rows[i].Visible = false;
                }
            }

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Data_Search();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
