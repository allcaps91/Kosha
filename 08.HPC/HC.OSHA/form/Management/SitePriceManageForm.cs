using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class SitePriceManageForm : CommonForm
    {
        private OshaPriceService oshaPriceService;
        private bool isParent = false;
        public SitePriceManageForm()
        {
            InitializeComponent();
            oshaPriceService = new OshaPriceService();
        }
        public SitePriceManageForm(bool isParent)
        {
            InitializeComponent();
            oshaPriceService = new OshaPriceService();
            this.isParent = isParent;
        }
        private void SitePriceManageForm_Load(object sender, EventArgs e)
        {
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYear.ToString());
                nYear--;
            }
            cboYear.SelectedIndex = 0;

            txtViewName.SetExecuteButton(BtnSearch);

            SS1.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 35 });
            SS1.AddColumnText("ID", nameof(OSHA_PRICE.SITE_ID), 50, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending, Aligen= FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SS1.AddColumnText("사업장명", nameof(OSHA_PRICE.SITE_NAME), 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending , Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left});
            SS1.AddColumnText("계약일", nameof(OSHA_PRICE.CONTRACTDATE), 85, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SS1.AddColumnText("계약시작일", nameof(OSHA_PRICE.CONTRACTSTARTDATE), 94, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SS1.AddColumnText("계약종료일", nameof(OSHA_PRICE.CONTRACTENDDATE), 94, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SS1.AddColumnText("인원", nameof(OSHA_PRICE.WORKERTOTALCOUNT), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending });
            SS1.AddColumnNumber("단가", nameof(OSHA_PRICE.UNITPRICE), 90, new SpreadCellTypeOption { IsSort = true, Aligen= FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            SS1.AddColumnNumber("계약금액", nameof(OSHA_PRICE.TOTALPRICE), 90,  new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            SS1.AddColumnNumber("근로자", "", 90, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            SS1.AddColumnNumber("뇌심혈관", "", 100, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            SS1.AddColumnNumber("사후관리","" , 100, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });
            Search();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long nTotAmt = 0;
            string strYear_1 = "";

            strYear_1 = (Int32.Parse(cboYear.Text) - 1).ToString();
            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT A.OSHA_SITE_ID,A.ESTIMATEDATE,A.WORKERTOTALCOUNT,A.SITEFEE,";
                SQL = SQL + ComNum.VBLF + "      (A.WORKERTOTALCOUNT * A.SITEFEE) AS Amt, ";
                SQL = SQL + ComNum.VBLF + "      A.STARTDATE,B.NAME  ";
                SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_ESTIMATE A,HIC_LTD B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.STARTDATE>='" + cboYear.Text + "-01-01' ";
                SQL = SQL + ComNum.VBLF + "  AND A.STARTDATE<='" + cboYear.Text + "-12-31' ";
                SQL = SQL + ComNum.VBLF + "  AND (A.ISDELETED='N' OR A.ISDELETED IS NULL) ";
                SQL = SQL + ComNum.VBLF + "  AND A.SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.OSHA_SITE_ID=B.CODE(+) ";
                if (txtViewName.TextLength > 0) SQL = SQL + " AND B.Name LIKE '%" + txtViewName.Text + "%' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.NAME ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    nTotAmt = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["OSHA_SITE_ID"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ESTIMATEDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["STARTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = ""; // dt.Rows[i]["CONTRACTENDDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WORKERTOTALCOUNT"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SITEFEE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Amt"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 8].Text = Read_Result1_Count(dt.Rows[i]["OSHA_SITE_ID"].ToString()).ToString();
                        SS1_Sheet1.Cells[i, 9].Text = Read_Result2_Count(dt.Rows[i]["OSHA_SITE_ID"].ToString()).ToString();
                        SS1_Sheet1.Cells[i, 10].Text = Read_Result3_Count(dt.Rows[i]["OSHA_SITE_ID"].ToString()).ToString();
                        nTotAmt = nTotAmt + long.Parse(dt.Rows[i]["Amt"].ToString());
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            lblCount.Text = "총: " + SS1_Sheet1.RowCount + " 건, " + VB.Format(nTotAmt,"###,###,##0") + " 원";
        }

        long Read_Result1_Count(string strSiteID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long nTotCnt = 0;
            string strYear = cboYear.Text;

            SQL = "SELECT COUNT(SITEID) AS CNT1 ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_SITE_WORKER ";
            SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + strSiteID + " ";
            SQL = SQL + ComNum.VBLF + "  AND (END_DATE IS NULL OR END_DATE>'" + cboYear.Text + "-12-31') ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0) nTotCnt = long.Parse(dt.Rows[0]["CNT1"].ToString());
            dt.Dispose();
            dt = null;

            return nTotCnt;
        }

        long Read_Result2_Count(string strSiteID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long nTotCnt = 0;
            string strYear_1 = "";

            strYear_1 = (Int32.Parse(cboYear.Text) - 1).ToString();

            SQL = "SELECT COUNT(SITEID) AS CNT1 ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_LTD_RESULT2 ";
            SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + strSiteID + " ";
            SQL = SQL + ComNum.VBLF + "  AND YEAR='" + strYear_1 + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0) nTotCnt = long.Parse(dt.Rows[0]["CNT1"].ToString());
            dt.Dispose();
            dt = null;

            return nTotCnt;
        }

        long Read_Result3_Count(string strSiteID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            long nTotCnt = 0;
            string strYear_1 = "";

            strYear_1 = (Int32.Parse(cboYear.Text) - 1).ToString();

            SQL = "SELECT COUNT(SITEID) AS CNT1 ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_LTD_RESULT3 ";
            SQL = SQL + ComNum.VBLF + "WHERE SITEID=" + strSiteID + " ";
            SQL = SQL + ComNum.VBLF + "  AND YEAR='" + strYear_1 + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0) nTotCnt = long.Parse(dt.Rows[0]["CNT1"].ToString());
            dt.Dispose();
            dt = null;

            return nTotCnt;
        }
    }
}
