using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Exceptions;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using FarPoint.Win.Spread.CellType;
using HC.Core.Common.Interface;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.form.Etc;
using HC_OSHA.form.Visit;
using HC_OSHA.StatusReport;
using HC_OSHA.Visit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace HC_OSHA
{
    /// <summary>
    /// 미수 빌드
    /// </summary>
    public partial class OshaChargeForm : CommonForm
    {
        HcOshaChargeService hcOshaChargeService;
        public OshaChargeForm()
        {
            InitializeComponent();
            hcOshaChargeService = new HcOshaChargeService();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void OshaChargeForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 24; i++)
            {
                CboMonth.Items.Add(dateTime.AddMonths(-i).ToString("yyyy-MM"));
            }
            CboMonth.SelectedIndex = 0;



            //System.Data.DataSet ds = new System.Data.DataSet();
            //DataTable name;

            //name = ds.Tables.Add("Customers");
            //name.Columns.AddRange(new DataColumn[] {new DataColumn("LastName", typeof(string)), new DataColumn("FirstName", typeof(string)), new DataColumn("ID", typeof(Int32))});

            //name.Rows.Add(new object[] { "Fielding", "William", 0 });
            //name.Rows.Add(new object[] { "Williams", "Arthur", 1 });
            //name.Rows.Add(new object[] { "Zuchini", "Theodore", 2 });

            //DataTable city;
            //city = ds.Tables.Add("City/State");
            //city.Columns.AddRange(new DataColumn[] {new DataColumn("City", typeof(string)), new DataColumn("Owner", typeof(Int32)), new DataColumn("State", typeof(string))});
            //city.Rows.Add(new object[] { "Atlanta", 0, "Georgia" });
            //city.Rows.Add(new object[] { "Boston", 1, "Mass." });
            //city.Rows.Add(new object[] { "Tampa", 2, "Fla." });

            //ds.Relations.Add("City/State", name.Columns["ID"], city.Columns["Owner"]);

            // SSList.DataSource = ds;
            // SSList.DataMember = "Customers";



            //SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
        

            //SSList.AddColumnText("회사코드", nameof(CHARGE_MODEL.SITE_ID), 83, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            //SSList.AddColumnText("회사명", nameof(CHARGE_MODEL.SITE_NAME), 190, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            //SSList.AddColumnText("방문예정일", nameof(CHARGE_MODEL.VISITRESERVEDATE), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            //SSList.AddColumnText("방문일", nameof(CHARGE_MODEL.VISITDATETIME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("방문자", nameof(CHARGE_MODEL.VISITUSERNAME), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("인원", nameof(CHARGE_MODEL.WORKERCOUNT), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnNumber("단가", nameof(CHARGE_MODEL.UNITPRICE), 100, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnNumber("발생금액", nameof(CHARGE_MODEL.TOTALPRICE), 100, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("선청구여부", nameof(CHARGE_MODEL.ISPRECHARGE), 54, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            //SSList.AddColumnText("미수번호", nameof(CHARGE_MODEL.WRTNO), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("청구일", nameof(CHARGE_MODEL.CHARGEDATE), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("인원", nameof(CHARGE_MODEL.E_WORKERCOUNT), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnNumber("단가", nameof(CHARGE_MODEL.E_UNITPRICE), 100, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnNumber("청구금액", nameof(CHARGE_MODEL.E_TOTALPRICE), 100, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("참고사항", nameof(CHARGE_MODEL.REMARK), 200, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, IsMulti = true, WordWrap = true });
            //SSList.AddColumnText("비고", nameof(CHARGE_MODEL.REMARK), 190, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            ////   SSList.AddColumnText("발생일자", nameof(CHARGE_MODEL.BDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            ////   SSList.AddColumnText("미수번호", nameof(CHARGE_MODEL.WRTNO), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //// SSList.Sheets.Add(new CustomSheet());
        }

        private void Search()
        {
            DateTime bDate = DtpBdate.Value;
            string startDate = CboMonth.GetValue() + "-01";
            string endDate = CboMonth.GetValue() + "-" + DateTime.DaysInMonth(CboMonth.Text.Substring(0, 4).To<int>(0), CboMonth.Text.Substring(5, 2).To<int>(0));

            List<CHARGE_MODEL> list = hcOshaChargeService.FindSite(startDate, endDate);

            System.Data.DataSet ds = new System.Data.DataSet();
            DataTable dt1 = null;
            DataTable dt2 = null;
            dt1 = ds.Tables.Add("list");
            dt1.Columns.AddRange(new DataColumn[] {  new DataColumn("C", typeof(bool) ),
                                                     new DataColumn("회사코드", typeof(string)), 
                                                     new DataColumn("회사명", typeof(string))
                                });
            

            dt2 = ds.Tables.Add("child");
            dt2.Columns.AddRange(new DataColumn[] {
                                                    new DataColumn("C", typeof(bool) ),
                                                    new DataColumn("회사코드", typeof(string)),
                                                    new DataColumn("회사명", typeof(string)),
                                                    new DataColumn("부모회사코드", typeof(string))
                                });
   

            foreach (CHARGE_MODEL model in list)
            {
                dt1.Rows.Add(new object[] {false, model.SITE_ID, model.SITE_NAME });

            }
            foreach (CHARGE_MODEL model in list)
            {
                Log.Debug("model.SITE_ID:" + model.SITE_ID);
                //사업장별 내역
                List <CHARGE_MODEL> childList = hcOshaChargeService.FindAll(model.SITE_ID, startDate, endDate);

                foreach (CHARGE_MODEL child in childList)
                {
                    Log.Debug("--->child.SITE_ID:" + child.SITE_ID);
                    dt2.Rows.Add(new object[] { false, child.SITE_ID, child.SITE_NAME, child.PARENT_SITE_ID });
                }

            }
            ds.Relations.Add("child", dt1.Columns["회사코드"], dt2.Columns["부모회사코드"]);

            SSList.DataSource = ds;
            SSList.DataMember = "list";


            IList<CHARGE_MODEL> tmp = new List<CHARGE_MODEL>();

            
            long visitTotalPrice = 0;
            long chargeTotalPrice = 0;
            foreach (CHARGE_MODEL model in list)
            {
                visitTotalPrice += model.TOTALPRICE;
                chargeTotalPrice += model.E_TOTALPRICE;
                // model.BDATE = bDate.ToString("yyyy-MM-dd");
            }
          //  SSList.SetDataSource(list);

            TxtVisitTotalPrice.Text = string.Format("{0:#,0}", visitTotalPrice);
            TxtChargeTotalPrice.Text = string.Format("{0:#,0}", chargeTotalPrice);
         
            int xxxx = SSList.ActiveSheet.GetChildSheets().Count;


            //            object value = SSList.ActiveSheet.Expandable = true;

            SSList.ActiveSheet.Columns[1].Width = 83; // 회사코드
            SSList.ActiveSheet.Columns[2].Width = 190;

            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
     //           object xx = SSList.ActiveSheet.GetChildView(i, 0).Cells[i, 1].Value;
               
                SSList.ActiveSheet.GetChildView(i, 0).Columns[1].Width = 83;
                SSList.ActiveSheet.GetChildView(i, 0).Columns[2].Width = 190;


            }
            
            for (int i = 0; i < SSList.RowCount(); i++)
            {
                 SSList.ActiveSheet.ExpandRow(i, true);

            }

        }

        /// <summary>
        /// 수수료발생 및 미수 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuild_Click(object sender, EventArgs e)
        {
            List<CHARGE_MODEL> list = new List<CHARGE_MODEL>();
            for (int i = 0; i < SSList.RowCount(); i++)
            {
                CheckBoxCellType c = SSList.ActiveSheet.Cells[i, 0].Column.CellType as CheckBoxCellType;
                object value = SSList.ActiveSheet.Cells[i, 0].Value;
                if (value != null)
                {
                    if (bool.Parse(value.ToString()) == true)
                    {
                        CHARGE_MODEL model = SSList.GetRowData(i) as CHARGE_MODEL;
                        list.Add(model);
                    }
                }           
            }//for

            if (list.Count > 0)
            {
                try
                {
                    if (hcOshaChargeService.Build(list, DtpBdate.Value, CboMonth.Text))
                    {
                        MessageUtil.Info("미수 빌드 완료");
                        Search();
                    }
                    else
                    {
                        MessageUtil.Info("미수 빌드 실패");
                    }

                }
                catch(MTSException ex)
                {
                    MessageUtil.Info("미수 빌드 실패 \n" + ex.Message);
                }

            }
            else
            {
                MessageUtil.Alert(" 미수 형성할 사업장을 선택하세요");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
          
            List<CHARGE_MODEL> list = new List<CHARGE_MODEL>();
            for (int i = 0; i < SSList.RowCount(); i++)
            {
                CheckBoxCellType c = SSList.ActiveSheet.Cells[i, 0].Column.CellType as CheckBoxCellType;
                object value = SSList.ActiveSheet.Cells[i, 0].Value;
                if (value != null)
                {
                    if (bool.Parse(value.ToString()) == true)
                    {
                        CHARGE_MODEL model = SSList.GetRowData(i) as CHARGE_MODEL;
                        if (model.ID > 0 && model.WRTNO>0)
                        {
                            list.Add(model);
                        }
                        
                    }
                }
            }//for
            if (list.Count > 0)
            {
                hcOshaChargeService.Delete(list);
                Search();
            }
            else
            {
                MessageUtil.Alert("삭제할 미수를 선택하세요");
            }
        }

        private void SSList_ChildViewCreated(object sender, FarPoint.Win.Spread.ChildViewCreatedEventArgs e)
        {
            string x = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.ColumnHeader == false)
            {
                return;
            }
            if (e.Column != 0)
            {
                return;
            }
            bool allChecked = false;
            if (SSList.Sheets[0].ColumnHeader.Cells[0, 0].Value == null || Convert.ToBoolean(SSList.Sheets[0].ColumnHeader.Cells[0, 0].Value) == false)
            {
                allChecked = false;
            }
            else
            {
                allChecked = true;
            }

            if (allChecked == true)
            {
                for (int i = 0; i < SSList.Sheets[0].RowCount; i++)
                {
                    SSList.Sheets[0].Cells[i, 0].Value = false;
                }

                SSList.Sheets[0].ColumnHeader.Cells[0, 0].Value = false;
            }
            else
            {
                for (int i = 0; i < SSList.Sheets[0].RowCount; i++)
                {
                    SSList.Sheets[0].Cells[i, 0].Value = true;
                }
                SSList.Sheets[0].ColumnHeader.Cells[0, 0].Value = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < SSList.RowCount(); i++)
            {
                object value =  SSList.ActiveSheet.Cells[i, 1].Value;
                object xx = SSList.ActiveSheet.GetChildView(i, 0).Cells[i, 1].Value;

            }
        }
    }

}
