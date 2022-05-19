using System;
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
using FarPoint.Win.Spread;
using ComBase;

namespace HC_OSHA
{
    public partial class SiteMSDSForm : CommonForm
    {
        private readonly HcMsdsProductService hcMsdsProductService;
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

            hcMsdsProductService = new HcMsdsProductService();
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
            SSProduct.AddColumnText("제품명", nameof(HC_SITE_PRODUCT.PRODUCTNAME), 250, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSProduct.AddColumnText("권고용도", nameof(HC_SITE_PRODUCT.USAGE), 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnText("제조사", nameof(HC_SITE_PRODUCT.MANUFACTURER), 170, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSProduct.AddColumnText("월취급량", nameof(HC_SITE_PRODUCT.MONTHLYAMOUNT), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnComboBox("단위", nameof(HC_SITE_PRODUCT.UNIT), 100, IsReadOnly.N, EduTypeComboBoxData, new SpreadCellTypeOption { IsSort = false });
            SSProduct.AddColumnDateTime("개정일자", nameof(HC_SITE_PRODUCT.REVISIONDATE), 120, IsReadOnly.N, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsSort = false, dateTimeEditorValue = DateTimeEditorValue.String });
            SSProduct.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += SSProduct_DelteButtonClick; ;
            SearchProduct();


            SSMSDSList.Initialize(new SpreadOption() { IsRowSelectColor = false});
            SSMSDSList.AddColumnText("CasNo", nameof(HC_MSDS.CASNO), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSMSDSList.AddColumnText("물질명", nameof(HC_MSDS.NAME), 170, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left, sortIndicator = SortIndicator.Ascending });
            SSMSDSList.AddColumnText("노출기준설정", nameof(HC_MSDS.EXPOSURE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("작업환경측정", nameof(HC_MSDS.WEM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("특수건강진단", nameof(HC_MSDS.SPECIALHEALTH_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("관리대상유해", nameof(HC_MSDS.MANAGETARGET_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("특별관리", nameof(HC_MSDS.SPECIALMANAGE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("허용기준설정", nameof(HC_MSDS.STANDARD_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("허가대상유해", nameof(HC_MSDS.PERMISSION_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSMSDSList.AddColumnText("PSM제출대상", nameof(HC_MSDS.PSM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSJepum.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderColor = Color.AliceBlue });
            SSJepum.AddColumnText("제품명 및 규격", nameof(HC_MSDS_PRODUCT.PRODUCTNAME), 250, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSJepum.AddColumnText("용도", nameof(HC_MSDS_PRODUCT.USAGE), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSJepum.AddColumnText("제조사", nameof(HC_MSDS_PRODUCT.MANUFACTURER), 170, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left });
            SSJepum.AddColumnText("MSDS코드", nameof(HC_MSDS_PRODUCT.MSDSCODE), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSJepum.AddColumnText("MSDS목록", nameof(HC_MSDS_PRODUCT.MSDSLIST), 120, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSJepum.AddColumnText("ID", nameof(HC_MSDS_PRODUCT.ID), 90, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSProductMsds.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSProductMsds.AddColumnCheckBox("삭제", "", 50, new CheckBoxBooleanCellType());
            SSProductMsds.AddColumnText("CasNo", nameof(HC_SITE_PRODUCT_MSDS_MODEL.CASNO), 70, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = CellHorizontalAlignment.Left });
            SSProductMsds.AddColumnText("물질명", nameof(HC_SITE_PRODUCT_MSDS_MODEL.NAME), 140, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = CellHorizontalAlignment.Left, sortIndicator = SortIndicator.Ascending });
            SSProductMsds.AddColumnText("함유량", nameof(HC_SITE_PRODUCT_MSDS_MODEL.QTY), 69, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, sortIndicator = SortIndicator.Ascending });
            SSProductMsds.AddColumnText("노출기준설정", nameof(HC_SITE_PRODUCT_MSDS_MODEL.EXPOSURE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("작업환경측정", nameof(HC_SITE_PRODUCT_MSDS_MODEL.WEM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("특수건강진단", nameof(HC_SITE_PRODUCT_MSDS_MODEL.SPECIALHEALTH_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("관리대상유해", nameof(HC_SITE_PRODUCT_MSDS_MODEL.MANAGETARGET_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("특별관리", nameof(HC_SITE_PRODUCT_MSDS_MODEL.SPECIALMANAGE_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("허용기준설정", nameof(HC_SITE_PRODUCT_MSDS_MODEL.STANDARD_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("허가대상유해", nameof(HC_SITE_PRODUCT_MSDS_MODEL.PERMISSION_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSProductMsds.AddColumnText("PSM제출대상", nameof(HC_SITE_PRODUCT_MSDS_MODEL.PSM_MATERIAL), 42, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSProductMsds.SetDataSource(new List<HC_SITE_PRODUCT_MSDS_MODEL>());
            SSJepum.Visible = false;
        }

        private bool IsMatch(long site_product_id, string casno)
        {
            for(int i=0; i< SSProductMsds.RowCount(); i++)
            {
                HC_SITE_PRODUCT_MSDS_MODEL model = SSProductMsds.GetRowData(i) as HC_SITE_PRODUCT_MSDS_MODEL;
                if( site_product_id == model.SITE_PRODUCT_ID && casno == model.CASNO)
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
            //model.MSDS_ID = msds.ID;

        }

        private void SSProduct_DelteButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            HC_SITE_PRODUCT dto =  SSProduct.GetRowData(e.Row) as HC_SITE_PRODUCT;
            SSProduct.DeleteRow(e.Row);
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
            if (dto == null) return;

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
            if (RdoMsdsUse.Checked == false)
            {
                List<HC_MSDS> list = hcMsdsService.Search(RdoMsdsName.Checked, TxtSearchMsdsWord.Text);
                SSMSDSList.SetDataSource(list);
            }
            else
            {
                List<HC_MSDS_PRODUCT> list = hcMsdsProductService.FindAll(TxtSearchMsdsWord.Text.Trim());
                SSJepum.SetDataSource(list);
            }
        }

        private void BtnSaveMsds_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strCasno = "";
            string strQty = "";

            if (SELECTED_PRODUCT == null)
            {
                ComFunc.MsgBox("저장할 제품을 선택 안함", "오류");
                Cursor.Current = Cursors.Default;
                return;
            }

            if (chkJepum.Checked == true)
            {
                if (JepumMsds_Save() == false) return;
                chkJepum.Checked = false;
            }

            //기존 저장된 화학물질을 삭제함
            if (SELECTED_PRODUCT.ID > 0)
            {
                SQL = "DELETE FROM HIC_SITE_PRODUCT_MSDS ";
                SQL = SQL + " WHERE SITE_PRODUCT_ID=" + SELECTED_PRODUCT.ID + " ";
                SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("MSDS 삭제 시 오류가 발생함", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < SSProductMsds.ActiveSheet.RowCount; i++)
                {
                    if (Convert.ToBoolean(SSProductMsds.ActiveSheet.Cells[i, 0].Value) == false)
                    {
                        strCasno = SSProductMsds_Sheet1.Cells[i, 1].Text.ToString().Trim();
                        strQty = SSProductMsds_Sheet1.Cells[i, 3].Text.ToString().Trim();

                        SQL = "INSERT INTO HIC_SITE_PRODUCT_MSDS (SITE_PRODUCT_ID,CASNO,QTY,SWLICENSE) VALUES (";
                        SQL = SQL + SELECTED_PRODUCT.ID + ",'" + strCasno + "','" + strQty + "',";
                        SQL = SQL + "'" + clsType.HosInfo.SwLicense + "') ";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("HIC_SITE_PRODUCT_MSDS 자료 등록시 오류가 발생함", "알림");
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }

            SearchSSProductMsds();
        }

        private bool JepumMsds_Save()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            long nID = 0;
            string strMsdsList = "";
            string strCasno = "";
            string strQty = "";
            string strUsage = "";

            string strProductName = TxtProductName.Text.Trim().ToUpper();
            if (strProductName == "") 
            {
                MessageUtil.Alert("상용 제품명이 공란입니다.");
                return false;
            }

            strMsdsList = "";
            for (int i = 0; i < SSProductMsds.RowCount(); i++)
            {
                if (Convert.ToBoolean(SSProductMsds.ActiveSheet.Cells[i, 0].Value) == false)
                {
                    strCasno = SSProductMsds_Sheet1.Cells[i, 1].Text.ToString().Trim();
                    strQty = SSProductMsds_Sheet1.Cells[i, 3].Text.ToString().Trim();
                    strMsdsList += strCasno + "|" + strQty + "{}";
                }
            }

            //상용제품명 ID 찾기
            SQL = "SELECT ID FROM HIC_OSHA_MSDS_PRODUCT ";
            SQL = SQL + ComNum.VBLF + "WHERE PRODUCTNAME='" + strProductName + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nID = 0;
            if (dt.Rows.Count > 0)
            {
                nID = long.Parse(dt.Rows[0]["ID"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            if (nID == 0)
            {
                nID = GET_SeqNextVal("HC_OSHA_MSDS_PRODUCT_SEQ");
                strUsage = "";
                if (SELECTED_PRODUCT != null) strUsage = SELECTED_PRODUCT.USAGE;

                SQL = "INSERT INTO HIC_OSHA_MSDS_PRODUCT (ID, PRODUCTNAME,USAGE,MANUFACTURER,MSDSCODE,";
                SQL = SQL + " MSDSLIST,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) VALUES (";
                SQL = SQL + nID + ",'" + strProductName + "','" + strUsage + "','','','" + strMsdsList + "',";
                SQL = SQL + "SYSDATE,'" + clsType.User.Sabun + "',SYSDATE,'" + clsType.User.Sabun;
                SQL = SQL + "','" + clsType.HosInfo.SwLicense + "') ";
            }
            else
            {
                SQL = "UPDATE HIC_OSHA_MSDS_PRODUCT SET ";
                SQL = SQL + " PRODUCTNAME='" + strProductName + "', ";
                SQL = SQL + " MSDSLIST='" + strMsdsList + "', ";
                SQL = SQL + " MODIFIED=SYSDATE,";
                SQL = SQL + " MODIFIEDUSER='" + clsType.User.Sabun + "' ";
                SQL = SQL + " WHERE ID='" + nID + "' ";
            }
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("상용코드 저장 시 오류가 발생함", "알림");
                Cursor.Current = Cursors.Default;
                return false;
            }

            return true;

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

        private void RdoMsdsName_CheckedChanged(object sender, EventArgs e)
        {
            if (SSJepum.Visible == true) { SSJepum.Visible = false; SSMSDSList.Visible = true; }
        }

        private void RdoMsdsCasNo_CheckedChanged(object sender, EventArgs e)
        {
            if (SSJepum.Visible == true) { SSJepum.Visible = false; SSMSDSList.Visible = true; }
        }

        private void RdoMsdsUse_CheckedChanged(object sender, EventArgs e)
        {
            if (SSJepum.Visible == false) { SSMSDSList.Visible = false; SSJepum.Visible = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SiteMSDSJepum form = new SiteMSDSJepum();
            form.Owner = this;
            form.Show();
        }

        private long GET_SeqNextVal(string strSeqName)
        {
            long nID = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //시컨스 찾기
            SQL = "SELECT " + strSeqName + ".NEXTVAL AS SeqNo FROM DUAL";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            nID = 0;
            if (dt.Rows.Count > 0) nID = long.Parse(dt.Rows[0]["SeqNo"].ToString().Trim());
            dt.Dispose();
            dt = null;

            return nID;
        }

        private void SSJepum_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (SELECTED_PRODUCT != null)
            {
                string strJepum = SSJepum.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                string strList = SSJepum.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                SearchSSProductMsds(strList);
            }
        }

        private void SearchSSProductMsds(string msdslist)
        {
            int nCnt = VB.I(msdslist, "{}");
            string strMsds = "";
            string strCasno = "";
            string strQty = "";

            SSProductMsds.ActiveSheet.RowCount = 0;
            for (int i = 1; i <= nCnt; i++)
            {
                strMsds = VB.Pstr(msdslist, "{}", i);
                if (strMsds != "")
                {
                    strCasno = VB.Pstr(strMsds, "|", 1);
                    strQty = VB.Pstr(strMsds, "|", 2);

                    HC_MSDS msds = hcMsdsService.FindByCasNo(strCasno);

                    SSProductMsds.AddRows();
                    SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 1].Text = strCasno;
                    SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 3].Text = strQty;
                    if (msds != null)
                    {
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 2].Text = msds.NAME;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 4].Text = msds.EXPOSURE_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 5].Text = msds.WEM_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 6].Text = msds.SPECIALHEALTH_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 7].Text = msds.MANAGETARGET_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 8].Text = msds.SPECIALMANAGE_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 9].Text = msds.STANDARD_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 10].Text = msds.PERMISSION_MATERIAL;
                        SSProductMsds.ActiveSheet.Cells[SSProductMsds_Sheet1.RowCount - 1, 11].Text = msds.PSM_MATERIAL;
                    }
                }
            }

        }

        private void SSMSDSList_CellClick(object sender, CellClickEventArgs e)
        {
            if (SELECTED_PRODUCT == null)
            {
                MessageUtil.Alert(" 제품을 선택하세요 ");

            }
            else
            {
                if (SELECTED_PRODUCT.ID <= 0)
                {
                    MessageUtil.Alert(" 제품을 선택하거나 추가된 제품은 저장하세요 ");
                    return;
                }
                HC_MSDS msds = SSMSDSList.GetRowData(e.Row) as HC_MSDS;

                if (IsMatch(SELECTED_PRODUCT.ID, msds.CASNO))
                {
                    MessageUtil.Alert(msds.NAME + "은 이미 등록된 화학물질입니다. ");
                }
                else
                {
                    SSProductMsds.AddRows();
                    HC_SITE_PRODUCT_MSDS_MODEL model = SSProductMsds.GetRowData(SSProductMsds.RowCount() - 1) as HC_SITE_PRODUCT_MSDS_MODEL;

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
    }
}
