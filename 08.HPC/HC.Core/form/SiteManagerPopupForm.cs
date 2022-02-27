using ComBase;
using ComDbB;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using HC.Core.Dto;
using HC.Core.Service;
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
    public partial class SiteManagerPopupForm : CommonForm
    {
        private long siteId = 0;
        private HcSiteWorkerService hcSiteWorkerService;

        private string emailList = "";

        public SiteManagerPopupForm()
        {
            InitializeComponent();
            hcSiteWorkerService = new HcSiteWorkerService();
        }

        /// 선택된 이메일 목록 가져오기
        public string GetWorker()
        {
            return emailList;
        }

        public void set_siteId(long siteId)
        {
            this.siteId = siteId;
        }

        private void Search()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SSWorkerList_Sheet1.RowCount = 0;
            SSWorkerList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            try
            {
                SQL = "SELECT * FROM HIC_OSHA_CONTRACT_MANAGER ";
                SQL = SQL + "WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + "  AND EMAIL IS NOT NULL ";
                SQL = SQL + "  AND ESTIMATE_ID IN (SELECT MAX(ID) AS ID FROM HIC_OSHA_ESTIMATE ";
                SQL = SQL + "      WHERE OSHA_SITE_ID=" + siteId + " ";
                SQL = SQL + "        AND ISDELETED='N' ";
                SQL = SQL + "        AND SWLicense='" + clsType.HosInfo.SwLicense + "') ";
                SQL = SQL + "ORDER BY NAME ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SSWorkerList_Sheet1.RowCount = dt.Rows.Count;
                    SSWorkerList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SSWorkerList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SSWorkerList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["EMAIL"].ToString().Trim();
                        SSWorkerList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WORKER_ROLE"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            
        }

        private void SetSelect()
        {
            emailList = "";
            for (int i = 0; i < SSWorkerList.RowCount(); i++)
            {
                if (Convert.ToBoolean(SSWorkerList.ActiveSheet.Cells[i, 0].Value) == true)
                {
                    if (emailList=="")
                    {
                        emailList = SSWorkerList.ActiveSheet.Cells[i, 2].Text.Trim();
                    }
                    else
                    {
                        emailList += "," + SSWorkerList.ActiveSheet.Cells[i, 2].Text.Trim();
                    }
                }
            }
            this.Close();
        }

        private void SiteManagerPopupForm_Load(object sender, EventArgs e)
        {
            //SpreadComboBoxData comboBoxData = codeService.GetSpreadComboBoxData("WORKER_ROLE", "OSHA");
            SSWorkerList.Initialize(new SpreadOption() { IsRowSelectColor = true });

            SSWorkerList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSWorkerList.AddColumnText("이름", "", 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("이메일", "", 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSWorkerList.AddColumnComboBox("직책", "", 150, IsReadOnly.Y, comboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSWorkerList.AddColumnText("직책", "", 150, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            Search();

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void BtnConfirm_Click_1(object sender, EventArgs e)
        {
            SetSelect();
        }
    }
}
