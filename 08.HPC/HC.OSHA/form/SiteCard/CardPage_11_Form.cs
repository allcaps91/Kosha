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
    /// 15.위험물질
    /// </summary>
    public partial class CardPage_11_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        public HcOshaCard15Service hcOshaCard15Service { get; }
        public CardPage_11_Form()
        {
            InitializeComponent();
            hcOshaCard15Service = new HcOshaCard15Service();
        }
        private void CardPage_11_Form_Load(object sender, EventArgs e)
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
            TxtTASKNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD15.TASKNAME) });
            TxtMANAGESTATUS.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD15.MANAGESTATUS) });
            TxtdNAME.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD15.NAME) });

            CboTASKTYPE.SetOptions(new ComboBoxOption { DataField = nameof(HC_OSHA_CARD15.TASKTYPE) });
            CboTASKTYPE.SetItems(codeService.FindActiveCodeByGroupCode("SITE_CARD15_TASKTYPE", "OSHA"), "CodeName", "Code");

            TxtQTY.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD15.QTY) });
            TxtSTATUS.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD15.STATUS) });
            TxtREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD15.REMARK) });

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("취급공정", nameof(HC_OSHA_CARD15.TASKNAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("분류", nameof(HC_OSHA_CARD15.TASKTYPE), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("물질명", nameof(HC_OSHA_CARD15.NAME), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("취급량(Kg/월)", nameof(HC_OSHA_CARD15.QTY), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("관리상태", nameof(HC_OSHA_CARD15.MANAGESTATUS), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

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
                if (pan15.Validate<HC_OSHA_CARD15>())
                {
                    HC_OSHA_CARD15 dto = pan15.GetData<HC_OSHA_CARD15>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    dto.SITE_ID = base.SelectedSite.ID;
                    //dto.YEAR = base.GetCurrentYear();
                    dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                    HC_OSHA_CARD15 saved = hcOshaCard15Service.Save(dto);

                    // pan15.SetData(saved);
                    pan15.Initialize();

                    Search();
                }
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD15 dto = pan15.GetData<HC_OSHA_CARD15>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard15Service.Delete(dto);
                    pan15.Initialize();

                    Search();
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            pan15.Initialize();
        }

        private void Clear()
        {
            SSList.ActiveSheet.RowCount = 0;
            pan15.Initialize();

            int row = 0;
            for (int i = 0; i < 16; i++)
            {
                row = i + 3;
                SSCard.ActiveSheet.Cells[row, 0].Value = "";
                SSCard.ActiveSheet.Cells[row, 1].Value = "";
                SSCard.ActiveSheet.Cells[row, 3].Value = "";
                SSCard.ActiveSheet.Cells[row, 6].Value = "";
                SSCard.ActiveSheet.Cells[row, 7].Value = "";
                SSCard.ActiveSheet.Cells[row, 8].Value = "";
                SSCard.ActiveSheet.Cells[3, 11].Value = "";
            }
        }
        private void Search()
        {
            Clear();
            if (base.SelectedSite == null)
            {
                return;
            }
            //List<HC_OSHA_CARD15> list = hcOshaCard15Service.hcOshaCard15Repository.FindAll(base.SelectedSite.ID, base.GetCurrentYear());
            List<HC_OSHA_CARD15> list = hcOshaCard15Service.hcOshaCard15Repository.FindAll(base.SelectedSite.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            if (list.Count > 0)
            {
                SSList.SetDataSource(list);

                //  해당없음 항목 삭제
                int row = 0;
                for(int i =0; i<list.Count; i++)
                {
                    row = i + 3;
                    SSCard.ActiveSheet.Cells[row, 0].Value = list[i].TASKNAME;

                    if (list[i].TASKTYPE == "0")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "1";
                    }
                    else if (list[i].TASKTYPE == "1")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "2";
                    }
                    else if (list[i].TASKTYPE == "2")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "3";
                    }
                    else if (list[i].TASKTYPE == "3")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "4";
                    }
                    else if (list[i].TASKTYPE == "4")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "5";
                    }
                    else if (list[i].TASKTYPE == "5")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "6";
                    }
                    else if (list[i].TASKTYPE == "6")
                    {
                        SSCard.ActiveSheet.Cells[row, 1].Value = "7";
                    }
                    SSCard.ActiveSheet.Cells[row, 3].Value = list[i].NAME;//
                    SSCard.ActiveSheet.Cells[row, 6].Value = list[i].QTY;
                    SSCard.ActiveSheet.Cells[row, 7].Value = list[i].MANAGESTATUS;
                    SSCard.ActiveSheet.Cells[row, 8].Value = list[i].STATUS;
            
                    SSCard.ActiveSheet.Cells[3, 11].Value = list[i].REMARK;
                }
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD15>());
                //  해당없음 항목 표시
                SSCard.ActiveSheet.Cells[0, 0].Text = "15. 위험물질 - 해당없음";
            }
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            HC_OSHA_CARD15 dto = SSList.GetRowData(e.Row) as HC_OSHA_CARD15;
            HC_OSHA_CARD15 saved = hcOshaCard15Service.hcOshaCard15Repository.FindOne(dto.ID);
            pan15.SetData(saved);

        }

        #endregion

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

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

        private void BtnMsds_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
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
            if (list != null)
            {
                try
                {
                    clsDB.setBeginTran(clsDB.DbCon);
                    foreach (HC_SITE_MSDS_MODEL model in list)
                    {
                        HC_OSHA_CARD15 dto = new HC_OSHA_CARD15();
                        dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                        dto.SITE_ID = base.SelectedSite.ID;
                        dto.TASKNAME = model.PROCESS;
                        dto.NAME = model.PRODUCTNAME;
                        dto.QTY = model.MONTHLYAMOUNT + model.UNIT;
                        //dto.YEAR = base.GetCurrentYear();
                        dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                        hcOshaCard15Service.Save(dto);
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Search();
                    if (list.Count > 0)
                    {
                        MessageUtil.Info("위험물질을 등록하였습니다");
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
