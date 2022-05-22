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
    public partial class FrmMsdsReport : Form
    {
        public FrmMsdsReport()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;

            TxtLtdcode.Text = "";
            BtnSearchSite.Location = new System.Drawing.Point(254, 10);

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = true, ColumnHeaderHeight = 30 });
            SSList.AddColumnText("회사명", "", 110, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("취급공정", "", 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("제품명", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("권고용도", "", 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("제조사", "", 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("월취급량", "", 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("단위", "", 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("개정일자", "", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("물질명", "", 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("CASNO", "", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("함유량", "", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("노출", "", 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("측정", "", 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("특검", "", 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = "화학물질 MSDS 목록 현황";
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\화학물질 MSDS 목록 현황.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strLtdCode = TxtLtdcode.Text.Trim();
            if (VB.InStr(strLtdCode, ".") > 0) strLtdCode = VB.Pstr(strLtdCode, ".", 1);
            string strView = txtView.Text.Trim();

            string strMsdsName = "";
            string strMsds1 = "";
            string strMsds2 = "";
            string strMsds3 = "";

            string strOld = "";
            string strNew = "";

            SSList.ActiveSheet.RowCount = 0;

            SQL = "SELECT A.*,B.NAME AS Sangho,C.*,D.Name AS MsdsName, ";
            SQL = SQL + " D.EXPOSURE_MATERIAL,D.WEM_MATERIAL,D.SPECIALHEALTH_MATERIAL ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_SITE_PRODUCT A,HC_SITE_VIEW B, ";
            SQL = SQL + ComNum.VBLF + "      HIC_SITE_PRODUCT_MSDS C,HIC_MSDS D ";
            SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
            if (strLtdCode!="") SQL = SQL + ComNum.VBLF + " AND A.SITE_ID = '" + strLtdCode + "' ";
            if (RdoJepum.Checked == true && strView!="")
            {
                SQL = SQL + ComNum.VBLF + " AND A.PRODUCTNAME LIKE '%" + strView + "%' ";
            }
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.ID = C.SITE_PRODUCT_ID(+) ";
            if (RdoCasNo.Checked == true && strView != "")
            {
                SQL = SQL + ComNum.VBLF + " AND C.CASNO = '" + strView + "' ";
            }
            SQL = SQL + ComNum.VBLF + "  AND C.CASNO = D.CASNO(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.NAME,A.PRODUCTNAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strMsds1 = dt.Rows[i]["EXPOSURE_MATERIAL"].ToString().Trim();
                    strMsds2 = dt.Rows[i]["WEM_MATERIAL"].ToString().Trim();
                    strMsds3 = dt.Rows[i]["SPECIALHEALTH_MATERIAL"].ToString().Trim();
                    if (strMsds1 != "Y") strMsds1 = "";

                    if (VB.InStr(strMsds2, "개월") > 0) strMsds2 = VB.Pstr(strMsds2, "개월", 1);
                    if (VB.InStr(strMsds2, "작업환경측정") > 0) strMsds2 = VB.Replace(strMsds2, "작업환경측정", "");
                    if (VB.InStr(strMsds2, "대상물질") > 0) strMsds2 = VB.Replace(strMsds2, "대상물질", "");
                    if (VB.InStr(strMsds2, "(") > 0) strMsds2 = VB.Replace(strMsds2, "(", "");
                    if (VB.InStr(strMsds2, ")") > 0) strMsds2 = VB.Replace(strMsds2, ")", "");
                    strMsds2 = strMsds2.Trim();

                    if (VB.InStr(strMsds3, "개월") > 0) strMsds3 = VB.Pstr(strMsds3, "개월", 1);
                    if (VB.InStr(strMsds3, "특수건강검진") > 0) strMsds3 = VB.Replace(strMsds3, "특수건강검진", "");
                    if (VB.InStr(strMsds3, "대상물질") > 0) strMsds3 = VB.Replace(strMsds3, "대상물질", "");
                    if (VB.InStr(strMsds3, "(") > 0) strMsds3 = VB.Replace(strMsds3, "(", "");
                    if (VB.InStr(strMsds3, ")") > 0) strMsds3 = VB.Replace(strMsds3, ")", "");
                    strMsds2 = strMsds2.Trim();

                    strNew = dt.Rows[i]["Sangho"].ToString().Trim();
                    strNew += dt.Rows[i]["PROCESS"].ToString().Trim();
                    strNew += dt.Rows[i]["PRODUCTNAME"].ToString().Trim();
                    strNew += dt.Rows[i]["USAGE"].ToString().Trim();
                    strNew += dt.Rows[i]["MANUFACTURER"].ToString().Trim();
                    
                    if (strOld != strNew)
                    {
                        strOld = strNew;
                        SSList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Sangho"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["PROCESS"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["PRODUCTNAME"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["USAGE"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["MANUFACTURER"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["MONTHLYAMOUNT"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["UNIT"].ToString().Trim();
                        SSList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["REVISIONDATE"].ToString().Trim();
                    }

                    strMsdsName = dt.Rows[i]["MsdsName"].ToString().Trim();
                    if (VB.Len(strMsdsName) > 25) strMsdsName = VB.Left(strMsdsName, 23) + "...";

                    SSList.ActiveSheet.Cells[i, 8].Text = strMsdsName;
                    SSList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["CASNO"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 11].Text = strMsds1;
                    SSList.ActiveSheet.Cells[i, 12].Text = strMsds2;
                    SSList.ActiveSheet.Cells[i, 13].Text = strMsds3;

                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 4;
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = "c:\\temp\\화학물질목록현황.pdf";

            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.FORM, false);
            sp.Title = "화학물질 MSDS 목록 현황";
            sp.orientation = PrintOrientation.Landscape;
            sp.ExportPDF(fileName);

            MessageUtil.Info("Temp 폴더에 저장하였습니다");

        }
    }
}
