using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using FarPoint.Win.Spread;
using HC.Core.Common.Interface;
using HC_Core;
using HC.Core.Common.Util;
using HC.Core.Model;
using HC.OSHA.Dto;
using HC.OSHA.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HC.OSHA.Model;
using ComBase;
using ComHpcLibB.Model;

namespace HC_OSHA
{
    /// <summary>
    /// 16.유해물질
    /// </summary>
    public partial class CardPage_12_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        private HcOshaCard16Service hcOshaCard16Service;
        public CardPage_12_Form()
        {
            InitializeComponent();
            hcOshaCard16Service = new HcOshaCard16Service();
            SSCard.ActiveSheet.ActiveRowIndex = 0;
            SSCard.ShowRow(0, 0, FarPoint.Win.Spread.VerticalPosition.Top);
        }
        private void CardPage_12_Form_Load(object sender, EventArgs e)
        {
            Init();
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;
            Clear();
            Search();

        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;
            Clear();
        }

     

        #region 
        private void Init()
        {
            TxtTASKNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.TASKNAME) });
            TxtEXPOSURE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.EXPOSURE) });
            TxtCOSENESS.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.COSENESS) });

            CboTASKTYPE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CARD16.TASKTYPE) });
            CboTASKTYPE.SetItems(codeService.FindActiveCodeByGroupCode("SITE_CARD16_TASKTYPE", "OSHA"), "CodeName", "Code");

            TxtNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.NAME) });
            TxtPROTECTION.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.PROTECTION) });
            TxtUSAGE.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.USAGE) });
            TxtQTY.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD16.QTY) });

            ChkISMSDSPUBLISH.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CARD16.ISMSDSPUBLISH), CheckValue = "1", UnCheckValue = "0" });
            ChkISALET.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CARD16.ISALET), CheckValue = "1", UnCheckValue = "0" });
            ChkISMSDSEDUCATION.SetOptions(new CheckBoxOption { DataField = nameof(HC_OSHA_CARD16.ISMSDSEDUCATION), CheckValue = "1", UnCheckValue = "0" });


            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("취급공정", nameof(HC_OSHA_CARD16.TASKNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("분류", nameof(HC_OSHA_CARD16.TASKTYPE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("물질명", nameof(HC_OSHA_CARD16.NAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("취급량(Kg/월)", nameof(HC_OSHA_CARD16.QTY), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("사용용도", nameof(HC_OSHA_CARD16.USAGE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("노출수준", nameof(HC_OSHA_CARD16.EXPOSURE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("밀폐국소배기상태", nameof(HC_OSHA_CARD16.COSENESS), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("보호구지급착용상태", nameof(HC_OSHA_CARD16.PROTECTION), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            SSList.AddColumnText("MSDS게시또는비치여부", nameof(HC_OSHA_CARD16.ISMSDSPUBLISH), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("경고표지부착여부", nameof(HC_OSHA_CARD16.ISALET), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("MSDS교육실시여부", nameof(HC_OSHA_CARD16.ISMSDSEDUCATION), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            Clear();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null || base.SelectedSite == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (pan16.Validate<HC_OSHA_CARD16>())
                {
                    HC_OSHA_CARD16 dto = pan16.GetData<HC_OSHA_CARD16>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    //dto.YEAR = base.GetCurrentYear();
                    dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                    HC_OSHA_CARD16 saved = hcOshaCard16Service.Save(dto);

                    pan16.Initialize();

                    Search();
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD16 dto = pan16.GetData<HC_OSHA_CARD16>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard16Service.Delete(dto);
                    pan16.Initialize();

                    Search();
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            pan16.Initialize();
        }

        private void Clear()
        {
            SSList.ActiveSheet.RowCount = 0;
            pan16.Initialize();

            SSCard.ActiveSheet.Cells[0, 0].Value = "16. 유해물질";
            int row = 0;
            for (int i = 0; i < 107; i++)
            {
                row = i + 4;
                SSCard.ActiveSheet.Cells[row, 0].Value = "";
                SSCard.ActiveSheet.Cells[row, 1].Value = "";
                SSCard.ActiveSheet.Cells[row, 2].Value = "";
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 4].Value = "";
                SSCard.ActiveSheet.Cells[row, 5].Value = "";
                SSCard.ActiveSheet.Cells[row, 6].Value = "";
                SSCard.ActiveSheet.Cells[row, 7].Value = "";

                SSCard.ActiveSheet.Cells[row, 8].Value = "";
                SSCard.ActiveSheet.Cells[row, 9].Value = "";
                SSCard.ActiveSheet.Cells[row, 10].Value = "";

                if (row > 42) SSCard.ActiveSheet.Rows[row].Visible = false;
            }
        }
        private void Search()
        {
            if(base.SelectedSite == null)
            {
                return;
            }
            Clear();
            List<HC_OSHA_CARD16> list = hcOshaCard16Service.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            if (list.Count > 0)
            {
                SSList.SetDataSource(list);

                int row = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    row = i + 4;
                    if (SSCard.ActiveSheet.Rows[row].Visible == false) SSCard.ActiveSheet.Rows[row].Visible = true;
                    SSCard.ActiveSheet.Cells[row, 0].Value = list[i].TASKNAME;
                    SSCard.ActiveSheet.Cells[row, 1].Value = list[i].TASKTYPE;
                    SSCard.ActiveSheet.Cells[row, 2].Value = list[i].NAME;
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].QTY;
                    SSCard.ActiveSheet.Cells[row, 4].Value = list[i].USAGE;
                    SSCard.ActiveSheet.Cells[row, 5].Value = list[i].EXPOSURE;
                    SSCard.ActiveSheet.Cells[row, 6].Value = list[i].COSENESS;
                    SSCard.ActiveSheet.Cells[row, 7].Value = list[i].PROTECTION;
                    SSCard.ActiveSheet.Cells[row, 8].Value = list[i].ISMSDSPUBLISH;
                    SSCard.ActiveSheet.Cells[row, 9].Value = list[i].ISALET;
                    SSCard.ActiveSheet.Cells[row, 10].Value = list[i].ISMSDSEDUCATION;
                }                  
            }
            else
            {
                SSCard.ActiveSheet.Cells[0, 0].Value = "16. 유해물질 - 해당없음";
                SSList.SetDataSource(new List<HC_OSHA_CARD16>());
            
            }
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD16 dto = SSList.GetRowData(e.Row) as HC_OSHA_CARD16;
            HC_OSHA_CARD16 saved = hcOshaCard16Service.hcOshaCard16Repository.FindOne(dto.ID);
            pan16.SetData(saved);

        }




        #endregion
        public void Print()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
        }

        public bool NewPrint()
        {
            Search();
            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute();
            return true;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void BtnMsds_Click(object sender, EventArgs e)
        {
            if(base.SelectedSite == null)
            {
                return;
            }

            SiteMSDSListForm form = new SiteMSDSListForm();
            form.SelectedSite = base.SelectedSite;
            form.SelectedEstimate = base.SelectedEstimate;
            (form as ISelectSite).Select(form.SelectedSite);
            form.ShowSiteCardButton();
            form.ShowDialog();

            List<HC_SITE_MSDS_MODEL> list = form.GetSelectedMsds();
            if(list != null)
            {
                try
                {
                    clsDB.setBeginTran(clsDB.DbCon);
                    foreach (HC_SITE_MSDS_MODEL model in list)
                    {
                        HC_OSHA_CARD16 dto = new HC_OSHA_CARD16();
                        dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                        dto.SITE_ID = base.SelectedSite.ID;
                        dto.TASKNAME = model.PROCESS;
                        dto.NAME = model.PRODUCTNAME;
                        dto.QTY = model.MONTHLYAMOUNT + model.UNIT;
                        //dto.YEAR = base.GetCurrentYear();
                        dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                        hcOshaCard16Service.Save(dto);
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Search();
                    if (list.Count > 0)
                    {
                        MessageUtil.Info("유해물질을 등록하였습니다");
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageUtil.Alert(ex.Message);
                }
            }
       
            
        }
    }
}
