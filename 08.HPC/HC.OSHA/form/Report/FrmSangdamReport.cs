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
    public partial class FrmSangdamReport : Form
    {
        private HealthCheckService healthCheckService;

        public FrmSangdamReport()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            TxtLtdcode.Text = "";
            TxtName.Text = "";
            BtnSearchSite.Location = new System.Drawing.Point(345, 8);

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
            SSList.AddColumnText("코드", "", 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("사업장명", "", 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("ID", "", 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("근로자명", "", 60, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("부서","", 70, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSList.AddColumnText("성별", "", 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Center });
            SSList.AddColumnText("일자", "", 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("상담 지도 내용", "", 300, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("상담 후 건의사항","", 300, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true, Aligen = CellHorizontalAlignment.Left });
            SSList.AddColumnText("상담자", "", 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = false, Aligen = CellHorizontalAlignment.Center });
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
            string strName = TxtName.Text.Trim();

            if (cboYear.Text.Trim() == "")
            {
                ComFunc.MsgBox("작업년도를 선택하세요");
                return;
            }

            SSList.ActiveSheet.RowCount = 0;

            SQL = "SELECT A.*,B.SANGHO,C.NAME AS UserName ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_HEALTHCHECK A, HIC_LTD B,HIC_USERS C ";
            SQL = SQL + ComNum.VBLF + "WHERE A.CHARTDATE>='" + cboYear.Text + "0101' ";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE<='" + cboYear.Text + "1231' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ISDELETED='N' ";
            // 특정 회사만 검색
            if (strLtdCode!="") SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID=" + strLtdCode + " ";
            // 이름으로 검색
            if (strName != "") SQL = SQL + ComNum.VBLF + "  AND A.NAME LIKE '%" + strName + "%' ";
            SQL = SQL + ComNum.VBLF + "  AND A.SITE_ID=B.CODE(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.MODIFIEDUSER=C.USERID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SANGHO,A.SITE_ID,A.NAME,A.WORKER_ID,A.CHARTDATE "; 
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SITE_ID"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SANGHO"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["WORKER_ID"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPT"].ToString().Trim();
                    if (dt.Rows[i]["SABUN"].ToString().Trim()!="")
                    {
                        SSList.ActiveSheet.Cells[i, 4].Text += "(" + dt.Rows[i]["SABUN"].ToString().Trim() + ")";
                    }
                    SSList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["GENDER"].ToString().Trim();
                    if (dt.Rows[i]["AGE"].ToString().Trim()!="0") SSList.ActiveSheet.Cells[i, 5].Text += "(" + dt.Rows[i]["AGE"].ToString().Trim() + ")";
                    SSList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["CHARTDATE"].ToString().Substring(4, 2) + "-" + dt.Rows[i]["CHARTDATE"].ToString().Substring(6, 2);

                    string exam = string.Empty;
                    if (!dt.Rows[i]["bpl"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "혈압: " + dt.Rows[i]["bpl"].ToString().Trim() + "/" + dt.Rows[i]["bpr"].ToString().Trim() + " ";
                    }
                    if (!dt.Rows[i]["bst"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "혈당: " + dt.Rows[i]["bst"].ToString().Trim() + " ";
                    }
                    if (!dt.Rows[i]["dan"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "단백뇨: " + dt.Rows[i]["dan"].ToString().Trim() + " ";
                    }
                    if (!dt.Rows[i]["WEIGHT"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "체중: " + dt.Rows[i]["WEIGHT"].ToString().Trim() + " ";
                    }
                    if (!dt.Rows[i]["BMI"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "체지방: " + dt.Rows[i]["BMI"].ToString().Trim() + " ";
                    }
                    if (!dt.Rows[i]["ALCHOL"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "음주량: " + dt.Rows[i]["ALCHOL"].ToString().Trim() + " ";
                    }
                    if (!dt.Rows[i]["SMOKE"].ToString().Trim().IsNullOrEmpty())
                    {
                        exam += "흡연량: " + dt.Rows[i]["SMOKE"].ToString().Trim() + " ";
                    }

                    SSList.ActiveSheet.Cells[i, 7].Text = exam + "\n" + dt.Rows[i]["content"].ToString() + "\n";
                    SSList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["suggestion"].ToString();
                    SSList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["UserName"].ToString();

                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight()+10;
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYear.Text.Trim() + "년 근로자 상담 내역";
            sp.Period = "회사명: " + TxtLtdcode.Text + "          " + "근로자: " + TxtName.Text;
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\사업장별 상담내역.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
