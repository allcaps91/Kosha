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
    public partial class FrmEHPReport : Form
    {
        public FrmEHPReport()
        {
            InitializeComponent();
            Screen_Set();
        }
        private void Screen_Set()
        {
            int i = 0;
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            TxtLtdcode.Text = "";
            BtnSearchSite.Location = new System.Drawing.Point(346, 8);
            if (cboYear.Items.Count == 0)
            {
                for (i = 0; i < 5; i++)
                {
                    cboYear.Items.Add(nYear.ToString());
                    nYear--;
                }
            }
            cboYear.SelectedIndex = 0;

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = true, ColumnHeaderHeight = 30 });
            SSList.AddColumnText("등록일자", "", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSList.AddColumnText("회사명", "", 160, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("기업건강증진지수", "", 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("근로자수", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("장년(50↑)", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("장시간", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("교대", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("근골부담", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("고객응대", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("근골질환", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("뇌심질환", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("미수검", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("유질환", "", 66, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
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

            if (cboYear.Text.Trim() == "")
            {
                ComFunc.MsgBox("작업년도를 선택하세요");
                return;
            }

            SSList_Sheet1.RowCount = 0;

            SQL = "SELECT A.*,B.NAME AS Sangho ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_EHP A,HC_SITE_VIEW B ";
            SQL = SQL + ComNum.VBLF + "WHERE A.WDATE >='" + cboYear.Text + "-01-01' ";
            SQL = SQL + ComNum.VBLF + "  AND A.WDATE<='" + cboYear.Text + "-12-31' ";
            if (strLtdCode != "") SQL = SQL + ComNum.VBLF + " AND A.SITE_ID = " + strLtdCode + " ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.WDATE,B.NAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList_Sheet1.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WDATE"].ToString().Substring(0, 10);
                    SSList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sangho"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JEMSU"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CNT01"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["CNT02"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CNT03"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["CNT04"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["CNT05"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["CNT06"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["CNT07"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["CNT08"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["CNT09"].ToString().Trim();
                    SSList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["CNT10"].ToString().Trim();

                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 4;
                }
            }
            dt.Dispose();
            dt = null;

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYear.Text.Trim() + "년 기업건강증진지수(EHP) 대장";
            //sp.Period = "회사명: " + TxtLtdcode.Text + "          " + "근로자: " + TxtName.Text;
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\기업건강증진지수(EHP)대장.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }

        }
    }
}
