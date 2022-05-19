using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase; //기본 클래스
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
using FarPoint.Win.Spread;
using System.Threading;

namespace HC_OSHA
{
    public partial class SiteMSDSJepum : CommonForm
    {
        private readonly HcMsdsProductService hcMsdsProductService;
        private readonly HcMsdsService hcMsdsService;
        private KoshaMsdsService koshaMsdsService;

        /// <summary>
        /// 선택된 제품 
        /// </summary>
        private HC_MSDS_PRODUCT SELECTED_PRODUCT = null;
        public SiteMSDSJepum( )
        {
            InitializeComponent();
       
            hcMsdsProductService = new HcMsdsProductService();
            hcMsdsService = new HcMsdsService();
            koshaMsdsService = new KoshaMsdsService();
        }

        private void SiteMSDSJepum_Load(object sender, EventArgs e)
        {
            TxtSearchMsdsWord.SetExecuteButton(BtnSearchMsds);

            SSProduct.Initialize(new SpreadOption() { IsRowSelectColor = false,  ColumnHeaderColor = Color.AliceBlue});

            SSProduct.AddColumnText("ID", nameof(HC_MSDS_PRODUCT.ID), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnText("제품명 및 규격", nameof(HC_MSDS_PRODUCT.PRODUCTNAME), 300, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSProduct.AddColumnText("용도", nameof(HC_MSDS_PRODUCT.USAGE), 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnText("제조사", nameof(HC_MSDS_PRODUCT.MANUFACTURER), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSProduct.AddColumnText("MSDS코드", nameof(HC_MSDS_PRODUCT.MSDSCODE), 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnText("MSDS목록", nameof(HC_MSDS_PRODUCT.MSDSLIST), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SSProduct_DelteButtonClick; 
            SearchProduct();

            SSMSDSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSMSDSList.AddColumnText("CHEMID", nameof(KoshaMsds.ChemId), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("물질명", nameof(KoshaMsds.ChemNameKor), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSMSDSList.AddColumnText("CasNo", nameof(KoshaMsds.CasNo), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSProductMsds.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSProductMsds.AddColumnCheckBox("삭제", "", 70, new CheckBoxBooleanCellType());
            SSProductMsds.AddColumnText("CHEMID","", 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("CasNo","", 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true });
            SSProductMsds.AddColumnText("물질명","", 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSProductMsds.AddColumnText("함유량","", 90, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, sortIndicator = SortIndicator.Ascending });
            SSProductMsds.AddColumnText("노출기준설정", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("작업환경측정", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("특수건강진단", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("관리대상유해", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("특별관리", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("허용기준설정", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("허가대상유해", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("PSM제출대상", "", 46, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
        }

        private bool IsMatch(string msds_CasNo)
        {
            string strCasNo = "";
            for (int i = 0; i < SSProductMsds.RowCount(); i++)
            {
                strCasNo = SSProductMsds_Sheet1.Cells[i, 2].Text.ToString().Trim();
                if (strCasNo == msds_CasNo)
                {
                    return true;
                }
            }
            return false;
        }

        private void SSProduct_DelteButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            long nID = long.Parse(SSProduct_Sheet1.Cells[e.Row, 0].Text.ToString().Trim());
            if (nID > 0)
            {
                if (ComFunc.MsgBoxQ("정말로 삭제를 하시겠습니까?", "삭제여부", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    hcMsdsProductService.Delete(nID);
                    SearchProduct();
                }
            }
        }

        private void SSProduct_ImageButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //HC_MSDS_PRODUCT dto = SSProduct.GetRowData(e.Row) as HC_MSDS_PRODUCT;

            //GHS_PictureForm form = new GHS_PictureForm();
            //form.PhictureString = dto.GHSPICTURE;
            //DialogResult result =  form.ShowDialog();
            //if( result == DialogResult.OK)
            //{
            //    dto.GHSPICTURE = form.PhictureString;
            //    dto.GhsImage = hcSiteProductService.GetProductGHSPictures(form.PhictureString);
            // //   SSProduct.ActiveSheet.Cells[e.Row, 6].Value = dto.GhsImage;
            //}

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
                IList<HC_MSDS_PRODUCT> list = SSProduct.GetEditbleData<HC_MSDS_PRODUCT>();
                if (list.Count > 0)
                {

                    if (hcMsdsProductService.Save(list))
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
            List<HC_MSDS_PRODUCT> list = hcMsdsProductService.FindAll(txtViewJepum.Text.Trim());
            SSProduct.SetDataSource(list);
        }
      

        private void SSProduct_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_MSDS_PRODUCT dto =  SSProduct.GetRowData(e.Row) as HC_MSDS_PRODUCT;
            if(dto == null)
            {
                
            }
            //panel1.SetData(dto);
            SELECTED_PRODUCT = dto;
            TxtProductName.SetValue(dto.PRODUCTNAME);

            SearchSSProductMsds(dto.MSDSLIST);
        }

        private void SearchSSProductMsds(string msdslist)
        {
            int nCnt = VB.I(msdslist, "{}");
            string strMsds = "";
            string strCasno = "";
            string strQty = "";

            SSProductMsds.ActiveSheet.RowCount = 0;
            for (int i=1; i<=nCnt; i++)
            {
                strMsds = VB.Pstr(msdslist, "{}", i);
                if (strMsds != "")
                {
                    strCasno = VB.Pstr(strMsds, "|", 1);
                    strQty = VB.Pstr(strMsds, "|", 2);

                    HC_MSDS msds = hcMsdsService.FindByCasNo(strCasno);

                    SSProductMsds.AddRows();
                    SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 1].Text = msds.CHEMID;
                    SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 2].Text = strCasno;
                    SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 4].Text = strQty;
                    if (msds != null)
                    {
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 3].Text = msds.NAME;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 5].Text = msds.EXPOSURE_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 6].Text = msds.WEM_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 7].Text = msds.SPECIALHEALTH_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 8].Text = msds.MANAGETARGET_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 9].Text = msds.SPECIALMANAGE_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 10].Text = msds.STANDARD_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 11].Text = msds.PERMISSION_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 12].Text = msds.PSM_MATERIAL;
                    }
                }
            }

        }
        private void BtnSearchMsds_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtSearchMsdsWord.Text == "")
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                List<KoshaMsds> list = koshaMsdsService.SearchKoshaMsds(RdoMsdsName.Checked, TxtSearchMsdsWord.Text);
                SSMSDSList.SetDataSource(list);
            }
            catch (Exception ex)
            {
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnSaveMsds_Click(object sender, EventArgs e)
        {
            string strMsdsList = "";
            string strChemid = "";
            string strCasno = "";
            string strQty = "";
            string strName = "";

            if (SELECTED_PRODUCT == null)
            {
                MessageUtil.Alert(" 제품을 선택하세요 ");
                return;
            }

            for (int i=0; i<SSProductMsds.RowCount(); i++)
            {
                if (Convert.ToBoolean(SSProductMsds.ActiveSheet.Cells[i, 0].Value)==false)
                {
                    strChemid = SSProductMsds_Sheet1.Cells[i, 1].Text.ToString().Trim();
                    strCasno = SSProductMsds_Sheet1.Cells[i, 2].Text.ToString().Trim();
                    strName = SSProductMsds_Sheet1.Cells[i, 3].Text.ToString().Trim();
                    strQty = SSProductMsds_Sheet1.Cells[i, 4].Text.ToString().Trim();
                    strMsdsList += strCasno + "|" + strQty + "{}";

                    //HIC_MSDS에 없으면 자동등록함
                    HC_MSDS msds = hcMsdsService.FindByCasNo(strCasno);
                    if (msds == null)
                    {
                        HC_MSDS dto = new HC_MSDS();

                        dto.ID = 0;
                        dto.NAME = strName;
                        dto.CHEMID = strChemid;
                        dto.CASNO = strCasno;

                        dto.EXPOSURE_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 5].Text.Trim();
                        dto.WEM_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 6].Text.Trim();
                        dto.SPECIALHEALTH_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 7].Text.Trim();
                        dto.MANAGETARGET_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 8].Text.Trim();
                        dto.SPECIALMANAGE_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 9].Text.Trim();
                        dto.STANDARD_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 10].Text.Trim();
                        dto.PERMISSION_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 11].Text.Trim();
                        dto.PSM_MATERIAL = SSProductMsds.ActiveSheet.Cells[i, 12].Text.Trim();
                        dto.GHS_PICTURE = koshaMsdsService.GetGHSPicture(strChemid);

                        dto.WEM_MATERIAL = VB.Replace(dto.WEM_MATERIAL, "작업환경측정대상물질 ", "");
                        dto.SPECIALHEALTH_MATERIAL = VB.Replace(dto.SPECIALHEALTH_MATERIAL, "특수건강진단대상물질 ", "");

                        if (dto.EXPOSURE_MATERIAL == "") dto.EXPOSURE_MATERIAL = "N";
                        if (dto.MANAGETARGET_MATERIAL == "") dto.MANAGETARGET_MATERIAL = "N";
                        if (dto.SPECIALMANAGE_MATERIAL == "") dto.SPECIALMANAGE_MATERIAL = "N";
                        if (dto.STANDARD_MATERIAL == "") dto.STANDARD_MATERIAL = "N";
                        if (dto.PERMISSION_MATERIAL == "") dto.PERMISSION_MATERIAL = "N";
                        if (dto.PSM_MATERIAL == "") dto.PSM_MATERIAL = "N";

                        hcMsdsService.Save(dto);
                    }
                }
            }

            hcMsdsProductService.Update_Msdslist(SELECTED_PRODUCT.ID, strMsdsList);
            SearchProduct();

            SSProductMsds.ActiveSheet.RowCount = 0;
            SELECTED_PRODUCT = null;
            TxtProductName.SetValue("");

            MessageUtil.Info("저장되었습니다.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnViewJepum_Click(object sender, EventArgs e)
        {
            SearchProduct();
        }

        private void SSMSDSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strCHEMID = "";
            string strCASNO = "";
            string strNAME = "";

            if (TxtProductName.Text.Trim()=="") { MessageUtil.Alert("제품명이 공란입니다."); return; }
            {
                if (SELECTED_PRODUCT.ID <= 0)
                {
                    MessageUtil.Alert(" 제품을 선택하거나 추가된 제품은 저장하세요 ");
                    return;
                }
                strCHEMID = SSMSDSList_Sheet1.Cells[e.Row, 0].Text.Trim();
                strCASNO = SSMSDSList_Sheet1.Cells[e.Row, 2].Text.Trim();
                strNAME = SSMSDSList_Sheet1.Cells[e.Row, 1].Text.Trim();

                if (IsMatch(strCASNO))
                {
                    MessageUtil.Alert(strNAME + "은 이미 추가된 화학물질입니다. ");
                }
                else
                {
                    HC_MSDS dto = koshaMsdsService.GetKoshaRule(strCHEMID);
                    Thread.Sleep(500);

                    if (dto != null)
                    {
                        SSProductMsds.AddRows();
                        SSProductMsds_Sheet1.Cells[SSProductMsds_Sheet1.RowCount - 1, 1].Text = strCHEMID;
                        SSProductMsds_Sheet1.Cells[SSProductMsds_Sheet1.RowCount - 1, 2].Text = strCASNO;
                        SSProductMsds_Sheet1.Cells[SSProductMsds_Sheet1.RowCount - 1, 3].Text = strNAME;
                        SSProductMsds_Sheet1.Cells[SSProductMsds_Sheet1.RowCount - 1, 4].Text = "";

                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 5].Text = dto.EXPOSURE_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 6].Text = dto.WEM_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 7].Text = dto.SPECIALHEALTH_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 8].Text = dto.MANAGETARGET_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 9].Text = dto.SPECIALMANAGE_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 10].Text = dto.STANDARD_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 11].Text = dto.PERMISSION_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 12].Text = dto.PSM_MATERIAL;
                    }

                }
            }
        }

        private void SSMSDSList_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
