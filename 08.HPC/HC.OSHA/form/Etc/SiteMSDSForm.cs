﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using HC.OSHA.Dto;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using ComBase.Mvc.Utils;
using System.IO;
using System.Drawing.Imaging;
using HC.Core.BaseCode.MSDS.Dto;
using HC.Core.BaseCode.MSDS.Service;
using HC.Core.Model;
using static HC.Core.Service.LogService;
using HC.Core.Service;
using ComBase.Mvc.Spread;


namespace HC_OSHA
{
    public partial class SiteMSDSForm : CommonForm
    {
        private readonly HcSiteProductService hcSiteProductService;
        private HcSiteProductMsdsService hcSiteProductMsdsService;
        private readonly HcMsdsService hcMsdsService;
        public SiteMSDSListForm SiteMSDSListForm { get; set; }

        /// <summary>
        /// 선택된 제품 
        /// </summary>
        private HC_SITE_PRODUCT SELECTED_PRODUCT = null;
        public SiteMSDSForm( )
        {
            InitializeComponent();
       
            hcSiteProductService = new HcSiteProductService();
            hcMsdsService = new HcMsdsService();
            hcSiteProductMsdsService = new HcSiteProductMsdsService();

        }
        //public override void SetSite(SiteInfo siteInfo)
        //{
        //    base.SetSite(siteInfo);

        //    LblSite.Text = selectedSite.Name;
        //    SearchProduct();
        //}

        
        private void SiteMSDSForm_Load(object sender, EventArgs e)
        {
            SpreadComboBoxData EduTypeComboBoxData = codeService.GetSpreadComboBoxData("MSDS", "OSHA");
            TxtSearchMsdsWord.SetExecuteButton(BtnSearchMsds);

            SSProduct.Initialize(new SpreadOption() { IsRowSelectColor = false,  ColumnHeaderColor = Color.AliceBlue});

            SSProduct.AddColumnText("취급공정", nameof(HC_SITE_PRODUCT.PROCESS), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSProduct.AddColumnText("제품명", nameof(HC_SITE_PRODUCT.PRODUCTNAME), 190, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false});
            SSProduct.AddColumnText("권고용도", nameof(HC_SITE_PRODUCT.USAGE), 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnText("제조사", nameof(HC_SITE_PRODUCT.MANUFACTURER), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnText("월취급량", nameof(HC_SITE_PRODUCT.MONTHLYAMOUNT), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnComboBox("단위", nameof(HC_SITE_PRODUCT.UNIT), 100, IsReadOnly.N, EduTypeComboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnDateTime("개정일자", nameof(HC_SITE_PRODUCT.REVISIONDATE), 90, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, dateTimeEditorValue = DateTimeEditorValue.String });
            SSProduct.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SSProduct_DelteButtonClick; ;
            SearchProduct();


            SSMSDSList.Initialize(new SpreadOption() { IsRowSelectColor = false});
      //      SSMSDSList.AddColumnText("ID", nameof(HC_MSDS.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
      //      SSMSDSList.AddColumnText("CHEMID", nameof(HC_MSDS.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("CasNo", nameof(HC_MSDS.CASNO), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("물질명", nameof(HC_MSDS.NAME), 170, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSMSDSList.AddColumnText("노출기준설정", nameof(HC_MSDS.EXPOSURE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("작업환경측정", nameof(HC_MSDS.WEM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("특수건강진단", nameof(HC_MSDS.SPECIALHEALTH_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("관리대상유해", nameof(HC_MSDS.MANAGETARGET_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("특별관리", nameof(HC_MSDS.SPECIALMANAGE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("허용기준설정", nameof(HC_MSDS.STANDARD_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("허가대상유해", nameof(HC_MSDS.PERMISSION_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("PSM제출대상", nameof(HC_MSDS.PSM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnButton("", 60, new SpreadCellTypeOption { ButtonText = "추가" }).ButtonClick += SSMSDSList_AddButtonClick; ; ;
       
            SSProductMsds.Initialize(new SpreadOption() { IsRowSelectColor = false });
        //    SSProductMsds.AddColumnText("PRODUCT_ID", nameof(HC_SITE_PRODUCT_MSDS_MODEL.SITE_PRODUCT_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
        //    SSProductMsds.AddColumnText("MSDS_ID", nameof(HC_SITE_PRODUCT_MSDS_MODEL.MSDS_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("CasNo", nameof(HC_SITE_PRODUCT_MSDS_MODEL.CASNO), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("물질명", nameof(HC_SITE_PRODUCT_MSDS_MODEL.NAME), 140, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSProductMsds.AddColumnText("함유량", nameof(HC_SITE_PRODUCT_MSDS_MODEL.QTY), 69, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSProductMsds.AddColumnText("노출기준설정", nameof(HC_SITE_PRODUCT_MSDS_MODEL.EXPOSURE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("작업환경측정", nameof(HC_SITE_PRODUCT_MSDS_MODEL.WEM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("특수건강진단", nameof(HC_SITE_PRODUCT_MSDS_MODEL.SPECIALHEALTH_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("관리대상유해", nameof(HC_SITE_PRODUCT_MSDS_MODEL.MANAGETARGET_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("특별관리", nameof(HC_SITE_PRODUCT_MSDS_MODEL.SPECIALMANAGE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("허용기준설정", nameof(HC_SITE_PRODUCT_MSDS_MODEL.STANDARD_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("허가대상유해", nameof(HC_SITE_PRODUCT_MSDS_MODEL.PERMISSION_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("PSM제출대상", nameof(HC_SITE_PRODUCT_MSDS_MODEL.PSM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnButton("", 50, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SSProductMsds_DeleteButtonClick;

            SSProductMsds.SetDataSource(new List<HC_SITE_PRODUCT_MSDS_MODEL>());

        }

        private void SSProductMsds_DeleteButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HC_SITE_PRODUCT_MSDS_MODEL model = SSProductMsds.GetRowData(e.Row) as HC_SITE_PRODUCT_MSDS_MODEL;

            if (model != null)
            {

                SSProductMsds.DeleteRow(e.Row);
            }
        }

        private void SSMSDSList_AddButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if(SELECTED_PRODUCT == null)
            {
                MessageUtil.Alert(" 제품을 선택하세요 ");

            }
            else
            {
                if(SELECTED_PRODUCT.ID <= 0)
                {
                    MessageUtil.Alert(" 제품을 선택하거나 추가된 제품은 저장하세요 ");
                    return;
                }
                HC_MSDS msds = SSMSDSList.GetRowData(e.Row) as HC_MSDS;

                if(IsMatch(SELECTED_PRODUCT.ID, msds.ID))
                {
                    MessageUtil.Alert(msds.NAME + "은 이미 추가된 화학물질입니다. ");
                }
                else
                {
                    SSProductMsds.AddRows();
                    HC_SITE_PRODUCT_MSDS_MODEL model = SSProductMsds.GetRowData(SSProductMsds.RowCount() - 1) as HC_SITE_PRODUCT_MSDS_MODEL;

                    model.MSDS_ID = msds.ID;
                    model.SITE_PRODUCT_ID = SELECTED_PRODUCT.ID;
                    model.NAME = msds.NAME;
                    model.CASNO = msds.CASNO;
                    model.EXPOSURE_MATERIAL = msds.EXPOSURE_MATERIAL;
                    model.WEM_MATERIAL = msds.WEM_MATERIAL;
                    model.SPECIALHEALTH_MATERIAL = msds.SPECIALHEALTH_MATERIAL;
                    model.MANAGETARGET_MATERIAL = msds.MANAGETARGET_MATERIAL;
                    model.SPECIALMANAGE_MATERIAL = msds.SPECIALMANAGE_MATERIAL;
                    model.STANDARD_MATERIAL = msds.STANDARD_MATERIAL;
                    model.PERMISSION_MATERIAL = msds.PERMISSION_MATERIAL;
                    model.PSM_MATERIAL = msds.PSM_MATERIAL;
                    model.GHS_PICTURE = msds.GHS_PICTURE;

                }
            }    
        }
        private bool IsMatch(long site_product_id, long msds_id)
        {
            for(int i=0; i< SSProductMsds.RowCount(); i++)
            {
                HC_SITE_PRODUCT_MSDS_MODEL model = SSProductMsds.GetRowData(i) as HC_SITE_PRODUCT_MSDS_MODEL;
                if( site_product_id == model.SITE_PRODUCT_ID && msds_id == model.MSDS_ID)
                {
                    return true;
                }
            }

            return false;
            
        }

        private void AddProductMsds(HC_MSDS msds)
        {
            SSProductMsds.AddRows();
            HC_SITE_PRODUCT_MSDS_MODEL model = SSProductMsds.GetRowData(SSProductMsds.RowCount()) as HC_SITE_PRODUCT_MSDS_MODEL;
            model.MSDS_ID = msds.ID;

        }

        private void SSProduct_DelteButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HC_SITE_PRODUCT dto =  SSProduct.GetRowData(e.Row) as HC_SITE_PRODUCT;
            SSProduct.DeleteRow(e.Row);
            //if(dto.RowStatus == ComBase.Mvc.RowStatus.Delete)
            //{
            //    message = dto.PRODUCTNAME + " 삭제 취소하시겠습니까?";
            //}
            //else
            //{
            //    message = dto.PRODUCTNAME + " 삭제하시겠습니까?";
            //}
            //DialogResult result = MessageUtil.Confirm(dto.PRODUCTNAME + " 삭제하시겠습니까?");
            //if (result == DialogResult.Yes)
            //{
            //    SSProduct.DeleteRow(e.Row);
            //}
        }

        private void SSProduct_ImageButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HC_SITE_PRODUCT dto = SSProduct.GetRowData(e.Row) as HC_SITE_PRODUCT;

            GHS_PictureForm form = new GHS_PictureForm();
            form.PhictureString = dto.GHSPICTURE;
            DialogResult result =  form.ShowDialog();
            if( result == DialogResult.OK)
            {
                dto.GHSPICTURE = form.PhictureString;
                dto.GhsImage = hcSiteProductService.GetProductGHSPictures(form.PhictureString);
             //   SSProduct.ActiveSheet.Cells[e.Row, 6].Value = dto.GhsImage;
            }

        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            SSProduct.AddRows();

            SSProductMsds.ActiveSheet.RowCount = 0;
            SELECTED_PRODUCT = null;
            TxtProductName.SetValue("");
        }

        private void BtnSaveProduct_Click(object sender, EventArgs e)
        {
            if (SSProduct.Validate())
            {
                IList<HC_SITE_PRODUCT> list = SSProduct.GetEditbleData<HC_SITE_PRODUCT>();
                if (list.Count > 0)
                {
                    LogService.Instance.Task(base.SelectedSite.ID, TaskName.MSDSPRODUCT);

                    if (hcSiteProductService.Save(base.SelectedSite.ID, list))
                    {
                        //MessageUtil.Info("제품을 저장하였습니다");
                        SearchProduct();
                    }
                    else
                    {
                        MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                    }
                }
                else
                {
                    MessageUtil.Info("저장할 제품이 없습니다");
                }
            }
        }

        private void SearchProduct()
        {
            if(SelectedSite == null)
            {
                MessageUtil.Alert(" 사업장 정보가 없습니다 ");
            }
            else
            {
                List<HC_SITE_PRODUCT> list = hcSiteProductService.FindAll(base.SelectedSite.ID);
                SSProduct.SetDataSource(list);
            }
            
        }

      

        private void SSProduct_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_SITE_PRODUCT dto =  SSProduct.GetRowData(e.Row) as HC_SITE_PRODUCT;
            if(dto == null)
            {
                
            }
            //panel1.SetData(dto);
            SELECTED_PRODUCT = dto;
            TxtProductName.SetValue(dto.PRODUCTNAME);

            SearchSSProductMsds();
        }
        private void SearchSSProductMsds()
        {
            SSProductMsds.SetDataSource(hcSiteProductMsdsService.FindAll(SELECTED_PRODUCT.ID));
        }
        private void BtnSearchMsds_Click(object sender, EventArgs e)
        {
            List<HC_MSDS> list = hcMsdsService.Search(RdoMsdsName.Checked, TxtSearchMsdsWord.Text);
            SSMSDSList.SetDataSource(list);
        }

        private void BtnSaveMsds_Click(object sender, EventArgs e)
        {
            //if (SSProductMsds.Validate())
            //{
            //    IList<HC_SITE_PRODUCT_MSDS_MODEL> list = SSProductMsds.GetEditbleData<HC_SITE_PRODUCT_MSDS_MODEL>();
            //    if (list.Count > 0)
            //    {
            //        if (hcSiteProductMsdsService.Save(list))
            //        {
            //            MessageUtil.Info("화학물질을 저장하였습니다");
            //        }
            //        else
            //        {
            //            MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
            //        }
            //    }
            //    else
            //    {
            //        MessageUtil.Info("저장할 데이타가 없습니다");
            //    }
            //}

            IList<HC_SITE_PRODUCT_MSDS_MODEL> list = SSProductMsds.GetEditbleData<HC_SITE_PRODUCT_MSDS_MODEL>();
            if (list.Count > 0)
            {
                if (hcSiteProductMsdsService.Save(list))
                {
                    MessageUtil.Info("화학물질을 저장하였습니다");
                }
                else
                {
                    MessageUtil.Error("트랜잭션 오류로 저장 할 수 없습니다");
                }
            }
            

            SearchSSProductMsds();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SiteMSDSForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SiteMSDSListForm.Search();
        }

        private void btnKosha_Click(object sender, EventArgs e)
        {
            MsdsForm form = new MsdsForm();
            form.Owner = this;
            form.Show();
        }
    }
}
