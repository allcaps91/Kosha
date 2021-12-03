using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.form.Etc;
using HC_OSHA.form.Visit;
using HC_OSHA.Migration;
using HC_OSHA.StatusReport;
using HC_OSHA.Visit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HC_OSHA
{
    /// <summary>
    /// 1. HIC_OSHA_SITE 의 ISPARENTCHARGE = Y 인 사업장은 하청소속이더라도 원청으로 표시된다
    /// 
    /// </summary>
    public partial class frmCharge : CommonForm
    {
        private ChargeService chargeService;
        private OshaPriceService oshaPriceService;
        public frmCharge()
        {
            InitializeComponent();
            chargeService = new ChargeService();
            oshaPriceService = new OshaPriceService();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmCharge_Load(object sender, EventArgs e)
        {
            QuaterChargeCheckBox.Enabled = false;

            if (CommonService.Instance.Session.UserId == "800594")
            {
                //BtnBuildQuater.Visible = true;
            }
            DateTime dateTime = codeService.CurrentDate;
            for (int i=0; i<=24; i++)
            {
                CboMonth.Items.Add(dateTime.AddMonths(-i).ToString("yyyy-MM"));
            }
            CboMonth.SelectedIndex = 0;

            string year = dateTime.Year.ToString();
            ChkQ1.Tag = year + "-01";
            ChkQ2.Tag = year + "-02";
            ChkQ3.Tag = year + "-03";
            ChkQ4.Tag = year + "-04";
            ChkQ5.Tag = year + "-05";
            ChkQ6.Tag = year + "-06";
            ChkQ7.Tag = year + "-07";
            ChkQ8.Tag = year + "-08";
            ChkQ9.Tag = year + "-09";
            ChkQ10.Tag = year + "-10";
            ChkQ11.Tag = year + "-11";
            ChkQ12.Tag = year + "-12";

            Search();
            //SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            //SSList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            //SSList.AddColumnText("회사코드", nameof(ChargeSiteModel.SITEID), 83, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            //SSList.AddColumnText("회사명", nameof(ChargeSiteModel.SITENAME), 190, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            //SSList.AddColumnNumber("금액", nameof(ChargeSiteModel.TOTALPRICE), 100, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("적요", nameof(ChargeSiteModel.REMARK), 190, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("발생일자", nameof(ChargeSiteModel.BDATE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("미수번호", nameof(ChargeSiteModel.WRTNO), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        private List<string> SortQuaterMonthCheckbox()
        {
            string[] yearMonthArray = new string[12];
            string[] monthArray = new string[12];
            foreach (Control control in QuaterChargeCheckBox.Controls)
            {
                CheckBox chk = control as CheckBox;
                if (chk.Checked)
                {
                    // yearMonthList.Add(chk.Tag.ToString());
                    string yearMonth = chk.Tag.ToString().Substring(5, 2);
                    if (yearMonth == "01")
                    {
                        yearMonthArray[0] = chk.Tag.ToString();
                        monthArray[1] = yearMonth;
                    }
                    else if (yearMonth == "02")
                    {
                        yearMonthArray[1] = chk.Tag.ToString();
                        monthArray[2] = yearMonth;
                    }
                    else if (yearMonth == "03")
                    {
                        yearMonthArray[2] = chk.Tag.ToString();
                        monthArray[0] = yearMonth;
                    }
                    else if (yearMonth == "04")
                    {
                        yearMonthArray[3] = chk.Tag.ToString();
                        monthArray[3] = yearMonth;
                    }
                    else if (yearMonth == "05")
                    {
                        yearMonthArray[4] = chk.Tag.ToString();
                        monthArray[4] = yearMonth;
                    }
                    else if (yearMonth == "06")
                    {
                        yearMonthArray[5] = chk.Tag.ToString();
                        monthArray[5] = yearMonth;
                    }
                    else if (yearMonth == "07")
                    {
                        yearMonthArray[6] = chk.Tag.ToString();
                        monthArray[6] = yearMonth;
                    }
                    else if (yearMonth == "08")
                    {
                        yearMonthArray[7] = chk.Tag.ToString();
                        monthArray[7] = yearMonth;
                    }
                    else if (yearMonth == "09")
                    {
                        yearMonthArray[8] = chk.Tag.ToString();
                        monthArray[8] = yearMonth;
                    }
                    else if (yearMonth == "10")
                    {
                        yearMonthArray[9] = chk.Tag.ToString();
                        monthArray[9] = yearMonth;
                    }
                    else if (yearMonth == "11")
                    {
                        yearMonthArray[10] = chk.Tag.ToString();
                        monthArray[10] = yearMonth;

                    }
                    else if (yearMonth == "12")
                    {
                        yearMonthArray[11] = chk.Tag.ToString();
                        monthArray[11] = yearMonth;
                    }
                }
            }
            List<string> list = new List<string>();
            for (int i=0; i<yearMonthArray.Length; i++)
            {
                if (yearMonthArray[i].NotEmpty())
                {
                    list.Add(yearMonthArray[i]);
                }
            }
            return list;
        }
        private void Search()
        {
            bool isQuaterCharge = ChkQuarterCharge.Checked;
            if (isQuaterCharge)
            {
                //분기 청구 합산 금액인지 1+2+3 월 합산

                List<string> yearMonthList = new List<string>();
                List<string> monthList = new List<string>();
                string[] yearMonthArray = new string[12];
                string[] monthArray = new string[12];
                foreach (Control control in QuaterChargeCheckBox.Controls)
                {
                    CheckBox chk = control as CheckBox;
                    if (chk.Checked)
                    {
                        // yearMonthList.Add(chk.Tag.ToString());
                        string yearMonth = chk.Tag.ToString().Substring(5, 2);
                        if (yearMonth == "01")
                        {
                            yearMonthArray[0] = chk.Tag.ToString();
                            monthArray[0] = yearMonth.Substring(1,1);
                        }
                        else if (yearMonth == "02")
                        {
                            yearMonthArray[1] = chk.Tag.ToString();
                            monthArray[1] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "03")
                        {
                            yearMonthArray[2] = chk.Tag.ToString();
                            monthArray[2] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "04")
                        {
                            yearMonthArray[3] = chk.Tag.ToString();
                            monthArray[3] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "05")
                        {
                            yearMonthArray[4] = chk.Tag.ToString();
                            monthArray[4] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "06")
                        {
                            yearMonthArray[5] = chk.Tag.ToString();
                            monthArray[5] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "07")
                        {
                            yearMonthArray[6] = chk.Tag.ToString();
                            monthArray[6] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "08")
                        {
                            yearMonthArray[7] = chk.Tag.ToString();
                            monthArray[7] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "09")
                        {
                            yearMonthArray[8] = chk.Tag.ToString();
                            monthArray[8] = yearMonth.Substring(1, 1);
                        }
                        else if (yearMonth == "10")
                        {
                            yearMonthArray[9] = chk.Tag.ToString();
                            monthArray[9] = yearMonth;
                        }
                        else if (yearMonth == "11")
                        {
                            yearMonthArray[10] = chk.Tag.ToString();
                            monthArray[10] = yearMonth;

                        }
                        else if (yearMonth == "12")
                        {
                            yearMonthArray[11] = chk.Tag.ToString();
                            monthArray[11] = yearMonth;
                        }
                    }
                }
              
                for (int i = 0; i < yearMonthArray.Length; i++)
                {
                    if (yearMonthArray[i].NotEmpty())
                    {
                        yearMonthList.Add(yearMonthArray[i]);
                    }
                }
                for (int i = 0; i < monthArray.Length; i++)
                {
                    if (monthArray[i].NotEmpty())
                    {
                        monthList.Add(monthArray[i]);
                    }
                }

                if (yearMonthList.Count == 0)
                {
                    MessageUtil.Alert("분기 월을 선택해주세요");
                    return;
                }
               
                List<ChargeSiteModel> parentList = new List<ChargeSiteModel>();
                List<ChargeSiteModel> childList = new List<ChargeSiteModel>();

             
                string startDate = yearMonthList[0] + "-01";
                int lastIndex = yearMonthList.Count - 1;
                string endDate = yearMonthList[lastIndex] + "-" + DateTime.DaysInMonth(yearMonthList[lastIndex].Substring(0, 4).To<int>(0), yearMonthList[lastIndex].Substring(5, 2).To<int>(0));
                
                string remark = String.Join(",", monthList);
                parentList = chargeService.ChargeRepository.FindByYear(startDate, endDate, true, true);

                foreach(ChargeSiteModel parent in parentList)
                {
                    long quaterTotalPrice = 0;
                    foreach(string yearMonth in yearMonthList)
                    {
                        string startDateQuater = startDate = yearMonth + "-01";
                        string endDateQuater = yearMonth + "-" + DateTime.DaysInMonth(yearMonth.Substring(0, 4).To<int>(0), yearMonth.Substring(5, 2).To<int>(0));
                        //월에 발생된 금액
                        long totalPrice = chargeService.ChargeRepository.GetQuarterTotalPrice(parent.SITEID, startDateQuater, endDateQuater);
                        if (totalPrice > 0)
                        {
                            quaterTotalPrice += totalPrice;
                        }
                        else
                        {
                            ///발생안된경우 계약에서 금액 가져오기
                            OSHA_PRICE lastPrice = oshaPriceService.OshaPriceRepository.FindMaxIdBySiteId(parent.SITEID);
                            quaterTotalPrice += lastPrice.TOTALPRICE;
                        }
                    }

                    parent.TOTALPRICE = quaterTotalPrice;
                }
                childList = chargeService.ChargeRepository.FindByYear(startDate, endDate, false, true);
                //foreach (string month in months)
                //{
                //    string startDate = month + "-01";
                //    string endDate = month + "-" + DateTime.DaysInMonth(month.Substring(0, 4).To<int>(0), month.Substring(5, 2).To<int>(0));
                //    string monthv = month.Substring(5, 2);
                //    if (month.Substring(0, 1) == "0")
                //    {
                //        monthv = month.Substring(1, 1);
                //    }

                //    foreach (ChargeSiteModel p in chargeService.ChargeRepository.FindByYear(startDate, endDate, true, true))
                //    {
                //        parentList.Add(p);
                //    }
                //    foreach (ChargeSiteModel c in chargeService.ChargeRepository.FindByYear(startDate, endDate, false, true))
                //    {
                //        childList.Add(c);
                //    }

                //}
                
                SetData(parentList, childList, remark, isQuaterCharge);
            }
            else
            {
                string startDate = startDate = CboMonth.GetValue() + "-01";
                string endDate = CboMonth.GetValue() + "-" + DateTime.DaysInMonth(CboMonth.Text.Substring(0, 4).To<int>(0), CboMonth.Text.Substring(5, 2).To<int>(0));
                string month = CboMonth.Text.Substring(5, 2);
                if (month.Substring(0, 1) == "0")
                {
                    month = month.Substring(1, 1);
                }

                List<ChargeSiteModel> parentList = chargeService.ChargeRepository.FindByYear(startDate, endDate, true, false);
                List<ChargeSiteModel> childList = chargeService.ChargeRepository.FindByYear(startDate, endDate, false, false);
                SetData(parentList, childList, month, isQuaterCharge);
            }
            
        }
        private void SetData(List<ChargeSiteModel> parentList, List<ChargeSiteModel> childList, string month, bool isQuaterCharge)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                //string startDate = "";
                //string endDate = "";
                //string month = "";
                //if (ChkQuarterCharge.Checked)
                //{

                //}
                //else
                //{
                //    startDate = CboMonth.GetValue() + "-01";
                //    endDate = CboMonth.GetValue() + "-" + DateTime.DaysInMonth(CboMonth.Text.Substring(0, 4).To<int>(0), CboMonth.Text.Substring(5, 2).To<int>(0));
                //    month = CboMonth.Text.Substring(5, 2);
                //    if (month.Substring(0, 1) == "0")
                //    {
                //        month = month.Substring(1, 1);
                //    }
                //}

                List<ChargeSiteModel> list = parentList.DistinctBy(r=> r.SITEID).ToList();
                //   List<ChargeSiteModel> list = chargeService.ChargeRepository.FindByYear(startDate, endDate, true, isQuaterCharge);

                ReRender(ref list, month, isQuaterCharge);


                System.Data.DataSet ds = new System.Data.DataSet();
                DataTable dt1 = null;
                DataTable dt2 = null;
                dt1 = ds.Tables.Add("list");
                dt1.Columns.AddRange(new DataColumn[] {  new DataColumn("C", typeof(bool) ),
                                                     new DataColumn("회사코드", typeof(string)),
                                                     new DataColumn("회사명", typeof(string)),
                                                     new DataColumn("금액", typeof(string)),
                                                     new DataColumn("적요", typeof(string)),
                                                     new DataColumn("발생일자", typeof(string)),
                                                     new DataColumn("미수번호", typeof(string)),
                                                     new DataColumn("담당자", typeof(string)),
                                                     new DataColumn("ESTIMATE_ID", typeof(string)),
                                });

                foreach (ChargeSiteModel parent in list)
                {
                    if (isQuaterCharge)
                    {
                        //분기 합계 금액으로
                    }
                    if (parent.TOTALPRICE <= 0)
                    {
                        MessageUtil.Alert(parent.SITENAME + " 금액이 0입니다");
                    }
                    dt1.Rows.Add(new object[] { false, parent.SITEID, parent.SITENAME, parent.TOTALPRICE, parent.REMARK, parent.BDATE, parent.WRTNO, parent.DAMNAME });
                }

          
                if (childList.Count > 0)
                {
                    dt2 = ds.Tables.Add("child");
                    dt2.Columns.AddRange(new DataColumn[] {
                                                    new DataColumn("C", typeof(bool) ),
                                                    new DataColumn("회사코드", typeof(string)),
                                                    new DataColumn("회사명", typeof(string)),
                                                    new DataColumn("금액", typeof(string)),
                                                    new DataColumn("적요", typeof(string)),
                                                    new DataColumn("발생일자", typeof(string)),
                                                    new DataColumn("미수번호", typeof(string)),
                                                    new DataColumn("담당자", typeof(string)),
                                                    new DataColumn("원청회사코드", typeof(string)),

                                });

                    //List<ChargeSiteModel> childList = chargeService.ChargeRepository.FindByYear(startDate, endDate, false, isQuaterCharge);
                    //List<ChargeSiteModel> childList = chargeService.ChargeRepository.FindByYear(startDate, endDate, false, isQuaterCharge);

                    ReRender(ref childList, month, isQuaterCharge);

                    foreach (ChargeSiteModel child in childList)
                    {
                        try
                        {
                            child.DAMNAME = GetDamName(list, child.PARENTSITE_ID);
                            if (child.PARENTSITE_ID == 43607)
                            {
                                string xx = "";

                            }
                            dt2.Rows.Add(new object[] { 
                                false, 
                                child.SITEID, 
                                child.SITENAME, 
                                child.TOTALPRICE, 
                                child.REMARK, 
                                child.BDATE, 
                                child.WRTNO, 
                                child.DAMNAME, 
                                child.PARENTSITE_ID 
                            });
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //ds.EnforceConstraints = false;
                    ds.Relations.Add("child", dt1.Columns["회사코드"], dt2.Columns["원청회사코드"]);
                }
                

                SSList.DataSource = ds;
                SSList.DataMember = "list";


                lblCount.Text = "원청: " + (list.Count) + " 건";
                lblChildCount.Text = "하청: " + (childList.Count) + " 건";
                lblTotalCount.Text = "총: " + (list.Count + childList.Count) + " 건";
                // foreach (ChargeSiteModel model in list)
                // {
                //     model.BDATE = bDate.ToString("yyyy-MM-dd");
                // }
                //// SSList.SetDataSource(list);

                // string month = CboMonth.Text.Substring(5, 2);
                // if(month.Substring(0,1) == "0")
                // {
                //     month = month.Substring(1, 1);
                // }
                // string remark = "보건관리 위탁수수료(" + month + "월)";
                // for (int i=0; i < list.Count; i++)
                // {
                //     long wrtNo = SSList.ActiveSheet.Cells[i, 6].Value.To<long>(0);

                //     if (wrtNo>0)
                //     {
                //         SSList.ActiveSheet.Cells[i, 6].Row.BackColor = Color.Red;
                //     }
                //     else
                //     {
                //         SSList.ActiveSheet.Cells[i, 4].Text = remark;
                //     }
                // }
                NumberCellType numberCellType = new NumberCellType();
                numberCellType.Separator = ",";
                numberCellType.DecimalPlaces = 0;
                numberCellType.ShowSeparator = true;
                //   numberCellType.DecimalPlaces = option.DecimalPlaces;

                SSList.ActiveSheet.Columns[0].Width = 30; SSList.ActiveSheet.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                SSList.ActiveSheet.Columns[1].Width = 83; SSList.ActiveSheet.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;// 회사코드
                SSList.ActiveSheet.Columns[2].Width = 190; SSList.ActiveSheet.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Left; //사업장명
                SSList.ActiveSheet.Columns[3].Width = 100; SSList.ActiveSheet.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center; //금액
                SSList.ActiveSheet.Columns[3].CellType = numberCellType;
                SSList.ActiveSheet.Columns[4].Width = 190; SSList.ActiveSheet.Columns[4].HorizontalAlignment = CellHorizontalAlignment.Center;
                SSList.ActiveSheet.Columns[5].Width = 100; SSList.ActiveSheet.Columns[5].HorizontalAlignment = CellHorizontalAlignment.Center;
                SSList.ActiveSheet.Columns[6].Width = 100; SSList.ActiveSheet.Columns[6].HorizontalAlignment = CellHorizontalAlignment.Center;

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    long siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                    long wrtNo = SSList.ActiveSheet.Cells[i, 6].Value.To<long>(0);
                    if (wrtNo > 0)
                    {
                        SSList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(237, 211, 237);
                    }
                    else
                    {
                        SSList.ActiveSheet.Rows[i].BackColor = Color.White;
                    }
                    int childRelrationCount = SSList.ActiveSheet.ChildRelationCount;
                    if (childRelrationCount > 0)
                    {
                        SheetView childView = SSList.ActiveSheet.GetChildView(i, 0);
                        if (childView.RowCount > 0)
                        {
                            childView.Columns[0].Width = 30; childView.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                            childView.Columns[1].Width = 83; childView.Columns[1].HorizontalAlignment = CellHorizontalAlignment.Center;// 회사코드
                            childView.Columns[2].Width = 190; childView.Columns[2].HorizontalAlignment = CellHorizontalAlignment.Left;//사업장명
                            childView.Columns[3].Width = 100; childView.Columns[3].HorizontalAlignment = CellHorizontalAlignment.Center;//금액
                            childView.Columns[3].CellType = numberCellType;
                            childView.Columns[4].Width = 190; childView.Columns[4].HorizontalAlignment = CellHorizontalAlignment.Center;
                            childView.Columns[5].Width = 100; childView.Columns[5].HorizontalAlignment = CellHorizontalAlignment.Center;
                            childView.Columns[6].Width = 100; childView.Columns[6].HorizontalAlignment = CellHorizontalAlignment.Center;
                            //childView.Columns[7].Visible = false; // 부모회사코드
                            childView.SetRowExpandable(0, true);
                            for (int j = 0; j < childView.RowCount; j++)
                            {
                                wrtNo = childView.Cells[j, 6].Value.To<long>(0);
                                if (wrtNo > 0)
                                {
                                    childView.Rows[j].BackColor = Color.FromArgb(237, 211, 237);
                                }
                                else
                                {
                                    childView.Rows[j].BackColor = Color.White;
                                }
                            }

                        }
                        else
                        {
                            SSList.ActiveSheet.SetRowExpandable(i, false);
                        }
                    }
              
                    SSList.ActiveSheet.ExpandRow(i, true);

                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
                MessageUtil.Alert(ex.Message + " 오류가 발생했습니다");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            
            }
        
           
        }
        private string GetDamName(List<ChargeSiteModel> list, long parentId)
        {
            string damName = "";
            foreach(ChargeSiteModel model in list)
            {
                if(model.SITEID == parentId)
                {
                    damName =  model.DAMNAME;
                    break;
                }
            }
            return damName;
        }
        private void ReRender(ref List<ChargeSiteModel> list, string month, bool isQuarter)
        {
            DateTime bDate = DtpBdate.Value;
            string remark = string.Empty;
            remark = "보건관리 위탁수수료(" + month + "월)";

            foreach (ChargeSiteModel model in list)
            {
                try
                {
                    if (model.WRTNO == 0)
                    {
                        model.BDATE = bDate.ToString("yyyy-MM-dd");
                    }
                    if (model.REMARK.IsNullOrEmpty())
                    {
                        model.REMARK = remark;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }


        /// <summary>
        /// 수수료가 발생되지 않은 사업장은 선청구 수수료를 발생시킨다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DateTime bDate = DtpBdate.Value;
                string year = CboMonth.Text.Substring(0, 4);
                string month = CboMonth.Text.Substring(5, 2);
                //if(month.Substring(0,1) == "0")
                //{
                //    month = month.Substring(1, 1);
                //}
                string pummok = "보건관리 위탁수수료(" + month + "월)";
                List<HC_MISU_MST> list = new List<HC_MISU_MST>();
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (SSList.ActiveSheet.Cells[i, 0].Value == null)
                    {
                        continue;
                    }

                    if (SSList.ActiveSheet.Cells[i, 0].Value.Equals(true))
                    {
                        long wrtNo = SSList.ActiveSheet.Cells[i, 6].Value.To<long>(0);
                        if (wrtNo == 0)
                        {
                            ChargeSiteModel model = new ChargeSiteModel();
                            model.SITEID = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                            model.TOTALPRICE = SSList.ActiveSheet.Cells[i, 3].Value.To<long>(0);
                            model.REMARK = SSList.ActiveSheet.Cells[i, 4].Text;
                            model.DAMNAME = SSList.ActiveSheet.Cells[i, 7].Text;
                            list.Add(CreateDto(model, bDate, year, month, pummok));
                        }

                    }
                    int childRelrationCount = SSList.ActiveSheet.ChildRelationCount;
                    if (childRelrationCount > 0)
                    {
                        SheetView childView = SSList.ActiveSheet.GetChildView(i, 0);
                        if (childView.RowCount > 0)
                        {
                            for (int j = 0; j < childView.RowCount; j++)
                            {
                                if (childView.Cells[j, 0].Value == null)
                                {
                                    continue;
                                }
                                if (childView.Cells[j, 0].Value.Equals(true))
                                {
                                    long wrtNo = childView.Cells[j, 6].Value.To<long>(0);
                                    if (wrtNo == 0)
                                    {
                                        ChargeSiteModel model = new ChargeSiteModel();
                                        model.SITEID = childView.Cells[j, 1].Value.To<long>(0);
                                        model.TOTALPRICE = childView.Cells[j, 3].Value.To<long>(0);
                                        model.REMARK = childView.Cells[j, 4].Text;
                                        model.DAMNAME = childView.Cells[j, 7].Text;
                                        list.Add(CreateDto(model, bDate, year, month, pummok));
                                    }

                                }
                            }

                        }
                    }

                }//for


                //wrtno 없는(청구안된놈들만)
                if (list.Count > 0)
                {
                    bool result = chargeService.Save(list);
                    if (result)
                    {
                        MessageUtil.Info("미수 형성 완료");
                        
                    }
                    else
                    {
                        MessageUtil.Alert("미수 형성 오류");
                    }
                }
                else
                {
                    MessageUtil.Alert("미수 형성할 사업장이 없습니다.");
                }
            }
            catch(Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageUtil.Alert(ex.Message);
                Log.Error(ex);
            }
            finally
            {
                Search();
                Cursor.Current = Cursors.Default;
            }
        
        }
        private HC_MISU_MST CreateDto(ChargeSiteModel model, DateTime bDate, string year, string month, string pummok)
        {
            HC_MISU_MST dto = new HC_MISU_MST();
            dto.LTDCODE = model.SITEID;
            dto.MISUJONG = "1";
            dto.BDATE = bDate;
            dto.GJONG = "82";
            dto.GBEND = "N";
            dto.MISUGBN = "1";
            dto.MISUAMT = model.TOTALPRICE;
            dto.IPGUMAMT = 0;
            dto.GAMAMT = 0;
            dto.SAKAMT = 0;
            dto.BANAMT = 0;
            dto.JANAMT = model.TOTALPRICE;
        //    long cno = chargeService.ChargeRepository.FindMaxCno(year + "-" + "01-01", year + "-" + "12-31");
          //  dto.GIRONO = (cno + 1).ToString() ;
          //  dto.CNO = cno + 1;
            dto.DANNAME = model.DAMNAME;
            dto.REMARK = model.REMARK;
            dto.MIRGBN = "1";
            dto.MIRCHAAMT = 0;
            dto.GBMISUBUILD = "Y";
            dto.YYMM_JIN = year + month;
            dto.PUMMOK = model.REMARK;//pummok;
      
            dto.ENTSABUN = CommonService.Instance.Session.UserId.To<long>(0);

            return dto;
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            List<HC_MISU_MST> list = new List<HC_MISU_MST>();
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                if (SSList.ActiveSheet.Cells[i, 0].Value == null)
                {
                    continue;
                }
                if (SSList.ActiveSheet.Cells[i, 0].Value.Equals(true))
                {
                    long wrtNo = SSList.ActiveSheet.Cells[i, 6].Value.To<long>(0);
                    if (wrtNo > 0)
                    {
                        HC_MISU_MST dto = new HC_MISU_MST();
                        dto.WRTNO = wrtNo;
                        dto.BDATE= DateTime.Parse(SSList.ActiveSheet.Cells[i, 5].Text); // 스프레드 발생일자로 변경해야함
                        dto.LTDCODE = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                        list.Add(dto);
                    }                    
                }
                int childRelrationCount = SSList.ActiveSheet.ChildRelationCount;
                if (childRelrationCount > 0)
                {
                    SheetView childView = SSList.ActiveSheet.GetChildView(i, 0);
                    if (childView.RowCount > 0)
                    {
                        for (int j = 0; j < childView.RowCount; j++)
                        {
                            if (childView.Cells[j, 0].Value == null)
                            {
                                continue;
                            }
                            if (childView.Cells[j, 0].Value.Equals(true))
                            {
                                long wrtNo = childView.Cells[j, 6].Value.To<long>(0);
                                if (wrtNo > 0)
                                {
                                    HC_MISU_MST dto = new HC_MISU_MST();
                                    dto.WRTNO = wrtNo;
                                    dto.LTDCODE = childView.Cells[j, 1].Value.To<long>(0);
                                    dto.BDATE = DateTime.Parse(childView.Cells[j, 5].Text);
                                    list.Add(dto);
                                }
                            }
                        }

                    }
                }
               
            }//for

            if (list.Count > 0)
            {
                if(MessageUtil.Confirm("선택된 미수자료를 삭제하시겠습니까?") == DialogResult.Yes)
                {
                    chargeService.Delete(list);
                    MessageUtil.Info("미수 삭제 완료");
                    Search();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chargeService.ChargeRepository.ChageTable();
        }

        private void SSList_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == false)
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

                    int childRelrationCount = SSList.ActiveSheet.ChildRelationCount;
                    if (childRelrationCount > 0)
                    {
                        SheetView childView = SSList.ActiveSheet.GetChildView(i, 0);
                        if (childView.RowCount > 0)
                        {
                            for (int j = 0; j < childView.RowCount; j++)
                            {
                                childView.Cells[j, 0].Value = false;
                            }
                        }
                    }
                        
                }

                SSList.Sheets[0].ColumnHeader.Cells[0, 0].Value = false;
            }
            else
            {
                for (int i = 0; i < SSList.Sheets[0].RowCount; i++)
                {
                    if(SSList.Sheets[0].Cells[i, 6].Value.To<long>(0)<= 0)
                    {
                        SSList.Sheets[0].Cells[i, 0].Value = true;
                    }

                    int childRelrationCount = SSList.ActiveSheet.ChildRelationCount;
                    if (childRelrationCount > 0)
                    {
                        SheetView childView = SSList.ActiveSheet.GetChildView(i, 0);
                        if (childView.RowCount > 0)
                        {
                            for (int j = 0; j < childView.RowCount; j++)
                            {
                                if (SSList.Sheets[0].Cells[i, 6].Value.To<long>(0) <= 0)
                                {
                                    childView.Cells[j, 0].Value = true;
                                }

                            }
                        }

                    }
            
                }//for
                SSList.Sheets[0].ColumnHeader.Cells[0, 0].Value = true;
            }
        }

        private void SSSiteList_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //List<HC_OSHA_SITE_MODEL> siteList = SSSiteList.GetEditbleData<HC_OSHA_SITE_MODEL>();


            //List<HC_OSHA_SITE_MODEL> list = oshaSiteLastTree1.GetSiteList();
            //foreach (HC_OSHA_SITE_MODEL model in list)
            //{
            //    model.RowStatus = ComBase.Mvc.RowStatus.Insert;

            //    siteList.Add(model);
            //}

            //SSSiteList.SetDataSource(new List<HC_OSHA_SITE_MODEL>());

            //SSSiteList.SetDataSource(siteList);
        }

        private void ChkQuarterCharge_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkQuarterCharge.Checked)
            {
                QuaterChargeCheckBox.Enabled = true;
            }
            else
            {
                QuaterChargeCheckBox.Enabled = false;
                foreach (Control control in QuaterChargeCheckBox.Controls)
                {
                    CheckBox chk = control as CheckBox;
                    chk.Checked = false;
                }
            }

            //Search();
        }
    }
}
