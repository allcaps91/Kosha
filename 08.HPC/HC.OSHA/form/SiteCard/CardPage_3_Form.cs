using ComBase.Controls;
using ComBase.Mvc.Enums;
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
using ComHpcLibB.Model;

namespace HC_OSHA
{
    public partial class CardPage_3_Form : CommonForm, ISelectSite, ISelectEstimate, IPrint
    {
        HcOshaCard4_2Service hcOshaCard4_2Service;
        HcOshaCard4_3Service hcOshaCard4_3Service;
        public CardPage_3_Form()
        {
            InitializeComponent();
            hcOshaCard4_2Service = new HcOshaCard4_2Service();
            hcOshaCard4_3Service = new HcOshaCard4_3Service();
        }

        private void CardPage_3_Form_Load(object sender, EventArgs e)
        {
            ClearCard();

            SSCard.ActiveSheet.DefaultStyle.Border = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);


            TxtTASK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_2.TASK) });
            TxtTASKUNIT.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_2.TASKUNIT) });
            TxtMSDS.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_2.MSDS) });
            TxtWorkerCount.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_2.WORKERCOUNT) }); 
            TxtWORKDESC.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_2.WORKDESC) });
            TxtREMARK.SetOptions(new TextBoxOption { DataField = nameof(HC_OSHA_CARD4_2.REMARK) });

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false , RowHeightAuto = true});
            SSList.AddColumnText("부서또는 공정", nameof(HC_OSHA_CARD4_2.TASK), 130, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("단위 작업 장소", nameof(HC_OSHA_CARD4_2.TASKUNIT), 130, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("유해인자", nameof(HC_OSHA_CARD4_2.MSDS), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("근로자수", nameof(HC_OSHA_CARD4_2.WORKERCOUNT), 37, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("작업내용", nameof(HC_OSHA_CARD4_2.WORKDESC), 130, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });
            SSList.AddColumnText("비고", nameof(HC_OSHA_CARD4_2.REMARK), 130, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, WordWrap = true, IsMulti = true });

        }

        private void ClearCard()
        {
            SSCard.ActiveSheet.RowCount = 20;
            for (int i = 2; i < SSCard.ActiveSheet.RowCount; i++)
            {
            //    string xx = SSCard_Sheet1.Cells[i, 15].Text;
                SSCard.ActiveSheet.Cells[i, 0].Text = "";
                SSCard.ActiveSheet.Cells[i, 1].Text = "";
                SSCard.ActiveSheet.Cells[i, 2].Text = "";
                SSCard.ActiveSheet.Cells[i, 3].Text = "";
                SSCard.ActiveSheet.Cells[i, 4].Text = "";
                SSCard.ActiveSheet.Cells[i, 5].Text = "";
                SSCard.ActiveSheet.Rows[i].Height = 47;
            }
        }

        public void Select(IEstimateModel estimateModel)
        {
            base.SelectedEstimate = estimateModel;

            Search();
        }

        private void Search()
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            ClearCard();

            //List<HC_OSHA_CARD4_2> list = hcOshaCard4_2Service.hcOshaCard4_2Repository.FindByEstimateId(base.SelectedEstimate.ID, base.GetCurrentYear());
            List<HC_OSHA_CARD4_2> list = hcOshaCard4_2Service.hcOshaCard4_2Repository.FindByEstimateId(base.SelectedEstimate.ID, base.SelectedEstimate.CONTRACTSTARTDATE.Left(4));
            if (list.Count > 0)
            {
                int row = 2;
                SSList.SetDataSource(list);


                SSCard.ActiveSheet.RowCount = list.Count + row;
        
                for (int i = 0; i < list.Count; i++)
                {
                  
                    //  SSCard.ActiveSheet.Cells[i + 2, 0]
                    //     row += i;
                    //SSCard.ActiveSheet.Cells[i + 2, 0].Value = list[i].TASK;
                    //SSCard.ActiveSheet.Cells[i + 2, 4].Value = list[i].TASKUNIT;
                    //SSCard.ActiveSheet.Cells[i + 2, 7].Value = list[i].MSDS;
                    //SSCard.ActiveSheet.Cells[i + 2, 12].Value = list[i].WORKERCOUNT;
                    //SSCard.ActiveSheet.Cells[i + 2, 15].Value = list[i].WORKDESC;
                    //SSCard.ActiveSheet.Cells[i + 2, 25].Value = list[i].REMARK;

                    SSCard.ActiveSheet.Cells[row + i, 0].Value = list[i].TASK;
                    SSCard.ActiveSheet.Cells[row + i, 1].Value = list[i].TASKUNIT;
                    SSCard.ActiveSheet.Cells[row + i, 2].Value = list[i].MSDS;
                    SSCard.ActiveSheet.Cells[row + i, 3].Value = list[i].WORKERCOUNT;
                    SSCard.ActiveSheet.Cells[row + i, 4].Value = list[i].WORKDESC;
                    SSCard.ActiveSheet.Cells[row + i, 5].Value = list[i].REMARK;
                    SSCard.ActiveSheet.Rows[row + i].Height = 47;
                    //SSCard.ActiveSheet.Rows[i].Height = SSCard.ActiveSheet.Rows[i].GetPreferredHeight();
                }

                if(SSCard.ActiveSheet.RowCount < 24)
                {
                    int gap = 24 - SSCard.RowCount();

                    for(int i = 0; i<gap; i++)
                    {
                        SSCard.ActiveSheet.RowCount++;
                        SSCard.ActiveSheet.Cells[SSCard.ActiveSheet.RowCount - 1, 0].Text = "  ";
                        SSCard.ActiveSheet.Rows[SSCard.ActiveSheet.RowCount - 1].Height = 47;
                    }
                }
            }
            else
            {
                SSList.SetDataSource(new List<HC_OSHA_CARD4_2>());
            }
        }
      
        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;

            panTask.SetData(new HC_OSHA_CARD4_2());
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            HC_OSHA_CARD4_2 dto = panTask.GetData<HC_OSHA_CARD4_2>();

            if (dto.ID > 0)
            {
                if (MessageUtil.Confirm("삭제하시겠습니까?") == DialogResult.Yes)
                {
                    hcOshaCard4_2Service.Delete(dto);
                    panTask.Initialize();

                    //MessageUtil.Info("삭제")
                    Search();
                }

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (base.SelectedEstimate == null)
            {
                MessageUtil.Info("사업장 계약 정보를 선택하세요");
            }
            else
            {
                if (panTask.Validate<HC_OSHA_CARD4_2>())
                {
                    HC_OSHA_CARD4_2 dto = panTask.GetData<HC_OSHA_CARD4_2>();
                    dto.ESTIMATE_ID = base.SelectedEstimate.ID;
                    //dto.YEAR = GetCurrentYear();
                    dto.YEAR = base.SelectedEstimate.CONTRACTSTARTDATE.Left(4);
                    HC_OSHA_CARD4_2 saved = hcOshaCard4_2Service.Save(dto);

                    panTask.SetData(saved);

                    panTask.Initialize();

                    Search();
                }
            }
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_OSHA_CARD4_2 dto = SSList.GetRowData(e.Row) as HC_OSHA_CARD4_2;
            HC_OSHA_CARD4_2 saved =  hcOshaCard4_2Service.hcOshaCard4_2Repository.FindOne(dto.ID);
            panTask.SetData(saved);
        }

     

        private void contentTitle1_Load(object sender, EventArgs e)
        {

        }

        public void Print()
        {
            Search();

            SpreadPrint print = new SpreadPrint(SSCard, PrintStyle.FORM, false);
            print.Execute(); 
            //Task.WaitAny(print.Execute());
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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            panTask.Initialize();
        }

        private void BtnAddCommit_Click(object sender, EventArgs e)
        {
          
        }

        private void BtnSaveCommit_Click(object sender, EventArgs e)
        {
           
        }

        private void SSList_CellClick(object sender, CellClickEventArgs e)
        {

        }
    }
}
