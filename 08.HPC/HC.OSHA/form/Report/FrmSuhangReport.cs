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
    public partial class FrmSuhangReport : Form
    {
        public FrmSuhangReport()
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

            //회사관계자 로그인
            if (clsType.User.LtdUser != "")
            {
                TxtLtdcode.Text = clsType.User.LtdUser + "." + clsType.User.JobName;
                TxtLtdcode.Enabled = false;
                BtnSearchSite.Enabled = false;
            }

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false, RowHeightAuto = true, RowHeaderVisible = true, ColumnHeaderHeight = 30 });
            SSList.AddColumnDateTime("방문일자", "", 90, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = true, IsShowCalendarButton = false });
            SSList.AddColumnText("회사명", "", 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, IsMulti = false, WordWrap = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("업무수행자", "", 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSList.AddColumnText("업무 수행 내용", "", 230, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("문제점 및 개선지도사항", "", 400, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("사업주조치", "", 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("서명", "", 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
        }

        private void BtnSearchSite_Click(object sender, EventArgs e)
        {
            SiteListForm form = new SiteListForm();

            HC_SITE_VIEW siteView = form.Search(TxtLtdcode.Text);
            if (siteView == null)
            {
                DialogResult result = form.ShowDialog();
                siteView = form.SelectedSite;
            }
            else
            {
                form.Close();
            }

            if (siteView != null)
            {
                TxtLtdcode.Text = siteView.ID.ToString() + "." + siteView.NAME;
            }

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

            SSList.ActiveSheet.RowCount = 0;

            SQL = "SELECT A.*,B.NAME LtdName ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_CARD19 A,HC_SITE_VIEW B ";
            SQL = SQL + ComNum.VBLF + "WHERE A.REGDATE >= TO_DATE('" + cboYear.Text + "-01-01', 'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "  AND A.REGDATE <= TO_DATE('" + cboYear.Text + "-12-31', 'YYYY-MM-DD') ";
            if (strLtdCode != "") SQL = SQL + ComNum.VBLF + " AND A.SITE_ID = " + strLtdCode + " ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID = B.ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.REGDATE,B.NAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Value = dt.Rows[i]["REGDATE"];
                    SSList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["LtdName"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["OPINION"].ToString().Trim();
                    if (dt.Rows[i]["STATUS"].ToString().Trim() == "0") SSList.ActiveSheet.Cells[i, 5].Text = "개선";
                    if (dt.Rows[i]["STATUS"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 5].Text = "진행";
                    if (dt.Rows[i]["STATUS"].ToString().Trim() == "2") SSList.ActiveSheet.Cells[i, 5].Text = "미개선";
                    SSList.ActiveSheet.Cells[i, 6].Text = "N";
                    if (dt.Rows[i]["SITESIGN"].ToString().Trim() != "") SSList.ActiveSheet.Cells[i, 6].Text = "Y";

                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 4;
                }
            }
            dt.Dispose();
            dt = null;

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYear.Text.Trim() + "년 위탁업무 수행일지";
            //sp.Period = "회사명: " + TxtLtdcode.Text + "          " + "근로자: " + TxtName.Text;
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\위탁업무수행일지.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
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
